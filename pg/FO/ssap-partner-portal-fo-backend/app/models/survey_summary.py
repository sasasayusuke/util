from datetime import datetime

from pynamodb.attributes import MapAttribute, NumberAttribute, UnicodeAttribute
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute


class DataTypeSummaryMonthIndexForUser(GlobalSecondaryIndex):
    """GSI：データ区分集計月検索"""

    class Meta:
        index_name = "data_type-summary_month-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    summary_month = UnicodeAttribute(range_key=True)


class DataTypeSummaryMonthIndexForSupporterOrganization(GlobalSecondaryIndex):
    """GSI：データ区分集計月検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-summary_month-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    summary_month = UnicodeAttribute(range_key=True)


class DataTypeSummaryMonthIndexForAll(GlobalSecondaryIndex):
    """GSI：データ区分集計月検索
    複数ファセットを持つテーブルのため、別名でGSIを定義する
    """

    class Meta:
        index_name = "data_type-summary_month-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    summary_month = UnicodeAttribute(range_key=True)


class CompletionContinuationSubAttribute(MapAttribute):
    """アンケートサマリTBL FASET共通 Attribute定義"""

    positive_count = NumberAttribute(null=False, default=0)
    negative_count = NumberAttribute(null=False, default=0)


class SurveySummaryUserModel(Model):
    """アンケートサマリTBL 支援者(営業担当者)別集計FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "SurveySummary"
        region = get_app_settings().aws_region

    summary_month = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    service_satisfaction_summary = NumberAttribute(null=False, default=0)
    service_satisfaction_average = NumberAttribute(null=False, default=0)
    service_satisfaction_unanswered = NumberAttribute(null=False, default=0)
    service_receive = NumberAttribute(null=False, default=0)
    completion_satisfaction_summary = NumberAttribute(null=False, default=0)
    completion_satisfaction_average = NumberAttribute(null=False, default=0)
    completion_satisfaction_unanswered = NumberAttribute(null=False, default=0)
    completion_continuation = CompletionContinuationSubAttribute(null=False)
    completion_continuation_percent = NumberAttribute(null=False, default=0)
    completion_continuation_unanswered = NumberAttribute(null=False, default=0)
    completion_recommended_summary = NumberAttribute(null=False, default=0)
    completion_recommended_average = NumberAttribute(null=False, default=0)
    completion_recommended_unanswered = NumberAttribute(null=False, default=0)
    completion_sales_summary = NumberAttribute(null=False, default=0)
    completion_sales_average = NumberAttribute(null=False, default=0)
    completion_sales_unanswered = NumberAttribute(null=False, default=0)
    completion_receive = NumberAttribute(null=False, default=0)
    quick_receive = NumberAttribute(null=False, default=0)

    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())

    data_type_summary_month_index = DataTypeSummaryMonthIndexForUser()


