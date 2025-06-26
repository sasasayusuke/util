from datetime import datetime

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute
from pynamodb.attributes import (BooleanAttribute, NumberAttribute,
                                 UnicodeAttribute)
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model


class UserIdNoticedAtIndex(GlobalSecondaryIndex):
    """GSI：お知らせ通知バー降順ソート"""

    class Meta:
        index_name = "user_id-noticed_at-index"
        projection = AllProjection()

    user_id = UnicodeAttribute(hash_key=True)
    noticed_at = JSTDateTimeAttribute(range_key=True)


class NotificationModel(Model):
    """お知らせTBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Notification"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    user_id = UnicodeAttribute(null=False)
    summary = UnicodeAttribute(null=False)
    url = UnicodeAttribute(null=True)
    message = UnicodeAttribute(null=True)
    confirmed = BooleanAttribute(null=False)
    noticed_at = JSTDateTimeAttribute(null=False)
    ttl = NumberAttribute(null=False)

    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())

    user_id_noticed_at_index = UserIdNoticedAtIndex()

    @staticmethod
    def get_total_notifications_count(user_id: str) -> int:
        """お知らせの総通知数を取得する"""

        return NotificationModel.user_id_noticed_at_index.count(hash_key=user_id)
