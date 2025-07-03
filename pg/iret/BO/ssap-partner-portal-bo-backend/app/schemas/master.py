from datetime import datetime
from typing import Dict, List, Union

from fastapi import Query
from pydantic import BaseModel, Field, validator

from app.resources.const import (
    DefaultPageItemCount,
    MasterDataType,
    QueryParameterForMasterDataType,
)
from app.schemas.base import CustomBaseModel


class ReserveInformation(BaseModel):
    info1: str = Field(None, title="予備情報1", example="string")
    info2: str = Field(None, title="予備情報2", example="string")
    info3: str = Field(None, title="予備情報3", example="string")
    info4: str = Field(None, title="予備情報4", example="string")
    info5: str = Field(None, title="予備情報5", example="string")

    class Config:
        orm_mode = True


class MasterItem(CustomBaseModel):
    """マスター共通クラス"""

    id: str = Field(..., title="データID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    data_type: str = Field(..., title="種別", example="master_supporter_organization")
    name: str = Field(..., title="名称", example="Ideation Service Team")
    value: str = Field(..., title="値", example="IST")
    attributes: ReserveInformation = Field(None, title="予備情報")
    order: int = Field(..., title="登録順", example=1)
    use: bool = Field(..., title="利用フラグ", example=True)
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
    version: int = Field(None, title="楽観ロックバージョン", example=1)

    class Config:
        orm_mode = True


class CreateMasterRespose(MasterItem):
    """マスター作成レスポンスクラス"""

    pass


class CreateMasterRequest(CustomBaseModel):
    """マスター作成リクエストクラス"""

    data_type: str = Field(..., title="種別", example="master_supporter_organization")
    name: str = Field(..., title="名称", example="Ideation Service Team")
    value: str = Field(..., title="値", example="IST")
    attributes: ReserveInformation = Field(None, title="予備情報")
    use: bool = Field(..., title="利用フラグ", example=True)

    @validator("data_type")
    def check_customer_category(cls, v: str) -> Union[str, ValueError]:
        if v is not None:
            data_type = [c.value for _, c in MasterDataType.__members__.items()]
            if v not in data_type:
                raise ValueError("Category should be either of " + ", ".join(data_type))

        return v


class GetMastersMasterQuery(CustomBaseModel):
    """マスター一覧取得マスタークエリクラス"""

    data_type: QueryParameterForMasterDataType = Query(
        QueryParameterForMasterDataType.ALL,
        title="データ区分（支援者組織情報、サービス種別、工数アラート設定、バッチ処理制御）",
    )
    limit: int = Query(
        DefaultPageItemCount.limit,
        title="最大取得件数",
        example=20,
    )
    offset_page: int = Query(1, title="リストの中で何ページ目を取得するか", example=1)


class GetMastersMasterResponse(CustomBaseModel):
    """マスター一覧取得マスターレスポンスクラス"""

    id: str = Field(..., title="データID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    data_type: str = Field(..., title="種別", example="master_supporter_organization")
    name: str = Field(..., title="名称", example="Ideation Service Team")
    value: str = Field(..., title="値", example="IST")
    order: int = Field(..., title="登録順", example=1)
    use: bool = Field(..., title="利用フラグ", example=True)
    create_at: datetime = Field(
        ..., title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_at: datetime = Field(
        ..., title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )


class GetMastersResponse(CustomBaseModel):
    """マスター一覧取得レスポンスクラス"""

    offset_page: int = Field(None, title="リストの総数内で何ページ目か")
    total: int = Field(..., title="件数", ge=0)
    masters: List[GetMastersMasterResponse]


class GetMasterByIdResponse(MasterItem):
    """マスター詳細取得レスポンスクラス"""

    pass


class UpdateMasterByIdQuery(CustomBaseModel):
    version: int = Query(..., ge=1)


class UpdateMasterByIdRequest(CustomBaseModel):
    name: str = Field(..., title="名称", example="Ideation Service Team")
    value: str = Field(..., title="値", example="IST")
    attributes: ReserveInformation = Field(None, title="予備情報")
    use: bool = Field(..., title="利用フラグ", example=True)


class UpdateMasterByIdResponse(MasterItem):
    pass


class GetServiceTypesQuery(CustomBaseModel):
    """サービス種別一覧取得クエリクラス"""

    disabled: bool = Query(
        False,
        title="無効データ含む",
    )


class ServiceTypes(CustomBaseModel):
    """サービス種別(id, name)"""

    id: str = Field(..., title="id", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="名称", example="アイデア可視化")


class GetServiceTypesResponse(CustomBaseModel):
    service_types: List[ServiceTypes] = Field(..., title="サービス種別")


class GetBatchControlResponse(CustomBaseModel):
    """バッチ処理最終完了日時取得レスポンスクラス"""

    batch_end_at: datetime = Field(
        ..., title="処理終了日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )


class FactorSetting(CustomBaseModel):
    """アラート設定クラス"""

    service_type_id: str = Field(..., title="サービス種別ID")
    direct_support_man_hour: float = Field(..., title="対面支援工数係数（%）", ge=0, le=999.99)
    direct_and_pre_support_man_hour: float = Field(
        ..., title="対面仕込支援工数係数（%）", ge=0, le=999
    )
    pre_support_man_hour: int = Field(None, title="仕込み工数係数（%）")
    hourly_man_hour_price: int = Field(None, title="契約1時間当たりの標準的な時間単価（万円）")
    monthly_profit: int = Field(None, title="月の粗利目標（万円）")


class AlertSettingAttribute(CustomBaseModel):
    """アラート設定属性クラス"""

    factor_setting: List[FactorSetting] = Field(..., title="係数設定")
    # 実装時点では不要なため型の定義のみ
    display_setting: Dict = Field(None, title="アラート表示設定")


class GetAlertSettingResponse(CustomBaseModel):
    """工数アラート設定一覧取得レスポンスクラス"""

    id: str = Field(
        ..., title="工数アラート設定ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="工数アラート設定名", example="支援工数アラート設定")
    attributes: AlertSettingAttribute = Field(..., title="制御情報")
    create_id: str = Field(
        ..., title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        ..., title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        ..., title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(..., title="楽観ロックバージョン", example=1)


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
