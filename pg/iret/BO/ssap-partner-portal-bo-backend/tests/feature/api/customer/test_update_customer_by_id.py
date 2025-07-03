from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.customer import CustomerModel
from app.models.project import ProjectModel
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestUpdateCustomerById:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.SURVEY_OPS.key],
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
        response = client.put(
            "/api/customers/1?version=1",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 422

    def setup_method(self, method):
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        ProjectModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        UserModel.create_table(read_capacity_units=5, write_capacity_units=5, wait=True)

        CustomerModel(
            id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
            data_type=DataType.CUSTOMER,
            name="ソニーグループ株式会社",
            category="ソニーグループ",
            salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            salesforce_update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
            salesforce_target=None,
            salesforce_credit_limit=None,
            salesforce_credit_get_month=None,
            salesforce_credit_manager=None,
            salesforce_credit_no_retry=None,
            salesforce_paws_credit_number=None,
            salesforce_customer_owner=None,
            salesforce_customer_segment=None,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
        ).save()

        UserModel(
            id="a9b67094-cdab-494c-818e-d4845088269b",
            data_type=DataType.USER,
            name="田中太郎",
            email="user@example.com",
            role=UserRoleType.CUSTOMER.value[1],
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
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
        ).save()

        ProjectModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.PROJECT,
            customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
            salesforce_customer_id=None,
            salesforce_opportunity_id=None,
            salesforce_update_at=None,
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type=None,
            create_new=None,
            continued=None,
            main_sales_user_id="c67b0ca2270d",
            contract_date=None,
            phase=None,
            customer_success=None,
            support_date_from="2021/03/01",
            support_date_to="2022/02/28",
            profit=None,
            gross=None,
            total_contract_time=200,
            main_customer_user_id="c67b0ca2270d",
            salesforce_main_customer=None,
            customer_user_ids=None,
            main_supporter_user_id=None,
            supporter_organization_id=None,
            salesforce_main_supporter_user_name=None,
            supporter_user_ids=None,
            salesforce_supporter_user_names=None,
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type=None,
            is_secret=False,
            salesforce_use_package=None,
            salesforce_via_pr=None,
            update_history=None,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()

    def teardown_method(self, method):
        CustomerModel.delete_table()
        ProjectModel.delete_table()
        UserModel.delete_table()

    @pytest.mark.parametrize(
        "customer_id, version, body, expected",
        [
            (
                "033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                1,
                {"name": "ソニーグループ株式会社", "category": "ソニーグループ"},
                {
                    "id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                    "name": "ソニーグループ株式会社",
                    "category": "ソニーグループ",
                    "salesforceCustomerId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 2,
                },
            ),
        ],
    )
    def test_ok(
        self,
        mock_auth_admin,
        customer_id,
        version,
        body,
        expected,
    ):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            f"/api/customers/{customer_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        actual.pop("updateId")
        actual.pop("updateAt")
        actual.pop("updateUserName")

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "role",
        [
            (UserRoleType.SALES.key),
            (UserRoleType.SUPPORTER_MGR.key),
            (UserRoleType.SALES_MGR.key),
            (UserRoleType.SURVEY_OPS.key),
            (UserRoleType.MAN_HOUR_OPS.key),
        ],
    )
    def test_auth_ok(
        self,
        mock_auth_admin,
        role,
    ):
        """ロール別権限テスト"""
        mock_auth_admin([role])

        customer_id = "033bd0b5-c2c7-4778-a58d-76a46500f7d9"
        version = 1
        body = {"name": "ソニーグループ株式会社", "category": "ソニーグループ"}

        response = client.put(
            f"/api/customers/{customer_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "body",
        [
            # name,category指定
            ({"name": "ソニーグループ株式会社", "category": "ソニーグループ"},),
            # nameのみ指定
            ({"name": "ソニーグループ株式会社"}),
            # categoryのみ指定
            ({"category": "ソニーグループ"}),
        ],
    )
    def test_body_pattern_type_ok(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        customer_id = "033bd0b5-c2c7-4778-a58d-76a46500f7d9"
        version = 1

        response = client.put(
            f"/api/customers/{customer_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "body",
        [
            ({"name": "ソニーグループ株式会社", "category": "ソニーグループ"},),
            ({"name": "ソニーグループ株式会社", "category": "大企業内新規案件"},),
            ({"name": "ソニーグループ株式会社", "category": "ベンチャー中小企業"},),
            ({"name": "ソニーグループ株式会社", "category": "事業/人材開発機関"},),
            ({"name": "ソニーグループ株式会社", "category": "教育/研究開発機関"},),
        ],
    )
    def test_customer_category_type_ok(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        customer_id = "033bd0b5-c2c7-4778-a58d-76a46500f7d9"
        version = 1

        response = client.put(
            f"/api/customers/{customer_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    def test_customer_not_found(self, mocker, mock_auth_admin):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        """取引先が存在しない時のテスト"""
        customer_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1
        body = {"name": "ソニーグループ株式会社", "category": "ソニーグループ"}

        response = client.put(
            f"/api/customers/{customer_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "body",
        [
            ({"name": "ソニーグループ株式会社", "category": "ソニーグループ"},),
        ],
    )
    def test_customer_version_conflict(self, mocker, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        customer_id = "033bd0b5-c2c7-4778-a58d-76a46500f7d9"
        version = 2

        response = client.put(
            f"/api/customers/{customer_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"

    @pytest.mark.parametrize(
        "body",
        [
            # ###########################
            # body(name,category)
            # ###########################
            # body項目存在チェック
            {},
            {"name": None, "category": None},
            # ###########################
            # name
            # ###########################
            # 空文字チェック
            {"name": "", "category": "ソニーグループ"},
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        customer_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        response = client.put(
            f"/api/customers/{customer_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )
        assert response.status_code == 422
