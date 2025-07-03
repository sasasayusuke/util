from datetime import datetime
from typing import List, Literal, Union

from fastapi import HTTPException, Query
from fastapi import status as api_status
from pydantic import EmailStr, Field, root_validator

from app.resources.const import (
    ContractType,
    DefaultPageItemCount,
    GetProjectsSortType,
    ImportModeType,
    ImportResultType,
    ProjectMemberRole,
    ProjectPhaseType,
    SuggestProjectsSortType,
    SupportStatusType,
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


class Gross(CustomBaseModel):
    """売上"""

    monthly: List[float] = Field(
        None,
        title="月毎",
        ge=0,
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
        None, title="Q毎", ge=0, example=[1200000, 1200000, 1200000, 1200000]
    )
    half: List[float] = Field(None, title="半年毎", ge=0, example=[2400000, 2400000])
    year: int = Field(..., title="FY", ge=0, example=4800000)


class UserId(CustomBaseModel):
    """ユーザ情報(idのみ)"""

    id: str = Field(None, example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")


class UserIdName(UserId):
    """ユーザ情報(id,name)"""

    name: str = Field(None, example="山田太郎")


class UserName(CustomBaseModel):
    """ユーザ情報(name)"""

    name: str = Field(None, example="山田太郎")


class ProjectMember(CustomBaseModel):
    """案件担当メンバー"""

    supporter_name: str = Field(..., title="名称", example="ソニー 太郎")
    role: ProjectMemberRole = Field(..., title="役割", example="主担当")
    is_confirm: bool = Field(..., title="工数確定済", example=True)


class Organization(CustomBaseModel):
    """案件担当課"""

    supporter_organization_id: str = Field(..., title="支援者組織ID", example="IST")
    supporter_organization_name: str = Field(..., title="支援者組織名", example="IST")
    members: List[ProjectMember] = Field(..., title="案件担当メンバー")


class SalesforceMainCustomer(CustomBaseModel):
    """Salesforce取引先責任者"""

    name: str = Field(None, title="名前", example="山田太郎")
    email: Union[EmailStr, Literal[""]] = Field(
        None, title="メールアドレス", example="yamada@example.com"
    )
    organization_name: str = Field(None, title="部署", example="IST")


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
    total_contract_time: float = Field(..., title="延べ契約時間（h）", example=200)
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
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )


class CreateProjectRequest(CustomBaseModel):
    """案件新規個別登録リクエストクラス"""

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
    phase: ProjectPhaseType = Field(..., title="フェーズ", example="プラン提示（D）")
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
    supporter_organization_id: str = Field(
        None, title="支援者組織（課）ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    salesforce_main_supporter_user_name: str = Field(
        None, title="Salesforce主担当（プロデューサー初期値）", example="山田太郎"
    )
    supporter_users: List[UserId] = Field(None, title="支援者メンバー（副担当） ※複数")
    salesforce_supporter_user_names: List[str] = Field(
        None, title="Salesforce副担当（アクセラレータ―初期値）", example=["山田太郎", "山田次郎"]
    )
    is_count_man_hour: bool = Field(..., title="工数調査するかしないか", example="True")
    is_karte_remind: bool = Field(..., title="カルテの書き忘れリマインド", example="True")
    is_master_karte_remind: bool = Field(..., title="マスターカルテの書き忘れリマインド", example="True")
    contract_type: ContractType = Field(..., title="契約形態", example="有償")
    is_secret: bool = Field(..., title="案件関係者以外参照不可", example="False")
    is_solver_project: bool = Field(..., title="ソルバー担当の案件か", example="False")
    dedicated_survey_user_name: str = Field(..., title="アンケート専用ユーザ名", example="山田太郎")
    dedicated_survey_user_email: str = Field(
        ..., title="アンケート専用メールアドレス", example="yamada@example.com"
    )
    survey_password: str = Field(..., title="匿名回答アンケートのパスワード", example="ABCDEFGH1234")
    is_survey_email_to_salesforce_main_customer: bool = Field(
        ..., title="取引担当者をアンケート宛先に指定", example=True
    )

    @root_validator
    def validate_support_date_from_to(cls, values):
        from_date = datetime.strptime(values.get("support_date_from"), "%Y/%m/%d")
        to_date = datetime.strptime(values.get("support_date_to"), "%Y/%m/%d")
        if from_date > to_date:
            raise ValueError(
                "supportDateTo is greater than or equal to supportDateFrom."
            )
        return values


class CreateProjectResponse(CustomBaseModel):
    """案件新規個別登録レスポンスクラス"""

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
    phase: ProjectPhaseType = Field(..., title="フェーズ", example="プラン提示（D）")
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
    dedicated_survey_user_name: str = Field(..., title="アンケート専用ユーザ名", example="山田太郎")
    dedicated_survey_user_email: str = Field(
        ..., title="アンケート専用メールアドレス", example="yamada@example.com"
    )
    survey_password: str = Field(..., title="匿名回答アンケートのパスワード", example="ABCDEFGH1234")
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
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)


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
    phase: ProjectPhaseType = Field(..., title="フェーズ", example="プラン提示（D）")
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
    dedicated_survey_user_name: str = Field(..., title="アンケート専用ユーザ名", example="山田太郎")
    dedicated_survey_user_email: str = Field(
        ..., title="アンケート専用メールアドレス", example="yamada@example.com"
    )
    survey_password: str = Field(..., title="匿名回答アンケートのパスワード", example="ABCDEFGH1234")
    is_survey_email_to_salesforce_main_customer: bool = Field(
        None, title="取引担当者をアンケート宛先に指定", example=True
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
    supporter_organization_id: str = Field(
        None, title="支援者組織（課）ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_users: List[UserId] = Field(None, title="支援者メンバー（副担当） ※複数")
    is_count_man_hour: bool = Field(..., title="工数調査するかしないか", example="True")
    is_karte_remind: bool = Field(..., title="カルテの書き忘れリマインド", example="True")
    is_master_karte_remind: bool = Field(..., title="マスターカルテの書き忘れリマインド", example="True")
    contract_type: ContractType = Field(..., title="契約形態", example="有償")
    is_secret: bool = Field(..., title="案件関係者以外参照不可", example="False")
    is_solver_project: bool = Field(..., title="ソルバー担当の案件か", example="False")
    dedicated_survey_user_name: str = Field(..., title="アンケート専用ユーザ名", example="山田太郎")
    dedicated_survey_user_email: str = Field(
        ..., title="アンケート専用メールアドレス", example="yamada@example.com"
    )
    survey_password: str = Field(..., title="匿名回答アンケートのパスワード", example="ABCDEFGH1234")
    is_survey_email_to_salesforce_main_customer: bool = Field(
        ..., title="取引担当者をアンケート宛先に指定", example=True
    )

    @root_validator
    def validate_support_date_from_to(cls, values):
        from_date = datetime.strptime(values.get("support_date_from"), "%Y/%m/%d")
        to_date = datetime.strptime(values.get("support_date_to"), "%Y/%m/%d")
        if from_date > to_date:
            raise ValueError(
                "supportDateTo is greater than or equal to supportDateFrom."
            )
        return values


class DeleteProjectByIdQuery(CustomBaseModel):
    """案件情報削除クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class SuggestProjectsQuery(CustomBaseModel):
    """案件サジェスト用データ取得クエリクラス"""

    customer_id: str = Query(None, title="取引先ID（結果の絞り込み）")
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

    name: str = Query(None, title="案件名")
    status: SupportStatusType = Query(
        None, title="ステータス（支援前、支援中、支援後）", description="支援期間で判定"
    )
    from_year_month: int = Query(None, title="対象年月（From）", example=202112)
    to_year_month: int = Query(None, title="対象年月（To）", example=202112)
    customer_id: str = Query(
        None, title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    main_sales_user_id: str = Query(
        None, title="営業担当者ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organization_id: str = Query(
        None,
        title="支援者組織ID",
        description="カンマ区切りで複数のIDを渡すことを可能とする",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d,89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    service_type_id: str = Query(
        None, title="サービス種別ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    is_karte_usage_project_list_of_sales: bool = Query(
        False, title="営業担当者のカルテ利用案件一覧", example=True
    )
    sort: GetProjectsSortType = Query(GetProjectsSortType.NAME_ASC, title="ソート")
    offset_page: int = Query(1, title="リストの中で何ページ目を取得するか", example=1)
    limit: int = Query(DefaultPageItemCount.limit, title="最大取得件数", example=10)

    @root_validator
    def validate_year_month(cls, v):
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

        return v


class GetProjectsResponse(CustomBaseModel):
    """案件一覧取得レスポンスクラス"""

    offset_page: int = Field(None, title="リストの総数内で何ページ目か", example=1)
    total: int = Field(..., title="件数", example=10, ge=0)
    projects: List[ProjectInfoForGetProjects] = Field(..., title="案件情報")


class HeaderInfoForGetMonthlyProjects(CustomBaseModel):
    """月次案件一覧取得レスポンスのヘッダー情報クラス"""

    supporter_organization_id: str = Field(
        ..., title="支援者組織（課）ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organization_name: str = Field(..., title="支援者組織（課）名", example="IST")


class DetailInfoForGetMonthlyProjects(CustomBaseModel):
    """月次案件一覧取得レスポンスの詳細情報クラス"""

    service_type_name: str = Field(..., title="サービス区分名", example="組織開発")
    name: str = Field(..., title="お客様名／案件名", example="ソニーグループ株式会社／サンプル案件")
    contract_type: ContractType = Field(..., title="契約形態（有償or無償）", example="有償")
    main_sales_user_name: str = Field(..., title="営業担当者名", example="ソニー 太郎")
    organizations: List[Organization] = Field(None, title="案件担当課")
    support_date_from: str = Field(
        ..., title="支援期間（From）（yyyy/mm/dd）", example="2022/04/01"
    )
    support_date_to: str = Field(
        ..., title="支援期間（To）（yyyy/mm/dd）", example="2022/05/31"
    )
    total_contract_time: float = Field(..., title="延べ契約時間", example=90)
    this_month_contract_time: float = Field(..., title="当月契約時間", example=44.26)
    total_profit: int = Field(..., title="延べ契約粗利額", example=2000000)
    this_month_profit: float = Field(..., title="平均（当月）粗利額", example=1000000)


class GetMonthlyProjectsReponse(CustomBaseModel):
    """月次案件一覧取得レスポンスクラス"""

    header: List[HeaderInfoForGetMonthlyProjects] = Field(..., title="月次案件一覧ヘッダー情報")
    details: List[DetailInfoForGetMonthlyProjects] = Field(..., title="月次案件一覧詳細情報")


class ImportProjectsRequest(CustomBaseModel):
    """案件情報一括登録リクエストクラス"""

    file: str = Field(..., title="S3アップロードパス", min_length=1)
    mode: ImportModeType = Field(
        ..., title="モード（check:取り込みチェック、execute:取り込み実行）", example="check"
    )


class ProjectInfoForImportProjects(CustomBaseModel):
    """ImportProjectsResponse.projectsのList要素"""

    customer_id: str = Field(
        None, title="顧客ID（取引先ID）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
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
    name: str = Field(None, title="商談名", example="サンプルプロジェクト")
    customer_name: str = Field(None, title="顧客名（取引先名）", example="ソニーグループ株式会社")
    service_type: str = Field(
        None,
        title="サービス区分",
        description="汎用マスターのサービス種別を参照。サービス種別のデータID。",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    service_type_name: str = Field(None, title="サービス区分名", example="組織開発")
    create_new: str = Field(None, title="新規・更新", example="02. 新規")
    continued: bool = Field(None, title="期をまたぐ案件", example="True")
    main_sales_user_id: str = Field(
        None, title="営業担当者", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    main_sales_user_name: str = Field(None, title="営業担当者名", example="山田太郎")
    contract_date: str = Field(None, title="契約締結日（yyyy/mm/dd）", example="2021/01/30")
    phase: ProjectPhaseType = Field(None, title="フェーズ", example="プラン提示（D）")
    customer_success: str = Field(None, title="カスタマーサクセス", example="DXの実現")
    support_date_from: str = Field(
        None, title="支援開始日（yyyy/mm/dd）", example="2021/01/30"
    )
    support_date_to: str = Field(None, title="支援終了日（yyyy/mm/dd）", example="2021/02/28")
    profit: Profit = Field(None, title="粗利")
    total_contract_time: float = Field(None, title="延べ契約時間（h）", example=200)
    salesforce_main_customer: SalesforceMainCustomer = Field(
        None, title="Salesforce取引先責任者"
    )
    main_supporter_user_name: str = Field(None, title="メイン支援者名", example="山田太郎")
    supporter_organization_id: str = Field(
        None, title="支援者組織（課）ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organization_name: str = Field(None, title="支援者組織（課）名", example="支援課")

    salesforce_main_supporter_user_name: str = Field(
        None, title="Salesforce主担当（プロデューサー初期値）", example="山田太郎"
    )
    supporter_users: List[UserName] = Field(None, title="支援者メンバー（副担当） ※複数")
    salesforce_supporter_user_names: List[str] = Field(
        None, title="Salesforce副担当（アクセラレータ―初期値）", example=["山田太郎"]
    )
    gross: Gross = Field(None, title="売上")
    salesforce_use_package: bool = Field(None, title="パッケージ利用", example=True)
    salesforce_via_pr: bool = Field(None, title="PR経由", example=True)
    error_message: List[str] = Field(None, title="エラーメッセージ", example=["商談名を入力してください。"])


class ImportProjectsResponse(CustomBaseModel):
    """案件情報一括登録レスポンスクラス"""

    mode: ImportModeType = Field(
        ..., title="モード（check:取り込みチェック、execute:取り込み実行）", example="check"
    )
    result: ImportResultType = Field(
        ..., title="結果（ok:取り込み可、ng:取り込み不可、done:取り込み完了、error:取り込み失敗）", example="ok"
    )
    projects: List[ProjectInfoForImportProjects] = Field(..., title="案件情報")
