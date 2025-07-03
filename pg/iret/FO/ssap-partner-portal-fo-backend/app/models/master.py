from datetime import datetime

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute
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


class DataTypeOrderIndex(GlobalSecondaryIndex):
    """GSI：プルダウン用並び順ソート"""

    class Meta:
        index_name = "data_type-order-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    order = NumberAttribute(range_key=True)


class DataTypeNameIndex(GlobalSecondaryIndex):
    """GSI：サジェスト用名称検索"""

    class Meta:
        index_name = "data_type-name-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class DataTypeNameIndexForAlertSetting(GlobalSecondaryIndex):
    class Meta:
        index_name = "data_type-name-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class DataTypeNameIndexForBatchControl(GlobalSecondaryIndex):
    """GSI：サジェスト用名称検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-name-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class DataTypeValueIndex(GlobalSecondaryIndex):
    """GSI：データ区分値検索"""

    class Meta:
        index_name = "data_type-value-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    value = UnicodeAttribute(range_key=True)


class DataTypeValueIndexForAlertSetting(GlobalSecondaryIndex):
    class Meta:
        index_name = "data_type-value-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    value = UnicodeAttribute(range_key=True)


class DataTypeValueIndexForBatchControl(GlobalSecondaryIndex):
    """GSI：データ区分値検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-value-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    value = UnicodeAttribute(range_key=True)


class DataTypeIndex(GlobalSecondaryIndex):
    """GSI：データ区分検索"""

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class DataTypeIndexForAlertSetting(GlobalSecondaryIndex):
    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class DataTypeIndexForBatchControl(GlobalSecondaryIndex):
    """GSI：データ区分検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class DataTypeIndexForUpdateKarteRecord(GlobalSecondaryIndex):
    """GSI：データ区分検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class DataTypeIndexForSelectItems(GlobalSecondaryIndex):
    """GSI：データ区分検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class DataTypeOrderIndexForSelectItems(GlobalSecondaryIndex):
    """GSI：データ区分検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-order-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    order = NumberAttribute(range_key=True)


class DataTypeIndexForServiceManager(GlobalSecondaryIndex):
    """GSI：データ区分検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class SupporterOrganizationAttribute(MapAttribute):
    """汎用マスターTBL 支援者組織情報FASET Attribute定義"""

    info1 = UnicodeAttribute(null=True)
    info2 = UnicodeAttribute(null=True)
    info3 = UnicodeAttribute(null=True)
    info4 = UnicodeAttribute(null=True)
    info5 = UnicodeAttribute(null=True)


class MasterSupporterOrganizationModel(Model):
    """汎用マスターTBL 支援者組織情報FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Master"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    value = UnicodeAttribute(null=True)
    attributes = SupporterOrganizationAttribute(null=True)
    order = NumberAttribute(null=False)
    use = BooleanAttribute(null=False, default=True)
    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    data_type_order_index = DataTypeOrderIndex()
    data_type_name_index = DataTypeNameIndex()
    data_type_value_index = DataTypeValueIndex()
    data_type_index = DataTypeIndex()

    @staticmethod
    def is_duplicate_data_type_and_value(data_type: str, value: str) -> bool:
        """種別 & 値の重複チェックを行う"""
        count = MasterSupporterOrganizationModel.data_type_value_index.count(
            hash_key=data_type,
            range_key_condition=MasterSupporterOrganizationModel.value == value,
        )
        return count >= 1

    @staticmethod
    def get_same_data_type_registrations_count(data_type: str) -> int:
        """同じ種別の中の登録件数を取得する"""
        return MasterSupporterOrganizationModel.data_type_index.count(
            hash_key=data_type
        )


class FactorSettingSubAttribute(MapAttribute):
    """汎用マスターTBL 支援工数アラート設定FASET Attribute定義"""

    service_type_id = UnicodeAttribute(null=False)
    direct_support_man_hour = NumberAttribute(null=False, default=0)
    direct_and_pre_support_man_hour = NumberAttribute(null=False, default=0)
    pre_support_man_hour = NumberAttribute(null=False, default=0)
    hourly_man_hour_price = NumberAttribute(null=False, default=0)
    monthly_profit = NumberAttribute(null=False, default=0)


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

    factor_setting = ListAttribute(of=FactorSettingSubAttribute, null=True)
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

    data_type_name_index = DataTypeNameIndexForAlertSetting()
    data_type_value_index = DataTypeValueIndexForAlertSetting()
    data_type_index = DataTypeIndexForAlertSetting()


class BatchControlAttribute(MapAttribute):
    """汎用マスターTBL バッチ処理制御FASET Attribute定義"""

    batch_start_at = JSTDateTimeAttribute(null=True)
    batch_end_at = JSTDateTimeAttribute(null=True)
    batch_status = UnicodeAttribute(null=True)
    batch_rerun_span = NumberAttribute(null=True)


class MasterBatchControlModel(Model):
    """汎用マスターTBL バッチ処理制御FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Master"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    value = UnicodeAttribute(null=False)
    attributes = BatchControlAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())

    data_type_name_index = DataTypeNameIndexForBatchControl()
    data_type_value_index = DataTypeValueIndexForBatchControl()
    data_type_index = DataTypeIndexForBatchControl()


class MasterUpdateKarteRecord(Model):
    """汎用マスターTBL 個別カルテ更新記録FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Master"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    update_count = NumberAttribute(null=False, default=0)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())

    data_type_index = DataTypeIndexForUpdateKarteRecord()


class MasterSelectItems(Model):
    """汎用マスターTBL 選択肢FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Master"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    category = UnicodeAttribute(null=True)
    name = UnicodeAttribute(null=True)
    order = NumberAttribute(null=True)
    use = BooleanAttribute(null=False, default=True)
    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute(null=False, default=1)

    data_type_index = DataTypeIndexForSelectItems()
    data_type_order_index = DataTypeOrderIndexForSelectItems()


class MasterServiceManagerModel(Model):
    """汎用マスターTBL サービス責任者FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Master"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    supporter_organization_id = UnicodeAttribute(null=False)
    supporter_organization_name = UnicodeAttribute(null=False)

    data_type_index = DataTypeIndexForServiceManager()
