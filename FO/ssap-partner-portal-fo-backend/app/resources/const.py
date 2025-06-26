from enum import Enum, StrEnum

DEFAULT_BIRTHDAY = '1900/01/01'


class AppEnvTypes(Enum):
    """環境"""

    LOCAL: str = "local"
    DEV: str = "dev"
    SUP: str = "sup"
    EVS: str = "evs"
    SQA: str = "sqa"
    PRD: str = "prd"


class DynamoDBOperation:
    """DynamoDBを操作する上での定数"""

    DEFAULT_PAGINATION_LIMIT = 30
    UNDEFINED = "undefined"


class MailType:
    """メール送信の共通定義"""

    USER_REGISTRATION_COMPLETED = "user_registration_completed"
    PROJECT_ASSIGN = "project_assign"
    SURVEY_ANSWER_PROVIDED = "survey_answer_provided"
    EXTERNAL_UPDATE_KARTE = "external_update_karte"
    INTERNAL_UPDATE_KARTE = "internal_update_karte"
    UPDATE_MASTER_KARTE_CURRENT_PROGRAM = "update_master_karte_current_program"
    UPDATE_MASTER_KARTE_NEXT_PROGRAM = "update_master_karte_next_program"
    UPDATE_SOLVER = "update_solver"
    SALESFORCE_DATA_SYNC = "salesforce_data_sync"
    SALESFORCE_DELETE_SOLVER_CORPORATION = "salesforce_delete_solver_corporation"
    MAIL_ERROR = "mail_error"


class SalesforceDataSyncMailType:
    """Salesforce連携メール種別定義"""
    CREATE_SOLVER_CORPORATION = "法人ソルバー新規登録"
    UPDATE_SOLVER_CORPORATION = "法人ソルバー更新"
    CRATE_SOLVER_CANDIDATE = "ソルバー候補人材要件応募"
    UPDATE_SOLVER_CANDIDATE = "ソルバー候補更新"
    APPLY_SOLVER = "個人ソルバー登録申請"
    CREATE_SOLVER = "個人ソルバー新規登録"
    UPDATE_SOLVER = "個人ソルバー更新"
    UPDATE_SOLVER_UTILIZATION_RATE = "個人ソルバー稼働率・単価更新"


class HourMinutes:
    """時分の共通定義"""

    MINIMUM = "00:00"
    MAXIMUM = "23:59"


class DateTimeHourMinutes:
    """年月日時分の共通定義"""

    MINIMUM = "1970/01/01 00:00"
    MAXIMUM = "9999/12/31 23:59"


class DateTime:
    """日付けの共通定義"""

    MINIMUM = "1970/01/01"
    MAXIMUM = "9999/12/31"


class UserRoleType(Enum):
    """ユーザのロール"""

    SYSTEM_ADMIN = ("system_admin", "システム管理者")
    SALES = ("sales", "営業担当者")
    SALES_MGR = ("sales_mgr", "営業責任者")
    SUPPORTER = ("supporter", "支援者")
    SUPPORTER_MGR = ("supporter_mgr", "支援者責任者")
    SURVEY_OPS = ("survey_ops", "アンケート事務局")
    MAN_HOUR_OPS = ("man_hour_ops", "稼働率調査事務局")
    BUSINESS_MGR = ("business_mgr", "事業者責任者")
    CUSTOMER = ("customer", "お客様")
    APT = ("apt", "APT")
    SOLVER_STAFF = ("solver_staff", "法人ソルバー")

    def __init__(self, key, value):
        self.key = key
        self.label = value


class SuggestUsersRole(str, Enum):
    """SuggestUsersクエリのロール"""

    CUSTOMER = "customer"
    SUPPORTER = "supporter"
    SALES = "sales"
    SUPPORTER_MGR = "supporter_mgr"
    SALES_MGR = "sales_mgr"
    SUPPORTER_OR_MGR = "supporter_or_mgr"
    SALES_OR_MGR = "sales_or_mgr"


