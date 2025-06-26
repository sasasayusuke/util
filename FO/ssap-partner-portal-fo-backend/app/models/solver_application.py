from datetime import datetime

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute
from pynamodb.attributes import (
    UnicodeAttribute,
    VersionAttribute,
)
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model


class DataTypeIndex(GlobalSecondaryIndex):
    """GSI：データ区分検索"""

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class SolverApplicationModel(Model):
    """ソルバー案件TBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "SolverApplication"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    solver_application_id = UnicodeAttribute(null=False)
    name = UnicodeAttribute(null=False)
    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    data_type_index = DataTypeIndex()
