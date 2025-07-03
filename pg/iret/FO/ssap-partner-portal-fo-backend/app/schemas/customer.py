from datetime import datetime
from typing import List

from app.resources.const import (
    DefaultPageItemCount,
    GetCustomersSortType,
    SuggestCustomersSortType,
)
from app.schemas.base import CustomBaseModel
from fastapi import Query
from pydantic import Field


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


class SuggestCustomersQuery(CustomBaseModel):
    """取引先サジェスト用データ取得クエリクラス"""

    sort: SuggestCustomersSortType = Query(..., title="ソート")


class SuggestCustomersResponse(CustomBaseModel):
    """取引先サジェスト用データ取得レスポンスクラス"""

    id: str = Field(..., title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="取引先名", example="ソニーグループ株式会社")


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
