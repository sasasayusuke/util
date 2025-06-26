import pytest
from app.main import app
from app.models.customer import CustomerModel
from app.models.user import UserModel
from app.resources.const import DataType
from app.utils.aws.ses import SesHelper
from fastapi.testclient import TestClient

client = TestClient(app)


class TestCreateUser:
    @pytest.mark.parametrize(
        "body, customer_model, expected",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "customer",
                    "customerId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "organizationName": "コンサルティング事業部",
                    "job": "部長",
                },
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
                ),
                {
                    "id": "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "customer",
                    "customerId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "customerName": "ソニーグループ株式会社",
                    "job": "部長",
                    "organizationName": "コンサルティング事業部",
                    "disabled": False,
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_ok(self, mocker, body, customer_model, expected):
        """正常時のテスト"""
        mock = mocker.patch.object(UserModel, "get_user_count_by_email")
        mock.return_value = 0
        customer_mock = mocker.patch.object(CustomerModel, "get")
        customer_mock.return_value = customer_model

        mocker.patch.object(UserModel, "save")
        mocker.patch.object(SesHelper, "send_mail")

        response = client.post("/api/users", json=body)
        actual = response.json()

        assert response.status_code == 200
        assert actual["name"] == expected["name"]
        assert actual["email"] == expected["email"]
        assert actual["customerId"] == expected["customerId"]
        assert actual["customerName"] == expected["customerName"]
        assert actual["job"] == expected["job"]
        assert actual["organizationName"] == expected["organizationName"]
        assert actual["disabled"] == expected["disabled"]

    @pytest.mark.usefixtures("auth_user")
    def test_duplicate_email(self, mocker):
        mock = mocker.patch.object(UserModel, "get_user_count_by_email")
        mock.return_value = 1

        body = {
            "name": "山田太郎",
            "email": "user@example.com",
            "role": "customer",
            "customerId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "organizationName": "コンサルティング事業部",
            "job": "部長",
            "projectId": "34cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        }

        response = client.post("/api/users", json=body)
        assert response.status_code == 400

    @pytest.mark.parametrize(
        "body",
        [
            # ###########################
            # name
            # ###########################
            # 必須チェック
            {
                # "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                "customer_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "organization_name": "コンサルティング事業部",
                "job": "部長",
            },
            # Noneチェック
            {
                "name": None,
                "email": "user@example.com",
                "role": "customer",
                "customer_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "organization_name": "コンサルティング事業部",
                "job": "部長",
            },
            # ###########################
            # email
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                # "email": "user@example.com",
                "role": "customer",
                "customer_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "organization_name": "コンサルティング事業部",
                "job": "部長",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": None,
                "role": "customer",
                "customer_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "organization_name": "コンサルティング事業部",
                "job": "部長",
            },
            # ###########################
            # role
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                # "role": "customer",
                "customer_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "organization_name": "コンサルティング事業部",
                "job": "部長",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": None,
                "customer_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "organization_name": "コンサルティング事業部",
                "job": "部長",
            },
            # Enumチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "supporter",
                "customer_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "organization_name": "コンサルティング事業部",
                "job": "部長",
            },
            # ###########################
            # customer_id
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                # "customer_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "organization_name": "コンサルティング事業部",
                "job": "部長",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                "customer_id": None,
                "customer_name": "取引先株式会社",
                "organization_name": "コンサルティング事業部",
                "job": "部長",
            },
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_validation(self, body):
        response = client.post("/api/users", json=body)

        assert response.status_code == 422
