import re
import uuid
from datetime import datetime
from typing import Dict, List, Union

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.customer import CustomerModel
from app.models.notification import NotificationModel
from app.models.user import UserModel
from app.resources.const import DataType, DefaultLoginAt, MailType, SuggestUsersRole, UserRoleType
from app.schemas.user import (
    CreateUserRequest,
    CreateUserResponse,
    GetUserByIdResponse,
    GetUserByMineResponse,
    GetUsersResponse,
    GetUsersUserResponse,
    PatchUserByMineRequest,
    SuggestUsersResponse,
    SupporterOrganizations,
    UpdateUserByIdRequest,
    UpdateUserByIdResponse,
)
from app.service.common_service.cached_db_items import CachedDbItems
from app.utils.aws.ses import SesHelper
from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist

logger = CustomLogger.get_logger()


class UserService:
    @staticmethod
    def _send_mail(template, to_addr, cc_addrs):
        fo_site_url = get_app_settings().fo_site_url
        SesHelper().send_mail(
            template_name=template,
            to=[to_addr],
            cc=cc_addrs,
            payload={"email_address": to_addr, "fo_top_url": fo_site_url},
        )

    @staticmethod
    def _is_email_unique(user: UserModel) -> bool:
        user_count = UserModel.get_user_count_by_email(
            data_type=DataType.USER, email=user.email
        )
        return user_count == 0

    @staticmethod
    def get_supporter_organization_name(
        supporter_organization_id: str,
    ) -> Union[str, None, HTTPException]:
        """汎用マスタから支援者組織名を取得.
            取得できない場合はNoneを発行.
            supporter_organization_idがNoneまたは空の場合、Noneを返却.

        Args:
            supporter_organization_id (str): 支援者組織（粗利メイン課、アンケート集計課）ID

        Raise:

        Returns:
            str: 支援者組織名
        """
        if not supporter_organization_id:
            return None

        supporter_organization_name = None

        # 支援者組織の一覧を取得
        supporter_organizations = CachedDbItems.get_supporter_organizations()

        # 一覧に指定された支援者組織IDが含まれているか検索
        for current_supporter_organization in supporter_organizations:
            if supporter_organization_id == current_supporter_organization["id"]:
                supporter_organization_name = current_supporter_organization["name"]
                break

        return supporter_organization_name

    @staticmethod
    def create_user(
        current_user: UserModel, item: CreateUserRequest
    ) -> GetUserByMineResponse:
        """ユーザーを1件登録する

        Args:
            current_user (UserModel): 認証済みユーザー
            item (CreateUserRequest): 登録内容

        Returns:
            CreateUserResponse: 登録結果
        """
        if not UserService._is_email_unique(user=item):
            logger.warning("CreateUser. email address is already registered.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="email address is already registered.",
            )

        try:
            customer = CustomerModel.get(
                hash_key=item.customer_id, range_key=DataType.CUSTOMER
            )
        except DoesNotExist:
            logger.warning("CreateUser. customer is not exits.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="customer is not exits.",
            )

        create_datetime = datetime.now()
        user = UserModel(
            id=str(uuid.uuid4()),
            data_type=DataType.USER,
            name=item.name,
            email=item.email,
            role=item.role,
            customer_id=item.customer_id,
            customer_name=customer.name,
            job=item.job,
            organization_name=item.organization_name,
            last_login_at=datetime.strptime(
                DefaultLoginAt.DEFAULT_LOGIN_AT, "%Y-%m-%d %H:%M:%S"
            ),
            create_id=current_user.id,
            create_at=create_datetime,
            update_at=create_datetime,
        )
        user.save()

        # 登録されたユーザーへメール通知
        UserService._send_mail(MailType.USER_REGISTRATION_COMPLETED, user.email, [])

        return CreateUserResponse(**user.attribute_values)

    @staticmethod
    def get_user_by_mine(current_user: UserModel) -> GetUserByMineResponse:
        """ユーザー自身の情報を取得する

        Args:
            current_user (UserModel): 認証済みユーザー

        Returns:
            GetUserByMineResponse: 取得結果
        """
        supporter_organizations = []

        if current_user.supporter_organization_id:
            for supporter_organization_id in current_user.supporter_organization_id:
                supporter_organizations.append(
                    SupporterOrganizations(
                        id=supporter_organization_id,
                        name=UserService.get_supporter_organization_name(
                            supporter_organization_id
                        ),
                    )
                )
        else:
            supporter_organizations = None

        notifications_count = NotificationModel.get_total_notifications_count(
            current_user.id
        )

        return GetUserByMineResponse(
            id=current_user.id,
            name=current_user.name,
            email=current_user.email,
            role=current_user.role,
            customer_id=current_user.customer_id,
            customer_name=current_user.customer_name,
            job=current_user.job,
            company=current_user.company,
            solver_corporation_id=current_user.solver_corporation_id,
            supporter_organizations=supporter_organizations,
            organization_name=current_user.organization_name,
            is_input_man_hour=current_user.is_input_man_hour,
            project_ids=current_user.project_ids,
            agreed=current_user.agreed,
            last_login_at=current_user.last_login_at,
            disabled=current_user.disabled,
            version=current_user.version,
            total_notifications=notifications_count,
        )

    @staticmethod
    def patch_user_by_mine(
        current_user: UserModel, item: PatchUserByMineRequest
    ) -> Dict:
        """ログイン中のユーザー情報の更新
        利用規約同意の更新

        Args:
            current_user (UserModel): 認証済みユーザー
            item (PatchUserByMineRequest): 更新内容

        Returns:
            Dict: 更新成功を表すメッセージ
        """
        current_user.update(actions=[UserModel.agreed.set(item.agreed)])

        return {"message": "OK"}

    @staticmethod
    def get_users(
        email: str,
        role: str,
        current_user: UserModel,
    ) -> GetUsersResponse:
        """Get /users 一般ユーザー一覧取得API

        Args:
            email (str): メールアドレス
            name (str): 氏名
            role (enum): ロール

        Returns:
            GetUsersResponse: 取得結果
        """

        # クエリ条件を指定
        condition = None
        if email:
            condition &= UserModel.email == email
        if role:
            condition &= UserModel.role == role

        user_iter = UserModel.data_type_name_index.query(
            hash_key=DataType.USER,
            filter_condition=condition,
        )

        # クエリ結果をリスト変数へ格納
        users_list: List[GetUsersUserResponse] = []
        for user in user_iter:
            # SupporterOrganizationオブジェクトをレスポンス用に作成する
            supporter_info_list: List[SupporterOrganizations] = []

            if user.supporter_organization_id:
                organization_ids = user.supporter_organization_id

                for organization_id in organization_ids:
                    supporter_info_list.append(
                        SupporterOrganizations(
                            id=organization_id,
                            name=UserService.get_supporter_organization_name(
                                organization_id
                            ),
                        )
                    )

            users_list.append(
                GetUsersUserResponse(
                    supporter_organizations=supporter_info_list, **user.attribute_values
                )
            )

        # 指定したアイテム数とページ位置のユーザーはFOでは対象外
        total = len(users_list)

        return GetUsersResponse(total=total, users=users_list)

    @staticmethod
    def get_user_by_id(current_user: UserModel, user_id: str) -> GetUserByIdResponse:
        """ "Get /users/{user_id} 一般ユーザー詳細取得API

        Args:
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetUserByIdResponse:
        """

        # user情報取得
        userInfo = UserModel
        try:
            userInfo = UserModel.get(hash_key=user_id, range_key=DataType.USER)
        except DoesNotExist:
            logger.warning(f"GetUserById user_id not found. user_id: {user_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        user_response = GetUserByIdResponse(**userInfo.attribute_values)

        # SupporterOrganizationオブジェクトをレスポンス用に作成する
        supporter_info_list: List[SupporterOrganizations] = []

        if userInfo.supporter_organization_id:
            organization_ids = userInfo.supporter_organization_id

            for organization_id in organization_ids:
                supporter_info_list.append(
                    SupporterOrganizations(
                        id=organization_id,
                        name=UserService.get_supporter_organization_name(
                            organization_id
                        ),
                    )
                )

        user_response.supporter_organizations = supporter_info_list

        return user_response

    @staticmethod
    def suggest_users(
        role: SuggestUsersRole, disabled: bool, sort: str, current_user: UserModel
    ) -> SuggestUsersResponse:
        """Get /users/suggest 一般ユーザーサジェストAPI

        Args:
            role (enum): ロール（結果の絞り込み） クエリパラメータで指定
            disabled (bool): trueならば無効なユーザーを含む クエリパラメータで指定
            user_id (str): ソート クエリパラメータで指定

        Returns:
            SuggestUsersResponse: サジェスト結果
        """
        sort_str = re.split(":", sort)
        sort_column = sort_str[0]
        sort_order = sort_str[1]

        # クエリ条件を指定
        filter_condition = None

        if role in [
            SuggestUsersRole.SALES_OR_MGR,
            SuggestUsersRole.SUPPORTER_OR_MGR,
        ]:
            role_spl = role.split("_or_")

            filter_condition = UserModel.role == role_spl[0]
            filter_condition |= UserModel.role == role_spl[0] + "_" + role_spl[1]

            if role_spl[0] == UserRoleType.SUPPORTER.key:
                filter_condition |= UserModel.role == UserRoleType.BUSINESS_MGR.key

        elif role in [
            SuggestUsersRole.SUPPORTER,
            SuggestUsersRole.SUPPORTER_MGR,
        ]:
            filter_condition = UserModel.role == role
            filter_condition |= UserModel.role == UserRoleType.BUSINESS_MGR.key

        else:
            filter_condition = UserModel.role == role

        if not disabled:
            filter_condition &= UserModel.disabled == disabled

        # name昇順で取得
        if sort_column == "name" and sort_order == "asc":
            # クエリ条件と名前順でのソートを指定しクエリを実行
            user_iter = UserModel.data_type_name_index.query(
                hash_key=DataType.USER,
                filter_condition=filter_condition,
                scan_index_forward=True,
            )

            # クエリ結果をリスト変数へ格納
            suggest_users_list: List[SuggestUsersResponse] = []
            for user in user_iter:
                suggest_users_list.append(SuggestUsersResponse(**user.attribute_values))

            return suggest_users_list
        # email昇順で取得
        elif sort_column == "email" and sort_order == "asc":
            # クエリ条件とメールアドレス順でのソートを指定しクエリを実行
            user_iter = UserModel.data_type_email_index.query(
                hash_key=DataType.USER,
                filter_condition=filter_condition,
                scan_index_forward=True,
            )

            # クエリ結果をリスト変数へ格納
            suggest_users_list: List[SuggestUsersResponse] = []
            for user in user_iter:
                suggest_users_list.append(SuggestUsersResponse(**user.attribute_values))

            return suggest_users_list
        else:
            # 呼出し元でsort項目チェック済の為、以下は発生しない想定
            raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR)

    @staticmethod
    def update_user_by_id(
        current_user: UserModel,
        version: int,
        user_id: str,
        item: UpdateUserByIdRequest,
    ) -> UpdateUserByIdResponse:
        """Put /users/{user_id} 一般ユーザー登録内容更新API

        Args:
            user_id (str): 一般ユーザーID クエリパラメータで指定
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            UpdateUserByIdResponse: 更新後の取得結果
        """
        user = UserModel.get(hash_key=user_id, range_key=DataType.USER)

        if version != user.version:
            logger.warning(
                f"UpdateUserById conflict. request_ver: {version} user_ver: {user.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        user.update(
            actions=[
                UserModel.name.set(item.name),
                UserModel.job.set(item.job),
                UserModel.company.set(item.company),
                UserModel.organization_name.set(item.organization_name),
                UserModel.update_id.set(current_user.id),
                UserModel.update_at.set(datetime.now()),
            ]
        )

        return UpdateUserByIdResponse(**user.attribute_values)
