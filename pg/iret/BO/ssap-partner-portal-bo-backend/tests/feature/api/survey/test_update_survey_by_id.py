import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.admin import AdminModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_survey import (
    AnswersAttribute,
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestUpdateSurveyById:
    @pytest.mark.parametrize(
        "body, project_survey_model, project_model, admin_model",
        [
            (
                {
                    "summaryMonth": "2022/03",
                    "isNotSummary": True,
                    "isSolverProject": True,
                },
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type="quick",
                    project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="IST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="51ff73f4-2414-421c-b64c-9981f988b331",
                        name="田中 三郎",
                        organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        organization_name="AST",
                    ),
                    supporter_users=[
                        SupporterUserAttribute(
                            id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            name="田中 次郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        )
                    ],
                    sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    service_type_name="組織開発",
                    answer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    answer_user_name="田中 次郎",
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    customer_name="あああ研究所",
                    company="ソニーグループ株式会社",
                    answers=[
                        AnswersAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            answer="問題なし",
                            point=3,
                        )
                    ],
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime="2022/01/15 01:10",
                    plan_survey_response_datetime="2022/01/31 17:00",
                    actual_survey_response_datetime="2022/01/30 10:00",
                    is_finished=False,
                    is_disclosure=True,
                    is_not_summary=False,
                    is_solver_project=True,
                    points=PointsAttribute(
                        satisfaction=3,
                        continuation=False,
                        recommended=3,
                        sales=0,
                        survey_satisfaction=4,
                        man_hour_satisfaction=4,
                        karte_satisfaction=4,
                        master_karte_satisfaction=0,
                    ),
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト１",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
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
                ),
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
            )
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        body,
        project_survey_model,
        project_model,
        admin_model,
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        project_survey_mock = mocker.patch.object(ProjectSurveyModel, "get")
        project_survey_mock.return_value = project_survey_model

        project_mock = mocker.patch.object(ProjectModel, "get")
        project_mock.return_value = project_model

        admin_mock = mocker.patch.object(AdminModel, "get")
        admin_mock.return_value = admin_model

        mocker.patch.object(ProjectSurveyModel, "update")

        response = client.put(
            "/api/surveys/11111449-5d63-42d4-ae1b-f0faf65a7076?version=1",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()

        assert response.status_code == 200
        assert actual["message"] == "OK"

    @pytest.mark.parametrize(
        "query_params, body, project_survey_model",
        [
            (
                "version=1",
                {
                    "summaryMonth": "2022/03",
                    "isNotSummary": True,
                    "isSolverProject": True,
                },
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type="quick",
                    project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="IST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="51ff73f4-2414-421c-b64c-9981f988b331",
                        name="田中 三郎",
                        organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        organization_name="AST",
                    ),
                    supporter_users=[
                        SupporterUserAttribute(
                            id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            name="田中 次郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        )
                    ],
                    sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    service_type_name="組織開発",
                    answer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    answer_user_name="田中 次郎",
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    customer_name="あああ研究所",
                    company="ソニーグループ株式会社",
                    answers=[
                        AnswersAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            answer="問題なし",
                            point=3,
                        )
                    ],
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime="2022/01/15 01:10",
                    plan_survey_response_datetime="2022/01/31 17:00",
                    actual_survey_response_datetime="2022/01/30 10:00",
                    is_finished=False,
                    is_disclosure=True,
                    is_not_summary=False,
                    is_solver_project=True,
                    points=PointsAttribute(
                        satisfaction=3,
                        continuation=False,
                        recommended=3,
                        sales=0,
                        survey_satisfaction=4,
                        man_hour_satisfaction=4,
                        karte_satisfaction=4,
                        master_karte_satisfaction=0,
                    ),
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=2,
                ),
            ),
        ],
    )
    def test_conflict(
        self, mocker, mock_auth_admin, query_params, body, project_survey_model
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        project_survey_mock = mocker.patch.object(ProjectSurveyModel, "get")
        project_survey_mock.return_value = project_survey_model

        response = client.put(
            f"/api/surveys/11111449-5d63-42d4-ae1b-f0faf65a7076?{query_params}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 409

    @pytest.mark.parametrize(
        "body",
        [
            ########################
            # 集計月
            ########################
            {
                # "summaryMonth": "2022/03",
                "isNotSummary": True
            },
            {
                "summaryMonth": None,
                "isNotSummary": True,
            },
            {
                "summaryMonth": "2022",
                "isNotSummary": True,
            },
            {
                "summaryMonth": "202203",
                "isNotSummary": True,
            },
            {
                "summaryMonth": "20220301",
                "isNotSummary": True,
            },
            ########################
            # 集計対象フラグ
            ########################
            {
                "summaryMonth": "2022/03",
                # "isNotSummary": True,
            },
            {
                "summaryMonth": "2022/03",
                "isNotSummary": None,
            },
            {
                "summaryMonth": "2022/03",
                "isNotSummary": "対象",
            },
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            "/api/surveys/11111449-5d63-42d4-ae1b-f0faf65a7076?version=1",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 422

    @pytest.mark.parametrize(
        "query_params",
        [
            "",
            "version=first",
            "version=true",
        ],
    )
    def test_not_version(self, mock_auth_admin, query_params):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        body = {
            "summaryMonth": "2022/03",
            "isNotSummary": True,
        }

        response = client.put(
            f"/api/surveys/11111449-5d63-42d4-ae1b-f0faf65a7076?{query_params}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 422
