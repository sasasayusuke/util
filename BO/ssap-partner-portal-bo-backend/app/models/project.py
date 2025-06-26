from datetime import datetime

from fastapi import HTTPException, status
from pynamodb.attributes import (
    BooleanAttribute,
    ListAttribute,
    MapAttribute,
    NumberAttribute,
    UnicodeAttribute,
    UnicodeSetAttribute,
    VersionAttribute,
)
from pynamodb.exceptions import DoesNotExist
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute
from app.resources.const import DataType


class CustomerIdNameIndex(GlobalSecondaryIndex):
    """GSI：顧客ID案件名検索"""

    class Meta:
        index_name = "customer_id-name-index"
        projection = AllProjection()

    customer_id = UnicodeAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class DataTypeNameIndex(GlobalSecondaryIndex):
    """GSI：データ区分案件名検索"""

    class Meta:
        index_name = "data_type-name-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class DataTypeSupportDateToIndex(GlobalSecondaryIndex):
    """GSI：支援終了日検索"""

    class Meta:
        index_name = "data_type-support_date_to-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    support_date_to = UnicodeAttribute(range_key=True)


class GrossProfitAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    monthly = ListAttribute(null=True)
    quarterly = ListAttribute(null=True)
    half = ListAttribute(null=True)
    year = NumberAttribute(null=True)


class SalesforceMainCustomerAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    name = UnicodeAttribute(null=True)
    email = UnicodeAttribute(null=True)
    organization_name = UnicodeAttribute(null=True)
    job = UnicodeAttribute(null=True)


class UpdateAttributesSubAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    attribute = UnicodeAttribute(null=True)
    value = UnicodeAttribute(null=True)


class UpdateHistoryAttribute(MapAttribute):
    """案件TBL Attribute定義"""

    update_id = UnicodeAttribute(null=True)
    update_attributes = ListAttribute(of=UpdateAttributesSubAttribute, null=True)


class ProjectModel(Model):
    """案件TBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Project"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    customer_id = UnicodeAttribute(null=False)
    salesforce_customer_id = UnicodeAttribute(null=True)
    salesforce_opportunity_id = UnicodeAttribute(null=True)
    salesforce_update_at = JSTDateTimeAttribute(null=True)
    name = UnicodeAttribute(null=False)
    customer_name = UnicodeAttribute(null=True)
    service_type = UnicodeAttribute(null=True)
    create_new = BooleanAttribute(null=True)
    continued = BooleanAttribute(null=True)
    main_sales_user_id = UnicodeAttribute(null=False)
    contract_date = UnicodeAttribute(null=True)
    phase = UnicodeAttribute(null=True)
    customer_success = UnicodeAttribute(null=True)
    support_date_from = UnicodeAttribute(null=False)
    support_date_to = UnicodeAttribute(null=False)
    profit = GrossProfitAttribute(null=True)
    gross = GrossProfitAttribute(null=True)
    total_contract_time = NumberAttribute(null=False)
    main_customer_user_id = UnicodeAttribute(null=True)
    salesforce_main_customer = SalesforceMainCustomerAttribute(null=True)
    customer_user_ids = UnicodeSetAttribute(null=True)
    service_manager_name = UnicodeAttribute(null=True)
    main_supporter_user_id = UnicodeAttribute(null=True)
    supporter_organization_id = UnicodeAttribute(null=True)
    salesforce_main_supporter_user_name = UnicodeAttribute(null=True)
    supporter_user_ids = UnicodeSetAttribute(null=True)
    salesforce_supporter_user_names = UnicodeSetAttribute(null=True)
    is_count_man_hour = BooleanAttribute(null=False, default=True)
    is_karte_remind = BooleanAttribute(null=False, default=True)
    is_master_karte_remind = BooleanAttribute(null=False, default=True)
    contract_type = UnicodeAttribute(null=True)
    is_secret = BooleanAttribute(null=False, default=False)
    is_solver_project = BooleanAttribute(default=False)
    salesforce_use_package = BooleanAttribute(null=True)
    salesforce_via_pr = BooleanAttribute(null=True)
    update_history = UpdateHistoryAttribute(null=True)
    survey_group_id = UnicodeAttribute(null=True)
    is_man_hour_input = BooleanAttribute(null=True, default=False)
    dedicated_survey_user_name = UnicodeAttribute(null=True)
    dedicated_survey_user_email = UnicodeAttribute(null=True)
    survey_password = UnicodeAttribute(null=True)
    is_survey_email_to_salesforce_main_customer = BooleanAttribute(null=True)
    this_month_type = UnicodeAttribute(null=True)
    no_send_reason = UnicodeAttribute(null=True)

    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    customer_id_name_index = CustomerIdNameIndex()
    data_type_support_date_to_index = DataTypeSupportDateToIndex()
    data_type_name_index = DataTypeNameIndex()

    @staticmethod
    def get_project(project_id: str):
        try:
            return ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)
        except DoesNotExist:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Project not found."
            )
