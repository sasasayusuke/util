from datetime import datetime

from pynamodb.attributes import (
    BooleanAttribute,
    ListAttribute,
    MapAttribute,
    NumberAttribute,
    UnicodeAttribute,
    UnicodeSetAttribute,
    VersionAttribute,
)
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute


class DataTypeIndex(GlobalSecondaryIndex):
    """GSI：データ区分検索"""

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class DataTypeNameIndex(GlobalSecondaryIndex):
    """GSI：データ区分法人ソルバー名検索"""

    class Meta:
        index_name = "data_type-name-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class ValueAndMemoAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    value = NumberAttribute(null=True)
    memo = UnicodeAttribute(null=True)


class AddressAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    postal_code = UnicodeAttribute(null=True)
    state = UnicodeAttribute(null=True)
    city = UnicodeAttribute(null=True)
    street = UnicodeAttribute(null=True)
    building = UnicodeAttribute(null=True)


class CorporatePhotoAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    file_name = UnicodeAttribute(null=True)
    path = UnicodeAttribute(null=True)


class CorporateInfoDocumentAttribute(MapAttribute):
    """案件カルテTBL Attribute定義"""

    file_name = UnicodeAttribute(null=True)
    path = UnicodeAttribute(null=True)


class MainChargeAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    name = UnicodeAttribute(null=True)
    kana = UnicodeAttribute(null=True)
    title = UnicodeAttribute(null=True)
    email = UnicodeAttribute(null=True)
    department = UnicodeAttribute(null=True)
    phone = UnicodeAttribute(null=True)


class DeputyChargeAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    name = UnicodeAttribute(null=True)
    kana = UnicodeAttribute(null=True)
    title = UnicodeAttribute(null=True)
    email = UnicodeAttribute(null=True)
    department = UnicodeAttribute(null=True)
    phone = UnicodeAttribute(null=True)


class OtherChargeAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    name = UnicodeAttribute(null=True)
    title = UnicodeAttribute(null=True)
    email = UnicodeAttribute(null=True)
    department = UnicodeAttribute(null=True)
    phone = UnicodeAttribute(null=True)


class SolverCorporationModel(Model):
    """ソルバー法人TBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "SolverCorporation"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    company_abbreviation = UnicodeAttribute(null=True)
    industry = UnicodeAttribute(null=True)
    established = UnicodeAttribute(null=True)
    management_team = UnicodeAttribute(null=True)
    employee = ValueAndMemoAttribute(null=True)
    capital = ValueAndMemoAttribute(null=True)
    earnings = ValueAndMemoAttribute(null=True)
    listing_exchange = UnicodeAttribute(null=True)
    business_content = UnicodeAttribute(null=True)
    address = AddressAttribute(null=True)
    corporate_photo = CorporatePhotoAttribute(null=True)
    corporate_info_document = ListAttribute(
        of=CorporateInfoDocumentAttribute, null=True
    )
    issue_map50 = UnicodeSetAttribute(null=True)
    vision = UnicodeAttribute(null=True)
    mission = UnicodeAttribute(null=True)
    main_charge = MainChargeAttribute(null=True)
    deputy_charge = DeputyChargeAttribute(null=True)
    other_charge = OtherChargeAttribute(null=True)
    notes = UnicodeAttribute(null=True)
    disabled = BooleanAttribute(null=False, default=False)
    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    price_and_operating_rate_update_at = JSTDateTimeAttribute(null=True)
    price_and_operating_rate_update_by = UnicodeAttribute(null=True)
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=True)
    version = VersionAttribute()
    utilization_rate_version = NumberAttribute(null=False, default=1)

    data_type_index = DataTypeIndex()
    data_type_name_index = DataTypeNameIndex()
