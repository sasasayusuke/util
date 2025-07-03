import copy
import datetime
import json
from datetime import timedelta
import os
from typing import List, Set

from fastapi import Depends, HTTPException, status
from fastapi import status as api_status
from fastapi.security import HTTPAuthorizationCredentials
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.update import Action
from pytz import timezone

from app.auth.jwt import JWTBearer
from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.master import MasterSupporterOrganizationModel, MasterUpdateKarteRecord
from app.models.project import ProjectModel
from app.models.project_karte import (
    DocumentsDeliverablesAttribute,
    KarteNotifyUpdateHistoryAttribute,
    LocationAttribute,
    ProjectKarteModel,
)
from app.models.user import UserModel
from app.resources.const import (
    DataType,
    FoAppUrl,
    KarteLocation,
    MailType,
    MasterDataType,
    MasterKartenProgramType,
    NotificationType,
    Presence,
    UserRoleType,
)
from app.schemas.karten import (
    DeliverablesInfo,
    DocumentInfo,
    GetKartenByIdResponse,
    GetKartenLatestResponse,
    GetKartenQuery,
    GetKartenResponse,
    Location,
    UpdateKarteByIdQuery,
    UpdateKarteByIdRequest,
    UpdateKarteByIdResponse,
)
from app.service.common_service.user_info import get_update_user_name
from app.service.notification_service import NotificationService
from app.service.common_service.request_processor import url_decode_data
from app.utils.aws.sqs import SqsHelper
from app.utils.date import get_datetime_now, get_day_of_week
from app.utils.platform import PlatformApiOperator

logger = CustomLogger.get_logger()