class DataType:
    """データ種別"""

    ADMIN = "admin"
    USER = "user"
    CUSTOMER = "customer"
    PROJECT = "project"
    SURVEY = "survey"
    SOLVER = "solver"
    SOLVER_CORPORATION = "solver_corporation"
    SOLVER_APPLICATION = "solver_application"


class MasterDataType(Enum):
    """マスターデータ種別"""

    MASTER_SUPPORTER_ORGANIZATION = "master_supporter_organization"
    MASTER_SERVICE_TYPE = "master_service_type"
    ALERT_SETTING = "alert_setting"
    BATCH_CONTROL = "batch_control"
    UPDATE_KARTE_RECORD = "update_karte_record"
    SELECT_ITEMS = "select_items"
    ISSUE_MAP50 = "issue_map50"
    INDUSTRY_SEGMENT = "industry_segment"
    SERVICE_MANAGER = "service_manager"


class SuggestCustomersSortType(str, Enum):
    """取引先サジェスト用情報取得ソート種別"""

    NAME_ASC = "name:asc"


class SurveyTypeForGetSurveys(str, Enum):
    """アンケート種別（サービス修了合算あり）"""

    SERVICE = "service"
    COMPLETION = "completion"
    SERVICE_AND_COMPLETION = "service_and_completion"
    QUICK = "quick"
    PP = "pp"
    NON_PP = "non_pp"


class GetCustomersSortType(str, Enum):
    """取引先一覧取得ソート種別"""

    NAME_ASC = "name:asc"
    NAME_DESC = "name:desc"


class GetSolverCorporationsSortType(str, Enum):
    """法人ソルバー一覧取得ソート種別"""

    NAME_ASC = "name:asc"
    NAME_DESC = "name:desc"


class GetSolversSortType(str, Enum):
    """個人ソルバー一覧取得ソート種別"""

    CREATE_AT_DESC = "create_at:desc"
    SEX_ASC = "sex:asc"
    SEX_DESC = "sex:desc"
    BIRTH_DAY_ASC = "birth_day:asc"
    BIRTH_DAY_DESC = "birth_day:desc"
    REGISTRATION_STATUS_ASC = "registration_status:asc"
    REGISTRATION_STATUS_DESC = "registration_status:desc"
    OPERATION_STATUS_ASC = "operating_status:asc"
    OPERATION_STATUS_DESC = "operating_status:desc"
    PROVIDED_OPERATING_RATE_ASC = "provided_operating_rate:asc"
    PROVIDED_OPERATING_RATE_DESC = "provided_operating_rate:desc"
    PROVIDED_OPERATING_RATE_NEXT_ASC = "provided_operating_rate_next:asc"
    PROVIDED_OPERATING_RATE_NEXT_DESC = "provided_operating_rate_next:desc"
    PRICE_PER_PERSON_MONTH_ASC = "price_per_person_month:asc"
    PRICE_PER_PERSON_MONTH_DESC = "price_per_person_month:desc"
    PRICE_AND_OPERATING_RATE_UPDATE_AT_ASC = "price_and_operating_rate_update_at:asc"
    PRICE_AND_OPERATING_RATE_UPDATE_AT_DESC = "price_and_operating_rate_update_at:desc"


class SurveyType(str, Enum):
    """アンケート種別"""

    SERVICE = "service"
    COMPLETION = "completion"
    QUICK = "quick"
    PP = "pp"


class SurveyTypeExcludingPP(str, Enum):
    """アンケート種別"""

    SERVICE = "service"
    COMPLETION = "completion"
    QUICK = "quick"


class SurveyTypeName:
    """アンケート種別名称"""

    SERVICE = "サービスアンケート"
    COMPLETION = "修了アンケート"
    QUICK = "クイックアンケート"
    PP = "PartnerPortal利用アンケート"


class SurveyTiming(str, Enum):
    """入力フォーマット"""

    MONTHLY = "monthly"
    MONTHLY_NOT_COMPLETION_MONTH = "monthly_not_completion_month"
    COMPLETION_MONTH = "completion_month"
    ANYTIME = "anytime"


class SurveyQuestionsFormat(str, Enum):
    """集計タイプ"""

    CEHCKBOX = "checkbox"
    RADIO = "radio"
    SELECTBOX = "selectbox"
    TEXTAREA = "textarea"


class SupportTiming(str, Enum):
    """スケジュールタイミング種別"""

    MONTHLY = "monthly"
    WEEKLY = "weekly"
    ONCE = "once"


class ScheduleType(str, Enum):
    SUPPORT = "support"
    SURVEY = "survey"


class SurveyQuestionsSummaryType(str, Enum):
    """アンケート頻度"""

    NORMAL = "normal"
    POINT = "point"
    SATISFACTION = "satisfaction"
    CONTINUATION = "continuation"
    RECOMMENDED = "recommended"
    SALES = "sales"
    SURVEY_SATISFACTION = "survey_satisfaction"
    MAN_HOUR_SATISFACTION = "man_hour_satisfaction"
    KARTE_SATISFACTION = "karte_satisfaction"
    MASTER_KARTE_SATISFACTION = "master_karte_satisfaction"


class SurveyRevisionStatusNumber(int, Enum):
    """アンケートバージョンのステータスナンバー"""

    DRAFT_REVISION = 0  # 下書きはバージョン0
    LATEST = 1  # 最新バージョンである場合は1
    NOT_LATEST = 0  # 最新バージョンでない場合は0


class IsLatest:
    """アンケートが最新バージョンかの識別"""

    TRUE = 1
    FALSE = 0


class ContractType(str, Enum):
    """契約形態"""

    FOR_VALUE = "有償"
    FREE_OF_CHARGE = "無償"


class ProjectPhaseType(str, Enum):
    """案件のフェーズ"""

    PLAN_PRESENTATION = "プラン提示(D)"
    COST_ESTIMATE_PRESENTATION = "見積もり提示(C)"
    VERBAL_INFORMAL_CONSENT = "内諾(B)"
    INTERNAL_SETTLEMENT_FINISHED = "内部決済完了(A)"
    LOST_ORDER = "失注(G)"
    OPEN = "Open"
    IN_ACTIVE = "活動中(E)"
    DROP = "Drop(F)"


class GetProjectsSortType(str, Enum):
    """案件一覧取得ソート種別"""

    NAME_ASC = "name:asc"
    CUSTOMER_NAME_ASC = "customerName:asc"
    SUPPORT_DATE_FROM_DESC = "supportDateFrom:desc"


class SuggestProjectsSortType(str, Enum):
    """案件サジェスト用情報取得ソート種別"""

    NAME_ASC = "name:asc"


class GetSurveysByMineSortType(str, Enum):
    """自身のアンケート取得ソート種別"""

    ACTUAL_SURVEY_REQUEST_DATETIME_DESC = "actual_survey_request_datetime:desc"
    PLAN_SURVEY_RESPONSE_DATETIME_DESC = "plan_survey_response_datetime:desc"


class DefaultPageItemCount:
    """ページネーション時に取得するアイテム数(limit)のデフォルト値"""

    limit = 20


class SalesSupportManHourType(str, Enum):
    """アンケート種別"""

    NEW = "new"
    CONTINUATION = "continuation"


class SupporterRoleType(str, Enum):
    """支援者の案件での役割"""

    PRODUCER = "producer"
    ACCELERATOR = "accelerator"


class SupporterRoleTypeName(str, Enum):
    """支援者の案件での役割"""

    PRODUCER = "プロデューサー"
    ACCELERATOR = "アクセラレーター"


class NotificationType(str, Enum):
    """通知種別"""

    PROJECT_ASSIGN = "案件アサイン"
    SURVEY_ANSWER_PROVIDED = "アンケートお客様回答"
    FORGOT_TO_WRITE_KARTEN_REMIND = "個別カルテ書き忘れリマインド通知"
    FORGOT_TO_WRITE_MASTER_KARTEN_CURRENT_PROGRAM_REMIND = (
        "マスターカルテ当期支援入力催促通知"
    )
    FORGOT_TO_WRITE_MASTER_KARTEN_NEXT_PROGRAM_REMIND = (
        "マスターカルテ次期支援入力催促通知"
    )
    SURVEY_ANSWER_REQUEST = "アンケート回答依頼通知"
    PP_SURVEY_ANSWER_REQUEST = "PP利用アンケート回答依頼通知"
    SURVEY_REMIND = "アンケート催促通知"
    MISSING_SUBMISSION_OF_MAN_HOURS = "工数提出漏れ通知"
    UPDATE_KARTE = "個別カルテ入力・更新通知"
    SURVEY_ANONYMOUS_ANSWER_REQUEST = "匿名アンケート回答依頼通知"
    SURVEY_ANONYMOUS_REMIND = "匿名アンケート催促通知"


