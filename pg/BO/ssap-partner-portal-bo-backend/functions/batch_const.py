from enum import Enum, StrEnum


class BatchFunctionName:
    """バッチ関数名"""

    SUMMARY_MAN_HOUR_BATCH = "BO工数情報集計データ作成処理"
    SUMMARY_SURVEY_BATCH = "BOアンケート情報集計データ作成処理"
    AUTOMATIC_LINK_BATCH = "BO取引先・案件情報自動連携処理"
    DOWNLOAD_DELIVERABLES_BATCH = "BO成果物ダウンロード処理"
    DOWNLOAD_DELIVERABLES_PROJECT_BATCH = "BO案件成果物ダウンロード処理"


class BatchFunctionId:
    """バッチ関数ID"""

    SUMMARY_MAN_HOUR_BATCH = (
        "partnerportal-backoffice-{landscape}-batch_summary_man_hour"
    )
    SUMMARY_SURVEY_BATCH = "partnerportal-backoffice-{landscape}-batch_summary_survey"
    AUTOMATIC_LINK_BATCH = "partnerportal-backoffice-{landscape}-batch_automatic_link"
    DOWNLOAD_DELIVERABLES_BATCH = (
        "partnerportal-backoffice-{landscape}-batch_download_deliverables"
    )
    DOWNLOAD_DELIVERABLES_PROJECT_BATCH = (
        "partnerportal-backoffice-{landscape}-batch_download_deliverables_project"
    )


class BatchInputParameterModeForSummaryManHour:
    """入力パラメータ(mode): BO工数情報集計データ作成処理"""

    PROJECT = "project"
    SERVICE_TYPE = "service_type"
    SUPPORTER_ORGANIZATION = "supporter_organization"


class BatchInputParameterModeForSummarySurvey:
    """入力パラメータ(mode): BOアンケート情報集計データ作成処理"""

    USER = "user"
    SUPPORTER_ORGANIZATION = "supporter_organization"
    ALL = "all"


class BatchStatus:
    """バッチ処理ステータス"""

    EXECUTED = "executed"
    RUNNING = "running"
    ERROR = "error"


class BatchSettingConst:
    """バッチ処理設定"""

    # 再処理可能期間(15分)
    BATCH_RERUN_SPAN = 15


class MailType:
    """メール送信の共通定義"""

    BATCH_ERROR_MAIL = "batch_error_mail"
    BATCH_DATA_LINKAGE_ERROR = "batch_data_linkage_error"
    PROJECT_REGISTRATION_COMPLETED = "project_registration_completed"
    PROJECT_ASSIGN = "project_assign"
    CUSTOMER_IMPORT_COMPLETED = "customer_import_completed"
    PROJECT_IMPORT_COMPLETED = "project_import_completed"
    BATCH_DOWNLOAD_DELIVERABLES_ERROR_MAIL = "batch_download_deliverables_error_mail"


class MasterDataType:
    """マスターデータ種別"""

    MASTER_SUPPORTER_ORGANIZATION = "master_supporter_organization"
    MASTER_SERVICE_TYPE = "master_service_type"
    ALERT_SETTING = "alert_setting"
    BATCH_CONTROL = "batch_control"


class ManHourDataTypePrefix:
    """支援工数のデータ区分プレフィックス(セパレータ含む)"""

    SUPPORTER = "supporter#"
    PROJECT_SUMMARY = "project_summary#"
    SERVICE_TYPE_SUMMARY = "service_type_summary#"
    SUPPORTER_ORGANIZATION_SUMMARY = "supporter_organization_summary#"


class SurveySummaryDataTypePrefix:
    """アンケートサマリのデータ区分プレフィックス(セパレータ含む)"""

    USER = "user#"
    SUPPORTER_ORGANIZATION = "supporter_organization#"
    ALL = "all"


class DataType:
    """データ区分"""

    PROJECT = "project"
    SURVEY = "survey"
    USER = "user"
    CUSTOMER = "customer"
    ADMIN = "admin"


class SupporterRoleType:
    """支援者の案件での役割"""

    PRODUCER = "プロデューサー"
    ACCELERATOR = "アクセラレーター"


