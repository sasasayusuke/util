import pytest
from app.main import app
from app.models.user import UserModel
from app.resources.const import DataType
from fastapi.testclient import TestClient

client = TestClient(app)


class TestSuggestUsers:
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
                        role="supporter",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at=None,
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
                        role="supporter",
                        customer_id="99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    ),
                ],
                [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "name": "山田太郎",
                        "email": "yamada@example.com",
                        "customerName": "〇〇株式会社",
                    },
                    {
                        "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "name": "田中太郎",
                        "email": "tanaka@example.com",
                        "customerName": "〇〇株式会社",
                    },
                ],
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_enabled_user_name_sort_ok(self, mocker, model_list, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(UserModel.data_type_name_index, "query")
        mock.return_value = model_list

        response = client.get(
            "/api/users/suggest?role=supporter&disabled=False&sort=name:asc"
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
                        id="99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="tanaka@example.com",
                        role="supporter",
                        customer_id="99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at=None,
                        disabled=True,
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
                        role="supporter",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        customer_name="〇〇株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=[],
                        is_input_man_hour=None,
                        project_ids=[],
                        agreed=True,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    ),
                ],
                [
                    {
                        "id": "99cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "name": "田中太郎",
                        "email": "tanaka@example.com",
                        "customerName": "〇〇株式会社",
                    },
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "name": "山田太郎",
                        "email": "yamada@example.com",
                        "customerName": "〇〇株式会社",
                    },
                ],
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_disabled_user_email_sort_ok(self, mocker, model_list, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock.return_value = model_list

        response = client.get(
            "/api/users/suggest?role=supporter&disabled=True&sort=email:asc"
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_supporter_user")
    def test_user_zero(self, mocker):
        """一般ユーザーが１件も存在しない時"""
        mock = mocker.patch.object(UserModel.data_type_name_index, "query")
        mock.return_value = []

        response = client.get(
            "/api/users/suggest?role=supporter&disabled=False&sort=name:asc"
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == []

    @pytest.mark.usefixtures("auth_supporter_user")
    def test_validation_role(self):
        """role指定のバリデーションチェック"""
        response = client.get("/api/users/suggest?disabled=False&sort=name:asc")
        assert response.status_code == 422

    @pytest.mark.usefixtures("auth_supporter_user")
    def test_validation_sort(self):
        """sort指定のバリデーションチェック"""
        sort = "name:desc"
        response = client.get(
            f"/api/users/suggest?role=supporter&disabled=False&sort={sort}"
        )
        assert response.status_code == 422
