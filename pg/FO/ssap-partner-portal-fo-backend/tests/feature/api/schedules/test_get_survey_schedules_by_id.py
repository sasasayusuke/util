import pytest
from app.main import app
from app.models.project import ProjectModel
from app.models.project_survey import (
    AnswersAttribute,
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.resources.const import DataType
from app.service.schedule_service import SchedulesService
from fastapi.testclient import TestClient

client = TestClient(app)


class TestGetSurveySchedulesById:
    @pytest.mark.parametrize(
        "project_id, project_model, survey_model, expected",
        [
            (
                "2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                ProjectModel(
                    id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                    survey_group_id="2833fc20-caea-44bb-ada5-d5ec3595f4c0",
                ),
                [
                    ProjectSurveyModel(
                        id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="quick",
                        project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
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
                        answer_user_id="",
                        answer_user_name="",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=[],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/08/01 09:00",
                        actual_survey_request_datetime="2022/08/01 01:10",
                        plan_survey_response_datetime="2022/09/01 17:00",
                        is_finished=False,
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
                        dedicated_survey_user_name="",
                        dedicated_survey_user_email="test@test.example.com",
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectSurveyModel(
                        id="1104d11d-b53e-4aee-acc9-a08e1daab69d",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="quick",
                        project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
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
                        answers=[],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/08/01 09:00",
                        actual_survey_request_datetime="2022/08/01 01:10",
                        plan_survey_response_datetime="2022/09/01 17:00",
                        is_finished=False,
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
                        dedicated_survey_user_name="",
                        dedicated_survey_user_email="test@test.example.com",
                        survey_group_id="2833fc20-caea-44bb-ada5-d5ec3595f4c0",
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectSurveyModel(
                        id="2204d11d-b53e-4aee-acc9-a08e1daab69d",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="quick",
                        project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
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
                        answer_user_id="",
                        answer_user_name="",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=[],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/08/01 09:00",
                        actual_survey_request_datetime="2022/08/01 01:10",
                        plan_survey_response_datetime="2022/09/01 17:00",
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
                        dedicated_survey_user_name="",
                        dedicated_survey_user_email="test@test.example.com",
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                {
                    "projectId": "2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                    "surveyGroupId": "2833fc20-caea-44bb-ada5-d5ec3595f4c0",
                    "projectSchedules": [
                        {
                            "scheduleGroupId": "",
                            "surveyId": "9904d11d-b53e-4aee-acc9-a08e1daab69d",
                            "sendDate": "2022/08/01",
                            "surveyName": "quick",
                            "surveyLimitDate": "2022/09/01",
                            "version": 1,
                        },
                        {
                            "scheduleGroupId": "2833fc20-caea-44bb-ada5-d5ec3595f4c0",
                            "surveyId": "1104d11d-b53e-4aee-acc9-a08e1daab69d",
                            "sendDate": "2022/08/01",
                            "surveyName": "quick",
                            "surveyLimitDate": "2022/09/01",
                            "version": 1,
                        },
                        {
                            "scheduleGroupId": "",
                            "surveyId": "2204d11d-b53e-4aee-acc9-a08e1daab69d",
                            "sendDate": "2022/08/01",
                            "surveyName": "quick",
                            "surveyLimitDate": "2022/09/01",
                            "version": 1,
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, project_id, project_model, survey_model, expected):
        """正常系のテスト"""
        mocker_method = mocker.patch.object(
            SchedulesService, "is_visible_project_get_by_id"
        )
        mocker_method.return_value = True

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_project_survey.return_value = survey_model

        response = client.get(f"/api/schedules/survey/{project_id}")
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected
