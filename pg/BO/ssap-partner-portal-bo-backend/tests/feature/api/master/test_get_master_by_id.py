from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.master import (
    MasterSupporterOrganizationModel,
    SupporterOrganizationAttribute,
)
from app.resources.const import MasterDataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetMasterById:
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
        master_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        response = client.get(f"/api/masters/{master_id}", headers=REQUEST_HEADERS)

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
            attributes=SupporterOrganizationAttribute(
                info1="予備情報１",
                info2="予備情報2",
                info3="予備情報3",
                info4="予備情報4",
                info5="予備情報5",
            ),
            order=3,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()

    def teardown_method(self, method):
        MasterSupporterOrganizationModel.delete_table()

    @pytest.mark.parametrize(
        "master_id, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "dataType": "master_supporter_organization",
                    "name": "Ideation Service Team",
                    "value": "IST",
                    "attributes": {
                        "info1": "予備情報１",
                        "info2": "予備情報2",
                        "info3": "予備情報3",
                        "info4": "予備情報4",
                        "info5": "予備情報5",
                    },
                    "order": 3,
                    "use": True,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": 1,
                },
            ),
        ],
    )
    def test_ok(self, mock_auth_admin, master_id, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(f"/api/masters/{master_id}", headers=REQUEST_HEADERS)

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
        mock_auth_admin([role])

        # ###########################
        # テスト実行
        # ###########################
        master_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        response = client.get(f"/api/masters/{master_id}", headers=REQUEST_HEADERS)

        # ###########################
        # 検証
        # ###########################
        if access_flag:
            assert response.status_code == 200
        else:
            assert response.status_code == 403

    def test_master_not_found(self, mocker, mock_auth_admin):
        """マスターが存在しない時のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        master_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270z"
        response = client.get(f"/api/masters/{master_id}", headers=REQUEST_HEADERS)

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
