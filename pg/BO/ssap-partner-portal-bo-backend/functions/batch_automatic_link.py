import copy
import json
import os
import re
import sys
import threading
import time
import traceback
import uuid
from concurrent.futures import ThreadPoolExecutor
from dataclasses import asdict, dataclass, field
from datetime import datetime
from logging import getLogger

from pynamodb.models import BatchWrite
from pytz import timezone

from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.customer import CustomerModel
from app.models.master import MasterSupporterOrganizationModel
from app.models.notification import NotificationModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
    UpdateAttributesSubAttribute,
    UpdateHistoryAttribute,
)
from app.models.user import UserModel
from app.service.notification_service import NotificationService
from app.utils.cipher_aes import AesCipherUtils
from app.utils.encryption import create_random_survey_password
from app.utils.platform import PlatformApiOperator
from functions.batch_common import (
    batch_end_common_procedure,
    batch_init_common_procedure,
    conv_blank,
    get_operation_datetime,
    is_float,
    is_int,
    send_mail,
    send_mail_batch,
)
from functions.batch_const import (
    UNSET_QUESTIONNAIRE_REASON_LENGTH,
    BatchFunctionId,
    BatchFunctionName,
    BatchStatus,
    BoAppUrl,
    ContractType,
    DataType,
    Delimiter,
    FoAppUrl,
    MailType,
    MasterDataType,
    NotificationType,
    PfCustomerInfoColumnName,
    PfProjectInfoColumnName,
    ProjectCreateNewType,
    ProjectPhaseType,
    QuestionnaireType,
    SurveyPasswordSetting,
    ThreadPoolMaxWorkers,
    TimezoneType,
    UserRoleType,
)
from functions.batch_const import ProjectColumnNameForUpdateLog as ProjectColumn
from functions.batch_exceptions import ExitHandler
from functions.batch_message import Message

try:
    import unzip_requirements
except ImportError:
    pass

# ログ出力設定をロードする
get_app_settings().configure_logging()
logger = getLogger(__name__)

# グローバル変数
# 汎用マスタ:支援者組織情報
master_supporter_organization_list: list[MasterSupporterOrganizationModel] = None
# 汎用マスタ:サービス種別
master_service_type_list: list[MasterSupporterOrganizationModel] = None
# 一般ユーザ情報
user_list: list[UserModel] = None
# 管理者ユーザ情報（有効なユーザ）
admin_list_enable_only: list[AdminModel] = None
# 並列処理時の排他制御（ロック）
lock = threading.Lock()


@dataclass
class ErrorInfo:
    """エラー情報を格納するクラス"""

    column_name: str
    message_list: list[str] = field(default_factory=list)


@dataclass
class PfCustomerInfo:
    """PFから取得した取引先情報（必要な項目のみ抜粋）を格納するクラス"""

    # PF取引先ID
    id: int
    # partnerPortal 取引先ID
    partner_portal_customer_id: str
    # Salesforce 取引先ID
    salesforce_customer_id: str
    # 企業名
    customer_name: str
    # 業界セグメント
    industry_segment: str
    # 取引先所有者
    customer_owner: str
    # カテゴリ
    category: str
    # PartnerPortal最終更新日時(取引先)
    partner_portal_last_update_date: str

    # エラーリスト（バリデーションエラー）
    error_info_list: list[ErrorInfo] = field(default_factory=list)

    def append_error_info_list(self, column_name: str, error_message: str):
        """エラーリスト（バリデーションエラー）にエラー情報を追加します"""
        exist_flag = False
        for error_item in self.error_info_list:
            if error_item.column_name == column_name:
                # 追加するエラー項目がエラー情報に存在する場合、そのエラー情報にエラーメッセージを追加
                error_item.message_list.append(error_message)
                exist_flag = True
                break

        if not exist_flag:
            # 追加するエラー項目がエラー情報に存在しない場合、追加
            self.error_info_list.append(
                ErrorInfo(column_name=column_name, message_list=[error_message])
            )


@dataclass
class PfProjectInfo:
    """PFから取得した案件情報（必要な項目のみ抜粋）を格納するクラス"""

    # PF案件ID
    id: int
    # PartnerPortal側取引先ID
    partner_portal_customer_id: str
    # PartnerPortal側案件ID
    partner_portal_project_id: str
    # Salesforce取引先ID
    salesforce_customer_id: str
    # Salesforce商談ID
    salesforce_opportunity_id: str
    # PartnerPortal最終更新日時(商談)
    partner_portal_last_update_date: str
    # 商談名(アンケート,PartnerPortalで使用)
    partner_portal_project_name: str
    # 商談名
    project_name: str
    # 取引先
    customer_name: str
    # サービス区分名
    service_type: str
    # 粗利メイン課
    profit: str
    # 新規・更新
    create_new: str
    # 取引先責任者
    customer_manager: str
    # 取引先責任者メールアドレス
    customer_manager_mail_address: str
    # 取引先責任者所属部署
    customer_manager_department: str
    # 商談所有者
    project_owner: str
    # フェーズ
    phase: str
    # カスタマーサクセス
    customer_success: str
    # PartnerPortal_FY粗利
    partner_portal_profit: int
    # PartnerPortal_FY売上
    partner_portal_gross: int
    # サービス責任者
    service_manager: str
    # プロデューサー(主担当1名)。複数指定する場合は + を挟む。
    producer: str
    # アクセラレーター(副担当複数名)。複数指定する場合は + を挟む。
    accelerator: str
    # 契約時間（対面支援工数）
    direct_support_man_hour: int
    # 支援開始日（yyyy/mm/dd）
    support_date_from: str
    # 支援終了日（yyyy/mm/dd）
    support_date_to: str
    # （アンケート）送付しない理由
    unsent_questionnaire_reason: str
    # （アンケート）今月の種類
    questionnaire_type: str

    # エラーリスト（バリデーションエラー）
    error_info_list: list[ErrorInfo] = field(default_factory=list)

    def append_error_info_list(self, column_name: str, error_message: str):
        """エラーリスト（バリデーションエラー）にエラー情報を追加します"""
        exist_flag = False
        for error_item in self.error_info_list:
            if error_item.column_name == column_name:
                # 追加するエラー項目がエラー情報に存在する場合、そのエラー情報にエラーメッセージを追加
                error_item.message_list.append(error_message)
                exist_flag = True
                break

        if not exist_flag:
            # 追加するエラー項目がエラー情報に存在しない場合、追加
            self.error_info_list.append(
                ErrorInfo(column_name=column_name, message_list=[error_message])
            )

    def get_accelerator_list(self):
        """accelerator（アクセラレーター）をリスト形式で返却します"""
        ret_list: list = []
        if self.accelerator:
            if Delimiter.PLUS in self.accelerator:
                ret_list.extend(self.accelerator.split(Delimiter.PLUS))
            else:
                ret_list.append(self.accelerator)

        return ret_list


@dataclass
class CustomerNewLog:
    """取引先の新規登録情報"""

    # 取引先名
    customer_name: str
    # お客様情報詳細URL(BO)
    customer_detail_url_bo: str


@dataclass
class UpdateColumnInfo:
    """更新情報"""

    # 更新項目
    update_column: str
    # 更新値
    update_value: str


@dataclass
class CustomerEditLog:
    """取引先の更新情報"""

    # 取引先名
    customer_name: str
    # 更新情報
    update_column_info: list[UpdateColumnInfo] = field(default_factory=list)


@dataclass
class ProjectNewLog:
    """案件の新規登録情報"""

    # 案件名
    project_name: str
    # 案件情報詳細URL(FO)
    fo_project_detail_url: str
    # 案件情報詳細URL(BO
    bo_project_detail_url: str


@dataclass
class ProjectEditLog:
    """案件の更新情報"""

    # 案件名
    project_name: str
    # 更新情報
    update_column_info: list[UpdateColumnInfo] = field(default_factory=list)


def get_pf_data(
    event,
    operation_start_datetime: datetime,
    batch_control_id: str,
    link_customer_list: list[PfCustomerInfo] = [],
    link_project_list: list[PfProjectInfo] = [],
) -> None:
    """PFデータ取得処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
        link_customer_list (list[PfCustomerInfo]): PFから取得した処理対象の取引先情報リスト（当該処理で格納）
        link_project_list (list[PfProjectInfo]): PFから取得した処理対象の案件情報リスト（当該処理で格納）
    """
    ###########################
    # PF案件一覧取得
    ###########################
    logger.info("PF案件一覧取得 開始")
    # 取得条件編集
    # limit, offsetPage
    get_pf_projects_params = {"limit": 100, "offsetPage": 1}
    # 連携可否フラグ: 連携可のみ取得
    get_pf_projects_params["isAlignment"] = True

    # PF案件一覧取得APIの呼び出し
    total_project_cnt = None
    retrieved_cnt = 0
    pf_data_list: list[dict] = []
    platform_api_operator = PlatformApiOperator(is_batch=True)

    while total_project_cnt is None or len(pf_data_list) < retrieved_cnt:
        status_code, pf_projects = platform_api_operator.get_projects(
            params=get_pf_projects_params
        )
        logger.info(f"platform getProjects statusCode: {status_code}")
        if status_code != 200:
            error_msg = "PF案件一覧取得APIの呼び出しに失敗しました。"
            logger.error(error_msg)
            raise Exception(error_msg)
        if len(pf_projects["projects"]) == 0:
            logger.info("PF案件一覧取得の結果が0件のため、処理終了")
            raise ExitHandler()
        pf_data_list.extend(pf_projects["projects"])
        retrieved_cnt = pf_projects["total"]
        get_pf_projects_params["offsetPage"] += 1

        if total_project_cnt is None:
            total_project_cnt = pf_projects["total"]

    logger.info(f"PFデータ（PF案件一覧）の取得件数: {len(pf_data_list)}件")
    logger.info("PF案件一覧取得 完了")

    #######################################
    # 処理対象の取引先情報と案件情報を抽出
    #######################################
    logger.info("処理対象の取引先情報と案件情報の抽出 開始")
    for pf_item in pf_data_list:
        # 取引先情報
        pf_customer_dict: dict = pf_item["customer"]
        if pf_customer_dict:
            # 取引先情報は案件情報に紐づく形で取得するため、重複するデータが存在する可能性あり
            # 重複する場合はスキップ
            chk_item_list = [x.id for x in link_customer_list]
            if pf_customer_dict["id"] in chk_item_list:
                continue

            link_customer_list.append(
                PfCustomerInfo(
                    # PF取引先ID
                    id=pf_customer_dict["id"],
                    # partnerPortal 取引先ID
                    partner_portal_customer_id=pf_customer_dict.get(
                        "partnerPortalCustomerId"
                    ),
                    # Salesforce 取引先ID
                    salesforce_customer_id=pf_customer_dict.get("salesforceCustomerId"),
                    # 企業名
                    customer_name=pf_customer_dict.get("customerName"),
                    # 業界セグメント
                    industry_segment=pf_customer_dict.get("industrySegment"),
                    # 取引先所有者
                    customer_owner=pf_customer_dict.get("customerOwner"),
                    # カテゴリ
                    category=pf_customer_dict.get("category"),
                    # PartnerPortal最終更新日時(取引先)
                    partner_portal_last_update_date=pf_customer_dict.get(
                        "partnerPortalLastUpdateDate"
                    ),
                )
            )

    for pf_item in pf_data_list:
        # 案件情報
        pf_project_dict: dict = pf_item["project"]
        if pf_project_dict:
            link_project_list.append(
                PfProjectInfo(
                    # PF案件ID
                    id=pf_project_dict["id"],
                    # PartnerPortal側取引先ID
                    partner_portal_customer_id=pf_project_dict.get(
                        "partnerPortalCustomerId"
                    ),
                    # PartnerPortal側案件ID
                    partner_portal_project_id=pf_project_dict.get(
                        "partnerPortalProjectId"
                    ),
                    # Salesforce取引先ID
                    salesforce_customer_id=pf_project_dict.get("salesforceCustomerId"),
                    # Salesforce商談ID
                    salesforce_opportunity_id=pf_project_dict.get(
                        "salesforceOpportunityId"
                    ),
                    # PartnerPortal最終更新日時(商談)
                    partner_portal_last_update_date=pf_project_dict.get(
                        "partnerPortalLastUpdateDate"
                    ),
                    # 商談名(アンケート,PartnerPortalで使用)
                    partner_portal_project_name=pf_project_dict.get(
                        "partnerPortalProjectName"
                    ),
                    # 商談名
                    project_name=pf_project_dict.get("projectName"),
                    # 取引先
                    customer_name=pf_project_dict.get("customerName"),
                    # サービス区分名
                    service_type=pf_project_dict.get("serviceType"),
                    # 粗利メイン課
                    profit=pf_project_dict.get("profit"),
                    # 新規・更新
                    create_new=pf_project_dict.get("createNew"),
                    # 取引先責任者
                    customer_manager=pf_project_dict.get("customerManager"),
                    # 取引先責任者メールアドレス
                    customer_manager_mail_address=pf_project_dict.get(
                        "customerManagerMailAddress"
                    ),
                    # 取引先責任者所属部署
                    customer_manager_department=pf_project_dict.get(
                        "customerManagerDepartment"
                    ),
                    # 商談所有者
                    project_owner=pf_project_dict.get("projectOwner"),
                    # フェーズ
                    phase=pf_project_dict.get("phase"),
                    # カスタマーサクセス
                    customer_success=pf_project_dict.get("customerSuccess"),
                    # PartnerPortal_FY粗利
                    partner_portal_profit=pf_project_dict.get("partnerPortalProfit"),
                    # PartnerPortal_FY売上
                    partner_portal_gross=pf_project_dict.get("partnerPortalGross"),
                    # サービス責任者
                    service_manager=pf_project_dict.get("serviceManager"),
                    # プロデューサー(主担当1名)。複数指定する場合は + を挟む。
                    producer=pf_project_dict.get("producer"),
                    # アクセラレーター(副担当複数名)。複数指定する場合は + を挟む。
                    accelerator=pf_project_dict.get("accelerator"),
                    # 契約時間（対面支援工数）
                    direct_support_man_hour=pf_project_dict.get("directSupportManHour"),
                    # 支援開始日（yyyy-mm-dd）
                    support_date_from=pf_project_dict.get("supportDateFrom"),
                    # 支援終了日（yyyy-mm-dd）
                    support_date_to=pf_project_dict.get("supportDateTo"),
                    # （アンケート）送付しない理由
                    unsent_questionnaire_reason=pf_project_dict.get(
                        "unsentQuestionnaireReason"
                    ),
                    # （アンケート）今月の種類
                    questionnaire_type=pf_project_dict.get("questionnaireType"),
                )
            )

    logger.info("処理対象の取引先情報と案件情報の抽出 完了")

    logger.info(f"処理対象の取引先情報の件数: {len(link_customer_list)}件")
    logger.info(f"処理対象の案件情報の件数: {len(link_project_list)}件")

    return link_customer_list, link_project_list


def check_enum_project_phase_type(target: str) -> bool:
    """フェーズの列挙値チェック"""
    phases = [c.value for _, c in ProjectPhaseType.__members__.items()]
    if target not in phases:
        return False
    return True


def check_enum_questionnaire_type(target: str) -> bool:
    """「（アンケート）今月の種類」の列挙値チェック"""
    phases = [c.value for _, c in QuestionnaireType.__members__.items()]
    if target not in phases:
        return False
    return True


def remove_prefix_num(column_value: str) -> str:
    """項目値から先頭に付与されたコード値を除去

    Args:
        column_value (str): 先頭にコード値が付与された項目値
            e.g. 「新規・更新」項目の値("01. 更新","02. 新規"))
    Returns:
        str: 先頭のコード値を除去した文字列 e.g. 「更新」「新規」
    """
    return re.sub(r"^\d+\.", "", column_value).strip()


def convert_create_new_to_bool(str_create_new: str) -> bool:
    """「新規・更新」項目の値を真理値に変換

    Args:
        str_create_new (str): 「新規・更新」項目の値("01. 更新","02. 新規"))
    Returns:
        bool: True(新規)、False(更新)
    Exception:
        ValueError: 真理値以外の場合

    """
    if str_create_new == ProjectCreateNewType.NEW:
        return True
    if str_create_new == ProjectCreateNewType.UPDATE:
        return False
    raise ValueError("Column 'create_new' is invalid.")


def is_bool_create_new(str_create_new: str) -> bool:
    """「新規・更新」項目の値が真理値に変換可能か判定.
        列挙値チェック.

    Args:
        str_create_new (str): 「新規・更新」項目の値("01. 更新","02. 新規"))
    Returns:
        bool: True("01. 更新","02. 新規"の場合)、False(それ以外)

    """
    """フェーズの列挙値チェック"""
    create_new_list = [c.value for _, c in ProjectCreateNewType.__members__.items()]
    if str_create_new not in create_new_list:
        return False
    return True


def validate_customer(
    event,
    operation_start_datetime: datetime,
    batch_control_id: str,
    link_customer_list: list[PfCustomerInfo],
    registration_customer_list: list[PfCustomerInfo] = [],
    validation_error_customer_list: list[PfCustomerInfo] = [],
) -> None:
    """取引先情報のバリデーション

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
        link_customer_list (list[PfCustomerInfo]): PFから取得した処理対象の取引先情報リスト
        registration_customer_list (list[PfCustomerInfo]): 登録対象の取引先情報リスト（当該処理で格納）
        validation_error_customer_list (list[PfCustomerInfo]): バリデーションエラーの取引先情報リスト（当該処理で格納）
    """
    ##############################
    # 取引先情報のバリデーション
    ##############################
    logger.info("取引先情報のバリデーション 開始")
    for customer_info in link_customer_list:
        # Salesforce 取引先ID（必須チェック）
        if not customer_info.salesforce_customer_id:
            error_column_name = PfCustomerInfoColumnName.SALESFORCE_CUSTOMER_ID
            customer_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )

        # PartnerPortal最終更新日時(取引先)（必須チェック、書式チェック）
        if not customer_info.partner_portal_last_update_date:
            error_column_name = PfCustomerInfoColumnName.PARTNER_PORTAL_LAST_UPDATE_DATE
            customer_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            pattern_match = re.fullmatch(
                r"\d{4}/\d{2}/\d{2} \d{1,2}:\d{2}:\d{2}",
                customer_info.partner_portal_last_update_date,
            )
            if not pattern_match:
                error_column_name = (
                    PfCustomerInfoColumnName.PARTNER_PORTAL_LAST_UPDATE_DATE
                )
                customer_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.FORMAT_ERROR.format(
                        format_value="yyyy/mm/dd hh:mm:ss",
                    ),
                )

            try:
                datetime.strptime(
                    customer_info.partner_portal_last_update_date, "%Y/%m/%d %H:%M:%S"
                )
            except Exception:
                error_column_name = (
                    PfCustomerInfoColumnName.PARTNER_PORTAL_LAST_UPDATE_DATE
                )
                customer_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.DATETIME_ERROR,
                )

        # 企業名（必須チェック）
        if not customer_info.customer_name:
            error_column_name = PfCustomerInfoColumnName.CUSTOMER_NAME
            customer_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )

    logger.info("取引先情報のバリデーション 完了")

    ######################################
    # 登録データとエラーデータの振り分け
    ######################################
    for customer_info in link_customer_list:
        if customer_info.error_info_list:
            # バリデーションエラーがある場合
            validation_error_customer_list.append(customer_info)
        else:
            # バリデーションがOKの場合
            registration_customer_list.append(customer_info)

    logger.info(f"登録対象の取引先情報リスト件数: {len(registration_customer_list)}件")
    logger.info(
        f"バリデーションエラーの取引先情報リスト件数: {len(validation_error_customer_list)}件"
    )

    return registration_customer_list, validation_error_customer_list


def validate_project(
    event,
    operation_start_datetime: datetime,
    batch_control_id: str,
    link_project_list: list[PfProjectInfo],
    registration_project_list: list[PfProjectInfo] = [],
    validation_error_project_list: list[PfProjectInfo] = [],
) -> None:
    """案件情報のバリデーション

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
        link_project_list (list[PfProjectInfo]): PFから取得した処理対象の案件情報リスト
        registration_project_list (list[PfProjectInfo]): 登録対象の案件情報リスト（当該処理で格納）
        validation_error_project_list (list[PfProjectInfo]): バリデーションエラーの案件情報リスト（当該処理で格納）
    """
    # 取引先テーブル取得（全件）
    customer_list: list[CustomerModel] = list(CustomerModel.scan())
    # グローバル変数
    global master_service_type_list, master_supporter_organization_list, user_list

    ##############################
    # 案件情報のバリデーション
    ##############################
    logger.info("案件情報のバリデーション 開始")
    for project_info in link_project_list:
        # Salesforce商談ID（必須チェック）
        if not project_info.salesforce_opportunity_id:
            error_column_name = PfProjectInfoColumnName.SALESFORCE_OPPORTUNITY_ID
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )

        # Salesforce取引先ID（必須チェック、存在チェック）
        customer_id = None
        customer_name = None
        if not project_info.salesforce_customer_id:
            error_column_name = PfProjectInfoColumnName.SALESFORCE_CUSTOMER_ID
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            for customer in customer_list:
                if (
                    customer.salesforce_customer_id
                    == project_info.salesforce_customer_id
                ):
                    customer_id = customer.id
                    customer_name = customer.name
                    break
            if not customer_id:
                error_column_name = PfProjectInfoColumnName.SALESFORCE_CUSTOMER_ID
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.NOT_EXIST_ERROR.format(
                        column_name=error_column_name
                    ),
                )

        # PartnerPortal最終更新日時(商談)（必須チェック、書式チェック）
        if not project_info.partner_portal_last_update_date:
            error_column_name = PfProjectInfoColumnName.PARTNER_PORTAL_LAST_UPDATE_DATE
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            pattern_match = re.fullmatch(
                r"\d{4}/\d{2}/\d{2} \d{1,2}:\d{2}:\d{2}",
                project_info.partner_portal_last_update_date,
            )
            if not pattern_match:
                error_column_name = (
                    PfProjectInfoColumnName.PARTNER_PORTAL_LAST_UPDATE_DATE
                )
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.FORMAT_ERROR.format(
                        format_value="yyyy/mm/dd hh:mm:ss"
                    ),
                )

            try:
                datetime.strptime(
                    project_info.partner_portal_last_update_date, "%Y/%m/%d %H:%M:%S"
                )
            except Exception:
                error_column_name = (
                    PfProjectInfoColumnName.PARTNER_PORTAL_LAST_UPDATE_DATE
                )
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.DATETIME_ERROR,
                )

        # 商談名(アンケート,PartnerPortalで使用)（必須チェック）
        if not project_info.partner_portal_project_name:
            error_column_name = PfProjectInfoColumnName.PARTNER_PORTAL_PROJECT_NAME
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )

        # 取引先名（必須チェック、存在チェック）
        if not project_info.customer_name:
            error_column_name = PfProjectInfoColumnName.CUSTOMER_NAME
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            if project_info.customer_name != customer_name:
                error_column_name = PfProjectInfoColumnName.CUSTOMER_NAME
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.NOT_EXIST_ERROR.format(
                        column_name=error_column_name
                    ),
                )

        # サービス区分名（必須チェック、列挙値チェック）
        if not project_info.service_type:
            error_column_name = PfProjectInfoColumnName.SERVICE_TYPE
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            service_type_id = None
            for master_service in master_service_type_list:
                if project_info.service_type == master_service.name:
                    service_type_id = master_service.id
            if not service_type_id:
                error_column_name = PfProjectInfoColumnName.SERVICE_TYPE
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.NOT_EXIST_ERROR.format(
                        column_name=error_column_name
                    ),
                )

        # (アンケート)粗利メイン課（存在チェック）
        if project_info.profit:
            supporter_organization_id = None
            for master_supporter_org in master_supporter_organization_list:
                if project_info.profit == master_supporter_org.value:
                    supporter_organization_id = master_supporter_org.id
            if not supporter_organization_id:
                error_column_name = PfProjectInfoColumnName.PROFIT
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.NOT_EXIST_ERROR.format(
                        column_name=error_column_name
                    ),
                )

        # 新規・更新（真理値変換可否チェック）
        if project_info.create_new:
            if not is_bool_create_new(project_info.create_new):
                error_column_name = PfProjectInfoColumnName.CREATE_NEW
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.VALUE_ERROR.format(
                        value=f"「{ProjectCreateNewType.UPDATE.value}」または「{ProjectCreateNewType.NEW.value}」",
                    ),
                )

        # 取引先責任者メールアドレス（書式チェック）
        if project_info.customer_manager_mail_address:
            email_pattern = r"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"
            if not re.fullmatch(
                email_pattern, project_info.customer_manager_mail_address
            ):
                error_column_name = (
                    PfProjectInfoColumnName.CUSTOMER_MANAGER_MAIL_ADDRESS
                )
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.EMAIL_FORMAT_ERROR,
                )

        # 商談所有者（必須チェック、存在チェック、有効化チェック）
        if not project_info.project_owner:
            error_column_name = PfProjectInfoColumnName.PROJECT_OWNER
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            main_sales_user_id = None
            for user in user_list:
                if project_info.project_owner == user.name and (
                    user.role == UserRoleType.SALES.key
                    or user.role == UserRoleType.SALES_MGR.key
                ):
                    main_sales_user_id = user.id

                    if user.disabled:
                        error_column_name = PfProjectInfoColumnName.PROJECT_OWNER
                        project_info.append_error_info_list(
                            column_name=error_column_name,
                            error_message=Message.AutomaticLinkCheckError.DISABLED_ERROR,
                        )

            if not main_sales_user_id:
                error_column_name = PfProjectInfoColumnName.PROJECT_OWNER
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.NOT_EXIST_ERROR.format(
                        column_name=error_column_name
                    ),
                )

        # フェーズ（必須チェック、列挙値チェック）
        if not project_info.phase:
            error_column_name = PfProjectInfoColumnName.PHASE
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            if not check_enum_project_phase_type(project_info.phase):
                error_column_name = PfProjectInfoColumnName.PHASE
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.VALUE_ERROR.format(
                        value="規定値"
                    ),
                )

        # 支援開始日（必須チェック、書式チェック）
        if not project_info.support_date_from:
            error_column_name = PfProjectInfoColumnName.SUPPORT_DATE_FROM
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            if not re.fullmatch(
                r"\d{4}/\d{2}/\d{2}",
                project_info.support_date_from,
            ):
                error_column_name = PfProjectInfoColumnName.SUPPORT_DATE_FROM
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.FORMAT_ERROR.format(
                        format_value="yyyy/mm/dd"
                    ),
                )

            try:
                datetime.strptime(project_info.support_date_from, "%Y/%m/%d")
            except Exception:
                error_column_name = PfProjectInfoColumnName.SUPPORT_DATE_FROM
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.DATE_ERROR,
                )

        # 支援終了日（必須チェック、書式チェック）
        if not project_info.support_date_to:
            error_column_name = PfProjectInfoColumnName.SUPPORT_DATE_TO
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            if not re.fullmatch(
                r"\d{4}/\d{2}/\d{2}",
                project_info.support_date_to,
            ):
                error_column_name = PfProjectInfoColumnName.SUPPORT_DATE_TO
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.FORMAT_ERROR.format(
                        format_value="yyyy/mm/dd"
                    ),
                )

            try:
                datetime.strptime(project_info.support_date_to, "%Y/%m/%d")
            except Exception:
                error_column_name = PfProjectInfoColumnName.SUPPORT_DATE_TO
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.DATE_ERROR,
                )

        # PartnerPortal_FY粗利（必須チェック、数値チェック）
        if not project_info.partner_portal_profit:
            error_column_name = PfProjectInfoColumnName.PARTNER_PORTAL_PROFIT
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            if not is_int(project_info.partner_portal_profit):
                error_column_name = PfProjectInfoColumnName.PARTNER_PORTAL_PROFIT
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.VALUE_ERROR.format(
                        value="数値"
                    ),
                )

        # PartnerPortal_FY売上（必須チェック、数値チェック）
        if not project_info.partner_portal_gross:
            error_column_name = PfProjectInfoColumnName.PARTNER_PORTAL_GROSS
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )
        else:
            if not is_int(project_info.partner_portal_gross):
                error_column_name = PfProjectInfoColumnName.PARTNER_PORTAL_GROSS
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.VALUE_ERROR.format(
                        value="数値"
                    ),
                )

        # サービス責任者（必須チェック）
        if not project_info.service_manager:
            error_column_name = PfProjectInfoColumnName.SERVICE_MANAGER
            project_info.append_error_info_list(
                column_name=error_column_name,
                error_message=Message.AutomaticLinkCheckError.REQUIRED_ERROR.format(
                    column_name=error_column_name
                ),
            )

        # 契約時間（対面支援工数）（数値チェック）
        if project_info.direct_support_man_hour:
            if not is_float(project_info.direct_support_man_hour):
                error_column_name = PfProjectInfoColumnName.DIRECT_SUPPORT_MAN_HOUR
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.VALUE_ERROR.format(
                        value="数値"
                    ),
                )

        # （アンケート）今月の種類（列挙値チェック）
        if project_info.questionnaire_type:
            if not check_enum_questionnaire_type(project_info.questionnaire_type):
                error_column_name = PfProjectInfoColumnName.QUESTIONNAIRE_TYPE
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.VALUE_ERROR.format(
                        value="規定値"
                    ),
                )

        # （アンケート）送付しない理由（文字数チェック）
        if project_info.unsent_questionnaire_reason:
            if (
                len(project_info.unsent_questionnaire_reason)
                > UNSET_QUESTIONNAIRE_REASON_LENGTH.MAX
            ):
                error_column_name = PfProjectInfoColumnName.UNSENT_QUESTIONNAIRE_REASON
                project_info.append_error_info_list(
                    column_name=error_column_name,
                    error_message=Message.AutomaticLinkCheckError.DATA_LENGTH_OVER_ERROR.format(
                        value=UNSET_QUESTIONNAIRE_REASON_LENGTH.MAX
                    ),
                )

    logger.info("案件情報のバリデーション 完了")

    ######################################
    # 登録データとエラーデータの振り分け
    ######################################
    for project_info in link_project_list:
        if project_info.error_info_list:
            # バリデーションエラーがある場合
            validation_error_project_list.append(project_info)
        else:
            # バリデーションがOKの場合
            registration_project_list.append(project_info)

    logger.info(f"登録対象の案件情報リスト件数: {len(registration_project_list)}件")
    logger.info(
        f"バリデーションエラーの案件情報リスト件数: {len(validation_error_project_list)}件"
    )

    return registration_project_list, validation_error_project_list


