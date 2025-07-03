from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.master import BatchControlAttribute, MasterBatchControlModel
from app.resources.const import MasterDataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetBatchControlById:
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
        batch_id = "partnerportal-backoffice-dev-batch_summary_man_hour"
        response = client.get(f"/api/masters/batch-control/{batch_id}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def setup_method(self, method):
        MasterBatchControlModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        MasterBatchControlModel(
            id="2ed6e959-c216-46a8-92c3-3d7d3f951d00",
            data_type=MasterDataType.BATCH_CONTROL.value,
            name="BO工数情報集計データ作成処理",
            value="partnerportal-backoffice-dev-batch_summary_man_hour",
            attributes=BatchControlAttribute(
                batch_start_at=datetime(2022, 8, 2, 22, 42, 5, 33000),
                batch_end_at=datetime(2022, 8, 3, 22, 42, 5, 33000),
                batch_status="executed",
                batch_rerun_span=10,
            ),
            update_at=datetime(2022, 8, 1, 22, 42, 5, 33000),
        ).save()

    def teardown_method(self, method):
        MasterBatchControlModel.delete_table()

    @pytest.mark.parametrize(
        "batch_id, expected",
        [
            (
                "partnerportal-backoffice-dev-batch_summary_man_hour",
                {
                    "batchEndAt": "2022-08-03T22:42:05.033+09:00",
                },
            )
        ],
    )
    def test_ok(self, mock_auth_admin, batch_id, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(
            f"api/masters/batch-control/{batch_id}", headers=REQUEST_HEADERS
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

        batch_id = "partnerportal-backoffice-dev-batch_summary_man_hour"

        response = client.get(
            f"api/masters/batch-control/{batch_id}", headers=REQUEST_HEADERS
        )

        if access_flag:
            assert response.status_code == 200
        else:
            assert response.status_code == 403

    def test_not_found(self, mock_auth_admin):
        """id が存在しないときのテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        batch_id = "222-c216-46a8-92c3-3d7d3f951d01"

        response = client.get(
            f"api/masters/batch-control/{batch_id}", headers=REQUEST_HEADERS
        )
        actual = response.json()

        assert response.status_code == 404
        assert actual["detail"] == "Not Found"
