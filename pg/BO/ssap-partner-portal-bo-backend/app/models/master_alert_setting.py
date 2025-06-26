from datetime import datetime

from pynamodb.attributes import (
    BooleanAttribute,
    ListAttribute,
    MapAttribute,
    NumberAttribute,
    UnicodeAttribute,
    VersionAttribute,
)
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute


class DataTypeNameIndex(GlobalSecondaryIndex):
    """GSI：サジェスト用名称検索"""

    class Meta:
        index_name = "data_type-name-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class DataTypeIndex(GlobalSecondaryIndex):
    """GSI：データ区分検索"""

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class FactorSettingSubAttribute(MapAttribute):
    """汎用マスターTBL 支援工数アラート設定FASET Attribute定義"""

    service_type_id = UnicodeAttribute(null=False)
    direct_support_man_hour = NumberAttribute(null=False, default=0)
    direct_and_pre_support_man_hour = NumberAttribute(null=False, default=0)
    pre_support_man_hour = NumberAttribute(null=True, default=0)
    hourly_man_hour_price = NumberAttribute(null=True, default=0)
    monthly_profit = NumberAttribute(null=True, default=0)


class DirectSupportManHourSubAttribute(MapAttribute):
    """汎用マスターTBL 支援工数アラート設定FASET Attribute定義"""

    summary_display_project = BooleanAttribute(null=True)
    summary_over_alert = BooleanAttribute(null=True)
    summary_prospect_alert = BooleanAttribute(null=True)
    this_month_display_project = BooleanAttribute(null=True)
    this_month_over_alert = BooleanAttribute(null=True)
    this_month_prospect_alert = BooleanAttribute(null=True)


class PreSupportManHourSubAttribute(MapAttribute):
    """汎用マスターTBL 支援工数アラート設定FASET Attribute定義"""

    summary_display_project = BooleanAttribute(null=True)
    summary_over_alert = BooleanAttribute(null=True)
    summary_prospect_alert = BooleanAttribute(null=True)
    this_month_display_project = BooleanAttribute(null=True)
    this_month_over_alert = BooleanAttribute(null=True)
    this_month_prospect_alert = BooleanAttribute(null=True)


class DisplaySettingSubAttribute(MapAttribute):
    """汎用マスターTBL 支援工数アラート設定FASET Attribute定義"""

    direct_support_man_hour = DirectSupportManHourSubAttribute(null=True)
    pre_support_man_hour = PreSupportManHourSubAttribute(null=True)


class AlertSettingAttribute(MapAttribute):
    """汎用マスターTBL 支援工数アラート設定FASET Attribute定義"""

    factor_setting = ListAttribute(of=FactorSettingSubAttribute, null=False)
    display_setting = DisplaySettingSubAttribute(null=True)


class MasterAlertSettingModel(Model):
    """汎用マスターTBL 支援工数アラート設定FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Master"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    value = UnicodeAttribute(null=True)
    attributes = AlertSettingAttribute(null=False)
    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    data_type_name_index = DataTypeNameIndex()
    data_type_index = DataTypeIndex()
