import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.admin import AdminModel
from app.models.notification import NotificationModel
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from app.service.project_service import ProjectService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetAdminByMine:
    @pytest.mark.parametrize(
        "admin_model, user_model, expected",
        [
            (
                AdminModel(
                    id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    data_type=DataType.ADMIN,
                    name="田中太郎",
                    email="user@example.com",
                    roles=["sales"],
                    company="ソニーグループ株式会社",
                    job="営業",
                    supporter_organization_id=[],
                    organization_name="IST",
                    disabled=False,
                ),
                iter(
                    [
                        UserModel(
                            id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
                            data_type=DataType.USER,
                            name="田中太郎",
                            email="user@example.com",
                            role=UserRoleType.SALES.key,
                            company="ソニーグループ株式会社",
                            organization_name="営業",
                            project_ids=[
                                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "79cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                            ],
                        )
                    ]
                ),
                {
                    "id": "2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    "name": "田中太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "営業",
                    "supporterOrganizations": None,
                    "organizationName": "IST",
                    "projectIds": [
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "79cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                    ],
                    "disabled": False,
                    "totalNotifications": 7,
                },
            ),
        ],
    )
    def test_auth_for_sales(
        self, mocker, mock_auth_admin, admin_model, user_model, expected
    ):
        """正常系のテスト（ロールが営業担当者の場合）"""
        mock_auth_admin([UserRoleType.SALES.key])
        mock = mocker.patch.object(NotificationModel, "get_total_notifications_count")
        mock.return_value = 7
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = admin_model

        user_model_mock = mocker.patch.object(UserModel.data_type_email_index, "query")
        user_model_mock.return_value = user_model

        response = client.get("/api/admins/me", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model, expected",
        [
            (
                AdminModel(
                    id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    data_type=DataType.ADMIN,
                    name="田中太郎",
                    email="user@example.com",
                    roles=["supporter_mgr"],
                    company="ソニーグループ株式会社",
                    job="部長",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    organization_name=None,
                    disabled=False,
                ),
                {
                    "id": "2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    "name": "田中太郎",
                    "email": "user@example.com",
                    "roles": ["supporter_mgr"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "組織開発"}
                    ],
                    "organizationName": None,
                    "projectIds": [],
                    "disabled": False,
                    "totalNotifications": 7,
                },
            ),
        ],
    )
    def test_auth_for_supporter_mgr(self, mocker, mock_auth_admin, model, expected):
        """正常系のテスト（ロールが支援者責任者の場合）"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])
        mock = mocker.patch.object(ProjectService, "get_supporter_organization_name")
        mock.return_value = "組織開発"
        mock = mocker.patch.object(NotificationModel, "get_total_notifications_count")
        mock.return_value = 7
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model

        response = client.get("/api/admins/me", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model, expected",
        [
            (
                AdminModel(
                    id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    data_type=DataType.ADMIN,
                    name="田中太郎",
                    email="user@example.com",
                    roles=["sales_mgr"],
                    company="ソニーグループ株式会社",
                    job="営業",
                    supporter_organization_id=[],
                    organization_name="IST",
                    disabled=False,
                ),
                {
                    "id": "2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    "name": "田中太郎",
                    "email": "user@example.com",
                    "roles": ["sales_mgr"],
                    "company": "ソニーグループ株式会社",
                    "job": "営業",
                    "supporterOrganizations": None,
                    "organizationName": "IST",
                    "projectIds": [],
                    "disabled": False,
                    "totalNotifications": 7,
                },
            ),
        ],
    )
    def test_auth_for_sales_mgr(self, mocker, mock_auth_admin, model, expected):
        """正常系のテスト（ロールが営業担当責任者の場合）"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])
        mock = mocker.patch.object(NotificationModel, "get_total_notifications_count")
        mock.return_value = 7
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model

        response = client.get("/api/admins/me", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model, expected",
        [
            (
                AdminModel(
                    id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    data_type=DataType.ADMIN,
                    name="田中太郎",
                    email="user@example.com",
                    roles=["survey_ops"],
                    company="ソニーグループ株式会社",
                    job="営業",
                    supporter_organization_id=[],
                    organization_name="IST",
                    disabled=False,
                ),
                {
                    "id": "2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    "name": "田中太郎",
                    "email": "user@example.com",
                    "roles": ["survey_ops"],
                    "company": "ソニーグループ株式会社",
                    "job": "営業",
                    "supporterOrganizations": None,
                    "organizationName": "IST",
                    "projectIds": [],
                    "disabled": False,
                    "totalNotifications": 7,
                },
            ),
        ],
    )
    def test_auth_for_survey_ops(self, mocker, mock_auth_admin, model, expected):
        """正常系のテスト（ロールがアンケート事務局の場合）"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])
        mock = mocker.patch.object(NotificationModel, "get_total_notifications_count")
        mock.return_value = 7
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model

        response = client.get("/api/admins/me", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model, expected",
        [
            (
                AdminModel(
                    id="2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    data_type=DataType.ADMIN,
                    name="田中太郎",
                    email="user@example.com",
                    roles=["man_hour_ops"],
                    company="ソニーグループ株式会社",
                    job="営業",
                    supporter_organization_id=[],
                    organization_name="IST",
                    disabled=False,
                ),
                {
                    "id": "2992902b-c830-49b6-97a2-45ef1bafcf5c",
                    "name": "田中太郎",
                    "email": "user@example.com",
                    "roles": ["man_hour_ops"],
                    "company": "ソニーグループ株式会社",
                    "job": "営業",
                    "supporterOrganizations": None,
                    "organizationName": "IST",
                    "projectIds": [],
                    "disabled": False,
                    "totalNotifications": 7,
                },
            ),
        ],
    )
    def test_auth_for_man_hour_ops(self, mocker, mock_auth_admin, model, expected):
        """正常系のテスト（ロールが稼働率調査事務局の場合）"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        mock = mocker.patch.object(NotificationModel, "get_total_notifications_count")
        mock.return_value = 7
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model

        response = client.get("/api/admins/me", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
