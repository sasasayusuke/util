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
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestUpdateSolverById:
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_for_apt_update_candidate(self, mocker):
        """正常系のテスト（APT mode:update_candidate）"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=[
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            is_solver=True,
            registration_status="new",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        mocker.patch.object(SolverModel, "update")

        mock_admin_name = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin_name.return_value = "テストシステム管理者"

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                "file_content": "テスト",
            }
        ]

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

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2

        body = {
            "mode": "update_candidate",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
            "title": "リーダー",
            "email": "yamada@example.com",
            "phone": "00-0000-0000",
            "issueMap50": [
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
            ],
            "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            "sex": "man",
            "birthDay": "2020/10/23",
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
            "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
            "notes": "特になし",
            "isSolver": True,
            "registrationStatus": "new",
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)
        actual = response.json()

        expected = {"message": "OK"}

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_for_apt_register_solver(self, mocker):
        """正常系のテスト（APT mode:register_solver）"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=[
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            is_solver=True,
            registration_status="new",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        mocker.patch.object(SolverModel, "update")

        mock_admin_name = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin_name.return_value = "テストシステム管理者"

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                "file_content": "テスト",
            }
        ]

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

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2

        body = {
            "mode": "register_solver",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
            "title": "リーダー",
            "email": "yamada@example.com",
            "phone": "00-0000-0000",
            "issueMap50": [
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
            ],
            "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            "sex": "man",
            "birthDay": "2020/10/23",
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
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
            "registrationStatus": "saved",
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)
        actual = response.json()

        expected = {"message": "OK"}

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_for_apt_update_solver(self, mocker):
        """正常系のテスト（APT mode:update_solver 一時保存から本登録）"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=[
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            is_solver=True,
            registration_status="temporary_saving",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        mock_solver_application_id = mocker.patch.object(SolverService, "create_solver_application")
        mock_solver_application_id.return_value = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mocker.patch.object(SolverModel, "update")

        mock_admin_name = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin_name.return_value = "テストシステム管理者"

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                "file_content": "テスト",
            }
        ]

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

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2

        body = {
            "mode": "update_solver",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
            "solverApplicationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22222",
            "solverApplicationName": "ソルバー案件",
            "title": "リーダー",
            "email": "yamada@example.com",
            "phone": "00-0000-0000",
            "issueMap50": [
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
            ],
            "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            "sex": "man",
            "birthDay": "2020/10/23",
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
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
            "registrationStatus": "saved",
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)
        actual = response.json()

        expected = {"message": "OK"}

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_ok_for_solver_staff(self, mocker):
        """正常系のテスト（法人ソルバー）"""
        mock_solver_application_batch_get = mocker.patch.object(SolverApplicationModel, "batch_get")
        mock_solver_application_batch_get.return_value = [
            SolverApplicationModel(
                id="503c385a-f568-4419-93d9-ca9ef802fe45",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="pL2x",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            ),
            SolverApplicationModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="Qw8K",
                name="ソルバー案件2",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mock_solver_application_scan = mocker.patch.object(SolverApplicationModel, "scan")
        mock_solver_application_scan.return_value = [
            SolverApplicationModel(
                id="503c385a-f568-4419-93d9-ca9ef802fe45",
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id="pL2x",
                name="ソルバー案件",
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
            )
        ]

        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a999",
            sex="man",
            issue_map50=[
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            is_solver=True,
            registration_status="saved",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
            solver_application_ids=["503c385a-f568-4419-93d9-ca9ef802fe45", "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
        )

        mocker.patch.object(SolverModel, "update")

        mock_admin_name = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin_name.return_value = "テストシステム管理者"

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                "file_content": "テスト",
            }
        ]

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

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2

        body = {
            "mode": "update_solver",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
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
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/906a3144-9650-4a34-8a23-3b02f3b9a999/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/906a3144-9650-4a34-8a23-3b02f3b9a999/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
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
            "registrationStatus": "saved",
            "deleteSolverApplicationIds": ["pL2x"]
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)
        actual = response.json()

        expected = {"message": "OK"}

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_apt_user")
    def test_error_register_solver_validate(self, mocker):
        """mode:register_solverの時の必須エラー"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=[
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            is_solver=True,
            registration_status="new",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        mocker.patch.object(SolverModel, "update")

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2

        body = {
            "mode": "register_solver",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
            "title": "リーダー",
            "email": "yamada@example.com",
            "phone": "00-0000-0000",
            "issueMap50": [
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
            ],
            "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            "sex": "man",
            "birthDay": "2020/10/23",
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
            "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
            "screening1": {"evaluation": True, "evidence": None},
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
            "registrationStatus": "saved",
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)

        actual = response.json()
        assert response.status_code == 400
        assert actual["detail"] == "if `screening_1.evaluation` is True, `screening_1.evidence` is required."

    @pytest.mark.usefixtures("auth_apt_user")
    def test_error_update_solver_validate(self, mocker):
        """mode:update_solverの時の必須エラー"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=[
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            is_solver=True,
            registration_status="certificated",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        mocker.patch.object(SolverModel, "update")

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2

        body = {
            "mode": "update_solver",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
            "title": "リーダー",
            "email": "yamada@example.com",
            "phone": "00-0000-0000",
            "issueMap50": [
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
            ],
            "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            "sex": "man",
            "birthDay": "2020/10/23",
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
            "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
            "screening1": {"evaluation": True, "evidence": None},
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
            "registrationStatus": "certificated",
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)

        actual = response.json()
        assert response.status_code == 400
        assert actual["detail"] == "if `screening_1.evaluation` is True, `screening_1.evidence` is required."

    @pytest.mark.usefixtures("auth_apt_user")
    def test_solver_not_found(self, mocker):
        """ソルバーが存在しない時のテスト"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.side_effect = DoesNotExist()

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2

        body = {
            "mode": "update_solver",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
            "title": "リーダー",
            "email": "yamada@example.com",
            "phone": "00-0000-0000",
            "issueMap50": [
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
            ],
            "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            "sex": "man",
            "birthDay": "2020/10/23",
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
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
            "registrationStatus": "certificated",
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_auth_error_solver_staff_403(self, mocker):
        """未所属の法人のソルバー情報にアクセス不可の時のテスト"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="906a3144-9650-4a34-8a23-3b02f3b9a111",
            sex="man",
            issue_map50=[
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            is_solver=True,
            registration_status="certificated",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2

        body = {
            "mode": "update_solver",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
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
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/906a3144-9650-4a34-8a23-3b02f3b9a999/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/906a3144-9650-4a34-8a23-3b02f3b9a999/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
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
            "registrationStatus": "certificated",
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)

        actual = response.json()
        assert response.status_code == 403
        assert actual["detail"] == "Forbidden"

    @pytest.mark.usefixtures("auth_apt_user")
    def test_solver_version_conflict(self, mocker):
        """バージョンが異なる時のテスト"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=[
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            is_solver=True,
            registration_status="certificated",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        body = {
            "mode": "update_solver",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
            "title": "リーダー",
            "email": "yamada@example.com",
            "phone": "00-0000-0000",
            "issueMap50": [
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
            ],
            "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            "sex": "man",
            "birthDay": "2020/10/23",
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
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
            "registrationStatus": "certificated",
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"

    @pytest.mark.usefixtures("auth_apt_user")
    def test_error_corporate_id_does_not_match(self, mocker):
        """正常系のテスト（APT mode:update_candidate）"""
        mock = mocker.patch.object(SolverModel, "get")
        mock.return_value = SolverModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SOLVER,
            name="山田太郎",
            corporate_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            sex="man",
            issue_map50=[
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ],
            birth_day="2000/10/23",
            work_history="コンサル10年目",
            is_consulting_firm=True,
            specialized_themes="デジタルマーケティング",
            main_achievements="特になし",
            is_solver=True,
            registration_status="new",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        mocker.patch.object(SolverModel, "update")

        mock_admin_name = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin_name.return_value = "テストシステム管理者"

        mocker_file_from_s3 = mocker.patch.object(SolverService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                "file_content": "テスト",
            }
        ]

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

        mocker_tsi_areas_name = mocker.patch.object(MasterService, "get_tsi_areas_name")
        mocker_tsi_areas_name.return_value = [
            "鉱業"
        ]

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mocker.patch.object(SesHelper, "send_mail_with_file")

        solver_id = "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 2

        body = {
            "mode": "update_candidate",
            "name": "山田太郎",
            "nameKana": "ヤマダタロウ",
            "title": "リーダー",
            "email": "yamada@example.com",
            "phone": "00-0000-0000",
            "issueMap50": [
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
            ],
            "corporateId": "8dbeeeb9-8096-4489-b497-6e33e4999999",
            "sex": "man",
            "birthDay": "2020/10/23",
            "operatingStatus": "working",
            "facePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/photos/テスト.jpeg",
            },
            "resume": [
                {
                    "fileName": "テスト.jpeg",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/solvers/11cbe2ed-f44c-4a1c-9408-c67b0ca2270d/resumes/テスト.jpeg",
                },
            ],
            "academicBackground": "大学卒",
            "workHistory": "コンサル10年目",
            "isConsultingFirm": True,
            "specializedThemes": "デジタルマーケティング",
            "mainAchievements": "特になし",
            "providedOperating_rate": 10,
            "providedOperatingRateNext": 20,
            "operationProspectsMonthAfterNext": "再来月の稼働見込みあり",
            "pricePerPersonMonth": 20000,
            "pricePerPersonMonthLower": 10000,
            "hourlyRate": 20000,
            "hourlyRateLower": 10000,
            "englishLevel": "reading_and_writing",
            "tsiAreas": ["89cbe2ed-f44c-4a1c-9408-c67b0ca22222"],
            "notes": "特になし",
            "isSolver": True,
            "registrationStatus": "new",
        }

        response = client.put(f"/api/solvers/{solver_id}?version={version}", json=body)
        assert response.status_code == 403
