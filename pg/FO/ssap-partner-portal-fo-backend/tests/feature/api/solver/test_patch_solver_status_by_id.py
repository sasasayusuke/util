import pytest
from app.main import app
from app.models.solver import SolverModel
from app.resources.const import DataType
from pynamodb.exceptions import DoesNotExist
from fastapi.testclient import TestClient

client = TestClient(app)


class TestPatchSolverStatusById:
    @pytest.mark.parametrize(
        "solver_id, version, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                SolverModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER,
                    corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                    is_solver=False,
                    registration_status="new",
                    version=1,
                ),
                {'message': 'OK'},
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_apt_ok(self, mocker, model, solver_id, version, expected):
        """正常系のテスト（APT）"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = model
        mocker.patch.object(SolverModel, "update")

        response = client.patch(
            f"api/solvers/{solver_id}?version={version}"
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "solver_id, version, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                SolverModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER,
                    corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                    is_solver=True,
                    registration_status="new",
                    version=1,
                ),
                {'message': 'OK'},
            )
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_solver_staff_ok(
        self, mocker, model, solver_id, version, expected
    ):
        """正常系のテスト（法人ソルバー）"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = model
        mocker.patch.object(SolverModel, "update")

        response = client.patch(
            f"api/solvers/{solver_id}?version={version}"
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "solver_id, version",
        [
            (
                "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_solver_not_found(
        self, mocker, solver_id, version
    ):
        """個人ソルバーが存在しない時のテスト"""
        solver_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock = mocker.patch.object(SolverModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.patch(
            f"api/solvers/{solver_id}?version={version}"
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "solver_id, version, model",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                SolverModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER,
                    corporate_id="666a3144-9650-4a34-8a23-3b02f3b9a999",
                    is_solver=False,
                    registration_status="new",
                    version=1,
                ),
            )
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_solver_forbidden(
        self, mocker, model, solver_id, version
    ):
        """個別ソルバーにアクセス不可の時のテスト"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = model
        mocker.patch.object(SolverModel, "update")

        response = client.patch(
            f"api/solvers/{solver_id}?version={version}"
        )

        actual = response.json()
        assert response.status_code == 403
        assert actual["detail"] == "Forbidden"

    @pytest.mark.parametrize(
        "solver_id, version, model",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                3,
                SolverModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER,
                    corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                    is_solver=False,
                    registration_status="new",
                    version=1,
                ),
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_solver_conflict(
        self, mocker, model, solver_id, version
    ):
        """バージョンが異なる時のテスト"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = model
        mocker.patch.object(SolverModel, "update")

        response = client.patch(
            f"api/solvers/{solver_id}?version={version}"
        )

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"
