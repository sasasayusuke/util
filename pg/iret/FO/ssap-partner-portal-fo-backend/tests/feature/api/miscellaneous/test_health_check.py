import pytest
from app.main import app
from fastapi.testclient import TestClient

client = TestClient(app)


class TestHealthCheck:
    @pytest.mark.parametrize(
        "expected",
        [
            {
                "results": ["Status OK"],
            },
        ],
    )
    def test_ok(self, expected):
        """正常系のテスト"""
        response = client.get("/api/misc/health-check")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
