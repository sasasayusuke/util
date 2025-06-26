from datetime import datetime
from typing import List

from app.resources.const import SalesSupportManHourType
from app.schemas.base import CustomBaseModel
from fastapi import Query
from pydantic import Field


class DirectSupportManHourItems(CustomBaseModel):
    project_id: str = Field(..., title="案件ID")
    project_name: str = Field(..., title="案件名")
    role: str = Field(..., title="（案件内ごとの支援者の）役割")
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
    role: str = Field(..., title="（案件内ごとの支援者の）役割")
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


class GetManHourByMineQuery(CustomBaseModel):
    """支援者別工数取得クエリクラス"""

    year: int = Query(..., title="対象年", example="2021", ge=2000, le=3000)
    month: int = Query(..., title="対象月", example="7", ge=1, le=12)


class GetManHourByMineResponse(CustomBaseModel):
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
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(None, title="作成日時", example="2020-10-23T03:21:39.356Z")
    update_id: str = Field(
        None, title="作成者ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(None, title="更新日時", example="2020-10-23T03:21:39.356Z")
    version: int = Field(None, title="楽観ロックバージョン", example=1)

    class Config:
        orm_mode = True


class GetSummaryProjectSupporterManHourStatusQuery(CustomBaseModel):
    """案件別工数取得クエリクラス"""

    summary_month: str = Field(None, title="集計月", example="202201")
    supporter_user_id: str = Field(
        ..., title="支援者ユーザーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )


class GetSummaryProjectSupporterManHourStatusResponse(CustomBaseModel):
    """案件別工数取得レスポンスクラス"""

    project_id: str = Field(..., title="案件ID")
    project_name: str = Field(..., title="案件名")
    customer_id: str = Field(..., title="取引先ID")
    customer_name: str = Field(..., title="取引先名")
    supporter_organization_id: str = Field(..., title="支援者組織ID")
    service_type: str = Field(..., title="サービス区分（組織開発orアイデア可視化of社会実装PoC）")
    support_date_from: str = Field(..., title="支援期間（From）（yyyy/mm/dd）")
    support_date_to: str = Field(..., title="支援期間（To）（yyyy/mm/dd）")
    total_contract_time: float = Field(..., title="契約時間（合計）：AA時間")
    this_month_contract_time: float = Field(..., title="契約時間（当月）：A時間")
    summary_supporter_direct_support_man_hour: float = Field(
        ..., title="対面支援時間 本人（合計）：BB時間"
    )
    this_month_supporter_direct_support_man_hour: float = Field(
        ..., title="対面支援時間 本人（当月）：B時間"
    )
    summary_direct_support_man_hour: float = Field(..., title="対面支援時間 全担当者（合計）：CC時間")
    this_month_direct_support_man_hour: float = Field(..., title="対面支援時間 全担当者（当月）：C時間")
    summary_direct_support_man_hour_limit: float = Field(
        ..., title="対面支援時間 全担当者（合計）：上限目安：DD時間"
    )
    this_month_direct_support_man_hour_limit: float = Field(
        ..., title="対面支援時間 全担当者（当月）：上限目安：D時間"
    )

    summary_supporter_pre_support_man_hour: float = Field(
        ..., title="仕込み時間 本人（合計）：EE時間"
    )
    this_month_supporter_pre_support_man_hour: float = Field(
        ..., title="仕込み時間 本人（当月）：E時間"
    )
    summary_pre_support_man_hour: float = Field(..., title="仕込み時間 全担当者（合計）：GG時間")
    this_month_pre_support_man_hour: float = Field(..., title="仕込み時間 全担当者（当月）：G時間")
    summary_pre_support_man_hour_limit: float = Field(
        ..., title="仕込み時間 全担当者（合計）：上限目安：HH時間"
    )
    this_month_pre_support_man_hour_limit: float = Field(
        ..., title="仕込み時間 全担当者（当月）：上限目安：H時間"
    )


class DirectSupportManHourItemsRequest(CustomBaseModel):
    project_id: str = Field(..., title="案件ID")
    role: str = Field(..., title="（案件内ごとの支援者の）役割")
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
    role: str = Field(..., title="（案件内ごとの支援者の）役割")
    service_type: str = Field(..., title="サービス種別")
    input_man_hour: float = Field(..., title="入力工数（h）")

    class Config:
        orm_mode = True


class PreSupportManHoursRequest(CustomBaseModel):
    items: List[PreSupportManHourItemsRequest] = Field(None, title="入力値")
    memo: str = Field(None, title="メモ")

    class Config:
        orm_mode = True


class UpdateManHourByMineQuery(CustomBaseModel):
    """支援者工数更新クエリクラス"""

    year: int = Query(..., title="対象年", example="2021", ge=2000, le=3000)
    month: int = Query(..., title="対象月", example="7", ge=1, le=12)
    version: int = Query(..., title="ロックキー（楽観ロック制御）", example="1", ge=1)


class UpdateManHourByMineRequest(CustomBaseModel):
    """支援者工数更新リクエストクラス"""

    supporter_user_name: str = Field(None, title="支援者名")
    supporter_organization_id: str = Field(..., title="支援者組織ID")
    supporter_organization_name: str = Field(None, title="支援者組織名")
    direct_support_man_hours: DirectSupportManHoursRequest = Field(..., title="直接支援工数")
    pre_support_man_hours: PreSupportManHoursRequest = Field(..., title="仕込支援工数")
    sales_support_man_hours: SalesSupportManHours = Field(..., title="新規・継続案件獲得工数")
    ssap_man_hours: SsapManHours = Field(..., title="SSAP業務工数")
    holidays_man_hours: HolidaysManHours = Field(..., title="休暇その他工数")
    is_confirm: bool = Field(..., title="工数確定済みフラグ")

    class Config:
        orm_mode = True


class UpdateManHourByMineResponse(CustomBaseModel):
    """支援者工数更新レスポンスクラス"""

    message: str = Field(..., title="実行結果")
