import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.master import MasterSupporterOrganizationModel
from app.resources.const import MasterDataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetSupporterOrganizations:
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
        response = client.get("/api/masters/supporter-organizations", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def setup_method(self, method):
        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        MasterSupporterOrganizationModel(
            id="180a3597-b7e7-42c8-902c-a29016afa662",
            data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
            name="Ideation Service Team",
            value="IST",
            order=1,
            use=True,
        ).save()
        MasterSupporterOrganizationModel(
            id="de40733f-6be9-4fef-8229-01052f43c1e2",
            data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
            name="Acceleration Service Team",
            value="AST",
            order=2,
            use=True,
        ).save()

    def teardown_method(self, method):
        MasterSupporterOrganizationModel.delete_table()

    @pytest.mark.parametrize(
        "expected",
        [
            {
                "supporterOrganizations": [
                    {
                        "id": "180a3597-b7e7-42c8-902c-a29016afa662",
                        "name": "Ideation Service Team",
                        "shortName": "IST",
                    },
                    {
                        "id": "de40733f-6be9-4fef-8229-01052f43c1e2",
                        "name": "Acceleration Service Team",
                        "shortName": "AST",
                    },
                ],
            },
        ],
    )
    def test_ok(self, mock_auth_admin, expected):
        """支援者組織の一覧を取得の成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(
            "/api/masters/supporter-organizations", headers=REQUEST_HEADERS
        )

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

        response = client.get(
            "/api/masters/supporter-organizations", headers=REQUEST_HEADERS
        )

        if access_flag:
            assert response.status_code == 200
        else:
            assert response.status_code == 403
