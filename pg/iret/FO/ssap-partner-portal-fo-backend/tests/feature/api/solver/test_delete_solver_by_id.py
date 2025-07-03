import pytest
from app.main import app
from app.models.solver import SolverModel
from app.resources.const import DataType, UserRoleType
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestDeleteSolverById:
    @pytest.mark.usefixtures("auth_apt_user")
    def test_APTロールで削除できること(self, mocker):
        solver_id = "A1cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            name_kana="ヤマダタロウ",
            corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            is_consulting_firm=True,
            is_solver=True,
            registration_status="new",
            create_id="a9b67094-cdab-494c-818e-d4845088269b",
            create_at="2020-10-23T03:21:39.356872Z",
            update_id="b9b67094-cdab-494c-818e-d4845088269b",
            update_at="2020-10-23T03:21:39.356872Z",
            version=1,
        )
        mocker.patch.object(SolverModel, "delete")

        response = client.delete(f"/api/solvers/{solver_id}?version=1")

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"message": "OK"}

    @pytest.mark.usefixtures("auth_apt_user")
    def test_排他チェックがされること(self, mocker):
        solver_id = "A1cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            name_kana="ヤマダタロウ",
            corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            is_consulting_firm=True,
            is_solver=True,
            registration_status="new",
            create_id="a9b67094-cdab-494c-818e-d4845088269b",
            create_at="2020-10-23T03:21:39.356872Z",
            update_id="b9b67094-cdab-494c-818e-d4845088269b",
            update_at="2020-10-23T03:21:39.356872Z",
            version=1,
        )

        response = client.delete(f"/api/solvers/{solver_id}?version=2")

        assert response.status_code == 409

    @pytest.mark.usefixtures("auth_apt_user")
    def test_存在しないIDの場合404が返されること(self, mocker):
        solver_id = "99999999-f44c-4a1c-9408-c67b0ca2270d"
        mock = mocker.patch.object(SolverModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.delete(f"/api/solvers/{solver_id}?version=1")

        assert response.status_code == 404

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_法人ソルバーロールで同じ法人のソルバーを削除できること(self, mocker):
        solver_id = "A1cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            name_kana="ヤマダタロウ",
            corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
            is_consulting_firm=True,
            is_solver=True,
            registration_status="new",
            create_id="a9b67094-cdab-494c-818e-d4845088269b",
            create_at="2020-10-23T03:21:39.356872Z",
            update_id="b9b67094-cdab-494c-818e-d4845088269b",
            update_at="2020-10-23T03:21:39.356872Z",
            version=1,
        )
        mocker.patch.object(SolverModel, "delete")

        response = client.delete(f"/api/solvers/{solver_id}?version=1")

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"message": "OK"}

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_法人ソルバーロールで違う法人のソルバーを削除しようとすると403が返されること(self, mocker):
        solver_id = "A1cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            name_kana="ヤマダタロウ",
            corporate_id="99999999-9999-9999-9999-99999999",
            is_consulting_firm=True,
            is_solver=True,
            registration_status="new",
            create_id="a9b67094-cdab-494c-818e-d4845088269b",
            create_at="2020-10-23T03:21:39.356872Z",
            update_id="b9b67094-cdab-494c-818e-d4845088269b",
            update_at="2020-10-23T03:21:39.356872Z",
            version=1,
        )

        response = client.delete(f"/api/solvers/{solver_id}?version=1")

        assert response.status_code == 403