class RoundSetting:
    """端数処理関連の定数"""

    # 端数処理後の小数点以下桁数
    DECIMAL_DIGITS = 0.1
    # 比率等の小数値の小数点以下桁数
    DECIMAL_DIGITS_RATE_COLUMN = 0.001


class ContractType:
    """契約形態"""

    FOR_VALUE = "有償"
    FREE_OF_CHARGE = "無償"


class SurveyType:
    """アンケート種別"""

    SERVICE = "service"
    COMPLETION = "completion"
    QUICK = "quick"
    PP = "pp"


class SurveyUnansweredColumn:
    """案件アンケート情報の未回答指標の値"""

    # 満足度
    SATISFACTION = "satisfaction"
    # 継続意思
    CONTINUATION = "continuation"
    # 紹介可能性
    RECOMMENDED = "recommended"
    # 営業評価
    SALES = "sales"
    # アンケート機能満足度
    SURVEY_SATISFACTION = "survey_satisfaction"
    # 工数機能満足度
    MAN_HOUR_SATISFACTION = "man_hour_satisfaction"
    # カルテ機能満足度
    KARTE_SATISFACTION = "karte_satisfaction"
    # マスターカルテ機能満足度
    MASTER_KARTE_SATISFACTION = "master_karte_satisfaction"


class HourMinutes:
    """時分の共通定義"""

    MINIMUM = "00:00"
    MAXIMUM = "23:59"


class NotificationType(StrEnum):
    """通知種別"""

    PROJECT_REGISTRATION = "案件情報新規登録"
    PROJECT_ASSIGN = "案件アサイン"
    SALESFORCE_CUSTOMER_IMPORT = "Salesforceお客様情報インポート"
    SALESFORCE_PROJECT_IMPORT = "Salesforce案件情報インポート"


class ThreadPoolMaxWorkers:
    """並列処理の並列数"""

    BATCH_AUTOMATIC_LINK = 3


class TimezoneType:
    """pytzに使用するタイムゾーン"""

    ASIA_TOKYO = "Asia/Tokyo"
    UTC = "UTC"


class BoAppUrl:
    """BOのURL"""

    CUSTOMER_DETAIL = "customer/{customerId}"
    PROJECT_DETAIL = "project/{projectId}"


class FoAppUrl:
    """FOのURL"""

    PROJECT_DETAIL = "project/{projectId}"
    KARTE_LIST = "karte/list/{projectId}"


class Delimiter:
    """区切り文字"""

    PLUS = "+"


class ProjectCreateNewType(StrEnum):
    """案件情報の「新規・更新」項目の値"""

    UPDATE = "01. 更新"
    NEW = "02. 新規"


class ProjectPhaseType(StrEnum):
    """案件のフェーズ"""

    PLAN_PRESENTATION = "プラン提示(D)"
    COST_ESTIMATE_PRESENTATION = "見積もり提示(C)"
    VERBAL_INFORMAL_CONSENT = "内諾(B)"
    INTERNAL_SETTLEMENT_FINISHED = "内部決済完了(A)"
    LOST_ORDER = "失注(G)"
    OPEN = "Open"
    IN_ACTIVE = "活動中(E)"
    DROP = "Drop(F)"


class QuestionnaireType(StrEnum):
    """（アンケート）今月の種類"""

    SERVICE_SURVEY = "サービスアンケート"
    COMPLETION_SURVEY = "修了アンケート"
    NOT_THIS_MONTH_DUE_TO_WRITE_REASON_ON_RIGHT = "今月はなし→理由を右に記載下さい"
    NOT_THIS_MONTH_DUE_TO_CUSTOMER_REQUESTS = "今月はなし（お客様要望）"
    NOT_THIS_MONTH_DUE_TO_TOO_EARLY = "今月はなし（支援開始当初で時期尚早）"
    NOT_THIS_MONTH_DUE_TO_NO_CS_SETTING = "今月はなし（CS設定無し）"
    NOT_THIS_MONTH_DUE_TO_SAME_CUSTOMER_SAME_CS_SET = (
        "今月はなし（同一顧客同一CS設定済み）"
    )
    NOT_THIS_MONTH_DUE_TO_INTERFARE_WITH_THE_NEXT = (
        "今月はなし（お客様要望で次回商談に支障をきたす）"
    )
    QUESTIONNAIRE_SECRETARIAT_UNCONFIRMED = "アンケート事務局未確認"
    SERVICE_SURVEY_PARTNER_PORTAL = "サービスアンケート(PartnerPortal)"
    COMPLETION_SURVEY_PARTNER_PORTAL = "修了アンケート(PartnerPortal)"
    SERVICE_SURVEY_EXCEL = "サービスアンケート(Excel)"
    COMPLETION_SURVEY_EXCEL = "修了アンケート(Excel)"


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

    def __init__(self, key, value):
        self.key = key
        self.label = value


