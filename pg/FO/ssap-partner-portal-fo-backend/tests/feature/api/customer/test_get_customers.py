from datetime import datetime

import pytest
from app.main import app
from app.models.customer import CustomerModel
from app.resources.const import DataType
from fastapi.testclient import TestClient

client = TestClient(app)


class TestGetCustomers:
    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "?name=ソニーグループ株式会社&sort=name:asc&limit=2&offsetPage=1",
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
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
    @pytest.mark.usefixtures("auth_user")
    def test_ok_1page(
        self,
        mocker,
        query_param,
        customer_models,
        expected,
    ):
        """正常1ページ目  total:5件,limit=2,offsetPage=1"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = customer_models

        response = client.get(f"/api/customers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "?name=ソニーグループ株式会社&sort=name:asc&limit=2&offsetPage=2",
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                ],
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
    @pytest.mark.usefixtures("auth_user")
    def test_ok_2page(
        self,
        mocker,
        query_param,
        customer_models,
        expected,
    ):
        """正常2ページ目  total:5件,limit=2,offsetPage=2"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = customer_models

        response = client.get(f"/api/customers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "?name=ソニーグループ株式会社&sort=name:asc&limit=2&offsetPage=3",
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                ],
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
    @pytest.mark.usefixtures("auth_user")
    def test_ok_3page(
        self,
        mocker,
        query_param,
        customer_models,
        expected,
    ):
        """正常3ページ目  total:5件,limit=2,offsetPage=3"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = customer_models

        response = client.get(f"/api/customers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "?sort=name:asc&limit=2&offsetPage=1",
                [],
                {
                    "offsetPage": 1,
                    "total": 0,
                    "customers": [],
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_ok_zero(
        self,
        mocker,
        query_param,
        customer_models,
        expected,
    ):
        """正常 0件  total:0件,limit=2,offsetPage=1"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = customer_models

        response = client.get(f"/api/customers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

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
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社２",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社３",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社４",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社５",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
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
    @pytest.mark.usefixtures("auth_user")
    def test_ok_no_query_param(
        self,
        mocker,
        query_param,
        customer_models,
        expected,
    ):
        """正常 クエリパラメータ指定なし（デフォルト）"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = customer_models

        response = client.get(f"/api/customers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_supporter_user")
    def test_auth_ok_supporter(
        self,
        mocker,
    ):
        """権限 支援者:制限なし"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = [
            CustomerModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社１",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社２",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社３",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
        ]

        response = client.get("/api/customers")

        assert response.status_code == 200

    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_auth_ok_supporter_mgr(
        self,
        mocker,
    ):
        """権限 支援者責任者:制限なし"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = [
            CustomerModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社１",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社２",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社３",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
        ]

        response = client.get("/api/customers")

        assert response.status_code == 200

    @pytest.mark.usefixtures("auth_sales_user")
    def test_auth_ok_sales(
        self,
        mocker,
    ):
        """権限 営業担当者:制限なし"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = [
            CustomerModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社１",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社２",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社３",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
        ]

        response = client.get("/api/customers")

        assert response.status_code == 200

    @pytest.mark.usefixtures("auth_sales_mgr_user")
    def test_auth_ok_sales_mgr(
        self,
        mocker,
    ):
        """権限 営業責任者:制限なし"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = [
            CustomerModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社１",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社２",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社３",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
        ]

        response = client.get("/api/customers")

        assert response.status_code == 200

    @pytest.mark.usefixtures("auth_customer_user")
    def test_auth_error_customer_403(
        self,
        mocker,
    ):
        """権限 顧客:アクセス不可"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = [
            CustomerModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社１",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社２",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社３",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
        ]

        response = client.get("/api/customers")

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "query_param",
        [
            # sortのENUM
            "?sort=name:asc",
            "?sort=name:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_enum_ok(
        self,
        mocker,
        query_param,
    ):
        """sortのENUM確認"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = [
            CustomerModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社１",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社２",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
            CustomerModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.CUSTOMER,
                name="ソニーグループ株式会社３",
                category="ソニーグループ",
                salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                salesforce_update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000", "%Y-%m-%dT%H:%M:%S.%f+0000"
                ),
                salesforce_target=None,
                salesforce_credit_limit=None,
                salesforce_credit_get_month=None,
                salesforce_credit_manager=None,
                salesforce_credit_no_retry=None,
                salesforce_paws_credit_number=None,
                salesforce_customer_owner=None,
                salesforce_customer_segment=None,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime.strptime(
                    "2020-10-23T02:21:39.356000+0000",
                    "%Y-%m-%dT%H:%M:%S.%f+0000",
                ),
                version=1,
            ),
        ]

        response = client.get(f"/api/customers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param, customer_models, expected",
        [
            (
                "?name=ソニーグループ株式会社&sort=name:asc&limit=2&offsetPage=4",
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                    CustomerModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社",
                        category="ソニーグループ",
                        salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        salesforce_update_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.strptime(
                            "2020-10-23T02:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.strptime(
                            "2020-10-23T04:21:39.356000+0000",
                            "%Y-%m-%dT%H:%M:%S.%f+0000",
                        ),
                        version=1,
                    ),
                ],
                {},
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_ok_page_not_found(
        self,
        mocker,
        query_param,
        customer_models,
        expected,
    ):
        """ページ Not found.  total:5件,limit=2,offsetPage=4"""
        mock_project = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock_project.return_value = customer_models

        response = client.get(f"/api/customers{query_param}")

        assert response.status_code == 404

    @pytest.mark.parametrize(
        "query_param",
        [
            # sort: Enum
            "?sort=name:aaa",
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_validation_enum(
        self,
        mocker,
        query_param,
    ):
        """sortのENUMバリデーションエラー"""

        response = client.get(f"/api/customers{query_param}")

        assert response.status_code == 422
