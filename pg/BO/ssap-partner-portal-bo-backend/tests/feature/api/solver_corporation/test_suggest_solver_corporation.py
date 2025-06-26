from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.solver_corporation import SolverCorporationModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestSuggestSolverCorporations:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
        ]
    )
    def test_access_ok(
        self,
        mock_auth_admin,
        role_types,
    ):
        mock_auth_admin(role_types)
        response = client.get(
            "/api/solvers/corporations/suggest?sort=name:asc&disabled=false", headers=REQUEST_HEADERS
        )

        assert response.status_code == 200

    def setup_method(self, method):
        SolverCorporationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        customer_models = [
            SolverCorporationModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_CORPORATION,
                name="サンプル法人01",
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            ),
            SolverCorporationModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_CORPORATION,
                name="サンプル法人02",
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            ),
            SolverCorporationModel(
                id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_CORPORATION,
                name="サンプル法人03",
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            ),
        ]

        for customer_model in customer_models:
            customer_model.save()

    def teardown_method(self, method):
        SolverCorporationModel.delete_table()

    @pytest.mark.parametrize(
        "expected",
        [
            [
                {
                    "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "サンプル法人01",
                },
                {
                    "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "サンプル法人02",
                },
                {
                    "id": "33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "サンプル法人03",
                },
            ],
        ],
    )
    def test_ok(self, mock_auth_admin, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(
            "/api/solvers/corporations/suggest?sort=name:asc&disabled=false", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "role",
        [
            (UserRoleType.SYSTEM_ADMIN.key),
        ],
    )
    def test_auth_ok(self, mock_auth_admin, role):
        """ロール別権限テスト"""
        mock_auth_admin([role])

        response = client.get(
            "/api/solvers/corporations/suggest?sort=name:asc&disabled=false", headers=REQUEST_HEADERS
        )
        assert response.status_code == 200

    def test_solver_corporation_zero(self, mock_auth_admin):
        """取引先が１件も存在しない時"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        SolverCorporationModel.delete_table()
        SolverCorporationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        response = client.get(
            "/api/solvers/corporations/suggest?sort=name:asc&disabled=false", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == []

    def test_validation_sort(self, mock_auth_admin):
        """sort指定のバリデーションチェック"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        sort = "name:desc"

        response = client.get(
            f"/api/solvers/corporations/suggest?sort={sort}&disabled=false", headers=REQUEST_HEADERS
        )
        assert response.status_code == 422