class SurveySummarySupporterOrganizationModel(Model):
    """アンケートサマリTBL 支援者組織(粗利メイン課)別集計FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "SurveySummary"
        region = get_app_settings().aws_region

    summary_month = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    supporter_organization_name = UnicodeAttribute(null=False)
    service_satisfaction_summary = NumberAttribute(null=False, default=0)
    service_satisfaction_average = NumberAttribute(null=False, default=0)
    service_satisfaction_unanswered = NumberAttribute(null=False, default=0)
    service_receive = NumberAttribute(null=False, default=0)
    completion_satisfaction_summary = NumberAttribute(null=False, default=0)
    completion_satisfaction_average = NumberAttribute(null=False, default=0)
    completion_satisfaction_unanswered = NumberAttribute(null=False, default=0)
    completion_continuation = CompletionContinuationSubAttribute(null=False)
    completion_continuation_percent = NumberAttribute(null=False, default=0)
    completion_continuation_unanswered = NumberAttribute(null=False, default=0)
    completion_recommended_summary = NumberAttribute(null=False, default=0)
    completion_recommended_average = NumberAttribute(null=False, default=0)
    completion_recommended_unanswered = NumberAttribute(null=False, default=0)
    completion_sales_summary = NumberAttribute(null=False, default=0)
    completion_sales_average = NumberAttribute(null=False, default=0)
    completion_sales_unanswered = NumberAttribute(null=False, default=0)
    completion_receive = NumberAttribute(null=False, default=0)
    total_satisfaction_summary = NumberAttribute(null=False, default=0)
    total_satisfaction_average = NumberAttribute(null=False, default=0)
    total_receive = NumberAttribute(null=False, default=0)

    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())

    data_type_summary_month_index = DataTypeSummaryMonthIndexForSupporterOrganization()


class ContinuationSubAttribute(MapAttribute):
    """アンケートサマリTBL 全集計FASET Attribute定義"""

    positive_count = NumberAttribute(null=False, default=0)
    negative_count = NumberAttribute(null=False, default=0)
    percent = NumberAttribute(null=False, default=0)
    unanswered = NumberAttribute(null=False, default=0)


class ServiceAttribute(MapAttribute):
    """アンケートサマリTBL 全集計FASET Attribute定義"""

    project_count = NumberAttribute(null=False, default=0)
    receive_count = NumberAttribute(null=False, default=0)
    receive_percent = NumberAttribute(null=False, default=0)
    satisfaction_summary = NumberAttribute(null=False, default=0)
    satisfaction_average = NumberAttribute(null=False, default=0)
    satisfaction_unanswered = NumberAttribute(null=False, default=0)


class CompletionAttribute(MapAttribute):
    """アンケートサマリTBL 全集計FASET Attribute定義"""

    project_count = NumberAttribute(null=False, default=0)
    receive_count = NumberAttribute(null=False, default=0)
    receive_percent = NumberAttribute(null=False, default=0)
    satisfaction_summary = NumberAttribute(null=False, default=0)
    satisfaction_average = NumberAttribute(null=False, default=0)
    satisfaction_unanswered = NumberAttribute(null=False, default=0)
    continuation = ContinuationSubAttribute(null=False)
    recommended_summary = NumberAttribute(null=False, default=0)
    recommended_average = NumberAttribute(null=False, default=0)
    recommended_unanswered = NumberAttribute(null=False, default=0)
    sales_summary = NumberAttribute(null=False, default=0)
    sales_average = NumberAttribute(null=False, default=0)
    sales_unanswered = NumberAttribute(null=False, default=0)


class KarteAttribute(MapAttribute):
    """アンケートサマリTBL 全集計FASET Attribute定義"""

    project_count = NumberAttribute(null=False, default=0)
    karte_count = NumberAttribute(null=False, default=0)
    use_percent = NumberAttribute(null=False, default=0)


class QuickAttribute(MapAttribute):
    """アンケートサマリTBL 全集計FASET Attribute定義"""

    send_count = NumberAttribute(null=False, default=0)
    receive_count = NumberAttribute(null=False, default=0)


class PpAttribute(MapAttribute):
    """アンケートサマリTBL 全集計FASET Attribute定義"""

    survey_satisfaction_summary = NumberAttribute(null=False, default=0)
    survey_satisfaction_average = NumberAttribute(null=False, default=0)
    man_hour_satisfaction_summary = NumberAttribute(null=False, default=0)
    man_hour_satisfaction_average = NumberAttribute(null=False, default=0)
    karte_satisfaction_summary = NumberAttribute(null=False, default=0)
    karte_satisfaction_average = NumberAttribute(null=False, default=0)
    master_karte_satisfaction_summary = NumberAttribute(null=False, default=0)
    master_karte_satisfaction_average = NumberAttribute(null=False, default=0)
    send_count = NumberAttribute(null=False, default=0)
    receive_count = NumberAttribute(null=False, default=0)
    receive_percent = NumberAttribute(null=False, default=0)


class SurveySummaryAllModel(Model):
    """アンケートサマリTBL 全集計FASET モデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "SurveySummary"
        region = get_app_settings().aws_region

    summary_month = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    service = ServiceAttribute(null=False)
    completion = CompletionAttribute(null=False)
    karte = KarteAttribute(null=False)
    quick = QuickAttribute(null=False)
    pp = PpAttribute(null=False)

    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())

    data_type_summary_month_index = DataTypeSummaryMonthIndexForAll()
