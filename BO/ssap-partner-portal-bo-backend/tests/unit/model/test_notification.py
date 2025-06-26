from datetime import datetime, timedelta

from moto import mock_dynamodb

from app.models.notification import NotificationModel


@mock_dynamodb
def test_model_notification():
    """お知らせTBL NotificationModelクラス テスト定義"""

    NotificationModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    id = "notification1"

    notification = NotificationModel(id)
    notification.user_id = "user1"
    notification.summary = "title1"
    notification.confirmed = False
    notification.noticed_at = datetime.now()
    dt = datetime.now() + timedelta(days=90)
    notification.ttl = dt.timestamp()
    notification.create_at = datetime.now()
    notification.update_at = datetime.now()

    notification.save()

    notification_item = NotificationModel.get(id)
    notification_item_by_query = NotificationModel.user_id_noticed_at_index.query(
        notification.user_id, notification.noticed_at
    )

    assert str(notification) == str(notification_item)
    assert notification_item_by_query is not None

    NotificationModel.delete_table()
