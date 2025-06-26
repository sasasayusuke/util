import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.man_hour import ManHourProjectSummaryModel, SupporterUsersAttribute
from app.resources.const import UserRoleType
from app.service.project_service import ProjectService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetSummaryProjectManHourAlerts:
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

        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        year = 2021
        month = 7
        response = client.get(f"/api/man-hours/summary/project-man-hour-alerts?year={year}&month={month}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

        ManHourProjectSummaryModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, model_list, expected",
        [
            (
                2021,
                7,
                [
                    # 1番目と2番目はサービスタイプが同じで組織が別
                    # 2番目と3番目はサービスタイプが別で組織が同じ
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                        project_name="プロジェクト1",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_name="顧客1",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                        project_name="プロジェクト2",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fg",
                        customer_name="顧客2",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                        project_name="プロジェクト3",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fh",
                        customer_name="顧客3",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad59",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                ],
                {
                    "summaryThisMonthContractTime": 90,
                    "projects": [
                        {
                            "projectId": "128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                            "projectName": "プロジェクト1",
                            "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                            "customerName": "顧客1",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "モック用組織",
                            "serviceType": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "モック用サービス",
                            "supportDateFrom": "2021/05/30",
                            "supportDateTo": "2021/12/31",
                            "totalContractTime": 200,
                            "thisMonthContractTime": 30,
                            "totalProfit": 1000,
                            "thisMonthProfit": 150,
                            "mainSupporterUser": {
                                "id": "a9b67094-cdab-494c-818e-d4845088269b",
                                "name": "田中太郎",
                            },
                            "supporterUsers": [
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269c",
                                    "name": "山田太郎",
                                },
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269d",
                                    "name": "佐藤太郎",
                                },
                            ],
                            "summaryDirectSupportManHour": 14,
                            "summaryPreSupportManHour": 15,
                            "thisMonthDirectSupportManHour": 10,
                            "thisMonthPreSupportManHour": 13,
                            "summaryTheoreticalDirectSupportManHour": 16,
                            "summaryTheoreticalPreSupportManHour": 17,
                            "thisMonthTheoreticalDirectSupportManHour": 18,
                            "thisMonthTheoreticalPreSupportManHour": 19,
                        },
                        {
                            "projectId": "128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                            "projectName": "プロジェクト2",
                            "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0fg",
                            "customerName": "顧客2",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e3",
                            "supporterOrganizationName": "モック用組織",
                            "serviceType": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "モック用サービス",
                            "supportDateFrom": "2021/05/30",
                            "supportDateTo": "2021/12/31",
                            "totalContractTime": 200,
                            "thisMonthContractTime": 30,
                            "totalProfit": 1000,
                            "thisMonthProfit": 150,
                            "mainSupporterUser": {
                                "id": "a9b67094-cdab-494c-818e-d4845088269b",
                                "name": "田中太郎",
                            },
                            "supporterUsers": [
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269c",
                                    "name": "山田太郎",
                                },
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269d",
                                    "name": "佐藤太郎",
                                },
                            ],
                            "summaryDirectSupportManHour": 14,
                            "summaryPreSupportManHour": 15,
                            "thisMonthDirectSupportManHour": 10,
                            "thisMonthPreSupportManHour": 13,
                            "summaryTheoreticalDirectSupportManHour": 16,
                            "summaryTheoreticalPreSupportManHour": 17,
                            "thisMonthTheoreticalDirectSupportManHour": 18,
                            "thisMonthTheoreticalPreSupportManHour": 19,
                        },
                        {
                            "projectId": "128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                            "projectName": "プロジェクト3",
                            "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0fh",
                            "customerName": "顧客3",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e3",
                            "supporterOrganizationName": "モック用組織",
                            "serviceType": "7ac8bddf-88da-46c9-a504-a03d1661ad59",
                            "serviceTypeName": "モック用サービス",
                            "supportDateFrom": "2021/05/30",
                            "supportDateTo": "2021/12/31",
                            "totalContractTime": 200,
                            "thisMonthContractTime": 30,
                            "totalProfit": 1000,
                            "thisMonthProfit": 150,
                            "mainSupporterUser": {
                                "id": "a9b67094-cdab-494c-818e-d4845088269b",
                                "name": "田中太郎",
                            },
                            "supporterUsers": [
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269c",
                                    "name": "山田太郎",
                                },
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269d",
                                    "name": "佐藤太郎",
                                },
                            ],
                            "summaryDirectSupportManHour": 14,
                            "summaryPreSupportManHour": 15,
                            "thisMonthDirectSupportManHour": 10,
                            "thisMonthPreSupportManHour": 13,
                            "summaryTheoreticalDirectSupportManHour": 16,
                            "summaryTheoreticalPreSupportManHour": 17,
                            "thisMonthTheoreticalDirectSupportManHour": 18,
                            "thisMonthTheoreticalPreSupportManHour": 19,
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, year, month, model_list, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        for model in model_list:
            model.save()

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "モック用組織"

        mock = mocker.patch.object(ProjectService, "cached_get_service_name")
        mock.return_value = "モック用サービス"

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/project-man-hour-alerts?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

        ManHourProjectSummaryModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, supporter_organization_id, model_list, expected",
        [
            (
                2021,
                7,
                "de40733f-6be9-4fef-8229-01052f43c1e2",
                [
                    # 1番目と2番目はサービスタイプが同じで組織が別
                    # 2番目と3番目はサービスタイプが別で組織が同じ
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                        project_name="プロジェクト1",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_name="顧客1",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                        project_name="プロジェクト2",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fg",
                        customer_name="顧客2",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                        project_name="プロジェクト3",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fh",
                        customer_name="顧客3",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad59",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                ],
                {
                    "summaryThisMonthContractTime": 30,
                    "projects": [
                        {
                            "projectId": "128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                            "projectName": "プロジェクト1",
                            "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                            "customerName": "顧客1",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "モック用組織",
                            "serviceType": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "モック用サービス",
                            "supportDateFrom": "2021/05/30",
                            "supportDateTo": "2021/12/31",
                            "totalContractTime": 200,
                            "thisMonthContractTime": 30,
                            "totalProfit": 1000,
                            "thisMonthProfit": 150,
                            "mainSupporterUser": {
                                "id": "a9b67094-cdab-494c-818e-d4845088269b",
                                "name": "田中太郎",
                            },
                            "supporterUsers": [
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269c",
                                    "name": "山田太郎",
                                },
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269d",
                                    "name": "佐藤太郎",
                                },
                            ],
                            "summaryDirectSupportManHour": 14,
                            "summaryPreSupportManHour": 15,
                            "thisMonthDirectSupportManHour": 10,
                            "thisMonthPreSupportManHour": 13,
                            "summaryTheoreticalDirectSupportManHour": 16,
                            "summaryTheoreticalPreSupportManHour": 17,
                            "thisMonthTheoreticalDirectSupportManHour": 18,
                            "thisMonthTheoreticalPreSupportManHour": 19,
                        },
                    ],
                },
            ),
        ],
    )
    def test_query_supporter_organization_id(
        self,
        mocker,
        mock_auth_admin,
        year,
        month,
        supporter_organization_id,
        model_list,
        expected,
    ):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        for model in model_list:
            model.save()

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "モック用組織"

        mock = mocker.patch.object(ProjectService, "cached_get_service_name")
        mock.return_value = "モック用サービス"

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/project-man-hour-alerts?year={year}&month={month}&supporterOrganizationId={supporter_organization_id}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

        ManHourProjectSummaryModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, service_type_id, model_list, expected",
        [
            (
                2021,
                7,
                "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                [
                    # 1番目と2番目はサービスタイプが同じで組織が別
                    # 2番目と3番目はサービスタイプが別で組織が同じ
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                        project_name="プロジェクト1",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_name="顧客1",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                        project_name="プロジェクト2",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fg",
                        customer_name="顧客2",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                        project_name="プロジェクト3",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fh",
                        customer_name="顧客3",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad59",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                ],
                {
                    "summaryThisMonthContractTime": 60,
                    "projects": [
                        {
                            "projectId": "128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                            "projectName": "プロジェクト1",
                            "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                            "customerName": "顧客1",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "モック用組織",
                            "serviceType": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "モック用サービス",
                            "supportDateFrom": "2021/05/30",
                            "supportDateTo": "2021/12/31",
                            "totalContractTime": 200,
                            "thisMonthContractTime": 30,
                            "totalProfit": 1000,
                            "thisMonthProfit": 150,
                            "mainSupporterUser": {
                                "id": "a9b67094-cdab-494c-818e-d4845088269b",
                                "name": "田中太郎",
                            },
                            "supporterUsers": [
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269c",
                                    "name": "山田太郎",
                                },
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269d",
                                    "name": "佐藤太郎",
                                },
                            ],
                            "summaryDirectSupportManHour": 14,
                            "summaryPreSupportManHour": 15,
                            "thisMonthDirectSupportManHour": 10,
                            "thisMonthPreSupportManHour": 13,
                            "summaryTheoreticalDirectSupportManHour": 16,
                            "summaryTheoreticalPreSupportManHour": 17,
                            "thisMonthTheoreticalDirectSupportManHour": 18,
                            "thisMonthTheoreticalPreSupportManHour": 19,
                        },
                        {
                            "projectId": "128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                            "projectName": "プロジェクト2",
                            "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0fg",
                            "customerName": "顧客2",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e3",
                            "supporterOrganizationName": "モック用組織",
                            "serviceType": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "モック用サービス",
                            "supportDateFrom": "2021/05/30",
                            "supportDateTo": "2021/12/31",
                            "totalContractTime": 200,
                            "thisMonthContractTime": 30,
                            "totalProfit": 1000,
                            "thisMonthProfit": 150,
                            "mainSupporterUser": {
                                "id": "a9b67094-cdab-494c-818e-d4845088269b",
                                "name": "田中太郎",
                            },
                            "supporterUsers": [
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269c",
                                    "name": "山田太郎",
                                },
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269d",
                                    "name": "佐藤太郎",
                                },
                            ],
                            "summaryDirectSupportManHour": 14,
                            "summaryPreSupportManHour": 15,
                            "thisMonthDirectSupportManHour": 10,
                            "thisMonthPreSupportManHour": 13,
                            "summaryTheoreticalDirectSupportManHour": 16,
                            "summaryTheoreticalPreSupportManHour": 17,
                            "thisMonthTheoreticalDirectSupportManHour": 18,
                            "thisMonthTheoreticalPreSupportManHour": 19,
                        },
                    ],
                },
            ),
        ],
    )
    def test_query_service_type_id(
        self,
        mocker,
        mock_auth_admin,
        year,
        month,
        service_type_id,
        model_list,
        expected,
    ):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        for model in model_list:
            model.save()

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "モック用組織"

        mock = mocker.patch.object(ProjectService, "cached_get_service_name")
        mock.return_value = "モック用サービス"

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/project-man-hour-alerts?year={year}&month={month}&serviceTypeId={service_type_id}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

        ManHourProjectSummaryModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, supporter_organization_id, service_type_id, model_list, expected",
        [
            (
                2021,
                7,
                "de40733f-6be9-4fef-8229-01052f43c1e3",
                "7ac8bddf-88da-46c9-a504-a03d1661ad59",
                [
                    # 1番目と2番目はサービスタイプが同じで組織が別
                    # 2番目と3番目はサービスタイプが別で組織が同じ
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                        project_name="プロジェクト1",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_name="顧客1",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a8",
                        project_name="プロジェクト2",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fg",
                        customer_name="顧客2",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                        year_month="2021/07",
                        project_id="128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                        project_name="プロジェクト3",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0fh",
                        customer_name="顧客3",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e3",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad59",
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
                        this_month_contract_time=30,
                        total_profit=1000,
                        this_month_profit=150,
                        this_month_direct_support_man_hour=10,
                        this_month_direct_support_man_hour_main=11,
                        this_month_direct_support_man_hour_sub=12,
                        this_month_pre_support_man_hour=13,
                        summary_direct_support_man_hour=14,
                        summary_pre_support_man_hour=15,
                        summary_theoretical_direct_support_man_hour=16,
                        summary_theoretical_pre_support_man_hour=17,
                        this_month_theoretical_direct_support_man_hour=18,
                        this_month_theoretical_pre_support_man_hour=19,
                    ),
                ],
                {
                    "summaryThisMonthContractTime": 30,
                    "projects": [
                        {
                            "projectId": "128a91f1-2239-4a2e-ae0a-b7915eb932a9",
                            "projectName": "プロジェクト3",
                            "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0fh",
                            "customerName": "顧客3",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e3",
                            "supporterOrganizationName": "モック用組織",
                            "serviceType": "7ac8bddf-88da-46c9-a504-a03d1661ad59",
                            "serviceTypeName": "モック用サービス",
                            "supportDateFrom": "2021/05/30",
                            "supportDateTo": "2021/12/31",
                            "totalContractTime": 200,
                            "thisMonthContractTime": 30,
                            "totalProfit": 1000,
                            "thisMonthProfit": 150,
                            "mainSupporterUser": {
                                "id": "a9b67094-cdab-494c-818e-d4845088269b",
                                "name": "田中太郎",
                            },
                            "supporterUsers": [
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269c",
                                    "name": "山田太郎",
                                },
                                {
                                    "id": "a9b67094-cdab-494c-818e-d4845088269d",
                                    "name": "佐藤太郎",
                                },
                            ],
                            "summaryDirectSupportManHour": 14,
                            "summaryPreSupportManHour": 15,
                            "thisMonthDirectSupportManHour": 10,
                            "thisMonthPreSupportManHour": 13,
                            "summaryTheoreticalDirectSupportManHour": 16,
                            "summaryTheoreticalPreSupportManHour": 17,
                            "thisMonthTheoreticalDirectSupportManHour": 18,
                            "thisMonthTheoreticalPreSupportManHour": 19,
                        },
                    ],
                },
            ),
        ],
    )
    def test_query_all(
        self,
        mocker,
        mock_auth_admin,
        year,
        month,
        supporter_organization_id,
        service_type_id,
        model_list,
        expected,
    ):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        for model in model_list:
            model.save()

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "モック用組織"

        mock = mocker.patch.object(ProjectService, "cached_get_service_name")
        mock.return_value = "モック用サービス"

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/project-man-hour-alerts?year={year}&month={month}&supporterOrganizationId={supporter_organization_id}&serviceTypeId={service_type_id}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

        ManHourProjectSummaryModel.delete_table()

    def test_empty_ok(self, mock_auth_admin):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        # ###########################
        # テスト実行
        # ###########################
        year = 2021
        month = 7
        response = client.get(
            f"/api/man-hours/summary/project-man-hour-alerts?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        expected = {"summaryThisMonthContractTime": 0, "projects": []}

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

        ManHourProjectSummaryModel.delete_table()
