from datetime import datetime

from moto import mock_dynamodb

from app.models.master import MasterBatchControlModel, MasterSupporterOrganizationModel
from app.models.master_alert_setting import (
    AlertSettingAttribute,
    FactorSettingSubAttribute,
    MasterAlertSettingModel,
)


@mock_dynamodb
def test_model_master_supporter_organization():
    """汎用マスターTBL MasterSupporterOrganizationModelクラス テスト定義"""

    MasterSupporterOrganizationModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    id = "id1"
    data_type = "supporter_organization"

    supporter_organization = MasterSupporterOrganizationModel(id, data_type)

    supporter_organization.name = "customer1"
    supporter_organization.order = 1
    supporter_organization.use = False
    supporter_organization.create_at = datetime.now()
    supporter_organization.update_at = datetime.now()

    supporter_organization.save()

    supporter_organization_item = MasterSupporterOrganizationModel.get(id, data_type)
    supporter_organization_item_by_query_order = (
        MasterSupporterOrganizationModel.data_type_order_index.query(
            data_type, supporter_organization.order
        )
    )
    supporter_organization_item_by_query_name = (
        MasterSupporterOrganizationModel.data_type_name_index.query(
            data_type, supporter_organization.name
        )
    )

    assert str(supporter_organization) == str(supporter_organization_item)
    assert supporter_organization_item_by_query_order is not None
    assert supporter_organization_item_by_query_name is not None

    MasterSupporterOrganizationModel.delete_table()


@mock_dynamodb
def test_model_master_alert_setting():
    """汎用マスターTBL MasterAlertSettingModelクラス テスト定義"""

    MasterAlertSettingModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    id = "id1"
    data_type = "alert_setting"

    alert_setting = MasterAlertSettingModel(id, data_type)

    alert_setting.name = "alert1"
    alert_setting.attributes = AlertSettingAttribute(
        factor_setting=[
            FactorSettingSubAttribute(
                service_type_id="service_1",
                direct_support_man_hour=10,
                direct_and_pre_support_man_hour=15,
                pre_support_man_hour=0,
                hourly_man_hour_price=0,
                monthly_profit=0,
            ),
            FactorSettingSubAttribute(
                service_type_id="service_2",
                direct_support_man_hour=15,
                direct_and_pre_support_man_hour=20,
                pre_support_man_hour=0,
                hourly_man_hour_price=0,
                monthly_profit=0,
            ),
        ],
        display_setting=None,
    )
    alert_setting.create_at = datetime.now()
    alert_setting.update_at = datetime.now()

    alert_setting.save()

    alert_setting_item = MasterAlertSettingModel.get(id, data_type)
    alert_setting_item_by_query = MasterAlertSettingModel.data_type_name_index.query(
        data_type, alert_setting.name
    )

    assert str(alert_setting) == str(alert_setting_item)
    assert alert_setting_item_by_query is not None

    MasterAlertSettingModel.delete_table()


@mock_dynamodb
def test_model_master_batch_control():
    """汎用マスターTBL MasterBatchControlModelクラス テスト定義"""

    MasterBatchControlModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    id = "id1"
    data_type = "batch_control"

    batch_control = MasterBatchControlModel(id, data_type)

    batch_control.name = "BOアンケート情報集計データ作成処理"
    batch_control.value = "function1"
    batch_control.update_at = datetime.now()

    batch_control.save()

    batch_control_item = MasterBatchControlModel.get(id, data_type)
    batch_control_item_by_query = MasterBatchControlModel.data_type_name_index.query(
        data_type, batch_control.name
    )

    assert str(batch_control) == str(batch_control_item)
    assert batch_control_item_by_query is not None

    MasterBatchControlModel.delete_table()
