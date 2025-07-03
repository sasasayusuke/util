from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.customer import CustomerModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestSuggestCustomers:
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
        response = client.get(
            "/api/customers/suggest?sort=name:asc", headers=REQUEST_HEADERS
        )

        assert response.status_code == 200

    def setup_method(self, method):
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        customer_models = [
            CustomerModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社１",
                category="ソニーグループ",
                salesforce_customer_id=None,
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
            ),
            CustomerModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社２",
                category="ソニーグループ",
                salesforce_customer_id=None,
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
            ),
            CustomerModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社３",
                category="ソニーグループ",
                salesforce_customer_id=None,
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
            ),
        ]

        for customer_model in customer_models:
            customer_model.save()

    def teardown_method(self, method):
        CustomerModel.delete_table()

    @pytest.mark.parametrize(
        "expected",
        [
            [
                {
                    "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社１",
                },
                {
                    "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社２",
                },
                {
                    "id": "33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社３",
                },
            ],
        ],
    )
    def test_ok(self, mock_auth_admin, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(
            "/api/customers/suggest?sort=name:asc", headers=REQUEST_HEADERS
        )

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

        response = client.get(
            "/api/customers/suggest?sort=name:asc", headers=REQUEST_HEADERS
        )
        assert response.status_code == 200

    def test_customer_zero(self, mock_auth_admin):
        """取引先が１件も存在しない時"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        CustomerModel.delete_table()
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        response = client.get(
            "/api/customers/suggest?sort=name:asc", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == []

    def test_validation_sort(self, mock_auth_admin):
        """sort指定のバリデーションチェック"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        sort = "name:desc"

        response = client.get(
            f"/api/customers/suggest?sort={sort}", headers=REQUEST_HEADERS
        )
        assert response.status_code == 422
