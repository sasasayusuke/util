import json
import os
import sys
import time
import traceback
import uuid
from datetime import date, datetime, timedelta
from logging import getLogger
from typing import List

from dateutil.relativedelta import relativedelta
from pydantic import BaseModel
from pynamodb.expressions.condition import size
from pytz import timezone

from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.man_hour import ManHourSupporterModel
from app.models.master import (
    BatchControlAttribute,
    MasterBatchControlModel,
    MasterServiceManagerModel,
    MasterSupporterOrganizationModel,
)
from app.models.notification import NotificationModel
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.project_survey import (
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.models.survey_master import SurveyMasterModel
from app.models.user import UserModel
from app.resources.const import SurveyQuestionsSummaryType, SurveyTypeForGetSurveys
from app.service.master_karten_service import MasterKartenService
from app.service.notification_service import NotificationService
from app.service.schedule_service import SchedulesService
from app.utils.aws.sqs import SqsHelper
from app.utils.platform import PlatformApiOperator
from functions.batch_common import (
    create_jwt,
    create_jwt_survey_payload,
    decrypt,
    get_day_of_week,
)
from functions.batch_const import (
    BatchFunctionId,
    BatchFunctionName,
    BatchInputParameterMode,
    BatchSettingConst,
    BatchStatus,
    DataType,
    FoAppUrl,
    JwtSettingInfo,
    MailType,
    MasterDataType,
    NotificationType,
    SurveyType,
    SurveyTypeName,
    UserRoleType,
)
from functions.batch_exceptions import ExitHandler

try:
    import unzip_requirements
except ImportError:
    pass

# ログ出力設定をロードする
get_app_settings().configure_logging()
logger = getLogger(__name__)


class MasterKarteProject(BaseModel):
    npf_project_id: str = None
    name: str = None
    main_sales_user_id: str = None
    main_supporter_user_id: str = None
    supporter_user_ids: List[str] = None


def send_mail(
    template, to_addr_list: List[str], cc_addr_list: List[str], payload: dict
):
    """メール送信(SqsHelper.send_message_to_queue()を使用)"""
    queue_name = get_app_settings().sqs_email_queue_name
    message_body = {
        "template": template,
        "to": to_addr_list,
        "cc": cc_addr_list,
        "payload": payload,
    }
    sqs_message_body = json.dumps(message_body)
    SqsHelper().send_message_to_queue(
        queue_name=queue_name, message_body=sqs_message_body
    )


def send_mail_batch(entries: List[dict]):
    """メール一括送信(SqsHelper.send_message_batch_to_queue()を使用)"""
    queue_name = get_app_settings().sqs_email_queue_name
    SqsHelper().send_message_batch_to_queue(queue_name=queue_name, entries=entries)


def get_operation_datetime() -> datetime:
    """起動日時の取得

    テストを効率よく行えるようLambda環境変数(OPERATION_DATETIME)から起動日時を取得する。
    ・「yyyy/mm/dd hh:mm」形式の値が入っている場合 : 左記を起動日時として利用
    ・上記以外の場合 : 現在日時

    Returns:
        datetime: 起動日時(JST)
    """
    try:
        env_datetime = os.getenv("OPERATION_DATETIME")
        if env_datetime:
            dt = datetime.strptime(env_datetime, "%Y/%m/%d %H:%M")
            return timezone("Asia/Tokyo").localize(dt)
    except ValueError:
        logger.debug("Incorrect format of datetime specified in environment variable")

    return datetime.now(timezone("Asia/Tokyo"))


def edit_supporter_user_attribute(
    supporter_user_id: str,
    user_list: List[UserModel],
    master_supporter_organization_list: List[MasterSupporterOrganizationModel],
) -> SupporterUserAttribute:
    """
        SupporterUserAttributeの編集

    Args:
        supporter_user_id (str): 支援者ユーザID
        user_list (List[UserModel]): 一般ユーザのリスト
        master_supporter_organization_list (List[MasterSupporterOrganizationModel]):
            汎用マスターの支援者組織のリスト
    Returns
        SupporterUserAttribute: 編集したAttribute
    """
    supporter_user: SupporterUserAttribute = None
    for user in user_list:
        if supporter_user_id == user.id:
            temp_id = []
            temp_name = []
            if user.supporter_organization_id:
                for organization_id in user.supporter_organization_id:
                    for supporter_organization in master_supporter_organization_list:
                        if organization_id == supporter_organization.id:
                            temp_id.append(supporter_organization.id)
                            temp_name.append(supporter_organization.value)
                            break
            supporter_organization_id = ";".join(temp_id)
            supporter_organization_name = ";".join(temp_name)

            supporter_user = SupporterUserAttribute(
                id=supporter_user_id,
                name=user.name,
                organization_id=supporter_organization_id,
                organization_name=supporter_organization_name,
            )
            break
    return supporter_user


def get_before_business_day(dt: datetime, past_day_count: int) -> datetime:
    """
        指定日時から○営業日前の日時を取得（祝日考慮なし）

    Args:
        dt (datetime): 指定日時
        past_day_count (int): 取得したい過去の営業日までの日数
    Returns
        datetime: ○営業日前の日時
    """
    temp_dt = dt
    for _ in range(past_day_count):
        temp_dt = temp_dt + timedelta(days=-1)
        while temp_dt.weekday() >= 5:
            # 土・日の場合
            temp_dt = temp_dt + timedelta(days=-1)
    return temp_dt


def get_after_business_day(dt: datetime, past_day_count: int) -> datetime:
    """
        指定日時から○営業日後の日時を取得（祝日考慮なし）

    Args:
        dt (datetime): 指定日時
        past_day_count (int): 取得したい未来の営業日までの日数
    Returns
        datetime: ○営業日後の日時
    """
    temp_dt = dt
    for _ in range(past_day_count):
        temp_dt = temp_dt + timedelta(days=1)
        while temp_dt.weekday() >= 5:
            # 土・日の場合
            temp_dt = temp_dt + timedelta(days=1)
    return temp_dt


def get_first_bussiness_day_of_the_month(
    year: int, month: int, offset: int = 0
) -> date:
    """
    月初の営業日を取得。（祝日考慮なし）
    offsetを指定することで、取得する営業日を変えることが可能。
      e.g. 月初3営業日を取得したい場合 -> offsetに2をセット

    Args:
        year (int): 年
        month (int): 月
        offset (int): オフセット値
            e.g. 月初3営業日を取得したい場合 -> offsetに2をセット
    Returns
        date: 月初の営業日（オフセット指定時は、オフセットした営業日）
    """
    temp_date = date(year=year, month=month, day=1)
    count = 0
    while True:
        if temp_date.weekday() < 5:
            # 月～金の場合
            if count >= offset:
                return temp_date
            else:
                count += 1
        temp_date = temp_date + timedelta(days=1)


def init_procedure(event, operation_start_datetime: datetime, batch_control_id: str):
    """
        初期処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """

    # 入力パラメータのチェック
    mode = event.get("mode")
    if not mode or mode not in [
        BatchInputParameterMode.REMIND_KARTE,
        BatchInputParameterMode.SCHEDULE_SURVEY,
        BatchInputParameterMode.REMIND_SURVEY,
        BatchInputParameterMode.REMIND_MANHOUR,
        BatchInputParameterMode.REMIND_CURRENT_PROGRAM,
        BatchInputParameterMode.REMIND_NEXT_PROGRAM,
    ]:
        raise Exception(f"Invalid parameter: {mode}")

    # 汎用マスター バッチ処理制御の取得
    batch_control_name = BatchFunctionName.REMIND_BATCH
    range_key_condition = MasterBatchControlModel.value == batch_control_id
    batch_control_list: List[MasterBatchControlModel] = list(
        MasterBatchControlModel.data_type_value_index.query(
            hash_key=MasterDataType.BATCH_CONTROL,
            range_key_condition=range_key_condition,
        )
    )

    # 現在日時
    datetime_now = datetime.now()

    if batch_control_list:
        batch_control_model = batch_control_list[0]
        env_datetime = os.getenv("OPERATION_DATETIME")
        # Lambda環境変数(OPERATION_DATETIME)が指定されていない場合のみ起動時刻をチェックする
        if not env_datetime:
            # 起動時刻が前回更新日時＋再処理可能期間を経過していない場合は処理終了
            rerun_limit = batch_control_model.update_at + timedelta(
                minutes=int(batch_control_model.attributes.batch_rerun_span)
            )
            if operation_start_datetime <= rerun_limit:
                # 処理終了
                logger.info(
                    "起動時刻が前回更新日時＋再処理可能期間を経過していないため、処理終了"
                )
                raise ExitHandler()

        batch_control_model.update(
            actions=[
                MasterBatchControlModel.attributes.batch_start_at.set(datetime_now),
                MasterBatchControlModel.attributes.batch_status.set(
                    BatchStatus.RUNNING
                ),
                MasterBatchControlModel.update_at.set(datetime_now),
            ]
        )

    else:
        # 汎用マスター バッチ処理制御の項目が存在しない場合、新規作成
        new_batch_control = MasterBatchControlModel(
            id=str(uuid.uuid4()),
            data_type=MasterDataType.BATCH_CONTROL,
            name=batch_control_name,
            value=batch_control_id,
            attributes=BatchControlAttribute(
                batch_start_at=datetime_now,
                batch_status=BatchStatus.RUNNING,
                batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
            ),
            update_at=datetime_now,
        )
        new_batch_control.save()


def end_procedure(event, batch_status: str, batch_control_id: str):
    """
        終了処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    # 汎用マスター バッチ処理制御の項目取得
    range_key_condition = MasterBatchControlModel.value == batch_control_id
    batch_control_list: List[MasterBatchControlModel] = list(
        MasterBatchControlModel.data_type_value_index.query(
            hash_key=MasterDataType.BATCH_CONTROL,
            range_key_condition=range_key_condition,
        )
    )
    datetime_now = datetime.now()
    batch_control_list[0].update(
        actions=[
            MasterBatchControlModel.attributes.batch_end_at.set(datetime_now),
            MasterBatchControlModel.attributes.batch_status.set(batch_status),
            MasterBatchControlModel.update_at.set(datetime_now),
        ]
    )


def remind_karte_procedure(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        カルテ書き忘れリマインド通知処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """

    # 条件: 支援日が「現在日-7日」、個別カルテ更新通知最終更新日時が存在しない
    filter_condition = None
    filter_condition &= (
        ProjectKarteModel.karte_notify_last_update_datetime.does_not_exist()
    )
    remind_datetime = operation_start_datetime + timedelta(days=-7)
    remind_date = remind_datetime.strftime("%Y/%m/%d")
    # 案件カルテの取得
    karte_list: List[ProjectKarteModel] = list(
        ProjectKarteModel.date_index.query(
            hash_key=remind_date, filter_condition=filter_condition
        )
    )
    logger.info(f"案件カルテ取得件数:{len(karte_list)}")
    # 案件情報の取得
    item_keys = [(x.project_id, DataType.PROJECT) for x in karte_list]
    project_list = list(ProjectModel.batch_get(item_keys))
    # 一般ユーザ情報の取得
    user_filter_condition = UserModel.disabled == False  # NOQA
    user_list: List[UserModel] = list(
        UserModel.scan(filter_condition=user_filter_condition)
    )

    # 通知関連情報
    notification_data: List[dict] = []
    fo_site_url = get_app_settings().fo_site_url
    target_cnt: int = 0
    # 案件カルテ単位で処理
    for karte in karte_list:
        data: dict = {}
        supporter_id_list: List[str] = []
        to_mail_list: List[str] = []
        message_param: dict = {}
        remind_flag: bool = True

        for project in project_list:
            if project.id == karte.project_id:
                # カルテの書き忘れリマインドしないの場合、スキップ
                if not project.is_karte_remind:
                    remind_flag = False
                    break

                target_cnt += 1
                if project.main_supporter_user_id:
                    for user in user_list:
                        if user.id == project.main_supporter_user_id:
                            to_mail_list.append(user.email)
                            supporter_id_list.append(project.main_supporter_user_id)
                            break
                if project.supporter_user_ids:
                    for supporter_id in project.supporter_user_ids:
                        for user in user_list:
                            if user.id == supporter_id:
                                to_mail_list.append(user.email)
                                supporter_id_list.append(supporter_id)
                                break

                message_param["project_name"] = project.name
                message_param["support_datetime"] = "{} {}～{}".format(
                    karte.date, karte.start_time, karte.end_time
                )
                message_param["fo_karte_list_url"] = (
                    fo_site_url + FoAppUrl.KARTE_LIST.format(projectId=project.id)
                )

                break

        if remind_flag:
            data["supporter_id_list"] = supporter_id_list
            data["to_mail_list"] = to_mail_list
            data["message_param"] = message_param
            notification_data.append(data)

    logger.info(f"案件カルテ処理件数:{target_cnt}")

    # お知らせ通知（一括処理）
    # お知らせ「個別カルテ書き忘れリマインド通知」
    notification_notice_at = datetime.now()
    with NotificationModel.batch_write() as notification_batch:
        for data in notification_data:
            summary, message = NotificationService.edit_message(
                notification_type=NotificationType.FORGOT_TO_WRITE_KARTEN_REMIND,
                message_param={
                    "project_name": data["message_param"]["project_name"],
                    "support_datetime": data["message_param"]["support_datetime"],
                },
            )
            notification_user_id_list = list(set(data["supporter_id_list"]))
            for user_id in notification_user_id_list:
                notification = NotificationModel(
                    id=str(uuid.uuid4()),
                    user_id=user_id,
                    summary=summary,
                    url=data["message_param"]["fo_karte_list_url"],
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

    # メール通知（一括処理）
    # 「個別カルテ書き忘れリマインド通知」メール
    # SQS send_message_batchのEntries
    send_message_entries: List[dict] = []
    for data in notification_data:
        to_addr_list = list(set(data["to_mail_list"]))
        message_body = {
            "template": MailType.FORGOT_TO_WRITE_KARTEN_REMIND,
            "to": to_addr_list,
            "cc": [],
            "payload": {
                "project_name": data["message_param"]["project_name"],
                "support_datetime": data["message_param"]["support_datetime"],
                "fo_karte_list_url": data["message_param"]["fo_karte_list_url"],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール送信
    send_mail_batch(entries=send_message_entries)

    logger.info(f"案件カルテ処理対象件数:{target_cnt}")
    logger.info(f"通知処理件数:{len(notification_data)}")


def schedule_survey_other_than_pp_procedure(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        アンケート回答依頼通知処理
            Partner Portalアンケート以外（アンケート回答依頼通知）

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    # 集計月
    summary_month = operation_start_datetime.strftime("%Y/%m")

    # PartnerPortalアンケート以外（アンケート回答依頼通知）
    # 案件アンケート取得
    range_filter_condition = ProjectSurveyModel.summary_month == summary_month
    # 回答（送信）依頼実績日時が空
    filter_condition = (
        ProjectSurveyModel.actual_survey_request_datetime.does_not_exist()
        | (size(ProjectSurveyModel.actual_survey_request_datetime) == 0)
    )
    # 回答（送信）依頼予定日時（送信予定）が現在日
    filter_condition &= ProjectSurveyModel.plan_survey_request_datetime.contains(
        operation_start_datetime.strftime("%Y/%m/%d")
    )
    survey_lists: List[ProjectSurveyModel] = list(
        ProjectSurveyModel.data_type_summary_month_index.query(
            hash_key=DataType.SURVEY,
            range_key_condition=range_filter_condition,
            filter_condition=filter_condition,
        )
    )
    # 案件id毎に並べ替え
    survey_list = sorted(survey_lists, key=lambda x: x.project_id)

    logger.info(f"案件アンケート取得件数:{len(survey_list)}")

    # アンケートマスタ取得（最新バージョン）
    survey_master_list: List[SurveyMasterModel] = list(
        SurveyMasterModel.is_latest_name_index.query(hash_key=1)
    )
    # 案件情報の取得
    item_keys = [(x.project_id, DataType.PROJECT) for x in survey_list]
    project_list = list(ProjectModel.batch_get(item_keys))
    # 一般ユーザ情報の取得
    user_list: List[UserModel] = list(UserModel.scan())
    # 汎用マスタ:支援者組織情報
    master_supporter_organization_list: List[MasterSupporterOrganizationModel] = list(
        MasterSupporterOrganizationModel.data_type_index.query(
            hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION
        )
    )
    # 汎用マスタ:サービス種別
    master_service_type_list: List[MasterSupporterOrganizationModel] = list(
        MasterSupporterOrganizationModel.data_type_index.query(
            hash_key=MasterDataType.MASTER_SERVICE_TYPE
        )
    )
    # 汎用マスタ:サービス責任者
    master_service_manager_list: list[MasterServiceManagerModel] = list(
        MasterServiceManagerModel.data_type_index.query(
            hash_key=MasterDataType.SERVICE_MANAGER
        )
    )
    # 管理ユーザ
    admin_filter_condition = AdminModel.disabled == False  # NOQA
    admin_list: List[AdminModel] = list(
        AdminModel.scan(filter_condition=admin_filter_condition)
    )

    update_at = datetime.now()
    current_datetime_str = update_at.strftime("%Y/%m/%d %H:%M")
    target_cnt: int = 0
    # 通常回答アンケートの通知関連情報
    notification_data: List[dict] = []
    # 匿名回答アンケートの通知関連情報
    anonymous_notification_data: List[dict] = []
    # 案件アンケートの更新
    with ProjectSurveyModel.batch_write() as survey_batch:
        unanswered_total_count = 0
        actual_survey_request_total_count = 0
        # 処理済みの案件かの判定
        processed_project_id = ""
        # 同じ案件で最初の送信アンケートか判定
        is_first_survey = True
        for survey in survey_list:
            data: dict = {}
            # 通常回答アンケート用
            notification_id_list: List[str] = []
            to_mail_list: List[str] = []
            cc_mail_list: List[str] = []
            bcc_mail_list: List[str] = []
            message_param: dict = {}
            # 匿名回答アンケート用
            anonymous_notification_id_list: List[str] = []
            anonymous_to_mail_list: List[str] = []
            anonymous_cc_mail_list: List[str] = []
            anonymous_bcc_mail_list: List[str] = []
            anonymous_message_param: dict = {}

            # アンケートマスタの最新バージョン取得
            survey_master_revision = ""
            for survey_master in survey_master_list:
                if survey.survey_master_id == survey_master.id:
                    survey_master_revision = survey_master.revision
                    break
            if not survey_master_revision:
                # アンケートマスタが取得できない場合
                error_message = f"Unable to get a survey master info. survey_id:{survey.id}, survey_master_id:{survey.survey_master_id}"
                logger.error(error_message)
                raise Exception(error_message)

            # 最新の案件情報
            latest_project: ProjectModel = None
            for project in project_list:
                if survey.project_id == project.id:
                    latest_project = project
                    break
            if not latest_project:
                # 案件情報が取得できない場合
                # 【暫定対応】エラーでなく、処理をスキップ
                # error_message = f"Unable to get a project info. survey_id:{survey.id}, project_id:{survey.project_id}"
                # logger.error(error_message)
                # raise Exception(error_message)
                continue

            # 案件アンケートの更新
            # アンケートマスターバージョン番号
            survey.survey_master_revision = survey_master_revision
            # 上記以外の情報
            survey.project_name = latest_project.name
            survey.customer_success = latest_project.customer_success
            survey.supporter_organization_id = latest_project.supporter_organization_id
            for supporter_organization in master_supporter_organization_list:
                if survey.supporter_organization_id == supporter_organization.id:
                    survey.supporter_organization_name = supporter_organization.value
                    break
            survey.support_date_from = latest_project.support_date_from
            survey.support_date_to = latest_project.support_date_to
            survey.main_supporter_user = edit_supporter_user_attribute(
                supporter_user_id=latest_project.main_supporter_user_id,
                user_list=user_list,
                master_supporter_organization_list=master_supporter_organization_list,
            )

            supporter_users: List[SupporterUserAttribute] = None
            if latest_project.supporter_user_ids:
                supporter_users = []
                for supporter_user_id in latest_project.supporter_user_ids:
                    supporter_users.append(
                        edit_supporter_user_attribute(
                            supporter_user_id=supporter_user_id,
                            user_list=user_list,
                            master_supporter_organization_list=master_supporter_organization_list,
                        )
                    )
            if survey.is_updated_evaluation_supporters:
                # アンケート評価対象者が更新されている場合は、評価対象者更新前のアクセラレータ（supporter_users_before_update）を更新
                survey.supporter_users_before_update = supporter_users
            else:
                # アンケート評価対象者が更新されていない場合は、suppporter_usresを更新
                survey.supporter_users = supporter_users

            survey.sales_user_id = latest_project.main_sales_user_id
            survey.service_type_id = latest_project.service_type
            service_type_name: str = None
            for master_service_type in master_service_type_list:
                if latest_project.service_type == master_service_type.id:
                    service_type_name = master_service_type.name
                    break
            survey.service_type_name = service_type_name
            service_manager_id: str = None
            service_manager_name: str = None
            for master_service_manager in master_service_manager_list:
                if (
                    master_service_manager.supporter_organization_id
                    == survey.supporter_organization_id
                ):
                    service_manager_id = master_service_manager.id
                    service_manager_name = master_service_manager.name
                    break
            survey.service_manager_id = service_manager_id
            survey.service_manager_name = service_manager_name
            survey.dedicated_survey_user_name = (
                latest_project.dedicated_survey_user_name
            )
            survey.dedicated_survey_user_email = (
                latest_project.dedicated_survey_user_email
            )
            survey.answer_user_id = latest_project.main_customer_user_id
            answer_user_name: str = None
            answer_user_email: str = None
            # 無効ユーザーか
            is_disabled_user: bool = False

            for user in user_list:
                if survey.answer_user_id == user.id:
                    if user.disabled:
                        is_disabled_user = True
                        break
                    answer_user_name = user.name
                    answer_user_email = user.email
                    break
            survey.answer_user_name = answer_user_name

            if is_disabled_user:
                # 案件のお客様代表が無効ユーザーの場合、アンケート送信せず・通知しない
                # 以降の処理はスキップ
                continue

            if survey.answer_user_id or survey.dedicated_survey_user_email:
                # 以下の場合のみ、回答依頼済みに更新
                # ・回答ユーザIDがセットされている場合
                # ・アンケート専用メールアドレスが設定されている場合（匿名回答アンケート）
                # 回答（送信）依頼実績日時
                survey.actual_survey_request_datetime = current_datetime_str

            # 「連続未回答数の加算処理」
            # クイックアンケートは連続未回答数を付与しない
            if survey.survey_type != SurveyType.QUICK:
                # 同じ案件の案件アンケートをすべて取得
                # 送信実績日が存在するものだけ
                # クイックアンケート以外
                unanswered_survey_list: List[ProjectSurveyModel] = list(
                    ProjectSurveyModel.project_id_summary_month_index.query(
                        hash_key=survey.project_id,
                        filter_condition=(
                            ProjectSurveyModel.actual_survey_request_datetime.exists()
                            & (ProjectSurveyModel.survey_type != SurveyType.QUICK)
                        ),
                    )
                )
                # 送信順が大きい順に並び替え
                sorted_unanswered_survey_list = sorted(
                    unanswered_survey_list,
                    key=lambda x: x.actual_survey_request_number,
                    reverse=True,
                )

                # 案件が処理済みか否か
                # 未処理（違う案件）の場合は[unanswered_total_count]と[actual_survey_request_total_count]と[is_first_survey]を初期化
                if survey.project_id != processed_project_id:
                    unanswered_total_count = 0
                    actual_survey_request_total_count = 0
                    is_first_survey = True

                # 送信済みのアンケートが存在する場合
                if len(sorted_unanswered_survey_list) > 0:
                    # 最初の送信アンケート
                    if is_first_survey:
                        # 送信済みの最新の連続未回答数に+1
                        unanswered_total_count = (
                            sorted_unanswered_survey_list[0].unanswered_surveys_number
                            + 1
                        )
                        survey.unanswered_surveys_number = unanswered_total_count
                        # 送信済みの最新の送信順に+1
                        actual_survey_request_total_count = (
                            sorted_unanswered_survey_list[
                                0
                            ].actual_survey_request_number
                            + 1
                        )
                        survey.actual_survey_request_number = (
                            actual_survey_request_total_count
                        )
                        processed_project_id = survey.project_id
                        is_first_survey = False
                    else:
                        unanswered_total_count += 1
                        survey.unanswered_surveys_number = unanswered_total_count
                        actual_survey_request_total_count += 1
                        survey.actual_survey_request_number = (
                            actual_survey_request_total_count
                        )
                # 送信済みのアンケートが存在しない場合
                else:
                    unanswered_total_count += 1
                    survey.unanswered_surveys_number = unanswered_total_count
                    actual_survey_request_total_count += 1
                    survey.actual_survey_request_number = (
                        actual_survey_request_total_count
                    )
                    processed_project_id = survey.project_id
            else:
                survey.unanswered_surveys_number = None
                survey.actual_survey_request_number = None

            survey.customer_id = latest_project.customer_id
            survey.customer_name = latest_project.customer_name
            survey.is_solver_project = latest_project.is_solver_project
            survey.update_id = batch_control_id
            survey.update_at = update_at
            survey.version += 1
            survey_batch.save(survey)

            target_cnt += 1

            if not survey.answer_user_id and not survey.dedicated_survey_user_email:
                # 以下がいずれもセットされていない場合、アンケート送信せず・通知しない
                # ・アンケート専用メールアドレス（匿名回答アンケート）
                # ・回答ユーザID
                # 以降の処理はスキップ
                continue

            # アンケート宛先の優先順位
            # ・お客様代表: 通常アンケート（入力されている場合）
            # ・匿名アンケート送信宛先: 匿名回答アンケート（入力されている場合）
            if survey.answer_user_id:
                # 通常回答アンケートの場合
                # お知らせ通知ユーザ/メール通知アドレス
                for user in user_list:
                    # 有効なユーザーのみに通知を送る
                    if not user.disabled:
                        # お客様
                        if survey.answer_user_id == user.id:
                            notification_id_list.append(survey.answer_user_id)
                            to_mail_list.append(answer_user_email)
                        # 営業担当者あるいは営業責任者
                        if survey.sales_user_id and survey.sales_user_id == user.id:
                            notification_id_list.append(user.id)
                            cc_mail_list.append(user.email)
                # アンケート事務局
                for admin in admin_list:
                    if UserRoleType.SURVEY_OPS.key in admin.roles:
                        bcc_mail_list.append(admin.email)

                # お知らせ・メールの本文の編集
                message_param["customer_name_project_name"] = (
                    "{customer}／{project}".format(
                        customer=survey.customer_name,
                        project=survey.project_name,
                    )
                )
                message_param["customer_name"] = "{customer}".format(
                    customer=survey.customer_name,
                )
                message_param["project_name"] = "{project}".format(
                    project=survey.project_name,
                )
                message_param["summary_month"] = "{year}年{month}月".format(
                    year=survey.summary_month[0:4],
                    month=survey.summary_month[5:],
                )
                message_param["formatted_month"] = "{month}".format(
                    month=int(survey.summary_month[5:]),
                )
                message_param["answer_user_name"] = answer_user_name
                survey_type_name = ""
                if survey.survey_type == SurveyType.SERVICE:
                    survey_type_name = SurveyTypeName.SERVICE
                elif survey.survey_type == SurveyType.COMPLETION:
                    survey_type_name = SurveyTypeName.COMPLETION
                elif survey.survey_type == SurveyType.QUICK:
                    survey_type_name = SurveyTypeName.QUICK
                message_param["survey_type_name"] = survey_type_name
                # URL
                fo_site_url = get_app_settings().fo_site_url
                fo_survey_detail_url = fo_site_url + FoAppUrl.SURVEY_DETAIL.format(
                    surveyId=survey.id
                )
                message_param["fo_survey_detail_url"] = fo_survey_detail_url

                if survey.plan_survey_response_datetime:
                    # ○月○日(曜日) ※ゼロ埋めなし
                    datetime_due_date = datetime.strptime(
                        survey.plan_survey_response_datetime, "%Y/%m/%d %H:%M"
                    )
                    message_param["survey_answer_due_date"] = (
                        f"{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日"
                        + f"({get_day_of_week(datetime_due_date)})"
                    )
                    message_param["survey_answer_due_date_title"] = (
                        f"（回答期限：{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日迄）"
                    )
                else:
                    message_param["survey_answer_due_date"] = ""
                    message_param["survey_answer_due_date_title"] = ""

                data["notification_id_list"] = notification_id_list
                data["to_mail_list"] = to_mail_list
                data["cc_mail_list"] = cc_mail_list
                data["bcc_mail_list"] = bcc_mail_list
                data["message_param"] = message_param
                notification_data.append(data)

            elif survey.dedicated_survey_user_email:
                # 匿名回答アンケートの場合

                # アンケートパスワード復号化
                survey_password_decrypted = decrypt(latest_project.survey_password)

                # メール通知アドレス
                anonymous_to_mail_list.append(survey.dedicated_survey_user_email)
                for user in user_list:
                    # 有効なユーザーのみに通知を送る
                    if not user.disabled:
                        # 営業担当者あるいは営業責任者
                        if survey.sales_user_id and survey.sales_user_id == user.id:
                            anonymous_cc_mail_list.append(user.email)
                            break
                # アンケート事務局
                for admin in admin_list:
                    if UserRoleType.SURVEY_OPS.key in admin.roles:
                        anonymous_bcc_mail_list.append(admin.email)

                # お知らせ・メールの本文の編集
                anonymous_message_param["customer_name_project_name"] = (
                    "{customer}／{project}".format(
                        customer=survey.customer_name,
                        project=survey.project_name,
                    )
                )
                anonymous_message_param["customer_name"] = "{customer}".format(
                    customer=survey.customer_name,
                )
                anonymous_message_param["project_name"] = "{project}".format(
                    project=survey.project_name,
                )
                anonymous_message_param["summary_month"] = "{year}年{month}月".format(
                    year=survey.summary_month[0:4],
                    month=survey.summary_month[5:],
                )
                anonymous_message_param["formatted_month"] = "{month}".format(
                    month=int(survey.summary_month[5:]),
                )
                anonymous_message_param["answer_user_name"] = (
                    survey.dedicated_survey_user_name
                )
                survey_type_name = ""
                if survey.survey_type == SurveyType.SERVICE:
                    survey_type_name = SurveyTypeName.SERVICE
                elif survey.survey_type == SurveyType.COMPLETION:
                    survey_type_name = SurveyTypeName.COMPLETION
                elif survey.survey_type == SurveyType.QUICK:
                    survey_type_name = SurveyTypeName.QUICK
                anonymous_message_param["survey_type_name"] = survey_type_name
                # URL
                fo_site_url = get_app_settings().fo_site_url
                jwt_current_datetime = get_operation_datetime()
                jwt_survey_payload = create_jwt_survey_payload(
                    survey_id=survey.id, current_datetime=jwt_current_datetime
                )
                jwt = create_jwt(jwt_survey_payload)
                anonymous_fo_survey_detail_url = (
                    fo_site_url
                    + FoAppUrl.ANONYMOUS_SURVEY
                    + JwtSettingInfo.URL_JWT_QUERY.format(jwt=jwt)
                )
                anonymous_message_param["fo_survey_detail_url"] = (
                    anonymous_fo_survey_detail_url
                )
                anonymous_message_param["survey_password"] = survey_password_decrypted

                if survey.plan_survey_response_datetime:
                    # ○月○日(曜日) ※ゼロ埋めなし
                    datetime_due_date = datetime.strptime(
                        survey.plan_survey_response_datetime, "%Y/%m/%d %H:%M"
                    )
                    anonymous_message_param["survey_answer_due_date"] = (
                        f"{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日"
                        + f"({get_day_of_week(datetime_due_date)})"
                    )
                    anonymous_message_param["survey_answer_due_date_title"] = (
                        f"（回答期限：{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日迄）"
                    )
                else:
                    anonymous_message_param["survey_answer_due_date"] = ""
                    anonymous_message_param["survey_answer_due_date_title"] = ""

                anonymous_message_param["project_identification_id"] = latest_project.id

                data["notification_id_list"] = anonymous_notification_id_list
                data["to_mail_list"] = anonymous_to_mail_list
                data["cc_mail_list"] = anonymous_cc_mail_list
                data["bcc_mail_list"] = anonymous_bcc_mail_list
                data["message_param"] = anonymous_message_param
                anonymous_notification_data.append(data)

    notification_notice_at = datetime.now()
    # お知らせ通知（一括処理）
    # お知らせ「アンケート回答依頼通知」
    # 対象: 通常回答アンケート
    with NotificationModel.batch_write() as notification_batch:
        for data in notification_data:
            summary, message = NotificationService.edit_message(
                notification_type=NotificationType.SURVEY_ANSWER_REQUEST,
                message_param={
                    "customer_name_project_name": data["message_param"][
                        "customer_name_project_name"
                    ],
                    "summary_month": data["message_param"]["summary_month"],
                    "survey_type_name": data["message_param"]["survey_type_name"],
                    "fo_survey_detail_url": data["message_param"][
                        "fo_survey_detail_url"
                    ],
                    "survey_answer_due_date_title": data["message_param"][
                        "survey_answer_due_date_title"
                    ],
                    "survey_answer_due_date": data["message_param"][
                        "survey_answer_due_date"
                    ],
                },
            )
            notification_user_id_list = list(set(data["notification_id_list"]))
            for user_id in notification_user_id_list:
                notification = NotificationModel(
                    id=str(uuid.uuid4()),
                    user_id=user_id,
                    summary=summary,
                    url=data["message_param"]["fo_survey_detail_url"],
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

    # SQS send_message_batchのEntries
    send_message_entries: List[dict] = []
    # メール通知（一括処理）
    # 「アンケート回答依頼通知」メール
    # 対象: 通常回答アンケート
    for data in notification_data:
        to_addr_list = list(set(data["to_mail_list"]))
        cc_addr_list = list(set(data["cc_mail_list"]))
        bcc_addr_list = list(set(data["bcc_mail_list"]))
        message_body = {
            "template": MailType.SURVEY_ANSWER_REQUEST,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "bcc": bcc_addr_list,
            "payload": {
                "customer_name": data["message_param"]["customer_name"],
                "project_name": data["message_param"]["project_name"],
                "summary_month": data["message_param"]["summary_month"],
                "formatted_month": data["message_param"]["formatted_month"],
                "answer_user_name": data["message_param"]["answer_user_name"],
                "survey_type_name": data["message_param"]["survey_type_name"],
                "fo_survey_detail_url": data["message_param"]["fo_survey_detail_url"],
                "survey_answer_due_date_title": data["message_param"][
                    "survey_answer_due_date_title"
                ],
                "survey_answer_due_date": data["message_param"][
                    "survey_answer_due_date"
                ],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール通知（一括処理）
    # 「匿名アンケート回答依頼通知」メール
    # 対象: 匿名回答アンケート
    for data in anonymous_notification_data:
        to_addr_list = list(set(data["to_mail_list"]))
        cc_addr_list = list(set(data["cc_mail_list"]))
        bcc_addr_list = list(set(data["bcc_mail_list"]))
        message_body = {
            "template": MailType.SURVEY_ANONYMOUS_ANSWER_REQUEST,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "bcc": bcc_addr_list,
            "payload": {
                "customer_name": data["message_param"]["customer_name"],
                "project_name": data["message_param"]["project_name"],
                "summary_month": data["message_param"]["summary_month"],
                "formatted_month": data["message_param"]["formatted_month"],
                "answer_user_name": (
                    data["message_param"]["answer_user_name"]
                    if data["message_param"]["answer_user_name"] != ""
                    else "担当者"
                ),
                "survey_type_name": data["message_param"]["survey_type_name"],
                "fo_survey_detail_url": data["message_param"]["fo_survey_detail_url"],
                "survey_answer_due_date_title": data["message_param"][
                    "survey_answer_due_date_title"
                ],
                "survey_answer_due_date": data["message_param"][
                    "survey_answer_due_date"
                ],
                "project_identification_id": data["message_param"][
                    "project_identification_id"
                ],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール通知（一括処理）
    # 「匿名回答アンケートパスワード通知」メール
    # 対象: 匿名回答アンケート
    for data in anonymous_notification_data:
        to_addr_list = list(set(data["to_mail_list"]))
        cc_addr_list = list(set(data["cc_mail_list"]))
        bcc_addr_list = list(set(data["bcc_mail_list"]))
        message_body = {
            "template": MailType.SURVEY_ANONYMOUS_PASSWORD,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "bcc": bcc_addr_list,
            "payload": {
                "customer_name": data["message_param"]["customer_name"],
                "project_name": data["message_param"]["project_name"],
                "summary_month": data["message_param"]["summary_month"],
                "formatted_month": data["message_param"]["formatted_month"],
                "answer_user_name": (
                    data["message_param"]["answer_user_name"]
                    if data["message_param"]["answer_user_name"] != ""
                    else "担当者"
                ),
                "survey_type_name": data["message_param"]["survey_type_name"],
                "survey_password": data["message_param"]["survey_password"],
                "project_identification_id": data["message_param"][
                    "project_identification_id"
                ],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール送信
    send_mail_batch(entries=send_message_entries)

    logger.info(f"案件アンケート更新件数:{target_cnt}")
    logger.info(f"通常アンケート 通知処理件数:{len(notification_data)}")
    logger.info(f"匿名アンケート 通知処理件数:{len(anonymous_notification_data)}")


def schedule_survey_pp_procedure(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        アンケート回答依頼通知処理
            Partner Portalアンケート（Partner Portal利用アンケート回答依頼通知）

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    # 集計月
    summary_month = operation_start_datetime.strftime("%Y/%m")
    # 現在日
    current_day: float = float(operation_start_datetime.strftime("%d"))
    # アンケートマスタ取得（PPアンケートの最新バージョン）
    survey_master_filter_condition = SurveyMasterModel.type == SurveyType.PP
    survey_master_list: List[SurveyMasterModel] = list(
        SurveyMasterModel.is_latest_name_index.query(
            hash_key=1, filter_condition=survey_master_filter_condition
        )
    )
    logger.info(f"アンケートマスタ取得件数:{len(survey_master_list)}")
    logger.info(
        f"アンケートマスタの送信日初期設定:{survey_master_list[0].init_send_day_setting}"
    )
    logger.info(f'起動日時:{operation_start_datetime.strftime("%Y/%m/%d %H:%M")}')

    target_cnt: int = 0
    if (
        survey_master_list
        and survey_master_list[0].init_send_day_setting == current_day
    ):
        # 送信日初期設定が本日と一致する場合に以下を処理する
        user_filter_condition = None
        user_filter_condition &= UserModel.role != UserRoleType.CUSTOMER.key
        user_filter_condition &= UserModel.role != UserRoleType.SOLVER_STAFF.key
        user_filter_condition &= UserModel.role != UserRoleType.APT.key
        user_filter_condition &= UserModel.disabled == False  # NOQA
        user_list: List[UserModel] = list(
            UserModel.scan(filter_condition=user_filter_condition)
        )

        current_datetime = datetime.now()
        plan_survey_response_datetime = None
        pp_answer_due_date = None
        due_datetime_week = None
        if survey_master_list[0].init_answer_limit_day_setting != float(0):
            temp_datetime = operation_start_datetime
            temp_answer_limit_int = int(
                survey_master_list[0].init_answer_limit_day_setting
            )
            # 月末日指定
            if temp_answer_limit_int == 99:
                # get_limit_dateを使用するためstr型に変換
                temp_datetime_str = SchedulesService.get_limit_date(
                    temp_datetime.strftime("%Y/%m/%d"), temp_answer_limit_int
                )
                # 返り値はstr型なのでdatetime型に変換
                temp_datetime = datetime.strptime(temp_datetime_str, "%Y/%m/%d")
            else:
                # get_limit_dateを使用するためstr型に変換
                temp_datetime_str = SchedulesService.get_limit_date(
                    temp_datetime.strftime("%Y/%m/%d"), temp_answer_limit_int
                )
                # 返り値はstr型なのでdatetime型に変換
                temp_datetime = datetime.strptime(temp_datetime_str, "%Y/%m/%d")
            plan_survey_response_datetime = (
                temp_datetime.strftime("%Y/%m")
                + "/"
                + temp_datetime.strftime("%d").zfill(2)
                + " "
                + "09:00"
            )
            # 回答期限日
            pp_answer_due_date = datetime(
                year=temp_datetime.year,
                month=temp_datetime.month,
                day=temp_datetime.day,
            )
            # 回答期限の曜日を取得
            due_datetime_week = get_day_of_week(pp_answer_due_date)
        # 管理ユーザ
        admin_filter_condition = AdminModel.disabled == False  # NOQA
        admin_list: List[AdminModel] = list(
            AdminModel.scan(filter_condition=admin_filter_condition)
        )
        # 通知関連情報
        notification_data: List[dict] = []
        with ProjectSurveyModel.batch_write() as survey_batch:
            for user in user_list:
                data: dict = {}
                notification_id_list: List[str] = []
                to_mail_list: List[str] = []
                cc_mail_list: List[str] = []
                message_param: dict = {}

                # 送信済みの案件アンケートをユーザ数分作成
                new_survey = ProjectSurveyModel(
                    id=str(uuid.uuid4()),
                    data_type=DataType.SURVEY,
                    survey_master_id=survey_master_list[0].id,
                    survey_master_revision=survey_master_list[0].revision,
                    survey_type=SurveyType.PP,
                    answer_user_id=user.id,
                    answer_user_name=user.name,
                    company=user.company,
                    summary_month=summary_month,
                    plan_survey_request_datetime=operation_start_datetime.strftime(
                        "%Y/%m/%d 09:00"
                    ),
                    actual_survey_request_datetime=current_datetime.strftime(
                        "%Y/%m/%d %H:%M"
                    ),
                    plan_survey_response_datetime=plan_survey_response_datetime,
                    points=PointsAttribute(
                        satisfaction=0,
                        continuation=False,
                        recommended=0,
                        sales=0,
                        survey_satisfaction=0,
                        man_hour_satisfaction=0,
                        karte_satisfaction=0,
                        master_karte_satisfaction=0,
                    ),
                    create_id=batch_control_id,
                    create_at=current_datetime,
                    update_id=batch_control_id,
                    update_at=current_datetime,
                    version=1,
                )
                survey_batch.save(new_survey)

                target_cnt += 1

                # お知らせ通知ユーザ
                notification_id_list.append(user.id)
                # メール通知アドレス
                to_mail_list.append(user.email)
                for admin in admin_list:
                    if UserRoleType.SYSTEM_ADMIN.key in admin.roles:
                        cc_mail_list.append(admin.email)

                # お知らせ・メールの本文の編集
                message_param["summary_month"] = "{year}年{month}月".format(
                    year=summary_month[0:4],
                    month=summary_month[5:],
                )
                message_param["pp_answer_due_date_title"] = (
                    (f"{int(pp_answer_due_date.month):d}月{pp_answer_due_date.day}日")
                    if pp_answer_due_date
                    else None
                )
                message_param["pp_answer_due_date"] = (
                    (
                        f"{int(pp_answer_due_date.month):d}月{pp_answer_due_date.day}日({due_datetime_week})"
                    )
                    if pp_answer_due_date
                    else None
                )
                message_param["survey_type_name"] = SurveyTypeName.PP
                # URL
                fo_site_url = get_app_settings().fo_site_url
                fo_survey_detail_url = fo_site_url + FoAppUrl.SURVEY_PP_DETAIL.format(
                    surveyId=new_survey.id
                )
                message_param["fo_survey_detail_url"] = fo_survey_detail_url

                data["notification_id_list"] = notification_id_list
                data["to_mail_list"] = to_mail_list
                data["cc_mail_list"] = cc_mail_list
                data["message_param"] = message_param
                notification_data.append(data)

        # お知らせ
        # お知らせ通知（一括処理）
        # お知らせ「Partner Portal利用アンケート回答依頼通知」
        notification_notice_at = datetime.now()
        with NotificationModel.batch_write() as notification_batch:
            for data in notification_data:
                summary, message = NotificationService.edit_message(
                    notification_type=NotificationType.PP_SURVEY_ANSWER_REQUEST,
                    message_param={
                        "summary_month": data["message_param"]["summary_month"],
                        "pp_answer_due_date_title": data["message_param"][
                            "pp_answer_due_date_title"
                        ],
                        "pp_answer_due_date": data["message_param"][
                            "pp_answer_due_date"
                        ],
                        "survey_type_name": data["message_param"]["survey_type_name"],
                        "fo_survey_detail_url": data["message_param"][
                            "fo_survey_detail_url"
                        ],
                    },
                )
                notification_user_id_list = list(set(data["notification_id_list"]))
                for user_id in notification_user_id_list:
                    notification = NotificationModel(
                        id=str(uuid.uuid4()),
                        user_id=user_id,
                        summary=summary,
                        url=data["message_param"]["fo_survey_detail_url"],
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

        # メール通知（一括処理）
        # 「Partner Portal利用アンケート回答依頼通知」メール
        # SQS send_message_batchのEntries
        send_message_entries: List[dict] = []
        for data in notification_data:
            to_addr_list = list(set(data["to_mail_list"]))
            cc_addr_list = list(set(data["cc_mail_list"]))
            message_body = {
                "template": MailType.PP_SURVEY_ANSWER_REQUEST,
                "to": to_addr_list,
                "cc": cc_addr_list,
                "payload": {
                    "summary_month": data["message_param"]["summary_month"],
                    "pp_answer_due_date_title": data["message_param"][
                        "pp_answer_due_date_title"
                    ],
                    "pp_answer_due_date": data["message_param"]["pp_answer_due_date"],
                    "survey_type_name": data["message_param"]["survey_type_name"],
                    "fo_survey_detail_url": data["message_param"][
                        "fo_survey_detail_url"
                    ],
                },
            }
            sqs_message_body = json.dumps(message_body)
            send_message_entries.append(
                {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
            )

        # メール送信
        send_mail_batch(entries=send_message_entries)

        logger.info(f"案件アンケート作成件数:{target_cnt}")
        logger.info(f"通知処理件数:{len(notification_data)}")


def remind_survey_procedure(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        アンケート催促通知処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    logger.info(f'起動日時:{operation_start_datetime.strftime("%Y/%m/%d %H:%M")}')
    # 非営業日は処理をスキップ
    if operation_start_datetime.weekday() >= 5:
        # 土・日の場合
        logger.info("非営業日のため、処理を終了します。")
        raise ExitHandler()
    # 集計月
    summary_month = (operation_start_datetime + relativedelta(months=-1)).strftime(
        "%Y/%m"
    )
    # 抽出条件の編集
    # 集計月：現在日の前月以降
    range_key_condition = ProjectSurveyModel.summary_month >= summary_month
    # 回答（送信）依頼実績日時（送信実績）が空でない
    filter_condition = None
    filter_condition &= ProjectSurveyModel.actual_survey_request_datetime.exists() & (
        size(ProjectSurveyModel.actual_survey_request_datetime) != 0
    )
    # 回答（受信）予定日時（回答期限）が空でない
    filter_condition &= ProjectSurveyModel.plan_survey_response_datetime.exists() & (
        size(ProjectSurveyModel.plan_survey_response_datetime) != 0
    )
    # 回答（受信）予定日時（回答期限）が「現在日プラス1営業日」または「現在日マイナス1営業日」
    current_datetime_minus_1_days = get_before_business_day(operation_start_datetime, 1)
    current_datetime_plus_1_days = get_after_business_day(operation_start_datetime, 1)
    filter_condition &= ProjectSurveyModel.plan_survey_response_datetime.contains(
        current_datetime_plus_1_days.strftime("%Y/%m/%d")
    ) | ProjectSurveyModel.plan_survey_response_datetime.contains(
        current_datetime_minus_1_days.strftime("%Y/%m/%d")
    )
    # 回答（受信）実績日時（回答実績）が空
    filter_condition &= (
        ProjectSurveyModel.actual_survey_response_datetime.does_not_exist()
        | (size(ProjectSurveyModel.plan_survey_response_datetime) == 0)
    )
    # Partner Portalアンケート以外
    filter_condition &= ProjectSurveyModel.survey_type != SurveyType.PP
    survey_list: List[ProjectSurveyModel] = list(
        ProjectSurveyModel.data_type_summary_month_index.query(
            hash_key=DataType.SURVEY,
            range_key_condition=range_key_condition,
            filter_condition=filter_condition,
        )
    )
    logger.info(f"案件アンケート取得件数:{len(survey_list)}")
    # 一般ユーザ情報の取得
    user_filter_condition = UserModel.disabled == False  # NOQA
    user_list: List[UserModel] = list(
        UserModel.scan(filter_condition=user_filter_condition)
    )
    # 管理ユーザ
    admin_filter_condition = AdminModel.disabled == False  # NOQA
    admin_list: List[AdminModel] = list(
        AdminModel.scan(filter_condition=admin_filter_condition)
    )
    # 案件情報の取得
    item_keys = [(x.project_id, DataType.PROJECT) for x in survey_list]
    project_list = list(ProjectModel.batch_get(item_keys))
    # 通常回答アンケートの通知関連情報
    notification_data: List[dict] = []
    # 匿名回答アンケートの通知関連情報
    anonymous_notification_data: List[dict] = []
    for survey in survey_list:
        # 最新の案件情報
        latest_project: ProjectModel = None
        for project in project_list:
            if survey.project_id == project.id:
                latest_project = project
                break
        if not latest_project:
            # 案件情報が取得できない場合、処理をスキップ
            continue

        data: dict = {}
        # 通常回答アンケート用
        notification_id_list: List[str] = []
        to_mail_list: List[str] = []
        cc_mail_list: List[str] = []
        bcc_mail_list: List[str] = []
        message_param: dict = {}
        # 匿名回答アンケート用
        anonymous_notification_id_list: List[str] = []
        anonymous_to_mail_list: List[str] = []
        anonymous_cc_mail_list: List[str] = []
        anonymous_bcc_mail_list: List[str] = []
        anonymous_message_param: dict = {}

        if not survey.answer_user_id and not survey.dedicated_survey_user_email:
            # 以下がいずれもセットされていない場合、アンケート送信せず・通知しない
            # ・案件情報のアンケート専用メールアドレス（匿名回答アンケート）
            # ・回答ユーザID
            # 以降の処理はスキップ
            continue

        # 満足度評価のみ回答URL付与対象アンケートかの確認
        # 対象外：クイックアンケート, 総合満足度設問が存在しない、または無効になっているサービス/修了アンケート
        survey_master = SurveyMasterModel.get(
            hash_key=survey.survey_master_id, range_key=survey.survey_master_revision
        )
        is_satisfaction_survey = False
        # クイックアンケートの場合
        if survey_master.type == SurveyTypeForGetSurveys.QUICK:
            is_satisfaction_survey = False
        else:
            for question in survey_master.questions:
                if (
                    question.summary_type == SurveyQuestionsSummaryType.SATISFACTION
                    and not question.disabled
                ):
                    is_satisfaction_survey = True
                    break

        message_param["is_satisfaction_survey"] = is_satisfaction_survey
        anonymous_message_param["is_satisfaction_survey"] = is_satisfaction_survey

        # アンケート宛先の優先順位
        # ・お客様代表: 通常アンケート（入力されている場合）
        # ・匿名アンケート送信宛先: 匿名回答アンケート（入力されている場合）
        if survey.answer_user_id:
            # 通常回答アンケートの場合
            # お知らせ通知ユーザ/メール通知アドレス
            for user in user_list:
                # 有効なユーザーのみに通知を送る
                if not user.disabled:
                    # お客様
                    if survey.answer_user_id == user.id:
                        notification_id_list.append(user.id)
                        to_mail_list.append(user.email)
                        answer_user_name = user.name
                    # 営業担当者あるいは営業責任者
                    if survey.sales_user_id and survey.sales_user_id == user.id:
                        notification_id_list.append(user.id)
                        cc_mail_list.append(user.email)

            if len(to_mail_list) == 0:
                # お客様が存在しない場合、催促通知を送付しない
                # 処理をスキップ
                continue

            # アンケート事務局
            for admin in admin_list:
                if UserRoleType.SURVEY_OPS.key in admin.roles:
                    bcc_mail_list.append(admin.email)

            # お知らせ・メールの本文の編集
            message_param["customer_name_project_name"] = (
                "{customer}／{project}".format(
                    customer=survey.customer_name,
                    project=survey.project_name,
                )
            )
            message_param["customer_name"] = "{customer}".format(
                customer=survey.customer_name,
            )
            message_param["project_name"] = "{project}".format(
                project=survey.project_name,
            )
            message_param["summary_month"] = "{year}年{month}月".format(
                year=survey.summary_month[0:4],
                month=survey.summary_month[5:],
            )
            message_param["formatted_month"] = "{month}".format(
                month=int(survey.summary_month[5:]),
            )
            message_param["answer_user_name"] = answer_user_name
            survey_type_name = ""
            if survey.survey_type == SurveyType.SERVICE:
                survey_type_name = SurveyTypeName.SERVICE
            elif survey.survey_type == SurveyType.COMPLETION:
                survey_type_name = SurveyTypeName.COMPLETION
            elif survey.survey_type == SurveyType.QUICK:
                survey_type_name = SurveyTypeName.QUICK
            message_param["survey_type_name"] = survey_type_name
            # URL
            fo_site_url = get_app_settings().fo_site_url
            fo_survey_detail_url = fo_site_url + FoAppUrl.SURVEY_DETAIL.format(
                surveyId=survey.id
            )
            message_param["fo_survey_detail_url"] = fo_survey_detail_url
            # 満足度評価のみ回答アンケートURL
            jwt_current_datetime = get_operation_datetime()
            jwt_survey_payload = create_jwt_survey_payload(
                survey_id=survey.id, current_datetime=jwt_current_datetime
            )
            jwt = create_jwt(jwt_survey_payload)
            fo_satisfaction_survey_url = (
                fo_site_url
                + FoAppUrl.SATISFACTION_SURVEY
                + JwtSettingInfo.URL_JWT_QUERY.format(jwt=jwt)
            )
            message_param["fo_satisfaction_survey_url"] = fo_satisfaction_survey_url

            if survey.plan_survey_response_datetime:
                # ○月○日(曜日) ※ゼロ埋めなし
                datetime_due_date = datetime.strptime(
                    survey.plan_survey_response_datetime, "%Y/%m/%d %H:%M"
                )
                message_param["survey_answer_due_date"] = (
                    f"{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日"
                    + f"({get_day_of_week(datetime_due_date)})"
                )
                message_param["survey_answer_due_date_title"] = (
                    f"（回答期限：{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日迄）"
                )
            else:
                message_param["survey_answer_due_date"] = ""
                message_param["survey_answer_due_date_title"] = ""

            data["notification_id_list"] = notification_id_list
            data["to_mail_list"] = to_mail_list
            data["cc_mail_list"] = cc_mail_list
            data["bcc_mail_list"] = bcc_mail_list
            data["message_param"] = message_param
            notification_data.append(data)

        elif survey.dedicated_survey_user_email:
            # 匿名回答アンケートの場合

            # アンケートパスワード復号化
            survey_password_decrypted = decrypt(latest_project.survey_password)

            # メール通知アドレス
            anonymous_to_mail_list.append(survey.dedicated_survey_user_email)
            for user in user_list:
                # 有効なユーザーのみに通知を送る
                if not user.disabled:
                    # 営業担当者あるいは営業責任者
                    if survey.sales_user_id and survey.sales_user_id == user.id:
                        anonymous_cc_mail_list.append(user.email)
                        break
            # アンケート事務局
            for admin in admin_list:
                if UserRoleType.SURVEY_OPS.key in admin.roles:
                    anonymous_bcc_mail_list.append(admin.email)

            # お知らせ・メールの本文の編集
            anonymous_message_param["customer_name_project_name"] = (
                "{customer}／{project}".format(
                    customer=survey.customer_name,
                    project=survey.project_name,
                )
            )
            anonymous_message_param["customer_name"] = "{customer}".format(
                customer=survey.customer_name,
            )
            anonymous_message_param["project_name"] = "{project}".format(
                project=survey.project_name,
            )
            anonymous_message_param["summary_month"] = "{year}年{month}月".format(
                year=survey.summary_month[0:4],
                month=survey.summary_month[5:],
            )
            anonymous_message_param["formatted_month"] = "{month}".format(
                month=int(survey.summary_month[5:]),
            )
            anonymous_message_param["answer_user_name"] = (
                survey.dedicated_survey_user_name
            )
            survey_type_name = ""
            if survey.survey_type == SurveyType.SERVICE:
                survey_type_name = SurveyTypeName.SERVICE
            elif survey.survey_type == SurveyType.COMPLETION:
                survey_type_name = SurveyTypeName.COMPLETION
            elif survey.survey_type == SurveyType.QUICK:
                survey_type_name = SurveyTypeName.QUICK
            anonymous_message_param["survey_type_name"] = survey_type_name
            # URL
            fo_site_url = get_app_settings().fo_site_url
            jwt_current_datetime = get_operation_datetime()
            jwt_survey_payload = create_jwt_survey_payload(
                survey_id=survey.id, current_datetime=jwt_current_datetime
            )
            jwt = create_jwt(jwt_survey_payload)
            anonymous_fo_survey_detail_url = (
                fo_site_url
                + FoAppUrl.ANONYMOUS_SURVEY
                + JwtSettingInfo.URL_JWT_QUERY.format(jwt=jwt)
            )
            anonymous_message_param["fo_survey_detail_url"] = (
                anonymous_fo_survey_detail_url
            )
            anonymous_message_param["survey_password"] = survey_password_decrypted
            # 満足度評価のみ回答アンケートURL
            fo_satisfaction_survey_url = (
                fo_site_url
                + FoAppUrl.SATISFACTION_SURVEY
                + JwtSettingInfo.URL_JWT_QUERY.format(jwt=jwt)
            )
            anonymous_message_param["fo_satisfaction_survey_url"] = (
                fo_satisfaction_survey_url
            )

            if survey.plan_survey_response_datetime:
                # ○月○日(曜日) ※ゼロ埋めなし
                datetime_due_date = datetime.strptime(
                    survey.plan_survey_response_datetime, "%Y/%m/%d %H:%M"
                )
                anonymous_message_param["survey_answer_due_date"] = (
                    f"{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日"
                    + f"({get_day_of_week(datetime_due_date)})"
                )
                anonymous_message_param["survey_answer_due_date_title"] = (
                    f"（回答期限：{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日迄）"
                )
            else:
                anonymous_message_param["survey_answer_due_date"] = ""
                anonymous_message_param["survey_answer_due_date_title"] = ""

            anonymous_message_param["project_identification_id"] = latest_project.id

            data["notification_id_list"] = anonymous_notification_id_list
            data["to_mail_list"] = anonymous_to_mail_list
            data["cc_mail_list"] = anonymous_cc_mail_list
            data["bcc_mail_list"] = anonymous_bcc_mail_list
            data["message_param"] = anonymous_message_param
            anonymous_notification_data.append(data)

    notification_notice_at = datetime.now()
    # お知らせ通知（一括処理）
    # お知らせ「アンケート催促通知」
    # 対象: 通常回答アンケート
    with NotificationModel.batch_write() as notification_batch:
        for data in notification_data:
            summary, message = NotificationService.edit_message(
                notification_type=NotificationType.SURVEY_REMIND,
                message_param={
                    "customer_name_project_name": data["message_param"][
                        "customer_name_project_name"
                    ],
                    "summary_month": data["message_param"]["summary_month"],
                    "survey_type_name": data["message_param"]["survey_type_name"],
                    "fo_survey_detail_url": data["message_param"][
                        "fo_survey_detail_url"
                    ],
                    "survey_answer_due_date_title": data["message_param"][
                        "survey_answer_due_date_title"
                    ],
                    "survey_answer_due_date": data["message_param"][
                        "survey_answer_due_date"
                    ],
                },
            )
            notification_user_id_list = list(set(data["notification_id_list"]))
            for user_id in notification_user_id_list:
                notification = NotificationModel(
                    id=str(uuid.uuid4()),
                    user_id=user_id,
                    summary=summary,
                    url=data["message_param"]["fo_survey_detail_url"],
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

    # メール通知（一括処理）
    # 「アンケート催促通知」メール
    # 対象: 通常回答アンケート
    # SQS send_message_batchのEntries
    send_message_entries: List[dict] = []
    for data in notification_data:
        to_addr_list = list(set(data["to_mail_list"]))
        cc_addr_list = list(set(data["cc_mail_list"]))
        bcc_addr_list = list(set(data["bcc_mail_list"]))
        message_body = {
            "template": MailType.SURVEY_REMIND,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "bcc": bcc_addr_list,
            "payload": {
                "customer_name": data["message_param"]["customer_name"],
                "project_name": data["message_param"]["project_name"],
                "summary_month": data["message_param"]["summary_month"],
                "formatted_month": data["message_param"]["formatted_month"],
                "answer_user_name": data["message_param"]["answer_user_name"],
                "survey_type_name": data["message_param"]["survey_type_name"],
                "fo_survey_detail_url": data["message_param"]["fo_survey_detail_url"],
                "fo_satisfaction_survey_url": data["message_param"][
                    "fo_satisfaction_survey_url"
                ],
                "survey_answer_due_date_title": data["message_param"][
                    "survey_answer_due_date_title"
                ],
                "survey_answer_due_date": data["message_param"][
                    "survey_answer_due_date"
                ],
                "is_satisfaction_survey": data["message_param"][
                    "is_satisfaction_survey"
                ],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール通知（一括処理）
    # 「匿名アンケート催促通知」メール
    # 対象: 匿名回答アンケート
    for data in anonymous_notification_data:
        to_addr_list = list(set(data["to_mail_list"]))
        cc_addr_list = list(set(data["cc_mail_list"]))
        bcc_addr_list = list(set(data["bcc_mail_list"]))
        message_body = {
            "template": MailType.SURVEY_ANONYMOUS_REMIND,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "bcc": bcc_addr_list,
            "payload": {
                "customer_name": data["message_param"]["customer_name"],
                "project_name": data["message_param"]["project_name"],
                "summary_month": data["message_param"]["summary_month"],
                "formatted_month": data["message_param"]["formatted_month"],
                "answer_user_name": (
                    data["message_param"]["answer_user_name"]
                    if data["message_param"]["answer_user_name"] != ""
                    else "担当者"
                ),
                "survey_type_name": data["message_param"]["survey_type_name"],
                "fo_survey_detail_url": data["message_param"]["fo_survey_detail_url"],
                "fo_satisfaction_survey_url": data["message_param"][
                    "fo_satisfaction_survey_url"
                ],
                "survey_answer_due_date_title": data["message_param"][
                    "survey_answer_due_date_title"
                ],
                "survey_answer_due_date": data["message_param"][
                    "survey_answer_due_date"
                ],
                "project_identification_id": data["message_param"][
                    "project_identification_id"
                ],
                "is_satisfaction_survey": data["message_param"][
                    "is_satisfaction_survey"
                ],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール通知（一括処理）
    # 「匿名回答アンケートパスワード通知」メール
    # 対象: 匿名回答アンケート
    for data in anonymous_notification_data:
        to_addr_list = list(set(data["to_mail_list"]))
        cc_addr_list = list(set(data["cc_mail_list"]))
        bcc_addr_list = list(set(data["bcc_mail_list"]))
        message_body = {
            "template": MailType.SURVEY_ANONYMOUS_PASSWORD,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "bcc": bcc_addr_list,
            "payload": {
                "customer_name": data["message_param"]["customer_name"],
                "project_name": data["message_param"]["project_name"],
                "summary_month": data["message_param"]["summary_month"],
                "formatted_month": data["message_param"]["formatted_month"],
                "answer_user_name": (
                    data["message_param"]["answer_user_name"]
                    if data["message_param"]["answer_user_name"] != ""
                    else "担当者"
                ),
                "survey_type_name": data["message_param"]["survey_type_name"],
                "survey_password": data["message_param"]["survey_password"],
                "project_identification_id": data["message_param"][
                    "project_identification_id"
                ],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール送信
    send_mail_batch(entries=send_message_entries)

    logger.info(f"通常アンケート 通知処理件数:{len(notification_data)}")
    logger.info(f"匿名アンケート 通知処理件数:{len(anonymous_notification_data)}")


def remind_manhour_procedure(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        工数提出漏れ通知処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    # 月初３営業日の日付を取得
    third_date_of_the_month: date = get_first_bussiness_day_of_the_month(
        operation_start_datetime.year, operation_start_datetime.month, 2
    )
    logger.info(f'月初3営業日目:{third_date_of_the_month.strftime("%Y/%m/%d")}')
    logger.info(f'起動日時:{operation_start_datetime.strftime("%Y/%m/%d %H:%M")}')
    if operation_start_datetime.date() != third_date_of_the_month:
        # 現在日が第3営業日でない場合、処理終了
        logger.info("月初3営業日目でないため、処理を終了します。")
        raise ExitHandler()

    # 支援工数を取得
    # 前月
    one_month_ago_datetime = operation_start_datetime - relativedelta(months=1)
    last_month_datetime = one_month_ago_datetime.strftime("%Y/%m")
    filter_condition = ManHourSupporterModel.is_confirm == False  # NOQA
    man_hour_list: List[ManHourSupporterModel] = list(
        ManHourSupporterModel.year_month_data_type_index.query(
            hash_key=last_month_datetime, filter_condition=filter_condition
        )
    )
    logger.info(f"支援工数取得件数:{len(man_hour_list)}")

    # 一般ユーザ情報の取得
    user_filter_condition = UserModel.disabled == False  # NOQA
    user_list: List[UserModel] = list(
        UserModel.scan(filter_condition=user_filter_condition)
    )
    # 管理ユーザ
    admin_filter_condition = AdminModel.disabled == False  # NOQA
    admin_list: List[AdminModel] = list(
        AdminModel.scan(filter_condition=admin_filter_condition)
    )
    # 通知関連情報
    notification_data: List[dict] = []
    for man_hour in man_hour_list:
        data: dict = {}
        notification_id_list: List[str] = []
        to_mail_list: List[str] = []
        cc_mail_list: List[str] = []
        message_param: dict = {}

        temp_supporter_organization_id = ""
        for user in user_list:
            if man_hour.supporter_user_id == user.id:
                # 支援者
                # お知らせ通知ユーザ
                notification_id_list.append(user.id)
                # メール通知アドレス
                to_mail_list.append(user.email)
                if user.role == UserRoleType.SUPPORTER.key:
                    temp_org_id_list = user.supporter_organization_id
                    if temp_org_id_list:
                        temp_supporter_organization_id = temp_org_id_list[0]

        if len(to_mail_list) == 0:
            # Toが存在しない場合、通知を送らない
            continue

        if temp_supporter_organization_id:
            for user in user_list:
                if user.supporter_organization_id:
                    if (
                        temp_supporter_organization_id in user.supporter_organization_id
                        and user.role == UserRoleType.SUPPORTER_MGR.key
                    ):
                        # 上長の支援者責任者
                        cc_mail_list.append(user.email)

        for admin in admin_list:
            if UserRoleType.MAN_HOUR_OPS.key in admin.roles:
                cc_mail_list.append(admin.email)

        # お知らせ・メールの本文の編集
        message_param["year_month"] = "{year}年{month}月".format(
            year=man_hour.year_month[0:4],
            month=man_hour.year_month[5:],
        )
        # URL
        fo_site_url = get_app_settings().fo_site_url
        fo_man_hour_input_url = fo_site_url + FoAppUrl.MAN_HOUR_INPUT.format(
            year=man_hour.year_month[0:4],
            month=man_hour.year_month[5:],
        )
        message_param["fo_man_hour_input_url"] = fo_man_hour_input_url

        data["notification_id_list"] = notification_id_list
        data["to_mail_list"] = to_mail_list
        data["cc_mail_list"] = cc_mail_list
        data["message_param"] = message_param
        notification_data.append(data)

    # お知らせ通知（一括処理）
    # お知らせ「工数提出漏れ通知」
    notification_notice_at = datetime.now()
    with NotificationModel.batch_write() as notification_batch:
        for data in notification_data:
            summary, message = NotificationService.edit_message(
                notification_type=NotificationType.MISSING_SUBMISSION_OF_MAN_HOURS,
                message_param={
                    "year_month": data["message_param"]["year_month"],
                    "fo_man_hour_input_url": data["message_param"][
                        "fo_man_hour_input_url"
                    ],
                },
            )
            notification_user_id_list = list(set(data["notification_id_list"]))
            for user_id in notification_user_id_list:
                notification = NotificationModel(
                    id=str(uuid.uuid4()),
                    user_id=user_id,
                    summary=summary,
                    url=data["message_param"]["fo_man_hour_input_url"],
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

    # メール通知（一括処理）
    # 「工数提出漏れ通知」メール
    # SQS send_message_batchのEntries
    send_message_entries: List[dict] = []
    for data in notification_data:
        to_addr_list = list(set(data["to_mail_list"]))
        cc_addr_list = list(set(data["cc_mail_list"]))
        message_body = {
            "template": MailType.MISSING_SUBMISSION_OF_MAN_HOURS,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "payload": {
                "year_month": data["message_param"]["year_month"],
                "fo_man_hour_input_url": data["message_param"]["fo_man_hour_input_url"],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール送信
    send_mail_batch(entries=send_message_entries)

    logger.info(f"通知処理件数:{len(notification_data)}")


def remind_master_karte_current_program_procedure(
    event,
    operation_start_datetime: datetime,
    batch_control_id: str,
):
    """
        マスターカルテ当期支援入力催促通知処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """

    # 条件：案件の支援期間終了日が「現在日-7」、当期支援の全項目が空欄（未入力）の場合
    remind_datetime = operation_start_datetime + timedelta(days=-7)
    remind_date = remind_datetime.strftime("%Y/%m/%d")
    # 支援期間終了日が「現在日-7」のPP案件情報の取得
    pp_project_list: List[ProjectModel] = list(
        ProjectModel.data_type_support_date_to_index.query(
            hash_key=DataType.PROJECT,
            range_key_condition=ProjectModel.support_date_to == remind_date,
            filter_condition=ProjectModel.is_master_karte_remind == True,  # NOQA
        )
    )
    logger.info(f"案件情報取得件数:{len(pp_project_list)}")
    if len(pp_project_list) == 0:
        logger.info("対象の案件が存在しないため、処理を終了します。")
        raise ExitHandler()

    pp_project_id_list = []
    salesforce_opportunity_id_list = []
    get_pf_projects_params = {"limit": 50, "offsetPage": 1}

    for pp_project in pp_project_list:
        pp_project_id_list.append(pp_project.id)
        if pp_project.salesforce_opportunity_id:
            # SF商談IDが存在する場合
            salesforce_opportunity_id_list.append(pp_project.salesforce_opportunity_id)
        else:
            # PP案件IDが存在する場合
            pp_project_id_list.append(pp_project.id)

    get_pf_projects_params["salesforceOpportunityId"] = (
        MasterKartenService.make_pf_request_param(salesforce_opportunity_id_list)
    )

    get_pf_projects_params["partnerPortalProjectId"] = (
        MasterKartenService.make_pf_request_param(pp_project_id_list)
    )

    # PF案件一覧取得APIの呼び出し
    total_project_cnt = None
    retrieved_cnt = 0
    pf_project_list = []
    platform_api_operator = PlatformApiOperator(is_batch=True)

    while total_project_cnt is None or len(pf_project_list) < retrieved_cnt:
        status_code, pf_projects = platform_api_operator.get_projects(
            params=get_pf_projects_params
        )
        if status_code != 200:
            logger.error("PF案件一覧取得APIの呼び出しに失敗しました。")
            raise ExitHandler()
        if len(pf_projects) == 0:
            logger.info("対象の案件が存在しないため、処理を終了します。")
            raise ExitHandler()
        pf_project_list.extend(pf_projects["projects"])
        retrieved_cnt = pf_projects["total"]
        get_pf_projects_params["offsetPage"] += 1

        if total_project_cnt is None:
            total_project_cnt = pf_projects["total"]

    # 当期支援の全項目が空欄（未入力）の案件情報の取得
    empty_program_pf_project_list = []

    for pf_project in pf_project_list:
        for program in pf_project["programs"]:
            if program["isCurrent"]:
                # 当期支援項目リスト
                current_program_item_list = [
                    "customerSuccessResult",
                    "customerSuccessResultFactor",
                    "nextSupportContent",
                    "SupportIssue",
                    "supportSuccessFactor",
                    "surveySsapAssessment",
                    "surveyCustomerSelfAssessment",
                ]

                # 当期支援の項目が全て空(未入力)か確認
                is_input = False
                for key, item in program.items():
                    if key in current_program_item_list and (item or item is not None):
                        is_input = True
                        break
                if not is_input:
                    empty_program_pf_project_list.append(pf_project)

    # 一般ユーザー情報の取得
    user_filter_condition = UserModel.disabled == False  # NOQA
    user_list: List[UserModel] = list(
        UserModel.scan(filter_condition=user_filter_condition)
    )

    # 通知関連情報
    notification_data: List[dict] = []
    target_cnt: int = 0

    # 案件単位で処理
    for pp_project in pp_project_list:
        data: dict = {}
        supporter_id_list: List[str] = []
        to_mail_list: List[str] = []
        message_param: dict = {}

        for pf_project in empty_program_pf_project_list:
            if (pp_project.id == pf_project["project"]["partnerPortalProjectId"]) or (
                pp_project.salesforce_opportunity_id
                and pp_project.salesforce_opportunity_id
                == pf_project["project"]["salesforceOpportunityId"]
            ):
                target_cnt += 1
                # 案件名
                message_param["project_name"] = pp_project.name
                # マスターカルテ詳細ページURL
                fo_site_url = (
                    get_app_settings().fo_site_url
                    + FoAppUrl.MASTER_KARTE_DETAIL.format(
                        npf_project_id=pf_project["project"]["id"]
                    )
                )
                message_param["fo_master_karte_detail_url"] = fo_site_url

                if pp_project.main_supporter_user_id:
                    for user in user_list:
                        if user.id == pp_project.main_supporter_user_id:
                            # お知らせ通知ユーザ
                            supporter_id_list.append(user.id)
                            # メール通知アドレス
                            to_mail_list.append(user.email)
                            break
                if pp_project.supporter_user_ids:
                    for supporter_user_id in pp_project.supporter_user_ids:
                        for user in user_list:
                            if user.id == supporter_user_id:
                                # お知らせ通知ユーザ
                                supporter_id_list.append(user.id)
                                # メール通知アドレス
                                to_mail_list.append(user.email)
                                break

                data["fo_master_karte_detail_url"] = fo_site_url
                data["project_name"] = pp_project.name
                data["supporter_id_list"] = supporter_id_list
                data["to_mail_list"] = to_mail_list
                data["message_param"] = message_param
                notification_data.append(data)

    logger.info(f"マスターカルテ処理件数:{target_cnt}")

    # お知らせ通知（一括処理）
    # お知らせ「マスターカルテ当期支援入力催促通知」
    notification_notice_at = datetime.now()
    with NotificationModel.batch_write() as notification_batch:
        for data in notification_data:
            summary, message = NotificationService.edit_message(
                notification_type=NotificationType.FORGOT_TO_WRITE_MASTER_KARTEN_CURRENT_PROGRAM_REMIND,
                message_param={
                    "project_name": data["project_name"],
                },
            )
            notification_user_id_list = list(set(data["supporter_id_list"]))
            for user_id in notification_user_id_list:
                notification = NotificationModel(
                    id=str(uuid.uuid4()),
                    user_id=user_id,
                    summary=summary,
                    url=data["fo_master_karte_detail_url"],
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

    # メール通知（一括処理）
    # 「マスターカルテ当期支援入力催促通知」メール
    # SQS send_message_batchのEntries
    send_message_entries: List[dict] = []
    for data in notification_data:
        to_adddr_list = list(set(data["to_mail_list"]))
        message_body = {
            "template": MailType.FORGOT_TO_WRITE_MASTER_KARTEN_CURRENT_PROGRAM_REMIND,
            "to": to_adddr_list,
            "cc": [],
            "payload": {
                "project_name": data["project_name"],
                "fo_master_karte_detail_url": data["fo_master_karte_detail_url"],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール送信
    send_mail_batch(entries=send_message_entries)

    logger.info(f"マスターカルテ処理対象件数:{target_cnt}")
    logger.info(f"通知処理件数:{len(notification_data)}")


def remind_master_karte_next_program_procedure(
    event,
    operation_start_datetime: datetime,
    batch_control_id: str,
):
    """
        マスターカルテ次期支援入力催促通知処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """

    # 条件：案件の支援期間終了日が「現在日+7」、次期支援の全項目が空欄（未入力）の場合
    remind_datetime = operation_start_datetime + timedelta(days=+7)
    remind_date = remind_datetime.strftime("%Y/%m/%d")
    # 支援期間終了日が「現在日+7」のPP案件情報の取得
    pp_project_list: List[ProjectModel] = list(
        ProjectModel.data_type_support_date_to_index.query(
            hash_key=DataType.PROJECT,
            range_key_condition=ProjectModel.support_date_to == remind_date,
            filter_condition=ProjectModel.is_master_karte_remind == True,  # NOQA
        )
    )
    logger.info(f"案件情報取得件数:{len(pp_project_list)}")
    if len(pp_project_list) == 0:
        logger.info("対象の案件が存在しないため、処理を終了します。")
        raise ExitHandler()

    pp_project_id_list = []
    salesforce_opportunity_id_list = []
    get_pf_projects_params = {"limit": 50, "offsetPage": 1}

    for pp_project in pp_project_list:
        pp_project_id_list.append(pp_project.id)
        if pp_project.salesforce_opportunity_id:
            # SF商談IDが存在する場合
            salesforce_opportunity_id_list.append(pp_project.salesforce_opportunity_id)
        else:
            # PP案件IDが存在する場合
            pp_project_id_list.append(pp_project.id)

    get_pf_projects_params["salesforceOpportunityId"] = (
        MasterKartenService.make_pf_request_param(salesforce_opportunity_id_list)
    )

    get_pf_projects_params["partnerPortalProjectId"] = (
        MasterKartenService.make_pf_request_param(pp_project_id_list)
    )

    # PF案件一覧取得APIの呼び出し
    total_project_cnt = None
    retrieved_cnt = 0
    pf_project_list = []
    platform_api_operator = PlatformApiOperator(is_batch=True)

    while total_project_cnt is None or len(pf_project_list) < retrieved_cnt:
        status_code, pf_projects = platform_api_operator.get_projects(
            params=get_pf_projects_params
        )
        if status_code != 200:
            logger.error("PF案件一覧取得APIの呼び出しに失敗しました。")
            raise ExitHandler()
        if len(pf_projects) == 0:
            logger.info("対象の案件が存在しないため、処理を終了します。")
            raise ExitHandler()
        pf_project_list.extend(pf_projects["projects"])
        retrieved_cnt = pf_projects["total"]
        get_pf_projects_params["offsetPage"] += 1

        if total_project_cnt is None:
            total_project_cnt = pf_projects["total"]

    # 次期支援の全項目が空欄（未入力）の案件情報の取得
    empty_program_pf_project_list = []

    for pf_project in pf_project_list:
        next_program = [
            program
            for program in pf_project["programs"]
            if program["isCurrent"] is False
        ]
        # 次期支援レコードが作成されていない場合、未入力とみなす
        if len(next_program) == 0:
            empty_program_pf_project_list.append(pf_project)

    # 一般ユーザー情報の取得
    user_filter_condition = UserModel.disabled == False  # NOQA
    user_list: List[UserModel] = list(
        UserModel.scan(filter_condition=user_filter_condition)
    )

    # 通知関連情報
    notification_data: List[dict] = []
    target_cnt: int = 0

    # 案件単位で処理
    for pp_project in pp_project_list:
        data: dict = {}
        notification_user_id_list: List[str] = []
        to_mail_list: List[str] = []
        message_param: dict = {}

        for pf_project in empty_program_pf_project_list:
            if (pp_project.id == pf_project["project"]["partnerPortalProjectId"]) or (
                pp_project.salesforce_opportunity_id
                and pp_project.salesforce_opportunity_id
                == pf_project["project"]["salesforceOpportunityId"]
            ):
                target_cnt += 1
                # 案件名
                message_param["project_name"] = pp_project.name
                # マスターカルテ詳細ページURL
                fo_site_url = (
                    get_app_settings().fo_site_url
                    + FoAppUrl.MASTER_KARTE_DETAIL.format(
                        npf_project_id=pf_project["project"]["id"]
                    )
                )
                message_param["fo_master_karte_detail_url"] = fo_site_url

                if pp_project.main_supporter_user_id:
                    for user in user_list:
                        if user.id == pp_project.main_supporter_user_id:
                            # お知らせ通知ユーザ
                            notification_user_id_list.append(user.id)
                            # メール通知アドレス
                            to_mail_list.append(user.email)
                            break
                if pp_project.supporter_user_ids:
                    for supporter_user_id in pp_project.supporter_user_ids:
                        for user in user_list:
                            if user.id == supporter_user_id:
                                # お知らせ通知ユーザ
                                notification_user_id_list.append(user.id)
                                # メール通知アドレス
                                to_mail_list.append(user.email)
                                break
                if pp_project.main_sales_user_id:
                    for user in user_list:
                        if user.id == pp_project.main_sales_user_id:
                            # お知らせ通知ユーザ
                            notification_user_id_list.append(user.id)
                            # メール通知アドレス
                            to_mail_list.append(user.email)
                            break

                data["fo_master_karte_detail_url"] = fo_site_url
                data["project_name"] = pp_project.name
                data["notification_user_id_list"] = notification_user_id_list
                data["to_mail_list"] = to_mail_list
                data["message_param"] = message_param
                notification_data.append(data)

    logger.info(f"マスターカルテ処理件数:{target_cnt}")

    # お知らせ通知（一括処理）
    # お知らせ「マスターカルテ次期支援入力催促通知」
    notification_notice_at = datetime.now()
    with NotificationModel.batch_write() as notification_batch:
        for data in notification_data:
            summary, message = NotificationService.edit_message(
                notification_type=NotificationType.FORGOT_TO_WRITE_MASTER_KARTEN_NEXT_PROGRAM_REMIND,
                message_param={
                    "project_name": data["project_name"],
                    "fo_master_karte_detail_url": data["fo_master_karte_detail_url"],
                },
            )
            notification_user_id_list = list(set(data["notification_user_id_list"]))
            for user_id in notification_user_id_list:
                notification = NotificationModel(
                    id=str(uuid.uuid4()),
                    user_id=user_id,
                    summary=summary,
                    url=data["fo_master_karte_detail_url"],
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

    # メール通知（一括処理）
    # 「マスターカルテ次期支援入力催促通知」メール
    # SQS send_message_batchのEntries
    send_message_entries: List[dict] = []
    for data in notification_data:
        to_adddr_list = list(set(data["to_mail_list"]))
        message_body = {
            "template": MailType.FORGOT_TO_WRITE_MASTER_KARTEN_NEXT_PROGRAM_REMIND,
            "to": to_adddr_list,
            "cc": [],
            "payload": {
                "project_name": data["project_name"],
                "fo_master_karte_detail_url": data["fo_master_karte_detail_url"],
            },
        }
        sqs_message_body = json.dumps(message_body)
        send_message_entries.append(
            {"Id": str(uuid.uuid4()), "MessageBody": sqs_message_body}
        )

    # メール送信
    send_mail_batch(entries=send_message_entries)

    logger.info(f"マスターカルテ処理対象件数:{target_cnt}")
    logger.info(f"通知処理件数:{len(notification_data)}")


def main_procedure(event, operation_start_datetime: datetime, batch_control_id: str):
    """
        主処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    if event["mode"] == BatchInputParameterMode.REMIND_KARTE:
        # カルテ書き忘れリマインド通知
        logger.info("remind_karte_procedure start.")
        remind_karte_procedure(event, operation_start_datetime, batch_control_id)
        logger.info("remind_karte_procedure end.")

    elif event["mode"] == BatchInputParameterMode.SCHEDULE_SURVEY:
        # アンケート回答依頼通知
        # Partner Portalアンケート以外（アンケート回答依頼通知）
        logger.info("schedule_survey_other_than_pp_procedure start.")
        schedule_survey_other_than_pp_procedure(
            event, operation_start_datetime, batch_control_id
        )
        logger.info("schedule_survey_other_than_pp_procedure end.")

        # Partner Portalアンケート（Partner Portal利用アンケート回答依頼通知）
        logger.info("schedule_survey_pp_procedure start.")
        schedule_survey_pp_procedure(event, operation_start_datetime, batch_control_id)
        logger.info("schedule_survey_pp_procedure end.")

    elif event["mode"] == BatchInputParameterMode.REMIND_SURVEY:
        # アンケート催促通知
        logger.info("remind_survey_procedure start.")
        remind_survey_procedure(event, operation_start_datetime, batch_control_id)
        logger.info("remind_survey_procedure end.")

    elif event["mode"] == BatchInputParameterMode.REMIND_MANHOUR:
        # 工数提出漏れ通知
        logger.info("remind_manhour_procedure start.")
        remind_manhour_procedure(event, operation_start_datetime, batch_control_id)
        logger.info("remind_manhour_procedure end.")

    elif event["mode"] == BatchInputParameterMode.REMIND_CURRENT_PROGRAM:
        # マスターカルテ当期支援入力催促通知
        logger.info("remind_master_karte_current_program_procedure start.")
        remind_master_karte_current_program_procedure(
            event, operation_start_datetime, batch_control_id
        )
        logger.info("remind_master_karte_current_program_procedure end.")

    elif event["mode"] == BatchInputParameterMode.REMIND_NEXT_PROGRAM:
        # マスターカルテ次期支援入力催促通知
        logger.info("remind_master_karte_next_program_procedure start.")
        remind_master_karte_next_program_procedure(
            event, operation_start_datetime, batch_control_id
        )
        logger.info("remind_master_karte_next_program_procedure end.")


def handler(event, context):
    """
    バッチ処理 : FO各種リマインド通知処理

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
    batch_control_id = (
        BatchFunctionId.REMIND_BATCH.format(landscape=event["stageParams"]["stage"])
        + "#"
        + event["mode"]
    )
    try:
        logger.info(f"Process start: mode: {event.get('mode')}")
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
        logger.info(f"Process normal end: mode: {event['mode']}")
        return "Normal end."

    except ExitHandler:
        # 処理の途中で処理終了する場合
        process_time = round(time.time() - start_time, 4)
        logger.info("process_time: {0}[sec]".format(str(process_time)))
        logger.info(f"Skipped Processing: mode: {event['mode']}")
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
                    "error_function": BatchFunctionName.REMIND_BATCH,
                    "error_message": traceback.format_exc(),
                },
            )
        end_procedure(event, BatchStatus.ERROR, batch_control_id)
        process_time = round(time.time() - start_time, 4)
        logger.info("process_time: {0}[sec]".format(str(process_time)))
        logger.exception("Error end.")
        return "Error end."


if __name__ == "__main__":
    param = {"mode": sys.argv[1], "stageParams": {"stage": sys.argv[2]}}
    handler(param, {})
