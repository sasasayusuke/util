from datetime import datetime

from moto import mock_dynamodb

from app.models.survey_summary import (
    SurveySummaryAllModel,
    SurveySummarySupporterOrganizationModel,
    SurveySummaryUserModel,
)


@mock_dynamodb
def test_model_survey_summary_user():
    """アンケートサマリTBL SurveySummaryUserModelクラス テスト定義"""

    SurveySummaryUserModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    summary_month = datetime.now().strftime("%Y/%m/%d")
    data_type = "user#1"

    survey_summary_user = SurveySummaryUserModel(summary_month, data_type)

    survey_summary_user.name = "田中太郎"
    survey_summary_user.service_satisfaction_summary = 1
    survey_summary_user.service_satisfaction_average = 1
    survey_summary_user.service_satisfaction_unanswered = 1
    survey_summary_user.service_receive = 1
    survey_summary_user.completion_satisfaction_summary = 1
    survey_summary_user.completion_satisfaction_average = 1
    survey_summary_user.completion_satisfaction_unanswered = 1
    survey_summary_user.completion_continuation = {
        "positive_count": 1,
        "negative_count": 1,
    }
    survey_summary_user.completion_continuation_percent = 1
    survey_summary_user.completion_continuation_unanswered = 1
    survey_summary_user.completion_recommended_summary = 1
    survey_summary_user.completion_recommended_average = 1
    survey_summary_user.completion_recommended_unanswered = 1
    survey_summary_user.completion_sales_summary = 1
    survey_summary_user.completion_sales_average = 1
    survey_summary_user.completion_sales_unanswered = 1
    survey_summary_user.completion_receive = 1
    survey_summary_user.quick_receive = 1

    survey_summary_user.create_at = datetime.now()
    survey_summary_user.update_at = datetime.now()

    survey_summary_user.save()

    survey_summary_user_item = SurveySummaryUserModel.get(summary_month, data_type)
    survey_summary_user_item_by_query = (
        SurveySummaryUserModel.data_type_summary_month_index.query(
            data_type, summary_month
        )
    )

    assert str(survey_summary_user) == str(survey_summary_user_item)
    assert survey_summary_user_item_by_query is not None

    SurveySummaryUserModel.delete_table()


@mock_dynamodb
def test_model_survey_summary_supporter_organization():
    """アンケートサマリTBL SurveySummarySupporterOrganizationModelクラス テスト定義"""

    SurveySummarySupporterOrganizationModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    summary_month = datetime.now().strftime("%Y/%m/%d")
    data_type = "supporter_organization#1"

    survey_summary_supporter_organization = SurveySummarySupporterOrganizationModel(
        summary_month, data_type
    )

    survey_summary_supporter_organization.supporter_organization_name = "テスト"
    survey_summary_supporter_organization.service_satisfaction_summary = 1
    survey_summary_supporter_organization.service_satisfaction_average = 1
    survey_summary_supporter_organization.service_satisfaction_unanswered = 1
    survey_summary_supporter_organization.service_receive = 1
    survey_summary_supporter_organization.completion_satisfaction_summary = 1
    survey_summary_supporter_organization.completion_satisfaction_average = 1
    survey_summary_supporter_organization.completion_satisfaction_unanswered = 1
    survey_summary_supporter_organization.completion_continuation = {
        "positive_count": 1,
        "negative_count": 1,
    }
    survey_summary_supporter_organization.completion_continuation_percent = 1
    survey_summary_supporter_organization.completion_continuation_unanswered = 1
    survey_summary_supporter_organization.completion_recommended_summary = 1
    survey_summary_supporter_organization.completion_recommended_average = 1
    survey_summary_supporter_organization.completion_recommended_unanswered = 1
    survey_summary_supporter_organization.completion_sales_summary = 1
    survey_summary_supporter_organization.completion_sales_average = 1
    survey_summary_supporter_organization.completion_sales_unanswered = 1
    survey_summary_supporter_organization.completion_receive = 1
    survey_summary_supporter_organization.total_satisfaction_summary = 1
    survey_summary_supporter_organization.total_satisfaction_average = 1
    survey_summary_supporter_organization.total_receive = 1

    survey_summary_supporter_organization.create_at = datetime.now()
    survey_summary_supporter_organization.update_at = datetime.now()

    survey_summary_supporter_organization.save()

    survey_summary_supporter_organization_item = (
        SurveySummarySupporterOrganizationModel.get(summary_month, data_type)
    )
    survey_summary_supporter_organization_item_by_query = (
        SurveySummarySupporterOrganizationModel.data_type_summary_month_index.query(
            data_type, summary_month
        )
    )

    assert str(survey_summary_supporter_organization) == str(
        survey_summary_supporter_organization_item
    )
    assert survey_summary_supporter_organization_item_by_query is not None

    SurveySummarySupporterOrganizationModel.delete_table()


@mock_dynamodb
def test_model_survey_summary_all():
    """アンケートサマリTBL SurveySummaryAllModelクラス テスト定義"""

    SurveySummaryAllModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    summary_month = datetime.now().strftime("%Y/%m/%d")
    data_type = "all"

    survey_summary_all = SurveySummaryAllModel(summary_month, data_type)

    survey_summary_all.service = {
        "project_count": 1,
        "receive_count": 1,
        "satisfaction_summary": 1,
        "satisfaction_average": 1,
        "satisfaction_unanswered": 1,
    }
    survey_summary_all.completion = {
        "project_count": 1,
        "receive_count": 1,
        "receive_percent": 1,
        "satisfaction_summary": 1,
        "satisfaction_average": 1,
        "satisfaction_unanswered": 1,
        "continuation": {
            "positive_count": 1,
            "negative_count": 1,
            "percent": 1,
            "unanswered": 1,
        },
        "recommended_summary": 1,
        "recommended_average": 1,
        "recommended_unanswered": 1,
        "sales_summary": 1,
        "sales_average": 1,
        "sales_unanswered": 1,
    }
    survey_summary_all.karte = {"project_count": 1, "karte_count": 1, "use_percent": 1}
    survey_summary_all.quick = {"send_count": 1, "receive_count": 1}
    survey_summary_all.pp = {
        "survey_satisfaction_summary": 1,
        "survey_satisfaction_average": 1,
        "man_hour_satisfaction_summary": 1,
        "man_hour_satisfaction_average": 1,
        "karte_satisfaction_summary": 1,
        "karte_satisfaction_average": 1,
        "master_karte_satisfaction_summary": 1,
        "master_karte_satisfaction_average": 1,
        "send_count": 1,
        "receive_count": 1,
        "receive_percent": 1,
    }
    survey_summary_all.create_at = datetime.now()
    survey_summary_all.update_at = datetime.now()

    survey_summary_all.save()

    survey_summary_all_item = SurveySummaryAllModel.get(summary_month, data_type)
    survey_summary_all_item_by_query = (
        SurveySummaryAllModel.data_type_summary_month_index.query(
            data_type, summary_month
        )
    )

    assert str(survey_summary_all) == str(survey_summary_all_item)
    assert survey_summary_all_item_by_query is not None

    SurveySummaryAllModel.delete_table()