def edit_target_customer(
    target_customer: CustomerModel,
    name: str,
    category: str,
    salesforce_update_at: datetime,
    salesforce_customer_owner: str,
    salesforce_customer_segment: str,
    update_id: str,
    update_at: datetime,
) -> tuple[list[dict], dict]:
    """取引先情報更新時の編集

    Args:
        target_customer (CustomerModel): 更新対象のCustomerModel
        name (str): 取引先名
        category (str): カテゴリ
        salesforce_update_at (datetime): Salesforce最終更新日時
        salesforce_customer_owner (str):Salesforce取引先所有者
        salesforce_customer_segment (str):Salesforce業界セグメント
        update_id (str):更新ユーザID
        update_at (datetime):更新日時

    Returns:
        tuple: 以下の情報のタプル
            row_change_log (list[UpdateColumnInfo]): 更新情報のリスト
            customer_name_update_map (dict): 取引先名更新マップ(key:取引先ID, value:取引先名)
    """
    row_change_log: list[UpdateColumnInfo] = []
    customer_name_update_map: dict = {}
    if target_customer.name != name:
        target_customer.name = name
        row_change_log.append(
            UpdateColumnInfo(
                update_column=PfCustomerInfoColumnName.CUSTOMER_NAME,
                update_value=conv_blank(name),
            )
        )
        # 取引先名の更新情報を格納
        customer_name_update_map[target_customer.id] = name
    if target_customer.category != category:
        target_customer.category = category
        row_change_log.append(
            UpdateColumnInfo(
                update_column=PfCustomerInfoColumnName.CATEGORY,
                update_value=conv_blank(category),
            )
        )
    if target_customer.salesforce_update_at != salesforce_update_at:
        target_customer.salesforce_update_at = salesforce_update_at
        log_value = None
        if salesforce_update_at:
            log_value = salesforce_update_at.strftime("%Y/%m/%d %H:%M:%S")
        row_change_log.append(
            UpdateColumnInfo(
                update_column=PfCustomerInfoColumnName.PARTNER_PORTAL_LAST_UPDATE_DATE,
                update_value=conv_blank(log_value),
            )
        )
    if target_customer.salesforce_customer_owner != salesforce_customer_owner:
        target_customer.salesforce_customer_owner = salesforce_customer_owner
        row_change_log.append(
            UpdateColumnInfo(
                update_column=PfCustomerInfoColumnName.CUSTOMER_OWNER,
                update_value=conv_blank(salesforce_customer_owner),
            )
        )
    if target_customer.salesforce_customer_segment != salesforce_customer_segment:
        target_customer.salesforce_customer_segment = salesforce_customer_segment
        row_change_log.append(
            UpdateColumnInfo(
                update_column=PfCustomerInfoColumnName.INDUSTRY_SEGMENT,
                update_value=conv_blank(salesforce_customer_segment),
            )
        )
    target_customer.update_id = update_id
    target_customer.update_at = update_at
    target_customer.version += 1

    return row_change_log, customer_name_update_map


def import_customer_execute_parallel(
    registration_customer_info: PfCustomerInfo,
    sf_customer_id_map: dict,
    customer_batch: BatchWrite[CustomerModel],
    batch_control_id: str,
    update_datetime: datetime,
    customer_name_update_map: dict,
    edit_log_list: list[CustomerEditLog],
    new_log_list: list[CustomerNewLog],
) -> None:
    """取引先情報の新規登録、更新の処理

    Args:
        registration_customer_info (PfCustomerInfo): 登録対象の取引先情報リスト
        sf_customer_id_map (dict): 取引先情報のDB取得結果から作成した辞書(key: salesforce_customer_id, value: CustomerModel)
        customer_batch (BatchWrite[CustomerModel]): BatchWriteオブジェクト
        batch_control_id (str): バッチ関数ID
        update_datetime (datetime): 更新日時
        customer_name_update_map (dict): 取引先名更新マップ(key:取引先ID, value:取引先名)
        edit_log_list (list[CustomerEditLog]): 更新用ログのリスト
        new_log_list (list[CustomerNewLog]): 新規登録用ログのリスト
    """
    # グローバル変数
    # 並列処理時の排他制御（ロック）
    global lock

    jst = timezone(TimezoneType.ASIA_TOKYO)

    # 登録内容の編集
    name = registration_customer_info.customer_name
    category = registration_customer_info.category
    salesforce_customer_id = registration_customer_info.salesforce_customer_id

    salesforce_update_at = (
        jst.localize(
            datetime.strptime(
                registration_customer_info.partner_portal_last_update_date,
                "%Y/%m/%d %H:%M:%S",
            )
        )
        if registration_customer_info.partner_portal_last_update_date
        else None
    )
    salesforce_customer_owner = registration_customer_info.customer_owner
    salesforce_customer_segment = registration_customer_info.industry_segment

    bo_site_url = get_app_settings().bo_site_url
    is_new: bool = False
    is_update: bool = False
    row_change_log: list[UpdateColumnInfo] = []
    temp_customer_name_update_map: dict = {}

    if salesforce_customer_id not in sf_customer_id_map.keys():
        is_new = True
        # 新規登録
        new_customer = CustomerModel(
            id=str(uuid.uuid4()),
            data_type=DataType.CUSTOMER,
            name=name,
            category=category,
            salesforce_customer_id=salesforce_customer_id,
            salesforce_update_at=salesforce_update_at,
            salesforce_customer_owner=salesforce_customer_owner,
            salesforce_customer_segment=salesforce_customer_segment,
            create_id=batch_control_id,
            create_at=update_datetime,
            update_id=batch_control_id,
            update_at=update_datetime,
            version=1,
        )

    else:
        # 更新
        target_customer: CustomerModel = copy.deepcopy(
            sf_customer_id_map[salesforce_customer_id]
        )
        # DBのSF最終更新日時 < PF取得データのSF最終更新日時の場合のみ、更新
        if target_customer.salesforce_update_at < salesforce_update_at:
            is_update = True
            (
                row_change_log,
                temp_customer_name_update_map,
            ) = edit_target_customer(
                target_customer=target_customer,
                name=name,
                category=category,
                salesforce_update_at=salesforce_update_at,
                salesforce_customer_owner=salesforce_customer_owner,
                salesforce_customer_segment=salesforce_customer_segment,
                update_id=batch_control_id,
                update_at=update_datetime,
            )

    # 並列処理の排他制御
    with lock:
        if is_new:
            customer_batch.save(new_customer)
            # メール/お知らせ通知用のログ編集（新規登録分）
            new_log_list.append(
                CustomerNewLog(
                    customer_name=name,
                    customer_detail_url_bo=bo_site_url
                    + BoAppUrl.CUSTOMER_DETAIL.format(customerId=new_customer.id),
                )
            )
        if is_update:
            customer_batch.save(target_customer)
            # メール/お知らせ通知用のログ編集（更新分）
            edit_log_list.append(
                CustomerEditLog(
                    customer_name=name,
                    update_column_info=row_change_log,
                )
            )
            customer_name_update_map.update(temp_customer_name_update_map)


def update_user_and_project_info(
    batch_control_id: str,
    update_datetime: datetime,
    customer_name_update_map: dict[str, str],
) -> None:
    """一般ユーザテーブル、および案件情報テーブルの更新

    Args:
        batch_control_id (str): バッチ関数ID
        update_datetime (datetime): 更新日時
        customer_name_update_map (dict[str, str]): 取引先名を更新する取引先（案件、一般ユーザ更新用）
            ※key: id(取引先ID), value: name(更新後の取引先名)の辞書
    """
    # 案件テーブルや一般ユーザテーブルの更新
    for customer_id, name in customer_name_update_map.items():
        # ユーザテーブルのcustomer_nameを更新
        with UserModel.batch_write() as user_batch:
            users_iter = UserModel.customer_id_email_index.query(hash_key=customer_id)
            for user in users_iter:
                user.customer_name = name
                user.update_id = batch_control_id
                user.update_at = update_datetime
                user.version += 1
                user_batch.save(user)

        # 案件テーブルのcustomer_nameを更新
        with ProjectModel.batch_write() as project_batch:
            projects_iter = ProjectModel.customer_id_name_index.query(
                hash_key=customer_id
            )
            for project in projects_iter:
                project.customer_name = name
                project.update_history = UpdateHistoryAttribute(
                    update_id=batch_control_id,
                    update_attributes=[
                        UpdateAttributesSubAttribute(
                            attribute="customer_name", value=name
                        )
                    ],
                )
                project.update_id = batch_control_id
                project.update_at = update_datetime
                project.version += 1
                project_batch.save(project)


def send_mail_and_notification_for_import_customer(
    batch_control_id: str,
    new_log_list: list[ProjectNewLog],
    edit_log_list: list[ProjectEditLog],
) -> None:
    """取引先情報の取込処理における各種メール送信及び通知情報の登録

    Args:
        batch_control_id (str): バッチ関数ID
        new_log_list (list[ProjectNewLog]): _description_
        edit_log_list (list[ProjectEditLog]): _description_
    """
    #######################
    # メール・お知らせ通知
    #######################
    if new_log_list or edit_log_list:
        new_log_dict_list = [asdict(x) for x in new_log_list]
        edit_log_dict_list = [asdict(x) for x in edit_log_list]
        # 新規登録または更新がある場合のみ通知
        payload = {
            "add_info_list": new_log_dict_list,
            "update_info_list": edit_log_dict_list,
        }

        ################################################
        # 「Salesforceお客様情報インポート通知」メール
        ###############################################
        to_email_list: list[str] = []
        bo_notification_user_id_list: list[str] = []
        admin_filter_condition = AdminModel.disabled == False  # NOQA
        for admin in AdminModel.scan(filter_condition=admin_filter_condition):
            if (
                UserRoleType.SYSTEM_ADMIN.key in admin.roles
                or UserRoleType.MAN_HOUR_OPS.key in admin.roles
                or UserRoleType.SURVEY_OPS.key in admin.roles
            ):
                to_email_list.append(admin.email)
                bo_notification_user_id_list.append(admin.id)
        # 宛先の重複を削除
        to_email_list = list(set(to_email_list))

        send_mail(
            template=MailType.CUSTOMER_IMPORT_COMPLETED,
            to_addr_list=to_email_list,
            cc_addr_list=[],
            payload=payload,
        )

        ################################################
        # お知らせ「Salesforceお客様情報インポート通知」
        ################################################
        # BO
        # 重複を削除
        bo_notification_user_id_list = list(set(bo_notification_user_id_list))
        notification_notice_at = datetime.now()
        NotificationService.save_notification(
            notification_type=NotificationType.SALESFORCE_CUSTOMER_IMPORT,
            user_id_list=bo_notification_user_id_list,
            message_param=payload,
            url="",
            noticed_at=notification_notice_at,
            create_id=batch_control_id,
            update_id=batch_control_id,
        )


def import_customer(
    event,
    operation_start_datetime: datetime,
    batch_control_id: str,
    registration_customer_list: list[PfCustomerInfo] = [],
):
    """PartnerPortal取込処理（取引先情報）

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
        registration_customer_list (list[PfCustomerInfo]): 登録対象の取引先情報リスト
    """
    ####################################
    # PartnerPortal取込処理（取引先情報）
    ####################################
    # 取引先情報取得（全件）
    customer_result_list: list[CustomerModel] = list(CustomerModel.scan())
    # DB取得結果から辞書を作成
    #  key: salesforce_customer_id, value: CustomerModel
    sf_customer_id_map: dict[str, CustomerModel] = {
        c.salesforce_customer_id: c for c in customer_result_list
    }

    # 取引先名を更新する取引先（案件、一般ユーザ更新用）
    #  key: id(取引先ID), value: name(更新後の取引先名)の辞書
    customer_name_update_map: dict[str, str] = {}

    # 新規登録の取引先名、お客様情報詳細URL(BO)のリスト
    #  key: "customer_name"(固定), value: 取引先名
    #   e.g. {
    #             "customer_name": "AAA株式会社",
    #             "customer_detail_url_bo": "https://bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/customer/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    #         }
    new_log_list: list[CustomerNewLog] = []

    # 更新分（取引先名、更新項目名、更新値）のリスト
    # e.g. [
    #         {
    #             "customer_name": "CCC株式会社",
    #             "update_column_info": [
    #                 {
    #                     "update_column": "最終更新日",
    #                     "update_value": "2021-01-01",
    #                 },
    #                 {
    #                     "update_column": "フラグ",
    #                     "update_value": "ON",
    #                 },
    #             ],
    #         },
    #      ]
    edit_log_list: list[CustomerEditLog] = []

    logger.info("取引先情報のPartnerPortal取込処理 開始")
    logger.info(
        f"取引先情報のPartnerPortal取込処理 処理対象件数: {len(registration_customer_list)}件"
    )

    update_datetime = datetime.now()
    try:
        with CustomerModel.batch_write() as customer_batch:
            with ThreadPoolExecutor(
                max_workers=ThreadPoolMaxWorkers.BATCH_AUTOMATIC_LINK
            ) as executor:
                features = []
                for count, registration_customer_info in enumerate(
                    registration_customer_list, 1
                ):
                    logger.info(
                        f"取引先情報のPartnerPortal取込処理 - 処理開始 ({count}/{len(registration_customer_list)})"
                    )
                    features.append(
                        executor.submit(
                            import_customer_execute_parallel,
                            registration_customer_info=registration_customer_info,
                            sf_customer_id_map=sf_customer_id_map,
                            customer_batch=customer_batch,
                            batch_control_id=batch_control_id,
                            update_datetime=update_datetime,
                            customer_name_update_map=customer_name_update_map,
                            edit_log_list=edit_log_list,
                            new_log_list=new_log_list,
                        )
                    )

                # 結果（Exception）の取得
                for feature in features:
                    feature.result()

        logger.info("取引先情報のPartnerPortal取込処理 完了")

        logger.info("一般ユーザテーブル及び案件情報テーブル更新処理 開始")
        # 一般ユーザテーブル、及び案件情報テーブル更新
        update_user_and_project_info(
            batch_control_id=batch_control_id,
            update_datetime=update_datetime,
            customer_name_update_map=customer_name_update_map,
        )
        logger.info("一般ユーザテーブル及び案件情報テーブル更新処理 完了")

        logger.info("メール・お知らせ通知処理（取引先情報のPartnerPortal取込） 開始")
        # 案件情報の取込処理における各種メール送信、及び通知情報の登録
        send_mail_and_notification_for_import_customer(
            batch_control_id=batch_control_id,
            new_log_list=new_log_list,
            edit_log_list=edit_log_list,
        )
        logger.info("メール・お知らせ通知処理（取引先情報のPartnerPortal取込） 完了")

    except Exception as e:
        logger.exception(Message.ImportExecuteError.EXECUTE_ERROR)
        raise e


