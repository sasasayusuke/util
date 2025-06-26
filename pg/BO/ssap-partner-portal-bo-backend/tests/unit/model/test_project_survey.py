from datetime import datetime

from moto import mock_dynamodb

from app.models.project_survey import ProjectSurveyModel


@mock_dynamodb
def test_model_project_survey():
    """案件アンケートTBL ProjectSurveyModelクラス テスト定義"""

    ProjectSurveyModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    id = "survey1"
    data_type = "survey"

    project_survey = ProjectSurveyModel(id, data_type)

    project_survey.survey_master_id = "survey-master"
    project_survey.survey_master_revision = 1
    project_survey.survey_type = "service"
    project_survey.points = {
        "satisfaction": 100,
        "continuation": True,
        "recommended": 1,
        "sales": 5,
        "survey_satisfaction": 100,
        "man_hour_satisfaction": 90,
        "karte_satisfaction": 80,
        "master_karte_satisfaction": 0,
    }
    project_survey.summary_month = datetime.now().strftime("%Y/%m")
    project_survey.plan_survey_request_datetime = datetime.now().strftime("%Y/%m/%d")
    project_survey.plan_survey_response_datetime = datetime.now().strftime("%Y/%m/%d")
    project_survey.is_finished = True
    project_survey.is_disclosure = True
    project_survey.is_not_summary = True
    project_survey.create_at = datetime.now()
    project_survey.update_at = datetime.now()

    project_survey.save()

    project_survey_item = ProjectSurveyModel.get(id, data_type)
    project_survey_item_by_query_summary_month = (
        ProjectSurveyModel.data_type_summary_month_index.query(
            data_type, project_survey.summary_month
        )
    )
    project_survey_item_by_query_plan_survey = (
        ProjectSurveyModel.data_type_plan_survey_request_datetime_index.query(
            data_type, project_survey.plan_survey_request_datetime
        )
    )
    project_survey_item_by_query_survey_type = (
        ProjectSurveyModel.survey_type_summary_month_index.query(
            project_survey.survey_type, project_survey.summary_month
        )
    )

    assert str(project_survey) == str(project_survey_item)
    assert project_survey_item_by_query_summary_month is not None
    assert project_survey_item_by_query_plan_survey is not None
    assert project_survey_item_by_query_survey_type is not None

    ProjectSurveyModel.delete_table()
