from datetime import datetime
from typing import List

from fastapi import Query
from pydantic import Field

from app.resources.const import SalesSupportManHourType, SupporterRoleTypeName
from app.schemas.base import CustomBaseModel


class YaerMonthQuery(CustomBaseModel):
    year: int = Query(..., title="対象年", example="2021", ge=2000, le=3000)
    month: int = Query(..., title="対象月", example="7", ge=1, le=12)


class GetSummarySupporterOrganizationsManHoursQuery(CustomBaseModel):
    """支援者組織(課)別工数取得クエリクラス"""

    year: int = Query(..., title="対象年", example="2021", ge=2000, le=3000)
    month: int = Query(..., title="対象月", example="7", ge=1, le=12)
    supporter_organization_id: str = Query(
        None, title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )


class GetSummarySupporterOrganizationsManHoursResponse(CustomBaseModel):
    """支援者組織(課)別工数取得レスポンスクラス"""

    supporter_organization_id: str = Field(..., title="支援者組織ID")
    supporter_organization_name: str = Field(..., title="支援者組織名")
    annual_sales: float = Field(..., title="年商（億円）")
    monthly_sales: float = Field(..., title="月商（百万円")
    monthly_project_price: float = Field(..., title="有償案件の月時間単価（万円/1時間）")
    monthly_contract_time: float = Field(..., title="有償案件の月契約時間（時間）")
    monthly_work_time: float = Field(..., title="有償案件の月稼働時間直接寄与（時間）")
    monthly_work_time_rate: int = Field(..., title="月契約時間当たりの直接寄与工数の比率")
    monthly_supporters: int = Field(..., title="月人員数（人）")
    monthly_man_hour: float = Field(..., title="月総工数（時間）")
    monthly_occupancy_rate: int = Field(..., title="月稼働率直接寄与（有償案件分のみ）")
    monthly_occupancy_total_time: float = Field(..., title="月稼働時間仕込含（時間）（無償案件の対応を含む）")
    monthly_occupancy_total_rate: int = Field(..., title="月稼働仕込含")
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )


class ManHourServiceTypeProjectItem(CustomBaseModel):
    """支援者組織(課)別工数取得レスポンスクラス"""

    project_id: str = Field(..., title="案件ID")
    project_name: str = Field(..., title="案件名")
    customer_id: str = Field(..., title="取引先ID")
    contract_type: str = Field(..., title="契約形態（有償or無償）")
    this_month_direct_support_man_hour_main: float = Field(
        ..., title="当月プロデューサー直接支援工数（h）"
    )
    this_month_direct_support_man_hour_sub: float = Field(
        ..., title="当月アクセラレータ直接支援工数（h）"
    )
    this_month_pre_support_man_hour: float = Field(..., title="当月支援仕込工数（h）")
    this_month_contract_time: float = Field(..., title="当月契約時間（h）")
    total_process_y_percent: int = Field(..., title="全行程のY%に相当（h)")

    class Config:
        orm_mode = True


class GetSummaryServiceTypesManHoursResponse(CustomBaseModel):
    """サービス種別別工数一覧取得レスポンスクラス"""

    service_type_id: str = Field(..., title="サービス種別ID")
    service_type_name: str = Field(..., title="サービス種別名")
    direct_support_man_hour_factor: int = Field(None, title="対面支援工数係数（%）")
    projects: List[ManHourServiceTypeProjectItem] = Field(..., title="案件別工数情報")

    class Config:
        orm_mode = True


class GetSummaryProjectManHourAlertsQuery(YaerMonthQuery):
    supporter_organization_id: str = Query(
        None,
        title="支援者組織（課）ID<br>カンマ区切りで複数のIDを渡すことを可能とする",
        example="supporterOrganizationId=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d,89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    service_type_id: str = Query(
        None,
        title="サービス種別ID<br>カンマ区切りで複数のIDを渡すことを可能とする",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )


class SupporterUser(CustomBaseModel):
    """支援者組織(課)別工数取得レスポンスクラス"""

    id: str = Field(..., title="ユーザーID")
    name: str = Field(..., title="ユーザー名")

    class Config:
        orm_mode = True


