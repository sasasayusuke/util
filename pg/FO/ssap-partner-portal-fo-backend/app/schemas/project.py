from datetime import datetime
from typing import List, Literal, Union

from fastapi import HTTPException, Query
from fastapi import status as api_status
from pydantic import EmailStr, Field, root_validator

from app.resources.const import (
    ContractType,
    DefaultPageItemCount,
    GetProjectsSortType,
    ProjectPhaseType,
    SuggestProjectsSortType,
)
from app.schemas.base import CustomBaseModel


class ProfitFy(CustomBaseModel):
    """FY粗利"""

    year: int = Field(..., title="FY", example=4800000)


class Profit(ProfitFy):
    """粗利"""

    monthly: List[float] = Field(
        None,
        title="月毎",
        example=[
            400000,
            400000,
            400000,
            400000,
            400000,
            400000,
            400000,
            400000,
            400000,
            400000,
            400000,
            400000,
        ],
    )
    quarterly: List[float] = Field(
        None, title="Q毎", example=[1200000, 1200000, 1200000, 1200000]
    )
    half: List[float] = Field(None, title="H毎", example=[2400000, 2400000])


class UserId(CustomBaseModel):
    """ユーザ情報(idのみ)"""

    id: str = Field(None, example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")


class UserIdName(UserId):
    """ユーザ情報(id,name)"""

    name: str = Field(None, example="山田太郎")


class SalesforceMainCustomer(CustomBaseModel):
    """Salesforce取引先責任者"""

    name: str = Field(None, title="名前", example="山田太郎")
    email: Union[EmailStr, Literal[""]] = Field(
        None, title="メールアドレス", example="yamada@example.com"
    )
    organization_name: str = Field(None, title="部署", example="IST")


class SuggestProjectsQuery(CustomBaseModel):
    """案件サジェスト用データ取得クエリクラス"""

    customer_id: str = Query(
        None, title="取引先ID（結果の絞り込み）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    sort: SuggestProjectsSortType = Query(..., title="ソート")


class SuggestProjectsResponse(CustomBaseModel):
    """案件サジェスト用データ取得レスポンスクラス"""

    id: str = Field(..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="商談名（案件名）", example="サンプルプロジェクト")
    display_name: str = Field(
        ...,
        title="表示名",
        description="案件に同名が存在するため識別用の名前",
        example="「案件名」：「サービス種別」：「期間（yyyy/mm/dd ～ yyyy/mm/dd）」：「取引先名」",
    )


class GetProjectsQuery(CustomBaseModel):
    """案件一覧取得クエリクラス"""

    from_year_month: int = Query(None, title="年月From", example=202112)
    to_year_month: int = Query(None, title="年月To", example=202112)
    from_date: int = Query(None, title="年月日From", example=20220401)
    to_date: int = Query(None, title="年月日To", example=20230331)
    all: bool = Query(False, title="全案件取得（未指定時は担当案件）")
    all_assigned: bool = Query(False, title="全担当取得（未指定時は主担当のみ）")
    customer_id: str = Query(
        None, title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    sort: GetProjectsSortType = Query(GetProjectsSortType.NAME_ASC, title="ソート")
    limit: int = Query(DefaultPageItemCount.limit, title="最大取得件数", example=10)
    offset_page: int = Query(1, title="リストの中で何ページ目を取得するか", example=1)

    @root_validator
    def validate_year_month(cls, v):
        # fromYearMonth,toYearMonth
        try:
            from_date = (
                datetime.strptime(str(v.get("from_year_month")), "%Y%m")
                if v.get("from_year_month")
                else None
            )
            to_date = (
                datetime.strptime(str(v.get("to_year_month")), "%Y%m")
                if v.get("to_year_month")
                else None
            )
        except ValueError:
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail="fromYearMonth and toYearMonth should be 6-digit numbers(yyyymm).",
            )
        if from_date and to_date:
            if from_date > to_date:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="toYearMonth is greater than or equal to fromYearMonth.",
                )

        # DateFrom,DateTo
        try:
            from_date_ymd = (
                datetime.strptime(str(v.get("from_date")), "%Y%m%d")
                if v.get("from_date")
                else None
            )
            to_date_ymd = (
                datetime.strptime(str(v.get("to_date")), "%Y%m%d")
                if v.get("to_date")
                else None
            )
        except ValueError:
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail="DateFrom and DateTo should be 8-digit numbers(yyyymmdd).",
            )
        if from_date_ymd and to_date_ymd:
            if from_date_ymd > to_date_ymd:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="DateTo is greater than or equal to DateFrom.",
                )

        return v


