import pytest
from app.main import app
from app.models.solver_corporation import SolverCorporationModel
from app.resources.const import DataType
from fastapi.testclient import TestClient

client = TestClient(app)


class TestGetSolverCorporations:
    @pytest.mark.parametrize(
        "disabled, sort, model_list, expected",
        [
            (
                False,
                "name:asc",
                [
                    SolverCorporationModel(
                        id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER_CORPORATION,
                        name="サンプル株式会社1",
                        update_at=None,
                    ),
                    SolverCorporationModel(
                        id="87cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER_CORPORATION,
                        name="サンプル株式会社2",
                        update_at=None,
                    ),
                    SolverCorporationModel(
                        id="99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER_CORPORATION,
                        name="サンプル株式会社3",
                        update_at="2020-10-23T03:21:39.356000+0000",
                    ),
                    SolverCorporationModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER_CORPORATION,
                        name="サンプル株式会社4",
                        update_at="2020-10-23T03:21:39.356000+0000",
                    ),
                ],
                {
                    "solverCorporations": [
                        {
                            "id": "98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "サンプル株式会社1",
                            "updateAt": None
                        },
                        {
                            "id": "87cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "サンプル株式会社2",
                            "updateAt": None
                        },
                        {
                            "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "サンプル株式会社3",
                            "updateAt": "2020-10-23T03:21:39.356+09:00",
                        },
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "サンプル株式会社4",
                            "updateAt": "2020-10-23T03:21:39.356+09:00",
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_get_solver_corporations_ok(self, mocker, disabled, sort, model_list, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(SolverCorporationModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get(f"/api/solver-corporations?disabled={disabled}&sort={sort}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "disabled, sort, model_list, expected",
        [
            (
                False,
                "name:asc",
                [],
                {"solverCorporations": []},
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_solver_corporation_zero(self, mocker, disabled, sort, model_list, expected):
        """法人ソルバーが1件も存在しない時"""
        mock = mocker.patch.object(SolverCorporationModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get(f"/api/solver-corporations?disabled={disabled}&sort={sort}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
