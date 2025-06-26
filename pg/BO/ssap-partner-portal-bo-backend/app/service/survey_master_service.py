import uuid
from datetime import datetime
from itertools import chain
from typing import Dict, List

from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist

from app.core.common_logging import CustomLogger
from app.models.admin import AdminModel
from app.models.survey_master import (
    ChoicesSubAttribute,
    GroupSubAttribute,
    QuestionFlowAttribute,
    QuestionsAttribute,
    SurveyMasterModel,
)
from app.resources.const import (
    SurveyRevisionStatus,
    SurveyRevisionStatusNumber,
    SurveyType,
    UserRoleType,
)
from app.schemas.survey_master import (
    CopySurveyMastersByIdResponse,
    CreateSurveyMastersRequest,
    CreateSurveyMastersResponse,
    GetSurveyMasterByIdAndRevisionResponse,
    GetSurveyMastersByIdResponse,
    GetSurveyMastersByIdSurveyMasterResponse,
    GetSurveyMastersResponse,
    PatchSurveyMasterRevisionByIdResponse,
    SurveyMaster,
    UpdateSurveyMasterDraftByIdRequest,
    UpdateSurveyMasterDraftByIdResponse,
    UpdateSurveyMasterLatestByIdRequest,
    UpdateSurveyMasterLatestByIdResponse,
)
from app.service.common_service.user_info import get_update_user_name

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
    def create_survey_masters(
        item: CreateSurveyMastersRequest, current_user: AdminModel
    ) -> CreateSurveyMastersResponse:
        """Post /survey-masters アンケートマスター登録API

        Args:
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            CreateSurveyMastersResponse: 登録後の結果
        """

        # DynamoDBテーブルへ書き込み
        create_datetime = datetime.now()
        survey_master = SurveyMasterModel(
            id=str(uuid.uuid4()),
            revision=0,  # 新規作成時のリビジョンは常に0(下書き)
            name=item.name,
            type=item.type,
            timing=item.timing,
            init_send_day_setting=item.init_send_day_setting,
            init_answer_limit_day_setting=item.init_answer_limit_day_setting,
            is_disclosure=item.is_disclosure,
            questions=SurveyMasterService.decompress_questions(item.questions),
            question_flow=SurveyMasterService.decompress_question_flows(
                item.question_flow
            ),
            is_latest=SurveyRevisionStatusNumber.LATEST,  # Create時は下書きしか存在しない状態なので下書きを最新とする
            memo=item.memo,
            create_id=current_user.id,
            create_at=create_datetime,
            update_id=current_user.id,
            update_at=create_datetime,
        )

        survey_master.save()

        return CreateSurveyMastersResponse(**survey_master.attribute_values)

    @staticmethod
    def is_visible_draft(current_user: AdminModel) -> bool:
        """アンケートマスタの下書き閲覧のアクセス制御.
            1.制限なし(アクセス可)
              ・アンケート事務局
              ・システム管理者
            2.アクセス不可
              ・営業担当者
              ・支援者責任者
              ・営業責任者
              ・稼働率調査事務局
              ・事業者責任者
        Args:
            current_user (AdminModel): ログインユーザ
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        access_ctrl_flag = False

        for role in current_user.roles:
            if role in [
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
            ]:
                return True

        return access_ctrl_flag

    @staticmethod
    def get_survey_masters(
        name: str,
        latest: bool,
        in_operation: bool,
        survey_type: SurveyType,
        current_user: AdminModel,
    ) -> GetSurveyMastersResponse:
        """Get /survey-masters アンケートひな形一覧取得API
        Returns:
            GetSurveyMastersResponse: 取得結果
        """

        range_key_condition = None

        if name:
            range_key_condition &= SurveyMasterModel.name.startswith(name)

        filter_condition = None

        if survey_type:
            filter_condition &= SurveyMasterModel.type == survey_type

        # 下書きの最新バージョンを取得しない制御
        if in_operation:
            filter_condition &= (
                SurveyMasterModel.revision != SurveyRevisionStatusNumber.DRAFT_REVISION
            )

        if latest:
            survey_master_iter = SurveyMasterModel.is_latest_name_index.query(
                hash_key=SurveyRevisionStatusNumber.LATEST,
                range_key_condition=range_key_condition,
                filter_condition=filter_condition,
                scan_index_forward=True,
            )
        else:
            # 最新のアンケートマスタを取得
            survey_master_latests = SurveyMasterModel.is_latest_name_index.query(
                hash_key=SurveyRevisionStatusNumber.LATEST,
                range_key_condition=range_key_condition,
                scan_index_forward=True,
            )

            # 最新でないアンケートマスタを取得
            survey_master_not_latests = SurveyMasterModel.is_latest_name_index.query(
                hash_key=SurveyRevisionStatusNumber.NOT_LATEST,
                range_key_condition=range_key_condition,
                scan_index_forward=True,
            )

            # 最新のアンケートマスタと最新でないアンケートマスタをマージ
            survey_master_iter = chain(survey_master_latests, survey_master_not_latests)

        survey_master_list: List[SurveyMaster] = []
        for survey_master in survey_master_iter:
            # ステータスを追加
            tmp_status: str = None
            if survey_master.is_latest == 1:  # 最新のバージョンの場合
                tmp_status = SurveyRevisionStatus.IN_OPERATION
            elif survey_master.revision == 0:  # 下書の場合
                tmp_status = SurveyRevisionStatus.DRAFT
            else:  # 下書きと最新以外のバージョンの場合
                tmp_status = SurveyRevisionStatus.ARCHIVE

            # 設問数を追加
            tmp_question_count: int = None
            tmp_question_count = len(survey_master.questions)

            survey_master_list.append(
                SurveyMaster(
                    id=survey_master.id,
                    revision=survey_master.revision,
                    name=survey_master.name,
                    type=survey_master.type,
                    timing=survey_master.timing,
                    init_send_day_setting=survey_master.init_send_day_setting,
                    init_answer_limit_day_setting=survey_master.init_answer_limit_day_setting,
                    is_disclosure=survey_master.is_disclosure,
                    is_latest=survey_master.is_latest,
                    memo=survey_master.memo,
                    status=tmp_status,
                    question_count=tmp_question_count,
                    update_at=survey_master.update_at,
                )
            )

        total = len(survey_master_list)

        return GetSurveyMastersResponse(total=total, masters=survey_master_list)

    def get_survey_masters_by_id(
        survey_master_id: str, current_user: AdminModel
    ) -> GetSurveyMastersByIdResponse:
        """Get /survey-masters/{id} アンケートマスター単一取得API

        Args:
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetSurveyMastersByIdResponse: 結果
        """

        # 指定されたアンケートマスタの全バージョンを新しい順(revisionが大きい順)で取得
        survey_master_iter = SurveyMasterModel.query(
            hash_key=survey_master_id, scan_index_forward=False
        )

        # 取得したアンケートマスタの全バージョンを配列へ格納
        survey_master_list: List[GetSurveyMastersByIdSurveyMasterResponse] = []
        for survey_master in survey_master_iter:
            # 下書き返却の制御
            if (
                survey_master.revision == SurveyRevisionStatusNumber.DRAFT_REVISION
                and not SurveyMasterService.is_visible_draft(current_user)
            ):
                continue

            # ステータスを追加
            tmp_status: str = None
            if (
                survey_master.is_latest == SurveyRevisionStatusNumber.LATEST
            ):  # 最新のバージョンの場合
                tmp_status = SurveyRevisionStatus.IN_OPERATION
            elif (
                survey_master.revision == SurveyRevisionStatusNumber.DRAFT_REVISION
            ):  # 下書の場合
                tmp_status = SurveyRevisionStatus.DRAFT
            else:  # 下書きと最新以外のバージョンの場合
                tmp_status = SurveyRevisionStatus.ARCHIVE

            # 設問数を追加
            tmp_question_count: int = 0

            for question in survey_master.questions:
                try:
                    if not question.disabled:
                        tmp_question_count += 1
                except AttributeError:  # disabledが存在しない場合の考慮
                    continue

            survey_master_list.append(
                GetSurveyMastersByIdSurveyMasterResponse(
                    id=survey_master.id,
                    revision=survey_master.revision,
                    name=survey_master.name,
                    type=survey_master.type,
                    timing=survey_master.timing,
                    init_send_day_setting=survey_master.init_send_day_setting,
                    init_answer_limit_day_setting=survey_master.init_answer_limit_day_setting,
                    is_disclosure=survey_master.is_disclosure,
                    is_latest=survey_master.is_latest,
                    memo=survey_master.memo,
                    status=tmp_status,
                    question_count=tmp_question_count,
                    update_at=survey_master.update_at,
                )
            )

        total_survey_master = len(survey_master_list)

        # バージョンが1件も存在しない場合は404を返却
        if total_survey_master == 0:
            logger.warning("GetSurveyMastersById master not found.")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        # 下書きが存在する場合は先頭へ移動
        if survey_master_list[-1].revision == 0:
            survey_master_list.insert(0, survey_master_list.pop(-1))

        return GetSurveyMastersByIdResponse(
            total=total_survey_master, masters=survey_master_list
        )

    def get_survey_master_by_id_and_revision(
        survey_master_id: str, revision: int, current_user: AdminModel
    ) -> GetSurveyMasterByIdAndRevisionResponse:
        """Get /survey-masters/{id}/{revision} アンケートマスターバージョン単一取得API

        Args:
            id (str): アンケートマスターID
            revision: アンケートマスターバージョン
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetSurveyMasterByIdAndRevisionResponse: 結果
        """

        try:
            survey_master = SurveyMasterModel.get(
                hash_key=survey_master_id, range_key=revision
            )
        except DoesNotExist:
            logger.warning("GetSurveyMasterByIdAndRevision master not found.")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        return GetSurveyMasterByIdAndRevisionResponse(
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
            memo=survey_master.memo,
            create_id=survey_master.update_id,
            create_at=survey_master.update_at,
            update_id=survey_master.update_id,
            update_user_name=get_update_user_name(survey_master.update_id),
            update_at=survey_master.update_at,
            version=survey_master.version,
        )

    def copy_survey_masters_by_id(
        origin_revision: int,
        survey_master_id: str,
        current_user: AdminModel,
    ) -> CopySurveyMastersByIdResponse:
        """Put /survey-masters/{id} アンケートマスター下書き作成API

        Args:
            origin_revision (int): コピー元バージョン
            id (str): アンケートマスタID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            CopySurveyMastersByIdResponse: 結果
        """

        # 下書きの存在チェック
        try:
            SurveyMasterModel.get(
                hash_key=survey_master_id,
                range_key=0,
                attributes_to_get=SurveyMasterModel.revision,
            )
        except DoesNotExist:
            pass
        else:
            # すでに下書きが存在する場合は400を返却
            logger.warning("CopySurveyMastersById bad request. draft is exits.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST, detail="draft is exits."
            )

        # 指定したコピー元バージョンを取得
        try:
            origin_survey_master = SurveyMasterModel.get(
                hash_key=survey_master_id, range_key=origin_revision
            )
        except DoesNotExist:
            # 指定されたバージョンが存在しない場合は404を返却
            logger.warning(
                f"CopySurveyMastersById not found. survey_master_id: {survey_master_id} origin_revision: {origin_revision}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )
        else:
            # コピー元バージョンが存在する場合はコピーして下書きを作成
            create_datetime = datetime.now()
            draft_survey_master = SurveyMasterModel(
                id=origin_survey_master.id,
                revision=SurveyRevisionStatusNumber.DRAFT_REVISION,
                name=origin_survey_master.name,
                type=origin_survey_master.type,
                timing=origin_survey_master.timing,
                init_send_day_setting=origin_survey_master.init_send_day_setting,
                init_answer_limit_day_setting=origin_survey_master.init_answer_limit_day_setting,
                is_disclosure=origin_survey_master.is_disclosure,
                questions=SurveyMasterService.decompress_questions(
                    origin_survey_master.questions
                ),
                question_flow=SurveyMasterService.decompress_question_flows(
                    origin_survey_master.question_flow
                ),
                is_latest=SurveyRevisionStatusNumber.NOT_LATEST,
                memo=origin_survey_master.memo,
                create_id=current_user.id,
                create_at=create_datetime,
                update_id=current_user.id,
                update_at=create_datetime,
            )
            draft_survey_master.save()

        return CopySurveyMastersByIdResponse(**draft_survey_master.attribute_values)

    def patch_survey_masters_by_id(
        survey_master_id: str,
        version: int,
        current_user: AdminModel,
    ) -> PatchSurveyMasterRevisionByIdResponse:
        """Patch /survey-masters/{id} 下書きバージョンを最新バージョンへ切り替えるAPI

        Args:
            id (str): アンケートマスタID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            PatchSurveyMasterRevisionByIdResponse: 結果
        """

        # 下書きの存在チェック
        try:
            draft = SurveyMasterModel.get(
                hash_key=survey_master_id,
                range_key=SurveyRevisionStatusNumber.DRAFT_REVISION,
            )
        except DoesNotExist:
            # 下書きが存在しない場合は400を返却
            logger.warning(
                "PatchSurveyMasterRevisionById bad request. draft is not exits"
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST, detail="draft is not exits."
            )

        if version != draft.version:
            logger.warning(
                f"PatchSurveyMasterRevisionById conflict. request_ver:{version} draft_ver: {draft.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 最新バージョンを取得
        try:
            current_latest = next(
                SurveyMasterModel.id_is_latest_index.query(
                    hash_key=survey_master_id,
                    scan_index_forward=False,
                    limit=1,
                )
            )
            new_latest_revision = int(current_latest.revision) + 1
        except StopIteration:
            new_latest_revision = 1

        # 設問のisNewをFalseへ一括変更
        if draft.questions:
            new_latest_questions = draft.questions

            for question in new_latest_questions:
                if question.is_new:
                    question.is_new = False
                if question.choices:
                    for choice in question.choices:
                        if choice.is_new:
                            choice.is_new = False
                        if choice.group:
                            for group in choice.group:
                                if group.is_new:
                                    group.is_new = False

        # 下書きから最新バージョンを作成 (DynamoDBのソートキーは更新できないため)
        create_datetime = datetime.now()
        new_latest = SurveyMasterModel(
            id=draft.id,
            revision=new_latest_revision,
            name=draft.name,
            type=draft.type,
            timing=draft.timing,
            init_send_day_setting=draft.init_send_day_setting,
            init_answer_limit_day_setting=draft.init_answer_limit_day_setting,
            is_disclosure=draft.is_disclosure,
            questions=new_latest_questions,
            question_flow=draft.question_flow,
            is_latest=SurveyRevisionStatusNumber.LATEST.value,
            memo=draft.memo,
            create_id=current_user.id,
            create_at=create_datetime,
            update_id=current_user.id,
            update_at=create_datetime,
        )
        new_latest.save()

        # 更新前の最新バージョンのステータスを変更
        if current_latest:
            current_latest.update(
                actions=[
                    SurveyMasterModel.is_latest.set(
                        SurveyRevisionStatusNumber.NOT_LATEST.value
                    )
                ]
            )

        # 下書きを削除
        delete_draft = SurveyMasterModel.get(
            hash_key=survey_master_id,
            range_key=SurveyRevisionStatusNumber.DRAFT_REVISION,
        )
        delete_draft.delete()

        return PatchSurveyMasterRevisionByIdResponse(
            status=SurveyRevisionStatus.IN_OPERATION,
            revision=new_latest.revision,
            memo=new_latest.memo,
            question_count=len(new_latest.questions),
            update_at=new_latest.update_at,
        )

    def update_survey_master_latest_by_id(
        item: UpdateSurveyMasterLatestByIdRequest,
        version: int,
        survey_master_id: str,
        current_user: AdminModel,
    ) -> UpdateSurveyMasterLatestByIdResponse:
        """Put /survey-masters/{id}/latest アンケートマスター詳細情報更新API

        Args:
            id (str): アンケートマスタID
            version (int): ロックキー（楽観ロック制御)
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            UpdateSurveyMasterLatestByIdResponse: 更新結果
        """

        # 最新バージョンを取得
        try:
            latest_iter = SurveyMasterModel.id_is_latest_index.query(
                hash_key=survey_master_id,
                scan_index_forward=False,
                limit=1,
            )
            latest = next(latest_iter)
        except (DoesNotExist, StopIteration):
            # バージョンが存在しない場合は404を返却
            logger.warning(
                "UpdateSurveyMasterLatestById not found. latest revision is not exits."
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="latest revision is not exits.",
            )

        # バージョンの楽観ロック制御
        if version != latest.version:
            logger.warning(
                f"UpdateSurveyMasterLatestById conflict. request_ver:{version} latest_ver: {latest.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        latest.update(
            actions=[
                SurveyMasterModel.name.set(item.name),
                SurveyMasterModel.type.set(item.type),
                SurveyMasterModel.timing.set(item.timing),
                SurveyMasterModel.init_send_day_setting.set(item.init_send_day_setting),
                SurveyMasterModel.init_answer_limit_day_setting.set(
                    item.init_answer_limit_day_setting
                ),
                SurveyMasterModel.is_disclosure.set(item.is_disclosure),
                SurveyMasterModel.memo.set(item.memo),
                SurveyMasterModel.update_id.set(current_user.id),
                SurveyMasterModel.update_at.set(datetime.now()),
            ]
        )

        return UpdateSurveyMasterLatestByIdResponse(**latest.attribute_values)

    def update_survey_master_draft_by_id(
        item: UpdateSurveyMasterDraftByIdRequest,
        version: int,
        survey_master_id: str,
        current_user: AdminModel,
    ) -> UpdateSurveyMasterDraftByIdResponse:
        """Put /survey-masters/{id}/draft アンケートマスター下書き更新API

        Args:
            id (str): アンケートマスタID
            version (int): ロックキー（楽観ロック制御)
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            UpdateSurveyMasterDraftByIdResponse: 更新結果
        """

        # 下書きを取得
        try:
            draft = SurveyMasterModel.get(
                hash_key=survey_master_id,
                range_key=SurveyRevisionStatusNumber.DRAFT_REVISION,
            )
        except DoesNotExist:
            logger.warning(
                "UpdateSurveyMasterDraftById not found. latest revision is not exits."
            )
            # 下書きが存在しない場合は404を返却
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="latest revision is not exits.",
            )

        # バージョンの楽観ロック制御
        if version != draft.version:
            logger.warning(
                f"UpdateSurveyMasterDraftById conflict. request_ver:{version} draft_ver: {draft.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        draft.update(
            actions=[
                SurveyMasterModel.name.set(item.name),
                SurveyMasterModel.timing.set(item.timing),
                SurveyMasterModel.init_send_day_setting.set(item.init_send_day_setting),
                SurveyMasterModel.init_answer_limit_day_setting.set(
                    item.init_answer_limit_day_setting
                ),
                SurveyMasterModel.is_disclosure.set(item.is_disclosure),
                SurveyMasterModel.questions.set(
                    SurveyMasterService.decompress_questions(item.questions)
                ),
                SurveyMasterModel.question_flow.set(
                    SurveyMasterService.decompress_question_flows(item.question_flow)
                ),
                SurveyMasterModel.memo.set(item.memo),
                SurveyMasterModel.update_id.set(current_user.id),
                SurveyMasterModel.update_at.set(datetime.now()),
            ]
        )

        return UpdateSurveyMasterDraftByIdResponse(
            id=draft.id,
            revision=draft.revision,
            name=draft.name,
            type=draft.type,
            timing=draft.timing,
            init_send_day_setting=draft.init_send_day_setting,
            init_answer_limit_day_setting=draft.init_answer_limit_day_setting,
            is_disclosure=draft.is_disclosure,
            questions=SurveyMasterService.decompress_questions(draft.questions),
            question_flow=SurveyMasterService.decompress_question_flows(
                draft.question_flow
            ),
            is_latest=draft.is_latest,
            memo=draft.memo,
            update_id=draft.update_id,
            update_at=draft.update_at,
            version=draft.version,
        )
