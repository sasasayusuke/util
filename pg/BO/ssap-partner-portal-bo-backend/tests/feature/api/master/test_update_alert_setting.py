from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.master_alert_setting import (
    AlertSettingAttribute,
    FactorSettingSubAttribute,
    MasterAlertSettingModel,
)
from app.resources.const import MasterDataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestUpdateAlertSetting:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.SURVEY_OPS.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_not_access(
        self,
        mock_auth_admin,
        role_types,
    ):
        mock_auth_admin(role_types)
        version = 1
        response = client.put(f"/api/masters/alert-setting?version={version}", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    def setup_method(self, method):
        MasterAlertSettingModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        MasterAlertSettingModel(
            id="b7aca788-82d6-4f98-80f5-e543a1810966",
            data_type=MasterDataType.ALERT_SETTING.value,
            name="工数アラート設定1",
            attributes=AlertSettingAttribute(
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
                display_setting={},
            ),
            create_id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
            create_at=datetime(2022, 6, 16, 17, 13, 29, 874000),
            update_id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
            update_at=datetime(2022, 6, 16, 17, 13, 29, 874000),
        ).save()

    def teardown_method(self, method):
        MasterAlertSettingModel.delete_table()

    @pytest.mark.parametrize(
        "version, body, expected",
        [
            (
                1,
                {
                    "factorSetting": [
                        {
                            "serviceTypeId": "service_1",
                            "directSupportManHour": 10,
                            "directAndPreSupportManHour": 15,
                            "preSupportManHour": 5,
                            "hourlyManHourPrice": 5,
                            "monthlyProfit": 0,
                        },
                        {
                            "serviceTypeId": "service_2",
                            "directSupportManHour": 15,
                            "directAndPreSupportManHour": 20,
                            "preSupportManHour": 0,
                            "hourlyManHourPrice": 5,
                            "monthlyProfit": 5,
                        },
                    ],
                    "displaySetting": {},
                },
                {
                    "id": "b7aca788-82d6-4f98-80f5-e543a1810966",
                    "name": "工数アラート設定1",
                    "attributes": {
                        "factorSetting": [
                            {
                                "serviceTypeId": "service_1",
                                "directSupportManHour": 10,
                                "directAndPreSupportManHour": 15,
                                "preSupportManHour": 5,
                                "hourlyManHourPrice": 5,
                                "monthlyProfit": 0,
                            },
                            {
                                "serviceTypeId": "service_2",
                                "directSupportManHour": 15,
                                "directAndPreSupportManHour": 20,
                                "preSupportManHour": 0,
                                "hourlyManHourPrice": 5,
                                "monthlyProfit": 5,
                            },
                        ],
                        "displaySetting": None,
                    },
                    "createId": "2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    "createAt": "2022-06-16T17:13:29.874+09:00",
                    "updateId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "version": 2,
                },
            )
        ],
    )
    def test_ok(self, mock_auth_admin, version, body, expected):
        """工数アラート設定の更新成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            f"/api/masters/alert-setting?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        actual.pop("updateAt")

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "role, access_flag",
        [
            (UserRoleType.SALES.key, False),
            (UserRoleType.SUPPORTER_MGR.key, False),
            (UserRoleType.SALES_MGR.key, False),
            (UserRoleType.SURVEY_OPS.key, False),
            (UserRoleType.MAN_HOUR_OPS.key, True),
        ],
    )
    def test_auth_ok(self, mock_auth_admin, role, access_flag):
        """ロール別権限テスト"""
        mock_auth_admin([role])

        version = 1
        body = {
            "factorSetting": [
                {
                    "serviceTypeId": "service_1",
                    "directSupportManHour": 10,
                    "directAndPreSupportManHour": 15,
                    "preSupportManHour": 5,
                    "hourlyManHourPrice": 5,
                    "monthlyProfit": 0,
                },
                {
                    "serviceTypeId": "service_2",
                    "directSupportManHour": 15,
                    "directAndPreSupportManHour": 20,
                    "preSupportManHour": 0,
                    "hourlyManHourPrice": 5,
                    "monthlyProfit": 5,
                },
            ],
            "displaySetting": {},
        }

        response = client.put(
            f"/api/masters/alert-setting?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        if access_flag:
            assert response.status_code == 200
        else:
            assert response.status_code == 403

    @pytest.mark.parametrize(
        "version, body",
        [
            (
                2,
                {
                    "factorSetting": [
                        {
                            "serviceTypeId": "service_1",
                            "directSupportManHour": 10,
                            "directAndPreSupportManHour": 15,
                            "preSupportManHour": 0,
                            "hourlyManHourPrice": 0,
                            "monthlyProfit": 0,
                        },
                        {
                            "serviceTypeId": "service_2",
                            "directSupportManHour": 15,
                            "directAndPreSupportManHour": 20,
                            "preSupportManHour": 0,
                            "hourlyManHourPrice": 0,
                            "monthlyProfit": 0,
                        },
                    ],
                    "displaySetting": {},
                },
            )
        ],
    )
    def test_conflict(self, mock_auth_admin, version, body):
        """楽観ロック制御のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            f"/api/masters/alert-setting?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 409

    @pytest.mark.parametrize(
        "version, body",
        [
            (
                1,
                {
                    "factorSetting": [
                        {
                            "serviceTypeId": "service_1",
                            "directSupportManHour": 10,
                            "directAndPreSupportManHour": 15,
                            "preSupportManHour": 0,
                            "hourlyManHourPrice": 0,
                            "monthlyProfit": 0,
                        },
                        {
                            "serviceTypeId": "service_2",
                            "directSupportManHour": 15,
                            "directAndPreSupportManHour": 20,
                            "preSupportManHour": 0,
                            "hourlyManHourPrice": 0,
                            "monthlyProfit": 0,
                        },
                    ],
                    "displaySetting": {},
                },
            )
        ],
    )
    def test_alert_setting_not_exist(self, mock_auth_admin, version, body):
        """工数アラート設定が存在しない場合"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        MasterAlertSettingModel.delete_table()
        MasterAlertSettingModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        response = client.put(
            f"/api/masters/alert-setting?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 404

    @pytest.mark.parametrize(
        "body",
        [
            # ###########################
            # factorSetting
            # ###########################
            # 必須チェック
            {
                "displaySetting": {},
            },
            # Noneチェック
            {
                "factorSetting": None,
                "displaySetting": {},
            },
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        version = 1

        response = client.put(
            f"/api/masters/alert-setting?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )
        assert response.status_code == 422
