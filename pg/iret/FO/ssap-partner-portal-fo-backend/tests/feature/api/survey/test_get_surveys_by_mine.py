import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_survey import (
    DataTypeActualSurveyRequestDatetimeIndex,
    PointsAttribute,
    ProjectIdActualSurveyRequestDatetimeIndex,
    ProjectSurveyModel,
)
from app.resources.const import DataType, SurveyType

client = TestClient(app)


class TestGetSurveysByMine:
    @pytest.mark.parametrize(
        "sort, project_survey_models, project_model, expected",
        [
            (
                "actual_survey_request_datetime:desc",
                [
                    ProjectSurveyModel(
                        id="19cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト1",
                        plan_survey_request_datetime="2022/07/01",
                        actual_survey_request_datetime="2022/07/01",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/15",
                        actual_survey_response_datetime="2022/07/15",
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=True,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト1株式会社",
                    ),
                    ProjectSurveyModel(
                        id="29cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト2",
                        plan_survey_request_datetime="2022/07/03",
                        actual_survey_request_datetime="2022/07/03",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/10",
                        actual_survey_response_datetime=None,
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=False,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.QUICK,
                        customer_name="テスト2株式会社",
                    ),
                    ProjectSurveyModel(
                        id="39cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト3",
                        plan_survey_request_datetime="2022/07/05",
                        actual_survey_request_datetime="2022/07/05",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/30",
                        actual_survey_response_datetime=None,
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=False,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.SERVICE,
                        customer_name="テスト3株式会社",
                    ),
                    ProjectSurveyModel(
                        id="49cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト4",
                        plan_survey_request_datetime="2022/07/20",
                        actual_survey_request_datetime="2022/07/20",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/08/01",
                        actual_survey_response_datetime=None,
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=False,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.COMPLETION,
                        customer_name="テスト4株式会社",
                    ),
                    ProjectSurveyModel(
                        id="59cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト5",
                        plan_survey_request_datetime="2022/07/09",
                        actual_survey_request_datetime="2022/07/09",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/18",
                        actual_survey_response_datetime=None,
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=False,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト5株式会社",
                    ),
                    ProjectSurveyModel(
                        id="69cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト6",
                        plan_survey_request_datetime="2022/07/10",
                        actual_survey_request_datetime="2022/07/10",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/16",
                        actual_survey_response_datetime="2022/07/16",
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=True,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト6株式会社",
                    ),
                ],
                ProjectModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="テストプロジェクト",
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
                    main_customer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                {
                    "total": 6,
                    "surveys": [
                        {
                            "id": "49cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "completion",
                            "projectName": "テストプロジェクト4",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/20",
                            "planSurveyResponseDatetime": "2022/08/01",
                            "actualSurveyResponseDatetime": None,
                            "answerUserName": "田中太郎",
                            "customerName": "テスト4株式会社",
                            "company": None,
                            "isFinished": False,
                        },
                        {
                            "id": "59cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "pp",
                            "projectName": "テストプロジェクト5",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/09",
                            "planSurveyResponseDatetime": "2022/07/18",
                            "actualSurveyResponseDatetime": None,
                            "answerUserName": "田中太郎",
                            "customerName": "テスト5株式会社",
                            "company": None,
                            "isFinished": False,
                        },
                        {
                            "id": "39cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "service",
                            "projectName": "テストプロジェクト3",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/05",
                            "planSurveyResponseDatetime": "2022/07/30",
                            "actualSurveyResponseDatetime": None,
                            "answerUserName": "田中太郎",
                            "customerName": "テスト3株式会社",
                            "company": None,
                            "isFinished": False,
                        },
                        {
                            "id": "29cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "quick",
                            "projectName": "テストプロジェクト2",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/03",
                            "planSurveyResponseDatetime": "2022/07/10",
                            "actualSurveyResponseDatetime": None,
                            "answerUserName": "田中太郎",
                            "customerName": "テスト2株式会社",
                            "company": None,
                            "isFinished": False,
                        },
                        {
                            "id": "69cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "pp",
                            "projectName": "テストプロジェクト6",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/10",
                            "planSurveyResponseDatetime": "2022/07/16",
                            "actualSurveyResponseDatetime": "2022/07/16",
                            "answerUserName": "田中太郎",
                            "customerName": "テスト6株式会社",
                            "company": None,
                            "isFinished": True,
                        },
                        {
                            "id": "19cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "pp",
                            "projectName": "テストプロジェクト1",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/01",
                            "planSurveyResponseDatetime": "2022/07/15",
                            "actualSurveyResponseDatetime": "2022/07/15",
                            "answerUserName": "田中太郎",
                            "customerName": "テスト1株式会社",
                            "company": None,
                            "isFinished": True,
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_customer_user")
    def test_ok(self, mocker, sort, project_survey_models, project_model, expected):

        mock = mocker.patch.object(
            ProjectSurveyModel.project_id_actual_survey_request_datetime_index, "query"
        )
        mock.return_value = project_survey_models
        mock_survey_2 = mocker.patch.object(
            ProjectSurveyModel.data_type_actual_survey_request_datetime_index, "query"
        )
        mock_survey_2.return_value = project_survey_models
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        response = client.get(f"/api/surveys/me?sort={sort}")
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "sort, limit, project_survey_models, project_model, expected",
        [
            (
                "actual_survey_request_datetime:desc",
                5,
                [
                    ProjectSurveyModel(
                        id="19cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト1",
                        plan_survey_request_datetime="2022/07/01",
                        actual_survey_request_datetime="2022/07/01",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/15",
                        actual_survey_response_datetime="2022/07/15",
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=True,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト1株式会社",
                    ),
                    ProjectSurveyModel(
                        id="29cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト2",
                        plan_survey_request_datetime="2022/07/03",
                        actual_survey_request_datetime="2022/07/03",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/10",
                        actual_survey_response_datetime=None,
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=False,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト2株式会社",
                    ),
                    ProjectSurveyModel(
                        id="39cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト3",
                        plan_survey_request_datetime="2022/07/05",
                        actual_survey_request_datetime="2022/07/05",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/30",
                        actual_survey_response_datetime=None,
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=False,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト3株式会社",
                    ),
                    ProjectSurveyModel(
                        id="49cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト4",
                        plan_survey_request_datetime="2022/07/20",
                        actual_survey_request_datetime="2022/07/20",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/08/01",
                        actual_survey_response_datetime=None,
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=False,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト4株式会社",
                    ),
                    ProjectSurveyModel(
                        id="59cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト5",
                        plan_survey_request_datetime="2022/07/09",
                        actual_survey_request_datetime="2022/07/09",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/18",
                        actual_survey_response_datetime=None,
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=False,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト5株式会社",
                    ),
                    ProjectSurveyModel(
                        id="69cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト6",
                        plan_survey_request_datetime="2022/07/10",
                        actual_survey_request_datetime="2022/07/10",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_response_datetime="2022/07/16",
                        actual_survey_response_datetime="2022/07/16",
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=True,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト6株式会社",
                    ),
                ],
                ProjectModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="テストプロジェクト",
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
                    main_customer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                {
                    "total": 5,
                    "surveys": [
                        {
                            "id": "49cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "pp",
                            "projectName": "テストプロジェクト4",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/20",
                            "planSurveyResponseDatetime": "2022/08/01",
                            "actualSurveyResponseDatetime": None,
                            "answerUserName": "田中太郎",
                            "customerName": "テスト4株式会社",
                            "company": None,
                            "isFinished": False,
                        },
                        {
                            "id": "59cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "pp",
                            "projectName": "テストプロジェクト5",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/09",
                            "planSurveyResponseDatetime": "2022/07/18",
                            "actualSurveyResponseDatetime": None,
                            "answerUserName": "田中太郎",
                            "customerName": "テスト5株式会社",
                            "company": None,
                            "isFinished": False,
                        },
                        {
                            "id": "39cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "pp",
                            "projectName": "テストプロジェクト3",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/05",
                            "planSurveyResponseDatetime": "2022/07/30",
                            "actualSurveyResponseDatetime": None,
                            "answerUserName": "田中太郎",
                            "customerName": "テスト3株式会社",
                            "company": None,
                            "isFinished": False,
                        },
                        {
                            "id": "29cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "pp",
                            "projectName": "テストプロジェクト2",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/03",
                            "planSurveyResponseDatetime": "2022/07/10",
                            "actualSurveyResponseDatetime": None,
                            "answerUserName": "田中太郎",
                            "customerName": "テスト2株式会社",
                            "company": None,
                            "isFinished": False,
                        },
                        {
                            "id": "69cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "pp",
                            "projectName": "テストプロジェクト6",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/10",
                            "planSurveyResponseDatetime": "2022/07/16",
                            "actualSurveyResponseDatetime": "2022/07/16",
                            "answerUserName": "田中太郎",
                            "customerName": "テスト6株式会社",
                            "company": None,
                            "isFinished": True,
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_customer_user")
    def test_ok_specify_limit(
        self, mocker, sort, limit, project_survey_models, project_model, expected
    ):

        mock = mocker.patch.object(
            ProjectSurveyModel.project_id_actual_survey_request_datetime_index, "query"
        )
        mock.return_value = project_survey_models
        mock_survey_2 = mocker.patch.object(
            ProjectSurveyModel.data_type_actual_survey_request_datetime_index, "query"
        )
        mock_survey_2.return_value = project_survey_models
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        response = client.get(f"/api/surveys/me?sort={sort}&limit={limit}")
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "sort, project_id, project_survey_models, project_model, expected",
        [
            (
                "actual_survey_request_datetime:desc",
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                [
                    ProjectSurveyModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        project_name="テストプロジェクト",
                        plan_survey_response_datetime="2022/07/15",
                        actual_survey_request_datetime="2022/07/01",
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        answer_user_name="田中太郎",
                        is_disclosure=False,
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/07/01",
                        actual_survey_response_datetime="2022/07/15",
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        is_finished=False,
                        survey_master_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        survey_type=SurveyType.PP,
                        customer_name="テスト株式会社",
                    ),
                ],
                ProjectModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="テストプロジェクト",
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
                    main_customer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                {
                    "total": 1,
                    "surveys": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "surveyType": "pp",
                            "projectName": "テストプロジェクト",
                            "summaryMonth": "2022/01",
                            "actualSurveyRequestDatetime": "2022/07/01",
                            "planSurveyResponseDatetime": "2022/07/15",
                            "actualSurveyResponseDatetime": "2022/07/15",
                            "answerUserName": "田中太郎",
                            "customerName": "テスト株式会社",
                            "company": None,
                            "isFinished": False,
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_customer_user")
    def test_ok_specify_project_id(
        self, mocker, sort, project_id, project_survey_models, project_model, expected
    ):

        mock = mocker.patch.object(
            ProjectSurveyModel.project_id_actual_survey_request_datetime_index, "query"
        )
        mock.return_value = project_survey_models
        mock_survey_2 = mocker.patch.object(
            ProjectSurveyModel.data_type_actual_survey_request_datetime_index, "query"
        )
        mock_survey_2.return_value = project_survey_models
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        response = client.get(f"/api/surveys/me?sort={sort}&project_id={project_id}")
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "sort, project_id, expected",
        [
            (
                "actual_survey_request_datetime:desc",
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                {"total": 0, "surveys": []},
            )
        ],
    )
    @pytest.mark.usefixtures("auth_customer_user")
    def test_ok_no_survey(self, mocker, sort, project_id, expected):

        mock = mocker.patch.object(ProjectIdActualSurveyRequestDatetimeIndex, "query")
        mock.return_value = []
        mock_survey_2 = mocker.patch.object(
            DataTypeActualSurveyRequestDatetimeIndex, "query"
        )
        mock_survey_2.return_value = []

        response = client.get(f"/api/surveys/me?sort={sort}&project_id={project_id}")
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
