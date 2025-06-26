import datetime
import copy
import botocore.exceptions
import json
from typing import List, Union

from app.auth.auth import AuthUser
from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.user import UserModel
from app.models.admin import AdminModel
from app.models.solver_corporation import (
    SolverCorporationModel,
    ValueAndMemoAttribute,
    AddressAttribute,
    CorporatePhotoAttribute,
    CorporateInfoDocumentAttribute,
    MainChargeAttribute,
    DeputyChargeAttribute,
    OtherChargeAttribute,
)
from app.resources.const import (
    DataType,
    MailType,
    SalesforceDataSyncMailType,
    UserRoleType,
    GetSolverCorporationsSortType,
)
from app.schemas.base import OKResponse
from app.schemas.solver_corporation import (
    Address,
    CorporateInfoDocument,
    CorporatePhoto,
    DeputyCharge,
    GetSolverCorporationsResponse,
    GetSolverCorporationByIdResponse,
    MainCharge,
    OtherCharge,
    SolverCorporationInfoForGetSolverCorporations,
    UpdateSolverCorporationByIdRequest,
    ValueAndMemo,
)
from app.service.master_service import MasterService
from app.service.common_service.request_processor import url_decode_data
from app.utils.aws.s3 import S3Helper
from app.utils.aws.ses import SesHelper
from app.utils.date import get_datetime_now

from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.update import Action

logger = CustomLogger.get_logger()


