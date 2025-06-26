import copy
import csv
import json
import re
import threading
import uuid
from concurrent.futures import ThreadPoolExecutor
from datetime import datetime
from distutils.util import strtobool
from typing import List, Tuple
from app.utils.platform import PlatformApiOperator

import botocore.exceptions
from fastapi import HTTPException, status
from fastapi.security import HTTPAuthorizationCredentials
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.update import Action
from pynamodb.models import BatchWrite
from pytz import timezone

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.customer import CustomerModel
from app.models.project import (
    ProjectModel,
    UpdateAttributesSubAttribute,
    UpdateHistoryAttribute,
)
from app.models.user import UserModel
from app.resources.const import BoAppUrl, DataType, GetCustomersSortType
from app.resources.const import ImportFileCustomerColumnName as CsvColumn
from app.resources.const import (
    ImportFileEncodingType,
    ImportFileLimitCount,
    ImportModeType,
    ImportResultType,
    MailType,
    NotificationType,
    SuggestCustomersSortType,
    ThreadPoolMaxWorkers,
    TimezoneType,
    UserRoleType,
)
from app.resources.message import Message
from app.schemas.customer import (
    CreateCustomerRequest,
    CreateCustomerResponse,
    CustomerInfoForGetCustomers,
    CustomerInfoForImportCustomers,
    DeleteCustomerByIdResponse,
    GetCustomerByIdResponse,
    GetCustomersResponse,
    ImportCustomersRequest,
    ImportCustomersResponse,
    SuggestCustomersResponse,
    UpdateCustomerByIdRequest,
    UpdateCustomerByIdResponse,
)
from app.service.common_service.pagination import EmptyPage, Paginator
from app.service.common_service.user_info import get_update_user_name
from app.service.notification_service import NotificationService
from app.utils.aws.s3 import S3Helper
from app.utils.aws.sqs import SqsHelper
from app.utils.utils import Utils

logger = CustomLogger.get_logger()


