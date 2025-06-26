from datetime import datetime

from pynamodb.attributes import NumberAttribute, UnicodeAttribute
from pynamodb.models import Model

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute


class SessionModel(Model):
    """セッションTBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Session"
        region = get_app_settings().aws_region

    cognito_id = UnicodeAttribute(hash_key=True, null=False)

    latest_access_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    ttl = NumberAttribute(null=True)
