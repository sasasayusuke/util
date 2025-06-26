import pytest
from app.main import app
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.user import UserModel
from app.resources.const import DataType
from app.schemas.karten import DeliverablesInfo, DocumentInfo, Location
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestGetKarteById:
    @pytest.mark.parametrize(
        "karte_id, model, expected",
        [
            (
                "3336e959-c216-46a8-92c3-3d7d3f951d00",
                ProjectKarteModel(
                    karte_id="3336e959-c216-46a8-92c3-3d7d3f951d00",
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
                    last_update_datetime="2022-07-12T02:21:39.356000+0000",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    customer_user_ids=[
                        "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                        "8a990e25-43da-49a3-ae76-863b5219fe6a",
                    ],
                    start_support_actual_time="13:00",
                    end_support_actual_time="14:00",
                    man_hour=5.0,
                    customers="田中様、加藤様",
                    support_team="山田、佐藤",
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[
                        DocumentInfo(
                            fileName="document",
                            path="path/aaa/bbb...",
                        )
                    ],
                    deliverables=[
                        DeliverablesInfo(
                            fileName="deliverables",
                            path="path/aaa/ccc...",
                        )
                    ],
                    location=Location(
                        type='online',
                        detail=''
                    ),
                    memo="memo",
                    human_resource_needed_for_customer="テストお客様に不足している人的リソース",
                    company_and_industry_recommended_to_customer="テストお客様に紹介したい企業や産業",
                    human_resource_needed_for_support="テスト支援に不足している人的リソース",
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-07-12T02:21:39.356000+0000",
                    version=2,
                ),
                {
                    "karteId": "3336e959-c216-46a8-92c3-3d7d3f951d00",
                    "projectId": "2854fc20-caea-44bb-ada5",
                    "date": "2022/08/31",
                    "startDatetime": "2022/08/31 13:00",
                    "startTime": "13:00",
                    "endTime": "14:00",
                    "draftSupporterId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "draftSupporterName": "テストユーザー",
                    "lastUpdateDatetime": "2022-07-12T02:21:39.356+09:00",
                    "startSupportActualTime": "13:00",
                    "endSupportActualTime": "14:00",
                    "manHour": 5.0,
                    "customers": "田中様、加藤様",
                    "supportTeam": "山田、佐藤",
                    "detail": "detail",
                    "feedback": "feedback",
                    "homework": "homework",
                    "documents": [
                        {
                            "fileName": "document",
                            "path": "path/aaa/bbb...",
                        }
                    ],
                    "deliverables": [
                        {
                            "fileName": "deliverables",
                            "path": "path/aaa/ccc...",
                        }
                    ],
                    "location": {
                        "type": "online",
                        "detail": ""
                    },
                    "memo": "memo",
                    "humanResourceNeededForCustomer": "テストお客様に不足している人的リソース",
                    "companyAndIndustryRecommendedToCustomer": "テストお客様に紹介したい企業や産業",
                    "humanResourceNeededForSupport": "テスト支援に不足している人的リソース",
                    "task": "task",
                    "isDraft": True,
                    "updateId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "updateUserName": "テストユーザー",
                    "updateAt": "2022-07-12T02:21:39.356+09:00",
                    "version": 2,
                },
            ),
            # 任意項目がNoneの場合
            # human_resource_needed_for_customer
            # company_and_industry_recommended_to_customer
            # human_resource_needed_for_support
            (
                "3336e959-c216-46a8-92c3-3d7d3f951d00",
                ProjectKarteModel(
                    karte_id="3336e959-c216-46a8-92c3-3d7d3f951d00",
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
                    last_update_datetime="2022-07-12T02:21:39.356000+0000",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    customer_user_ids=[
                        "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                        "8a990e25-43da-49a3-ae76-863b5219fe6a",
                    ],
                    start_support_actual_time="13:00",
                    end_support_actual_time="14:00",
                    man_hour=5.0,
                    customers="田中様、加藤様",
                    support_team="山田、佐藤",
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[
                        DocumentInfo(
                            fileName="document",
                            path="path/aaa/bbb...",
                        )
                    ],
                    deliverables=[
                        DeliverablesInfo(
                            fileName="deliverables",
                            path="path/aaa/ccc...",
                        )
                    ],
                    memo="memo",
                    human_resource_needed_for_customer=None,
                    company_and_industry_recommended_to_customer=None,
                    human_resource_needed_for_support=None,
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-07-12T02:21:39.356000+0000",
                    version=2,
                ),
                {
                    "karteId": "3336e959-c216-46a8-92c3-3d7d3f951d00",
                    "projectId": "2854fc20-caea-44bb-ada5",
                    "date": "2022/08/31",
                    "startDatetime": "2022/08/31 13:00",
                    "startTime": "13:00",
                    "endTime": "14:00",
                    "draftSupporterId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "draftSupporterName": "テストユーザー",
                    "lastUpdateDatetime": "2022-07-12T02:21:39.356+09:00",
                    'location': None,
                    "startSupportActualTime": "13:00",
                    "endSupportActualTime": "14:00",
                    "manHour": 5.0,
                    "customers": "田中様、加藤様",
                    "supportTeam": "山田、佐藤",
                    "detail": "detail",
                    "feedback": "feedback",
                    "homework": "homework",
                    "documents": [
                        {
                            "fileName": "document",
                            "path": "path/aaa/bbb...",
                        }
                    ],
                    "deliverables": [
                        {
                            "fileName": "deliverables",
                            "path": "path/aaa/ccc...",
                        }
                    ],
                    "memo": "memo",
                    "humanResourceNeededForCustomer": None,
                    "companyAndIndustryRecommendedToCustomer": None,
                    "humanResourceNeededForSupport": None,
                    "task": "task",
                    "isDraft": True,
                    "updateId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "updateUserName": "テストユーザー",
                    "updateAt": "2022-07-12T02:21:39.356+09:00",
                    "version": 2,
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, karte_id, model, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(ProjectKarteModel, "get")
        mock.return_value = model

        # メソッド
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
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
        )

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

        response = client.get(f"/api/karten/{karte_id}")
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_supporter_user")
    def test_karte_not_found(self, mocker):
        """カルテが存在しない時"""
        karte_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock = mocker.patch.object(ProjectKarteModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.get(f"/api/karten/{karte_id}")

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "karte_id, queryModel, model, expected",
        [
            (
                "3336e959-c216-46a8-92c3-3d7d3f951d11",
                [
                    ProjectKarteModel(
                        karte_id="3336e959-c216-46a8-92c3-3d7d3f951d11",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/08/31",
                        start_datetime="2022/08/01 13:00",
                        start_time="13:00",
                        end_time="14:00",
                        supporter_ids=[
                            "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                            "b9b67094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="2022-07-12T02:21:39.356000+0000",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                            "8a990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        start_support_actual_time="13:00",
                        end_support_actual_time="14:00",
                        man_hour=5.0,
                        detail="detail",
                        feedback="feedback",
                        homework="homework",
                        documents=[
                            DocumentInfo(
                                fileName="document",
                                path="path/aaa/bbb...",
                            )
                        ],
                        deliverables=[
                            DeliverablesInfo(
                                fileName="deliverables",
                                path="path/aaa/ccc...",
                            )
                        ],
                        location=Location(
                            type='online',
                            detail=''
                        ),
                        memo="memo",
                        human_resource_needed_for_customer="テストお客様に不足している人的リソース",
                        company_and_industry_recommended_to_customer="テストお客様に紹介したい企業や産業",
                        human_resource_needed_for_support="テスト支援に不足している人的リソース",
                        task="task",
                        is_draft=True,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        update_at="2022-07-12T02:21:39.356000+0000",
                        version=2,
                    ),
                    ProjectKarteModel(
                        karte_id="3336e959-c216-46a8-92c3-3d7d3f951d22",
                        project_id="2854fc20-caea-44bb-ada5",
                        date="2022/08/31",
                        start_datetime="2022/08/02 13:00",
                        start_time="13:00",
                        end_time="14:00",
                        supporter_ids=[
                            "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                            "b9b67094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="2022-07-12T02:21:39.356000+0000",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                            "8a990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        start_support_actual_time="13:00",
                        end_support_actual_time="14:00",
                        man_hour=5.0,
                        customers="田中様、加藤様",
                        support_team="山田、佐藤",
                        detail="detail",
                        feedback="feedback",
                        homework="homework",
                        documents=[
                            DocumentInfo(
                                fileName="document",
                                path="path/aaa/bbb...",
                            )
                        ],
                        deliverables=[
                            DeliverablesInfo(
                                fileName="deliverables",
                                path="path/aaa/ccc...",
                            )
                        ],
                        location=Location(
                            type='online',
                            detail=''
                        ),
                        memo="memo",
                        human_resource_needed_for_customer="テストお客様に不足している人的リソース",
                        company_and_industry_recommended_to_customer="テストお客様に紹介したい企業や産業",
                        human_resource_needed_for_support="テスト支援に不足している人的リソース",
                        task="task",
                        is_draft=True,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        update_at="2022-07-12T02:21:39.356000+0000",
                        version=2,
                    ),
                ],
                ProjectKarteModel(
                    karte_id="3336e959-c216-46a8-92c3-3d7d3f951d00",
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
                    last_update_datetime="2022-07-12T02:21:39.356000+0000",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    customer_user_ids=[
                        "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                        "8a990e25-43da-49a3-ae76-863b5219fe6a",
                    ],
                    start_support_actual_time="13:00",
                    end_support_actual_time="14:00",
                    man_hour=5.0,
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[
                        DocumentInfo(
                            fileName="document",
                            path="path/aaa/bbb...",
                        )
                    ],
                    deliverables=[
                        DeliverablesInfo(
                            fileName="deliverables",
                            path="path/aaa/ccc...",
                        )
                    ],
                    location=Location(
                        type='online',
                        detail=''
                    ),
                    memo="memo",
                    human_resource_needed_for_customer="テストお客様に不足している人的リソース",
                    company_and_industry_recommended_to_customer="テストお客様に紹介したい企業や産業",
                    human_resource_needed_for_support="テスト支援に不足している人的リソース",
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-07-12T02:21:39.356000+0000",
                    version=2,
                ),
                {
                    "karteId": "3336e959-c216-46a8-92c3-3d7d3f951d00",
                    "projectId": "2854fc20-caea-44bb-ada5",
                    "date": "2022/08/31",
                    "startDatetime": "2022/08/31 13:00",
                    "startTime": "13:00",
                    "endTime": "14:00",
                    "draftSupporterId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "draftSupporterName": "テストユーザー",
                    "lastUpdateDatetime": "2022-07-12T02:21:39.356+09:00",
                    "startSupportActualTime": "13:00",
                    "endSupportActualTime": "14:00",
                    "manHour": 5.0,
                    "customers": "田中様、加藤様",
                    "supportTeam": "山田、佐藤",
                    "detail": "detail",
                    "feedback": "feedback",
                    "homework": "homework",
                    "documents": [
                        {
                            "fileName": "document",
                            "path": "path/aaa/bbb...",
                        }
                    ],
                    "deliverables": [
                        {
                            "fileName": "deliverables",
                            "path": "path/aaa/ccc...",
                        }
                    ],
                    "location": {
                        "type": "online",
                        "detail": ""
                    },
                    "memo": "memo",
                    "humanResourceNeededForCustomer": "テストお客様に不足している人的リソース",
                    "companyAndIndustryRecommendedToCustomer": "テストお客様に紹介したい企業や産業",
                    "humanResourceNeededForSupport": "テスト支援に不足している人的リソース",
                    "task": "task",
                    "isDraft": True,
                    "updateId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "updateUserName": "テストユーザー",
                    "updateAt": "2022-07-12T02:21:39.356+09:00",
                    "version": 2,
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok_previous_attendees(self, mocker, karte_id, queryModel, model, expected):
        """正常系のテスト"""
        queryMock = mocker.patch.object(ProjectKarteModel, "query")
        queryMock.return_value = queryModel

        mock = mocker.patch.object(ProjectKarteModel, "get")
        mock.return_value = model

        # メソッド
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
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
        )

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

        response = client.get(f"/api/karten/{karte_id}")
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected
