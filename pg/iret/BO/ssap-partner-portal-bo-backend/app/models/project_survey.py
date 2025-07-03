from datetime import datetime

from pynamodb.attributes import (
    BooleanAttribute,
    ListAttribute,
    MapAttribute,
    NumberAttribute,
    UnicodeAttribute,
    UnicodeSetAttribute,
    VersionAttribute,
)
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute


class DataTypeSummaryMonthIndex(GlobalSecondaryIndex):
    """GSI：データ区分集計月検索"""

    class Meta:
        index_name = "data_type-summary_month-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    summary_month = UnicodeAttribute(range_key=True)


class DataTypePlanSurveyRequestDatetimeIndex(GlobalSecondaryIndex):
    """TODO FOで使用していない"""

    class Meta:
        index_name = "data_type-plan_survey_request_datetime-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    plan_survey_request_datetime = UnicodeAttribute(range_key=True)


class DataTypeActualSurveyRequestDatetimeIndex(GlobalSecondaryIndex):
    """TODO BOで使用していない"""

    class Meta:
        index_name = "data_type-actual_survey_request_datetime-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    actual_survey_request_datetime = UnicodeAttribute(range_key=True)


class DataTypeActualSurveyResponseDatetimeIndex(GlobalSecondaryIndex):
    """TODO FO&BOで使用していない"""

    class Meta:
        index_name = "data_type-actual_survey_response_datetime-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    actual_survey_response_datetime = UnicodeAttribute(range_key=True)


class DataTypePlanSurveyResponseDatetimeIndex(GlobalSecondaryIndex):
    """GSI：データ区分回答（受信）実績日時検索"""

    class Meta:
        index_name = "data_type-plan_survey_response_datetime-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    plan_survey_response_datetime = UnicodeAttribute(range_key=True)


class ServiceTypeSummaryMonthIndex(GlobalSecondaryIndex):
    """TODO BO&FOで使用していない"""

    class Meta:
        index_name = "service_type_id-summary_month-index"
        projection = AllProjection()

    service_type_id = UnicodeAttribute(hash_key=True)
    summary_month = UnicodeAttribute(range_key=True)


class ProjectIdSummaryMonthIndex(GlobalSecondaryIndex):
    """GSI：案件ID集計月検索"""

    class Meta:
        index_name = "project_id-summary_month-index"
        projection = AllProjection()

    project_id = UnicodeAttribute(hash_key=True)
    summary_month = UnicodeAttribute(range_key=True)


class ProjectIdActualSurveyRequestDatetimeIndex(GlobalSecondaryIndex):
    class Meta:
        index_name = "project_id-actual_survey_request_datetime-index"
        projection = AllProjection()

    project_id = UnicodeAttribute(hash_key=True)
    actual_survey_request_datetime = UnicodeAttribute(range_key=True)


class SurveyTypeSummaryMonthIndex(GlobalSecondaryIndex):
    """GSI：サービス種別集計月検索"""

    class Meta:
        index_name = "survey_type-summary_month-index"
        projection = AllProjection()

    survey_type = UnicodeAttribute(hash_key=True)
    summary_month = UnicodeAttribute(range_key=True)


class SurveyTypeActualSurveyResponseDatetimeIndex(GlobalSecondaryIndex):
    """GSI：アンケート種別回答（受信）実績日時検索"""

    class Meta:
        index_name = "survey_type-actual_survey_response_datetime-index"
        projection = AllProjection()

    survey_type = UnicodeAttribute(hash_key=True)
    actual_survey_response_datetime = UnicodeAttribute(range_key=True)


class ProjectIdActualSurveyResponseDatetimeIndex(GlobalSecondaryIndex):
    """GSI：案件ID回答依頼実績日時検索"""

    class Meta:
        index_name = "project_id-actual_survey_response_datetime-index"
        projection = AllProjection()

    project_id = UnicodeAttribute(hash_key=True)
    actual_survey_response_datetime = UnicodeAttribute(range_key=True)


class AnswersAttribute(MapAttribute):
    """案件アンケートTBL Attribute定義"""

    id = UnicodeAttribute(null=False)
    answer = UnicodeAttribute(null=False)
    point = NumberAttribute(null=True)
    choice_ids = UnicodeSetAttribute(null=True)
    summary_type = UnicodeAttribute(null=True)
    other_input = UnicodeAttribute(null=True)


class PointsAttribute(MapAttribute):
    """案件アンケートTBL Attribute定義"""

    satisfaction = NumberAttribute(null=False, default=0)
    continuation = BooleanAttribute(null=False, default=False)
    recommended = NumberAttribute(null=False, default=0)
    sales = NumberAttribute(null=False, default=0)
    survey_satisfaction = NumberAttribute(null=False, default=0)
    man_hour_satisfaction = NumberAttribute(null=False, default=0)
    karte_satisfaction = NumberAttribute(null=False, default=0)
    master_karte_satisfaction = NumberAttribute(null=False, default=0)
    unanswered = UnicodeSetAttribute(null=True)


