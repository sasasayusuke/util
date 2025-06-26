import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.man_hour import ManHourSupporterOrganizationSummaryModel
from app.resources.const import UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetSummarySupporterOrganizationsManHours:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.MAN_HOUR_OPS.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_access_ok(
        self,
        mock_auth_admin,
        role_types,
    ):
        mock_auth_admin(role_types)

        ManHourSupporterOrganizationSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        year = 2021
        month = 7
        response = client.get(f"/api/man-hours/summary/supporter-organizations?year={year}&month={month}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

        ManHourSupporterOrganizationSummaryModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, model_list, expected",
        [
            (
                2021,
                7,
                [
                    ManHourSupporterOrganizationSummaryModel(
                        data_type="supporter_organization_summary#89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        year_month="2021/07",
                        supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        supporter_organization_name="ソニー株式会社1",
                        annual_sales=1,
                        monthly_sales=1,
                        monthly_project_price=1,
                        monthly_contract_time=1,
                        monthly_work_time=1,
                        monthly_work_time_rate=1,
                        monthly_supporters=1,
                        monthly_man_hour=1,
                        monthly_occupancy_rate=1,
                        monthly_occupancy_total_time=1,
                        monthly_occupancy_total_rate=1,
                    ),
                    ManHourSupporterOrganizationSummaryModel(
                        data_type="supporter_organization_summary#89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                        year_month="2021/07",
                        supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                        supporter_organization_name="ソニー株式会社2",
                        annual_sales=1,
                        monthly_sales=1,
                        monthly_project_price=1,
                        monthly_contract_time=1,
                        monthly_work_time=1,
                        monthly_work_time_rate=1,
                        monthly_supporters=1,
                        monthly_man_hour=1,
                        monthly_occupancy_rate=1,
                        monthly_occupancy_total_time=1,
                        monthly_occupancy_total_rate=1,
                    ),
                    ManHourSupporterOrganizationSummaryModel(
                        data_type="supporter_organization_summary#89cbe2ed-f44c-4a1c-9408-c67b0ca2270f",
                        year_month="2021/04",
                        supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270f",
                        supporter_organization_name="ソニー株式会社3",
                        annual_sales=1,
                        monthly_sales=1,
                        monthly_project_price=1,
                        monthly_contract_time=1,
                        monthly_work_time=1,
                        monthly_work_time_rate=1,
                        monthly_supporters=1,
                        monthly_man_hour=1,
                        monthly_occupancy_rate=1,
                        monthly_occupancy_total_time=1,
                        monthly_occupancy_total_rate=1,
                    ),
                ],
                [
                    {
                        "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "supporterOrganizationName": "ソニー株式会社1",
                        "annualSales": 1.0,
                        "monthlySales": 1.0,
                        "monthlyProjectPrice": 1.0,
                        "monthlyContractTime": 1.0,
                        "monthlyWorkTime": 1.0,
                        "monthlyWorkTimeRate": 100,
                        "monthlySupporters": 1.0,
                        "monthlyManHour": 1.0,
                        "monthlyOccupancyRate": 100,
                        "monthlyOccupancyTotalTime": 1.0,
                        "monthlyOccupancyTotalRate": 100,
                        "updateAt": "2022-06-24T17:56:28.983+09:00",
                    },
                    {
                        "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                        "supporterOrganizationName": "ソニー株式会社2",
                        "annualSales": 1.0,
                        "monthlySales": 1.0,
                        "monthlyProjectPrice": 1.0,
                        "monthlyContractTime": 1.0,
                        "monthlyWorkTime": 1.0,
                        "monthlyWorkTimeRate": 100,
                        "monthlySupporters": 1.0,
                        "monthlyManHour": 1.0,
                        "monthlyOccupancyRate": 100,
                        "monthlyOccupancyTotalTime": 1.0,
                        "monthlyOccupancyTotalRate": 100,
                        "updateAt": "2022-06-24T17:56:28.983+09:00",
                    },
                ],
            ),
        ],
    )
    def test_ok(self, mock_auth_admin, year, month, model_list, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        ManHourSupporterOrganizationSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for model in model_list:
            model.save()

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/supporter-organizations?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        expected[0]["updateAt"] = actual[0]["updateAt"]
        expected[1]["updateAt"] = actual[1]["updateAt"]
        assert response.status_code == 200
        assert actual == expected

        ManHourSupporterOrganizationSummaryModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, supporter_organization_id, model_list, expected",
        [
            (
                2021,
                7,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d,89cbe2ed-f44c-4a1c-9408-c67b0ca2270f",
                [
                    ManHourSupporterOrganizationSummaryModel(
                        data_type="supporter_organization_summary#89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        year_month="2021/07",
                        supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        supporter_organization_name="ソニー株式会社1",
                        annual_sales=1,
                        monthly_sales=1,
                        monthly_project_price=1,
                        monthly_contract_time=1,
                        monthly_work_time=1,
                        monthly_work_time_rate=1,
                        monthly_supporters=1,
                        monthly_man_hour=1,
                        monthly_occupancy_rate=1,
                        monthly_occupancy_total_time=1,
                        monthly_occupancy_total_rate=1,
                    ),
                    ManHourSupporterOrganizationSummaryModel(
                        data_type="supporter_organization_summary#89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                        year_month="2021/07",
                        supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                        supporter_organization_name="ソニー株式会社2",
                        annual_sales=1,
                        monthly_sales=1,
                        monthly_project_price=1,
                        monthly_contract_time=1,
                        monthly_work_time=1,
                        monthly_work_time_rate=1,
                        monthly_supporters=1,
                        monthly_man_hour=1,
                        monthly_occupancy_rate=1,
                        monthly_occupancy_total_time=1,
                        monthly_occupancy_total_rate=1,
                    ),
                    ManHourSupporterOrganizationSummaryModel(
                        data_type="supporter_organization_summary#89cbe2ed-f44c-4a1c-9408-c67b0ca2270f",
                        year_month="2021/07",
                        supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270f",
                        supporter_organization_name="ソニー株式会社3",
                        annual_sales=1,
                        monthly_sales=1,
                        monthly_project_price=1,
                        monthly_contract_time=1,
                        monthly_work_time=1,
                        monthly_work_time_rate=1,
                        monthly_supporters=1,
                        monthly_man_hour=1,
                        monthly_occupancy_rate=1,
                        monthly_occupancy_total_time=1,
                        monthly_occupancy_total_rate=1,
                    ),
                ],
                [
                    {
                        "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "supporterOrganizationName": "ソニー株式会社1",
                        "annualSales": 1.0,
                        "monthlySales": 1.0,
                        "monthlyProjectPrice": 1.0,
                        "monthlyContractTime": 1.0,
                        "monthlyWorkTime": 1.0,
                        "monthlyWorkTimeRate": 100,
                        "monthlySupporters": 1.0,
                        "monthlyManHour": 1.0,
                        "monthlyOccupancyRate": 100,
                        "monthlyOccupancyTotalTime": 1.0,
                        "monthlyOccupancyTotalRate": 100,
                        "updateAt": "2022-06-24T17:56:28.983+09:00",
                    },
                    {
                        "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270f",
                        "supporterOrganizationName": "ソニー株式会社3",
                        "annualSales": 1.0,
                        "monthlySales": 1.0,
                        "monthlyProjectPrice": 1.0,
                        "monthlyContractTime": 1.0,
                        "monthlyWorkTime": 1.0,
                        "monthlyWorkTimeRate": 100,
                        "monthlySupporters": 1.0,
                        "monthlyManHour": 1.0,
                        "monthlyOccupancyRate": 100,
                        "monthlyOccupancyTotalTime": 1.0,
                        "monthlyOccupancyTotalRate": 100,
                        "updateAt": "2022-06-24T17:56:28.983+09:00",
                    },
                ],
            ),
        ],
    )
    def test_query_supporter_organization_id(
        self,
        mock_auth_admin,
        year,
        month,
        supporter_organization_id,
        model_list,
        expected,
    ):
        """クエリパラメータ(supporter_organization_id)のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        ManHourSupporterOrganizationSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for model in model_list:
            model.save()

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/supporter-organizations?year={year}&month={month}&supporterOrganizationId={supporter_organization_id}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        expected[0]["updateAt"] = actual[0]["updateAt"]
        expected[1]["updateAt"] = actual[1]["updateAt"]
        assert response.status_code == 200
        assert actual == expected

        ManHourSupporterOrganizationSummaryModel.delete_table()

    def test_empty_ok(
        self,
        mock_auth_admin,
    ):
        """クエリパラメータ(supporter_organization_id)のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        ManHourSupporterOrganizationSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        # ###########################
        # テスト実行
        # ###########################
        year = 2021
        month = 7
        response = client.get(
            f"/api/man-hours/summary/supporter-organizations?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        expected = []

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

        ManHourSupporterOrganizationSummaryModel.delete_table()
