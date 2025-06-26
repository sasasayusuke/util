from datetime import datetime
from typing import List, Union

from app.resources.const import SuggestUsersRole, UserRoleType
from app.schemas.base import CustomBaseModel
from fastapi import Query
from pydantic import EmailStr, Field, validator


class SupporterOrganizations(CustomBaseModel):
    """支援者組織情報"""

    id: str = Field(
        None, title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(None, title="支援者組織名", example="IST")


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
        ...,
        title="取引先ID（顧客のみ。顧客テーブルの組織情報を参照）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    organization_name: str = Field(None, title="部署", example="コンサルティング事業部")
    job: str = Field(None, title="役職", example="部長")

    @validator("role")
    def check_role(cls, v: str) -> Union[str, ValueError]:
        if v != UserRoleType.CUSTOMER.key:
            raise ValueError("Role must be customer.")

        return v


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
        ...,
        title="取引先ID（顧客のみ。顧客テーブルの組織情報を参照）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    customer_name: str = Field(..., title="取引先名（顧客のみ。顧客テーブルの組織情報を参照）", example="○○株式会社")
    job: str = Field(None, title="役職", example="部長")
    organization_name: str = Field(None, title="部署", example="コンサルティング事業部")
    disabled: bool = Field(..., title="ユーザーの有効・無効", example="True")
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class GetUserByMineResponse(CustomBaseModel):
    """ログイン中一般ユーザー情報取得レスポンスクラス"""

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
        None, title="参加案件ID", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    agreed: bool = Field(None, title="規約同意済（新規作成時はfalse）")
    last_login_at: datetime = Field(
        None, title="最終ログイン日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    disabled: bool = Field(..., title="ユーザーの有効・無効", example="True")
    total_notifications: int = Field(..., title="通知数", example=10)


class PatchUserByMineRequest(CustomBaseModel):
    """ログイン中ユーザー情報更新レスポンスクラス"""

    agreed: bool = Field(..., title="利用規約同意", example="True")


class GetUsersUserQuery(CustomBaseModel):
    """一般ユーザー一覧取得ユーザークエリクラス"""

    email: str = Query(None, title="メールアドレス", example="tanaka@example.con")
    name: str = Query(None, title="氏名", example="田中太郎")
    role: str = Query(
        None,
        title="ロール",
        regex="^customer$|^supporter$|^sales$|^supporter_mgr$|^sales_mgr$|",
    )


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
    supporter_organizations: List[SupporterOrganizations] = Field(None, title="支援者組織情報")
    organization_name: str = Field(None, title="部署", example="コンサルティング事業部")
    is_input_man_hour: bool = Field(
        None, title="工数調査要否（支援者、支援者責任者のみ。不要にした場合は工数入力不可）", example=True
    )
    project_ids: List[str] = Field(
        None, title="参加案件ID", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    agreed: bool = Field(None, title="規約同意済（新規作成時はfalse）")
    last_login_at: datetime = Field(
        None, title="最終ログイン日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    disabled: bool = Field(..., title="無効（ログイン不可）デフォルト、指定しなかった場合はfalse", example=True)


class GetUsersResponse(CustomBaseModel):
    """一般ユーザー一覧取得レスポンスクラス"""

    total: int = Field(..., title="件数", example="10")
    users: List[GetUsersUserResponse]


class GetUserByIdResponse(CustomBaseModel):
    """一般ユーザー単一取得レスポンスクラス"""

    id: str = Field(
        ..., title="一般ユーザーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="氏名", example="山田太郎")
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
    supporter_organizations: List[SupporterOrganizations] = Field(None, title="支援者組織情報")
    organization_name: str = Field(None, title="部署", example="コンサルティング事業部")
    is_input_man_hour: bool = Field(
        None, title="工数調査要否（支援者、支援者責任者のみ。不要にした場合は工数入力不可）", example=True
    )
    project_ids: List[str] = Field(
        None, title="参加案件ID", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
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
    version: str = Field(None, title="ロックキー（楽観ロック制御）", example="1")


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
    organization_name: str = Field(None, title="部署名（支援者、支援者責任者以外）", example="管理部")


class UpdateUserByIdResponse(CustomBaseModel):
    """一般ユーザー更新レスポンスクラス"""

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
    organization_name: str = Field(None, title="部署名（支援者、支援者責任者以外）", example="管理部")
    version: str = Field(None, title="ロックキー（楽観ロック制御）", example="1")
