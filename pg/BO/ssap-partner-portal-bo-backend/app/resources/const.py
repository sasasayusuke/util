from enum import Enum


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

    ADMIN_REGISTRATION_COMPLETED = "admin_registration_completed"
    USER_REGISTRATION_COMPLETED = "user_registration_completed"
    OTP_PASSWORD = "otp_password"
    PROJECT_REGISTRATION_COMPLETED = "project_registration_completed"
    PROJECT_ASSIGN = "project_assign"
    CUSTOMER_IMPORT_COMPLETED = "customer_import_completed"
    PROJECT_IMPORT_COMPLETED = "project_import_completed"
    SURVEY_ANSWER_REQUEST = "survey_answer_request"
    SURVEY_ANONYMOUS_ANSWER_REQUEST = "survey_anonymous_answer_request"
    SURVEY_ANONYMOUS_PASSWORD = "survey_anonymous_password"


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


class Date:
    """日付けの共通定義"""

    MINIMUM = "1970/01"
    MAXIMUM = "9999/12"


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
    CUSTOMER = ("customer", "顧客")
    APT = ("apt", "APT")
    SOLVER_STAFF = ("solver_staff", "ソルバー法人")

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
    PROJECT_SUMMARY = "project_summary"
    SURVEY = "survey"
    ALERT_SETTING = "alert_setting"
    SOLVER_CORPORATION = "solver_corporation"


class MasterDataType(Enum):
    """マスターデータ種別"""

    MASTER_SUPPORTER_ORGANIZATION = "master_supporter_organization"
    MASTER_SERVICE_TYPE = "master_service_type"
    ALERT_SETTING = "alert_setting"
    BATCH_CONTROL = "batch_control"
    SERVICE_MANAGER = "service_manager"


class QueryParameterForMasterDataType(str, Enum):
    """クエリパラメータ用のマスターデータ種別"""

    ALL = "all"  # すべてのマスターメンテナンスデータタイプ
    MASTER_SUPPORTER_ORGANIZATION = "master_supporter_organization"
    MASTER_SERVICE_TYPE = "master_service_type"


class SuggestCustomersSortType(str, Enum):
    """取引先サジェスト用情報取得ソート種別"""

    NAME_ASC = "name:asc"


class SuggestSolverCorporationsSortType(str, Enum):
    """法人サジェスト用情報取得ソート種別"""
    NAME_ASC = "name:asc"


class GetSurveysSortType(str, Enum):
    """アンケート一覧ソート種別"""

    PlAN_SURVEY_RESPONSE_DATETIME = "plan_survey_response_datetime:desc"
    ACTUAL_SURVEY_RESPONSE_DATETIME = "actual_survey_response_datetime:desc"


class SurveyTypeForGetSurveys(str, Enum):
    """アンケート種別（サービス修了合算あり）"""

    SERVICE = "service"
    COMPLETION = "completion"
    SERVICE_AND_COMPLETION = "service_and_completion"
    QUICK = "quick"
    PP = "pp"


class SurveyTypeName(str, Enum):
    """アンケート種別（サービス修了合算あり）"""

    SERVICE = "サービスアンケート"
    COMPLETION = "修了アンケート"
    SERVICE_AND_COMPLETION = "サービスアンケート修了アンケート合算"
    QUICK = "クイックアンケート"
    PP = "PartnerPortalアンケート"


class SendSurveyTypeName(str, Enum):
    """アンケート種別（サービス修了合算あり）"""

    SERVICE = "サービス評価"
    COMPLETION = "修了評価"
    QUICK = "クイック評価"


class ContractType(str, Enum):
    """契約形態"""

    FOR_VALUE = "有償"
    FREE_OF_CHARGE = "無償"


class ProjectMemberRole(str, Enum):
    """案件担当メンバーの役割"""

    MAIN = "プロデューサー"
    SUB = "アクセラレーター"


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


class ScheduleTiming(str, Enum):
    """スケジュールタイミング種別"""

    MONTHLY = "monthly"
    WEEKLY = "weekly"
    ONCE = "once"


