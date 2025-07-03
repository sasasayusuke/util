from typing import List
from app.schemas.base import CustomBaseModel
from fastapi import Query
from pydantic import Field, root_validator


class GetMasterKartenQuery(CustomBaseModel):
    """マスタ案件カルテの一覧取得クエリパラメータクラス"""

    offset_page: int = Query(..., title="リストの中で何ページ目を取得するか")
    limit: int = Query(None, title="最大取得件数")
    customer_id: str = Query(None, title="取引先ID")
    support_date_from: str = Query(None, title="支援開始日")
    support_date_to: str = Query(None, title="支援終了日")
    all: bool = Query(False, title="全ての案件か担当案件か")
    is_current_program: bool = Query(
        False, title="当期支援か次期支援か。true:当期支援、false:次期支援"
    )
    category: str = Query(None, title="顧客セグメント")
    industry_segment: str = Query(None, title="業界セグメント")
    department_name: str = Query(None, title="部署名")
    current_situation: str = Query(None, title="現状")
    issue: str = Query(None, title="課題")
    customer_success: str = Query(None, title="カスタマーサクセス")
    lineup: str = Query(None, title="ラインナップ")
    required_personal_skill: str = Query(None, title="お客様に不足している人的リソース")
    required_partner: str = Query(None, title="お客様に紹介したい企業や産業")
    strength: str = Query(None, title="自社の強み")
    all: bool = Query(False, title="全ての案件か担当案件か")


class UsageHistory(CustomBaseModel):
    service_type: str = Field(None, title="サービス種別")
    project_name: str = Field(None, title="プロジェクト名")
    npf_project_id: str = Field(None, title="NPF案件ID")


class SatisfactionEvaluation(CustomBaseModel):
    is_answer: bool = Field(None, title="回答の選択か否か")
    title: str = Field(None, title="タイトル")


class FundamentalInformation(CustomBaseModel):
    president_policy: str = Field(None, title="社長方針")
    kpi: str = Field(None, title="KPI")
    to_be_three_years: str = Field(None, title="3年後になっていること")
    current_situation: str = Field(None, title="現状")
    issue: str = Field(None, title="課題")
    request: str = Field(None, title="要望")
    customer_success: str = Field(None, title="カスタマーサクセス")
    customer_success_reuse: bool = Field(None, title="カスタマーサクセスの再利用")
    schedule: str = Field(None, title="支援期間")
    lineup: str = Field(None, title="ラインナップ")
    support_contents: str = Field(None, title="支援内容")
    required_personal_skill: str = Field(None, title="お客様に不足している人的リソース")
    required_partner: str = Field(None, title="お客様に紹介したい企業や産業")
    supplement_human_resource_to_sap: str = Field(None, title="SAP支援チームに補充したい人的リソース")
    current_customer_profile: str = Field(None, title="当該プロジェクトのお客様が現在持っている顧客（像）")
    want_acquire_customer_profile: str = Field(None, title="当該プロジェクトのお客様が今後獲得したい顧客（像）")
    our_strength: str = Field(None, title="自社の強み")
    aspiration: str = Field(None, title="お客様に関する留意事項")
    usage_history: List[UsageHistory] = Field(None, title="利用履歴")


class Result(CustomBaseModel):
    customer_success_result: str = Field(None, title="カスタマーサクセス結果")
    customer_success_result_factor: str = Field(None, title="カスタマーサクセス達成/未達要因")
    next_support_content: str = Field(None, title="次期支援内容")
    support_issue: str = Field(None, title="支援で生じた課題")
    support_success_factor: str = Field(None, title="解決できた要因/解決できなかった要因")
    survey_customer_assessment: str = Field(None, title="アンケート（顧客の評価）")
    survey_ssap_assessment: str = Field(None, title="アンケート（SSAPの自己評価）")
    survey_id: str = Field(None, title="アンケートID")
    satisfaction_evaluation: List[SatisfactionEvaluation] = Field(None, title="満足度評価")
    is_disclosure: bool = Field(None, title="支援者へ開示OK", example=True)


class CompanyDepartment(CustomBaseModel):
    customer_name: str = Field(None, title="取引先名")
    customer_url: str = Field(None, title="取引先URL")
    category: str = Field(None, title="顧客セグメント")
    establishment: str = Field(None, title="設立日")
    employee: str = Field(None, title="従業員数")
    capital_stock: str = Field(None, title="資本金")
    business_summary: str = Field(None, title="事業内容")
    industry_segment: str = Field(None, title="業界セグメント")
    department_id: str = Field(None, title="部署ID")
    department_name: str = Field(None, title="部署名")


