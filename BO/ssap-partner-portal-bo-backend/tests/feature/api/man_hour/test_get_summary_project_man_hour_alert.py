import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.man_hour import (
    ManHourProjectSummaryModel,
    SupporterUsersAttribute,
    SupportManHourAttribute,
)
from app.resources.const import UserRoleType
from app.service.man_hour_service import ManHourService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetSummaryProjectManHourAlert:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
            [UserRoleType.SALES_MGR.key],
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
        year = 2021
        month = 7
        project_id = "128a91f1-2239-4a2e-ae0a-b7915eb932a7"
        response = client.get(f"/api/man-hours/summary/project-man-hour-alert?year={year}&month={month}&projectId={project_id}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def setup_method(self, method):
        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

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
            this_month_supporter_direct_support_man_hours=[
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269c",
                    input_man_hour=10,
                ),
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    input_man_hour=10,
                ),
            ],
            this_month_supporter_pre_support_man_hours=[
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    input_man_hour=10,
                ),
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269c",
                    input_man_hour=10,
                ),
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269d",
                    input_man_hour=5,
                ),
            ],
            summary_supporter_direct_support_man_hours=[
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    input_man_hour=30,
                ),
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269c",
                    input_man_hour=20,
                ),
            ],
            summary_supporter_pre_support_man_hours=[
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    input_man_hour=20,
                ),
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269c",
                    input_man_hour=10,
                ),
                SupportManHourAttribute(
                    supporter_user_id="a9b67094-cdab-494c-818e-d4845088269d",
                    input_man_hour=5,
                ),
            ],
        ).save()

    def teardown_method(self, method):
        ManHourProjectSummaryModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, project_id, expected",
        [
            (
                2021,
                7,
                "128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                {
                    "projectId": "128a91f1-2239-4a2e-ae0a-b7915eb932a7",
                    "projectName": "プロジェクト1",
                    "customerId": "3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    "customerName": "顧客1",
                    "manHours": [
                        {
                            "supporterUserId": "a9b67094-cdab-494c-818e-d4845088269b",
                            "supporterUserName": "田中太郎",
                            "role": "プロデューサー",
                            "thisMonthSupporterDirectSupportManHours": 10,
                            "thisMonthSupporterPreSupportManHours": 10,
                            "summarySupporterDirectSupportManHours": 30,
                            "summarySupporterPreSupportManHours": 20,
                        },
                        {
                            "supporterUserId": "a9b67094-cdab-494c-818e-d4845088269c",
                            "supporterUserName": "山田太郎",
                            "role": "アクセラレーター",
                            "thisMonthSupporterDirectSupportManHours": 10,
                            "thisMonthSupporterPreSupportManHours": 10,
                            "summarySupporterDirectSupportManHours": 20,
                            "summarySupporterPreSupportManHours": 10,
                        },
                        {
                            "supporterUserId": "a9b67094-cdab-494c-818e-d4845088269d",
                            "supporterUserName": "佐藤太郎",
                            "role": "アクセラレーター",
                            "thisMonthSupporterDirectSupportManHours": None,
                            "thisMonthSupporterPreSupportManHours": 5,
                            "summarySupporterDirectSupportManHours": None,
                            "summarySupporterPreSupportManHours": 5,
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, year, month, project_id, expected):
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
            f"/api/man-hours/summary/project-man-hour-alert?year={year}&month={month}&projectId={project_id}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "year, month, project_id",
        [
            (
                2021,
                7,
                "128a91f1-2239-4a2e-ae0a-b7915eb932a7",
            ),
        ],
    )
    def test_supporter_mgr_403(self, mocker, mock_auth_admin, year, month, project_id):
        """正常系のテスト"""
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
            f"/api/man-hours/summary/project-man-hour-alert?year={year}&month={month}&projectId={project_id}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 403