def divide_user_id(before_list: list[str], target_list: list[str]) -> dict:
    """UPDATE時に更新前のユーザIDリストと更新するユーザIDリストを基に、
        案件情報から除外されたユーザIDと新規に追加されたユーザIDに振り分ける。

    Args:
        before_list (list[str]): 更新前のユーザIDリスト
        target_list (list[str]): 更新するユーザIDリスト

    Returns:
        dict: 除外リストと追加リストを含む辞書
              exclude: 除外リスト（list）
              add: 追加リスト（list）
    """
    exclude_list = []
    add_list = []
    for before_user_id in before_list:
        if before_user_id not in target_list:
            exclude_list.append(before_user_id)
    for target_user_id in target_list:
        if target_user_id not in before_list:
            add_list.append(target_user_id)
    return {"exclude": exclude_list, "add": add_list}


def get_user_info(user_id_list: list[str]) -> list[UserModel]:
    """指定されたユーザIDのユーザ情報を取得

    Args:
        user_id_list (list[str]): 取得するユーザIDのリスト

    Returns:
        list[UserModel]: 取得結果（ユーザ情報リスト）
    """
    user_model_list = []
    item_keys = [(id, DataType.USER) for id in user_id_list]
    for item in UserModel.batch_get(item_keys):
        user_model_list.append(item)
    return user_model_list


def edit_target_project(
    target_project: ProjectModel,
    customer_id: str,
    salesforce_customer_id: str,
    salesforce_update_at: datetime,
    name: str,
    customer_name: str,
    service_type: str,
    service_type_name: str,
    create_new: bool,
    main_sales_user_id: str,
    main_sales_user_name: str,
    phase: str,
    customer_success: str,
    support_date_from: str,
    support_date_to: str,
    profit: GrossProfitAttribute,
    gross: GrossProfitAttribute,
    total_contract_time: float,
    salesforce_main_customer: SalesforceMainCustomerAttribute,
    service_manager_name: str,
    supporter_organization_id: str,
    supporter_organization_name: str,
    salesforce_main_supporter_user_name: str,
    salesforce_supporter_user_names: set,
    this_month_type: str,
    no_send_reason: str,
    update_id: str,
    update_at: datetime,
) -> list[UpdateColumnInfo]:
    """案件情報更新時の編集

    Args:
        target_project (ProjectModel): ProjectModel
        customer_id (str): 取引先ID
        salesforce_customer_id (str): Salesforce取引先ID
        salesforce_update_at (datetime): Salesforce最終更新日時
        name (str): 商談名（案件名）
        customer_name (str): 取引先名
        service_type (str): サービス区分
        service_type_name (str): サービス区分名 ※メールの更新履歴編集用
        create_new (bool): 新規・更新
        main_sales_user_id (str): 営業担当者
        main_sales_user_name (str): 営業担当者名 ※メールの更新履歴編集用
        phase (str): フェーズ
        customer_success (str): カスタマーサクセス
        support_date_from (str): 支援開始日
        support_date_to (str): 支援終了日
        profit (GrossProfitAttribute): 粗利
        gross (GrossProfitAttribute): 売上
        total_contract_time (float): 延べ契約時間
        salesforce_main_customer (SalesforceMainCustomerAttribute): Salesforce取引先責任者
        service_manager_name (str): サービス責任者
        supporter_organization_id (str): 支援者組織ID
        supporter_organization_name (str): 支援者組織名 ※メールの更新履歴編集用
        salesforce_main_supporter_user_name (str): Salesforce主担当
        salesforce_supporter_user_names (set): Salesforce主担当
        this_month_type (str): （アンケート）今月の種類
        no_send_reason (str): （アンケート）送付しない理由
        update_id (str): 更新ユーザID
        update_at (datetime): 更新日時

    Returns:
        row_change_log (list[UpdateColumnInfo]): 更新情報リスト
    """
    row_change_log: list[UpdateColumnInfo] = []
    update_attributes: list[UpdateAttributesSubAttribute] = []

    if target_project.customer_id != customer_id:
        target_project.customer_id = customer_id
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.CUSTOMER_ID.tbl_column_name,
                value=customer_id,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.CUSTOMER_ID.log_column_name,
                update_value=conv_blank(customer_id),
            )
        )
    if target_project.salesforce_customer_id != salesforce_customer_id:
        target_project.salesforce_customer_id = salesforce_customer_id
        # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.SALESFORCE_CUSTOMER_ID.tbl_column_name,
                value=salesforce_customer_id,
            )
        )
    if target_project.salesforce_update_at != salesforce_update_at:
        target_project.salesforce_update_at = salesforce_update_at
        log_value = None
        if salesforce_update_at:
            log_value = salesforce_update_at.strftime("%Y/%m/%d %H:%M:%S")
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.SALESFORCE_UPDATE_AT.tbl_column_name,
                value=log_value,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.SALESFORCE_UPDATE_AT.log_column_name,
                update_value=conv_blank(log_value),
            )
        )
    if target_project.name != name:
        target_project.name = name
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.NAME.tbl_column_name,
                value=name,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.NAME.log_column_name,
                update_value=conv_blank(name),
            )
        )
    if target_project.customer_name != customer_name:
        target_project.customer_name = customer_name
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.CUSTOMER_NAME.tbl_column_name,
                value=customer_name,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.CUSTOMER_NAME.log_column_name,
                update_value=conv_blank(customer_name),
            )
        )
    if target_project.service_type != service_type:
        target_project.service_type = service_type
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.SERVICE_TYPE.tbl_column_name,
                value=service_type,
            )
        )
        log_value_service_type = service_type
        if service_type_name:
            log_value_service_type = service_type_name
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.SERVICE_TYPE.log_column_name,
                update_value=conv_blank(log_value_service_type),
            )
        )
    if target_project.create_new != create_new:
        target_project.create_new = create_new
        sub_attribute_create_new = None
        if target_project.create_new is not None:
            sub_attribute_create_new = str(target_project.create_new)
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.CREATE_NEW.tbl_column_name,
                value=sub_attribute_create_new,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.CREATE_NEW.log_column_name,
                update_value=conv_blank(sub_attribute_create_new),
            )
        )
    if target_project.main_sales_user_id != main_sales_user_id:
        target_project.main_sales_user_id = main_sales_user_id
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.MAIN_SALES_USER_ID.tbl_column_name,
                value=main_sales_user_id,
            )
        )
        log_value_main_sales_user_name = main_sales_user_id
        if main_sales_user_name:
            log_value_main_sales_user_name = main_sales_user_name
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.MAIN_SALES_USER_ID.log_column_name,
                update_value=conv_blank(log_value_main_sales_user_name),
            )
        )
    if target_project.phase != phase:
        target_project.phase = phase
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.PHASE.tbl_column_name,
                value=phase,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.PHASE.log_column_name,
                update_value=conv_blank(phase),
            )
        )
    if target_project.customer_success != customer_success:
        target_project.customer_success = customer_success
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.CUSTOMER_SUCCESS.tbl_column_name,
                value=customer_success,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.CUSTOMER_SUCCESS.log_column_name,
                update_value=conv_blank(customer_success),
            )
        )
    if target_project.support_date_from != support_date_from:
        target_project.support_date_from = support_date_from
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.SUPPORT_DATE_FROM.tbl_column_name,
                value=support_date_from,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.SUPPORT_DATE_FROM.log_column_name,
                update_value=conv_blank(support_date_from),
            )
        )
    if target_project.support_date_to != support_date_to:
        target_project.support_date_to = support_date_to
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.SUPPORT_DATE_TO.tbl_column_name,
                value=support_date_to,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.SUPPORT_DATE_TO.log_column_name,
                update_value=conv_blank(support_date_to),
            )
        )

    if profit is not None:
        if target_project.profit is None:
            target_project.profit = profit
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.PROFIT_YEAR.tbl_column_name,
                    value=str(profit.year),
                )
            )
            row_change_log.append(
                UpdateColumnInfo(
                    update_column=ProjectColumn.PROFIT_YEAR.log_column_name,
                    update_value=str(profit.year),
                )
            )
        else:
            if target_project.profit.year != profit.year:
                target_project.profit.year = profit.year
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.PROFIT_YEAR.tbl_column_name,
                        value=str(profit.year),
                    )
                )
                row_change_log.append(
                    UpdateColumnInfo(
                        update_column=ProjectColumn.PROFIT_YEAR.log_column_name,
                        update_value=str(profit.year),
                    )
                )
    else:
        # 事前チェックで必須項目のため、Noneは想定外
        raise Exception("Unexpected: Column 'profit' is None.")

    if gross is not None:
        if target_project.gross is None:
            target_project.gross = gross
            # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.GROSS_YEAR.tbl_column_name,
                    value=str(gross.year),
                )
            )
        if target_project.gross is not None:
            if target_project.gross.year != gross.year:
                target_project.gross.year = gross.year
                # 画面に存在しない項目のため案件テーブルの更新履歴のみ編集（メール通知の更新履歴なし）
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.GROSS_YEAR.tbl_column_name,
                        value=str(gross.year),
                    )
                )
    else:
        # 事前チェックで必須項目のため、Noneは想定外
        raise Exception("Unexpected: Column 'gross' is None.")

    if target_project.total_contract_time != total_contract_time:
        target_project.total_contract_time = total_contract_time
        log_value = None
        if total_contract_time is not None:
            log_value = str(total_contract_time)
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.TOTAL_CONTRACT_TIME.tbl_column_name,
                value=log_value,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.TOTAL_CONTRACT_TIME.log_column_name,
                update_value=conv_blank(log_value),
            )
        )
    if salesforce_main_customer is not None:
        if target_project.salesforce_main_customer is None:
            target_project.salesforce_main_customer = salesforce_main_customer
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.tbl_column_name,
                    value=salesforce_main_customer.name,
                )
            )
            row_change_log.append(
                UpdateColumnInfo(
                    update_column=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.log_column_name,
                    update_value=conv_blank(salesforce_main_customer.name),
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.tbl_column_name,
                    value=salesforce_main_customer.email,
                )
            )
            row_change_log.append(
                UpdateColumnInfo(
                    update_column=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.log_column_name,
                    update_value=conv_blank(salesforce_main_customer.email),
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.tbl_column_name,
                    value=salesforce_main_customer.organization_name,
                )
            )
            row_change_log.append(
                UpdateColumnInfo(
                    update_column=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.log_column_name,
                    update_value=conv_blank(salesforce_main_customer.organization_name),
                )
            )
            if target_project.is_survey_email_to_salesforce_main_customer:
                # 「取引先担当者をアンケート宛先に指定」がTrueの場合、匿名アンケート宛先に取引先担当者の情報をセット
                target_project.dedicated_survey_user_name = (
                    salesforce_main_customer.name
                )
                target_project.dedicated_survey_user_email = (
                    salesforce_main_customer.email
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.DEDICATED_SURVEY_USER_NAME.tbl_column_name,
                        value=target_project.dedicated_survey_user_name,
                    )
                )
                row_change_log.append(
                    UpdateColumnInfo(
                        update_column=ProjectColumn.DEDICATED_SURVEY_USER_NAME.log_column_name,
                        update_value=conv_blank(
                            target_project.dedicated_survey_user_name
                        ),
                    )
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.tbl_column_name,
                        value=target_project.dedicated_survey_user_email,
                    )
                )
                row_change_log.append(
                    UpdateColumnInfo(
                        update_column=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.log_column_name,
                        update_value=conv_blank(
                            target_project.dedicated_survey_user_email
                        ),
                    )
                )

        else:
            if (
                target_project.salesforce_main_customer.name
                != salesforce_main_customer.name
            ):
                target_project.salesforce_main_customer.name = (
                    salesforce_main_customer.name
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.tbl_column_name,
                        value=salesforce_main_customer.name,
                    )
                )
                row_change_log.append(
                    UpdateColumnInfo(
                        update_column=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.log_column_name,
                        update_value=conv_blank(salesforce_main_customer.name),
                    )
                )
                if target_project.is_survey_email_to_salesforce_main_customer:
                    # 「取引先担当者をアンケート宛先に指定」がTrueの場合、匿名アンケート宛先に取引先担当者の情報をセット
                    target_project.dedicated_survey_user_name = (
                        salesforce_main_customer.name
                    )
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.DEDICATED_SURVEY_USER_NAME.tbl_column_name,
                            value=target_project.dedicated_survey_user_name,
                        )
                    )
                    row_change_log.append(
                        UpdateColumnInfo(
                            update_column=ProjectColumn.DEDICATED_SURVEY_USER_NAME.log_column_name,
                            update_value=conv_blank(
                                target_project.dedicated_survey_user_name
                            ),
                        )
                    )
            if (
                target_project.salesforce_main_customer.email
                != salesforce_main_customer.email
            ):
                target_project.salesforce_main_customer.email = (
                    salesforce_main_customer.email
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.tbl_column_name,
                        value=salesforce_main_customer.email,
                    )
                )
                row_change_log.append(
                    UpdateColumnInfo(
                        update_column=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.log_column_name,
                        update_value=conv_blank(salesforce_main_customer.email),
                    )
                )
                if target_project.is_survey_email_to_salesforce_main_customer:
                    # 「取引先担当者をアンケート宛先に指定」がTrueの場合、匿名アンケート宛先に取引先担当者の情報をセット
                    target_project.dedicated_survey_user_email = (
                        salesforce_main_customer.email
                    )
                    update_attributes.append(
                        UpdateAttributesSubAttribute(
                            attribute=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.tbl_column_name,
                            value=target_project.dedicated_survey_user_email,
                        )
                    )
                    row_change_log.append(
                        UpdateColumnInfo(
                            update_column=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.log_column_name,
                            update_value=conv_blank(
                                target_project.dedicated_survey_user_email
                            ),
                        )
                    )
            if (
                target_project.salesforce_main_customer.organization_name
                != salesforce_main_customer.organization_name
            ):
                target_project.salesforce_main_customer.organization_name = (
                    salesforce_main_customer.organization_name
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.tbl_column_name,
                        value=salesforce_main_customer.organization_name,
                    )
                )
                row_change_log.append(
                    UpdateColumnInfo(
                        update_column=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.log_column_name,
                        update_value=conv_blank(
                            salesforce_main_customer.organization_name
                        ),
                    )
                )
    else:
        if target_project.salesforce_main_customer is not None:
            target_project.salesforce_main_customer = None
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.tbl_column_name,
                    value=None,
                )
            )
            row_change_log.append(
                UpdateColumnInfo(
                    update_column=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_NAME.log_column_name,
                    update_value="",
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.tbl_column_name,
                    value=None,
                )
            )
            row_change_log.append(
                UpdateColumnInfo(
                    update_column=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_EMAIL.log_column_name,
                    update_value="",
                )
            )
            update_attributes.append(
                UpdateAttributesSubAttribute(
                    attribute=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.tbl_column_name,
                    value=None,
                )
            )
            row_change_log.append(
                UpdateColumnInfo(
                    update_column=ProjectColumn.SALESFORCE_MAIN_CUSTOMER_ORGANIZATION_NAME.log_column_name,
                    update_value="",
                )
            )
            if target_project.is_survey_email_to_salesforce_main_customer:
                # 「取引先担当者をアンケート宛先に指定」がTrueの場合、匿名アンケート宛先に取引先担当者の情報をセット
                target_project.dedicated_survey_user_name = None
                target_project.dedicated_survey_user_email = None
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.DEDICATED_SURVEY_USER_NAME.tbl_column_name,
                        value=None,
                    )
                )
                row_change_log.append(
                    UpdateColumnInfo(
                        update_column=ProjectColumn.DEDICATED_SURVEY_USER_NAME.log_column_name,
                        update_value="",
                    )
                )
                update_attributes.append(
                    UpdateAttributesSubAttribute(
                        attribute=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.tbl_column_name,
                        value=None,
                    )
                )
                row_change_log.append(
                    UpdateColumnInfo(
                        update_column=ProjectColumn.DEDICATED_SURVEY_USER_EMAIL.log_column_name,
                        update_value="",
                    )
                )

    if target_project.service_manager_name != service_manager_name:
        target_project.service_manager_name = service_manager_name
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.SERVICE_MANAGER_NAME.tbl_column_name,
                value=service_manager_name,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.SERVICE_MANAGER_NAME.log_column_name,
                update_value=conv_blank(service_manager_name),
            )
        )

    if target_project.supporter_organization_id != supporter_organization_id:
        target_project.supporter_organization_id = supporter_organization_id
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.SUPPORTER_ORGANIZATION_ID.tbl_column_name,
                value=supporter_organization_id,
            )
        )
        log_value_supporter_organization_name = supporter_organization_id
        if supporter_organization_name:
            log_value_supporter_organization_name = supporter_organization_name
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.SUPPORTER_ORGANIZATION_ID.log_column_name,
                update_value=conv_blank(log_value_supporter_organization_name),
            )
        )
    if (
        target_project.salesforce_main_supporter_user_name
        != salesforce_main_supporter_user_name
    ):
        target_project.salesforce_main_supporter_user_name = (
            salesforce_main_supporter_user_name
        )
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.SALESFORCE_MAIN_SUPPORTER_USER_NAME.tbl_column_name,
                value=salesforce_main_supporter_user_name,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.SALESFORCE_MAIN_SUPPORTER_USER_NAME.log_column_name,
                update_value=conv_blank(salesforce_main_supporter_user_name),
            )
        )
    if (
        target_project.salesforce_supporter_user_names
        != salesforce_supporter_user_names
    ):
        target_project.salesforce_supporter_user_names = salesforce_supporter_user_names

        log_str_salesforce_supporter_user_names = ""
        if salesforce_supporter_user_names:
            log_str_salesforce_supporter_user_names = ",".join(
                salesforce_supporter_user_names
            )

        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.SALESFORCE_SUPPORTER_USER_NAMES.tbl_column_name,
                value=log_str_salesforce_supporter_user_names,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.SALESFORCE_SUPPORTER_USER_NAMES.log_column_name,
                update_value=conv_blank(log_str_salesforce_supporter_user_names),
            )
        )

    if target_project.this_month_type != this_month_type:
        target_project.this_month_type = this_month_type
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.THIS_MONTH_TYPE.tbl_column_name,
                value=this_month_type,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.THIS_MONTH_TYPE.log_column_name,
                update_value=conv_blank(this_month_type),
            )
        )
    if target_project.no_send_reason != no_send_reason:
        target_project.no_send_reason = no_send_reason
        update_attributes.append(
            UpdateAttributesSubAttribute(
                attribute=ProjectColumn.NO_SEND_REASON.tbl_column_name,
                value=no_send_reason,
            )
        )
        row_change_log.append(
            UpdateColumnInfo(
                update_column=ProjectColumn.NO_SEND_REASON.log_column_name,
                update_value=conv_blank(no_send_reason),
            )
        )

    target_project.update_history = UpdateHistoryAttribute(
        update_id=update_id,
        update_attributes=update_attributes,
    )
    target_project.update_id = update_id
    target_project.update_at = update_at
    target_project.version += 1

    return row_change_log