class ScheduleType(str, Enum):
    SUPPORT = "support"
    SURVEY = "survey"


class SurveyTypeExcludingPP(str, Enum):
    """アンケート種別"""

    SERVICE = "service"
    COMPLETION = "completion"
    QUICK = "quick"


class SurveyLimitDateType(str, Enum):
    """回答期限日タイプ"""

    LAST_DATE = "last_date"
    SPECIFIED_DATE = "specified_date"
    NONE = "none"


class SurveyType(str, Enum):
    """アンケート種別"""

    SERVICE = "service"
    COMPLETION = "completion"
    QUICK = "quick"
    PP = "pp"


class SurveySummaryType(str, Enum):
    """アンケートサマリ種別"""

    ALL = "all"


class SurveyAnswerSummaryType(str, Enum):
    """アンケート設問の集計タイプのマッピング"""

    NORMAL = "通常の設問"
    POINT = "点数形式"
    SATISFACTION = "満足度"
    CONTINUATION = "継続意思"
    RECOMMENDED = "紹介可能性"
    SALES = "営業評価"
    SURVEY_SATISFACTION = "アンケート満足度"
    MAN_HOUR_SATISFACTION = "工数機能満足度"
    KARTE_SATISFACTION = "カルテ機能満足度"
    MASTER_KARTE_SATISFACTION = "マスターカルテ機能満足度"


class SurveyTiming(str, Enum):
    """アンケート頻度"""

    MONTHLY = "monthly"
    MONTHLY_NOT_COMPLETION_MONTH = "monthly_not_completion_month"
    COMPLETION_MONTH = "completion_month"
    ANYTIME = "anytime"


class SurveyQuestionsFormat(str, Enum):
    """入力フォーマット"""

    CEHCKBOX = "checkbox"
    RADIO = "radio"
    SELECTBOX = "selectbox"
    TEXTAREA = "textarea"


class SurveyQuestionsSummaryType(str, Enum):
    """集計タイプ"""

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


class SurveyRevisionStatus(str, Enum):
    """アンケートバージョンのステータス"""

    DRAFT = "draft"
    ARCHIVE = "archive"
    IN_OPERATION = "in_operation"


class SurveyRevisionStatusNumber(int, Enum):
    """アンケートバージョンのステータスナンバー"""

    DRAFT_REVISION = 0  # 下書きはバージョン0
    LATEST = 1  # 最新バージョンである場合は1
    NOT_LATEST = 0  # 最新バージョンでない場合は0


class ExportSurveysModeType(str, Enum):
    """案件アンケートCSV出力モード種別"""

    RAW = "raw"
    SUPPORTER = "supporter"
    ORGANIZATION = "organization"


class DefaultPageItemCount:
    """ページネーション時に取得するアイテム数(limit)のデフォルト値"""

    limit = 20


class SuggestProjectsSortType(str, Enum):
    """案件サジェスト用情報取得ソート種別"""

    NAME_ASC = "name:asc"


class NotificationType(str, Enum):
    """通知種別"""

    PROJECT_REGISTRATION = "案件情報新規登録"
    PROJECT_ASSIGN = "案件アサイン"
    SALESFORCE_CUSTOMER_IMPORT = "Salesforceお客様情報インポート"
    SALESFORCE_PROJECT_IMPORT = "Salesforce案件情報インポート"
    SURVEY_ANSWER_REQUEST = "アンケート回答依頼通知"
    SURVEY_ANONYMOUS_ANSWER_REQUEST = "匿名アンケート回答依頼通知"
    SURVEY_ANONYMOUS_REMIND = "匿名アンケート催促通知"


class GetCustomersSortType(str, Enum):
    """取引先一覧取得ソート種別"""

    NAME_ASC = "name:asc"
    NAME_DESC = "name:desc"
    SALESFORCE_UPDATE_AT_ASC = "salesforceUpdateAt:asc"
    SALESFORCE_UPDATE_AT_DESC = "salesforceUpdateAt:desc"
    UPDATE_AT_ASC = "updateAt:asc"
    UPDATE_AT_DESC = "updateAt:desc"


