import base64
import hashlib
import botocore.exceptions
import copy
import json
import uuid
from datetime import datetime
from typing import List, Union, Optional

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.solver import (
    FacePhotoAttribute,
    ScreeningAttribute,
    SolverModel,
    ResumeAttribute,
)
from app.models.solver_application import SolverApplicationModel
from app.models.solver_corporation import SolverCorporationModel
from app.models.user import UserModel
from app.resources.const import (
    DEFAULT_BIRTHDAY,
    DataType,
    GetSolversSortType,
    MailType,
    SalesforceDataSyncMailType,
    UserRoleType,
)
from app.schemas.base import OKResponse
from app.schemas.solver import (
    CreateSolverRequest,
    DeleteSolverByIdQuery,
    GetSolverByIdResponse,
    GetSolversQuery,
    GetSolversResponse,
    PatchSolverStatusByIdQuery,
    SolverInfoForCreateSolver,
    SolverInfoForGetSolvers,
    UpdateSolverUtilizationRateRequest,
    SolverApplications,
    UpdateSolverByIdRequest,
    FacePhoto,
    Resume,
    Screening,
    UpdateSolverUtilizationRateResponse,
)
from app.service.common_service.pagination import EmptyPage, Paginator
from app.service.common_service.user_info import get_update_user_name
from app.service.common_service.request_processor import url_decode_data
from app.service.master_service import MasterService
from app.utils.aws.s3 import S3Helper
from app.utils.aws.ses import SesHelper
from app.utils.date import get_datetime_now
from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.update import Action

logger = CustomLogger.get_logger()