class ManHourProjectSummary(CustomBaseModel):
    """支援者組織(課)別工数取得レスポンスクラス"""

    project_id: str = Field(..., title="案件ID")
    project_name: str = Field(..., title="案件名")
    customer_id: str = Field(..., title="取引先ID")
    customer_name: str = Field(..., title="取引先名")
    supporter_organization_id: str = Field(..., title="支援者組織ID（粗利メイン課）")
    supporter_organization_name: str = Field(None, title="支援者組織名（粗利メイン課）")
    service_type: str = Field(..., title="サービス区分（組織開発orアイデア可視化of社会実装PoC）")
    service_type_name: str = Field(..., title="サービス区分名")
    support_date_from: str = Field(..., title="支援期間（From）（yyyy/mm/dd）")
    support_date_to: str = Field(..., title="支援期間（To）（yyyy/mm/dd）")
    total_contract_time: float = Field(..., title="延べ契約時間")
    this_month_contract_time: float = Field(..., title="当月契約時間")
    total_profit: int = Field(..., title="延べ契約粗利額")
    this_month_profit: int = Field(..., title="平均（当月）粗利額")
    main_supporter_user: SupporterUser = Field(None, title="メイン支援者（主担当）")
    supporter_users: List[SupporterUser] = Field(None, title="支援者メンバー（副担当）")
    summary_direct_support_man_hour: float = Field(..., title="累積全体直接支援工数")
    summary_pre_support_man_hour: float = Field(..., title="累積全体仕込支援工数")
    this_month_direct_support_man_hour: float = Field(..., title="当月全体直接支援工数")
    this_month_pre_support_man_hour: float = Field(..., title="当月全体仕込支援工数")
    summary_theoretical_direct_support_man_hour: float = Field(
        ..., title="累積理論直接支援工数 ※理論値についてはV1.0ドロップ"
    )
    summary_theoretical_pre_support_man_hour: float = Field(
        ..., title="累積理論仕込支援工数 ※理論値についてはV1.0ドロップ"
    )
    this_month_theoretical_direct_support_man_hour: float = Field(
        ..., title="当月（一か月あたり）理論直接支援工数 ※理論値についてはV1.0ドロップ"
    )
    this_month_theoretical_pre_support_man_hour: float = Field(
        ..., title="当月（一か月あたり）理論仕込支援工数 ※理論値についてはV1.0ドロップ"
    )

    class Config:
        orm_mode = True


class GetSummaryProjectManHourAlertsResponse(CustomBaseModel):
    """案件別工数アラート一覧取得レスポンスクラス"""

    summary_this_month_contract_time: float = Field(..., title="当月契約時間合計")
    projects: List[ManHourProjectSummary] = Field(...)

    class Config:
        orm_mode = True


class GetSummaryProjectManHourAlertQuery(YaerMonthQuery):
    project_id: str = Query(
        ...,
        title="案件ID",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )


class SupportManHours(CustomBaseModel):
    """支援者工数クラス"""

    supporter_user_id: str = Field(None, title="支援者ID")
    supporter_user_name: str = Field(None, title="支援者名")
    role: SupporterRoleTypeName = Field(None, title="分類（プロデューサー or アクセラレーター）")
    this_month_supporter_direct_support_man_hours: float = Field(
        None, title="当月支援者別直接支援工数"
    )
    this_month_supporter_pre_support_man_hours: float = Field(
        None, title="当月支援者別仕込支援工数"
    )
    summary_supporter_direct_support_man_hours: float = Field(
        None, title="累積支援者別直接支援工数"
    )
    summary_supporter_pre_support_man_hours: float = Field(None, title="累積支援者別仕込支援工数")

    class Config:
        orm_mode = True


class GetSummaryProjectManHourAlertResponse(CustomBaseModel):
    """案件別工数アラート一覧取得レスポンスクラス"""

    project_id: str = Field(..., title="案件ID")
    project_name: str = Field(..., title="案件名")
    customer_id: str = Field(..., title="取引先ID")
    customer_name: str = Field(..., title="取引先名")
    man_hours: List[SupportManHours] = Field(..., title="支援者ごとの工数（プロデューサーが先頭）")

    class Config:
        orm_mode = True


