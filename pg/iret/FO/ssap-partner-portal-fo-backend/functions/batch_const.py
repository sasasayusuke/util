from enum import Enum


class DataType:
    """データ区分"""

    PROJECT = "project"
    SURVEY = "survey"


class MasterDataType(str, Enum):
    """マスターデータ種別"""

    MASTER_SUPPORTER_ORGANIZATION = "master_supporter_organization"
    MASTER_SERVICE_TYPE = "master_service_type"
    SERVICE_MANAGER = "service_manager"
    ALERT_SETTING = "alert_setting"
    BATCH_CONTROL = "batch_control"


class BatchFunctionName:
    """バッチ関数名"""

    REMIND_BATCH = "FO各種リマインド通知処理"


class BatchFunctionId:
    """バッチ関数ID"""

    REMIND_BATCH = "partnerportal-frontoffice-{landscape}-batch_remind"


class BatchInputParameterMode:
    """入力パラメータ(mode)"""

    REMIND_KARTE = "remind_karte"
    SCHEDULE_SURVEY = "schedule_survey"
    REMIND_SURVEY = "remind_survey"
    REMIND_MANHOUR = "remind_manhour"
    REMIND_CURRENT_PROGRAM = "remind_current_program"
    REMIND_NEXT_PROGRAM = "remind_next_program"


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
    FORGOT_TO_WRITE_KARTEN_REMIND = "forgot_to_write_karten_remind"
    FORGOT_TO_WRITE_MASTER_KARTEN_CURRENT_PROGRAM_REMIND = (
        "forgot_to_write_master_karten_current_program_remind"
    )
    FORGOT_TO_WRITE_MASTER_KARTEN_NEXT_PROGRAM_REMIND = (
        "forgot_to_write_master_karten_next_program_remind"
    )
    SURVEY_ANSWER_REQUEST = "survey_answer_request"
    PP_SURVEY_ANSWER_REQUEST = "pp_survey_answer_request"
    SURVEY_REMIND = "survey_remind"
    MISSING_SUBMISSION_OF_MAN_HOURS = "missing_submission_of_man_hours"
    SURVEY_ANONYMOUS_ANSWER_REQUEST = "survey_anonymous_answer_request"
    SURVEY_ANONYMOUS_PASSWORD = "survey_anonymous_password"
    SURVEY_ANONYMOUS_REMIND = "survey_anonymous_remind"


class FoAppUrl:
    """FOのURL"""

    KARTE_LIST = "karte/list/{projectId}"
    SURVEY_DETAIL = "survey/{surveyId}"
    SURVEY_PP_DETAIL = "survey/pp/{surveyId}"
    MAN_HOUR_INPUT = "man-hour?year={year}&month={month}"
    ANONYMOUS_SURVEY = "anonymous-survey/auth"
    MASTER_KARTE_DETAIL = "master-karte/{npf_project_id}"
    SATISFACTION_SURVEY = "satisfaction-survey"


class NotificationType(str, Enum):
    """通知種別"""

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
    SURVEY_ANONYMOUS_ANSWER_REQUEST = "匿名アンケート回答依頼通知"
    SURVEY_ANONYMOUS_REMIND = "匿名アンケート催促通知"


class SurveyType(str, Enum):
    """アンケート種別"""

    SERVICE = "service"
    COMPLETION = "completion"
    QUICK = "quick"
    PP = "pp"


class SurveyTypeName:
    """アンケート種別名称"""

    SERVICE = "サービス評価"
    COMPLETION = "修了評価"
    QUICK = "クイック評価"
    PP = "PartnerPortal利用アンケート"


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
    SOLVER_STAFF = ("solver_staff", "法人ソルバー")

    def __init__(self, key, value):
        self.key = key
        self.label = value


class JwtSettingInfo:
    """JWT関連の情報"""

    # 暗号化アルゴリズム
    ALGORITHM = "HS256"
    # 暗号化に使用する鍵情報
    # ハッシュ出力のサイズ（e.g.「HS256」の場合は256ビット=32文字）以上の桁数のキーを設定
    SECRET_KEY = "MAB1xeocSpAcP448t1oXnCPQ5QRr7EDK"
    # クエリに付与するパラメータ
    URL_JWT_QUERY = "?s={jwt}"


class CipherAES:
    # AES256: secret_key 256bit(32bytes)
    SECRET_KEY = "327871654807426b58a924bfb32f3b7c"
