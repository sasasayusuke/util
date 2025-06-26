from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.customer import CustomerModel
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetCustomerById:
    def setup_method(self, method):
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        UserModel.create_table(read_capacity_units=5, write_capacity_units=5, wait=True)

        CustomerModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
            update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
        ).save()

        UserModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.USER,
            name="山田太郎",
            email="taro.yamada@example.com",
            job="部長",
            supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            role="supporter_mgr",
        ).save()

    def teardown_method(self, method):
        CustomerModel.delete_table()
        UserModel.delete_table()

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
        response = client.get("/api/customers/1", headers=REQUEST_HEADERS)

        assert response.status_code == 404

    @pytest.mark.parametrize(
        "customer_id, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社",
                    "category": "ソニーグループ",
                    "salesforceCustomerId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "updateUserName": "山田太郎",
                    "updateAt": "2020-10-23T04:21:39.356+09:00",
                    "version": 1,
                },
            ),
        ],
    )
    def test_ok(self, mock_auth_admin, customer_id, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(f"/api/customers/{customer_id}", headers=REQUEST_HEADERS)

        actual = response.json()
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
    def test_auth_ok(self, mock_auth_admin, role):
        """ロール別権限テスト"""
        mock_auth_admin([role])

        customer_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        response = client.get(f"/api/customers/{customer_id}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def test_customer_not_found(self, mock_auth_admin):
        """取引先が存在しない時のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        customer_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        response = client.get(f"/api/customers/{customer_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
