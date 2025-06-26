import copy
import json
from datetime import datetime
from typing import List, Set, Union

from fastapi import HTTPException
from fastapi import status
from fastapi import status as api_status
from fastapi.security import HTTPAuthorizationCredentials
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.update import Action

from app.auth.auth import AuthUser
from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.customer import CustomerModel
from app.models.master import MasterSupporterOrganizationModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
    UpdateAttributesSubAttribute,
    UpdateHistoryAttribute,
)
from app.models.user import UserModel
from app.resources.const import (
    DataType,
    FoAppUrl,
    GetProjectsSortType,
    MailType,
    MasterDataType,
    NotificationType,
)
from app.resources.const import ProjectColumnNameForUpdateLog as ProjectColumn
from app.resources.const import SuggestProjectsSortType, UserRoleType
from app.schemas.base import OKResponse
from app.schemas.project import (
    GetProjectByIdResponse,
    GetProjectsResponse,
    Profit,
    ProfitFy,
    ProjectInfoForGetProjects,
    SalesforceMainCustomer,
    SuggestProjectsResponse,
    UpdateProjectByIdRequest,
    UserId,
    UserIdName,
)
from app.service.common_service.pagination import EmptyPage, Paginator
from app.service.common_service.user_info import get_update_user_name
from app.service.notification_service import NotificationService
from app.utils.aws.sqs import SqsHelper
from app.utils.platform import PlatformApiOperator

logger = CustomLogger.get_logger()


