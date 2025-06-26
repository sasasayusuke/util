from datetime import datetime
from typing import List

from fastapi import Query
from pydantic import Field, root_validator

from app.resources.const import (
    DefaultPageItemCount,
    GetCustomersSortType,
    ImportModeType,
    ImportResultType,
    SuggestCustomersSortType,
)
from app.schemas.base import CustomBaseModel


class CreateCustomerRequest(CustomBaseModel):
    """取引先新規個別登録リクエストクラス"""

    # name: 必須. テーブルのソートキーは空文字を登録できない為、空文字チェック(1文字以上).
    name: str = Field(..., min_length=1, title="取引先名", example="ソニーグループ株式会社")
    # category: 任意.
    category: str = Field(None, title="カテゴリ", example="ソニーグループ")


class CreateCustomerResponse(CustomBaseModel):
    """取引先新規個別登録レスポンスクラス"""

    id: str = Field(..., title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="取引先名", example="ソニーグループ株式会社")
    category: str = Field(None, title="カテゴリ", example="ソニーグループ")
    salesforce_customer_id: str = Field(
        None, title="Salesforce取引先ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    salesforce_update_at: datetime = Field(
        None, title="Salesforce最終更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)


class GetCustomerByIdResponse(CustomBaseModel):
    """取引先詳細取得レスポンスクラス"""

    id: str = Field(..., title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="取引先名", example="ソニーグループ株式会社")
    category: str = Field(None, title="カテゴリ", example="ソニーグループ")
    salesforce_customer_id: str = Field(
        None, title="Salesforce取引先ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    salesforce_update_at: datetime = Field(
        None, title="Salesforce最終更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_user_name: str = Field(None, title="更新ユーザー名", example="ソニー太郎")
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)


class UpdateCustomerByIdQuery(CustomBaseModel):
    """取引先更新クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class UpdateCustomerByIdRequest(CustomBaseModel):
    """取引先更新リクエストクラス"""

    # name: 任意. テーブルのソートキーは空文字を登録できない為、空文字チェック(1文字以上)
    name: str = Field(None, min_length=1, title="取引先名", example="ソニーグループ株式会社")
    # category: 任意.
    category: str = Field(None, title="カテゴリ", example="ソニーグループ")

    @root_validator
    def validate_exist_field(cls, values):
        if values.get("name") is None and values.get("category") is None:
            raise ValueError("name or category is required.")
        return values


class UpdateCustomerByIdResponse(CustomBaseModel):
    """取引先更新レスポンスクラス"""

    id: str = Field(..., title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="取引先名", example="ソニーグループ株式会社")
    category: str = Field(None, title="カテゴリ", example="ソニーグループ")
    salesforce_customer_id: str = Field(
        None, title="Salesforce取引先ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    salesforce_update_at: datetime = Field(
        None, title="Salesforce最終更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_user_name: str = Field(None, title="更新ユーザー名", example="ソニー太郎")
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)


class SuggestCustomersQuery(CustomBaseModel):
    """取引先サジェスト用データ取得クエリクラス"""

    sort: SuggestCustomersSortType = Query(..., title="ソート")


class SuggestCustomersResponse(CustomBaseModel):
    """取引先サジェスト用データ取得レスポンスクラス"""

    id: str = Field(..., title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="取引先名", example="ソニーグループ株式会社")


class DeleteCustomerByIdQuery(CustomBaseModel):
    """取引先削除クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class DeleteCustomerByIdResponse(CustomBaseModel):
    """取引先削除レスポンスクラス"""

    id: str = Field(..., title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="取引先名", example="ソニーグループ株式会社")
    category: str = Field(None, title="カテゴリ", example="ソニーグループ")
    salesforce_customer_id: str = Field(
        None, title="Salesforce取引先ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    salesforce_update_at: datetime = Field(
        None, title="Salesforce最終更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)


class GetCustomersQuery(CustomBaseModel):
    """取引先一覧取得クエリクラス"""

    name: str = Query(None, title="取引先名")
    sort: GetCustomersSortType = Query(GetCustomersSortType.NAME_ASC, title="ソート")
    limit: int = Query(DefaultPageItemCount.limit, title="最大取得件数", example=20)
    offset_page: int = Query(1, title="リストの中で何ページ目を取得するか", example=1)


class CustomerInfoForGetCustomers(CustomBaseModel):
    """GetCustomersResponse.customersのList要素"""

    id: str = Field(..., title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="取引先名", example="ソニーグループ株式会社")
    category: str = Field(None, title="カテゴリ", example="ソニーグループ")
    salesforce_update_at: datetime = Field(
        None, title="Salesforce最終更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )


class GetCustomersResponse(CustomBaseModel):
    """取引先一覧取得レスポンスクラス"""

    offset_page: int = Field(None, title="リストの総数内で何ページ目か", example=1)
    total: int = Field(..., title="件数", example=10, ge=0)
    customers: List[CustomerInfoForGetCustomers] = Field(..., title="取引先情報")


class ImportCustomersRequest(CustomBaseModel):
    """取引先一括登録リクエストクラス"""

    file: str = Field(..., title="S3アップロードパス", min_length=1)
    mode: ImportModeType = Field(
        ..., title="モード（check:取り込みチェック、execute:取り込み実行）", example="check"
    )


class CustomerInfoForImportCustomers(CustomBaseModel):
    """ImportCustomersResponse.customersのList要素"""

    name: str = Field(None, title="取引先名", example="ソニーグループ株式会社")
    category: str = Field(None, title="カテゴリ", example="ソニーグループ")
    salesforce_customer_id: str = Field(
        None, title="Salesfoce取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    salesforce_update_at: str = Field(
        None, title="Salesfoce最終更新日時", example="2020/10/23 11:21"
    )
    salesforce_target: str = Field(None, title="Salesforce戦略ターゲット・コアターゲット", example="1")
    salesforce_credit_limit: str = Field(
        None, title="Salesforce月与信上限", example="10000000"
    )
    salesforce_credit_get_month: str = Field(
        None, title="Salesforce与信決済取得年月", example="2022/02/18"
    )
    salesforce_credit_manager: str = Field(
        None, title="Salesforce与信取得担当者", example="山田太郎"
    )
    salesforce_credit_no_retry: str = Field(
        None, title="Salesforce与信再取得不要", example="0"
    )
    salesforce_paws_credit_number: str = Field(
        None, title="SalesforcePAWS決裁番号", example="99999999"
    )
    salesforce_customer_owner: str = Field(
        None, title="Salesforce取引先所有者", example="山田太郎"
    )
    salesforce_customer_segment: str = Field(
        None, title="Salesforce業界セグメント", example="情報・通信業"
    )
    error_message: List[str] = Field(None, title="エラーメッセージ", example=["取引先名を入力してください。"])


class ImportCustomersResponse(CustomBaseModel):
    """取引先一括登録レスポンスクラス"""

    mode: ImportModeType = Field(
        ..., title="モード（check:取り込みチェック、execute:取り込み実行）", example="check"
    )
    result: ImportResultType = Field(
        ..., title="結果（ok:取り込み可、ng:取り込み不可、done:取り込み完了、error:取り込み失敗）", example="ok"
    )
    customers: List[CustomerInfoForImportCustomers] = Field(..., title="取引先情報")