class SupportStatusType(str, Enum):
    """支援ステータス"""

    BEFORE = "before"
    DURING = "during"
    AFTER = "after"


class GetProjectsSortType(str, Enum):
    """案件一覧取得ソート種別"""

    NAME_ASC = "name:asc"
    NAME_DESC = "name:desc"


class ImportModeType(str, Enum):
    """インポート処理モード"""

    CHECK = "check"
    EXECUTE = "execute"


class ImportResultType(str, Enum):
    """インポート結果"""

    OK = "ok"
    NG = "ng"
    DONE = "done"
    ERROR = "error"


class ImportFileEncodingType:
    """インポートCSVファイルの文字コード"""

    CP932 = "cp932"


class ImportFileCustomerColumnName:
    """取引先CSVファイルのカラム名"""

    NAME = "取引先名"
    CATEGORY = "カテゴリ"
    SALESFORCE_CUSTOMER_ID = "取引先 ID"
    SALESFORCE_UPDATE_AT = "PP最終更新日時(取引先)"
    SALESFORCE_TARGET = "戦略ターゲット・コアターゲット"
    SALESFORCE_CREDIT_LIMIT = "月与信上限"
    SALESFORCE_CREDIT_GET_MONTH = "与信決裁取得年月"
    SALESFORCE_CREDIT_MANAGER = "与信取得担当者"
    SALESFORCE_CREDIT_NO_RETRY = "与信再取得不要"
    SALESFORCE_PAWS_CREDIT_NUMBER = "PAWS決裁番号(与信)"
    SALESFORCE_CUSTOMER_OWNER = "取引先 所有者"
    SALESFORCE_CUSTOMER_SEGMENT = "業界セグメント"


class TimezoneType:
    """pytzに使用するタイムゾーン"""

    ASIA_TOKYO = "Asia/Tokyo"
    UTC = "UTC"


class FoAppUrl:
    """FOのURL"""

    PROJECT_DETAIL = "project/{projectId}"
    KARTE_LIST = "karte/list/{projectId}"
    SURVEY_DETAIL = "survey/{surveyId}"
    ANONYMOUS_SURVEY = "anonymous-survey/auth"


class BoAppUrl:
    """BOのURL"""

    CUSTOMER_DETAIL = "customer/{customerId}"
    PROJECT_DETAIL = "project/{projectId}"


class ThreadPoolMaxWorkers:
    """並列処理の並列数"""

    IMPORT_CUSTOMERS = 3
    IMPORT_PROJECTS = 3