class CustomerService:

    jst = timezone(TimezoneType.ASIA_TOKYO)

    # 並列処理時の排他制御（ロック）
    lock = threading.Lock()

    @staticmethod
    def send_mail(
        template, to_addr_list: List[str], cc_addr_list: List[str], payload: dict
    ):
        queue_name = get_app_settings().sqs_email_queue_name
        message_body = {
            "template": template,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "payload": payload,
        }
        sqs_message_body = json.dumps(message_body)
        SqsHelper().send_message_to_queue(
            queue_name=queue_name, message_body=sqs_message_body
        )

    @staticmethod
    def create_customer(
        item: CreateCustomerRequest,
        current_user: AdminModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> CreateCustomerResponse:
        """取引先情報を登録する

        Args:
            item (CreateCustomerRequest): 登録内容
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            CreateCustomerResponse: 登録結果
        """
        create_datetime = datetime.now()
        customer = CustomerModel(
            id=str(uuid.uuid4()),
            data_type=DataType.CUSTOMER,
            name=item.name,
            category=item.category,
            create_id=current_user.id,
            create_at=create_datetime,
            update_id=current_user.id,
            update_at=create_datetime,
        )

        customer.save()

        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)
        params = {
            "partnerPortalCustomerId": customer.id,
            "customerName": item.name,
            "category": item.category,
        }
        status_code, response = platform_api_operator.create_customer(params=params)

        logger.info(f"platform postProjectByPfId statusCode: {status_code}")
        if status_code != 200:
            logger.info("platform postProjectByPfId response:" + json.dumps(response))
            raise HTTPException(
                status_code=status_code,
                detail=json.dumps(response),
            )

        return CreateCustomerResponse(**customer.attribute_values)

    @staticmethod
    def suggest_customers(
        sort: SuggestCustomersSortType, current_user: AdminModel
    ) -> SuggestCustomersResponse:
        """取引先のサジェスト用データを取得します

        Args:
            sort (SuggestCustomersSortType): ソート （'name:asc'）
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            SuggestCustomersResponse: 取得結果
        """
        if sort == SuggestCustomersSortType.NAME_ASC:
            # name昇順で取得
            customer_iter = CustomerModel.data_type_name_index.query(
                hash_key=DataType.CUSTOMER, scan_index_forward=True
            )
            suggest_list: List[SuggestCustomersResponse] = []
            for customer in customer_iter:
                suggest_list.append(
                    SuggestCustomersResponse(**customer.attribute_values)
                )
            return suggest_list
        else:
            # 呼出し元でsort項目チェック済の為、以下は発生しない想定
            raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR)

    @staticmethod
    def get_customer_by_id(customer_id: str, current_user: AdminModel):
        """取引先IDを指定して取引先情報を取得する

        Args:
            customer_id (str): 取引先ID
            current_user (Behavior, optional): 認証済みのユーザー
        Raises:
            HTTPException: 404 Not found

        Returns:
            GetCustomerByIdResponse: 取得結果
        """
        try:
            customer = CustomerModel.get(
                hash_key=customer_id, range_key=DataType.CUSTOMER
            )
        except DoesNotExist:
            logger.warning(
                f"GetCustomerById customer_id not found. customer_id: {customer_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )
        user_name: str = get_update_user_name(customer.update_id)

        return GetCustomerByIdResponse(
            update_user_name=user_name, **customer.attribute_values
        )

    @staticmethod
    def get_customers(
        name: str,
        sort: GetCustomersSortType,
        limit: int,
        offset_page: int,
        current_user: AdminModel,
    ) -> GetCustomersResponse:
        """案件を検索し、案件一覧を取得する
        Args:
            name (str): 案件名
            sort (GetCustomersSortType): ソート
            limit (int): 最大取得件数
            offset_page (int): リストの中で何ページ目を取得するか
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            GetCustomersResponse: 取得結果
        """
        # GSIソート順(name)
        scan_index_forward = None
        if sort == GetCustomersSortType.NAME_ASC:
            scan_index_forward = True
        elif sort == GetCustomersSortType.NAME_DESC:
            scan_index_forward = False

        # 抽出条件
        range_key_condition = None
        # 抽出条件: 案件名
        if name:
            range_key_condition &= CustomerModel.name == name

        # 取引先情報取得
        result_list: List[CustomerModel] = list(
            CustomerModel.data_type_name_index.query(
                hash_key=DataType.CUSTOMER,
                range_key_condition=range_key_condition,
                scan_index_forward=scan_index_forward,
            )
        )

        # ソート(sort条件: name以外)
        if result_list:
            if sort == GetCustomersSortType.SALESFORCE_UPDATE_AT_ASC:
                result_list.sort(
                    key=lambda x: x.salesforce_update_at.replace(tzinfo=None)
                    if x.salesforce_update_at
                    else datetime.min
                )
            elif sort == GetCustomersSortType.SALESFORCE_UPDATE_AT_DESC:
                result_list.sort(
                    key=lambda x: x.salesforce_update_at.replace(tzinfo=None)
                    if x.salesforce_update_at
                    else datetime.min,
                    reverse=True,
                )
            elif sort == GetCustomersSortType.UPDATE_AT_ASC:
                result_list.sort(key=lambda x: x.update_at)
            elif sort == GetCustomersSortType.UPDATE_AT_DESC:
                result_list.sort(
                    key=lambda x: x.update_at,
                    reverse=True,
                )

        customer_info_list: List[CustomerInfoForGetCustomers] = [
            CustomerInfoForGetCustomers(**customer.attribute_values)
            for customer in result_list
        ]

        # ページネーション
        try:
            p = Paginator(customer_info_list, limit)
            current_page = p.page(offset_page).object_list
        except EmptyPage:
            logger.warning(f"GetCustomers not found. offset_page:{offset_page}")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        return GetCustomersResponse(
            offset_page=offset_page,
            total=len(customer_info_list),
            customers=current_page,
        )

    @staticmethod
    def update_customer_by_id(
        item: UpdateCustomerByIdRequest,
        version: int,
        customer_id: str,
        current_user: AdminModel,
    ) -> UpdateCustomerByIdResponse:
        """取引先IDを指定して取引先情報を更新する

        Args:
            item (UpdateCustomerByIdRequest): 更新内容
            version (int): ロックキー（楽観ロック制御）
            customer_id (str): 取引先ID
            current_user (Behavior, optional): 認証済みのユーザー
        Raises:
            HTTPException: 404 Not found
            HTTPException: 409 Conflict

        Returns:
            GetCustomerByIdResponse: 取得結果
        """
        try:
            customer = CustomerModel.get(
                hash_key=customer_id, range_key=DataType.CUSTOMER
            )
        except DoesNotExist:
            logger.warning(
                f"UpdateCustomerById customer_id not found. customer_id: {customer_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 排他チェック
        if version != customer.version:
            logger.warning(
                f"UpdateCustomerById conflict. request_ver: {version} customer_ver: {customer.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        update_datetime = datetime.now()

        # 取引先テーブルの更新
        update_action: List[Action] = []
        if item.name is not None:
            update_action.append(CustomerModel.name.set(item.name))
        if item.category is not None:
            update_action.append(CustomerModel.category.set(item.category))
        update_action.append(CustomerModel.update_id.set(current_user.id))
        update_action.append(CustomerModel.update_at.set(update_datetime))
        customer.update(actions=update_action)

        if item.name is not None:
            # ユーザテーブルのcustomer_nameを更新
            with UserModel.batch_write() as user_batch:
                users_iter = UserModel.customer_id_email_index.query(
                    hash_key=customer.id
                )
                for user in users_iter:
                    user.customer_name = item.name
                    user.update_id = current_user.id
                    user.update_at = update_datetime
                    user.version += 1
                    user_batch.save(user)

            # 案件テーブルのcustomer_nameを更新
            with ProjectModel.batch_write() as project_batch:
                projects_iter = ProjectModel.customer_id_name_index.query(
                    hash_key=customer.id
                )
                for project in projects_iter:
                    project.customer_name = item.name
                    project.update_history = UpdateHistoryAttribute(
                        update_id=current_user.id,
                        update_attributes=[
                            UpdateAttributesSubAttribute(
                                attribute="customer_name", value=item.name
                            )
                        ],
                    )
                    project.update_id = current_user.id
                    project.update_at = update_datetime
                    project.version += 1
                    project_batch.save(project)

        return UpdateCustomerByIdResponse(
            update_user_name=current_user.name, **customer.attribute_values
        )

    @staticmethod
    def delete_customer_by_id(
        version: int, customer_id: str, current_user: AdminModel
    ) -> DeleteCustomerByIdResponse:
        """取引先IDを指定して取引先情報を削除する。
            取引先に紐づく情報案件または一般ユーザーがある場合削除は不可となる。

        Args:
            version (int): ロックキー（楽観ロック制御）
            customer_id (str): 取引先ID
            current_user (Behavior, optional): 認証済みのユーザー
        Raises:
            HTTPException: 404 Not found
            HTTPException: 409 Conflict

        Returns:
            DeleteCustomerByIdResponse: 削除結果
        """
        try:
            customer = CustomerModel.get(
                hash_key=customer_id, range_key=DataType.CUSTOMER
            )
        except DoesNotExist:
            logger.warning(
                f"DeleteCustomerById customer_id not found. customer_id: {customer_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 排他チェック
        if version != customer.version:
            logger.warning(
                f"DeleteCustomerById conflict. request_ver: {version} customer_ver: {customer.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 案件テーブルのチェック
        projects = ProjectModel.customer_id_name_index.query(hash_key=customer_id)
        for project in projects:
            # 取引先IDに紐づいた情報が案件に存在する場合、取引先は削除不可
            logger.warning("Cannot be deleted because related project info exists.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Cannot be deleted because related project info exists.",
            )
        # 一般ユーザテーブルのチェック
        users = UserModel.customer_id_email_index.query(hash_key=customer_id)
        for user in users:
            # 取引先IDに紐づいた情報が一般ユーザに存在する場合、取引先は削除不可
            logger.warning("Cannot be deleted because related user info exists.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Cannot be deleted because related user info exists.",
            )

        # 指定された取引先IDの情報を取引先から削除
        customer.delete()

        return DeleteCustomerByIdResponse(**customer.attribute_values)

    @staticmethod
    def check_csv_format(csv_dict_reader: csv.DictReader) -> bool:
        """CSVファイルのフォーマットチェック（規定ヘッダの有無チェック）

        Args:
            csv_dict_reader (csv.DictReader): CSVファイル情報

        Returns:
            bool: チェック結果
        """
        columns = [
            CsvColumn.NAME,
            CsvColumn.CATEGORY,
            CsvColumn.SALESFORCE_CUSTOMER_ID,
            CsvColumn.SALESFORCE_UPDATE_AT,
            CsvColumn.SALESFORCE_TARGET,
            CsvColumn.SALESFORCE_CREDIT_LIMIT,
            CsvColumn.SALESFORCE_CREDIT_GET_MONTH,
            CsvColumn.SALESFORCE_CREDIT_MANAGER,
            CsvColumn.SALESFORCE_CREDIT_NO_RETRY,
            CsvColumn.SALESFORCE_PAWS_CREDIT_NUMBER,
            CsvColumn.SALESFORCE_CUSTOMER_OWNER,
            CsvColumn.SALESFORCE_CUSTOMER_SEGMENT,
        ]
        for col in columns:
            if col not in csv_dict_reader.fieldnames:
                return False
        return True

    @staticmethod
    def get_import_response_customer_info(
        import_mode: ImportModeType, data: dict
    ) -> CustomerInfoForImportCustomers:
        """インポート処理のレスポンス編集

        Args:
            import_mode (ImportModeType): インポート処理モード
            data (dict): CSVファイルの行データ

        Returns:
            CustomerInfoForImportCustomers: 行単位のcustomer情報
        """
        return CustomerInfoForImportCustomers(
            name=data.get(CsvColumn.NAME),
            category=data.get(CsvColumn.CATEGORY),
            salesforce_customer_id=data.get(CsvColumn.SALESFORCE_CUSTOMER_ID),
            salesforce_update_at=data.get(CsvColumn.SALESFORCE_UPDATE_AT),
            salesforce_target=data.get(CsvColumn.SALESFORCE_TARGET),
            salesforce_credit_limit=data.get(CsvColumn.SALESFORCE_CREDIT_LIMIT),
            salesforce_credit_get_month=data.get(CsvColumn.SALESFORCE_CREDIT_GET_MONTH),
            salesforce_credit_manager=data.get(CsvColumn.SALESFORCE_CREDIT_MANAGER),
            salesforce_credit_no_retry=data.get(CsvColumn.SALESFORCE_CREDIT_NO_RETRY),
            salesforce_paws_credit_number=data.get(
                CsvColumn.SALESFORCE_PAWS_CREDIT_NUMBER
            ),
            salesforce_customer_owner=data.get(CsvColumn.SALESFORCE_CUSTOMER_OWNER),
            salesforce_customer_segment=data.get(CsvColumn.SALESFORCE_CUSTOMER_SEGMENT),
        )

    @staticmethod
    def edit_import_change_column_log(
        edit_log_list: List[dict],
        customer_name: str,
        row_change_log: List[dict],
    ) -> None:
        """インポート処理の更新箇所の辞書（取引先名、更新項目名、更新値）リストに追記
        e.g. [
                {
                    "customer_name": "CCC株式会社",
                    "update_column_info": [
                        {
                            "update_column": "最終更新日",
                            "update_value": "2021-01-01",
                        },
                        {
                            "update_column": "フラグ",
                            "update_value": "ON",
                        },
                    ],
                },
                {
                    "customer_name": "DDD株式会社",
                    "update_column_info": [
                        {
                            "update_column": "最終更新日",
                            "update_value": "2021-01-01",
                        },
                    ],
                },
            ],

        Args:
            edit_log_list (List[dict]): 更新用ログのリスト
            customer_name (str): 取引先名
            row_change_log (List[dict]): 変更点のリスト
        """
        edit_log_list.append(
            {
                "customer_name": customer_name,
                "update_column_info": row_change_log,
            }
        )

    @staticmethod
    def edit_target_customer(
        target_customer: CustomerModel,
        name: str,
        category: str,
        salesforce_update_at: datetime,
        salesforce_target: bool,
        salesforce_credit_limit: int,
        salesforce_credit_get_month: str,
        salesforce_credit_manager: str,
        salesforce_credit_no_retry: bool,
        salesforce_paws_credit_number: str,
        salesforce_customer_owner: str,
        salesforce_customer_segment: str,
        update_id: str,
        update_at: datetime,
    ) -> Tuple[List[dict], dict]:
        """取引先情報更新時の編集

        Args:
            target_customer (CustomerModel): 更新対象のCustomerModel
            name (str): 取引先名
            category (str): カテゴリ
            salesforce_update_at (datetime): Salesfoce最終更新日時
            salesforce_target (bool): Salesforce戦略ターゲット・コアターゲット
            salesforce_credit_limit (int): Salesforce月与信上限
            salesforce_credit_get_month (str):Salesforce与信決済取得年月
            salesforce_credit_manager (str):Salesforce与信取得担当者
            salesforce_credit_no_retry (bool):Salesforce与信再取得不要
            salesforce_paws_credit_number (str):SalesforcePAWS決裁番号
            salesforce_customer_owner (str):Salesforce取引先所有者
            salesforce_customer_segment (str):Salesforce業界セグメント
            update_id (str):更新ユーザID
            update_at (datetime):更新日時

        Returns:
            tuple: 以下の情報のタプル
                row_change_log (List[dict]): 新規登録の変更点
                customer_name_update_map (dict): 取引先名更新マップ(key:取引先ID, value:取引先名)
        """
        row_change_log: List[dict] = []
        customer_name_update_map: dict = {}
        if target_customer.name != name:
            target_customer.name = name
            row_change_log.append(
                {
                    "update_column": CsvColumn.NAME,
                    "update_value": name,
                }
            )
            # 取引先名の更新情報を格納
            customer_name_update_map[target_customer.id] = name
        if target_customer.category != category:
            target_customer.category = category
            row_change_log.append(
                {
                    "update_column": CsvColumn.CATEGORY,
                    "update_value": category,
                }
            )
        if target_customer.salesforce_update_at != salesforce_update_at:
            target_customer.salesforce_update_at = salesforce_update_at
            row_change_log.append(
                {
                    "update_column": CsvColumn.SALESFORCE_UPDATE_AT,
                    "update_value": salesforce_update_at.strftime("%Y/%m/%d %H:%M"),
                }
            )
        if target_customer.salesforce_target != salesforce_target:
            target_customer.salesforce_target = salesforce_target
            row_change_log.append(
                {
                    "update_column": CsvColumn.SALESFORCE_TARGET,
                    "update_value": salesforce_target,
                }
            )
        if target_customer.salesforce_credit_limit != salesforce_credit_limit:
            target_customer.salesforce_credit_limit = salesforce_credit_limit
            row_change_log.append(
                {
                    "update_column": CsvColumn.SALESFORCE_CREDIT_LIMIT,
                    "update_value": salesforce_credit_limit,
                }
            )
        if target_customer.salesforce_credit_get_month != salesforce_credit_get_month:
            target_customer.salesforce_credit_get_month = salesforce_credit_get_month
            row_change_log.append(
                {
                    "update_column": CsvColumn.SALESFORCE_CREDIT_GET_MONTH,
                    "update_value": salesforce_credit_get_month,
                }
            )
        if target_customer.salesforce_credit_manager != salesforce_credit_manager:
            target_customer.salesforce_credit_manager = salesforce_credit_manager
            row_change_log.append(
                {
                    "update_column": CsvColumn.SALESFORCE_CREDIT_MANAGER,
                    "update_value": salesforce_credit_manager,
                }
            )
        if target_customer.salesforce_credit_no_retry != salesforce_credit_no_retry:
            target_customer.salesforce_credit_no_retry = salesforce_credit_no_retry
            row_change_log.append(
                {
                    "update_column": CsvColumn.SALESFORCE_CREDIT_NO_RETRY,
                    "update_value": salesforce_credit_no_retry,
                }
            )
        if (
            target_customer.salesforce_paws_credit_number
            != salesforce_paws_credit_number
        ):
            target_customer.salesforce_paws_credit_number = (
                salesforce_paws_credit_number
            )
            row_change_log.append(
                {
                    "update_column": CsvColumn.SALESFORCE_PAWS_CREDIT_NUMBER,
                    "update_value": salesforce_paws_credit_number,
                }
            )
        if target_customer.salesforce_customer_owner != salesforce_customer_owner:
            target_customer.salesforce_customer_owner = salesforce_customer_owner
            row_change_log.append(
                {
                    "update_column": CsvColumn.SALESFORCE_CUSTOMER_OWNER,
                    "update_value": salesforce_customer_owner,
                }
            )
        if target_customer.salesforce_customer_segment != salesforce_customer_segment:
            target_customer.salesforce_customer_segment = salesforce_customer_segment
            row_change_log.append(
                {
                    "update_column": CsvColumn.SALESFORCE_CUSTOMER_SEGMENT,
                    "update_value": salesforce_customer_segment,
                }
            )
        target_customer.update_id = update_id
        target_customer.update_at = update_at
        target_customer.version += 1

        return row_change_log, customer_name_update_map

    @staticmethod
    def import_execute_parallel(
        row: dict,
        customers: List[CustomerInfoForImportCustomers],
        sf_customer_id_map: dict,
        customer_batch: BatchWrite[CustomerModel],
        current_user: AdminModel,
        update_datetime: datetime,
        customer_name_update_map: dict,
        edit_log_list: List[dict],
        new_log_list: List[str],
    ) -> None:
        """取引先情報の新規登録、更新の処理

        Args:
            row (dict): CSVの行データ
            customers (List[CustomerInfoForImportCustomers]): レスポンス項目
            sf_customer_id_map (dict): 取引先情報のDB取得結果から作成した辞書(key: salesforce_customer_id, value: CustomerModel)
            customer_batch (BatchWrite[CustomerModel]): BatchWriteオブジェクト
            current_user (AdminModel): 認証済のユーザ
            update_datetime (datetime): 更新日時
            customer_name_update_map (dict): 取引先名更新マップ(key:取引先ID, value:取引先名)
            edit_log_list (List[dict]): 更新用ログのリスト
            new_log_list (List[str]): 新規登録用ログのリスト
        """
        # 登録内容の編集
        name = row[CsvColumn.NAME] if row[CsvColumn.NAME] else None
        category = row[CsvColumn.CATEGORY] if row[CsvColumn.CATEGORY] else None
        salesforce_customer_id = (
            row[CsvColumn.SALESFORCE_CUSTOMER_ID]
            if row[CsvColumn.SALESFORCE_CUSTOMER_ID]
            else None
        )
        salesforce_update_at = (
            CustomerService.jst.localize(
                datetime.strptime(
                    row[CsvColumn.SALESFORCE_UPDATE_AT],
                    "%Y/%m/%d %H:%M",
                )
            )
            if row[CsvColumn.SALESFORCE_UPDATE_AT]
            else None
        )
        salesforce_target = (
            strtobool(row[CsvColumn.SALESFORCE_TARGET])
            if row[CsvColumn.SALESFORCE_TARGET]
            else None
        )
        salesforce_credit_limit = (
            int(row[CsvColumn.SALESFORCE_CREDIT_LIMIT])
            if row[CsvColumn.SALESFORCE_CREDIT_LIMIT]
            else None
        )
        salesforce_credit_get_month = (
            row[CsvColumn.SALESFORCE_CREDIT_GET_MONTH]
            if row[CsvColumn.SALESFORCE_CREDIT_GET_MONTH]
            else None
        )
        salesforce_credit_manager = (
            row[CsvColumn.SALESFORCE_CREDIT_MANAGER]
            if row[CsvColumn.SALESFORCE_CREDIT_MANAGER]
            else None
        )
        salesforce_credit_no_retry = (
            strtobool(row[CsvColumn.SALESFORCE_CREDIT_NO_RETRY])
            if row[CsvColumn.SALESFORCE_CREDIT_NO_RETRY]
            else None
        )
        salesforce_paws_credit_number = (
            row[CsvColumn.SALESFORCE_PAWS_CREDIT_NUMBER]
            if row[CsvColumn.SALESFORCE_PAWS_CREDIT_NUMBER]
            else None
        )
        salesforce_customer_owner = (
            row[CsvColumn.SALESFORCE_CUSTOMER_OWNER]
            if row[CsvColumn.SALESFORCE_CUSTOMER_OWNER]
            else None
        )
        salesforce_customer_segment = (
            row[CsvColumn.SALESFORCE_CUSTOMER_SEGMENT]
            if row[CsvColumn.SALESFORCE_CUSTOMER_SEGMENT]
            else None
        )

        bo_site_url = get_app_settings().bo_site_url
        is_new: bool = False
        is_update: bool = False
        row_change_log: List[dict] = []
        temp_customer_name_update_map: dict = {}

        if salesforce_customer_id not in sf_customer_id_map.keys():
            is_new = True
            # 新規登録
            new_customer = CustomerModel(
                id=str(uuid.uuid4()),
                data_type=DataType.CUSTOMER,
                name=name,
                category=category,
                salesforce_customer_id=salesforce_customer_id,
                salesforce_update_at=salesforce_update_at,
                salesforce_target=salesforce_target,
                salesforce_credit_limit=salesforce_credit_limit,
                salesforce_credit_get_month=salesforce_credit_get_month,
                salesforce_credit_manager=salesforce_credit_manager,
                salesforce_credit_no_retry=salesforce_credit_no_retry,
                salesforce_paws_credit_number=salesforce_paws_credit_number,
                salesforce_customer_owner=salesforce_customer_owner,
                salesforce_customer_segment=salesforce_customer_segment,
                create_id=current_user.id,
                create_at=update_datetime,
                update_id=current_user.id,
                update_at=update_datetime,
                version=1,
            )

        else:
            # 更新
            target_customer: CustomerModel = copy.deepcopy(
                sf_customer_id_map[salesforce_customer_id]
            )
            # DBのSF最終更新日時 < CSVのSF最終更新日時の場合のみ、更新
            if target_customer.salesforce_update_at < salesforce_update_at:
                is_update = True
                (
                    row_change_log,
                    temp_customer_name_update_map,
                ) = CustomerService.edit_target_customer(
                    target_customer=target_customer,
                    name=name,
                    category=category,
                    salesforce_update_at=salesforce_update_at,
                    salesforce_target=salesforce_target,
                    salesforce_credit_limit=salesforce_credit_limit,
                    salesforce_credit_get_month=salesforce_credit_get_month,
                    salesforce_credit_manager=salesforce_credit_manager,
                    salesforce_credit_no_retry=salesforce_credit_no_retry,
                    salesforce_paws_credit_number=salesforce_paws_credit_number,
                    salesforce_customer_owner=salesforce_customer_owner,
                    salesforce_customer_segment=salesforce_customer_segment,
                    update_id=current_user.id,
                    update_at=update_datetime,
                )

        # レスポンス編集
        response_customer_info = None
        if is_new or is_update:
            # 新規、更新の場合、レスポンス編集
            # 処理スキップした行はレスポンスに含めない
            response_customer_info = CustomerService.get_import_response_customer_info(
                ImportModeType.EXECUTE, row
            )
        # 並列処理の排他制御
        with CustomerService.lock:
            if is_new:
                customers.append(response_customer_info)
                customer_batch.save(new_customer)
                new_log_list.append(
                    {
                        "customer_name": name,
                        "customer_detail_url_bo": bo_site_url
                        + BoAppUrl.CUSTOMER_DETAIL.format(customerId=new_customer.id),
                    }
                )
            if is_update:
                customers.append(response_customer_info)
                customer_batch.save(target_customer)
                # メール/お知らせ通知用のログ編集（更新分）
                CustomerService.edit_import_change_column_log(
                    edit_log_list=edit_log_list,
                    customer_name=name,
                    row_change_log=row_change_log,
                )
                customer_name_update_map.update(temp_customer_name_update_map)

    @staticmethod
    def import_customer(
        item: ImportCustomersRequest, current_user: AdminModel
    ) -> ImportCustomersResponse:
        """顧客のCSVデータのエラーチェックまたは取り込みを行う.

        Args:
            item (ImportCustomersRequest): 登録内容
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            ImportCustomersResponse: 登録結果
        """

        # S3からCSV取得
        try:
            csv_data: str = S3Helper().get_object_content(
                bucket_name=get_app_settings().upload_s3_bucket_name,
                object_key=item.file,
                encoding=ImportFileEncodingType.CP932,
            )
        except botocore.exceptions.ClientError as e:
            if e.response["Error"]["Code"] == "NoSuchKey":
                logger.warning("ImportCustomers. File not found.")
                raise HTTPException(
                    status_code=status.HTTP_400_BAD_REQUEST, detail="File not found."
                )
            else:
                logger.error(e)
                raise e
        except UnicodeDecodeError:
            logger.warning("ImportCustomers. cp932 codec can't decode.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="cp932 codec can't decode.",
            )

        # 空ファイルチェック
        if not csv_data:
            logger.warning("ImportCustomers. Illegal CSV format.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST, detail="Illegal CSV format."
            )
        # CSVファイルの件数上限チェック
        csv_count = 0
        # DictReaderはイテレータのため、件数チェック用に別に読み込む
        csv_count_check_dict_reader = csv.DictReader(csv_data.splitlines())
        for row in csv_count_check_dict_reader:
            csv_count += 1
        if csv_count > ImportFileLimitCount.LIMIT:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail=f"Over {str(ImportFileLimitCount.LIMIT)} lines. CSV data contains {csv_count} lines.",
            )

        # CSV読込
        csv_dict_reader = csv.DictReader(csv_data.splitlines())
        # フォーマットチェック（ヘッダー項目の存在チェック）
        if not CustomerService.check_csv_format(csv_dict_reader):
            logger.warning("ImportCustomers. Illegal CSV format.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST, detail="Illegal CSV format."
            )

        customers: List[CustomerInfoForImportCustomers] = []
        result: ImportResultType = None

        if item.mode == ImportModeType.CHECK:
            # モード：check
            result = ImportResultType.OK
            for row in csv_dict_reader:
                # レスポンス編集
                customer_info = CustomerService.get_import_response_customer_info(
                    ImportModeType.CHECK, row
                )
                customer_info.error_message = []
                customers.append(customer_info)
                # 取引先ID（必須チェック）
                if not customer_info.salesforce_customer_id:
                    customer_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_CUSTOMER_ID
                        )
                    )

                # 最終更新日時（必須チェック、書式チェック）
                if not customer_info.salesforce_update_at:
                    customer_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_UPDATE_AT
                        )
                    )

                pattern_match = re.fullmatch(
                    r"\d{4}/\d{2}/\d{2} \d{1,2}:\d{2}",
                    customer_info.salesforce_update_at,
                )
                if not pattern_match:
                    customer_info.error_message.append(
                        Message.ImportCheckError.FORMAT_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_UPDATE_AT,
                            format_value="yyyy/mm/dd h:mm",
                        )
                    )

                try:
                    datetime.strptime(
                        customer_info.salesforce_update_at, "%Y/%m/%d %H:%M"
                    )
                except Exception:
                    customer_info.error_message.append(
                        Message.ImportCheckError.DATETIME_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_UPDATE_AT,
                        )
                    )

                # 取引先名（必須チェック）
                if not customer_info.name:
                    customer_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.NAME
                        )
                    )

                # 戦略ターゲット・コアターゲット（真理値変換可否チェック）
                if customer_info.salesforce_target:
                    if not Utils.is_bool(customer_info.salesforce_target):
                        customer_info.error_message.append(
                            Message.ImportCheckError.VALUE_ERROR.format(
                                column_name=CsvColumn.SALESFORCE_TARGET,
                                value="真理値（1または0）",
                            )
                        )

                # 月与信上限（数値チェック）
                if customer_info.salesforce_credit_limit:
                    if not Utils.is_int(customer_info.salesforce_credit_limit):
                        customer_info.error_message.append(
                            Message.ImportCheckError.VALUE_ERROR.format(
                                column_name=CsvColumn.SALESFORCE_CREDIT_LIMIT,
                                value="数値",
                            )
                        )

                # 与信決裁取得年月（書式チェック）
                if customer_info.salesforce_credit_get_month:
                    pattern_match = re.fullmatch(
                        r"\d{4}/\d{2}/\d{2}",
                        customer_info.salesforce_credit_get_month,
                    )
                    if not pattern_match:
                        customer_info.error_message.append(
                            Message.ImportCheckError.FORMAT_ERROR.format(
                                column_name=CsvColumn.SALESFORCE_CREDIT_GET_MONTH,
                                format_value="yyyy/mm/dd",
                            )
                        )

                    try:
                        datetime.strptime(
                            customer_info.salesforce_credit_get_month, "%Y/%m/%d"
                        )
                    except Exception:
                        customer_info.error_message.append(
                            Message.ImportCheckError.DATE_ERROR.format(
                                column_name=CsvColumn.SALESFORCE_CREDIT_GET_MONTH,
                            )
                        )

                # 与信再取得不要（真理値変換可否チェック）
                if customer_info.salesforce_credit_no_retry:
                    if not Utils.is_bool(customer_info.salesforce_credit_no_retry):
                        customer_info.error_message.append(
                            Message.ImportCheckError.VALUE_ERROR.format(
                                column_name=CsvColumn.SALESFORCE_CREDIT_NO_RETRY,
                                value="真理値（1または0）",
                            )
                        )

            for customer in customers:
                if customer.error_message:
                    result = ImportResultType.NG
                    continue

            # チェックOKの場合：登録されるデータ(重複レコード削除済みデータ)
            # チェックNGの場合：CSVファイルの全データ
            if result == ImportResultType.OK:
                # 重複チェック用の一時格納用配列
                duplicate_checked_customers: List[CustomerInfoForImportCustomers] = []

                for customer in customers:
                    # 重複要素の検索
                    duplicate_record = [
                        checked_customer
                        for checked_customer in duplicate_checked_customers
                        if customer.salesforce_customer_id
                        == checked_customer.salesforce_customer_id
                    ]

                    # 取引先IDの重複がなければ、そのまま新配列に追加
                    if not duplicate_record:
                        duplicate_checked_customers.append(customer)
                        continue

                    # 重複かつ更新日が古いものは、対象に登録しない
                    if datetime.strptime(
                        customer.salesforce_update_at, "%Y/%m/%d %H:%M"
                    ) < datetime.strptime(
                        duplicate_record[0].salesforce_update_at, "%Y/%m/%d %H:%M"
                    ):
                        continue

                    # 更新日が同じ、または新しい場合、既存の要素削除&新規要素を追加
                    duplicate_checked_customers.remove(duplicate_record[0])
                    duplicate_checked_customers.append(customer)
                    continue

                # 重複レコードの削除が済んだ配列を、改めてcustomersとして利用
                customers = duplicate_checked_customers

        elif item.mode == ImportModeType.EXECUTE:
            # モード：execute
            result = ImportResultType.DONE
            # 取引先情報取得（全件）
            customer_result_list: List[CustomerModel] = list(CustomerModel.scan())
            # DB取得結果から辞書を作成
            #  key: salesforce_customer_id, value: CustomerModel
            sf_customer_id_map: dict[str, CustomerModel] = {
                c.salesforce_customer_id: c for c in customer_result_list
            }

            # 取引先名を更新する取引先（案件、一般ユーザ更新用）
            #  key: id(取引先ID), value: name(更新後の取引先名)の辞書
            customer_name_update_map: dict[str, str] = {}

            # 新規登録の取引先名、お客様情報詳細URL(BO)のリスト
            #  key: "customer_name"(固定), value: 取引先名
            #   e.g. {
            #             "customer_name": "AAA株式会社",
            #             "customer_detail_url_bo": "https://bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/customer/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            #         }
            new_log_list: List[str] = []

            # 更新分（取引先名、更新項目名、更新値）のリスト
            # e.g. [
            #         {
            #             "customer_name": "CCC株式会社",
            #             "update_column_info": [
            #                 {
            #                     "update_column": "最終更新日",
            #                     "update_value": "2021-01-01",
            #                 },
            #                 {
            #                     "update_column": "フラグ",
            #                     "update_value": "ON",
            #                 },
            #             ],
            #         },
            #      ]
            edit_log_list: List[dict] = []

            update_datetime = datetime.now()
            try:
                with CustomerModel.batch_write() as customer_batch:
                    with ThreadPoolExecutor(
                        max_workers=ThreadPoolMaxWorkers.IMPORT_CUSTOMERS
                    ) as executor:
                        duplicate_checked_records = []  # 重複チェック用の一時格納用配列

                        for row in csv_dict_reader:
                            # 重複要素の検索
                            duplicate_record = [
                                checked_customer
                                for checked_customer in duplicate_checked_records
                                if row[CsvColumn.SALESFORCE_CUSTOMER_ID]
                                == checked_customer[CsvColumn.SALESFORCE_CUSTOMER_ID]
                            ]

                            # 取引先IDの重複がなければ、そのまま新配列に追加
                            if not duplicate_record:
                                duplicate_checked_records.append(row)
                                continue

                            # 重複かつ更新日が古いものは、対象に登録しない
                            if datetime.strptime(
                                row[CsvColumn.SALESFORCE_UPDATE_AT], "%Y/%m/%d %H:%M"
                            ) < datetime.strptime(
                                duplicate_record[0][CsvColumn.SALESFORCE_UPDATE_AT],
                                "%Y/%m/%d %H:%M",
                            ):
                                continue

                            # 更新日が同じ、または新しい場合、既存の要素削除&新規要素を追加
                            duplicate_checked_records.remove(duplicate_record[0])
                            duplicate_checked_records.append(row)
                            continue

                        # 重複レコードの削除が済んだ配列を、改めてrecordssとして利用
                        records = duplicate_checked_records

                        features = []
                        for row in records:
                            features.append(
                                executor.submit(
                                    CustomerService.import_execute_parallel,
                                    row=row,
                                    customers=customers,
                                    sf_customer_id_map=sf_customer_id_map,
                                    customer_batch=customer_batch,
                                    current_user=current_user,
                                    update_datetime=update_datetime,
                                    customer_name_update_map=customer_name_update_map,
                                    edit_log_list=edit_log_list,
                                    new_log_list=new_log_list,
                                )
                            )

                        # 結果（Exception）の取得
                        for feature in features:
                            feature.result()

                # 案件テーブルや一般ユーザテーブルの更新
                for customer_id, name in customer_name_update_map.items():
                    # ユーザテーブルのcustomer_nameを更新
                    with UserModel.batch_write() as user_batch:
                        users_iter = UserModel.customer_id_email_index.query(
                            hash_key=customer_id
                        )
                        for user in users_iter:
                            user.customer_name = name
                            user.update_id = current_user.id
                            user.update_at = update_datetime
                            user.version += 1
                            user_batch.save(user)

                    # 案件テーブルのcustomer_nameを更新
                    with ProjectModel.batch_write() as project_batch:
                        projects_iter = ProjectModel.customer_id_name_index.query(
                            hash_key=customer_id
                        )
                        for project in projects_iter:
                            project.customer_name = name
                            project.update_history = UpdateHistoryAttribute(
                                update_id=current_user.id,
                                update_attributes=[
                                    UpdateAttributesSubAttribute(
                                        attribute="customer_name", value=name
                                    )
                                ],
                            )
                            project.update_id = current_user.id
                            project.update_at = update_datetime
                            project.version += 1
                            project_batch.save(project)

                # メール通知
                if new_log_list or edit_log_list:
                    # 新規登録または更新がある場合のみ通知
                    payload = {
                        "add_info_list": new_log_list,
                        "update_info_list": edit_log_list,
                    }
                    to_email_list: List[str] = []
                    bo_notification_user_id_list: List[str] = []
                    admin_filter_condition = AdminModel.disabled == False # NOQA
                    for admin in AdminModel.scan(filter_condition=admin_filter_condition):
                        if (
                            UserRoleType.SYSTEM_ADMIN.key in admin.roles
                            or UserRoleType.MAN_HOUR_OPS.key in admin.roles
                            or UserRoleType.SURVEY_OPS.key in admin.roles
                        ):
                            to_email_list.append(admin.email)
                            bo_notification_user_id_list.append(admin.id)
                    # 宛先の重複を削除
                    to_email_list = list(set(to_email_list))

                    CustomerService.send_mail(
                        template=MailType.CUSTOMER_IMPORT_COMPLETED,
                        to_addr_list=to_email_list,
                        cc_addr_list=[],
                        payload=payload,
                    )

                    # お知らせ通知
                    # BO
                    # 重複を削除
                    bo_notification_user_id_list = list(
                        set(bo_notification_user_id_list)
                    )
                    notification_notice_at = datetime.now()
                    NotificationService.save_notification(
                        notification_type=NotificationType.SALESFORCE_CUSTOMER_IMPORT,
                        user_id_list=bo_notification_user_id_list,
                        message_param=payload,
                        url="",
                        noticed_at=notification_notice_at,
                        create_id=current_user.id,
                        update_id=current_user.id,
                    )

            except Exception:
                result = ImportResultType.ERROR
                customers = []
                logger.exception(Message.ImportExecuteError.EXECUTE_ERROR)

        return ImportCustomersResponse(
            mode=item.mode, result=result, customers=customers
        )
