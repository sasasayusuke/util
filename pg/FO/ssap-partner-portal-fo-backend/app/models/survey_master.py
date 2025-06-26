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
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute
from app.resources.const import SurveyRevisionStatusNumber, SurveyTypeExcludingPP


class IsLatestNameIndex(GlobalSecondaryIndex):
    """GSI：最新バージョン一覧検索"""

    class Meta:
        index_name = "is_latest-name-index"
        projection = AllProjection()

    is_latest = NumberAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class IdIsLatestIndex(GlobalSecondaryIndex):
    """GSI：最新バージョン取得"""

    class Meta:
        index_name = "id-is_latest-index"
        projection = AllProjection()

    id = UnicodeAttribute(hash_key=True)
    is_latest = NumberAttribute(range_key=True)


class TypeIsLatestIndex(GlobalSecondaryIndex):
    class Meta:
        index_name = "type-is_latest-index"
        projection = AllProjection()

    type = UnicodeAttribute(hash_key=True)
    is_latest = NumberAttribute(range_key=True)


class GroupSubAttribute(MapAttribute):
    """アンケートマスタTBL Attribute定義"""

    id = UnicodeAttribute(null=True)
    title = UnicodeAttribute(null=True)
    disabled = BooleanAttribute(null=True)
    is_new = BooleanAttribute(null=False, default=True)


class ChoicesSubAttribute(MapAttribute):
    """アンケートマスタTBL Attribute定義"""

    description = UnicodeAttribute(null=True)
    group = ListAttribute(of=GroupSubAttribute, null=True)
    is_new = BooleanAttribute(null=False, default=True)


class QuestionsAttribute(MapAttribute):
    """アンケートマスタTBL Attribute定義"""

    id = UnicodeAttribute(null=False)
    required = BooleanAttribute(null=False, default=False)
    description = UnicodeAttribute(null=False)
    format = UnicodeAttribute(null=False)
    summary_type = UnicodeAttribute(null=True)
    choices = ListAttribute(of=ChoicesSubAttribute, null=True)
    other_description = UnicodeAttribute(null=True)
    disabled = BooleanAttribute(null=False, default=False)
    is_new = BooleanAttribute(null=False, default=True)


class QuestionFlowAttribute(MapAttribute):
    """アンケートマスタTBL Attribute定義"""

    id = UnicodeAttribute(null=False)
    condition_id = UnicodeAttribute(null=False)
    condition_choice_ids = UnicodeSetAttribute(null=True)


class SurveyMasterModel(Model):
    """アンケートマスタTBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "SurveyMaster"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    revision = NumberAttribute(range_key=True, null=False)

    name = UnicodeAttribute(null=False)
    type = UnicodeAttribute(null=False)
    timing = UnicodeAttribute(null=False)
    init_send_day_setting = NumberAttribute(null=False, default=0)
    init_answer_limit_day_setting = NumberAttribute(null=False, default=0)
    is_disclosure = BooleanAttribute(null=False, default=False)
    questions = ListAttribute(of=QuestionsAttribute, null=False)
    question_flow = ListAttribute(of=QuestionFlowAttribute, null=False)
    is_latest = NumberAttribute(null=False, default=0)
    memo = UnicodeAttribute(null=True)

    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    is_latest_name_index = IsLatestNameIndex()
    id_is_latest_index = IdIsLatestIndex()
    type_is_latest_index = TypeIsLatestIndex()

    @staticmethod
    def get_latest_survey_masters(survey_type: str, survey_master_id: str = None):
        """最新のアンケートマスタを取得する

        Raises:
            HTTPException: 404

        Returns:
            SurveyMasterModel
        """
        if survey_type == SurveyTypeExcludingPP.QUICK.value:
            latest_survey_masters = SurveyMasterModel.id_is_latest_index.query(
                hash_key=survey_master_id,
                range_key_condition=SurveyMasterModel.is_latest
                == SurveyRevisionStatusNumber.LATEST,
            )
        else:
            latest_survey_masters = SurveyMasterModel.type_is_latest_index.query(
                hash_key=survey_type,
                range_key_condition=SurveyMasterModel.is_latest
                == SurveyRevisionStatusNumber.LATEST,
            )

        try:
            return next(latest_survey_masters)
        except StopIteration:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found survey master."
            )
