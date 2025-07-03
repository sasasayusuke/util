from datetime import datetime

import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.admin import AdminModel
from app.models.man_hour import (ManHourProjectSummaryModel,
                                 SupporterUsersAttribute)
from app.models.master import MasterSupporterOrganizationModel
from app.models.project import (GrossProfitAttribute, ProjectModel,
                                SalesforceMainCustomerAttribute)
from app.models.user import UserModel
from app.resources.const import DataType, MasterDataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetMonthlyProjects:
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
        response = client.get(
            "/api/projects/summary/2024/01", headers=REQUEST_HEADERS
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SALES.key],
            [UserRoleType.SURVEY_OPS.key],
        ]
    )
    def test_not_access(
        self,
        mock_auth_admin,
        role_types,
    ):
        """権限のないロールはアクセス不可"""
        mock_auth_admin(role_types)
        response = client.get(
            "/api/projects/summary/2024/01", headers=REQUEST_HEADERS
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "man_hour_models, project_models, expected, master_models, user_models",
        [
            (
                [
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトA",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="58541ec4-fbd8-46bd-b357-ab1c352d2adf",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="有償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトB",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="有償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトC",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="有償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトC",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="無償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                ],
                [
                    ProjectModel(
                        id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                        salesforce_update_at="2020/10/23 03:21",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        name="テストプロジェクトA",
                        customer_name="株式会社アアアアア",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="2b2b6bb8-7743-438e-b233-e72baf850eaa",
                        contract_date="2021/01/30",
                        phase="プラン提示（D）",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["7a10e814-fd3f-4476-b485-21d230cad914"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                {
                    "header": [
                        {
                            "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                            "supporterOrganizationName": "IST",
                        },
                        {
                            "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                            "supporterOrganizationName": "AST",
                        },
                    ],
                    "details": [
                        {
                            "serviceTypeName": "アイデア可視化",
                            "name": "株式会社アアアアア／テストプロジェクトB",
                            "contractType": "有償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                        {
                            "serviceTypeName": "アイデア可視化",
                            "name": "株式会社アアアアア／テストプロジェクトC",
                            "contractType": "有償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                        {
                            "serviceTypeName": "アイデア可視化",
                            "name": "株式会社アアアアア／テストプロジェクトC",
                            "contractType": "無償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                        {
                            "serviceTypeName": "組織開発",
                            "name": "株式会社アアアアア／テストプロジェクトA",
                            "contractType": "有償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                    ],
                },
                [
                    MasterSupporterOrganizationModel(
                        id="58541ec4-fbd8-46bd-b357-ab1c352d2adf",
                        data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                        name="組織開発",
                        value=None,
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                        name="アイデア可視化",
                        value=None,
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Startup Marketing Team",
                        value="SMT",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="f794e69d-f056-41db-8dbb-759809a80108",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Ideation Service Team",
                        value="IST",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Acceleration Service Team",
                        value="AST",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
                [
                    UserModel(
                        id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                        data_type=DataType.USER,
                        name="ソニー 太郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                        data_type=DataType.USER,
                        name="ソニー 次郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                        data_type=DataType.USER,
                        name="ソニー 三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="2b2b6bb8-7743-438e-b233-e72baf850eaa",
                        data_type=DataType.USER,
                        name="セールス 三郎",
                        email="user@example.com",
                        role=UserRoleType.SALES_MGR.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=None,
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        man_hour_models,
        project_models,
        expected,
        master_models,
        user_models,
    ):
        """正常確認.
        支援者責任者：自分の課の案件のみ参照可.
        """
        year = "2022"
        month = "06"

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock_man_hour = mocker.patch.object(
            ManHourProjectSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour.return_value = man_hour_models
        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )
        mock_master_service = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_value_index, "query"
        )
        mock_master_service.return_value = master_models
        mock_master_supporter = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_order_index, "query"
        )
        mock_master_supporter.return_value = master_models
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models

        mock_project = mocker.patch.object(ProjectModel, "scan")
        mock_project.return_value = project_models

        response = client.get(
            f"/api/projects/summary/{year}/{month}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "man_hour_models, project_models, expected, master_models, user_models",
        [
            (
                [
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトA",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="58541ec4-fbd8-46bd-b357-ab1c352d2adf",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="有償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトB",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="有償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトC",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="有償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトC",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="無償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                ],
                [
                    ProjectModel(
                        id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                        salesforce_update_at="2020/10/23 03:21",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        name="テストプロジェクトA",
                        customer_name="株式会社アアアアア",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="2b2b6bb8-7743-438e-b233-e72baf850eaa",
                        contract_date="2021/01/30",
                        phase="プラン提示（D）",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["7a10e814-fd3f-4476-b485-21d230cad914"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                {
                    "header": [
                        {
                            "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                            "supporterOrganizationName": "IST",
                        },
                        {
                            "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                            "supporterOrganizationName": "AST",
                        },
                    ],
                    "details": [
                        {
                            "serviceTypeName": "アイデア可視化",
                            "name": "株式会社アアアアア／テストプロジェクトB",
                            "contractType": "有償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                        {
                            "serviceTypeName": "アイデア可視化",
                            "name": "株式会社アアアアア／テストプロジェクトC",
                            "contractType": "有償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                        {
                            "serviceTypeName": "アイデア可視化",
                            "name": "株式会社アアアアア／テストプロジェクトC",
                            "contractType": "無償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                        {
                            "serviceTypeName": "組織開発",
                            "name": "株式会社アアアアア／テストプロジェクトA",
                            "contractType": "有償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                    ],
                },
                [
                    MasterSupporterOrganizationModel(
                        id="58541ec4-fbd8-46bd-b357-ab1c352d2adf",
                        data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                        name="組織開発",
                        value=None,
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                        name="アイデア可視化",
                        value=None,
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Startup Marketing Team",
                        value="SMT",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="f794e69d-f056-41db-8dbb-759809a80108",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Ideation Service Team",
                        value="IST",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Acceleration Service Team",
                        value="AST",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
                [
                    UserModel(
                        id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                        data_type=DataType.USER,
                        name="ソニー 太郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                        data_type=DataType.USER,
                        name="ソニー 次郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                        data_type=DataType.USER,
                        name="ソニー 三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="2b2b6bb8-7743-438e-b233-e72baf850eaa",
                        data_type=DataType.USER,
                        name="セールス 三郎",
                        email="user@example.com",
                        role=UserRoleType.SALES_MGR.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=None,
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_visible_ok_supporter_mgr(
        self,
        mocker,
        mock_auth_admin,
        man_hour_models,
        project_models,
        expected,
        master_models,
        user_models,
    ):
        year = "2022"
        month = "06"

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SUPPORTER_MGR.key},
            company=None,
            job="部長",
            supporter_organization_id={"f59023c8-417e-47a5-8984-d346d40a9f1e"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token="111111",
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_man_hour = mocker.patch.object(
            ManHourProjectSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour.return_value = man_hour_models

        mock_master_service = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_value_index, "query"
        )
        mock_master_service.return_value = master_models
        mock_master_supporter = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_order_index, "query"
        )
        mock_master_supporter.return_value = master_models
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models

        mock_project = mocker.patch.object(ProjectModel, "scan")
        mock_project.return_value = project_models

        response = client.get(
            f"/api/projects/summary/{year}/{month}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "man_hour_models, project_models, expected, master_models, user_models",
        [
            (
                [
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトA",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="58541ec4-fbd8-46bd-b357-ab1c352d2adf",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="有償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトB",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="ac0caef9-afbb-4d62-84c0-7308832a5818",
                        service_type="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="有償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトC",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="ac0caef9-afbb-4d62-84c0-7308832a5818",
                        service_type="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="有償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                    ManHourProjectSummaryModel(
                        data_type="project_summary#c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        year_month="2022/06",
                        project_id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        project_name="テストプロジェクトC",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        customer_name="株式会社アアアアア",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        service_type="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        support_date_from="2021/10/01",
                        support_date_to="2022/04/30",
                        contract_type="無償",
                        main_supporter_user=SupporterUsersAttribute(
                            id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                            organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                            name="ソニー 太郎",
                            is_confirm=True,
                        ),
                        supporter_users=[
                            SupporterUsersAttribute(
                                id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                                organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                                name="ソニー 次郎",
                                is_confirm=False,
                            ),
                            SupporterUsersAttribute(
                                id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                                organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                                name="ソニー 三郎",
                                is_confirm=True,
                            ),
                        ],
                        total_contract_time=300.8,
                        this_month_contract_time=100.5,
                        total_profit=2000000,
                        this_month_profit=500000.5,
                        create_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                        update_at=datetime(2022, 6, 24, 17, 5, 37, 186082),
                    ),
                ],
                [
                    ProjectModel(
                        id="c3e0c9f2-0554-4d09-b9f3-740f7d846791",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                        salesforce_update_at="2020/10/23 03:21",
                        customer_id="6f8560be-efd3-44dd-b5f7-0bf059b9061d",
                        name="テストプロジェクトA",
                        customer_name="株式会社アアアアア",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="2b2b6bb8-7743-438e-b233-e72baf850eaa",
                        contract_date="2021/01/30",
                        phase="プラン提示（D）",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                        supporter_organization_id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["7a10e814-fd3f-4476-b485-21d230cad914"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                {
                    "header": [
                        {
                            "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                            "supporterOrganizationName": "IST",
                        },
                        {
                            "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                            "supporterOrganizationName": "AST",
                        },
                    ],
                    "details": [
                        {
                            "serviceTypeName": "アイデア可視化",
                            "name": "株式会社アアアアア／テストプロジェクトC",
                            "contractType": "無償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                        {
                            "serviceTypeName": "組織開発",
                            "name": "株式会社アアアアア／テストプロジェクトA",
                            "contractType": "有償",
                            "mainSalesUserName": "セールス 三郎",
                            "organizations": [
                                {
                                    "supporterOrganizationId": "f794e69d-f056-41db-8dbb-759809a80108",
                                    "supporterOrganizationName": "IST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 太郎",
                                            "role": "プロデューサー",
                                            "isConfirm": True,
                                        },
                                        {
                                            "supporterName": "ソニー 次郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": False,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "9a48a37e-106d-4083-9d37-e22c18fb819c",
                                    "supporterOrganizationName": "AST",
                                    "members": [
                                        {
                                            "supporterName": "ソニー 三郎",
                                            "role": "アクセラレーター",
                                            "isConfirm": True,
                                        },
                                    ],
                                },
                            ],
                            "supportDateFrom": "2021/10/01",
                            "supportDateTo": "2022/04/30",
                            "totalContractTime": 300.8,
                            "thisMonthContractTime": 100.5,
                            "totalProfit": 2000000,
                            "thisMonthProfit": 500000.5,
                        },
                    ],
                },
                [
                    MasterSupporterOrganizationModel(
                        id="58541ec4-fbd8-46bd-b357-ab1c352d2adf",
                        data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                        name="組織開発",
                        value=None,
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                        name="アイデア可視化",
                        value=None,
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Startup Marketing Team",
                        value="SMT",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="f794e69d-f056-41db-8dbb-759809a80108",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Ideation Service Team",
                        value="IST",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Acceleration Service Team",
                        value="AST",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
                [
                    UserModel(
                        id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                        data_type=DataType.USER,
                        name="ソニー 太郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                        data_type=DataType.USER,
                        name="ソニー 次郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                        data_type=DataType.USER,
                        name="ソニー 三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="2b2b6bb8-7743-438e-b233-e72baf850eaa",
                        data_type=DataType.USER,
                        name="セールス 三郎",
                        email="user@example.com",
                        role=UserRoleType.SALES_MGR.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=None,
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_visible_only_supporter_mgr(
        self,
        mocker,
        mock_auth_admin,
        man_hour_models,
        project_models,
        expected,
        master_models,
        user_models,
    ):
        year = "2022"
        month = "06"

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SUPPORTER_MGR.key},
            company=None,
            job="部長",
            supporter_organization_id={"f59023c8-417e-47a5-8984-d346d40a9f1e"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token="111111",
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_man_hour = mocker.patch.object(
            ManHourProjectSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour.return_value = man_hour_models

        mock_master_service = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_value_index, "query"
        )
        mock_master_service.return_value = master_models
        mock_master_supporter = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_order_index, "query"
        )
        mock_master_supporter.return_value = master_models
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models

        mock_project = mocker.patch.object(ProjectModel, "scan")
        mock_project.return_value = project_models

        response = client.get(
            f"/api/projects/summary/{year}/{month}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "man_hour_models, project_models, expected, master_models, user_models",
        [
            (
                [],
                [],
                {
                    "header": [],
                    "details": [],
                },
                [
                    MasterSupporterOrganizationModel(
                        id="58541ec4-fbd8-46bd-b357-ab1c352d2adf",
                        data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                        name="組織開発",
                        value=None,
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="828d7a1b-7cc2-429e-bf93-79427c83ab07",
                        data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                        name="アイデア可視化",
                        value=None,
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="f59023c8-417e-47a5-8984-d346d40a9f1e",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Startup Marketing Team",
                        value="SMT",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="f794e69d-f056-41db-8dbb-759809a80108",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Ideation Service Team",
                        value="IST",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    MasterSupporterOrganizationModel(
                        id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                        data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                        name="Acceleration Service Team",
                        value="AST",
                        attributes=None,
                        order=1,
                        use=True,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
                [
                    UserModel(
                        id="83e3e580-c48a-4bfb-8acc-2312de4f7114",
                        data_type=DataType.USER,
                        name="ソニー 太郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="a5ef86a2-95b1-47ae-ae04-1664b5f0fe56",
                        data_type=DataType.USER,
                        name="ソニー 次郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="f794e69d-f056-41db-8dbb-759809a80108",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="560946f4-861b-4a01-bf4c-3679b90c4de7",
                        data_type=DataType.USER,
                        name="ソニー 三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id="9a48a37e-106d-4083-9d37-e22c18fb819c",
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    UserModel(
                        id="2b2b6bb8-7743-438e-b233-e72baf850eaa",
                        data_type=DataType.USER,
                        name="セールス 三郎",
                        email="user@example.com",
                        role=UserRoleType.SALES_MGR.key,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=None,
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_non_data(
        self,
        mocker,
        mock_auth_admin,
        man_hour_models,
        project_models,
        expected,
        master_models,
        user_models,
    ):
        year = "2022"
        month = "06"

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_man_hour = mocker.patch.object(
            ManHourProjectSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour.return_value = man_hour_models

        mock_master_service = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_value_index, "query"
        )
        mock_master_service.return_value = master_models
        mock_master_supporter = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_order_index, "query"
        )
        mock_master_supporter.return_value = master_models
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models

        mock_project = mocker.patch.object(ProjectModel, "scan")
        mock_project.return_value = project_models

        response = client.get(
            f"/api/projects/summary/{year}/{month}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
