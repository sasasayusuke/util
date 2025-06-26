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


class ProjectIdStartDatetimeIndex(GlobalSecondaryIndex):
    """GSI：案件ID支援開始日時ソート"""

    class Meta:
        index_name = "project_id-start_datetime-index"
        projection = AllProjection()

    project_id = UnicodeAttribute(hash_key=True)
    start_datetime = UnicodeAttribute(range_key=True)


class DateIndex(GlobalSecondaryIndex):
    """GSI：支援日検索"""

    class Meta:
        index_name = "date-index"
        projection = AllProjection()

    date = UnicodeAttribute(hash_key=True)


class DocumentsDeliverablesAttribute(MapAttribute):
    """案件カルテTBL Attribute定義"""

    file_name = UnicodeAttribute(null=True)
    path = UnicodeAttribute(null=True)


class KarteNotifyUpdateHistoryAttribute(MapAttribute):
    """案件カルテTBL Attribute定義"""

    user_id = UnicodeAttribute(null=False)
    user_name = UnicodeAttribute(null=False)
    organization_id = UnicodeAttribute(null=False)
    organization_name = UnicodeAttribute(null=False)
    karte_update_date = UnicodeAttribute(null=False)


class LocationAttribute(MapAttribute):
    """案件カルテTBL Attribute定義"""

    type = UnicodeAttribute(null=True)
    detail = UnicodeAttribute(null=True)


class ProjectKarteModel(Model):
    """案件カルテTBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "ProjectKarte"
        region = get_app_settings().aws_region

    karte_id = UnicodeAttribute(hash_key=True, null=False)
    project_id = UnicodeAttribute(null=False)
    customer_id = UnicodeAttribute(null=False)
    date = UnicodeAttribute(null=False)
    start_datetime = UnicodeAttribute(null=True)
    start_time = UnicodeAttribute(null=True)
    end_time = UnicodeAttribute(null=True)
    supporter_ids = UnicodeSetAttribute(null=True)
    draft_supporter_id = UnicodeAttribute(null=True)
    last_update_datetime = JSTDateTimeAttribute(null=True)
    karte_notify_last_update_datetime = JSTDateTimeAttribute(null=True)
    karte_notify_update_history = ListAttribute(
        of=KarteNotifyUpdateHistoryAttribute, null=True
    )
    customer_user_ids = UnicodeSetAttribute(null=True)
    start_support_actual_time = UnicodeAttribute(null=True)
    end_support_actual_time = UnicodeAttribute(null=True)
    man_hour = NumberAttribute(null=False, default=0)
    customers = UnicodeAttribute(null=True)
    support_team = UnicodeAttribute(null=True)
    detail = UnicodeAttribute(null=True)
    feedback = UnicodeAttribute(null=True)
    homework = UnicodeAttribute(null=True)
    documents = ListAttribute(of=DocumentsDeliverablesAttribute, null=True)
    deliverables = ListAttribute(of=DocumentsDeliverablesAttribute, null=True)
    memo = UnicodeAttribute(null=True)
    human_resource_needed_for_customer = UnicodeAttribute(null=True)
    company_and_industry_recommended_to_customer = UnicodeAttribute(null=True)
    human_resource_needed_for_support = UnicodeAttribute(null=True)
    task = UnicodeAttribute(null=True)
    location = LocationAttribute(null=True)
    is_draft = BooleanAttribute(null=False, default=True)

    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    project_id_start_datetime_index = ProjectIdStartDatetimeIndex()
    date_index = DateIndex()
