import pytest
from app.main import app
from app.models.user import UserModel
from app.resources.const import DataType
from fastapi.testclient import TestClient

client = TestClient(app)


class TestUpdateUserById:
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
                    organization_name="管理部",
                    company="ソニーグループ株式会社",
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
                    "job": "部長",
                    "company": "ソニーグループ株式会社",
                    "organizationName": "管理部",
                },
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "supporter",
                    "customerId": None,
                    "customerName": None,
                    "job": "部長",
                    "organizationName": "管理部",
                    "version": "1",
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_supporter_update_ok(self, mocker, user_id, model, body, version, expected):
        """支援者ユーザーの更新成功"""
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model
        mocker.patch.object(UserModel, "update")

        response = client.put(f"/api/users/{user_id}?version={version}", json=body)

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
                    organization_name="管理部",
                    company=None,
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
                    "organizationName": "管理部",
                },
                1,
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "name": "山田太郎",
                    "email": "user@example.com",
                    "role": "customer",
                    "customerId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "customerName": "〇〇株式会社",
                    "job": "部長",
                    "organizationName": "管理部",
                    "version": "1",
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_customer_update_ok(self, mocker, user_id, model, body, version, expected):
        """顧客ユーザーの更新成功"""
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model
        mocker.patch.object(UserModel, "update")

        response = client.put(f"/api/users/{user_id}?version={version}", json=body)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

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
                "job": "部長",
                "organizationName": "管理部"
            },
            # Noneチェック
            {
                "name": None,
                "company": "ソニーグループ株式会社",
                "job": "部長",
                "organizationName": "管理部"
            },
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_validation(self, body):
        user_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        response = client.put(f"/api/users/{user_id}?version={version}", json=body)
        assert response.status_code == 422