class KartenService:
    @staticmethod
    def is_visible_karte(user: UserModel, project_id: str) -> bool:
        """個別カルテへアクセス可能か判定.
            1.営業責任者、事業者責任者、支援者責任者
              制限なし
            2.支援者、営業担当者
              所属案件：アクセス可
              上記以外：アクセス不可
            3.顧客
              自身の案件：アクセス可
              上記以外：アクセス不可
        Args:
            user (UserModel): ユーザー
            project (ProjectModel)
            karte（ProjectKarteModel）
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)

        if (
            user.role == UserRoleType.SALES_MGR.key
            or user.role == UserRoleType.BUSINESS_MGR.key
            or user.role == UserRoleType.SUPPORTER_MGR.key
        ):
            return True

        elif user.role in UserRoleType.SUPPORTER.key:
            if project.main_supporter_user_id == user.id or (
                project.supporter_user_ids and user.id in project.supporter_user_ids
            ):
                # 所属案件
                return True

            return False

        elif user.role == UserRoleType.SALES.key:
            if project.main_sales_user_id == user.id:
                # 所属案件
                return True

            return False

        elif user.role == UserRoleType.CUSTOMER.key:
            for project_itr in user.project_ids:
                if project_itr == project.id:
                    # 参加している案件
                    return True

            # 参加していない案件
            return False

        return False

    @staticmethod
    def is_visible_karten_latest(user: UserModel, project_id: str) -> bool:
        """案件情報がアクセス可能か判定.
            1.所属案件(アクセス可)
              ・顧客
              ・支援者
              ・営業担当者
              ・支援者責任者
              ・事業者責任者
            2.アクセス不可
              ・営業責任者 (API呼出権限で制御)

        Args:
            user (UserModel)
            project_id: プロジェクトid
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)

        if user.role in [
            UserRoleType.SUPPORTER.key,
            UserRoleType.SUPPORTER_MGR.key,
            UserRoleType.BUSINESS_MGR.key,
        ]:
            if (
                project.main_supporter_user_id != user.id
                and user.id not in project.supporter_user_ids
            ):
                # 所属していない案件
                return False

            return True

        elif user.role == UserRoleType.SALES.key:
            if project.main_sales_user_id != user.id:
                # 所属していない案件
                return False

            return True

        elif user.role == UserRoleType.CUSTOMER.key:
            for project_itr in user.project_ids:
                if project_itr == project.id:
                    # 参加している案件
                    return True

            # 参加していない案件
            return False

        return False

    @staticmethod
    def is_visible_update_karte_by_id(user: UserModel, project_id: str) -> bool:
        """案件情報がアクセス可能か判定.
                1.所属案件(アクセス可)
                ・支援者
                ・支援者責任者
                ・事業者責任者
                2.アクセス不可(API呼出権限で制御)
                ・顧客
                ・営業担当者
                ・営業責任者

        Args:
            project (ProjectModel)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)

        if (
            project.main_supporter_user_id != user.id
            and user.id not in project.supporter_user_ids
        ):
            # 所属していない案件
            return False

        return True

    @staticmethod
    def send_mail(
        template, to_addr_list: List[str], cc_addr_list: List[str], payload: dict, bcc_addr_list: List[str] = []
    ):
        queue_name = get_app_settings().sqs_email_queue_name
        message_body = {
            "template": template,
            "to": to_addr_list,
            "cc": cc_addr_list,
            "bcc": bcc_addr_list,
            "payload": payload,
        }
        sqs_message_body = json.dumps(message_body)
        SqsHelper().send_message_to_queue(
            queue_name=queue_name, message_body=sqs_message_body
        )

    @staticmethod
    def convert_id_list_to_set(id_list: List[str]) -> Set:
        """IdリストをSetに変換.
            パラメータのid_listがNoneの場合、Noneを返却.
            e.g. [{'id':'aaa'},{'id':'bbb'}] -> {'aaa','bbb'}

        Args:
            user_id_class_list (List[UserId]): 変換対象

        Returns:
            Set: 変換結果
        """
        if id_list is None:
            return None

        return set(id_list)

    @staticmethod
    def get_master_karte_items(
        authorization: HTTPAuthorizationCredentials,
        pp_project_id: str,
        salesforce_opportunity_id: str,
    ) -> tuple:
        """マスターカルテの項目を取得

        マスターカルテから以下の項目を取得して返却します.
        - 部署名

        Args:
            authorization (HTTPAuthorizationCredentials)
            project_id (str): 案件ID

        Returns:
            tuple:
                str: 部署名
        """
        # 部署名
        pf_department_name = None

        # PF案件IDを取得するため、マスターカルテを検索
        # 検索条件
        # ・PartnerPortal案件ID
        # ・当期支援
        pf_params = {}

        if salesforce_opportunity_id:
            # SF商談IDが存在する場合
            pf_params["salesforceOpportunityId"] = salesforce_opportunity_id
        else:
            # PP案件IDが存在する場合
            pf_params["partnerPortalProjectId"] = pp_project_id

        pf_params["targetProgram"] = MasterKartenProgramType.CURRENT

        platform_api_operator = PlatformApiOperator(jwt_token=authorization.jwt_token)
        # PF案件一覧取得APIの呼び出し
        pf_get_projects_status_code, pf_projects = platform_api_operator.get_projects(
            params=pf_params
        )
        logger.info(f"platform getProjects statusCode: {pf_get_projects_status_code}")

        # PF案件ID
        npf_project_id = None
        if pf_get_projects_status_code == 200 and len(pf_projects["projects"]) > 0:
            # project_idで検索しているため、レスポンスは1件
            pf_project_item = pf_projects["projects"][0]
            npf_project_id = pf_project_item["project"]["id"]
        elif pf_get_projects_status_code != 200:
            # エラーにせず、処理継続
            logger.info("platform getProjects response:" + json.dumps(pf_projects))

        # 上記で取得したPF案件IDを条件に、マスターカルテ詳細を取得
        if npf_project_id:
            # PF案件詳細情報取得APIの呼び出し
            pf_get_project_by_id_status_code, pf_project_detail = (
                platform_api_operator.get_project_by_pf_id(npf_project_id)
            )
            logger.info(
                f"platform getProjectByPfId statusCode: {pf_get_project_by_id_status_code}"
            )

            if pf_get_project_by_id_status_code == 200:
                # マスターカルテ詳細のレスポンスから取得
                # 部署名
                pf_department_name = pf_project_detail["department"]["departmentName"]
            else:
                # エラーにせず、処理継続
                logger.info(
                    "platform getProjectByPfId response:"
                    + json.dumps(pf_project_detail)
                )

        return pf_department_name

    def edit_karte_notify_update_history_attribute(
        user: UserModel,
        master_supporter_organization_list: List[MasterSupporterOrganizationModel],
        karte_update_date: str,
    ) -> KarteNotifyUpdateHistoryAttribute:
        """
            KarteNotifyUpdateHistoryAttributeの編集

        Args:
            user (UserModel): 支援者ユーザのユーザモデル
            master_supporter_organization_list (List[MasterSupporterOrganizationModel]): 汎用マスターの支援者組織のリスト
            karte_update_date (str): 現在日の文字列

        Returns:
            KarteNotifyUpdateHistoryAttribute: 編集したAttribute
        """
        temp_id = []
        temp_name = []
        if user.supporter_organization_id:
            for organization_id in user.supporter_organization_id:
                for supporter_organization in master_supporter_organization_list:
                    if organization_id == supporter_organization.id:
                        temp_id.append(supporter_organization.id)
                        temp_name.append(supporter_organization.value)
                        break
        supporter_organization_id = ";".join(temp_id)
        supporter_organization_name = ";".join(temp_name)

        karte_notify_update_history = KarteNotifyUpdateHistoryAttribute(
            user_id=user.id,
            user_name=user.name,
            organization_id=supporter_organization_id,
            organization_name=supporter_organization_name,
            karte_update_date=karte_update_date,
        )

        return karte_notify_update_history

    @staticmethod
    def get_karten(
        query_params: GetKartenQuery, current_user: UserModel
    ) -> GetKartenResponse:
        """Get /karten 案件カルテの一覧取得API

        Args:
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetKartenResponse: 取得結果
        """

        karten_list: List[GetKartenResponse] = []
        item_update_user: str
        karten = ProjectKarteModel.project_id_start_datetime_index.query(
            hash_key=query_params.project_id
        )

        if not KartenService.is_visible_karte(current_user, query_params.project_id):
            # 案件にアクセス不可能
            logger.warning("GetKarten forbidden.")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        else:
            # 更新者がいない(draft_supporter_idがNULL)を考慮
            item_update_user: str = ""
            for karten_itr in karten:
                if karten_itr.draft_supporter_id is not None:
                    # 支援者名取得
                    user_info = UserModel
                    try:
                        user_info = UserModel.get(
                            hash_key=karten_itr.draft_supporter_id,
                            range_key=DataType.USER,
                        )
                        item_update_user = user_info.name
                    except DoesNotExist:
                        item_update_user = ""
                else:
                    item_update_user = ""

                karten_list.append(
                    GetKartenResponse(
                        karte_id=karten_itr.karte_id,
                        date=karten_itr.date,
                        start_time=karten_itr.start_time,
                        end_time=karten_itr.end_time,
                        is_draft=karten_itr.is_draft,
                        last_update_datetime=karten_itr.last_update_datetime,
                        update_user=item_update_user,
                    )
                )
            return karten_list

    @staticmethod
    def get_karten_latest(current_user: UserModel) -> GetKartenLatestResponse:
        """Get /karten/latest 最近更新されたカルテ一覧取得API
            ・顧客以外の場合は当日週から過去一週間以内に存在する自身のアサイン案件のカルテを表示する
            ・最大10件取得

        Args:
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetKartenLatestResponse: 取得結果
        """

        karten_item: List[ProjectKarteModel] = []
        # ユーザーロールが顧客かそれ以外かで処理を分ける
        if current_user.role == UserRoleType.CUSTOMER.key:
            karten: List[ProjectKarteModel] = []
            if current_user.project_ids is not None:
                for project_id_itr in list(current_user.project_ids):
                    if not (
                        KartenService.is_visible_karten_latest(
                            current_user, project_id_itr
                        )
                    ):
                        # 案件にアクセス不可能
                        logger.warning("GetKartenLatest forbidden.")
                        raise HTTPException(
                            status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                        )

                    get_karten = (
                        ProjectKarteModel.project_id_start_datetime_index.query(
                            hash_key=project_id_itr
                        )
                    )
                    # for文でDBからカルテ取得
                    # query で last_update_datetime がnullをはじいていないため、分岐して格納
                    # 下書きのカルテは顧客に表示しない
                    for get_karten_itr in get_karten:
                        if (
                            get_karten_itr.last_update_datetime is not None
                            and not get_karten_itr.is_draft
                        ):
                            karten.append(get_karten_itr)

            if len(karten) > 0:
                karten = sorted(
                    karten, reverse=True, key=lambda x: x.last_update_datetime
                )

                # 最大5件をソートしたリストの先頭から取得
                if len(karten) < 5:
                    karten_item = karten[0 : len(karten)]
                elif len(karten) >= 5:
                    karten_item = karten[0:5]

        else:
            karten: List[ProjectKarteModel] = []
            # ユーザー：プロジェクトは　１：N の関係
            if current_user.project_ids is not None:
                for project_id_itr in list(current_user.project_ids):
                    if not (
                        KartenService.is_visible_karten_latest(
                            current_user, project_id_itr
                        )
                    ):
                        # 案件にアクセス不可能
                        logger.warning("GetKartenLatest forbidden.")
                        raise HTTPException(
                            status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                        )

                    get_karten = (
                        ProjectKarteModel.project_id_start_datetime_index.query(
                            hash_key=project_id_itr
                        )
                    )
                    # for文でDBからカルテ取得
                    for get_karten_itr in get_karten:
                        karten.append(copy.deepcopy(get_karten_itr))

            if len(karten) > 0:
                # 取得したカルテのstart_datetime(支援開始日時)が
                # -7days <=  today_date
                today = get_datetime_now()
                before_seven_days = today - timedelta(days=7)
                for karte_days_itr in karten:
                    # 取得したカルテのstart_datetimeを比較用にdatetime型へ
                    target_date = datetime.datetime.strptime(
                        karte_days_itr.start_datetime, "%Y/%m/%d %H:%M"
                    )
                    # time zone設定
                    time_zone_info = timezone("Asia/Tokyo")
                    target_date = target_date.replace(tzinfo=time_zone_info)

                    # 時刻を含めずに、実施予定日を比較する
                    if (
                        before_seven_days.date() <= target_date.date()
                        and target_date.date() <= today.date()
                    ):
                        karten_item.append(karte_days_itr)

                karten_item = sorted(
                    karten_item, reverse=True, key=lambda x: x.start_datetime
                )

                if len(karten_item) > 0:
                    # 最大10件をソートしたリストの先頭から取得
                    karten_item = karten_item[:10]

        # レスポンス整形
        karten_response_list: List[GetKartenLatestResponse] = []
        for karte_item_itr in karten_item:
            item_project_info = ProjectModel.get(
                hash_key=karte_item_itr.project_id, range_key=DataType.PROJECT
            )
            # 更新されていないカルテの場合、最終更新者名に空文字を設定
            if karte_item_itr.draft_supporter_id is None:
                item_draft_supporter_name = ""
            else:
                try:
                    item_draft_supporter_name = (
                        UserModel.get(
                            hash_key=karte_item_itr.draft_supporter_id,
                            range_key=DataType.USER,
                        )
                    ).name
                except DoesNotExist:
                    # ユーザが存在しない→DBデータが破損しているため500エラーを出す
                    logger.warning(
                        "GetKartenLatest server error. DB data doesn't correct"
                    )
                    raise HTTPException(
                        status_code=api_status.HTTP_500_INTERNAL_SERVER_ERROR
                    )

            karten_response_list.append(
                GetKartenLatestResponse(
                    karte_id=karte_item_itr.karte_id,
                    project_id=karte_item_itr.project_id,
                    project_name=item_project_info.name,
                    customer_name=item_project_info.customer_name,
                    date=karte_item_itr.date,
                    start_time=karte_item_itr.start_time,
                    end_time=karte_item_itr.end_time,
                    day=get_day_of_week(
                        datetime.datetime.strptime(karte_item_itr.date, "%Y/%m/%d")
                    ),
                    last_update_datetime=karte_item_itr.last_update_datetime,
                    draft_supporter_name=item_draft_supporter_name,
                )
            )
        return karten_response_list

    @staticmethod
    def get_karten_by_id(
        karte_id: str, current_user: UserModel
    ) -> GetKartenByIdResponse:
        """Get /karten/{id} 案件カルテをIDで取得API

        Args:
            karte_id: カルテID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetKartenByIdResponse: 取得結果
        """

        try:
            karten = ProjectKarteModel.get(hash_key=karte_id)
        except DoesNotExist:
            logger.warning(f"GetKartenById not found. karte_id: {karte_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if not KartenService.is_visible_karte(current_user, karten.project_id):
            # 案件にアクセス不可能
            logger.warning("GetKartenById forbidden")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        else:
            # DBから取得した値をNULL検証して、レスポンス整形する
            item_update_user_name: str = ""
            item_update_user_id: str = ""
            item_draft_supporter_id: str = ""
            item_draft_supporter_name: str = ""
            item_documents_list: List[DocumentInfo] = []
            item_deliverables_list: List[DeliverablesInfo] = []
            # 以下、DB登録必須項目ではないため、空文字かチェック
            if karten.draft_supporter_id is not None:
                item_draft_supporter_id = karten.draft_supporter_id
                # 支援者名取得
                try:
                    item_draft_supporter_name = (
                        UserModel.get(
                            hash_key=item_draft_supporter_id, range_key=DataType.USER
                        )
                    ).name
                except DoesNotExist:
                    # ユーザが存在しない→DBデータが破損しているため500エラーを出す
                    logger.warning(
                        "GetKartenById server error. DB data doesn't correct"
                    )
                    raise HTTPException(
                        status_code=api_status.HTTP_500_INTERNAL_SERVER_ERROR
                    )
            else:
                item_draft_supporter_id = ""

            if karten.documents is not None:
                for documents_itr in karten.documents:
                    item_documents_list.append(
                        DocumentInfo(
                            fileName=documents_itr.file_name, path=documents_itr.path
                        )
                    )
            if karten.deliverables is not None:
                for deliverables_itr in karten.deliverables:
                    item_deliverables_list.append(
                        DeliverablesInfo(
                            fileName=deliverables_itr.file_name,
                            path=deliverables_itr.path,
                        )
                    )

            # 例外処理を行うため、レスポンスオブジェクト作成前にデータ取得処理を行う
            if karten.update_id is not None:
                item_update_user_id = karten.update_id
                item_update_user_name = get_update_user_name(karten.update_id)

            # 出席者が設定されていない場合、前回の出席者を設定する
            karten = KartenService._set_previous_attendees(karten)

            # レスポンス作成
            return_karten = GetKartenByIdResponse(
                karte_id=karten.karte_id,
                project_id=karten.project_id,
                date=karten.date,
                start_datetime=karten.start_datetime,
                start_time=karten.start_time,
                end_time=karten.end_time,
                draft_supporter_id=item_draft_supporter_id,
                draft_supporter_name=item_draft_supporter_name,
                last_update_datetime=karten.last_update_datetime,
                start_support_actual_time=karten.start_support_actual_time,
                end_support_actual_time=karten.end_support_actual_time,
                man_hour=karten.man_hour,
                customers=karten.customers,
                support_team=karten.support_team,
                detail=karten.detail,
                feedback=karten.feedback,
                homework=karten.homework,
                documents=item_documents_list,
                deliverables=item_deliverables_list,
                memo=karten.memo,
                human_resource_needed_for_customer=karten.human_resource_needed_for_customer,
                company_and_industry_recommended_to_customer=karten.company_and_industry_recommended_to_customer,
                human_resource_needed_for_support=karten.human_resource_needed_for_support,
                task=karten.task,
                location=Location(
                    type=karten.location.type,
                    detail=karten.location.detail,
                ) if karten.location else None,
                is_draft=karten.is_draft,
                update_id=item_update_user_id,
                update_user_name=item_update_user_name,
                update_at=karten.update_at,
                version=karten.version,
            )
            return return_karten

    @staticmethod
    def _set_previous_attendees(karten: ProjectKarteModel):
        """出席者が設定されていない場合、前回の出席者を設定する

        Args:
            karten: 取得した個別カルテ

        Returns:
            karten: 前回の出席者を設定済みの個別カルテ
        """
        if karten.customers and karten.support_team:
            return karten

        target_kartens: list[ProjectKarteModel] = ProjectKarteModel.project_id_start_datetime_index.query(
            hash_key=karten.project_id,
            scan_index_forward=False
        )

        def _strptipe(start_datetime):
            fmt = "%Y/%m/%d %H:%M"
            try:
                return datetime.datetime.strptime(start_datetime, fmt)
            except Exception:
                return datetime.datetime(1900, 1, 1, 0, 0)

        filtered = [
            target_karten for target_karten in target_kartens
            if _strptipe(target_karten.start_datetime) < _strptipe(karten.start_datetime)
        ]

        # 前回のお客様を設定
        for target_karten in filtered:
            if target_karten.customers:
                karten.customers = target_karten.customers
                break

        # 前回のSAP支援チーム
        for target_karten in filtered:
            if target_karten.support_team:
                karten.support_team = target_karten.support_team
                break

        return karten

    @staticmethod
    def update_karte_by_id(
        karte_id: str,
        item: UpdateKarteByIdRequest,
        query_params: UpdateKarteByIdQuery,
        current_user: UserModel,
        authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
    ) -> UpdateKarteByIdResponse:
        """Get /karten 案件カルテの更新API

        Args:
            current_user (Behavior, optional): 認証済みのユーザー
            query_params(version): クエリパラメータで指定
            id(str): カルテID パスパラメータで指定
            authorization(HTTPAuthorizationCredentials)

        Returns:
            UpdateKartenByIdResponse: 更新結果
        """

        try:
            karte = ProjectKarteModel.get(hash_key=karte_id)
        except DoesNotExist:
            logger.warning(f"UpdateKartenById not found. karte_id: {karte_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if not KartenService.is_visible_update_karte_by_id(
            current_user, karte.project_id
        ):
            # 案件にアクセス不可能
            logger.warning("UpdateKartenById forbidden")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # 排他チェック
        if query_params.version != karte.version:
            logger.warning(
                f"UpdateKartenById conflict. request_ver:{query_params.version} karte_ver: {karte.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        update_action: List[Action] = []
        mail_date: str = karte.date

        # 汎用マスタ:支援者組織情報
        master_supporter_organization_list: List[MasterSupporterOrganizationModel] = (
            list(
                MasterSupporterOrganizationModel.data_type_index.query(
                    hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value
                )
            )
        )

        update_action.append(
            ProjectKarteModel.start_support_actual_time.set(
                item.start_support_actual_time
            )
        )

        update_action.append(
            ProjectKarteModel.end_support_actual_time.set(item.end_support_actual_time)
        )

        update_action.append(ProjectKarteModel.man_hour.set(item.man_hour))

        update_action.append(ProjectKarteModel.customers.set(item.customers))

        update_action.append(ProjectKarteModel.support_team.set(item.support_team))

        update_action.append(ProjectKarteModel.detail.set(item.detail))

        update_action.append(ProjectKarteModel.feedback.set(item.feedback))

        update_action.append(ProjectKarteModel.homework.set(item.homework))

        if item.documents is not None:
            update_documents: List[DocumentsDeliverablesAttribute] = []
            for documents_itr in item.documents:
                update_documents.append(
                    DocumentsDeliverablesAttribute(
                        file_name=documents_itr.file_name, path=documents_itr.path
                    )
                )
            update_action.append(ProjectKarteModel.documents.set(update_documents))

        if item.deliverables is not None:
            update_deliverables: List[DocumentsDeliverablesAttribute] = []
            for deliverables_itr in item.deliverables:
                update_deliverables.append(
                    DocumentsDeliverablesAttribute(
                        file_name=deliverables_itr.file_name, path=deliverables_itr.path
                    )
                )
            update_action.append(
                ProjectKarteModel.deliverables.set(update_deliverables)
            )

        update_action.append(ProjectKarteModel.memo.set(item.memo))

        update_action.append(
            ProjectKarteModel.human_resource_needed_for_customer.set(
                item.human_resource_needed_for_customer
            )
        )
        update_action.append(
            ProjectKarteModel.company_and_industry_recommended_to_customer.set(
                item.company_and_industry_recommended_to_customer
            )
        )
        update_action.append(
            ProjectKarteModel.human_resource_needed_for_support.set(
                item.human_resource_needed_for_support
            )
        )

        update_action.append(ProjectKarteModel.task.set(item.task))

        # 場所
        if item.location and item.location.type:
            update_action.append(
                ProjectKarteModel.location.set(
                    LocationAttribute(
                        type=item.location.type,
                        detail=item.location.detail
                    )
                )
            )

        # 更新必須項目
        update_action.append(ProjectKarteModel.is_draft.set(item.is_draft))

        # API を実行できるのは支援者と支援者責任者だけであるためdraft_supporter_idを更新
        update_action.append(ProjectKarteModel.draft_supporter_id.set(current_user.id))

        update_action.append(
            ProjectKarteModel.last_update_datetime.set(datetime.datetime.now())
        )

        # APIを実行して更新した時刻・実行者
        update_action.append(ProjectKarteModel.update_at.set(datetime.datetime.now()))
        update_action.append(ProjectKarteModel.update_id.set(current_user.id))

        # 「カルテの更新を通知する」をONの場合
        if item.is_notify_update_karte:
            # 個別カルテ更新通知最終更新日時を更新
            update_action.append(
                ProjectKarteModel.karte_notify_last_update_datetime.set(
                    datetime.datetime.now()
                )
            )

            # 個別カルテ更新通知履歴を更新
            append_item_karte_history = KartenService.edit_karte_notify_update_history_attribute(
                user=current_user,
                master_supporter_organization_list=master_supporter_organization_list,
                karte_update_date=get_datetime_now().strftime("%Y/%m/%d"),
            )
            update_action.append(
                ProjectKarteModel.karte_notify_update_history.set(
                    (ProjectKarteModel.karte_notify_update_history | []).append(
                        [append_item_karte_history]
                    )
                )
            )

        karte.update(actions=update_action)

        # メール通知
        # 「カルテの更新を通知する」をONにして登録、更新を行ったとき
        logger.info("start editing mail setting")
        if item.is_notify_update_karte:
            mail_project = ProjectModel.get(
                hash_key=karte.project_id, range_key=DataType.PROJECT
            )

            fo_site_url = get_app_settings().fo_site_url
            fo_karte_detail_url = fo_site_url + FoAppUrl.KARTE_DETAIL.format(
                karteId=karte_id
            )

            # 案件の「プロデューサー」に設定されているユーザー情報の取得処理
            # 設定されていない場合は空白を返す
            karte_main_supporter_user_id = mail_project.main_supporter_user_id

            karte_main_supporter_name = None
            karte_main_supporter_email = None

            if karte_main_supporter_user_id:
                karte_main_supporter_user = UserModel.get(
                    hash_key=karte_main_supporter_user_id, range_key=DataType.USER
                )

                # ユーザーの無効化チェック
                if not karte_main_supporter_user.disabled:
                    karte_main_supporter_name = karte_main_supporter_user.name
                    karte_main_supporter_email = karte_main_supporter_user.email

            # マスターカルテから以下を取得
            # ・部署名
            pf_department_name = KartenService.get_master_karte_items(
                authorization=authorization,
                pp_project_id=mail_project.id,
                salesforce_opportunity_id=mail_project.salesforce_opportunity_id,
            )

            fo_notification_user_id_list: List[str] = []

            # 個別カルテ入力・更新通知（外部向け）
            # お客様公開の場合
            if not item.is_draft:
                payload_external_update_karte = {
                    "project_name": mail_project.name,
                    "fo_karte_URL": fo_karte_detail_url,
                    "detail": item.detail if item.detail is not None else "",
                    "feedback": item.feedback if item.feedback is not None else "",
                    "next_action": item.homework if item.homework is not None else "",
                    "date": mail_date,
                    "customer_success": mail_project.customer_success
                    if mail_project.customer_success is not None
                    else "",
                    "producer_name": karte_main_supporter_name
                    if karte_main_supporter_name is not None
                    else "",
                    "producer_email": karte_main_supporter_email
                    if karte_main_supporter_email is not None
                    else "",
                }
                to_external_email_list: List[str] = []
                cc_external_email_list: List[str] = []

                # URLデコード
                payload_external_update_karte = url_decode_data(payload_external_update_karte)

                # TO：案件に設定されたプロデューサー、アクセラレーター、お客様メンバー
                # 下書きの場合は顧客には通知しない
                # CC：案件に設定された商談所有者、営業責任者（全員）、案件に設定された粗利メイン課と同じ課に所属する支援者責任者
                logger.info("start making address")
                user_filter_condition = UserModel.disabled == False  # NOQA
                for mail_user_itr in UserModel.scan(
                    filter_condition=user_filter_condition
                ):
                    if (
                        (
                            mail_project.supporter_user_ids is not None
                            and mail_user_itr.id in mail_project.supporter_user_ids
                        )
                        or mail_user_itr.id == mail_project.main_supporter_user_id
                        or (
                            mail_user_itr.id == mail_project.main_customer_user_id
                            and not item.is_draft
                        )
                        or (
                            mail_project.customer_user_ids is not None
                            and mail_user_itr.id in mail_project.customer_user_ids
                            and not item.is_draft
                        )
                    ):
                        to_external_email_list.append(mail_user_itr.email)
                        fo_notification_user_id_list.append(mail_user_itr.id)
                        continue

                    if (
                        mail_user_itr.id == mail_project.main_sales_user_id
                        or mail_user_itr.role == UserRoleType.SALES_MGR.key
                        or (
                            mail_user_itr.role == UserRoleType.SUPPORTER_MGR.key
                            and mail_user_itr.supporter_organization_id is not None
                            and mail_project.supporter_organization_id
                            in mail_user_itr.supporter_organization_id
                        )
                    ):
                        cc_external_email_list.append(mail_user_itr.email)

                # 宛先の重複を削除
                to_external_email_list = list(set(to_external_email_list))
                cc_external_email_list = list(set(cc_external_email_list))

                logger.info("start sending mail")
                KartenService.send_mail(
                    template=MailType.EXTERNAL_UPDATE_KARTE,
                    to_addr_list=to_external_email_list,
                    cc_addr_list=cc_external_email_list,
                    payload=payload_external_update_karte,
                )

            # 個別カルテ入力・更新通知（内部向け）
            # YYYY（会計年度）を取得
            # 1月~3月の場合は年度を-1する
            now_datetime = get_datetime_now()
            if now_datetime.month < 4:
                financial_year = now_datetime.year - 1
            else:
                financial_year = now_datetime.year

            # NNN（年度単位で連番）を取得
            try:
                masters: List[MasterUpdateKarteRecord] = (
                    MasterUpdateKarteRecord.data_type_index.query(
                        hash_key=MasterDataType.UPDATE_KARTE_RECORD.value
                    )
                )
            except DoesNotExist:
                logger.warning(
                    f"data_type not found. data_type: {MasterDataType.UPDATE_KARTE_RECORD.value}"
                )
                raise HTTPException(
                    status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
                )

            for master in masters:
                # 個別カルテ更新記録の最終更新日時が同年の3/31以前かつ個別カルテ更新日時が同年の4/1以降の場合、年度内個別カルテ更新数を初期化する
                if master.update_count == 0 or (
                    now_datetime.date() >= datetime.date(now_datetime.year, 4, 1)
                    and master.update_at.date()
                    <= datetime.date(now_datetime.year, 3, 31)
                ):
                    master.update_count = 1
                    master.update(
                        actions=[
                            MasterUpdateKarteRecord.update_count.set(
                                master.update_count
                            ),
                            MasterUpdateKarteRecord.update_at.set(
                                datetime.datetime.now()
                            ),
                        ]
                    )
                # 上記以外の場合、年度内個別カルテ更新数をプラス1する
                else:
                    master.update_count += 1
                    master.update(
                        actions=[
                            MasterUpdateKarteRecord.update_count.set(
                                master.update_count
                            ),
                            MasterUpdateKarteRecord.update_at.set(
                                datetime.datetime.now()
                            ),
                        ]
                    )

            # 支援「年」・支援「月」・支援「日」を取得
            date_split: List[str] = mail_date.split("/")

            # 曜日を取得
            day_of_week = get_day_of_week(
                datetime.datetime.strptime(mail_date, "%Y/%m/%d")
            )

            # カルテ更新者の苗字を取得
            karte_updater_split: List[str] = (
                current_user.name.split() if current_user.name is not None else []
            )

            # 部署名
            mail_department_name = (
                # お客様名と同じ場合はお客様名のみ表示する
                ""
                if pf_department_name == mail_project.customer_name or pf_department_name is None
                else pf_department_name
            )

            mail_human_resource_needed_for_customer = (
                item.human_resource_needed_for_customer
                if item.human_resource_needed_for_customer is not None
                else ""
            )
            mail_company_and_industry_recommended_to_customer = (
                item.company_and_industry_recommended_to_customer
                if item.company_and_industry_recommended_to_customer is not None
                else ""
            )
            mail_human_resource_needed_for_support = (
                item.human_resource_needed_for_support
                if item.human_resource_needed_for_support is not None
                else ""
            )

            location = ''
            if item.location and item.location.type:
                location = (
                    KarteLocation.get_label(item.location.type)
                    if item.location.type in [KarteLocation.ONLINE.value, KarteLocation.HEAD_OFFICE.value]
                    else f'{KarteLocation.get_label(item.location.type)}：{item.location.detail}'
                )

            payload_internal_update_karte = {
                "financial_year": financial_year,
                "update_count": master.update_count,
                "project_name": mail_project.name,
                "karte_updater_surname": karte_updater_split[0]
                if len(karte_updater_split) > 0
                else "",
                "customer_name": mail_project.customer_name,
                "department_name": mail_department_name,
                "date": mail_date,
                "support_year": date_split[0],
                "support_month": date_split[1].lstrip("0"),
                "support_day": date_split[2].lstrip("0"),
                "day_of_week": day_of_week,
                "start_support_actual_time": item.start_support_actual_time
                if item.start_support_actual_time is not None
                else "",
                "end_support_actual_time": item.end_support_actual_time
                if item.end_support_actual_time is not None
                else "",
                "service_manager_name": mail_project.service_manager_name if mail_project.service_manager_name else "",
                "customer_success": mail_project.customer_success
                if mail_project.customer_success is not None
                else "",
                "customers": item.customers if item.customers is not None else "",
                "support_team": item.support_team if item.support_team is not None else "",
                "detail": item.detail if item.detail is not None else "",
                "feedback": item.feedback if item.feedback is not None else "",
                "next_action": item.homework if item.homework is not None else "",
                "memo": item.memo if item.memo is not None else "",
                "human_resource_needed_for_customer": mail_human_resource_needed_for_customer,
                "company_and_industry_recommended_to_customer": mail_company_and_industry_recommended_to_customer,
                "human_resource_needed_for_support": mail_human_resource_needed_for_support,
                "task": item.task if item.task is not None else "",
                "fo_karte_URL": fo_karte_detail_url,
                "location": location,
                "has_attachment": KartenService._get_attachment_presence(item),
                "producer_name": (
                    karte_main_supporter_name
                    if karte_main_supporter_name is not None
                    else ""
                ),
                "salesforce_opportunity_id": (
                    mail_project.salesforce_opportunity_id
                    if mail_project.salesforce_opportunity_id
                    else ""
                ),
                "karte_id": karte.karte_id
            }
            to_internal_email_list: List[str] = []
            cc_internal_email_list: List[str] = []
            bcc_internal_email_list: List[str] = []

            # URLデコード
            payload_internal_update_karte = url_decode_data(payload_internal_update_karte)

            # TO：事業者責任者（全員）、営業責任者（全員）、支援者責任者（全員）
            # CC：案件に設定されたプロデューサー、アクセラレーター（全員）、案件に設定された商談所有者
            logger.info("start making address")
            user_filter_condition = UserModel.disabled == False  # NOQA
            for mail_user_itr in UserModel.scan(filter_condition=user_filter_condition):
                if mail_user_itr.role in [
                    UserRoleType.BUSINESS_MGR.key,
                    UserRoleType.SALES_MGR.key,
                    UserRoleType.SUPPORTER_MGR.key,
                ]:
                    to_internal_email_list.append(mail_user_itr.email)
                    fo_notification_user_id_list.append(mail_user_itr.id)

                if (
                    mail_user_itr.id == mail_project.main_supporter_user_id
                    or (
                        mail_project.supporter_user_ids is not None
                        and mail_user_itr.id in mail_project.supporter_user_ids
                    )
                    or mail_user_itr.id == mail_project.main_sales_user_id
                ):
                    cc_internal_email_list.append(mail_user_itr.email)

            if mail_project.main_supporter_user_id == current_user.id:
                if os.getenv("SF_EMAIL_ADDRESS"):
                    bcc_internal_email_list.append(os.getenv("SF_EMAIL_ADDRESS"))

            # 宛先の重複を削除
            to_internal_email_list = list(set(to_internal_email_list))
            cc_internal_email_list = list(set(cc_internal_email_list))
            bcc_internal_email_list = list(set(bcc_internal_email_list))

            logger.info("start sending mail")
            KartenService.send_mail(
                template=MailType.INTERNAL_UPDATE_KARTE,
                to_addr_list=to_internal_email_list,
                cc_addr_list=cc_internal_email_list,
                payload=payload_internal_update_karte,
                bcc_addr_list=bcc_internal_email_list,
            )

            # お知らせ通知
            # 重複を削除
            fo_notification_user_id_list = list(set(fo_notification_user_id_list))
            notification_notice_at = datetime.datetime.now()
            NotificationService.save_notification(
                notification_type=NotificationType.UPDATE_KARTE,
                user_id_list=fo_notification_user_id_list,
                message_param=payload_internal_update_karte,
                url=fo_karte_detail_url,
                noticed_at=notification_notice_at,
                create_id=current_user.id,
                update_id=current_user.id,
            )

        return UpdateKarteByIdResponse(message="OK")

    @staticmethod
    def _get_attachment_presence(item: UpdateKarteByIdRequest):
        """添付ファイルの存在有無を判定し、メール用の文字列を取得する

        Args:
            item: 更新リクエスト

        Returns:
            メール用の添付ファイルの存在有無
        """
        return Presence.PRESENT if KartenService._has_attachment(item) else Presence.ABSENT

    @staticmethod
    def _has_attachment(item: UpdateKarteByIdRequest):
        """添付ファイルの存在有無を判定する

        Args:
            item: 更新リクエスト

        Returns:
            添付ファイルの存在有無
        """
        if item.deliverables and len(item.deliverables) > 0:
            return True

        if item.documents and len(item.documents) > 0:
            return True

        return False
