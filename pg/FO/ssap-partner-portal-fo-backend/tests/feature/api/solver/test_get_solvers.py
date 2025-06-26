import pytest
from app.main import app
from app.models.solver import SolverModel
from app.resources.const import DataType
from fastapi.testclient import TestClient

client = TestClient(app)


class TestGetSolvers:
    @pytest.mark.parametrize(
        "query_param, solver_models, expected",
        [
            (
                "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&solver_type=solver&sex=man&certificationStatus=working&operatingStatus=working&sort=create_at:desc&offsetPage=1&limit=2",
                [
                    SolverModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人5",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人4",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1974/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人3",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人2",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人1",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                ],
                {
                    "offsetPage": 1,
                    "total": 5,
                    "solvers": [
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "テスト法人5",
                            'corporateId': '89cbe2ed-f44c-4a1c-9408-c67b0ca2270d',
                            "sex": "man",
                            "age": 32,
                            "birthDay": "1992/10/23",
                            "specializedThemes": "デジタルマーケティング",
                            "operatingStatus": "working",
                            "providedOperatingRate": 10,
                            "providedOperatingRateNext": 20,
                            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                            "pricePerPersonMonth": 20000,
                            "pricePerPersonMonthLower": 10000,
                            "hourlyRate": 20000,
                            "hourlyRateLower": 10000,
                            "registrationStatus": "certificated",
                            "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                            "version": 1,
                            'isSolver': True,
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "テスト法人4",
                            'corporateId': '89cbe2ed-f44c-4a1c-9408-c67b0ca2270d',
                            "sex": "man",
                            "age": 50,
                            "birthDay": "1974/10/23",
                            "specializedThemes": "デジタルマーケティング",
                            "operatingStatus": "working",
                            "providedOperatingRate": 10,
                            "providedOperatingRateNext": 20,
                            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                            "pricePerPersonMonth": 20000,
                            "pricePerPersonMonthLower": 10000,
                            "hourlyRate": 20000,
                            "hourlyRateLower": 10000,
                            "registrationStatus": "certificated",
                            "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                            "version": 1,
                            'isSolver': True,
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_1page(
        self,
        mocker,
        query_param,
        solver_models,
        expected,
    ):
        """権限 APT:制限なし"""
        """正常1ページ目  total:5件,limit=2,offsetPage=1"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_create_at_index, "query"
        )
        mock_project.return_value = solver_models

        response = client.get(f"/api/solvers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, solver_models, expected",
        [
            (
                "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&solver_type=solver_candidate&sex=man&certificationStatus=working&sort=operating_status:asc&offsetPage=2&limit=2",
                [
                    SolverModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人1",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人2",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1974/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人3",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人4",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1974/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人5",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                ],
                {
                    "offsetPage": 2,
                    "total": 5,
                    "solvers": [
                        {
                            "id": "33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "テスト法人3",
                            'corporateId': '89cbe2ed-f44c-4a1c-9408-c67b0ca2270d',
                            "sex": "man",
                            "age": 32,
                            "birthDay": "1992/10/23",
                            "specializedThemes": "デジタルマーケティング",
                            "operatingStatus": "working",
                            "providedOperatingRate": 10,
                            "providedOperatingRateNext": 20,
                            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                            "pricePerPersonMonth": 20000,
                            "pricePerPersonMonthLower": 10000,
                            "hourlyRate": 20000,
                            "hourlyRateLower": 10000,
                            "registrationStatus": "certificated",
                            "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                            "version": 1,
                            'isSolver': False,
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "テスト法人4",
                            'corporateId': '89cbe2ed-f44c-4a1c-9408-c67b0ca2270d',
                            "sex": "man",
                            "age": 50,
                            "birthDay": "1974/10/23",
                            "specializedThemes": "デジタルマーケティング",
                            "operatingStatus": "working",
                            "providedOperatingRate": 10,
                            "providedOperatingRateNext": 20,
                            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                            "pricePerPersonMonth": 20000,
                            "pricePerPersonMonthLower": 10000,
                            "hourlyRate": 20000,
                            "hourlyRateLower": 10000,
                            "registrationStatus": "certificated",
                            "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                            "version": 1,
                            'isSolver': False,
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_2page(
        self,
        mocker,
        query_param,
        solver_models,
        expected,
    ):
        """正常2ページ目  total:5件,limit=2,offsetPage=2"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_operating_status_index, "query"
        )
        mock_project.return_value = solver_models

        response = client.get(f"/api/solvers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, solver_models, expected",
        [
            (
                "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&solver_type=all&sex=man&certificationStatus=working&operatingStatus=working&sort=price_and_operating_rate_update_at:asc&offsetPage=3&limit=2",
                [
                    SolverModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人1",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人2",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1974/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人3",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人4",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1974/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人5",
                        corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                ],
                {
                    "offsetPage": 3,
                    "total": 5,
                    "solvers": [
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "テスト法人5",
                            'corporateId': '89cbe2ed-f44c-4a1c-9408-c67b0ca2270d',
                            "sex": "man",
                            "age": 32,
                            "birthDay": "1992/10/23",
                            "specializedThemes": "デジタルマーケティング",
                            "operatingStatus": "working",
                            "providedOperatingRate": 10,
                            "providedOperatingRateNext": 20,
                            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                            "pricePerPersonMonth": 20000,
                            "pricePerPersonMonthLower": 10000,
                            "hourlyRate": 20000,
                            "hourlyRateLower": 10000,
                            "registrationStatus": "certificated",
                            "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                            "version": 1,
                            'isSolver': True,
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_3page(
        self,
        mocker,
        query_param,
        solver_models,
        expected,
    ):
        """正常3ページ目  total:5件,limit=2,offsetPage=3"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_price_and_operating_rate_update_at_index, "query"
        )
        mock_project.return_value = solver_models

        response = client.get(f"/api/solvers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, solver_models, expected",
        [
            (
                "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&solver_type=solver&sex=man&certificationStatus=working&operatingStatus=working&sort=create_at:desc&offsetPage=1&limit=2",
                [],
                {
                    "offsetPage": 1,
                    "total": 0,
                    "solvers": [],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_zero(
        self,
        mocker,
        query_param,
        solver_models,
        expected,
    ):
        """正常0件  total:0,limit=2,offsetPage=1"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_create_at_index, "query"
        )
        mock_project.return_value = solver_models

        response = client.get(f"/api/solvers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, solver_models, expected",
        [
            (
                "?id=906a3144-9650-4a34-8a23-3b02f3b9a999&solver_type=all&sex=man&certificationStatus=working&operatingStatus=working&sort=create_at:desc&offsetPage=1&limit=2",
                [
                    SolverModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人5",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人4",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1974/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人3",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人2",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人1",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=False,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                ],
                {
                    "offsetPage": 1,
                    "total": 5,
                    "solvers": [
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "テスト法人5",
                            'corporateId': '906a3144-9650-4a34-8a23-3b02f3b9a999',
                            "sex": "man",
                            "age": 32,
                            "birthDay": "1992/10/23",
                            "specializedThemes": "デジタルマーケティング",
                            "operatingStatus": "working",
                            "providedOperatingRate": 10,
                            "providedOperatingRateNext": 20,
                            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                            "pricePerPersonMonth": 20000,
                            "pricePerPersonMonthLower": 10000,
                            "hourlyRate": 20000,
                            "hourlyRateLower": 10000,
                            "registrationStatus": "certificated",
                            "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                            "version": 1,
                            'isSolver': False,
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "テスト法人4",
                            'corporateId': '906a3144-9650-4a34-8a23-3b02f3b9a999',
                            "sex": "man",
                            "age": 50,
                            "birthDay": "1974/10/23",
                            "specializedThemes": "デジタルマーケティング",
                            "operatingStatus": "working",
                            "providedOperatingRate": 10,
                            "providedOperatingRateNext": 20,
                            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                            "pricePerPersonMonth": 20000,
                            "pricePerPersonMonthLower": 10000,
                            "hourlyRate": 20000,
                            "hourlyRateLower": 10000,
                            "registrationStatus": "certificated",
                            "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                            "version": 1,
                            'isSolver': False,
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_auth_ok_solver_staff(
        self,
        mocker,
        query_param,
        solver_models,
        expected,
    ):
        """権限 法人ソルバー:制限なし"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_create_at_index, "query"
        )
        mock_project.return_value = solver_models

        response = client.get(f"/api/solvers{query_param}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, solver_models",
        [
            (
                "?id=111a3144-9650-4a34-8a23-3b02f3b9a999&solver_type=solver&sex=man&certificationStatus=working&operatingStatus=working&sort=create_at:desc&offsetPage=1&limit=2",
                [
                    SolverModel(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人5",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人4",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1974/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="33cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人3",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人2",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                    SolverModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SOLVER,
                        name="テスト法人1",
                        corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                        sex="man",
                        birth_day="1992/10/23",
                        specialized_themes="デジタルマーケティング",
                        operating_status="working",
                        provided_operating_rate=10,
                        provided_operating_rate_next=20,
                        operation_prospects_month_after_next="再来月の稼働見込みあり",
                        price_per_person_month=20000,
                        price_per_person_month_lower=10000,
                        hourly_rate=20000,
                        hourly_rate_lower=10000,
                        is_solver=True,
                        registration_status="certificated",
                        price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        version=1
                    ),
                ],
            )
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_auth_error_solver_staff_403(
        self,
        mocker,
        query_param,
        solver_models,
    ):
        """権限 法人ソルバー:アクセス不可"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_create_at_index, "query"
        )
        mock_project.return_value = solver_models

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=create_at:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_enum_create_at_ok(
        self,
        mocker,
        query_param,
    ):
        """sort:create_atのENUM確認"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_create_at_index, "query"
        )
        mock_project.return_value = [
            SolverModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人1",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1992/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
            SolverModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人2",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1974/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
        ]

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=sex:asc",
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=sex:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_enum_sex_ok(
        self,
        mocker,
        query_param,
    ):
        """sort:sexのENUM確認"""
        mock_project = mocker.patch.object(SolverModel.data_type_sex_index, "query")
        mock_project.return_value = [
            SolverModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人1",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1992/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
            SolverModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人2",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1974/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
        ]

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=birth_day:asc",
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=birth_day:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_enum_birth_day_ok(
        self,
        mocker,
        query_param,
    ):
        """sort:birth_dayのENUM確認"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_birth_day_index, "query"
        )
        mock_project.return_value = [
            SolverModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人1",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1992/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
            SolverModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人2",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1974/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
        ]

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=operating_status:asc",
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=operating_status:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_enum_operating_status_ok(
        self,
        mocker,
        query_param,
    ):
        """sort:operating_statusのENUM確認"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_operating_status_index, "query"
        )
        mock_project.return_value = [
            SolverModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人1",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1992/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
            SolverModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人2",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1974/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
        ]

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=provided_operating_rate:asc",
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=provided_operating_rate:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_enum_provided_operating_rate_ok(
        self,
        mocker,
        query_param,
    ):
        """sort:provided_operating_rateのENUM確認"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_provided_operating_rate_index, "query"
        )
        mock_project.return_value = [
            SolverModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人1",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1992/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
            SolverModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人2",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1974/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
        ]

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=provided_operating_rate_next:asc",
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=provided_operating_rate_next:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_enum_provided_operating_rate_next_ok(
        self,
        mocker,
        query_param,
    ):
        """sort:provided_operating_rate_nextのENUM確認"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_provided_operating_rate_next_index, "query"
        )
        mock_project.return_value = [
            SolverModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人1",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1992/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
            SolverModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人2",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1974/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
        ]

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=price_per_person_month:asc",
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=price_per_person_month:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_enum_price_per_person_month_ok(
        self,
        mocker,
        query_param,
    ):
        """sort:price_per_person_monthのENUM確認"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_price_per_person_month_index, "query"
        )
        mock_project.return_value = [
            SolverModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人1",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1992/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
            SolverModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人2",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1974/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
        ]

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=registration_status:asc",
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=registration_status:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_enum_registration_status_ok(
        self,
        mocker,
        query_param,
    ):
        """sort:registration_statusのENUM確認"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_registration_status_index, "query"
        )
        mock_project.return_value = [
            SolverModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人1",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1992/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
            SolverModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人2",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1974/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
        ]

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=price_and_operating_rate_update_at:asc",
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=price_and_operating_rate_update_at:desc",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_enum_price_and_operating_rate_update_at_ok(
        self,
        mocker,
        query_param,
    ):
        """sort:price_and_operating_rate_update_atのENUM確認"""
        mock_project = mocker.patch.object(
            SolverModel.data_type_price_and_operating_rate_update_at_index, "query"
        )
        mock_project.return_value = [
            SolverModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人1",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1992/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
            SolverModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER,
                name="テスト法人2",
                corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                sex="man",
                birth_day="1974/10/23",
                specialized_themes="デジタルマーケティング",
                operating_status="working",
                provided_operating_rate=10,
                provided_operating_rate_next=20,
                operation_prospects_month_after_next="再来月の稼働見込みあり",
                price_per_person_month=20000,
                price_per_person_month_lower=10000,
                hourly_rate=20000,
                hourly_rate_lower=10000,
                is_solver=True,
                registration_status="certificated",
                price_and_operating_rate_update_at="2020-10-23T03:21:39.356000+0000",
                create_at="2020-10-23T03:21:39.356000+0000",
                version=1
            ),
        ]

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "query_param",
        [
            "?id=89cbe2ed-f44c-4a1c-9408-c67b0ca2270d&sort=create_at:aaa",
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_validation_enum(
        self,
        mocker,
        query_param,
    ):
        """sortのENUMバリデーションエラー"""

        response = client.get(f"/api/solvers{query_param}")

        assert response.status_code == 422
