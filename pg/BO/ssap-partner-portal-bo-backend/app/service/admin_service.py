import copy
import re
import uuid
from datetime import datetime
from typing import List

from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.update import Action

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.notification import NotificationModel
from app.models.user import UserModel
from app.resources.const import DataType, DefaultLoginAt, MailType, UserRoleType
from app.schemas.admin import (
    CreateAdminRequest,
    CreateAdminResponse,
    GetAdminByIdResponse,
    GetAdminByMineResponse,
    GetAdminsAdminResponse,
    GetAdminsResponse,
    PatchAdminStatusByIdResponse,
    UpdateAdminByIdRequest,
    UpdateAdminByIdResponse,
)
from app.schemas.master import ServiceTypes
from app.service.project_service import ProjectService
from app.utils.aws.ses import SesHelper

logger = CustomLogger.get_logger()


class AdminService:
    @staticmethod
    def is_email_unique(admin: AdminModel) -> bool:
        admin_count = admin.get_user_count_by_email(
            data_type=admin.data_type, email=admin.email
        )
        return admin_count == 0

    @staticmethod
    def create_admin(
        item: CreateAdminRequest,
        current_user: AdminModel,
    ) -> CreateAdminResponse:
        """Post /admins 管理ユーザー登録API

        Args:
            item (CreateAdminRequest): 登録内容
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            CreateAdminResponse: 登録結果
        """

        # 支援者責任者のロールのみ支援者組織を登録できるように制御する
        for role_itr in item.roles:
            if role_itr == UserRoleType.SUPPORTER_MGR.key:
                save_organization = item.supporter_organization_id
                break
            else:
                save_organization = None

        # defaultでlogin_atをセットする
        create_datetime = datetime.now()
        admin = AdminModel(
            id=str(uuid.uuid4()),
            data_type="admin",
            name=item.name,
            email=item.email,
            roles=item.roles,
            company=item.company,
            job=item.job,
            supporter_organization_id=save_organization,
            organization_name=item.organization_name,
            last_login_at=datetime.strptime(
                DefaultLoginAt.DEFAULT_LOGIN_AT, "%Y-%m-%d %H:%M:%S"
            ),
            disabled=item.disabled,
            create_at=create_datetime,
            update_at=create_datetime,
        )

        if not AdminService.is_email_unique(admin=admin):
            logger.warning("email address is already registered.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="email address is already registered.",
            )

        admin.save()

        # メール通知
        bo_site_url = get_app_settings().bo_site_url
        SesHelper().send_mail(
            template_name=MailType.ADMIN_REGISTRATION_COMPLETED,
            to=[admin.email],
            cc=[],
            payload={"email_address": admin.email, "bo_top_url": bo_site_url},
        )

        return CreateAdminResponse(**admin.attribute_values)

    @classmethod
    def get_admins(
        self,
        email: str,
        name: str,
        sort: str,
        current_user: AdminModel,
    ) -> GetAdminsResponse:
        """Get /admins 管理者ユーザー一覧取得API

        Args:
            email (str): メールアドレス
            name (str): 氏名
            sort (str): ソート

        Returns:
            GetAdminsResponse: 取得結果
        """

        # クエリ条件を指定
        condition = None
        if email:
            condition &= AdminModel.email == email
        if name:
            condition &= AdminModel.name.startswith(name)

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
        admin = AdminModel.data_type_last_login_at_index.query(
            hash_key=DataType.ADMIN,
            filter_condition=condition,
            scan_index_forward=sort_asc,
        )

        # クエリ結果をリスト変数へ格納
        admins_list: List[GetAdminsAdminResponse] = []

        for admin_itr in admin:
            # supporter_organizations 取得・格納
            if admin_itr.supporter_organization_id:
                supporter_organizations = self.get_support_organizations(
                    admin_itr, admin_itr.supporter_organization_id
                )
            else:
                supporter_organizations = None

            admins_list.append(
                GetAdminsAdminResponse(
                    id=admin_itr.id,
                    name=admin_itr.name,
                    email=admin_itr.email,
                    roles=admin_itr.roles,
                    company=admin_itr.company,
                    job=admin_itr.job,
                    supporter_organizations=copy.deepcopy(supporter_organizations),
                    organization_name=admin_itr.organization_name,
                    last_login_at=admin_itr.last_login_at,
                    disabled=admin_itr.disabled,
                )
            )

        total = len(admins_list)

        return GetAdminsResponse(total=total, admins=admins_list)

    @staticmethod
    def patch_admin_status_by_id(
        version: int, enable: bool, admin_id: str, current_user: AdminModel
    ) -> PatchAdminStatusByIdResponse:
        """Patch /admins/{admin_id} 管理ユーザー削除API

        Args:
            version (int): ロックキー（楽観ロック制御）
            enable (bool): 有効化 クエリパラメータで指定
            admin_id (str): 管理ユーザーID クエリパラメータで指定
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            PatchAdminStatusByIdResponse: 有効化/無効化後の取得結果
        """

        try:
            admin = AdminModel.get(hash_key=admin_id, range_key=DataType.ADMIN)
        except DoesNotExist:
            logger.warning(f"PatchAdminStatus admin_id not found. admin_id: {admin_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if version != admin.version:
            logger.warning(
                f"PatchAdminStatus conflict. request_ver:{version} admin_ver: {admin.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        admin.update(
            actions=[
                AdminModel.disabled.set(enable),
                AdminModel.update_id.set(current_user.id),
                AdminModel.update_at.set(datetime.now()),
            ]
        )

        return PatchAdminStatusByIdResponse(**admin.attribute_values)

    @classmethod
    def get_admin_by_mine(self, current_user: AdminModel) -> GetAdminByMineResponse:
        """Get /admins/me ログイン中管理者ユーザー情報取得API

        Args:
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetAdminByMineResponse: 取得結果
        """

        admin = AdminModel.get(hash_key=current_user.id, range_key=DataType.ADMIN)

        if admin.supporter_organization_id:
            supporter_organizations = self.get_support_organizations(
                admin, admin.supporter_organization_id
            )
        else:
            supporter_organizations = None

        project_ids = []
        if UserRoleType.SALES.key in current_user.roles:
            try:
                user = next(
                    UserModel.data_type_email_index.query(
                        hash_key=DataType.USER,
                        range_key_condition=UserModel.email == current_user.email,
                    )
                )
                project_ids = user.project_ids
            except StopIteration:
                pass

        # 総お知らせ数を取得
        total_notifications_count = NotificationModel.get_total_notifications_count(
            current_user.id
        )

        return GetAdminByMineResponse(
            id=admin.id,
            name=admin.name,
            email=admin.email,
            roles=admin.roles,
            company=admin.company,
            job=admin.job,
            supporter_organizations=supporter_organizations,
            organization_name=admin.organization_name,
            project_ids=project_ids,
            disabled=admin.disabled,
            total_notifications=total_notifications_count,
        )

    @classmethod
    def get_admin_by_id(
        self, admin_id, current_user: AdminModel
    ) -> GetAdminByIdResponse:
        """Get /admins/{admin_id} 管理ユーザー詳細取得API

        Args:
            admin_id (str): 管理ユーザーID クエリパラメータで指定
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetAdminByIdResponse: 取得結果
        """

        try:
            admin = AdminModel.get(hash_key=admin_id, range_key=DataType.ADMIN)
        except DoesNotExist:
            logger.warning(f"GetAdminById admin_id not found. admin_id: {admin_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # supporter_organizations 取得・格納
        if admin.supporter_organization_id:
            supporter_organizations = self.get_support_organizations(
                admin, admin.supporter_organization_id
            )
        else:
            supporter_organizations = None

        return GetAdminByIdResponse(
            id=admin.id,
            name=admin.name,
            email=admin.email,
            roles=admin.roles,
            company=admin.company,
            job=admin.job,
            supporterOrganizations=supporter_organizations,
            organizationName=admin.organization_name,
            disabled=admin.disabled,
            createId=admin.create_id,
            createAt=admin.create_at,
            updateId=admin.update_id,
            updateAt=admin.update_at,
            version=admin.version,
        )

    @staticmethod
    def get_support_organizations(
        admin, supporter_organization_id
    ) -> List[ServiceTypes]:

        supporter_organizations = []
        for supporter_organization_id in admin.supporter_organization_id:
            # 支援者組織のnameを取得
            supporter_organization_name = (
                ProjectService.get_supporter_organization_name(
                    supporter_organization_id
                )
            )
            supporter_organizations.append(
                ServiceTypes(
                    id=supporter_organization_id, name=supporter_organization_name
                )
            )

        return supporter_organizations

    @staticmethod
    def update_admin_by_id(
        item: UpdateAdminByIdRequest,
        version,
        admin_id,
        current_user,
    ) -> UpdateAdminByIdResponse:
        """Put /admins/{admin_id} 管理ユーザー詳細更新API

        Args:
            version (int): ロックキー（楽観ロック制御）
            item (UpdateAdminByIdRequest): 更新内容
            admin_id (str): 管理ユーザーID クエリパラメータで指定
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            UpdateAdminByIdResponse: 更新結果
        """

        try:
            admin = AdminModel.get(hash_key=admin_id, range_key=DataType.ADMIN)
        except DoesNotExist:
            logger.warning(f"UpdateAdminById admin_id not found. admin_id: {admin_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if version != admin.version:
            logger.warning(
                f"UpdateAdminById conflict. request_ver:{version} admin_ver: {admin.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        update_action: List[Action] = []
        update_action = [
            AdminModel.name.set(item.name),
            AdminModel.roles.set(item.roles),
            AdminModel.company.set(item.company),
            AdminModel.job.set(item.job),
            AdminModel.organization_name.set(item.organization_name),
            AdminModel.disabled.set(item.disabled),
            AdminModel.update_id.set(current_user.id),
            AdminModel.update_at.set(datetime.now()),
        ]

        # 支援者責任者が含まれていない且つDBにsupporter_organization_idがセットされている
        # remove処理
        if (
            UserRoleType.SUPPORTER_MGR.key not in item.roles
            and admin.supporter_organization_id is not None
        ):
            update_action.append(AdminModel.supporter_organization_id.remove())

        # 支援者責任者の場合、supporter_organization_idを更新
        elif UserRoleType.SUPPORTER_MGR.key in item.roles:
            update_action.append(
                AdminModel.supporter_organization_id.set(item.supporter_organization_id)
            )

        admin.update(update_action)

        return UpdateAdminByIdResponse(**admin.attribute_values)
