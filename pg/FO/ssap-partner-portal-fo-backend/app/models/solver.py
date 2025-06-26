from datetime import datetime

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute


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


class DataTypeIndex(GlobalSecondaryIndex):
    """GSI：データ区分検索"""

    class Meta:
        index_name = "data_type-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)


class DataTypeCreateAtIndex(GlobalSecondaryIndex):
    """GSI：データ区分登録日時検索"""

    class Meta:
        index_name = "data_type-create_at-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    create_at = JSTDateTimeAttribute(range_key=True)


class DataTypeSexIndex(GlobalSecondaryIndex):
    """GSI：データ区分性別検索"""

    class Meta:
        index_name = "data_type-sex-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    sex = UnicodeAttribute(range_key=True)


class DataTypeBirthDayIndex(GlobalSecondaryIndex):
    """GSI：法人ID生年月日検索"""

    class Meta:
        index_name = "data_type-birth_day-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    birth_day = UnicodeAttribute(range_key=True)


class DataTypeOperatingStatusIndex(GlobalSecondaryIndex):
    """GSI：データ区分稼働状況検索"""

    class Meta:
        index_name = "data_type-operating_status-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    operating_status = UnicodeAttribute(range_key=True)


class DataTypeProvidedOperatingRateIndex(GlobalSecondaryIndex):
    """GSI：データ区分提供稼働率（今月）検索"""

    class Meta:
        index_name = "data_type-provided_operating_rate-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    provided_operating_rate = NumberAttribute(range_key=True)


class DataTypeProvidedOperatingRateNextIndex(GlobalSecondaryIndex):
    """GSI：データ区分提供稼働率（来月）検索"""

    class Meta:
        index_name = "data_type-provided_operating_rate_next-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    provided_operating_rate_next = NumberAttribute(range_key=True)


class DataTypePricePerPersonMonthIndex(GlobalSecondaryIndex):
    """GSI：データ区分人月単価（上限）検索"""

    class Meta:
        index_name = "data_type-price_per_person_month-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    price_per_person_month = NumberAttribute(range_key=True)


class DataTypeRegistrationStatusIndex(GlobalSecondaryIndex):
    """GSI：データ区分登録状態検索"""

    class Meta:
        index_name = "data_type-registration_status-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    registration_status = UnicodeAttribute(range_key=True)


class DataTypePriceAndOperatingRateUpdateAtIndex(GlobalSecondaryIndex):
    """GSI：データ区分稼働率・単価最終更新日時検索"""

    class Meta:
        index_name = "data_type-price_and_operating_rate_update_at-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    price_and_operating_rate_update_at = JSTDateTimeAttribute(range_key=True)


class FacePhotoAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    file_name = UnicodeAttribute(null=True)
    path = UnicodeAttribute(null=True)


class ResumeAttribute(MapAttribute):
    """案件カルテTBL Attribute定義"""

    file_name = UnicodeAttribute(null=True)
    path = UnicodeAttribute(null=True)


class ScreeningAttribute(MapAttribute):
    """案件カルテTBL Attribute定義"""

    evaluation = BooleanAttribute(null=False, default=False)
    evidence = UnicodeAttribute(null=True)


class SolverModel(Model):
    """個人ソルバーTBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Solver"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    name_kana = UnicodeAttribute(null=True)
    solver_application_ids = UnicodeSetAttribute(null=True)
    corporate_id = UnicodeAttribute(null=True)
    sex = UnicodeAttribute(null=True)
    birth_day = UnicodeAttribute(null=True)
    email = UnicodeAttribute(null=True)
    phone = UnicodeAttribute(null=True)
    english_level = UnicodeAttribute(null=True)
    work_history = UnicodeAttribute(null=True)
    is_consulting_firm = BooleanAttribute(null=False, default=False)
    specialized_themes = UnicodeAttribute(null=True)
    academic_background = UnicodeAttribute(null=True)
    title = UnicodeAttribute(null=True)
    face_photo = FacePhotoAttribute(null=True)
    resume = ListAttribute(of=ResumeAttribute, null=True)
    operating_status = UnicodeAttribute(null=True)
    provided_operating_rate = NumberAttribute(null=True)
    provided_operating_rate_next = NumberAttribute(null=True)
    operation_prospects_month_after_next = UnicodeAttribute(null=True)
    price_per_person_month = NumberAttribute(null=True)
    price_per_person_month_lower = NumberAttribute(null=True)
    hourly_rate = NumberAttribute(null=True)
    hourly_rate_lower = NumberAttribute(null=True)
    price_and_operating_rate_update_at = JSTDateTimeAttribute(null=True)
    price_and_operating_rate_update_by = UnicodeAttribute(null=True)
    tsi_areas = UnicodeSetAttribute(null=True)
    issue_map50 = UnicodeSetAttribute(null=True)
    main_achievements = UnicodeAttribute(null=True)
    screening_1 = ScreeningAttribute(null=True)
    screening_2 = ScreeningAttribute(null=True)
    screening_3 = ScreeningAttribute(null=True)
    screening_4 = ScreeningAttribute(null=True)
    screening_5 = ScreeningAttribute(null=True)
    screening_6 = ScreeningAttribute(null=True)
    screening_7 = ScreeningAttribute(null=True)
    screening_8 = ScreeningAttribute(null=True)
    criteria_1 = UnicodeAttribute(null=True)
    criteria_2 = UnicodeAttribute(null=True)
    criteria_3 = UnicodeAttribute(null=True)
    criteria_4 = UnicodeAttribute(null=True)
    criteria_5 = UnicodeAttribute(null=True)
    criteria_6 = UnicodeAttribute(null=True)
    criteria_7 = UnicodeAttribute(null=True)
    criteria_8 = UnicodeAttribute(null=True)
    notes = UnicodeAttribute(null=True)
    is_solver = BooleanAttribute(null=False, default=False)
    registration_status = UnicodeAttribute(null=False, default="new")
    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()
    file_key_id = UnicodeAttribute(null=True)

    data_type_index = DataTypeIndex()
    data_type_create_at_index = DataTypeCreateAtIndex()
    data_type_sex_index = DataTypeSexIndex()
    data_type_birth_day_index = DataTypeBirthDayIndex()
    data_type_operating_status_index = DataTypeOperatingStatusIndex()
    data_type_provided_operating_rate_index = DataTypeProvidedOperatingRateIndex()
    data_type_provided_operating_rate_next_index = (
        DataTypeProvidedOperatingRateNextIndex()
    )
    data_type_price_per_person_month_index = DataTypePricePerPersonMonthIndex()
    data_type_registration_status_index = DataTypeRegistrationStatusIndex()
    data_type_price_and_operating_rate_update_at_index = (
        DataTypePriceAndOperatingRateUpdateAtIndex()
    )
