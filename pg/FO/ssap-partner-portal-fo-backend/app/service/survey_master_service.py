from typing import Dict, List

from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist

from app.auth.auth import AuthUser
from app.core.common_logging import CustomLogger
from app.models.survey_master import (
    ChoicesSubAttribute,
    GroupSubAttribute,
    QuestionFlowAttribute,
    QuestionsAttribute,
    SurveyMasterModel,
)
from app.models.project_survey import ProjectSurveyModel
from app.resources.const import (
    IsLatest,
    SurveyRevisionStatusNumber,
    SurveyType,
    DataType,
    SurveyQuestionsSummaryType,
)
from app.schemas.survey_master import (
    CheckAndGetSurveyMastersAnonymousByIdAndRevisionRequest,
    GetSurveyMastersByIdAndRevisionResponse,
    GetSurveyMastersResponse,
    SurveyMasters,
    GetSurveyMasterOfSatisfactionByIdAndRevisionRequest,
    GetSurveyMasterOfSatisfactionByIdAndRevisionResponse,
)
from app.schemas.survey import (
    PostCheckSurveyByIdPasswordRequest,
    PostCheckSurveyByIdRequest,
)
from app.service.survey_service import SurveyService
from app.utils.encryption import decode_jwt

logger = CustomLogger.get_logger()


