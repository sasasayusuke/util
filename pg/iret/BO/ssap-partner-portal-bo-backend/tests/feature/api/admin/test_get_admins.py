import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.admin import AdminModel
from app.resources.const import DataType, UserRoleType
from app.service.project_service import ProjectService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestgetAdmins:
    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    AdminModel(
                        id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
                        data_type=DataType.ADMIN,
                        name="田中太郎",
                        email="user@example.com",
                        roles=["sales"],
                        company="ソニーグループ株式会社",
                        job="部長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name="IST",
                        last_login_at="2022-06-03T17:35:07.198Z",
                        disabled=False,
                    ),
                    AdminModel(
                        id="85d3cd05-ae7f-4724-a89e-f32a61d2bc04",
                        data_type=DataType.ADMIN,
                        name="佐藤花子",
                        email="user1@example.com",
                        roles=["supporter_mgr"],
                        company="ソニーグループ株式会社",
                        job="課長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name=None,
                        last_login_at="2022-06-03T17:36:07.198Z",
                        disabled=False,
                    ),
                ],
                {
                    "total": 2,
                    "admins": [
                        {
                            "id": "2992902b-c830-49b6-97a2-45ef1bafcf5c",
                            "name": "田中太郎",
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
                            "lastLoginAt": "2022-06-03T17:35:07.198+09:00",
                            "disabled": False,
                        },
                        {
                            "id": "85d3cd05-ae7f-4724-a89e-f32a61d2bc04",
                            "name": "佐藤花子",
                            "email": "user1@example.com",
                            "roles": ["supporter_mgr"],
                            "company": "ソニーグループ株式会社",
                            "job": "課長",
                            "supporterOrganizations": [
                                {
                                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    "name": "組織開発",
                                }
                            ],
                            "organizationName": None,
                            "lastLoginAt": "2022-06-03T17:36:07.198+09:00",
                            "disabled": False,
                        },
                    ],
                },
            )
        ],
    )
    def test_sort_asc_ok(self, mocker, mock_auth_admin, model_list, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(ProjectService, "get_supporter_organization_name")
        mock.return_value = "組織開発"
        mock = mocker.patch.object(AdminModel.data_type_last_login_at_index, "query")
        mock.return_value = model_list

        response = client.get(
            "/api/admins?sort=last_login_at:asc", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    AdminModel(
                        id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
                        data_type=DataType.ADMIN,
                        name="田中太郎",
                        email="user@example.com",
                        roles=["sales"],
                        company="ソニーグループ株式会社",
                        job="部長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name="IST",
                        last_login_at="2022-06-03T17:35:07.198Z",
                        disabled=False,
                    ),
                    AdminModel(
                        id="85d3cd05-ae7f-4724-a89e-f32a61d2bc04",
                        data_type=DataType.ADMIN,
                        name="佐藤花子",
                        email="user1@example.com",
                        roles=["supporter_mgr"],
                        company="ソニーグループ株式会社",
                        job="課長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name=None,
                        last_login_at="2022-06-03T17:34:07.198Z",
                        disabled=False,
                    ),
                ],
                {
                    "total": 2,
                    "admins": [
                        {
                            "id": "2992902b-c830-49b6-97a2-45ef1bafcf5c",
                            "name": "田中太郎",
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
                            "lastLoginAt": "2022-06-03T17:35:07.198+09:00",
                            "disabled": False,
                        },
                        {
                            "id": "85d3cd05-ae7f-4724-a89e-f32a61d2bc04",
                            "name": "佐藤花子",
                            "email": "user1@example.com",
                            "roles": ["supporter_mgr"],
                            "company": "ソニーグループ株式会社",
                            "job": "課長",
                            "supporterOrganizations": [
                                {
                                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    "name": "組織開発",
                                }
                            ],
                            "organizationName": None,
                            "lastLoginAt": "2022-06-03T17:34:07.198+09:00",
                            "disabled": False,
                        },
                    ],
                },
            )
        ],
    )
    def test_sort_desc_ok(self, mocker, mock_auth_admin, model_list, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(ProjectService, "get_supporter_organization_name")
        mock.return_value = "組織開発"
        mock = mocker.patch.object(AdminModel.data_type_last_login_at_index, "query")
        mock.return_value = model_list

        response = client.get(
            "/api/admins?sort=last_login_at:desc", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    def test_admin_zero(self, mocker, mock_auth_admin):
        """管理ユーザーが１件も存在しない時"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(AdminModel.data_type_last_login_at_index, "query")
        mock.return_value = []

        response = client.get("/api/admins", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == {"total": 0, "admins": []}

    def test_auth_for_sales(self, mock_auth_admin):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])

        response = client.get("/api/admins", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    def test_auth_for_supporter_mgr(self, mock_auth_admin):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        response = client.get("/api/admins", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    def test_auth_for_sales_mgr(self, mock_auth_admin):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])

        response = client.get("/api/admins", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    def test_auth_for_survey_ops(self, mock_auth_admin):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])

        response = client.get("/api/admins", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    def test_auth_for_man_hour_ops(self, mock_auth_admin):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])

        response = client.get("/api/admins", headers=REQUEST_HEADERS)

        assert response.status_code == 403
