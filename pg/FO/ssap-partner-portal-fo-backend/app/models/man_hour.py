from datetime import datetime

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute
from pynamodb.attributes import (BooleanAttribute, ListAttribute, MapAttribute,
                                 NumberAttribute, UnicodeAttribute,
                                 VersionAttribute)
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model


class YearMonthDataTypeIndex(GlobalSecondaryIndex):
    """GSI：年月データ区分検索"""

    class Meta:
        index_name = "year_month-data_type-index"
        projection = AllProjection()

    year_month = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True)


class SupportItemsSubAttribute(MapAttribute):
    """支援工数TBL 支援者別工数FASET  Attribute定義"""

    project_id = UnicodeAttribute(null=True)
    role = UnicodeAttribute(null=True)
    service_type = UnicodeAttribute(null=True)
    input_man_hour = NumberAttribute(null=True, default=0)


class AcquirementItemsSubAttribute(MapAttribute):
    """支援工数TBL 支援者別工数FASET  Attribute定義"""

    project_name = UnicodeAttribute(null=True)
    customer_name = UnicodeAttribute(null=True)
    type = UnicodeAttribute(null=True)
    input_man_hour = NumberAttribute(null=True, default=0)


class DirectSupportManHoursAttribute(MapAttribute):
    """支援工数TBL 支援者別工数FASET  Attribute定義"""

    items = ListAttribute(of=SupportItemsSubAttribute, null=True)
    memo = UnicodeAttribute(null=True)


class PreSupportManHoursAttribute(MapAttribute):
    """支援工数TBL 支援者別工数FASET  Attribute定義"""

    items = ListAttribute(of=SupportItemsSubAttribute, null=True)
    memo = UnicodeAttribute(null=True)


class SalesSupportManHoursAttribute(MapAttribute):
    """支援工数TBL 支援者別工数FASET  Attribute定義"""

    items = ListAttribute(of=AcquirementItemsSubAttribute, null=True)
    memo = UnicodeAttribute(null=True)


class SsapManHoursAttribute(MapAttribute):
    """支援工数TBL 支援者別工数FASET  Attribute定義"""

    meeting = NumberAttribute(null=True, default=0)
    study = NumberAttribute(null=True, default=0)
    learning = NumberAttribute(null=True, default=0)
    new_service = NumberAttribute(null=True, default=0)
    startdash = NumberAttribute(null=True, default=0)
    improvement = NumberAttribute(null=True, default=0)
    ssap = NumberAttribute(null=True, default=0)
    qc = NumberAttribute(null=True, default=0)
    accounting = NumberAttribute(null=True, default=0)
    management = NumberAttribute(null=True, default=0)
    office_work = NumberAttribute(null=True, default=0)
    others = NumberAttribute(null=True, default=0)
    memo = UnicodeAttribute(null=True)


class HolidaysManHoursAttribute(MapAttribute):
    """支援工数TBL 支援者別工数FASET  Attribute定義"""

    paid_holiday = NumberAttribute(null=True, default=0)
    holiday = NumberAttribute(null=True, default=0)
    private = NumberAttribute(null=True, default=0)
    others = NumberAttribute(null=True, default=0)
    department_others = NumberAttribute(null=True, default=0)
    memo = UnicodeAttribute(null=True)


class SummaryManHourAllowNullAttribute(MapAttribute):
    """支援工数TBL 支援者別工数FASET  Attribute定義"""

    direct = NumberAttribute(null=True, default=0)
    pre = NumberAttribute(null=True, default=0)
    sales = NumberAttribute(null=True, default=0)
    ssap = NumberAttribute(null=True, default=0)
    others = NumberAttribute(null=True, default=0)
    total = NumberAttribute(null=True, default=0)


class ManHourSupporterModel(Model):
    """支援工数TBL 支援者別工数FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "ManHour"
        region = get_app_settings().aws_region

    data_type = UnicodeAttribute(hash_key=True, null=False)
    year_month = UnicodeAttribute(range_key=True, null=False)
    supporter_user_id = UnicodeAttribute(null=False)
    supporter_name = UnicodeAttribute(null=False)
    supporter_organization_id = UnicodeAttribute(null=False)
    supporter_organization_name = UnicodeAttribute(null=False)
    direct_support_man_hours = DirectSupportManHoursAttribute(null=False)
    pre_support_man_hours = PreSupportManHoursAttribute(null=False)
    sales_support_man_hours = SalesSupportManHoursAttribute(null=False)
    ssap_man_hours = SsapManHoursAttribute(null=False)
    holidays_man_hours = HolidaysManHoursAttribute(null=False)
    summary_man_hour = SummaryManHourAllowNullAttribute(null=True)
    is_confirm = BooleanAttribute(null=False)

    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    year_month_data_type_index = YearMonthDataTypeIndex()


class ManHourSupporterOrganizationSummaryModel(Model):
    """支援工数TBL 支援者組織(課)別工数集計FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "ManHour"
        region = get_app_settings().aws_region

    data_type = UnicodeAttribute(hash_key=True, null=False)
    year_month = UnicodeAttribute(range_key=True, null=False)
    supporter_organization_id = UnicodeAttribute(null=False)
    supporter_organization_name = UnicodeAttribute(null=False)
    annual_sales = NumberAttribute(null=False, default=0)
    monthly_sales = NumberAttribute(null=False, default=0)
    monthly_project_price = NumberAttribute(null=False, default=0)
    monthly_contract_time = NumberAttribute(null=False, default=0)
    monthly_work_time = NumberAttribute(null=False, default=0)
    monthly_work_time_rate = NumberAttribute(null=False, default=0)
    monthly_supporters = NumberAttribute(null=False, default=0)
    monthly_man_hour = NumberAttribute(null=False, default=0)
    monthly_occupancy_rate = NumberAttribute(null=False, default=0)
    monthly_occupancy_total_time = NumberAttribute(null=False, default=0)
    monthly_occupancy_total_rate = NumberAttribute(null=False, default=0)

    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())

    year_month_data_type_index = YearMonthDataTypeIndex()


