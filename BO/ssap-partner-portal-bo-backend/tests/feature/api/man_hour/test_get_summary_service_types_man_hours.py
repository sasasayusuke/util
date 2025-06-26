import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.man_hour import (
    ManHourProjectSummaryModel,
    ManHourServiceTypeSummaryModel,
    SupporterUsersAttribute,
)
from app.models.master import MasterSupporterOrganizationModel
from app.models.master_alert_setting import (
    AlertSettingAttribute,
    FactorSettingSubAttribute,
    MasterAlertSettingModel,
)
from app.resources.const import UserRoleType
from app.service.man_hour_service import ManHourService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetSummaryServiceTypesManHours:
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
        mocker
    ):
        mock_auth_admin(role_types)

        # アクセス権限
        mock = mocker.patch.object(ManHourService, "is_visible_man_hour")
        mock.return_value = True

        year = 2021
        month = 7
        response = client.get(f"/api/man-hours/summary/service-types?year={year}&month={month}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def setup_method(self, method):
        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        MasterAlertSettingModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        ManHourServiceTypeSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        MasterSupporterOrganizationModel(
            id="90cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
            data_type="master_service_type",
            name="サービスタイプ1",
            order=1,
            use=True,
        ).save()
        MasterSupporterOrganizationModel(
            id="90cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
            data_type="master_service_type",
            name="サービスタイプ2",
            order=2,
            use=True,
        ).save()

        MasterAlertSettingModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type="alert_setting",
            name="支援工数アラート設定",
            attributes=AlertSettingAttribute(
                factor_setting=[
                    FactorSettingSubAttribute(
                        service_type_id="90cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
                        direct_support_man_hour=10,
                        direct_and_pre_support_man_hour=10,
                    ),
                    FactorSettingSubAttribute(
                        service_type_id="90cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
                        direct_support_man_hour=20,
                        direct_and_pre_support_man_hour=20,
                    ),
                ]
            ),
        ).save()

        # 1番目と2番目はサービスタイプが同じで組織が別
        # 2番目と3番目はサービスタイプが別で組織が同じ
        ManHourProjectSummaryModel(
            data_type="project_summary#91cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
            year_month="2021/07",
            project_id="91cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
            project_name="プロジェクト1",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            customer_name="顧客1",
            supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
            service_type="90cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
            support_date_from="2021/05/30",
            support_date_to="2021/12/31",
            contract_type="有償",
            main_supporter_user=SupporterUsersAttribute(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                name="田中太郎",
                is_confirm=False,
            ),
            supporter_users=[
                SupporterUsersAttribute(
                    id="a9b67094-cdab-494c-818e-d4845088269c",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    name="山田太郎",
                    is_confirm=False,
                ),
                SupporterUsersAttribute(
                    id="a9b67094-cdab-494c-818e-d4845088269d",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    name="佐藤太郎",
                    is_confirm=False,
                ),
            ],
            total_contract_time=200,
            this_month_contract_time=40,
            total_profit=1000,
            this_month_profit=150,
            this_month_direct_support_man_hour=10,
            this_month_direct_support_man_hour_main=10,
            this_month_direct_support_man_hour_sub=20,
            this_month_pre_support_man_hour=30,
            summary_direct_support_man_hour=14,
            summary_pre_support_man_hour=15,
            summary_theoretical_direct_support_man_hour=16,
            summary_theoretical_pre_support_man_hour=17,
            this_month_theoretical_direct_support_man_hour=18,
            this_month_theoretical_pre_support_man_hour=19,
        ).save()
        ManHourProjectSummaryModel(
            data_type="project_summary#91cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
            year_month="2021/07",
            project_id="91cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
            project_name="プロジェクト2",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fg",
            customer_name="顧客2",
            supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
            service_type="90cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
            support_date_from="2021/05/30",
            support_date_to="2021/12/31",
            contract_type="有償",
            main_supporter_user=SupporterUsersAttribute(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                name="田中太郎",
                is_confirm=False,
            ),
            supporter_users=[
                SupporterUsersAttribute(
                    id="a9b67094-cdab-494c-818e-d4845088269c",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                    name="山田太郎",
                    is_confirm=False,
                ),
                SupporterUsersAttribute(
                    id="a9b67094-cdab-494c-818e-d4845088269d",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                    name="佐藤太郎",
                    is_confirm=False,
                ),
            ],
            total_contract_time=200,
            this_month_contract_time=41,
            total_profit=1000,
            this_month_profit=150,
            this_month_direct_support_man_hour=10,
            this_month_direct_support_man_hour_main=11,
            this_month_direct_support_man_hour_sub=21,
            this_month_pre_support_man_hour=31,
            summary_direct_support_man_hour=14,
            summary_pre_support_man_hour=15,
            summary_theoretical_direct_support_man_hour=16,
            summary_theoretical_pre_support_man_hour=17,
            this_month_theoretical_direct_support_man_hour=18,
            this_month_theoretical_pre_support_man_hour=19,
        ).save()
        ManHourProjectSummaryModel(
            data_type="project_summary#91cbe2ed-f44c-4a1c-9408-c67b0ca2270c",
            year_month="2021/07",
            project_id="91cbe2ed-f44c-4a1c-9408-c67b0ca2270c",
            project_name="プロジェクト3",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fh",
            customer_name="顧客3",
            supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
            service_type="90cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
            support_date_from="2021/05/30",
            support_date_to="2021/12/31",
            contract_type="有償",
            main_supporter_user=SupporterUsersAttribute(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                name="田中太郎",
                is_confirm=False,
            ),
            supporter_users=[
                SupporterUsersAttribute(
                    id="a9b67094-cdab-494c-818e-d4845088269c",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                    name="山田太郎",
                    is_confirm=False,
                ),
                SupporterUsersAttribute(
                    id="a9b67094-cdab-494c-818e-d4845088269d",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                    name="佐藤太郎",
                    is_confirm=False,
                ),
            ],
            total_contract_time=200,
            this_month_contract_time=42,
            total_profit=1000,
            this_month_profit=150,
            this_month_direct_support_man_hour=10,
            this_month_direct_support_man_hour_main=12,
            this_month_direct_support_man_hour_sub=22,
            this_month_pre_support_man_hour=32,
            summary_direct_support_man_hour=14,
            summary_pre_support_man_hour=15,
            summary_theoretical_direct_support_man_hour=16,
            summary_theoretical_pre_support_man_hour=17,
            this_month_theoretical_direct_support_man_hour=18,
            this_month_theoretical_pre_support_man_hour=19,
        ).save()

        ManHourServiceTypeSummaryModel(
            data_type="service_type_summary#90cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
            year_month="2021/07",
            service_type_id="90cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
            service_type_name="サービスタイプ1",
            this_month_contract_time=100,
            factor=5,
        ).save()
        ManHourServiceTypeSummaryModel(
            data_type="service_type_summary#90cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
            year_month="2021/07",
            service_type_id="90cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
            service_type_name="サービスタイプ2",
            this_month_contract_time=200,
            factor=10,
        ).save()

    def teardown_method(self, method):
        MasterSupporterOrganizationModel.delete_table()
        MasterAlertSettingModel.delete_table()
        ManHourProjectSummaryModel.delete_table()
        ManHourServiceTypeSummaryModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, expected",
        [
            (
                2021,
                7,
                [
                    {
                        "serviceTypeId": "90cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
                        "serviceTypeName": "サービスタイプ1",
                        "directSupportManHourFactor": 10,
                        "projects": [
                            {
                                "projectId": "91cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
                                "projectName": "プロジェクト2",
                                "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0fg",
                                "contractType": "有償",
                                "thisMonthDirectSupportManHourMain": 11,
                                "thisMonthDirectSupportManHourSub": 21,
                                "thisMonthPreSupportManHour": 31,
                                "thisMonthContractTime": 41,
                                "totalProcessYPercent": 205,
                            },
                            {
                                "projectId": "91cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
                                "projectName": "プロジェクト1",
                                "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                                "contractType": "有償",
                                "thisMonthDirectSupportManHourMain": 10,
                                "thisMonthDirectSupportManHourSub": 20,
                                "thisMonthPreSupportManHour": 30,
                                "thisMonthContractTime": 40,
                                "totalProcessYPercent": 200,
                            },
                        ],
                    },
                    {
                        "serviceTypeId": "90cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
                        "serviceTypeName": "サービスタイプ2",
                        "directSupportManHourFactor": 20,
                        "projects": [
                            {
                                "projectId": "91cbe2ed-f44c-4a1c-9408-c67b0ca2270c",
                                "projectName": "プロジェクト3",
                                "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0fh",
                                "contractType": "有償",
                                "thisMonthDirectSupportManHourMain": 12,
                                "thisMonthDirectSupportManHourSub": 22,
                                "thisMonthPreSupportManHour": 32,
                                "thisMonthContractTime": 42,
                                "totalProcessYPercent": 420,
                            }
                        ],
                    },
                ],
            ),
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        year,
        month,
        expected,
    ):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        mock = mocker.patch.object(ManHourService, "is_visible_man_hour")
        mock.return_value = True

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/service-types?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "year, month, expected",
        [
            (
                2021,
                7,
                [
                    {
                        "serviceTypeId": "90cbe2ed-f44c-4a1c-9408-c67b0ca2270a",
                        "serviceTypeName": "サービスタイプ1",
                        "directSupportManHourFactor": 10,
                        "projects": [],
                    },
                    {
                        "serviceTypeId": "90cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
                        "serviceTypeName": "サービスタイプ2",
                        "directSupportManHourFactor": 20,
                        "projects": [],
                    },
                ],
            ),
        ],
    )
    def test_supporter_mgr_empty(
        self,
        mocker,
        mock_auth_admin,
        year,
        month,
        expected,
    ):
        """支援者責任者のアクセス制御テスト"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        # ###########################
        # モック化
        # ###########################
        mock = mocker.patch.object(ManHourService, "is_visible_man_hour")
        mock.return_value = False

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/service-types?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
