from datetime import datetime

from pynamodb.attributes import (
    BooleanAttribute,
    MapAttribute,
    NumberAttribute,
    UnicodeAttribute,
    VersionAttribute,
    ListAttribute,
)
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute


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


class DataTypeIndexForBatchControl(GlobalSecondaryIndex):
    """GSI：データ区分検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


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
    def is_duplicate_data_type_and_value(data_type: str, value: str, count=0) -> bool:
        """種別 & 値の重複チェックを行う"""
        count += MasterSupporterOrganizationModel.data_type_value_index.count(
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


class BatchControlAttribute(MapAttribute):
    """汎用マスターTBL バッチ処理制御FASET Attribute定義"""

    batch_start_at = JSTDateTimeAttribute(null=True)
    batch_end_at = JSTDateTimeAttribute(null=True)
    batch_status = UnicodeAttribute(null=True)
    batch_rerun_span = NumberAttribute(null=True)


class BatchControlErrors(MapAttribute):
    """汎用マスターTBL バッチ処理制御FASET Attribute定義"""

    type = UnicodeAttribute(null=False)
    details = ListAttribute(of=MapAttribute, null=True)


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
    errors = ListAttribute(of=BatchControlErrors, null=True)

    data_type_name_index = DataTypeNameIndexForBatchControl()
    data_type_value_index = DataTypeValueIndexForBatchControl()
    data_type_index = DataTypeIndexForBatchControl()


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
