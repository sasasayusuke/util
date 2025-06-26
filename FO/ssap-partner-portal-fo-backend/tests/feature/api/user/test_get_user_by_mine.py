import pytest
from app.main import app
from app.models.notification import NotificationModel
from app.service.user_service import UserService
from fastapi.testclient import TestClient

client = TestClient(app)


class TestCreateUser:
    @pytest.mark.parametrize(
        "expected",
        [
            {
                "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                "name": "山田太郎",
                "email": "taro.yamada@example.com",
                "role": "customer",
                "customerId": "106a3144-9650-4a34-8a23-3b02f3b9aeac",
                "customerName": "取引先株式会社",
                "job": "部長",
                "company": None,
                'solverCorporationId': None,
                "supporterOrganizations": None,
                "organizationName": "コンサルティング事業部",
                "isInputManHour": None,
                "projectIds": [
                    "886a3144-9650-4a34-8a23-3b02f3b9aeac",
                ],
                "agreed": True,
                "lastLoginAt": None,
                "disabled": False,
                "totalNotifications": 3,
            },
        ],
    )
    @pytest.mark.usefixtures("auth_customer_user")
    def test_ok_for_customer(self, mocker, expected):
        """正常時のテスト（顧客）"""
        mock = mocker.patch.object(NotificationModel, "get_total_notifications_count")
        mock.return_value = 3

        response = client.get("/api/users/me")
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "expected",
        [
            {
                "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                "name": "山田太郎",
                "email": "taro.yamada@example.com",
                "role": "supporter",
                "customerId": None,
                "customerName": None,
                "job": None,
                "company": "テスト株式会社",
                'solverCorporationId': None,
                "supporterOrganizations": [
                    {"id": "556a3144-9650-4a34-8a23-3b02f3b9aeac", "name": "組織開発"}
                ],
                "organizationName": None,
                "isInputManHour": True,
                "projectIds": [
                    "886a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "126a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "236a3144-9650-4a34-8a23-3b02f3b9aeac",
                ],
                "agreed": True,
                "lastLoginAt": None,
                "disabled": False,
                "totalNotifications": 3,
            },
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok_for_supporter(self, mocker, expected):
        """正常時のテスト（顧客以外）"""
        mock = mocker.patch.object(UserService, "get_supporter_organization_name")
        mock.return_value = "組織開発"
        mock = mocker.patch.object(NotificationModel, "get_total_notifications_count")
        mock.return_value = 3

        response = client.get("/api/users/me")
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "expected",
        [
            {
                "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                "name": "山田太郎",
                "email": "taro.yamada@example.com",
                "role": "apt",
                "customerId": None,
                "customerName": None,
                "job": None,
                "company": "テスト株式会社",
                'solverCorporationId': None,
                "supporterOrganizations": None,
                "organizationName": None,
                "isInputManHour": True,
                "projectIds": [],
                "agreed": True,
                "lastLoginAt": None,
                "disabled": False,
                "totalNotifications": 3,
            },
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_for_apt(self, mocker, expected):
        """正常時のテスト（APT）"""
        mock = mocker.patch.object(NotificationModel, "get_total_notifications_count")
        mock.return_value = 3

        response = client.get("/api/users/me")
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "expected",
        [
            {
                "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                "name": "山田太郎",
                "email": "taro.yamada@example.com",
                "role": "solver_staff",
                "customerId": None,
                "customerName": None,
                "job": None,
                "company": "テスト株式会社",
                'solverCorporationId': "906a3144-9650-4a34-8a23-3b02f3b9a999",
                "supporterOrganizations": None,
                "organizationName": None,
                "isInputManHour": True,
                "projectIds": [],
                "agreed": True,
                "lastLoginAt": None,
                "disabled": False,
                "totalNotifications": 3,
            },
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_ok_for_solver_staff(self, mocker, expected):
        """正常時のテスト（法人ソルバー）"""
        mock = mocker.patch.object(NotificationModel, "get_total_notifications_count")
        mock.return_value = 3

        response = client.get("/api/users/me")
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected
