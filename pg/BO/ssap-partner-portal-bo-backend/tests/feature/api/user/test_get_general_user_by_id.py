import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.user import UserModel
from app.models.solver_corporation import SolverCorporationModel
from app.resources.const import DataType, UserRoleType
from app.service.project_service import ProjectService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetUserById:
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
        response = client.get("/api/users/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", headers=REQUEST_HEADERS)

        assert response.status_code == 404

    @pytest.mark.parametrize(
        "user_id, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="supporter",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    solver_corporation_id=None,
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "supporter",
                    "customerId": None,
                    "customerName": None,
                    "job": "部長",
                    "company": "ソニーグループ株式会社",
                    "solverCorporationId": None,
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "IST"}
                    ],
                    "organizationName": None,
                    "isInputManHour": True,
                    "projectIds": [],
                    "agreed": False,
                    "lastLoginAt": None,
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": "1",
                },
            ),
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, user_id, model, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.get(f"/api/users/{user_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="apt",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    solver_corporation_id=None,
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "apt",
                    "customerId": None,
                    "customerName": None,
                    "job": "部長",
                    "company": "ソニーグループ株式会社",
                    "solverCorporationId": None,
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "IST"}
                    ],
                    "organizationName": None,
                    "isInputManHour": True,
                    "projectIds": [],
                    "agreed": False,
                    "lastLoginAt": None,
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": "1",
                },
            ),
        ],
    )
    def test_apt_ok(self, mocker, mock_auth_admin, user_id, model, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.get(f"/api/users/{user_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, model, solver_corporation_model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="solver_staff",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    solver_corporation_id="89cbe2ed-f44c-4a1c-9408-c67b0ca22777",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                SolverCorporationModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca22777",
                    data_type=DataType.SOLVER_CORPORATION,
                    name="ソニーグループ株式会社01",
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "solver_staff",
                    "customerId": None,
                    "customerName": None,
                    "job": "部長",
                    "company": "ソニーグループ株式会社01",
                    "solverCorporationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca22777",
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "IST"}
                    ],
                    "organizationName": None,
                    "isInputManHour": True,
                    "projectIds": [],
                    "agreed": False,
                    "lastLoginAt": None,
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": "1",
                },
            ),
        ],
    )
    def test_solver_staff_ok(
        self,
        mocker,
        mock_auth_admin,
        user_id,
        model,
        solver_corporation_model,
        expected
    ):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model
        mock_solver_corporation_model = mocker.patch.object(SolverCorporationModel, "get")
        mock_solver_corporation_model.return_value = solver_corporation_model

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.get(f"/api/users/{user_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    def test_user_not_found(self, mock_auth_admin, mocker):
        """一般ユーザーが存在しない時のテスト"""

        user_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.get(f"/api/users/{user_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "user_id, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="supporter",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    solver_corporation_id=None,
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "supporter",
                    "customerId": None,
                    "customerName": None,
                    "job": "部長",
                    "company": "ソニーグループ株式会社",
                    "solverCorporationId": None,
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "IST"}
                    ],
                    "organizationName": None,
                    "isInputManHour": True,
                    "projectIds": [],
                    "agreed": False,
                    "lastLoginAt": None,
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": "1",
                },
            ),
        ],
    )
    def test_auth_for_sales(self, mocker, mock_auth_admin, user_id, model, expected):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.get(f"/api/users/{user_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="supporter",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    solver_corporation_id=None,
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "supporter",
                    "customerId": None,
                    "customerName": None,
                    "job": "部長",
                    "company": "ソニーグループ株式会社",
                    "solverCorporationId": None,
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "IST"}
                    ],
                    "organizationName": None,
                    "isInputManHour": True,
                    "projectIds": [],
                    "agreed": False,
                    "lastLoginAt": None,
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": "1",
                },
            ),
        ],
    )
    def test_auth_for_supporter_mgr(
        self, mocker, mock_auth_admin, user_id, model, expected
    ):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.get(f"/api/users/{user_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="supporter",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    solver_corporation_id=None,
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "supporter",
                    "customerId": None,
                    "customerName": None,
                    "job": "部長",
                    "company": "ソニーグループ株式会社",
                    "solverCorporationId": None,
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "IST"}
                    ],
                    "organizationName": None,
                    "isInputManHour": True,
                    "projectIds": [],
                    "agreed": False,
                    "lastLoginAt": None,
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": "1",
                },
            ),
        ],
    )
    def test_auth_for_sales_mgr(
        self, mocker, mock_auth_admin, user_id, model, expected
    ):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.get(f"/api/users/{user_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="supporter",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    solver_corporation_id=None,
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "supporter",
                    "customerId": None,
                    "customerName": None,
                    "job": "部長",
                    "company": "ソニーグループ株式会社",
                    "solverCorporationId": None,
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "IST"}
                    ],
                    "organizationName": None,
                    "isInputManHour": True,
                    "projectIds": [],
                    "agreed": False,
                    "lastLoginAt": None,
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": "1",
                },
            ),
        ],
    )
    def test_auth_for_survey_ops(
        self, mocker, mock_auth_admin, user_id, model, expected
    ):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.get(f"/api/users/{user_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="supporter",
                    customer_id=None,
                    customer_name=None,
                    job="部長",
                    company="ソニーグループ株式会社",
                    solver_corporation_id=None,
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    is_input_man_hour=True,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "supporter",
                    "customerId": None,
                    "customerName": None,
                    "job": "部長",
                    "company": "ソニーグループ株式会社",
                    "solverCorporationId": None,
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "IST"}
                    ],
                    "organizationName": None,
                    "isInputManHour": True,
                    "projectIds": [],
                    "agreed": False,
                    "lastLoginAt": None,
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                    "version": "1",
                },
            ),
        ],
    )
    def test_auth_for_man_hour_ops(
        self, mocker, mock_auth_admin, user_id, model, expected
    ):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        response = client.get(f"/api/users/{user_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
