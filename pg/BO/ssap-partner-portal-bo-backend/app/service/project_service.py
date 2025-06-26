import copy
import csv
import json
import re
import threading
import uuid
from concurrent.futures import ThreadPoolExecutor
from datetime import datetime, timedelta
from distutils.util import strtobool
from typing import List, Set, Union

import botocore.exceptions
from fastapi import HTTPException, status
from fastapi import status as api_status
from fastapi.security import HTTPAuthorizationCredentials
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.update import Action
from pynamodb.models import BatchWrite
from pytz import timezone

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.customer import CustomerModel
from app.models.man_hour import ManHourProjectSummaryModel
from app.models.master import MasterSupporterOrganizationModel
from app.models.notification import NotificationModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
    UpdateAttributesSubAttribute,
    UpdateHistoryAttribute,
)
from app.models.project_karte import ProjectKarteModel
from app.models.project_survey import ProjectSurveyModel
from app.models.user import UserModel
from app.resources.const import (
    BoAppUrl,
    ContractType,
    DataType,
    FoAppUrl,
    GetProjectsSortType,
    ImportFileEncodingType,
    ImportFileLimitCount,
    ImportFileProjectColumnName,
)
from app.resources.const import ImportFileProjectColumnName as CsvColumn
from app.resources.const import (
    ImportModeType,
    ImportResultType,
    MailType,
    MasterDataType,
    NotificationType,
)
from app.resources.const import ProjectColumnNameForUpdateLog as ProjectColumn
from app.resources.const import (
    ProjectCreateNewType,
    ProjectMemberRole,
    ProjectPhaseType,
    SuggestProjectsSortType,
    SupportStatusType,
    SurveyPasswordSetting,
    ThreadPoolMaxWorkers,
    TimezoneType,
    UserRoleType,
)
from app.resources.message import Message
from app.schemas.base import OKResponse
from app.schemas.project import (
    CreateProjectRequest,
    CreateProjectResponse,
    DetailInfoForGetMonthlyProjects,
    GetMonthlyProjectsReponse,
    GetProjectByIdResponse,
    GetProjectsResponse,
    Gross,
    HeaderInfoForGetMonthlyProjects,
    ImportProjectsRequest,
    ImportProjectsResponse,
    Organization,
    Profit,
    ProfitFy,
    ProjectInfoForGetProjects,
    ProjectInfoForImportProjects,
    ProjectMember,
    SalesforceMainCustomer,
    SuggestProjectsResponse,
    UpdateProjectByIdRequest,
    UserId,
    UserIdName,
    UserName,
)
from app.service.common_service.cached_db_items import CachedDbItems
from app.service.common_service.pagination import EmptyPage, Paginator
from app.service.common_service.user_info import get_update_user_name
from app.service.notification_service import NotificationService
from app.utils.aws.s3 import S3Helper
from app.utils.aws.sqs import SqsHelper
from app.utils.date import get_datetime_now
from app.utils.utils import Utils
from app.utils.cipher_aes import AesCipherUtils
from app.utils.encryption import create_random_survey_password
from app.utils.platform import PlatformApiOperator

logger = CustomLogger.get_logger()


