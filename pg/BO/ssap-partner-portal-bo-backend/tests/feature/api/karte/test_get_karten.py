import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.admin import AdminModel
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetKarten:
    @pytest.mark.parametrize(
        "project_id, model, expected",
        [
            (
                "2854fc20-caea-44bb-ada5",
                [
                    ProjectKarteModel(
                        karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/08/31",
                        start_datetime="2022/08/31 13:00",
                        start_time="13:00",
                        end_time="14:00",
                        supporter_ids=[
                            "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                            "b9b67094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="2022-08-04T15:37:11.588277+09:00",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                            "8a990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=5,
                        is_draft=True,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                    ProjectKarteModel(
                        karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/09/01",
                        start_datetime="2022/09/01 13:00",
                        start_time="13:00",
                        end_time="17:00",
                        supporter_ids=[
                            "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                            "45667094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime=None,
                        customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "4561b7e4-5625-4361-be0a-65f0e49829ea",
                            "78990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=0,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                ],
                [
                    {
                        "karteId": "9992b0c4-496b-41c2-8273-9db2c95fe927",
                        "date": "2022/08/31",
                        "startTime": "13:00",
                        "endTime": "14:00",
                        "isDraft": True,
                        "lastUpdateDatetime": "2022-08-04T15:37:11.588+09:00",
                        "updateUser": "テストユーザー",
                    },
                    {
                        "karteId": "9982b0c4-496b-41c2-8273-9db2c95fe927",
                        "date": "2022/09/01",
                        "startTime": "13:00",
                        "endTime": "17:00",
                        "isDraft": False,
                        "lastUpdateDatetime": None,
                        "updateUser": "テストユーザー",
                    },
                ],
            )
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, project_id, model, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(ProjectKarteModel, "query")
        mock.return_value = model

        # メソッド
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
            email="user@example.com",
            role="customer",
            customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_name="〇〇株式会社",
            job="部長",
            company=None,
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            is_input_man_hour=None,
            project_ids=[],
            agreed=True,
            last_login_at="2022-04-25T03:21:39.356Z",
            disabled=False,
        )

        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            data_type=DataType.ADMIN,
            name="テストユーザー",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_user_auth = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user_auth.return_value = []

        mock_project_get = mocker.patch.object(ProjectModel, "get")
        mock_project_get.return_value = ProjectModel(
            id="2854fc20-caea-44bb-ada5",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at="2020/10/23 03:21",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            create_new=True,
            continued=True,
            main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
            contract_date="2021/01/30",
            phase="プラン提示(D)",
            customer_success="DXの実現",
            support_date_from="2021/01/30",
            support_date_to="2021/02/28",
            profit=[],
            total_contract_time=200,
            main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            salesforce_main_customer={},
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        response = client.get(
            f"/api/karten/?projectId={project_id}", headers=REQUEST_HEADERS
        )
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_id, model, expected",
        [
            (
                "2854fc20-caea-44bb-ada5",
                [
                    ProjectKarteModel(
                        karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/08/31",
                        start_datetime="2022/08/31 13:00",
                        start_time="13:00",
                        end_time="14:00",
                        supporter_ids=[
                            "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                            "b9b67094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="2022-08-04T15:37:11.588277+09:00",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                            "8a990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=5,
                        is_draft=True,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                    ProjectKarteModel(
                        karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/09/01",
                        start_datetime="2022/09/01 13:00",
                        start_time="13:00",
                        end_time="17:00",
                        supporter_ids=[
                            "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                            "45667094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime=None,
                        customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "4561b7e4-5625-4361-be0a-65f0e49829ea",
                            "78990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=0,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                ],
                [
                    {
                        "karteId": "9992b0c4-496b-41c2-8273-9db2c95fe927",
                        "date": "2022/08/31",
                        "startTime": "13:00",
                        "endTime": "14:00",
                        "isDraft": True,
                        "lastUpdateDatetime": "2022-08-04T15:37:11.588+09:00",
                        "updateUser": "テストユーザー",
                    },
                    {
                        "karteId": "9982b0c4-496b-41c2-8273-9db2c95fe927",
                        "date": "2022/09/01",
                        "startTime": "13:00",
                        "endTime": "17:00",
                        "isDraft": False,
                        "lastUpdateDatetime": None,
                        "updateUser": "テストユーザー",
                    },
                ],
            )
        ],
    )
    def test_auth_system_admin_ok(
        self, mocker, mock_auth_admin, project_id, model, expected
    ):
        """権限のテスト：システム管理者：正常"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(ProjectKarteModel, "query")
        mock.return_value = model

        # メソッド
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
            email="user@example.com",
            role="customer",
            customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_name="〇〇株式会社",
            job="部長",
            company=None,
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            is_input_man_hour=None,
            project_ids=[],
            agreed=True,
            last_login_at="2022-04-25T03:21:39.356Z",
            disabled=False,
        )

        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            data_type=DataType.ADMIN,
            name="テストユーザー",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_user_auth = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user_auth.return_value = []

        mock_project_get = mocker.patch.object(ProjectModel, "get")
        mock_project_get.return_value = ProjectModel(
            id="2854fc20-caea-44bb-ada5",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at="2020/10/23 03:21",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            create_new=True,
            continued=True,
            main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
            contract_date="2021/01/30",
            phase="プラン提示(D)",
            customer_success="DXの実現",
            support_date_from="2021/01/30",
            support_date_to="2021/02/28",
            profit=[],
            total_contract_time=200,
            main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            salesforce_main_customer={},
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=True,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        response = client.get(
            f"/api/karten/?projectId={project_id}", headers=REQUEST_HEADERS
        )
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_id, model, expected",
        [
            (
                "2854fc20-caea-44bb-ada5",
                [
                    ProjectKarteModel(
                        karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/08/31",
                        start_datetime="2022/08/31 13:00",
                        start_time="13:00",
                        end_time="14:00",
                        supporter_ids=[
                            "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                            "b9b67094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="2022-08-04T15:37:11.588277+09:00",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                            "8a990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=5,
                        is_draft=True,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                    ProjectKarteModel(
                        karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/09/01",
                        start_datetime="2022/09/01 13:00",
                        start_time="13:00",
                        end_time="17:00",
                        supporter_ids=[
                            "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                            "45667094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime=None,
                        customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "4561b7e4-5625-4361-be0a-65f0e49829ea",
                            "78990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=0,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                ],
                [
                    {
                        "karteId": "9992b0c4-496b-41c2-8273-9db2c95fe927",
                        "date": "2022/08/31",
                        "startTime": "13:00",
                        "endTime": "14:00",
                        "isDraft": True,
                        "lastUpdateDatetime": "2022-08-04T15:37:11.588+09:00",
                        "updateUser": "テストユーザー",
                    },
                    {
                        "karteId": "9982b0c4-496b-41c2-8273-9db2c95fe927",
                        "date": "2022/09/01",
                        "startTime": "13:00",
                        "endTime": "17:00",
                        "isDraft": False,
                        "lastUpdateDatetime": None,
                        "updateUser": "テストユーザー",
                    },
                ],
            )
        ],
    )
    def test_auth_sales_mgr_ok(
        self, mocker, mock_auth_admin, project_id, model, expected
    ):
        """権限のテスト：営業責任者：正常"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])
        mock = mocker.patch.object(ProjectKarteModel, "query")
        mock.return_value = model

        # メソッド
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
            email="user@example.com",
            role="customer",
            customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_name="〇〇株式会社",
            job="部長",
            company=None,
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            is_input_man_hour=None,
            project_ids=[],
            agreed=True,
            last_login_at="2022-04-25T03:21:39.356Z",
            disabled=False,
        )

        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            data_type=DataType.ADMIN,
            name="テストユーザー",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_user_auth = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user_auth.return_value = []

        mock_project_get = mocker.patch.object(ProjectModel, "get")
        mock_project_get.return_value = ProjectModel(
            id="2854fc20-caea-44bb-ada5",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at="2020/10/23 03:21",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            create_new=True,
            continued=True,
            main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
            contract_date="2021/01/30",
            phase="プラン提示(D)",
            customer_success="DXの実現",
            support_date_from="2021/01/30",
            support_date_to="2021/02/28",
            profit=[],
            total_contract_time=200,
            main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            salesforce_main_customer={},
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=True,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        response = client.get(
            f"/api/karten/?projectId={project_id}", headers=REQUEST_HEADERS
        )
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_id, model, expected",
        [
            (
                "2854fc20-caea-44bb-ada5",
                [
                    ProjectKarteModel(
                        karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/08/31",
                        start_datetime="2022/08/31 13:00",
                        start_time="13:00",
                        end_time="14:00",
                        supporter_ids=[
                            "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                            "b9b67094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="2022-08-04T15:37:11.588277+09:00",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                            "8a990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=5,
                        is_draft=True,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                    ProjectKarteModel(
                        karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/09/01",
                        start_datetime="2022/09/01 13:00",
                        start_time="13:00",
                        end_time="17:00",
                        supporter_ids=[
                            "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                            "45667094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime=None,
                        customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "4561b7e4-5625-4361-be0a-65f0e49829ea",
                            "78990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=0,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                ],
                [
                    {
                        "karteId": "9992b0c4-496b-41c2-8273-9db2c95fe927",
                        "date": "2022/08/31",
                        "startTime": "13:00",
                        "endTime": "14:00",
                        "isDraft": True,
                        "lastUpdateDatetime": "2022-08-04T15:37:11.588+09:00",
                        "updateUser": "テストユーザー",
                    },
                    {
                        "karteId": "9982b0c4-496b-41c2-8273-9db2c95fe927",
                        "date": "2022/09/01",
                        "startTime": "13:00",
                        "endTime": "17:00",
                        "isDraft": False,
                        "lastUpdateDatetime": None,
                        "updateUser": "テストユーザー",
                    },
                ],
            )
        ],
    )
    def test_auth_business_mgr_ok(
        self, mocker, mock_auth_admin, project_id, model, expected
    ):
        """権限のテスト：事業者責任者：正常"""
        mock_auth_admin([UserRoleType.BUSINESS_MGR.key])
        mock = mocker.patch.object(ProjectKarteModel, "query")
        mock.return_value = model

        # メソッド
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
            email="user@example.com",
            role="customer",
            customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_name="〇〇株式会社",
            job="部長",
            company=None,
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            is_input_man_hour=None,
            project_ids=[],
            agreed=True,
            last_login_at="2022-04-25T03:21:39.356Z",
            disabled=False,
        )

        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            data_type=DataType.ADMIN,
            name="テストユーザー",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_user_auth = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user_auth.return_value = []

        mock_project_get = mocker.patch.object(ProjectModel, "get")
        mock_project_get.return_value = ProjectModel(
            id="2854fc20-caea-44bb-ada5",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at="2020/10/23 03:21",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            create_new=True,
            continued=True,
            main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
            contract_date="2021/01/30",
            phase="プラン提示(D)",
            customer_success="DXの実現",
            support_date_from="2021/01/30",
            support_date_to="2021/02/28",
            profit=[],
            total_contract_time=200,
            main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            salesforce_main_customer={},
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=True,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        response = client.get(
            f"/api/karten/?projectId={project_id}", headers=REQUEST_HEADERS
        )
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_model",
        [
            (
                # 自身の課の公開案件
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                )
            ),
            (
                # 自身の課の非公開案件
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                )
            ),
        ],
    )
    def test_auth_supporter_mgr_ok(self, mocker, mock_auth_admin, project_model):
        """権限のテスト：支援者責任者：正常"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])
        mock = mocker.patch.object(ProjectKarteModel, "query")
        mock.return_value = [
            ProjectKarteModel(
                karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                project_id="2854fc20-caea-44bb-ada5",
                date="2022/08/31",
                start_datetime="2022/08/31 13:00",
                start_time="13:00",
                end_time="14:00",
                supporter_ids=[
                    "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                    "b9b67094-cdab-494c-818e-d4845088269b",
                ],
                draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                last_update_datetime="2022-08-04T15:37:11.588277+09:00",
                customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                customer_user_ids=[
                    "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                    "8a990e25-43da-49a3-ae76-863b5219fe6a",
                ],
                man_hour=5,
                is_draft=True,
                update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            ),
            ProjectKarteModel(
                karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                project_id="2854fc20-caea-44bb-ada5",
                date="2022/09/01",
                start_datetime="2022/09/01 13:00",
                start_time="13:00",
                end_time="17:00",
                supporter_ids=[
                    "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                    "45667094-cdab-494c-818e-d4845088269b",
                ],
                draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                last_update_datetime=None,
                customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                customer_user_ids=[
                    "4561b7e4-5625-4361-be0a-65f0e49829ea",
                    "78990e25-43da-49a3-ae76-863b5219fe6a",
                ],
                man_hour=0,
                is_draft=False,
                update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            ),
        ]

        project_id = "2854fc20-caea-44bb-ada5"

        # メソッド
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
            email="user@example.com",
            role="customer",
            customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_name="〇〇株式会社",
            job="部長",
            company=None,
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            is_input_man_hour=None,
            project_ids=[],
            agreed=True,
            last_login_at="2022-04-25T03:21:39.356Z",
            disabled=False,
        )

        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            data_type=DataType.ADMIN,
            name="テストユーザー",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_user_auth = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user_auth.return_value = []

        mock_project_get = mocker.patch.object(ProjectModel, "get")
        mock_project_get.return_value = project_model

        expected = [
            {
                "karteId": "9992b0c4-496b-41c2-8273-9db2c95fe927",
                "date": "2022/08/31",
                "startTime": "13:00",
                "endTime": "14:00",
                "isDraft": True,
                "lastUpdateDatetime": "2022-08-04T15:37:11.588+09:00",
                "updateUser": "テストユーザー",
            },
            {
                "karteId": "9982b0c4-496b-41c2-8273-9db2c95fe927",
                "date": "2022/09/01",
                "startTime": "13:00",
                "endTime": "17:00",
                "isDraft": False,
                "lastUpdateDatetime": None,
                "updateUser": "テストユーザー",
            },
        ]

        response = client.get(
            f"/api/karten/?projectId={project_id}", headers=REQUEST_HEADERS
        )
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_model_list_auth, project_model",
        [
            (
                [
                    UserModel(
                        id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        name="ソニー太郎",
                        email="taro.sony@example.com",
                        role="sales",
                        job="部長",
                        company="テスト株式会社",
                        organization_name="営業部",
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids={"2854fc20-caea-44bb-ada5"},
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                    )
                ],
                # 所属している公開案件
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
            (
                [
                    UserModel(
                        id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        name="ソニー太郎",
                        email="taro.sony@example.com",
                        role="sales",
                        job="部長",
                        company="テスト株式会社",
                        organization_name="営業部",
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids={"2854fc20-caea-44bb-ada5"},
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                    )
                ],
                # 所属している非公開案件
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_auth_sales_ok(
        self, mocker, mock_auth_admin, user_model_list_auth, project_model
    ):
        """権限のテスト：営業担当者：正常"""
        mock_auth_admin([UserRoleType.SALES.key])
        mock = mocker.patch.object(ProjectKarteModel, "query")
        mock.return_value = [
            ProjectKarteModel(
                karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                project_id="2854fc20-caea-44bb-ada5",
                date="2022/08/31",
                start_datetime="2022/08/31 13:00",
                start_time="13:00",
                end_time="14:00",
                supporter_ids=[
                    "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                    "b9b67094-cdab-494c-818e-d4845088269b",
                ],
                draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                last_update_datetime="2022-08-04T15:37:11.588277+09:00",
                customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                customer_user_ids=[
                    "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                    "8a990e25-43da-49a3-ae76-863b5219fe6a",
                ],
                man_hour=5,
                is_draft=True,
                update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            ),
            ProjectKarteModel(
                karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                project_id="2854fc20-caea-44bb-ada5",
                date="2022/09/01",
                start_datetime="2022/09/01 13:00",
                start_time="13:00",
                end_time="17:00",
                supporter_ids=[
                    "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                    "45667094-cdab-494c-818e-d4845088269b",
                ],
                draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                last_update_datetime=None,
                customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                customer_user_ids=[
                    "4561b7e4-5625-4361-be0a-65f0e49829ea",
                    "78990e25-43da-49a3-ae76-863b5219fe6a",
                ],
                man_hour=0,
                is_draft=False,
                update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            ),
        ]

        project_id = "2854fc20-caea-44bb-ada5"

        # メソッド
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
            email="user@example.com",
            role="customer",
            customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_name="〇〇株式会社",
            job="部長",
            company=None,
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            is_input_man_hour=None,
            project_ids=[],
            agreed=True,
            last_login_at="2022-04-25T03:21:39.356Z",
            disabled=False,
        )

        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            data_type=DataType.ADMIN,
            name="テストユーザー",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_user_auth = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user_auth.return_value = user_model_list_auth

        mock_project_get = mocker.patch.object(ProjectModel, "get")
        mock_project_get.return_value = project_model

        expected = [
            {
                "karteId": "9992b0c4-496b-41c2-8273-9db2c95fe927",
                "date": "2022/08/31",
                "startTime": "13:00",
                "endTime": "14:00",
                "isDraft": True,
                "lastUpdateDatetime": "2022-08-04T15:37:11.588+09:00",
                "updateUser": "テストユーザー",
            },
            {
                "karteId": "9982b0c4-496b-41c2-8273-9db2c95fe927",
                "date": "2022/09/01",
                "startTime": "13:00",
                "endTime": "17:00",
                "isDraft": False,
                "lastUpdateDatetime": None,
                "updateUser": "テストユーザー",
            },
        ]

        response = client.get(
            f"/api/karten/?projectId={project_id}", headers=REQUEST_HEADERS
        )
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_model_list_auth, project_model",
        [
            (
                [
                    UserModel(
                        id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        name="ソニー太郎",
                        email="taro.sony@example.com",
                        role="sales",
                        job="部長",
                        company="テスト株式会社",
                        organization_name="営業部",
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        # project_idsが未設定
                        # project_ids=set(),
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                    )
                ],
                # 所属していない公開案件
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
            (
                [
                    UserModel(
                        id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        name="ソニー太郎",
                        email="taro.sony@example.com",
                        role="sales",
                        job="部長",
                        company="テスト株式会社",
                        organization_name="営業部",
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=set(),
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                    )
                ],
                # 所属していない公開案件
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
            (
                [],
                # 所属していない公開案件(メールアドレスが一致する一般ユーザ情報が存在しない場合)
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
            (
                [
                    UserModel(
                        id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        name="ソニー太郎",
                        email="taro.sony@example.com",
                        role="sales",
                        job="部長",
                        company="テスト株式会社",
                        organization_name="営業部",
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        # project_idsが未設定
                        # project_ids=set(),
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                    )
                ],
                # 所属していない非公開案件
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
            (
                [
                    UserModel(
                        id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        name="ソニー太郎",
                        email="taro.sony@example.com",
                        role="sales",
                        job="部長",
                        company="テスト株式会社",
                        organization_name="営業部",
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=set(),
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                    )
                ],
                # 所属していない非公開案件
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
            (
                [],
                # 所属していない非公開案件(メールアドレスが一致する一般ユーザ情報が存在しない場合)
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                    salesforce_update_at="2020/10/23 03:21",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=[],
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer={},
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_auth_sales_403(
        self, mocker, mock_auth_admin, user_model_list_auth, project_model
    ):
        """権限のテスト：営業担当者：エラー"""
        mock_auth_admin([UserRoleType.SALES.key])
        mock = mocker.patch.object(ProjectKarteModel, "query")
        mock.return_value = [
            ProjectKarteModel(
                karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                project_id="2854fc20-caea-44bb-ada5",
                date="2022/08/31",
                start_datetime="2022/08/31 13:00",
                start_time="13:00",
                end_time="14:00",
                supporter_ids=[
                    "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                    "b9b67094-cdab-494c-818e-d4845088269b",
                ],
                draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                last_update_datetime="2022-08-04T15:37:11.588277+09:00",
                customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                customer_user_ids=[
                    "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                    "8a990e25-43da-49a3-ae76-863b5219fe6a",
                ],
                man_hour=5,
                is_draft=True,
                update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            ),
            ProjectKarteModel(
                karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                project_id="2854fc20-caea-44bb-ada5",
                date="2022/09/01",
                start_datetime="2022/09/01 13:00",
                start_time="13:00",
                end_time="17:00",
                supporter_ids=[
                    "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                    "45667094-cdab-494c-818e-d4845088269b",
                ],
                draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                last_update_datetime=None,
                customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                customer_user_ids=[
                    "4561b7e4-5625-4361-be0a-65f0e49829ea",
                    "78990e25-43da-49a3-ae76-863b5219fe6a",
                ],
                man_hour=0,
                is_draft=False,
                update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            ),
        ]

        project_id = "2854fc20-caea-44bb-ada5"

        # メソッド
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
            email="user@example.com",
            role="customer",
            customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_name="〇〇株式会社",
            job="部長",
            company=None,
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            is_input_man_hour=None,
            project_ids=[],
            agreed=True,
            last_login_at="2022-04-25T03:21:39.356Z",
            disabled=False,
        )

        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            data_type=DataType.ADMIN,
            name="テストユーザー",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_user_auth = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user_auth.return_value = user_model_list_auth

        mock_project_get = mocker.patch.object(ProjectModel, "get")
        mock_project_get.return_value = project_model

        response = client.get(
            f"/api/karten/?projectId={project_id}", headers=REQUEST_HEADERS
        )

        assert response.status_code == 403