class ProjectInfoForGetProjects(CustomBaseModel):
    """GetProjectsResponse.projectsのList要素"""

    id: str = Field(..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    customer_id: str = Field(
        ..., title="顧客ID（取引先ID）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    salesforce_update_at: str = Field(
        None, title="Salesforce最終更新日時", example="2020/10/23 03:21"
    )
    name: str = Field(..., title="商談名", example="サンプルプロジェクト")
    customer_name: str = Field(..., title="顧客名（取引先名）", example="ソニーグループ株式会社")
    service_type: str = Field(
        ...,
        title="サービス区分",
        description="汎用マスターのサービス種別を参照。サービス種別のデータID。",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    service_type_name: str = Field(None, title="サービス区分名", example="組織開発")
    create_new: bool = Field(None, title="新規・更新", example="True")
    main_sales_user_id: str = Field(
        ..., title="営業担当者", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    main_sales_user_name: str = Field(None, title="営業担当者名", example="山田太郎")
    phase: ProjectPhaseType = Field(..., title="フェーズ", example="プラン提示（D）")
    customer_success: str = Field(None, title="カスタマーサクセス", example="DXの実現")
    support_date_from: str = Field(..., title="支援開始日（yyyy/mm/dd）", example="2021/01/30")
    support_date_to: str = Field(..., title="支援終了日（yyyy/mm/dd）", example="2021/02/28")
    total_contract_time: float = Field(..., title="延べ契約時間（h）", example=200.0)
    main_customer_user_id: str = Field(
        None, title="担当顧客（取引先責任者）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    main_customer_user_name: str = Field(None, title="担当顧客名", example="山田太郎")
    salesforce_main_customer: SalesforceMainCustomer = Field(
        None, title="Salesforce取引先責任者"
    )
    customer_users: List[UserIdName] = Field(None, title="顧客メンバー ※複数")
    main_supporter_user_id: str = Field(
        None, title="メイン支援者（主担当）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    main_supporter_user_name: str = Field(None, title="メイン支援者名", example="山田太郎")
    supporter_organization_id: str = Field(
        None, title="支援者組織（課）ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organization_name: str = Field(None, title="支援者組織（課）名", example="支援課")

    salesforce_main_supporter_user_name: str = Field(
        None, title="Salesforce主担当（プロデューサー初期値）", example="山田太郎"
    )
    supporter_users: List[UserIdName] = Field(None, title="支援者メンバー（副担当） ※複数")
    salesforce_supporter_user_names: List[str] = Field(
        None, title="Salesforce副担当（アクセラレータ―初期値）", example=["山田太郎"]
    )
    is_count_man_hour: bool = Field(..., title="工数調査するかしないか", example="True")
    is_karte_remind: bool = Field(..., title="カルテの書き忘れリマインド", example="True")
    is_master_karte_remind: bool = Field(..., title="マスターカルテの書き忘れリマインド", example="True")
    contract_type: ContractType = Field(..., title="契約形態", example="有償")
    is_secret: bool = Field(..., title="案件関係者以外参照不可", example="False")
    is_solver_project: bool = Field(..., title="ソルバー担当の案件か", example="False")


class GetProjectsResponse(CustomBaseModel):
    """案件一覧取得レスポンスクラス"""

    offset_page: int = Field(None, title="リストの総数内で何ページ目か", example=1)
    total: int = Field(..., title="件数", example=10, ge=0)
    projects: List[ProjectInfoForGetProjects] = Field(..., title="案件情報")


class GetProjectByIdResponse(CustomBaseModel):
    """案件情報詳細取得レスポンスクラス"""

    id: str = Field(..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    customer_id: str = Field(
        ..., title="顧客ID（取引先ID）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    salesforce_customer_id: str = Field(
        None, title="Salesforce取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    salesforce_opportunity_id: str = Field(
        None, title="Salesforce商談ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    salesforce_update_at: str = Field(
        None, title="Salesforce最終更新日時", example="2020/10/23 03:21"
    )
    name: str = Field(..., title="商談名", example="サンプルプロジェクト")
    customer_name: str = Field(..., title="顧客名（取引先名）", example="ソニーグループ株式会社")
    service_type: str = Field(
        ...,
        title="サービス区分",
        description="汎用マスターのサービス種別を参照。サービス種別のデータID。",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    service_type_name: str = Field(None, title="サービス区分名", example="組織開発")
    create_new: bool = Field(None, title="新規・更新", example="True")
    main_sales_user_id: str = Field(
        ..., title="営業担当者", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    main_sales_user_name: str = Field(None, title="営業担当者名", example="山田太郎")
    phase: ProjectPhaseType = Field(..., title="フェーズ", example="プラン提示(D)")
    customer_success: str = Field(None, title="カスタマーサクセス", example="DXの実現")
    support_date_from: str = Field(..., title="支援開始日（yyyy/mm/dd）", example="2021/01/30")
    support_date_to: str = Field(..., title="支援終了日（yyyy/mm/dd）", example="2021/02/28")
    profit: Profit = Field(..., title="粗利")
    total_contract_time: float = Field(..., title="延べ契約時間（h）", example=200)
    main_customer_user_id: str = Field(
        None, title="担当顧客（取引先責任者）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    main_customer_user_name: str = Field(None, title="担当顧客名", example="山田太郎")
    salesforce_main_customer: SalesforceMainCustomer = Field(
        None, title="Salesforce取引先責任者"
    )
    customer_users: List[UserIdName] = Field(None, title="顧客メンバー ※複数")
    service_manager_name: str = Field(None, title="サービス責任者", example="山田太郎")
    main_supporter_user_id: str = Field(
        None, title="メイン支援者（主担当）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    main_supporter_user_name: str = Field(None, title="メイン支援者名", example="山田太郎")
    supporter_organization_id: str = Field(
        None, title="支援者組織（課）ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organization_name: str = Field(None, title="支援者組織（課）名", example="支援課")

    salesforce_main_supporter_user_name: str = Field(
        None, title="Salesforce主担当（プロデューサー初期値）", example="山田太郎"
    )
    supporter_users: List[UserIdName] = Field(None, title="支援者メンバー（副担当） ※複数")
    salesforce_supporter_user_names: List[str] = Field(
        None, title="Salesforce副担当（アクセラレータ―初期値）", example=["山田太郎"]
    )
    is_count_man_hour: bool = Field(..., title="工数調査するかしないか", example="True")
    is_karte_remind: bool = Field(..., title="カルテの書き忘れリマインド", example="True")
    is_master_karte_remind: bool = Field(..., title="マスターカルテの書き忘れリマインド", example="True")
    contract_type: ContractType = Field(..., title="契約形態", example="有償")
    is_secret: bool = Field(..., title="案件関係者以外参照不可", example="False")
    is_solver_project: bool = Field(..., title="ソルバー担当の案件か", example="False")
    dedicated_survey_user_name: str = Field(None, title="アンケート専用ユーザ名", example="山田太郎")
    dedicated_survey_user_email: str = Field(
        None, title="アンケート専用メールアドレス", example="yamada@example.com"
    )
    is_survey_email_to_salesforce_main_customer: bool = Field(
        ..., title="取引担当者をアンケート宛先に指定", example=True
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


class UpdateProjectByIdQuery(CustomBaseModel):
    """案件情報変更クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class UpdateProjectByIdRequest(CustomBaseModel):
    """案件情報変更リクエストクラス"""

    customer_id: str = Field(
        ..., title="顧客ID（取引先ID）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="商談名", example="サンプルプロジェクト")
    service_type: str = Field(
        ...,
        title="サービス区分",
        description="汎用マスターのサービス種別を参照。サービス種別のデータID。",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    create_new: bool = Field(None, title="新規・更新", example="True")
    main_sales_user_id: str = Field(
        ..., title="営業担当者", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    customer_success: str = Field(None, title="カスタマーサクセス", example="DXの実現")
    support_date_from: str = Field(
        ...,
        title="支援開始日（yyyy/mm/dd）",
        example="2021/01/30",
    )
    support_date_to: str = Field(
        ...,
        title="支援終了日（yyyy/mm/dd）",
        example="2021/02/28",
    )
    profit: ProfitFy = Field(..., title="粗利")
    total_contract_time: float = Field(..., title="延べ契約時間（h）", ge=0, example=200)
    main_customer_user_id: str = Field(
        None, title="担当顧客（取引先責任者）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    salesforce_main_customer: SalesforceMainCustomer = Field(
        None, title="Salesforce取引先責任者"
    )
    customer_users: List[UserId] = Field(None, title="顧客メンバー ※複数")
    main_supporter_user_id: str = Field(
        None, title="メイン支援者（主担当）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_users: List[UserId] = Field(None, title="支援者メンバー（副担当） ※複数")
    is_count_man_hour: bool = Field(..., title="工数調査するかしないか", example="True")
    is_karte_remind: bool = Field(..., title="カルテの書き忘れリマインド", example="True")
    is_master_karte_remind: bool = Field(..., title="マスターカルテの書き忘れリマインド", example="True")
    contract_type: ContractType = Field(..., title="契約形態", example="有償")
    is_secret: bool = Field(..., title="案件関係者以外参照不可", example="False")

    @root_validator
    def validate_support_date_from_to(cls, values):
        from_date = datetime.strptime(values.get("support_date_from"), "%Y/%m/%d")
        to_date = datetime.strptime(values.get("support_date_to"), "%Y/%m/%d")
        if from_date > to_date:
            raise ValueError(
                "supportDateTo is greater than or equal to supportDateFrom."
            )
        return values