class ProjectService:

    jst = timezone(TimezoneType.ASIA_TOKYO)

    # 並列処理時の排他制御（ロック）
    lock = threading.Lock()

    # CSVの粗利項目(YEAR以外)
    profit_month_keys = [
        CsvColumn.PROFIT_MONTHLY_01,
        CsvColumn.PROFIT_MONTHLY_02,
        CsvColumn.PROFIT_MONTHLY_03,
        CsvColumn.PROFIT_MONTHLY_04,
        CsvColumn.PROFIT_MONTHLY_05,
        CsvColumn.PROFIT_MONTHLY_06,
        CsvColumn.PROFIT_MONTHLY_07,
        CsvColumn.PROFIT_MONTHLY_08,
        CsvColumn.PROFIT_MONTHLY_09,
        CsvColumn.PROFIT_MONTHLY_10,
        CsvColumn.PROFIT_MONTHLY_11,
        CsvColumn.PROFIT_MONTHLY_12,
    ]
    profit_quarter_keys = [
        CsvColumn.PROFIT_QUARTERLY_1Q,
        CsvColumn.PROFIT_QUARTERLY_2Q,
        CsvColumn.PROFIT_QUARTERLY_3Q,
        CsvColumn.PROFIT_QUARTERLY_4Q,
    ]
    profit_half_keys = [CsvColumn.PROFIT_HALF_1H, CsvColumn.PROFIT_HALF_2H]

    # CSVの売上項目(YEAR以外)
    gross_month_keys = [
        CsvColumn.GROSS_MONTHLY_01,
        CsvColumn.GROSS_MONTHLY_02,
        CsvColumn.GROSS_MONTHLY_03,
        CsvColumn.GROSS_MONTHLY_04,
        CsvColumn.GROSS_MONTHLY_05,
        CsvColumn.GROSS_MONTHLY_06,
        CsvColumn.GROSS_MONTHLY_07,
        CsvColumn.GROSS_MONTHLY_08,
        CsvColumn.GROSS_MONTHLY_09,
        CsvColumn.GROSS_MONTHLY_10,
        CsvColumn.GROSS_MONTHLY_11,
        CsvColumn.GROSS_MONTHLY_12,
    ]
    gross_quarter_keys = [
        CsvColumn.GROSS_QUARTERLY_1Q,
        CsvColumn.GROSS_QUARTERLY_2Q,
        CsvColumn.GROSS_QUARTERLY_3Q,
        CsvColumn.GROSS_QUARTERLY_4Q,
    ]
    gross_half_keys = [CsvColumn.GROSS_HALF_1H, CsvColumn.GROSS_HALF_2H]

    @staticmethod
    def check_enum_project_phase_type(target: str) -> bool:
        """フェーズの列挙値チェック"""
        phases = [c.value for _, c in ProjectPhaseType.__members__.items()]
        if target not in phases:
            return False
        return True

    @staticmethod
    def remove_prefix_num(csv_column_value: str) -> str:
        """CSVの項目値から先頭に付与されたコード値を除去

        Args:
            csv_column_value (str): 先頭にコード値が付与された項目値
                e.g. 「新規・更新」項目の値("01. 更新","02. 新規"))
        Returns:
            bool: True(新規)、False(更新)
        """
        return re.sub(r"^\d+\.", "", csv_column_value).strip()

    @staticmethod
    def convert_create_new_to_bool(str_create_new: str) -> bool:
        """CSVの「新規・更新」項目の値を真理値に変換

        Args:
            str_create_new (str): 「新規・更新」項目の値("01. 更新","02. 新規"))
        Returns:
            bool: True(新規)、False(更新)
        Exception:
            ValueError: 真理値以外の場合

        """
        if str_create_new == ProjectCreateNewType.NEW:
            return True
        if str_create_new == ProjectCreateNewType.UPDATE:
            return False
        logger.warning("Column 'create_new' is invalid.")
        raise ValueError("Column 'create_new' is invalid.")

    @staticmethod
    def is_bool_create_new(str_create_new: str) -> bool:
        """CSVの「新規・更新」項目の値が真理値に変換可能か判定.
            列挙値チェック.

        Args:
            str_create_new (str): 「新規・更新」項目の値("01. 更新","02. 新規"))
        Returns:
            bool: True("01. 更新","02. 新規"の場合)、False(それ以外)

        """
        """フェーズの列挙値チェック"""
        create_new_list = [c.value for _, c in ProjectCreateNewType.__members__.items()]
        if str_create_new not in create_new_list:
            return False
        return True

    @staticmethod
    def is_visible_project(current_user: AdminModel, project: ProjectModel) -> bool:
        """案件情報がアクセス可能か判定.
          ユーザ情報は管理ユーザ情報から取得.
            1.制限なし(アクセス可)
              ・営業担当者
              ・営業責任者
              ・アンケート事務局
              ・稼働率調査事務局
              ・システム管理者
              ・事業者責任者
            2.支援者責任者
              「自身の課以外の非公開案件」：アクセス不可
              上記以外：アクセス可
        Args:
            current_user (AdminModel): ログインユーザ
            project (ProjectModel)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        access_ctrl_flag = True

        for role in current_user.roles:
            if role in [
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True
            elif role == UserRoleType.SUPPORTER_MGR.key:
                if not project.supporter_organization_id and project.is_secret:
                    # 自身の課以外の非公開案件
                    # (支援者組織IDがNoneまたは空の場合も自身の課以外とする)
                    access_ctrl_flag = False
                elif (
                    project.supporter_organization_id
                    not in current_user.supporter_organization_id
                    and project.is_secret
                ):
                    # 自身の課以外の非公開案件
                    access_ctrl_flag = False

        return access_ctrl_flag

    @staticmethod
    def is_visible_karte_by_sales(
        current_user: AdminModel, project: ProjectModel, user_list: List[UserModel]
    ) -> bool:
        """営業担当者が案件情報へアクセス可能か判定.
          案件に所属しているか判定する際は一般ユーザ情報を使用する.
            1.制限なし(アクセス可)
              ・支援者責任者
              ・営業責任者
              ・アンケート事務局
              ・稼働率調査事務局
              ・システム管理者
              ・事業者責任者
            2.営業担当者
              「所属していない非公開案件」：アクセス不可
              上記以外：アクセス可
        Args:
            user_id (str): ログインユーザID
            project (ProjectModel)
            user_list (List[UserModel])
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        access_ctrl_flag = True

        # アカウントが有効で、メールアドレスが一致する一般ユーザ情報を取得
        user_info: UserModel = None
        for user in user_list:
            if not user.disabled and user.email == current_user.email:
                user_info = user
                break

        for role in current_user.roles:
            if role in [
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True
            elif role == UserRoleType.SALES.key:
                if user_info:
                    # 一般ユーザ情報が存在する場合
                    if (
                        not user_info.project_ids
                        or project.id not in user_info.project_ids
                    ) and project.is_secret:
                        # 所属していない非公開案件
                        access_ctrl_flag = False
                else:
                    # 一般ユーザ情報が存在しない場合
                    if project.is_secret:
                        # 所属していない非公開案件
                        # (一般ユーザ情報が存在しない場合も所属していないとみなす)
                        access_ctrl_flag = False

        return access_ctrl_flag

    @staticmethod
    def is_visible_man_hour_by_supporter_mgr(
        current_user: AdminModel, man_hour: ManHourProjectSummaryModel
    ) -> bool:
        """支援工数情報がアクセス可能か判定.
          ユーザ情報は管理ユーザ情報から取得.
          支援者責任者の場合、自身の課の案件のみ参照可能.
          上記以外のユーザの場合は、Trueで返却.

            1.支援者責任者
              自身の課の案件：アクセス可
              上記以外：アクセス不可
            2.上記以外のユーザ
              アクセス可(True)を返却

        Args:
            current_user (AdminModel): ログインユーザID
            man_hour (ManHourProjectSummaryModel)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        access_ctrl_flag = True

        for role in current_user.roles:
            if role in [
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True
            elif role == UserRoleType.SUPPORTER_MGR.key:
                if not man_hour.supporter_organization_id:
                    # 自身の課以外の案件
                    # (支援者組織IDがNoneまたは空の場合も自身の課以外とする)
                    access_ctrl_flag = False
                elif (
                    man_hour.supporter_organization_id
                    not in current_user.supporter_organization_id
                ):
                    # 自身の課以外の案件
                    access_ctrl_flag = False

        return access_ctrl_flag

    @staticmethod
    def get_customer_name(customer_id: str) -> Union[str, None, HTTPException]:
        """取引先からidを条件にnameを取得.
            取得できない場合は400エラーを発行.
            customer_idがNoneまたは空の場合、Noneを返却.

        Args:
            customer_id (str): 取引先ID

        Raise:
            HTTP_400_BAD_REQUEST

        Returns:
            str: 取引先名
        """
        if not customer_id:
            return None

        try:
            customer = CustomerModel.get(
                hash_key=customer_id, range_key=DataType.CUSTOMER
            )
        except DoesNotExist:
            logger.warning(f"Not exist. customerId: {customer_id}")
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail=[{"loc": ["body", "customerId"], "msg": "Not exist."}],
            )
        return customer.name

    @staticmethod
    def get_user_info(user_id_list: List[str]) -> List[UserModel]:
        user_model_list = []
        item_keys = [(id, DataType.USER) for id in user_id_list]
        for item in UserModel.batch_get(item_keys):
            user_model_list.append(item)
        return user_model_list

    @staticmethod
    def get_user_name(
        id: str, user_table_info: List[UserModel], column_name_for_log: str
    ) -> Union[str, None, HTTPException]:
        """一般ユーザからidを条件にnameを取得.
            取得できない場合は400エラーを発行.
            idがNoneまたは空の場合、Noneを返却.

        Args:
            id (str): 一般ユーザID
            user_table_info (List[UserModel]): 一般ユーザから取得した情報
            column_name_for_log: 400エラーのdetailに出力する項目名

        Raise:
            HTTP_400_BAD_REQUEST

        Returns:
            str: 一般ユーザ名
        """
        if not id:
            return None

        for user in user_table_info:
            if id == user.id:
                return user.name

        logger.warning(f"Not exist user name. user_id: {user.id}")
        raise HTTPException(
            status_code=api_status.HTTP_400_BAD_REQUEST,
            detail=[{"loc": ["body", column_name_for_log], "msg": "Not exist."}],
        )

    @staticmethod
    def get_service_name(service_type: str) -> Union[str, HTTPException]:
        """汎用マスタからサービス区分名を取得.
            取得できない場合は400エラーを発行.
            service_typeがNoneまたは空の場合、Noneを返却.
        Args:
            service_type (str): サービス区分
        Raise:
            HTTP_400_BAD_REQUEST
        Returns:
            str: サービス区分名
        """
        if not service_type:
            return None

        try:
            master = MasterSupporterOrganizationModel.get(
                hash_key=service_type,
                range_key=MasterDataType.MASTER_SERVICE_TYPE.value,
            )
        except DoesNotExist:
            logger.warning(f"Not exist. serviceType: {service_type}")
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail=[{"loc": ["body", "serviceType"], "msg": "Not exist."}],
            )
        return master.name

    @staticmethod
    def cached_get_service_name(
        service_type: str, none: bool
    ) -> Union[str, HTTPException]:
        """汎用マスタからサービス区分名を取得.
            取得できずnoneフラグがFalseの場合は400エラーを発行.
            service_typeがNoneまたは空の場合、Noneを返却.

        Args:
            service_type (str): サービス区分

        Raise:
            HTTP_400_BAD_REQUEST

        Returns:
            str: サービス区分名
        """
        if not service_type:
            return None

        service_type_name = None

        # サービスタイプ区分の一覧を取得
        service_types = CachedDbItems.ReturnServiceTypes()

        # 一覧に指定されたサービスタイプIDが含まれているか検索
        for current_service_type in service_types:
            if service_type == current_service_type["id"]:
                service_type_name = current_service_type["name"]
                break

        if service_type_name:
            return service_type_name
        if not service_type_name and none:
            return ""
        else:
            logger.warning(f"Not exist. serviceType: {service_type}")
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail=[{"loc": ["body", "serviceType"], "msg": "Not exist."}],
            )

    @staticmethod
    def get_supporter_organization_name(
        supporter_organization_id: str,
    ) -> Union[str, None, HTTPException]:
        """汎用マスタから支援者組織名を取得.
            取得できない場合は400エラーを発行.
            supporter_organization_idがNoneまたは空の場合、Noneを返却.
        Args:
            supporter_organization_id (str): 支援者組織（粗利メイン課、アンケート集計課）ID
        Raise:
            HTTP_400_BAD_REQUEST
        Returns:
            str: 支援者組織名
        """
        if not supporter_organization_id:
            return None

        try:
            master = MasterSupporterOrganizationModel.get(
                hash_key=supporter_organization_id,
                range_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                attributes_to_get=MasterSupporterOrganizationModel.value,
            )
        except DoesNotExist:
            logger.warning(
                f"Not exist. supporterOrganizationId: {supporter_organization_id}"
            )
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail=[
                    {"loc": ["body", "supporterOrganizationId"], "msg": "Not exist."}
                ],
            )
        return master.value

    @staticmethod
    def cached_get_supporter_organization_name(
        supporter_organization_id: str,
    ) -> Union[str, None, HTTPException]:
        """汎用マスタから支援者組織名を取得.
            取得できない場合は400エラーを発行.
            supporter_organization_idがNoneまたは空の場合、Noneを返却.

        Args:
            supporter_organization_id (str): 支援者組織（粗利メイン課、アンケート集計課）ID

        Raise:
            HTTP_400_BAD_REQUEST

        Returns:
            str: 支援者組織名
        """
        if not supporter_organization_id:
            return None

        supporter_organization_name = None

        # 支援者組織の一覧を取得
        supporter_organizations = CachedDbItems.ReturnSupporterOrganizations()

        # 複数課の場合 例）IST;VST
        if ';' in supporter_organization_id:
            # 課idのリストに変換
            supporter_organization_id_list = supporter_organization_id.split(';')
            organization_names = ""

            for supporter_organization_id in supporter_organization_id_list:
                for current_supporter_organization in supporter_organizations:
                    if supporter_organization_id == current_supporter_organization["id"]:
                        organization_names += current_supporter_organization["name"]
                        organization_names += ";"
            # 文末の「;」を削除
            supporter_organization_name = organization_names.rstrip(";")
        else:
            # 単一課の場合
            for current_supporter_organization in supporter_organizations:
                if supporter_organization_id == current_supporter_organization["id"]:
                    supporter_organization_name = current_supporter_organization["name"]
                    break

        if supporter_organization_name:
            return supporter_organization_name
        else:
            logger.warning(
                f"Not exist. supporterOrganizationId: {supporter_organization_id}"
            )
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail=[
                    {"loc": ["body", "supporterOrganizationId"], "msg": "Not exist."}
                ],
            )

    @staticmethod
    def convert_user_id_class_list_to_set(user_id_class_list: List[UserId]) -> Set:
        """UserIdクラスのリストをSetに変換.
            パラメータのuser_id_class_listがNoneの場合、Noneを返却.
            e.g. [{'id':'aaa'},{'id':'bbb'}] -> {'aaa','bbb'}

        Args:
            user_id_class_list (List[UserId]): 変換対象

        Returns:
            Set: 変換結果
        """
        if user_id_class_list is None:
            return None

        conv_list = [user_id_class.id for user_id_class in user_id_class_list]
        return set(conv_list)

    @staticmethod
    def get_user_id_name_class_list(
        user_id_class_list: List[UserId],
        user_table_info: List[UserModel],
        column_name_for_log: str,
    ) -> List[UserIdName]:
        """UserIdクラスのリストをUserIdNameクラスのリストに変換.
            idが一般ユーザに存在するかチェックし、nameを取得.
            存在しない場合は400エラーを発行.
            パラメータのuser_id_class_listがNoneの場合、Noneを返却.
            e.g. [{'id':'aaa'},{'id':'bbb'}] -> [{'id':'aaa','name':'山田太郎'},{'id':'bbb','name':'山田次郎'}]

        Args:
            user_id_class_list (List[UserId]): ユーザid情報のリスト
            user_table_info (List[UserModel]): 一般ユーザから取得した情報
            column_name_for_log: 400エラーのdetailに出力する項目名

        Returns:
            List[UserIdName]: ユーザid,name情報のリスト
        """
        if user_id_class_list is None:
            return None

        user_id_name_list = []
        for user_id in user_id_class_list:
            name = ""
            for info in user_table_info:
                if info.id == user_id.id:
                    name = info.name
                    break
            if not name:
                logger.warning(f"Not exist. user: {user_id.id}")
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail=[
                        {"loc": ["body", column_name_for_log], "msg": "Not exist."}
                    ],
                )
            user_id_name_list.append(UserIdName(id=user_id.id, name=name))

        return user_id_name_list

    @staticmethod
    def send_mail(
        template,
        to_addr_list: List[str],
        cc_addr_list: List[str],
        payload: dict,
        bcc_addr_list: List[str] = [],
    ):
        queue_name = get_app_settings().sqs_email_queue_name
        message_body = {
            "template": template,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "bcc": bcc_addr_list,
            "payload": payload,
        }
        sqs_message_body = json.dumps(message_body)
        SqsHelper().send_message_to_queue(
            queue_name=queue_name, message_body=sqs_message_body
        )

    @staticmethod
    def send_mail_batch(entries: List[dict]):
        queue_name = get_app_settings().sqs_email_queue_name
        SqsHelper().send_message_batch_to_queue(queue_name=queue_name, entries=entries)

    @staticmethod
    def distribute_profit(profit_fy: ProfitFy) -> Profit:
        """FY粗利を月毎、Q毎、H毎に按分.

        Args:
            profit_fy (ProfitFy): FY粗利

        Returns:
            Profit: 粗利
        """
        _digits = 2
        half = round(profit_fy.year / 2, _digits)
        quarterly = round(profit_fy.year / 4, _digits)
        monthly = round(profit_fy.year / 12, _digits)

        profit: Profit = Profit(
            year=profit_fy.year,
            half=[half for _ in range(2)],
            quarterly=[quarterly for _ in range(4)],
            monthly=[monthly for _ in range(12)],
        )
        return profit

    @staticmethod
    def divide_user_id(before_list: List[str], target_list: List[str]) -> dict:
        """UPDATE時に更新前のユーザIDリストと更新するユーザIDリストを基に、
            案件情報から除外されたユーザIDと新規に追加されたユーザIDに振り分ける。

        Args:
            before_list (List[str]): 更新前のユーザIDリスト
            target_list (List[str]): 更新するユーザIDリスト

        Returns:
            dict: 除外リストと追加リストを含む辞書
                  exclude: 除外リスト（List）
                  add: 追加リスト（List）
        """
        exclude_list = []
        add_list = []
        for before_user_id in before_list:
            if before_user_id not in target_list:
                exclude_list.append(before_user_id)
        for target_user_id in target_list:
            if target_user_id not in before_list:
                add_list.append(target_user_id)
        return {"exclude": exclude_list, "add": add_list}

    @staticmethod
    def check_valid_user(item: Union[CreateProjectRequest, UpdateProjectByIdRequest], user_table_info: List[UserModel]):
        """有効なユーザか判定

        Args:
            item (Union[CreateProjectRequest, UpdateProjectByIdRequest])
            user_table_info (List[UserModel]): 一般ユーザ情報

        Returns:
            なし
        """
        for user_info in user_table_info:
            if user_info.disabled:
                # 商談所有者
                if user_info.id == item.main_sales_user_id:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="disabled mainSales is set",
                    )
                # お客様（代表）
                if user_info.id == item.main_customer_user_id:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="disabled mainCustomer is set",
                    )
                # プロデューサー
                if user_info.id == item.main_supporter_user_id:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="disabled mainSupporter is set",
                    )
                # お客様
                if item.customer_users:
                    for customer_user in item.customer_users:
                        if user_info.id == customer_user.id:
                            raise HTTPException(
                                status_code=status.HTTP_400_BAD_REQUEST,
                                detail="disabled customer is set",
                            )
                # アクセラレーター
                if item.supporter_users:
                    for supporter_user in item.supporter_users:
                        if user_info.id == supporter_user.id:
                            raise HTTPException(
                                status_code=status.HTTP_400_BAD_REQUEST,
                                detail="disabled supporter is set",
                            )

    @staticmethod
    def create_project(
        item: CreateProjectRequest,
        current_user: AdminModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> CreateProjectResponse:
        """案件情報を登録する

        Args:
            item (CreateProjectRequest): 登録内容
            current_user (Behavior, optional): 認証済みのユーザー
            authorization (HTTPAuthorizationCredentials): 認証情報

        Returns:
            CreateProjectResponse: 登録結果
        """
        # 取引先IDの存在チェック、取引先名の取得
        customer_name = ProjectService.get_customer_name(item.customer_id)
        # 指定されたサービス区分を汎用マスタで存在チェック、サービス区分の名称を取得
        service_type_name = ProjectService.get_service_name(item.service_type)
        # 支援者組織（課）IDを汎用マスタで存在チェック、支援者組織（課）名を取得
        supporter_organization_name = ProjectService.get_supporter_organization_name(
            item.supporter_organization_id
        )

        # 一般ユーザの取得
        target_user_id_list = []
        if item.main_sales_user_id:
            target_user_id_list.append(item.main_sales_user_id)
        if item.main_customer_user_id:
            target_user_id_list.append(item.main_customer_user_id)
        if item.main_supporter_user_id:
            target_user_id_list.append(item.main_supporter_user_id)
        if item.customer_users:
            for customer_user in item.customer_users:
                target_user_id_list.append(customer_user.id)
        if item.supporter_users:
            for supporter_user in item.supporter_users:
                target_user_id_list.append(supporter_user.id)
        # 重複を削除
        target_user_id_list = list(set(target_user_id_list))
        user_table_info: List[UserModel] = ProjectService.get_user_info(
            target_user_id_list
        )

        # ユーザの有効化チェック
        ProjectService.check_valid_user(item, user_table_info)

        # 営業担当者IDの存在チェック。営業担当者名を取得。
        main_sales_user_name = ProjectService.get_user_name(
            item.main_sales_user_id, user_table_info, "mainSalesUserId"
        )
        # 担当顧客（取引先責任者）IDの存在チェック。担当顧客名を取得。
        main_customer_user_name = ProjectService.get_user_name(
            item.main_customer_user_id, user_table_info, "mainCustomerUserId"
        )
        # メイン支援者（主担当）IDの存在チェック。メイン支援者名を取得。
        main_supporter_user_name = ProjectService.get_user_name(
            item.main_supporter_user_id, user_table_info, "mainSupporterUserId"
        )
        # 顧客メンバーID、支援メンバー（副担当）IDの存在チェック。名称を取得しレスポンス編集。
        res_customer_users = ProjectService.get_user_id_name_class_list(
            item.customer_users, user_table_info, "customerUsers"
        )
        res_supporter_users = ProjectService.get_user_id_name_class_list(
            item.supporter_users, user_table_info, "supporterUsers"
        )

        # 「案件情報新規登録通知」メールの宛先TOに入れるユーザID
        # （案件にアサインされた支援者、事業者責任者、営業担当者）
        to_mail_user_id_list = [item.main_sales_user_id, item.main_supporter_user_id]
        if item.supporter_users:
            for supporter_user in item.supporter_users:
                to_mail_user_id_list.append(supporter_user.id)
        # 重複を削除
        to_mail_user_id_list = list(set(to_mail_user_id_list))

        # お知らせ（案件情報新規登録通知）を通知するユーザID
        fo_notification_user_id_list = []
        bo_notification_user_id_list = []

        # 「案件アサイン通知」メールの宛先TOに入れるユーザID
        # （支援者or支援者責任者or事業者責任者or営業担当者orお客様メンバー）
        to_assign_mail_user_id_list = [
            item.main_sales_user_id,
            item.main_supporter_user_id,
            item.main_customer_user_id,
        ]
        if item.supporter_users:
            for supporter_user in item.supporter_users:
                to_assign_mail_user_id_list.append(supporter_user.id)
        if item.customer_users:
            for customer_user in item.customer_users:
                to_assign_mail_user_id_list.append(customer_user.id)
        # 重複を削除
        to_assign_mail_user_id_list = list(set(to_assign_mail_user_id_list))

        # お知らせ（アサイン）を通知するユーザID
        fo_assign_notification_user_id_list = []

        # 粗利項目の編集
        # FY粗利を按分し、月毎、Q毎、H毎の粗利を算出
        distributed_profit = ProjectService.distribute_profit(item.profit)

        # 匿名アンケートパスワードの暗号化
        encrypted_survey_password = (
            AesCipherUtils.encrypt(item.survey_password) if item.survey_password else ""
        )

        create_datetime = datetime.now()
        project = ProjectModel(
            id=str(uuid.uuid4()),
            data_type=DataType.PROJECT,
            customer_id=item.customer_id,
            name=item.name,
            customer_name=customer_name,
            service_type=item.service_type,
            create_new=item.create_new,
            main_sales_user_id=item.main_sales_user_id,
            phase=item.phase,
            customer_success=item.customer_success,
            support_date_from=item.support_date_from,
            support_date_to=item.support_date_to,
            profit=GrossProfitAttribute(
                monthly=distributed_profit.monthly,
                quarterly=distributed_profit.quarterly,
                half=distributed_profit.half,
                year=distributed_profit.year,
            ),
            total_contract_time=item.total_contract_time,
            main_customer_user_id=item.main_customer_user_id,
            salesforce_main_customer=SalesforceMainCustomerAttribute(
                name=item.salesforce_main_customer.name,
                email=item.salesforce_main_customer.email,
                organization_name=item.salesforce_main_customer.organization_name,
            ),
            customer_user_ids=ProjectService.convert_user_id_class_list_to_set(
                item.customer_users
            ),
            main_supporter_user_id=item.main_supporter_user_id,
            supporter_organization_id=item.supporter_organization_id,
            salesforce_main_supporter_user_name=item.salesforce_main_supporter_user_name,
            supporter_user_ids=ProjectService.convert_user_id_class_list_to_set(
                item.supporter_users
            ),
            salesforce_supporter_user_names=item.salesforce_supporter_user_names,
            is_count_man_hour=item.is_count_man_hour,
            is_karte_remind=item.is_karte_remind,
            is_master_karte_remind=item.is_master_karte_remind,
            contract_type=item.contract_type,
            is_secret=item.is_secret,
            is_solver_project=item.is_solver_project,
            dedicated_survey_user_name=item.dedicated_survey_user_name,
            dedicated_survey_user_email=item.dedicated_survey_user_email,
            survey_password=encrypted_survey_password,
            is_survey_email_to_salesforce_main_customer=item.is_survey_email_to_salesforce_main_customer,
            create_id=current_user.id,
            create_at=create_datetime,
            update_id=current_user.id,
            update_at=create_datetime,
        )

        project.save()

        # PF案件情報を登録
        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        try:
            customer = CustomerModel.get(
                hash_key=item.customer_id, range_key=DataType.CUSTOMER
            )
        except DoesNotExist:
            raise HTTPException(
                status_code=api_status.HTTP_500_INTERNAL_SERVER_ERROR,
                detail={"message": "Not exist customer."},
            )

        post_pf_project_params = {
            "partnerPortalCustomerId": item.customer_id
            if customer and not customer.salesforce_customer_id
            else None,
            "partnerPortalProjectId": project.id,
            "salesforceCustomerId": customer.salesforce_customer_id
            if customer and customer.salesforce_customer_id
            else None,
            "partnerPortalLastUpdateDate": project.update_at,
            "partnerPortalProjectName": item.name,
            "projectName": item.name,
            "customerName": ProjectService.get_customer_name(item.customer_id),
            "serviceType": service_type_name,
            "profit": supporter_organization_name,
            "projectOwner": item.salesforce_main_customer.name,
            "phase": item.phase,
            "customerSuccess": item.customer_success,
            "partnerPortalProfit": item.profit.year,
            "partnerPortalGross": project.gross
            if (project.gross and project.gross.year)
            else None,
            "serviceManager": "",
            "directSupportManHour": item.total_contract_time,
            "supportDateFrom": item.support_date_from,
            "supportDateTo": item.support_date_to,
            "unsentQuestionnaireReason": "",
            "questionnaireType": "",
            "isAlignment": False,
            "isPublishable": not item.is_secret,
        }

        (
            status_code,
            craate_project_response,
        ) = platform_api_operator.create_project(params=post_pf_project_params)
        logger.info(f"platform postProjectByPfId statusCode: {status_code}")
        if status_code != 200:
            logger.info(
                "platform postProjectByPfId response:"
                + json.dumps(craate_project_response)
            )
            raise HTTPException(
                status_code=status_code,
                detail=json.dumps(craate_project_response),
            )

        # 当期支援の作成
        status_code, create_program_response = platform_api_operator.create_program(
            params={
                "projectId": craate_project_response["id"],
                "isCurrent": True,
            }
        )
        if status_code != 200:
            logger.info(
                "platform CraateProgramAPI response:"
                + json.dumps(create_program_response)
            )
            raise HTTPException(
                status_code=status_code,
                detail=json.dumps(create_program_response),
            )

        # 一般ユーザの更新
        update_datetime = datetime.now()
        if user_table_info:
            to_email_list = []
            cc_email_list = []
            to_assign_email_list = []
            with UserModel.batch_write() as user_batch:
                # 更新対象の情報が存在する場合、他処理の更新を考慮し最新情報を取得
                user_table_info = ProjectService.get_user_info(target_user_id_list)
                for user in user_table_info:
                    if user.id in to_mail_user_id_list:
                        to_email_list.append(user.email)
                        fo_notification_user_id_list.append(user.id)
                    if user.id in to_assign_mail_user_id_list:
                        to_assign_email_list.append(user.email)
                        fo_assign_notification_user_id_list.append(user.id)

                    if user.project_ids is not None:
                        user.project_ids.add(project.id)
                    else:
                        user.project_ids = set([project.id])
                    user.update_id = current_user.id
                    user.update_at = update_datetime
                    user.version += 1
                    user_batch.save(user)

            fo_site_url = get_app_settings().fo_site_url
            bo_site_url = get_app_settings().bo_site_url
            fo_karte_list_url = fo_site_url + FoAppUrl.KARTE_LIST.format(
                projectId=project.id
            )
            fo_project_detail_url = fo_site_url + FoAppUrl.PROJECT_DETAIL.format(
                projectId=project.id
            )
            bo_project_detail_url = bo_site_url + BoAppUrl.PROJECT_DETAIL.format(
                projectId=project.id
            )

            # 「案件情報新規登録通知」メール
            # 通知メールのCCに設定する管理ユーザの全件取得
            admin_filter_condition = AdminModel.disabled == False  # NOQA
            for admin in AdminModel.scan(filter_condition=admin_filter_condition):
                if (
                    UserRoleType.SYSTEM_ADMIN.key in admin.roles
                    or UserRoleType.MAN_HOUR_OPS.key in admin.roles
                    or UserRoleType.SURVEY_OPS.key in admin.roles
                ):
                    cc_email_list.append(admin.email)
                    bo_notification_user_id_list.append(admin.id)
            # 宛先の重複を削除
            to_email_list = list(set(to_email_list))
            cc_email_list = list(set(cc_email_list))
            ProjectService.send_mail(
                MailType.PROJECT_REGISTRATION_COMPLETED,
                to_email_list,
                cc_email_list,
                {
                    "project_name": project.name,
                    "fo_project_detail_url": fo_project_detail_url,
                    "bo_project_detail_url": bo_project_detail_url,
                },
            )

            # 「案件アサイン通知」メール
            # 宛先の重複を削除
            to_assign_email_list = list(set(to_assign_email_list))
            ProjectService.send_mail(
                MailType.PROJECT_ASSIGN,
                to_assign_email_list,
                [],
                {
                    "project_name": project.name,
                    "mail_address_to": " ".join(to_assign_email_list),
                    "fo_karte_list_url": fo_karte_list_url,
                },
            )

            notification_notice_at = datetime.now()
            # お知らせ「案件情報新規登録通知」
            # FO
            # 重複を削除
            fo_notification_user_id_list = list(set(fo_notification_user_id_list))
            NotificationService.save_notification(
                notification_type=NotificationType.PROJECT_REGISTRATION,
                user_id_list=fo_notification_user_id_list,
                message_param={"project_name": project.name},
                url=fo_project_detail_url,
                noticed_at=notification_notice_at,
                create_id=current_user.id,
                update_id=current_user.id,
            )
            # BO
            # 重複を削除
            bo_notification_user_id_list = list(set(bo_notification_user_id_list))
            NotificationService.save_notification(
                notification_type=NotificationType.PROJECT_REGISTRATION,
                user_id_list=bo_notification_user_id_list,
                message_param={"project_name": project.name},
                url=bo_project_detail_url,
                noticed_at=notification_notice_at,
                create_id=current_user.id,
                update_id=current_user.id,
            )
            # お知らせ「案件アサイン通知」
            # FO
            # 重複を削除
            fo_assign_notification_user_id_list = list(
                set(fo_assign_notification_user_id_list)
            )
            NotificationService.save_notification(
                notification_type=NotificationType.PROJECT_ASSIGN,
                user_id_list=fo_assign_notification_user_id_list,
                message_param={"project_name": project.name},
                url=fo_karte_list_url,
                noticed_at=notification_notice_at,
                create_id=current_user.id,
                update_id=current_user.id,
            )

        # modelとschemasで異なる型が定義された部分のレスポンス編集
        project.profit = distributed_profit
        project.salesforce_main_customer = item.salesforce_main_customer

        # 匿名回答アンケートのパスワードは暗号化前の文字列をレスポンス
        project.survey_password = item.survey_password

        return CreateProjectResponse(
            service_type_name=service_type_name,
            main_sales_user_name=main_sales_user_name,
            main_customer_user_name=main_customer_user_name,
            main_supporter_user_name=main_supporter_user_name,
            supporter_organization_name=supporter_organization_name,
            customer_users=res_customer_users,
            supporter_users=res_supporter_users,
            **project.attribute_values,
        )

    @staticmethod
    def suggest_projects(
        customer_id: str, sort: SuggestProjectsSortType, current_user: AdminModel
    ) -> List[SuggestProjectsResponse]:
        """案件のサジェスト用データを取得します

        Args:
            customer_id (str): 取引先ID
            sort (SuggestProjectsSortType): ソート （'name:asc'）
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            List[SuggestProjectsResponse]: 取得結果
        """
        if sort == SuggestProjectsSortType.NAME_ASC:
            # name昇順で取得
            if customer_id:
                # 取引先ID指定あり
                project_iter = ProjectModel.customer_id_name_index.query(
                    hash_key=customer_id, scan_index_forward=True
                )
            else:
                # 取引先ID指定なし
                project_iter = ProjectModel.data_type_name_index.query(
                    hash_key=DataType.PROJECT, scan_index_forward=True
                )
            suggest_list: List[SuggestProjectsResponse] = []
            for project in project_iter:
                # 権限チェック
                # アクセス可の案件情報のみリストに追加
                if ProjectService.is_visible_project(current_user, project):
                    # 指定されたサービス区分を汎用マスタで存在チェック、サービス区分の名称を取得
                    service_type_name = ProjectService.cached_get_service_name(
                        service_type=project.service_type, none=True
                    )
                    display_name = f"{project.name}：{service_type_name}：{project.support_date_from} ～ {project.support_date_to}：{project.customer_name}"
                    suggest_list.append(
                        SuggestProjectsResponse(
                            display_name=display_name, **project.attribute_values
                        )
                    )
            return suggest_list
        else:
            # 呼出し元でsort項目チェック済の為、以下は発生しない想定
            raise HTTPException(status_code=api_status.HTTP_500_INTERNAL_SERVER_ERROR)

    @staticmethod
    def get_monthly_projects(
        year: int, month: int, current_user: AdminModel
    ) -> GetMonthlyProjectsReponse:
        """対象年、対象月を指定して月次案件情報を取得する

        Args:
            year (int): 対象年
            month (int): 対象月
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetMonthlyProjectsReponse: 取得結果
        """
        # 支援工数テーブル取得
        year_month: str = str(year) + "/" + str(month).zfill(2)
        range_key_condition = None
        range_key_condition &= ManHourProjectSummaryModel.data_type.startswith(
            DataType.PROJECT_SUMMARY
        )
        man_hour_list: List[ManHourProjectSummaryModel] = list(
            ManHourProjectSummaryModel.year_month_data_type_index.query(
                hash_key=year_month, range_key_condition=range_key_condition
            )
        )

        # 権限チェック
        visible_man_hour_list: List[ManHourProjectSummaryModel] = []
        for man_hour in man_hour_list:
            if ProjectService.is_visible_man_hour_by_supporter_mgr(
                current_user=current_user, man_hour=man_hour
            ):
                visible_man_hour_list.append(man_hour)

        # その他の情報取得
        project_list: List[ProjectModel] = list(ProjectModel.scan())
        user_list: List[UserModel] = list(UserModel.scan())
        master_service_list: List[MasterSupporterOrganizationModel] = list(
            MasterSupporterOrganizationModel.data_type_value_index.query(
                hash_key=MasterDataType.MASTER_SERVICE_TYPE.value
            )
        )

        # flake8エラーのため、boolを変数として定義
        bool_true: bool = True
        filter_condition_master_supporter_org = (
            MasterSupporterOrganizationModel.use == bool_true
        )
        # ソート順：並び順(order)の昇順, 抽出条件：利用フラグ有効(use=True)
        master_supporter_org_list: List[MasterSupporterOrganizationModel] = list(
            MasterSupporterOrganizationModel.data_type_order_index.query(
                hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                filter_condition=filter_condition_master_supporter_org,
            )
        )

        # 対象データから所属課のidを抽出
        belonging_organization_id_set: Set = set([])
        for visible_man_hour in visible_man_hour_list:
            if visible_man_hour.main_supporter_user:
                belonging_organization_id_set.add(
                    visible_man_hour.main_supporter_user.organization_id
                )
            if visible_man_hour.supporter_users:
                for supporter in visible_man_hour.supporter_users:
                    belonging_organization_id_set.add(supporter.organization_id)

        # レスポンス編集
        # ヘッダー情報の編集
        header_response: List[HeaderInfoForGetMonthlyProjects] = []
        for supporter_org in master_supporter_org_list:
            if supporter_org.id in belonging_organization_id_set:
                header_response.append(
                    HeaderInfoForGetMonthlyProjects(
                        supporter_organization_id=supporter_org.id,
                        supporter_organization_name=supporter_org.value,
                    )
                )

        # 詳細情報の編集
        details_response: List[DetailInfoForGetMonthlyProjects] = []
        for man_hour in visible_man_hour_list:
            name = man_hour.customer_name + "／" + man_hour.project_name

            temp_main_sales_user_id = ""
            main_sales_user_name = ""
            service_type_name = ""
            organizations = []
            for project in project_list:
                if project.id == man_hour.project_id:
                    temp_main_sales_user_id = project.main_sales_user_id
            for user in user_list:
                if user.id == temp_main_sales_user_id:
                    main_sales_user_name = user.name
            for master_service in master_service_list:
                if master_service.id == man_hour.service_type:
                    service_type_name = master_service.name

            # Organizationsの編集
            org_id_member_map: dict[str, List[ProjectMember]] = {}
            if man_hour.main_supporter_user or man_hour.supporter_users:
                # 支援者組織毎に振分け
                if man_hour.main_supporter_user:
                    temp_org_id = man_hour.main_supporter_user.organization_id
                    org_id_member_map[temp_org_id] = [
                        ProjectMember(
                            supporter_name=man_hour.main_supporter_user.name,
                            role=ProjectMemberRole.MAIN,
                            is_confirm=man_hour.main_supporter_user.is_confirm,
                        )
                    ]
                if man_hour.supporter_users:
                    for supporter in man_hour.supporter_users:
                        temp_org_id = supporter.organization_id
                        temp_member_list: List[ProjectMember] = []
                        if temp_org_id in org_id_member_map.keys():
                            temp_member_list = org_id_member_map[temp_org_id]

                        temp_member_list.append(
                            ProjectMember(
                                supporter_name=supporter.name,
                                role=ProjectMemberRole.SUB,
                                is_confirm=supporter.is_confirm,
                            )
                        )
                        org_id_member_map[temp_org_id] = temp_member_list

            for header in header_response:
                organizations.append(
                    Organization(
                        supporter_organization_id=header.supporter_organization_id,
                        supporter_organization_name=header.supporter_organization_name,
                        members=org_id_member_map.get(
                            header.supporter_organization_id, []
                        ),
                    )
                )

            temp_detail_response = DetailInfoForGetMonthlyProjects(
                name=name,
                main_sales_user_name=main_sales_user_name,
                service_type_name=service_type_name,
                organizations=organizations,
                **man_hour.attribute_values,
            )
            details_response.append(temp_detail_response)

        # ソート
        # 分類の文字コード順（昇順）
        # 案件名の文字コード順（昇順）
        # 有償無償の文字コード順（昇順）
        details_response.sort(
            key=lambda x: (x.service_type_name, x.name, x.contract_type)
        )

        return GetMonthlyProjectsReponse(
            header=header_response, details=details_response
        )

    @staticmethod
    def get_project_by_id(
        project_id: str, current_user: AdminModel
    ) -> GetProjectByIdResponse:
        """案件IDを指定して案件情報を取得する

        Args:
            project_id (str): 案件ID
            current_user (Behavior, optional): 認証済みのユーザー
        Raises:
            HTTPException: 404 Not found

        Returns:
            GetProjectByIdResponse: 取得結果
        """
        try:
            project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)
        except DoesNotExist:
            logger.warning(f"GetProjectById not found. project_id: {project_id}")
            raise HTTPException(
                status_code=api_status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 指定されたサービス区分を汎用マスタで存在チェック、サービス区分の名称を取得
        service_type_name = ProjectService.get_service_name(project.service_type)
        # 支援者組織（課）IDを汎用マスタで存在チェック、支援者組織（課）名を取得
        supporter_organization_name = ProjectService.get_supporter_organization_name(
            project.supporter_organization_id
        )

        # 一般ユーザの取得
        target_user_id_list = []
        if project.main_sales_user_id:
            target_user_id_list.append(project.main_sales_user_id)
        if project.main_customer_user_id:
            target_user_id_list.append(project.main_customer_user_id)
        if project.main_supporter_user_id:
            target_user_id_list.append(project.main_supporter_user_id)
        if project.customer_user_ids:
            for customer_user_id in project.customer_user_ids:
                target_user_id_list.append(customer_user_id)
        if project.supporter_user_ids:
            for supporter_user_id in project.supporter_user_ids:
                target_user_id_list.append(supporter_user_id)
        user_table_info: List[UserModel] = ProjectService.get_user_info(
            target_user_id_list
        )

        # 営業担当者IDの存在チェック。営業担当者名を取得。
        main_sales_user_name = ProjectService.get_user_name(
            project.main_sales_user_id, user_table_info, "mainSalesUserId"
        )
        # 担当顧客（取引先責任者）IDの存在チェック。担当顧客名を取得。
        main_customer_user_name = ProjectService.get_user_name(
            project.main_customer_user_id, user_table_info, "mainCustomerUserId"
        )
        # メイン支援者（主担当）IDの存在チェック。メイン支援者名を取得。
        main_supporter_user_name = ProjectService.get_user_name(
            project.main_supporter_user_id, user_table_info, "mainSupporterUserId"
        )
        # 顧客メンバーID、支援メンバー（副担当）IDの存在チェック。名称を取得しレスポンス編集。
        res_customer_users: List[UserIdName] = None
        if project.customer_user_ids:
            res_customer_users = ProjectService.get_user_id_name_class_list(
                [UserId(id=x) for x in project.customer_user_ids],
                user_table_info,
                "customerUsers",
            )
        res_supporter_users: List[UserIdName] = None
        if project.supporter_user_ids:
            res_supporter_users = ProjectService.get_user_id_name_class_list(
                [UserId(id=x) for x in project.supporter_user_ids],
                user_table_info,
                "supporterUsers",
            )
        # 更新ユーザ名
        update_user_name = get_update_user_name(project.update_id)

        # modelとschemasで異なる型が定義された部分のレスポンス編集
        project.profit = Profit(
            monthly=project.profit.monthly,
            quarterly=project.profit.quarterly,
            half=project.profit.half,
            year=project.profit.year,
        )
        if project.salesforce_main_customer:
            project.salesforce_main_customer = SalesforceMainCustomer(
                name=project.salesforce_main_customer.name,
                email=project.salesforce_main_customer.email,
                organization_name=project.salesforce_main_customer.organization_name,
            )
        else:
            project.salesforce_main_customer = SalesforceMainCustomer(
                name=None,
                email=None,
                organization_name=None,
            )
        if project.salesforce_update_at:
            project.salesforce_update_at = project.salesforce_update_at.strftime(
                "%Y/%m/%d %H:%M"
            )
        # schemasの定義が必須のため、Noneを空文字に変換
        if project.dedicated_survey_user_name is None:
            project.dedicated_survey_user_name = ""
        if project.dedicated_survey_user_email is None:
            project.dedicated_survey_user_email = ""
        if project.survey_password is None:
            project.survey_password = ""

        # survey_passwordの復号化
        if project.survey_password:
            project.survey_password = AesCipherUtils.decrypt(project.survey_password)

        return GetProjectByIdResponse(
            service_type_name=service_type_name,
            main_sales_user_name=main_sales_user_name,
            main_customer_user_name=main_customer_user_name,
            main_supporter_user_name=main_supporter_user_name,
            supporter_organization_name=supporter_organization_name,
            customer_users=res_customer_users,
            supporter_users=res_supporter_users,
            update_user_name=update_user_name,
            **project.attribute_values,
        )

    @staticmethod
    def get_projects(
        name: str,
        status: SupportStatusType,
        from_year_month: int,
        to_year_month: int,
        customer_id: str,
        main_sales_user_id: str,
        supporter_organization_id_list: List[str],
        service_type_id: str,
        is_karte_usage_project_list_of_sales: bool,
        sort: GetProjectsSortType,
        offset_page: int,
        limit: int,
        current_user: AdminModel,
    ) -> GetProjectsResponse:
        """案件を検索し、案件一覧を取得する

        Args:
            name (str): 案件名
            status (SupportStatusType): ステータス（支援前、支援中、支援後）
            from_year_month (int): 対象年月（From）
            to_year_month (int): 対象年月（To）
            customer_id (str): 取引先ID
            main_sales_user_id (str): 営業担当者ID
            supporter_organization_id_list (List[str]): 支援者組織ID
            service_type_id (str): サービス種別ID
            is_karte_usage_project_list_of_sales (bool): 営業担当者のカルテ利用案件一覧
            sort (GetProjectsSortType): ソート
            offset_page (int): リストの中で何ページ目を取得するか
            limit (int): 最大取得件数
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetProjectsResponse: 取得結果
        """

        # 抽出条件
        range_key_condition = None
        filter_condition = None
        range_key_condition_gsi_support_date_to = None
        filter_condition_gsi_support_date_to = None
        # 抽出条件: 案件名
        if name:
            range_key_condition &= ProjectModel.name == name
            filter_condition_gsi_support_date_to &= ProjectModel.name == name

        # 抽出条件: ステータス
        str_today = get_datetime_now().strftime("%Y/%m/%d")
        if status == SupportStatusType.BEFORE:
            filter_condition &= ProjectModel.support_date_from > str_today
            filter_condition_gsi_support_date_to &= (
                ProjectModel.support_date_from > str_today
            )
        elif status == SupportStatusType.DURING:
            filter_condition &= (ProjectModel.support_date_from <= str_today) & (
                ProjectModel.support_date_to >= str_today
            )
            filter_condition_gsi_support_date_to &= (
                ProjectModel.support_date_from <= str_today
            )
            # GSI(data_type_support_date_to_index)のrange_key_conditionは複数条件設定不可の為、
            # 後続で他項目の条件と含めてまとめて設定する
        elif status == SupportStatusType.AFTER:
            filter_condition &= ProjectModel.support_date_to < str_today
            # GSI(data_type_support_date_to_index)のrange_key_conditionは複数条件設定不可の為、
            # 後続で他項目の条件と含めてまとめて設定する

        # 抽出条件: 対象年月(from),対象年月(to)
        # 対象年月のfromは01日、toに31日を補完
        str_from_year_month = (
            datetime.strptime(str(from_year_month), "%Y%m").strftime("%Y/%m/01")
            if from_year_month
            else None
        )
        str_to_year_month = (
            datetime.strptime(str(to_year_month), "%Y%m").strftime("%Y/%m/31")
            if to_year_month
            else None
        )
        if from_year_month:
            # 対象年月from以降に支援終了日がある案件情報が対象
            filter_condition &= ProjectModel.support_date_to >= str_from_year_month
            # GSI(data_type_support_date_to_index)のrange_key_conditionは複数条件設定不可の為、
            # 後続で他項目の条件と含めてまとめて設定する
        if to_year_month:
            # 対象年月to以前に支援開始日がある案件情報が対象
            filter_condition &= ProjectModel.support_date_from <= str_to_year_month
            filter_condition_gsi_support_date_to &= (
                ProjectModel.support_date_from <= str_to_year_month
            )

        # 抽出条件: 営業担当者ID
        if main_sales_user_id:
            filter_condition &= ProjectModel.main_sales_user_id == main_sales_user_id
            filter_condition_gsi_support_date_to &= (
                ProjectModel.main_sales_user_id == main_sales_user_id
            )
        # 抽出条件: 支援者組織ID
        if supporter_organization_id_list:
            filter_condition &= ProjectModel.supporter_organization_id.is_in(
                *supporter_organization_id_list
            )
            filter_condition_gsi_support_date_to &= (
                ProjectModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
            )
        # 抽出条件: サービス種別ID
        if service_type_id:
            filter_condition &= ProjectModel.service_type == service_type_id
            filter_condition_gsi_support_date_to &= (
                ProjectModel.service_type == service_type_id
            )

        # GSI(data_type_support_date_to_index)のrange_key_condition設定
        # 複数条件設定不可の為、複数項目の条件をまとめて設定
        # 抽出条件: ステータス、対象年月(from)
        if status == SupportStatusType.DURING:
            if from_year_month:
                temp_today_datetime = datetime.strptime(str_today, "%Y/%m/%d")
                temp_from_year_month_datetime = datetime.strptime(
                    str_from_year_month, "%Y/%m/%d"
                )
                if temp_today_datetime >= temp_from_year_month_datetime:
                    range_key_condition_gsi_support_date_to &= (
                        ProjectModel.support_date_to >= str_today
                    )
                else:
                    range_key_condition_gsi_support_date_to &= (
                        ProjectModel.support_date_to >= str_from_year_month
                    )
            else:
                range_key_condition_gsi_support_date_to &= (
                    ProjectModel.support_date_to >= str_today
                )
        elif status == SupportStatusType.AFTER:
            if from_year_month:
                # betweenは両端を含むため、終端は1日前
                str_today_minus_one_day = (
                    get_datetime_now() + timedelta(days=-1)
                ).strftime("%Y/%m/%d")
                range_key_condition_gsi_support_date_to &= (
                    ProjectModel.support_date_to.between(
                        str_from_year_month, str_today_minus_one_day
                    )
                )
            else:
                range_key_condition_gsi_support_date_to &= (
                    ProjectModel.support_date_to < str_today
                )

        # 案件情報取得
        # クエリパラメータ有無によりGSIを使い分ける
        if range_key_condition_gsi_support_date_to is not None:
            if customer_id:
                # 取引先IDの指定あり
                filter_condition_gsi_support_date_to &= (
                    ProjectModel.customer_id == customer_id
                )
            project_list = list(
                ProjectModel.data_type_support_date_to_index.query(
                    hash_key=DataType.PROJECT,
                    range_key_condition=range_key_condition_gsi_support_date_to,
                    filter_condition=filter_condition_gsi_support_date_to,
                )
            )

        else:
            if customer_id:
                # 取引先IDの指定あり
                project_list = list(
                    ProjectModel.customer_id_name_index.query(
                        hash_key=customer_id,
                        range_key_condition=range_key_condition,
                        filter_condition=filter_condition,
                    )
                )
            else:
                # 取引先IDの指定なし
                project_list = list(
                    ProjectModel.data_type_name_index.query(
                        hash_key=DataType.PROJECT,
                        range_key_condition=range_key_condition,
                        filter_condition=filter_condition,
                    )
                )

        # 汎用マスタ(サービス種別)を取得
        master_service_list: List[MasterSupporterOrganizationModel] = list(
            MasterSupporterOrganizationModel.data_type_value_index.query(
                hash_key=MasterDataType.MASTER_SERVICE_TYPE.value
            )
        )
        # 汎用マスタ(支援者組織情報)を取得
        master_supporter_org_list: List[MasterSupporterOrganizationModel] = list(
            MasterSupporterOrganizationModel.data_type_value_index.query(
                hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value
            )
        )
        # 一般ユーザ情報を取得
        user_list: List[UserModel] = list(
            UserModel.data_type_name_index.query(hash_key=DataType.USER)
        )

        project_info_list: List[ProjectInfoForGetProjects] = []
        for project in project_list:
            # 営業担当者のカルテ利用案件一覧の場合、追加の権限チェックを行う
            if is_karte_usage_project_list_of_sales:
                if not ProjectService.is_visible_karte_by_sales(
                    current_user=current_user, project=project, user_list=user_list
                ):
                    continue

            # サービス区分の名称
            service_type_name = ""
            for master_service in master_service_list:
                if master_service.id == project.service_type:
                    service_type_name = master_service.name
                    break
            # 支援者組織（課）名
            supporter_organization_name = ""
            for master_supporter_org in master_supporter_org_list:
                if master_supporter_org.id == project.supporter_organization_id:
                    supporter_organization_name = master_supporter_org.value
                    break
            # 営業担当者名、担当顧客名、メイン支援者名
            main_sales_user_name = ""
            main_customer_user_name = ""
            main_supporter_user_name = ""
            for user in user_list:
                if user.id == project.main_sales_user_id:
                    main_sales_user_name = user.name
                if user.id == project.main_customer_user_id:
                    main_customer_user_name = user.name
                if user.id == project.main_supporter_user_id:
                    main_supporter_user_name = user.name
                if (
                    main_sales_user_name
                    and main_customer_user_name
                    and main_supporter_user_name
                ):
                    break
            # 顧客メンバー名
            res_customer_users = []
            if project.customer_user_ids:
                for customer_user_id in project.customer_user_ids:
                    name = ""
                    for user in user_list:
                        if user.id == customer_user_id:
                            name = user.name
                            break
                    res_customer_users.append(
                        UserIdName(id=customer_user_id, name=name)
                    )
            # 支援メンバー（副担当）名
            res_supporter_users = []
            if project.supporter_user_ids:
                for supporter_user_id in project.supporter_user_ids:
                    name = ""
                    for user in user_list:
                        if user.id == supporter_user_id:
                            name = user.name
                            break
                    res_supporter_users.append(
                        UserIdName(id=supporter_user_id, name=name)
                    )

            # modelとschemasで異なる型が定義された部分のレスポンス編集
            if project.salesforce_main_customer:
                project.salesforce_main_customer = SalesforceMainCustomer(
                    name=project.salesforce_main_customer.name,
                    email=project.salesforce_main_customer.email,
                    organization_name=project.salesforce_main_customer.organization_name,
                )
            else:
                project.salesforce_main_customer = SalesforceMainCustomer(
                    name=None,
                    email=None,
                    organization_name=None,
                )
            if project.salesforce_update_at:
                project.salesforce_update_at = project.salesforce_update_at.strftime(
                    "%Y/%m/%d %H:%M"
                )

            project_info_list.append(
                ProjectInfoForGetProjects(
                    service_type_name=service_type_name,
                    main_sales_user_name=main_sales_user_name,
                    main_customer_user_name=main_customer_user_name,
                    main_supporter_user_name=main_supporter_user_name,
                    supporter_organization_name=supporter_organization_name,
                    customer_users=res_customer_users,
                    supporter_users=res_supporter_users,
                    **project.attribute_values,
                )
            )

        # ソート
        # 第一ソートキー: サービス名 昇順（文字コード順）
        # 第二ソートキー: 取引先名 昇順（文字コード順）
        # 第三ソートキー: 案件名 クエリパラメータ指定順
        if project_info_list:
            # 案件名
            if sort == GetProjectsSortType.NAME_DESC:
                project_info_list.sort(key=lambda x: x.name, reverse=True)
            else:
                project_info_list.sort(key=lambda x: x.name)
            # 取引先名
            project_info_list.sort(key=lambda x: x.customer_name)
            # サービス名
            project_info_list.sort(key=lambda x: x.service_type_name)

        # ページネーション
        try:
            p = Paginator(project_info_list, limit)
            current_page = p.page(offset_page).object_list
        except EmptyPage:
            logger.warning(f"Not found. offset_page: {offset_page}")
            raise HTTPException(status_code=api_status.HTTP_404_NOT_FOUND)

        return GetProjectsResponse(
            offset_page=offset_page,
            total=len(project_info_list),
            projects=current_page,
        )

    @staticmethod
    def update_project_by_id(
        item: UpdateProjectByIdRequest,
        version: int,
        project_id: str,
        current_user: AdminModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> OKResponse:
        """案件IDを指定して案件情報を更新する

        Args:
            item (UpdateProjectByIdRequest): 更新内容
            version (int): ロックキー（楽観ロック制御）
            project_id (str): 案件ID
            current_user (Behavior, optional): 認証済みのユーザー
            authorization (HTTPAuthorizationCredentials): 認証情報
        Raises:
            HTTPException: 404 Not found
            HTTPException: 409 Conflict

        Returns:
            OKResponse: 取得結果
        """
        try:
            project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)
        except DoesNotExist:
            logger.warning(f"UpdateProjectById not found. project_id: {project_id}")
            raise HTTPException(
                status_code=api_status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 権限チェック
        if not ProjectService.is_visible_project(current_user, project):
            logger.warning(
                f"UpdateProjectById forbidden. current_user:{current_user.id} project_id: {project_id}"
            )
            raise HTTPException(
                status_code=api_status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 排他チェック
        if version != project.version:
            logger.warning(
                f"UpdateProjectById conflict. request_ver:{version} project_ver: {project.version}"
            )
            raise HTTPException(
                status_code=api_status.HTTP_409_CONFLICT, detail="Conflict"
            )

        # 取引先IDの存在チェック、取引先名の取得
        customer_name = ProjectService.get_customer_name(item.customer_id)
        # 指定されたサービス区分を汎用マスタで存在チェック
        ProjectService.get_service_name(item.service_type)
        # 支援者組織（課）IDを汎用マスタで存在チェック
        ProjectService.get_supporter_organization_name(item.supporter_organization_id)

        # 更新前の営業担当者ID、担当顧客（取引先責任者）ID、メイン支援者（主担当）ID、
        # 顧客メンバーID、支援メンバー（副担当）IDを取得
        before_user_id_list = []
        if project.main_sales_user_id:
            before_user_id_list.append(project.main_sales_user_id)
        if project.main_customer_user_id:
            before_user_id_list.append(project.main_customer_user_id)
        if project.main_supporter_user_id:
            before_user_id_list.append(project.main_supporter_user_id)
        if project.customer_user_ids:
            for customer_user_id in project.customer_user_ids:
                before_user_id_list.append(customer_user_id)
        if project.supporter_user_ids:
            for supporter_user_id in project.supporter_user_ids:
                before_user_id_list.append(supporter_user_id)
        # 重複を削除
        before_user_id_list = list(set(before_user_id_list))

        # 一般ユーザの取得
        target_user_id_list = []
        if item.main_sales_user_id:
            target_user_id_list.append(item.main_sales_user_id)
        if item.main_customer_user_id:
            target_user_id_list.append(item.main_customer_user_id)
        if item.main_supporter_user_id:
            target_user_id_list.append(item.main_supporter_user_id)
        if item.customer_users:
            for customer_user in item.customer_users:
                target_user_id_list.append(customer_user.id)
        if item.supporter_users:
            for supporter_user in item.supporter_users:
                target_user_id_list.append(supporter_user.id)
        # 重複を削除
        target_user_id_list = list(set(target_user_id_list))
        user_table_info: List[UserModel] = ProjectService.get_user_info(
            target_user_id_list
        )

        # ユーザの有効化チェック
        ProjectService.check_valid_user(item, user_table_info)

        # 営業担当者IDの存在チェック
        ProjectService.get_user_name(
            item.main_sales_user_id, user_table_info, "mainSalesUserId"
        )
        # 担当顧客（取引先責任者）IDの存在チェック。
        ProjectService.get_user_name(
            item.main_customer_user_id, user_table_info, "mainCustomerUserId"
        )
        # メイン支援者（主担当）IDの存在チェック。
        ProjectService.get_user_name(
            item.main_supporter_user_id, user_table_info, "mainSupporterUserId"
        )
        # 顧客メンバーID、支援メンバー（副担当）IDの存在チェック
        ProjectService.get_user_id_name_class_list(
            item.customer_users, user_table_info, "customerUsers"
        )
        ProjectService.get_user_id_name_class_list(
            item.supporter_users, user_table_info, "supporterUsers"
        )

        # ユーザIDが指定された項目から更新前後で除外/追加されたユーザIDのリストを取得
        divided_user_id = ProjectService.divide_user_id(
            before_user_id_list, target_user_id_list
        )
        # 「案件アサイン通知」メールの宛先TOに入れるユーザID
        # （支援者or支援者責任者or事業者責任者or営業担当者orお客様メンバー）
        to_mail_user_id_list = [
            item.main_sales_user_id,
            item.main_supporter_user_id,
            item.main_customer_user_id,
        ]
        if item.supporter_users:
            for supporter_user in item.supporter_users:
                to_mail_user_id_list.append(supporter_user.id)
        if item.customer_users:
            for customer_user in item.customer_users:
                to_mail_user_id_list.append(customer_user.id)
        # 重複を削除
        to_mail_user_id_list = list(set(to_mail_user_id_list))

        # お知らせ（アサイン）を通知するユーザID
        fo_assign_notification_user_id_list = []

        # 粗利項目の編集
        # FY粗利を按分し、月毎、Q毎、H毎の粗利を算出
        distributed_profit = ProjectService.distribute_profit(item.profit)

        # 案件の更新
        update_datetime = datetime.now()
        update_action: List[Action] = []
        update_attributes: List[UpdateAttributesSubAttribute] = []
        if project.customer_id != item.customer_id:
            update_action.append(ProjectModel.customer_id.set(item.customer_id))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_ID.tbl_column_name,
                    value=item.customer_id,
                )
            )
        if project.name != item.name:
            update_action.append(ProjectModel.name.set(item.name))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.NAME.tbl_column_name,
                    value=item.name,
                )
            )
        if project.customer_name != customer_name:
            update_action.append(ProjectModel.customer_name.set(customer_name))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_NAME.tbl_column_name,
                    value=customer_name,
                )
            )
        if project.service_type != item.service_type:
            update_action.append(ProjectModel.service_type.set(item.service_type))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SERVICE_TYPE.tbl_column_name,
                    value=item.service_type,
                )
            )
        if project.create_new != item.create_new:
            update_action.append(ProjectModel.create_new.set(item.create_new))
            sub_attribute_create_new = None
            if item.create_new is not None:
                sub_attribute_create_new = str(item.create_new)
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CREATE_NEW.tbl_column_name,
                    value=sub_attribute_create_new,
                )
            )
        if project.main_sales_user_id != item.main_sales_user_id:
            update_action.append(
                ProjectModel.main_sales_user_id.set(item.main_sales_user_id)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.MAIN_SALES_USER_ID.tbl_column_name,
                    value=item.main_sales_user_id,
                )
            )
        if project.customer_success != item.customer_success:
            update_action.append(
                ProjectModel.customer_success.set(item.customer_success)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_SUCCESS.tbl_column_name,
                    value=item.customer_success,
                )
            )
        if project.support_date_from != item.support_date_from:
            update_action.append(
                ProjectModel.support_date_from.set(item.support_date_from)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SUPPORT_DATE_FROM.tbl_column_name,
                    value=item.support_date_from,
                )
            )
        if project.support_date_to != item.support_date_to:
            update_action.append(ProjectModel.support_date_to.set(item.support_date_to))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SUPPORT_DATE_TO.tbl_column_name,
                    value=item.support_date_to,
                )
            )

        if project.profit is None or (
            project.profit.year != float(distributed_profit.year)
        ):
            update_action.append(
                ProjectModel.profit.set(
                    GrossProfitAttribute(
                        monthly=distributed_profit.monthly,
                        quarterly=distributed_profit.quarterly,
                        half=distributed_profit.half,
                        year=distributed_profit.year,
                    )
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.PROFIT_MONTHLY.tbl_column_name,
                    value=",".join(map(str, distributed_profit.monthly)),
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.PROFIT_QUARTERLY.tbl_column_name,
                    value=",".join(map(str, distributed_profit.quarterly)),
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.PROFIT_HALF.tbl_column_name,
                    value=",".join(map(str, distributed_profit.half)),
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.PROFIT_YEAR.tbl_column_name,
                    value=str(distributed_profit.year),
                )
            )

        if project.total_contract_time != item.total_contract_time:
            update_action.append(
                ProjectModel.total_contract_time.set(item.total_contract_time)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.TOTAL_CONTRACT_TIME.tbl_column_name,
                    value=str(item.total_contract_time),
                )
            )
        if project.main_customer_user_id != item.main_customer_user_id:
            update_action.append(
                ProjectModel.main_customer_user_id.set(item.main_customer_user_id)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.MAIN_CUSTOMER_USER_ID.tbl_column_name,
                    value=item.main_customer_user_id,
                )
            )
        if item.salesforce_main_customer is not None:
            if (
                project.salesforce_main_customer is None
                or (
                    project.salesforce_main_customer.name
                    != item.salesforce_main_customer.name
                )
                or project.salesforce_main_customer.email
                != item.salesforce_main_customer.email
                or project.salesforce_main_customer.organization_name
                != item.salesforce_main_customer.organization_name
            ):
                update_action.append(
                    ProjectModel.salesforce_main_customer.set(
                        SalesforceMainCustomerAttribute(
                            name=item.salesforce_main_customer.name,
                            email=item.salesforce_main_customer.email,
                            organization_name=item.salesforce_main_customer.organization_name,
                        )
                    )
                )
                if (
                    project.salesforce_main_customer is None
                    or project.salesforce_main_customer.name
                    != item.salesforce_main_customer.name
                ):
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.tbl_column_name,
                            value=item.salesforce_main_customer.name,
                        )
                    )
                if (
                    project.salesforce_main_customer is None
                    or project.salesforce_main_customer.email
                    != item.salesforce_main_customer.email
                ):
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.tbl_column_name,
                            value=item.salesforce_main_customer.email,
                        )
                    )
                if (
                    project.salesforce_main_customer is None
                    or project.salesforce_main_customer.organization_name
                    != item.salesforce_main_customer.organization_name
                ):
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.tbl_column_name,
                            value=item.salesforce_main_customer.organization_name,
                        )
                    )

        if (
            project.customer_user_ids
            != ProjectService.convert_user_id_class_list_to_set(item.customer_users)
        ):
            edit_customer_user_ids = ProjectService.convert_user_id_class_list_to_set(
                item.customer_users
            )
            if not edit_customer_user_ids:
                edit_customer_user_ids = None
            update_action.append(
                ProjectModel.customer_user_ids.set(edit_customer_user_ids)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_USER_IDS.tbl_column_name,
                    value=",".join(
                        ProjectService.convert_user_id_class_list_to_set(
                            item.customer_users
                        )
                    ),
                )
            )

        if project.main_supporter_user_id != item.main_supporter_user_id:
            update_action.append(
                ProjectModel.main_supporter_user_id.set(item.main_supporter_user_id)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.MAIN_SUPPORTER_USER_ID.tbl_column_name,
                    value=item.main_supporter_user_id,
                )
            )
        if project.supporter_organization_id != item.supporter_organization_id:
            update_action.append(
                ProjectModel.supporter_organization_id.set(
                    item.supporter_organization_id
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SUPPORTER_ORGANIZATION_ID.tbl_column_name,
                    value=item.supporter_organization_id,
                )
            )
        if (
            project.supporter_user_ids
            != ProjectService.convert_user_id_class_list_to_set(item.supporter_users)
        ):
            edit_supporter_user_ids = ProjectService.convert_user_id_class_list_to_set(
                item.supporter_users
            )
            if not edit_supporter_user_ids:
                edit_supporter_user_ids = None
            update_action.append(
                ProjectModel.supporter_user_ids.set(edit_supporter_user_ids)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SUPPORTER_USER_IDS.tbl_column_name,
                    value=",".join(
                        ProjectService.convert_user_id_class_list_to_set(
                            item.supporter_users
                        )
                    ),
                )
            )
        if project.is_count_man_hour != item.is_count_man_hour:
            update_action.append(
                ProjectModel.is_count_man_hour.set(item.is_count_man_hour)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.IS_COUNT_MAN_HOUR.tbl_column_name,
                    value=str(item.is_count_man_hour),
                )
            )
        if project.is_karte_remind != item.is_karte_remind:
            update_action.append(ProjectModel.is_karte_remind.set(item.is_karte_remind))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.IS_KARTE_REMIND.tbl_column_name,
                    value=str(item.is_karte_remind),
                )
            )
        if project.is_master_karte_remind != item.is_master_karte_remind:
            update_action.append(
                ProjectModel.is_master_karte_remind.set(item.is_master_karte_remind)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.IS_MASTER_KARTE_REMIND.tbl_column_name,
                    value=str(item.is_master_karte_remind),
                )
            )

        if project.contract_type != item.contract_type:
            update_action.append(ProjectModel.contract_type.set(item.contract_type))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CONTRACT_TYPE.tbl_column_name,
                    value=item.contract_type,
                )
            )
        before_changed_is_secret = copy.deepcopy(project.is_secret)
        if project.is_secret != item.is_secret:
            update_action.append(ProjectModel.is_secret.set(item.is_secret))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.IS_SECRET.tbl_column_name,
                    value=str(item.is_secret),
                )
            )
        if project.is_solver_project != item.is_solver_project:
            update_action.append(ProjectModel.is_solver_project.set(item.is_solver_project))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.IS_SOLVER_PROJECT.tbl_column_name,
                    value=str(item.is_solver_project),
                )
            )
        if project.dedicated_survey_user_name != item.dedicated_survey_user_name:
            update_action.append(
                ProjectModel.dedicated_survey_user_name.set(
                    item.dedicated_survey_user_name
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.DEDICATED_SURVEY_USER_NAME.tbl_column_name,
                    value=item.dedicated_survey_user_name,
                )
            )
        if project.dedicated_survey_user_email != item.dedicated_survey_user_email:
            update_action.append(
                ProjectModel.dedicated_survey_user_email.set(
                    item.dedicated_survey_user_email
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.tbl_column_name,
                    value=item.dedicated_survey_user_email,
                )
            )
        decrypted_survey_passowrd = (
            AesCipherUtils.decrypt(project.survey_password)
            if project.survey_password
            else project.survey_password
        )
        if decrypted_survey_passowrd != item.survey_password:
            encrypted_str = (
                AesCipherUtils.encrypt(item.survey_password)
                if item.survey_password
                else ""
            )
            update_action.append(ProjectModel.survey_password.set(encrypted_str))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SURVEY_PASSWORD.tbl_column_name,
                    value=encrypted_str,
                )
            )
        if (
            project.is_survey_email_to_salesforce_main_customer
            != item.is_survey_email_to_salesforce_main_customer
        ):
            update_action.append(
                ProjectModel.is_survey_email_to_salesforce_main_customer.set(
                    item.is_survey_email_to_salesforce_main_customer
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.IS_SURVEY_EMAIL_TO_SALESFORCE_MAIN_CUSTOMER.tbl_column_name,
                    value=str(item.is_survey_email_to_salesforce_main_customer),
                )
            )
        update_action.append(
            ProjectModel.update_history.set(
                UpdateHistoryAttribute(
                    update_id=current_user.id,
                    update_attributes=update_attributes,
                )
            )
        )
        update_action.append(ProjectModel.update_id.set(current_user.id))
        update_action.append(ProjectModel.update_at.set(update_datetime))
        project.update(actions=update_action)

        # pf案件情報の更新
        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        if project.salesforce_opportunity_id:
            # SF案件の場合
            # 案件のステータス（公開・非公開）が変更時、PFに"publication"を連携
            if before_changed_is_secret != item.is_secret:
                put_project_publication_pf_project_params = {
                    "salesforceOpportunityIds": [project.salesforce_opportunity_id],
                    "publication": not item.is_secret,
                }

                status_code, body = platform_api_operator.put_project_publication(
                    params=put_project_publication_pf_project_params
                )
                logger.info(
                    f"platform UpdateProjectPublication API statusCode: {status_code}"
                )
                if status_code != 200:
                    logger.info(
                        "platform putProjectPublication response:" + json.dumps(body)
                    )
                    raise HTTPException(
                        status_code=status_code,
                        detail=json.dumps(body),
                    )
        else:
            try:
                customer = CustomerModel.get(
                    hash_key=item.customer_id, range_key=DataType.CUSTOMER
                )
            except DoesNotExist:
                raise HTTPException(
                    status_code=api_status.HTTP_500_INTERNAL_SERVER_ERROR,
                    detail={"message": "Not exist customer."},
                )

            pf_get_projects_params = {}
            pf_get_projects_params["partnerPortalProjectId"] = project_id
            (
                pf_get_projects_status_code,
                pf_get_projects_response,
            ) = platform_api_operator.get_projects(params=pf_get_projects_params)

            if pf_get_projects_status_code != 200:
                logger.info(
                    "platform GetProjects API response:"
                    + json.dumps(pf_get_projects_response)
                )
                raise HTTPException(
                    status_code=status_code,
                    detail=json.dumps(pf_get_projects_response),
                )
            if len(pf_get_projects_response["projects"]) > 0:
                # 案件の更新
                put_pf_project_params = {
                    "partnerPortalCustomerId": item.customer_id
                    if customer and not customer.salesforce_customer_id
                    else None,
                    "partnerPortalProjectId": project.id,
                    "salesforceCustomerId": customer.salesforce_customer_id
                    if customer and customer.salesforce_customer_id
                    else None,
                    "partnerPortalProjectName": item.name,
                    "projectName": item.name,
                    "customerName": ProjectService.get_customer_name(item.customer_id),
                    "serviceType": ProjectService.get_service_name(item.service_type),
                    "profit": ProjectService.get_supporter_organization_name(
                        item.supporter_organization_id
                    ),
                    "partnerPortalProfit": item.profit.year if item.profit else None,
                    "customerSuccess": item.customer_success,
                    "directSupportManHour": item.total_contract_time,
                    "supportDateFrom": item.support_date_from,
                    "supportDateTo": item.support_date_to,
                    "isPublishable": not item.is_secret,
                    "version": pf_get_projects_response["projects"][0]["project"][
                        "version"
                    ],
                }

                status_code, body = platform_api_operator.update_project_by_pf_id(
                    project_id=pf_get_projects_response["projects"][0]["project"]["id"],
                    params=put_pf_project_params,
                )
                logger.info(f"platform updateProjectByPfId statusCode: {status_code}")
                if status_code != 200:
                    logger.info(
                        "platform updateProjectByPfId response:" + json.dumps(body)
                    )
                    raise HTTPException(
                        status_code=status_code,
                        detail=json.dumps(body),
                    )

        # 一般ユーザの更新
        update_datetime = datetime.now()
        if user_table_info:
            to_email_list = []
            with UserModel.batch_write() as user_batch:
                # 更新対象の情報が存在する場合、他処理の更新を考慮し最新情報を取得
                user_table_info = ProjectService.get_user_info(
                    target_user_id_list + before_user_id_list
                )
                for user in user_table_info:
                    if user.id in divided_user_id["add"]:
                        # 案件情報に新規追加されたユーザ
                        if user.id in to_mail_user_id_list:
                            to_email_list.append(user.email)
                            fo_assign_notification_user_id_list.append(user.id)
                        if user.project_ids is not None:
                            user.project_ids.add(project.id)
                        else:
                            user.project_ids = set([project.id])
                        user.update_id = current_user.id
                        user.update_at = update_datetime
                        user.version += 1
                        user_batch.save(user)
                    elif user.id in divided_user_id["exclude"]:
                        # 案件情報から除外されたユーザ
                        if user.project_ids is not None:
                            user.project_ids.discard(project.id)
                            user.update_id = current_user.id
                            user.update_at = update_datetime
                            user.version += 1
                            user_batch.save(user)

            fo_site_url = get_app_settings().fo_site_url
            fo_karte_list_url = fo_site_url + FoAppUrl.KARTE_LIST.format(
                projectId=project.id
            )

            # メール通知：アサイン
            if to_email_list:
                # 宛先の重複を削除
                to_email_list = list(set(to_email_list))
                ProjectService.send_mail(
                    MailType.PROJECT_ASSIGN,
                    to_email_list,
                    [],
                    {
                        "project_name": project.name,
                        "mail_address_to": " ".join(to_email_list),
                        "fo_karte_list_url": fo_karte_list_url,
                    },
                )

            # お知らせ：アサイン
            notification_notice_at = datetime.now()
            # お知らせ「案件アサイン通知」
            # FO
            # 重複を削除
            fo_assign_notification_user_id_list = list(
                set(fo_assign_notification_user_id_list)
            )
            NotificationService.save_notification(
                notification_type=NotificationType.PROJECT_ASSIGN,
                user_id_list=fo_assign_notification_user_id_list,
                message_param={"project_name": project.name},
                url=fo_karte_list_url,
                noticed_at=notification_notice_at,
                create_id=current_user.id,
                update_id=current_user.id,
            )

        return OKResponse()

    @staticmethod
    def delete_project_by_id(
        version: int,
        project_id: str,
        current_user: AdminModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> OKResponse:
        """案件IDを指定して案件情報を削除する。
            紐づく案件スケジュールと案件カルテも削除する.

        Args:
            version (int): ロックキー（楽観ロック制御）
            project_id (str): 案件ID
            current_user (Behavior, optional): 認証済みのユーザー
            authorization (HTTPAuthorizationCredentials): 認証情報
        Raises:
            HTTPException: 404 Not found
            HTTPException: 409 Conflict

        Returns:
            OKResponse: 取得結果
        """
        try:
            project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)
        except DoesNotExist:
            logger.warning(f"DeleteProjectById not found. project_id: {project_id}")
            raise HTTPException(
                status_code=api_status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 排他チェック
        if version != project.version:
            logger.warning(
                f"DeleteProjectById conflict. request_ver:{version} project_ver: {project.version}"
            )
            raise HTTPException(
                status_code=api_status.HTTP_409_CONFLICT, detail="Conflict"
            )

        # 削除可否チェック
        # 案件に紐づくアンケートが一通でも送信されていれば、削除不可
        project_survey_list: List[ProjectSurveyModel] = list(
            ProjectSurveyModel.project_id_summary_month_index.query(hash_key=project.id)
        )
        for survey in project_survey_list:
            if survey.actual_survey_request_datetime:
                # アンケート送信済み
                logger.warning(
                    f"DeleteProjectById cannot delete the project because surveys has already been sent. project_id: {project_id}"
                )
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="The project cannot be deleted because surveys has already been sent.",
                )
        # 工数入力されている案件（一時保存含む）は削除不可
        if project.is_man_hour_input:
            # 工数入力済み
            logger.warning(
                f"DeleteProjectById cannot delete the project because man-hours have already been entered. project_id: {project_id}"
            )
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail="The project cannot be deleted because man-hours have already been entered.",
            )

        # 案件にアサインされたユーザの情報を取得
        target_user_id_list = []
        if project.main_sales_user_id:
            target_user_id_list.append(project.main_sales_user_id)
        if project.main_customer_user_id:
            target_user_id_list.append(project.main_customer_user_id)
        if project.main_supporter_user_id:
            target_user_id_list.append(project.main_supporter_user_id)
        if project.customer_user_ids:
            for customer_user_id in project.customer_user_ids:
                target_user_id_list.append(customer_user_id)
        if project.supporter_user_ids:
            for supporter_user_id in project.supporter_user_ids:
                target_user_id_list.append(supporter_user_id)
        # 重複を削除
        target_user_id_list = list(set(target_user_id_list))
        user_table_info: List[UserModel] = ProjectService.get_user_info(
            target_user_id_list
        )

        # 案件テーブルから削除
        project.delete()

        # 紐づく案件アンケートを削除
        with ProjectSurveyModel.batch_write() as project_survey_batch:
            for survey in project_survey_list:
                project_survey_batch.delete(survey)
        # 紐づく案件カルテを削除
        with ProjectKarteModel.batch_write() as project_karte_batch:
            project_karte_iter = (
                ProjectKarteModel.project_id_start_datetime_index.query(
                    hash_key=project_id
                )
            )
            for project_karte in project_karte_iter:
                project_karte_batch.delete(project_karte)
        # 一般ユーザの更新
        # ユーザのproject_idsから削除案件のIDを削除
        update_datetime = datetime.now()
        if user_table_info:
            with UserModel.batch_write() as user_batch:
                for user in user_table_info:
                    if user.project_ids is not None:
                        user.project_ids.discard(project.id)
                        user.update_id = current_user.id
                        user.update_at = update_datetime
                        user.version += 1
                        user_batch.save(user)

        # PF案件情報を削除
        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)

        pf_get_projects_params = {}
        if project.salesforce_opportunity_id:
            pf_get_projects_params[
                "salesforceOpportunityId"
            ] = project.salesforce_opportunity_id
        else:
            pf_get_projects_params["partnerPortalProjectId"] = project.id
        (
            pf_get_projects_status_code,
            pf_get_projects_response,
        ) = platform_api_operator.get_projects(params=pf_get_projects_params)

        if pf_get_projects_status_code != 200:
            logger.info(
                "platform GetProjects API response:"
                + json.dumps(pf_get_projects_response)
            )
            raise HTTPException(
                status_code=pf_get_projects_status_code,
                detail=json.dumps(pf_get_projects_response),
            )

        # SF案件の場合
        if project.salesforce_opportunity_id:
            put_project_publication_pf_project_params = {
                "salesforceOpportunityIds": [project.salesforce_opportunity_id],
                "importCsv": False,
            }
            status_code, put_project_publication_response = platform_api_operator.put_project_publication(
                params=put_project_publication_pf_project_params
            )
            logger.info(
                f"platform putProjectPublication API statusCode: {status_code}"
            )
            if status_code != 200:
                logger.info(
                    "platform putProjectPublication response:"
                    + put_project_publication_response["detail"]["message"]
                )
                raise HTTPException(
                    status_code=status_code,
                    detail=put_project_publication_response["detail"]["message"],
                )
        # PP独自案件の場合
        else:
            status_code, delete_project_by_pf_id_response = platform_api_operator.delete_project_by_pf_id(
                pf_project_id=pf_get_projects_response["projects"][0]["project"]["id"],
                version=pf_get_projects_response["projects"][0]["project"]["version"],
            )
            logger.info(f"platform deleteProjectByPfId statusCode: {status_code}")
            if status_code != 200:
                logger.info(
                    "platform deleteProjectByPfId response:"
                    + delete_project_by_pf_id_response["detail"]["message"]
                )
                raise HTTPException(
                    status_code=status_code,
                    detail=delete_project_by_pf_id_response["detail"]["message"],
                )

        return OKResponse()

    @staticmethod
    def check_csv_format(csv_dict_reader: csv.DictReader) -> bool:
        """CSVファイルのフォーマットチェック（規定ヘッダの有無チェック）
        Args:
            csv_dict_reader (csv.DictReader): CSVファイル情報
        Returns:
            bool: チェック結果
        """
        columns = [
            CsvColumn.SALESFORCE_OPPORTUNITY_ID,
            CsvColumn.SALESFORCE_CUSTOMER_ID,
            CsvColumn.SALESFORCE_UPDATE_AT,
            CsvColumn.NAME,
            CsvColumn.PJ_NAME_IN_SURVEY,
            CsvColumn.CUSTOMER_NAME,
            CsvColumn.SERVICE_TYPE,
            CsvColumn.SUPPORTER_ORGANIZATION_SHORT_NAME,
            CsvColumn.SALESFORCE_USE_PACKAGE,
            CsvColumn.CREATE_NEW,
            CsvColumn.CONTINUED,
            CsvColumn.SALESFORCE_VIA_PR,
            CsvColumn.SALESFORCE_MAIN_CUSTOMER_NAME,
            CsvColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL,
            CsvColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME,
            CsvColumn.SALESFORCE_MAIN_CUSTOMER_JOB,
            CsvColumn.MAIN_SALES_USER_NAME,
            CsvColumn.CONTRACT_DATE,
            CsvColumn.PHASE,
            CsvColumn.CUSTOMER_SUCCESS,
            CsvColumn.SUPPORT_DATE_FROM,
            CsvColumn.SUPPORT_DATE_TO,
            CsvColumn.PROFIT_MONTHLY_04,
            CsvColumn.PROFIT_MONTHLY_05,
            CsvColumn.PROFIT_MONTHLY_06,
            CsvColumn.PROFIT_MONTHLY_07,
            CsvColumn.PROFIT_MONTHLY_08,
            CsvColumn.PROFIT_MONTHLY_09,
            CsvColumn.PROFIT_MONTHLY_10,
            CsvColumn.PROFIT_MONTHLY_11,
            CsvColumn.PROFIT_MONTHLY_12,
            CsvColumn.PROFIT_MONTHLY_01,
            CsvColumn.PROFIT_MONTHLY_02,
            CsvColumn.PROFIT_MONTHLY_03,
            CsvColumn.PROFIT_QUARTERLY_1Q,
            CsvColumn.PROFIT_QUARTERLY_2Q,
            CsvColumn.PROFIT_QUARTERLY_3Q,
            CsvColumn.PROFIT_QUARTERLY_4Q,
            CsvColumn.PROFIT_HALF_1H,
            CsvColumn.PROFIT_HALF_2H,
            CsvColumn.PROFIT_YEAR,
            CsvColumn.GROSS_MONTHLY_04,
            CsvColumn.GROSS_MONTHLY_05,
            CsvColumn.GROSS_MONTHLY_06,
            CsvColumn.GROSS_MONTHLY_07,
            CsvColumn.GROSS_MONTHLY_08,
            CsvColumn.GROSS_MONTHLY_09,
            CsvColumn.GROSS_MONTHLY_10,
            CsvColumn.GROSS_MONTHLY_11,
            CsvColumn.GROSS_MONTHLY_12,
            CsvColumn.GROSS_MONTHLY_01,
            CsvColumn.GROSS_MONTHLY_02,
            CsvColumn.GROSS_MONTHLY_03,
            CsvColumn.GROSS_QUARTERLY_1Q,
            CsvColumn.GROSS_QUARTERLY_2Q,
            CsvColumn.GROSS_QUARTERLY_3Q,
            CsvColumn.GROSS_QUARTERLY_4Q,
            CsvColumn.GROSS_HALF_1H,
            CsvColumn.GROSS_HALF_2H,
            CsvColumn.GROSS_YEAR,
            CsvColumn.SALESFORCE_MAIN_SUPPORTER_USER_NAME,
            CsvColumn.SALESFORCE_SUPPORTER_USER_NAMES,
            CsvColumn.TOTAL_CONTRACT_TIME,
        ]
        for col in columns:
            if col not in csv_dict_reader.fieldnames:
                return False
        return True

    @staticmethod
    def edit_profit(data: dict) -> Profit:
        """CSVデータを基に粗利項目を編集
        Args:
            data (dict): CSVファイルの行データ
        Returns:
            Profit: 粗利情報
        """
        monthly: List[float] = [
            float(data.get(key)) if data.get(key) else 0.0
            for key in ProjectService.profit_month_keys
        ]
        quarterly: List[float] = [
            float(data.get(key)) if data.get(key) else 0.0
            for key in ProjectService.profit_quarter_keys
        ]
        half: List[float] = [
            float(data.get(key)) if data.get(key) else 0.0
            for key in ProjectService.profit_half_keys
        ]
        year: int = (
            int(data.get(CsvColumn.PROFIT_YEAR))
            if data.get(CsvColumn.PROFIT_YEAR)
            else 0
        )

        return Profit(year=year, monthly=monthly, quarterly=quarterly, half=half)

    @staticmethod
    def edit_gross(data: dict) -> Gross:
        """CSVデータを基に売上項目を編集
        Args:
            data (dict): CSVファイルの行データ
        Returns:
            Gross: 売上情報
        """
        monthly: List[float] = [
            float(data.get(key)) if data.get(key) else 0.0
            for key in ProjectService.gross_month_keys
        ]
        quarterly: List[float] = [
            float(data.get(key)) if data.get(key) else 0.0
            for key in ProjectService.gross_quarter_keys
        ]
        half: List[float] = [
            float(data.get(key)) if data.get(key) else 0.0
            for key in ProjectService.gross_half_keys
        ]
        year: int = (
            int(data.get(CsvColumn.GROSS_YEAR)) if data.get(CsvColumn.GROSS_YEAR) else 0
        )

        return Gross(year=year, monthly=monthly, quarterly=quarterly, half=half)

    @staticmethod
    def get_import_response_project_info(
        import_mode: ImportModeType, data: dict
    ) -> ProjectInfoForImportProjects:
        """インポート処理のレスポンス編集
        Args:
            import_mode (ImportModeType): インポート処理モード
            data (dict): CSVファイルの行データ
        Returns:
            ProjectInfoForImportProjects: 行単位のproject情報
        """
        salesforce_supporter_user_names: List[str] = []
        temp_user_names = data.get(CsvColumn.SALESFORCE_SUPPORTER_USER_NAMES)
        if temp_user_names:
            salesforce_supporter_user_names = [
                x.strip() for x in temp_user_names.split(";")
            ]
        customer_success = (
            data.get(CsvColumn.CUSTOMER_SUCCESS)
            if data.get(CsvColumn.CUSTOMER_SUCCESS)
            else None
        )
        customer_id = None
        service_type = None
        create_new = None
        continued = None
        profit = None
        total_contract_time = None
        salesforce_main_customer = None
        supporter_organization_id = None
        gross = None
        salesforce_use_package = None
        salesforce_via_pr = None
        main_sales_user_id = None
        phase = None

        return ProjectInfoForImportProjects(
            customer_id=customer_id,
            salesforce_customer_id=data.get(CsvColumn.SALESFORCE_CUSTOMER_ID),
            salesforce_opportunity_id=data.get(CsvColumn.SALESFORCE_OPPORTUNITY_ID),
            salesforce_update_at=data.get(CsvColumn.SALESFORCE_UPDATE_AT),
            name=data.get(CsvColumn.NAME),
            customer_name=data.get(CsvColumn.CUSTOMER_NAME),
            service_type=service_type,
            service_type_name=data.get(CsvColumn.SERVICE_TYPE),
            create_new=create_new,
            continued=continued,
            main_sales_user_id=main_sales_user_id,
            main_sales_user_name=data.get(CsvColumn.MAIN_SALES_USER_NAME),
            contract_date=data.get(CsvColumn.CONTRACT_DATE),
            phase=phase,
            customer_success=customer_success,
            support_date_from=data.get(CsvColumn.SUPPORT_DATE_FROM),
            support_date_to=data.get(CsvColumn.SUPPORT_DATE_TO),
            profit=profit,
            total_contract_time=total_contract_time,
            salesforce_main_customer=salesforce_main_customer,
            main_supporter_user_name=None,
            supporter_organization_id=supporter_organization_id,
            supporter_organization_name=data.get(
                CsvColumn.SUPPORTER_ORGANIZATION_SHORT_NAME
            ),
            salesforce_main_supporter_user_name=data.get(
                CsvColumn.SALESFORCE_MAIN_SUPPORTER_USER_NAME
            ),
            supporter_users=None,
            salesforce_supporter_user_names=salesforce_supporter_user_names,
            gross=gross,
            salesforce_use_package=salesforce_use_package,
            salesforce_via_pr=salesforce_via_pr,
        )

    @staticmethod
    def edit_import_change_column_log(
        edit_log_list: List[dict],
        project_name: str,
        row_change_log: List[dict],
    ) -> None:
        # """インポート処理の更新箇所の辞書（案件名、更新項目名、更新値）リストに追記
        # e.g. [
        #         {
        #             "project_name": "CCC案件",
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
        #         {
        #             "project_name": "DDD案件",
        #             "update_column_info": [
        #                 {
        #                     "update_column": "最終更新日",
        #                     "update_value": "2021-01-01",
        #                 },
        #             ],
        #         },
        #     ],
        # Args:
        #     edit_log_list (List[dict]): 更新用ログのリスト
        #     project_name (str): 案件名
        #     row_change_log (List[dict]): 変更点のリスト
        # """
        edit_log_list.append(
            {
                "project_name": project_name,
                "update_column_info": row_change_log,
            }
        )

    @staticmethod
    def edit_target_project(
        target_project: ProjectModel,
        customer_id: str,
        salesforce_customer_id: str,
        salesforce_update_at: datetime,
        name: str,
        customer_name: str,
        service_type: str,
        service_type_name: str,
        create_new: bool,
        continued: bool,
        main_sales_user_id: str,
        main_sales_user_name: str,
        contract_date: str,
        phase: str,
        customer_success: str,
        support_date_from: str,
        support_date_to: str,
        profit: GrossProfitAttribute,
        gross: GrossProfitAttribute,
        total_contract_time: float,
        salesforce_main_customer: SalesforceMainCustomerAttribute,
        supporter_organization_id: str,
        supporter_organization_name: str,
        salesforce_main_supporter_user_name: str,
        salesforce_supporter_user_names: set,
        salesforce_use_package: bool,
        salesforce_via_pr: bool,
        update_id: str,
        update_at: datetime,
    ) -> List[dict]:
        """案件情報更新時の編集
        Args:
            target_project (ProjectModel): ProjectModel
            customer_id (str): 取引先ID
            salesforce_customer_id (str): Salesforce取引先ID
            salesforce_update_at (datetime): Salesforce最終更新日時
            name (str): 商談名（案件名）
            customer_name (str): 取引先名
            service_type (str): サービス区分
            service_type_name (str): サービス区分名 ※メールの更新履歴編集用
            create_new (bool): 新規・更新
            continued (bool): 期をまたぐ案件
            main_sales_user_id (str): 営業担当者
            main_sales_user_name (str): 営業担当者名 ※メールの更新履歴編集用
            contract_date (str): 契約締結日
            phase (str): フェーズ
            customer_success (str): カスタマーサクセス
            support_date_from (str): 支援開始日
            support_date_to (str): 支援終了日
            profit (GrossProfitAttribute): 粗利
            gross (GrossProfitAttribute): 売上
            total_contract_time (float): 延べ契約時間
            salesforce_main_customer (SalesforceMainCustomerAttribute): Salesforce取引先責任者
            supporter_organization_id (str): 支援者組織ID
            supporter_organization_name (str): 支援者組織名 ※メールの更新履歴編集用
            salesforce_main_supporter_user_name (str): Salesforce主担当
            salesforce_supporter_user_names (set): Salesforce主担当
            salesforce_use_package (bool): パッケージ利用
            salesforce_via_pr (bool): PR経由

        Returns:
            row_change_log (List[dict]): 新規登録の変更点
        """
        row_change_log: List[dict] = []
        update_attributes: List[UpdateAttributesSubAttribute] = []

        if target_project.customer_id != customer_id:
            target_project.customer_id = customer_id
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_ID.tbl_column_name,
                    value=customer_id,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.CUSTOMER_ID.log_column_name,
                    "update_value": customer_id,
                }
            )
        if target_project.salesforce_customer_id != salesforce_customer_id:
            target_project.salesforce_customer_id = salesforce_customer_id
            # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_CUSTOMER_ID.tbl_column_name,
                    value=salesforce_customer_id,
                )
            )
        if target_project.salesforce_update_at != salesforce_update_at:
            target_project.salesforce_update_at = salesforce_update_at
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_UPDATE_AT.tbl_column_name,
                    value=salesforce_update_at.strftime("%Y/%m/%d %H:%M"),
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.SALESFORCE_UPDATE_AT.log_column_name,
                    "update_value": salesforce_update_at.strftime("%Y/%m/%d %H:%M"),
                }
            )
        if target_project.name != name:
            target_project.name = name
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.NAME.tbl_column_name,
                    value=name,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.NAME.log_column_name,
                    "update_value": name,
                }
            )
        if target_project.customer_name != customer_name:
            target_project.customer_name = customer_name
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_NAME.tbl_column_name,
                    value=customer_name,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.CUSTOMER_NAME.log_column_name,
                    "update_value": customer_name,
                }
            )
        if target_project.service_type != service_type:
            target_project.service_type = service_type
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SERVICE_TYPE.tbl_column_name,
                    value=service_type,
                )
            )
            log_value_service_type = service_type
            if service_type_name:
                log_value_service_type = service_type_name
            row_change_log.append(
                {
                    "update_column": ProjectColumn.SERVICE_TYPE.log_column_name,
                    "update_value": log_value_service_type,
                }
            )
        if target_project.create_new != create_new:
            target_project.create_new = create_new
            sub_attribute_create_new = None
            if target_project.create_new is not None:
                sub_attribute_create_new = str(target_project.create_new)
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CREATE_NEW.tbl_column_name,
                    value=sub_attribute_create_new,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.CREATE_NEW.log_column_name,
                    "update_value": sub_attribute_create_new,
                }
            )
        if target_project.continued != continued:
            target_project.continued = continued
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CONTINUED.tbl_column_name,
                    value=str(continued),
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.CONTINUED.log_column_name,
                    "update_value": str(continued),
                }
            )
        if target_project.main_sales_user_id != main_sales_user_id:
            target_project.main_sales_user_id = main_sales_user_id
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.MAIN_SALES_USER_ID.tbl_column_name,
                    value=main_sales_user_id,
                )
            )
            log_value_main_sales_user_name = main_sales_user_id
            if main_sales_user_name:
                log_value_main_sales_user_name = main_sales_user_name
            row_change_log.append(
                {
                    "update_column": ProjectColumn.MAIN_SALES_USER_ID.log_column_name,
                    "update_value": log_value_main_sales_user_name,
                }
            )
        if target_project.contract_date != contract_date:
            target_project.contract_date = contract_date
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CONTRACT_DATE.tbl_column_name,
                    value=contract_date,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.CONTRACT_DATE.log_column_name,
                    "update_value": contract_date,
                }
            )
        if target_project.phase != phase:
            target_project.phase = phase
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.PHASE.tbl_column_name,
                    value=phase,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.PHASE.log_column_name,
                    "update_value": phase,
                }
            )
        if target_project.customer_success != customer_success:
            target_project.customer_success = customer_success
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_SUCCESS.tbl_column_name,
                    value=customer_success,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.CUSTOMER_SUCCESS.log_column_name,
                    "update_value": customer_success,
                }
            )
        if target_project.support_date_from != support_date_from:
            target_project.support_date_from = support_date_from
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SUPPORT_DATE_FROM.tbl_column_name,
                    value=support_date_from,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.SUPPORT_DATE_FROM.log_column_name,
                    "update_value": support_date_from,
                }
            )
        if target_project.support_date_to != support_date_to:
            target_project.support_date_to = support_date_to
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SUPPORT_DATE_TO.tbl_column_name,
                    value=support_date_to,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.SUPPORT_DATE_TO.log_column_name,
                    "update_value": support_date_to,
                }
            )

        if profit is not None:
            if target_project.profit is None:
                target_project.profit = profit
                # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.PROFIT_MONTHLY.tbl_column_name,
                        value=",".join(map(str, profit.monthly)),
                    )
                )
                # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.PROFIT_QUARTERLY.tbl_column_name,
                        value=",".join(map(str, profit.quarterly)),
                    )
                )
                # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.PROFIT_HALF.tbl_column_name,
                        value=",".join(map(str, profit.half)),
                    )
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.PROFIT_YEAR.tbl_column_name,
                        value=str(profit.year),
                    )
                )
                row_change_log.append(
                    {
                        "update_column": ProjectColumn.PROFIT_YEAR.log_column_name,
                        "update_value": str(profit.year),
                    }
                )
            else:
                if target_project.profit.monthly != profit.monthly:
                    target_project.profit.monthly = profit.monthly
                    # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.PROFIT_MONTHLY.tbl_column_name,
                            value=",".join(map(str, profit.monthly)),
                        )
                    )
                if target_project.profit.quarterly != profit.quarterly:
                    target_project.profit.quarterly = profit.quarterly
                    # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.PROFIT_QUARTERLY.tbl_column_name,
                            value=",".join(map(str, profit.quarterly)),
                        )
                    )
                if target_project.profit.half != profit.half:
                    target_project.profit.half = profit.half
                    # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.PROFIT_HALF.tbl_column_name,
                            value=",".join(map(str, profit.half)),
                        )
                    )
                if target_project.profit.year != profit.year:
                    target_project.profit.year = profit.year
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.PROFIT_YEAR.tbl_column_name,
                            value=str(profit.year),
                        )
                    )
                    row_change_log.append(
                        {
                            "update_column": ProjectColumn.PROFIT_YEAR.log_column_name,
                            "update_value": str(profit.year),
                        }
                    )
        else:
            # 事前チェックで必須項目のため、Noneは想定外
            raise Exception("Unexpected: Column 'profit' is None.")

        if gross is not None:
            if target_project.gross is None:
                target_project.gross = gross
                # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.GROSS_MONTHLY.tbl_column_name,
                        value=",".join(map(str, gross.monthly)),
                    )
                )
                # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.GROSS_QUARTERLY.tbl_column_name,
                        value=",".join(map(str, gross.quarterly)),
                    )
                )
                # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.GROSS_HALF.tbl_column_name,
                        value=",".join(map(str, gross.half)),
                    )
                )
                # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.GROSS_YEAR.tbl_column_name,
                        value=str(gross.year),
                    )
                )
            if target_project.gross is not None:
                if target_project.gross.monthly != gross.monthly:
                    target_project.gross.monthly = gross.monthly
                    # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.GROSS_MONTHLY.tbl_column_name,
                            value=",".join(map(str, gross.monthly)),
                        )
                    )
                if target_project.gross.quarterly != gross.quarterly:
                    target_project.gross.quarterly = gross.quarterly
                    # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.GROSS_QUARTERLY.tbl_column_name,
                            value=",".join(map(str, gross.quarterly)),
                        )
                    )
                if target_project.gross.half != gross.half:
                    target_project.gross.half = gross.half
                    # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.GROSS_HALF.tbl_column_name,
                            value=",".join(map(str, gross.half)),
                        )
                    )
                if target_project.gross.year != gross.year:
                    target_project.gross.year = gross.year
                    # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.GROSS_YEAR.tbl_column_name,
                            value=str(gross.year),
                        )
                    )
        else:
            # 事前チェックで必須項目のため、Noneは想定外
            raise Exception("Unexpected: Column 'gross' is None.")

        if target_project.total_contract_time != total_contract_time:
            target_project.total_contract_time = total_contract_time
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.TOTAL_CONTRACT_TIME.tbl_column_name,
                    value=str(total_contract_time),
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.TOTAL_CONTRACT_TIME.log_column_name,
                    "update_value": str(total_contract_time),
                }
            )
        if salesforce_main_customer is not None:
            if target_project.salesforce_main_customer is None:
                target_project.salesforce_main_customer = salesforce_main_customer
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.tbl_column_name,
                        value=salesforce_main_customer.name,
                    )
                )
                row_change_log.append(
                    {
                        "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.log_column_name,
                        "update_value": salesforce_main_customer.name,
                    }
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.tbl_column_name,
                        value=salesforce_main_customer.email,
                    )
                )
                row_change_log.append(
                    {
                        "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.log_column_name,
                        "update_value": salesforce_main_customer.email,
                    }
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.tbl_column_name,
                        value=salesforce_main_customer.organization_name,
                    )
                )
                row_change_log.append(
                    {
                        "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.log_column_name,
                        "update_value": salesforce_main_customer.organization_name,
                    }
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_JOB.tbl_column_name,
                        value=salesforce_main_customer.job,
                    )
                )
                row_change_log.append(
                    {
                        "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_JOB.log_column_name,
                        "update_value": salesforce_main_customer.job,
                    }
                )
                if target_project.is_survey_email_to_salesforce_main_customer:
                    # 「取引先担当者をアンケート宛先に指定」がTrueの場合、匿名アンケート宛先に取引先担当者の情報をセット
                    target_project.dedicated_survey_user_name = (
                        salesforce_main_customer.name
                    )
                    target_project.dedicated_survey_user_email = (
                        salesforce_main_customer.email
                    )
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.DEDICATED_SURVEY_USER_NAME.tbl_column_name,
                            value=target_project.dedicated_survey_user_name,
                        )
                    )
                    row_change_log.append(
                        {
                            "update_column": ProjectColumn.DEDICATED_SURVEY_USER_NAME.log_column_name,
                            "update_value": target_project.dedicated_survey_user_name,
                        }
                    )
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.tbl_column_name,
                            value=target_project.dedicated_survey_user_email,
                        )
                    )
                    row_change_log.append(
                        {
                            "update_column": ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.log_column_name,
                            "update_value": target_project.dedicated_survey_user_email,
                        }
                    )

            else:
                if (
                    target_project.salesforce_main_customer.name
                    != salesforce_main_customer.name
                ):
                    target_project.salesforce_main_customer.name = (
                        salesforce_main_customer.name
                    )
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.tbl_column_name,
                            value=salesforce_main_customer.name,
                        )
                    )
                    row_change_log.append(
                        {
                            "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.log_column_name,
                            "update_value": salesforce_main_customer.name,
                        }
                    )
                    if target_project.is_survey_email_to_salesforce_main_customer:
                        # 「取引先担当者をアンケート宛先に指定」がTrueの場合、匿名アンケート宛先に取引先担当者の情報をセット
                        target_project.dedicated_survey_user_name = (
                            salesforce_main_customer.name
                        )
                        update_attributes.append(
                            UpdateAttributesSubAttribute(
                                attribute=ProjectColumn.DEDICATED_SURVEY_USER_NAME.tbl_column_name,
                                value=target_project.dedicated_survey_user_name,
                            )
                        )
                        row_change_log.append(
                            {
                                "update_column": ProjectColumn.DEDICATED_SURVEY_USER_NAME.log_column_name,
                                "update_value": target_project.dedicated_survey_user_name,
                            }
                        )
                if (
                    target_project.salesforce_main_customer.email
                    != salesforce_main_customer.email
                ):
                    target_project.salesforce_main_customer.email = (
                        salesforce_main_customer.email
                    )
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.tbl_column_name,
                            value=salesforce_main_customer.email,
                        )
                    )
                    row_change_log.append(
                        {
                            "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.log_column_name,
                            "update_value": salesforce_main_customer.email,
                        }
                    )
                    if target_project.is_survey_email_to_salesforce_main_customer:
                        # 「取引先担当者をアンケート宛先に指定」がTrueの場合、匿名アンケート宛先に取引先担当者の情報をセット
                        target_project.dedicated_survey_user_email = (
                            salesforce_main_customer.email
                        )
                        update_attributes.append(
                            UpdateAttributesSubAttribute(
                                attribute=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.tbl_column_name,
                                value=target_project.dedicated_survey_user_email,
                            )
                        )
                        row_change_log.append(
                            {
                                "update_column": ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.log_column_name,
                                "update_value": target_project.dedicated_survey_user_email,
                            }
                        )
                if (
                    target_project.salesforce_main_customer.organization_name
                    != salesforce_main_customer.organization_name
                ):
                    target_project.salesforce_main_customer.organization_name = (
                        salesforce_main_customer.organization_name
                    )
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.tbl_column_name,
                            value=salesforce_main_customer.organization_name,
                        )
                    )
                    row_change_log.append(
                        {
                            "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.log_column_name,
                            "update_value": salesforce_main_customer.organization_name,
                        }
                    )
                if (
                    target_project.salesforce_main_customer.job
                    != salesforce_main_customer.job
                ):
                    target_project.salesforce_main_customer.job = (
                        salesforce_main_customer.job
                    )
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_JOB.tbl_column_name,
                            value=salesforce_main_customer.job,
                        )
                    )
                    row_change_log.append(
                        {
                            "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_JOB.log_column_name,
                            "update_value": salesforce_main_customer.job,
                        }
                    )
        else:
            if target_project.salesforce_main_customer is not None:
                target_project.salesforce_main_customer = None
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.tbl_column_name,
                        value=None,
                    )
                )
                row_change_log.append(
                    {
                        "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.log_column_name,
                        "update_value": None,
                    }
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.tbl_column_name,
                        value=None,
                    )
                )
                row_change_log.append(
                    {
                        "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.log_column_name,
                        "update_value": None,
                    }
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.tbl_column_name,
                        value=None,
                    )
                )
                row_change_log.append(
                    {
                        "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.log_column_name,
                        "update_value": None,
                    }
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_JOB.tbl_column_name,
                        value=None,
                    )
                )
                row_change_log.append(
                    {
                        "update_column": ProjectColumn.SALESFORCE_MAIN_CUSTOMER_JOB.log_column_name,
                        "update_value": None,
                    }
                )
                if target_project.is_survey_email_to_salesforce_main_customer:
                    # 「取引先担当者をアンケート宛先に指定」がTrueの場合、匿名アンケート宛先に取引先担当者の情報をセット
                    target_project.dedicated_survey_user_name = None
                    target_project.dedicated_survey_user_email = None
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.DEDICATED_SURVEY_USER_NAME.tbl_column_name,
                            value=None,
                        )
                    )
                    row_change_log.append(
                        {
                            "update_column": ProjectColumn.DEDICATED_SURVEY_USER_NAME.log_column_name,
                            "update_value": None,
                        }
                    )
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.tbl_column_name,
                            value=None,
                        )
                    )
                    row_change_log.append(
                        {
                            "update_column": ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.log_column_name,
                            "update_value": None,
                        }
                    )

        if target_project.supporter_organization_id != supporter_organization_id:
            target_project.supporter_organization_id = supporter_organization_id
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SUPPORTER_ORGANIZATION_ID.tbl_column_name,
                    value=supporter_organization_id,
                )
            )
            log_value_supporter_organization_name = supporter_organization_id
            if supporter_organization_name:
                log_value_supporter_organization_name = supporter_organization_name
            row_change_log.append(
                {
                    "update_column": ProjectColumn.SUPPORTER_ORGANIZATION_ID.log_column_name,
                    "update_value": log_value_supporter_organization_name,
                }
            )
        if (
            target_project.salesforce_main_supporter_user_name
            != salesforce_main_supporter_user_name
        ):
            target_project.salesforce_main_supporter_user_name = (
                salesforce_main_supporter_user_name
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_MAIN_SUPPORTER_USER_NAME.tbl_column_name,
                    value=salesforce_main_supporter_user_name,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.SALESFORCE_MAIN_SUPPORTER_USER_NAME.log_column_name,
                    "update_value": salesforce_main_supporter_user_name,
                }
            )
        if (
            target_project.salesforce_supporter_user_names
            != salesforce_supporter_user_names
        ):
            target_project.salesforce_supporter_user_names = (
                salesforce_supporter_user_names
            )

            log_str_salesforce_supporter_user_names = ""
            if salesforce_supporter_user_names:
                log_str_salesforce_supporter_user_names = ",".join(
                    salesforce_supporter_user_names
                )

            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_SUPPORTER_USER_NAMES.tbl_column_name,
                    value=log_str_salesforce_supporter_user_names,
                )
            )
            row_change_log.append(
                {
                    "update_column": ProjectColumn.SALESFORCE_SUPPORTER_USER_NAMES.log_column_name,
                    "update_value": log_str_salesforce_supporter_user_names,
                }
            )
        if target_project.salesforce_use_package != salesforce_use_package:
            target_project.salesforce_use_package = salesforce_use_package
            # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_USE_PACKAGE.tbl_column_name,
                    value=str(salesforce_use_package),
                )
            )
        if target_project.salesforce_via_pr != salesforce_via_pr:
            target_project.salesforce_via_pr = salesforce_via_pr
            # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_VIA_PR.tbl_column_name,
                    value=str(salesforce_via_pr),
                )
            )
        target_project.update_history = UpdateHistoryAttribute(
            update_id=update_id,
            update_attributes=update_attributes,
        )
        target_project.update_id = update_id
        target_project.update_at = update_at
        target_project.version += 1

        return row_change_log

    @staticmethod
    def import_execute_parallel(
        row: dict,
        projects: List[ProjectInfoForImportProjects],
        sf_opportunity_id_map: dict,
        customer_list: List[CustomerModel],
        master_service_list: List[MasterSupporterOrganizationModel],
        master_supporter_org_list: List[MasterSupporterOrganizationModel],
        user_list: List[UserModel],
        project_batch: BatchWrite[ProjectModel],
        current_user: AdminModel,
        update_datetime: datetime,
        user_project_update_map: dict,
        before_user_project_map: dict,
        to_mail_user_id_list: List[str],
        to_assign_mail_user_id_list: List[str],
        edit_log_list: List[dict],
        new_log_list: List[str],
    ) -> None:
        """案件情報の新規登録、更新の処理
        Args:
            row (dict): CSVの行データ
            projects (List[ProjectInfoForImportProjects]): レスポンス項目
            sf_opportunity_id_map (dict): 案件情報のDB取得結果から作成した辞書(key: salesforce_opportunity_id, value: CustomerModel)
            customer_list (List[CustomerModel]): 取引先情報
            master_service_list (List[MasterSupporterOrganizationModel]): 汎用マスタ(サービス種別)
            master_supporter_org_list (List[MasterSupporterOrganizationModel]): 汎用マスタ(支援者組織)
            user_list (List[UserModel]): 一般ユーザ情報
            project_batch (BatchWrite[ProjectModel]): BatchWriteオブジェクト
            current_user (AdminModel): 認証済のユーザ
            update_datetime (datetime): 更新日時
            user_project_update_map (dict): 一般ユーザ案件更新マップ(key:案件ID, value:ユーザIDリスト)
            before_user_project_map (dict): 更新前の一般ユーザ案件更新マップ
            to_mail_user_id_list (List[dict]): 「案件情報新規登録通知」メールの宛先TOに入れるユーザID
            to_assign_mail_user_id_list (List[dict]): 「案件アサイン通知」メールの宛先TOに入れるユーザID
            edit_log_list (List[dict]): 更新用ログのリスト
            new_log_list (List[str]): 新規登録用ログのリスト
        """
        # 登録内容の編集
        salesforce_customer_id = (
            row[CsvColumn.SALESFORCE_CUSTOMER_ID]
            if row[CsvColumn.SALESFORCE_CUSTOMER_ID]
            else None
        )
        salesforce_opportunity_id = (
            row[CsvColumn.SALESFORCE_OPPORTUNITY_ID]
            if row[CsvColumn.SALESFORCE_OPPORTUNITY_ID]
            else None
        )

        # Salesforce商談IDを基に取引先ID,取引先名を取得
        customer_id = None
        customer_name = None
        if salesforce_customer_id:
            for customer in customer_list:
                if customer.salesforce_customer_id == salesforce_customer_id:
                    customer_id = customer.id
                    customer_name = customer.name

        salesforce_update_at = (
            ProjectService.jst.localize(
                datetime.strptime(
                    row[CsvColumn.SALESFORCE_UPDATE_AT],
                    "%Y/%m/%d %H:%M",
                )
            )
            if row[CsvColumn.SALESFORCE_UPDATE_AT]
            else None
        )
        name = row[CsvColumn.NAME] if row[CsvColumn.NAME] else None

        service_type = None
        service_type_name = None
        service_type_name_no_prefix = ProjectService.remove_prefix_num(
            row[CsvColumn.SERVICE_TYPE]
        )
        for master_service in master_service_list:
            if service_type_name_no_prefix == master_service.name:
                service_type = master_service.id
                service_type_name = master_service.name

        create_new = (
            ProjectService.convert_create_new_to_bool(row[CsvColumn.CREATE_NEW])
            if row[CsvColumn.CREATE_NEW]
            else None
        )
        continued = (
            strtobool(row[CsvColumn.CONTINUED]) if row[CsvColumn.CONTINUED] else None
        )
        main_sales_user_id = None
        main_sales_user_name = None
        for user in user_list:
            if row[CsvColumn.MAIN_SALES_USER_NAME] == user.name and (
                user.role == UserRoleType.SALES.key or user.role == UserRoleType.SALES_MGR.key
            ):
                if not user.disabled:
                    main_sales_user_id = user.id
                    main_sales_user_name = user.name
        contract_date = (
            row[CsvColumn.CONTRACT_DATE] if row[CsvColumn.CONTRACT_DATE] else None
        )
        phase = row[CsvColumn.PHASE] if row[CsvColumn.PHASE] else None
        customer_success = (
            row[CsvColumn.CUSTOMER_SUCCESS] if row[CsvColumn.CUSTOMER_SUCCESS] else None
        )
        support_date_from = (
            row[CsvColumn.SUPPORT_DATE_FROM]
            if row[CsvColumn.SUPPORT_DATE_FROM]
            else None
        )
        support_date_to = (
            row[CsvColumn.SUPPORT_DATE_TO] if row[CsvColumn.SUPPORT_DATE_TO] else None
        )
        profit = ProjectService.edit_profit(row)
        gross = ProjectService.edit_gross(row)
        total_contract_time = (
            float(row[CsvColumn.TOTAL_CONTRACT_TIME])
            if row[CsvColumn.TOTAL_CONTRACT_TIME]
            else 0.0
        )
        salesforce_main_customer = (
            SalesforceMainCustomer(
                name=row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_NAME]
                if row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_NAME]
                else None,
                email=row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL]
                if row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL]
                else None,
                organization_name=row[
                    CsvColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME
                ]
                if row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME]
                else None,
                job=row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_JOB]
                if row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_JOB]
                else None,
            )
            if (
                row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_NAME]
                or row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL]
                or row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME]
                or row[CsvColumn.SALESFORCE_MAIN_CUSTOMER_JOB]
            )
            else None
        )

        supporter_organization_id = None
        supporter_organization_name = None
        for master_supporter_org in master_supporter_org_list:
            if (
                row[CsvColumn.SUPPORTER_ORGANIZATION_SHORT_NAME]
                == master_supporter_org.value
            ):
                supporter_organization_id = master_supporter_org.id
                supporter_organization_name = master_supporter_org.value
        salesforce_main_supporter_user_name = (
            row[CsvColumn.SALESFORCE_MAIN_SUPPORTER_USER_NAME]
            if row[CsvColumn.SALESFORCE_MAIN_SUPPORTER_USER_NAME]
            else None
        )

        salesforce_supporter_user_names: set = None
        temp_user_names = row[CsvColumn.SALESFORCE_SUPPORTER_USER_NAMES]
        if temp_user_names:
            salesforce_supporter_user_names = set()
            for temp in temp_user_names.split(";"):
                salesforce_supporter_user_names.add(temp.strip())
        contract_type = ContractType.FOR_VALUE
        salesforce_use_package = (
            strtobool(row[CsvColumn.SALESFORCE_USE_PACKAGE])
            if row[CsvColumn.SALESFORCE_USE_PACKAGE]
            else None
        )
        salesforce_via_pr = (
            strtobool(row[CsvColumn.SALESFORCE_VIA_PR])
            if row[CsvColumn.SALESFORCE_VIA_PR]
            else None
        )

        fo_site_url = get_app_settings().fo_site_url
        bo_site_url = get_app_settings().bo_site_url
        is_new: bool = False
        is_update: bool = False
        row_change_log: List[dict] = []
        temp_user_project_update_map: dict = {}
        temp_before_user_project_map: dict = {}
        temp_to_mail_user_id_dict: dict = {}
        temp_to_assign_mail_user_id_dict: dict = {}

        if salesforce_opportunity_id not in sf_opportunity_id_map.keys():
            # 新規登録
            is_new = True
            # メイン支援者（主担当）は、新規登録の場合のみ登録
            # プロデューサー(主担当1名)が支援者、支援者責任者又は事業者責任者とヒットすれば、メイン支援者（主担当）にユーザIDをセット
            main_supporter_user_id = None
            main_supporter_user_name = None
            if salesforce_main_supporter_user_name:
                non_prefix_user_name = None
                temp_split_value = salesforce_main_supporter_user_name.split("_")
                if len(temp_split_value) > 1:
                    non_prefix_user_name = temp_split_value[1].strip()
                else:
                    non_prefix_user_name = temp_split_value[0].strip()
                # 一般ユーザーテーブルの支援者、支援者責任者又は事業者責任者ユーザーの名前を参照しユーザーIDを取得
                for user in user_list:
                    if user.role in [
                        UserRoleType.SUPPORTER.key,
                        UserRoleType.SUPPORTER_MGR.key,
                        UserRoleType.BUSINESS_MGR.key,
                    ]:
                        if user.name == non_prefix_user_name:
                            if not user.disabled:
                                main_supporter_user_id = user.id
                                main_supporter_user_name = user.name

            # 支援者メンバー（副担当）は、新規登録の場合のみ登録
            # アクセラレーター(副担当複数名)が支援者、支援者責任者又は事業者責任者とヒットすれば、支援者メンバー（副担当）にユーザIDをセット
            supporter_user_ids: set = None
            supporter_user_name_list: List[str] = None
            if salesforce_supporter_user_names:
                for user_name in salesforce_supporter_user_names:
                    non_prefix_user_name = None
                    temp_split_value = user_name.split("_")
                    if len(temp_split_value) > 1:
                        non_prefix_user_name = temp_split_value[1].strip()
                    else:
                        non_prefix_user_name = temp_split_value[0].strip()
                    # 一般ユーザーテーブルの支援者、支援者責任者又は事業者責任者ユーザーの名前を参照しユーザーIDを取得
                    for user in user_list:
                        if user.role in [
                            UserRoleType.SUPPORTER.key,
                            UserRoleType.SUPPORTER_MGR.key,
                            UserRoleType.BUSINESS_MGR.key,
                        ]:
                            if user.name == non_prefix_user_name:
                                if not user.disabled:
                                    if supporter_user_ids is None:
                                        supporter_user_ids = set()
                                    if supporter_user_name_list is None:
                                        supporter_user_name_list = []
                                    supporter_user_ids.add(user.id)
                                    supporter_user_name_list.append(user.name)
            # 新規登録の場合、Salesforce取引先責任者の情報を匿名アンケート宛先にセット
            is_survey_email_to_salesforce_main_customer = True
            dedicated_survey_user_name = None
            dedicated_survey_user_email = None
            if salesforce_main_customer:
                dedicated_survey_user_name = salesforce_main_customer.name
                dedicated_survey_user_email = salesforce_main_customer.email
            # 新規登録の場合、匿名アンケートパスワードを生成してセット
            encrypted_survey_password = AesCipherUtils.encrypt(
                create_random_survey_password(SurveyPasswordSetting.PASSWORD_LENGTH)
            )

            new_project = ProjectModel(
                id=str(uuid.uuid4()),
                data_type=DataType.PROJECT,
                customer_id=customer_id,
                salesforce_customer_id=salesforce_customer_id,
                salesforce_opportunity_id=salesforce_opportunity_id,
                salesforce_update_at=salesforce_update_at,
                name=name,
                customer_name=customer_name,
                service_type=service_type,
                create_new=create_new,
                continued=continued,
                main_sales_user_id=main_sales_user_id,
                contract_date=contract_date,
                phase=phase,
                customer_success=customer_success,
                support_date_from=support_date_from,
                support_date_to=support_date_to,
                profit=GrossProfitAttribute(
                    monthly=profit.monthly,
                    quarterly=profit.quarterly,
                    half=profit.half,
                    year=profit.year,
                )
                if profit
                else None,
                gross=GrossProfitAttribute(
                    monthly=gross.monthly,
                    quarterly=gross.quarterly,
                    half=gross.half,
                    year=gross.year,
                )
                if gross
                else None,
                total_contract_time=total_contract_time,
                salesforce_main_customer=SalesforceMainCustomerAttribute(
                    name=salesforce_main_customer.name,
                    email=salesforce_main_customer.email,
                    organization_name=salesforce_main_customer.organization_name,
                    job=salesforce_main_customer.job,
                )
                if salesforce_main_customer
                else None,
                main_supporter_user_id=main_supporter_user_id,
                supporter_organization_id=supporter_organization_id,
                salesforce_main_supporter_user_name=salesforce_main_supporter_user_name,
                supporter_user_ids=supporter_user_ids,
                salesforce_supporter_user_names=salesforce_supporter_user_names,
                contract_type=contract_type,
                salesforce_use_package=salesforce_use_package,
                salesforce_via_pr=salesforce_via_pr,
                dedicated_survey_user_name=dedicated_survey_user_name,
                dedicated_survey_user_email=dedicated_survey_user_email,
                survey_password=encrypted_survey_password,
                is_survey_email_to_salesforce_main_customer=is_survey_email_to_salesforce_main_customer,
                create_id=current_user.id,
                create_at=update_datetime,
                update_id=current_user.id,
                update_at=update_datetime,
                version=1,
            )
            # 一般ユーザ更新用のデータ編集
            target_user_id_list = []
            if new_project.main_sales_user_id:
                target_user_id_list.append(new_project.main_sales_user_id)
            if new_project.main_customer_user_id:
                target_user_id_list.append(new_project.main_customer_user_id)
            if new_project.main_supporter_user_id:
                target_user_id_list.append(new_project.main_supporter_user_id)
            if new_project.customer_user_ids:
                for customer_user_id in new_project.customer_user_ids:
                    target_user_id_list.append(customer_user_id)
            if new_project.supporter_user_ids:
                for supporter_user_id in new_project.supporter_user_ids:
                    target_user_id_list.append(supporter_user_id)
            target_user_id_list = list(set(target_user_id_list))
            temp_user_project_update_map[new_project.id] = target_user_id_list

            # 新規登録メール通知対象(宛先To)の編集（案件にアサインされた支援者、事業者責任者、営業担当者）
            temp_to_mail_user_id_dict["project_id"] = new_project.id
            temp_to_mail_user_id_dict["project_name"] = new_project.name
            user_id_list = [
                new_project.main_sales_user_id,
                new_project.main_supporter_user_id,
            ]
            if new_project.supporter_user_ids:
                for supporter_user_id in new_project.supporter_user_ids:
                    user_id_list.append(supporter_user_id)
            user_id_list = list(set(user_id_list))
            temp_to_mail_user_id_dict["user_id_list"] = user_id_list

            # アサインメール通知対象(宛先To)の編集
            temp_to_assign_mail_user_id_dict["project_id"] = new_project.id
            temp_to_assign_mail_user_id_dict["project_name"] = new_project.name
            user_id_list = [
                new_project.main_sales_user_id,
                new_project.main_supporter_user_id,
            ]
            if new_project.supporter_user_ids:
                for supporter_user_id in new_project.supporter_user_ids:
                    user_id_list.append(supporter_user_id)
            user_id_list = list(set(user_id_list))
            temp_to_assign_mail_user_id_dict["user_id_list"] = user_id_list

        else:
            # 更新
            target_project: ProjectModel = copy.deepcopy(
                sf_opportunity_id_map[salesforce_opportunity_id]
            )
            # DBのSF最終更新日時 < CSVのSF最終更新日時の場合のみ、更新
            if target_project.salesforce_update_at < salesforce_update_at:
                is_update = True
                # 一般ユーザ更新用のデータ編集（更新前）
                before_user_id_list = []
                if target_project.main_sales_user_id:
                    before_user_id_list.append(target_project.main_sales_user_id)
                if target_project.main_customer_user_id:
                    before_user_id_list.append(target_project.main_customer_user_id)
                if target_project.main_supporter_user_id:
                    before_user_id_list.append(target_project.main_supporter_user_id)
                if target_project.customer_user_ids:
                    for customer_user_id in target_project.customer_user_ids:
                        before_user_id_list.append(customer_user_id)
                if target_project.supporter_user_ids:
                    for supporter_user_id in target_project.supporter_user_ids:
                        before_user_id_list.append(supporter_user_id)
                before_user_id_list = list(set(before_user_id_list))
                temp_before_user_project_map[target_project.id] = before_user_id_list

                row_change_log = ProjectService.edit_target_project(
                    target_project=target_project,
                    customer_id=customer_id,
                    salesforce_customer_id=salesforce_customer_id,
                    salesforce_update_at=salesforce_update_at,
                    name=name,
                    customer_name=customer_name,
                    service_type=service_type,
                    service_type_name=service_type_name,
                    create_new=create_new,
                    continued=continued,
                    main_sales_user_id=main_sales_user_id,
                    main_sales_user_name=main_sales_user_name,
                    contract_date=contract_date,
                    phase=phase,
                    customer_success=customer_success,
                    support_date_from=support_date_from,
                    support_date_to=support_date_to,
                    profit=GrossProfitAttribute(
                        monthly=profit.monthly,
                        quarterly=profit.quarterly,
                        half=profit.half,
                        year=profit.year,
                    ),
                    gross=GrossProfitAttribute(
                        monthly=gross.monthly,
                        quarterly=gross.quarterly,
                        half=gross.half,
                        year=gross.year,
                    ),
                    total_contract_time=total_contract_time,
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name=salesforce_main_customer.name,
                        email=salesforce_main_customer.email,
                        organization_name=salesforce_main_customer.organization_name,
                        job=salesforce_main_customer.job,
                    )
                    if salesforce_main_customer
                    else None,
                    supporter_organization_id=supporter_organization_id,
                    supporter_organization_name=supporter_organization_name,
                    salesforce_main_supporter_user_name=salesforce_main_supporter_user_name,
                    salesforce_supporter_user_names=salesforce_supporter_user_names,
                    salesforce_use_package=salesforce_use_package,
                    salesforce_via_pr=salesforce_via_pr,
                    update_id=current_user.id,
                    update_at=update_datetime,
                )

                # 一般ユーザ更新用のデータ編集（更新後）
                target_user_id_list = []
                if target_project.main_sales_user_id:
                    target_user_id_list.append(target_project.main_sales_user_id)
                if target_project.main_customer_user_id:
                    target_user_id_list.append(target_project.main_customer_user_id)
                if target_project.main_supporter_user_id:
                    target_user_id_list.append(target_project.main_supporter_user_id)
                if target_project.customer_user_ids:
                    for customer_user_id in target_project.customer_user_ids:
                        target_user_id_list.append(customer_user_id)
                if target_project.supporter_user_ids:
                    for supporter_user_id in target_project.supporter_user_ids:
                        target_user_id_list.append(supporter_user_id)
                target_user_id_list = list(set(target_user_id_list))
                temp_user_project_update_map[target_project.id] = target_user_id_list

                # アサインメール通知対象(宛先To)の編集
                temp_to_assign_mail_user_id_dict["project_id"] = target_project.id
                temp_to_assign_mail_user_id_dict["project_name"] = target_project.name
                user_id_list = [
                    target_project.main_sales_user_id,
                    target_project.main_supporter_user_id,
                ]
                if target_project.supporter_user_ids:
                    for supporter_user_id in target_project.supporter_user_ids:
                        user_id_list.append(supporter_user_id)
                user_id_list = list(set(user_id_list))
                temp_to_assign_mail_user_id_dict["user_id_list"] = user_id_list

        # レスポンス編集
        response_project_info = None
        if is_new or is_update:
            # 新規、更新の場合、レスポンス編集
            # 処理スキップした行はレスポンスに含めない
            response_project_info = ProjectService.get_import_response_project_info(
                ImportModeType.EXECUTE, row
            )
            response_project_info.customer_id = customer_id
            response_project_info.service_type = service_type
            response_project_info.create_new = (
                row[CsvColumn.CREATE_NEW] if row[CsvColumn.CREATE_NEW] else None
            )
            response_project_info.continued = continued
            response_project_info.profit = profit
            response_project_info.total_contract_time = total_contract_time
            response_project_info.salesforce_main_customer = salesforce_main_customer
            response_project_info.supporter_organization_id = supporter_organization_id
            response_project_info.gross = gross
            response_project_info.salesforce_use_package = salesforce_use_package
            response_project_info.salesforce_via_pr = salesforce_via_pr
            response_project_info.main_sales_user_id = main_sales_user_id
            response_project_info.phase = phase
            if is_new:
                # 新規のみ
                if main_supporter_user_name:
                    response_project_info.main_supporter_user_name = (
                        main_supporter_user_name
                    )
                if supporter_user_name_list:
                    response_project_info.supporter_users = [
                        UserName(name=x) for x in supporter_user_name_list
                    ]

        # 並列処理の排他制御
        with ProjectService.lock:
            if is_new:
                projects.append(response_project_info)
                project_batch.save(new_project)
                new_log_list.append(
                    {
                        "project_name": name,
                        "fo_project_detail_url": fo_site_url
                        + BoAppUrl.PROJECT_DETAIL.format(projectId=new_project.id),
                        "bo_project_detail_url": bo_site_url
                        + BoAppUrl.PROJECT_DETAIL.format(projectId=new_project.id),
                    }
                )
            if is_update:
                projects.append(response_project_info)
                project_batch.save(target_project)
                # メール/お知らせ通知用のログ編集（更新分）
                ProjectService.edit_import_change_column_log(
                    edit_log_list=edit_log_list,
                    project_name=name,
                    row_change_log=row_change_log,
                )

            user_project_update_map.update(temp_user_project_update_map)
            before_user_project_map.update(temp_before_user_project_map)
            to_mail_user_id_list.append(temp_to_mail_user_id_dict)
            to_assign_mail_user_id_list.append(temp_to_assign_mail_user_id_dict)

    @staticmethod
    def import_project(
        item: ImportProjectsRequest,
        current_user: AdminModel,
        authorization: HTTPAuthorizationCredentials,
    ) -> ImportProjectsResponse:
        """案件のCSVデータのエラーチェックまたは取り込みを行う.
        Args:
            item (ImportProjectsRequest): 登録内容
            current_user (Behavior, optional): 認証済みのユーザー
            authorization (HTTPAuthorizationCredentials): 認証情報
        Returns:
            ImportProjectsResponse: 登録結果
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
                logger.warning("ImportProjects not found. File not found.")
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="File not found.",
                )
            else:
                logger.error(e)
                raise e
        except UnicodeDecodeError:
            logger.warning("ImportProjects. cp932 codec can't decode.")
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail="cp932 codec can't decode.",
            )

        # 空ファイルチェック
        if not csv_data:
            logger.warning("ImportProjects. Illegal CSV format.")
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail="Illegal CSV format.",
            )
        # CSVファイルの件数上限チェック
        csv_count = 0
        # DictReaderはイテレータのため、件数チェック用に別に読み込む
        csv_count_check_dict_reader = csv.DictReader(csv_data.splitlines())
        for row in csv_count_check_dict_reader:
            csv_count += 1
        if csv_count > ImportFileLimitCount.LIMIT:
            logger.warning("ImportProjects. Over data lines.")
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail=f"Over {str(ImportFileLimitCount.LIMIT)} lines. CSV data contains {csv_count} lines.",
            )

        # CSV読込
        csv_dict_reader = csv.DictReader(csv_data.splitlines())
        # フォーマットチェック（ヘッダー項目の存在チェック）
        if not ProjectService.check_csv_format(csv_dict_reader):
            logger.warning("ImportProjects. Illegal CSV format.")
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail="Illegal CSV format.",
            )

        projects: List[ProjectInfoForImportProjects] = []
        result: ImportResultType = None

        # 取引先テーブル取得（全件）
        customer_list: List[CustomerModel] = list(CustomerModel.scan())
        # 汎用マスタ(サービス種別)を取得
        master_service_list: List[MasterSupporterOrganizationModel] = list(
            MasterSupporterOrganizationModel.data_type_value_index.query(
                hash_key=MasterDataType.MASTER_SERVICE_TYPE.value
            )
        )
        # 汎用マスタ(支援者組織情報)を取得
        master_supporter_org_list: List[MasterSupporterOrganizationModel] = list(
            MasterSupporterOrganizationModel.data_type_value_index.query(
                hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value
            )
        )
        # 一般ユーザ情報を取得
        user_list: List[UserModel] = list(
            UserModel.data_type_name_index.query(hash_key=DataType.USER)
        )

        if item.mode == ImportModeType.CHECK:
            # モード：check
            result = ImportResultType.OK
            for row in csv_dict_reader:
                # レスポンス編集
                # 以下はチェック後にセット
                # ・取引先ID
                # ・サービス区分
                # ・支援者組織ID（粗利メイン課）
                # ・新規・更新
                # ・前年度で契約し期をまたぐ商談
                # ・粗利
                # ・延べ契約時間（当年度内）
                # ・取引先責任者の４項目
                # ・売上
                # ・PKG利用
                # ・PR経由
                # ・商談所有者ID（営業担当者ID）
                # ・フェーズ
                project_info = ProjectService.get_import_response_project_info(
                    ImportModeType.CHECK, row
                )
                projects.append(project_info)
                project_info.error_message = []

                # 商談 ID（必須チェック）
                if not project_info.salesforce_opportunity_id:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_OPPORTUNITY_ID
                        )
                    )

                # 取引先 ID（必須チェック、存在チェック）
                if not project_info.salesforce_customer_id:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_CUSTOMER_ID
                        )
                    )

                customer_id = None
                customer_name = None
                for customer in customer_list:
                    if (
                        customer.salesforce_customer_id
                        == project_info.salesforce_customer_id
                    ):
                        customer_id = customer.id
                        customer_name = customer.name
                        break
                if not customer_id:
                    project_info.error_message.append(
                        Message.ImportCheckError.NOT_EXIST_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_CUSTOMER_ID
                        )
                    )

                else:
                    # レスポンスにcustomer_idをセット
                    project_info.customer_id = customer_id

                # 最終更新日時（必須チェック、書式チェック）
                if not project_info.salesforce_update_at:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_UPDATE_AT
                        )
                    )

                pattern_match = re.fullmatch(
                    r"\d{4}/\d{2}/\d{2} \d{1,2}:\d{2}",
                    project_info.salesforce_update_at,
                )
                if not pattern_match:
                    project_info.error_message.append(
                        Message.ImportCheckError.FORMAT_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_UPDATE_AT,
                            format_value="yyyy/mm/dd h:mm",
                        )
                    )

                try:
                    datetime.strptime(
                        project_info.salesforce_update_at, "%Y/%m/%d %H:%M"
                    )
                except Exception:
                    logger.warning(
                        "ImportProjects. Illegal salesforce_update_at format."
                    )
                    project_info.error_message.append(
                        Message.ImportCheckError.DATETIME_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_UPDATE_AT,
                        )
                    )

                # 商談名（必須チェック）
                if not project_info.name:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.NAME
                        )
                    )

                # 取引先名（必須チェック、存在チェック）
                if not project_info.customer_name:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.CUSTOMER_NAME
                        )
                    )

                if project_info.customer_name != customer_name:
                    project_info.error_message.append(
                        Message.ImportCheckError.NOT_EXIST_ERROR.format(
                            column_name=CsvColumn.CUSTOMER_NAME
                        )
                    )

                # サービス区分（必須チェック、列挙値チェック）
                if not project_info.service_type_name:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SERVICE_TYPE
                        )
                    )

                service_type_id = None
                service_type_name_no_prefix = ProjectService.remove_prefix_num(
                    project_info.service_type_name
                )
                for master_service in master_service_list:
                    if service_type_name_no_prefix == master_service.name:
                        service_type_id = master_service.id
                if not service_type_id:
                    project_info.error_message.append(
                        Message.ImportCheckError.NOT_EXIST_ERROR.format(
                            column_name=CsvColumn.SERVICE_TYPE
                        )
                    )

                else:
                    # レスポンスにservice_typeをセット
                    project_info.service_type = service_type_id

                # (アンケート)粗利メイン課（存在チェック）
                if project_info.supporter_organization_name:
                    supporter_organization_id = None
                    for master_supporter_org in master_supporter_org_list:
                        if (
                            project_info.supporter_organization_name
                            == master_supporter_org.value
                        ):
                            supporter_organization_id = master_supporter_org.id
                    if not supporter_organization_id:
                        project_info.error_message.append(
                            Message.ImportCheckError.NOT_EXIST_ERROR.format(
                                column_name=CsvColumn.SUPPORTER_ORGANIZATION_SHORT_NAME
                            )
                        )

                    else:
                        # レスポンスにsupporter_organization_idをセット
                        project_info.supporter_organization_id = (
                            supporter_organization_id
                        )

                # PKG利用（必須チェック、真理値変換可否チェック）
                if not row.get(CsvColumn.SALESFORCE_USE_PACKAGE):
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_USE_PACKAGE
                        )
                    )

                if not Utils.is_bool(row.get(CsvColumn.SALESFORCE_USE_PACKAGE)):
                    project_info.error_message.append(
                        Message.ImportCheckError.VALUE_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_USE_PACKAGE,
                            value="真理値（1または0）",
                        )
                    )

                else:
                    # レスポンスにsalesforce_use_packageをセット
                    project_info.salesforce_use_package = strtobool(
                        row.get(CsvColumn.SALESFORCE_USE_PACKAGE)
                    )

                # 新規・更新（真理値変換可否チェック）
                if row.get(CsvColumn.CREATE_NEW):
                    if not ProjectService.is_bool_create_new(
                        row.get(CsvColumn.CREATE_NEW)
                    ):
                        project_info.error_message.append(
                            Message.ImportCheckError.VALUE_ERROR.format(
                                column_name=CsvColumn.CREATE_NEW,
                                value=f"「{ProjectCreateNewType.UPDATE.value}」または「{ProjectCreateNewType.NEW.value}」",
                            )
                        )

                    else:
                        # レスポンスにcreate_newをセット
                        project_info.create_new = row.get(CsvColumn.CREATE_NEW)

                # 前年度で契約し期をまたぐ商談（必須チェック、真理値変換可否チェック）
                if not row.get(CsvColumn.CONTINUED):
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.CONTINUED
                        )
                    )

                if not Utils.is_bool(row.get(CsvColumn.CONTINUED)):
                    project_info.error_message.append(
                        Message.ImportCheckError.VALUE_ERROR.format(
                            column_name=CsvColumn.CONTINUED,
                            value="真理値（1または0）",
                        )
                    )

                else:
                    # レスポンスにcontinuedをセット
                    project_info.continued = strtobool(row.get(CsvColumn.CONTINUED))

                # PR経由（必須チェック、真理値変換可否チェック）
                if not row.get(CsvColumn.SALESFORCE_VIA_PR):
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_VIA_PR
                        )
                    )

                if not Utils.is_bool(row.get(CsvColumn.SALESFORCE_VIA_PR)):
                    project_info.error_message.append(
                        Message.ImportCheckError.VALUE_ERROR.format(
                            column_name=CsvColumn.SALESFORCE_VIA_PR,
                            value="真理値（1または0）",
                        )
                    )

                else:
                    # レスポンスにsalesforce_via_prをセット
                    project_info.salesforce_via_pr = strtobool(
                        row.get(CsvColumn.SALESFORCE_VIA_PR)
                    )

                # 取引先責任者
                if (
                    row.get(CsvColumn.SALESFORCE_MAIN_CUSTOMER_NAME)
                    or row.get(CsvColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL)
                    or row.get(CsvColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME)
                    or row.get(CsvColumn.SALESFORCE_MAIN_CUSTOMER_JOB)
                ):
                    # レスポンスにsalesforce_main_customerをセット。但し、メールアドレスのみチェック後にセット。
                    project_info.salesforce_main_customer = SalesforceMainCustomer(
                        name=row.get(CsvColumn.SALESFORCE_MAIN_CUSTOMER_NAME),
                        organization_name=row.get(
                            CsvColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME
                        ),
                        job=row.get(CsvColumn.SALESFORCE_MAIN_CUSTOMER_JOB),
                    )
                # 取引先責任者メールアドレス（書式チェック）
                if row.get(CsvColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL):
                    email_pattern = r"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"
                    if not re.fullmatch(
                        email_pattern, row.get(CsvColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL)
                    ):
                        project_info.error_message.append(
                            Message.ImportCheckError.EMAIL_FORMAT_ERROR.format(
                                column_name=CsvColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL
                            )
                        )

                    else:
                        # レスポンスにsalesforce_main_customerのemailをセット
                        project_info.salesforce_main_customer.email = row.get(
                            CsvColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL
                        )

                # 商談 所有者（必須チェック、存在チェック、有効化チェック）
                if not project_info.main_sales_user_name:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.MAIN_SALES_USER_NAME
                        )
                    )

                main_sales_user_id = None
                for user in user_list:
                    if project_info.main_sales_user_name == user.name and (
                        user.role == UserRoleType.SALES.key or user.role == UserRoleType.SALES_MGR.key
                    ):
                        main_sales_user_id = user.id

                        if user.disabled:
                            project_info.error_message.append(
                                Message.ImportCheckError.DISABLED_ERROR.format(
                                    column_name=CsvColumn.MAIN_SALES_USER_NAME
                                )
                            )
                        else:
                            # レスポンスにmain_sales_user_idをセット
                            project_info.main_sales_user_id = main_sales_user_id
                            break

                if not main_sales_user_id:
                    project_info.error_message.append(
                        Message.ImportCheckError.NOT_EXIST_ERROR.format(
                            column_name=CsvColumn.MAIN_SALES_USER_NAME
                        )
                    )

                # 完了予定日（必須チェック、書式チェック）
                if not project_info.contract_date:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.CONTRACT_DATE
                        )
                    )

                if not re.fullmatch(
                    r"\d{4}/\d{2}/\d{2}",
                    project_info.contract_date,
                ):
                    project_info.error_message.append(
                        Message.ImportCheckError.FORMAT_ERROR.format(
                            column_name=CsvColumn.CONTRACT_DATE,
                            format_value="yyyy/mm/dd",
                        )
                    )

                try:
                    datetime.strptime(project_info.contract_date, "%Y/%m/%d")
                except Exception:
                    logger.warning("ImportProjects. Illegal contract_date format.")
                    project_info.error_message.append(
                        Message.ImportCheckError.DATE_ERROR.format(
                            column_name=CsvColumn.CONTRACT_DATE,
                        )
                    )

                # フェーズ（必須チェック、列挙値チェック）
                if not row.get(CsvColumn.PHASE):
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.PHASE
                        )
                    )

                if not ProjectService.check_enum_project_phase_type(
                    row.get(CsvColumn.PHASE)
                ):
                    project_info.error_message.append(
                        Message.ImportCheckError.VALUE_ERROR.format(
                            column_name=CsvColumn.PHASE,
                            value="規定値",
                        )
                    )

                else:
                    # レスポンスにphaseをセット
                    project_info.phase = row.get(CsvColumn.PHASE)

                # 支援開始日（必須チェック、書式チェック）
                if not project_info.support_date_from:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SUPPORT_DATE_FROM
                        )
                    )

                if not re.fullmatch(
                    r"\d{4}/\d{2}/\d{2}",
                    project_info.support_date_from,
                ):
                    project_info.error_message.append(
                        Message.ImportCheckError.FORMAT_ERROR.format(
                            column_name=CsvColumn.SUPPORT_DATE_FROM,
                            format_value="yyyy/mm/dd",
                        )
                    )

                try:
                    datetime.strptime(project_info.support_date_from, "%Y/%m/%d")
                except Exception:
                    logger.warning("ImportProjects. Illegal support_date_from format.")
                    project_info.error_message.append(
                        Message.ImportCheckError.DATE_ERROR.format(
                            column_name=CsvColumn.SUPPORT_DATE_FROM,
                        )
                    )

                # 支援終了日（必須チェック、書式チェック）
                if not project_info.support_date_to:
                    project_info.error_message.append(
                        Message.ImportCheckError.REQUIRED_ERROR.format(
                            column_name=CsvColumn.SUPPORT_DATE_TO
                        )
                    )

                if not re.fullmatch(
                    r"\d{4}/\d{2}/\d{2}",
                    project_info.support_date_to,
                ):
                    project_info.error_message.append(
                        Message.ImportCheckError.FORMAT_ERROR.format(
                            column_name=CsvColumn.SUPPORT_DATE_TO,
                            format_value="yyyy/mm/dd",
                        )
                    )

                try:
                    datetime.strptime(project_info.support_date_to, "%Y/%m/%d")
                except Exception:
                    logger.warning("ImportProjects. Illegal support_date_to format.")
                    project_info.error_message.append(
                        Message.ImportCheckError.DATE_ERROR.format(
                            column_name=CsvColumn.SUPPORT_DATE_TO,
                        )
                    )

                # 粗利（必須チェック、数値チェック）
                # 売上（必須チェック、数値チェック）
                # 必須＋数値チェック: Q毎,H毎,FY
                required_int_check_keys = (
                    ProjectService.profit_quarter_keys
                    + ProjectService.profit_half_keys
                    + [CsvColumn.PROFIT_YEAR]
                    + ProjectService.gross_quarter_keys
                    + ProjectService.gross_half_keys
                    + [CsvColumn.GROSS_YEAR]
                )
                # 任意＋数値チェック: 月毎
                optional_int_check_keys = (
                    ProjectService.profit_month_keys + ProjectService.gross_month_keys
                )
                # チェック対象: ALL
                all_check_keys = required_int_check_keys + optional_int_check_keys

                for check_key in all_check_keys:
                    if check_key in required_int_check_keys:
                        if not row.get(check_key):
                            project_info.error_message.append(
                                Message.ImportCheckError.REQUIRED_ERROR.format(
                                    column_name=check_key
                                )
                            )

                    if row.get(check_key):
                        if not Utils.is_int(row.get(check_key)):
                            project_info.error_message.append(
                                Message.ImportCheckError.VALUE_ERROR.format(
                                    column_name=check_key,
                                    value="数値",
                                )
                            )

                if not project_info.error_message:
                    # リクエストのprofit、grossをセット
                    project_info.profit = ProjectService.edit_profit(row)
                    project_info.gross = ProjectService.edit_gross(row)

                # 延べ契約時間（数値チェック）
                if row.get(CsvColumn.TOTAL_CONTRACT_TIME):
                    if not Utils.is_float(row.get(CsvColumn.TOTAL_CONTRACT_TIME)):
                        project_info.error_message.append(
                            Message.ImportCheckError.VALUE_ERROR.format(
                                column_name=CsvColumn.TOTAL_CONTRACT_TIME,
                                value="数値",
                            )
                        )
                    else:
                        # レスポンスにtotal_contract_timeをセット
                        project_info.total_contract_time = float(
                            row.get(CsvColumn.TOTAL_CONTRACT_TIME)
                        )

            for project in projects:
                if project.error_message:
                    result = ImportResultType.NG
                    continue

            # チェックOKの場合：登録されるデータ(重複レコード削除済みデータ)
            # チェックNGの場合：CSVファイルの全データ
            if result == ImportResultType.OK:
                # 重複チェック用の一時格納用配列
                duplicate_checked_projects: List[ProjectInfoForImportProjects] = []

                for project in projects:
                    # 重複要素の検索
                    duplicate_record = [
                        checked_project
                        for checked_project in duplicate_checked_projects
                        if project.salesforce_opportunity_id
                        == checked_project.salesforce_opportunity_id
                    ]

                    # 取引先IDの重複がなければ、そのまま新配列に追加
                    if not duplicate_record:
                        duplicate_checked_projects.append(project)
                        continue

                    # 重複かつ更新日が古いものは、対象に登録しない
                    if datetime.strptime(
                        project.salesforce_update_at, "%Y/%m/%d %H:%M"
                    ) < datetime.strptime(
                        duplicate_record[0].salesforce_update_at, "%Y/%m/%d %H:%M"
                    ):
                        continue

                    # 更新日が同じ、または新しい場合、既存の要素削除&新規要素を追加
                    duplicate_checked_projects.remove(duplicate_record[0])
                    duplicate_checked_projects.append(project)
                    continue

                # 重複レコードの削除が済んだ配列を、改めてprojectsとして利用
                projects = duplicate_checked_projects

        elif item.mode == ImportModeType.EXECUTE:
            # モード：execute
            result = ImportResultType.DONE
            # 取引先情報取得（全件）
            project_result_list: List[ProjectModel] = list(ProjectModel.scan())
            # DB取得結果から辞書を作成
            #  key: salesforce_opportunity_id, value: CustomerModel
            sf_opportunity_id_map: dict[str, CustomerModel] = {
                p.salesforce_opportunity_id: p for p in project_result_list
            }

            # 一般ユーザの参加案件IDを更新するユーザの辞書
            #  key: project_id(案件ID), value: user_id(ユーザID)リストの辞書
            user_project_update_map: dict[str, List[str]] = {}
            # 更新前の一般ユーザの参加案件IDの辞書
            #  key: project_id(案件ID), value: user_id(ユーザID)リストの辞書
            before_user_project_map: dict[str, List[str]] = {}

            # 案件（参加案件ID）毎のユーザの除外リストと追加リストを含む辞書
            # key: 案件ID
            # value: ユーザID (dict)
            #    exclude: 除外リスト（List）
            #    add: 追加リスト（List）
            project_divided_user_id_map: dict[str, dict[str, List[str]]] = {}

            # 「Salesforce案件情報インポート通知」メールの宛先TOに入れるメールアドレス
            to_import_email_list = []
            # お知らせ（Salesforce案件情報インポート通知）を通知するユーザID
            bo_import_notification_user_id_list = []

            # 「案件情報新規登録通知」メールの宛先TOに入れるユーザID
            # （案件にアサインされた支援者、事業者責任者、営業担当者）
            # dict(key: project_id, project_name, user_id_list)のリスト
            to_mail_user_id_list: List[dict] = []
            # 「案件情報新規登録通知」メールの宛先TO, CCに入れるメールアドレス
            # dict(key: project_id, project_name, email_list)のリスト
            to_email_list: List[dict] = []
            cc_email = []
            # お知らせ（案件情報新規登録通知）を通知するユーザID
            # dict(key: project_id, project_name, user_id_list)のリスト
            fo_notification_user_id_list: List[dict] = []
            bo_notification_user_id = []

            # 「案件アサイン通知」メールの宛先TOに入れるユーザID
            # （支援者or支援者責任者or事業者責任者or営業担当者orお客様メンバー）
            # dict(key: project_id, project_name, user_id_list)のリスト
            to_assign_mail_user_id_list: List[dict] = []
            # 「案件アサイン通知」メールの宛先TOに入れるメールアドレス
            # dict(key: project_id, project_name, email_list)のリスト
            to_assign_email_list: List[dict] = []
            # お知らせ（アサイン）を通知するユーザID
            # dict(key: project_id, project_name, user_id_list)のリスト
            fo_assign_notification_user_id_list: List[dict] = []

            # 新規登録の案件名、案件情報詳細URL(FO)、案件情報詳細URL(BO)のリスト
            #   e.g. {
            #             "project_name": "AAA案件",
            #             "fo_project_detail_url": "https://dev.partner-portal.inhouse-sony-startup-acceleration-program.com/project/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            #             "bo_project_detail_url": "https://bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/project/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            #         }
            new_log_list: List[str] = []

            # 更新分（案件名、更新項目名、更新値）のリスト
            # e.g. [
            #         {
            #             "project_name": "CCC案件",
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
                with ProjectModel.batch_write() as project_batch:
                    with ThreadPoolExecutor(
                        max_workers=ThreadPoolMaxWorkers.IMPORT_PROJECTS
                    ) as executor:
                        duplicate_checked_records = []  # 重複チェック用の一時格納用配列

                        for row in csv_dict_reader:
                            # 重複要素の検索
                            duplicate_record = [
                                checked_project
                                for checked_project in duplicate_checked_records
                                if row[CsvColumn.SALESFORCE_OPPORTUNITY_ID]
                                == checked_project[CsvColumn.SALESFORCE_OPPORTUNITY_ID]
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

                        # 重複レコードの削除が済んだ配列を、改めてrecordsとして利用
                        records = duplicate_checked_records

                        features = []
                        for row in records:
                            features.append(
                                executor.submit(
                                    ProjectService.import_execute_parallel,
                                    row=row,
                                    projects=projects,
                                    sf_opportunity_id_map=sf_opportunity_id_map,
                                    customer_list=customer_list,
                                    master_service_list=master_service_list,
                                    master_supporter_org_list=master_supporter_org_list,
                                    user_list=user_list,
                                    project_batch=project_batch,
                                    current_user=current_user,
                                    update_datetime=update_datetime,
                                    user_project_update_map=user_project_update_map,
                                    before_user_project_map=before_user_project_map,
                                    to_mail_user_id_list=to_mail_user_id_list,
                                    to_assign_mail_user_id_list=to_assign_mail_user_id_list,
                                    edit_log_list=edit_log_list,
                                    new_log_list=new_log_list,
                                )
                            )

                        # 結果（Exception）の取得
                        for feature in features:
                            feature.result()

                        # salesforce_opportunity_id一覧
                        salesforce_opportunity_ids = [
                            p[ImportFileProjectColumnName.SALESFORCE_OPPORTUNITY_ID]
                            for p in duplicate_checked_records
                        ]

                        # pf案件公開情報の更新
                        platform_api_operator = PlatformApiOperator(
                            jwt_token=authorization.jwt_token
                        )

                        put_project_publication_pf_project_params = {
                            "salesforceOpportunityIds": salesforce_opportunity_ids,
                            "importCsv": True,
                        }

                        (
                            status_code,
                            body,
                        ) = platform_api_operator.put_project_publication(
                            params=put_project_publication_pf_project_params
                        )

                        logger.info(
                            f"platform putProjectPublication statusCode: {status_code}"
                        )
                        if status_code != 200:
                            logger.info(
                                "platform putProjectPublication response:" + body
                            )
                            raise HTTPException(
                                status_code=status_code,
                                detail=body["detail"]["message"],
                            )

                # ユーザIDが指定された項目から更新前後で除外/追加されたユーザIDのリストを取得
                # 案件（参加案件ID）毎のユーザの除外リストと追加リストの辞書を生成
                for project_id, target_user_id_list in user_project_update_map.items():
                    divided_user_id_dict = ProjectService.divide_user_id(
                        before_user_project_map.get(project_id, []), target_user_id_list
                    )
                    project_divided_user_id_map[project_id] = divided_user_id_dict

                # ユーザ更新用のdictionaryを作成
                # 作成元:
                #   key: 案件ID
                #   value: ユーザID (dict)
                #      exclude: 除外リスト（List）
                #      add: 追加リスト（List）
                # 作成対象:
                #   key: ユーザID
                #   value: 案件ID (dict)
                #      exclude: 除外リスト（List）
                #      add: 追加リスト（List）
                user_devided_project_id_map: dict[str, dict[str, List[str]]] = {}
                for (
                    project_id,
                    divided_user_id_dict,
                ) in project_divided_user_id_map.items():
                    for add_user_id in divided_user_id_dict["add"]:
                        devided_project_dict = user_devided_project_id_map.get(
                            add_user_id
                        )
                        add_project_list = []
                        if devided_project_dict is None:
                            user_devided_project_id_map[add_user_id] = {}
                        else:
                            add_project_list = devided_project_dict.get("add")
                        add_project_list.append(project_id)
                        user_devided_project_id_map[add_user_id][
                            "add"
                        ] = add_project_list
                    for exclude_user_id in divided_user_id_dict["exclude"]:
                        devided_project_dict = user_devided_project_id_map.get(
                            exclude_user_id
                        )
                        exclude_project_list = []
                        if devided_project_dict is None:
                            user_devided_project_id_map[exclude_user_id] = {}
                        else:
                            exclude_project_list = devided_project_dict.get("exclude")
                        exclude_project_list.append(project_id)
                        user_devided_project_id_map[exclude_user_id][
                            "exclude"
                        ] = exclude_project_list

                # 一般ユーザ情報を取得
                user_table_info = ProjectService.get_user_info(
                    user_devided_project_id_map.keys()
                )

                # メール/お知らせ対象のユーザ情報取得
                for (
                    project_id,
                    divided_user_id_dict,
                ) in project_divided_user_id_map.items():

                    target_user_info: dict = {}
                    for to_mail_user_info in to_mail_user_id_list:
                        if to_mail_user_info:
                            if to_mail_user_info["project_id"] == project_id:
                                target_user_info = to_mail_user_info
                    target_assign_user_info: dict = {}
                    for to_assign_mail_user_info in to_assign_mail_user_id_list:
                        if to_assign_mail_user_info:
                            if to_assign_mail_user_info["project_id"] == project_id:
                                target_assign_user_info = to_assign_mail_user_info

                    temp_email = []
                    temp_user_id = []
                    temp_assign_email = []
                    temp_assign_user_id = []

                    for user in user_table_info:
                        if user.id in divided_user_id_dict["add"]:
                            # 新規/更新で新たにアサインされたユーザのみ対象
                            if target_user_info:
                                if user.id in target_user_info["user_id_list"]:
                                    temp_email.append(user.email)
                                    temp_user_id.append(user.id)
                            if target_assign_user_info:
                                if user.id in target_assign_user_info["user_id_list"]:
                                    temp_assign_email.append(user.email)
                                    temp_assign_user_id.append(user.id)

                    # 新規登録通知
                    if temp_email:
                        temp_to_email_dict = {
                            "project_id": target_user_info["project_id"],
                            "project_name": target_user_info["project_name"],
                            "email_list": temp_email,
                        }
                        to_email_list.append(temp_to_email_dict)
                    if temp_user_id:
                        temp_fo_user_id_dict = {
                            "project_id": target_user_info["project_id"],
                            "project_name": target_user_info["project_name"],
                            "user_id_list": temp_user_id,
                        }
                        fo_notification_user_id_list.append(temp_fo_user_id_dict)
                    # アサイン通知
                    if temp_assign_email:
                        temp_to_assign_email_dict = {
                            "project_id": target_assign_user_info["project_id"],
                            "project_name": target_assign_user_info["project_name"],
                            "email_list": temp_assign_email,
                        }
                        to_assign_email_list.append(temp_to_assign_email_dict)
                    if temp_assign_user_id:
                        temp_fo_assign_user_id_dict = {
                            "project_id": target_assign_user_info["project_id"],
                            "project_name": target_assign_user_info["project_name"],
                            "user_id_list": temp_assign_user_id,
                        }
                        fo_assign_notification_user_id_list.append(
                            temp_fo_assign_user_id_dict
                        )

                # 一般ユーザ更新
                update_datetime = datetime.now()
                with UserModel.batch_write() as user_batch:
                    for user in user_table_info:
                        divided_project_id_dict = user_devided_project_id_map.get(
                            user.id
                        )
                        if divided_project_id_dict:
                            add_project_id_list = divided_project_id_dict.get("add")
                            exclude_project_id_list = divided_project_id_dict.get(
                                "exclude"
                            )

                            if add_project_id_list:
                                # 参加案件を追加
                                if user.project_ids is not None:
                                    user.project_ids.update(add_project_id_list)
                                else:
                                    user.project_ids = set(add_project_id_list)
                            if exclude_project_id_list:
                                # 参加案件を削除
                                if user.project_ids is not None:
                                    for project_id in exclude_project_id_list:
                                        user.project_ids.discard(project_id)
                            user.update_id = current_user.id
                            user.update_at = update_datetime
                            user.version += 1
                            user_batch.save(user)

                fo_site_url = get_app_settings().fo_site_url
                bo_site_url = get_app_settings().bo_site_url

                # メール・お知らせ通知
                if new_log_list or edit_log_list:
                    # SQS send_message_batchのEntries
                    send_message_entries: List[dict] = []
                    # 新規登録または更新がある場合のみ通知
                    payload = {
                        "add_info_list": new_log_list,
                        "update_info_list": edit_log_list,
                    }

                    # システム管理者、工数調査事務局、アンケート事務局の情報
                    admin_info_for_mail: List[dict] = []
                    admin_filter_condition = AdminModel.disabled == False  # NOQA
                    for admin in AdminModel.scan(filter_condition=admin_filter_condition):
                        if (
                            UserRoleType.SYSTEM_ADMIN.key in admin.roles
                            or UserRoleType.MAN_HOUR_OPS.key in admin.roles
                            or UserRoleType.SURVEY_OPS.key in admin.roles
                        ):
                            temp_info: dict = {}
                            temp_info["id"] = admin.id
                            temp_info["email"] = admin.email
                            admin_info_for_mail.append(temp_info)

                    # 「Salesforce案件情報インポート通知」メール
                    # 通知メールのTOに設定する管理ユーザ
                    for admin in admin_info_for_mail:
                        to_import_email_list.append(admin["email"])
                        bo_import_notification_user_id_list.append(admin["id"])
                    to_import_email_list = list(set(to_import_email_list))
                    message_body = {
                        "template": MailType.PROJECT_IMPORT_COMPLETED,
                        "to": to_import_email_list,
                        "cc": [],
                        "payload": payload,
                    }
                    sqs_message_body = json.dumps(message_body)
                    send_message_entries.append(
                        {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
                    )

                    # 「案件情報新規登録通知」メール
                    # 通知メールのCCに設定する管理ユーザ
                    for admin in admin_info_for_mail:
                        cc_email.append(admin["email"])
                        bo_notification_user_id.append(admin["id"])
                    cc_email = list(set(cc_email))
                    for email_info in to_email_list:
                        fo_project_detail_url = (
                            fo_site_url
                            + FoAppUrl.PROJECT_DETAIL.format(
                                projectId=email_info["project_id"]
                            )
                        )
                        bo_project_detail_url = (
                            bo_site_url
                            + BoAppUrl.PROJECT_DETAIL.format(
                                projectId=email_info["project_id"]
                            )
                        )
                        to_addr_list = list(set(email_info["email_list"]))
                        message_body = {
                            "template": MailType.PROJECT_REGISTRATION_COMPLETED,
                            "to": to_addr_list,
                            "cc": cc_email,
                            "payload": {
                                "project_name": email_info["project_name"],
                                "fo_project_detail_url": fo_project_detail_url,
                                "bo_project_detail_url": bo_project_detail_url,
                            },
                        }
                        sqs_message_body = json.dumps(message_body)
                        send_message_entries.append(
                            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
                        )

                    # 「案件アサイン通知」メール
                    for assign_email_info in to_assign_email_list:
                        fo_karte_list_url = fo_site_url + FoAppUrl.KARTE_LIST.format(
                            projectId=assign_email_info["project_id"]
                        )
                        to_addr_list = list(set(assign_email_info["email_list"]))
                        message_body = {
                            "template": MailType.PROJECT_ASSIGN,
                            "to": to_addr_list,
                            "cc": [],
                            "payload": {
                                "project_name": assign_email_info["project_name"],
                                "mail_address_to": " ".join(to_addr_list),
                                "fo_karte_list_url": fo_karte_list_url,
                            },
                        }
                        sqs_message_body = json.dumps(message_body)
                        send_message_entries.append(
                            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
                        )

                    # メール送信
                    ProjectService.send_mail_batch(entries=send_message_entries)

                    notification_notice_at = datetime.now()
                    with NotificationModel.batch_write() as notification_batch:
                        # お知らせ「Salesforce案件情報インポート通知」
                        # BO
                        bo_import_notification_user_id_list = list(
                            set(bo_import_notification_user_id_list)
                        )
                        summary, message = NotificationService.edit_message(
                            notification_type=NotificationType.SALESFORCE_PROJECT_IMPORT,
                            message_param=payload,
                        )
                        for user_id in bo_import_notification_user_id_list:
                            notification = NotificationModel(
                                id=str(uuid.uuid4()),
                                user_id=user_id,
                                summary=summary,
                                url="",
                                message=message,
                                confirmed=False,
                                noticed_at=notification_notice_at,
                                ttl=NotificationService.get_unix_time_after_90_days(
                                    notification_notice_at
                                ),
                                create_id=current_user.id,
                                create_at=notification_notice_at,
                                update_id=current_user.id,
                                update_at=notification_notice_at,
                            )
                            notification_batch.save(notification)

                        # お知らせ「案件情報新規登録通知」
                        for fo_notification_info in fo_notification_user_id_list:
                            fo_project_detail_url = (
                                fo_site_url
                                + FoAppUrl.PROJECT_DETAIL.format(
                                    projectId=fo_notification_info["project_id"]
                                )
                            )
                            bo_project_detail_url = (
                                bo_site_url
                                + BoAppUrl.PROJECT_DETAIL.format(
                                    projectId=fo_notification_info["project_id"]
                                )
                            )
                            project_name = fo_notification_info["project_name"]
                            user_id_list = list(
                                set(fo_notification_info["user_id_list"])
                            )
                            summary, message = NotificationService.edit_message(
                                notification_type=NotificationType.PROJECT_REGISTRATION,
                                message_param={"project_name": project_name},
                            )
                            for user_id in user_id_list:
                                notification = NotificationModel(
                                    id=str(uuid.uuid4()),
                                    user_id=user_id,
                                    summary=summary,
                                    url=fo_project_detail_url,
                                    message=message,
                                    confirmed=False,
                                    noticed_at=notification_notice_at,
                                    ttl=NotificationService.get_unix_time_after_90_days(
                                        notification_notice_at
                                    ),
                                    create_id=current_user.id,
                                    create_at=notification_notice_at,
                                    update_id=current_user.id,
                                    update_at=notification_notice_at,
                                )
                                notification_batch.save(notification)
                            # BO
                            user_id_list = list(set(bo_notification_user_id))
                            summary, message = NotificationService.edit_message(
                                notification_type=NotificationType.PROJECT_REGISTRATION,
                                message_param={"project_name": project_name},
                            )
                            for user_id in user_id_list:
                                notification = NotificationModel(
                                    id=str(uuid.uuid4()),
                                    user_id=user_id,
                                    summary=summary,
                                    url=bo_project_detail_url,
                                    message=message,
                                    confirmed=False,
                                    noticed_at=notification_notice_at,
                                    ttl=NotificationService.get_unix_time_after_90_days(
                                        notification_notice_at
                                    ),
                                    create_id=current_user.id,
                                    create_at=notification_notice_at,
                                    update_id=current_user.id,
                                    update_at=notification_notice_at,
                                )
                                notification_batch.save(notification)

                        # お知らせ「案件アサイン通知」
                        # FO
                        for (
                            fo_assign_notification_info
                        ) in fo_assign_notification_user_id_list:
                            fo_karte_list_url = (
                                fo_site_url
                                + FoAppUrl.KARTE_LIST.format(
                                    projectId=fo_assign_notification_info["project_id"]
                                )
                            )
                            user_id_list = list(
                                set(fo_assign_notification_info["user_id_list"])
                            )
                            summary, message = NotificationService.edit_message(
                                notification_type=NotificationType.PROJECT_ASSIGN,
                                message_param={
                                    "project_name": fo_assign_notification_info[
                                        "project_name"
                                    ]
                                },
                            )
                            for user_id in user_id_list:
                                notification = NotificationModel(
                                    id=str(uuid.uuid4()),
                                    user_id=user_id,
                                    summary=summary,
                                    url=fo_karte_list_url,
                                    message=message,
                                    confirmed=False,
                                    noticed_at=notification_notice_at,
                                    ttl=NotificationService.get_unix_time_after_90_days(
                                        notification_notice_at
                                    ),
                                    create_id=current_user.id,
                                    create_at=notification_notice_at,
                                    update_id=current_user.id,
                                    update_at=notification_notice_at,
                                )
                                notification_batch.save(notification)

            except Exception:
                result = ImportResultType.ERROR
                projects = []
                logger.exception(Message.ImportExecuteError.EXECUTE_ERROR)

        return ImportProjectsResponse(mode=item.mode, result=result, projects=projects)
