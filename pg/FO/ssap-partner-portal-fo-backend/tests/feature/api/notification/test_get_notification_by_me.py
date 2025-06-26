import pytest
from app.main import app
from app.models.notification import NotificationModel
from fastapi.testclient import TestClient

client = TestClient(app)


class TestGetNotificationByMe:
    @pytest.mark.parametrize(
        "model_list,expected",
        [
            (
                [
                    NotificationModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        user_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        summary="カルテを記入してください。",
                        url="https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        message="支援が完了したのでカルテを記入してください。",
                        confirmed=False,
                        noticed_at="2020-10-23T03:21:39.356872Z",
                        ttl="1661658683.825009",
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2020-10-23T04:21:39.356000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2020-10-23T04:21:39.356000+0000",
                    ),
                    NotificationModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        user_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        summary="案件情報が更新されました。",
                        url="https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        message="支援が完了しました。",
                        confirmed=True,
                        noticed_at="2020-10-22T03:21:39.356872Z",
                        ttl="1661658683.825009",
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2020-10-23T04:21:39.356000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2020-10-23T04:21:39.356000+0000",
                    ),
                    NotificationModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        user_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        summary="案件情報が新規登録されました。",
                        url="https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        message="以下の案件情報が新規登録されました。",
                        confirmed=False,
                        noticed_at="2020-10-21T03:21:39.356872Z",
                        ttl="1661658683.825009",
                        create_id="998c3cd7-63b5-453f-9de2-5af21d569956",
                        create_at="2020-10-24T04:21:39.356000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d569956",
                        update_at="2020-10-24T04:21:39.356000+0000",
                    ),
                ],
                [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "userId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "summary": "カルテを記入してください。",
                        "url": "https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "message": "支援が完了したのでカルテを記入してください。",
                        "confirmed": False,
                        "noticedAt": "2020-10-23T03:21:39.356+09:00",
                    },
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "userId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "summary": "案件情報が更新されました。",
                        "url": "https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "message": "支援が完了しました。",
                        "confirmed": True,
                        "noticedAt": "2020-10-22T03:21:39.356+09:00",
                    },
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "userId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "summary": "案件情報が新規登録されました。",
                        "url": "https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "message": "以下の案件情報が新規登録されました。",
                        "confirmed": False,
                        "noticedAt": "2020-10-21T03:21:39.356+09:00",
                    },
                ],
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, model_list, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(NotificationModel.user_id_noticed_at_index, "query")
        mock.return_value = model_list

        response = client.get("/api/notifications/me")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