def import_project_execute_parallel(
    registration_project_info: PfProjectInfo,
    sf_opportunity_id_map: dict,
    customer_list: list[CustomerModel],
    master_service_list: list[MasterSupporterOrganizationModel],
    master_supporter_org_list: list[MasterSupporterOrganizationModel],
    user_list: list[UserModel],
    project_batch: BatchWrite[ProjectModel],
    batch_control_id: str,
    update_datetime: datetime,
    user_project_update_map: dict,
    before_user_project_map: dict,
    to_mail_user_id_list: list[str],
    to_assign_mail_user_id_list: list[str],
    edit_log_list: list[ProjectEditLog],
    new_log_list: list[ProjectNewLog],
) -> None:
    """案件情報の新規登録、更新の処理
    Args:
        registration_project_info (PfProjectInfo): 登録対象の案件情報リスト
        sf_opportunity_id_map (dict): 案件情報のDB取得結果から作成した辞書(key: salesforce_opportunity_id, value: CustomerModel)
        customer_list (list[CustomerModel]): 取引先情報
        master_service_list (list[MasterSupporterOrganizationModel]): 汎用マスタ(サービス種別)
        master_supporter_org_list (list[MasterSupporterOrganizationModel]): 汎用マスタ(支援者組織)
        user_list (list[UserModel]): 一般ユーザ情報
        project_batch (BatchWrite[ProjectModel]): BatchWriteオブジェクト
        batch_control_id (str): バッチ関数ID
        update_datetime (datetime): 更新日時
        user_project_update_map (dict): 一般ユーザ案件更新マップ(key:案件ID, value:ユーザIDリスト)
        before_user_project_map (dict): 更新前の一般ユーザ案件更新マップ
        to_mail_user_id_list (list[dict]): 「案件情報新規登録通知」メールの宛先TOに入れるユーザID
        to_assign_mail_user_id_list (list[dict]): 「案件アサイン通知」メールの宛先TOに入れるユーザID
        edit_log_list (list[ProjectEditLog]): 更新用ログのリスト
        new_log_list (list[ProjectNewLog]): 新規登録用ログのリスト
    """
    # グローバル変数
    # 並列処理時の排他制御（ロック）
    global lock

    jst = timezone(TimezoneType.ASIA_TOKYO)

    # 登録内容の編集
    salesforce_customer_id = registration_project_info.salesforce_customer_id
    salesforce_opportunity_id = registration_project_info.salesforce_opportunity_id

    # Salesforce取引先IDを基に取引先ID,取引先名を取得
    customer_id = None
    customer_name = None
    if salesforce_customer_id:
        for customer in customer_list:
            if customer.salesforce_customer_id == salesforce_customer_id:
                customer_id = customer.id
                customer_name = customer.name

    salesforce_update_at = (
        jst.localize(
            datetime.strptime(
                registration_project_info.partner_portal_last_update_date,
                "%Y/%m/%d %H:%M:%S",
            )
        )
        if registration_project_info.partner_portal_last_update_date
        else None
    )
    name = registration_project_info.partner_portal_project_name

    service_type = None
    service_type_name = None
    service_type_name_no_prefix = remove_prefix_num(
        registration_project_info.service_type
    )
    for master_service in master_service_list:
        if service_type_name_no_prefix == master_service.name:
            service_type = master_service.id
            service_type_name = master_service.name

    create_new = (
        convert_create_new_to_bool(registration_project_info.create_new)
        if registration_project_info.create_new
        else None
    )

    main_sales_user_id = None
    main_sales_user_name = None
    for user in user_list:
        if registration_project_info.project_owner == user.name and (
            user.role == UserRoleType.SALES.key
            or user.role == UserRoleType.SALES_MGR.key
        ):
            if not user.disabled:
                main_sales_user_id = user.id
                main_sales_user_name = user.name

    phase = registration_project_info.phase
    customer_success = registration_project_info.customer_success
    support_date_from = registration_project_info.support_date_from
    support_date_to = registration_project_info.support_date_to
    profit = (
        GrossProfitAttribute(year=registration_project_info.partner_portal_profit)
        if registration_project_info.partner_portal_profit is not None
        else None
    )
    gross = (
        GrossProfitAttribute(year=registration_project_info.partner_portal_gross)
        if registration_project_info.partner_portal_gross is not None
        else None
    )
    total_contract_time = (
        float(registration_project_info.direct_support_man_hour)
        if registration_project_info.direct_support_man_hour
        else 0.0
    )
    salesforce_main_customer = (
        SalesforceMainCustomerAttribute(
            name=registration_project_info.customer_manager,
            email=registration_project_info.customer_manager_mail_address,
            organization_name=registration_project_info.customer_manager_department,
        )
        if (
            registration_project_info.customer_manager
            or registration_project_info.customer_manager_mail_address
            or registration_project_info.customer_manager_department
        )
        else None
    )

    service_manager_name = registration_project_info.service_manager

    supporter_organization_id = None
    supporter_organization_name = None
    for master_supporter_org in master_supporter_org_list:
        if registration_project_info.profit == master_supporter_org.value:
            supporter_organization_id = master_supporter_org.id
            supporter_organization_name = master_supporter_org.value
    salesforce_main_supporter_user_name = (
        registration_project_info.producer
        if registration_project_info.producer
        else None
    )

    salesforce_supporter_user_names: set = None
    for temp_user_names in registration_project_info.get_accelerator_list():
        if salesforce_supporter_user_names is None:
            salesforce_supporter_user_names = set()
        salesforce_supporter_user_names.add(temp_user_names)

    this_month_type = registration_project_info.questionnaire_type
    no_send_reason = registration_project_info.unsent_questionnaire_reason

    contract_type = ContractType.FOR_VALUE

    fo_site_url = get_app_settings().fo_site_url
    bo_site_url = get_app_settings().bo_site_url
    is_new: bool = False
    is_update: bool = False
    row_change_log: list[UpdateColumnInfo] = []
    temp_user_project_update_map: dict = {}
    temp_before_user_project_map: dict = {}
    temp_to_mail_user_id_dict: dict = {}
    temp_to_assign_mail_user_id_dict: dict = {}

    if salesforce_opportunity_id not in sf_opportunity_id_map.keys():
        # 新規登録
        is_new = True
        # メイン支援者（主担当）は、新規登録の場合のみ登録
        # プロデューサー(主担当1名)が支援者、支援者責任者又は事業者責任者とヒットすれば、メイン支援者（主担当）にユーザIDをセット
        main_supporter_user_id = None

        if salesforce_main_supporter_user_name:
            non_prefix_user_name = None
            temp_split_value = salesforce_main_supporter_user_name.split("_")
            if len(temp_split_value) > 1:
                non_prefix_user_name = temp_split_value[1].strip()
            else:
                non_prefix_user_name = temp_split_value[0].strip()
            # 一般ユーザーテーブルの支援者、支援者責任者又は事業者責任者ユーザーの名前を参照しユーザーIDを取得
            for user in user_list:
                if user.role in [
                    UserRoleType.SUPPORTER.key,
                    UserRoleType.SUPPORTER_MGR.key,
                    UserRoleType.BUSINESS_MGR.key,
                ]:
                    if user.name == non_prefix_user_name:
                        if not user.disabled:
                            main_supporter_user_id = user.id

        # 支援者メンバー（副担当）は、新規登録の場合のみ登録
        # アクセラレーター(副担当複数名)が支援者、支援者責任者又は事業者責任者とヒットすれば、支援者メンバー（副担当）にユーザIDをセット
        supporter_user_ids: set = None
        supporter_user_name_list: list[str] = None
        if salesforce_supporter_user_names:
            for user_name in salesforce_supporter_user_names:
                non_prefix_user_name = None
                temp_split_value = user_name.split("_")
                if len(temp_split_value) > 1:
                    non_prefix_user_name = temp_split_value[1].strip()
                else:
                    non_prefix_user_name = temp_split_value[0].strip()
                # 一般ユーザーテーブルの支援者、支援者責任者又は事業者責任者ユーザーの名前を参照しユーザーIDを取得
                for user in user_list:
                    if user.role in [
                        UserRoleType.SUPPORTER.key,
                        UserRoleType.SUPPORTER_MGR.key,
                        UserRoleType.BUSINESS_MGR.key,
                    ]:
                        if user.name == non_prefix_user_name:
                            if not user.disabled:
                                if supporter_user_ids is None:
                                    supporter_user_ids = set()
                                if supporter_user_name_list is None:
                                    supporter_user_name_list = []
                                supporter_user_ids.add(user.id)
                                supporter_user_name_list.append(user.name)
        # 新規登録の場合、Salesforce取引先責任者の情報を匿名アンケート宛先にセット
        is_survey_email_to_salesforce_main_customer = True
        dedicated_survey_user_name = None
        dedicated_survey_user_email = None
        if salesforce_main_customer:
            dedicated_survey_user_name = salesforce_main_customer.name
            dedicated_survey_user_email = salesforce_main_customer.email
        # 新規登録の場合、匿名アンケートパスワードを生成してセット
        encrypted_survey_password = AesCipherUtils.encrypt(
            create_random_survey_password(SurveyPasswordSetting.PASSWORD_LENGTH)
        )

        new_project = ProjectModel(
            id=str(uuid.uuid4()),
            data_type=DataType.PROJECT,
            customer_id=customer_id,
            salesforce_customer_id=salesforce_customer_id,
            salesforce_opportunity_id=salesforce_opportunity_id,
            salesforce_update_at=salesforce_update_at,
            name=name,
            customer_name=customer_name,
            service_type=service_type,
            create_new=create_new,
            main_sales_user_id=main_sales_user_id,
            phase=phase,
            customer_success=customer_success,
            support_date_from=support_date_from,
            support_date_to=support_date_to,
            profit=profit,
            gross=gross,
            total_contract_time=total_contract_time,
            salesforce_main_customer=salesforce_main_customer,
            service_manager_name=service_manager_name,
            main_supporter_user_id=main_supporter_user_id,
            supporter_organization_id=supporter_organization_id,
            salesforce_main_supporter_user_name=salesforce_main_supporter_user_name,
            supporter_user_ids=supporter_user_ids,
            salesforce_supporter_user_names=salesforce_supporter_user_names,
            contract_type=contract_type,
            dedicated_survey_user_name=dedicated_survey_user_name,
            dedicated_survey_user_email=dedicated_survey_user_email,
            survey_password=encrypted_survey_password,
            is_survey_email_to_salesforce_main_customer=is_survey_email_to_salesforce_main_customer,
            this_month_type=this_month_type,
            no_send_reason=no_send_reason,
            create_id=batch_control_id,
            create_at=update_datetime,
            update_id=batch_control_id,
            update_at=update_datetime,
            version=1,
        )
        # 一般ユーザ更新用のデータ編集
        target_user_id_list = []
        if new_project.main_sales_user_id:
            target_user_id_list.append(new_project.main_sales_user_id)
        if new_project.main_customer_user_id:
            target_user_id_list.append(new_project.main_customer_user_id)
        if new_project.main_supporter_user_id:
            target_user_id_list.append(new_project.main_supporter_user_id)
        if new_project.customer_user_ids:
            for customer_user_id in new_project.customer_user_ids:
                target_user_id_list.append(customer_user_id)
        if new_project.supporter_user_ids:
            for supporter_user_id in new_project.supporter_user_ids:
                target_user_id_list.append(supporter_user_id)
        target_user_id_list = list(set(target_user_id_list))
        temp_user_project_update_map[new_project.id] = target_user_id_list

        # 新規登録メール通知対象(宛先To)の編集（案件にアサインされた支援者、事業者責任者、営業担当者）
        temp_to_mail_user_id_dict["project_id"] = new_project.id
        temp_to_mail_user_id_dict["project_name"] = new_project.name
        user_id_list = [
            new_project.main_sales_user_id,
            new_project.main_supporter_user_id,
        ]
        if new_project.supporter_user_ids:
            for supporter_user_id in new_project.supporter_user_ids:
                user_id_list.append(supporter_user_id)
        user_id_list = list(set(user_id_list))
        temp_to_mail_user_id_dict["user_id_list"] = user_id_list

        # アサインメール通知対象(宛先To)の編集
        temp_to_assign_mail_user_id_dict["project_id"] = new_project.id
        temp_to_assign_mail_user_id_dict["project_name"] = new_project.name
        user_id_list = [
            new_project.main_sales_user_id,
            new_project.main_supporter_user_id,
        ]
        if new_project.supporter_user_ids:
            for supporter_user_id in new_project.supporter_user_ids:
                user_id_list.append(supporter_user_id)
        user_id_list = list(set(user_id_list))
        temp_to_assign_mail_user_id_dict["user_id_list"] = user_id_list

    else:
        # 更新
        target_project: ProjectModel = copy.deepcopy(
            sf_opportunity_id_map[salesforce_opportunity_id]
        )
        # DBのSF最終更新日時 < PF取得データのSF最終更新日時の場合のみ、更新
        if target_project.salesforce_update_at < salesforce_update_at:
            is_update = True
            # 一般ユーザ更新用のデータ編集（更新前）
            before_user_id_list = []
            if target_project.main_sales_user_id:
                before_user_id_list.append(target_project.main_sales_user_id)
            if target_project.main_customer_user_id:
                before_user_id_list.append(target_project.main_customer_user_id)
            if target_project.main_supporter_user_id:
                before_user_id_list.append(target_project.main_supporter_user_id)
            if target_project.customer_user_ids:
                for customer_user_id in target_project.customer_user_ids:
                    before_user_id_list.append(customer_user_id)
            if target_project.supporter_user_ids:
                for supporter_user_id in target_project.supporter_user_ids:
                    before_user_id_list.append(supporter_user_id)
            before_user_id_list = list(set(before_user_id_list))
            temp_before_user_project_map[target_project.id] = before_user_id_list

            row_change_log = edit_target_project(
                target_project=target_project,
                customer_id=customer_id,
                salesforce_customer_id=salesforce_customer_id,
                salesforce_update_at=salesforce_update_at,
                name=name,
                customer_name=customer_name,
                service_type=service_type,
                service_type_name=service_type_name,
                create_new=create_new,
                main_sales_user_id=main_sales_user_id,
                main_sales_user_name=main_sales_user_name,
                phase=phase,
                customer_success=customer_success,
                support_date_from=support_date_from,
                support_date_to=support_date_to,
                profit=profit,
                gross=gross,
                total_contract_time=total_contract_time,
                salesforce_main_customer=salesforce_main_customer,
                service_manager_name=service_manager_name,
                supporter_organization_id=supporter_organization_id,
                supporter_organization_name=supporter_organization_name,
                salesforce_main_supporter_user_name=salesforce_main_supporter_user_name,
                salesforce_supporter_user_names=salesforce_supporter_user_names,
                this_month_type=this_month_type,
                no_send_reason=no_send_reason,
                update_id=batch_control_id,
                update_at=update_datetime,
            )

            # 一般ユーザ更新用のデータ編集（更新後）
            target_user_id_list = []
            if target_project.main_sales_user_id:
                target_user_id_list.append(target_project.main_sales_user_id)
            if target_project.main_customer_user_id:
                target_user_id_list.append(target_project.main_customer_user_id)
            if target_project.main_supporter_user_id:
                target_user_id_list.append(target_project.main_supporter_user_id)
            if target_project.customer_user_ids:
                for customer_user_id in target_project.customer_user_ids:
                    target_user_id_list.append(customer_user_id)
            if target_project.supporter_user_ids:
                for supporter_user_id in target_project.supporter_user_ids:
                    target_user_id_list.append(supporter_user_id)
            target_user_id_list = list(set(target_user_id_list))
            temp_user_project_update_map[target_project.id] = target_user_id_list

            # アサインメール通知対象(宛先To)の編集
            temp_to_assign_mail_user_id_dict["project_id"] = target_project.id
            temp_to_assign_mail_user_id_dict["project_name"] = target_project.name
            user_id_list = [
                target_project.main_sales_user_id,
                target_project.main_supporter_user_id,
            ]
            if target_project.supporter_user_ids:
                for supporter_user_id in target_project.supporter_user_ids:
                    user_id_list.append(supporter_user_id)
            user_id_list = list(set(user_id_list))
            temp_to_assign_mail_user_id_dict["user_id_list"] = user_id_list

    # 並列処理の排他制御
    with lock:
        if is_new:
            project_batch.save(new_project)
            # メール/お知らせ通知用のログ編集（新規登録分）
            new_log_list.append(
                ProjectNewLog(
                    project_name=name,
                    fo_project_detail_url=fo_site_url
                    + BoAppUrl.PROJECT_DETAIL.format(projectId=new_project.id),
                    bo_project_detail_url=bo_site_url
                    + BoAppUrl.PROJECT_DETAIL.format(projectId=new_project.id),
                )
            )
        if is_update:
            project_batch.save(target_project)
            # メール/お知らせ通知用のログ編集（更新分）
            edit_log_list.append(
                ProjectEditLog(
                    project_name=name,
                    update_column_info=row_change_log,
                )
            )

        user_project_update_map.update(temp_user_project_update_map)
        before_user_project_map.update(temp_before_user_project_map)
        to_mail_user_id_list.append(temp_to_mail_user_id_dict)
        to_assign_mail_user_id_list.append(temp_to_assign_mail_user_id_dict)


