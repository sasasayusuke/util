import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestUpdateUserById:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.SURVEY_OPS.key],
            [UserRoleType.MAN_HOUR_OPS.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_not_access(
        self,
        mock_auth_admin,
        role_types,
    ):
        """システム管理者以外はアクセス不可"""
        mock_auth_admin(role_types)
        response = client.put(
            "/api/users/1?version=1",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, model, body, version, expected",
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
                    supporter_organization_id=["033bd0b5-c2c7-4778-a58d-76a46500f7d9"],
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
                    "name": "山田太郎",
                    "company": "ソニーグループ株式会社",
                    "solver_corporation_id": None,
                    "supporter_organizations": [
                        {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                    ],
                    "is_input_man_hour": True,
                },
                1,
                {"message": "OK"},
            ),
        ],
    )
    def test_supporter_update_ok(
        self, mocker, mock_auth_admin, user_id, model, body, version, expected
    ):
        """支援者ユーザーの更新成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model
        mocker.patch.object(UserModel, "update")

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, model, body, version, expected",
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
                    supporter_organization_id=["033bd0b5-c2c7-4778-a58d-76a46500f7d9"],
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
                    "name": "山田太郎",
                    "company": "ソニーグループ株式会社",
                    "solver_corporation_id": None,
                    "supporter_organizations": [
                        {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                    ],
                    "is_input_man_hour": True,
                },
                1,
                {"message": "OK"},
            ),
        ],
    )
    def test_apt_update_ok(
        self, mocker, mock_auth_admin, user_id, model, body, version, expected
    ):
        """APTユーザーの更新成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model
        mocker.patch.object(UserModel, "update")

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, model, body, version, expected",
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
                    supporter_organization_id=["033bd0b5-c2c7-4778-a58d-76a46500f7d9"],
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
                    "name": "山田太郎",
                    "company": "ソニーグループ株式会社",
                    "solver_corporation_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca22777",
                    "supporter_organizations": [
                        {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                    ],
                    "is_input_man_hour": True,
                },
                1,
                {"message": "OK"},
            ),
        ],
    )
    def test_solver_staff_update_ok(
        self, mocker, mock_auth_admin, user_id, model, body, version, expected
    ):
        """法人ソルバーユーザーの更新成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model
        mocker.patch.object(UserModel, "update")

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, model, body, version, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                UserModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.USER,
                    name="山田太郎",
                    email="user@example.com",
                    role="customer",
                    customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    customer_name="〇〇株式会社",
                    job="部長",
                    company=None,
                    solver_corporation_id=None,
                    supporter_organization_id=[],
                    is_input_man_hour=None,
                    project_ids=[],
                    agreed=False,
                    last_login_at=None,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "name": "山田太郎",
                    "job": "部長",
                    "supporter_organizations": [],
                    "organization_name": "顧客組織",
                    "solver_corporation_id": None,
                },
                1,
                {"message": "OK"},
            ),
        ],
    )
    def test_customer_update_ok(
        self, mocker, mock_auth_admin, user_id, model, body, version, expected
    ):
        """顧客ユーザーの更新成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model
        mocker.patch.object(UserModel, "update")

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "user_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "name": "山田太郎",
                    "job": "部長",
                    "supporter_organizations": [],
                    "organization_name": "顧客組織",
                },
            ),
        ],
    )
    def test_auth_for_sales(self, mock_auth_admin, user_id, version, body):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "name": "山田太郎",
                    "job": "部長",
                    "supporter_organizations": [],
                    "organization_name": "顧客組織",
                },
            ),
        ],
    )
    def test_auth_for_supporter_mgr(self, mock_auth_admin, user_id, version, body):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "name": "山田太郎",
                    "job": "部長",
                    "supporter_organizations": [],
                    "organization_name": "顧客組織",
                },
            ),
        ],
    )
    def test_auth_for_sales_mgr(self, mock_auth_admin, user_id, version, body):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "name": "山田太郎",
                    "job": "部長",
                    "supporter_organizations": [],
                    "organization_name": "顧客組織",
                },
            ),
        ],
    )
    def test_auth_for_survey_ops(self, mock_auth_admin, user_id, version, body):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "user_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "name": "山田太郎",
                    "job": "部長",
                    "supporter_organizations": [],
                    "organization_name": "顧客組織",
                },
            ),
        ],
    )
    def test_auth_for_man_hour_ops(self, mock_auth_admin, user_id, version, body):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body",
        [
            # ###########################
            # name
            # ###########################
            # 必須チェック
            {
                # 'name': '山田太郎',
                "company": "ソニーグループ株式会社",
                "supporter_organizations": [
                    {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                ],
                "organization_name": "管理部",
                "is_input_man_hour": True,
            },
            # Noneチェック
            {
                "name": None,
                "company": "ソニーグループ株式会社",
                "supporter_organizations": [
                    {"id": "033bd0b5-c2c7-4778-a58d-76a46500f7d9"}
                ],
                "organization_name": "管理部",
                "is_input_man_hour": True,
            },
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        user_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        response = client.put(
            f"/api/users/{user_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )
        assert response.status_code == 422
