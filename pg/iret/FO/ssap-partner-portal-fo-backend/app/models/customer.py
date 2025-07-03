from datetime import datetime

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute
from pynamodb.attributes import (BooleanAttribute, NumberAttribute,
                                 UnicodeAttribute, VersionAttribute)
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model


class DataTypeNameIndex(GlobalSecondaryIndex):
    """GSI：データ区分取引先名検索"""

    class Meta:
        index_name = "data_type-name-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class DataTypeCategoryIndex(GlobalSecondaryIndex):
    """GSI：データ区分カテゴリ検索"""

    class Meta:
        index_name = "data_type-category-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    category = UnicodeAttribute(range_key=True)


class CustomerModel(Model):
    """取引先TBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Customer"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    category = UnicodeAttribute(null=True)
    salesforce_customer_id = UnicodeAttribute(null=True)
    salesforce_update_at = JSTDateTimeAttribute(null=True)
    salesforce_target = BooleanAttribute(null=True)
    salesforce_credit_limit = NumberAttribute(null=True)
    salesforce_credit_get_month = UnicodeAttribute(null=True)
    salesforce_credit_manager = UnicodeAttribute(null=True)
    salesforce_credit_no_retry = BooleanAttribute(null=True)
    salesforce_paws_credit_number = UnicodeAttribute(null=True)
    salesforce_customer_owner = UnicodeAttribute(null=True)
    salesforce_customer_segment = UnicodeAttribute(null=True)

    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    data_type_name_index = DataTypeNameIndex()
    data_type_category_index = DataTypeCategoryIndex()

    @staticmethod
    def get_customer_count(
        customer_id: str,
        data_type: str,
    ) -> int:
        return CustomerModel.count(
            hash_key=customer_id,
            range_key_condition=CustomerModel.data_type == data_type,
        )
