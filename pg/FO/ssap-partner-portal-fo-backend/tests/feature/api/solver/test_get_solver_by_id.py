import pytest
from app.main import app
from app.models.user import UserModel
from app.models.solver import SolverModel
from app.models.solver_application import SolverApplicationModel
from app.resources.const import DataType, UserRoleType
from app.schemas.solver import (
    SolverApplications,
    IssueMap50,
    FacePhoto,
    Resume,
    Screening,
)
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestGetSolverCorporationById:
    @pytest.mark.parametrize(
        "solver_id, solver_model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                SolverModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER,
                    name="山田太郎",
                    name_kana="ヤマダタロウ",
                    solver_application_ids=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    sex="man",
                    birth_day="2020/10/23",
                    email="yamada@example.com",
                    phone="00-0000-0000",
                    english_level="読み書き程度",
                    work_history="コンサル10年目",
                    is_consulting_firm=True,
                    specialized_themes="デジタルマーケティング",
                    academic_background="大学卒",
                    title="リーダー",
                    face_photo=FacePhoto(
                        file_name="ファイルA", path="http://www.example.com"
                    ),
                    resume=[
                        Resume(
                            file_name="ファイルA", path="http://www.example.com"
                        )
                    ],
                    operating_status="稼働中",
                    provided_operating_rate=10,
                    provided_operating_rate_next=20,
                    operation_prospects_month_after_next="再来月の稼働見込みあり",
                    price_per_person_month=20000,
                    price_per_person_month_lower=10000,
                    hourly_rate=20000,
                    hourly_rate_lower=10000,
                    price_and_operating_rate_update_at="2020-10-23T03:21:39.356872Z",
                    price_and_operating_rate_update_by="ソニー太郎",
                    tsi_areas=["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    issue_map50=[
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                    ],
                    main_achievements="特になし",
                    screening_1=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_2=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_3=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_4=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_5=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_6=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_7=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_8=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    criteria_1="特になし",
                    criteria_2="特になし",
                    criteria_3="特になし",
                    criteria_4="特になし",
                    criteria_5="特になし",
                    criteria_6="特になし",
                    criteria_7="特になし",
                    criteria_8="特になし",
                    notes="特になし",
                    is_solver=True,
                    registration_status="new",
                    create_id="a9b67094-cdab-494c-818e-d4845088269b",
                    create_at="2020-10-23T03:21:39.356872Z",
                    update_id="b9b67094-cdab-494c-818e-d4845088269b",
                    update_at="2020-10-23T03:21:39.356872Z",
                    version=1,
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplications": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                            "name": "ソルバー案件",
                            "projectCode": "ODljYmUyZWQtZjQ0Yy00YTFjLTk0MDgtYzY3YjBjYTIyMjIyO-OCveODq-ODkOODvOahiOS7tjsyZDYyMzVlN2ZlNTBjODlkNjY0YTJjYzhmMjE2MTc5ZTJlYjc5NGE3MzFkM2Q4ZGJkZTRiZjY3YjMwZDM0NWMx"
                        }
                    ],
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": [
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                    ],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "稼働中",
                    "facePhoto": {
                        "fileName": "ファイルA",
                        "path": "http://www.example.com",
                    },
                    "resume": [
                        {"fileName": "ファイルA", "path": "http://www.example.com"}
                    ],
                    "academicBackground": "大学卒",
                    "workHistory": "コンサル10年目",
                    "isConsultingFirm": True,
                    "specializedThemes": "デジタルマーケティング",
                    "mainAchievements": "特になし",
                    "providedOperatingRate": 10,
                    "providedOperatingRateNext": 20,
                    "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                    "pricePerPersonMonth": 20000,
                    "pricePerPersonMonthLower": 10000,
                    "hourlyRate": 20000,
                    "hourlyRateLower": 10000,
                    "englishLevel": "読み書き程度",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "screening1": {"evaluation": False, "evidence": "特になし"},
                    "screening2": {"evaluation": False, "evidence": "特になし"},
                    "screening3": {"evaluation": False, "evidence": "特になし"},
                    "screening4": {"evaluation": False, "evidence": "特になし"},
                    "screening5": {"evaluation": False, "evidence": "特になし"},
                    "screening6": {"evaluation": False, "evidence": "特になし"},
                    "screening7": {"evaluation": False, "evidence": "特になし"},
                    "screening8": {"evaluation": False, "evidence": "特になし"},
                    "criteria1": "特になし",
                    "criteria2": "特になし",
                    "criteria3": "特になし",
                    "criteria4": "特になし",
                    "criteria5": "特になし",
                    "criteria6": "特になし",
                    "criteria7": "特になし",
                    "criteria8": "特になし",
                    "notes": "特になし",
                    "isSolver": True,
                    "registrationStatus": "new",
                    "createId": "a9b67094-cdab-494c-818e-d4845088269b",
                    "createUserName": "テストAPT",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                    "priceAndOperatingRateUpdateBy": "ソニー太郎",
                    "updateId": "b9b67094-cdab-494c-818e-d4845088269b",
                    "updateUserName": "テスト法人ソルバー",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 1,
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_auth_ok_apt(
        self,
        mocker,
        solver_id,
        solver_model,
        expected,
    ):
        """正常系のテスト（APT）"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = solver_model

        mock = mocker.patch.object(SolverApplicationModel, "batch_get")
        mock.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                name="ソルバー案件",
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222"
            ),
        ]

        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.side_effect = [
            UserModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="テストAPT",
                email="user@example.com",
                role=UserRoleType.APT,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company="ソニーグループ株式会社",
                solver_corporation_id=None,
                supporter_organization_id=None,
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
            UserModel(
                id="b9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="テスト法人ソルバー",
                email="user@example.com",
                role=UserRoleType.SOLVER_STAFF,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company="ソニーグループ株式会社",
                solver_corporation_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                supporter_organization_id=None,
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
        ]

        response = client.get(f"/api/solvers/{solver_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "solver_id, solver_model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                SolverModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER,
                    name="山田太郎",
                    name_kana="ヤマダタロウ",
                    solver_application_ids=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                    sex="man",
                    birth_day="2020/10/23",
                    email="yamada@example.com",
                    phone="00-0000-0000",
                    english_level="読み書き程度",
                    work_history="コンサル10年目",
                    is_consulting_firm=True,
                    specialized_themes="デジタルマーケティング",
                    academic_background="大学卒",
                    title="リーダー",
                    face_photo=FacePhoto(
                        file_name="ファイルA", path="http://www.example.com"
                    ),
                    resume=[
                        Resume(
                            file_name="ファイルA", path="http://www.example.com"
                        )
                    ],
                    operating_status="稼働中",
                    provided_operating_rate=10,
                    provided_operating_rate_next=20,
                    operation_prospects_month_after_next="再来月の稼働見込みあり",
                    price_per_person_month=20000,
                    price_per_person_month_lower=10000,
                    hourly_rate=20000,
                    hourly_rate_lower=10000,
                    price_and_operating_rate_update_at="2020-10-23T03:21:39.356872Z",
                    price_and_operating_rate_update_by="ソニー太郎",
                    tsi_areas=["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    issue_map50=[
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                    ],
                    main_achievements="特になし",
                    screening_1=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_2=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_3=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_4=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_5=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_6=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_7=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_8=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    criteria_1="特になし",
                    criteria_2="特になし",
                    criteria_3="特になし",
                    criteria_4="特になし",
                    criteria_5="特になし",
                    criteria_6="特になし",
                    criteria_7="特になし",
                    criteria_8="特になし",
                    notes="特になし",
                    is_solver=True,
                    registration_status="new",
                    create_id="a9b67094-cdab-494c-818e-d4845088269b",
                    create_at="2020-10-23T03:21:39.356872Z",
                    update_id="b9b67094-cdab-494c-818e-d4845088269b",
                    update_at="2020-10-23T03:21:39.356872Z",
                    version=1,
                    file_key_id='89cbe2ed-f44c-4a1c-9408-c67b0ca00000'
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplications": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                            "name": "ソルバー案件",
                            "projectCode": "ODljYmUyZWQtZjQ0Yy00YTFjLTk0MDgtYzY3YjBjYTIyMjIyO-OCveODq-ODkOODvOahiOS7tjsyZDYyMzVlN2ZlNTBjODlkNjY0YTJjYzhmMjE2MTc5ZTJlYjc5NGE3MzFkM2Q4ZGJkZTRiZjY3YjMwZDM0NWMx"
                        }
                    ],
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": [
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                    ],
                    "corporateId": "906a3144-9650-4a34-8a23-3b02f3b9a999",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "稼働中",
                    "facePhoto": {
                        "fileName": "ファイルA",
                        "path": "http://www.example.com",
                    },
                    "resume": [
                        {"fileName": "ファイルA", "path": "http://www.example.com"}
                    ],
                    "academicBackground": "大学卒",
                    "workHistory": "コンサル10年目",
                    "isConsultingFirm": True,
                    "specializedThemes": "デジタルマーケティング",
                    "mainAchievements": "特になし",
                    "providedOperatingRate": 10,
                    "providedOperatingRateNext": 20,
                    "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                    "pricePerPersonMonth": 20000,
                    "pricePerPersonMonthLower": 10000,
                    "hourlyRate": 20000,
                    "hourlyRateLower": 10000,
                    "englishLevel": "読み書き程度",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "screening1": {"evaluation": False, "evidence": "特になし"},
                    "screening2": {"evaluation": False, "evidence": "特になし"},
                    "screening3": {"evaluation": False, "evidence": "特になし"},
                    "screening4": {"evaluation": False, "evidence": "特になし"},
                    "screening5": {"evaluation": False, "evidence": "特になし"},
                    "screening6": {"evaluation": False, "evidence": "特になし"},
                    "screening7": {"evaluation": False, "evidence": "特になし"},
                    "screening8": {"evaluation": False, "evidence": "特になし"},
                    "criteria1": "特になし",
                    "criteria2": "特になし",
                    "criteria3": "特になし",
                    "criteria4": "特になし",
                    "criteria5": "特になし",
                    "criteria6": "特になし",
                    "criteria7": "特になし",
                    "criteria8": "特になし",
                    "notes": "特になし",
                    "isSolver": True,
                    "registrationStatus": "new",
                    "createId": "a9b67094-cdab-494c-818e-d4845088269b",
                    "createUserName": "テストAPT",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                    "priceAndOperatingRateUpdateBy": "ソニー太郎",
                    "updateId": "b9b67094-cdab-494c-818e-d4845088269b",
                    "updateUserName": "テスト法人ソルバー",
                    "updateAt": "2020-10-23T03:21:39.356+09:00",
                    "version": 1,
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca00000",
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_auth_ok_solver_staff(
        self,
        mocker,
        solver_id,
        solver_model,
        expected,
    ):
        """正常系のテスト（法人ソルバー）"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = solver_model

        mock = mocker.patch.object(SolverApplicationModel, "batch_get")
        mock.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                name="ソルバー案件",
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222"
            ),
        ]

        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.side_effect = [
            UserModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="テストAPT",
                email="user@example.com",
                role=UserRoleType.APT,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company="ソニーグループ株式会社",
                solver_corporation_id=None,
                supporter_organization_id=None,
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
            UserModel(
                id="b9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="テスト法人ソルバー",
                email="user@example.com",
                role=UserRoleType.SOLVER_STAFF,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company="ソニーグループ株式会社",
                solver_corporation_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                supporter_organization_id=None,
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
        ]

        response = client.get(f"/api/solvers/{solver_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "solver_id, solver_model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                SolverModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER,
                    name="山田太郎",
                    name_kana="ヤマダタロウ",
                    solver_application_ids=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    corporate_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    sex="man",
                    birth_day="2020/10/23",
                    email="yamada@example.com",
                    phone="00-0000-0000",
                    english_level="読み書き程度",
                    work_history="コンサル10年目",
                    is_consulting_firm=True,
                    specialized_themes="デジタルマーケティング",
                    academic_background="大学卒",
                    title="リーダー",
                    face_photo=FacePhoto(
                        file_name="ファイルA", path="http://www.example.com"
                    ),
                    resume=[
                        Resume(
                            file_name="ファイルA", path="http://www.example.com"
                        )
                    ],
                    operating_status="稼働中",
                    provided_operating_rate=10,
                    provided_operating_rate_next=20,
                    operation_prospects_month_after_next="再来月の稼働見込みあり",
                    price_per_person_month=20000,
                    price_per_person_month_lower=10000,
                    hourly_rate=20000,
                    hourly_rate_lower=10000,
                    price_and_operating_rate_update_at="2020-10-23T03:21:39.356872Z",
                    price_and_operating_rate_update_by="ソニー太郎",
                    tsi_areas=["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    issue_map50=[
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                    ],
                    main_achievements="特になし",
                    screening_1=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_2=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_3=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_4=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_5=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_6=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_7=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_8=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    criteria_1="特になし",
                    criteria_2="特になし",
                    criteria_3="特になし",
                    criteria_4="特になし",
                    criteria_5="特になし",
                    criteria_6="特になし",
                    criteria_7="特になし",
                    criteria_8="特になし",
                    notes="特になし",
                    is_solver=True,
                    registration_status="new",
                    create_id="a9b67094-cdab-494c-818e-d4845088269b",
                    create_at="2020-10-23T03:21:39.356872Z",
                    update_id=None,
                    update_at=None,
                    version=1,
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplications": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                            "name": "ソルバー案件",
                            "projectCode": "ODljYmUyZWQtZjQ0Yy00YTFjLTk0MDgtYzY3YjBjYTIyMjIyO-OCveODq-ODkOODvOahiOS7tjsyZDYyMzVlN2ZlNTBjODlkNjY0YTJjYzhmMjE2MTc5ZTJlYjc5NGE3MzFkM2Q4ZGJkZTRiZjY3YjMwZDM0NWMx"
                        }
                    ],
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": [
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                    ],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "稼働中",
                    "facePhoto": {
                        "fileName": "ファイルA",
                        "path": "http://www.example.com",
                    },
                    "resume": [
                        {"fileName": "ファイルA", "path": "http://www.example.com"}
                    ],
                    "academicBackground": "大学卒",
                    "workHistory": "コンサル10年目",
                    "isConsultingFirm": True,
                    "specializedThemes": "デジタルマーケティング",
                    "mainAchievements": "特になし",
                    "providedOperatingRate": 10,
                    "providedOperatingRateNext": 20,
                    "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
                    "pricePerPersonMonth": 20000,
                    "pricePerPersonMonthLower": 10000,
                    "hourlyRate": 20000,
                    "hourlyRateLower": 10000,
                    "englishLevel": "読み書き程度",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "screening1": {"evaluation": False, "evidence": "特になし"},
                    "screening2": {"evaluation": False, "evidence": "特になし"},
                    "screening3": {"evaluation": False, "evidence": "特になし"},
                    "screening4": {"evaluation": False, "evidence": "特になし"},
                    "screening5": {"evaluation": False, "evidence": "特になし"},
                    "screening6": {"evaluation": False, "evidence": "特になし"},
                    "screening7": {"evaluation": False, "evidence": "特になし"},
                    "screening8": {"evaluation": False, "evidence": "特になし"},
                    "criteria1": "特になし",
                    "criteria2": "特になし",
                    "criteria3": "特になし",
                    "criteria4": "特になし",
                    "criteria5": "特になし",
                    "criteria6": "特になし",
                    "criteria7": "特になし",
                    "criteria8": "特になし",
                    "notes": "特になし",
                    "isSolver": True,
                    "registrationStatus": "new",
                    "createId": "a9b67094-cdab-494c-818e-d4845088269b",
                    "createUserName": "テストAPT",
                    "createAt": "2020-10-23T03:21:39.356+09:00",
                    "priceAndOperatingRateUpdateAt": "2020-10-23T03:21:39.356+09:00",
                    "priceAndOperatingRateUpdateBy": "ソニー太郎",
                    "updateId": "",
                    "updateUserName": "",
                    "updateAt": None,
                    "version": 1,
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_only_create_ok(
        self,
        mocker,
        solver_id,
        solver_model,
        expected,
    ):
        """正常系のテスト（作成のみ）"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = solver_model

        mock = mocker.patch.object(SolverApplicationModel, "batch_get")
        mock.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                name="ソルバー案件",
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222"
            ),
        ]

        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.side_effect = [
            UserModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="テストAPT",
                email="user@example.com",
                role=UserRoleType.APT,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company="ソニーグループ株式会社",
                solver_corporation_id=None,
                supporter_organization_id=None,
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
        ]

        response = client.get(f"/api/solvers/{solver_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "solver_id, solver_model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                SolverModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER,
                    name="山田太郎",
                    name_kana="ヤマダタロウ",
                    solver_application_ids=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    corporate_id="116a3144-9650-4a34-8a23-3b02f3b9a999",
                    sex="man",
                    birth_day="2020/10/23",
                    email="yamada@example.com",
                    phone="00-0000-0000",
                    english_level="読み書き程度",
                    work_history="コンサル10年目",
                    is_consulting_firm=True,
                    specialized_themes="デジタルマーケティング",
                    academic_background="大学卒",
                    title="リーダー",
                    face_photo=FacePhoto(
                        file_name="ファイルA", path="http://www.example.com"
                    ),
                    resume=[
                        Resume(
                            file_name="ファイルA", path="http://www.example.com"
                        )
                    ],
                    operating_status="稼働中",
                    provided_operating_rate=10,
                    provided_operating_rate_next=20,
                    operation_prospects_month_after_next="再来月の稼働見込みあり",
                    price_per_person_month=20000,
                    price_per_person_month_lower=10000,
                    hourly_rate=20000,
                    hourly_rate_lower=10000,
                    price_and_operating_rate_update_at="2020-10-23T03:21:39.356872Z",
                    price_and_operating_rate_update_by="ソニー太郎",
                    tsi_areas=["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    issue_map50=[
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                    ],
                    main_achievements="特になし",
                    screening_1=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_2=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_3=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_4=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_5=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_6=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_7=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    screening_8=Screening(
                        evaluation=False, evidence="特になし"
                    ),
                    criteria_1="特になし",
                    criteria_2="特になし",
                    criteria_3="特になし",
                    criteria_4="特になし",
                    criteria_5="特になし",
                    criteria_6="特になし",
                    criteria_7="特になし",
                    criteria_8="特になし",
                    notes="特になし",
                    is_solver=True,
                    registration_status="new",
                    create_id="a9b67094-cdab-494c-818e-d4845088269b",
                    create_at="2020-10-23T03:21:39.356872Z",
                    update_id="b9b67094-cdab-494c-818e-d4845088269b",
                    update_at="2020-10-23T03:21:39.356872Z",
                    version=1,
                ),
                {
                    "detail": "Forbidden",
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_auth_error_solver_staff_403(
        self,
        mocker,
        solver_id,
        solver_model,
        expected,
    ):
        """権限確認: 法人ソルバー: アクセス不可"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = solver_model

        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.side_effect = [
            UserModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="テストAPT",
                email="user@example.com",
                role=UserRoleType.APT,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company="ソニーグループ株式会社",
                solver_corporation_id=None,
                supporter_organization_id=None,
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
            UserModel(
                id="b9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="テスト法人ソルバー",
                email="user@example.com",
                role=UserRoleType.SOLVER_STAFF,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company="ソニーグループ株式会社",
                solver_corporation_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                supporter_organization_id=None,
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
        ]

        response = client.get(f"/api/solvers/{solver_id}")

        actual = response.json()
        assert response.status_code == 403
        assert actual == expected

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_solver_corporation_not_found(
        self,
        mocker,
    ):
        """個人ソルバーが存在しない時のテスト"""
        solver_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        mock = mocker.patch.object(SolverModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.get(f"/api/solvers/{solver_id}")

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