class DirectSupportManHourItems(CustomBaseModel):
    project_id: str = Field(..., title="案件ID")
    project_name: str = Field(..., title="案件名")
    role: SupporterRoleTypeName = Field(..., title="（案件内ごとの支援者の）役割")
    customer_id: str = Field(..., title="顧客ID")
    customer_name: str = Field(..., title="顧客名")
    service_type: str = Field(..., title="サービス種別")
    karte_man_hour: float = Field(..., title="カルテ入力工数（h）")
    input_man_hour: float = Field(..., title="入力工数（h）")

    class Config:
        orm_mode = True


class DirectSupportManHours(CustomBaseModel):
    items: List[DirectSupportManHourItems] = Field(None, title="入力値")
    memo: str = Field(None, title="メモ")

    class Config:
        orm_mode = True


class PreSupportManHourItems(CustomBaseModel):
    project_id: str = Field(..., title="案件ID")
    project_name: str = Field(..., title="案件名")
    role: SupporterRoleTypeName = Field(..., title="（案件内ごとの支援者の）役割")
    customer_id: str = Field(..., title="顧客ID")
    customer_name: str = Field(..., title="顧客名")
    service_type: str = Field(..., title="サービス種別")
    input_man_hour: float = Field(..., title="入力工数（h）")

    class Config:
        orm_mode = True


class PreSupportManHours(CustomBaseModel):
    items: List[PreSupportManHourItems] = Field(None, title="入力値")
    memo: str = Field(None, title="メモ")

    class Config:
        orm_mode = True


class SalesSupportManHourItems(CustomBaseModel):
    project_name: str = Field(..., title="案件名")
    customer_name: str = Field(..., title="取引先名")
    type: SalesSupportManHourType = Field(..., title="新規・継続")
    input_man_hour: float = Field(..., title="入力工数（h）")

    class Config:
        orm_mode = True


class SalesSupportManHours(CustomBaseModel):
    items: List[SalesSupportManHourItems] = Field(None, title="入力値")
    memo: str = Field(None, title="メモ")

    class Config:
        orm_mode = True


class SsapManHours(CustomBaseModel):
    meeting: float = Field(None, title="課内の情報共有を行う会議体への参加および準備（h）")
    study: float = Field(None, title="課内の勉強会への参加および準備（h）")
    learning: float = Field(
        None,
        title="上記に含まれないセミナー・e-Learning・トレーニング・自己研鑽・インプットのための勉強会・講習会・研修への参加及びその準備、時事情報収集（h）",
    )
    new_service: float = Field(None, title="新サービス提案に関わる活動（調査・資料作成・関係者とのMTG）（h）")
    startdash: float = Field(None, title="StartDash改善に向けた対応工数全般（h）")
    improvement: float = Field(
        None, title="上記に含まれない部門内改善業務及び会議体への参加（加速支援レビュー、IECサロン、Be Heard等）（h）"
    )
    ssap: float = Field(
        None,
        title="SSAPが対外的に提供するイベント・サービス運営業務全般（オーディション、SOID、SOIV、FirstFlight運営業務等）（h）",
    )
    qc: float = Field(None, title="品質・QA・CS・コンプライアンス・委員会・セキュリティ関連対応（h）")
    accounting: float = Field(None, title="勤怠処理・精算処理・健康診断（h）")
    management: float = Field(None, title="組織のマネジメント業務（課長・担当者共通）（h）")
    office_work: float = Field(None, title="上記に含まれない会議・事務工数全般（h）")
    others: float = Field(None, title="上記に含まれないその他の工数の合計（h）")
    memo: str = Field(None, title="メモ")

    class Config:
        orm_mode = True


class HolidaysManHours(CustomBaseModel):
    paid_holiday: float = Field(None, title="有給休暇（h）")
    holiday: float = Field(None, title="その他休暇（h）")
    private: float = Field(None, title="休憩・私用外出・家事対応・半休（h）")
    others: float = Field(None, title="上記に含まれない雑務（h）")
    department_others: float = Field(None, title="SSAP以外の部署の業務（h）")
    memo: str = Field(None, title="メモ")

    class Config:
        orm_mode = True


