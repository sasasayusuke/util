import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.customer import CustomerModel
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from app.service.user_service import UserService
from app.utils.aws.ses import SesHelper

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestCreateUser:
    @pytest.mark.parametrize(
        "customer_model, body, expected",
        [
            (
                CustomerModel(
                    id="4b47f7a9-9558-49d8-aca0-f584a483b22f",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    create_at="2020-10-23T03:21:39.356Z",
                    update_at="2020-10-23T03:21:39.356Z",
                ),
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "customer",
                    "job": "部長",
                    "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
                    "solver_corporation_id": None,
                },
                {
                    "id": "ca17e3b5-f6e5-43e5-9de4-ff7bde217bd7",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "customer",
                    "job": "部長",
                    "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
                    "customer_name": "〇〇株式会社",
                    "solver_corporation_id": None,
                },
            )
        ],
    )
    def test_customer_user_create_ok(
        self, mock_auth_admin, mocker, customer_model, body, expected
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(UserModel, "save")
        mock = mocker.patch.object(CustomerModel, "get_customer_count")
        mock.return_value = 1
        mock = mocker.patch.object(CustomerModel, "get")
        mock.return_value = customer_model

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual["name"] == expected["name"]
        assert actual["email"] == expected["email"]
        assert actual["role"] == expected["role"]
        assert actual["job"] == expected["job"]
        assert actual["customerId"] == expected["customer_id"]

    @pytest.mark.parametrize(
        "body, expected",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "supporter",
                    "company": "ソニーグループ株式会社",
                    "solver_corporation_id": None,
                    "supporter_organizations": [
                        {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                    ],
                    "is_input_man_hour": True,
                },
                {
                    "id": "ca17e3b5-f6e5-43e5-9de4-ff7bde217bd7",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "supporter",
                    "company": "ソニーグループ株式会社",
                    "solver_corporation_id": None,
                    "supporter_organizations": [
                        {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9", "name": "IST"}
                    ],
                    "is_input_man_hour": True,
                },
            )
        ],
    )
    def test_not_customer_user_create_ok(self, mocker, mock_auth_admin, body, expected):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(UserModel, "save")
        mock = mocker.patch.object(
            UserService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual["name"] == expected["name"]
        assert actual["email"] == expected["email"]
        assert actual["role"] == expected["role"]
        assert actual["company"] == expected["company"]
        assert actual["supporterOrganizations"] == expected["supporter_organizations"]
        assert actual["isInputManHour"] == expected["is_input_man_hour"]

    @pytest.mark.parametrize(
        "body, expected",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "solver_staff",
                    "company": "ソニーグループ株式会社",
                    "solver_corporation_id": "ca17e3b5-f6e5-43e5-9de4-ff7bde217777",
                    "supporter_organizations": [
                        {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                    ],
                    "is_input_man_hour": True,
                },
                {
                    "id": "ca17e3b5-f6e5-43e5-9de4-ff7bde217bd7",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "solver_staff",
                    "company": "ソニーグループ株式会社",
                    "solver_corporation_id": "ca17e3b5-f6e5-43e5-9de4-ff7bde217777",
                    "supporter_organizations": [
                        {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9", "name": "IST"}
                    ],
                    "is_input_man_hour": True,
                },
            )
        ],
    )
    def test_solver_staff_user_create_ok(self, mocker, mock_auth_admin, body, expected):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(UserModel, "save")
        mock = mocker.patch.object(
            UserService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual["name"] == expected["name"]
        assert actual["email"] == expected["email"]
        assert actual["role"] == expected["role"]
        assert actual["company"] == expected["company"]
        assert actual["supporterOrganizations"] == expected["supporter_organizations"]
        assert actual["isInputManHour"] == expected["is_input_man_hour"]

    @pytest.mark.parametrize(
        "body, expected",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "apt",
                    "company": "ソニーグループ株式会社",
                    "solver_corporation_id": None,
                    "supporter_organizations": [
                        {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                    ],
                    "is_input_man_hour": True,
                },
                {
                    "id": "ca17e3b5-f6e5-43e5-9de4-ff7bde217bd7",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "apt",
                    "company": "ソニーグループ株式会社",
                    "solver_corporation_id": None,
                    "supporter_organizations": [
                        {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9", "name": "IST"}
                    ],
                    "is_input_man_hour": True,
                },
            )
        ],
    )
    def test_apt_user_create_ok(self, mocker, mock_auth_admin, body, expected):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(UserModel, "save")
        mock = mocker.patch.object(
            UserService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual["name"] == expected["name"]
        assert actual["email"] == expected["email"]
        assert actual["role"] == expected["role"]
        assert actual["company"] == expected["company"]
        assert actual["supporterOrganizations"] == expected["supporter_organizations"]
        assert actual["isInputManHour"] == expected["is_input_man_hour"]

    def test_duplicate_email(self, mocker, mock_auth_admin):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get_user_count_by_email")
        mock.return_value = 1

        body = {
            "name": "山田太郎",
            "email": "user@example.com",
            "role": "customer",
            "job": "部長",
            "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
        }

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 400

    def test_auth_for_sales(self, mock_auth_admin):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])

        body = {
            "name": "山田太郎",
            "email": "user@example.com",
            "role": "customer",
            "job": "部長",
            "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
        }

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 403

    def test_auth_for_supporter_mgr(self, mock_auth_admin):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        body = {
            "name": "山田太郎",
            "email": "user@example.com",
            "role": "customer",
            "job": "部長",
            "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
        }

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 403

    def test_auth_for_sales_mgr(self, mock_auth_admin):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])

        body = {
            "name": "山田太郎",
            "email": "user@example.com",
            "role": "customer",
            "job": "部長",
            "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
        }

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 403

    def test_auth_for_survey_ops(self, mock_auth_admin):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])

        body = {
            "name": "山田太郎",
            "email": "user@example.com",
            "role": "customer",
            "job": "部長",
            "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
        }

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 403

    def test_auth_for_man_hour_ops(self, mock_auth_admin):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])

        body = {
            "name": "山田太郎",
            "email": "user@example.com",
            "role": "customer",
            "job": "部長",
            "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
        }

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body",
        [
            # ###########################
            # name
            # ###########################
            # 必須チェック
            {
                # 'name': '山田太郎',
                "email": "user@example.com",
                "role": "customer",
                "job": "部長",
                "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
            },
            # Noneチェック
            {
                "name": None,
                "email": "user@example.com",
                "role": "customer",
                "job": "部長",
                "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
            },
            # ###########################
            # email
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                # 'email': 'user@example.com',
                "role": "customer",
                "job": "部長",
                "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": None,
                "role": "customer",
                "job": "部長",
                "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
            },
            # 型チェック
            {
                "name": "山田太郎",
                "email": "user",
                "role": "customer",
                "job": "部長",
                "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
            },
            # ###########################
            # role
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                # "role": "customer",
                "job": "部長",
                "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": None,
                "job": "部長",
                "customer_id": "4b47f7a9-9558-49d8-aca0-f584a483b22f",
            },
            # ###########################
            # customer_id
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                "job": "部長",
                # 'customer_id': '4b47f7a9-9558-49d8-aca0-f584a483b22f',
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                "job": "部長",
                "customer_id": None,
            },
            # ###########################
            # company
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                # 'company': 'ソニーグループ株式会社',
                "supporter_organizations": [
                    {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                ],
                "is_input_man_hour": True,
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                "company": None,
                "supporter_organizations": [
                    {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                ],
                "is_input_man_hour": True,
            },
            # ###########################
            # supporter_organization_id
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                "company": "ソニーグループ株式会社",
                # "supporter_organizations": [{"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}],
                "organization_name": "IST",
                "is_input_man_hour": True,
            },
            # ###########################
            # is_input_man_hour
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                "company": "ソニーグループ株式会社",
                "supporter_organizations": [
                    {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                ],
                # 'is_input_man_hour': True,
                "disabled": False,
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "role": "customer",
                "company": "ソニーグループ株式会社",
                "supporter_organizations": [
                    {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                ],
                "is_input_man_hour": None,
            },
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.post("/api/users", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 422