class Others(CustomBaseModel):
    mission: str = Field(None, title="ミッション")
    number_of_people: str = Field(None, title="メンバー数")
    manager: str = Field(None, title="マネージャー")
    commercialization_skill: str = Field(None, title="商品化")
    exist_partners: str = Field(None, title="既存パートナー")
    support_order: str = Field(None, title="支援依頼")
    exist_evaluation: str = Field(None, title="既存評価")
    exist_audition: str = Field(None, title="既存オーディション")
    exist_ideation: str = Field(None, title="既存アイディエーション")
    exist_idea_review: str = Field(None, title="既存アイデアレビュー")
    budget: str = Field(None, title="予算")
    human_resource: str = Field(None, title="人材")
    idea: str = Field(None, title="アイデア")
    theme: str = Field(None, title="テーマ")
    client: str = Field(None, title="取引先")
    client_issue: str = Field(None, title="取引先課題")
    solution: str = Field(None, title="ソリューション")
    originality: str = Field(None, title="独自性")
    mvp: str = Field(None, title="MVP")
    tam: str = Field(None, title="TAM")
    sam: str = Field(None, title="SAM")
    is_right_time: str = Field(None, title="タイミング")
    road_map: str = Field(None, title="ロードマップ")


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
    npf_project_id: str = Field(None, title="NPF案件ID")
    pp_project_id: str = Field(None, title="PP案件ID")
    supporter_organization_id: str = Field(None, title="支援組織ID")
    service: str = Field(None, title="サービス")
    project: str = Field(None, title="プロジェクト")
    client: str = Field(None, title="取引先")
    support_date_from: str = Field(None, title="支援開始日")
    support_date_to: str = Field(None, title="支援修了日")
    current_program: CurrentProgram = Field(None, title="当期支援")
    next_program: NextProgram = Field(None, title="次期支援")


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


class SelectBoxItem(CustomBaseModel):
    label: str = Field(None, title="選択肢のラベル", example="アイデア可視化")
    value: str = Field(None, title="選択肢の値", example="idea")


class GetMasterKartenSelectBoxResponse(CustomBaseModel):
    name: str = Field(None, title="選択肢種別名", example="serviceType")
    items: List[SelectBoxItem] = Field([], title="選択肢リスト")


class PutResultCurrentProgram(CustomBaseModel):
    customer_success_result: str = Field(None, title="カスタマーサクセス結果")
    customer_success_result_factor: str = Field(None, title="カスタマーサクセス達成/未達要因")
    next_support_content: str = Field(None, title="次期支援内容")
    support_issue: str = Field(None, title="支援で生じた課題")
    support_success_factor: str = Field(None, title="解決できた要因/解決できなかった要因")
    survey_customer_assessment: str = Field(None, title="アンケート（顧客の評価）")
    survey_ssap_assessment: str = Field(None, title="アンケート（SSAPの自己評価）")


class PutMasterKartenCurrentProgram(CustomBaseModel):
    version: int = Field(..., title="バージョン")
    result: PutResultCurrentProgram = Field(None, title="結果")


class PutNextProgramFundamentalInformation(CustomBaseModel):
    version: int = Field(None, title="バージョン")
    president_policy: str = Field(None, title="社長方針", max_length=1000)
    kpi: str = Field(None, title="KPI", max_length=1000)
    to_be_three_years: str = Field(None, title="3年後になっていること", max_length=1000)
    current_situation: str = Field(None, title="現状", max_length=1000)
    issue: str = Field(None, title="課題", max_length=1000)
    request: str = Field(None, title="要望", max_length=1000)
    customer_success: str = Field(None, title="カスタマーサクセス", max_length=500)
    customer_success_reuse: bool = Field(..., title="カスタマーサクセスリユース有無")
    schedule: str = Field(None, title="支援時期", max_length=500)
    lineup: str = Field(None, title="ラインナップ")
    support_contents: str = Field(None, title="支援内容", max_length=1000)
    required_personal_skill: str = Field(None, title="お客様に不足している人的リソース", max_length=1000)
    required_partner: str = Field(None, title="お客様に紹介したい企業や産業", max_length=1000)
    supplement_human_resource_to_sap: str = Field(None, title="SAP支援チームに補充したい人的リソース")
    current_customer_profile: str = Field(None, title="当該プロジェクトのお客様が現在持っている顧客（像）")
    want_acquire_customer_profile: str = Field(None, title="当該プロジェクトのお客様が今後獲得したい顧客（像）")
    our_strength: str = Field(None, title="自社の強み", max_length=1000)
    aspiration: str = Field(None, title="お客様に関する留意事項", max_length=1000)


