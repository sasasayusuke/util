import uuid
from app.models.solver_corporation import SolverCorporationModel
from app.utils.aws.ses import SesHelper
import pytest
from app.main import app
from app.models.solver import SolverModel
from app.resources.const import DataType
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestUpdateUtilizationRate:
    @pytest.mark.usefixtures("auth_apt_user")
    def test_APTロールで稼働率と単価が更新できること(self, mocker):
        solver_id = str(uuid.uuid4())
        solver_corporate_id = str(uuid.uuid4())
        mock_solver_corporation = mocker.patch.object(SolverCorporationModel, "get")
        mock_solver_corporation.return_value = SolverCorporationModel(
            id=solver_corporate_id,
            data_type=DataType.SOLVER_CORPORATION,
            name="法人ソルバー1株式会社",
            update_at=None,
            version=1,
            utilization_rate_version=1
        )
        mocker.patch.object(SolverCorporationModel, "update")

        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id=solver_id,
            data_type=DataType.SOLVER,
            name="山田太郎",
            name_kana="ヤマダタロウ",
            corporate_id=solver_corporate_id,
            is_consulting_firm=True,
            is_solver=True,
            registration_status="new",
            create_id="a9b67094-cdab-494c-818e-d4845088269b",
            create_at="2020-10-23T03:21:39.356872Z",
            update_id="b9b67094-cdab-494c-818e-d4845088269b",
            update_at="2020-10-23T03:21:39.356872Z",
            version=1,
        )
        mocker.patch.object(SolverModel, "update")
        mocker.patch.object(SesHelper, "send_mail")

        request_body = {
            "utilizationRate": [
                {
                    "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca22656",
                    "name": "山田太郎",
                    "providedOperatingRate": 50,
                    "providedOperatingRateNext": 100,
                    "pricePerPersonMonth": 1000000,
                    "pricePerPersonMonthLower": 800000,
                    "hourlyRate": 10000,
                    "hourlyRateLower": 8000
                }
            ]
        }

        response = client.put(f"/api/solvers/utilization-rate/{solver_corporate_id}?version=1", json=request_body)

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"deleted": []}

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_法人ソルバーロールで稼働率と単価が更新できること(self, mocker):
        solver_id = str(uuid.uuid4())
        solver_corporate_id = "906a3144-9650-4a34-8a23-3b02f3b9a999"
        mock_solver_corporation = mocker.patch.object(SolverCorporationModel, "get")
        mock_solver_corporation.return_value = SolverCorporationModel(
            id=solver_corporate_id,
            data_type=DataType.SOLVER_CORPORATION,
            name="法人ソルバー1株式会社",
            update_at=None,
            version=1,
            utilization_rate_version=1
        )
        mocker.patch.object(SolverCorporationModel, "update")

        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id=solver_id,
            data_type=DataType.SOLVER,
            name="山田太郎",
            name_kana="ヤマダタロウ",
            corporate_id=solver_corporate_id,
            is_consulting_firm=True,
            is_solver=True,
            registration_status="new",
            create_id="a9b67094-cdab-494c-818e-d4845088269b",
            create_at="2020-10-23T03:21:39.356872Z",
            update_id="b9b67094-cdab-494c-818e-d4845088269b",
            update_at="2020-10-23T03:21:39.356872Z",
            version=1,
        )
        mocker.patch.object(SolverModel, "update")
        mocker.patch.object(SesHelper, "send_mail")

        request_body = {
            "utilizationRate": [
                {
                    "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca22656",
                    "name": "山田太郎",
                    "providedOperatingRate": 50,
                    "providedOperatingRateNext": 100,
                    "pricePerPersonMonth": 1000000,
                    "pricePerPersonMonthLower": 800000,
                    "hourlyRate": 10000,
                    "hourlyRateLower": 8000
                }
            ]
        }

        response = client.put(f"/api/solvers/utilization-rate/{solver_corporate_id}?version=1", json=request_body)

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"deleted": []}

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_法人ソルバーロールで自身の法人以外を指定すると403が返されること(self, mocker):
        solver_id = str(uuid.uuid4())
        solver_corporate_id = "906a3144-9650-4a34-8a23-3b02f3b9a999"
        mock_solver_corporation = mocker.patch.object(SolverCorporationModel, "get")
        mock_solver_corporation.return_value = SolverCorporationModel(
            id=solver_corporate_id,
            data_type=DataType.SOLVER_CORPORATION,
            name="法人ソルバー1株式会社",
            update_at=None,
            version=1,
            utilization_rate_version=1
        )
        mocker.patch.object(SolverCorporationModel, "update")

        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id=solver_id,
            data_type=DataType.SOLVER,
            name="山田太郎",
            name_kana="ヤマダタロウ",
            corporate_id=solver_corporate_id,
            is_consulting_firm=True,
            is_solver=True,
            registration_status="new",
            create_id="a9b67094-cdab-494c-818e-d4845088269b",
            create_at="2020-10-23T03:21:39.356872Z",
            update_id="b9b67094-cdab-494c-818e-d4845088269b",
            update_at="2020-10-23T03:21:39.356872Z",
            version=1,
        )
        mocker.patch.object(SolverModel, "update")
        mocker.patch.object(SesHelper, "send_mail")

        request_body = {
            "utilizationRate": [
                {
                    "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca22656",
                    "name": "山田太郎",
                    "providedOperatingRate": 50,
                    "providedOperatingRateNext": 100,
                    "pricePerPersonMonth": 1000000,
                    "pricePerPersonMonthLower": 800000,
                    "hourlyRate": 10000,
                    "hourlyRateLower": 8000
                }
            ]
        }

        response = client.put("/api/solvers/utilization-rate/99999999-9999-9999-9999-99999999?version=1", json=request_body)

        assert response.status_code == 403

    @pytest.mark.usefixtures("auth_apt_user")
    def test_稼働と単価率を更新する前にデータが削除されている場合に200でdeleted_userに値が設定された状態で返されること(self, mocker):
        solver_corporate_id = str(uuid.uuid4())
        mock_solver_corporation = mocker.patch.object(SolverCorporationModel, "get")
        mock_solver_corporation.return_value = SolverCorporationModel(
            id=solver_corporate_id,
            data_type=DataType.SOLVER_CORPORATION,
            name="法人ソルバー1株式会社",
            update_at=None,
            version=1,
            utilization_rate_version=1
        )

        mocker.patch.object(SolverCorporationModel, "update")

        mock = mocker.patch.object(SolverModel, "get")
        mock.side_effect = DoesNotExist()
        mocker.patch.object(SolverModel, "update")
        mocker.patch.object(SesHelper, "send_mail")

        request_body = {
            "utilizationRate": [
                {
                    "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca22656",
                    "name": "山田太郎",
                    "providedOperatingRate": 50,
                    "providedOperatingRateNext": 100,
                    "pricePerPersonMonth": 1000000,
                    "pricePerPersonMonthLower": 800000,
                    "hourlyRate": 10000,
                    "hourlyRateLower": 8000
                }
            ]
        }

        response = client.put(f"/api/solvers/utilization-rate/{solver_corporate_id}?version=1", json=request_body)
        actual = response.json()

        assert response.status_code == 200
        assert actual["deleted"] == ["山田太郎"]

    @pytest.mark.usefixtures("auth_apt_user")
    def test_該当の法人ソルバーが存在しない場合に404を返すこと(self, mocker):
        solver_corporation_id = str(uuid.uuid4())
        """正常系のテスト（APT）"""
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.side_effect = DoesNotExist()

        request_body = {
            "utilizationRate": [
                {
                    "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca22656",
                    "providedOperatingRate": 50,
                    "providedOperatingRateNext": 100,
                    "pricePerPersonMonth": 1000000,
                    "pricePerPersonMonthLower": 800000,
                    "hourlyRate": 10000,
                    "hourlyRateLower": 8000
                }
            ]
        }
        response = client.put(f"/api/solvers/utilization-rate]/{solver_corporation_id}", json=request_body)

        assert response.status_code == 404

    @pytest.mark.usefixtures("auth_apt_user")
    @pytest.mark.parametrize(
        "request_body",
        [
            {
                "utilizationRate": [
                    {
                        "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca22656",
                        "providedOperatingRate": 500,
                        "providedOperatingRateNext": 100,
                        "pricePerPersonMonth": 9999,
                        "pricePerPersonMonthLower": 10000,
                        "hourlyRate": 10000,
                        "hourlyRateLower": 8000
                    }
                ]
            },
            {
                "utilizationRate": [
                    {
                        "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca22656",
                        "providedOperatingRate": 500,
                        "providedOperatingRateNext": 100,
                        "pricePerPersonMonth": 10001,
                        "pricePerPersonMonthLower": 10000,
                        "hourlyRate": 999,
                        "hourlyRateLower": 1000
                    }
                ]
            }
        ],
    )
    def test_バリデーションチェックが機能し400が返されること(self, request_body):
        response = client.put("/api/solvers/utilization-rate/99999999-9999-9999-9999-99999999?version=1", json=request_body)

        assert response.status_code == 400
