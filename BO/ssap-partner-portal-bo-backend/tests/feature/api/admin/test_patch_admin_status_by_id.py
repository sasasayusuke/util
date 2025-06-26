import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.admin import AdminModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestPatchAdminStatusById:
    @pytest.mark.parametrize(
        "admin_id, version, enable, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                True,
                AdminModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.ADMIN,
                    disabled=True,
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "disabled": True,
                    "version": "1",
                },
            )
        ],
    )
    def test_ok(
        self, mocker, mock_auth_admin, admin_id, version, enable, model, expected
    ):
        """管理ユーザー情報の削除（無効化）成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model
        mocker.patch.object(AdminModel, "update")

        response = client.patch(
            f"/api/admins/{admin_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "admin_id, version, enable",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", 1, False)],
    )
    def test_admin_not_found(self, mocker, mock_auth_admin, admin_id, version, enable):
        """管理ユーザーが存在しない時のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        admin_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock = mocker.patch.object(AdminModel, "get")
        mock.side_effect = DoesNotExist()
        mocker.patch.object(AdminModel, "update")

        response = client.patch(
            f"/api/admins/{admin_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "admin_id, version, enable, model ",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                False,
                AdminModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.ADMIN,
                    disabled=False,
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-10-23T03:21:39.356Z",
                    version="2",
                ),
            )
        ],
    )
    def test_admin_version_conflict(
        self, mocker, mock_auth_admin, admin_id, version, enable, model
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model
        mocker.patch.object(AdminModel, "update")

        response = client.patch(
            f"/api/admins/{admin_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"

    @pytest.mark.parametrize(
        "admin_id, version, enable",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", 1, False)],
    )
    def test_auth_for_sales(self, mock_auth_admin, admin_id, version, enable):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])

        response = client.patch(
            f"/api/admins/{admin_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id, version, enable",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", 1, False)],
    )
    def test_auth_for_supporter_mgr(self, mock_auth_admin, admin_id, version, enable):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        response = client.patch(
            f"/api/admins/{admin_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id, version, enable",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", 1, False)],
    )
    def test_auth_for_sales_mgr(self, mock_auth_admin, admin_id, version, enable):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])

        response = client.patch(
            f"/api/admins/{admin_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id, version, enable",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", 1, False)],
    )
    def test_auth_for_survey_ops(self, mock_auth_admin, admin_id, version, enable):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])

        response = client.patch(
            f"/api/admins/{admin_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id, version, enable",
        [("89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", 1, False)],
    )
    def test_auth_for_man_hour_ops(self, mock_auth_admin, admin_id, version, enable):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])

        response = client.patch(
            f"/api/admins/{admin_id}?version={version}&enable={enable}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403