class FoAppUrl:
    """FOのURL"""

    PROJECT_DETAIL = "project/{projectId}"
    SURVEY_DETAIL = "survey/{surveyId}"
    KARTE_DETAIL = "karte/{karteId}"
    KARTE_LIST = "karte/list/{projectId}"
    MASTER_KARTE_DETAIL = "master-karte/{npfProjectId}"
    MASTER_KARTE_DETAIL_QUERY_PARAM = "?currentProgram={isCurrent}"


class BoAppUrl:
    """BOのURL"""

    SURVEY_ANSWER_RESULT = "survey/{surveyId}"


class ProjectColumnNameForUpdateLog(Enum):
    """テーブル項目名、画面の項目名"""

    CUSTOMER_ID = ("customer_id", "取引先識別ID（お客様識別ID）")
    SALESFORCE_OPPORTUNITY_ID = ("salesforce_opportunity_id", "商談ID")
    # 画面に存在しない項目
    SALESFORCE_CUSTOMER_ID = ("salesforce_customer_id", "")
    SALESFORCE_UPDATE_AT = ("salesforce_update_at", "SF最終更新日時")
    NAME = ("name", "商談名（案件名）")
    CUSTOMER_NAME = ("customer_name", "取引先名（お客様名）")
    SERVICE_TYPE = ("service_type", "サービス区分（サービス名）")
    SUPPORTER_ORGANIZATION_ID = (
        "supporter_organization_id",
        "粗利メイン課（アンケート集計課）",
    )
    # 画面に存在しない項目
    SALESFORCE_USE_PACKAGE = ("salesforce_use_package", "")
    CREATE_NEW = ("create_new", "新規・更新")
    CONTINUED = ("continued", "前年度で契約し期をまたぐ商談")
    # 画面に存在しない項目
    SALESFORCE_VIA_PR = ("salesforce_via_pr", "")
    SALESFORCE_MAIN_CUSTOMER_NAME = (
        "salesforce_main_customer.name",
        "取引先担当者（お客様担当者） 名前",
    )
    SALESFORCE_MAIN_CUSTOMER_EMAIL = (
        "salesforce_main_customer.email",
        "取引先担当者（お客様担当者） メールアドレス",
    )
    SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME = (
        "salesforce_main_customer.organization_name",
        "取引先担当者（お客様担当者） 部署",
    )
    SALESFORCE_MAIN_CUSTOMER_JOB = (
        "salesforce_main_customer.job",
        "取引先担当者（お客様担当者） 役職",
    )
    MAIN_SALES_USER_ID = ("main_sales_user_id", "商談所有者（営業担当者）")
    CONTRACT_DATE = ("contract_date", "完了予定日（契約締結日）")
    PHASE = ("phase", "フェーズ")
    CUSTOMER_SUCCESS = ("customer_success", "カスタマーサクセス")
    SUPPORT_DATE_FROM = ("support_date_from", "支援開始日")
    SUPPORT_DATE_TO = ("support_date_to", "支援終了日")
    # 画面に存在しない項目
    PROFIT_MONTHLY = ("profit.monthly", "")
    # 画面に存在しない項目
    PROFIT_QUARTERLY = ("profit.quarterly", "")
    # 画面に存在しない項目
    PROFIT_HALF = ("profit.half", "")
    PROFIT_YEAR = ("profit.year", "FY粗利")
    # 画面に存在しない項目
    GROSS_MONTHLY = ("gross.monthly", "")
    # 画面に存在しない項目
    GROSS_QUARTERLY = ("gross.quarterly", "")
    # 画面に存在しない項目
    GROSS_HALF = ("gross.half", "")
    # 画面に存在しない項目
    GROSS_YEAR = ("gross.year", "")
    SALESFORCE_MAIN_SUPPORTER_USER_NAME = (
        "salesforce_main_supporter_user_name",
        "プロデューサー（初期値）",
    )
    SALESFORCE_SUPPORTER_USER_NAMES = (
        "salesforce_supporter_user_names",
        "アクセラレーター（初期値）",
    )
    TOTAL_CONTRACT_TIME = ("total_contract_time", "延べ契約時間")
    CONTRACT_TYPE = ("contract_type", "有償/無償")

    # 以下はCSVに存在しない項目（TBL更新時の更新履歴用）
    MAIN_CUSTOMER_USER_ID = ("main_customer_user_id", "")
    CUSTOMER_USER_IDS = ("customer_user_ids", "")
    MAIN_SUPPORTER_USER_ID = ("main_supporter_user_id", "")
    SUPPORTER_USER_IDS = ("supporter_user_ids", "")
    IS_COUNT_MAN_HOUR = ("is_count_man_hour", "")
    IS_KARTE_REMIND = ("is_karte_remind", "")
    IS_MASTER_KARTE_REMIND = ("is_master_karte_remind", "")
    IS_SECRET = ("is_secret", "")

    def __init__(self, tbl_column_name, log_column_name):
        self.tbl_column_name = tbl_column_name
        self.log_column_name = log_column_name