class SupporterUsersAttribute(MapAttribute):
    """支援工数TBL プロジェクト別工数集計FASET  Attribute定義"""

    id = UnicodeAttribute(null=True)
    name = UnicodeAttribute(null=True)
    is_confirm = BooleanAttribute(null=True)


class SupportManHourAttribute(MapAttribute):
    """支援工数TBL プロジェクト別工数集計FASET  Attribute定義"""

    supporter_user_id = UnicodeAttribute(null=False)
    input_man_hour = NumberAttribute(null=False, default=0)


class ManHourProjectSummaryModel(Model):
    """支援工数TBL プロジェクト別工数集計FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "ManHour"
        region = get_app_settings().aws_region

    data_type = UnicodeAttribute(hash_key=True, null=False)
    year_month = UnicodeAttribute(range_key=True, null=False)
    project_id = UnicodeAttribute(null=False)
    project_name = UnicodeAttribute(null=False)
    customer_id = UnicodeAttribute(null=False)
    customer_name = UnicodeAttribute(null=False)
    supporter_organization_id = UnicodeAttribute(null=False)
    service_type = UnicodeAttribute(null=False)
    support_date_from = UnicodeAttribute(null=True)
    support_date_to = UnicodeAttribute(null=True)
    contract_type = UnicodeAttribute(null=True)
    main_supporter_user = SupporterUsersAttribute(null=True)
    supporter_users = ListAttribute(of=SupporterUsersAttribute, null=True)
    total_contract_time = NumberAttribute(null=False)
    this_month_contract_time = NumberAttribute(null=True)
    total_profit = NumberAttribute(null=True)
    this_month_profit = NumberAttribute(null=True)
    this_month_direct_support_man_hour = NumberAttribute(null=True, default=0)
    this_month_direct_support_man_hour_main = NumberAttribute(null=True, default=0)
    this_month_direct_support_man_hour_sub = NumberAttribute(null=True, default=0)
    this_month_pre_support_man_hour = NumberAttribute(null=True, default=0)
    summary_direct_support_man_hour = NumberAttribute(null=True, default=0)
    summary_pre_support_man_hour = NumberAttribute(null=True, default=0)
    summary_theoretical_direct_support_man_hour = NumberAttribute(null=True, default=0)
    summary_theoretical_pre_support_man_hour = NumberAttribute(null=True, default=0)
    this_month_theoretical_direct_support_man_hour = NumberAttribute(
        null=True, default=0
    )
    this_month_theoretical_pre_support_man_hour = NumberAttribute(null=True, default=0)
    this_month_supporter_direct_support_man_hours = ListAttribute(
        of=SupportManHourAttribute, null=True
    )
    this_month_supporter_pre_support_man_hours = ListAttribute(
        of=SupportManHourAttribute, null=True
    )
    summary_supporter_direct_support_man_hours = ListAttribute(
        of=SupportManHourAttribute, null=True
    )
    summary_supporter_pre_support_man_hours = ListAttribute(
        of=SupportManHourAttribute, null=True
    )

    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())

    year_month_data_type_index = YearMonthDataTypeIndex()


class ManHourServiceTypeSummaryModel(Model):
    """支援工数TBL サービス種別別工数集計FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "ManHour"
        region = get_app_settings().aws_region

    data_type = UnicodeAttribute(hash_key=True, null=False)
    year_month = UnicodeAttribute(range_key=True, null=False)
    service_type_id = UnicodeAttribute(null=False)
    service_type_name = UnicodeAttribute(null=False)
    this_month_contract_time = NumberAttribute(null=False, default=0)
    this_month_paid_contract_time = NumberAttribute(null=False, default=0)
    number_of_supporters = NumberAttribute(null=True, default=0)
    man_hour_rate = NumberAttribute(null=True, default=0)
    total_man_hour = NumberAttribute(null=True, default=0)
    total_man_hour_y_percent = NumberAttribute(null=True, default=0)
    factor = NumberAttribute(null=True, default=0)

    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())

    year_month_data_type_index = YearMonthDataTypeIndex()
