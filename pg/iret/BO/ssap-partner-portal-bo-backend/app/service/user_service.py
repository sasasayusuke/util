import re
import uuid
from datetime import datetime
from typing import List, Optional, Union

from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.customer import CustomerModel
from app.models.user import UserModel
from app.models.solver_corporation import (
    AddressAttribute,
    CorporatePhotoAttribute,
    DeputyChargeAttribute,
    MainChargeAttribute,
    OtherChargeAttribute,
    SolverCorporationModel,
    ValueAndMemoAttribute
)
from app.resources.const import DataType, DefaultLoginAt, MailType, UserRoleType
from app.schemas.user import (
    CreateUserRequest,
    CreateUserResponse,
    GetUserByIdResponse,
    GetUsersResponse,
    GetUsersUserResponse,
    PatchUserStatusByIdResponse,
    SuggestUsersResponse,
    SuggestUsersRole,
    SupporterOrganizations,
    UpdateUserByIdRequest,
    UpdateUserByIdResponse,
)
from app.service.common_service.cached_db_items import CachedDbItems
from app.service.common_service.pagination import EmptyPage, Paginator
from app.service.project_service import ProjectService
from app.utils.aws.ses import SesHelper
from app.utils.date import get_datetime_now

logger = CustomLogger.get_logger()