class TtlTime:
    """キャッシュする際のTTL"""

    DEFAULT = 29  # Lambdaの待機時間


class ScheduleTiming(str, Enum):
    """スケジュールタイミング種別"""

    MONTHLY = "monthly"
    WEEKLY = "weekly"
    ONCE = "once"


class SurveyResponseDate(str, Enum):
    """アンケート回答期限日"""

    NONE = 0
    MONTH_END = 99


class DefaultLoginAt:
    "Login_atの作成時デフォルトログイン日時"

    DEFAULT_LOGIN_AT = "2000-1-1 00:00:00"


class CipherAES:
    # AES256: secret_key 256bit(32bytes)
    SECRET_KEY = "327871654807426b58a924bfb32f3b7c"


class JwtSettingInfo:
    """JWT関連の情報"""

    # 暗号化アルゴリズム
    ALGORITHM = "HS256"
    # 暗号化に使用する鍵情報
    # ハッシュ出力のサイズ（e.g.「HS256」の場合は256ビット=32文字）以上の桁数のキーを設定
    SECRET_KEY = "MAB1xeocSpAcP448t1oXnCPQ5QRr7EDK"
    # クエリに付与するパラメータ
    URL_JWT_QUERY = "?s={jwt}"


class SurveyPasswordSetting:
    """アンケートパスワードの設定"""

    # パスワードの長さ
    PASSWORD_LENGTH = 12


class MasterKartenProgramType:
    """マスターカルテのプログラムが当期支援か次期支援か"""

    CURRENT = "current"
    NEXT = "next"


class MasterKartenUpdateUserType:
    """マスターカルテ最終更新者種別"""

    SALESFORCE = "Salesforce"


class MasterKartenCustomerCategory(StrEnum):
    """マスターカルテの顧客セグメント"""

    SONY_GROUP = "ソニーグループ"


class BatchFunctionId:
    """バッチ関数ID"""

    AUTOMATIC_LINK_BATCH = "partnerportal-backoffice-{landscape}-batch_automatic_link"


class KarteLocation(Enum):
    """個別カルテの場所"""

    ONLINE = ("online", 'オンライン')
    HEAD_OFFICE = ("head_office", '品川本社')
    CLIENT_OFFICE = ("client_office", '顧客オフィス')
    OTHER = ("other", 'その他')

    def __init__(self, value: any, label: str):
        self._value_ = value
        self.label = label

    @classmethod
    def get_label(cls, value: str) -> str:
        for member in cls:
            if member.value == value:
                return member.label
        raise ValueError(f"Unknown value: {value}")


class Presence:
    PRESENT = "有"
    ABSENT = "無"