class SolverCorporationService:
    @staticmethod
    def get_formatted_solver_corporation_email_data(
        solver_corporation_id: str,
        item: UpdateSolverCorporationByIdRequest,
        original_solver_corporation: SolverCorporationModel,
        current_user: UserModel,
    ) -> dict:
        """法人ソルバーの更新対象の項目をメール用にデータ整形し取得
        Args:
            solver_corporation_id (str): 法人ソルバーID
            item (UpdateSolverCorporationByIdRequest): 法人ソルバー更新リクエストデータ
            original_solver_corporation (SolverCorporationModel): 法人ソルバー情報
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            solver_corporation_email_data（dict): 更新対象の項目
        """
        solver_corporation_email_data: dict = {}

        # PPID（常に連携）
        solver_corporation_email_data.update({"PPId__c": solver_corporation_id})

        # 法人ソルバー名
        # 初回更新の場合、データが更新されているかに関係なくSFに連携
        if original_solver_corporation.update_at is None:
            solver_corporation_email_data.update({"Name": item.name})
        elif item.name != original_solver_corporation.name:
            solver_corporation_email_data.update({"Name": item.name})

        # 企業名略称
        if item.company_abbreviation != original_solver_corporation.company_abbreviation:
            solver_corporation_email_data.update({"CompanyAbbreviation__c": item.company_abbreviation})

        # 業種
        if item.industry != original_solver_corporation.industry:
            solver_corporation_email_data.update({"Industry__c": item.industry})

        # 設立
        if item.established != original_solver_corporation.established:
            solver_corporation_email_data.update({"Established__c": item.established})

        # 経営陣
        if item.management_team != original_solver_corporation.management_team:
            solver_corporation_email_data.update({"ManagementTeam__c": item.management_team})

        # 従業員数
        if (
            original_solver_corporation.employee is None
            or item.employee.value != original_solver_corporation.employee.value
        ):
            solver_corporation_email_data.update({"Employee__c": item.employee.value})

        # 従業員数メモ
        if (
            (
                original_solver_corporation.employee is None
                or original_solver_corporation.employee.memo is None
            )
            and item.employee.memo
        ) or (
            original_solver_corporation.employee
            and item.employee.memo != original_solver_corporation.employee.memo
        ):
            solver_corporation_email_data.update({"EmployeeMemo__c": item.employee.memo})

        # 資本金
        if (
            original_solver_corporation.capital is None
            or item.capital.value != original_solver_corporation.capital.value
        ):
            solver_corporation_email_data.update({"Capital__c": item.capital.value})

        # 資本金メモ
        if (
            (
                original_solver_corporation.capital is None
                or original_solver_corporation.capital.memo is None
            )
            and item.capital.memo
        ) or (
            original_solver_corporation.capital
            and item.capital.memo != original_solver_corporation.capital.memo
        ):
            solver_corporation_email_data.update({"CapitalMemo__c": item.capital.memo})

        # 売上
        if (
            original_solver_corporation.earnings is None
            or item.earnings.value != original_solver_corporation.earnings.value
        ):
            solver_corporation_email_data.update({"Earnings__c": item.earnings.value})

        # 売上メモ
        if (
            (
                original_solver_corporation.earnings is None
                or original_solver_corporation.earnings.memo is None
            )
            and item.earnings.memo
        ) or (
            original_solver_corporation.earnings
            and item.earnings.memo != original_solver_corporation.earnings.memo
        ):
            solver_corporation_email_data.update({"EarningsMemo__c": item.earnings.memo})

        # 上場取引所
        if item.listing_exchange != original_solver_corporation.listing_exchange:
            solver_corporation_email_data.update({"ListingExchange__c": item.listing_exchange})

        # 事業内容
        if item.business_content != original_solver_corporation.business_content:
            solver_corporation_email_data.update({"BusinessContent__c": item.business_content})

        # 郵便番号
        if (
            original_solver_corporation.address is None
            or item.address.postal_code != original_solver_corporation.address.postal_code
        ):
            solver_corporation_email_data.update({"PostalCode__c": item.address.postal_code})

        # 都道府県
        if (
            original_solver_corporation.address is None
            or item.address.state != original_solver_corporation.address.state
        ):
            solver_corporation_email_data.update({"State__c": item.address.state})

        # 市区郡
        if (
            original_solver_corporation.address is None
            or item.address.city != original_solver_corporation.address.city
        ):
            solver_corporation_email_data.update({"City__c": item.address.city})

        # 町名・番地
        if (
            original_solver_corporation.address is None
            or item.address.street != original_solver_corporation.address.street
        ):
            solver_corporation_email_data.update({"Street__c": item.address.street})

        # 建物名
        if (
            original_solver_corporation.address is None
            or item.address.building != original_solver_corporation.address.building
        ):
            solver_corporation_email_data.update({"Building__c": item.address.building})

        # 削除された法人ソルバー画像および会社案件資料がある場合、削除ファイル名を記録
        # 削除ファイルが複数ある場合、セミコロンで区切る
        deleted_corporate_info_document: List = []
        # 法人ソルバー画像
        if (
            (
                original_solver_corporation.corporate_photo
                and original_solver_corporation.corporate_photo.path is not None
            ) and (
                not item.corporate_photo
                or original_solver_corporation.corporate_photo.path != item.corporate_photo.path
            )
        ):
            deleted_corporate_info_document.append(original_solver_corporation.corporate_photo["file_name"])
        # 会社案内資料
        if original_solver_corporation.corporate_info_document:
            if not item.corporate_info_document:
                for model_corporate_info_document in original_solver_corporation.corporate_info_document:
                    deleted_corporate_info_document.append(model_corporate_info_document["file_name"])
            else:
                for model_corporate_info_document in original_solver_corporation.corporate_info_document:
                    # DBと一致する会社案件資料があるかを判定するフラグ
                    is_existed_corporate_info_document: bool = False
                    for request_corporate_info_document in item.corporate_info_document:
                        if model_corporate_info_document.path == request_corporate_info_document.path:
                            is_existed_corporate_info_document = True
                            break
                    if not is_existed_corporate_info_document:
                        deleted_corporate_info_document.append(model_corporate_info_document["file_name"])
        if deleted_corporate_info_document:
            solver_corporation_email_data.update({"DeleteFileNames__c": ";delimiter".join(deleted_corporate_info_document)})

        # 課題マップ50（選択値を全て連携）
        # 選択値が複数ある場合、セミコロンで区切る
        issue_map50_name: List[str] = MasterService.get_issue_map50_name(item.issue_map50)
        solver_corporation_email_data.update({"IssueMap50__c": ";".join(issue_map50_name) if issue_map50_name else ""})

        # ビジョン
        if item.vision != original_solver_corporation.vision:
            solver_corporation_email_data.update({"Vision__c": item.vision})

        # ミッション
        if item.mission != original_solver_corporation.mission:
            solver_corporation_email_data.update({"Mission__c": item.mission})

        # 主担当者（名前）
        if (
            original_solver_corporation.main_charge is None
            or item.main_charge.name != original_solver_corporation.main_charge.name
        ):
            solver_corporation_email_data.update({"MainCharge__c": item.main_charge.name})

        # 主担当者（ふりがな）
        if (
            original_solver_corporation.main_charge is None
            or item.main_charge.kana != original_solver_corporation.main_charge.kana
        ):
            solver_corporation_email_data.update({"MainChargeKana__c": item.main_charge.kana})

        # 主担当者（役職）
        if (
            original_solver_corporation.main_charge is None
            or item.main_charge.title != original_solver_corporation.main_charge.title
        ):
            solver_corporation_email_data.update({"MC_Title__c": item.main_charge.title})

        # 主担当者（連絡先メアド）
        if (
            original_solver_corporation.main_charge is None
            or item.main_charge.email != original_solver_corporation.main_charge.email
        ):
            solver_corporation_email_data.update({"MC_Email__c": item.main_charge.email})

        # 主担当者（部署名）
        if (
            original_solver_corporation.main_charge is None
            or item.main_charge.department != original_solver_corporation.main_charge.department
        ):
            solver_corporation_email_data.update({"MC_Department__c": item.main_charge.department})

        # 主担当者（電話番号）
        if (
            original_solver_corporation.main_charge is None
            or item.main_charge.phone != original_solver_corporation.main_charge.phone
        ):
            solver_corporation_email_data.update({"MC_Phone__c": item.main_charge.phone})

        # 副担当者（名前）
        if (
            (
                original_solver_corporation.deputy_charge is None
                or original_solver_corporation.deputy_charge.name is None
            )
            and item.deputy_charge.name
        ) or (
            original_solver_corporation.deputy_charge
            and item.deputy_charge.name != original_solver_corporation.deputy_charge.name
        ):
            solver_corporation_email_data.update({"DeputyCharge__c": item.deputy_charge.name})

        # 副担当者（ふりがな）
        if (
            (
                original_solver_corporation.deputy_charge is None
                or original_solver_corporation.deputy_charge.kana is None
            )
            and item.deputy_charge.kana
        ) or (
            original_solver_corporation.deputy_charge
            and item.deputy_charge.kana != original_solver_corporation.deputy_charge.kana
        ):
            solver_corporation_email_data.update({"DeputyChargeKana__c": item.deputy_charge.kana})

        # 副担当者（役職）
        if (
            (
                original_solver_corporation.deputy_charge is None
                or original_solver_corporation.deputy_charge.title is None
            )
            and item.deputy_charge.title
        ) or (
            original_solver_corporation.deputy_charge
            and item.deputy_charge.title != original_solver_corporation.deputy_charge.title
        ):
            solver_corporation_email_data.update({"DC_Title__c": item.deputy_charge.title})

        # 副担当者（連絡先メアド）
        if (
            (
                original_solver_corporation.deputy_charge is None
                or original_solver_corporation.deputy_charge.email is None
            )
            and item.deputy_charge.email
        ) or (
            original_solver_corporation.deputy_charge
            and item.deputy_charge.email != original_solver_corporation.deputy_charge.email
        ):
            solver_corporation_email_data.update({"DC_Email__c": item.deputy_charge.email})

        # 副担当者（部署名）
        if (
            (
                original_solver_corporation.deputy_charge is None
                or original_solver_corporation.deputy_charge.department is None
            )
            and item.deputy_charge.department
        ) or (
            original_solver_corporation.deputy_charge
            and item.deputy_charge.department != original_solver_corporation.deputy_charge.department
        ):
            solver_corporation_email_data.update({"DC_Department__c": item.deputy_charge.department})

        # 副担当者（電話番号）
        if (
            (
                original_solver_corporation.deputy_charge is None
                or original_solver_corporation.deputy_charge.phone is None
            )
            and item.deputy_charge.phone
        ) or (
            original_solver_corporation.deputy_charge
            and item.deputy_charge.phone != original_solver_corporation.deputy_charge.phone
        ):
            solver_corporation_email_data.update({"DC_Phone__c": item.deputy_charge.phone})

        # その他担当者（名前）
        if (
            (
                original_solver_corporation.other_charge is None
                or original_solver_corporation.other_charge.name is None
            )
            and item.other_charge.name
        ) or (
            original_solver_corporation.other_charge
            and item.other_charge.name != original_solver_corporation.other_charge.name
        ):
            solver_corporation_email_data.update({"OtherCharge__c": item.other_charge.name})

        # その他担当者（役職）
        if (
            (
                original_solver_corporation.other_charge is None
                or original_solver_corporation.other_charge.title is None
            )
            and item.other_charge.title
        ) or (
            original_solver_corporation.other_charge
            and item.other_charge.title != original_solver_corporation.other_charge.title
        ):
            solver_corporation_email_data.update({"OC_Title__c": item.other_charge.title})

        # その他担当者（連絡先メアド）
        if (
            (
                original_solver_corporation.other_charge is None
                or original_solver_corporation.other_charge.email is None
            )
            and item.other_charge.email
        ) or (
            original_solver_corporation.other_charge
            and item.other_charge.email != original_solver_corporation.other_charge.email
        ):
            solver_corporation_email_data.update({"OC_Email__c": item.other_charge.email})

        # その他担当者（部署名）
        if (
            (
                original_solver_corporation.other_charge is None
                or original_solver_corporation.other_charge.department is None
            )
            and item.other_charge.department
        ) or (
            original_solver_corporation.other_charge
            and item.other_charge.department != original_solver_corporation.other_charge.department
        ):
            solver_corporation_email_data.update({"OC_Department__c": item.other_charge.department})

        # その他担当者（電話番号）
        if (
            (
                original_solver_corporation.other_charge is None
                or original_solver_corporation.other_charge.phone is None
            )
            and item.other_charge.phone
        ) or (
            original_solver_corporation.other_charge
            and item.other_charge.phone != original_solver_corporation.other_charge.phone
        ):
            solver_corporation_email_data.update({"OC_Phone__c": item.other_charge.phone})

        # 備考
        if item.notes != original_solver_corporation.notes:
            solver_corporation_email_data.update({"Notes__c": item.notes})

        now = get_datetime_now()
        # 削除フラグ・作成日時・作成者
        # 初回更新の場合、データが更新されているかに関係なくSFに連携
        if original_solver_corporation.update_at is None:
            solver_corporation_email_data.update({
                "IsDelete__c": False,
                "PartnerPortalCreatedDate__c": now.strftime("%Y/%m/%d %H:%M"),
                "PartnerPortalCreatedBy__c": AdminModel.get_update_user_name(original_solver_corporation.create_id),
            })

        # 更新日時
        solver_corporation_email_data.update({"PartnerPortalModifiedDate__c": now.strftime("%Y/%m/%d %H:%M")})

        # 更新者
        solver_corporation_email_data.update({"PartnerPortalModifiedBy__c": current_user.name})

        return solver_corporation_email_data

    @staticmethod
    def get_file_from_s3(
        file: Union[CorporatePhoto, CorporateInfoDocument],
    ) -> dict:
        """ファイルをS3から取得

        Args:
            file (Union[CorporatePhoto, CorporateInfoDocument]): ファイル
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
    def update_solver_corporation_table(
        item: Union[GetSolverCorporationByIdResponse, UpdateSolverCorporationByIdRequest],
        current_user: UserModel,
    ) -> List[Action]:
        """法人ソルバーテーブルを更新
        Args:
            item (UpdateSolverCorporationById): 更新内容
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            update_action (List[Action])
        """

        update_action: List[Action] = []

        # 法人ソルバー名
        update_action.append(SolverCorporationModel.name.set(item.name))

        # 企業名略称
        update_action.append(
            SolverCorporationModel.company_abbreviation.set(item.company_abbreviation)
        )

        # 業種
        update_action.append(SolverCorporationModel.industry.set(item.industry))

        # 設立
        update_action.append(SolverCorporationModel.established.set(item.established))

        # 経営陣
        update_action.append(
            SolverCorporationModel.management_team.set(item.management_team)
        )

        # 従業員数、メモ
        if item.employee is not None:
            update_action.append(
                SolverCorporationModel.employee.set(
                    ValueAndMemoAttribute(
                        value=item.employee.value,
                        memo=item.employee.memo,
                    )
                )
            )

        # 資本金、メモ
        if item.capital is not None:
            update_action.append(
                SolverCorporationModel.capital.set(
                    ValueAndMemoAttribute(
                        value=item.capital.value,
                        memo=item.capital.memo,
                    )
                )
            )

        # 売上、メモ
        if item.earnings is not None:
            update_action.append(
                SolverCorporationModel.earnings.set(
                    ValueAndMemoAttribute(
                        value=item.earnings.value,
                        memo=item.earnings.memo,
                    )
                )
            )

        # 上場取引所
        update_action.append(
            SolverCorporationModel.listing_exchange.set(item.listing_exchange)
        )

        # 事業内容
        update_action.append(
            SolverCorporationModel.business_content.set(item.business_content)
        )

        # 住所
        if item.address is not None:
            update_action.append(
                SolverCorporationModel.address.set(
                    AddressAttribute(
                        postal_code=item.address.postal_code,
                        state=item.address.state,
                        city=item.address.city,
                        street=item.address.street,
                        building=item.address.building,
                    )
                )
            )

        # 法人ソルバー画像
        if item.corporate_photo is not None:
            update_action.append(
                SolverCorporationModel.corporate_photo.set(
                    CorporatePhotoAttribute(
                        file_name=item.corporate_photo.file_name,
                        path=item.corporate_photo.path,
                    )
                )
            )

        # 会社案内資料
        if item.corporate_info_document is not None:
            update_documents: List[CorporateInfoDocumentAttribute] = []
            for documents_itr in item.corporate_info_document:
                update_documents.append(
                    CorporateInfoDocumentAttribute(
                        file_name=documents_itr.file_name,
                        path=documents_itr.path,
                    )
                )
            update_action.append(
                SolverCorporationModel.corporate_info_document.set(update_documents)
            )

        # 課題マップ50
        update_action.append(SolverCorporationModel.issue_map50.set(item.issue_map50))

        # ビジョン
        update_action.append(SolverCorporationModel.vision.set(item.vision))

        # ミッション
        update_action.append(SolverCorporationModel.mission.set(item.mission))

        # 主担当者
        if item.main_charge is not None:
            update_action.append(
                SolverCorporationModel.main_charge.set(
                    MainChargeAttribute(
                        name=item.main_charge.name,
                        kana=item.main_charge.kana,
                        title=item.main_charge.title,
                        email=item.main_charge.email,
                        department=item.main_charge.department,
                        phone=item.main_charge.phone,
                    )
                )
            )

        # 副担当者
        if item.deputy_charge is not None:
            update_action.append(
                SolverCorporationModel.deputy_charge.set(
                    DeputyChargeAttribute(
                        name=item.deputy_charge.name,
                        kana=item.deputy_charge.kana,
                        title=item.deputy_charge.title,
                        email=item.deputy_charge.email,
                        department=item.deputy_charge.department,
                        phone=item.deputy_charge.phone,
                    )
                )
            )

        # その他担当者
        if item.other_charge is not None:
            update_action.append(
                SolverCorporationModel.other_charge.set(
                    OtherChargeAttribute(
                        name=item.other_charge.name,
                        title=item.other_charge.title,
                        email=item.other_charge.email,
                        department=item.other_charge.department,
                        phone=item.other_charge.phone,
                    )
                )
            )

        # 備考
        update_action.append(SolverCorporationModel.notes.set(item.notes))

        # APIを実行して更新した時刻・実行者
        if hasattr(item, "update_id"):
            update_action.append(SolverCorporationModel.update_id.set(item.update_id))
            update_action.append(SolverCorporationModel.update_at.set(item.update_at))
        else:
            update_action.append(SolverCorporationModel.update_id.set(current_user.id))
            update_action.append(SolverCorporationModel.update_at.set(datetime.datetime.now()))

        return update_action

    @staticmethod
    def get_solver_corporations(
        disabled: bool,
        sort: GetSolverCorporationsSortType,
        current_user: AuthUser,
    ) -> GetSolverCorporationsResponse:
        """Get /solver-corporations 法人ソルバー一覧取得API
        Args:
            disabled (bool): trueならば無効の法人ソルバーのみを取得 クエリパラメータで指定
            sort (GetSolverCorporationsSortType): ソート
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            GetSolverCorporationsResponse: 取得結果
        """
        # GSIソート順(name)
        scan_index_forward = None
        if sort == GetSolverCorporationsSortType.NAME_ASC:
            scan_index_forward = True
        elif sort == GetSolverCorporationsSortType.NAME_DESC:
            scan_index_forward = False

        # クエリ条件を指定
        filter_condition = None
        # falseの場合は無効化の法人は取得しない
        filter_condition &= SolverCorporationModel.disabled == disabled

        # 法人ソルバー情報取得
        result_list: List[SolverCorporationModel] = list(
            SolverCorporationModel.data_type_name_index.query(
                hash_key=DataType.SOLVER_CORPORATION,
                filter_condition=filter_condition,
                scan_index_forward=scan_index_forward,
            )
        )

        # 取得した法人ソルバー情報をリストに格納
        solver_corporation_info_list: List[
            SolverCorporationInfoForGetSolverCorporations
        ] = [
            SolverCorporationInfoForGetSolverCorporations(
                **solver_corporation.attribute_values
            )
            for solver_corporation in result_list
        ]

        return GetSolverCorporationsResponse(
            solverCorporations=solver_corporation_info_list
        )

    @staticmethod
    def get_solver_corporation_by_id(
        current_user: UserModel, solver_corporation_id: str
    ) -> GetSolverCorporationByIdResponse:
        """Get /solver-corporations/{solver_corporation_id} 法人ソルバー情報取得API

        Args:
            solver_corporation_id (str): 法人ソルバーID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetSolverCorporationByIdResponse:
        """

        try:
            solverCorporationInfo = SolverCorporationModel.get(
                hash_key=solver_corporation_id, range_key=DataType.SOLVER_CORPORATION
            )
        except DoesNotExist:
            logger.warning(
                f"GetSolverCorporationById solver_corporation_id not found. solver_corporation_id: {solver_corporation_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if current_user.role == UserRoleType.SOLVER_STAFF.key:
            if current_user.solver_corporation_id != solverCorporationInfo.id:
                # 所属していない法人の法人情報はアクセス不可
                logger.warning("GetSolverCorporationById forbidden.")
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )

        # DBから取得した値をNULL検証して、レスポンス整形する
        item_create_user_id: str = ""
        item_create_user_name: str = ""
        item_update_user_id: str = ""
        item_update_user_name: str = ""
        item_corporate_info_document_list: List[CorporateInfoDocument] = []

        # 従業員数
        if solverCorporationInfo.employee:
            solverCorporationInfo.employee = ValueAndMemo(
                value=solverCorporationInfo.employee.value,
                memo=solverCorporationInfo.employee.memo,
            )

        # 資本金
        if solverCorporationInfo.capital:
            solverCorporationInfo.capital = ValueAndMemo(
                value=solverCorporationInfo.capital.value,
                memo=solverCorporationInfo.capital.memo,
            )

        # 売上
        if solverCorporationInfo.earnings:
            solverCorporationInfo.earnings = ValueAndMemo(
                value=solverCorporationInfo.earnings.value,
                memo=solverCorporationInfo.earnings.memo,
            )

        # 住所
        if solverCorporationInfo.address:
            solverCorporationInfo.address = Address(
                postal_code=solverCorporationInfo.address.postal_code,
                state=solverCorporationInfo.address.state,
                city=solverCorporationInfo.address.city,
                street=solverCorporationInfo.address.street,
                building=solverCorporationInfo.address.building,
            )

        # 法人ソルバー画像
        if solverCorporationInfo.corporate_photo:
            solverCorporationInfo.corporate_photo = CorporatePhoto(
                file_name=solverCorporationInfo.corporate_photo.file_name,
                path=solverCorporationInfo.corporate_photo.path,
            )

        # 会社案内資料
        if solverCorporationInfo.corporate_info_document is not None:
            for (
                corporate_info_document_itr
            ) in solverCorporationInfo.corporate_info_document:
                item_corporate_info_document_list.append(
                    CorporateInfoDocument(
                        file_name=corporate_info_document_itr.file_name,
                        path=corporate_info_document_itr.path,
                    )
                )

        # 主担当者
        if solverCorporationInfo.main_charge:
            solverCorporationInfo.main_charge = MainCharge(
                name=solverCorporationInfo.main_charge.name,
                kana=solverCorporationInfo.main_charge.kana,
                title=solverCorporationInfo.main_charge.title,
                email=solverCorporationInfo.main_charge.email,
                department=solverCorporationInfo.main_charge.department,
                phone=solverCorporationInfo.main_charge.phone,
            )

        # 副担当者
        if solverCorporationInfo.deputy_charge:
            solverCorporationInfo.deputy_charge = DeputyCharge(
                name=solverCorporationInfo.deputy_charge.name,
                kana=solverCorporationInfo.deputy_charge.kana,
                title=solverCorporationInfo.deputy_charge.title,
                email=solverCorporationInfo.deputy_charge.email,
                department=solverCorporationInfo.deputy_charge.department,
                phone=solverCorporationInfo.deputy_charge.phone,
            )

        # その他担当者
        if solverCorporationInfo.other_charge:
            solverCorporationInfo.other_charge = OtherCharge(
                name=solverCorporationInfo.other_charge.name,
                title=solverCorporationInfo.other_charge.title,
                email=solverCorporationInfo.other_charge.email,
                department=solverCorporationInfo.other_charge.department,
                phone=solverCorporationInfo.other_charge.phone,
            )

        # 登録者名と最終更新者名を取得
        if solverCorporationInfo.create_id is not None:
            item_create_user_id = solverCorporationInfo.create_id
            item_create_user_name = (
                AdminModel.get_update_user_name(solverCorporationInfo.create_id)
            )
        if solverCorporationInfo.update_id is not None:
            item_update_user_id = solverCorporationInfo.update_id
            item_update_user_name = (
                UserModel.get_update_user_name(solverCorporationInfo.update_id)
            )

        # レスポンス作成
        return_solver_corporation = GetSolverCorporationByIdResponse(
            id=solverCorporationInfo.id,
            name=solverCorporationInfo.name,
            company_abbreviation=solverCorporationInfo.company_abbreviation,
            industry=solverCorporationInfo.industry,
            established=solverCorporationInfo.established,
            management_team=solverCorporationInfo.management_team,
            employee=solverCorporationInfo.employee,
            capital=solverCorporationInfo.capital,
            earnings=solverCorporationInfo.earnings,
            listing_exchange=solverCorporationInfo.listing_exchange,
            business_content=solverCorporationInfo.business_content,
            address=solverCorporationInfo.address,
            corporate_photo=solverCorporationInfo.corporate_photo,
            corporate_info_document=item_corporate_info_document_list,
            issue_map50=solverCorporationInfo.issue_map50,
            vision=solverCorporationInfo.vision,
            mission=solverCorporationInfo.mission,
            main_charge=solverCorporationInfo.main_charge,
            deputy_charge=solverCorporationInfo.deputy_charge,
            other_charge=solverCorporationInfo.other_charge,
            notes=solverCorporationInfo.notes,
            disabled=solverCorporationInfo.disabled,
            create_id=item_create_user_id,
            create_user_name=item_create_user_name,
            create_at=solverCorporationInfo.create_at,
            price_and_operating_rate_update_at=solverCorporationInfo.price_and_operating_rate_update_at,
            price_and_operating_rate_update_by=solverCorporationInfo.price_and_operating_rate_update_by,
            update_id=item_update_user_id,
            update_user_name=item_update_user_name,
            update_at=solverCorporationInfo.update_at,
            version=solverCorporationInfo.version,
            utilization_rate_version=solverCorporationInfo.utilization_rate_version
        )
        return return_solver_corporation

    @staticmethod
    def update_solver_corporation_by_id(
        current_user: UserModel,
        solver_corporation_id: str,
        version: int,
        item: UpdateSolverCorporationByIdRequest,
    ) -> OKResponse:
        """Put /solvers/corporations/{solver_corporation_id} 法人ソルバー登録内容更新API

        Args:
            current_user (Behavior, optional): 認証済みのユーザー
            solver_corporation_id (str): 法人ソルバーID パスパラメータで指定
            item (UpdateSolverCorporationByIdRequest): 更新内容

        Returns:
            OKResponse: 更新後の取得結果
        """

        try:
            solver_corporation = SolverCorporationModel.get(
                hash_key=solver_corporation_id, range_key=DataType.SOLVER_CORPORATION
            )
        except DoesNotExist:
            logger.warning(
                f"UpdateSolverCorporationById not found. solver_corporation_id: {solver_corporation_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 権限チェック
        # 法人ソルバーかつ未所属の法人情報の場合はアクセス不可
        if (
            current_user.role == UserRoleType.SOLVER_STAFF.key
            and current_user.solver_corporation_id != solver_corporation_id
        ):
            logger.warning("UpdateSolverCorporationById forbidden")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 排他チェック
        if version != solver_corporation.version:
            logger.warning(
                f"UpdateSolverCorporationById conflict. request_ver: {version} solver_corporation_ver: {solver_corporation.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        files: List = []
        # S3から更新した法人ソルバー画像を取得
        if (
            solver_corporation.corporate_photo is None
            and item.corporate_photo
            and item.corporate_photo.path
        ) or (
            solver_corporation.corporate_photo
            and item.corporate_photo
            and item.corporate_photo.path
            and item.corporate_photo.path != solver_corporation.corporate_photo.path
        ):
            files.append(SolverCorporationService.get_file_from_s3(file=item.corporate_photo))

        # S3から更新した会社案件資料を取得
        if item.corporate_info_document:
            for request_corporate_info_document in item.corporate_info_document:
                # DBと一致する会社案件資料があるかを判定するフラグ
                is_exited_corporate_info_document: bool = False
                if solver_corporation.corporate_info_document is None:
                    files.append(SolverCorporationService.get_file_from_s3(file=request_corporate_info_document))
                else:
                    for model_corporate_info_document in solver_corporation.corporate_info_document:
                        if (
                            request_corporate_info_document.path == model_corporate_info_document["path"]
                        ):
                            is_exited_corporate_info_document = True
                            break

                    if not is_exited_corporate_info_document:
                        files.append(SolverCorporationService.get_file_from_s3(file=request_corporate_info_document))

        # 更新前のDB情報を保存
        original_solver_corporation = copy.deepcopy(solver_corporation)

        # 法人ソルバー情報の更新
        update_action = SolverCorporationService.update_solver_corporation_table(item=item, current_user=current_user)
        solver_corporation.update(actions=update_action)

        # 法人情報登録・更新データ連携通知（メール）
        # メール内容の編集
        # 更新した項目のみSalesForceに連携
        logger.info("start editing mail setting")
        solver_corporation_email_data = SolverCorporationService.get_formatted_solver_corporation_email_data(
            solver_corporation_id=solver_corporation_id,
            item=item,
            original_solver_corporation=original_solver_corporation,
            current_user=current_user,
        )

        # URLデコードを適用
        decoded_email_data = url_decode_data(solver_corporation_email_data)

        # JSON文字列化
        solver_corporation_data = json.dumps(decoded_email_data, ensure_ascii=False)

        payload = {
            "subject": SalesforceDataSyncMailType.CREATE_SOLVER_CORPORATION if original_solver_corporation.update_at is None else SalesforceDataSyncMailType.UPDATE_SOLVER_CORPORATION,
            "solver_corporation_data": solver_corporation_data,
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
        now = get_datetime_now()
        result = SesHelper().send_mail_with_file(
            template_name=MailType.SALESFORCE_DATA_SYNC,
            to=[get_app_settings().salesforce_address_for_solver_corporation],
            cc=cc_email_list,
            payload=payload,
            payload_error={
                "error_datetime": now.strftime("%Y/%m/%d %H:%M"),
                "error_function": SalesforceDataSyncMailType.UPDATE_SOLVER_CORPORATION,
                "user_name": current_user.name,
                "data": solver_corporation_data,
            },
            files=files,
        )

        # エラーメール通知が送信された場合、ロールバックを行う
        if result and result["email_type"] == "error":
            update_action = SolverCorporationService.update_solver_corporation_table(item=original_solver_corporation, current_user=current_user)
            solver_corporation.update(actions=update_action)

        return OKResponse()

    @staticmethod
    def delete_solver_corporation_by_id(
        current_user: UserModel,
        solver_corporation_id: str,
        version: int
    ):
        """法人ソルバーを無効化する

        Args:
            current_user (UserModel): API実行ユーザー
            solver_corporation_id (str): 法人ソルバーID
            version: ロックキー

        Raises:
            HTTPException: 404 Not found
            HTTPException: 409 Conflict

        Returns:
            200: 成功レスポンス
        """

        try:
            solver_corporation = SolverCorporationModel.get(
                hash_key=solver_corporation_id, range_key=DataType.SOLVER_CORPORATION
            )
        except DoesNotExist:
            logger.warning(
                f"DeleteSolverCorporationById solver_corporation_id not found. solver_corporation_id: {solver_corporation_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 排他チェック
        if solver_corporation.version != version:
            logger.warning(
                f"DeleteSolverCorporationById conflict. request_ver: {version} solver_corporation_ver: {solver_corporation.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 更新前のDB情報を保存
        original_solver_corporation = copy.deepcopy(solver_corporation)

        now = get_datetime_now()
        # 法人ソルバー担当無効化
        filter_condition = None
        filter_condition &= UserModel.disabled == False  # NOQA
        filter_condition &= UserModel.solver_corporation_id == solver_corporation_id

        solver_staff_list: List[UserModel] = list(UserModel.data_type_name_index.query(
            hash_key=DataType.USER,
            filter_condition=filter_condition,
        ))
        original_solver_staff_list = copy.deepcopy(solver_staff_list)
        with UserModel.batch_write() as user_batch:
            for user in solver_staff_list:
                user.disabled = True
                user.update_id = current_user.id
                user.update_at = now
                user.version += 1
                user_batch.save(user)

        # 法人無効化
        solver_corporation.update(
            actions=[
                SolverCorporationModel.disabled.set(True),
                SolverCorporationModel.update_id.set(current_user.id),
                SolverCorporationModel.update_at.set(now)
            ]
        )

        # 法人情報更新データ連携通知（メール）
        # メール内容の編集
        logger.info("start editing mail setting")
        solver_corporation_data = json.dumps(
            {
                "PPId__c": solver_corporation_id,
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
        result = SesHelper().send_mail_with_file(
            template_name=MailType.SALESFORCE_DATA_SYNC,
            to=[get_app_settings().salesforce_address_for_solver_corporation],
            cc=cc_email_list,
            payload={
                "subject": SalesforceDataSyncMailType.UPDATE_SOLVER_CORPORATION,
                "solver_corporation_data": solver_corporation_data
            },
            payload_error={
                "error_datetime": now.strftime("%Y/%m/%d %H:%M"),
                "error_function": SalesforceDataSyncMailType.UPDATE_SOLVER_CORPORATION,
                "user_name": current_user.name,
                "data": solver_corporation_data,
            },
        )

        # エラーメール通知が送信された場合、ロールバックを行う
        if result and result["email_type"] == "error":
            solver_corporation.update(
                actions=[
                    SolverCorporationModel.disabled.set(False),
                    SolverCorporationModel.update_id.set(original_solver_corporation.update_id),
                    SolverCorporationModel.update_at.set(original_solver_corporation.update_at),
                ]
            )
            with UserModel.batch_write() as user_batch:
                for user in original_solver_staff_list:
                    user_batch.save(user)

        return OKResponse()