class ProjectService:
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

        logger.warning(f"get_user_name method bad request. Not exist user_id: {id}")
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
            logger.warning(
                f"get_service_name method bad request. Not exist service_type: {service_type}"
            )
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail=[{"loc": ["body", "serviceType"], "msg": "Not exist."}],
            )
        return master.name

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
                f"get_supporter_organization_name method bad request. supporter_organization_id: {supporter_organization_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail=[
                    {"loc": ["body", "supporterOrganizationId"], "msg": "Not exist."}
                ],
            )
        return master.value

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
                logger.warning("get_user_id_name_class_list method bad request.")
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail=[
                        {"loc": ["body", column_name_for_log], "msg": "Not exist."}
                    ],
                )
            user_id_name_list.append(UserIdName(id=user_id.id, name=name))

        return user_id_name_list

    @staticmethod
    def is_visible_project(
        current_user: UserModel,
        role: str,
        project: ProjectModel,
        user_list: List[UserModel],
    ) -> bool:
        """案件情報がアクセス可能か判定.
            1.営業責任者、事業者責任者、支援者責任者
              制限なし（アクセス可）
            2.支援者、営業担当者
              所属していない非公開案件：アクセス不可
              上記以外：アクセス可
            3.顧客
              自身の案件：アクセス可
              上記以外：アクセス不可
        Args:
            current_user(UserModel): ログインユーザー
            role (str): ロール
            project (ProjectModel)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        if role in [
            UserRoleType.SALES_MGR.key,
            UserRoleType.BUSINESS_MGR.key,
            UserRoleType.SUPPORTER_MGR.key,
        ]:
            return True

        elif role == UserRoleType.SUPPORTER.key:
            if (
                project.main_supporter_user_id != current_user.id
                and (
                    not project.supporter_user_ids
                    or (current_user.id not in project.supporter_user_ids)
                )
                and project.is_secret
            ):
                # 所属していない非公開案件
                return False

            return True

        elif role == UserRoleType.SALES.key:
            if project.main_sales_user_id != current_user.id and project.is_secret:
                # 所属していない非公開案件
                return False

            return True

        elif role == UserRoleType.CUSTOMER.key:
            user: UserModel = None
            for user_model in user_list:
                if user_model.id == current_user.id:
                    user = user_model

            if user.project_ids:
                if project.id in user.project_ids:
                    # 参加している案件
                    return True

                # 参加していない案件
                return False

        return False

    @staticmethod
    def is_visible_project_for_suggest(
        current_user: UserModel, project: ProjectModel
    ) -> bool:
        """案件情報がアクセス可能か判定.
            1.営業責任者、事業者責任者
              制限なし（アクセス可）
            2.支援者
              担当案件
            3.支援者責任者
              担当案件
              所属課案件
              その他公開案件
            4.営業担当者
              担当案件
              その他公開案件
            ※顧客
              呼出し不可(API呼出権限で制御)
        Args:
            current_user (UserModel): ログインユーザー
            project (ProjectModel)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        if current_user.role in [
            UserRoleType.SALES_MGR.key,
            UserRoleType.BUSINESS_MGR.key,
        ]:
            return True

        elif current_user.role == UserRoleType.SUPPORTER.key:
            if current_user.project_ids and project.id in current_user.project_ids:
                return True

        elif current_user.role == UserRoleType.SUPPORTER_MGR.key:
            if current_user.project_ids and project.id in current_user.project_ids:
                return True
            elif (
                project.supporter_organization_id
                and project.supporter_organization_id
                in current_user.supporter_organization_id
            ):
                return True
            elif not project.is_secret:
                return True

        elif current_user.role == UserRoleType.SALES.key:
            if current_user.project_ids and project.id in current_user.project_ids:
                return True
            elif not project.is_secret:
                return True

        return False

    @staticmethod
    def is_visible_project_for_update(
        user_id: str, role: str, project: ProjectModel
    ) -> bool:
        """案件情報がアクセス可能か判定.
            1.事業者責任者
              制限なし（アクセス可）
            2.支援者、支援者責任者、営業担当者
              所属案件：アクセス可
              上記以外：アクセス不可
            ※顧客、営業責任者
              呼出し不可(API呼出権限で制御)
        Args:
            role (str): ロール
            project (ProjectModel)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        if role == UserRoleType.BUSINESS_MGR.key:
            return True

        if role in [
            UserRoleType.SUPPORTER.key,
            UserRoleType.SUPPORTER_MGR.key,
        ]:
            if project.main_supporter_user_id == user_id or (
                project.supporter_user_ids and user_id in project.supporter_user_ids
            ):
                # 所属案件:アクセス可
                return True

        elif role == UserRoleType.SALES.key:
            if project.main_sales_user_id == user_id:
                # 所属案件:アクセス可
                return True

        return False

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
            logger.warning(
                f"get_customer_name method bad request. customer_id: {customer_id}"
            )
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail=[{"loc": ["body", "customerId"], "msg": "Not exist."}],
            )
        return customer.name

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
    def check_valid_user(item: UpdateProjectByIdRequest, user_table_info: List[UserModel]):
        """有効なユーザか判定
        Args:
            item (UpdateProjectByIdRequest)
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
    def get_project_by_id(
        project_id: str, current_user: AuthUser
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
        # 認証済みユーザの情報も取得
        target_user_id_list.append(current_user.id)
        user_table_info: List[UserModel] = ProjectService.get_user_info(
            target_user_id_list
        )

        # 権限チェック
        if not ProjectService.is_visible_project(
            current_user=current_user,
            role=current_user.role,
            project=project,
            user_list=user_table_info,
        ):
            logger.warning("GetProjectById forbidden.")
            raise HTTPException(
                status_code=api_status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 指定されたサービス区分を汎用マスタで存在チェック、サービス区分の名称を取得
        service_type_name = ProjectService.get_service_name(project.service_type)
        # 支援者組織（課）IDを汎用マスタで存在チェック、支援者組織（課）名を取得
        supporter_organization_name = ProjectService.get_supporter_organization_name(
            project.supporter_organization_id
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
        year_month_from: int,
        year_month_to: int,
        date_from: int,
        date_to: int,
        all: bool,
        all_assigned: bool,
        customer_id: str,
        sort: GetProjectsSortType,
        limit: int,
        offset_page: int,
        current_user: AuthUser,
    ) -> GetProjectsResponse:
        """案件の検索・一覧を取得します。 顧客の場合は自身のアサイン案件のみ取得。

        Args:
            year_month_from (int): 年月From
            year_month_to (int): 年月To
            date_from (int): 年月日From
            date_to (int): 年月日To
            all (bool): 全案件取得（未指定時は担当案件）
            all_assigned (bool): 全担当取得（未指定時は主担当のみ）
            customer_id (str): 取引先ID
            sort (GetProjectsSortType): ソート
            limit (int): 最大取得件数
            offset_page (int): リストの中で何ページ目を取得するか
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetProjectsResponse: 取得結果
        """

        # ソート順(name)
        scan_index_forward = None
        if sort == GetProjectsSortType.NAME_ASC:
            scan_index_forward = True

        # 抽出条件
        filter_condition = None
        range_key_condition_gsi_support_date_to = None
        filter_condition_gsi_support_date_to = None

        # 抽出条件: 年月From,年月To
        # 対象年月のfromは01日、toに31日を補完
        str_year_month_from = (
            datetime.strptime(str(year_month_from), "%Y%m").strftime("%Y/%m/01")
            if year_month_from
            else None
        )
        str_year_month_to = (
            datetime.strptime(str(year_month_to), "%Y%m").strftime("%Y/%m/31")
            if year_month_to
            else None
        )
        if year_month_from:
            # 対象年月from以降に支援終了日がある案件情報が対象
            filter_condition &= ProjectModel.support_date_to >= str_year_month_from
            range_key_condition_gsi_support_date_to &= (
                ProjectModel.support_date_to >= str_year_month_from
            )
        if year_month_to:
            # 対象年月to以前に支援開始日がある案件情報が対象
            filter_condition &= ProjectModel.support_date_from <= str_year_month_to
            filter_condition_gsi_support_date_to &= (
                ProjectModel.support_date_from <= str_year_month_to
            )

        # 抽出条件: 年月日From,年月日To
        str_date_from = (
            datetime.strptime(str(date_from), "%Y%m%d").strftime("%Y/%m/%d")
            if date_from
            else None
        )
        str_date_to = (
            datetime.strptime(str(date_to), "%Y%m%d").strftime("%Y/%m/%d")
            if date_to
            else None
        )
        if date_from:
            # 対象年月from以降に支援終了日がある案件情報が対象
            filter_condition &= ProjectModel.support_date_to >= str_date_from
            range_key_condition_gsi_support_date_to &= (
                ProjectModel.support_date_to >= str_date_from
            )
        if date_to:
            # 対象年月to以前に支援開始日がある案件情報が対象
            filter_condition &= ProjectModel.support_date_from <= str_date_to
            filter_condition_gsi_support_date_to &= (
                ProjectModel.support_date_from <= str_date_to
            )

        # ユーザ権限ごとに取得条件を設定
        if current_user.role == UserRoleType.CUSTOMER.key:
            # 顧客の場合
            # 自身の案件情報を取得
            filter_condition &= (
                ProjectModel.main_customer_user_id == current_user.id
            ) | (ProjectModel.customer_user_ids.contains(current_user.id))
            filter_condition_gsi_support_date_to &= (
                ProjectModel.main_customer_user_id == current_user.id
            ) | (ProjectModel.customer_user_ids.contains(current_user.id))

        elif current_user.role in [
            UserRoleType.SUPPORTER.key,
            UserRoleType.SUPPORTER_MGR.key,
            UserRoleType.BUSINESS_MGR.key,
        ]:
            # 支援者、支援者責任者、事業者責任者の場合
            if all:
                # 全ての案件
                pass
            else:
                if all_assigned:
                    # 自身がプロデューサーまたはアクセラレーターの案件
                    filter_condition &= (
                        ProjectModel.main_supporter_user_id == current_user.id
                    ) | (ProjectModel.supporter_user_ids.contains(current_user.id))
                    filter_condition_gsi_support_date_to &= (
                        ProjectModel.main_supporter_user_id == current_user.id
                    ) | (ProjectModel.supporter_user_ids.contains(current_user.id))
                else:
                    # 自身がプロデューサーの案件
                    filter_condition &= (
                        ProjectModel.main_supporter_user_id == current_user.id
                    )
                    filter_condition_gsi_support_date_to &= (
                        ProjectModel.main_supporter_user_id == current_user.id
                    )

        elif (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.SALES_MGR.key
        ):
            # 営業担当者・営業責任者の場合
            if all:
                # 全ての案件
                pass
            else:
                if all_assigned:
                    # 全ての案件
                    pass
                else:
                    # 自身が営業担当者としてアサインされた案件
                    filter_condition &= (
                        ProjectModel.main_sales_user_id == current_user.id
                    )
                    filter_condition_gsi_support_date_to &= (
                        ProjectModel.main_sales_user_id == current_user.id
                    )

        # 案件情報取得
        # クエリパラメータ有無によりGSIを使い分ける
        if range_key_condition_gsi_support_date_to is not None:
            if customer_id:
                # 取引先IDの指定あり
                filter_condition_gsi_support_date_to &= (
                    ProjectModel.customer_id == customer_id
                )
            project_list: List[ProjectModel] = list(
                ProjectModel.data_type_support_date_to_index.query(
                    hash_key=DataType.PROJECT,
                    range_key_condition=range_key_condition_gsi_support_date_to,
                    filter_condition=filter_condition_gsi_support_date_to,
                    scan_index_forward=scan_index_forward,
                )
            )
            # ソート
            if project_list:
                # name
                if scan_index_forward:
                    project_list.sort(key=lambda x: x.name)
                # customer_name
                if sort == GetProjectsSortType.CUSTOMER_NAME_ASC:
                    project_list.sort(key=lambda x: x.customer_name)
                # support_date_from
                if sort == GetProjectsSortType.SUPPORT_DATE_FROM_DESC:
                    project_list.sort(key=lambda x: x.support_date_from, reverse=True)

        else:
            if customer_id:
                # 取引先IDの指定あり
                project_list: List[ProjectModel] = list(
                    ProjectModel.customer_id_name_index.query(
                        hash_key=customer_id,
                        filter_condition=filter_condition,
                        scan_index_forward=scan_index_forward,
                    )
                )
            else:
                # 取引先IDの指定なし
                project_list: List[ProjectModel] = list(
                    ProjectModel.data_type_name_index.query(
                        hash_key=DataType.PROJECT,
                        filter_condition=filter_condition,
                        scan_index_forward=scan_index_forward,
                    )
                )
            # ソート
            if project_list:
                # customer_name
                if sort == GetProjectsSortType.CUSTOMER_NAME_ASC:
                    project_list.sort(key=lambda x: x.customer_name)
                # support_date_from
                if sort == GetProjectsSortType.SUPPORT_DATE_FROM_DESC:
                    project_list.sort(key=lambda x: x.support_date_from, reverse=True)

        # 一般ユーザ情報を取得
        user_list: List[UserModel] = list(
            UserModel.data_type_name_index.query(hash_key=DataType.USER)
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

        project_info_list: List[ProjectInfoForGetProjects] = []
        for project in project_list:
            # 権限チェック
            # アクセス不可の案件情報はスキップ
            if not ProjectService.is_visible_project(
                current_user, current_user.role, project, user_list
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

        # ページネーション
        if limit == -1:
            # 全件取得
            current_page = project_info_list
        else:
            try:
                p = Paginator(project_info_list, limit)
                current_page = p.page(offset_page).object_list
            except EmptyPage:
                logger.warning(f"GetProjects not found. offset_page: {offset_page}")
                raise HTTPException(status_code=api_status.HTTP_404_NOT_FOUND)

        return GetProjectsResponse(
            offset_page=offset_page,
            total=len(project_info_list),
            projects=current_page,
        )

    @staticmethod
    def suggest_projects(
        customer_id: str, sort: SuggestProjectsSortType, current_user: AuthUser
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
                if ProjectService.is_visible_project_for_suggest(
                    current_user=current_user, project=project
                ):
                    # 指定されたサービス区分を汎用マスタで存在チェック、サービス区分の名称を取得
                    service_type_name = ProjectService.get_service_name(
                        project.service_type
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
    def update_project_by_id(
        item: UpdateProjectByIdRequest,
        version: int,
        project_id: str,
        current_user: UserModel,
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
        if not ProjectService.is_visible_project_for_update(
            user_id=current_user.id,
            role=current_user.role,
            project=project,
        ):
            logger.warning("UpdateProjectById not found.")
            raise HTTPException(
                status_code=api_status.HTTP_404_NOT_FOUND, detail="Not found"
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
        ProjectService.get_supporter_organization_name(
            project.supporter_organization_id
        )

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
        # 担当顧客（取引先責任者）IDの存在チェック
        ProjectService.get_user_name(
            item.main_customer_user_id, user_table_info, "mainCustomerUserId"
        )
        # メイン支援者（主担当）IDの存在チェック
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
        # （支援者or支援責任者or事業者責任者or営業担当者orお客様メンバー）
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
        if project.customer_id != item.customer_id and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(ProjectModel.customer_id.set(item.customer_id))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_ID.tbl_column_name,
                    value=item.customer_id,
                )
            )
        if project.name != item.name and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(ProjectModel.name.set(item.name))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.NAME.tbl_column_name,
                    value=item.name,
                )
            )
        if project.customer_name != customer_name and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(ProjectModel.customer_name.set(customer_name))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_NAME.tbl_column_name,
                    value=customer_name,
                )
            )
        if project.service_type != item.service_type and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(ProjectModel.service_type.set(item.service_type))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SERVICE_TYPE.tbl_column_name,
                    value=item.service_type,
                )
            )
        if project.create_new != item.create_new and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
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
        if project.main_sales_user_id != item.main_sales_user_id and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(
                ProjectModel.main_sales_user_id.set(item.main_sales_user_id)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.MAIN_SALES_USER_ID.tbl_column_name,
                    value=item.main_sales_user_id,
                )
            )
        if project.customer_success != item.customer_success and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(
                ProjectModel.customer_success.set(item.customer_success)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.CUSTOMER_SUCCESS.tbl_column_name,
                    value=item.customer_success,
                )
            )
        if project.support_date_from != item.support_date_from and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(
                ProjectModel.support_date_from.set(item.support_date_from)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SUPPORT_DATE_FROM.tbl_column_name,
                    value=item.support_date_from,
                )
            )
        if project.support_date_to != item.support_date_to and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(ProjectModel.support_date_to.set(item.support_date_to))
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SUPPORT_DATE_TO.tbl_column_name,
                    value=item.support_date_to,
                )
            )

        if (
            project.profit is None
            or (project.profit.year != float(distributed_profit.year))
        ) and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
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

        if project.total_contract_time != item.total_contract_time and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(
                ProjectModel.total_contract_time.set(item.total_contract_time)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.TOTAL_CONTRACT_TIME.tbl_column_name,
                    value=str(item.total_contract_time),
                )
            )
        if project.main_customer_user_id != item.main_customer_user_id and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
            update_action.append(
                ProjectModel.main_customer_user_id.set(item.main_customer_user_id)
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.MAIN_CUSTOMER_USER_ID.tbl_column_name,
                    value=item.main_customer_user_id,
                )
            )
        if item.salesforce_main_customer is not None and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
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
            and current_user.role in [UserRoleType.SALES.key, UserRoleType.BUSINESS_MGR.key, UserRoleType.SUPPORTER.key, UserRoleType.SUPPORTER_MGR.key]
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
        if project.is_count_man_hour != item.is_count_man_hour and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
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
        if project.contract_type != item.contract_type and (
            current_user.role == UserRoleType.SALES.key
            or current_user.role == UserRoleType.BUSINESS_MGR.key
        ):
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
                logger.info(f"platform putProjectPublication statusCode: {status_code}")
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
                    "partnerPortalCustomerId": (
                        item.customer_id
                        if customer and not customer.salesforce_customer_id
                        else None
                    ),
                    "partnerPortalProjectId": project.id,
                    "salesforceCustomerId": (
                        customer.salesforce_customer_id
                        if customer and customer.salesforce_customer_id
                        else None
                    ),
                    "partnerPortalProjectName": item.name,
                    "projectName": item.name,
                    "customerName": ProjectService.get_customer_name(item.customer_id),
                    "serviceType": ProjectService.get_service_name(item.service_type),
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
