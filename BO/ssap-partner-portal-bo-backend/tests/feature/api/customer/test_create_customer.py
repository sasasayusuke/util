from app.utils.platform import PlatformApiOperator
import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.customer import CustomerModel
from app.resources.const import UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestCreateCustomer:
    def setup_method(self, method):
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

    def teardown_method(self, method):
        CustomerModel.delete_table()

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
        response = client.post("/api/customers", headers=REQUEST_HEADERS)

        assert response.status_code == 422

    @pytest.mark.parametrize(
        "body, expected",
        [
            (
                {"name": "ソニーグループ株式会社", "category": "ソニーグループ"},
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社",
                    "category": "ソニーグループ",
                    "salesforceCustomerId": None,
                    "salesforceUpdateAt": None,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 1,
                },
            ),
            (
                {
                    "name": "ソニーグループ株式会社",
                    # "category": "ソニーグループ"
                },
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社",
                    "category": None,
                    "salesforceCustomerId": None,
                    "salesforceUpdateAt": None,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 1,
                },
            ),
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, body, expected):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        pf_api_mock = mocker.patch.object(PlatformApiOperator, "create_customer")
        pf_api_mock.return_value = (200, {"massage": "OK"})

        response = client.post("/api/customers", json=body, headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual["name"] == expected["name"]
        assert actual["category"] == expected["category"]

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
    def test_auth_ok(self, mocker, mock_auth_admin, role):
        """ロール別権限テスト"""
        mock_auth_admin([role])
        pf_api_mock = mocker.patch.object(PlatformApiOperator, "create_customer")
        pf_api_mock.return_value = (200, {"massage": "OK"})

        body = {"name": "ソニーグループ株式会社", "category": "ソニーグループ"}

        response = client.post("/api/customers", json=body, headers=REQUEST_HEADERS)

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "body, expected",
        [
            (
                {"name": "ソニーグループ株式会社", "category": "ソニーグループ"},
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社",
                    "category": "ソニーグループ",
                    "salesforceCustomerId": None,
                    "salesforceUpdateAt": None,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 1,
                },
            ),
            (
                {"name": "ソニーグループ株式会社", "category": "大企業内新規事業"},
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社",
                    "category": "大企業内新規事業",
                    "salesforceCustomerId": None,
                    "salesforceUpdateAt": None,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 1,
                },
            ),
            (
                {"name": "ソニーグループ株式会社", "category": "ベンチャー中小企業"},
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社",
                    "category": "ベンチャー中小企業",
                    "salesforceCustomerId": None,
                    "salesforceUpdateAt": None,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 1,
                },
            ),
            (
                {"name": "ソニーグループ株式会社", "category": "事業/人材開発機関"},
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社",
                    "category": "事業/人材開発機関",
                    "salesforceCustomerId": None,
                    "salesforceUpdateAt": None,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 1,
                },
            ),
            (
                {"name": "ソニーグループ株式会社", "category": "教育/研究開発機関"},
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "ソニーグループ株式会社",
                    "category": "教育/研究開発機関",
                    "salesforceCustomerId": None,
                    "salesforceUpdateAt": None,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 1,
                },
            ),
        ],
    )
    def test_customer_category_type_ok(self, mocker, mock_auth_admin, body, expected):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        pf_api_mock = mocker.patch.object(PlatformApiOperator, "create_customer")
        pf_api_mock.return_value = (200, {"massage": "OK"})

        response = client.post("/api/customers", json=body, headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual["name"] == expected["name"]
        assert actual["category"] == expected["category"]

    @pytest.mark.parametrize(
        "body",
        [
            # ###########################
            # name
            # ###########################
            # 必須チェック
            {
                # "name": "ソニーグループ株式会社",
                "category": "ソニーグループ"
            },
            # Noneチェック
            {"name": None, "category": "ソニーグループ"},
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.post("/api/customers", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 422
