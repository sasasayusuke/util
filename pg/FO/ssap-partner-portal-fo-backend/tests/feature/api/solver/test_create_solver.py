import pytest
from app.main import app
from app.models.admin import AdminModel
from app.models.solver import SolverModel
from app.models.solver_application import SolverApplicationModel
from app.resources.const import DataType, UserRoleType
from app.service.master_service import MasterService
from app.service.solver_service import SolverService
from app.utils.aws.ses import SesHelper
from fastapi.testclient import TestClient

client = TestClient(app)


class TestCreateSolver:
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_create_candidate_for_apt(self, mocker):
        """正常系のテスト（APT、新規ソルバー候補申請、1人）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_solver_application = mocker.patch.object(SolverApplicationModel, "query")
        mock_solver_application.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mocker.patch.object(SolverApplicationModel, "save")

        mocker.patch.object(SolverModel, "save")

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            }
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        mode = "create_candidate"
        body = {
            "solversInfo": [
                {
                    "mode": mode,
                    "is_registered_solver": False,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                }
            ]
        }

        response = client.post("/api/solvers", json=body)

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"message": "OK"}

    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_create_multiple_candidate(self, mocker):
        """正常系のテスト（APT、新規ソルバー候補申請、複数人登録）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_solver_application = mocker.patch.object(SolverApplicationModel, "query")
        mock_solver_application.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mocker.patch.object(SolverApplicationModel, "save")

        mocker.patch.object(SolverModel, "save")

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            }
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        mode = "create_candidate"
        body = {
            "solversInfo": [
                {
                    "mode": mode,
                    "is_registered_solver": False,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                },
                {
                    "mode": mode,
                    "is_registered_solver": False,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                }
            ]
        }

        response = client.post("/api/solvers", json=body)

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"message": "OK"}

    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_create_existing_candidate(self, mocker):
        """正常系のテスト（APT、既存のソルバー候補申請）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_solver_application = mocker.patch.object(SolverApplicationModel, "query")
        mock_solver_application.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mocker.patch.object(SolverApplicationModel, "save")

        mock_solver = mocker.patch.object(SolverModel, "get")
        mock_solver.return_value = SolverModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            # ソルバー候補のテストのため、False
            is_solver=False,
            registration_status="new",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        mocker.patch.object(SolverModel, "save")

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            }
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        mode = "create_candidate"
        body = {
            "solversInfo": [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "mode": mode,
                    "is_registered_solver": True,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                },
            ]
        }

        response = client.post("/api/solvers?corporateId=8dbeeeb9-8096-4489-b497-6e33e4dabb1d", json=body)

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"message": "OK"}

    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_create_existing_solver_candidate(self, mocker):
        """正常系のテスト（APT、既存の個人ソルバー候補申請）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_solver_application = mocker.patch.object(SolverApplicationModel, "query")
        mock_solver_application.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mocker.patch.object(SolverApplicationModel, "save")

        mock_solver = mocker.patch.object(SolverModel, "get")
        mock_solver.return_value = SolverModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            # 個人ソルバーのテストため、True
            is_solver=True,
            registration_status="new",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        mocker.patch.object(SolverModel, "save")

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            }
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        mode = "create_candidate"
        body = {
            "solversInfo": [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "mode": mode,
                    "is_registered_solver": True,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                },
            ]
        }

        response = client.post("/api/solvers?corporateId=8dbeeeb9-8096-4489-b497-6e33e4dabb1d", json=body)

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"message": "OK"}

    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_for_apt_create_solver(self, mocker):
        """正常系のテスト（APT、新規個人ソルバー登録申請）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_solver_application = mocker.patch.object(SolverApplicationModel, "query")
        mock_solver_application.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mocker.patch.object(SolverApplicationModel, "save")

        mocker.patch.object(SolverModel, "save")

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            }
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        mode = "create_solver"
        body = {
            "solversInfo": [
                {
                    "mode": mode,
                    "is_registered_solver": False,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "screening1": {
                        "evaluation": False,
                        "evidence": "特になし"
                    },
                    "screening2": {
                        "evaluation": True,
                        "evidence": "特になし"
                    },
                    "screening3": {
                        "evaluation": False,
                        "evidence": "特になし"
                    },
                    "screening4": {
                        "evaluation": True,
                        "evidence": "特になし"
                    },
                    "screening5": {
                        "evaluation": False,
                        "evidence": "特になし"
                    },
                    "screening6": {
                        "evaluation": True,
                        "evidence": "特になし"
                    },
                    "screening7": {
                        "evaluation": False,
                        "evidence": "特になし"
                    },
                    "screening8": {
                        "evaluation": True,
                        "evidence": "特になし"
                    },
                    "criteria1": "特になし",
                    "criteria2": "特になし",
                    "criteria3": "特になし",
                    "criteria4": "特になし",
                    "criteria5": "特になし",
                    "criteria6": "特になし",
                    "criteria7": "特になし",
                    "criteria8": "特になし",
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                }
            ]
        }

        response = client.post("/api/solvers", json=body)

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"message": "OK"}

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_ok_for_solver_staff(self, mocker):
        """正常系のテスト（法人ソルバー）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_solver_application = mocker.patch.object(SolverApplicationModel, "query")
        mock_solver_application.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mocker.patch.object(SolverApplicationModel, "save")

        mocker.patch.object(SolverModel, "save")

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            }
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        mode = "create_candidate"
        body = {
            "solversInfo": [
                {
                    "mode": mode,
                    "is_registered_solver": False,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "906a3144-9650-4a34-8a23-3b02f3b9a999",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                }
            ]
        }

        response = client.post("/api/solvers", json=body)

        actual = response.json()

        assert response.status_code == 200
        assert actual == {"message": "OK"}

    @pytest.mark.usefixtures("auth_apt_user")
    def test_error_create_candidate_validate(self):
        """新規ソルバー候補申請の必須チェック（案件ID）"""
        mode = "create_candidate"
        body = {
            "solversInfo": [
                {
                    "mode": mode,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    # "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                }
            ]
        }

        response = client.post("/api/solvers", json=body)

        actual = response.json()

        assert response.status_code == 400
        assert actual["detail"] == "`solver_application_id` is required."

    @pytest.mark.usefixtures("auth_apt_user")
    def test_error_create_solver_validate(self):
        """新規個人ソルバー登録申請の必須チェック（スクリーニング項目）"""
        mode = "create_solver"
        body = {
            "solversInfo": [
                {
                    "mode": mode,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "screening1": {
                        "evaluation": True,
                        # "evidence": "特になし"
                    },
                    "screening2": {
                        "evaluation": True,
                        "evidence": "特になし"
                    },
                    "screening3": {
                        "evaluation": False,
                        "evidence": "特になし"
                    },
                    "screening4": {
                        "evaluation": True,
                        "evidence": "特になし"
                    },
                    "screening5": {
                        "evaluation": False,
                        "evidence": "特になし"
                    },
                    "screening6": {
                        "evaluation": True,
                        "evidence": "特になし"
                    },
                    "screening7": {
                        "evaluation": False,
                        "evidence": "特になし"
                    },
                    "screening8": {
                        "evaluation": True,
                        "evidence": "特になし"
                    },
                    "criteria1": "特になし",
                    "criteria2": "特になし",
                    "criteria3": "特になし",
                    "criteria4": "特になし",
                    "criteria5": "特になし",
                    "criteria6": "特になし",
                    "criteria7": "特になし",
                    "criteria8": "特になし",
                    "notes": "特になし",
                    "registrationStatus": "saved",
                }
            ]
        }

        response = client.post("/api/solvers", json=body)

        actual = response.json()

        assert response.status_code == 400
        assert actual["detail"] == "if `screening_1.evaluation` is True, `screening_1.evidence` is required."

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_permission_error_for_solver_staff(self, mocker):
        """正常系のテスト（法人ソルバー）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SOLVER_STAFF.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mode = "create_candidate"
        body = {
            "solversInfo": [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "mode": mode,
                    "is_registered_solver": False,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                }
            ]
        }

        response = client.post("/api/solvers", json=body)
        assert response.status_code == 403

    @pytest.mark.usefixtures("auth_apt_user")
    def test_403_create_candidate_apt(self, mocker):
        """異常系のテスト（APT、所属法人の相違）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_solver_application = mocker.patch.object(SolverApplicationModel, "query")
        mock_solver_application.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mocker.patch.object(SolverApplicationModel, "save")

        mock_solver = mocker.patch.object(SolverModel, "get")
        mock_solver.return_value = SolverModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            # ソルバー候補のテストのため、False
            is_solver=False,
            registration_status="new",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        mocker.patch.object(SolverModel, "save")

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            }
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        mode = "create_candidate"
        body = {
            "solversInfo": [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "mode": mode,
                    "is_registered_solver": True,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                },
            ]
        }

        response = client.post("/api/solvers?corporateId=8dbeeeb9-8096-4489-b497-6e33e4dabb1", json=body)
        assert response.status_code == 403

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_403_create_candidate_solver_staff(self, mocker):
        """異常系のテスト（法人ソルバー、所属法人の相違）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_solver_application = mocker.patch.object(SolverApplicationModel, "query")
        mock_solver_application.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mocker.patch.object(SolverApplicationModel, "save")

        mocker.patch.object(SolverModel, "save")

        mock_solver = mocker.patch.object(SolverModel, "get")
        mock_solver.return_value = SolverModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            # ソルバー候補のテストのため、False
            is_solver=False,
            registration_status="new",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        mocker.patch.object(SolverModel, "save")

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            }
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        mode = "create_candidate"
        body = {
            "solversInfo": [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "mode": mode,
                    "is_registered_solver": True,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "906a3144-9650-4a34-8a23-3b02f3b9a999",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                },
            ]
        }

        response = client.post("/api/solvers", json=body)

        assert response.status_code == 403

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_403_different_mode(self, mocker):
        """異常系のテスト（modeが「create_candidate」「create_solver」以外）"""
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_solver_application = mocker.patch.object(SolverApplicationModel, "query")
        mock_solver_application.return_value = [
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mocker.patch.object(SolverApplicationModel, "save")

        mocker.patch.object(SolverModel, "save")

        mock_solver = mocker.patch.object(SolverModel, "get")
        mock_solver.return_value = SolverModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            # ソルバー候補のテストのため、False
            is_solver=False,
            registration_status="new",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        mocker.patch.object(SolverModel, "save")

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png",
                "file_content": "テスト",
            }
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        mode = "update_candidate"
        body = {
            "solversInfo": [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "mode": mode,
                    "is_registered_solver": True,
                    "name": "山田太郎",
                    "nameKana": "ヤマダタロウ",
                    "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
                    "solverApplicationName": "ソルバー案件",
                    "title": "リーダー",
                    "email": "yamada@example.com",
                    "phone": "00-0000-0000",
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "corporateId": "906a3144-9650-4a34-8a23-3b02f3b9a999",
                    "sex": "man",
                    "birthDay": "2020/10/23",
                    "operatingStatus": "working",
                    "facePhoto": {
                        "fileName": "テスト.jpeg",
                        "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/photos/テスト.jpeg"
                    },
                    "resume": [
                        {
                            "fileName": "テスト.txt",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.txt"
                        },
                        {
                            "fileName": "テスト.png",
                            "path": "solver-corporation/c8f352eb-38ec-488d-a51c-5cc8770a15cb/solvers/89cbe2ed-f44c-4a1c-9408-c67b0ca22600/resumes/テスト.png"
                        },
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
                    "englishLevel": "reading_and_writing",
                    "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
                    "notes": "特になし",
                    "registrationStatus": "new",
                    "fileKeyId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22600",
                },
            ]
        }

        response = client.post("/api/solvers", json=body)

        assert response.status_code == 403
