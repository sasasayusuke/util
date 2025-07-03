from datetime import datetime

from app.models.man_hour import (ManHourProjectSummaryModel,
                                 ManHourServiceTypeSummaryModel,
                                 ManHourSupporterModel,
                                 ManHourSupporterOrganizationSummaryModel)
from moto import mock_dynamodb


@mock_dynamodb
def test_model_man_hour_supporter():
    """支援工数TBL ManHourSupporterModelクラス テスト定義
    """

    ManHourSupporterModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    data_type = 'supporter'
    year_month = datetime.now().strftime("%Y/%m")

    supporter = ManHourSupporterModel(data_type, year_month)

    supporter.supporter_user_id = 'supporter1'
    supporter.supporter_name = 'tanaka'
    supporter.supporter_organization_id = 'organization1'
    supporter.supporter_organization_name = '株式会社アイレット'
    supporter.direct_support_man_hours = {}
    supporter.pre_support_man_hours = {}
    supporter.sales_support_man_hours = {}
    supporter.ssap_man_hours = {}
    supporter.holidays_man_hours = {}
    supporter.is_confirm = False
    supporter.create_at = datetime.now()
    supporter.update_at = datetime.now()

    supporter.save()

    supporter_item = ManHourSupporterModel.get(data_type, year_month)
    supporter_item_by_query = ManHourSupporterModel.year_month_data_type_index.query(
        year_month, data_type
    )

    assert str(supporter) == str(supporter_item)
    assert supporter_item_by_query is not None

    ManHourSupporterModel.delete_table()


@mock_dynamodb
def test_model_man_hour_supporter_organization_summary():
    """支援工数TBL ManHourSupporterOrganizationSummaryModelクラス テスト定義
    """

    ManHourSupporterOrganizationSummaryModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    data_type = 'supporter_organization_summary'
    year_month = datetime.now().strftime("%Y/%m")

    supporter_organization_summary = ManHourSupporterOrganizationSummaryModel(
        data_type, year_month
    )

    supporter_organization_summary.supporter_organization_id = 'organization1'
    supporter_organization_summary.supporter_organization_name = '株式会社アイレット'
    supporter_organization_summary.is_confirm = False
    supporter_organization_summary.annual_sales = 2000000000
    supporter_organization_summary.monthly_sales = 200000000
    supporter_organization_summary.monthly_project_price = 10
    supporter_organization_summary.monthly_contract_time = 300
    supporter_organization_summary.monthly_work_time = 200
    supporter_organization_summary.monthly_work_time_rate = 2
    supporter_organization_summary.monthly_supporters = 20
    supporter_organization_summary.monthly_man_hour = 30
    supporter_organization_summary.monthly_occupancy_rate = 20
    supporter_organization_summary.monthly_occupancy_total_time = 180
    supporter_organization_summary.monthly_occupancy_total_rate = 150
    supporter_organization_summary.create_at = datetime.now()
    supporter_organization_summary.update_at = datetime.now()

    supporter_organization_summary.save()

    supporter_organization_summary_item = ManHourSupporterOrganizationSummaryModel.get(
        data_type, year_month
    )
    supporter_organization_summary_item_by_query = ManHourSupporterOrganizationSummaryModel.year_month_data_type_index.query(
        year_month, data_type
    )

    assert str(supporter_organization_summary
               ) == str(supporter_organization_summary_item)
    assert supporter_organization_summary_item_by_query is not None

    ManHourSupporterOrganizationSummaryModel.delete_table()


@mock_dynamodb
def test_model_man_hour_project_summary():
    """支援工数TBL ManHourProjectSummaryModelクラス テスト定義
    """

    ManHourProjectSummaryModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    data_type = 'project_summary'
    year_month = datetime.now().strftime("%Y/%m")

    project_summary = ManHourProjectSummaryModel(data_type, year_month)

    project_summary.project_id = 'project1'
    project_summary.project_name = 'project-x'
    project_summary.customer_id = 'customer1'
    project_summary.customer_name = '株式会社アイレット'
    project_summary.supporter_organization_id = 'organization1'
    project_summary.service_type = '組織開発'
    project_summary.total_contract_time = 20
    project_summary.create_at = datetime.now()
    project_summary.update_at = datetime.now()

    project_summary.save()

    project_summary_item = ManHourProjectSummaryModel.get(
        data_type, year_month
    )
    project_summary_item_by_query = ManHourProjectSummaryModel.year_month_data_type_index.query(
        year_month, data_type
    )

    assert str(project_summary) == str(project_summary_item)
    assert project_summary_item_by_query is not None

    ManHourProjectSummaryModel.delete_table()


@mock_dynamodb
def test_model_man_hour_service_type_summary():
    """支援工数TBL ManHourServiceTypeSummaryModelクラス テスト定義
    """

    ManHourServiceTypeSummaryModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    data_type = 'service_type_summary'
    year_month = datetime.now().strftime("%Y/%m")

    service_type_summary = ManHourServiceTypeSummaryModel(
        data_type, year_month
    )

    service_type_summary.service_type_id = 'service1'
    service_type_summary.service_type_name = 'servicex'
    service_type_summary.this_month_contract_time = 300
    service_type_summary.create_at = datetime.now()
    service_type_summary.update_at = datetime.now()

    service_type_summary.save()

    service_type_summary_item = ManHourServiceTypeSummaryModel.get(
        data_type, year_month
    )
    service_type_summary_item_by_query = ManHourServiceTypeSummaryModel.year_month_data_type_index.query(
        year_month, data_type
    )

    assert str(service_type_summary) == str(service_type_summary_item)
    assert service_type_summary_item_by_query is not None

    ManHourServiceTypeSummaryModel.delete_table()
