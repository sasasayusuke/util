import pytest
from app.main import app
from app.models.customer import CustomerModel
from app.resources.const import DataType
from fastapi.testclient import TestClient

client = TestClient(app)


class TestSuggestCustomers:
    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社１",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社２",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社３",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_ok(self, mocker, model_list, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get("/api/customers/suggest?sort=name:asc")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社１",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社２",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社３",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_auth_ok_supporter(self, mocker, model_list, expected):
        """権限 支援者：制限なし"""
        mock = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get("/api/customers/suggest?sort=name:asc")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社１",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社２",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社３",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_auth_ok_supporter_mgr(self, mocker, model_list, expected):
        """権限 支援者責任者：制限なし"""
        mock = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get("/api/customers/suggest?sort=name:asc")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社１",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社２",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社３",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_sales_user")
    def test_auth_ok_sales(self, mocker, model_list, expected):
        """権限 営業担当者：制限なし"""
        mock = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get("/api/customers/suggest?sort=name:asc")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社１",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社２",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社３",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_sales_mgr_user")
    def test_auth_ok_sales_mgr(self, mocker, model_list, expected):
        """権限 営業責任者：制限なし"""
        mock = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get("/api/customers/suggest?sort=name:asc")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    CustomerModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社１",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社２",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                    CustomerModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.CUSTOMER,
                        name="ソニーグループ株式会社３",
                        category="ソニーグループ",
                        salesforce_customer_id=None,
                        salesforce_update_at="2020-10-23T02:21:39.356000+0000",
                        salesforce_target=None,
                        salesforce_credit_limit=None,
                        salesforce_credit_get_month=None,
                        salesforce_credit_manager=None,
                        salesforce_credit_no_retry=None,
                        salesforce_paws_credit_number=None,
                        salesforce_customer_owner=None,
                        salesforce_customer_segment=None,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T04:21:39.356000+0000",
                        version=1,
                    ),
                ],
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_customer_user")
    def test_auth_error_customer_403(self, mocker, model_list, expected):
        """権限 顧客：アクセス不可"""
        mock = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get("/api/customers/suggest?sort=name:asc")

        assert response.status_code == 403

    @pytest.mark.usefixtures("auth_user")
    def test_customer_zero(self, mocker):
        """取引先が１件も存在しない時"""
        mock = mocker.patch.object(CustomerModel.data_type_name_index, "query")
        mock.return_value = []

        response = client.get("/api/customers/suggest?sort=name:asc")

        actual = response.json()
        assert response.status_code == 200
        assert actual == []

    @pytest.mark.usefixtures("auth_user")
    def test_validation_sort(self):
        """sort指定のバリデーションチェック"""
        sort = "name:desc"
        response = client.get(f"/api/customers/suggest?sort={sort}")
        assert response.status_code == 422
