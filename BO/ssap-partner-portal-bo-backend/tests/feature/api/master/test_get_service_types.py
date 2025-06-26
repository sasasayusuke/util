import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.master import MasterSupporterOrganizationModel
from app.resources.const import MasterDataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetServiceTypes:
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
        response = client.get("/api/masters/service-types", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def setup_method(self, method):
        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        MasterSupporterOrganizationModel(
            id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
            name="組織開発",
            order=1,
            use=True,
        ).save()
        MasterSupporterOrganizationModel(
            id="bad0b57c-de29-48e0-9deb-94be98437441",
            data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
            name="アイデア可視化",
            order=2,
            use=True,
        ).save()
        MasterSupporterOrganizationModel(
            id="1cdf118f-662d-4ae1-8765-f87f94ba345b",
            data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
            name="事業育成＆検証",
            order=3,
            use=True,
        ).save()

    def teardown_method(self, method):
        MasterSupporterOrganizationModel.delete_table()

    @pytest.mark.parametrize(
        "expected",
        [
            {
                "serviceTypes": [
                    {"id": "7ac8bddf-88da-46c9-a504-a03d1661ad58", "name": "組織開発"},
                    {
                        "id": "bad0b57c-de29-48e0-9deb-94be98437441",
                        "name": "アイデア可視化",
                    },
                    {
                        "id": "1cdf118f-662d-4ae1-8765-f87f94ba345b",
                        "name": "事業育成＆検証",
                    },
                ],
            },
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, expected):
        """サービス種別一覧情報の取得成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get("/api/masters/service-types", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "role, access_flag",
        [
            (UserRoleType.SALES.key, True),
            (UserRoleType.SUPPORTER_MGR.key, True),
            (UserRoleType.SALES_MGR.key, True),
            (UserRoleType.SURVEY_OPS.key, True),
            (UserRoleType.MAN_HOUR_OPS.key, True),
        ],
    )
    def test_auth_ok(self, mock_auth_admin, role, access_flag):
        """ロール別権限テスト"""
        mock_auth_admin([role])

        response = client.get("/api/masters/service-types", headers=REQUEST_HEADERS)

        if access_flag:
            assert response.status_code == 200
        else:
            assert response.status_code == 403
