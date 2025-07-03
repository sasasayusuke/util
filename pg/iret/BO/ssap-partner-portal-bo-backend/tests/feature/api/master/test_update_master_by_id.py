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
class TestUpdateMasterById:
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
        version = 1
        response = client.put(f"/api/masters/{master_id}?version={version}", headers=REQUEST_HEADERS)

        assert response.status_code == 422

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
        "master_id, version, body, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "name": "Ideation Service Team",
                    "value": "IST",
                    "attributes": {
                        "info1": "予備情報１",
                        "info2": "予備情報2",
                        "info3": "予備情報3",
                        "info4": "予備情報4",
                        "info5": "予備情報5",
                    },
                    "use": True,
                },
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
                    "updateId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "version": 2,
                },
            ),
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, master_id, version, body, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        response = client.put(
            f"/api/masters/{master_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        actual.pop("updateAt")

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
        version = 1
        body = {
            "name": "Ideation Service Team",
            "value": "IST",
            "attributes": {
                "info1": "予備情報１",
                "info2": "予備情報2",
                "info3": "予備情報3",
                "info4": "予備情報4",
                "info5": "予備情報5",
            },
            "use": False,
        }

        response = client.put(
            f"/api/masters/{master_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        if access_flag:
            assert response.status_code == 200
        else:
            assert response.status_code == 403

    def test_duplicate_data_type_and_value(self, mock_auth_admin):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        master_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1
        body = {
            "name": "Ideation Service Team",
            "value": "IST",
            "attributes": {
                "info1": "予備情報１",
                "info2": "予備情報2",
                "info3": "予備情報3",
                "info4": "予備情報4",
                "info5": "予備情報5",
            },
            "use": True,
        }

        MasterSupporterOrganizationModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
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

        response = client.put(
            f"/api/masters/{master_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 400
        assert actual["detail"] == "Data type and value is already exist."

    def test_master_not_found(self, mock_auth_admin):
        """マスターが存在しない時のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        master_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270e"
        version = 1
        body = {
            "name": "Ideation Service Team",
            "value": "IST",
            "attributes": {
                "info1": "予備情報１",
                "info2": "予備情報2",
                "info3": "予備情報3",
                "info4": "予備情報4",
                "info5": "予備情報5",
            },
            "use": True,
        }

        response = client.put(
            f"/api/masters/{master_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    def test_version_conflict(self, mocker, mock_auth_admin):
        """楽観ロック制御テスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        master_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2
        body = {
            "name": "Ideation Service Team",
            "value": "IST",
            "attributes": {
                "info1": "予備情報１",
                "info2": "予備情報2",
                "info3": "予備情報3",
                "info4": "予備情報4",
                "info5": "予備情報5",
            },
            "use": True,
        }

        response = client.put(
            f"/api/masters/{master_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"

    @pytest.mark.parametrize(
        "body",
        [
            # ###########################
            # name
            # ###########################
            # 必須チェック
            {
                "dataType": "master_supporter_organization",
                # 'name': 'Ideation Service Team',
                "value": "IST",
                "attributes": {
                    "info1": "info1",
                    "info2": "info2",
                    "info3": "info3",
                    "info4": "info4",
                    "info5": "info5",
                },
                "use": True,
                "version": 1,
            },
            # Noneチェック
            {
                "dataType": "master_supporter_organization",
                "name": None,
                "value": "IST",
                "attributes": {
                    "info1": "info1",
                    "info2": "info2",
                    "info3": "info3",
                    "info4": "info4",
                    "info5": "info5",
                },
                "use": True,
                "version": 1,
            },
            # ###########################
            # value
            # ###########################
            # 必須チェック
            {
                "dataType": "master_supporter_organization",
                "name": "Ideation Service Team",
                # 'value': 'IST',
                "attributes": {
                    "info1": "info1",
                    "info2": "info2",
                    "info3": "info3",
                    "info4": "info4",
                    "info5": "info5",
                },
                "use": True,
                "version": 1,
            },
            # Noneチェック
            {
                "dataType": "master_supporter_organization",
                "name": "Ideation Service Team",
                "value": None,
                "attributes": {
                    "info1": "info1",
                    "info2": "info2",
                    "info3": "info3",
                    "info4": "info4",
                    "info5": "info5",
                },
                "use": True,
                "version": 1,
            },
            # ###########################
            # use
            # ###########################
            # 必須チェック
            {
                "dataType": "master_supporter_organization",
                "name": "Ideation Service Team",
                "value": "IST",
                "attributes": {
                    "info1": "info1",
                    "info2": "info2",
                    "info3": "info3",
                    "info4": "info4",
                    "info5": "info5",
                },
                # 'use': True,
                "version": 1,
            },
            # Noneチェック
            {
                "dataType": "master_supporter_organization",
                "name": "Ideation Service Team",
                "value": "IST",
                "attributes": {
                    "info1": "info1",
                    "info2": "info2",
                    "info3": "info3",
                    "info4": "info4",
                    "info5": "info5",
                },
                "use": None,
                "version": 1,
            },
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        master_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        response = client.put(
            f"/api/masters/{master_id}?{version}", json=body, headers=REQUEST_HEADERS
        )
        assert response.status_code == 422
