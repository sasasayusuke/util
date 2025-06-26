import pytest
from app.main import app
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.user import UserModel
from app.resources.const import DataType
from fastapi.testclient import TestClient

client = TestClient(app)


class TestGetKarten:
    @pytest.mark.parametrize(
        "project_id, model, expected",
        [
            (
                "886a3144-9650-4a34-8a23-3b02f3b9aeac",
                [
                    ProjectKarteModel(
                        karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                        project_id="886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                        "lastUpdateDatetime": "2022-08-04T15:37:11.588+09:00",
                        "isDraft": True,
                        "updateUser": "",
                    },
                    {
                        "karteId": "9982b0c4-496b-41c2-8273-9db2c95fe927",
                        "date": "2022/09/01",
                        "startTime": "13:00",
                        "endTime": "17:00",
                        "lastUpdateDatetime": None,
                        "isDraft": False,
                        "updateUser": "",
                    },
                ],
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, project_id, model, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(ProjectKarteModel, "query")
        mock.return_value = model

        mock_project_get = mocker.patch.object(ProjectModel, "get")
        mock_project_get.return_value = ProjectModel(
            id="886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
            main_supporter_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
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

        response = client.get(f"/api/karten/?projectId={project_id}")
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected
