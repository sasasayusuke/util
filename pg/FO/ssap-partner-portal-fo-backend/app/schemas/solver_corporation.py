from datetime import datetime
from typing import List
from app.resources.const import (
    GetSolverCorporationsSortType,
)
from app.schemas.base import CustomBaseModel
from fastapi import Query
from pydantic import Field


class GetSolverCorporationsQuery(CustomBaseModel):
    """取引先一覧取得クエリクラス"""

    disabled: bool = Query(False, title="無効フラグ", example="True")
    sort: GetSolverCorporationsSortType = Query(
        GetSolverCorporationsSortType.NAME_ASC, title="ソート"
    )


class SolverCorporationInfoForGetSolverCorporations(CustomBaseModel):
    """GetSolverCorporationsResponse.solver_corporationsのList要素"""

    id: str = Field(
        ..., title="法人ソルバーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="法人ソルバー名", example="ソニーグループ株式会社")
    update_at: datetime = Field(
        None, title="最終更新日時", example="2020-10-23T03:21:39.356Z"
    )


class GetSolverCorporationsResponse(CustomBaseModel):
    """取引先一覧取得レスポンスクラス"""

    solverCorporations: List[SolverCorporationInfoForGetSolverCorporations] = Field(
        ..., title="法人ソルバー情報"
    )


class ValueAndMemo(CustomBaseModel):
    """従業員数・資本金・売上"""

    value: int = Field(None, title="値", example=100)
    memo: str = Field(None, title="メモ", example="2024年度")


class Address(CustomBaseModel):
    """住所"""

    postal_code: str = Field(None, title="郵便番号", example="000-0000")
    state: str = Field(None, title="都道府県", example="東京都")
    city: str = Field(None, title="市区郡", example="千代田区")
    street: str = Field(None, title="町名、番地", example="丸の内1-1-1")
    building: str = Field(None, title="建物名", example="丸の内ビルディング")


class CorporatePhoto(CustomBaseModel):
    """法人ソルバー画像"""

    file_name: str = Field(None, title="ファイル名", example="ファイルA")
    path: str = Field(
        None, title="パス", example="http://www.example.com"
    )


class CorporateInfoDocument(CustomBaseModel):
    """会社案内資料"""

    file_name: str = Field(None, title="ファイル名", example="ファイルA")
    path: str = Field(
        None, title="パス", example="http://www.example.com"
    )


class MainCharge(CustomBaseModel):
    """主担当者"""

    name: str = Field(None, title="名前", example="山田太郎")
    kana: str = Field(None, title="ふりがな", example="やまだたろう")
    title: str = Field(None, title="役職", example="リーダー")
    email: str = Field(None, title="連絡先メールアドレス", example="yamada@example.com")
    department: str = Field(None, title="部署", example="経営企画部")
    phone: str = Field(None, title="電話番号", example="00-0000-0000")


class DeputyCharge(CustomBaseModel):
    """副担当者"""

    name: str = Field(None, title="名前", example="山田太郎")
    kana: str = Field(None, title="ふりがな", example="やまだたろう")
    title: str = Field(None, title="役職", example="リーダー")
    email: str = Field(None, title="連絡先メールアドレス", example="yamada@example.com")
    department: str = Field(None, title="部署", example="経営企画部")
    phone: str = Field(None, title="電話番号", example="00-0000-0000")


class OtherCharge(CustomBaseModel):
    """その他担当者"""

    name: str = Field(None, title="名前", example="山田太郎")
    title: str = Field(None, title="役職", example="リーダー")
    email: str = Field(None, title="連絡先メールアドレス", example="yamada@example.com")
    department: str = Field(None, title="部署", example="経営企画部")
    phone: str = Field(None, title="電話番号", example="00-0000-0000")


class GetSolverCorporationByIdResponse(CustomBaseModel):
    """法人ソルバー情報取得レスポンスクラス"""

    id: str = Field(
        ..., title="法人ソルバーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="法人ソルバー名", example="株式会社A")
    company_abbreviation: str = Field(None, title="企業名略称", example="A")
    industry: str = Field(None, title="業種", example="IT")
    established: str = Field(None, title="設立", example="2024/10/30")
    management_team: str = Field(None, title="経営陣", example="山田太郎")
    employee: ValueAndMemo = Field(None, title="従業員数")
    capital: ValueAndMemo = Field(None, title="資本金")
    earnings: ValueAndMemo = Field(None, title="売上")
    listing_exchange: str = Field(None, title="上場取引所", example="東京証券取引所")
    business_content: str = Field(
        None, title="事業内容", example="ITソリューションの提供"
    )
    address: Address = Field(None, title="住所")
    corporate_photo: CorporatePhoto = Field(None, title="法人ソルバー画像")
    corporate_info_document: List[CorporateInfoDocument] = Field(
        None, title="会社案内資料"
    )
    issue_map50: List[str] = Field(None, title="課題マップ50", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"])
    vision: str = Field(None, title="ビジョン", example="新規事業開発")
    mission: str = Field(None, title="ミッション", example="新規事業開発")
    main_charge: MainCharge = Field(None, title="主担当者")
    deputy_charge: DeputyCharge = Field(None, title="副担当者")
    other_charge: OtherCharge = Field(None, title="その他担当者")
    notes: str = Field(None, title="備考", example="特になし")
    disabled: bool = Field(None, title="無効フラグ", example=True)
    create_id: str = Field(
        ..., title="登録者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_user_name: str = Field(None, title="登録者名", example="ソニー太郎")
    create_at: datetime = Field(
        ..., title="登録日時", example="2020-10-23T03:21:39.356872Z"
    )
    price_and_operating_rate_update_at: datetime = Field(
        None, title="稼働率・単価最終更新日時", example="2020-10-23T03:21:39.356872Z"
    )
    price_and_operating_rate_update_by: str = Field(
        None, title="稼働率・単価最終更新者", example="ソニー太郎"
    )
    update_id: str = Field(
        None, title="最終更新者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_user_name: str = Field(None, title="最終更新者名", example="ソニー太郎")
    update_at: datetime = Field(
        None, title="最終更新日時", example="2020-10-23T03:21:39.356872Z"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)
    utilization_rate_version: int = Field(None, title="稼働率・単価更新用のロックキー（楽観ロック制御）", example=1)


class UpdateSolverCorporationByIdQuery(CustomBaseModel):
    """法人ソルバー更新クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1)


class UpdateSolverCorporationByIdRequest(CustomBaseModel):
    """法人ソルバー更新リクエストクラス"""

    name: str = Field(..., title="法人ソルバー名", example="株式会社A")
    company_abbreviation: str = Field(..., title="企業名略称", example="A")
    industry: str = Field(..., title="業種", example="IT")
    established: str = Field(..., title="設立", example="2024/10/30")
    management_team: str = Field(..., title="経営陣", example="山田太郎")
    employee: ValueAndMemo = Field(..., title="従業員数")
    capital: ValueAndMemo = Field(..., title="資本金")
    earnings: ValueAndMemo = Field(..., title="売上")
    listing_exchange: str = Field(..., title="上場取引所", example="東京証券取引所")
    business_content: str = Field(
        ..., title="事業内容", example="ITソリューションの提供"
    )
    address: Address = Field(..., title="住所")
    corporate_photo: CorporatePhoto = Field(None, title="法人ソルバー画像")
    corporate_info_document: List[CorporateInfoDocument] = Field(
        None, title="会社案内資料"
    )
    issue_map50: List[str] = Field(..., title="課題マップ50", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"])
    vision: str = Field(..., title="ビジョン", example="新規事業開発")
    mission: str = Field(..., title="ミッション", example="新規事業開発")
    main_charge: MainCharge = Field(..., title="主担当者")
    deputy_charge: DeputyCharge = Field(None, title="副担当者")
    other_charge: OtherCharge = Field(None, title="その他担当者")
    notes: str = Field(None, title="設立", example="特になし")


class DeleteSolverCorporationByIdQuery(CustomBaseModel):
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)
