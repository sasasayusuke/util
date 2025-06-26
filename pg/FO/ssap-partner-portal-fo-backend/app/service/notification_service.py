import uuid
from datetime import datetime, timedelta
from typing import List, Tuple

from app.models.notification import NotificationModel
from app.models.user import UserModel
from app.resources.const import NotificationType
from app.schemas.notification import (
    GetNotificationsByMineResponse,
    PatchCheckNotificationsResponse,
)


class NotificationService:

    # お知らせ：案件アサイン通知
    ASSIGN_NOTIFICATION_SUMMARY = "{project_name} へ招待されました"
    ASSIGN_NOTIFICATION_MESSAGE = "以下の案件へ招待されました。\n\n・案件名：{project_name}"
    # お知らせ：アンケートお客様回答通知
    SURVEY_ANSWER_PROVIDED_SUMMARY = "{customer_name_project_name} のアンケートに回答がありました"
    SURVEY_ANSWER_PROVIDED_MESSAGE = (
        "{customer_name_project_name} の {survey_month} のアンケートに回答がありました。"
    )
    FORGOT_TO_WRITE_KARTEN_REMIND_SUMMARY = "{project_name} ：個別カルテが作成もしくは送信されていません"
    FORGOT_TO_WRITE_KARTEN_REMIND_MESSAGE = (
        "{project_name}の{support_datetime}の個別カルテ（支援議事録）が作成もしくは送信されていません。"
    )
    FORGOT_TO_WRITE_MASTER_KARTEN_CURRENT_PROGRAM_REMIND_SUMMARY = (
        "【Partner Portal】{project_name}：案件結果・実績を入力しましょう。"
    )
    FORGOT_TO_WRITE_MASTER_KARTEN_CURRENT_PROGRAM_REMIND_MESSAGE = (
        "本支援プログラムが完了致しましたので、案件結果・実績につき入力をお願い致します。"
    )
    FORGOT_TO_WRITE_MASTER_KARTEN_NEXT_PROGRAM_REMIND_SUMMARY = (
        "【Partner Portal】{project_name}：マスターカルテを活用しましょう。"
    )
    FORGOT_TO_WRITE_MASTER_KARTEN_NEXT_PROGRAM_REMIND_MESSAGE = (
        "本支援プログラム完了後のお客様の課題・アクションの整理や、継続案件の要件整理の為、{project_name}のマスターカルテをご活用下さい。"
    )
    SURVEY_ANSWER_REQUEST_SUMMARY = (
        "{customer_name_project_name}のアンケートへの回答をお願いします{survey_answer_due_date_title}"
    )
    SURVEY_ANSWER_REQUEST_MESSAGE = "{customer_name_project_name}の{summary_month}のアンケート回答をお願い申し上げます。\n{survey_answer_due_date}"
    PP_SURVEY_ANSWER_REQUEST_SUMMARY = "アンケートへの回答をお願いします（回答期限：{pp_answer_due_date_title}迄）"
    PP_SURVEY_ANSWER_REQUEST_SUMMARY_NO_DEADLINE = "アンケートへの回答をお願いします"
    PP_SURVEY_ANSWER_REQUEST_MESSAGE = "{summary_month}のアンケート回答をお願い申し上げます。\n回答期限：{pp_answer_due_date}"
    PP_SURVEY_ANSWER_REQUEST_MESSAGE_NO_DEADLINE = "{summary_month}のアンケート回答をお願い申し上げます。"
    SURVEY_REMIND_SUMMARY = (
        "{customer_name_project_name}のアンケートへの回答をお願いします{survey_answer_due_date_title}"
    )
    SURVEY_REMIND_MESSAGE = "{customer_name_project_name}の{summary_month}のアンケート回答をお願い申し上げます。\n{survey_answer_due_date}"
    MISSING_SUBMISSION_OF_MAN_HOURS_SUMMARY = "{year_month}の工数が提出されていません"
    MISSING_SUBMISSION_OF_MAN_HOURS_MESSAGE = "{year_month}の工数提出をお願いします。"
    UPDATE_KARTE_SUMMARY = "{project_name} ：個別カルテが更新されました"
    UPDATE_KARTE_MESSAGE = "{project_name} の {date} の個別カルテが更新されました"
    SURVEY_ANONYMOUS_ANSWER_REQUEST_SUMMARY = (
        "{customer_name_project_name}のアンケートへの回答をお願いします"
    )
    SURVEY_ANONYMOUS_ANSWER_REQUEST_MESSAGE = (
        "{customer_name_project_name}の{summary_month}のアンケート回答をお願い申し上げます。"
    )
    SURVEY_ANONYMOUS_REMIND_SUMMARY = "{customer_name_project_name}のアンケートへの回答をお願いします"
    SURVEY_ANONYMOUS_REMIND_MESSAGE = (
        "{customer_name_project_name}の{summary_month}のアンケート回答をお願い申し上げます。"
    )

    @staticmethod
    def get_unix_time_after_90_days(dt: datetime) -> float:
        """90日後のUNIX TIMEを取得

        Args:
            dt (datetime): 基準日時

        Returns:
            float: 基準日時の90日後のUNIX TIME
        """
        dt_after_90_days = dt + timedelta(days=90)
        return dt_after_90_days.timestamp()

    @staticmethod
    def edit_message(
        notification_type: NotificationType, message_param: dict
    ) -> Tuple[str, str]:
        """お知らせのお知らせタイトル、お知らせ文言を編集.

        Args:
            notification_type (NotificationType): 通知種別(ENUM)
            message_param (dict): メッセージ内の置換文字の辞書(変数名,値)
                e.g. {"project_name" : "サンプルプロジェクト"}
        Return:
            tuple: 以下の情報のTuple
                summary (str): お知らせタイトル
                message (str): お知らせ文言
        """
        # 通知種別ごとのお知らせタイトル/文言の編集
        summary = ""
        message = ""
        if notification_type == NotificationType.PROJECT_ASSIGN:
            summary = NotificationService.ASSIGN_NOTIFICATION_SUMMARY.format(
                project_name=message_param.get("project_name")
            )
            message = NotificationService.ASSIGN_NOTIFICATION_MESSAGE.format(
                project_name=message_param.get("project_name")
            )
        if notification_type == NotificationType.SURVEY_ANSWER_PROVIDED:
            summary = NotificationService.SURVEY_ANSWER_PROVIDED_SUMMARY.format(
                customer_name_project_name=message_param.get(
                    "customer_name_project_name"
                )
            )
            message = NotificationService.SURVEY_ANSWER_PROVIDED_MESSAGE.format(
                customer_name_project_name=message_param.get(
                    "customer_name_project_name"
                ),
                survey_month=message_param.get("survey_month"),
            )
        if notification_type == NotificationType.FORGOT_TO_WRITE_KARTEN_REMIND:
            summary = NotificationService.FORGOT_TO_WRITE_KARTEN_REMIND_SUMMARY.format(
                project_name=message_param.get("project_name")
            )
            message = NotificationService.FORGOT_TO_WRITE_KARTEN_REMIND_MESSAGE.format(
                project_name=message_param.get("project_name"),
                support_datetime=message_param.get("support_datetime"),
            )
        if notification_type == NotificationType.SURVEY_ANSWER_REQUEST:
            summary = NotificationService.SURVEY_ANSWER_REQUEST_SUMMARY.format(
                customer_name_project_name=message_param.get(
                    "customer_name_project_name"
                ),
                survey_answer_due_date_title=message_param.get(
                    "survey_answer_due_date_title"
                ),
            )
            survey_answer_due_date = ""
            if message_param.get("survey_answer_due_date"):
                survey_answer_due_date = "回答期限：" + message_param.get(
                    "survey_answer_due_date"
                )

            message = NotificationService.SURVEY_ANSWER_REQUEST_MESSAGE.format(
                customer_name_project_name=message_param.get(
                    "customer_name_project_name"
                ),
                summary_month=message_param.get("summary_month"),
                survey_answer_due_date=survey_answer_due_date,
            )
        if notification_type == NotificationType.PP_SURVEY_ANSWER_REQUEST:
            if message_param.get("pp_answer_due_date_title") and message_param.get("pp_answer_due_date"):
                summary = NotificationService.PP_SURVEY_ANSWER_REQUEST_SUMMARY.format(
                    pp_answer_due_date_title=message_param.get("pp_answer_due_date_title"),
                )
                message = NotificationService.PP_SURVEY_ANSWER_REQUEST_MESSAGE.format(
                    summary_month=message_param.get("summary_month"),
                    pp_answer_due_date=message_param.get("pp_answer_due_date"),
                )
            else:
                summary = NotificationService.PP_SURVEY_ANSWER_REQUEST_SUMMARY_NO_DEADLINE
                message = NotificationService.PP_SURVEY_ANSWER_REQUEST_MESSAGE_NO_DEADLINE.format(
                    summary_month=message_param.get("summary_month"),
                )
        if notification_type == NotificationType.SURVEY_REMIND:
            summary = NotificationService.SURVEY_REMIND_SUMMARY.format(
                customer_name_project_name=message_param.get(
                    "customer_name_project_name"
                ),
                survey_answer_due_date_title=message_param.get(
                    "survey_answer_due_date_title"
                ),
            )
            survey_answer_due_date = ""
            if message_param.get("survey_answer_due_date"):
                survey_answer_due_date = "回答期限：" + message_param.get(
                    "survey_answer_due_date"
                )
            message = NotificationService.SURVEY_REMIND_MESSAGE.format(
                customer_name_project_name=message_param.get(
                    "customer_name_project_name"
                ),
                summary_month=message_param.get("summary_month"),
                survey_answer_due_date=survey_answer_due_date,
            )
        if notification_type == NotificationType.MISSING_SUBMISSION_OF_MAN_HOURS:
            summary = (
                NotificationService.MISSING_SUBMISSION_OF_MAN_HOURS_SUMMARY.format(
                    year_month=message_param.get("year_month")
                )
            )
            message = (
                NotificationService.MISSING_SUBMISSION_OF_MAN_HOURS_MESSAGE.format(
                    year_month=message_param.get("year_month"),
                )
            )
        if notification_type == NotificationType.UPDATE_KARTE:
            summary = NotificationService.UPDATE_KARTE_SUMMARY.format(
                project_name=message_param.get("project_name"),
            )
            message = NotificationService.UPDATE_KARTE_MESSAGE.format(
                project_name=message_param.get("project_name"),
                date=message_param.get("date"),
            )
        if notification_type == NotificationType.SURVEY_ANONYMOUS_ANSWER_REQUEST:
            summary = (
                NotificationService.SURVEY_ANONYMOUS_ANSWER_REQUEST_SUMMARY.format(
                    customer_name_project_name=message_param.get(
                        "customer_name_project_name"
                    )
                )
            )
            message = (
                NotificationService.SURVEY_ANONYMOUS_ANSWER_REQUEST_MESSAGE.format(
                    customer_name_project_name=message_param.get(
                        "customer_name_project_name"
                    ),
                    summary_month=message_param.get("summary_month"),
                )
            )
        if notification_type == NotificationType.SURVEY_ANONYMOUS_REMIND:
            summary = NotificationService.SURVEY_ANONYMOUS_REMIND_SUMMARY.format(
                customer_name_project_name=message_param.get(
                    "customer_name_project_name"
                )
            )
            message = NotificationService.SURVEY_ANONYMOUS_REMIND_MESSAGE.format(
                customer_name_project_name=message_param.get(
                    "customer_name_project_name"
                ),
                summary_month=message_param.get("summary_month"),
            )
        if notification_type == NotificationType.FORGOT_TO_WRITE_MASTER_KARTEN_CURRENT_PROGRAM_REMIND:
            summary = NotificationService.FORGOT_TO_WRITE_MASTER_KARTEN_CURRENT_PROGRAM_REMIND_SUMMARY.format(
                project_name=message_param.get("project_name")
            )
            message = NotificationService.FORGOT_TO_WRITE_MASTER_KARTEN_CURRENT_PROGRAM_REMIND_MESSAGE.format(
                project_name=message_param.get("project_name"),
                fo_master_karte_detail_url=message_param.get("fo_master_karte_detail_url"),
            )
        if notification_type == NotificationType.FORGOT_TO_WRITE_MASTER_KARTEN_NEXT_PROGRAM_REMIND:
            summary = NotificationService.FORGOT_TO_WRITE_MASTER_KARTEN_NEXT_PROGRAM_REMIND_SUMMARY.format(
                project_name=message_param.get("project_name")
            )
            message = NotificationService.FORGOT_TO_WRITE_MASTER_KARTEN_NEXT_PROGRAM_REMIND_MESSAGE.format(
                project_name=message_param.get("project_name"),
            )
        return summary, message

    @staticmethod
    def save_notification(
        notification_type: NotificationType,
        user_id_list: List[str],
        message_param: dict,
        url: str,
        noticed_at: datetime,
        create_id: str,
        update_id: str,
    ) -> None:
        """お知らせを登録.
            お知らせ日時(notice_at)はYYYY/MM/DD形式で登録.
            ttlはお知らせ日時の90日後のUNIX TIMEを登録.

        Args:
            notification_type (NotificationType): 通知種別(ENUM)
            user_id_list (List[str]): お知らせ対象ユーザIDのリスト
            message_param (dict): メッセージ内の置換文字の辞書(変数名,値)
                e.g. {"project_name" : "サンプルプロジェクト"}
            url (str): お知らせリンク
            noticed_at (datetime): お知らせ日時
            create_id (str): 作成ID
            update_id (str): 更新ID
        """
        # 通知種別ごとのお知らせタイトル/文言の編集
        summary, message = NotificationService.edit_message(
            notification_type=notification_type, message_param=message_param
        )

        create_datetime = datetime.now()
        with NotificationModel.batch_write() as notification_batch:
            for user_id in user_id_list:
                notification = NotificationModel(
                    id=str(uuid.uuid4()),
                    user_id=user_id,
                    summary=summary,
                    url=url,
                    message=message,
                    confirmed=False,
                    noticed_at=noticed_at,
                    ttl=NotificationService.get_unix_time_after_90_days(noticed_at),
                    create_id=create_id,
                    create_at=create_datetime,
                    update_id=update_id,
                    update_at=create_datetime,
                )
                notification_batch.save(notification)

    @staticmethod
    def get_notification_by_me(
        current_user: UserModel,
    ) -> GetNotificationsByMineResponse:
        """一般ユーザーIDに紐づくお知らせ情報を取得します。

        Args:
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetNotificationsByMineResponse: 取得結果
        """
        notification_list: List[GetNotificationsByMineResponse] = []

        notification_itr = NotificationModel.user_id_noticed_at_index.query(
            hash_key=current_user.id, scan_index_forward=False
        )
        for notification in notification_itr:
            notification_list.append(
                GetNotificationsByMineResponse(**notification.attribute_values)
            )

        return notification_list

    @staticmethod
    def patch_check_notifications(
        current_user: UserModel,
    ) -> PatchCheckNotificationsResponse:
        """お知らせ情報を既読に更新します。

        Args:
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            PatchCheckNotificationsResponse: 更新結果
        """
        notification_itr = NotificationModel.user_id_noticed_at_index.query(
            hash_key=current_user.id
        )

        for notification in notification_itr:
            # confirm フラグを既読のTrueへ更新
            notification.update(
                actions=[
                    NotificationModel.update_id.set(current_user.id),
                    NotificationModel.update_at.set(datetime.now()),
                    NotificationModel.confirmed.set(True),
                ]
            )

        return PatchCheckNotificationsResponse(message="OK")
