import pytest
from app.main import app
from app.models.notification import NotificationModel
from fastapi.testclient import TestClient

client = TestClient(app)


class TestPatchCheckNotification:
    @pytest.mark.parametrize(
        "model_list,expected",
        [
            (
                [
                    # 特定のユーザーから取得したお知らせ情報を更新するため、user_idは一意にした
                    NotificationModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        user_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        summary="カルテを記入してください。",
                        url="https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        message="支援が完了したのでカルテを記入してください。",
                        confirmed=False,
                        noticed_at="2020-10-23T04:21:39.356000+0000",
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
                        noticed_at="2020-10-23T04:21:39.356000+0000",
                        ttl="1661658683.825009",
                        create_id="998c3cd7-63b5-453f-9de2-5af21d569956",
                        create_at="2020-10-24T04:21:39.356000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d569956",
                        update_at="2020-10-24T04:21:39.356000+0000",
                    ),
                    NotificationModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        user_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        summary="案件情報が更新されました。",
                        url="https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        message="支援が完了しました。",
                        confirmed=True,
                        noticed_at="2020-10-23T04:21:39.356000+0000",
                        ttl="1661658683.825009",
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2020-10-23T04:21:39.356000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2020-10-23T04:21:39.356000+0000",
                    ),
                ],
                {"message": "OK"},
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, model_list, expected):
        """正常系のテスト"""
        mock_notification = mocker.patch.object(
            NotificationModel.user_id_noticed_at_index, "query"
        )
        mock_notification.return_value = model_list
        mocker.patch.object(NotificationModel, "update")

        response = client.patch("/api/notifications")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
