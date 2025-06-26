import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.admin import AdminModel
from app.resources.const import DataType, UserRoleType
from app.service.project_service import ProjectService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetAdminById:
    @pytest.mark.parametrize(
        "admin_id, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                AdminModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.ADMIN,
                    name="山田太郎",
                    email="user@example.com",
                    roles=["sales"],
                    company="ソニーグループ株式会社",
                    job="部長",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    organization_name="IST",
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizations": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "組織開発",
                        }
                    ],
                    "organizationName": "IST",
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": "1",
                },
            ),
        ],
    )
    def test_auth_for_system_admin(
        self, mocker, mock_auth_admin, admin_id, model, expected
    ):
        """正常系のテスト(システム管理者)"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(ProjectService, "get_supporter_organization_name")
        mock.return_value = "組織開発"
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model

        response = client.get(f"/api/admins/{admin_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "admin_id",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")],
    )
    def test_auth_for_sales(self, mocker, mock_auth_admin, admin_id):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])
        mock = mocker.patch.object(ProjectService, "get_supporter_organization_name")
        mock.return_value = "組織開発"

        response = client.get(f"/api/admins/{admin_id}", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")],
    )
    def test_auth_for_supporter_mgr(self, mocker, mock_auth_admin, admin_id):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])
        mock = mocker.patch.object(ProjectService, "get_supporter_organization_name")
        mock.return_value = "組織開発"

        response = client.get(f"/api/admins/{admin_id}", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")],
    )
    def test_auth_for_sales_mgr(self, mocker, mock_auth_admin, admin_id):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])
        mock = mocker.patch.object(ProjectService, "get_supporter_organization_name")
        mock.return_value = "組織開発"

        response = client.get(f"/api/admins/{admin_id}", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")],
    )
    def test_auth_for_survey_ops(self, mocker, mock_auth_admin, admin_id):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])
        mock = mocker.patch.object(ProjectService, "get_supporter_organization_name")
        mock.return_value = "組織開発"

        response = client.get(f"/api/admins/{admin_id}", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")],
    )
    def test_auth_for_man_hour_ops(self, mocker, mock_auth_admin, admin_id):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        mock = mocker.patch.object(ProjectService, "get_supporter_organization_name")
        mock.return_value = "組織開発"

        response = client.get(f"/api/admins/{admin_id}", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    def test_admin_not_found(self, mocker, mock_auth_admin):
        """管理ユーザーが存在しない時のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        admin_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock = mocker.patch.object(AdminModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.get(f"/api/admins/{admin_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
