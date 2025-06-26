import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.admin import AdminModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestUpdateAdminById:
    @pytest.mark.parametrize(
        "admin_id, model, body, version, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                AdminModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.ADMIN,
                    name="山田太郎",
                    email="user@example.com",
                    roles=["sales"],
                    company="ソニーグループ株式会社",
                    job="部長",
                    supporter_organization_id=[],
                    organization_name="IST",
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-10-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-10-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                },
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-10-23T03:21:39.356+09:00",
                    "version": "1",
                },
            )
        ],
    )
    def test_ok(
        self, mocker, mock_auth_admin, admin_id, model, body, version, expected
    ):
        """管理ユーザー情報の更新成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model
        mocker.patch.object(AdminModel, "update")

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "admin_id, model, body, version, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                AdminModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.ADMIN,
                    name="山田太郎",
                    email="user@example.com",
                    roles=["supporter_mgr"],
                    company="ソニーグループ株式会社",
                    job="部長",
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    organization_name="IST",
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-10-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-10-23T03:21:39.356Z",
                    version="1",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "roles": ["supporter_mgr"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "organizationName": "IST",
                    "disabled": False,
                },
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "roles": ["supporter_mgr"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "organizationName": "IST",
                    "disabled": False,
                    "createId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "createAt": "2022-10-23T03:21:39.356+09:00",
                    "updateId": "fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    "updateAt": "2022-10-23T03:21:39.356+09:00",
                    "version": "1",
                },
            )
        ],
    )
    def test_ok_supporter_mgr(
        self, mocker, mock_auth_admin, admin_id, model, body, version, expected
    ):
        """管理ユーザー情報の更新成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model
        mocker.patch.object(AdminModel, "update")

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "admin_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                },
            )
        ],
    )
    def test_auth_for_sales(self, mock_auth_admin, admin_id, body, version):
        """権限テスト(営業担当者)"""
        mock_auth_admin([UserRoleType.SALES.key])

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                },
            )
        ],
    )
    def test_auth_for_supporter_mgr(self, mock_auth_admin, admin_id, body, version):
        """権限テスト(支援者責任者)"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                },
            )
        ],
    )
    def test_auth_for_sales_mgr(self, mock_auth_admin, admin_id, body, version):
        """権限テスト(営業担当責任者)"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                },
            )
        ],
    )
    def test_auth_for_survey_ops(self, mock_auth_admin, admin_id, body, version):
        """権限テスト(アンケート事務局)"""
        mock_auth_admin([UserRoleType.SURVEY_OPS.key])

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                },
            )
        ],
    )
    def test_auth_for_man_hour_ops(self, mock_auth_admin, admin_id, body, version):
        """権限テスト(稼働率調査事務局)"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_id, version, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                },
            )
        ],
    )
    def test_admin_not_found(self, mocker, mock_auth_admin, admin_id, body, version):
        """管理ユーザーが存在しない時のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        admin_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock = mocker.patch.object(AdminModel, "get")
        mock.side_effect = DoesNotExist()
        mocker.patch.object(AdminModel, "update")

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "admin_id, model, body, version",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                AdminModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.ADMIN,
                    name="山田太郎",
                    email="user@example.com",
                    roles=["sales"],
                    company="ソニーグループ株式会社",
                    job="部長",
                    supporter_organization_id=[],
                    organization_name="IST",
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-10-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-10-23T03:21:39.356Z",
                    version="2",
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "roles": ["sales"],
                    "company": "ソニーグループ株式会社",
                    "job": "部長",
                    "supporterOrganizationId": [],
                    "organizationName": "IST",
                    "disabled": False,
                },
                1,
            )
        ],
    )
    def test_admin_version_conflict(
        self, mocker, mock_auth_admin, admin_id, model, body, version
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(AdminModel, "get")
        mock.return_value = model
        mocker.patch.object(AdminModel, "update")

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"

    @pytest.mark.parametrize(
        "admin_id, version, body,",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                [
                    # ###########################
                    # name
                    # ###########################
                    # 必須チェック
                    {
                        # 'name': '山田太郎',
                        "roles": ["sales"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # Noneチェック
                    {
                        "name": None,
                        "roles": ["sales"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # ###########################
                    # roles
                    # ###########################
                    # 必須チェック
                    {
                        "name": "山田太郎",
                        # 'roles': ['sales'],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": None,
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # ###########################
                    # company
                    # ###########################
                    # 必須チェック
                    {
                        "name": "山田太郎",
                        "roles": ["sales"],
                        # 'company': 'ソニーグループ株式会社',
                        "job": "部長",
                        "supporterOrganizationId": [],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": ["sales"],
                        "company": None,
                        "job": "部長",
                        "supporterOrganizationId": [],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # ###########################
                    # job
                    # ###########################
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": ["sales"],
                        "company": "ソニーグループ株式会社",
                        "job": None,
                        "supporterOrganizationId": [],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # ###########################
                    # organizationName
                    # roles によって必要不要が分かれる
                    # ###########################
                    # 必須チェック
                    {
                        "name": "山田太郎",
                        "roles": ["sales"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [],
                        # 'organizationName': 'IST',
                        "disabled": False,
                    },
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": ["sales"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [],
                        "organizationName": None,
                        "disabled": False,
                    },
                    # ###########################
                    # disabled
                    # ###########################
                    # 必須チェック
                    {
                        "name": "山田太郎",
                        "roles": ["sales"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [],
                        "organizationName": "IST",
                        # 'disabled': False
                    },
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": ["sales"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [],
                        "organizationName": "IST",
                        "disabled": None,
                    },
                    # ###########################
                    # name
                    # ###########################
                    # 必須チェック
                    {
                        # 'name': '山田太郎',
                        "roles": ["supporter_mgr"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # Noneチェック
                    {
                        "name": None,
                        "roles": ["supporter_mgr"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # ###########################
                    # roles
                    # ###########################
                    # 必須チェック
                    {
                        "name": "山田太郎",
                        # 'roles': ['sales'],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": None,
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # ###########################
                    # company
                    # ###########################
                    # 必須チェック
                    {
                        "name": "山田太郎",
                        "roles": ["supporter_mgr"],
                        # 'company': 'ソニーグループ株式会社',
                        "job": "部長",
                        "supporterOrganizationId": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": ["supporter_mgr"],
                        "company": None,
                        "job": "部長",
                        "supporterOrganizationId": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # ###########################
                    # job
                    # ###########################
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": ["supporter_mgr"],
                        "company": "ソニーグループ株式会社",
                        "job": None,
                        "supporterOrganizationId": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # ###########################
                    # supporterOrganizationId
                    # ###########################
                    # 必須チェック
                    {
                        "name": "山田太郎",
                        "roles": ["supporter_mgr"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        # 'supporterOrganizationId':
                        #     ['89cbe2ed-f44c-4a1c-9408-c67b0ca2270d'],
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": ["supporter_mgr"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": None,
                        "organizationName": "IST",
                        "disabled": False,
                    },
                    # ###########################
                    # disabled
                    # ###########################
                    # 必須チェック
                    {
                        "name": "山田太郎",
                        "roles": ["supporter_mgr"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        "organizationName": "IST",
                        # 'disabled': False
                    },
                    # Noneチェック
                    {
                        "name": "山田太郎",
                        "roles": ["supporter_mgr"],
                        "company": "ソニーグループ株式会社",
                        "job": "部長",
                        "supporterOrganizationId": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        "organizationName": "IST",
                        "disabled": None,
                    },
                ],
            )
        ],
    )
    def test_validation(self, mock_auth_admin, admin_id, version, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            f"/api/admins/{admin_id}?version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )
        assert response.status_code == 422
