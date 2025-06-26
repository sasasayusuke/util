from datetime import datetime
from typing import List

from fastapi import Query
from pydantic import EmailStr, Field, root_validator

from app.core.common_logging import CustomLogger
from app.resources.const import DefaultPageItemCount, SuggestUsersRole, UserRoleType
from app.schemas.base import CustomBaseModel

logger = CustomLogger.get_logger()


class SupporterOrganizations(CustomBaseModel):
    """支援者組織情報"""

    id: str = Field(
        None, title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(None, title="支援者組織名", example="IST")


class SupporterOrganizationsId(CustomBaseModel):
    """支援者組織情報"""

    id: str = Field(
        None, title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )


class CreateUserRequest(CustomBaseModel):
    """一般ユーザー作成リクエストクラス"""

    name: str = Field(..., title="氏名", example="田中太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="yamada@example.com")
    role: str = Field(
        ...,
        title="ロール識別子",
        example="customer",
    )
    customer_id: str = Field(
        None,
        title="取引先ID（顧客のみ。顧客テーブルの組織情報を参照）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    job: str = Field(None, title="役職", example="部長")
    company: str = Field(None, title="所属会社（顧客以外）", example="ソニーグループ株式会社")
    solver_corporation_id: str = Field(
        None, title="法人ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organizations: List[SupporterOrganizationsId] = Field(
        None, title="支援者組織（支援者、支援者責任者のみ）"
    )
    organization_name: str = Field(None, title="部署名（支援者、支援者責任者以外）", example="管理部")
    is_input_man_hour: bool = Field(
        None, title="工数調査要否（支援者、支援者責任者のみ。不要にした場合は工数入力不可）", example=True
    )

    @root_validator(pre=True)
    def require_and_exist_role(cls, values):
        """ロールの必須チェックと存在チェック"""
        try:
            if values["role"] not in [e.key for e in UserRoleType]:
                logger.warning("type error in role")
                raise ValueError("type error in role")
        except KeyError:
            logger.warning("role is required")
            raise ValueError("role is required")
        return values

    @root_validator
    def require_customer_id(cls, values):
        """顧客の場合の必須チェック"""
        if (
            values["role"] == UserRoleType.CUSTOMER.key
            and values["customer_id"] is None
        ):
            logger.warning("customer_id is required")
            raise ValueError("customer_id is required")
        return values

    @root_validator
    def require_company(cls, values):
        """顧客でない場合の必須チェック"""
        if (
            values["role"] != UserRoleType.CUSTOMER.key
            and values["role"] in [e.key for e in UserRoleType]
            and values["company"] is None
        ):
            logger.warning("company is required")
            raise ValueError("company is required")
        return values

    @root_validator
    def require_is_input_man_hour(cls, values):
        """支援者か支援者責任者の場合の必須チェック"""
        if (
            values["role"] == UserRoleType.SUPPORTER.key
            or values["role"] == UserRoleType.SUPPORTER_MGR.key
        ) and (
            values["is_input_man_hour"] is None
            or len(values["supporter_organizations"]) == 0
        ):
            logger.warning("is_input_man_hour and supporter_organizations is required")
            raise ValueError(
                "is_input_man_hour and supporter_organizations is required"
            )
        return values

    @root_validator
    def duplicate_customer_id_or_company(cls, values):
        """ユーザー種別の重複チェック"""
        if values["customer_id"] is not None and values["company"] is not None:
            logger.warning("specify either customer_id or company")
            raise ValueError("specify either customer_id or company")
        return values


class CreateUserResponse(CustomBaseModel):
    """一般ユーザー作成レスポンスクラス"""

    id: str = Field(
        ..., title="一般ユーザーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="氏名", example="田中太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="yamada@example.com")
    role: str = Field(
        ...,
        title="ロール識別子",
        example="customer",
    )
    customer_id: str = Field(
        None,
        title="取引先ID（顧客のみ。顧客テーブルの組織情報を参照）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    customer_name: str = Field(
        None, title="取引先名（顧客のみ。顧客テーブルの組織情報を参照）", example="○○株式会社"
    )
    job: str = Field(None, title="役職", example="部長")
    company: str = Field(None, title="所属会社（顧客以外）", example="ソニーグループ株式会社")
    solver_corporation_id: str = Field(
        None, title="法人ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organizations: List[SupporterOrganizations] = Field(
        None, title="支援者組織（支援者、支援者責任者のみ）"
    )
    organization_name: str = Field(None, title="部署名（支援者、支援者責任者以外）", example="管理部")
    is_input_man_hour: bool = Field(
        None, title="工数調査要否（支援者、支援者責任者のみ。不要にした場合は工数入力不可）", example=True
    )
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    version: str = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class GetUsersUserQuery(CustomBaseModel):
    """一般ユーザー一覧取得ユーザークエリクラス"""

    email: str = Query(None, title="メールアドレス", example="tanaka@example.con")
    name: str = Query(None, title="氏名", example="田中太郎")
    role: str = Query(
        None,
        title="ロール",
        regex="^customer$|^supporter$|^sales$|^supporter_mgr$|^sales_mgr$|^supporter_or_mgr$|^sales_or_mgr$|^apt$|^solver_staff$|",
    )
    sort: str = Query(
        None, title="ソート", regex="^last_login_at:asc$|^last_login_at:desc$"
    )
    offset_page: int = Query(1, title="リストの中で何ページ目を取得するか", example=1)
    limit: int = Query(DefaultPageItemCount.limit, title="最大取得件数（省略時は限度なし）")


class GetUsersUserResponse(CustomBaseModel):
    """一般ユーザー一覧取得ユーザーレスポンスクラス"""

    id: str = Field(
        ..., title="一般ユーザーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="氏名", example="田中太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="yamada@example.com")
    role: str = Field(
        ...,
        title="ロール識別子",
        example="customer",
    )
    customer_id: str = Field(
        None,
        title="取引先ID（顧客のみ。顧客テーブルの組織情報を参照）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    customer_name: str = Field(
        None, title="取引先名（顧客のみ。顧客テーブルの組織情報を参照）", example="○○株式会社"
    )
    job: str = Field(None, title="役職", example="部長")
    company: str = Field(None, title="所属会社（顧客以外）", example="ソニーグループ株式会社")
    supporter_organizations: List[SupporterOrganizations] = Field(
        None, title="支援者組織（支援者、支援者責任者のみ）"
    )
    organization_name: str = Field(None, title="部署名（支援者、支援者責任者以外）", example="管理部")
    is_input_man_hour: bool = Field(
        None, title="工数調査要否（支援者、支援者責任者のみ。不要にした場合は工数入力不可）", example=True
    )
    project_ids: List[str] = Field(
        ..., title="参加案件ID", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    agreed: bool = Field(None, title="規約同意済（新規作成時はfalse）")
    last_login_at: datetime = Field(
        None, title="最終ログイン日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    disabled: bool = Field(..., title="無効（ログイン不可）デフォルト、指定しなかった場合はfalse", example=True)


class GetUsersResponse(CustomBaseModel):
    """一般ユーザー一覧取得レスポンスクラス"""

    offset_page: int = Field(None, title="リストの総数内で何ページ目か")
    total: int = Field(..., title="件数", example="10")
    users: List[GetUsersUserResponse]


class GetUserByIdResponse(CustomBaseModel):
    """一般ユーザー単一取得レスポンスクラス"""

    id: str = Field(
        ..., title="一般ユーザーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="氏名", example="田中太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="yamada@example.com")
    role: str = Field(
        ...,
        title="ロール識別子",
        example="customer",
    )
    customer_id: str = Field(
        None,
        title="取引先ID（顧客のみ。顧客テーブルの組織情報を参照）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    customer_name: str = Field(
        None, title="取引先名（顧客のみ。顧客テーブルの組織情報を参照）", example="○○株式会社"
    )
    job: str = Field(None, title="役職", example="部長")
    company: str = Field(None, title="所属会社（顧客以外）", example="ソニーグループ株式会社")
    solver_corporation_id: str = Field(
        None, title="法人ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organizations: List[SupporterOrganizations] = Field(None, title="支援者組織情報")
    organization_name: str = Field(None, title="部署", example="コンサルティング事業部")
    is_input_man_hour: bool = Field(
        None, title="工数調査要否（支援者、支援者責任者のみ。不要にした場合は工数入力不可）", example=True
    )
    project_ids: List[str] = Field(
        ..., title="参加案件ID", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    agreed: bool = Field(None, title="規約同意済（新規作成時はfalse）")
    last_login_at: datetime = Field(
        None, title="最終ログイン日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    disabled: bool = Field(..., title="無効（ログイン不可）デフォルト、指定しなかった場合はfalse", example=True)
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    version: str = Field(..., title="ロックキー（楽観ロック制御）", example="1")


class SuggestUsersQuery(CustomBaseModel):
    """一般ユーザーサジェストクエリクラス"""

    role: SuggestUsersRole = Query(
        ...,
        title="ロール（結果の絞り込み）",
    )
    disabled: bool = Query(
        False,
        title="trueならば無効なユーザーを含む",
    )
    sort: str = Query(..., title="ソート", regex="^name:asc$|^email:asc$")


class SuggestUsersResponse(CustomBaseModel):
    """一般ユーザーサジェストレスポンスクラス"""

    id: str = Field(
        ..., title="一般ユーザーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="氏名", example="田中太郎")
    email: EmailStr = Field(..., title="メールアドレス", example="yamada@example.com")
    customer_name: str = Field(None, title="取引先名（顧客のみ）", example="○○株式会社")


class UpdateUserByIdQuery(CustomBaseModel):
    """一般ユーザー更新クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1)


class UpdateUserByIdRequest(CustomBaseModel):
    """一般ユーザー更新リクエストクラス"""

    name: str = Field(..., title="氏名", example="田中太郎")
    job: str = Field(None, title="役職", example="部長")
    company: str = Field(None, title="所属会社（顧客以外）", example="ソニーグループ株式会社")
    solver_corporation_id: str = Field(
        None, title="法人ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organizations: List[SupporterOrganizationsId] = Field(
        None, title="支援者組織（支援者、支援者責任者のみ）"
    )
    organization_name: str = Field(None, title="部署名（支援者、支援者責任者以外）", example="管理部")
    is_input_man_hour: bool = Field(
        None, title="工数調査要否（支援者、支援者責任者のみ。不要にした場合は工数入力不可）", example=True
    )


class UpdateUserByIdResponse(CustomBaseModel):
    """一般ユーザー更新レスポンスクラス"""

    message: str = Field(..., title="実行結果", example="OK")


class PatchUserStatusByIdResponse(CustomBaseModel):
    """一般ユーザー有効化/無効化レスポンスクラス"""

    id: str = Field(
        ..., title="一般ユーザーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    disabled: bool = Field(..., title="無効（ログイン不可）デフォルト、指定しなかった場合はfalse", example=True)
    version: str = Field(..., title="ロックキー（楽観ロック制御）", example="1")