class UNSET_QUESTIONNAIRE_REASON_LENGTH:
    """（アンケート）送付しない理由の文字列長"""

    MAX = 255


class SurveyPasswordSetting:
    """アンケートパスワードの設定"""

    # パスワードの長さ
    PASSWORD_LENGTH = 12


class PfCustomerInfoColumnName:
    """PFの取引先情報のカラム名"""

    SALESFORCE_CUSTOMER_ID = "Salesforce取引先ID"
    PARTNER_PORTAL_LAST_UPDATE_DATE = "PartnerPortal最終更新日時(取引先)"
    CUSTOMER_NAME = "企業名"
    CATEGORY = "カテゴリ"
    CUSTOMER_OWNER = "取引先所有者"
    INDUSTRY_SEGMENT = "業界セグメント"


class PfProjectInfoColumnName:
    """PFの案件情報のカラム名"""

    SALESFORCE_OPPORTUNITY_ID = "Salesforce商談ID"
    SALESFORCE_CUSTOMER_ID = "Salesforce取引先ID"
    PARTNER_PORTAL_LAST_UPDATE_DATE = "PartnerPortal最終更新日時(商談)"
    PARTNER_PORTAL_PROJECT_NAME = "商談名(アンケート,PartnerPortalで使用)"
    PROJECT_NAME = "商談名"
    CUSTOMER_NAME = "取引先名"
    SERVICE_TYPE = "サービス区分名"
    PROFIT = "粗利メイン課"
    CREATE_NEW = "新規・更新"
    CUSTOMER_MANAGER_MAIL_ADDRESS = "取引先責任者メールアドレス"
    PROJECT_OWNER = "商談所有者"
    PHASE = "フェーズ"
    SUPPORT_DATE_FROM = "支援開始日"
    SUPPORT_DATE_TO = "支援修了日"
    PARTNER_PORTAL_PROFIT = "PartnerPortal_FY粗利"
    PARTNER_PORTAL_GROSS = "PartnerPortal_FY売上"
    SERVICE_MANAGER = "サービス責任者"
    DIRECT_SUPPORT_MAN_HOUR = "契約時間（対面支援工数）"
    QUESTIONNAIRE_TYPE = "（アンケート）今月の種類"
    UNSENT_QUESTIONNAIRE_REASON = "（アンケート）送付しない理由"


class ProjectColumnNameForUpdateLog(Enum):
    """自動連携バッチの案件情報の更新履歴/メール通知用カラム名
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

    SERVICE_MANAGER_NAME = ("service_manager_name", "サービス責任者")

    # 以下は連携項目に存在しない項目（TBL更新時の更新履歴用）
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
    THIS_MONTH_TYPE = (
        "this_month_type",
        "（アンケート）今月の種類",
    )
    NO_SEND_REASON = (
        "no_send_reason",
        "（アンケート）今月の種類",
    )

    def __init__(self, tbl_column_name, log_column_name):
        self.tbl_column_name = tbl_column_name
        self.log_column_name = log_column_name


class BatchErrorType:
    """バッチエラータイプ"""

    OVERFLOW_FILES = "overflow_files"


class SolverIdentifier:
    """ソルバー識別子"""

    # ユーザ名のプレフィックス
    NAME_PREFIX = "（ソルバー）"
