import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.admin import AdminModel
from app.resources.const import UserRoleType
from app.utils.aws.ses import SesHelper

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestCreateAdmin:
    @pytest.mark.parametrize(
        "body, expected",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                },
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": None,
                    "organizationName": "IST",
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": "1",
                },
            )
        ],
    )
    def test_auth_for_system_admin(self, mocker, mock_auth_admin, body, expected):
        """正常系のテスト(システム管理者)"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(AdminModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(AdminModel, "save")

        response = client.post("/api/admins", json=body, headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual["name"] == expected["name"]
        assert actual["email"] == expected["email"]
        assert actual["roles"] == expected["roles"]
        assert actual["company"] == expected["company"]
        assert actual["job"] == expected["job"]
        assert actual["supporterOrganizationId"] == expected["supporterOrganizationId"]
        assert actual["organizationName"] == expected["organizationName"]
        assert actual["disabled"] == expected["disabled"]

    @pytest.mark.parametrize(
        "body",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                }
            )
        ],
    )
    def test_auth_for_sales(self, mocker, mock_auth_admin, body):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])
        mock = mocker.patch.object(AdminModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(AdminModel, "save")

        response = client.post("/api/admins", json=body, headers=REQUEST_HEADERS)

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                }
            )
        ],
    )
    def test_auth_for_supporter_mrg(self, mocker, mock_auth_admin, body):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])
        mock = mocker.patch.object(AdminModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(AdminModel, "save")

        response = client.post("/api/admins", json=body, headers=REQUEST_HEADERS)

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                }
            )
        ],
    )
    def test_auth_for_sales_mgr(self, mocker, mock_auth_admin, body):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])
        mock = mocker.patch.object(AdminModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(AdminModel, "save")

        response = client.post("/api/admins", json=body, headers=REQUEST_HEADERS)

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                }
            )
        ],
    )
    def test_auth_for_survey_ops(self, mocker, mock_auth_admin, body):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])
        mock = mocker.patch.object(AdminModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(AdminModel, "save")

        response = client.post("/api/admins", json=body, headers=REQUEST_HEADERS)

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                }
            )
        ],
    )
    def test_auth_for_man_hour_ops(self, mocker, mock_auth_admin, body):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        mock = mocker.patch.object(AdminModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(AdminModel, "save")

        response = client.post("/api/admins", json=body, headers=REQUEST_HEADERS)

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body, expected",
        [
            (
                {
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["supporter_mgr"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "organizationName": "IST",
                    "disabled": False,
                },
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["supporter_mgr"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "organizationName": "IST",
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": "1",
                },
            )
        ],
    )
    def test_ok_supporter_mgr(self, mocker, mock_auth_admin, body, expected):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(AdminModel, "get_user_count_by_email")
        mock.return_value = 0
        mocker.patch.object(SesHelper, "send_mail")
        mocker.patch.object(AdminModel, "save")

        response = client.post("/api/admins", json=body, headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual["name"] == expected["name"]
        assert actual["email"] == expected["email"]
        assert actual["roles"] == expected["roles"]
        assert actual["company"] == expected["company"]
        assert actual["job"] == expected["job"]
        assert actual["supporterOrganizationId"] == expected["supporterOrganizationId"]
        assert actual["organizationName"] == expected["organizationName"]
        assert actual["disabled"] == expected["disabled"]

    def test_duplicate_email(self, mocker, mock_auth_admin):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(AdminModel, "get_user_count_by_email")
        mock.return_value = 1

        body = {
            "name": "山田太郎",
            "email": "user1@example.com",
            "roles": ["sales"],
            "company": "ソニーグループ株式会社",
            "job": "部長",
            "supporterOrganizationId": [],
            "organizationName": "IST",
            "disabled": "false",
        }

        response = client.post("/api/admins", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 400

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
                "roles": ["sales"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            # Noneチェック
            {
                "name": None,
                "email": "user@example.com",
                "roles": ["sales"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            # ###########################
            # email
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                # 'email': 'user@example.com',
                "roles": ["sales"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": None,
                "roles": ["sales"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            # 型チェック
            {
                "name": "山田太郎",
                "email": "user",
                "roles": ["sales"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            {
                "name": "山田太郎",
                "email": "user@example",
                "roles": ["sales"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            # ###########################
            # role
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                # 'roles': ['sales'],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": None,
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            # ###########################
            # company
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["sales"],
                # 'company': 'ソニーグループ株式会社',
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["sales"],
                "company": None,
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": "false",
            },
            # ###########################
            # disabled
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["sales"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                # 'disabled': 'false'
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["sales"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": [],
                "organizationName": "IST",
                "disabled": None,
            },
            # ###########################
            # name
            # ###########################
            # 必須チェック
            {
                # 'name': '山田太郎',
                "email": "user@example.com",
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            # Noneチェック
            {
                "name": None,
                "email": "user@example.com",
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            # ###########################
            # email
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                # 'email': 'user@example.com',
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": None,
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            # 型チェック
            {
                "name": "山田太郎",
                "email": "user",
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            {
                "name": "山田太郎",
                "email": "user@example",
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            # ###########################
            # role
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                # 'roles': ['supporter_mgr'],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": None,
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            # ###########################
            # company
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["supporter_mgr"],
                # 'company': 'ソニーグループ株式会社',
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["supporter_mgr"],
                "company": None,
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": "false",
            },
            # ###########################
            # supporter_organization_id
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                # 'supporterOrganizationId':
                #     ['89cbe2ed-f44c-4a1c-9408-c67b0ca2270d'],
                "organizationName": "IST",
                "disabled": "false",
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": None,
                "organizationName": "IST",
                "disabled": "false",
            },
            # ###########################
            # disabled
            # ###########################
            # 必須チェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                # 'disabled': 'false'
            },
            # Noneチェック
            {
                "name": "山田太郎",
                "email": "user@example.com",
                "roles": ["supporter_mgr"],
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                "organizationName": "IST",
                "disabled": None,
            },
        ],
    )
    def test_validation(self, body, mock_auth_admin):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.post("/api/admins", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 422
