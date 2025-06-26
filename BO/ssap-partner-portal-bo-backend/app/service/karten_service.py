from typing import List

from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist

from app.core.common_logging import CustomLogger
from app.models.admin import AdminModel
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from app.schemas.karten import (
    DeliverablesInfo,
    DocumentInfo,
    GetKartenByIdResponse,
    GetKartenQuery,
    GetKartenResponse,
    Location,
)
from app.service.common_service.user_info import get_update_user_name

logger = CustomLogger.get_logger()


class KartenService:
    @staticmethod
    def is_visible_karte(admin: AdminModel, project_id: str) -> bool:
        """個別カルテへアクセス可能か判定.
            1.システム管理者、営業責任者、事業者責任者、支援者責任者
              制限なし
            2.アンケート事務局、稼働率調査事務局
              アクセス不可
            3.営業担当者
              所属案件：アクセス可
              上記以外：アクセス不可
        Args:
            admin (AdminModel): 管理ユーザー
            project (ProjectModel)
            karte（ProjectKarteModel）
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)

        # メールアドレスが一致する一般ユーザ情報を取得
        range_key_condition = UserModel.email == admin.email
        filter_condition = UserModel.disabled == False
        user_info_list: List[UserModel] = list(
            UserModel.data_type_email_index.query(
                hash_key=DataType.USER,
                range_key_condition=range_key_condition,
                filter_condition=filter_condition,
            )
        )
        # 取得結果は0件または1件のため、リストの先頭を取得
        user_info = user_info_list[0] if user_info_list else None

        for role in admin.roles:
            if role in [
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
            ]:
                # システム管理者、営業責任者、事業者責任者、支援者責任者の場合
                return True

            elif role in [UserRoleType.SURVEY_OPS.key, UserRoleType.MAN_HOUR_OPS.key]:
                # アンケート事務局、稼働率調査事務局の場合
                continue

            elif role == UserRoleType.SALES.key:
                # 営業担当者の場合
                if user_info:
                    # 一般ユーザ情報が存在する場合
                    if project.main_sales_user_id == user_info.id:
                        # 所属案件
                        return True

                continue

        return False

    @staticmethod
    def get_karten(
        query_params: GetKartenQuery, current_user: AdminModel
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
                    userInfo = UserModel
                    try:
                        userInfo = UserModel.get(
                            hash_key=karten_itr.draft_supporter_id,
                            range_key=DataType.USER,
                        )
                        item_update_user = userInfo.name
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
    def get_karten_by_id(
        karte_id: str, current_user: AdminModel
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
            logger.warning("GetKartenById forbidden.")
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
                        "GetKartenById DB data doesn't correct. user does't exist"
                    )
                    raise HTTPException(
                        status_code=status.HTTP_500_INTERNAL_SERVER_ERROR
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
