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
class TestGetCustomers:
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
        response = client.get("/api/customers", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def setup_method(self, method):
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        customer_models = [
            CustomerModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
            ),
            CustomerModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
            ),
            CustomerModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
            ),
            CustomerModel(
                id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
            ),
            CustomerModel(
                id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
            ),
        ]

        for customer_model in customer_models:
            customer_model.save()

    def teardown_method(self, method):
        CustomerModel.delete_table()

    @pytest.mark.parametrize(
        "query_param, expected",
        [
            (
                "?name=ソニーグループ株式会社&sort=name:asc&limit=2&offsetPage=1",
                {
                    "offsetPage": 1,
                    "total": 5,
                    "customers": [
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok_1page(
        self,
        mock_auth_admin,
        query_param,
        expected,
    ):
        """正常1ページ目  total:5件,limit=2,offsetPage=1"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, expected",
        [
            (
                "?name=ソニーグループ株式会社&sort=name:asc&limit=2&offsetPage=2",
                {
                    "offsetPage": 2,
                    "total": 5,
                    "customers": [
                        {
                            "id": "33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok_2page(
        self,
        mock_auth_admin,
        query_param,
        expected,
    ):
        """正常2ページ目  total:5件,limit=2,offsetPage=2"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, expected",
        [
            (
                "?name=ソニーグループ株式会社&sort=name:asc&limit=2&offsetPage=3",
                {
                    "offsetPage": 3,
                    "total": 5,
                    "customers": [
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok_3page(
        self,
        mock_auth_admin,
        query_param,
        expected,
    ):
        """正常3ページ目  total:5件,limit=2,offsetPage=3"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "?sort=salesforceUpdateAt:asc&limit=20&offsetPage=1",
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime(2020, 10, 24, 2, 21, 39, 356000),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime(2020, 10, 21, 2, 21, 39, 356000),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime(2020, 10, 22, 2, 21, 39, 356000),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=None,
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                ],
                {
                    "offsetPage": 1,
                    "total": 5,
                    "customers": [
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": None,
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-21T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-22T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-24T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok_sort_salesforce_update_at_asc(
        self,
        mock_auth_admin,
        query_param,
        customer_models,
        expected,
    ):
        """ソート確認: salesforceUpdateAt:asc"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        CustomerModel.delete_table()
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for customer_model in customer_models:
            customer_model.save()

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "?sort=salesforceUpdateAt:desc&limit=20&offsetPage=1",
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime(2020, 10, 24, 2, 21, 39, 356000),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime(2020, 10, 21, 2, 21, 39, 356000),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime(2020, 10, 22, 2, 21, 39, 356000),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=None,
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                ],
                {
                    "offsetPage": 1,
                    "total": 5,
                    "customers": [
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-24T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-22T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-21T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": None,
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok_sort_salesforce_update_at_desc(
        self,
        mock_auth_admin,
        query_param,
        customer_models,
        expected,
    ):
        """ソート確認:salesforceUpdateAt:desc"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        CustomerModel.delete_table()
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for customer_model in customer_models:
            customer_model.save()

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "?sort=updateAt:asc&limit=20&offsetPage=1",
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 24, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 21, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 22, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 20, 4, 21, 39, 356000),
                    ),
                ],
                {
                    "offsetPage": 1,
                    "total": 5,
                    "customers": [
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-20T04:21:39.356+09:00",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-21T04:21:39.356+09:00",
                        },
                        {
                            "id": "33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-22T04:21:39.356+09:00",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-24T04:21:39.356+09:00",
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok_sort_update_at_asc(
        self,
        mock_auth_admin,
        query_param,
        customer_models,
        expected,
    ):
        """ソート確認: updateAt:asc"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        CustomerModel.delete_table()
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for customer_model in customer_models:
            customer_model.save()

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "?sort=updateAt:desc&limit=20&offsetPage=1",
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 24, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 21, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 22, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 20, 4, 21, 39, 356000),
                    ),
                ],
                {
                    "offsetPage": 1,
                    "total": 5,
                    "customers": [
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-24T04:21:39.356+09:00",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-22T04:21:39.356+09:00",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-21T04:21:39.356+09:00",
                        },
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-20T04:21:39.356+09:00",
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok_sort_update_at_desc(
        self,
        mocker,
        mock_auth_admin,
        query_param,
        customer_models,
        expected,
    ):
        """ソート確認:updateAt:desc"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        CustomerModel.delete_table()
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for customer_model in customer_models:
            customer_model.save()

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, expected",
        [
            (
                "?sort=name:asc&limit=2&offsetPage=1",
                {
                    "offsetPage": 1,
                    "total": 0,
                    "customers": [],
                },
            ),
        ],
    )
    def test_ok_zero(
        self,
        mock_auth_admin,
        query_param,
        expected,
    ):
        """正常 0件  total:0件,limit=2,offsetPage=1"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        CustomerModel.delete_table()
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

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

        CustomerModel.delete_table()
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        query_param = "?sort=name:asc&limit=2&offsetPage=1"

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "",
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社１",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社２",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社３",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社４",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社５",
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
                        create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                    ),
                ],
                {
                    "offsetPage": 1,
                    "total": 5,
                    "customers": [
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社１",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社２",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社３",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社４",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "ソニーグループ株式会社５",
                            "category": "ソニーグループ",
                            "salesforceUpdateAt": "2020-10-23T02:21:39.356+09:00",
                            "updateAt": "2020-10-23T04:21:39.356+09:00",
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok_no_query_param(
        self,
        mock_auth_admin,
        query_param,
        customer_models,
        expected,
    ):
        """正常 クエリパラメータ指定なし（デフォルト）"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        CustomerModel.delete_table()
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for customer_model in customer_models:
            customer_model.save()

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param",
        [
            # sortのENUM
            "?sort=name:asc",
            "?sort=name:desc",
            "?sort=salesforceUpdateAt:asc",
            "?sort=salesforceUpdateAt:desc",
            "?sort=updateAt:asc",
            "?sort=updateAt:desc",
        ],
    )
    def test_enum_ok(
        self,
        mock_auth_admin,
        query_param,
    ):
        """sortのENUM確認"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        CustomerModel.delete_table()
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        customer_models = [
            CustomerModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社１",
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
                create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
            ),
            CustomerModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社２",
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
                create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
            ),
            CustomerModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社３",
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
                create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
            ),
        ]

        for customer_model in customer_models:
            customer_model.save()

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [("?name=ソニーグループ株式会社&sort=name:asc&limit=2&offsetPage=4")],
    )
    def test_ok_page_not_found(
        self,
        mock_auth_admin,
        query_param,
    ):
        """ページ Not found.  total:5件,limit=2,offsetPage=4"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        assert response.status_code == 404

    @pytest.mark.parametrize(
        "query_param",
        [
            # sort: Enum
            "?sort=name:aaa",
        ],
    )
    def test_validation_enum(
        self,
        mock_auth_admin,
        query_param,
    ):
        """sortのENUMバリデーションエラー"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(f"/api/customers{query_param}", headers=REQUEST_HEADERS)

        assert response.status_code == 422
