import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestPatchUserStatusById:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.SURVEY_OPS.key],
            [UserRoleType.MAN_HOUR_OPS.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_not_access(
        self,
        mock_auth_admin,
        role_types,
    ):
        """システム管理者以外はアクセス不可"""
        mock_auth_admin(role_types)
        response = client.patch(
            "/api/users/1?version=1&enable=true",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, version, enable, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                True,
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="supporter",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    supporter_organization_id=["033bd0b5-c2c7-4778-a58d-76a46500f7d9"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=True,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "disabled": True,
                    "version": "1",
                },
            ),
        ],
    )
    def test_disabled_ok(
        self, mocker, mock_auth_admin, user_id, version, enable, model, expected
    ):
        """ユーザー無効化の成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model
        mocker.patch.object(UserModel, "update")

        response = client.patch(
            f"/api/users/{user_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, version, enable, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                False,
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="supporter",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    supporter_organization_id=["033bd0b5-c2c7-4778-a58d-76a46500f7d9"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "disabled": False,
                    "version": "1",
                },
            ),
        ],
    )
    def test_enable_ok(
        self, mocker, mock_auth_admin, user_id, version, enable, model, expected
    ):
        """ユーザー有効化の成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model
        mocker.patch.object(UserModel, "update")

        response = client.patch(
            f"/api/users/{user_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, version, enable",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                True,
            ),
        ],
    )
    def test_auth_for_sales(self, mock_auth_admin, user_id, version, enable):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])

        response = client.patch(
            f"/api/users/{user_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, version, enable",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                True,
            ),
        ],
    )
    def test_auth_for_supporter_mgr(self, mock_auth_admin, user_id, version, enable):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        response = client.patch(
            f"/api/users/{user_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, version, enable",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                True,
            ),
        ],
    )
    def test_auth_for_sales_mgr(self, mock_auth_admin, user_id, version, enable):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])

        response = client.patch(
            f"/api/users/{user_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, version, enable",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                True,
            ),
        ],
    )
    def test_auth_for_survey_ops(self, mock_auth_admin, user_id, version, enable):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])

        response = client.patch(
            f"/api/users/{user_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, version, enable",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                True,
            ),
        ],
    )
    def test_auth_for_man_hour_ops(self, mock_auth_admin, user_id, version, enable):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])

        response = client.patch(
            f"/api/users/{user_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    def test_validation_version(self, mock_auth_admin):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        self.user_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        self.enable = False

        response = client.patch(
            f"/api/users/{self.user_id}?enable={self.enable}", headers=REQUEST_HEADERS
        )
        assert response.status_code == 422

    def test_validation_enable(self, mock_auth_admin):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        self.user_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        self.version = 1

        response = client.patch(
            f"/api/users/{self.user_id}?version={self.version}", headers=REQUEST_HEADERS
        )
        assert response.status_code == 422