class PutNextProgramOthers(CustomBaseModel):
    mission: str = Field(None, title="ミッション", max_length=1000)
    number_of_people: str = Field(None, title="人数", max_length=100)
    manager: str = Field(None, title="責任者情報", max_length=100)
    commercialization_skill: str = Field(None, title="事業化スキル", max_length=100)
    exist_partners: str = Field(None, title="パートナー・外注先の有無", max_length=100)
    support_order: str = Field(None, title="事務局による支援体制", max_length=100)
    exist_evaluation: str = Field(None, title="事業の提案評価プロセスの有無や課題", max_length=100)
    exist_audition: str = Field(None, title="オーディション・社内公募企画の有無や課題", max_length=100)
    exist_ideation: str = Field(None, title="アイディエーション施策の有無や課題", max_length=100)
    exist_idea_review: str = Field(None, title="事業アイデア審査体制の有無や課題", max_length=100)
    budget: str = Field(None, title="予算", max_length=100)
    human_resource: str = Field(None, title="人事制度", max_length=100)
    idea: str = Field(None, title=" アイデアは既にあるか？", max_length=100)
    theme: str = Field(None, title="テーマはあるか？(アイデアがない場合)", max_length=100)
    client: str = Field(None, title="顧客は誰か？", max_length=100)
    client_issue: str = Field(None, title="顧客の課題は何か？", max_length=100)
    solution: str = Field(None, title="顧客の課題に対し、どのようなソリューションか？", max_length=100)
    originality: str = Field(None, title="独自性は何か？", max_length=100)
    mvp: str = Field(None, title="必要不可欠な特徴・機能（MVP）は何か？", max_length=100)
    tam: str = Field(None, title="提供サービスの市場（TAM）の金額規模と将来の伸び率は？", max_length=100)
    sam: str = Field(None, title="ターゲットセグメント（SAM）の金額規模と将来の伸び率は？", max_length=100)
    is_right_time: str = Field(None, title="市場投入のタイミングは適切か？", max_length=100)
    road_map: str = Field(None, title="事業化後の長期戦略やロードマップはあるか？", max_length=100)


class PutMasterKartenNextProgram(CustomBaseModel):
    version: int = Field(..., title="バージョン")
    is_customer_public: bool = Field(False, title="顧客公開か否か")
    fundamental_information: PutNextProgramFundamentalInformation = Field(
        None, title="基本情報"
    )
    others: PutNextProgramOthers = Field(None, title="その他")


class PutMasterKartenByIdRequest(CustomBaseModel):
    npf_project_id: str = Field(..., title="NPF案件ID")
    pp_project_id: str = Field(..., title="PP案件ID")
    current_program: PutMasterKartenCurrentProgram = Field(None, title="当期支援")
    next_program: PutMasterKartenNextProgram = Field(None, title="次期支援")
    is_notify_update_master_karte: bool = Field(..., title="マスターカルテの更新を通知するか否か")

    @root_validator
    def validate_required(cls, v):
        if not v.get("current_program") and not v.get("next_program"):
            raise ValueError("Either currentProgram or nextProgram is required.")

        return v


class PutMasterKartenByIdResponse(GetMasterKartenByIdResponse):
    pass


class PostMasterKartenCurrentProgram(CustomBaseModel):
    result: PutResultCurrentProgram = Field(None, title="結果")


class PostMasterKartenNextProgram(CustomBaseModel):
    is_customer_public: bool = Field(False, title="顧客公開か否か")
    fundamental_information: PutNextProgramFundamentalInformation = Field(
        None, title="基本情報"
    )
    others: PutNextProgramOthers = Field(None, title="その他")


class PostMasterKartenRequest(CustomBaseModel):
    npf_project_id: str = Field(..., title="NPF案件ID")
    pp_project_id: str = Field(..., title="PP案件ID")
    current_program: PutMasterKartenCurrentProgram = Field(None, title="当期支援")
    next_program: PutMasterKartenNextProgram = Field(None, title="次期支援")
    is_notify_update_master_karte: bool = Field(..., title="マスターカルテの更新を通知するか否か")

    @root_validator
    def validate_required(cls, v):
        if not v.get("current_program") and not v.get("next_program"):
            raise ValueError("Either currentProgram or nextProgram is required.")

        return v


class PostMasterKartenResponse(GetMasterKartenByIdResponse):
    pass
