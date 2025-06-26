from datetime import datetime
from typing import List

from fastapi import Query
from pydantic import Field

from app.resources.const import DefaultPageItemCount
from app.schemas.base import CustomBaseModel


class UsageHistory(CustomBaseModel):
    """プログラム利用履歴"""

    service_type: str = Field(None, title="サービス種別")
    project_name: str = Field(None, title="プロジェクト名")
    npf_project_id: str = Field(None, title="NPF案件ID")


class FundamentalInformation(CustomBaseModel):
    """プログラムの基本情報"""

    president_policy: str = Field(None, title="社長方針", example="○○を目指す")
    kpi: str = Field(None, title="ゴール・KPI", example="2025年: 事業化")
    to_be_three_years: str = Field(None, title="3年後のTo Be像", example="○○を達成する")
    current_situation: str = Field(None, title="現状", example="○○ができていない")
    issue: str = Field(None, title="課題", example="○○を解決したい")
    request: str = Field(None, title="リクエスト", example="○○を実現したい")
    customer_success: str = Field(None, title="カスタマーサクセス", example="ユーザー数を10倍にする")
    customer_success_reuse: bool = Field(None, title="カスタマーサクセス(リユース有無)")
    schedule: str = Field(None, title="スケジュール", example="2025年: 事業化")
    lineup: str = Field(None, title="ラインナップ", example="ラインナップ")
    support_contents: str = Field(None, title="支援内容", example="ユーザー数を10倍にする")
    required_personal_skill: str = Field(
        None, title="お客様に不足している人的リソース", example="十分なマーケティング能力を備えていること"
    )
    required_partner: str = Field(None, title="お客様に紹介したい企業や産業", example="潤沢な資金があること")
    supplement_human_resource_to_sap: str = Field(None, title="SAP支援チームに補充したい人的リソース")
    current_customer_profile: str = Field(None, title="当該プロジェクトのお客様が現在持っている顧客（像）")
    want_acquire_customer_profile: str = Field(None, title="当該プロジェクトのお客様が今後獲得したい顧客（像）")
    our_strengths: str = Field(None, title="自社の強み", example="潤沢な資金があること")
    aspiration: str = Field(None, title="お客様に関する留意事項")
    usage_history: List[UsageHistory] = Field(None, title="プログラム利用履歴")


class SatisfactionEvaluation(CustomBaseModel):
    is_answer: bool = Field(None, title="回答の選択か否か")
    title: str = Field(None, title="タイトル")


class Result(CustomBaseModel):
    """プログラムの案件結果・実績情報"""

    customer_success_result: str = Field(None, title="カスタマーサクセス結果")
    customer_success_result_factor: str = Field(None, title="カスタマーサクセス達成/未達要因")
    next_support_content: str = Field(None, title="次期支援内容")
    support_issue: str = Field(None, title="支援で生じた課題")
    support_success_factor: str = Field(None, title="解決できた要因/解決できなかった要因")
    survey_customer_assessment: str = Field(None, title="アンケート（顧客の評価）")
    survey_ssap_assessment: str = Field(None, title="アンケート（SSAPの自己評価）")
    survey_id: str = Field(None, title="アンケートID")
    satisfaction_evaluation: List[SatisfactionEvaluation] = Field(None, title="満足度評価")


