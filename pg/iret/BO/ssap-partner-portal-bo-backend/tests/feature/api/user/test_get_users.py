import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetUsers:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.SURVEY_OPS.key],
            [UserRoleType.MAN_HOUR_OPS.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_access_ok(
        self,
        mock_auth_admin,
        role_types,
    ):
        mock_auth_admin(role_types)
        response = client.get(
            "/api/users?role=customer&offset_page=1&limit=2&sort=last_login_at:asc",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    UserModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="yamada@example.com",
                        role="customer",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        solver_corporation_id=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at="2022-04-23T03:21:39.356Z",
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    ),
                    UserModel(
                        id="99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="tanaka@example.com",
                        role="customer",
                        customer_id="99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        solver_corporation_id=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at="2022-04-24T03:21:39.356Z",
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    ),
                    UserModel(
                        id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.USER,
                        name="佐藤太郎",
                        email="sato@example.com",
                        role="customer",
                        customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        solver_corporation_id=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    ),
                ],
                {
                    "offsetPage": 1,
                    "total": 3,
                    "users": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "山田太郎",
                            "email": "yamada@example.com",
                            "role": "customer",
                            "customerId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "customerName": "〇〇株式会社",
                            "job": "部長",
                            "company": None,
                            "supporterOrganizations": [],
                            "organizationName": None,
                            "isInputManHour": None,
                            "projectIds": [],
                            "agreed": True,
                            "lastLoginAt": "2022-04-23T03:21:39.356+09:00",
                            "disabled": False,
                        },
                        {
                            "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "田中太郎",
                            "email": "tanaka@example.com",
                            "role": "customer",
                            "customerId": "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "customerName": "〇〇株式会社",
                            "job": "部長",
                            "company": None,
                            "supporterOrganizations": [],
                            "organizationName": None,
                            "isInputManHour": None,
                            "projectIds": [],
                            "agreed": True,
                            "lastLoginAt": "2022-04-24T03:21:39.356+09:00",
                            "disabled": False,
                        },
                    ],
                },
            )
        ],
    )
    def test_sort_asc_ok(self, mocker, mock_auth_admin, model_list, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = model_list

        response = client.get(
            "/api/users?role=customer&offset_page=1&limit=2&sort=last_login_at:asc",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    UserModel(
                        id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.USER,
                        name="佐藤太郎",
                        email="sato@example.com",
                        role="customer",
                        customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        solver_corporation_id=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    ),
                    UserModel(
                        id="99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="tanaka@example.com",
                        role="customer",
                        customer_id="99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        solver_corporation_id=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at="2022-04-24T03:21:39.356Z",
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    ),
                    UserModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="yamada@example.com",
                        role="customer",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        solver_corporation_id=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at="2022-04-23T03:21:39.356Z",
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    ),
                ],
                {
                    "offsetPage": 1,
                    "total": 3,
                    "users": [
                        {
                            "id": "98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "佐藤太郎",
                            "email": "sato@example.com",
                            "role": "customer",
                            "customerId": "98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "customerName": "〇〇株式会社",
                            "job": "部長",
                            "company": None,
                            "supporterOrganizations": [],
                            "organizationName": None,
                            "isInputManHour": None,
                            "projectIds": [],
                            "agreed": True,
                            "lastLoginAt": "2022-04-25T03:21:39.356+09:00",
                            "disabled": False,
                        },
                        {
                            "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "田中太郎",
                            "email": "tanaka@example.com",
                            "role": "customer",
                            "customerId": "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "customerName": "〇〇株式会社",
                            "job": "部長",
                            "company": None,
                            "supporterOrganizations": [],
                            "organizationName": None,
                            "isInputManHour": None,
                            "projectIds": [],
                            "agreed": True,
                            "lastLoginAt": "2022-04-24T03:21:39.356+09:00",
                            "disabled": False,
                        },
                    ],
                },
            )
        ],
    )
    def test_sort_desc_ok(self, mocker, mock_auth_admin, model_list, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = model_list

        response = client.get(
            "/api/users?role=customer&offset_page=1&limit=2&sort=last_login_at:desc",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    UserModel(
                        id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.USER,
                        name="佐藤太郎",
                        email="sato@example.com",
                        role="apt",
                        customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        solver_corporation_id=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    )
                ],
                {
                    "offsetPage": 1,
                    "total": 1,
                    "users": [
                        {
                            "id": "98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "佐藤太郎",
                            "email": "sato@example.com",
                            "role": "apt",
                            "customerId": "98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "customerName": "〇〇株式会社",
                            "job": "部長",
                            "company": None,
                            "supporterOrganizations": [],
                            "organizationName": None,
                            "isInputManHour": None,
                            "projectIds": [],
                            "agreed": True,
                            "lastLoginAt": "2022-04-25T03:21:39.356+09:00",
                            "disabled": False,
                        }
                    ],
                },
            )
        ],
    )
    def test_apt_ok(self, mocker, mock_auth_admin, model_list, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = model_list

        response = client.get(
            "/api/users?role=apt&offsetPage=1&limit=20",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    UserModel(
                        id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.USER,
                        name="佐藤太郎",
                        email="sato@example.com",
                        role="solver_staff",
                        customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        solver_corporation_id="98cbe2ed-f44c-4a1c-9408-c67b0ca22777",
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at="2022-04-25T03:21:39.356Z",
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    )
                ],
                {
                    "offsetPage": 1,
                    "total": 1,
                    "users": [
                        {
                            "id": "98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "name": "佐藤太郎",
                            "email": "sato@example.com",
                            "role": "solver_staff",
                            "customerId": "98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "customerName": "〇〇株式会社",
                            "job": "部長",
                            "company": None,
                            "supporterOrganizations": [],
                            "organizationName": None,
                            "isInputManHour": None,
                            "projectIds": [],
                            "agreed": True,
                            "lastLoginAt": "2022-04-25T03:21:39.356+09:00",
                            "disabled": False,
                        }
                    ],
                },
            )
        ],
    )
    def test_solver_staff_ok(self, mocker, mock_auth_admin, model_list, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = model_list

        response = client.get(
            "/api/users?role=solver_staff&offsetPage=1&limit=20",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    def test_user_zero(self, mocker, mock_auth_admin):
        """一般ユーザーが１件も存在しない時"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = []

        response = client.get("/api/users", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == {"offsetPage": 1, "total": 0, "users": []}

    def test_auth_for_sales(self, mocker, mock_auth_admin):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = []

        response = client.get("/api/users", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == {"offsetPage": 1, "total": 0, "users": []}

    def test_auth_for_supporter_mgr(self, mocker, mock_auth_admin):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = []

        response = client.get("/api/users", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == {"offsetPage": 1, "total": 0, "users": []}

    def test_auth_for_sales_mgr(self, mocker, mock_auth_admin):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = []

        response = client.get("/api/users", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == {"offsetPage": 1, "total": 0, "users": []}

    def test_auth_for_survey_ops(self, mocker, mock_auth_admin):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = []

        response = client.get("/api/users", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == {"offsetPage": 1, "total": 0, "users": []}

    def test_auth_for_man_hour_ops(self, mocker, mock_auth_admin):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        mock = mocker.patch.object(UserModel.data_type_last_login_at_index, "query")
        mock.return_value = []

        response = client.get("/api/users", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == {"offsetPage": 1, "total": 0, "users": []}