class ImportFileProjectColumnName:
    """案件CSVファイルのカラム名"""

    SALESFORCE_OPPORTUNITY_ID = "商談 ID"
    SALESFORCE_CUSTOMER_ID = "取引先 ID"
    SALESFORCE_UPDATE_AT = "PP最終更新日時（商談）"
    NAME = "商談名(アンケート,PartnerPortalで使用)"
    PJ_NAME_IN_SURVEY = "商談名"
    CUSTOMER_NAME = "取引先名"
    SERVICE_TYPE = "サービス区分"
    SUPPORTER_ORGANIZATION_SHORT_NAME = "(アンケート)粗利メイン課"
    SALESFORCE_USE_PACKAGE = "PKG利用"
    CREATE_NEW = "新規・更新"
    CONTINUED = (
        "支援期間が前年度をまたぐ際はチェックを入れ、別途前年度の商談を登録してください"
    )
    SALESFORCE_VIA_PR = "PR経由"
    SALESFORCE_MAIN_CUSTOMER_NAME = "取引先責任者"
    SALESFORCE_MAIN_CUSTOMER_EMAIL = "取引先責任者メールアドレス"
    SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME = "取引先責任者所属部署"
    SALESFORCE_MAIN_CUSTOMER_JOB = "取引先責任者役職"
    MAIN_SALES_USER_NAME = "商談 所有者"
    CONTRACT_DATE = "完了予定日"
    PHASE = "フェーズ"
    CUSTOMER_SUCCESS = "カスタマーサクセス"
    SUPPORT_DATE_FROM = "支援開始日"
    SUPPORT_DATE_TO = "支援終了日"
    PROFIT_MONTHLY_04 = "PP_04粗利"
    PROFIT_MONTHLY_05 = "PP_05粗利"
    PROFIT_MONTHLY_06 = "PP_06粗利"
    PROFIT_MONTHLY_07 = "PP_07粗利"
    PROFIT_MONTHLY_08 = "PP_08粗利"
    PROFIT_MONTHLY_09 = "PP_09粗利"
    PROFIT_MONTHLY_10 = "PP_10粗利"
    PROFIT_MONTHLY_11 = "PP_11粗利"
    PROFIT_MONTHLY_12 = "PP_12粗利"
    PROFIT_MONTHLY_01 = "PP_01粗利"
    PROFIT_MONTHLY_02 = "PP_02粗利"
    PROFIT_MONTHLY_03 = "PP_03粗利"
    PROFIT_QUARTERLY_1Q = "PP_1Q粗利"
    PROFIT_QUARTERLY_2Q = "PP_2Q粗利"
    PROFIT_QUARTERLY_3Q = "PP_3Q粗利"
    PROFIT_QUARTERLY_4Q = "PP_4Q粗利"
    PROFIT_HALF_1H = "PP_1H粗利"
    PROFIT_HALF_2H = "PP_2H粗利"
    PROFIT_YEAR = "PP_FY粗利"
    GROSS_MONTHLY_04 = "PP_04売上"
    GROSS_MONTHLY_05 = "PP_05売上"
    GROSS_MONTHLY_06 = "PP_06売上"
    GROSS_MONTHLY_07 = "PP_07売上"
    GROSS_MONTHLY_08 = "PP_08売上"
    GROSS_MONTHLY_09 = "PP_09売上"
    GROSS_MONTHLY_10 = "PP_10売上"
    GROSS_MONTHLY_11 = "PP_11売上"
    GROSS_MONTHLY_12 = "PP_12売上"
    GROSS_MONTHLY_01 = "PP_01売上"
    GROSS_MONTHLY_02 = "PP_02売上"
    GROSS_MONTHLY_03 = "PP_03売上"
    GROSS_QUARTERLY_1Q = "PP_1Q売上"
    GROSS_QUARTERLY_2Q = "PP_2Q売上"
    GROSS_QUARTERLY_3Q = "PP_3Q売上"
    GROSS_QUARTERLY_4Q = "PP_4Q売上"
    GROSS_HALF_1H = "PP_1H売上"
    GROSS_HALF_2H = "PP_2H売上"
    GROSS_YEAR = "PP_FY売上"
    SALESFORCE_MAIN_SUPPORTER_USER_NAME = "プロデューサー(主担当1名)"
    SALESFORCE_SUPPORTER_USER_NAMES = "アクセラレーター(副担当複数名)"
    TOTAL_CONTRACT_TIME = "契約時間（対面支援工数）"


class ProjectCreateNewType(str, Enum):
    """案件情報の「新規・更新」項目の値"""

    UPDATE = "01. 更新"
    NEW = "02. 新規"


class ProjectColumnNameForUpdateLog(Enum):
    """案件CSVファイルImport時の更新履歴/メール通知用カラム名
    テーブル項目名、画面の項目名
    """

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
    IS_SOLVER_PROJECT = ("is_solver_project", "")
    DEDICATED_SURVEY_USER_NAME = (
        "dedicated_survey_user_name",
        "匿名アンケート送信設定 名前",
    )
    DEDICATED_SURVEY_USER_EMAIL = (
        "dedicated_survey_user_email",
        "匿名アンケート送信設定 メールアドレス",
    )
    SURVEY_PASSWORD = (
        "survey_password",
        "匿名アンケートパスワード",
    )
    IS_SURVEY_EMAIL_TO_SALESFORCE_MAIN_CUSTOMER = (
        "is_survey_email_to_salesforce_main_customer",
        "匿名アンケートの宛先に指定する",
    )

    def __init__(self, tbl_column_name, log_column_name):
        self.tbl_column_name = tbl_column_name
        self.log_column_name = log_column_name