class SupporterUserAttribute(MapAttribute):
    """案件アンケートTBL Attribute定義"""

    id = UnicodeAttribute(null=False)
    name = UnicodeAttribute(null=False)
    organization_id = UnicodeAttribute(null=False)
    organization_name = UnicodeAttribute(null=False)


class EvaluationSupporterAttribute(MapAttribute):
    """案件アンケートTBL Attribute定義"""

    supporter_id = UnicodeAttribute(null=False)
    karte_ids = ListAttribute(null=False)


class ProjectSurveyModel(Model):
    """案件アンケートTBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "ProjectSurvey"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)

    survey_master_id = UnicodeAttribute(null=False)
    survey_master_revision = NumberAttribute(null=False)
    survey_type = UnicodeAttribute(null=False)
    project_id = UnicodeAttribute(null=True)
    project_name = UnicodeAttribute(null=True)
    customer_success = UnicodeAttribute(null=True)
    supporter_organization_id = UnicodeAttribute(null=True)
    supporter_organization_name = UnicodeAttribute(null=True)
    support_date_from = UnicodeAttribute(null=True)
    support_date_to = UnicodeAttribute(null=True)
    main_supporter_user = SupporterUserAttribute(null=True)
    supporter_users = ListAttribute(of=SupporterUserAttribute, null=True)
    evaluation_supporters = ListAttribute(of=EvaluationSupporterAttribute, null=True)
    supporter_users_before_update = ListAttribute(of=SupporterUserAttribute, null=True)
    is_updated_evaluation_supporters = BooleanAttribute(null=True, default=False)
    sales_user_id = UnicodeAttribute(null=True)
    service_type_id = UnicodeAttribute(null=True)
    service_type_name = UnicodeAttribute(null=True)
    service_manager_id = UnicodeAttribute(null=True)
    service_manager_name = UnicodeAttribute(null=True)
    answer_user_id = UnicodeAttribute(null=True)
    answer_user_name = UnicodeAttribute(null=True)
    customer_id = UnicodeAttribute(null=True)
    customer_name = UnicodeAttribute(null=True)
    company = UnicodeAttribute(null=True)
    answers = ListAttribute(of=AnswersAttribute, null=True)
    summary_month = UnicodeAttribute(null=False)
    plan_survey_request_datetime = UnicodeAttribute(null=False)
    actual_survey_request_datetime = UnicodeAttribute(null=True)
    plan_survey_response_datetime = UnicodeAttribute(null=True)
    actual_survey_response_datetime = UnicodeAttribute(null=True)
    is_finished = BooleanAttribute(null=False, default=False)
    is_disclosure = BooleanAttribute(null=False, default=True)
    is_not_summary = BooleanAttribute(null=False, default=False)
    is_solver_project = BooleanAttribute(default=False)
    points = PointsAttribute(null=False)
    dedicated_survey_user_name = UnicodeAttribute(null=True)
    dedicated_survey_user_email = UnicodeAttribute(null=True)
    survey_group_id = UnicodeAttribute(null=True)
    unanswered_surveys_number = NumberAttribute(null=True)
    this_month_type = UnicodeAttribute(null=True)
    no_send_reason = UnicodeAttribute(null=True)

    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    data_type_summary_month_index = DataTypeSummaryMonthIndex()
    data_type_plan_survey_request_datetime_index = (
        DataTypePlanSurveyRequestDatetimeIndex()
    )
    data_type_actual_survey_request_datetime_index = (
        DataTypeActualSurveyRequestDatetimeIndex()
    )
    data_type_actual_survey_response_datetime_index = (
        DataTypeActualSurveyResponseDatetimeIndex()
    )
    data_type_plan_survey_response_datetime_index = (
        DataTypePlanSurveyResponseDatetimeIndex()
    )
    service_type_id_summary_month_index = ServiceTypeSummaryMonthIndex()
    project_id_summary_month_index = ProjectIdSummaryMonthIndex()
    survey_type_summary_month_index = SurveyTypeSummaryMonthIndex()
    project_id_actual_survey_request_datetime_index = (
        ProjectIdActualSurveyRequestDatetimeIndex()
    )
    survey_type_actual_survey_response_datetime_index = (
        SurveyTypeActualSurveyResponseDatetimeIndex()
    )
    project_id_actual_survey_response_datetime_index = (
        ProjectIdActualSurveyResponseDatetimeIndex()
    )
