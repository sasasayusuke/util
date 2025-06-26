import pytest
from app.main import app
from app.models.user import UserModel
from fastapi.testclient import TestClient

client = TestClient(app)


class TestPatchUserByMine:
    @pytest.mark.usefixtures("auth_user")
    def test_ok(self, mocker):
        """正常時のテスト"""

        mocker.patch.object(UserModel, "update")

        body = {"agreed": True}

        response = client.patch("/api/users/me", json=body)
        actual = response.json()

        assert response.status_code == 200
        assert actual["message"] == "OK"

    @pytest.mark.usefixtures("auth_user")
    def test_validation(self):
        """バリデーションテスト（必須チェック）"""

        response = client.patch("/api/users/me", json={})
        assert response.status_code == 422