class TtlTime:
    """キャッシュする際のTTL"""

    DEFAULT = 29  # Lambdaの待機時間


class SupporterRoleType(Enum):
    """支援者の案件での役割"""

    PRODUCER = ("producer", "プロデューサー")
    ACCELERATOR = ("accelerator", "アクセラレーター")


class SupporterRoleTypeName(str, Enum):
    """支援者の案件での役割"""

    PRODUCER = "プロデューサー"
    ACCELERATOR = "アクセラレーター"


class SalesSupportManHourType(str, Enum):
    """アンケート種別"""

    NEW = "new"
    CONTINUATION = "continuation"


class SummaryManHourType(str, Enum):
    """工数分類"""

    DIRECT_SUPPORT = "1. 対面支援"
    PRE_SUPPORT = "2. 支援仕込"
    SALES_SUPPORT = "3. 商談/提案準備"
    SSAP = "4. 内部業務"
    HOLIDAYS = "5. 休憩その他"


class SurveyResponseDate(str, Enum):
    """アンケート回答期限日"""

    NONE = 0
    MONTH_END = 99


class SurveyCsvAttributeOfRaw(Enum):
    """ExportSurveys(RAW)の属性値"""

    DATA_TYPE_H = ("data_type", "H")
    DATA_TYPE_D = ("data_type", "D")
    PROJECT_SURVEY_ID = ("project_survey_id", "案件アンケートID")
    SURVEY_MASTER_ID = "survey_master_id"
    SURVEY_MASTER_NAME = ("survey_master_name", "アンケートひな型名")
    SURVEY_MASTER_REVISION = ("survey_master_revision", "アンケートひな型バージョン")
    SURVEY_TYPE_NAME = ("survey_type_name", "アンケート種別")
    COMPANY = ("company", "会社名")
    PROJECT_NAME = ("project_name", "案件名")
    SUPPORTER_ORGANIZATION_NAME = ("supporter_organization_name", "粗利メイン課")
    SERVICE_TYPE_NAME = ("service_type_name", "サービス名")
    CUSTOMER_SUCCESS = ("customer_success", "カスタマーサクセス")
    ANSWER_USER_NAME = ("answer_user_name", "回答者")
    MAIN_SUPPORTER_USER = ("main_supporter_user", "プロデューサー")
    SUPPORTER_USER = ("supporter_user", "アクセラレーター")
    IS_SOLVER_PROJECT = ("is_solver_project", "ソルバー担当フラグ")
    SALES_USER_NAME = ("sales_user_name", "営業")
    SUMMARY_MONTH = ("summary_month", "集計月")
    PLAN_SURVEY_REQUEST_DATETIME = (
        "plan_survey_request_datetime",
        "回答（送信）依頼予定日時",
    )
    ACTUAL_SURVEY_REQUEST_DATETIME = (
        "actual_survey_request_datetime",
        "回答（送信）依頼実績日時",
    )
    PLAN_SURVEY_RESPONSE_DATETIME = (
        "plan_survey_response_datetime",
        "回答（受信）予定日時",
    )
    ACTUAL_SURVEY_RESPONSE_DATETIME = (
        "actual_survey_response_datetime",
        "回答（受信）実績日時",
    )
    IS_DISCLOSURE = ("is_disclosure", "開示承認")
    IS_NOT_SUMMARY = ("is_not_summary", "集計対象")
    ANSWERS_COUNT = ("answers_count", "設定設問数")
    ANSWER_ID = ("answer_id", "設問ID")
    ANSWER_STR = "answer_str"
    ANSWER_POINT = ("answer_point", "回答（数値表現）")
    ANSWER_SUMMARY_TYPE = ("answer_summary_type", "集計タイプ")
    ANSWER_OTHER_INPUT = ("answer_other_input", "回答任意入力")
    YES = "はい"
    NO = "いいえ"
    YES_SOLVER = "Yes"
    NO_SOLVER = "No"
    APPROVE = "承認"
    NOT_APPROVE = "非承認"
    INCLUDED = "対象"
    EXCLUDED = "非対象"