class SummarySupportManHours(CustomBaseModel):
    direct: float = Field(..., title="直接支援計")
    pre: float = Field(..., title="仕込支援計")
    sales: float = Field(..., title="新規・継続獲得計")
    ssap: float = Field(..., title="SSAP業務工数計")
    others: float = Field(..., title="その他計")
    total: float = Field(..., title="合計")

    class Config:
        orm_mode = True


class GetManHourBySupporterUserIdResponse(CustomBaseModel):
    """支援者別工数取得レスポンスクラス"""

    year_month: str = Field(..., title="発生年月", example="2022/01")
    supporter_user_id: str = Field(
        ..., title="支援者ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_name: str = Field(None, title="支援者名")
    supporter_organization_id: str = Field(
        ..., title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organization_name: str = Field(None, title="支援者組織名", example="IST")
    direct_support_man_hours: DirectSupportManHours = Field(..., title="直接支援工数")
    pre_support_man_hours: PreSupportManHours = Field(..., title="仕込支援工数")
    sales_support_man_hours: SalesSupportManHours = Field(..., title="新規・継続案件獲得工数")
    ssap_man_hours: SsapManHours = Field(..., title="SSAP業務工数")
    holidays_man_hours: HolidaysManHours = Field(..., title="休暇その他工数")
    summary_man_hour: SummarySupportManHours = Field(None, title="工数計")
    is_confirm: bool = Field(..., title="工数確定済みフラグ")

    create_id: str = Field(
        ..., title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時", example="2020-10-23T03:21:39.356+09:00"
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


class UpdateManHourBySupporterUserIdQuery(YaerMonthQuery):
    """支援者別工数更新クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example="1", ge=1)


class DirectSupportManHourItemsRequest(CustomBaseModel):
    project_id: str = Field(..., title="案件ID")
    role: SupporterRoleTypeName = Field(..., title="（案件内ごとの支援者の）役割")
    service_type: str = Field(..., title="サービス種別")
    input_man_hour: float = Field(..., title="入力工数（h）")

    class Config:
        orm_mode = True


class DirectSupportManHoursRequest(CustomBaseModel):
    items: List[DirectSupportManHourItemsRequest] = Field(None, title="入力値")
    memo: str = Field(None, title="メモ")

    class Config:
        orm_mode = True


class PreSupportManHourItemsRequest(CustomBaseModel):
    project_id: str = Field(..., title="案件ID")
    role: SupporterRoleTypeName = Field(..., title="（案件内ごとの支援者の）役割")
    service_type: str = Field(..., title="サービス種別")
    input_man_hour: float = Field(..., title="入力工数（h）")

    class Config:
        orm_mode = True


class PreSupportManHoursRequest(CustomBaseModel):
    items: List[PreSupportManHourItemsRequest] = Field(None, title="入力値")
    memo: str = Field(None, title="メモ")

    class Config:
        orm_mode = True


class UpdateManHourBySupporterUserIdRequest(CustomBaseModel):
    """支援者別工数更新リクエストクラス"""

    supporter_organization_id: str = Field(..., title="支援者組織ID")
    direct_support_man_hours: DirectSupportManHoursRequest = Field(..., title="直接支援工数")
    pre_support_man_hours: PreSupportManHoursRequest = Field(..., title="仕込支援工数")
    sales_support_man_hours: SalesSupportManHours = Field(..., title="新規・継続案件獲得工数")
    ssap_man_hours: SsapManHours = Field(..., title="SSAP業務工数")
    holidays_man_hours: HolidaysManHours = Field(..., title="休暇その他工数")
    is_confirm: bool = Field(..., title="工数確定済みフラグ")

    class Config:
        orm_mode = True


class UpdateManHourBySupporterUserIdResponse(CustomBaseModel):
    """支援者別工数更新レスポンスクラス"""

    message: str = Field(..., title="実行結果")


class SupporterOrganizationTotal(CustomBaseModel):
    """月次工数分類別工数一覧レスポンスクラス"""

    supporter_organization_id: str = Field(..., title="支援者組織ID")
    supporter_organization_name: str = Field(..., title="支援者組織略名")
    man_hour: float = Field(None, title="工数計")

    class Config:
        orm_mode = True


class Supporters(CustomBaseModel):
    """月次工数分類別工数一覧レスポンスクラス"""

    id: str = Field(..., title="支援者ID")
    name: str = Field(..., title="支援者名")
    man_hour: float = Field(..., title="工数")

    class Config:
        orm_mode = True


class SupporterOrganizationMonHours(CustomBaseModel):
    """月次工数分類別工数一覧レスポンスクラス"""

    supporter_organization_id: str = Field(..., title="支援者組織ID")
    supporter_organization_name: str = Field(..., title="支援者組織略名")
    supporters: List[Supporters] = Field(None, title="支援者別工数")

    class Config:
        orm_mode = True


class GetSummaryManHourTypeHeader(CustomBaseModel):
    """月次工数分類別工数一覧レスポンスヘッダークラス"""

    supporter_organization_total: List[SupporterOrganizationTotal] = Field(
        ..., title="支援者組織合計"
    )
    supporter_organization_man_hours: List[SupporterOrganizationMonHours] = Field(
        ..., title="支援者組織別稼働工数"
    )

    class Config:
        orm_mode = True


class SummaryManHours(CustomBaseModel):
    """月次工数分類別工数一覧レスポンスクラス"""

    man_hour_type_name: str = Field(..., title="工数分類名（分類1）")
    sub_name: str = Field(None, title="取引先名+案件名or工数内訳（分類2）")
    service_type_name: str = Field(None, title="サービス種別名")
    role_name: SupporterRoleTypeName = Field(None, title="役割名")
    supporter_organization_total: List[SupporterOrganizationTotal] = Field(
        ..., title="支援者組織合計"
    )
    supporter_organization_man_hours: List[SupporterOrganizationMonHours] = Field(
        ..., title="支援者組織別稼働工数"
    )

    class Config:
        orm_mode = True


class GetSummaryManHourTypeResponse(CustomBaseModel):
    """月次工数分類別工数一覧レスポンスクラス"""

    year_month: str = Field(..., title="発生年月", example="2022/01")
    header: GetSummaryManHourTypeHeader = Field(..., title="ヘッダー(フロントエンド用)")
    man_hours: List[SummaryManHours] = Field(..., title="工数")

    class Config:
        orm_mode = True


class GetSummarySupporterManHoursQuery(YaerMonthQuery):
    supporter_organization_id: str = Query(
        None,
        title="支援者組織（課）ID<br>カンマ区切りで複数のIDを渡すことを可能とする",
        example="supporterOrganizationId=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d,89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )


class SummarySupporterManHour(CustomBaseModel):
    """月次担当者別工数一覧レスポンスクラス"""

    direct: float = Field(..., title="対面支援計（1. 対面支援）")
    pre: float = Field(..., title="支援仕込計（2. 支援仕込）")
    sales: float = Field(..., title="商談/提案準備計（3. 商談/提案準備）")
    ssap: float = Field(..., title="内部業務計（4. 内部業務）")
    others: float = Field(..., title="休憩・その他計（5. 休憩・その他）")
    total: float = Field(..., title="稼働合計")

    class Config:
        orm_mode = True


class SummarySupporterContractTime(CustomBaseModel):
    """月次担当者別工数一覧レスポンスクラス"""

    producer: float = Field(..., title="主担当（プロデューサー）")
    accelerator: float = Field(..., title="他担当（アクセラレータ）")
    total: float = Field(..., title="合計")

    class Config:
        orm_mode = True


class SummarySupporterManHours(CustomBaseModel):
    """月次担当者別工数一覧レスポンスクラス"""

    supporter_organization_id: str = Field(None, title="支援者組織ID")
    supporter_organization_name: str = Field(..., title="支援者組織略名")
    supporter_id: str = Field(None, title="支援者ID")
    supporter_name: str = Field(..., title="支援者名")
    summary_man_hour: SummarySupporterManHour = Field(..., title="工数分類別計（h）")
    contract_time: SummarySupporterContractTime = Field(..., title="契約時間")
    is_confirm: bool = Field(..., title="工数確定済みフラグ")

    class Config:
        orm_mode = True


class GetSummarySupporterManHoursResponse(CustomBaseModel):
    """月次担当者別工数一覧レスポンスクラス"""

    year_month: str = Field(..., title="発生年月", example="2022/01")
    man_hours: List[SummarySupporterManHours] = Field(..., title="工数")

    class Config:
        orm_mode = True