class CompanyDepartment(CustomBaseModel):
    """プログラムの企業・部署情報"""

    customer_name: str = Field(None, title="企業名", example="○○株式会社")
    customer_url: str = Field(None, title="企業のホームページURL", example="www.example.com")
    category: str = Field(None, title="顧客セグメント", example="大企業")
    establishment: str = Field(None, title="設立年月日", example="2020年10月")
    employee: str = Field(None, title="従業員数", example="1000")
    capital_stock: str = Field(None, title="資本金", example="1000")
    business_summary: str = Field(None, title="事業概要", example="〇〇を使って社会に貢献する。")
    industry_segment: str = Field(None, title="業界セグメント", example="IT")
    department_id: str = Field(
        None, title="部署ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    department_name: str = Field(None, title="部署名", example="○○部")


class Others(CustomBaseModel):
    """その他チェック事項"""

    mission: str = Field(None, title="部署のミッション・機能", example="新規事業立ち上げ")
    number_of_people: str = Field(None, title="人数", example="実施体制: 10名")
    manager: str = Field(None, title="責任者", example="田中太郎")
    commercialization_skill: str = Field(None, title="事業化スキル", example="○○ができる")
    exist_partners: str = Field(None, title="パートナー・外注先の有無", example="コンサルを起用し、一緒に進めていく")
    support_order: str = Field(None, title="事務局による支援体制", example="3名で支援を行う")
    exist_evaluation: str = Field(
        None, title="事業の提案評価プロセスの有無や課題", example="なし。ただし今後プロセス整備をしたい。"
    )
    exist_audition: str = Field(
        None, title="オーディション・社内公募企画の有無や課題", example="なし。ただし今後プロセス整備をしたい。"
    )
    exist_ideation: str = Field(
        None, title="アイディエーション試作の有無や課題", example="なし。ただし今後プロセス整備をしたい。"
    )
    exist_idea_review: str = Field(
        None, title="事業アイデア審査体制の有無や課題", example="なし。ただし今後プロセス整備をしたい。"
    )
    budget: str = Field(None, title="予算の有無や課題", example="未定。")
    human_resource: str = Field(None, title="人事制度", example="リファラル採用を強めていく")
    idea: str = Field(None, title="アイデアは既にあるか？", example="ある")
    theme: str = Field(None, title="テーマはあるか？(アイデアがない場合)", example="なし")
    client: str = Field(None, title="顧客は誰か", example="ソニーグループ会社")
    client_issue: str = Field(None, title="顧客の課題は何か", example="ユーザーが増えない")
    solution: str = Field(None, title="顧客に対しどのようなソリューションか？", example="未定")
    originality: str = Field(None, title="独自性は何か？", example="未定")
    mvp: str = Field(None, title="必要不可欠な特徴・機能(mvp)は何か？", example="未定")
    tam: str = Field(None, title="提供サービスの市場(tam)の金額規模と将来の伸び率は？", example="未定")
    sam: str = Field(None, title="ターゲットセグメント(sam)の金額規模と将来の伸び率は？", example="未定")
    is_right_time: str = Field(None, title="市場投入のタイミングは適切か？", example="未定")
    road_map: str = Field(None, title="事業化後の長期戦略やロードマップはあるか？", example="未定")


class CurrentProgram(CustomBaseModel):
    id: str = Field(None, title="ID")
    version: int = Field(None, title="バージョン")
    fundamental_information: FundamentalInformation = Field(None, title="基本情報")
    result: Result = Field(None, title="結果")
    company_department: CompanyDepartment = Field(None, title="取引先部署")
    others: Others = Field(None, title="その他")
    last_update_datetime: str = Field(None, title="最終更新日時")
    last_update_by: str = Field(None, title="最終更新者")


class NextProgram(CustomBaseModel):
    id: str = Field(None, title="ID")
    version: int = Field(None, title="バージョン")
    is_customer_public: bool = Field(None, title="顧客公開か否か")
    fundamental_information: FundamentalInformation = Field(None, title="基本情報")
    others: Others = Field(None, title="その他")
    last_update_datetime: str = Field(None, title="最終更新日時")
    last_update_by: str = Field(None, title="最終更新者")


class GetMasterKartenByIdResponse(CustomBaseModel):
    """マスターカルテ詳細情報取得レスポンス"""

    master_karte_id: str = Field(
        ..., title="マスターカルテID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    pp_project_id: str = Field(
        ..., title="PP案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organization_id: str = Field(
        None, title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    service: str = Field(..., title="サービス名", example="組織開発")
    project: str = Field(..., title="案件名", example="○○プロジェクト")
    client: str = Field(..., title="お客様名・部署名", example="ソニーグループ株式会社 人事部")
    support_date_from: str = Field(
        ..., title="支援期間（From）（yyyy/mm/dd）", example="2023/08/30"
    )
    support_date_to: str = Field(
        ..., title="支援期間（To）（yyyy/mm/dd）", example="2023/09/30"
    )
    current_program: CurrentProgram = Field(None, title="当期支援情報")
    next_program: NextProgram = Field(None, title="次期支援情報")


class MasterKartenInfoForGetMasterKarten(CustomBaseModel):
    """GetMasterKartenResponse.kartenのList要素"""

    npf_project_id: str = Field(
        None, title="新PF案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    pp_project_id: str = Field(
        ..., title="PP案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    service_name: str = Field(..., title="サービス名", example="組織開発")
    project: str = Field(..., title="案件名", example="地域活性プロジェクト")
    client: str = Field(..., title="お客様名・部署名", example="ソニーグループ株式会社")
    support_date_from: str = Field(..., title="支援開始日（yyyy/mm/dd）", example="2021/01/30")
    support_date_to: str = Field(..., title="支援終了日（yyyy/mm/dd）", example="2021/02/28")
    is_accessible_karten: bool = Field(..., title="個別カルテにアクセスできるかどうか", example=True)
    is_accessible_master_karten: bool = Field(..., title="マスターカルテにアクセスできるかどうか", example=True)


class GetMasterKartenResponse(CustomBaseModel):
    """マスターカルテの一覧取得レスポンスクラス"""

    offset_page: int = Field(None, title="リストの総数内で何ページ目か", example=1)
    total: int = Field(..., title="マスターカルテ総件数", example=20, ge=0)
    karten: List[MasterKartenInfoForGetMasterKarten] = Field(None, title="マスターカルテ情報")


class GetMasterKartenQuery(CustomBaseModel):
    """マスターカルテの一覧取得クエリパラメータクラス"""

    offset_page: int = Query(..., title="リストの総数内で何ページ目か", example=1)
    limit: int = Query(DefaultPageItemCount.limit, title="最大取得件数", example=20)
    customer_id: str = Query(
        None, title="顧客ID（取引先ID）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    support_date_from: str = Query(
        None, title="支援開始日（yyyy/mm/dd）", example="2021/01/30"
    )
    support_date_to: str = Query(None, title="支援終了日（yyyy/mm/dd）", example="2021/02/28")
    all: bool = Query(None, title="全ての案件か担当案件か", example="False")
    is_current_program: bool = Field(
        None, title="当期支援か次期支援か。(true:当期支援、false:次期支援)", example="True"
    )
    department_name: str = Query(None, title="部署名", example="SGC")
    current_situation: str = Query(None, title="現状", example="順調")
    issue: str = Query(None, title="課題", example="メンバー不足")
    customer_success: str = Query(None, title="カスタマーサクセス", example="利益率120%")
    required_personal_skill: str = Query(
        None, title="お客様に不足している人的リソース", example="十分なマーケティング能力を備えていること"
    )
    required_partner: str = Query(None, title="お客様に紹介したい企業や産業", example="潤沢な資金があること")
    strength: str = Query(None, title="自社の強み", example="マーケティング能力")


class SelectBoxItems(CustomBaseModel):
    label: str = Field(None, title="選択肢のラベル", example="アイデア可視化")
    value: str = Field(None, title="選択肢の値", example="idea")


class GetMasterKartenSelectBoxResponse(CustomBaseModel):
    """マスターカルテ一覧セレクトボックス候補用レスポンス"""

    name: str = Field(None, title="選択肢種別名", example="serviceType")
    items: List[SelectBoxItems] = Field([], title="選択肢リスト")


class GetNpfProjectIdResponse(CustomBaseModel):
    """NPF案件ID取得レスポンスクラス"""

    npf_project_id: str = Field(
        ..., title="NPF案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