def divide_and_update_user_info(
    batch_control_id: str,
    user_project_update_map: dict[str, list[str]],
    before_user_project_map: dict[str, list[str]],
    to_mail_user_id_list: list[dict],
    to_assign_mail_user_id_list: list[dict],
    project_divided_user_id_map: dict[str, dict[str, list[str]]],
    to_email_list: list[dict],
    fo_notification_user_id_list: list[dict],
    to_assign_email_list: list[dict],
    fo_assign_notification_user_id_list: list[dict],
) -> None:
    """ユーザ情報の参加案件の追加/削除等の振り分け、および一般ユーザテーブルの更新（各種通知メールの宛先編集も含む）

    Args:
        batch_control_id (str): バッチ関数ID
        user_project_update_map (dict[str, list[str]]): 一般ユーザの参加案件IDを更新するユーザの辞書
            ※key: project_id(案件ID), value: user_id(ユーザID)リストの辞書
        before_user_project_map (dict[str, list[str]]): 更新前の一般ユーザの参加案件IDの辞書
            ※key: project_id(案件ID), value: user_id(ユーザID)リストの辞書
        to_mail_user_id_list (list[dict]): 「案件情報新規登録通知」メールの宛先TOに入れるユーザID
            （案件にアサインされた支援者、事業者責任者、営業担当者）
            ※dict(key: project_id, project_name, user_id_list)のリスト
        to_assign_mail_user_id_list (list[dict]): 「案件アサイン通知」メールの宛先TOに入れるユーザID
            （支援者or支援者責任者or事業者責任者or営業担当者orお客様メンバー）
            ※dict(key: project_id, project_name, user_id_list)のリスト
        project_divided_user_id_map (dict[str, dict[str, list[str]]]): 【当該処理内で更新】案件（参加案件ID）毎のユーザの除外リストと追加リストを含む辞書
            ・key: 案件ID
            ・value: ユーザID (dict)
              ・exclude: 除外リスト（List）
              ・add: 追加リスト（List）
        to_email_list (list[dict]): 【当該処理内で更新】「案件情報新規登録通知」メールの宛先TOに入れるメールアドレス
            ※dict(key: project_id, project_name, email_list)のリスト
        fo_notification_user_id_list (list[dict]): 【当該処理内で更新】お知らせ（案件情報新規登録通知）を通知するユーザID
            ※dict(key: project_id, project_name, user_id_list)のリスト
        to_assign_email_list (list[dict]): 【当該処理内で更新】「案件アサイン通知」メールの宛先TOに入れるメールアドレス
            ※dict(key: project_id, project_name, email_list)のリスト
        fo_assign_notification_user_id_list (list[dict]): 【当該処理内で更新】お知らせ（アサイン）を通知するユーザID
            ※dict(key: project_id, project_name, user_id_list)のリスト
    """
    ############################################
    # ユーザ情報の参加案件の追加/削除等の振り分け
    ############################################
    # ユーザIDが指定された項目から更新前後で除外/追加されたユーザIDのリストを取得
    # 案件（参加案件ID）毎のユーザの除外リストと追加リストの辞書を生成
    for project_id, target_user_id_list in user_project_update_map.items():
        divided_user_id_dict = divide_user_id(
            before_user_project_map.get(project_id, []), target_user_id_list
        )
        project_divided_user_id_map[project_id] = divided_user_id_dict

    # ユーザ更新用のdictionaryを作成
    # 作成元:
    #   key: 案件ID
    #   value: ユーザID (dict)
    #      exclude: 除外リスト（List）
    #      add: 追加リスト（List）
    # 作成対象:
    #   key: ユーザID
    #   value: 案件ID (dict)
    #      exclude: 除外リスト（List）
    #      add: 追加リスト（List）
    user_divided_project_id_map: dict[str, dict[str, list[str]]] = {}
    for (
        project_id,
        divided_user_id_dict,
    ) in project_divided_user_id_map.items():
        for add_user_id in divided_user_id_dict["add"]:
            divided_project_dict = user_divided_project_id_map.get(add_user_id)
            add_project_list = []
            if divided_project_dict is None:
                user_divided_project_id_map[add_user_id] = {}
            else:
                add_project_list = divided_project_dict.get("add")
            add_project_list.append(project_id)
            user_divided_project_id_map[add_user_id]["add"] = add_project_list
        for exclude_user_id in divided_user_id_dict["exclude"]:
            divided_project_dict = user_divided_project_id_map.get(exclude_user_id)
            exclude_project_list = []
            if divided_project_dict is None:
                user_divided_project_id_map[exclude_user_id] = {}
            else:
                exclude_project_list = divided_project_dict.get("exclude")
            exclude_project_list.append(project_id)
            user_divided_project_id_map[exclude_user_id]["exclude"] = (
                exclude_project_list
            )

    # 一般ユーザ情報を取得
    user_table_info = get_user_info(user_divided_project_id_map.keys())

    ####################################
    # メール/お知らせ対象のユーザ情報取得
    ####################################
    for (
        project_id,
        divided_user_id_dict,
    ) in project_divided_user_id_map.items():
        target_user_info: dict = {}
        for to_mail_user_info in to_mail_user_id_list:
            if to_mail_user_info:
                if to_mail_user_info["project_id"] == project_id:
                    target_user_info = to_mail_user_info
        target_assign_user_info: dict = {}
        for to_assign_mail_user_info in to_assign_mail_user_id_list:
            if to_assign_mail_user_info:
                if to_assign_mail_user_info["project_id"] == project_id:
                    target_assign_user_info = to_assign_mail_user_info

        temp_email = []
        temp_user_id = []
        temp_assign_email = []
        temp_assign_user_id = []

        for user in user_table_info:
            if user.id in divided_user_id_dict["add"]:
                # 新規/更新で新たにアサインされたユーザのみ対象
                if target_user_info:
                    if user.id in target_user_info["user_id_list"]:
                        temp_email.append(user.email)
                        temp_user_id.append(user.id)
                if target_assign_user_info:
                    if user.id in target_assign_user_info["user_id_list"]:
                        temp_assign_email.append(user.email)
                        temp_assign_user_id.append(user.id)

        # 新規登録通知
        if temp_email:
            temp_to_email_dict = {
                "project_id": target_user_info["project_id"],
                "project_name": target_user_info["project_name"],
                "email_list": temp_email,
            }
            to_email_list.append(temp_to_email_dict)
        if temp_user_id:
            temp_fo_user_id_dict = {
                "project_id": target_user_info["project_id"],
                "project_name": target_user_info["project_name"],
                "user_id_list": temp_user_id,
            }
            fo_notification_user_id_list.append(temp_fo_user_id_dict)
        # アサイン通知
        if temp_assign_email:
            temp_to_assign_email_dict = {
                "project_id": target_assign_user_info["project_id"],
                "project_name": target_assign_user_info["project_name"],
                "email_list": temp_assign_email,
            }
            to_assign_email_list.append(temp_to_assign_email_dict)
        if temp_assign_user_id:
            temp_fo_assign_user_id_dict = {
                "project_id": target_assign_user_info["project_id"],
                "project_name": target_assign_user_info["project_name"],
                "user_id_list": temp_assign_user_id,
            }
            fo_assign_notification_user_id_list.append(temp_fo_assign_user_id_dict)

    #################
    # 一般ユーザ更新
    #################
    update_datetime = datetime.now()
    with UserModel.batch_write() as user_batch:
        for user in user_table_info:
            divided_project_id_dict = user_divided_project_id_map.get(user.id)
            if divided_project_id_dict:
                add_project_id_list = divided_project_id_dict.get("add")
                exclude_project_id_list = divided_project_id_dict.get("exclude")

                if add_project_id_list:
                    # 参加案件を追加
                    if user.project_ids is not None:
                        user.project_ids.update(add_project_id_list)
                    else:
                        user.project_ids = set(add_project_id_list)
                if exclude_project_id_list:
                    # 参加案件を削除
                    if user.project_ids is not None:
                        for project_id in exclude_project_id_list:
                            user.project_ids.discard(project_id)
                user.update_id = batch_control_id
                user.update_at = update_datetime
                user.version += 1
                user_batch.save(user)


