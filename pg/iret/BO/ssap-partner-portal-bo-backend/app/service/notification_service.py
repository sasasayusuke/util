import uuid
from datetime import datetime, timedelta
from typing import List, Tuple

from app.models.admin import AdminModel
from app.models.notification import NotificationModel
from app.resources.const import NotificationType
from app.schemas.notification import (
    GetNotificationsByMineResponse,
    PatchCheckNotificationsResponse,
)


class NotificationService:

    # お知らせ：案件情報新規登録通知
    REGISTRATION_NOTIFICATION_SUMMARY = "案件情報が新規登録されました"
    REGISTRATION_NOTIFICATION_MESSAGE = (
        "以下の案件情報が新規登録されました。\n\n「新規登録」\n・案件名：{project_name}"
    )
    # お知らせ：案件アサイン通知
    ASSIGN_NOTIFICATION_SUMMARY = "{project_name} へ招待されました"
    ASSIGN_NOTIFICATION_MESSAGE = "以下の案件へ招待されました。\n\n・案件名：{project_name}"
    # お知らせ：Salesforceお客様情報インポート通知
    SALESFORCE_CUSTOMER_IMPORT_SUMMARY = "お客様情報がインポートされました"
    SALESFORCE_CUSTOMER_IMPORT_MESSAGE_TOP = "以下のお客様情報がインポートされました。\n\n"
    SALESFORCE_CUSTOMER_IMPORT_MESSAGE_ADD_1 = "「新規登録」\n"
    SALESFORCE_CUSTOMER_IMPORT_MESSAGE_ADD_2 = "・お客様名：{customer_name}\n"
    SALESFORCE_CUSTOMER_IMPORT_MESSAGE_UPDATE_1 = "「更新」\n"
    SALESFORCE_CUSTOMER_IMPORT_MESSAGE_UPDATE_2 = "・お客様名：{customer_name}\n"
    SALESFORCE_CUSTOMER_IMPORT_MESSAGE_UPDATE_3 = "　・{update_column}：{update_value}\n"
    # お知らせ：Salesforce案件情報インポート通知
    SALESFORCE_PROJECT_IMPORT_SUMMARY = "案件情報がインポートされました"
    SALESFORCE_PROJECT_IMPORT_MESSAGE_TOP = "以下の案件情報がインポートされました。\n\n"
    SALESFORCE_PROJECT_IMPORT_MESSAGE_ADD_1 = "「新規登録」\n"
    SALESFORCE_PROJECT_IMPORT_MESSAGE_ADD_2 = "・案件名：{project_name}\n"
    SALESFORCE_PROJECT_IMPORT_MESSAGE_UPDATE_1 = "「更新」\n"
    SALESFORCE_PROJECT_IMPORT_MESSAGE_UPDATE_2 = "・案件名：{project_name}\n"
    SALESFORCE_PROJECT_IMPORT_MESSAGE_UPDATE_3 = "　・Partner Portal項目名\n"
    SALESFORCE_PROJECT_IMPORT_MESSAGE_UPDATE_4 = "　　{update_column}：{update_value}\n"
    # お知らせ：アンケート回答依頼通知
    SURVEY_ANSWER_REQUEST_SUMMARY = (
        "{customer_name_project_name}のアンケートへの回答をお願いします{survey_answer_due_date_title}"
    )
    SURVEY_ANSWER_REQUEST_MESSAGE = "{customer_name_project_name}の{summary_month}のアンケート回答をお願い申し上げます。\n{survey_answer_due_date}"
    # お知らせ：匿名アンケート回答依頼通知
    SURVEY_ANONYMOUS_ANSWER_REQUEST_SUMMARY = (
        "{customer_name_project_name}のアンケートへの回答をお願いします"
    )
    SURVEY_ANONYMOUS_ANSWER_REQUEST_MESSAGE = (
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
        if notification_type == NotificationType.PROJECT_REGISTRATION:
            summary = NotificationService.REGISTRATION_NOTIFICATION_SUMMARY
            message = NotificationService.REGISTRATION_NOTIFICATION_MESSAGE.format(
                project_name=message_param.get("project_name")
            )
        elif notification_type == NotificationType.PROJECT_ASSIGN:
            summary = NotificationService.ASSIGN_NOTIFICATION_SUMMARY.format(
                project_name=message_param.get("project_name")
            )
            message = NotificationService.ASSIGN_NOTIFICATION_MESSAGE.format(
                project_name=message_param.get("project_name")
            )
        elif notification_type == NotificationType.SALESFORCE_CUSTOMER_IMPORT:
            summary = NotificationService.SALESFORCE_CUSTOMER_IMPORT_SUMMARY
            message = NotificationService.SALESFORCE_CUSTOMER_IMPORT_MESSAGE_TOP
            add_info_list = message_param.get("add_info_list")
            update_info_list = message_param.get("update_info_list")
            if add_info_list:
                message += NotificationService.SALESFORCE_CUSTOMER_IMPORT_MESSAGE_ADD_1
                for add_info in add_info_list:
                    message += NotificationService.SALESFORCE_CUSTOMER_IMPORT_MESSAGE_ADD_2.format(
                        customer_name=add_info.get("customer_name")
                    )
            if update_info_list:
                message += (
                    NotificationService.SALESFORCE_CUSTOMER_IMPORT_MESSAGE_UPDATE_1
                )
                for update_info in update_info_list:
                    message += NotificationService.SALESFORCE_CUSTOMER_IMPORT_MESSAGE_UPDATE_2.format(
                        customer_name=update_info.get("customer_name")
                    )
                    for column_info in update_info.get("update_column_info"):
                        message += NotificationService.SALESFORCE_CUSTOMER_IMPORT_MESSAGE_UPDATE_3.format(
                            update_column=column_info.get("update_column"),
                            update_value=column_info.get("update_value"),
                        )
        elif notification_type == NotificationType.SALESFORCE_PROJECT_IMPORT:
            summary = NotificationService.SALESFORCE_PROJECT_IMPORT_SUMMARY
            message = NotificationService.SALESFORCE_PROJECT_IMPORT_MESSAGE_TOP
            add_info_list = message_param.get("add_info_list")
            update_info_list = message_param.get("update_info_list")
            if add_info_list:
                message += NotificationService.SALESFORCE_PROJECT_IMPORT_MESSAGE_ADD_1
                for add_info in add_info_list:
                    message += NotificationService.SALESFORCE_PROJECT_IMPORT_MESSAGE_ADD_2.format(
                        project_name=add_info.get("project_name")
                    )
            if update_info_list:
                message += (
                    NotificationService.SALESFORCE_PROJECT_IMPORT_MESSAGE_UPDATE_1
                )
                for update_info in update_info_list:
                    message += NotificationService.SALESFORCE_PROJECT_IMPORT_MESSAGE_UPDATE_2.format(
                        project_name=update_info.get("project_name")
                    )
                    message += (
                        NotificationService.SALESFORCE_PROJECT_IMPORT_MESSAGE_UPDATE_3
                    )
                    for column_info in update_info.get("update_column_info"):
                        message += NotificationService.SALESFORCE_PROJECT_IMPORT_MESSAGE_UPDATE_4.format(
                            update_column=column_info.get("update_column"),
                            update_value=column_info.get("update_value"),
                        )
        elif notification_type == NotificationType.SURVEY_ANSWER_REQUEST:
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
        elif notification_type == NotificationType.SURVEY_ANONYMOUS_ANSWER_REQUEST:
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
        current_user: AdminModel,
    ) -> GetNotificationsByMineResponse:
        """自身の管理ユーザーIDに紐づくお知らせ情報を取得します。

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
        current_user: AdminModel,
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
