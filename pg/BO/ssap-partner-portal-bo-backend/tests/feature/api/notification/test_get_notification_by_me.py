from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.notification import NotificationModel
from app.resources.const import UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetNotificationByMe:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.SURVEY_OPS.key],
            [UserRoleType.MAN_HOUR_OPS.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_access_ok(
        self,
        mock_auth_admin,
        role_types,
    ):
        mock_auth_admin(role_types)
        response = client.get("/api/notifications/me", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def setup_method(self, method):
        NotificationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        NotificationModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            summary="カルテを記入してください。",
            url="https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            message="支援が完了したのでカルテを記入してください。",
            confirmed=False,
            noticed_at=datetime(2020, 10, 23, 3, 21, 39, 356872),
            ttl="1661658683.825009",
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
        ).save()
        NotificationModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca22333",
            user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            summary="案件情報が新規登録されました。",
            url="https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca22333",
            message="以下の案件情報が新規登録されました。",
            confirmed=False,
            noticed_at=datetime(2020, 10, 22, 3, 21, 39, 356872),
            ttl="1661658683.825009",
            create_id="998c3cd7-63b5-453f-9de2-5af21d569956",
            create_at=datetime(2020, 10, 24, 4, 21, 39, 356000),
            update_id="998c3cd7-63b5-453f-9de2-5af21d569956",
            update_at=datetime(2020, 10, 24, 4, 21, 39, 356000),
        ).save()
        NotificationModel(
            id="89cbe2ed-f44c-4a1c-9408-c65614564270d",
            user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            summary="案件情報が更新されました。",
            url="https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c65614564270d",
            message="支援が完了しました。",
            confirmed=True,
            noticed_at=datetime(2020, 10, 21, 3, 21, 39, 356872),
            ttl="1661658683.825009",
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
        ).save()

    def teardown_method(self, method):
        NotificationModel.delete_table()

    @pytest.mark.parametrize(
        "expected",
        [
            [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "userId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "summary": "カルテを記入してください。",
                    "url": "https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "message": "支援が完了したのでカルテを記入してください。",
                    "confirmed": False,
                    "noticedAt": "2020-10-23T03:21:39.356+09:00",
                },
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22333",
                    "userId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "summary": "案件情報が新規登録されました。",
                    "url": "https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca22333",
                    "message": "以下の案件情報が新規登録されました。",
                    "confirmed": False,
                    "noticedAt": "2020-10-22T03:21:39.356+09:00",
                },
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c65614564270d",
                    "userId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "summary": "案件情報が更新されました。",
                    "url": "https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c65614564270d",
                    "message": "支援が完了しました。",
                    "confirmed": True,
                    "noticedAt": "2020-10-21T03:21:39.356+09:00",
                },
            ],
        ],
    )
    def test_ok(self, mock_auth_admin, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get("/api/notifications/me", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "role",
        [
            (UserRoleType.SALES.key),
            (UserRoleType.SUPPORTER_MGR.key),
            (UserRoleType.SALES_MGR.key),
            (UserRoleType.SURVEY_OPS.key),
            (UserRoleType.MAN_HOUR_OPS.key),
        ],
    )
    def test_auth_ok(self, mock_auth_admin, role):
        """ロール別権限テスト"""
        mock_auth_admin([role])

        expected = [
            {
                "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "userId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                "summary": "カルテを記入してください。",
                "url": "https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "message": "支援が完了したのでカルテを記入してください。",
                "confirmed": False,
                "noticedAt": "2020-10-23T03:21:39.356+09:00",
            },
            {
                "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22333",
                "userId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                "summary": "案件情報が新規登録されました。",
                "url": "https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca22333",
                "message": "以下の案件情報が新規登録されました。",
                "confirmed": False,
                "noticedAt": "2020-10-22T03:21:39.356+09:00",
            },
            {
                "id": "89cbe2ed-f44c-4a1c-9408-c65614564270d",
                "userId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                "summary": "案件情報が更新されました。",
                "url": "https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c65614564270d",
                "message": "支援が完了しました。",
                "confirmed": True,
                "noticedAt": "2020-10-21T03:21:39.356+09:00",
            },
        ]

        response = client.get("/api/notifications/me", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