class SolverService:
    @staticmethod
    def create_solver_application(
        item: Union[SolverInfoForCreateSolver, UpdateSolverByIdRequest],
        current_user: UserModel
    ) -> str:
        """ソルバー案件を新規登録
        Args:
            item（Union[SolverInfoForCreateSolver, UpdateSolverByIdRequest]）: 登録・更新内容
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            str: ソルバー案件ID（PK）
        """
        now_datetime = get_datetime_now()

        filter_condition = SolverApplicationModel.solver_application_id == item.solver_application_id
        solver_application_list: List[SolverApplicationModel] = list(SolverApplicationModel.data_type_index.query(
            hash_key=DataType.SOLVER_APPLICATION, filter_condition=filter_condition
        ))
        if len(solver_application_list) == 0:
            solver_application = SolverApplicationModel(
                id=str(uuid.uuid4()),
                data_type=DataType.SOLVER_APPLICATION,
                solver_application_id=item.solver_application_id,
                name=item.solver_application_name,
                create_id=current_user.id,
                create_at=now_datetime,
                update_id=current_user.id,
                update_at=now_datetime,
            )
            solver_application.save()

            return solver_application.id

        return solver_application_list[0].id

    @staticmethod
    def get_formatted_create_solver_email_data(
        item: SolverInfoForCreateSolver,
        solver_id: str,
        current_user: UserModel,
        now_datetime: datetime,
    ) -> dict:
        """ソルバーの新規登録項目をメール用にデータ整形し取得
        Args:
            item (CreateSolverRequest): 登録内容
            solver_id (str): ソルバーID
            current_user (Behavior, optional): 認証済みのユーザー
            now_datetime (datetime): 現在時刻
        Returns:
            create_solver_email_data（dict): 登録対象の項目
        """
        create_solver_email_data: dict = {}

        # PPID
        create_solver_email_data.update({"PPId__c": solver_id})

        # 案件ID
        create_solver_email_data.update({"ProjectID__c": [item.solver_application_id] if item.solver_application_id else []})

        # 所属法人ID
        create_solver_email_data.update({"CorporateID__c": item.corporate_id})

        # 個人ソルバー名
        create_solver_email_data.update({"Name": item.name})

        # 個人ソルバーふりがな
        create_solver_email_data.update({"NameKana__c": item.name_kana if item.name_kana else ""})

        # 性別
        sex: str = ""
        if item.sex == "man":
            sex = "男性"
        elif item.sex == "woman":
            sex = "女性"
        elif item.sex == "not_set":
            sex = "未設定"
        create_solver_email_data.update({"Sex__c": sex})

        # 生年月日
        create_solver_email_data.update({"Birthday__c": None if item.birth_day == DEFAULT_BIRTHDAY else item.birth_day})

        # 連絡先メールアドレス
        create_solver_email_data.update({"Email__c": item.email if item.email else ""})

        # 電話番号
        create_solver_email_data.update({"Phone__c": item.phone if item.phone else ""})

        # 英語レベル
        english_level: str = ""
        if item.english_level == "reading_and_writing":
            english_level = "読み書き程度"
        elif item.english_level == "ordinary_conversation":
            english_level = "日常会話"
        elif item.english_level == "business":
            english_level = "ビジネス"
        elif item.english_level == "native":
            english_level = "ネイティブ"
        elif item.english_level == "unknown":
            english_level = "不明"
        create_solver_email_data.update({"EnglishLevel__c": english_level})

        # 学歴
        create_solver_email_data.update({"AcademicBackground__c": item.academic_background if item.academic_background else ""})

        # 職歴
        create_solver_email_data.update({"WorkHistory__c": item.work_history})

        # 専門テーマ
        create_solver_email_data.update({"SpecializedThemes__c": item.specialized_themes})

        # コンサルティングファーム経験有無
        create_solver_email_data.update({"ConsultingFirmExperience__c": item.is_consulting_firm})

        # 役職
        create_solver_email_data.update({"Title__c": item.title if item.title else ""})

        # 主な実績
        create_solver_email_data.update({"MainAchievements__c": item.main_achievements})

        # 人月単位（上限）
        create_solver_email_data.update({"PricePerPersonMonth__c": item.price_per_person_month if item.price_per_person_month is not None else ""})

        # 人月単位（下限）
        create_solver_email_data.update({"PricePerPersonMonth_Lower__c": item.price_per_person_month_lower if item.price_per_person_month_lower is not None else ""})

        # 時間単価（上限）
        create_solver_email_data.update({"HourlyRate__c": item.hourly_rate if item.hourly_rate is not None else ""})

        # 時間単価（下限）
        create_solver_email_data.update({"HourlyRate_Lower__c": item.hourly_rate_lower if item.hourly_rate_lower is not None else ""})

        # 稼働状況
        operating_status: str = ""
        if item.operating_status == "not_working":
            operating_status = "未稼働"
        elif item.operating_status == "working":
            operating_status = "稼働中"
        elif item.operating_status == "inactive":
            operating_status = "休止中"
        create_solver_email_data.update({"OperatingStatus__c": operating_status})

        # 提供稼働率（今月）
        create_solver_email_data.update({"ProvidedOperatingRate__c": item.provided_operating_rate if item.provided_operating_rate is not None else ""})

        # 提供稼働率（来月）
        create_solver_email_data.update({"ProvidedOperatingRate_next__c": item.provided_operating_rate_next if item.provided_operating_rate_next is not None else ""})

        # 再来月以降の稼働見込み
        create_solver_email_data.update({"OperationProspects_MonthAfterNext__c": item.operation_prospects_month_after_next if item.operation_prospects_month_after_next else ""})

        # 東証33業種経験/対応可能領域
        # 選択値が複数ある場合、セミコロンで区切る
        tsi_areas_name: List[str] = MasterService.get_tsi_areas_name(item.tsi_areas)
        create_solver_email_data.update({"TSIAreas__c": ";".join(tsi_areas_name) if tsi_areas_name else ""})

        # 課題マップ50
        # 選択値が複数ある場合、セミコロンで区切る
        issue_map50_name: List[str] = MasterService.get_issue_map50_name(item.issue_map50)
        create_solver_email_data.update({"IssueMap50__c": ";".join(issue_map50_name) if issue_map50_name else ""})

        # スクリーニングおよびクライテリア項目はソルバー候補申請画面にないため、個人ソルバー登録申請時のみ連携
        if item.mode == "create_solver" or item.mode == "update_solver":
            # スクリーニング項目1（評価）
            create_solver_email_data.update({"ScreeningEvaluation1__c": "〇" if item.screening_1.evaluation else ""})
            # スクリーニング項目1（エビデンス）
            create_solver_email_data.update({"ScreeningEvidence1__c": item.screening_1.evidence if item.screening_1.evidence else ""})

            # スクリーニング項目2（評価）
            create_solver_email_data.update({"ScreeningEvaluation2__c": "〇" if item.screening_2.evaluation else ""})
            # スクリーニング項目2（エビデンス）
            create_solver_email_data.update({"ScreeningEvidence2__c": item.screening_2.evidence if item.screening_2.evidence else ""})

            # スクリーニング項目3（評価）
            create_solver_email_data.update({"ScreeningEvaluation3__c": "〇" if item.screening_3.evaluation else ""})
            # スクリーニング項目3（エビデンス）
            create_solver_email_data.update({"ScreeningEvidence3__c": item.screening_3.evidence if item.screening_3.evidence else ""})

            # スクリーニング項目4（評価）
            create_solver_email_data.update({"ScreeningEvaluation4__c": "〇" if item.screening_4.evaluation else ""})
            # スクリーニング項目4（エビデンス）
            create_solver_email_data.update({"ScreeningEvidence4__c": item.screening_4.evidence if item.screening_4.evidence else ""})

            # スクリーニング項目5（評価）
            create_solver_email_data.update({"ScreeningEvaluation5__c": "〇" if item.screening_5.evaluation else ""})
            # スクリーニング項目5（エビデンス）
            create_solver_email_data.update({"ScreeningEvidence5__c": item.screening_5.evidence if item.screening_5.evidence else ""})

            # スクリーニング項目6（評価）
            create_solver_email_data.update({"ScreeningEvaluation6__c": "〇" if item.screening_6.evaluation else ""})
            # スクリーニング項目6（エビデンス）
            create_solver_email_data.update({"ScreeningEvidence6__c": item.screening_6.evidence if item.screening_6.evidence else ""})

            # スクリーニング項目7（評価）
            create_solver_email_data.update({"ScreeningEvaluation7__c": "〇" if item.screening_7.evaluation else ""})
            # スクリーニング項目7（エビデンス）
            create_solver_email_data.update({"ScreeningEvidence7__c": item.screening_7.evidence if item.screening_7.evidence else ""})

            # スクリーニング項目8（評価）
            create_solver_email_data.update({"ScreeningEvaluation8__c": "〇" if item.screening_8.evaluation else ""})
            # スクリーニング項目8（エビデンス）
            create_solver_email_data.update({"ScreeningEvidence8__c": item.screening_8.evidence if item.screening_8.evidence else ""})

            # クライテリア項目1（エビデンス）
            create_solver_email_data.update({"CriteriaEvidence1__c": item.criteria_1 if item.criteria_1 else ""})

            # クライテリア項目2（エビデンス）
            create_solver_email_data.update({"CriteriaEvidence2__c": item.criteria_2 if item.criteria_2 else ""})

            # クライテリア項目3（エビデンス）
            create_solver_email_data.update({"CriteriaEvidence3__c": item.criteria_3 if item.criteria_3 else ""})

            # クライテリア項目4（エビデンス）
            create_solver_email_data.update({"CriteriaEvidence4__c": item.criteria_4 if item.criteria_4 else ""})

            # クライテリア項目5（エビデンス）
            create_solver_email_data.update({"CriteriaEvidence5__c": item.criteria_5 if item.criteria_5 else ""})

            # クライテリア項目6（エビデンス）
            create_solver_email_data.update({"CriteriaEvidence6__c": item.criteria_6 if item.criteria_6 else ""})

            # クライテリア項目7（エビデンス）
            create_solver_email_data.update({"CriteriaEvidence7__c": item.criteria_7 if item.criteria_7 else ""})

            # クライテリア項目8（エビデンス）
            create_solver_email_data.update({"CriteriaEvidence8__c": item.criteria_8 if item.criteria_8 else ""})

        # 備考
        create_solver_email_data.update({"Notes__c": item.notes if item.notes else ""})

        # 申請区分
        if item.mode == "create_candidate":
            create_solver_email_data.update({"ApplicationCategory__c": "候補"})
        elif item.mode == "create_solver" or item.mode == "update_solver":
            create_solver_email_data.update({"ApplicationCategory__c": "登録"})

        # 登録区分
        create_solver_email_data.update({"RegistrationCategory__c": "新規"})

        # ブール不要フラグ
        create_solver_email_data.update({"NoPool": False})

        # 削除フラグ
        create_solver_email_data.update({"IsDelete__c": False})

        # 作成日時
        create_solver_email_data.update({"PartnerPortalCreatedDate__c": now_datetime.strftime("%Y/%m/%d %H:%M")})

        # 作成者
        create_solver_email_data.update({"PartnerPortalCreatedBy__c": current_user.name})

        # 更新日時
        create_solver_email_data.update({"PartnerPortalModifiedDate__c": now_datetime.strftime("%Y/%m/%d %H:%M")})

        # 更新者
        create_solver_email_data.update({"PartnerPortalModifiedBy__c": current_user.name})

        return create_solver_email_data

    @staticmethod
    def get_formatted_update_solver_email_data(
        solver_id: str,
        item: Union[SolverInfoForCreateSolver, UpdateSolverByIdRequest],
        original_solver: SolverModel,
        current_user: UserModel,
    ) -> dict:
        """ソルバーの更新対象の項目をメール用にデータ整形し取得
        Args:
            solver_id (str): 個人ソルバーID
            item (Union[SolverInfoForCreateSolver, UpdateSolverByIdRequest]): ソルバー更新リクエストデータ
            original_solver (SolverModel): ソルバー情報
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            solver_email_data（dict): 更新対象の項目
        """
        update_solver_email_data: dict = {}

        # PPID（常に連携）
        update_solver_email_data.update({"PPId__c": solver_id})

        # 法人ソルバーPPID（常に連携）
        update_solver_email_data.update({"CorporateID__c": item.corporate_id})

        # 案件ID（既存のソルバー候補もしくは既存の個人ソルバーを応募した際に連携）
        if item.mode == "create_candidate":
            solver_application_id_list: List[str] = []

            if original_solver.solver_application_ids:
                solver_application_id_list = [
                    item.solver_application_id for item in
                    SolverService.get_solver_application_info(list(original_solver.solver_application_ids))
                ]

            if (
                original_solver.solver_application_ids is None
                or (solver_application_id_list and item.solver_application_id not in solver_application_id_list)
            ):
                solver_application_id_list.append(item.solver_application_id)
                update_solver_email_data.update({"ProjectID__c": solver_application_id_list})
        elif (
            isinstance(item, UpdateSolverByIdRequest)
            and item.delete_solver_application_ids and len(item.delete_solver_application_ids) > 0
        ):
            saved_solver_application_ids = [
                item.solver_application_id for item in
                SolverService.get_solver_application_info(list(original_solver.solver_application_ids))
            ]
            solver_application_id_list = (
                list(set(saved_solver_application_ids) - set(item.delete_solver_application_ids))
            )
            update_solver_email_data.update({"ProjectID__c": solver_application_id_list})

        # 個人ソルバー名
        if item.name != original_solver.name:
            update_solver_email_data.update({"Name": item.name})

        # 個人ソルバーかな
        if item.name_kana != original_solver.name_kana:
            update_solver_email_data.update({"NameKana__c": item.name_kana})

        # 役職
        if item.title != original_solver.title:
            update_solver_email_data.update({"Title__c": item.title})

        # 連絡先メールアドレス
        if item.email != original_solver.email:
            update_solver_email_data.update({"Email__c": item.email})

        # 電話番号
        if item.phone != original_solver.phone:
            update_solver_email_data.update({"Phone__c": item.phone})

        # 課題マップ50
        # 選択値が複数ある場合、セミコロンで区切る
        issue_map50_name: List[str] = MasterService.get_issue_map50_name(item.issue_map50)
        update_solver_email_data.update({"IssueMap50__c": ";".join(issue_map50_name) if issue_map50_name else ""})

        # 性別
        if item.sex == "not_set" and original_solver.sex != item.sex:
            update_solver_email_data.update({"Sex__c": "未設定"})
        elif item.sex == "man" and original_solver.sex != item.sex:
            update_solver_email_data.update({"Sex__c": "男性"})
        elif item.sex == "woman" and original_solver.sex != item.sex:
            update_solver_email_data.update({"Sex__c": "女性"})

        # 生年月日
        if item.birth_day != original_solver.birth_day:
            update_solver_email_data.update({"Birthday__c": None if item.birth_day == DEFAULT_BIRTHDAY else item.birth_day})

        # 稼働状況
        if (
            item.operating_status == "not_working"
            and original_solver.operating_status != item.operating_status
        ):
            update_solver_email_data.update({"OperatingStatus__c": "未稼働"})
        elif (
            item.operating_status == "working"
            and original_solver.operating_status != item.operating_status
        ):
            update_solver_email_data.update({"OperatingStatus__c": "稼働中"})
        elif (
            item.operating_status == "inactive"
            and original_solver.operating_status != item.operating_status
        ):
            update_solver_email_data.update({"OperatingStatus__c": "休止中"})

        # 削除された個人ソルバー画像および添付資料がある場合、削除ファイル名を記録
        # 削除ファイルが複数ある場合、セミコロンで区切る
        deleted_face_photo: List = []
        # 法人ソルバー画像
        if (
            original_solver.face_photo and original_solver.face_photo.path is not None
        ) and (
            not item.face_photo
            or original_solver.face_photo.path != item.face_photo.path
        ):
            deleted_face_photo.append(original_solver.face_photo["file_name"])
        # 添付資料
        if original_solver.resume:
            if not item.resume:
                for model_resume in original_solver.resume:
                    deleted_face_photo.append(model_resume["file_name"])
            else:
                for model_resume in original_solver.resume:
                    # DBと一致する添付資料があるかを判定するフラグ
                    is_existed_resume: bool = False
                    for request_resume in item.resume:
                        if model_resume.path == request_resume.path:
                            is_existed_resume = True
                            break
                    if not is_existed_resume:
                        deleted_face_photo.append(model_resume["file_name"])
        if deleted_face_photo:
            update_solver_email_data.update(
                {"DeleteFileNames__c": ";delimiter".join(deleted_face_photo)}
            )

        # 学歴
        if item.academic_background != original_solver.academic_background:
            update_solver_email_data.update(
                {"AcademicBackground__c": item.academic_background}
            )

        # 職歴
        if item.work_history != original_solver.work_history:
            update_solver_email_data.update({"WorkHistory__c": item.work_history})

        # 専門テーマ
        if item.specialized_themes != original_solver.specialized_themes:
            update_solver_email_data.update({"SpecializedThemes__c": item.specialized_themes})

        # コンサルティングファーム経験有無
        if item.is_consulting_firm != original_solver.is_consulting_firm:
            update_solver_email_data.update({"ConsultingFirmExperience__c": item.is_consulting_firm})

        # 主な実績
        if item.main_achievements != original_solver.main_achievements:
            update_solver_email_data.update({"MainAchievements__c": item.main_achievements})

        # ソルバー候補登録/更新または個人ソルバー登録申請の場合は以下もメールに追加
        if item.mode == "create_candidate" or item.mode == "update_candidate" or item.mode == "register_solver":
            # 提供稼働率（今月）
            if item.provided_operating_rate != original_solver.provided_operating_rate:
                update_solver_email_data.update({"ProvidedOperatingRate__c": item.provided_operating_rate})

            # 提供稼働率（来月）
            if item.provided_operating_rate_next != original_solver.provided_operating_rate_next:
                update_solver_email_data.update({"ProvidedOperatingRate_next__c": item.provided_operating_rate_next})

            # 再来月以降の稼働見込み
            if item.operation_prospects_month_after_next != original_solver.operation_prospects_month_after_next:
                update_solver_email_data.update({"OperationProspects_MonthAfterNext__c": item.operation_prospects_month_after_next})

            # 人月単価（上限）
            if item.price_per_person_month != original_solver.price_per_person_month:
                update_solver_email_data.update({"PricePerPersonMonth__c": item.price_per_person_month})

            # 人月単価（下限）
            if item.price_per_person_month_lower != original_solver.price_per_person_month_lower:
                update_solver_email_data.update({"PricePerPersonMonth_Lower__c": item.price_per_person_month_lower})

            # 時間単価（上限）
            if item.hourly_rate != original_solver.hourly_rate:
                update_solver_email_data.update({"HourlyRate__c": item.hourly_rate})

            # 時間単価（下限）
            if item.hourly_rate_lower != original_solver.hourly_rate_lower:
                update_solver_email_data.update({"HourlyRate_Lower__c": item.hourly_rate_lower})

        # 英語レベル
        if (
            item.english_level == "reading_and_writing"
            and original_solver.english_level != item.english_level
        ):
            update_solver_email_data.update({"EnglishLevel__c": "読み書き程度"})
        elif (
            item.english_level == "ordinary_conversation"
            and original_solver.english_level != item.english_level
        ):
            update_solver_email_data.update({"EnglishLevel__c": "日常会話"})
        elif (
            item.english_level == "business"
            and original_solver.english_level != item.english_level
        ):
            update_solver_email_data.update({"EnglishLevel__c": "ビジネス"})
        elif (
            item.english_level == "native"
            and original_solver.english_level != item.english_level
        ):
            update_solver_email_data.update({"EnglishLevel__c": "ネイティブ"})
        elif (
            item.english_level == "unknown"
            and original_solver.english_level != item.english_level
        ):
            update_solver_email_data.update({"EnglishLevel__c": "不明"})

        # 東証33業種経験/対応可能領域
        # 選択値が複数ある場合、セミコロンで区切る
        tsi_areas_name: List[str] = MasterService.get_tsi_areas_name(item.tsi_areas)
        update_solver_email_data.update({"TSIAreas__c": ";".join(tsi_areas_name) if tsi_areas_name else ""})

        # 申請区分・登録区分
        # 既存のソルバー候補登録・ソルバー候補更新（個人ソルバー登録申請前）
        if (
            (item.mode == "create_candidate" and not original_solver.is_solver)
            or (item.mode == "update_candidate" and not original_solver.is_solver and original_solver.registration_status != "saved")
            or (item.mode == "register_solver" and item.registration_status == "temporary_saving")
        ):
            update_solver_email_data.update(
                {
                    "ApplicationCategory__c": "候補",
                    "RegistrationCategory__c": "更新",
                }
            )
        # 個人ソルバー登録申請
        elif item.mode == "register_solver" and item.registration_status == "saved":
            update_solver_email_data.update(
                {
                    "ApplicationCategory__c": "登録",
                    "RegistrationCategory__c": "新規",
                }
            )
        # 既存の個人ソルバー候補登録・個人ソルバー更新・ソルバー候補更新（個人ソルバー登録申請済み）
        elif (
            (item.mode == "create_candidate" and original_solver.is_solver)
            or item.mode == "update_solver"
            or (item.mode == "update_candidate" and not original_solver.is_solver and original_solver.registration_status == "saved")
        ):
            update_solver_email_data.update(
                {
                    "ApplicationCategory__c": "登録",
                    "RegistrationCategory__c": "更新",
                }
            )

        # 個人ソルバー登録申請の場合
        # スクリーニング/クライテリア項目（常に連携）
        if item.mode == "register_solver" and item.registration_status == "saved":
            update_solver_email_data.update({"ScreeningEvaluation1__c": "〇" if item.screening_1.evaluation else ""})
            update_solver_email_data.update({"ScreeningEvidence1__c": item.screening_1.evidence})
            update_solver_email_data.update({"ScreeningEvaluation2__c": "〇" if item.screening_2.evaluation else ""})
            update_solver_email_data.update({"ScreeningEvidence2__c": item.screening_2.evidence})
            update_solver_email_data.update({"ScreeningEvaluation3__c": "〇" if item.screening_3.evaluation else ""})
            update_solver_email_data.update({"ScreeningEvidence3__c": item.screening_3.evidence})
            update_solver_email_data.update({"ScreeningEvaluation4__c": "〇" if item.screening_4.evaluation else ""})
            update_solver_email_data.update({"ScreeningEvidence4__c": item.screening_4.evidence})
            update_solver_email_data.update({"ScreeningEvaluation5__c": "〇" if item.screening_5.evaluation else ""})
            update_solver_email_data.update({"ScreeningEvidence5__c": item.screening_5.evidence})
            update_solver_email_data.update({"ScreeningEvaluation6__c": "〇" if item.screening_6.evaluation else ""})
            update_solver_email_data.update({"ScreeningEvidence6__c": item.screening_6.evidence})
            update_solver_email_data.update({"ScreeningEvaluation7__c": "〇" if item.screening_7.evaluation else ""})
            update_solver_email_data.update({"ScreeningEvidence7__c": item.screening_7.evidence})
            update_solver_email_data.update({"ScreeningEvaluation8__c": "〇" if item.screening_8.evaluation else ""})
            update_solver_email_data.update({"ScreeningEvidence8__c": item.screening_8.evidence})
            update_solver_email_data.update({"CriteriaEvidence1__c": item.criteria_1 if item.criteria_1 else ""})
            update_solver_email_data.update({"CriteriaEvidence2__c": item.criteria_2 if item.criteria_2 else ""})
            update_solver_email_data.update({"CriteriaEvidence3__c": item.criteria_3 if item.criteria_3 else ""})
            update_solver_email_data.update({"CriteriaEvidence4__c": item.criteria_4 if item.criteria_4 else ""})
            update_solver_email_data.update({"CriteriaEvidence5__c": item.criteria_5 if item.criteria_5 else ""})
            update_solver_email_data.update({"CriteriaEvidence6__c": item.criteria_6 if item.criteria_6 else ""})
            update_solver_email_data.update({"CriteriaEvidence7__c": item.criteria_7 if item.criteria_7 else ""})
            update_solver_email_data.update({"CriteriaEvidence8__c": item.criteria_8 if item.criteria_8 else ""})

        # ソルバー候補更新（個人ソルバー登録申請済み）の場合
        if item.mode == "update_candidate" and not original_solver.is_solver and original_solver.registration_status == "saved":
            # スクリーニング項目1（評価）
            if (
                original_solver.screening_1 is None
                or item.screening_1.evaluation != original_solver.screening_1.evaluation
            ):
                update_solver_email_data.update(
                    {"ScreeningEvaluation1__c": "〇" if item.screening_1.evaluation else ""}
                )
            # スクリーニング項目1（エビデンス）
            if (
                original_solver.screening_1 is None
                or item.screening_1.evidence != original_solver.screening_1.evidence
            ):
                update_solver_email_data.update(
                    {"ScreeningEvidence1__c": item.screening_1.evidence}
                )

            # スクリーニング項目2（評価）
            if (
                original_solver.screening_2 is None
                or item.screening_2.evaluation != original_solver.screening_2.evaluation
            ):
                update_solver_email_data.update(
                    {"ScreeningEvaluation2__c": "〇" if item.screening_2.evaluation else ""}
                )
            # スクリーニング項目2（エビデンス）
            if (
                original_solver.screening_2 is None
                or item.screening_2.evidence != original_solver.screening_2.evidence
            ):
                update_solver_email_data.update(
                    {"ScreeningEvidence2__c": item.screening_2.evidence}
                )

            # スクリーニング項目3（評価）
            if (
                original_solver.screening_3 is None
                or item.screening_3.evaluation != original_solver.screening_3.evaluation
            ):
                update_solver_email_data.update(
                    {"ScreeningEvaluation3__c": "〇" if item.screening_3.evaluation else ""}
                )
            # スクリーニング項目3（エビデンス）
            if (
                original_solver.screening_3 is None
                or item.screening_3.evidence != original_solver.screening_3.evidence
            ):
                update_solver_email_data.update(
                    {"ScreeningEvidence3__c": item.screening_3.evidence}
                )

            # スクリーニング項目4（評価）
            if (
                original_solver.screening_4 is None
                or item.screening_4.evaluation != original_solver.screening_4.evaluation
            ):
                update_solver_email_data.update(
                    {"ScreeningEvaluation4__c": "〇" if item.screening_4.evaluation else ""}
                )
            # スクリーニング項目4（エビデンス）
            if (
                original_solver.screening_4 is None
                or item.screening_4.evidence != original_solver.screening_4.evidence
            ):
                update_solver_email_data.update(
                    {"ScreeningEvidence4__c": item.screening_4.evidence}
                )

            # スクリーニング項目5（評価）
            if (
                original_solver.screening_5 is None
                or item.screening_5.evaluation != original_solver.screening_5.evaluation
            ):
                update_solver_email_data.update(
                    {"ScreeningEvaluation5__c": "〇" if item.screening_5.evaluation else ""}
                )
            # スクリーニング項目5（エビデンス）
            if (
                original_solver.screening_5 is None
                or item.screening_5.evidence != original_solver.screening_5.evidence
            ):
                update_solver_email_data.update(
                    {"ScreeningEvidence5__c": item.screening_5.evidence}
                )

            # スクリーニング項目6（評価）
            if (
                original_solver.screening_6 is None
                or item.screening_6.evaluation != original_solver.screening_6.evaluation
            ):
                update_solver_email_data.update(
                    {"ScreeningEvaluation6__c": "〇" if item.screening_6.evaluation else ""}
                )
            # スクリーニング項目6（エビデンス）
            if (
                original_solver.screening_6 is None
                or item.screening_6.evidence != original_solver.screening_6.evidence
            ):
                update_solver_email_data.update(
                    {"ScreeningEvidence6__c": item.screening_6.evidence}
                )

            # スクリーニング項目7（評価）
            if (
                original_solver.screening_7 is None
                or item.screening_7.evaluation != original_solver.screening_7.evaluation
            ):
                update_solver_email_data.update(
                    {"ScreeningEvaluation7__c": "〇" if item.screening_7.evaluation else ""}
                )
            # スクリーニング項目7（エビデンス）
            if (
                original_solver.screening_7 is None
                or item.screening_7.evidence != original_solver.screening_7.evidence
            ):
                update_solver_email_data.update(
                    {"ScreeningEvidence7__c": item.screening_7.evidence}
                )

            # スクリーニング項目8（評価）
            if (
                original_solver.screening_8 is None
                or item.screening_8.evaluation != original_solver.screening_8.evaluation
            ):
                update_solver_email_data.update(
                    {"ScreeningEvaluation8__c": "〇" if item.screening_8.evaluation else ""}
                )
            # スクリーニング項目8（エビデンス）
            if (
                original_solver.screening_8 is None
                or item.screening_8.evidence != original_solver.screening_8.evidence
            ):
                update_solver_email_data.update(
                    {"ScreeningEvidence8__c": item.screening_8.evidence}
                )

            # クライテリア項目1エビデンス
            if item.criteria_1 != original_solver.criteria_1:
                update_solver_email_data.update({"CriteriaEvidence1__c": item.criteria_1})

            # クライテリア項目2エビデンス
            if item.criteria_2 != original_solver.criteria_2:
                update_solver_email_data.update({"CriteriaEvidence2__c": item.criteria_2})

            # クライテリア項目3エビデンス
            if item.criteria_3 != original_solver.criteria_3:
                update_solver_email_data.update({"CriteriaEvidence3__c": item.criteria_3})

            # クライテリア項目4エビデンス
            if item.criteria_4 != original_solver.criteria_4:
                update_solver_email_data.update({"CriteriaEvidence4__c": item.criteria_4})

            # クライテリア項目5エビデンス
            if item.criteria_5 != original_solver.criteria_5:
                update_solver_email_data.update({"CriteriaEvidence5__c": item.criteria_5})

            # クライテリア項目6エビデンス
            if item.criteria_6 != original_solver.criteria_6:
                update_solver_email_data.update({"CriteriaEvidence6__c": item.criteria_6})

            # クライテリア項目7エビデンス
            if item.criteria_7 != original_solver.criteria_7:
                update_solver_email_data.update({"CriteriaEvidence7__c": item.criteria_7})

            # クライテリア項目8エビデンス
            if item.criteria_8 != original_solver.criteria_8:
                update_solver_email_data.update({"CriteriaEvidence8__c": item.criteria_8})

        # 備考
        if item.notes != original_solver.notes:
            update_solver_email_data.update({"Notes__c": item.notes})

        now = get_datetime_now()
        # 削除フラグ・作成日時・作成者
        # 初回更新の場合、データが更新されているかに関係なくSFに連携
        if original_solver.update_at is None:
            update_solver_email_data.update(
                {
                    "IsDelete__c": False,
                    "PartnerPortalCreatedDate__c": now.strftime("%Y/%m/%d %H:%M"),
                    "PartnerPortalCreatedBy__c": AdminModel.get_update_user_name(
                        original_solver.create_id
                    ),
                }
            )

        # 更新日時
        update_solver_email_data.update(
            {"PartnerPortalModifiedDate__c": now.strftime("%Y/%m/%d %H:%M")}
        )

        # 更新者
        update_solver_email_data.update({"PartnerPortalModifiedBy__c": current_user.name})

        return update_solver_email_data

    @staticmethod
    def get_solver_application_info(
        solver_application_id_list: List[str],
    ) -> List[SolverApplicationModel]:
        """ソルバー案件テーブルからソルバー案件情報を取得
        Args:
            solver_application_id_list (List[str]): ソルバー案件ID（PK）のリスト
        Returns:
            solver_application_model_list（List[SolverApplicationModel]）: ソルバー案件情報
        """
        solver_application_model_list = []
        item_keys = [
            (id, DataType.SOLVER_APPLICATION) for id in solver_application_id_list
        ]

        for item in SolverApplicationModel.batch_get(item_keys):
            solver_application_model_list.append(item)

        return solver_application_model_list

    @staticmethod
    def get_solver_application_info_by_application_ids(
        solver_application_ids: List[str],
    ) -> List[SolverApplicationModel]:
        """ソルバー案件テーブルからソルバー案件情報を取得
        Args:
            solver_application_ids (List[str]): ソルバー案件IDのリスト
        Returns:
            solver_application_model_list（List[SolverApplicationModel]）: ソルバー案件情報
        """
        condition = None
        for solver_application_id in solver_application_ids:
            if condition is None:
                condition = SolverApplicationModel.solver_application_id == solver_application_id
            else:
                condition |= SolverApplicationModel.solver_application_id == solver_application_id

        return list(
            SolverApplicationModel.scan(condition)
        )

    @staticmethod
    def get_file_from_s3(
        file: Union[FacePhoto, Resume],
    ) -> dict:
        """ファイルをS3から取得
        Args:
            file (Union[FacePhoto, Resume]): ファイル
        Returns:
            file_info: ファイル情報（ファイル名・S3保存パス・内容）
        """

        try:
            file_content: str = S3Helper().get_object_content(
                bucket_name=get_app_settings().upload_s3_bucket_name,
                object_key=file.path,
            )
            # ファイル名・S3保存パス・内容をリストに格納
            file_info = {
                "file_name": file.file_name,
                "path": file.path,
                "file_content": file_content,
            }
        except botocore.exceptions.ClientError as e:
            if e.response["Error"]["Code"] == "NoSuchKey":
                logger.warning("File not found.")
                raise HTTPException(
                    status_code=status.HTTP_404_NOT_FOUND,
                    detail="File not found.",
                )
            else:
                logger.error(e)
                raise e

        return file_info

    @staticmethod
    def create_solver(
        items: CreateSolverRequest,
        current_user: UserModel,
    ) -> OKResponse:
        """Post /solvers 新規ソルバー候補申請・新規個人ソルバー登録申請API
        Args:
            items (List[SolverInfoForCreateSolver]): 登録内容
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            OKResponse: 登録結果
        """
        # ソルバー登録データ連携通知（メール）
        # メールの宛先
        # TO：SalesForce、CC：システム管理者
        logger.info("start making address")
        cc_email_list: List[str] = []
        admin_filter_condition = AdminModel.disabled == False  # NOQA
        for mail_admin_itr in AdminModel.scan(filter_condition=admin_filter_condition):
            if UserRoleType.SYSTEM_ADMIN.key in mail_admin_itr.roles:
                cc_email_list.append(mail_admin_itr.email)

        now_datetime = get_datetime_now()
        solver_application_id: str = ""
        for item in items.solvers_info:
            files: List = []
            if (
                current_user.role == UserRoleType.SOLVER_STAFF.key
                and current_user.solver_corporation_id != item.corporate_id
            ):
                # 所属していない法人のソルバー作成は不可
                logger.warning("CreateSolverById forbidden.")
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )

            # modeが「create_candidate」「create_solver」以外の場合はエラーを返す
            # FIXME Schemaの箇所で定義
            if item.mode not in ["create_candidate", "create_solver"]:
                logger.warning("Forbidden: createSolver API is only create_candidate or create_solver.")
                raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden")

            # ソルバー案件テーブルに対象の案件がない場合、新規登録
            if item.solver_application_id:
                solver_application_id = SolverService.create_solver_application(
                    item=item,
                    current_user=current_user
                )

            # ソルバーテーブルにidが存在：更新処理、存在しない：登録処理
            if item.id:
                # 法人ソルバー担当
                try:
                    solver = SolverModel.get(hash_key=item.id, range_key=DataType.SOLVER)
                except DoesNotExist:
                    logger.warning(f"Not found solver id: {item.id}")
                    raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Not found")
                if current_user.role == UserRoleType.SOLVER_STAFF.key:
                    # 法人ソルバー担当の所属法人とリクエストのソルバーの所属法人が一致しない場合は更新不可
                    if current_user.solver_corporation_id != solver.corporate_id:
                        logger.warning("Forbidden: solver_corporate_id does not match current_user corporate_id.")
                        raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden")
                # アライアンス担当
                elif current_user.role == UserRoleType.APT.key:
                    try:
                        # リクエストボディの法人IDと取得したソルバーの所属法人が一致しない場合は更新不可
                        if item.corporate_id != solver.corporate_id:
                            logger.warning("Forbidden: corporate_id does not match request corporate_id.")
                            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden")
                    except DoesNotExist:
                        logger.warning(f"Not found solver id: {item.id}")
                        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Not found")

                # S3から更新したソルバー画像を取得
                if (
                    solver.face_photo is None
                    and item.face_photo
                    and item.face_photo.path
                ) or (
                    solver.face_photo
                    and item.face_photo
                    and item.face_photo.path
                    and item.face_photo.path != solver.face_photo.path
                ):
                    files.append(SolverService.get_file_from_s3(file=item.face_photo))

                # S3から更新した添付資料を取得
                if item.resume:
                    for request_resume in item.resume:
                        # DBと一致する添付資料があるかを判定するフラグ
                        is_exited_resume: bool = False
                        if solver.resume is None:
                            files.append(SolverService.get_file_from_s3(file=request_resume))
                        else:
                            for model_resume in solver.resume:
                                if request_resume.path == model_resume["path"]:
                                    is_exited_resume = True
                                    break

                            if not is_exited_resume:
                                files.append(SolverService.get_file_from_s3(file=request_resume))

                # 更新前のDB情報を保存
                original_solver = copy.deepcopy(solver)

                solver.name = item.name
                solver.name_kana = item.name_kana
                solver.solver_application_ids = solver.solver_application_ids or set()
                if solver_application_id:
                    solver.solver_application_ids.add(solver_application_id)
                solver.corporate_id = item.corporate_id
                solver.sex = item.sex
                solver.birth_day = item.birth_day
                solver.email = item.email
                solver.phone = item.phone
                solver.english_level = item.english_level
                solver.work_history = item.work_history
                solver.is_consulting_firm = item.is_consulting_firm
                solver.specialized_themes = item.specialized_themes
                solver.academic_background = item.academic_background
                solver.title = item.title
                solver.face_photo = FacePhotoAttribute(
                    file_name=item.face_photo.file_name,
                    path=item.face_photo.path
                ) if item.face_photo else None
                solver.resume = [
                    ResumeAttribute(**resume_itr.dict()) for resume_itr in item.resume
                ] if item.resume else None
                solver.operating_status = item.operating_status
                solver.provided_operating_rate = item.provided_operating_rate
                solver.provided_operating_rate_next = item.provided_operating_rate_next
                solver.operation_prospects_month_after_next = item.operation_prospects_month_after_next
                solver.price_per_person_month = item.price_per_person_month
                solver.price_per_person_month_lower = item.price_per_person_month_lower
                solver.hourly_rate = item.hourly_rate
                solver.hourly_rate_lower = item.hourly_rate_lower
                solver.tsi_areas = item.tsi_areas
                solver.issue_map50 = item.issue_map50
                solver.main_achievements = item.main_achievements
                solver.notes = item.notes
                solver.update_id = current_user.id
                solver.update_at = now_datetime

                solver.save()
                logger.info(f"CreateSolver updated. id: {item.id}")

            else:
                # IDを生成
                item.id = str(uuid.uuid4())

                # S3から個人ソルバー画像を取得
                if item.face_photo and item.face_photo.path:
                    files.append(SolverService.get_file_from_s3(file=item.face_photo))

                # S3から添付資料を取得
                if item.resume:
                    for resume in item.resume:
                        files.append(SolverService.get_file_from_s3(file=resume))

                solver = SolverModel(
                    id=item.id,
                    data_type=DataType.SOLVER,
                    name=item.name,
                    name_kana=item.name_kana,
                    solver_application_ids={solver_application_id} if solver_application_id else None,
                    corporate_id=item.corporate_id,
                    sex=item.sex,
                    birth_day=item.birth_day,
                    email=item.email,
                    phone=item.phone,
                    english_level=item.english_level,
                    work_history=item.work_history,
                    is_consulting_firm=item.is_consulting_firm,
                    specialized_themes=item.specialized_themes,
                    academic_background=item.academic_background,
                    title=item.title,
                    face_photo=FacePhotoAttribute(
                        file_name=item.face_photo.file_name,
                        path=item.face_photo.path,
                    ) if item.face_photo else None,
                    resume=[
                        ResumeAttribute(**resume_itr.dict()) for resume_itr in item.resume
                    ] if item.resume else None,
                    operating_status=item.operating_status,
                    provided_operating_rate=item.provided_operating_rate,
                    provided_operating_rate_next=item.provided_operating_rate_next,
                    operation_prospects_month_after_next=item.operation_prospects_month_after_next,
                    price_per_person_month=item.price_per_person_month,
                    price_per_person_month_lower=item.price_per_person_month_lower,
                    hourly_rate=item.hourly_rate,
                    hourly_rate_lower=item.hourly_rate_lower,
                    tsi_areas=item.tsi_areas,
                    issue_map50=item.issue_map50,
                    main_achievements=item.main_achievements,
                    notes=item.notes,
                    registration_status=item.registration_status,
                    create_id=current_user.id,
                    create_at=now_datetime,
                    update_id=current_user.id,
                    update_at=now_datetime,
                    file_key_id=item.file_key_id,
                )

                # 新規個人ソルバー登録申請の場合、スクリーニングおよびクライテリア項目を追加
                if item.mode == "create_solver":
                    solver.is_solver = True
                    solver.screening_1 = ScreeningAttribute(
                        evaluation=item.screening_1.evaluation,
                        evidence=item.screening_1.evidence,
                    )
                    solver.screening_2 = ScreeningAttribute(
                        evaluation=item.screening_2.evaluation,
                        evidence=item.screening_2.evidence,
                    )
                    solver.screening_3 = ScreeningAttribute(
                        evaluation=item.screening_3.evaluation,
                        evidence=item.screening_3.evidence,
                    )
                    solver.screening_4 = ScreeningAttribute(
                        evaluation=item.screening_4.evaluation,
                        evidence=item.screening_4.evidence,
                    )
                    solver.screening_5 = ScreeningAttribute(
                        evaluation=item.screening_5.evaluation,
                        evidence=item.screening_5.evidence,
                    )
                    solver.screening_6 = ScreeningAttribute(
                        evaluation=item.screening_6.evaluation,
                        evidence=item.screening_6.evidence,
                    )
                    solver.screening_7 = ScreeningAttribute(
                        evaluation=item.screening_7.evaluation,
                        evidence=item.screening_7.evidence,
                    )
                    solver.screening_8 = ScreeningAttribute(
                        evaluation=item.screening_8.evaluation,
                        evidence=item.screening_8.evidence,
                    )
                    solver.criteria_1 = item.criteria_1
                    solver.criteria_2 = item.criteria_2
                    solver.criteria_3 = item.criteria_3
                    solver.criteria_4 = item.criteria_4
                    solver.criteria_5 = item.criteria_5
                    solver.criteria_6 = item.criteria_6
                    solver.criteria_7 = item.criteria_7
                    solver.criteria_8 = item.criteria_8

                solver.save()
                logger.info(f"CreateSolver created. id: {item.id}")

            # 新規個人ソルバー登録申請にて一時保存をした場合、SFデータ連携メールは送信しない
            if item.mode == "create_solver" and item.registration_status == "temporary_saving":
                return OKResponse()

            # メール内容の編集
            logger.info("start editing mail setting")
            if item.is_registered_solver:
                email_data = SolverService.get_formatted_update_solver_email_data(
                    solver_id=item.id,
                    item=item,
                    original_solver=original_solver,
                    current_user=current_user,
                )

                # URLデコード
                decoded_email_data = url_decode_data(email_data)
                solver_data = json.dumps(decoded_email_data, ensure_ascii=False)

                payload = {
                    "subject": SalesforceDataSyncMailType.UPDATE_SOLVER if solver.is_solver else SalesforceDataSyncMailType.UPDATE_SOLVER_CANDIDATE,
                    "solver_corporation_data": solver_data,
                }
                payload_error = {
                    "error_datetime": now_datetime,
                    "error_function": SalesforceDataSyncMailType.UPDATE_SOLVER if solver.is_solver else SalesforceDataSyncMailType.UPDATE_SOLVER_CANDIDATE,
                    "user_name": current_user.name,
                    "data": solver_data,
                }
            else:
                email_data = SolverService.get_formatted_create_solver_email_data(
                    item=item,
                    solver_id=item.id,
                    current_user=current_user,
                    now_datetime=now_datetime,

                )

                # URLデコード
                decoded_email_data = url_decode_data(email_data)
                solver_data = json.dumps(decoded_email_data, ensure_ascii=False)

                payload = {
                    "subject": SalesforceDataSyncMailType.CRATE_SOLVER_CANDIDATE if item.mode == "create_candidate" else SalesforceDataSyncMailType.CREATE_SOLVER,
                    "solver_corporation_data": solver_data,
                }
                payload_error = {
                    "error_datetime": now_datetime,
                    "error_function": SalesforceDataSyncMailType.CRATE_SOLVER_CANDIDATE if item.mode == "create_candidate" else SalesforceDataSyncMailType.CREATE_SOLVER,
                    "user_name": current_user.name,
                    "data": solver_data,
                }

            # SESで直接メールを送信
            logger.info("start sending mail")
            result = SesHelper().send_mail_with_file(
                template_name=MailType.SALESFORCE_DATA_SYNC,
                to=[get_app_settings().salesforce_address_for_solver],
                cc=cc_email_list,
                payload=payload,
                payload_error=payload_error,
                files=files,
            )

            # エラーメール通知が送信された場合、ロールバックを行う
            # 既存ソルバーの場合は更新前の状態に戻し、新規ソルバーの場合は削除
            if result and result["email_type"] == "error":
                if item.is_registered_solver:
                    solver.name = original_solver.name
                    solver.name_kana = original_solver.name_kana
                    solver.solver_application_ids = original_solver.solver_application_ids or set()
                    solver.corporate_id = original_solver.corporate_id
                    solver.sex = original_solver.sex
                    solver.birth_day = original_solver.birth_day
                    solver.email = original_solver.email
                    solver.phone = original_solver.phone
                    solver.english_level = original_solver.english_level
                    solver.work_history = original_solver.work_history
                    solver.is_consulting_firm = original_solver.is_consulting_firm
                    solver.specialized_themes = original_solver.specialized_themes
                    solver.academic_background = original_solver.academic_background
                    solver.title = original_solver.title
                    solver.face_photo = FacePhotoAttribute(
                        file_name=original_solver.face_photo.file_name,
                        path=original_solver.face_photo.path
                    ) if original_solver.face_photo else None
                    solver.resume = [
                        ResumeAttribute(**resume_itr.dict()) for resume_itr in original_solver.resume
                    ] if original_solver.resume else None
                    solver.operating_status = original_solver.operating_status
                    solver.provided_operating_rate = original_solver.provided_operating_rate
                    solver.provided_operating_rate_next = original_solver.provided_operating_rate_next
                    solver.operation_prospects_month_after_next = original_solver.operation_prospects_month_after_next
                    solver.price_per_person_month = original_solver.price_per_person_month
                    solver.price_per_person_month_lower = original_solver.price_per_person_month_lower
                    solver.hourly_rate = original_solver.hourly_rate
                    solver.hourly_rate_lower = original_solver.hourly_rate_lower
                    solver.tsi_areas = original_solver.tsi_areas
                    solver.issue_map50 = original_solver.issue_map50
                    solver.main_achievements = original_solver.main_achievements
                    solver.notes = original_solver.notes
                    solver.update_id = original_solver.update_id
                    solver.update_at = original_solver.update_at
                    solver.save()
                else:
                    solver.delete()

        return OKResponse()

    @staticmethod
    def get_solvers(
        query_params: GetSolversQuery,
        current_user: UserModel,
    ) -> GetSolversResponse:
        """Get /solvers 個人ソルバー一覧取得API
        Args:
            query_params (GetSolversQuery): クエリパラメータ
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            GetSolversResponse: 取得結果
        """
        if (
            current_user.role == UserRoleType.SOLVER_STAFF.key
            and current_user.solver_corporation_id != query_params.id
        ):
            # 所属していない法人のソルバー一覧はアクセス不可
            logger.warning("GetSolvers forbidden.")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # GSIソート順
        # ソート種別が一致したら条件文を抜ける
        scan_index_forward = None

        # ソート:sex
        if query_params.sort == GetSolversSortType.SEX_ASC:
            scan_index_forward = True
        elif query_params.sort == GetSolversSortType.SEX_DESC:
            scan_index_forward = False

        # ソート:birth_day
        # フロントで「年齢」の昇順・降順が逆転する現象が起きてしまう為ソートを逆にする
        # ※誕生日でのソートの場合は年数が古いほうが新しい
        elif query_params.sort == GetSolversSortType.BIRTH_DAY_ASC:
            scan_index_forward = False
        elif query_params.sort == GetSolversSortType.BIRTH_DAY_DESC:
            scan_index_forward = True

        # ソート:registration_status
        elif query_params.sort == GetSolversSortType.REGISTRATION_STATUS_ASC:
            scan_index_forward = True
        elif query_params.sort == GetSolversSortType.REGISTRATION_STATUS_DESC:
            scan_index_forward = False

        # ソート:operating_status
        elif query_params.sort == GetSolversSortType.OPERATION_STATUS_ASC:
            scan_index_forward = True
        elif query_params.sort == GetSolversSortType.OPERATION_STATUS_DESC:
            scan_index_forward = False

        # ソート:provided_operating_rate
        elif query_params.sort == GetSolversSortType.PROVIDED_OPERATING_RATE_ASC:
            scan_index_forward = True
        elif query_params.sort == GetSolversSortType.PROVIDED_OPERATING_RATE_DESC:
            scan_index_forward = False

        # ソート:provided_operating_rate_next
        elif query_params.sort == GetSolversSortType.PROVIDED_OPERATING_RATE_NEXT_ASC:
            scan_index_forward = True
        elif query_params.sort == GetSolversSortType.PROVIDED_OPERATING_RATE_NEXT_DESC:
            scan_index_forward = False

        # ソート:price_per_person_month
        elif query_params.sort == GetSolversSortType.PRICE_PER_PERSON_MONTH_ASC:
            scan_index_forward = True
        elif query_params.sort == GetSolversSortType.PRICE_PER_PERSON_MONTH_DESC:
            scan_index_forward = False

        # ソート:price_and_operating_rate_update_at
        elif (
            query_params.sort
            == GetSolversSortType.PRICE_AND_OPERATING_RATE_UPDATE_AT_ASC
        ):
            scan_index_forward = True
        elif (
            query_params.sort
            == GetSolversSortType.PRICE_AND_OPERATING_RATE_UPDATE_AT_DESC
        ):
            scan_index_forward = False

        # ソート:create_at:desc（デフォルト）
        else:
            scan_index_forward = GetSolversSortType.CREATE_AT_DESC

        # Modelのqueryでflake8(E712)エラーが出るため定義
        bool_true: bool = True
        bool_false: bool = False
        # クエリ条件を指定
        filter_condition = None
        # クエリ条件:id
        filter_condition &= SolverModel.corporate_id == query_params.id

        # クエリ条件:solver_type
        if query_params.solver_type:
            if query_params.solver_type == "all":
                # ソルバー候補(is_solverがFalse) + 登録済み個人ソルバー (is_solverがTrue 且つ registration_statusがsavedかcertificated)
                filter_condition &= (SolverModel.is_solver == bool_false) | (
                    (SolverModel.is_solver == bool_true)
                    & ((SolverModel.registration_status == "saved") | (SolverModel.registration_status == "certificated"))
                )
            elif query_params.solver_type == "solver_candidate":
                filter_condition &= SolverModel.is_solver == bool_false
            elif query_params.solver_type == "solver":
                filter_condition &= SolverModel.is_solver == bool_true
            elif query_params.solver_type == "certificated_solver":
                filter_condition &= (SolverModel.is_solver == bool_true) & (SolverModel.registration_status == "certificated")

        # クエリ条件:name
        if query_params.name:
            filter_condition &= SolverModel.name.startswith(query_params.name)

        # クエリ条件:sex
        if query_params.sex and query_params.sex != "all":
            filter_condition &= SolverModel.sex == query_params.sex

        # クエリ条件:certification_status
        if query_params.certification_status:
            if query_params.certification_status == "before":
                # 新規登録(registration_statusがnew) + 一時保存(registration_statusがtemporary_saving)
                filter_condition &= (SolverModel.registration_status == "new") | (
                    SolverModel.registration_status == "temporary_saving"
                )
            if query_params.certification_status == "during":
                filter_condition &= SolverModel.registration_status == "saved"

        # クエリ条件:operating_status
        if query_params.operating_status and query_params.operating_status != "all":
            filter_condition &= SolverModel.operating_status == query_params.operating_status

        # 個人ソルバー情報取得
        # 取得条件が一致したら条件文を抜ける
        if query_params.sort in [
            GetSolversSortType.SEX_ASC,
            GetSolversSortType.SEX_DESC,
        ]:
            if query_params.sex:
                # range_keyがsexのGSIでなく別のGSIを使用する
                result_list: List[SolverModel] = list(
                    SolverModel.data_type_index.query(
                        hash_key=DataType.SOLVER,
                        filter_condition=filter_condition,
                    )
                )
            else:
                result_list: List[SolverModel] = list(
                    SolverModel.data_type_sex_index.query(
                        hash_key=DataType.SOLVER,
                        filter_condition=filter_condition,
                        scan_index_forward=scan_index_forward,
                    )
                )

        elif query_params.sort in [
            GetSolversSortType.BIRTH_DAY_ASC,
            GetSolversSortType.BIRTH_DAY_DESC,
        ]:
            result_list: List[SolverModel] = list(
                SolverModel.data_type_birth_day_index.query(
                    hash_key=DataType.SOLVER,
                    filter_condition=filter_condition,
                    scan_index_forward=scan_index_forward,
                )
            )

        elif query_params.sort in [
            GetSolversSortType.REGISTRATION_STATUS_ASC,
            GetSolversSortType.REGISTRATION_STATUS_DESC,
        ]:
            if query_params.certification_status:
                # range_keyがcertification_statusのGSIでなく別のGSIを使用する
                result_list: List[SolverModel] = list(
                    SolverModel.data_type_index.query(
                        hash_key=DataType.SOLVER,
                        filter_condition=filter_condition,
                    )
                )
            else:
                result_list: List[SolverModel] = list(
                    SolverModel.data_type_registration_status_index.query(
                        hash_key=DataType.SOLVER,
                        filter_condition=filter_condition,
                        scan_index_forward=scan_index_forward,
                    )
                )

        elif query_params.sort in [
            GetSolversSortType.OPERATION_STATUS_ASC,
            GetSolversSortType.OPERATION_STATUS_DESC,
        ]:
            if query_params.operating_status:
                # range_keyがoperating_statusのGSIでなく別のGSIを使用する
                result_list: List[SolverModel] = list(
                    SolverModel.data_type_index.query(
                        hash_key=DataType.SOLVER,
                        filter_condition=filter_condition,
                    )
                )
            else:
                result_list: List[SolverModel] = list(
                    SolverModel.data_type_operating_status_index.query(
                        hash_key=DataType.SOLVER,
                        filter_condition=filter_condition,
                        scan_index_forward=scan_index_forward,
                    )
                )

        elif query_params.sort in [
            GetSolversSortType.PROVIDED_OPERATING_RATE_ASC,
            GetSolversSortType.PROVIDED_OPERATING_RATE_DESC,
        ]:
            result_list: List[SolverModel] = list(
                SolverModel.data_type_provided_operating_rate_index.query(
                    hash_key=DataType.SOLVER,
                    filter_condition=filter_condition,
                    scan_index_forward=scan_index_forward,
                )
            )

        elif query_params.sort in [
            GetSolversSortType.PROVIDED_OPERATING_RATE_NEXT_ASC,
            GetSolversSortType.PROVIDED_OPERATING_RATE_NEXT_DESC,
        ]:
            result_list: List[SolverModel] = list(
                SolverModel.data_type_provided_operating_rate_next_index.query(
                    hash_key=DataType.SOLVER,
                    filter_condition=filter_condition,
                    scan_index_forward=scan_index_forward,
                )
            )

        elif query_params.sort in [
            GetSolversSortType.PRICE_PER_PERSON_MONTH_ASC,
            GetSolversSortType.PRICE_PER_PERSON_MONTH_DESC,
        ]:
            result_list: List[SolverModel] = list(
                SolverModel.data_type_price_per_person_month_index.query(
                    hash_key=DataType.SOLVER,
                    filter_condition=filter_condition,
                    scan_index_forward=scan_index_forward,
                )
            )

        elif query_params.sort in [
            GetSolversSortType.PRICE_AND_OPERATING_RATE_UPDATE_AT_ASC,
            GetSolversSortType.PRICE_AND_OPERATING_RATE_UPDATE_AT_DESC,
        ]:
            result_list: List[SolverModel] = list(
                SolverModel.data_type_price_and_operating_rate_update_at_index.query(
                    hash_key=DataType.SOLVER,
                    filter_condition=filter_condition,
                    scan_index_forward=scan_index_forward,
                )
            )

        else:
            result_list: List[SolverModel] = list(
                SolverModel.data_type_create_at_index.query(
                    hash_key=DataType.SOLVER,
                    filter_condition=filter_condition,
                    scan_index_forward=False,
                )
            )

        # 年齢を含むソルバー情報リストの作成
        solver_info_list: List[SolverInfoForGetSolvers] = []
        today = datetime.now()
        for solver in result_list:
            solver_data = solver.attribute_values
            # 生年月日の存在を確認し、存在する場合は年齢を計算
            birth_day = solver_data.get("birth_day")
            if birth_day and birth_day != DEFAULT_BIRTHDAY:
                birth_date = datetime.strptime(birth_day, "%Y/%m/%d")
                age = (
                    today.year
                    - birth_date.year
                    - ((today.month, today.day) < (birth_date.month, birth_date.day))
                )
                solver_data["age"] = age
            else:
                solver_data["age"] = None

            # SolverInfoForGetSolversに追加
            solver_info_list.append(SolverInfoForGetSolvers(**solver_data))

        # ページネーション
        current_page: List[SolverInfoForGetSolvers] = []
        if query_params.limit and query_params.offset_page:
            try:
                p = Paginator(solver_info_list, query_params.limit)
                current_page = p.page(query_params.offset_page).object_list
            except EmptyPage:
                logger.warning("GetSolvers not found.")
                raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        return GetSolversResponse(
            offset_page=query_params.offset_page,
            total=len(solver_info_list),
            solvers=current_page if current_page else solver_info_list,
        )

    @staticmethod
    def get_solver_by_id(
        current_user: UserModel, solver_id: str
    ) -> GetSolverByIdResponse:
        """Get /solvers/{solver_id} ソルバー情報取得API
        Args:
            solver_id (str): 個人ソルバーID
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            GetSolverByIdResponse:
        """

        try:
            solverInfo = SolverModel.get(hash_key=solver_id, range_key=DataType.SOLVER)
        except DoesNotExist:
            logger.warning(f"GetSolverById solver_id not found. solver_id: {solver_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if current_user.role == UserRoleType.SOLVER_STAFF.key:
            if current_user.solver_corporation_id != solverInfo.corporate_id:
                # 所属していない法人の個人ソルバー情報はアクセス不可
                logger.warning("GetSolverById forbidden.")
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )

        # DBから取得した値をNULL検証して、レスポンス整形する
        item_create_user_id: str = ""
        item_create_user_name: str = ""
        item_update_user_id: str = ""
        item_update_user_name: str = ""
        item_tsi_areas_list = []
        item_issue_map50_list = []
        item_solver_applications_list: List[SolverApplications] = []
        item_resume_list: List[Resume] = []

        # 案件名取得
        target_solver_application_id_list = []
        if solverInfo.solver_application_ids:
            for solver_application_id in solverInfo.solver_application_ids:
                target_solver_application_id_list.append(solver_application_id)

        solver_application_table_info: List[SolverApplicationModel] = (
            SolverService.get_solver_application_info(target_solver_application_id_list)
        )

        # 案件情報
        if solver_application_table_info is not None:
            for solver_application_table_info_itr in solver_application_table_info:
                item_solver_applications_list.append(
                    SolverApplications(
                        id=solver_application_table_info_itr.solver_application_id,
                        name=solver_application_table_info_itr.name,
                        project_code=SolverService.create_project_code(
                            solver_application_table_info_itr.solver_application_id,
                            solver_application_table_info_itr.name
                        )
                    )
                )

        # 課題マップ50（配列）
        if solverInfo.issue_map50 is not None:
            for issue_map50_itr in solverInfo.issue_map50:
                item_issue_map50_list.append(issue_map50_itr)

        # 個人ソルバー画像
        if solverInfo.face_photo:
            solverInfo.face_photo = FacePhoto(
                file_name=solverInfo.face_photo.file_name,
                path=solverInfo.face_photo.path,
            )

        # 添付資料
        if solverInfo.resume is not None:
            for resume_itr in solverInfo.resume:
                item_resume_list.append(
                    Resume(file_name=resume_itr.file_name, path=resume_itr.path)
                )

        # 東証33業種経験/対応可能領域
        if solverInfo.tsi_areas is not None:
            for tsi_areas_itr in solverInfo.tsi_areas:
                item_tsi_areas_list.append(tsi_areas_itr)

        # スクリーニング項目
        if solverInfo.screening_1:
            solverInfo.screening_1 = Screening(
                evaluation=solverInfo.screening_1.evaluation,
                evidence=solverInfo.screening_1.evidence,
            )
        if solverInfo.screening_2:
            solverInfo.screening_2 = Screening(
                evaluation=solverInfo.screening_2.evaluation,
                evidence=solverInfo.screening_2.evidence,
            )
        if solverInfo.screening_3:
            solverInfo.screening_3 = Screening(
                evaluation=solverInfo.screening_3.evaluation,
                evidence=solverInfo.screening_3.evidence,
            )
        if solverInfo.screening_4:
            solverInfo.screening_4 = Screening(
                evaluation=solverInfo.screening_4.evaluation,
                evidence=solverInfo.screening_4.evidence,
            )
        if solverInfo.screening_5:
            solverInfo.screening_5 = Screening(
                evaluation=solverInfo.screening_5.evaluation,
                evidence=solverInfo.screening_5.evidence,
            )
        if solverInfo.screening_6:
            solverInfo.screening_6 = Screening(
                evaluation=solverInfo.screening_6.evaluation,
                evidence=solverInfo.screening_6.evidence,
            )
        if solverInfo.screening_7:
            solverInfo.screening_7 = Screening(
                evaluation=solverInfo.screening_7.evaluation,
                evidence=solverInfo.screening_7.evidence,
            )
        if solverInfo.screening_8:
            solverInfo.screening_8 = Screening(
                evaluation=solverInfo.screening_8.evaluation,
                evidence=solverInfo.screening_8.evidence,
            )

        # 登録者名と最終更新者名を取得
        if solverInfo.create_id is not None:
            item_create_user_id = solverInfo.create_id
            item_create_user_name = get_update_user_name(solverInfo.create_id)
        if solverInfo.update_id is not None:
            item_update_user_id = solverInfo.update_id
            item_update_user_name = get_update_user_name(solverInfo.update_id)

        # レスポンス作成
        return_solver = GetSolverByIdResponse(
            id=solverInfo.id,
            name=solverInfo.name,
            name_kana=solverInfo.name_kana,
            solver_applications=item_solver_applications_list,
            title=solverInfo.title,
            email=solverInfo.email,
            phone=solverInfo.phone,
            issue_map50=item_issue_map50_list,
            corporate_id=solverInfo.corporate_id,
            sex=solverInfo.sex,
            birth_day=solverInfo.birth_day,
            operating_status=solverInfo.operating_status,
            face_photo=solverInfo.face_photo,
            resume=item_resume_list,
            academic_background=solverInfo.academic_background,
            work_history=solverInfo.work_history,
            is_consulting_firm=solverInfo.is_consulting_firm,
            specialized_themes=solverInfo.specialized_themes,
            main_achievements=solverInfo.main_achievements,
            provided_operating_rate=solverInfo.provided_operating_rate,
            provided_operating_rate_next=solverInfo.provided_operating_rate_next,
            operation_prospects_month_after_next=solverInfo.operation_prospects_month_after_next,
            price_per_person_month=solverInfo.price_per_person_month,
            price_per_person_month_lower=solverInfo.price_per_person_month_lower,
            hourly_rate=solverInfo.hourly_rate,
            hourly_rate_lower=solverInfo.hourly_rate_lower,
            english_level=solverInfo.english_level,
            tsi_areas=item_tsi_areas_list,
            screening_1=solverInfo.screening_1,
            screening_2=solverInfo.screening_2,
            screening_3=solverInfo.screening_3,
            screening_4=solverInfo.screening_4,
            screening_5=solverInfo.screening_5,
            screening_6=solverInfo.screening_6,
            screening_7=solverInfo.screening_7,
            screening_8=solverInfo.screening_8,
            criteria_1=solverInfo.criteria_1,
            criteria_2=solverInfo.criteria_2,
            criteria_3=solverInfo.criteria_3,
            criteria_4=solverInfo.criteria_4,
            criteria_5=solverInfo.criteria_5,
            criteria_6=solverInfo.criteria_6,
            criteria_7=solverInfo.criteria_7,
            criteria_8=solverInfo.criteria_8,
            notes=solverInfo.notes,
            is_solver=solverInfo.is_solver,
            registration_status=solverInfo.registration_status,
            create_id=item_create_user_id,
            create_user_name=item_create_user_name,
            create_at=solverInfo.create_at,
            price_and_operating_rate_update_at=solverInfo.price_and_operating_rate_update_at,
            price_and_operating_rate_update_by=solverInfo.price_and_operating_rate_update_by,
            update_id=item_update_user_id,
            update_user_name=item_update_user_name,
            update_at=solverInfo.update_at,
            version=solverInfo.version,
            # ファイルキーID　存在しない場合はIDを設定
            file_key_id=(
                solverInfo.file_key_id if solverInfo.file_key_id else solverInfo.id
            ),
        )
        return return_solver

    @staticmethod
    def update_solver_by_id(
        current_user: UserModel,
        solver_id: str,
        version: int,
        item: UpdateSolverByIdRequest,
    ) -> OKResponse:
        """Put /solvers/{solver_id} ソルバー情報更新API

        Args:
            current_user (Behavior, optional): 認証済みのユーザー
            solver_id (str): 個人ソルバーID パスパラメータで指定
            version (int): 楽観ロック制御
            item (UpdateSolverByIdRequest): 更新内容

        Returns:
            OKResponse: 更新後の結果
        """
        try:
            solver = SolverModel.get(hash_key=solver_id, range_key=DataType.SOLVER)
        except DoesNotExist:
            logger.warning(f"UpdateSolverById not found. solver_id: {solver_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 更新モードの確認（update_candidate、register_solver、update_solver、temporary_save_solver以外はエラー）
        if item.mode not in ["update_candidate", "register_solver", "update_solver", "temporary_save_solver"]:
            logger.warning("The mode is not appropriate")
            raise HTTPException(status_code=status.HTTP_400_BAD_REQUEST, detail="The mode is not appropriate")

        # 権限チェック
        # ログインユーザーが法人ソルバーかつ未所属の法人情報の場合はアクセス不可
        if (
            current_user.role == UserRoleType.SOLVER_STAFF.key
            and current_user.solver_corporation_id != solver.corporate_id
        ):
            logger.warning("UpdateSolverById forbidden")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 所属の法人IDとリクエストの法人IDが一致しているかチェック
        if solver.corporate_id != item.corporate_id:
            logger.warning("CorporateId Does Not Match forbidden")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 排他チェック
        if version != solver.version:
            logger.warning(
                f"UpdateSolverById conflict. request_ver: {version} solver_ver: {solver.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        files: List = []
        if item.mode == "update_solver" and solver.registration_status == "temporary_saving":
            # S3から個人ソルバー画像を取得
            if item.face_photo and item.face_photo.path:
                files.append(SolverService.get_file_from_s3(file=item.face_photo))

            # S3から添付資料を取得
            if item.resume:
                for resume in item.resume:
                    files.append(SolverService.get_file_from_s3(file=resume))
        else:
            # S3から更新したソルバー画像を取得
            if (
                solver.face_photo is None
                and item.face_photo
                and item.face_photo.path
            ) or (
                solver.face_photo
                and item.face_photo
                and item.face_photo.path
                and item.face_photo.path != solver.face_photo.path
            ):
                files.append(SolverService.get_file_from_s3(file=item.face_photo))

            # S3から更新した添付資料を取得
            if item.resume:
                for request_resume in item.resume:
                    # DBと一致する添付資料があるかを判定するフラグ
                    is_exited_resume: bool = False
                    if solver.resume is None:
                        files.append(SolverService.get_file_from_s3(file=request_resume))
                    else:
                        for model_resume in solver.resume:
                            if request_resume.path == model_resume["path"]:
                                is_exited_resume = True
                                break

                        if not is_exited_resume:
                            files.append(
                                SolverService.get_file_from_s3(file=request_resume)
                            )

        # 更新前のDB情報を保存
        original_solver = copy.deepcopy(solver)

        # ソルバー情報の更新
        update_action: List[Action] = []

        # 個人ソルバー名
        update_action.append(SolverModel.name.set(item.name))

        # 個人ソルバーかな
        update_action.append(SolverModel.name_kana.set(item.name_kana))

        # 個人ソルバー情報更新（一時保存から本登録）または
        # 個人ソルバー情報更新（一時保存）の場合
        if (
            (item.mode == "update_solver" and original_solver.registration_status == "temporary_saving")
            or item.mode == "temporary_save_solver"
        ):
            # ソルバー案件テーブルに対象の案件がない場合、新規登録
            if item.solver_application_id:
                solver_application_id = SolverService.create_solver_application(
                    item=item,
                    current_user=current_user
                )
                # 案件ID
                update_action.append(SolverModel.solver_application_ids.set([solver_application_id]))

        # 削除案件IDリスト
        if item.delete_solver_application_ids and len(item.delete_solver_application_ids) > 0:
            saved_solver_application_ids = solver.solver_application_ids if solver.solver_application_ids else []
            delete_solver_application_ids = [
                solver_application.id
                for solver_application in
                SolverService.get_solver_application_info_by_application_ids(item.delete_solver_application_ids)
            ]
            solver_application_ids = list(set(saved_solver_application_ids) - set(delete_solver_application_ids))
            # 案件ID
            update_action.append(SolverModel.solver_application_ids.set(
                solver_application_ids if solver_application_ids else None
            ))
        # 役職
        update_action.append(SolverModel.title.set(item.title))

        # 連絡先メールアドレス
        update_action.append(SolverModel.email.set(item.email))

        # 電話番号
        update_action.append(SolverModel.phone.set(item.phone))

        # 課題マップ50
        update_action.append(SolverModel.issue_map50.set(item.issue_map50 if item.issue_map50 else None))

        # 法人内ID
        update_action.append(SolverModel.corporate_id.set(item.corporate_id))

        # 性別
        update_action.append(SolverModel.sex.set(item.sex))

        # 生年月日
        if item.birth_day:
            update_action.append(SolverModel.birth_day.set(item.birth_day))

        # 稼働状況
        update_action.append(SolverModel.operating_status.set(item.operating_status))

        # 個人ソルバー画像
        if item.face_photo is not None:
            update_action.append(
                SolverModel.face_photo.set(
                    FacePhotoAttribute(
                        file_name=item.face_photo.file_name,
                        path=item.face_photo.path,
                    )
                )
            )

        # 添付資料
        if item.resume is not None:
            update_resume: List[ResumeAttribute] = []
            for resume_itr in item.resume:
                update_resume.append(
                    ResumeAttribute(
                        file_name=resume_itr.file_name,
                        path=resume_itr.path,
                    )
                )
            update_action.append(SolverModel.resume.set(update_resume))

        # 学歴
        update_action.append(
            SolverModel.academic_background.set(item.academic_background)
        )

        # 職歴
        update_action.append(SolverModel.work_history.set(item.work_history))

        # コンサルティングファーム経験有無
        update_action.append(
            SolverModel.is_consulting_firm.set(item.is_consulting_firm)
        )

        # 専門テーマ
        update_action.append(
            SolverModel.specialized_themes.set(item.specialized_themes)
        )

        # 主な実績
        update_action.append(
            SolverModel.main_achievements.set(item.main_achievements)
        )

        # ソルバー候補更新または
        # 個人ソルバー登録申請または
        # 個人ソルバー情報更新（一時保存から本登録）の場合
        # 個人ソルバー情報更新（一時保存）の場合
        if (
            item.mode == "update_candidate"
            or item.mode == "register_solver"
            or (item.mode == "update_solver" and original_solver.registration_status == "temporary_saving")
            or item.mode == "temporary_save_solver"
        ):
            # 提供稼働率（今月）
            update_action.append(
                SolverModel.provided_operating_rate.set(item.provided_operating_rate)
            )

            # 提供稼働率（来月）
            update_action.append(
                SolverModel.provided_operating_rate_next.set(item.provided_operating_rate_next)
            )

            # 再来月以降の稼働見込み
            update_action.append(
                SolverModel.operation_prospects_month_after_next.set(item.operation_prospects_month_after_next)
            )

            # 人月単価（上限）
            update_action.append(
                SolverModel.price_per_person_month.set(item.price_per_person_month)
            )

            # 人月単価（下限）
            update_action.append(
                SolverModel.price_per_person_month_lower.set(item.price_per_person_month_lower)
            )

            # 時間単価（上限）
            update_action.append(
                SolverModel.hourly_rate.set(item.hourly_rate)
            )

            # 時間単価（下限）
            update_action.append(
                SolverModel.hourly_rate_lower.set(item.hourly_rate_lower)
            )

        # 英語レベル
        update_action.append(SolverModel.english_level.set(item.english_level))

        # 東証33業種経験/対応可能領域
        update_action.append(SolverModel.tsi_areas.set(item.tsi_areas if item.tsi_areas else None))

        # 個人ソルバー登録申請または
        # 個人ソルバー情報更新（一時保存から本登録）の場合
        # 個人ソルバー情報更新（一時保存）の場合
        # ソルバー候補更新（個人ソルバー申請済み）の場合
        if (
            item.mode == "register_solver"
            or (item.mode == "update_solver" and original_solver.registration_status == "temporary_saving")
            or item.mode == "temporary_save_solver"
            or (item.mode == "update_candidate" and not original_solver.is_solver and original_solver.registration_status == "saved")
        ):
            # スクリーニング項目（評価、エビデンス）
            if item.screening_1 is not None:
                update_action.append(
                    SolverModel.screening_1.set(
                        ScreeningAttribute(
                            evaluation=item.screening_1.evaluation,
                            evidence=item.screening_1.evidence,
                        )
                    )
                )
            if item.screening_2 is not None:
                update_action.append(
                    SolverModel.screening_2.set(
                        ScreeningAttribute(
                            evaluation=item.screening_2.evaluation,
                            evidence=item.screening_2.evidence,
                        )
                    )
                )
            if item.screening_3 is not None:
                update_action.append(
                    SolverModel.screening_3.set(
                        ScreeningAttribute(
                            evaluation=item.screening_3.evaluation,
                            evidence=item.screening_3.evidence,
                        )
                    )
                )
            if item.screening_4 is not None:
                update_action.append(
                    SolverModel.screening_4.set(
                        ScreeningAttribute(
                            evaluation=item.screening_4.evaluation,
                            evidence=item.screening_4.evidence,
                        )
                    )
                )
            if item.screening_5 is not None:
                update_action.append(
                    SolverModel.screening_5.set(
                        ScreeningAttribute(
                            evaluation=item.screening_5.evaluation,
                            evidence=item.screening_5.evidence,
                        )
                    )
                )
            if item.screening_6 is not None:
                update_action.append(
                    SolverModel.screening_6.set(
                        ScreeningAttribute(
                            evaluation=item.screening_6.evaluation,
                            evidence=item.screening_6.evidence,
                        )
                    )
                )
            if item.screening_7 is not None:
                update_action.append(
                    SolverModel.screening_7.set(
                        ScreeningAttribute(
                            evaluation=item.screening_7.evaluation,
                            evidence=item.screening_7.evidence,
                        )
                    )
                )
            if item.screening_8 is not None:
                update_action.append(
                    SolverModel.screening_8.set(
                        ScreeningAttribute(
                            evaluation=item.screening_8.evaluation,
                            evidence=item.screening_8.evidence,
                        )
                    )
                )

            # クライテリア項目
            update_action.append(SolverModel.criteria_1.set(item.criteria_1))
            update_action.append(SolverModel.criteria_2.set(item.criteria_2))
            update_action.append(SolverModel.criteria_3.set(item.criteria_3))
            update_action.append(SolverModel.criteria_4.set(item.criteria_4))
            update_action.append(SolverModel.criteria_5.set(item.criteria_5))
            update_action.append(SolverModel.criteria_6.set(item.criteria_6))
            update_action.append(SolverModel.criteria_7.set(item.criteria_7))
            update_action.append(SolverModel.criteria_8.set(item.criteria_8))

        # 備考
        update_action.append(SolverModel.notes.set(item.notes))

        # 個人ソルバーか
        update_action.append(SolverModel.is_solver.set(item.is_solver))

        # 登録状態
        update_action.append(
            SolverModel.registration_status.set(item.registration_status)
        )

        # APIを実行して更新した時刻・実行者
        update_action.append(SolverModel.update_id.set(current_user.id))
        update_action.append(SolverModel.update_at.set(datetime.now()))

        solver.update(actions=update_action)

        # 個人ソルバー情報更新 （一時保存）以外の場合に連携通知
        if SolverService.is_temporary(item):
            return OKResponse()

        # ソルバー登録・更新データ連携通知（メール）
        # メール内容の編集
        logger.info("start editing mail setting")
        now = get_datetime_now()
        # 個人ソルバー情報更新（一時保存から本登録）の場合
        if item.mode == "update_solver" and original_solver.registration_status == "temporary_saving":
            email_data = SolverService.get_formatted_create_solver_email_data(
                item=item,
                solver_id=solver_id,
                current_user=current_user,
                now_datetime=get_datetime_now(),
            )

            # URLデコード
            decoded_email_data = url_decode_data(email_data)
            solver_data = json.dumps(decoded_email_data, ensure_ascii=False)

            payload = {
                "subject": SalesforceDataSyncMailType.CREATE_SOLVER,
                "solver_corporation_data": solver_data,
            }
            payload_error = {
                "error_datetime": now.strftime("%Y/%m/%d %H:%M"),
                "error_function": SalesforceDataSyncMailType.CREATE_SOLVER,
                "user_name": current_user.name,
                "data": solver_data,
            }
        else:
            email_data = SolverService.get_formatted_update_solver_email_data(
                solver_id=solver_id,
                item=item,
                original_solver=original_solver,
                current_user=current_user,
            )

            # ★URLデコード
            decoded_email_data = url_decode_data(email_data)
            solver_data = json.dumps(decoded_email_data, ensure_ascii=False)

            if item.mode == "update_candidate" or (item.mode == "register_solver" and item.registration_status == "temporary_saving"):
                payload = {
                    "subject": SalesforceDataSyncMailType.UPDATE_SOLVER_CANDIDATE,
                    "solver_corporation_data": solver_data,
                }
                payload_error = {
                    "error_datetime": now.strftime("%Y/%m/%d %H:%M"),
                    "error_function": SalesforceDataSyncMailType.UPDATE_SOLVER_CANDIDATE,
                    "user_name": current_user.name,
                    "data": solver_data,
                }
            elif item.mode == "register_solver" and item.registration_status == "saved":
                payload = {
                    "subject": SalesforceDataSyncMailType.APPLY_SOLVER,
                    "solver_corporation_data": solver_data,
                }
                payload_error = {
                    "error_datetime": now.strftime("%Y/%m/%d %H:%M"),
                    "error_function": SalesforceDataSyncMailType.APPLY_SOLVER,
                    "user_name": current_user.name,
                    "data": solver_data,
                }
            elif item.mode == "update_solver":
                payload = {
                    "subject": SalesforceDataSyncMailType.UPDATE_SOLVER,
                    "solver_corporation_data": solver_data,
                }
                payload_error = {
                    "error_datetime": now.strftime("%Y/%m/%d %H:%M"),
                    "error_function": SalesforceDataSyncMailType.UPDATE_SOLVER,
                    "user_name": current_user.name,
                    "data": solver_data,
                }

        # メールの宛先
        # TO：SalesForce、CC：システム管理者
        logger.info("start making address")
        cc_email_list: List[str] = []
        admin_filter_condition = AdminModel.disabled == False  # NOQA
        for mail_admin_itr in AdminModel.scan(filter_condition=admin_filter_condition):
            if UserRoleType.SYSTEM_ADMIN.key in mail_admin_itr.roles:
                cc_email_list.append(mail_admin_itr.email)

        # SESで直接メールを送信
        logger.info("start sending mail")
        result = SesHelper().send_mail_with_file(
            template_name=MailType.SALESFORCE_DATA_SYNC,
            to=[get_app_settings().salesforce_address_for_solver],
            cc=cc_email_list,
            payload=payload,
            payload_error=payload_error,
            files=files,
        )

        # エラーメール通知が送信された場合、ロールバックを行う
        if result and result["email_type"] == "error":
            solver.name = original_solver.name
            solver.name_kana = original_solver.name_kana
            solver.solver_application_ids = original_solver.solver_application_ids or set()
            solver.corporate_id = original_solver.corporate_id
            solver.sex = original_solver.sex
            solver.birth_day = original_solver.birth_day
            solver.email = original_solver.email
            solver.phone = original_solver.phone
            solver.english_level = original_solver.english_level
            solver.work_history = original_solver.work_history
            solver.is_consulting_firm = original_solver.is_consulting_firm
            solver.specialized_themes = original_solver.specialized_themes
            solver.academic_background = original_solver.academic_background
            solver.title = original_solver.title
            solver.face_photo = FacePhotoAttribute(
                file_name=original_solver.face_photo.file_name,
                path=original_solver.face_photo.path
            ) if original_solver.face_photo else None
            solver.resume = [
                ResumeAttribute(**resume_itr.dict()) for resume_itr in original_solver.resume
            ] if original_solver.resume else None
            solver.operating_status = original_solver.operating_status
            solver.provided_operating_rate = original_solver.provided_operating_rate
            solver.provided_operating_rate_next = original_solver.provided_operating_rate_next
            solver.operation_prospects_month_after_next = original_solver.operation_prospects_month_after_next
            solver.price_per_person_month = original_solver.price_per_person_month
            solver.price_per_person_month_lower = original_solver.price_per_person_month_lower
            solver.hourly_rate = original_solver.hourly_rate
            solver.hourly_rate_lower = original_solver.hourly_rate_lower
            solver.tsi_areas = original_solver.tsi_areas
            solver.issue_map50 = original_solver.issue_map50
            solver.main_achievements = original_solver.main_achievements
            solver.screening_1 = ScreeningAttribute(
                evaluation=original_solver.screening_1.evaluation,
                evidence=original_solver.screening_1.evidence,
            ) if original_solver.screening_1 else None
            solver.screening_2 = ScreeningAttribute(
                evaluation=original_solver.screening_2.evaluation,
                evidence=original_solver.screening_2.evidence,
            ) if original_solver.screening_2 else None
            solver.screening_3 = ScreeningAttribute(
                evaluation=original_solver.screening_3.evaluation,
                evidence=original_solver.screening_3.evidence,
            ) if original_solver.screening_3 else None
            solver.screening_4 = ScreeningAttribute(
                evaluation=original_solver.screening_4.evaluation,
                evidence=original_solver.screening_4.evidence,
            ) if original_solver.screening_4 else None
            solver.screening_5 = ScreeningAttribute(
                evaluation=original_solver.screening_5.evaluation,
                evidence=original_solver.screening_5.evidence,
            ) if original_solver.screening_5 else None
            solver.screening_6 = ScreeningAttribute(
                evaluation=original_solver.screening_6.evaluation,
                evidence=original_solver.screening_6.evidence,
            ) if original_solver.screening_6 else None
            solver.screening_7 = ScreeningAttribute(
                evaluation=original_solver.screening_7.evaluation,
                evidence=original_solver.screening_7.evidence,
            ) if original_solver.screening_7 else None
            solver.screening_8 = ScreeningAttribute(
                evaluation=original_solver.screening_8.evaluation,
                evidence=original_solver.screening_8.evidence,
            ) if original_solver.screening_8 else None
            solver.criteria_1 = original_solver.criteria_1
            solver.criteria_2 = original_solver.criteria_2
            solver.criteria_3 = original_solver.criteria_3
            solver.criteria_4 = original_solver.criteria_4
            solver.criteria_5 = original_solver.criteria_5
            solver.criteria_6 = original_solver.criteria_6
            solver.criteria_7 = original_solver.criteria_7
            solver.criteria_8 = original_solver.criteria_8
            solver.notes = original_solver.notes
            solver.is_solver = original_solver.is_solver
            solver.registration_status = original_solver.registration_status
            solver.update_id = original_solver.id
            solver.update_at = original_solver.update_at

            solver.save()

        return OKResponse()

    @staticmethod
    def is_temporary(item: UpdateSolverByIdRequest):
        if item.mode == "temporary_save_solver":
            return True
        else:
            return False

    @staticmethod
    def delete_solver_by_id(
        current_user: UserModel,
        solver_id: str,
        query_params: DeleteSolverByIdQuery,
    ):
        """個人ソルバーを削除する

        Args:
            current_user (UserModel): API実行ユーザー情報
            solver_id (str): 個人ソルバーID
            query_params (DeleteSolverByIdQuery): クエリパラメータ

        Raises:
            HTTPException: 403 Forbidden
            HTTPException: 404 Not found
            HTTPException: 409 Conflict

        Returns:
            OKResponse: 成功メッセージ
        """
        try:
            solver = SolverModel.get(hash_key=solver_id, range_key=DataType.SOLVER)
        except DoesNotExist:
            logger.warning(f"DeleteSolverById solver_id not found. solver_id: {solver_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )
        # 排他チェック
        if query_params.version != solver.version:
            logger.warning(
                f"DeleteSolverById conflict. request_ver:{query_params.version} solver_ver: {solver.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 法人ソルバーの所属先かのチェック
        if current_user.role == UserRoleType.SOLVER_STAFF.key:
            if current_user.solver_corporation_id != solver.corporate_id:
                logger.warning("DeleteSolverId forbidden.")
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )

        # 更新前のDB情報を保存
        original_solver = copy.deepcopy(solver)

        solver.delete()

        # ソルバー情報更新データ連携送信
        # メール内容の編集
        logger.info("start editing mail setting")
        solver_data = json.dumps(
            {
                "PPId__c": solver_id,
                "IsDelete__c": True,
            },
            ensure_ascii=False
        )

        # メールの宛先
        # TO：SalesForce、CC：システム管理者
        logger.info("start making address")
        cc_email_list = []
        admin_filter_condition = AdminModel.disabled == False  # NOQA
        for mail_admin_itr in AdminModel.scan(filter_condition=admin_filter_condition):
            if UserRoleType.SYSTEM_ADMIN.key in mail_admin_itr.roles:
                cc_email_list.append(mail_admin_itr.email)

        # SESで直接メールを送信
        logger.info("start sending mail")
        now = get_datetime_now()
        result = SesHelper().send_mail_with_file(
            template_name=MailType.SALESFORCE_DATA_SYNC,
            to=[get_app_settings().salesforce_address_for_solver],
            cc=cc_email_list,
            payload={
                "subject": SalesforceDataSyncMailType.UPDATE_SOLVER if solver.is_solver else SalesforceDataSyncMailType.UPDATE_SOLVER_CANDIDATE,
                "solver_corporation_data": solver_data,
            },
            payload_error={
                "error_datetime": now.strftime("%Y/%m/%d %H:%M"),
                "error_function": SalesforceDataSyncMailType.UPDATE_SOLVER if solver.is_solver else SalesforceDataSyncMailType.UPDATE_SOLVER_CANDIDATE,
                "user_name": current_user.name,
                "data": solver_data,
            },
        )

        # エラーメール通知が送信された場合、ロールバックを行う
        if result and result["email_type"] == "error":
            solver = SolverModel(
                id=original_solver.id,
                data_type=DataType.SOLVER,
                name=original_solver.name,
                name_kana=original_solver.name_kana,
                solver_application_ids=original_solver.solver_application_ids if original_solver.solver_application_ids else None,
                corporate_id=original_solver.corporate_id,
                sex=original_solver.sex,
                birth_day=original_solver.birth_day,
                email=original_solver.email,
                phone=original_solver.phone,
                english_level=original_solver.english_level,
                work_history=original_solver.work_history,
                is_consulting_firm=original_solver.is_consulting_firm,
                specialized_themes=original_solver.specialized_themes,
                academic_background=original_solver.academic_background,
                title=original_solver.title,
                face_photo=FacePhotoAttribute(
                    file_name=original_solver.face_photo.file_name,
                    path=original_solver.face_photo.path,
                ) if original_solver.face_photo else None,
                resume=[
                    ResumeAttribute(**resume_itr.dict()) for resume_itr in original_solver.resume
                ] if original_solver.resume else None,
                operating_status=original_solver.operating_status,
                provided_operating_rate=original_solver.provided_operating_rate,
                provided_operating_rate_next=original_solver.provided_operating_rate_next,
                operation_prospects_month_after_next=original_solver.operation_prospects_month_after_next,
                price_per_person_month=original_solver.price_per_person_month,
                price_per_person_month_lower=original_solver.price_per_person_month_lower,
                hourly_rate=original_solver.hourly_rate,
                hourly_rate_lower=original_solver.hourly_rate_lower,
                tsi_areas=original_solver.tsi_areas,
                issue_map50=original_solver.issue_map50,
                main_achievements=original_solver.main_achievements,
                screening_1=ScreeningAttribute(
                    evaluation=original_solver.screening_1.evaluation,
                    evidence=original_solver.screening_1.evidence,
                ) if original_solver.screening_1 else None,
                screening_2=ScreeningAttribute(
                    evaluation=original_solver.screening_2.evaluation,
                    evidence=original_solver.screening_2.evidence,
                ) if original_solver.screening_2 else None,
                screening_3=ScreeningAttribute(
                    evaluation=original_solver.screening_3.evaluation,
                    evidence=original_solver.screening_3.evidence,
                ) if original_solver.screening_3 else None,
                screening_4=ScreeningAttribute(
                    evaluation=original_solver.screening_4.evaluation,
                    evidence=original_solver.screening_4.evidence,
                ) if original_solver.screening_4 else None,
                screening_5=ScreeningAttribute(
                    evaluation=original_solver.screening_5.evaluation,
                    evidence=original_solver.screening_5.evidence,
                ) if original_solver.screening_5 else None,
                screening_6=ScreeningAttribute(
                    evaluation=original_solver.screening_6.evaluation,
                    evidence=original_solver.screening_6.evidence,
                ) if original_solver.screening_6 else None,
                screening_7=ScreeningAttribute(
                    evaluation=original_solver.screening_7.evaluation,
                    evidence=original_solver.screening_7.evidence,
                ) if original_solver.screening_7 else None,
                screening_8=ScreeningAttribute(
                    evaluation=original_solver.screening_8.evaluation,
                    evidence=original_solver.screening_8.evidence,
                ) if original_solver.screening_8 else None,
                criteria_1=original_solver.criteria_1,
                criteria_2=original_solver.criteria_2,
                criteria_3=original_solver.criteria_3,
                criteria_4=original_solver.criteria_4,
                criteria_5=original_solver.criteria_5,
                criteria_6=original_solver.criteria_6,
                criteria_7=original_solver.criteria_7,
                criteria_8=original_solver.criteria_8,
                notes=original_solver.notes,
                is_solver=original_solver.is_solver,
                registration_status=original_solver.registration_status,
                create_id=original_solver.create_id,
                create_at=original_solver.create_at,
                update_id=original_solver.update_id,
                update_at=original_solver.update_at,
            )
            solver.save()

        return OKResponse(message="OK")

    @staticmethod
    def update_solver_utilization_rate(
        item: UpdateSolverUtilizationRateRequest,
        solver_corporation_id: str,
        version: int,
        current_user: UserModel
    ):
        """稼働率・単価更新をする

        Args:
            item (UpdateSolverUtilizationRateRequest): リクエストデータ
            solver_corporation_id (str): 法人ソルバーID
            version (int): 楽観ロックキー
            current_user (UserModel): API実行ユーザー

        Raises:
            HTTPException: 400 Some solvers don't exist
            HTTPException: 403 Forbidden
            HTTPException: 409 Conflict

        Returns:
            OKResponse: 成功メッセージ
        """

        # 法人ソルバーの所属先かのチェック
        if current_user.role == UserRoleType.SOLVER_STAFF.key:
            if current_user.solver_corporation_id != solver_corporation_id:
                logger.warning("UpdateSolverUtilizationRate forbidden.")
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )

        # 排他チェック
        # 単価・稼働率が更新されるのは当APIのみのため、個人ソルバー個別で排他チェックは行わない。
        # 代わりに法人ソルバーに持っている稼働率・単価更新バージョンを利用して排他制御を行う。
        try:
            solver_corporation = SolverCorporationModel.get(
                hash_key=solver_corporation_id, range_key=DataType.SOLVER_CORPORATION
            )
        except DoesNotExist:
            logger.warning(f"UpdateSolverUtilizationRate solver_corporation_id not found. solver_corporation_id: {solver_corporation_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if version != solver_corporation.utilization_rate_version:
            logger.warning(
                f"UpdateSolverUtilizationRate conflict. request_ver:{version} solver_ver: {solver_corporation.utilization_rate_version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        now = get_datetime_now()

        deleted_solver = []
        isError = True
        for utilization_rate_info in item.utilization_rate:
            try:
                solver = SolverModel.get(hash_key=utilization_rate_info.id, range_key=DataType.SOLVER)
            except DoesNotExist:
                deleted_solver.append(utilization_rate_info.name)
                continue

            # 更新前のDB情報を保存
            original_solver = copy.deepcopy(solver)

            solver.update(
                actions=[
                    SolverModel.provided_operating_rate.set(utilization_rate_info.provided_operating_rate),
                    SolverModel.provided_operating_rate_next.set(utilization_rate_info.provided_operating_rate_next),
                    SolverModel.operation_prospects_month_after_next.set(utilization_rate_info.operation_prospects_month_after_next),
                    SolverModel.price_per_person_month.set(utilization_rate_info.price_per_person_month),
                    SolverModel.price_per_person_month_lower.set(utilization_rate_info.price_per_person_month_lower),
                    SolverModel.hourly_rate.set(utilization_rate_info.hourly_rate),
                    SolverModel.hourly_rate_lower.set(utilization_rate_info.hourly_rate_lower),
                    SolverModel.price_and_operating_rate_update_by.set(current_user.id),
                    SolverModel.price_and_operating_rate_update_at.set(now)
                ]
            )

            # Salesforceへの連携
            # メール送信
            # FIXME:　Salesforceへの連携処理を共通化したい。Salesforceへの連携項目もモデルで管理したい。
            # メール内容の編集
            logger.info("start editing mail setting")
            email_data = {
                "PPId__c": solver.id,
                "CorporateID__c": solver.corporate_id,
                "PricePerPersonMonth__c": solver.price_per_person_month,
                "PricePerPersonMonth_Lower__c": solver.price_per_person_month_lower,
                "HourlyRate__c": solver.hourly_rate,
                "HourlyRate_Lower__c": solver.hourly_rate_lower,
                "ProvidedOperatingRate__c": solver.provided_operating_rate,
                "ProvidedOperatingRate_next__c": solver.provided_operating_rate_next,
                "OperationProspects_MonthAfterNext__c": solver.operation_prospects_month_after_next,
                "NoPool": True,
                "PartnerPortalModifiedDate__c": now.strftime("%Y/%m/%d %H:%M"),
                "PartnerPortalModifiedBy__c": current_user.name
            }

            # URLデコード
            decoded_email_data = url_decode_data(email_data)
            solver_data = json.dumps(decoded_email_data, ensure_ascii=False)

            # メールの宛先
            # TO：SalesForce、CC：システム管理者
            logger.info("start making address")
            cc_email_list = []
            admin_filter_condition = AdminModel.disabled == False  # NOQA
            for mail_admin_itr in AdminModel.scan(filter_condition=admin_filter_condition):
                if UserRoleType.SYSTEM_ADMIN.key in mail_admin_itr.roles:
                    cc_email_list.append(mail_admin_itr.email)

            # SESで直接メールを送信
            logger.info("start sending mail")
            result = SesHelper().send_mail_with_file(
                template_name=MailType.SALESFORCE_DATA_SYNC,
                to=[get_app_settings().salesforce_address_for_solver],
                cc=cc_email_list,
                payload={
                    "subject": SalesforceDataSyncMailType.UPDATE_SOLVER_UTILIZATION_RATE,
                    "solver_corporation_data": solver_data,
                },
                payload_error={
                    "error_datetime": now.strftime("%Y/%m/%d %H:%M"),
                    "error_function": SalesforceDataSyncMailType.UPDATE_SOLVER_UTILIZATION_RATE,
                    "user_name": current_user.name,
                    "data": solver_data,
                },
            )

            # エラーメール通知が送信された場合、ロールバックを行う
            if result and result["email_type"] == "error":
                solver.update(
                    actions=[
                        SolverModel.provided_operating_rate.set(original_solver.provided_operating_rate),
                        SolverModel.provided_operating_rate_next.set(original_solver.provided_operating_rate_next),
                        SolverModel.operation_prospects_month_after_next.set(original_solver.operation_prospects_month_after_next),
                        SolverModel.price_per_person_month.set(original_solver.price_per_person_month),
                        SolverModel.price_per_person_month_lower.set(original_solver.price_per_person_month_lower),
                        SolverModel.hourly_rate.set(original_solver.hourly_rate),
                        SolverModel.hourly_rate_lower.set(original_solver.hourly_rate_lower),
                        SolverModel.price_and_operating_rate_update_by.set(original_solver.price_and_operating_rate_update_by),
                        SolverModel.price_and_operating_rate_update_at.set(original_solver.price_and_operating_rate_update_at),
                    ]
                )
            else:
                isError = False

        if not isError:
            solver_corporation.update(
                actions=[
                    SolverCorporationModel.price_and_operating_rate_update_by.set(current_user.id),
                    SolverCorporationModel.price_and_operating_rate_update_at.set(now),
                    SolverCorporationModel.utilization_rate_version.set(version + 1)
                ]
            )

        return UpdateSolverUtilizationRateResponse(deleted=deleted_solver)

    @staticmethod
    def patch_solver_status_by_id(
        current_user: UserModel, solver_id: str, item: PatchSolverStatusByIdQuery
    ) -> OKResponse:
        """ソルバー候補から個人ソルバーに変更する。
        Args:
            current_user (Behavior, optional): 認証済みのユーザー
            item (PatchSolverStatusByIdQuery): 更新内容
        Returns:
            OKResponse: 更新結果
        """

        try:
            solver = SolverModel.get(hash_key=solver_id, range_key=DataType.SOLVER)
        except DoesNotExist:
            logger.warning(
                f"PatchSolverStatusById solver_id not found. solver_id: {solver_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if (
            current_user.role == UserRoleType.SOLVER_STAFF.key
            and current_user.solver_corporation_id != solver.corporate_id
        ):
            # 所属していない法人のソルバーはアクセス不可
            logger.warning("PatchSolverStatusById forbidden.")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        if item.version != solver.version:
            logger.warning(
                f"PatchSolverStatusById conflict. request_ver:{item.version} solver_ver: {solver.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 対象のソルバーが個人ソルバーの場合
        if solver.is_solver:
            solver.update(
                actions=[
                    SolverModel.price_and_operating_rate_update_at.set(datetime.now()),
                    SolverModel.price_and_operating_rate_update_by.set(current_user.id),
                    SolverModel.registration_status.set("certificated"),
                    SolverModel.update_id.set(current_user.id),
                    SolverModel.update_at.set(datetime.now()),
                ]
            )
        # 対象のソルバーがソルバー候補の場合
        else:
            solver.update(
                actions=[
                    SolverModel.price_and_operating_rate_update_at.set(datetime.now()),
                    SolverModel.price_and_operating_rate_update_by.set(current_user.id),
                    SolverModel.is_solver.set(True),
                    SolverModel.registration_status.set("certificated"),
                    SolverModel.update_id.set(current_user.id),
                    SolverModel.update_at.set(datetime.now()),
                ]
            )

        return OKResponse()

    @staticmethod
    def create_project_code(id: str, name: str) -> str:
        """案件IDと案件名から案件コードを作成する
        Args:
            id (str): 案件ID
            name (str): 案件名
        Returns:
            案件コード:
        """
        # 案件IDの文字列を逆順にする
        reversed_id: str = id[::-1]

        # 作成した文字列と案件名をSHA3-256ハッシュにかけてフレーズを作成する
        hash_object: hashlib._Hash = hashlib.sha3_256((reversed_id + name).encode())
        phrase = hash_object.hexdigest()

        # 案件IDと案件名とフレーズをセミコロン（;）で結合した上でBase64URLエンコードにかける
        target_text: str = ';'.join([id, name, phrase])
        encoded_bytes: str = base64.b64encode(target_text.encode('utf-8'))

        # Base64URLエンコードの変換
        project_code: str = encoded_bytes.decode('utf-8')
        return (
            project_code
            .rstrip('=')
            .replace('+', '-')
            .replace('/', '_')
        )
