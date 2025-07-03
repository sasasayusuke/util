import pytest
from app.main import app
from app.models.customer import CustomerModel
from app.models.user import UserModel
from app.resources.const import DataType
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestGetCustomerById:
    @pytest.mark.parametrize(
        "customer_id, model, user_model, expected",
        [
            (
                "033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                CustomerModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
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
                    update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
                UserModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="taro.yamada@example.com",
                    job="部長",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    role="supporter_mgr",
                ),
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
    @pytest.mark.usefixtures("auth_user")
    def test_ok(self, mocker, customer_id, model, user_model, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(CustomerModel, "get")
        mock.return_value = model
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        response = client.get(f"/api/customers/{customer_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "customer_id, model, user_model, expected",
        [
            (
                "033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                CustomerModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
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
                    update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
                UserModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="taro.yamada@example.com",
                    job="部長",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    role="supporter_mgr",
                ),
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
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_auth_ok_supporter(self, mocker, customer_id, model, user_model, expected):
        """権限確認: 支援者: 制限なし"""
        mock = mocker.patch.object(CustomerModel, "get")
        mock.return_value = model
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        response = client.get(f"/api/customers/{customer_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "customer_id, model, user_model, expected",
        [
            (
                "033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                CustomerModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
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
                    update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
                UserModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="taro.yamada@example.com",
                    job="部長",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    role="supporter_mgr",
                ),
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
    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_auth_ok_supporter_mgr(
        self, mocker, customer_id, model, user_model, expected
    ):
        """権限確認: 支援者責任者: 制限なし"""
        mock = mocker.patch.object(CustomerModel, "get")
        mock.return_value = model
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        response = client.get(f"/api/customers/{customer_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "customer_id, model, user_model, expected",
        [
            (
                "033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                CustomerModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
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
                    update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
                UserModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="taro.yamada@example.com",
                    job="部長",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    role="supporter_mgr",
                ),
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
    @pytest.mark.usefixtures("auth_sales_user")
    def test_auth_ok_sales(self, mocker, customer_id, model, user_model, expected):
        """権限確認: 営業担当者: 制限なし"""
        mock = mocker.patch.object(CustomerModel, "get")
        mock.return_value = model
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        response = client.get(f"/api/customers/{customer_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "customer_id, model, user_model, expected",
        [
            (
                "033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                CustomerModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
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
                    update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
                UserModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="taro.yamada@example.com",
                    job="部長",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    role="supporter_mgr",
                ),
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
    @pytest.mark.usefixtures("auth_sales_mgr_user")
    def test_auth_ok_sales_mgr(self, mocker, customer_id, model, user_model, expected):
        """権限確認: 営業責任者: 制限なし"""
        mock = mocker.patch.object(CustomerModel, "get")
        mock.return_value = model
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        response = client.get(f"/api/customers/{customer_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "customer_id, model, user_model, expected",
        [
            (
                "033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                CustomerModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
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
                    update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
                UserModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="taro.yamada@example.com",
                    job="部長",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    role="supporter_mgr",
                ),
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
    @pytest.mark.usefixtures("auth_customer_user")
    def test_auth_error_customer_403(
        self, mocker, customer_id, model, user_model, expected
    ):
        """権限確認: 顧客: 呼出不可"""
        mock = mocker.patch.object(CustomerModel, "get")
        mock.return_value = model
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        response = client.get(f"/api/customers/{customer_id}")

        assert response.status_code == 403

    @pytest.mark.usefixtures("auth_user")
    def test_customer_not_found(self, mocker):
        """取引先が存在しない時のテスト"""
        customer_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock = mocker.patch.object(CustomerModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.get(f"/api/customers/{customer_id}")

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
