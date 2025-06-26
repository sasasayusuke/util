from datetime import datetime
from typing import List, Union

from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist

from app.auth.auth import AuthUser
from app.core.common_logging import CustomLogger
from app.models.man_hour import (
    AcquirementItemsSubAttribute,
    DirectSupportManHoursAttribute,
    HolidaysManHoursAttribute,
    ManHourProjectSummaryModel,
    ManHourSupporterModel,
    PreSupportManHoursAttribute,
    SalesSupportManHoursAttribute,
    SsapManHoursAttribute,
    SummaryManHourAllowNullAttribute,
    SupportItemsSubAttribute,
)
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.user import UserModel
from app.resources.const import DataType, SupporterRoleTypeName, UserRoleType
from app.schemas.man_hour import (
    DirectSupportManHourItems,
    GetManHourByMineResponse,
    GetSummaryProjectSupporterManHourStatusResponse,
    PreSupportManHourItems,
    UpdateManHourByMineRequest,
    UpdateManHourByMineResponse,
)
from app.service.common_service.cached_db_items import CachedDbItems

logger = CustomLogger.get_logger()


class ManHourService:
    @staticmethod
    def get_project_info_from_cache(project_id: str, attr: str = "name"):
        """案件IDから指定された案件情報を取得.
            指定された案件情報が解決できない場合、Noneを返却.
        Args:
            project_id (str): 案件ID
            attr (str): 取得したい案件の情報 (案件テーブルに存在する属性値を指定可能)
        Raise:
        Returns:
            str: 案件情報
        """
        if not project_id:
            return None

        target_value = None

        # プロジェクトの一覧を取得
        projects = CachedDbItems.ReturnProjects()

        # 一覧に指定された案件IDと属性値が含まれているか検索
        for project in projects:
            if project_id == project["id"]:
                target_value = project.get(attr)
                break

        return target_value

    @staticmethod
    def get_supporter_organization_name_from_cache(
        supporter_organization_id: str,
    ) -> Union[str, None, HTTPException]:
        """支援者IDから支援者組織名を取得.
            取得できない場合はNoneを返却.
            supporter_organization_idがNoneまたは空の場合、Noneを返却.
        Args:
            supporter_organization_id (str): 支援者組織（粗利メイン課、アンケート集計課）ID
        Raise:

        Returns:
            str: 支援者組織名
        """
        if not supporter_organization_id:
            return None

        supporter_organization_name = None

        # 支援者組織の一覧を取得
        supporter_organizations = CachedDbItems.get_supporter_organizations()

        # 一覧に指定された支援者組織IDが含まれているか検索
        for current_supporter_organization in supporter_organizations:
            if supporter_organization_id == current_supporter_organization["id"]:
                supporter_organization_name = current_supporter_organization["name"]
                break

        return supporter_organization_name

    @staticmethod
    def is_visible_man_hour(user: UserModel, role: str, project_id: str) -> bool:
        """案件情報がアクセス可能か判定.
            1.制限なし(アクセス可)
              ・営業責任者
              ・事業者責任者
            2.支援者、支援者責任者、営業担当者
              所属していない非公開案件：アクセス不可
              上記以外：アクセス可
            3.顧客
              自身の案件：アクセス可
              上記以外：アクセス不可
        Args:
            role (str): ロール
            project (ProjectModel)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)

        if role in [
            UserRoleType.SALES_MGR.key,
            UserRoleType.BUSINESS_MGR.key,
        ]:
            return True

        elif role in [
            UserRoleType.SUPPORTER.key,
            UserRoleType.SUPPORTER_MGR.key,
        ]:
            if (
                project.main_supporter_user_id != user.id
                # 条件：アクセレーターが未設定 or アクセレーターの中に「user.id」がない
                and (
                    project.supporter_user_ids is None
                    or user.id not in project.supporter_user_ids
                )
                and project.is_secret
            ):
                # 所属していない非公開案件
                return False

            return True

        elif role == UserRoleType.SALES.key:
            if project.main_sales_user_id != user.id and project.is_secret:
                # 所属していない非公開案件
                return False

            return True

        elif role == UserRoleType.CUSTOMER.key:
            if (
                project.main_customer_user_id == user.id
                # 条件：お客様が設定されている且つお客様の中に「user.id」がある
                or (
                    project.customer_user_ids is not None
                    and user.id in project.customer_user_ids
                )
            ):
                # 参加している案件
                return True

        return False

    @staticmethod
    def get_man_hour_by_mine(
        year: int, month: int, current_user: AuthUser
    ) -> GetManHourByMineResponse:
        """Get /man-hours/me 支援者別工数取得API
        Args:
            year (int): 対象年
            month (int): 対象月
            current_user (AuthUser): 認証済みユーザー
        Raises:

        Returns:
            GetManHourByMineResponse: 取得結果
        """

        data_type = "supporter#" + current_user.id

        month_str = str(month)
        month_zero_padding = month_str.zfill(2)
        year_month = str(year) + "/" + month_zero_padding

        # 支援者別工数を取得
        try:
            man_hour = ManHourSupporterModel.get(
                hash_key=data_type, range_key=year_month
            )
        except DoesNotExist:
            man_hour = None

        assigned_projects: List = []
        direct_support_confirm_projects: List = []
        pre_support_confirm_projects: List = []

        direct_support_man_hour_list: List[DirectSupportManHourItems] = []
        pre_support_man_hour_list: List[PreSupportManHourItems] = []

        if man_hour:
            # 直接支援工数
            for item in man_hour.direct_support_man_hours.items:
                project_name: str = ManHourService.get_project_info_from_cache(
                    item.project_id,
                )
                customer_id: str = ManHourService.get_project_info_from_cache(
                    item.project_id, "customer_id"
                )
                customer_name: str = ManHourService.get_project_info_from_cache(
                    item.project_id, "customer_name"
                )

                # 支援者別工数内のプロジェクトIDから案件カルテに入力されている当月分の工数を取得
                start_date = f"{year}/{month_zero_padding}/01 00:00"
                end_date = f"{year}/{month_zero_padding}/31 23:59"
                karte_man_hour_iter = (
                    ProjectKarteModel.project_id_start_datetime_index.query(
                        hash_key=item.project_id,
                        range_key_condition=ProjectKarteModel.start_datetime.between(
                            start_date, end_date
                        ),
                    )
                )
                karte_man_hour_list: List[float] = []
                for karte_man_hour in karte_man_hour_iter:
                    karte_man_hour_list.append(karte_man_hour.man_hour)

                direct_support_man_hour_list.append(
                    DirectSupportManHourItems(
                        project_id=item.project_id,
                        project_name=project_name,
                        role=item.role,
                        customer_id=customer_id,
                        customer_name=customer_name,
                        service_type=item.service_type,
                        karte_man_hour=round(sum(karte_man_hour_list), 2),
                        input_man_hour=round(item.input_man_hour, 2),
                    )
                )
                # 工数入力済み案件を格納
                direct_support_confirm_projects.append(item.project_id)

            # 仕込支援工数
            for item in man_hour.pre_support_man_hours.items:
                # 支援者別工数内のプロジェクトIDからプロジェクト名を取得
                project_name: str = ManHourService.get_project_info_from_cache(
                    item.project_id
                )
                customer_id: str = ManHourService.get_project_info_from_cache(
                    item.project_id, "customer_id"
                )
                customer_name: str = ManHourService.get_project_info_from_cache(
                    item.project_id, "customer_name"
                )

                pre_support_man_hour_list.append(
                    PreSupportManHourItems(
                        project_id=item.project_id,
                        project_name=project_name,
                        role=item.role,
                        customer_id=customer_id,
                        customer_name=customer_name,
                        service_type=item.service_type,
                        input_man_hour=round(item.input_man_hour, 2),
                    )
                )
                # 工数入力済み案件を格納
                pre_support_confirm_projects.append(item.project_id)

        # 工数未入力案件の追加処理
        if not man_hour or not man_hour.is_confirm:
            # boolの条件でflake8(E712)が発生するため、boolを定義
            bool_true = True
            # アサイン中の案件一覧を取得（工数調査対象のみ）
            ongoing_projects = ProjectModel.data_type_support_date_to_index.query(
                hash_key=DataType.PROJECT,
                range_key_condition=ProjectModel.support_date_to
                >= f"{year}/{month_zero_padding}/01",
                filter_condition=(
                    ProjectModel.support_date_from <= f"{year}/{month_zero_padding}/31"
                )
                & (
                    (ProjectModel.main_supporter_user_id == current_user.id)
                    | (ProjectModel.supporter_user_ids.contains(current_user.id))
                )
                & (ProjectModel.is_count_man_hour == bool_true),
            )

            for on_going_project in ongoing_projects:
                assigned_projects.append(on_going_project.id)

            # アサイン中の案件で自分が工数入力していない案件を特定
            direct_support_not_confirmed_myself_projects = set(assigned_projects) - set(
                direct_support_confirm_projects
            )
            pre_support_not_confirmed_myself_projects = set(assigned_projects) - set(
                pre_support_confirm_projects
            )

            start_date = f"{year}/{month_zero_padding}/01 00:00"
            end_date = f"{year}/{month_zero_padding}/31 23:59"

            # 自分が工数入力していない案件を特定しレスポンスへ追加
            for direct_support_not_confirmed_myself_project in list(
                direct_support_not_confirmed_myself_projects
            ):
                current_project_kartes: List = (
                    ProjectKarteModel.project_id_start_datetime_index.query(
                        hash_key=direct_support_not_confirmed_myself_project,
                        range_key_condition=ProjectKarteModel.start_datetime.between(
                            start_date, end_date
                        ),
                    )
                )

                # 指定月の案件カルテに入力されている工数情報を集計
                karte_man_hours: List[int] = []

                for project_karte in current_project_kartes:
                    if project_karte.man_hour:
                        karte_man_hours.append(project_karte.man_hour)

                project_info = ProjectModel.get(
                    hash_key=direct_support_not_confirmed_myself_project,
                    range_key=DataType.PROJECT,
                )

                role: str = ""

                if current_user.id == project_info.main_supporter_user_id:
                    role = SupporterRoleTypeName.PRODUCER
                else:
                    role = SupporterRoleTypeName.ACCELERATOR

                direct_support_man_hour_list.append(
                    DirectSupportManHourItems(
                        project_id=project_info.id,
                        project_name=project_info.name,
                        role=role,
                        customer_id=project_info.customer_id,
                        customer_name=project_info.customer_name,
                        service_type=project_info.service_type,
                        karte_man_hour=round(sum(karte_man_hours), 2),
                        input_man_hour=0,
                    )
                )

            for pre_support_not_confirmed_myself_project in list(
                pre_support_not_confirmed_myself_projects
            ):
                project_info = ProjectModel.get(
                    hash_key=pre_support_not_confirmed_myself_project,
                    range_key=DataType.PROJECT,
                )

                role: str = ""

                if current_user.id == project_info.main_supporter_user_id:
                    role = SupporterRoleTypeName.PRODUCER
                else:
                    role = SupporterRoleTypeName.ACCELERATOR

                pre_support_man_hour_list.append(
                    PreSupportManHourItems(
                        project_id=project_info.id,
                        project_name=project_info.name,
                        role=role,
                        customer_id=project_info.customer_id,
                        customer_name=project_info.customer_name,
                        service_type=project_info.service_type,
                        input_man_hour=0,
                    )
                )

        # 案件名でソート
        direct_support_man_hour_list_sorterd = sorted(
            direct_support_man_hour_list, key=lambda x: x.project_name
        )
        pre_support_man_hour_list_sorterd = sorted(
            pre_support_man_hour_list, key=lambda x: x.project_name
        )
        sales_support_man_hour_list_sorterd = (
            sorted(man_hour.sales_support_man_hours.items, key=lambda x: x.project_name)
            if man_hour
            else None
        )

        return GetManHourByMineResponse(
            year_month=man_hour.year_month if man_hour else year_month,
            supporter_user_id=man_hour.supporter_user_id
            if man_hour
            else current_user.id,
            supporter_name=man_hour.supporter_name if man_hour else current_user.name,
            supporter_organization_id=man_hour.supporter_organization_id
            if man_hour
            else current_user.supporter_organization_id[0],
            supporter_organization_name=man_hour.supporter_organization_name
            if man_hour
            else ManHourService.get_supporter_organization_name_from_cache(
                current_user.supporter_organization_id[0]
            ),
            direct_support_man_hours=dict(
                items=direct_support_man_hour_list_sorterd,
                memo=man_hour.direct_support_man_hours.memo if man_hour else None,
            ),
            pre_support_man_hours=dict(
                items=pre_support_man_hour_list_sorterd,
                memo=man_hour.pre_support_man_hours.memo if man_hour else None,
            ),
            sales_support_man_hours=dict(
                items=sales_support_man_hour_list_sorterd,
                memo=man_hour.sales_support_man_hours.memo if man_hour else None,
            ),
            ssap_man_hours=dict(
                meeting=man_hour.ssap_man_hours.meeting if man_hour else None,
                study=man_hour.ssap_man_hours.study if man_hour else None,
                learning=man_hour.ssap_man_hours.learning if man_hour else None,
                new_service=man_hour.ssap_man_hours.new_service if man_hour else None,
                startdash=man_hour.ssap_man_hours.startdash if man_hour else None,
                improvement=man_hour.ssap_man_hours.improvement if man_hour else None,
                ssap=man_hour.ssap_man_hours.ssap if man_hour else None,
                qc=man_hour.ssap_man_hours.qc if man_hour else None,
                accounting=man_hour.ssap_man_hours.accounting if man_hour else None,
                management=man_hour.ssap_man_hours.management if man_hour else None,
                office_work=man_hour.ssap_man_hours.office_work if man_hour else None,
                others=man_hour.ssap_man_hours.others if man_hour else None,
                memo=man_hour.ssap_man_hours.memo if man_hour else None,
            ),
            holidays_man_hours=dict(
                paid_holiday=man_hour.holidays_man_hours.paid_holiday
                if man_hour
                else None,
                holiday=man_hour.holidays_man_hours.holiday if man_hour else None,
                private=man_hour.holidays_man_hours.private if man_hour else None,
                others=man_hour.holidays_man_hours.others if man_hour else None,
                department_others=man_hour.holidays_man_hours.department_others
                if man_hour
                else None,
                memo=man_hour.holidays_man_hours.memo if man_hour else None,
            ),
            summary_man_hour=dict(
                direct=man_hour.summary_man_hour.direct,
                pre=man_hour.summary_man_hour.pre,
                sales=man_hour.summary_man_hour.sales,
                ssap=man_hour.summary_man_hour.ssap,
                others=man_hour.summary_man_hour.others,
                total=man_hour.summary_man_hour.total,
            )
            if man_hour
            else None,
            is_confirm=man_hour.is_confirm if man_hour else False,
            create_id=man_hour.create_id if man_hour else None,
            create_at=man_hour.create_at if man_hour else None,
            update_id=man_hour.update_id if man_hour else None,
            update_at=man_hour.update_at if man_hour else None,
            version=man_hour.version if man_hour else None,
        )

    @staticmethod
    def get_summary_project_supporter_man_hour_status(
        summary_month: str,
        supporter_user_id: str,
        project_id: str,
        current_user: AuthUser,
    ) -> GetSummaryProjectSupporterManHourStatusResponse:
        """Get /man-hours/summary/project/{id}/supporter 案件別工数取得API
        Args:
            id (str): 案件ID
            summary_month (str): 集計月
            supporter_user_id (str): 支援者ユーザーID
            current_user (AuthUser): 認証済みユーザー
        Raises:
            HTTPException: 403
            HTTPException: 404
        Returns:
            GetSummaryProjectSupporterManHourStatusResponse: 取得結果
        """

        # アクセス制御
        if not ManHourService.is_visible_man_hour(
            current_user, current_user.role, project_id
        ):
            # 案件にアクセス不可能
            logger.warning("GetSummaryProjectSupporterManHourStatus forbidden.")
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        data_type = "project_summary#" + project_id

        year_month = summary_month[:4] + "/" + summary_month[4:6]

        # 案件別工数集計を取得
        try:
            project_man_hour = ManHourProjectSummaryModel.get(
                hash_key=data_type, range_key=year_month
            )
        except DoesNotExist:
            logger.warning("GetSummaryProjectSupporterManHourStatus not found.")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 指定された支援者の工数集計を抽出

        # 対面支援時間 本人（当月）
        this_month_supporter_direct_support_man_hour: float = 0
        if project_man_hour.this_month_supporter_direct_support_man_hours is not None:
            for (
                current
            ) in project_man_hour.this_month_supporter_direct_support_man_hours:
                if current.supporter_user_id == supporter_user_id:
                    this_month_supporter_direct_support_man_hour = (
                        current.input_man_hour
                    )

        # 対面支援時間 本人（合計）
        summary_supporter_direct_support_man_hour: float = 0
        if project_man_hour.summary_supporter_direct_support_man_hours is not None:
            for current in project_man_hour.summary_supporter_direct_support_man_hours:
                if current.supporter_user_id == supporter_user_id:
                    summary_supporter_direct_support_man_hour = current.input_man_hour

        # 仕込み時間 本人（当月）
        this_month_supporter_pre_support_man_hour: float = 0
        if project_man_hour.this_month_supporter_pre_support_man_hours is not None:
            for current in project_man_hour.this_month_supporter_pre_support_man_hours:
                if current.supporter_user_id == supporter_user_id:
                    this_month_supporter_pre_support_man_hour = current.input_man_hour

        # 仕込み時間 本人（合計）
        summary_supporter_pre_support_man_hour: float = 0
        if project_man_hour.summary_supporter_pre_support_man_hours is not None:
            for current in project_man_hour.summary_supporter_pre_support_man_hours:
                if current.supporter_user_id == supporter_user_id:
                    summary_supporter_pre_support_man_hour = current.input_man_hour

        # ToDo: 工数アラート関連は実装されるまで0を返却
        summary_direct_support_man_hour_limit = 0
        this_month_direct_support_man_hour_limit = 0
        summary_pre_support_man_hour_limit = 0
        this_month_pre_support_man_hour_limit = 0

        return GetSummaryProjectSupporterManHourStatusResponse(
            project_id=project_man_hour.project_id,
            project_name=project_man_hour.project_name,
            customer_id=project_man_hour.customer_id,
            customer_name=project_man_hour.customer_name,
            supporter_organization_id=project_man_hour.supporter_organization_id,
            service_type=project_man_hour.service_type,
            support_date_from=project_man_hour.support_date_from,
            support_date_to=project_man_hour.support_date_to,
            total_contract_time=round(project_man_hour.total_contract_time, 1),
            this_month_contract_time=round(
                project_man_hour.this_month_contract_time, 1
            ),
            summary_supporter_direct_support_man_hour=round(
                summary_supporter_direct_support_man_hour, 2
            ),
            this_month_supporter_direct_support_man_hour=round(
                this_month_supporter_direct_support_man_hour, 2
            ),
            summary_direct_support_man_hour=round(
                project_man_hour.summary_direct_support_man_hour, 2
            ),
            this_month_direct_support_man_hour=round(
                project_man_hour.this_month_direct_support_man_hour, 2
            ),
            summary_direct_support_man_hour_limit=summary_direct_support_man_hour_limit,
            this_month_direct_support_man_hour_limit=this_month_direct_support_man_hour_limit,
            summary_supporter_pre_support_man_hour=round(
                summary_supporter_pre_support_man_hour, 2
            ),
            this_month_supporter_pre_support_man_hour=round(
                this_month_supporter_pre_support_man_hour, 2
            ),
            summary_pre_support_man_hour=round(
                project_man_hour.summary_pre_support_man_hour, 2
            ),
            this_month_pre_support_man_hour=round(
                project_man_hour.this_month_pre_support_man_hour, 2
            ),
            summary_pre_support_man_hour_limit=summary_pre_support_man_hour_limit,
            this_month_pre_support_man_hour_limit=this_month_pre_support_man_hour_limit,
        )

    @staticmethod
    def update_man_hour_by_mine(
        year: int,
        month: int,
        version: int,
        body: UpdateManHourByMineRequest,
        current_user: AuthUser,
    ) -> UpdateManHourByMineResponse:
        """Put /api/man-hours/mine 支援者別工数更新API
        Args:
            year (int): 対象年
            month (int): 対象月
            version (int): ロックキー（楽観ロック制御）
            body (UpdateManHourByMineRequest): リクエストボディ
            current_user (Behavior, optional): 認証済みのユーザー
        Returns:
            UpdateManHourByMineResponse: 更新結果
        """

        data_type = "supporter#" + current_user.id

        month_str = str(month)
        year_month = str(year) + "/" + month_str.zfill(2)

        # 支援者別工数を取得 (取得できた場合は更新処理)
        try:
            man_hour = ManHourSupporterModel.get(
                hash_key=data_type, range_key=year_month
            )
        except DoesNotExist:
            man_hour = None

        # 楽観ロック制御
        if man_hour and version != man_hour.version:
            logger.warning(
                f"GetSummaryProjectSupporterManHour conflict. request_ver:{version} man_hour_ver: {man_hour.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # 工数集計処理
        total_direct_support: float = 0
        total_pre_support: float = 0
        total_sales_support: float = 0
        total_ssap: float = 0
        total_holidays: float = 0

        # 更新処理
        # Bodyからネストされている項目を抽出 (直接書き込めないため)
        # 直接支援工数
        if len(body.direct_support_man_hours.items) > 0:
            tmp_direct_support_man_hours_list: List[SupportItemsSubAttribute] = []
            for direct_support_man_hour in body.direct_support_man_hours.items:
                tmp_direct_support_man_hours_list.append(
                    SupportItemsSubAttribute(
                        project_id=direct_support_man_hour.project_id,
                        role=direct_support_man_hour.role,
                        service_type=direct_support_man_hour.service_type,
                        input_man_hour=round(direct_support_man_hour.input_man_hour, 2),
                    )
                )
                # 工数集計処理
                total_direct_support = (
                    total_direct_support + direct_support_man_hour.input_man_hour
                )
        else:
            tmp_direct_support_man_hours_list: List = []

        # 仕込支援工数
        if len(body.pre_support_man_hours.items) > 0:
            tmp_pre_support_man_hours_list: List[SupportItemsSubAttribute] = []
            for pre_support_man_hour in body.pre_support_man_hours.items:
                tmp_pre_support_man_hours_list.append(
                    SupportItemsSubAttribute(
                        project_id=pre_support_man_hour.project_id,
                        role=pre_support_man_hour.role,
                        service_type=pre_support_man_hour.service_type,
                        input_man_hour=round(pre_support_man_hour.input_man_hour, 2),
                    )
                )
                # 工数集計処理
                total_pre_support = (
                    total_pre_support + pre_support_man_hour.input_man_hour
                )
        else:
            tmp_pre_support_man_hours_list: List = []

        # 新規・継続案件獲得工数
        if len(body.sales_support_man_hours.items) > 0:
            tmp_sales_support_man_hours_list: List[AcquirementItemsSubAttribute] = []
            for sales_support_man_hour in body.sales_support_man_hours.items:
                tmp_sales_support_man_hours_list.append(
                    AcquirementItemsSubAttribute(
                        project_name=sales_support_man_hour.project_name,
                        customer_name=sales_support_man_hour.customer_name,
                        type=sales_support_man_hour.type,
                        input_man_hour=round(sales_support_man_hour.input_man_hour, 2),
                    )
                )
                # 工数集計処理
                total_sales_support = (
                    total_sales_support + sales_support_man_hour.input_man_hour
                )
        else:
            tmp_sales_support_man_hours_list: List = []

        # 内部業務工数
        for k_category in body.ssap_man_hours:
            # メモはスキップ
            if k_category[0] == "memo":
                continue
            total_ssap = total_ssap + k_category[1]

        # 休暇その他工数
        for k_category in body.holidays_man_hours:
            # メモはスキップ
            if k_category[0] == "memo":
                continue
            total_holidays = total_holidays + k_category[1]

        man_hours_total = (
            total_direct_support
            + total_pre_support
            + total_sales_support
            + total_ssap
            + total_holidays
        )

        if man_hour:
            # 工数の更新
            man_hour.update(
                actions=[
                    ManHourSupporterModel.supporter_name.set(body.supporter_user_name),
                    ManHourSupporterModel.supporter_organization_id.set(
                        body.supporter_organization_id
                    ),
                    ManHourSupporterModel.supporter_organization_name.set(
                        body.supporter_organization_name
                    ),
                    ManHourSupporterModel.direct_support_man_hours.set(
                        DirectSupportManHoursAttribute(
                            items=tmp_direct_support_man_hours_list,
                            memo=body.direct_support_man_hours.memo,
                        )
                    ),
                    ManHourSupporterModel.pre_support_man_hours.set(
                        PreSupportManHoursAttribute(
                            items=tmp_pre_support_man_hours_list,
                            memo=body.pre_support_man_hours.memo,
                        )
                    ),
                    ManHourSupporterModel.sales_support_man_hours.set(
                        SalesSupportManHoursAttribute(
                            items=tmp_sales_support_man_hours_list,
                            memo=body.sales_support_man_hours.memo,
                        )
                    ),
                    ManHourSupporterModel.ssap_man_hours.set(
                        SsapManHoursAttribute(
                            meeting=round(body.ssap_man_hours.meeting, 2),
                            study=round(body.ssap_man_hours.study, 2),
                            learning=round(body.ssap_man_hours.learning, 2),
                            new_service=round(body.ssap_man_hours.new_service, 2),
                            startdash=round(body.ssap_man_hours.startdash, 2),
                            improvement=round(body.ssap_man_hours.improvement, 2),
                            ssap=round(body.ssap_man_hours.ssap, 2),
                            qc=round(body.ssap_man_hours.qc, 2),
                            accounting=round(body.ssap_man_hours.accounting, 2),
                            management=round(body.ssap_man_hours.management, 2),
                            office_work=round(body.ssap_man_hours.office_work, 2),
                            others=round(body.ssap_man_hours.others, 2),
                            memo=body.ssap_man_hours.memo,
                        )
                    ),
                    ManHourSupporterModel.holidays_man_hours.set(
                        HolidaysManHoursAttribute(
                            paid_holiday=round(body.holidays_man_hours.paid_holiday, 2),
                            holiday=round(body.holidays_man_hours.holiday, 2),
                            private=round(body.holidays_man_hours.private, 2),
                            others=round(body.holidays_man_hours.others, 2),
                            department_others=round(
                                body.holidays_man_hours.department_others, 2
                            ),
                            memo=body.holidays_man_hours.memo,
                        )
                    ),
                    ManHourSupporterModel.summary_man_hour.set(
                        SummaryManHourAllowNullAttribute(
                            direct=round(total_direct_support, 2),
                            pre=round(total_pre_support, 2),
                            sales=round(total_sales_support, 2),
                            ssap=round(total_ssap, 2),
                            others=round(total_holidays, 2),
                            total=round(man_hours_total, 2),
                        )
                    ),
                    ManHourSupporterModel.is_confirm.set(body.is_confirm),
                    ManHourSupporterModel.update_id.set(current_user.id),
                    ManHourSupporterModel.update_at.set(datetime.now()),
                ]
            )
        else:
            create_datetime = datetime.now()
            # 工数の新規登録
            man_hour = ManHourSupporterModel(
                data_type=data_type,
                year_month=year_month,
                supporter_user_id=current_user.id,
                supporter_name=body.supporter_user_name,
                supporter_organization_id=body.supporter_organization_id,
                supporter_organization_name=body.supporter_organization_name,
                direct_support_man_hours=DirectSupportManHoursAttribute(
                    items=tmp_direct_support_man_hours_list,
                    memo=body.direct_support_man_hours.memo,
                ),
                pre_support_man_hours=PreSupportManHoursAttribute(
                    items=tmp_pre_support_man_hours_list,
                    memo=body.pre_support_man_hours.memo,
                ),
                sales_support_man_hours=SalesSupportManHoursAttribute(
                    items=tmp_sales_support_man_hours_list,
                    memo=body.sales_support_man_hours.memo,
                ),
                ssap_man_hours=SsapManHoursAttribute(
                    meeting=round(body.ssap_man_hours.meeting, 2),
                    study=round(body.ssap_man_hours.study, 2),
                    learning=round(body.ssap_man_hours.learning, 2),
                    new_service=round(body.ssap_man_hours.new_service, 2),
                    startdash=round(body.ssap_man_hours.startdash, 2),
                    improvement=round(body.ssap_man_hours.improvement, 2),
                    ssap=round(body.ssap_man_hours.ssap, 2),
                    qc=round(body.ssap_man_hours.qc, 2),
                    accounting=round(body.ssap_man_hours.accounting, 2),
                    management=round(body.ssap_man_hours.management, 2),
                    office_work=round(body.ssap_man_hours.office_work, 2),
                    others=round(body.ssap_man_hours.others, 2),
                    memo=body.ssap_man_hours.memo,
                ),
                holidays_man_hours=HolidaysManHoursAttribute(
                    paid_holiday=round(body.holidays_man_hours.paid_holiday, 2),
                    holiday=round(body.holidays_man_hours.holiday, 2),
                    private=round(body.holidays_man_hours.private, 2),
                    others=round(body.holidays_man_hours.others, 2),
                    department_others=round(
                        body.holidays_man_hours.department_others, 2
                    ),
                    memo=body.holidays_man_hours.memo,
                ),
                summary_man_hour=SummaryManHourAllowNullAttribute(
                    direct=round(total_direct_support, 2),
                    pre=round(total_pre_support, 2),
                    sales=round(total_sales_support, 2),
                    ssap=round(total_ssap, 2),
                    others=round(total_holidays, 2),
                    total=round(man_hours_total, 2),
                ),
                is_confirm=body.is_confirm,
                create_id=current_user.id,
                create_at=create_datetime,
                update_at=create_datetime,
            )

            man_hour.save()

        # 工数入力済みの案件のフラグを変更
        target_projects: List[str] = []

        if man_hour.direct_support_man_hours.items:
            for direct_support in man_hour.direct_support_man_hours.items:
                target_projects.append(direct_support.project_id)

        if man_hour.pre_support_man_hours.items:
            for pre_support in man_hour.pre_support_man_hours.items:
                target_projects.append(pre_support.project_id)

        target_projects = sorted(set(target_projects))
        projects = list(ProjectModel.scan())

        for target_project in target_projects:
            for project in projects:
                if target_project == project.id and not project.is_man_hour_input:
                    project.update(
                        actions=[
                            ProjectModel.is_man_hour_input.set(True),
                            ProjectModel.update_at.set(datetime.now()),
                        ]
                    )

        return UpdateManHourByMineResponse(message="OK")
