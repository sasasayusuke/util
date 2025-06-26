import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.master import MasterSupporterOrganizationModel
from app.resources.const import UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestCreateMaster:
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
        response = client.post("/api/masters", headers=REQUEST_HEADERS)

        assert response.status_code == 422

    def setup_method(self, method):
        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

    def teardown_method(self, method):
        MasterSupporterOrganizationModel.delete_table()

    @pytest.mark.parametrize(
        "body ,expected",
        [
            (
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
                    "use": True,
                },
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
                    "order": 1,
                    "use": True,
                    "createId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "updateId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "version": 1,
                },
            )
        ],
    )
    def test_ok(self, mock_auth_admin, body, expected):
        # ###########################
        # モック化
        # ###########################
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        response = client.post("/api/masters", json=body, headers=REQUEST_HEADERS)

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        actual.pop("id")
        actual.pop("createAt")
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

        # ###########################
        # モック化
        # ###########################
        mock_auth_admin([role])

        # ###########################
        # テスト実行
        # ###########################
        body = {
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
            "use": True,
        }

        response = client.post("/api/masters", json=body, headers=REQUEST_HEADERS)

        # ###########################
        # 検証
        # ###########################
        if access_flag:
            assert response.status_code == 200
        else:
            assert response.status_code == 403

    def test_duplicate_data_type_and_value(self, mock_auth_admin):
        # ###########################
        # モック化
        # ###########################
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        body = {
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
            "use": True,
        }

        # 初回の登録
        client.post("/api/masters", json=body, headers=REQUEST_HEADERS)

        response = client.post("/api/masters", json=body, headers=REQUEST_HEADERS)

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 400

    @pytest.mark.parametrize(
        "body",
        [
            # ###########################
            # data_type
            # ###########################
            # 必須チェック
            {
                # 'dataType': 'master_supporter_organization',
                "name": "Ideation Service Team",
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
                "dataType": None,
                "name": "Ideation Service Team",
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
            # Enumチェック
            {
                "dataType": "supporter",
                "name": "Ideation Service Team",
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

        response = client.post("/api/masters", json=body, headers=REQUEST_HEADERS)
        assert response.status_code == 422