class UserService:
    @staticmethod
    def cached_get_supporter_organization_name(
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
        supporter_organizations = CachedDbItems.ReturnSupporterOrganizations()

        # 一覧に指定された支援者組織IDが含まれているか検索
        for current_supporter_organization in supporter_organizations:
            if supporter_organization_id == current_supporter_organization["id"]:
                supporter_organization_name = current_supporter_organization["name"]
                break

        return supporter_organization_name

    @staticmethod
    def _send_mail(template, to_addr, cc_addr):
        fo_site_url = get_app_settings().fo_site_url
        SesHelper().send_mail(
            template_name=template,
            to=[to_addr],
            cc=cc_addr,
            payload={"email_address": to_addr, "fo_top_url": fo_site_url},
        )

    @staticmethod
    def _is_email_unique(user: UserModel) -> bool:
        user_count = UserModel.get_user_count_by_email(
            data_type=DataType.USER, email=user.email
        )
        return user_count == 0

    @staticmethod
    def _get_customer_exist(user: UserModel) -> bool:
        customer_count = CustomerModel.get_customer_count(
            customer_id=user.customer_id, data_type=DataType.CUSTOMER
        )
        return customer_count == 1

    @staticmethod
    def create_user(
        body: CreateUserRequest, current_user: AdminModel
    ) -> CreateUserResponse:
        """Post /users 一般ユーザー作成API

        Args:
            body (CreateUserRequest): リクエストボディ
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            CreateUserResponse: 作成結果
        """
        if not UserService._is_email_unique(user=body):
            logger.warning("CreateUser. email address is already registered.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="email address is already registered.",
            )

        customer_name = None
        if body.role == UserRoleType.CUSTOMER.key:
            # 顧客の場合の取引先存在チェック
            if not UserService._get_customer_exist(user=body):
                logger.warning("CreateUser. customer is not exits.")
                raise HTTPException(
                    status_code=status.HTTP_400_BAD_REQUEST,
                    detail="customer is not exits.",
                )
            # 取引先テーブルを参照し取引先IDをキーに取引先名を取得
            customer_name = CustomerModel.get(
                body.customer_id, DataType.CUSTOMER, attributes_to_get=["name"]
            ).name

        # 支援者組織IDの抽出処理
        supporter_organizations_ids: List = []

        if body.supporter_organizations:
            if len(body.supporter_organizations) > 0:
                supporter_organizations_ids: List = []
                for supporter_organization in body.supporter_organizations:
                    supporter_organizations_ids.append(supporter_organization.id)

        # defaultでlogin_atをセットする
        create_datetime = datetime.now()
        user = UserModel(
            id=str(uuid.uuid4()),
            data_type=DataType.USER,
            name=body.name,
            email=body.email,
            role=body.role,
            customer_id=body.customer_id,
            customer_name=customer_name,
            job=body.job,
            company=body.company,
            solver_corporation_id=body.solver_corporation_id,
            supporter_organization_id=supporter_organizations_ids,
            organization_name=body.organization_name,
            is_input_man_hour=body.is_input_man_hour,
            last_login_at=datetime.strptime(
                DefaultLoginAt.DEFAULT_LOGIN_AT, "%Y-%m-%d %H:%M:%S"
            ),
            create_id=current_user.id,
            create_at=create_datetime,
            update_at=create_datetime,
        )

        # ソルバー法人 且つ リクエストに「法人ID」がない場合、新たに法人を作成
        if (user.role == UserRoleType.SOLVER_STAFF.key and not user.solver_corporation_id):

            # 法人IDを発行し「UserModel」に追加
            user.solver_corporation_id = str(uuid.uuid4())

            create_datetime = datetime.now()
            solver_corporation = SolverCorporationModel(
                id=user.solver_corporation_id,
                data_type=DataType.SOLVER_CORPORATION,
                name=user.company,
                employee=ValueAndMemoAttribute(
                    value=None,
                    memo=""
                ),
                capital=ValueAndMemoAttribute(
                    value=None,
                    memo=""
                ),
                earnings=ValueAndMemoAttribute(
                    value=None,
                    memo=""
                ),
                address=AddressAttribute(
                    postal_code="",
                    state="",
                    city="",
                    street="",
                    building="",
                ),
                corporate_photo=CorporatePhotoAttribute(
                    file_name="",
                    path="",
                ),
                corporate_info_document=[],
                issue_map50=[],
                main_charge=MainChargeAttribute(
                    name="",
                    kana="",
                    title="",
                    email="",
                    department="",
                    phone="",
                ),
                deputy_charge=DeputyChargeAttribute(
                    name="",
                    kana="",
                    title="",
                    email="",
                    department="",
                    phone="",
                ),
                other_charge=OtherChargeAttribute(
                    name="",
                    title="",
                    email="",
                    department="",
                    phone="",
                ),
                create_id=current_user.id,
                create_at=create_datetime,
            )
            solver_corporation.save()

        user.save()

        # 登録されたユーザーへメール通知
        UserService._send_mail(MailType.USER_REGISTRATION_COMPLETED, user.email, [])

        # 支援者組織名の追加
        supporter_organizations: List = []

        if user.supporter_organization_id:
            for supporter_organization_id in user.supporter_organization_id:
                supporter_organizations.append(
                    SupporterOrganizations(
                        id=supporter_organization_id,
                        name=UserService.cached_get_supporter_organization_name(
                            supporter_organization_id
                        ),
                    )
                )
        else:
            supporter_organizations = None

        return CreateUserResponse(
            supporter_organizations=supporter_organizations, **user.attribute_values
        )

    @staticmethod
    def get_users(
        email: str,
        name: str,
        role: str,
        sort: str,
        offset_page: int,
        limit: int,
        current_user: AdminModel,
    ) -> GetUsersResponse:
        """Get /users 一般ユーザー一覧取得API

        Args:
            email (str): メールアドレス
            name (str): 氏名
            role (enum): ロール
            sort (str): ソート
            offset_page (int): リストの中で何ページ目を取得するか
            limit (int): 最大取得件数（省略時は限度なし）

        Raises:
            HTTPException: 404

        Returns:
            GetUsersResponse: 取得結果
        """

        # クエリ条件を指定
        condition = None
        if email:
            condition &= UserModel.email == email
        if name:
            condition &= UserModel.name.startswith(name)
        if role:
            condition &= UserModel.role == role

        # ソート条件を指定
        if sort is not None:
            sort_str = re.split(":", sort)
            sort_order = sort_str[1]
            if sort_order == "asc":
                sort_asc = True
            else:
                sort_asc = False
        else:
            sort_asc = None

        # クエリ条件とソート条件を指定しクエリを実行
        user_iter = UserModel.data_type_last_login_at_index.query(
            hash_key=DataType.USER,
            filter_condition=condition,
            scan_index_forward=sort_asc,
        )

        # クエリ結果をリスト変数へ格納
        users_list: List[GetUsersUserResponse] = []
        for user in user_iter:
            # 支援者組織IDから支援者組織名を取得
            supporter_organizations: List[SupporterOrganizations] = []
            if user.supporter_organization_id is not None:
                for supporter_organization_id in user.supporter_organization_id:
                    supporter_organization_name: str = (
                        UserService.cached_get_supporter_organization_name(
                            supporter_organization_id
                        )
                    )

                    supporter_organizations.append(
                        SupporterOrganizations(
                            id=supporter_organization_id,
                            name=supporter_organization_name,
                        )
                    )

            # 案件に未アサインなら空配列を返却 (レスポンスで必須のため)
            project_ids: List = []
            if user.project_ids is not None:
                project_ids = user.project_ids

            # ソルバー法人の場合、ソルバー法人テーブルから所属会社を取得
            company: Optional[str] = None
            if user.role == UserRoleType.SOLVER_STAFF.key:
                solver_corporation: Optional[SolverCorporationModel] = (
                    UserService.get_solver_corporation_by_id(user.solver_corporation_id)
                )
                company = solver_corporation.name if solver_corporation else None
            else:
                company = user.company

            users_list.append(
                GetUsersUserResponse(
                    id=user.id,
                    name=user.name,
                    email=user.email,
                    role=user.role,
                    customer_id=user.customer_id,
                    customer_name=user.customer_name,
                    job=user.job,
                    company=company,
                    supporter_organizations=supporter_organizations,
                    organization_name=user.organization_name,
                    is_input_man_hour=user.is_input_man_hour,
                    project_ids=project_ids,
                    agreed=user.agreed,
                    last_login_at=user.last_login_at,
                    disabled=user.disabled,
                )
            )

        # 指定したアイテム数とページ位置のユーザーを取得
        try:
            p = Paginator(users_list, limit)
            current_page = p.page(offset_page).object_list
        except EmptyPage:
            logger.warning(f"GetUsers not found. offset_page:{offset_page}")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        total = len(users_list)

        return GetUsersResponse(
            offset_page=offset_page, total=total, users=current_page
        )

    @staticmethod
    def suggest_users(
        role: SuggestUsersRole, disabled: bool, sort: str, current_user: AdminModel
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
        if (
            role == SuggestUsersRole.SALES_OR_MGR
            or role == SuggestUsersRole.SUPPORTER_OR_MGR
        ):
            role_spl = role.split("_or_")

            filter_condition &= UserModel.role == role_spl[0]
            filter_condition |= UserModel.role == role_spl[0] + "_" + role_spl[1]
            if role_spl[0] == UserRoleType.SUPPORTER.key:
                filter_condition |= UserModel.role == UserRoleType.BUSINESS_MGR.key
        else:
            filter_condition = UserModel.role == role

        if role in [
            UserRoleType.SUPPORTER.key, UserRoleType.SUPPORTER_MGR.key
        ]:
            # 事業者責任者は支援者で指定されたらサジェストに含める
            filter_condition |= UserModel.role == UserRoleType.BUSINESS_MGR.key

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
    def get_user_by_id(
        user_id: str,
        current_user: AdminModel,
    ) -> GetUsersResponse:
        """Get /users/{user_id} 一般ユーザー詳細取得API

        Args:
            user_id (str): 一般ユーザーID クエリパラメータで指定
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetUserByIdResponse: 取得結果
        """
        try:
            user = UserModel.get(hash_key=user_id, range_key=DataType.USER)
        except DoesNotExist:
            logger.warning(f"GetUserById user_id not found. user_id: {user_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 支援者組織名の追加
        supporter_organizations: List = []

        if user.supporter_organization_id:
            for supporter_organization_id in user.supporter_organization_id:
                supporter_organizations.append(
                    SupporterOrganizations(
                        id=supporter_organization_id,
                        name=ProjectService.cached_get_supporter_organization_name(
                            supporter_organization_id
                        ),
                    )
                )
        else:
            supporter_organizations = None

        # 案件に未アサインなら空配列を返却 (レスポンスで必須のため)
        project_ids: List = []

        if user.project_ids is not None:
            project_ids = user.project_ids

        # ソルバー法人の場合、ソルバー法人テーブルから所属会社を取得
        company: Optional[str] = None
        if user.role == UserRoleType.SOLVER_STAFF.key:
            solver_corporation: Optional[SolverCorporationModel] = (
                UserService.get_solver_corporation_by_id(user.solver_corporation_id)
            )
            company = solver_corporation.name if solver_corporation else None
        else:
            company = user.company

        return GetUserByIdResponse(
            id=user.id,
            name=user.name,
            email=user.email,
            role=user.role,
            customer_id=user.customer_id,
            customer_name=user.customer_name,
            job=user.job,
            company=company,
            solver_corporation_id=user.solver_corporation_id,
            supporter_organizations=supporter_organizations,
            organization_name=user.organization_name,
            is_input_man_hour=user.is_input_man_hour,
            project_ids=project_ids,
            agreed=user.agreed,
            last_login_at=user.last_login_at,
            disabled=user.disabled,
            create_id=user.create_id,
            create_at=user.create_at,
            update_id=user.update_id,
            update_at=user.update_at,
            version=user.version,
        )

    @staticmethod
    def update_user_by_id(
        version: int,
        body: UpdateUserByIdRequest,
        user_id: str,
        current_user: AdminModel,
    ) -> UpdateUserByIdResponse:
        """Put /users/{user_id} 一般ユーザー登録内容更新API

        Args:
            user_id (str): 一般ユーザーID クエリパラメータで指定
            body (UpdateUserByIdRequest): リクエストボディ
            version (int): 楽観ロック制御
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            UpdateUserByIdResponse: 更新結果
        """
        user = UserModel.get(hash_key=user_id, range_key=DataType.USER)

        if version != user.version:
            logger.warning(
                f"UpdateUserById conflict. request_ver:{version} user_ver: {user.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 支援者組織IDの抽出処理
        supporter_organizations_ids: List = []

        for supporter_organization in body.supporter_organizations:
            supporter_organizations_ids.append(supporter_organization.id)

        user.update(
            actions=[
                UserModel.name.set(body.name),
                UserModel.job.set(body.job),
                UserModel.company.set(body.company),
                UserModel.solver_corporation_id.set(body.solver_corporation_id),
                UserModel.supporter_organization_id.set(supporter_organizations_ids)
                if len(supporter_organizations_ids) > 0
                else UserModel.supporter_organization_id.remove(),
                UserModel.organization_name.set(body.organization_name),
                UserModel.is_input_man_hour.set(body.is_input_man_hour),
                UserModel.update_id.set(current_user.id),
                UserModel.update_at.set(get_datetime_now()),
            ]
        )

        # ソルバー法人 且つ リクエストに「法人ID」がない場合、新たに法人を作成
        if (user.role == UserRoleType.SOLVER_STAFF.key and not user.solver_corporation_id):

            # 法人IDを発行し「UserModel」に追加
            user.update(
                actions=[
                    UserModel.solver_corporation_id.set(str(uuid.uuid4()))
                ]
            )

            create_datetime = datetime.now()
            solver_corporation = SolverCorporationModel(
                id=user.solver_corporation_id,
                data_type=DataType.SOLVER_CORPORATION,
                name=user.company,
                employee=ValueAndMemoAttribute(
                    value=None,
                    memo=""
                ),
                capital=ValueAndMemoAttribute(
                    value=None,
                    memo=""
                ),
                earnings=ValueAndMemoAttribute(
                    value=None,
                    memo=""
                ),
                address=AddressAttribute(
                    postal_code="",
                    state="",
                    city="",
                    street="",
                    building="",
                ),
                corporate_photo=CorporatePhotoAttribute(
                    file_name="",
                    path="",
                ),
                corporate_info_document=[],
                main_charge=MainChargeAttribute(
                    name="",
                    kana="",
                    title="",
                    email="",
                    department="",
                    phone="",
                ),
                deputy_charge=DeputyChargeAttribute(
                    name="",
                    kana="",
                    title="",
                    email="",
                    department="",
                    phone="",
                ),
                other_charge=OtherChargeAttribute(
                    name="",
                    title="",
                    email="",
                    department="",
                    phone="",
                ),
                create_id=current_user.id,
                create_at=create_datetime,
            )

            solver_corporation.save()

        return UpdateUserByIdResponse(message="OK")

    @staticmethod
    def patch_user_status_by_id(
        version: int, enable: bool, user_id: str, current_user: AdminModel
    ) -> PatchUserStatusByIdResponse:
        """Delete /users/{user_id} 一般ユーザー有効化/無効化API

        Args:
            version (int): ロックキー（楽観ロック制御） クエリパラメータで指定
            enable (bool): 有効化 クエリパラメータで指定
            user_id (str): 一般ユーザーID クエリパラメータで指定

        Returns:
            PatchUserByIdResponse: 有効化/無効化後の取得結果
        """
        user = UserModel.get(hash_key=user_id, range_key=DataType.USER)

        if version != user.version:
            logger.warning(
                f"PatchUserById conflict. request_ver:{version} user_ver: {user.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        user.update(
            actions=[
                UserModel.disabled.set(enable),
                UserModel.update_id.set(current_user.id),
                UserModel.update_at.set(get_datetime_now()),
            ]
        )

        return PatchUserStatusByIdResponse(**user.attribute_values)

    @staticmethod
    def get_solver_corporation_by_id(id: str) -> Optional[SolverCorporationModel]:
        """ソルバー法人取得取得

        Args:
            id (str): ソルバー法人ID

        Returns:
            SolverCorporationModel: ソルバー法人
        """
        try:
            return SolverCorporationModel.get(id, DataType.SOLVER_CORPORATION)
        except DoesNotExist:
            return None