class SurveyCsvAttributeOfSupporter(Enum):
    """ExportSurveys(Supporter)の属性値"""

    ACCELERATOR_NAME = "アクセラレーター氏名"
    SUPPORTER_ORGANIZATION_NAME = "課名 （支援者が属する課）"
    SURVEY_TYPE = "survey_type"
    SERVICE_SURVEY_TOTAL_SATISFACTION = "サービスアンケート総合満足度"
    SERVICE_SURVEY_N = "サービスアンケートN数"
    COMPLETION_SURVEY_TOTAL_SATISFACTION = "修了アンケート総合満足度"
    COMPLETION_SURVEY_CONTINUATION = "修了アンケート継続意思"
    COMPLETION_SURVEY_RECOMMENDED = "修了アンケート紹介可能性"
    COMPLETION_SURVEY_N = "修了アンケートN数"
    TOTAL_SATISFACTION_EVALUATION = "総合満足度評価"
    TOTAL_SATISFACTION_N = "総合満足度N数"


class SurveyCsvAttributeOfOrganization(Enum):
    """ExportSurveys(Organization)の属性値"""

    SUPPORTER_ORGANIZATION_NAME = "課名 （粗利メイン課）"
    PRODUCER_NAME = "プロデューサー氏名"
    SURVEY_TYPE = "survey_type"
    SERVICE_SURVEY_TOTAL_SATISFACTION = "サービスアンケート総合満足度"
    SERVICE_SURVEY_N = "サービスアンケートN数"
    COMPLETION_SURVEY_TOTAL_SATISFACTION = "修了アンケート総合満足度"
    COMPLETION_SURVEY_CONTINUATION = "修了アンケート継続意思"
    COMPLETION_SURVEY_RECOMMENDED = "修了アンケート紹介可能性"
    COMPLETION_SURVEY_N = "修了アンケートN数"
    TOTAL_SATISFACTION_EVALUATION = "総合満足度評価"
    TOTAL_SATISFACTION_N = "総合満足度N数"


class CsvFormatName(str, Enum):
    """CSVファイルのフォーマット名"""

    RAW_SURVEY = "アンケートデータ"
    SUPPORTER_SURVEY = "支援者別アンケート集計"
    SUPPORTER_ORGANIZATION_SURVEY = "課別アンケート集計"


class S3PresignedExpire(str, Enum):
    """S3の署名付きURLの有効期限"""

    DEFAULT = 60


class DefaultLoginAt:
    "Login_atの作成時デフォルトログイン日時"

    DEFAULT_LOGIN_AT = "2000-1-1 00:00:00"


class ImportFileLimitCount:
    """インポートCSVファイルの上限件数"""

    LIMIT = 500


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


class SurveyUserType(str, Enum):
    """アンケート送付先種類"""

    REGISTERED_MAIN_CUSTOMER = "登録済のお客様代表"
    SALESFORCE_MAIN_CUSTOMER = "Salesforce取引先責任者"
    MANUAL_SETTING = "手動設定"


class MasterKartenProgramType:
    """マスターカルテのプログラムが当期支援か次期支援か"""

    CURRENT = "current"
    NEXT = "next"


class MasterKartenUpdateUserType:
    """マスターカルテ最終更新者種別"""

    SALESFORCE = "Salesforce"


class SolverIdentifier:
    """ソルバー識別子"""

    # ユーザ名のプレフィックス
    NAME_PREFIX = "（ソルバー）"


class BatchFunctionId:
    """バッチ関数ID"""

    AUTOMATIC_LINK_BATCH = "partnerportal-backoffice-{landscape}-batch_automatic_link"
