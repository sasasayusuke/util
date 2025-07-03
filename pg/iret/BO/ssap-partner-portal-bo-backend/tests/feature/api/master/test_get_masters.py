from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.master import MasterSupporterOrganizationModel
from app.resources.const import MasterDataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetMasters:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
            [UserRoleType.SALES_MGR.key],
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
        response = client.get("/api/masters", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def setup_method(self, method):
        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        MasterSupporterOrganizationModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
            name="Ideation Service Team",
            value="IST",
            order=3,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()
        MasterSupporterOrganizationModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
            data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
            name="Acceleration Service Team",
            value="AST",
            order=2,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()
        MasterSupporterOrganizationModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270f",
            data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
            name="組織開発",
            value="2072785c-6257-4b68-9bd0-da3a1c826265",
            order=1,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()

    def teardown_method(self, method):
        MasterSupporterOrganizationModel.delete_table()

    @pytest.mark.parametrize(
        "expected",
        [
            {
                "offsetPage": 1,
                "total": 3,
                "masters": [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270f",
                        "dataType": "master_service_type",
                        "name": "組織開発",
                        "value": "2072785c-6257-4b68-9bd0-da3a1c826265",
                        "order": 1,
                        "use": True,
                        "createAt": "2022-04-23T03:21:39.356+09:00",
                        "updateAt": "2022-04-23T03:21:39.356+09:00",
                    },
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                        "dataType": "master_supporter_organization",
                        "name": "Acceleration Service Team",
                        "value": "AST",
                        "order": 2,
                        "use": True,
                        "createAt": "2022-04-23T03:21:39.356+09:00",
                        "updateAt": "2022-04-23T03:21:39.356+09:00",
                    },
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "dataType": "master_supporter_organization",
                        "name": "Ideation Service Team",
                        "value": "IST",
                        "order": 3,
                        "use": True,
                        "createAt": "2022-04-23T03:21:39.356+09:00",
                        "updateAt": "2022-04-23T03:21:39.356+09:00",
                    },
                ],
            },
        ],
    )
    def test_ok(self, mock_auth_admin, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        response = client.get("/api/masters", headers=REQUEST_HEADERS)

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "role, access_flag",
        [
            (UserRoleType.SALES.key, False),
            (UserRoleType.SUPPORTER_MGR.key, False),
            (UserRoleType.SALES_MGR.key, True),
            (UserRoleType.SURVEY_OPS.key, True),
            (UserRoleType.MAN_HOUR_OPS.key, True),
        ],
    )
    def test_auth_ok(self, mock_auth_admin, role, access_flag):
        """ロール別権限テスト"""
        mock_auth_admin([role, access_flag])

        # ###########################
        # テスト実行
        # ###########################
        response = client.get("/api/masters", headers=REQUEST_HEADERS)

        # ###########################
        # 検証
        # ###########################
        if access_flag:
            assert response.status_code == 200
        else:
            assert response.status_code == 403

    def test_master_zero(self, mock_auth_admin):
        """マスターが１件も存在しない時"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        MasterSupporterOrganizationModel.delete_table()
        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        response = client.get("/api/masters", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == {"offsetPage": 1, "total": 0, "masters": []}

    def test_parameter_validation(self, mock_auth_admin):
        """クエリパラメータのバリデーションチェック"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get("/api/masters?dataType=test", headers=REQUEST_HEADERS)

        assert response.status_code == 422