class SurveyMasterService:
    @staticmethod
    def decompress_questions(
        questions: QuestionsAttribute,
    ):
        """リクエストボディのquestions内でネストされている項目を取り出して変数化"""
        questions_item: List[str] = []
        for question in questions:
            choices_item: List[str] = []
            if question.choices:
                for choice in question.choices:
                    group_item: List[str] = []
                    if choice.group:
                        for group in choice.group:
                            tmp_group: Dict[str, str] = {}
                            tmp_group = GroupSubAttribute(
                                id=group.id,
                                title=group.title,
                                disabled=group.disabled,
                                is_new=group.is_new,
                            )
                            group_item.append(tmp_group.attribute_values)
                    tmp_choices: Dict[str, str] = {}
                    tmp_choices = ChoicesSubAttribute(
                        description=choice.description,
                        group=group_item,
                        is_new=choice.is_new,
                    )
                    choices_item.append(tmp_choices.attribute_values)
            tmp_question: Dict[str, str] = {}
            tmp_question = QuestionsAttribute(
                id=question.id,
                required=question.required,
                description=question.description,
                format=question.format,
                summary_type=question.summary_type,
                choices=choices_item,
                other_description=question.other_description,
                disabled=question.disabled,
                is_new=question.is_new,
            )
            questions_item.append(tmp_question.attribute_values)

        return questions_item

    @staticmethod
    def decompress_question_flows(
        question_flows: QuestionFlowAttribute,
    ):
        """リクエストボディのquestion_flow内でネストされている項目を取り出して変数化"""
        question_flows_item: List[str] = []
        for question_flow in question_flows:
            tmp_question_flow: Dict[str, str] = {}
            tmp_question_flow = QuestionFlowAttribute(
                id=question_flow.id,
                condition_id=question_flow.condition_id,
                condition_choice_ids=question_flow.condition_choice_ids,
            )
            question_flows_item.append(tmp_question_flow.attribute_values)

        return question_flows_item

    @staticmethod
    def get_survey_masters(
        survey_type: SurveyType, in_operation: bool, current_user: AuthUser
    ) -> GetSurveyMastersResponse:
        """Get /survey-masters アンケートひな形一覧取得API

        Returns:
            GetSurveyMastersResponse: 取得結果
        """

        filter_condition = None

        if survey_type:
            filter_condition &= SurveyMasterModel.type == survey_type

        # 下書きの最新バージョンを取得しない制御
        if in_operation:
            filter_condition &= (
                SurveyMasterModel.revision != SurveyRevisionStatusNumber.DRAFT_REVISION
            )

        master_iter = SurveyMasterModel.is_latest_name_index.query(
            hash_key=IsLatest.TRUE,
            filter_condition=filter_condition,
        )

        masters_list: List[SurveyMasters] = []
        for master in master_iter:
            masters_list.append(
                SurveyMasters(
                    id=master.id,
                    revision=master.revision,
                    name=master.name,
                    type=master.type,
                    timing=master.timing,
                    init_send_day_setting=master.init_send_day_setting,
                    init_answer_limit_day_setting=master.init_answer_limit_day_setting,
                    is_disclosure=master.is_disclosure,
                    is_latest=master.is_latest,
                )
            )
        total = len(masters_list)

        return GetSurveyMastersResponse(total=total, masters=masters_list)

    @staticmethod
    def get_survey_masters_by_id_and_revision(
        survey_master_id: str, revision: int, current_user: AuthUser
    ) -> GetSurveyMastersByIdAndRevisionResponse:
        """Get /survey-masters/{id}/{revision} アンケートマスター単一取得API

        Args:
            survey_master_id(str): アンケートマスタID
            revision(int): アンケートマスターバージョン番号
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetSurveyMastersByIdAndRevisionResponse: 結果
        """

        try:
            survey_master = SurveyMasterModel.get(
                hash_key=survey_master_id, range_key=revision
            )
        except DoesNotExist:
            logger.warning(
                f"GetSurveyMastersByIdAndRevision not found. survey_master_id: {survey_master_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        return GetSurveyMastersByIdAndRevisionResponse(
            id=survey_master.id,
            revision=survey_master.revision,
            name=survey_master.name,
            type=survey_master.type,
            timing=survey_master.timing,
            init_send_day_setting=survey_master.init_send_day_setting,
            init_answer_limit_day_setting=survey_master.init_answer_limit_day_setting,
            is_disclosure=survey_master.is_disclosure,
            questions=SurveyMasterService.decompress_questions(survey_master.questions),
            question_flow=SurveyMasterService.decompress_question_flows(
                survey_master.question_flow
            ),
            is_latest=survey_master.is_latest,
            update_id=survey_master.update_id,
            update_at=survey_master.update_at,
            version=survey_master.version,
        )

    @staticmethod
    def check_and_get_survey_masters_anonymous_by_id_and_revision(
        item: CheckAndGetSurveyMastersAnonymousByIdAndRevisionRequest,
        survey_master_id: str,
        revision: int,
    ) -> GetSurveyMastersByIdAndRevisionResponse:
        """Get /survey-masters/{id}/{revision} 匿名アンケート用。アンケートマスター単一取得API

        Args:
            item (CheckAndGetSurveyMastersAnonymousByIdAndRevisionRequest): 認証情報
            survey_master_id(str): アンケートマスタID
            revision(int): アンケートマスターバージョン番号

        Returns:
            GetSurveyMastersByIdAndRevisionResponse: 結果
        """

        # 匿名アンケート認証
        SurveyService.post_check_survey_by_id_password(
            PostCheckSurveyByIdPasswordRequest(token=item.token, password=item.password)
        )

        try:
            survey_master = SurveyMasterModel.get(
                hash_key=survey_master_id, range_key=revision
            )
        except DoesNotExist:
            logger.warning(
                f"GetSurveyMastersByIdAndRevision not found. survey_master_id: {survey_master_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        return GetSurveyMastersByIdAndRevisionResponse(
            id=survey_master.id,
            revision=survey_master.revision,
            name=survey_master.name,
            type=survey_master.type,
            timing=survey_master.timing,
            init_send_day_setting=survey_master.init_send_day_setting,
            init_answer_limit_day_setting=survey_master.init_answer_limit_day_setting,
            is_disclosure=survey_master.is_disclosure,
            questions=SurveyMasterService.decompress_questions(survey_master.questions),
            question_flow=SurveyMasterService.decompress_question_flows(
                survey_master.question_flow
            ),
            is_latest=survey_master.is_latest,
            update_id=survey_master.update_id,
            update_at=survey_master.update_at,
            version=survey_master.version,
        )

    @staticmethod
    def get_survey_master_satisfaction_by_id_and_revision(
        item: GetSurveyMasterOfSatisfactionByIdAndRevisionRequest,
        survey_master_id: str,
        revision: int,
    ) -> GetSurveyMasterOfSatisfactionByIdAndRevisionResponse:
        """Get /survey-masters/{id}/{revision} 満足度評価のみ回答アンケート用。アンケートマスター単一取得API

        Args:
            item (GetSurveyMasterOfSatisfactionByIdAndRevisionRequest): 認証情報
            survey_master_id(str): アンケートマスタID
            revision(int): アンケートマスターバージョン番号

        Returns:
            GetSurveyMasterOfSatisfactionByIdAndRevisionResponse: 結果
        """

        # 満足度評価のみ回答アンケート認証
        SurveyService.post_check_survey_by_id(
            PostCheckSurveyByIdRequest(token=item.token)
        )

        try:
            survey_master = SurveyMasterModel.get(
                hash_key=survey_master_id, range_key=revision
            )
        except DoesNotExist:
            logger.warning(
                f"GetSurveyMasterOfSatisfactionByIdAndRevison not found. survey_master_id: {survey_master_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        survey_id: str = decode_jwt(item.token)["survey_id"]

        try:
            project_survey = ProjectSurveyModel.get(
                hash_key=survey_id, range_key=DataType.SURVEY
            )
        except DoesNotExist:
            logger.warning(
                f"GetSurveyMasterOfSatisfactionByIdAndRevison not found. survey_id: {survey_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        questions = []
        question_flow = []
        for question in survey_master.questions:
            # 満足度評価の設問のみ取得
            if question.summary_type == SurveyQuestionsSummaryType.SATISFACTION:
                questions.append(question)
                # 満足度評価の設問フローのみ取得
                for item in survey_master.question_flow:
                    if question.id == item.id:
                        question_flow.append(item)

        return GetSurveyMasterOfSatisfactionByIdAndRevisionResponse(
            id=survey_master.id,
            project_name=project_survey.project_name,
            customer_name=project_survey.customer_name,
            questions=SurveyMasterService.decompress_questions(questions),
            question_flow=SurveyMasterService.decompress_question_flows(question_flow),
            version=survey_master.version,
        )