def send_mail_and_notification_for_import_project(
    batch_control_id: str,
    new_log_list: list[ProjectNewLog],
    edit_log_list: list[ProjectEditLog],
    to_email_list: list[dict],
    to_assign_email_list: list[dict],
    fo_notification_user_id_list: list[dict],
    fo_assign_notification_user_id_list: list[dict],
) -> None:
    """案件情報の取込処理における各種メール送信及び通知情報の登録

    Args:
        batch_control_id (str): バッチ関数ID
        new_log_list (list[ProjectNewLog]): _description_
        edit_log_list (list[ProjectEditLog]): _description_
        to_email_list (list[dict]): 「案件情報新規登録通知」メールの宛先TOに入れるメールアドレス ※dict(key: project_id, project_name, email_list)のリスト
        to_assign_email_list (list[dict]): 「案件アサイン通知」メールの宛先TOに入れるメールアドレス ※dict(key: project_id, project_name, email_list)のリスト
        fo_notification_user_id_list (list[dict]): お知らせ（案件情報新規登録通知）を通知するユーザID ※dict(key: project_id, project_name, user_id_list)のリスト
        fo_assign_notification_user_id_list (list[dict]): お知らせ（アサイン）を通知するユーザID ※dict(key: project_id, project_name, user_id_list)のリスト
    """
    # 「Salesforce案件情報インポート通知」メールの宛先TOに入れるメールアドレス
    to_import_email_list = []
    # お知らせ（Salesforce案件情報インポート通知）を通知するユーザID
    bo_import_notification_user_id_list = []
    # 「案件情報新規登録通知」メールの宛先CCに入れるメールアドレス
    cc_email = []
    # お知らせ（案件情報新規登録通知）を通知するユーザID
    bo_notification_user_id = []

    fo_site_url = get_app_settings().fo_site_url
    bo_site_url = get_app_settings().bo_site_url

    #######################
    # メール・お知らせ通知
    #######################
    if new_log_list or edit_log_list:
        new_log_dict_list = [asdict(x) for x in new_log_list]
        edit_log_dict_list = [asdict(x) for x in edit_log_list]
        # SQS send_message_batchのEntries
        send_message_entries: list[dict] = []
        # 新規登録または更新がある場合のみ通知
        payload = {
            "add_info_list": new_log_dict_list,
            "update_info_list": edit_log_dict_list,
        }

        # システム管理者、工数調査事務局、アンケート事務局の情報
        admin_info_for_mail: list[dict] = []
        admin_filter_condition = AdminModel.disabled == False  # NOQA
        for admin in AdminModel.scan(filter_condition=admin_filter_condition):
            if (
                UserRoleType.SYSTEM_ADMIN.key in admin.roles
                or UserRoleType.MAN_HOUR_OPS.key in admin.roles
                or UserRoleType.SURVEY_OPS.key in admin.roles
            ):
                temp_info: dict = {}
                temp_info["id"] = admin.id
                temp_info["email"] = admin.email
                admin_info_for_mail.append(temp_info)

        ############################################
        # 「Salesforce案件情報インポート通知」メール
        ############################################
        # 通知メールのTOに設定する管理ユーザ
        for admin in admin_info_for_mail:
            to_import_email_list.append(admin["email"])
            bo_import_notification_user_id_list.append(admin["id"])
        to_import_email_list = list(set(to_import_email_list))
        message_body = {
            "template": MailType.PROJECT_IMPORT_COMPLETED,
            "to": to_import_email_list,
            "cc": [],
            "payload": payload,
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

        ###################################
        # 「案件情報新規登録通知」メール
        ###################################
        # 通知メールのCCに設定する管理ユーザ
        for admin in admin_info_for_mail:
            cc_email.append(admin["email"])
            bo_notification_user_id.append(admin["id"])
        cc_email = list(set(cc_email))
        for email_info in to_email_list:
            fo_project_detail_url = fo_site_url + FoAppUrl.PROJECT_DETAIL.format(
                projectId=email_info["project_id"]
            )
            bo_project_detail_url = bo_site_url + BoAppUrl.PROJECT_DETAIL.format(
                projectId=email_info["project_id"]
            )
            to_addr_list = list(set(email_info["email_list"]))
            message_body = {
                "template": MailType.PROJECT_REGISTRATION_COMPLETED,
                "to": to_addr_list,
                "cc": cc_email,
                "payload": {
                    "project_name": email_info["project_name"],
                    "fo_project_detail_url": fo_project_detail_url,
                    "bo_project_detail_url": bo_project_detail_url,
                },
            }
            sqs_message_body = json.dumps(message_body)
            send_message_entries.append(
                {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
            )

        ################################
        # 「案件アサイン通知」メール
        ################################
        for assign_email_info in to_assign_email_list:
            fo_karte_list_url = fo_site_url + FoAppUrl.KARTE_LIST.format(
                projectId=assign_email_info["project_id"]
            )
            to_addr_list = list(set(assign_email_info["email_list"]))
            message_body = {
                "template": MailType.PROJECT_ASSIGN,
                "to": to_addr_list,
                "cc": [],
                "payload": {
                    "project_name": assign_email_info["project_name"],
                    "mail_address_to": " ".join(to_addr_list),
                    "fo_karte_list_url": fo_karte_list_url,
                },
            }
            sqs_message_body = json.dumps(message_body)
            send_message_entries.append(
                {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
            )

        ##############
        # メール送信
        ##############
        send_mail_batch(entries=send_message_entries)

        ##############
        # お知らせ
        ##############
        notification_notice_at = datetime.now()
        with NotificationModel.batch_write() as notification_batch:
            #############################################
            # お知らせ「Salesforce案件情報インポート通知」
            #############################################
            # BO
            bo_import_notification_user_id_list = list(
                set(bo_import_notification_user_id_list)
            )
            summary, message = NotificationService.edit_message(
                notification_type=NotificationType.SALESFORCE_PROJECT_IMPORT,
                message_param=payload,
            )
            for user_id in bo_import_notification_user_id_list:
                notification = NotificationModel(
                    id=str(uuid.uuid4()),
                    user_id=user_id,
                    summary=summary,
                    url="",
                    message=message,
                    confirmed=False,
                    noticed_at=notification_notice_at,
                    ttl=NotificationService.get_unix_time_after_90_days(
                        notification_notice_at
                    ),
                    create_id=batch_control_id,
                    create_at=notification_notice_at,
                    update_id=batch_control_id,
                    update_at=notification_notice_at,
                )
                notification_batch.save(notification)

            ##################################
            # お知らせ「案件情報新規登録通知」
            ##################################
            for fo_notification_info in fo_notification_user_id_list:
                fo_project_detail_url = fo_site_url + FoAppUrl.PROJECT_DETAIL.format(
                    projectId=fo_notification_info["project_id"]
                )
                bo_project_detail_url = bo_site_url + BoAppUrl.PROJECT_DETAIL.format(
                    projectId=fo_notification_info["project_id"]
                )
                project_name = fo_notification_info["project_name"]
                user_id_list = list(set(fo_notification_info["user_id_list"]))
                summary, message = NotificationService.edit_message(
                    notification_type=NotificationType.PROJECT_REGISTRATION,
                    message_param={"project_name": project_name},
                )
                for user_id in user_id_list:
                    notification = NotificationModel(
                        id=str(uuid.uuid4()),
                        user_id=user_id,
                        summary=summary,
                        url=fo_project_detail_url,
                        message=message,
                        confirmed=False,
                        noticed_at=notification_notice_at,
                        ttl=NotificationService.get_unix_time_after_90_days(
                            notification_notice_at
                        ),
                        create_id=batch_control_id,
                        create_at=notification_notice_at,
                        update_id=batch_control_id,
                        update_at=notification_notice_at,
                    )
                    notification_batch.save(notification)
                # BO
                user_id_list = list(set(bo_notification_user_id))
                summary, message = NotificationService.edit_message(
                    notification_type=NotificationType.PROJECT_REGISTRATION,
                    message_param={"project_name": project_name},
                )
                for user_id in user_id_list:
                    notification = NotificationModel(
                        id=str(uuid.uuid4()),
                        user_id=user_id,
                        summary=summary,
                        url=bo_project_detail_url,
                        message=message,
                        confirmed=False,
                        noticed_at=notification_notice_at,
                        ttl=NotificationService.get_unix_time_after_90_days(
                            notification_notice_at
                        ),
                        create_id=batch_control_id,
                        create_at=notification_notice_at,
                        update_id=batch_control_id,
                        update_at=notification_notice_at,
                    )
                    notification_batch.save(notification)

            ##################################
            # お知らせ「案件アサイン通知」
            ##################################
            # FO
            for fo_assign_notification_info in fo_assign_notification_user_id_list:
                fo_karte_list_url = fo_site_url + FoAppUrl.KARTE_LIST.format(
                    projectId=fo_assign_notification_info["project_id"]
                )
                user_id_list = list(set(fo_assign_notification_info["user_id_list"]))
                summary, message = NotificationService.edit_message(
                    notification_type=NotificationType.PROJECT_ASSIGN,
                    message_param={
                        "project_name": fo_assign_notification_info["project_name"]
                    },
                )
                for user_id in user_id_list:
                    notification = NotificationModel(
                        id=str(uuid.uuid4()),
                        user_id=user_id,
                        summary=summary,
                        url=fo_karte_list_url,
                        message=message,
                        confirmed=False,
                        noticed_at=notification_notice_at,
                        ttl=NotificationService.get_unix_time_after_90_days(
                            notification_notice_at
                        ),
                        create_id=batch_control_id,
                        create_at=notification_notice_at,
                        update_id=batch_control_id,
                        update_at=notification_notice_at,
                    )
                    notification_batch.save(notification)


def import_project(
    event,
    operation_start_datetime: datetime,
    batch_control_id: str,
    registration_project_list: list[PfProjectInfo] = [],
):
    """PartnerPortal取込処理（案件情報）

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
        registration_project_list (list[PfProjectInfo]): 登録対象の案件情報リスト
    """
    # グローバル変数
    global master_supporter_organization_list, master_service_type_list, user_list

    # 取引先情報取得（全件）
    customer_list: list[CustomerModel] = list(CustomerModel.scan())
    # 案件情報取得（全件）
    project_result_list: list[ProjectModel] = list(ProjectModel.scan())
    # DB取得結果から辞書を作成
    #  key: salesforce_opportunity_id, value: ProjectModel
    sf_opportunity_id_map: dict[str, ProjectModel] = {
        p.salesforce_opportunity_id: p for p in project_result_list
    }

    # 一般ユーザの参加案件IDを更新するユーザの辞書
    #  key: project_id(案件ID), value: user_id(ユーザID)リストの辞書
    user_project_update_map: dict[str, list[str]] = {}
    # 更新前の一般ユーザの参加案件IDの辞書
    #  key: project_id(案件ID), value: user_id(ユーザID)リストの辞書
    before_user_project_map: dict[str, list[str]] = {}

    # 案件（参加案件ID）毎のユーザの除外リストと追加リストを含む辞書
    # key: 案件ID
    # value: ユーザID (dict)
    #    exclude: 除外リスト（List）
    #    add: 追加リスト（List）
    project_divided_user_id_map: dict[str, dict[str, list[str]]] = {}

    # 「案件情報新規登録通知」メールの宛先TOに入れるユーザID
    # （案件にアサインされた支援者、事業者責任者、営業担当者）
    # dict(key: project_id, project_name, user_id_list)のリスト
    to_mail_user_id_list: list[dict] = []
    # 「案件情報新規登録通知」メールの宛先TOに入れるメールアドレス
    # dict(key: project_id, project_name, email_list)のリスト
    to_email_list: list[dict] = []
    # お知らせ（案件情報新規登録通知）を通知するユーザID
    # dict(key: project_id, project_name, user_id_list)のリスト
    fo_notification_user_id_list: list[dict] = []

    # 「案件アサイン通知」メールの宛先TOに入れるユーザID
    # （支援者or支援者責任者or事業者責任者or営業担当者orお客様メンバー）
    # dict(key: project_id, project_name, user_id_list)のリスト
    to_assign_mail_user_id_list: list[dict] = []
    # 「案件アサイン通知」メールの宛先TOに入れるメールアドレス
    # dict(key: project_id, project_name, email_list)のリスト
    to_assign_email_list: list[dict] = []
    # お知らせ（アサイン）を通知するユーザID
    # dict(key: project_id, project_name, user_id_list)のリスト
    fo_assign_notification_user_id_list: list[dict] = []

    # 新規登録の案件名、案件情報詳細URL(FO)、案件情報詳細URL(BO)のリスト
    #   e.g. {
    #             "project_name": "AAA案件",
    #             "fo_project_detail_url": "https://dev.partner-portal.inhouse-sony-startup-acceleration-program.com/project/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    #             "bo_project_detail_url": "https://bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/project/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    #         }
    new_log_list: list[ProjectNewLog] = []

    # 更新分（案件名、更新項目名、更新値）のリスト
    # e.g. [
    #         {
    #             "project_name": "CCC案件",
    #             "update_column_info": [
    #                 {
    #                     "update_column": "最終更新日",
    #                     "update_value": "2021-01-01",
    #                 },
    #                 {
    #                     "update_column": "フラグ",
    #                     "update_value": "ON",
    #                 },
    #             ],
    #         },
    #      ]
    edit_log_list: list[ProjectEditLog] = []

    logger.info("案件情報のPartnerPortal取込処理 開始")
    logger.info(
        f"案件情報のPartnerPortal取込処理 処理対象件数: {len(registration_project_list)}件"
    )

    update_datetime = datetime.now()
    try:
        with ProjectModel.batch_write() as project_batch:
            with ThreadPoolExecutor(
                max_workers=ThreadPoolMaxWorkers.BATCH_AUTOMATIC_LINK
            ) as executor:
                features = []
                for count, registration_project_info in enumerate(
                    registration_project_list, 1
                ):
                    logger.info(
                        f"案件情報のPartnerPortal取込処理 - 処理開始 ({count}/{len(registration_project_list)})"
                    )
                    features.append(
                        executor.submit(
                            import_project_execute_parallel,
                            registration_project_info=registration_project_info,
                            sf_opportunity_id_map=sf_opportunity_id_map,
                            customer_list=customer_list,
                            master_service_list=master_service_type_list,
                            master_supporter_org_list=master_supporter_organization_list,
                            user_list=user_list,
                            project_batch=project_batch,
                            batch_control_id=batch_control_id,
                            update_datetime=update_datetime,
                            user_project_update_map=user_project_update_map,
                            before_user_project_map=before_user_project_map,
                            to_mail_user_id_list=to_mail_user_id_list,
                            to_assign_mail_user_id_list=to_assign_mail_user_id_list,
                            edit_log_list=edit_log_list,
                            new_log_list=new_log_list,
                        )
                    )

                # 結果（Exception）の取得
                for feature in features:
                    feature.result()

            # salesforce_opportunity_id一覧
            salesforce_opportunity_ids = [
                x.salesforce_opportunity_id for x in registration_project_list
            ]

            if salesforce_opportunity_ids:
                # pf案件公開情報の更新
                platform_api_operator = PlatformApiOperator(is_batch=True)

                put_project_publication_pf_project_params = {
                    "salesforceOpportunityIds": salesforce_opportunity_ids,
                    "importCsv": True,
                }

                (
                    status_code,
                    body,
                ) = platform_api_operator.put_project_publication(
                    params=put_project_publication_pf_project_params
                )

                logger.info(f"platform putProjectPublication statusCode: {status_code}")
                if status_code != 200:
                    logger.info(f"platform putProjectPublication response: {body}")
                    error_msg = "PFのAPI（案件の公開/非公開およびCSV取り込み状態変更）の呼び出しに失敗しました。"
                    logger.error(error_msg)
                    raise Exception(error_msg)

        logger.info("案件情報のPartnerPortal取込処理 完了")

        logger.info("一般ユーザテーブル更新処理 開始")
        # ユーザ情報の参加案件の追加/削除等の振り分け
        # および一般ユーザテーブルの更新（各種通知メールの宛先編集も含む）
        divide_and_update_user_info(
            batch_control_id=batch_control_id,
            user_project_update_map=user_project_update_map,
            before_user_project_map=before_user_project_map,
            to_mail_user_id_list=to_mail_user_id_list,
            to_assign_mail_user_id_list=to_assign_mail_user_id_list,
            project_divided_user_id_map=project_divided_user_id_map,
            to_email_list=to_email_list,
            fo_notification_user_id_list=fo_notification_user_id_list,
            to_assign_email_list=to_assign_email_list,
            fo_assign_notification_user_id_list=fo_assign_notification_user_id_list,
        )
        logger.info("一般ユーザテーブル更新処理 完了")

        logger.info("メール・お知らせ通知処理（案件情報のPartnerPortal取込） 開始")
        # 案件情報の取込処理における各種メール送信、及び通知情報の登録
        send_mail_and_notification_for_import_project(
            batch_control_id=batch_control_id,
            new_log_list=new_log_list,
            edit_log_list=edit_log_list,
            to_email_list=to_email_list,
            to_assign_email_list=to_assign_email_list,
            fo_notification_user_id_list=fo_notification_user_id_list,
            fo_assign_notification_user_id_list=fo_assign_notification_user_id_list,
        )
        logger.info("メール・お知らせ通知処理（案件情報のPartnerPortal取込） 完了")

    except Exception as e:
        logger.exception(Message.ImportExecuteError.EXECUTE_ERROR)
        raise e


def send_error_mail_of_data_link(
    event,
    operation_start_datetime: datetime,
    batch_control_id: str,
    validation_error_customer_list: list[PfCustomerInfo] = [],
    validation_error_project_list: list[PfProjectInfo] = [],
):
    """取引先・案件情報データ連携エラー通知処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
        validation_error_customer_list (list[PfCustomerInfo]): バリデーションエラーの取引先情報リスト
        validation_error_project_list (list[PfProjectInfo]): バリデーションエラーの案件情報リスト
    """
    global admin_list_enable_only

    logger.info("取引先・案件情報データ連携エラー通知処理 開始")
    ###########################################
    # 取引先・案件情報データ連携エラー通知処理
    ###########################################
    if validation_error_customer_list or validation_error_project_list:
        # エラー情報が存在する場合
        # dataclassを辞書リストに変換
        validation_error_customer_dict_list: list[dict] = [
            asdict(x) for x in validation_error_customer_list
        ]
        validation_error_project_dict_list: list[dict] = [
            asdict(x) for x in validation_error_project_list
        ]

        logger.info(
            f"取引先情報データ連携エラー件数: {len(validation_error_customer_dict_list)}件"
        )
        logger.info(
            f"案件情報データ連携エラー件数: {len(validation_error_project_dict_list)}件"
        )

        # 宛先の編集
        # TO: システム管理者（全員）、アンケート事務局（全員）
        to_email_list: list[str] = []
        for admin in admin_list_enable_only:
            if (
                UserRoleType.SYSTEM_ADMIN.key in admin.roles
                or UserRoleType.SURVEY_OPS.key in admin.roles
            ):
                to_email_list.append(admin.email)

        # 宛先の重複を削除
        to_email_list = list(set(to_email_list))

        # メール送信（取引先・案件情報データ連携エラー通知）
        send_mail(
            template=MailType.BATCH_DATA_LINKAGE_ERROR,
            to_addr_list=to_email_list,
            cc_addr_list=[],
            payload={
                "error_datetime": datetime.now().strftime("%Y/%m/%d %H:%M"),
                "customer_error_dict_list": validation_error_customer_dict_list,
                "project_error_dict_list": validation_error_project_dict_list,
            },
        )

    logger.info("取引先・案件情報データ連携エラー通知処理 完了")


def init_procedure(event, operation_start_datetime: datetime, batch_control_id: str):
    """初期処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """

    batch_init_common_procedure(
        event=event,
        operation_start_datetime=operation_start_datetime,
        batch_control_id=batch_control_id,
        batch_control_name=BatchFunctionName.AUTOMATIC_LINK_BATCH,
    )

    global \
        master_supporter_organization_list, \
        master_service_type_list, \
        user_list, \
        admin_list_enable_only
    # 汎用マスタ:支援者組織情報
    master_supporter_organization_list = list(
        MasterSupporterOrganizationModel.data_type_index.query(
            hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION
        )
    )
    # 汎用マスタ:サービス種別
    master_service_type_list = list(
        MasterSupporterOrganizationModel.data_type_index.query(
            hash_key=MasterDataType.MASTER_SERVICE_TYPE
        )
    )
    # 一般ユーザ情報
    user_list = list(UserModel.data_type_name_index.query(hash_key=DataType.USER))
    # 管理者ユーザ情報(有効なユーザ)
    admin_filter_condition = AdminModel.disabled == False  # NOQA
    admin_list_enable_only = list(
        AdminModel.scan(filter_condition=admin_filter_condition)
    )


def end_procedure(event, batch_status: str, batch_control_id: str):
    """終了処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    batch_end_common_procedure(
        event=event, batch_status=batch_status, batch_control_id=batch_control_id
    )


def main_procedure(event, operation_start_datetime: datetime, batch_control_id: str):
    """主処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """

    # PFから取得した処理対象の取引先情報リスト
    link_customer_list: list[PfCustomerInfo] = []
    # PFから取得した処理対象の案件情報リスト
    link_project_list: list[PfProjectInfo] = []
    # 登録対象の取引先情報リスト
    registration_customer_list: list[PfCustomerInfo] = []
    # バリデーションエラーの取引先情報リスト
    validation_error_customer_list: list[PfCustomerInfo] = []
    # 登録対象の案件情報リスト
    registration_project_list: list[PfProjectInfo] = []
    # バリデーションエラーの案件情報リスト
    validation_error_project_list: list[PfProjectInfo] = []

    global user_list

    ######################
    # PFデータ取得処理
    ######################
    logger.info("get_pf_data start.")
    get_pf_data(
        event=event,
        operation_start_datetime=operation_start_datetime,
        batch_control_id=batch_control_id,
        link_customer_list=link_customer_list,
        link_project_list=link_project_list,
    )
    logger.info("get_pf_data end.")

    ######################
    # 取引先情報
    ######################
    # 取引先情報のバリデーション
    logger.info("validate_customer start.")
    validate_customer(
        event=event,
        operation_start_datetime=operation_start_datetime,
        batch_control_id=batch_control_id,
        link_customer_list=link_customer_list,
        registration_customer_list=registration_customer_list,
        validation_error_customer_list=validation_error_customer_list,
    )
    logger.info("validate_customer end.")

    # PartnerPortal取込処理（取引先情報）
    logger.info("import_customer start.")
    import_customer(
        event=event,
        operation_start_datetime=operation_start_datetime,
        batch_control_id=batch_control_id,
        registration_customer_list=registration_customer_list,
    )
    logger.info("import_customer end.")

    logger.info("user_list reacquisition start.")
    # 取込処理で更新されるため、グローバル変数の一般ユーザ情報を再取得
    user_list = list(UserModel.data_type_name_index.query(hash_key=DataType.USER))
    logger.info("user_list reacquisition end.")

    ######################
    # 案件情報
    ######################
    # 案件情報のバリデーション
    # バリデーション内で取引先情報を参照しているため、取引先情報の取込後に処理する
    logger.info("validate_project start.")
    validate_project(
        event=event,
        operation_start_datetime=operation_start_datetime,
        batch_control_id=batch_control_id,
        link_project_list=link_project_list,
        registration_project_list=registration_project_list,
        validation_error_project_list=validation_error_project_list,
    )
    logger.info("validate_project end.")

    # PartnerPortal取込処理（案件情報）
    logger.info("import_project start.")
    import_project(
        event=event,
        operation_start_datetime=operation_start_datetime,
        batch_control_id=batch_control_id,
        registration_project_list=registration_project_list,
    )
    logger.info("import_project end.")

    logger.info("user_list reacquisition start.")
    # 取込処理で更新されるため、グローバル変数の一般ユーザ情報を再取得
    user_list = list(UserModel.data_type_name_index.query(hash_key=DataType.USER))
    logger.info("user_list reacquisition end.")

    ###########################################
    # 取引先・案件情報データ連携エラー通知処理
    ###########################################
    logger.info("send_error_mail_of_data_link start.")
    send_error_mail_of_data_link(
        event=event,
        operation_start_datetime=operation_start_datetime,
        batch_control_id=batch_control_id,
        validation_error_customer_list=validation_error_customer_list,
        validation_error_project_list=validation_error_project_list,
    )
    logger.info("send_error_mail_of_data_link end.")


def handler(event, context):
    """バッチ処理 : BO取引先・案件情報自動連携処理

    Args:
        event:
        context:

    Returns:
        str: 実行結果("Normal end.","Skipped processing.","Error end.")
    """
    logger.debug(event)
    logger.debug(context)

    start_time = time.time()
    # バッチ関数名
    batch_control_id = BatchFunctionId.AUTOMATIC_LINK_BATCH.format(
        landscape=event["stageParams"]["stage"]
    )
    try:
        # 起動日時
        operation_start_datetime: datetime = get_operation_datetime()
        logger.info(f"operation_start_datetime: {operation_start_datetime}")

        # 初期処理
        init_procedure(event, operation_start_datetime, batch_control_id)

        # 主処理
        main_procedure(event, operation_start_datetime, batch_control_id)

        # 終了処理
        end_procedure(event, BatchStatus.EXECUTED, batch_control_id)

        process_time = round(time.time() - start_time, 4)
        logger.info("process_time: {0}[sec]".format(str(process_time)))
        logger.info("Process normal end.")
        return "Normal end."

    except ExitHandler:
        # 処理の途中で処理終了する場合
        process_time = round(time.time() - start_time, 4)
        logger.info("process_time: {0}[sec]".format(str(process_time)))
        logger.info("Skipped Processing.")
        return "Skipped processing."
    except Exception:
        error_mail_to = os.getenv("ERROR_MAIL_TO")
        if error_mail_to:
            # 環境変数ERROR_MAIL_TOに宛先が設定されている場合、エラーメール送信
            send_mail(
                template=MailType.BATCH_ERROR_MAIL,
                to_addr_list=[error_mail_to],
                cc_addr_list=[],
                payload={
                    "error_datetime": datetime.now().strftime("%Y/%m/%d %H:%M"),
                    "error_function": BatchFunctionName.AUTOMATIC_LINK_BATCH,
                    "error_message": traceback.format_exc(),
                },
            )
        end_procedure(event, BatchStatus.ERROR, batch_control_id)
        process_time = round(time.time() - start_time, 4)
        logger.info("process_time: {0}[sec]".format(str(process_time)))
        logger.exception("Error end.")
        return "Error end."


if __name__ == "__main__":
    param = {"stageParams": {"stage": sys.argv[1]}}
    handler(param, {})
