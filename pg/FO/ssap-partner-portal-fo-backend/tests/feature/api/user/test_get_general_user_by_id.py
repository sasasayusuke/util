import pytest
from app.main import app
from app.models.user import UserModel
from app.resources.const import DataType
from app.service.user_service import UserService
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestGetUserById:
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
                    supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    is_input_man_hour=True,
                    project_ids=[
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ],
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
                    "supporterOrganizations": [
                        {"id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d", "name": "IST"}
                    ],
                    "organizationName": None,
                    "isInputManHour": True,
                    "projectIds": [
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ],
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
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, user_id, model, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(UserModel, "get")
        mock.return_value = model

        mock = mocker.patch.object(UserService, "get_supporter_organization_name")
        mock.return_value = "IST"

        response = client.get(f"/api/users/{user_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [],
                {"detail": "Not found"},
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_user_zero(self, mocker, model_list, expected):
        """一般ユーザーが存在しない時のテスト"""
        user_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock = mocker.patch.object(UserModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.get(f"/api/users/{user_id}")

        actual = response.json()
        assert response.status_code == 404
        assert actual == expected
