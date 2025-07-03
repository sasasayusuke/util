import pytest
from app.main import app
from app.models.user import UserModel
from app.resources.const import DataType
from app.service.user_service import UserService
from fastapi.testclient import TestClient

client = TestClient(app)


class TestGetUsers:
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
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_get_users_ok(self, mocker, model_list, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(UserModel.data_type_name_index, "query")
        mock.return_value = model_list

        mock = mocker.patch.object(UserService, "get_supporter_organization_name")
        mock.return_value = None

        response = client.get("/api/users?role=customer")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [],
                {"total": 0, "users": []},
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_user_zero(self, mocker, model_list, expected):
        """一般ユーザーが１件も存在しない時"""
        mock = mocker.patch.object(UserModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get("/api/users")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
