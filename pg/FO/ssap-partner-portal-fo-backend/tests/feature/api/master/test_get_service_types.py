import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.master import MasterSupporterOrganizationModel

client = TestClient(app)


class TestGetServiceTypes:
    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    MasterSupporterOrganizationModel(
                        id="7ac8bddf-88da-46c9-a504-a03d1661ad58", name="組織開発"
                    ),
                    MasterSupporterOrganizationModel(
                        id="bad0b57c-de29-48e0-9deb-94be98437441", name="アイデア可視化"
                    ),
                    MasterSupporterOrganizationModel(
                        id="1cdf118f-662d-4ae1-8765-f87f94ba345b", name="事業育成＆検証"
                    ),
                ],
                {
                    "serviceTypes": [
                        {"id": "7ac8bddf-88da-46c9-a504-a03d1661ad58", "name": "組織開発"},
                        {"id": "bad0b57c-de29-48e0-9deb-94be98437441", "name": "アイデア可視化"},
                        {"id": "1cdf118f-662d-4ae1-8765-f87f94ba345b", "name": "事業育成＆検証"},
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, model_list, expected):
        """サービス種別一覧情報の取得成功"""
        mock = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_order_index, "query"
        )
        mock.return_value = model_list

        response = client.get("/api/masters/service-types")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
