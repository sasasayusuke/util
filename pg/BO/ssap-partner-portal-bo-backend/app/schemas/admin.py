from datetime import datetime
from typing import List

from fastapi import Query
from pydantic import EmailStr, Field, root_validator

from app.core.common_logging import CustomLogger
from app.resources.const import UserRoleType
from app.schemas.base import CustomBaseModel

logger = CustomLogger.get_logger()


class CreateAdminRequest(CustomBaseModel):
    """管理ユーザー作成リクエストクラス"""

    name: str = Field(..., title="名前", example="田中太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="user@example.com")
    roles: List[str] = Field(..., title="ロール", example=["sales"])
    company: str = Field(..., title="会社名", example="ソニーグループ株式会社")
    job: str = Field(None, title="役職", example="部長")
    supporter_organization_id: List[str] = Field(
        None, title="支援者組織ID", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    organization_name: str = Field(None, title="部署", example="IST")
    disabled: bool = Field(..., title="ログイン可否", example="false")

    @root_validator
    def validate_exist_field(cls, v):
        """
        支援者責任者の場合は支援者組織ID（supporter_organaizaion_id）が必要
        支援者責任者の以外の場合は部署名（organaizaion_name）が必要
        """
        if UserRoleType.SUPPORTER_MGR.key in v.get("roles") and (
            not v.get("supporter_organization_id")
        ):
            logger.warning("supporter_organization_id is required.")
            raise ValueError("supporter_organization_id is required.")

        if UserRoleType.SUPPORTER_MGR.key not in v.get("roles") and (
            v.get("supporter_organization_id")
        ):
            logger.warning("supporter_organization_id is required.")
            raise ValueError("supporter_organization_id is not required.")

        if UserRoleType.SUPPORTER_MGR.key not in v.get("roles") and (
            not v.get("organization_name")
        ):
            logger.warning("organization_name is required.")
            raise ValueError("organization_name is required.")

        return v


class CreateAdminResponse(CustomBaseModel):
    id: str = Field(..., title="id", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="名前", example="山田太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="user@example.com")
    roles: List[str] = Field(..., title="ロール", example="sales")
    company: str = Field(..., title="会社名", example="ソニーグループ株式会社")
    job: str = Field(None, title="役職", example="部長")
    supporter_organization_id: List[str] = Field(
        None, title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    organization_name: str = Field(None, title="部署", example="IST")
    disabled: bool = Field(..., title="ログイン可否", example="false")
    create_id: str = Field(
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        None, title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: str = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class SupporterOrganizations(CustomBaseModel):
    id: str = Field(
        ..., title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="支援者組織名", example="IST")


class GetAdminByIdResponse(CustomBaseModel):
    id: str = Field(..., title="id", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="名前", example="山田太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="user@example.com")
    roles: List[str] = Field(..., title="ロール", example="sales")
    company: str = Field(..., title="会社名", example="ソニーグループ株式会社")
    job: str = Field(None, title="役職", example="部長")
    supporter_organizations: List[SupporterOrganizations] = Field(
        None, title="支援者組織（支援者責任者のみ）"
    )
    organization_name: str = Field(None, title="部署", example="IST")
    disabled: bool = Field(..., title="ログイン可否", example="false")
    create_id: str = Field(
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        None, title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: str = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class UpdateAdminByIdQuery(CustomBaseModel):
    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class UpdateAdminByIdRequest(CustomBaseModel):
    name: str = Field(..., title="名前", example="山田太郎")
    roles: List[str] = Field(..., title="ロール", example=["sales"])
    company: str = Field(..., title="会社名", example="ソニーグループ株式会社")
    job: str = Field(None, title="役職", example="部長")
    supporter_organization_id: List[str] = Field(
        None, title="支援者組織ID", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    organization_name: str = Field(None, title="部署", example="IST")
    disabled: bool = Field(..., title="ログイン可否", example="false")

    @root_validator
    def validate_exist_field(cls, v):
        """
        支援者責任者の場合は支援者組織ID（supporter_organaizaion_id）が必要
        支援者責任者の以外の場合は部署名（organaizaion_name）が必要
        """
        if UserRoleType.SUPPORTER_MGR.key in v.get("roles") and (
            not v.get("supporter_organization_id")
        ):
            raise ValueError("supporter_organization_id is required.")

        if UserRoleType.SUPPORTER_MGR.key not in v.get("roles") and (
            v.get("supporter_organization_id")
        ):
            raise ValueError("supporter_organization_id is not required.")

        if UserRoleType.SUPPORTER_MGR.key not in v.get("roles") and (
            not v.get("organization_name")
        ):
            raise ValueError("organization_name is required.")

        return v


class UpdateAdminByIdResponse(CustomBaseModel):
    id: str = Field(..., title="id", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="名前", example="山田太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="user@example.com")
    roles: List[str] = Field(..., title="ロール", example="sales")
    company: str = Field(..., title="会社名", example="ソニーグループ株式会社")
    job: str = Field(None, title="役職", example="部長")
    supporter_organization_id: List[str] = Field(
        None, title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    organization_name: str = Field(None, title="部署", example="IST")
    disabled: bool = Field(..., title="ログイン可否", example="false")
    create_id: str = Field(
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        None, title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: str = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class PatchAdminStatusByIdQuery(CustomBaseModel):
    enable: bool = Query(..., title="有効化", example="true")
    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class PatchAdminStatusByIdResponse(CustomBaseModel):
    id: str = Field(..., title="id", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    disabled: bool = Field(..., title="ログイン可否", example="false")
    version: str = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class GetAdminByMineResponse(CustomBaseModel):
    id: str = Field(..., title="id", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="名前", example="山田太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="user@example.com")
    roles: List[str] = Field(..., title="ロール", example="sales")
    company: str = Field(..., title="会社名", example="ソニーグループ株式会社")
    job: str = Field(None, title="役職", example="部長")
    supporter_organizations: List[SupporterOrganizations] = Field(None, title="支援者組織ID")
    organization_name: str = Field(None, title="部署", example="IST")
    project_ids: List[str] = Field(None, title="プロジェクトID")
    disabled: bool = Field(..., title="ログイン可否", example="false")
    total_notifications: int = Field(..., title="通知数", example=10)


class GetAdminsAdminResponse(CustomBaseModel):
    id: str = Field(..., title="id", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="名前", example="山田太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="user@example.com")
    roles: List[str] = Field(..., title="ロール", example="sales")
    company: str = Field(..., title="会社名", example="ソニーグループ株式会社")
    job: str = Field(None, title="役職", example="部長")
    supporter_organizations: List[SupporterOrganizations] = Field(
        None, title="支援者組織（支援者責任者のみ）"
    )
    organization_name: str = Field(None, title="部署", example="IST")
    last_login_at: datetime = Field(
        None, title="最終ログイン日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    disabled: bool = Field(..., title="無効（ログイン不可）デフォルト、指定しなかった場合はfalse", example=True)


class GetAdminsQuery(CustomBaseModel):
    name: str = Query(None, title="氏名", example="田中太郎")
    email: str = Query(None, title="メールアドレス", example="tanaka@example.con")
    sort: str = Query(
        None, title="ソート", regex="^last_login_at:asc$|^last_login_at:desc$"
    )


class GetAdminsResponse(CustomBaseModel):
    total: int = Field(..., title="件数", example="10")
    admins: List[GetAdminsAdminResponse]
