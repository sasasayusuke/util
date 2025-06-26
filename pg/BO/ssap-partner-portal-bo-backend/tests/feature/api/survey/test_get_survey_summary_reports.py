import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.project import ProjectModel
from app.models.project_survey import (
    AnswersAttribute,
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.resources.const import DataType, SurveyType, UserRoleType
from app.service.survey_service import SurveyService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetSurveySummaryReports:
    @pytest.mark.parametrize(
        "project_survey_models, expected",
        [
            (
                [
                    ProjectSurveyModel(
                        id="33333449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type=SurveyType.SERVICE,
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
                        summary_month="2022/04",
                        plan_survey_request_datetime="2022/04/15 01:00",
                        actual_survey_request_datetime="2022/04/15 01:10",
                        plan_survey_response_datetime="2022/04/30 17:00",
                        actual_survey_response_datetime="2022/04/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=False,
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
                    ),
                    ProjectSurveyModel(
                        id="33333449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type=SurveyType.SERVICE,
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
                        summary_month="2022/05",
                        plan_survey_request_datetime="2022/04/15 01:00",
                        actual_survey_request_datetime="2022/04/15 01:10",
                        plan_survey_response_datetime="2022/04/30 17:00",
                        # actual_survey_response_datetime="2022/04/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=False,
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
                    ),
                    ProjectSurveyModel(
                        id="33333449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type=SurveyType.COMPLETION,
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
                        summary_month="2022/04",
                        plan_survey_request_datetime="2022/04/15 01:00",
                        actual_survey_request_datetime="2022/04/15 01:10",
                        plan_survey_response_datetime="2022/04/30 17:00",
                        actual_survey_response_datetime="2022/04/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=False,
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
                    ),
                    ProjectSurveyModel(
                        id="33333449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type=SurveyType.COMPLETION,
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
                        summary_month="2022/05",
                        plan_survey_request_datetime="2022/04/15 01:00",
                        actual_survey_request_datetime="2022/04/15 01:10",
                        plan_survey_response_datetime="2022/04/30 17:00",
                        # actual_survey_response_datetime="2022/04/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=False,
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
                    ),
                    ProjectSurveyModel(
                        id="33333449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type=SurveyType.PP,
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
                        summary_month="2022/04",
                        plan_survey_request_datetime="2022/04/15 01:00",
                        actual_survey_request_datetime="2022/04/15 01:10",
                        plan_survey_response_datetime="2022/04/30 17:00",
                        actual_survey_response_datetime="2022/04/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=False,
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
                    ),
                    ProjectSurveyModel(
                        id="33333449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type=SurveyType.PP,
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
                        summary_month="2022/05",
                        plan_survey_request_datetime="2022/04/15 01:00",
                        actual_survey_request_datetime="2022/04/15 01:10",
                        plan_survey_response_datetime="2022/04/30 17:00",
                        # actual_survey_response_datetime="2022/04/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=False,
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
                    ),
                ],
                {
                    "summaryMonth": "2022/04",
                    "service": {
                        "plan": 1,
                        "collect": 1,
                        "percent": 100,
                        "summaryPlan": 2,
                        "summaryCollect": 1,
                        "summaryPercent": 50,
                    },
                    "completion": {
                        "plan": 1,
                        "collect": 1,
                        "percent": 100,
                        "summaryPlan": 2,
                        "summaryCollect": 1,
                        "summaryPercent": 50,
                    },
                    "serviceAndCompletion": {
                        "plan": 2,
                        "collect": 2,
                        "percent": 100,
                        "summaryPlan": 4,
                        "summaryCollect": 2,
                        "summaryPercent": 50,
                    },
                    "pp": {
                        "plan": 1,
                        "collect": 1,
                        "percent": 100,
                        "summaryPlan": 2,
                        "summaryCollect": 1,
                        "summaryPercent": 50,
                    },
                },
            )
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, project_survey_models, expected):

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        project_survey_mock = mocker.patch.object(
            ProjectSurveyModel.data_type_summary_month_index, "query"
        )
        project_survey_mock.return_value = project_survey_models
        mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock = mocker.patch.object(SurveyService, "is_visible_survey_for_get_surveys")
        mock.return_value = True

        response = client.get(
            "/api/surveys/summary/reports?summaryMonth=202204", headers=REQUEST_HEADERS
        )

        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_params",
        [
            (
                "summaryMonth: 2022/03",
                "summaryMonth: 2022123",
                "summaryMonth: 2022",
            )
        ],
    )
    def test_validation(self, mock_auth_admin, query_params):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(
            f"/api/surveys/summary/reports?{query_params}", headers=REQUEST_HEADERS
        )
        response.status_code == 400
