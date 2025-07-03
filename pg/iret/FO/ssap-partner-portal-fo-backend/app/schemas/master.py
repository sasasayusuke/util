from datetime import datetime
from typing import List

from fastapi import Query
from pydantic import BaseModel, Field

from app.schemas.base import CustomBaseModel


class ReserveInformation(BaseModel):
    info1: str = Field(None, title="予備情報1", example="string")
    info2: str = Field(None, title="予備情報2", example="string")
    info3: str = Field(None, title="予備情報3", example="string")
    info4: str = Field(None, title="予備情報4", example="string")
    info5: str = Field(None, title="予備情報5", example="string")


class MasterItem(CustomBaseModel):
    """マスター共通クラス"""

    id: str = Field(..., title="データID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    data_type: str = Field(..., title="種別", example="supporter_organization")
    name: str = Field(..., title="名称", example="Ideation Service Team")
    value: str = Field(..., title="値", example="IST")
    attributes: ReserveInformation = Field(None, title="予備情報")
    order: int = Field(..., title="登録順", example=1)
    use: bool = Field(..., title="利用フラグ", example=True)
    create_id: str = Field(
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(None, title="作成日時", example="2020-10-23T03:21:39.356Z")
    update_id: str = Field(
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(None, title="更新日時", example="2020-10-23T03:21:39.356Z")
    version: int = Field(None, title="楽観ロックバージョン", example=1)


class GetServiceTypesQuery(CustomBaseModel):
    """サービス種別一覧取得クエリクラス"""

    disabled: bool = Query(
        False,
        title="無効データ含む",
    )


class ServiceTypes(CustomBaseModel):
    """サービス種別(id, name)"""

    id: str = Field(..., example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., example="アイデア可視化")


class GetServiceTypesResponse(CustomBaseModel):
    service_types: List[ServiceTypes] = Field(..., title="サービス種別")


class GetBatchControlResponse(CustomBaseModel):
    """バッチ処理最終完了日時取得レスポンスクラス"""

    batch_end_at: datetime = Field(
        ..., title="処理終了日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )


class GetSupporterOrganizationsQuery(CustomBaseModel):
    """支援者組織一覧取得クエリクラス"""

    disabled: bool = Query(
        False,
        title="無効データ含む",
    )


class GetSupporterOrganizationsResponseItems(CustomBaseModel):
    id: str = Field(..., title="id", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="名称", example="Ideation Service Team​")
    short_name: str = Field(..., title="値", example="IST")


class GetSupporterOrganizationsResponse(CustomBaseModel):
    supporter_organizations: List[GetSupporterOrganizationsResponseItems] = Field(
        ..., title="支援者組織"
    )


class GetNpfProjectIdResponse(CustomBaseModel):
    """NPF案件ID取得レスポンスクラス"""

    npf_project_id: str = Field(..., title="NPF案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")


class GetSelectItemsQuery(CustomBaseModel):
    """セレクトの選択肢一覧取得クエリクラス"""

    data_type: str = Query(..., title="データ区分", example="issue_map50")


class SelectItems(CustomBaseModel):
    """セレクトの選択肢(id, category, name)"""

    id: str = Field(..., title="ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    category: str = Field(None, title="カテゴリ", example="人材育成")
    name: str = Field(
        ..., title="課題名", example="新規事業開発を担うリーダーが不足している"
    )


class GetSelectItemsResponse(CustomBaseModel):
    """セレクトの選択肢一覧取得レスポンスクラス"""

    masters: List[SelectItems] = Field(..., title="セレクトの選択肢")
