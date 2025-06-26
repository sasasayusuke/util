from collections import defaultdict
from typing import Dict, List

from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist

from app.core.common_logging import CustomLogger
from app.models.admin import AdminModel
from app.models.man_hour import (
    AcquirementItemsSubAttribute,
    DirectSupportManHoursAttribute,
    HolidaysManHoursAttribute,
    ManHourProjectSummaryModel,
    ManHourServiceTypeSummaryModel,
    ManHourSupporterModel,
    ManHourSupporterOrganizationSummaryModel,
    PreSupportManHoursAttribute,
    SalesSupportManHoursAttribute,
    SsapManHoursAttribute,
    SummaryManHourAllowNullAttribute,
    SupportItemsSubAttribute,
)
from app.models.master import MasterSupporterOrganizationModel
from app.models.master_alert_setting import MasterAlertSettingModel
from app.models.project_karte import ProjectKarteModel
from app.models.user import UserModel
from app.resources.const import (
    DataType,
    MasterDataType,
    SummaryManHourType,
    SupporterRoleTypeName,
    UserRoleType,
)
from app.schemas.man_hour import (
    DirectSupportManHourItems,
    GetManHourBySupporterUserIdResponse,
    GetSummaryManHourTypeHeader,
    GetSummaryManHourTypeResponse,
    GetSummaryProjectManHourAlertResponse,
    GetSummaryProjectManHourAlertsResponse,
    GetSummaryServiceTypesManHoursResponse,
    GetSummarySupporterManHoursResponse,
    GetSummarySupporterOrganizationsManHoursResponse,
    ManHourProjectSummary,
    ManHourServiceTypeProjectItem,
    PreSupportManHourItems,
    SummaryManHours,
    SummarySupporterContractTime,
    SummarySupporterManHour,
    SummarySupporterManHours,
    SupporterOrganizationMonHours,
    SupporterOrganizationTotal,
    Supporters,
    SupportManHours,
    UpdateManHourBySupporterUserIdRequest,
    UpdateManHourBySupporterUserIdResponse,
)
from app.service.common_service.cached_db_items import CachedDbItems
from app.service.project_service import ProjectService
from app.utils.date import get_datetime_now

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
        projects = CachedDbItems.get_projects()

        # 一覧に指定された案件IDと属性値が含まれているか検索
        for project in projects:
            if project_id == project["id"]:
                target_value = project.get(attr)
                break

        return target_value

    @staticmethod
    def get_customer_id_from_cache(project_id: str):
        """案件IDから顧客IDを取得.
            customer_idが解決できない場合、Noneを返却.

        Args:
            project_id (str): 案件ID

        Raise:

        Returns:
            str: 顧客ID
        """
        if not project_id:
            return None

        customer_id = None

        # サービスタイプ区分の一覧を取得
        projects = CachedDbItems.get_projects()

        # 一覧に指定されたサービスタイプIDが含まれているか検索
        for project in projects:
            if project_id == project["id"]:
                customer_id = project["customer_id"]
                break

        return customer_id

    @staticmethod
    def get_customer_name_from_cache(customer_id: str):
        """顧客IDから顧客名を取得.
            customer_nameが解決できない場合、Noneを返却.

        Args:
            customer_id (str): 顧客ID

        Raise:

        Returns:
            str: 顧客名
        """
        if not customer_id:
            return None

        customer_name = None

        # サービスタイプ区分の一覧を取得
        customers = CachedDbItems.get_customers()

        # 一覧に指定されたサービスタイプIDが含まれているか検索
        for customer in customers:
            if customer_id == customer["id"]:
                customer_name = customer["name"]
                break

        return customer_name

    @staticmethod
    def is_visible_man_hour(
        current_user: AdminModel, supporter_organization_id: str
    ) -> bool:
        """支援者組織IDを利用し、支援工数へアクセス可能か判定.
          ユーザ情報は管理ユーザ情報から取得.
            1.制限なし(アクセス可)
              ・営業責任者
              ・稼働率調査事務局
              ・システム管理者
              ・事業者責任者
            2.支援者責任者
              「自身の課以外の案件」：アクセス不可
              上記以外：アクセス可
        Args:
            current_user (AdminModel): ログインユーザ
            supporter_organization_id (str): プロジェクト別工数モデルに登録されている支援者組織ID
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        access_ctrl_flag = True

        for role in current_user.roles:
            # 支援者責任者以外の許可されたロールが付与されている場合は許可
            if role in [
                UserRoleType.SALES_MGR.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True
            elif role == UserRoleType.SUPPORTER_MGR.value[0]:
                # 支援者責任者が組織に所属していない場合は拒否
                if current_user.supporter_organization_id is None:
                    access_ctrl_flag = False
                # 支援者責任者が案件の粗利メイン課に属していない場合は拒否
                elif (
                    supporter_organization_id
                    not in current_user.supporter_organization_id
                ):
                    access_ctrl_flag = False

        return access_ctrl_flag

    def is_visible_project(current_user: AdminModel, project_id: str) -> bool:
        """案件IDを利用し、案件へアクセス可能か判定.
          ユーザ情報は管理ユーザ情報から取得.
            1.制限なし(アクセス可)
              ・営業責任者
              ・稼働率調査事務局
              ・システム管理者
              ・事業者責任者
            2.支援者責任者
              「自身の課以外の案件」：アクセス不可
              上記以外：アクセス可
        Args:
            current_user (AdminModel): ログインユーザ
            project_id (str): 対象の案件ID
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """

        # 案件IDから粗利メイン課となっている組織IDを特定
        supporter_organization_id: str = None
        projects = CachedDbItems.get_projects()

        for project in projects:
            if project_id == project.get("id"):
                supporter_organization_id = project.get("supporter_organization_id")
                break

        access_ctrl_flag = True

        # 支援者責任者が対象の案件の粗利メイン課に属しているか判定
        for role in current_user.roles:
            # 支援者責任者以外の許可されたロールが付与されている場合は許可
            if role in [
                UserRoleType.SALES_MGR.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True
            elif role == UserRoleType.SUPPORTER_MGR.value[0]:
                # 支援者責任者が組織に所属していないか案件に支援者組織情報が登録されていない場合は拒否
                if (
                    current_user.supporter_organization_id is None
                    or supporter_organization_id is None
                ):
                    access_ctrl_flag = False
                # 支援者責任者が案件の粗利メイン課に属していない場合は拒否
                elif (
                    supporter_organization_id
                    not in current_user.supporter_organization_id
                ):
                    access_ctrl_flag = False

        return access_ctrl_flag

    @staticmethod
    def get_summary_supporter_organizations_man_hours(
        year: int, month: int, supporter_organization_id: str, current_user: AdminModel
    ) -> List[GetSummarySupporterOrganizationsManHoursResponse]:
        """Get /man-hours/summary/supporter-organizations 支援者組織(課)別工数取得API

        Args:
            year (int): 対象年
            month (int): 対象月
            supporter_organization_id (str): 支援者組織ID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            List[GetSummarySupporterOrganizationsManHoursResponse]: 取得結果
        """

        data_type_prefix = "supporter_organization_summary#"

        month_str = str(month)
        year_month = str(year) + "/" + month_str.zfill(2)

        range_key_condition = (
            ManHourSupporterOrganizationSummaryModel.data_type.startswith(
                data_type_prefix
            )
        )
        filter_condition = None

        if supporter_organization_id:
            # カンマ区切りで分割
            supporter_organization_iter = iter(supporter_organization_id.split(","))

            current_id = next(supporter_organization_iter)
            filter_condition = (
                ManHourSupporterOrganizationSummaryModel.supporter_organization_id
                == current_id
            )
            for current_id in supporter_organization_iter:
                filter_condition |= (
                    ManHourSupporterOrganizationSummaryModel.supporter_organization_id
                    == current_id
                )

        man_hour_iter = (
            ManHourSupporterOrganizationSummaryModel.year_month_data_type_index.query(
                hash_key=year_month,
                range_key_condition=range_key_condition,
                filter_condition=filter_condition,
            )
        )

        man_hours: List[GetSummarySupporterOrganizationsManHoursResponse] = []
        for man_hour in man_hour_iter:
            # 比率の百分率変換
            monthly_occupancy_rate: int = 0
            monthly_occupancy_total_rate: int = 0
            monthly_work_time_rate: int = 0

            monthly_occupancy_rate = round(man_hour.monthly_occupancy_rate * 100)
            monthly_occupancy_total_rate = round(
                man_hour.monthly_occupancy_total_rate * 100
            )
            monthly_work_time_rate = round(man_hour.monthly_work_time_rate * 100)

            man_hours.append(
                GetSummarySupporterOrganizationsManHoursResponse(
                    supporter_organization_id=man_hour.supporter_organization_id,
                    supporter_organization_name=man_hour.supporter_organization_name,
                    annual_sales=man_hour.annual_sales,
                    monthly_sales=man_hour.monthly_sales,
                    monthly_project_price=man_hour.monthly_project_price,
                    monthly_contract_time=man_hour.monthly_contract_time,
                    monthly_work_time=man_hour.monthly_work_time,
                    monthly_work_time_rate=monthly_work_time_rate,
                    monthly_supporters=man_hour.monthly_supporters,
                    monthly_man_hour=man_hour.monthly_man_hour,
                    monthly_occupancy_rate=monthly_occupancy_rate,
                    monthly_occupancy_total_time=man_hour.monthly_occupancy_total_time,
                    monthly_occupancy_total_rate=monthly_occupancy_total_rate,
                    update_at=man_hour.update_at,
                )
            )

        return man_hours

    @staticmethod
    def get_summary_service_types_man_hours(
        year: int,
        month: int,
        current_user: AdminModel,
    ) -> List[GetSummaryServiceTypesManHoursResponse]:
        """Get /api/man-hours/summary/service-types サービス種別別工数指標取得API

        Args:
            year (int): 対象年
            month (int): 対象月
            supporterOrganizationId (str): 支援者組織ID
            serviceTypeId (str): 支援者組織ID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            List[GetSummaryServiceTypesManHoursResponse]: 取得結果
        """

        data_type_prefix = "service_type_summary#"
        project_data_type_prefix = "project_summary#"

        month_str = str(month)
        year_month = str(year) + "/" + month_str.zfill(2)

        # サービス別支援工数を取得
        service_man_hours = (
            ManHourServiceTypeSummaryModel.year_month_data_type_index.query(
                hash_key=year_month,
                range_key_condition=ManHourServiceTypeSummaryModel.data_type.startswith(
                    data_type_prefix
                ),
            )
        )

        # 支援工数アラートの係数設定を取得
        factor_setting = list(
            next(
                MasterAlertSettingModel.data_type_index.query(
                    hash_key=DataType.ALERT_SETTING
                )
            ).attributes.factor_setting
        )

        # サービス種別別支援工数の集計処理
        service_man_hour_list: List[GetSummaryServiceTypesManHoursResponse] = []

        for service_man_hour in service_man_hours:
            # 支援工数アラートのサービス種別に設定された対面支援工数係数を取得
            direct_support_man_hour_factor: int = None

            for current_service in factor_setting:
                if service_man_hour.service_type_id == current_service.service_type_id:
                    direct_support_man_hour_factor = (
                        current_service.direct_support_man_hour
                    )

            # サービスタイプが同じプロジェクト支援工数を取得
            project_man_hours = (
                ManHourProjectSummaryModel.year_month_data_type_index.query(
                    hash_key=year_month,
                    range_key_condition=ManHourProjectSummaryModel.data_type.startswith(
                        project_data_type_prefix
                    ),
                    filter_condition=ManHourProjectSummaryModel.service_type
                    == service_man_hour.service_type_id,
                )
            )

            # サービスタイプが同じプロジェクト支援工数の集計処理
            projects: List[ManHourServiceTypeProjectItem] = []

            for project_man_hour in project_man_hours:
                # 支援者責任者のアクセス判定、自分の組織以外の案件の場合はスキップ
                if not ManHourService.is_visible_man_hour(
                    current_user=current_user,
                    supporter_organization_id=project_man_hour.supporter_organization_id,
                ):
                    continue

                # totalProcessYPercentの算出
                # totalProcessYPercentはサービス種別別支援工数に設定されているFactorとプロジェクトの当月契約時間をかけた値
                total_process_y_percent = (
                    service_man_hour.factor * project_man_hour.this_month_contract_time
                )

                projects.append(
                    ManHourServiceTypeProjectItem(
                        total_process_y_percent=total_process_y_percent,
                        **project_man_hour.attribute_values,
                    )
                )

            projects.sort(key=lambda x: x.this_month_contract_time, reverse=True)
            projects.sort(key=lambda x: x.contract_type)

            service_man_hour_list.append(
                GetSummaryServiceTypesManHoursResponse(
                    direct_support_man_hour_factor=direct_support_man_hour_factor,
                    projects=projects,
                    **service_man_hour.attribute_values,
                )
            )

        # マスターのサービス種別のソート順のリストを作成
        master_service_type_orders: List[str] = []
        service_type_iter = (
            MasterSupporterOrganizationModel.data_type_order_index.query(
                hash_key=MasterDataType.MASTER_SERVICE_TYPE.value
            )
        )
        for service_type in service_type_iter:
            master_service_type_orders.append(service_type.id)

        # サービスタイプのsort順ごとにソート
        # マスターのサービス種別をorder順に取り出し、実際のデータに一致するサービス種別が存在する場合はソートを行う
        loop_index = 0
        sort_index = 0

        while loop_index < len(master_service_type_orders):
            # 実際のデータのサービス種別のリストを作成
            # ループごとに変更されるのでループ内でリストを再構成する
            actual_service_types: List[str] = []
            for current in service_man_hour_list:
                actual_service_types.append(current.service_type_id)

            # 現在のIndexに対応するマスターのサービス種別IDを取得
            target_service_type_id = master_service_type_orders[loop_index]

            # 現在のマスターのサービス種別IDが、実際のデータに存在する場合はSortIndexの位置へ入れ替える
            if target_service_type_id in actual_service_types:
                target_index = actual_service_types.index(target_service_type_id)
                (
                    service_man_hour_list[sort_index],
                    service_man_hour_list[target_index],
                ) = (
                    service_man_hour_list[target_index],
                    service_man_hour_list[sort_index],
                )
                sort_index += 1

            loop_index += 1

        return service_man_hour_list

    @staticmethod
    def get_summary_project_man_hour_alerts(
        year: int,
        month: int,
        supporter_organization_id: str,
        service_type_id: str,
        current_user: AdminModel,
    ) -> GetSummaryProjectManHourAlertsResponse:
        """Get /api/man-hours/summary/project-man-hour-alerts 案件別工数アラート一覧取得API

        Args:
            year (int): 対象年
            month (int): 対象月
            supporterOrganizationId (str): 支援者組織ID
            serviceTypeId (str): 支援者組織ID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetSummaryProjectManHourAlertsResponse: 取得結果
        """

        data_type_prefix = "project_summary#"

        month_str = str(month)
        year_month = str(year) + "/" + month_str.zfill(2)

        range_key_condition = ManHourProjectSummaryModel.data_type.startswith(
            data_type_prefix
        )
        filter_condition = None
        filter_condition_organization = None
        filter_condition_service = None

        if supporter_organization_id:
            # カンマ区切りで分割
            supporter_organization_iter = iter(supporter_organization_id.split(","))

            current_id = next(supporter_organization_iter)

            filter_condition_organization = (
                ManHourProjectSummaryModel.supporter_organization_id == current_id
            )

            for current_id in supporter_organization_iter:
                filter_condition_organization |= (
                    ManHourProjectSummaryModel.supporter_organization_id == current_id
                )

        if service_type_id:
            # カンマ区切りで分割
            service_type_iter = iter(service_type_id.split(","))

            current_id = next(service_type_iter)

            filter_condition_service = (
                ManHourProjectSummaryModel.service_type == current_id
            )

            for current_id in service_type_iter:
                filter_condition_service |= (
                    ManHourProjectSummaryModel.service_type == current_id
                )

        if (
            filter_condition_organization is not None
            and filter_condition_service is not None
        ):
            filter_condition = filter_condition_organization & filter_condition_service
        elif (
            filter_condition_organization is not None
            and filter_condition_service is None
        ):
            filter_condition = filter_condition_organization
        else:
            filter_condition = filter_condition_service

        man_hour_alerts = ManHourProjectSummaryModel.year_month_data_type_index.query(
            hash_key=year_month,
            range_key_condition=range_key_condition,
            filter_condition=filter_condition,
        )

        summary_this_month_contract_time_list: List[float] = []
        man_hour_alert_list: List[ManHourProjectSummary] = []
        for man_hour_alert in man_hour_alerts:
            # 支援者組織名の取得
            supporter_organization_name = (
                ProjectService.cached_get_supporter_organization_name(
                    man_hour_alert.supporter_organization_id
                )
            )

            # サービスタイプ区分名の取得
            service_type_name = ProjectService.cached_get_service_name(
                service_type=man_hour_alert.service_type, none=True
            )

            man_hour_alert_list.append(
                ManHourProjectSummary(
                    project_id=man_hour_alert.project_id,
                    project_name=man_hour_alert.project_name,
                    customer_id=man_hour_alert.customer_id,
                    customer_name=man_hour_alert.customer_name,
                    supporter_organization_id=man_hour_alert.supporter_organization_id,
                    supporter_organization_name=supporter_organization_name,
                    service_type=man_hour_alert.service_type,
                    service_type_name=service_type_name,
                    support_date_from=man_hour_alert.support_date_from,
                    support_date_to=man_hour_alert.support_date_to,
                    total_contract_time=round(man_hour_alert.total_contract_time, 1),
                    this_month_contract_time=round(
                        man_hour_alert.this_month_contract_time, 1
                    ),
                    total_profit=man_hour_alert.total_profit,
                    this_month_profit=man_hour_alert.this_month_profit,
                    main_supporter_user=man_hour_alert.main_supporter_user,
                    supporter_users=man_hour_alert.supporter_users,
                    summary_direct_support_man_hour=round(
                        man_hour_alert.summary_direct_support_man_hour, 2
                    ),
                    summary_pre_support_man_hour=round(
                        man_hour_alert.summary_pre_support_man_hour, 2
                    ),
                    this_month_direct_support_man_hour=round(
                        man_hour_alert.this_month_direct_support_man_hour, 2
                    ),
                    this_month_pre_support_man_hour=round(
                        man_hour_alert.this_month_pre_support_man_hour, 2
                    ),
                    summary_theoretical_direct_support_man_hour=round(
                        man_hour_alert.summary_theoretical_direct_support_man_hour, 2
                    ),
                    summary_theoretical_pre_support_man_hour=round(
                        man_hour_alert.summary_theoretical_pre_support_man_hour, 2
                    ),
                    this_month_theoretical_direct_support_man_hour=round(
                        man_hour_alert.this_month_theoretical_direct_support_man_hour, 2
                    ),
                    this_month_theoretical_pre_support_man_hour=round(
                        man_hour_alert.this_month_theoretical_pre_support_man_hour, 2
                    ),
                )
            )

            # 当月契約時間を集計
            summary_this_month_contract_time_list.append(
                man_hour_alert.this_month_contract_time
            )

        man_hour_alert_list_sorted = sorted(
            man_hour_alert_list,
            key=lambda x: [x.service_type_name, x.customer_name, x.project_name],
        )

        return GetSummaryProjectManHourAlertsResponse(
            summary_this_month_contract_time=round(
                sum(summary_this_month_contract_time_list), 1
            ),
            projects=man_hour_alert_list_sorted,
        )

    @staticmethod
    def get_summary_project_man_hour_alert(
        year: int,
        month: int,
        project_id: str,
        current_user: AdminModel,
    ) -> GetSummaryProjectManHourAlertResponse:
        """Get /api/man-hours/summary/project-man-hour-alert 案件別工数アラート取得API

        Args:
            year (int): 対象年
            month (int): 対象月
            projectId (str): 案件ID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetSummaryProjectManHourAlertResponse: 取得結果
        """

        data_type = "project_summary#" + project_id

        month_str = str(month)
        year_month = str(year) + "/" + month_str.zfill(2)

        range_key_condition = ManHourProjectSummaryModel.data_type == data_type

        try:
            man_hour_alerts = next(
                ManHourProjectSummaryModel.year_month_data_type_index.query(
                    hash_key=year_month,
                    range_key_condition=range_key_condition,
                )
            )
        except StopIteration:
            logger.warning("GetSummaryProjectManHourAlert not found.")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        # 支援者責任者のアクセス判定、自分の組織以外の案件の場合は403を返却
        if not ManHourService.is_visible_man_hour(
            current_user=current_user,
            supporter_organization_id=man_hour_alerts.supporter_organization_id,
        ):
            logger.warning("GetSummaryProjectManHourAlert forbidden.")
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN)

        # 支援者のIDと名前の一覧を作成
        supporter_users: Dict[str, str] = {}

        try:
            producer_id = man_hour_alerts.main_supporter_user.id
            supporter_users[producer_id] = man_hour_alerts.main_supporter_user.name
        except AttributeError:
            producer_id = None

        try:
            for supporter_user in man_hour_alerts.supporter_users:
                supporter_users[supporter_user.id] = supporter_user.name
        except TypeError:
            pass

        # 支援者別工数の集計処理
        _supporter_man_hours: Dict = {}

        # 当月支援者別直接支援工数を追加
        try:
            for item in man_hour_alerts.this_month_supporter_direct_support_man_hours:
                try:
                    _supporter_man_hours[item.supporter_user_id]
                except KeyError:
                    supporter_user_name: str = supporter_users.get(
                        item.supporter_user_id
                    )
                    role: str = None

                    if item.supporter_user_id == producer_id:
                        role = SupporterRoleTypeName.PRODUCER
                    else:
                        role = SupporterRoleTypeName.ACCELERATOR

                    _supporter_man_hours[item.supporter_user_id] = dict(
                        supporter_user_id=item.supporter_user_id,
                        supporter_user_name=supporter_user_name,
                        role=role,
                    )

                _supporter_man_hours[item.supporter_user_id].update(
                    this_month_supporter_direct_support_man_hours=item.input_man_hour,
                )
        except TypeError:
            pass

        # 当月支援者別仕込支援工数を追加
        try:
            for item in man_hour_alerts.this_month_supporter_pre_support_man_hours:
                try:
                    _supporter_man_hours[item.supporter_user_id]
                except KeyError:
                    supporter_user_name: str = supporter_users.get(
                        item.supporter_user_id
                    )
                    role: str = None

                    if item.supporter_user_id == producer_id:
                        role = SupporterRoleTypeName.PRODUCER
                    else:
                        role = SupporterRoleTypeName.ACCELERATOR

                    _supporter_man_hours[item.supporter_user_id] = dict(
                        supporter_user_id=item.supporter_user_id,
                        supporter_user_name=supporter_user_name,
                        role=role,
                    )

                _supporter_man_hours[item.supporter_user_id].update(
                    this_month_supporter_pre_support_man_hours=item.input_man_hour,
                )
        except TypeError:
            pass

        # 累積支援者別直接支援工数を追加
        try:
            for item in man_hour_alerts.summary_supporter_direct_support_man_hours:
                try:
                    _supporter_man_hours[item.supporter_user_id]
                except KeyError:
                    supporter_user_name: str = supporter_users.get(
                        item.supporter_user_id
                    )
                    role: str = None

                    if item.supporter_user_id == producer_id:
                        role = SupporterRoleTypeName.PRODUCER
                    else:
                        role = SupporterRoleTypeName.ACCELERATOR

                    _supporter_man_hours[item.supporter_user_id] = dict(
                        supporter_user_id=item.supporter_user_id,
                        supporter_user_name=supporter_user_name,
                        role=role,
                    )

                _supporter_man_hours[item.supporter_user_id].update(
                    summary_supporter_direct_support_man_hours=item.input_man_hour,
                )
        except TypeError:
            pass

        # 累積支援者別仕込支援工数を追加
        try:
            for item in man_hour_alerts.summary_supporter_pre_support_man_hours:
                try:
                    _supporter_man_hours[item.supporter_user_id]
                except KeyError:
                    supporter_user_name: str = supporter_users.get(
                        item.supporter_user_id
                    )
                    role: str = None

                    if item.supporter_user_id == producer_id:
                        role = SupporterRoleTypeName.PRODUCER
                    else:
                        role = SupporterRoleTypeName.ACCELERATOR

                    _supporter_man_hours[item.supporter_user_id] = dict(
                        supporter_user_id=item.supporter_user_id,
                        supporter_user_name=supporter_user_name,
                        role=role,
                    )

                _supporter_man_hours[item.supporter_user_id].update(
                    summary_supporter_pre_support_man_hours=item.input_man_hour,
                )
        except TypeError:
            pass

        # アクセラレーターの支援工数を追加しプロデューサーを先頭へ移動
        supporter_man_hours: List[SupportManHours] = []

        for supporter_man_hour in _supporter_man_hours.values():
            this_month_supporter_direct_support_man_hours = supporter_man_hour.get(
                "this_month_supporter_direct_support_man_hours"
            )
            this_month_supporter_pre_support_man_hours = supporter_man_hour.get(
                "this_month_supporter_pre_support_man_hours"
            )
            summary_supporter_direct_support_man_hours = supporter_man_hour.get(
                "summary_supporter_direct_support_man_hours"
            )
            summary_supporter_pre_support_man_hours = supporter_man_hour.get(
                "summary_supporter_pre_support_man_hours"
            )

            if (
                supporter_man_hour.get("supporter_user_id") == producer_id
                and producer_id is not None
            ):
                supporter_man_hours.insert(
                    0,
                    SupportManHours(
                        supporter_user_id=supporter_man_hour["supporter_user_id"],
                        supporter_user_name=supporter_man_hour["supporter_user_name"],
                        role=supporter_man_hour["role"],
                        this_month_supporter_direct_support_man_hours=round(
                            this_month_supporter_direct_support_man_hours, 2
                        )
                        if this_month_supporter_direct_support_man_hours
                        else None,
                        this_month_supporter_pre_support_man_hours=round(
                            this_month_supporter_pre_support_man_hours, 2
                        )
                        if this_month_supporter_pre_support_man_hours
                        else None,
                        summary_supporter_direct_support_man_hours=round(
                            summary_supporter_direct_support_man_hours, 2
                        )
                        if summary_supporter_direct_support_man_hours
                        else None,
                        summary_supporter_pre_support_man_hours=round(
                            summary_supporter_pre_support_man_hours, 2
                        )
                        if summary_supporter_pre_support_man_hours
                        else None,
                    ),
                )
            else:
                supporter_man_hours.append(
                    SupportManHours(
                        supporter_user_id=supporter_man_hour["supporter_user_id"],
                        supporter_user_name=supporter_man_hour["supporter_user_name"],
                        role=supporter_man_hour["role"],
                        this_month_supporter_direct_support_man_hours=round(
                            this_month_supporter_direct_support_man_hours, 2
                        )
                        if this_month_supporter_direct_support_man_hours
                        else None,
                        this_month_supporter_pre_support_man_hours=round(
                            this_month_supporter_pre_support_man_hours, 2
                        )
                        if this_month_supporter_pre_support_man_hours
                        else None,
                        summary_supporter_direct_support_man_hours=round(
                            summary_supporter_direct_support_man_hours, 2
                        )
                        if summary_supporter_direct_support_man_hours
                        else None,
                        summary_supporter_pre_support_man_hours=round(
                            summary_supporter_pre_support_man_hours, 2
                        )
                        if summary_supporter_pre_support_man_hours
                        else None,
                    )
                )

        return GetSummaryProjectManHourAlertResponse(
            man_hours=supporter_man_hours, **man_hour_alerts.attribute_values
        )

    @staticmethod
    def get_man_hour_by_supporter_user_id(
        year: int,
        month: int,
        supporter_user_id: str,
        current_user: AdminModel,
    ) -> GetManHourBySupporterUserIdResponse:
        """Get /api/man-hours/{supporter_user_id} 支援者別工数取得API

        Args:
            year (int): 対象年
            month (int): 対象月
            supporter_user_id (str): 支援者ユーザーID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetManHourBySupporterUserIdResponse: 取得結果
        """

        data_type = "supporter#" + supporter_user_id

        month_str = str(month)
        year_month = str(year) + "/" + month_str.zfill(2)

        # 支援者別工数を取得
        try:
            man_hour = ManHourSupporterModel.get(
                hash_key=data_type, range_key=year_month
            )
        except DoesNotExist:
            logger.warning(" GetManHourBySupporterUserId not found.")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        direct_support_man_hour_list: List[DirectSupportManHourItems] = []
        for item in man_hour.direct_support_man_hours.items:
            # 支援者責任者のアクセス判定、自分の組織以外の案件の場合はスキップ
            if not ManHourService.is_visible_project(
                current_user=current_user,
                project_id=item.project_id,
            ):
                continue

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

            # 支援者別工数内のプロジェクトIDから案件カルテに入力されている当月分の工数を取得
            start_date = f"{year_month}/01"
            end_date = f"{year_month}/31"
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

        pre_support_man_hour_list: List[PreSupportManHourItems] = []
        for item in man_hour.pre_support_man_hours.items:
            # 支援者責任者のアクセス判定、自分の組織以外の案件の場合はスキップ
            if not ManHourService.is_visible_project(
                current_user=current_user,
                project_id=item.project_id,
            ):
                continue

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

        # 案件名の昇順に並べ替え
        direct_support_man_hour_list_sorted = sorted(
            direct_support_man_hour_list, key=lambda x: x.project_name
        )
        pre_support_man_hour_list_sorted = sorted(
            pre_support_man_hour_list, key=lambda x: x.project_name
        )
        sales_support_man_hour_list_sorted = sorted(
            man_hour.sales_support_man_hours.items, key=lambda x: x.project_name
        )

        return GetManHourBySupporterUserIdResponse(
            year_month=man_hour.year_month,
            supporter_user_id=man_hour.supporter_user_id,
            supporter_name=man_hour.supporter_name,
            supporter_organization_id=man_hour.supporter_organization_id,
            supporter_organization_name=man_hour.supporter_organization_name,
            direct_support_man_hours=dict(
                items=direct_support_man_hour_list_sorted,
                memo=man_hour.direct_support_man_hours.memo,
            ),
            pre_support_man_hours=dict(
                items=pre_support_man_hour_list_sorted,
                memo=man_hour.pre_support_man_hours.memo,
            ),
            sales_support_man_hours=dict(
                items=sales_support_man_hour_list_sorted,
                memo=man_hour.sales_support_man_hours.memo,
            ),
            ssap_man_hours=dict(
                meeting=round(man_hour.ssap_man_hours.meeting, 2),
                study=round(man_hour.ssap_man_hours.study, 2),
                learning=round(man_hour.ssap_man_hours.learning, 2),
                new_service=round(man_hour.ssap_man_hours.new_service, 2),
                startdash=round(man_hour.ssap_man_hours.startdash, 2),
                improvement=round(man_hour.ssap_man_hours.improvement, 2),
                ssap=round(man_hour.ssap_man_hours.ssap, 2),
                qc=round(man_hour.ssap_man_hours.qc, 2),
                accounting=round(man_hour.ssap_man_hours.accounting, 2),
                management=round(man_hour.ssap_man_hours.management, 2),
                office_work=round(man_hour.ssap_man_hours.office_work, 2),
                others=round(man_hour.ssap_man_hours.others, 2),
                memo=man_hour.ssap_man_hours.memo,
            ),
            holidays_man_hours=dict(
                paid_holiday=round(man_hour.holidays_man_hours.paid_holiday, 2),
                holiday=round(man_hour.holidays_man_hours.holiday, 2),
                private=round(man_hour.holidays_man_hours.private, 2),
                others=round(man_hour.holidays_man_hours.others, 2),
                department_others=round(
                    man_hour.holidays_man_hours.department_others, 2
                ),
                memo=man_hour.holidays_man_hours.memo,
            ),
            summary_man_hour=dict(
                direct=round(man_hour.summary_man_hour.direct, 2),
                pre=round(man_hour.summary_man_hour.pre, 2),
                sales=round(man_hour.summary_man_hour.sales, 2),
                ssap=round(man_hour.summary_man_hour.ssap, 2),
                others=round(man_hour.summary_man_hour.others, 2),
                total=round(man_hour.summary_man_hour.total, 2),
            ),
            is_confirm=man_hour.is_confirm,
            create_id=man_hour.create_id,
            create_at=man_hour.create_at,
            update_id=man_hour.update_id,
            update_at=man_hour.update_at,
            version=man_hour.version,
        )

    @staticmethod
    def update_man_hour_by_supporter_user_id(
        year: int,
        month: int,
        version: int,
        body: UpdateManHourBySupporterUserIdRequest,
        supporter_user_id: str,
        current_user: AdminModel,
    ) -> UpdateManHourBySupporterUserIdResponse:
        """Put /api/man-hours/{supporter_user_id} 支援者別工数更新API

        Args:
            year (int): 対象年
            month (int): 対象月
            version (int): ロックキー（楽観ロック制御）
            supporter_user_id (str): 支援者ユーザーID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            UpdateManHourBySupporterUserIdResponse: 更新結果
        """

        data_type = "supporter#" + supporter_user_id

        month_str = str(month)
        year_month = str(year) + "/" + month_str.zfill(2)

        # 支援者別工数を取得
        try:
            man_hour = ManHourSupporterModel.get(
                hash_key=data_type, range_key=year_month
            )
        except DoesNotExist:
            logger.warning("UpdateManHourBySupporterUserId not found.")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 楽観ロック制御
        if version != man_hour.version:
            logger.warning(
                f"UpdateManHourBySupporterUserId conflict. request_ver:{version} man_hour_ver: {man_hour.version}"
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

        # 対象の支援者名と支援者組織名を取得
        supporter_name = UserModel.get(
            hash_key=supporter_user_id, range_key=DataType.USER
        ).name

        supporter_organization_name = MasterSupporterOrganizationModel.get(
            hash_key=body.supporter_organization_id,
            range_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
        ).value

        # 工数の更新
        man_hour.update(
            actions=[
                ManHourSupporterModel.supporter_name.set(supporter_name),
                ManHourSupporterModel.supporter_organization_id.set(
                    body.supporter_organization_id
                ),
                ManHourSupporterModel.supporter_organization_name.set(
                    supporter_organization_name
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
                ManHourSupporterModel.update_at.set(get_datetime_now()),
            ]
        )

        return UpdateManHourBySupporterUserIdResponse(message="OK")

    @staticmethod
    def get_summary_man_hour_type(
        year: int,
        month: int,
        current_user: AdminModel,
    ) -> GetManHourBySupporterUserIdResponse:
        """Get /api/man-hours/{supporter_user_id} 支援者別工数取得API

        Args:
            year (int): 対象年
            month (int): 対象月
            supporter_user_id (str): 支援者ユーザーID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetManHourBySupporterUserIdResponse: 取得結果
        """

        data_type_prefix = "supporter#"

        month_str = str(month)
        year_month = str(year) + "/" + month_str.zfill(2)

        # 支援者別工数を取得
        supporter_man_hours = ManHourSupporterModel.year_month_data_type_index.query(
            hash_key=year_month,
            range_key_condition=ManHourSupporterModel.data_type.startswith(
                data_type_prefix
            ),
        )

        # ####################################
        # 集計(工数種別別・課別・ロール別に支援者工数を集計)
        # ####################################
        # デフォルトだとDictのネストに直接書き込めないのでその対処

        """対面支援と支援仕込工数はこの形式で集計
        {
            project_id: {
                "project_name": "案件名",
                "customer_name": "顧客名",
                "service_type_name": "サービス種別名"
                role_name: {
                    supporter_ornization_id: {
                        "supporter_ornization_name": "支援者組織名",
                        "total": "課のロールの工数の合計",
                        "supporter": {
                            supporter_user_id: {
                                "supporter_name": "支援者名",
                                "total_s": "支援者ごとの工数の合計"
                            }
                        }
                    }
                }
            }
        }
        """
        summary_direct_support_man_hours = defaultdict(
            lambda: defaultdict(
                lambda: defaultdict(lambda: defaultdict(lambda: defaultdict(dict)))
            )
        )
        summary_pre_support_man_hours = defaultdict(
            lambda: defaultdict(
                lambda: defaultdict(lambda: defaultdict(lambda: defaultdict(dict)))
            )
        )

        """商談/提案準備工数はこの形式で集計
        {
            project_name: {
                "customer_name": "顧客名",
                "supporter_organizations": {
                    supporter_organization_id: {
                        "supporter_organization_name": "支援者組織名",
                        "total": "課のロールの工数の合計",
                        "supporter": {
                            supporter_user_id: {
                                supporter_name: "支援者ごとの工数の合計",
                                "total_s": "支援者ごとの工数の合計"
                            }
                        }
                    }
                }
            }
        }
        """
        summary_sales_support_man_hours = defaultdict(
            lambda: defaultdict(
                lambda: defaultdict(lambda: defaultdict(lambda: defaultdict(dict)))
            )
        )

        """内部業務と休憩その他工数はこの形式で集計
        {
            category_name: {
                supporter_ornization_id: {
                    "supporter_ornization_name": "支援者組織名",
                    "total": "課のロールの工数の合計",
                    "supporter": {
                        supporter_user_id: {
                            supporter_name: "支援者ごとの工数の合計",
                            "total_s": "支援者ごとの工数の合計"
                        }
                    }
                }
            }
        }
        """
        summary_ssap_man_hours = defaultdict(
            lambda: defaultdict(lambda: defaultdict(lambda: defaultdict(dict)))
        )
        summary_holidays_man_hours = defaultdict(
            lambda: defaultdict(lambda: defaultdict(lambda: defaultdict(dict)))
        )

        """ヘッダーはこの形式で集計
        {
            supporter_organization_id: {
                "supporter_ornization_name": "支援者組織名",
                "header_total_o": "課ごとの全工数種別の工数合計"
                supporters: {
                    supporter_user_id: {
                        supporter_name: "支援者ごとの工数の合計",
                        "header_total_s": "支援者ごとの全工数種別の工数合計"
                    }
                }
            }
        }
        """
        summary_man_hour_header = defaultdict(
            lambda: defaultdict(lambda: defaultdict(dict))
        )

        # ####################################
        # 工数種別ごとの集計処理
        # ####################################
        for supporter_man_hour in supporter_man_hours:
            # 支援者責任者のアクセス制御 (自身の課の支援者工数のみ参照可))
            if not ManHourService.is_visible_man_hour(
                current_user=current_user,
                supporter_organization_id=supporter_man_hour.supporter_organization_id,
            ):
                continue

            supporter_user_id = supporter_man_hour.supporter_user_id
            supporter_name = supporter_man_hour.supporter_name
            supporter_organization_id = supporter_man_hour.supporter_organization_id
            supporter_organization_name = supporter_man_hour.supporter_organization_name

            # ####################################
            # 対面支援工数
            # ####################################
            for (
                direct_support_man_hour
            ) in supporter_man_hour.direct_support_man_hours.items:
                project_id = direct_support_man_hour.project_id
                project_name = ManHourService.get_project_info_from_cache(
                    direct_support_man_hour.project_id
                )
                customer_id = ManHourService.get_customer_id_from_cache(
                    direct_support_man_hour.project_id
                )
                customer_name = ManHourService.get_customer_name_from_cache(customer_id)
                role = direct_support_man_hour.role
                service_type_name = ProjectService.cached_get_service_name(
                    service_type=direct_support_man_hour.service_type, none=True
                )
                input_man_hour = direct_support_man_hour.input_man_hour

                # 案件IDがまだ存在しない場合に案件名と顧客名とサービス種別名を追加
                if summary_direct_support_man_hours[project_id] == {}:
                    summary_direct_support_man_hours[project_id][
                        "project_name"
                    ] = project_name
                    summary_direct_support_man_hours[project_id][
                        "customer_name"
                    ] = customer_name
                    summary_direct_support_man_hours[project_id][
                        "service_type_name"
                    ] = service_type_name

                # 課ごとの工数の合計を集計
                current_total = summary_direct_support_man_hours[project_id][role][
                    supporter_organization_id
                ]["total"]

                if current_total == {}:
                    current_total = 0

                summary_direct_support_man_hours[project_id][role][
                    supporter_organization_id
                ]["total"] = (current_total + input_man_hour)

                # 課名の追加
                summary_direct_support_man_hours[project_id][role][
                    supporter_organization_id
                ]["supporter_organization_name"] = supporter_organization_name

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_supporter_total = summary_direct_support_man_hours[
                        project_id
                    ][role][supporter_organization_id]["supporter"][supporter_user_id][
                        "total_s"
                    ]
                except KeyError:
                    current_supporter_total = 0

                summary_direct_support_man_hours[project_id][role][
                    supporter_organization_id
                ]["supporter"][supporter_user_id]["total_s"] = (
                    current_supporter_total + input_man_hour
                )

                try:
                    summary_direct_support_man_hours[project_id][role][
                        supporter_organization_id
                    ]["supporter"][supporter_user_id]["supporter_name"]
                except KeyError:
                    summary_direct_support_man_hours[project_id][role][
                        supporter_organization_id
                    ]["supporter"][supporter_user_id]["supporter_name"] = supporter_name

                # ####################################
                # ヘッダー集計
                # ####################################
                # 支援者組織IDがまだ存在しない場合に支援者組織名を追加
                if summary_man_hour_header[supporter_organization_id] == {}:
                    summary_man_hour_header[supporter_organization_id][
                        "supporter_organization_name"
                    ] = supporter_organization_name

                # 課ごとの工数の合計を集計
                current_header_total_s = summary_man_hour_header[
                    supporter_organization_id
                ]["header_total_o"]

                if current_header_total_s == {}:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["header_total_o"] = (
                    current_header_total_s + input_man_hour
                )

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_header_total_s = summary_man_hour_header[
                        supporter_organization_id
                    ]["supporters"][supporter_user_id]["header_total_s"]
                except KeyError:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["supporters"][
                    supporter_user_id
                ]["header_total_s"] = (current_header_total_s + input_man_hour)

                try:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"]
                except KeyError:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"] = supporter_name

            # ####################################
            # 仕込支援工数
            # ####################################
            for pre_support_man_hour in supporter_man_hour.pre_support_man_hours.items:
                project_id = pre_support_man_hour.project_id
                project_name = ManHourService.get_project_info_from_cache(
                    pre_support_man_hour.project_id
                )
                customer_id = ManHourService.get_customer_id_from_cache(
                    pre_support_man_hour.project_id
                )
                customer_name = ManHourService.get_customer_name_from_cache(customer_id)
                role = pre_support_man_hour.role
                service_type_name = ProjectService.cached_get_service_name(
                    service_type=pre_support_man_hour.service_type, none=True
                )
                input_man_hour = pre_support_man_hour.input_man_hour

                # 案件名がまだ存在しない場合に顧客名とサービス種別名を追加
                if summary_pre_support_man_hours[project_id] == {}:
                    summary_pre_support_man_hours[project_id][
                        "project_name"
                    ] = project_name
                    summary_pre_support_man_hours[project_id][
                        "customer_name"
                    ] = customer_name
                    summary_pre_support_man_hours[project_id][
                        "service_type_name"
                    ] = service_type_name

                # 課ごとの工数の合計を集計
                current_total = summary_pre_support_man_hours[project_id][role][
                    supporter_organization_id
                ]["total"]

                if current_total == {}:
                    current_total = 0

                summary_pre_support_man_hours[project_id][role][
                    supporter_organization_id
                ]["total"] = (current_total + input_man_hour)

                # 課名の追加
                summary_pre_support_man_hours[project_id][role][
                    supporter_organization_id
                ]["supporter_organization_name"] = supporter_organization_name

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_supporter_total = summary_pre_support_man_hours[project_id][
                        role
                    ][supporter_organization_id]["supporter"][supporter_user_id][
                        "total_s"
                    ]
                except KeyError:
                    current_supporter_total = 0

                summary_pre_support_man_hours[project_id][role][
                    supporter_organization_id
                ]["supporter"][supporter_user_id]["total_s"] = (
                    current_supporter_total + input_man_hour
                )

                try:
                    summary_pre_support_man_hours[project_id][role][
                        supporter_organization_id
                    ]["supporter"][supporter_user_id]["supporter_name"]
                except KeyError:
                    summary_pre_support_man_hours[project_id][role][
                        supporter_organization_id
                    ]["supporter"][supporter_user_id]["supporter_name"] = supporter_name

                # ####################################
                # ヘッダー集計
                # ####################################
                # 支援者組織IDがまだ存在しない場合に支援者組織名を追加
                if summary_man_hour_header[supporter_organization_id] == {}:
                    summary_man_hour_header[supporter_organization_id][
                        "supporter_organization_name"
                    ] = supporter_organization_name

                # 課ごとの工数の合計を集計
                current_header_total_s = summary_man_hour_header[
                    supporter_organization_id
                ]["header_total_o"]

                if current_header_total_s == {}:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["header_total_o"] = (
                    current_header_total_s + input_man_hour
                )

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_header_total_s = summary_man_hour_header[
                        supporter_organization_id
                    ]["supporters"][supporter_user_id]["header_total_s"]
                except KeyError:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["supporters"][
                    supporter_user_id
                ]["header_total_s"] = (current_header_total_s + input_man_hour)

                try:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"]
                except KeyError:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"] = supporter_name

            # ####################################
            # 商談/提案準備工数
            # ####################################
            for (
                sales_support_man_hour
            ) in supporter_man_hour.sales_support_man_hours.items:
                project_name = sales_support_man_hour.project_name
                customer_name = sales_support_man_hour.customer_name
                input_man_hour = sales_support_man_hour.input_man_hour

                # 案件名がまだ存在しない場合に顧客名を追加
                if summary_sales_support_man_hours[project_name] == {}:
                    summary_sales_support_man_hours[project_name][
                        "customer_name"
                    ] = customer_name

                # 課ごとの工数の合計を集計
                current_total = summary_sales_support_man_hours[project_name][
                    "supporter_organizations"
                ][supporter_organization_id]["total"]

                if current_total == {}:
                    current_total = 0

                summary_sales_support_man_hours[project_name][
                    "supporter_organizations"
                ][supporter_organization_id]["total"] = (current_total + input_man_hour)

                summary_sales_support_man_hours[project_name][
                    "supporter_organizations"
                ][supporter_organization_id][
                    "supporter_organization_name"
                ] = supporter_organization_name

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_supporter_total = summary_sales_support_man_hours[
                        project_name
                    ]["supporter_organizations"][supporter_organization_id][
                        "supporter"
                    ][
                        supporter_user_id
                    ][
                        "total_s"
                    ]
                except KeyError:
                    current_supporter_total = 0

                summary_sales_support_man_hours[project_name][
                    "supporter_organizations"
                ][supporter_organization_id]["supporter"][supporter_user_id][
                    "total_s"
                ] = (
                    current_supporter_total + input_man_hour
                )

                try:
                    summary_sales_support_man_hours[project_name][
                        "supporter_organizations"
                    ][supporter_organization_id]["supporter"][supporter_user_id][
                        "supporter_name"
                    ]
                except KeyError:
                    summary_sales_support_man_hours[project_name][
                        "supporter_organizations"
                    ][supporter_organization_id]["supporter"][supporter_user_id][
                        "supporter_name"
                    ] = supporter_name

                # ####################################
                # ヘッダー集計
                # ####################################
                # 支援者組織IDがまだ存在しない場合に支援者組織名を追加
                if summary_man_hour_header[supporter_organization_id] == {}:
                    summary_man_hour_header[supporter_organization_id][
                        "supporter_organization_name"
                    ] = supporter_organization_name

                # 課ごとの工数の合計を集計
                current_header_total_s = summary_man_hour_header[
                    supporter_organization_id
                ]["header_total_o"]

                if current_header_total_s == {}:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["header_total_o"] = (
                    current_header_total_s + input_man_hour
                )

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_header_total_s = summary_man_hour_header[
                        supporter_organization_id
                    ]["supporters"][supporter_user_id]["header_total_s"]
                except KeyError:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["supporters"][
                    supporter_user_id
                ]["header_total_s"] = (current_header_total_s + input_man_hour)

                try:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"]
                except KeyError:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"] = supporter_name

            # ####################################
            # 内部業務工数
            # ####################################
            for k_category in supporter_man_hour.ssap_man_hours:
                # メモはスキップ
                if k_category == "memo":
                    continue

                # 課ごとの工数の合計を集計
                current_total = summary_ssap_man_hours[k_category][
                    supporter_organization_id
                ]["total"]

                if current_total == {}:
                    current_total = 0

                summary_ssap_man_hours[k_category][supporter_organization_id][
                    "total"
                ] = (current_total + supporter_man_hour.ssap_man_hours[k_category])

                summary_ssap_man_hours[k_category][supporter_organization_id][
                    "supporter_organization_name"
                ] = supporter_organization_name

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_supporter_total = summary_ssap_man_hours[k_category][
                        supporter_organization_id
                    ]["supporter"][supporter_user_id]["total_s"]
                except KeyError:
                    current_supporter_total = 0

                summary_ssap_man_hours[k_category][supporter_organization_id][
                    "supporter"
                ][supporter_user_id]["total_s"] = (
                    current_supporter_total
                    + supporter_man_hour.ssap_man_hours[k_category]
                )

                try:
                    summary_ssap_man_hours[k_category][supporter_organization_id][
                        "supporter"
                    ][supporter_user_id]["supporter_name"]
                except KeyError:
                    summary_ssap_man_hours[k_category][supporter_organization_id][
                        "supporter"
                    ][supporter_user_id]["supporter_name"] = supporter_name

                # ####################################
                # ヘッダー集計
                # ####################################
                # 支援者組織IDがまだ存在しない場合に支援者組織名を追加
                if summary_man_hour_header[supporter_organization_id] == {}:
                    summary_man_hour_header[supporter_organization_id][
                        "supporter_organization_name"
                    ] = supporter_organization_name

                # 課ごとの工数の合計を集計
                current_header_total_s = summary_man_hour_header[
                    supporter_organization_id
                ]["header_total_o"]

                if current_header_total_s == {}:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["header_total_o"] = (
                    current_header_total_s
                    + supporter_man_hour.ssap_man_hours[k_category]
                )

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_header_total_s = summary_man_hour_header[
                        supporter_organization_id
                    ]["supporters"][supporter_user_id]["header_total_s"]
                except KeyError:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["supporters"][
                    supporter_user_id
                ]["header_total_s"] = (
                    current_header_total_s
                    + supporter_man_hour.ssap_man_hours[k_category]
                )

                try:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"]
                except KeyError:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"] = supporter_name

            # ####################################
            # 休憩その他工数
            # ####################################
            for k_category in supporter_man_hour.holidays_man_hours:
                # メモはスキップ
                if k_category == "memo":
                    continue

                # 課ごとの工数の合計を集計
                current_total = summary_holidays_man_hours[k_category][
                    supporter_organization_id
                ]["total"]

                if current_total == {}:
                    current_total = 0

                summary_holidays_man_hours[k_category][supporter_organization_id][
                    "total"
                ] = (current_total + supporter_man_hour.holidays_man_hours[k_category])

                summary_holidays_man_hours[k_category][supporter_organization_id][
                    "supporter_organization_name"
                ] = supporter_organization_name

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_supporter_total = summary_holidays_man_hours[k_category][
                        supporter_organization_id
                    ]["supporter"][supporter_user_id]["total_s"]
                except KeyError:
                    current_supporter_total = 0

                summary_holidays_man_hours[k_category][supporter_organization_id][
                    "supporter"
                ][supporter_user_id]["total_s"] = (
                    current_supporter_total
                    + supporter_man_hour.holidays_man_hours[k_category]
                )

                try:
                    summary_holidays_man_hours[k_category][supporter_organization_id][
                        "supporter"
                    ][supporter_user_id]["supporter_name"]
                except KeyError:
                    summary_holidays_man_hours[k_category][supporter_organization_id][
                        "supporter"
                    ][supporter_user_id]["supporter_name"] = supporter_name

                # ####################################
                # ヘッダー集計
                # ####################################
                # 支援者組織IDがまだ存在しない場合に支援者組織名を追加
                if summary_man_hour_header[supporter_organization_id] == {}:
                    summary_man_hour_header[supporter_organization_id][
                        "supporter_organization_name"
                    ] = supporter_organization_name

                # 課ごとの工数の合計を集計
                current_header_total_s = summary_man_hour_header[
                    supporter_organization_id
                ]["header_total_o"]

                if current_header_total_s == {}:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["header_total_o"] = (
                    current_header_total_s
                    + supporter_man_hour.holidays_man_hours[k_category]
                )

                # 支援者名ごとの工数の合計を集計 (defaultdictで指定していないのでnullの場合はKeyErrorが返る)
                try:
                    current_header_total_s = summary_man_hour_header[
                        supporter_organization_id
                    ]["supporters"][supporter_user_id]["header_total_s"]
                except KeyError:
                    current_header_total_s = 0

                summary_man_hour_header[supporter_organization_id]["supporters"][
                    supporter_user_id
                ]["header_total_s"] = (
                    current_header_total_s
                    + supporter_man_hour.holidays_man_hours[k_category]
                )

                try:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"]
                except KeyError:
                    summary_man_hour_header[supporter_organization_id]["supporters"][
                        supporter_user_id
                    ]["supporter_name"] = supporter_name

        # ####################################
        # レスポンス生成
        # ####################################
        res_man_hours: List[SummaryManHours] = []

        # 対面支援工数
        for k_project, v in summary_direct_support_man_hours.items():
            project_id = k_project
            project_name = v["project_name"]
            customer_name = v["customer_name"]
            service_type_name = v["service_type_name"]

            # プロデューサーロールの集計
            if v[SupporterRoleTypeName.PRODUCER]:
                res_direct_support_organization_totals_list: List[
                    SupporterOrganizationTotal
                ] = []
                res_direct_support_organization_supporters_list: List[
                    SupporterOrganizationMonHours
                ] = []

                for k_organization, v2 in v[SupporterRoleTypeName.PRODUCER].items():
                    res_direct_support_organization_supporter_totals_list: List[
                        Supporters
                    ] = []

                    res_direct_support_organization_totals_list.append(
                        SupporterOrganizationTotal(
                            supporter_organization_id=k_organization,
                            supporter_organization_name=v2[
                                "supporter_organization_name"
                            ],
                            man_hour=v2["total"],
                        )
                    )

                    for k_supporter_user_id, v3 in v2["supporter"].items():
                        res_direct_support_organization_supporter_totals_list.append(
                            Supporters(
                                id=k_supporter_user_id,
                                name=v3["supporter_name"],
                                man_hour=v3["total_s"],
                            )
                        )

                    res_direct_support_organization_supporters_list.append(
                        SupporterOrganizationMonHours(
                            supporter_organization_id=k_organization,
                            supporter_organization_name=v2[
                                "supporter_organization_name"
                            ],
                            supporters=res_direct_support_organization_supporter_totals_list,
                        )
                    )

                res_man_hours.append(
                    SummaryManHours(
                        man_hour_type_name=SummaryManHourType.DIRECT_SUPPORT.value,
                        sub_name=f"{customer_name}／{project_name}",
                        service_type_name=service_type_name,
                        role_name=SupporterRoleTypeName.PRODUCER,
                        supporter_organization_total=res_direct_support_organization_totals_list,
                        supporter_organization_man_hours=res_direct_support_organization_supporters_list,
                    )
                )

            # アクセラレーターロールの集計
            if v[SupporterRoleTypeName.ACCELERATOR]:
                res_direct_support_organization_totals_list: List[
                    SupporterOrganizationTotal
                ] = []
                res_direct_support_organization_supporters_list: List[
                    SupporterOrganizationMonHours
                ] = []

                for k_organization, v2 in v[SupporterRoleTypeName.ACCELERATOR].items():
                    res_direct_support_organization_supporter_totals_list: List[
                        Supporters
                    ] = []

                    res_direct_support_organization_totals_list.append(
                        SupporterOrganizationTotal(
                            supporter_organization_id=k_organization,
                            supporter_organization_name=v2[
                                "supporter_organization_name"
                            ],
                            man_hour=v2["total"],
                        )
                    )

                    for k_supporter_user_id, v3 in v2["supporter"].items():
                        res_direct_support_organization_supporter_totals_list.append(
                            Supporters(
                                id=k_supporter_user_id,
                                name=v3["supporter_name"],
                                man_hour=v3["total_s"],
                            )
                        )

                    res_direct_support_organization_supporters_list.append(
                        SupporterOrganizationMonHours(
                            supporter_organization_id=k_organization,
                            supporter_organization_name=v2[
                                "supporter_organization_name"
                            ],
                            supporters=res_direct_support_organization_supporter_totals_list,
                        )
                    )

                res_man_hours.append(
                    SummaryManHours(
                        man_hour_type_name=SummaryManHourType.DIRECT_SUPPORT.value,
                        sub_name=f"{customer_name}／{project_name}",
                        service_type_name=service_type_name,
                        role_name=SupporterRoleTypeName.ACCELERATOR,
                        supporter_organization_total=res_direct_support_organization_totals_list,
                        supporter_organization_man_hours=res_direct_support_organization_supporters_list,
                    )
                )

        # 支援仕込工数
        for k_project, v in summary_pre_support_man_hours.items():
            project_id = k_project
            project_name = v["project_name"]
            customer_name = v["customer_name"]
            service_type_name = v["service_type_name"]

            # プロデューサーロールの集計
            if v[SupporterRoleTypeName.PRODUCER]:
                res_pre_support_organization_totals_list: List[
                    SupporterOrganizationTotal
                ] = []
                res_pre_support_organization_supporters_list: List[
                    SupporterOrganizationMonHours
                ] = []

                for k_organization, v2 in v[SupporterRoleTypeName.PRODUCER].items():
                    res_pre_support_organization_supporter_totals_list: List[
                        Supporters
                    ] = []

                    res_pre_support_organization_totals_list.append(
                        SupporterOrganizationTotal(
                            supporter_organization_id=k_organization,
                            supporter_organization_name=v2[
                                "supporter_organization_name"
                            ],
                            man_hour=v2["total"],
                        )
                    )

                    for k_supporter_user_id, v3 in v2["supporter"].items():
                        res_pre_support_organization_supporter_totals_list.append(
                            Supporters(
                                id=k_supporter_user_id,
                                name=v3["supporter_name"],
                                man_hour=v3["total_s"],
                            )
                        )

                    res_pre_support_organization_supporters_list.append(
                        SupporterOrganizationMonHours(
                            supporter_organization_id=k_organization,
                            supporter_organization_name=v2[
                                "supporter_organization_name"
                            ],
                            supporters=res_pre_support_organization_supporter_totals_list,
                        )
                    )

                res_man_hours.append(
                    SummaryManHours(
                        man_hour_type_name=SummaryManHourType.PRE_SUPPORT.value,
                        sub_name=f"{customer_name}／{project_name}",
                        service_type_name=service_type_name,
                        role_name=SupporterRoleTypeName.PRODUCER,
                        supporter_organization_total=res_pre_support_organization_totals_list,
                        supporter_organization_man_hours=res_pre_support_organization_supporters_list,
                    )
                )

            # アクセラレーターロールの集計
            if v[SupporterRoleTypeName.ACCELERATOR]:
                res_pre_support_organization_totals_list: List[
                    SupporterOrganizationTotal
                ] = []
                res_pre_support_organization_supporters_list: List[
                    SupporterOrganizationMonHours
                ] = []

                for k_organization, v2 in v[SupporterRoleTypeName.ACCELERATOR].items():
                    res_pre_support_organization_supporter_totals_list: List[
                        Supporters
                    ] = []

                    res_pre_support_organization_totals_list.append(
                        SupporterOrganizationTotal(
                            supporter_organization_id=k_organization,
                            supporter_organization_name=v2[
                                "supporter_organization_name"
                            ],
                            man_hour=v2["total"],
                        )
                    )

                    for k_supporter_user_id, v3 in v2["supporter"].items():
                        res_pre_support_organization_supporter_totals_list.append(
                            Supporters(
                                id=k_supporter_user_id,
                                name=v3["supporter_name"],
                                man_hour=v3["total_s"],
                            )
                        )

                    res_pre_support_organization_supporters_list.append(
                        SupporterOrganizationMonHours(
                            supporter_organization_id=k_organization,
                            supporter_organization_name=v2[
                                "supporter_organization_name"
                            ],
                            supporters=res_pre_support_organization_supporter_totals_list,
                        )
                    )

                res_man_hours.append(
                    SummaryManHours(
                        man_hour_type_name=SummaryManHourType.PRE_SUPPORT.value,
                        sub_name=f"{customer_name}／{project_name}",
                        service_type_name=service_type_name,
                        role_name=SupporterRoleTypeName.ACCELERATOR,
                        supporter_organization_total=res_pre_support_organization_totals_list,
                        supporter_organization_man_hours=res_pre_support_organization_supporters_list,
                    )
                )

        # 商談/提案準備工数
        for k_project, v in summary_sales_support_man_hours.items():
            res_sales_support_organization_totals_list: List[
                SupporterOrganizationTotal
            ] = []
            res_sales_support_organization_supporters_list: List[
                SupporterOrganizationMonHours
            ] = []

            project_name = k_project
            customer_name = v["customer_name"]

            for k_organization, v2 in v["supporter_organizations"].items():
                res_sales_support_organization_supporter_totals_list: List[
                    Supporters
                ] = []

                res_sales_support_organization_totals_list.append(
                    SupporterOrganizationTotal(
                        supporter_organization_id=k_organization,
                        supporter_organization_name=v2["supporter_organization_name"],
                        man_hour=v2["total"],
                    )
                )

                for k_supporter_user_id, v3 in v2["supporter"].items():
                    res_sales_support_organization_supporter_totals_list.append(
                        Supporters(
                            id=k_supporter_user_id,
                            name=v3["supporter_name"],
                            man_hour=v3["total_s"],
                        )
                    )

                res_sales_support_organization_supporters_list.append(
                    SupporterOrganizationMonHours(
                        supporter_organization_id=k_organization,
                        supporter_organization_name=v2["supporter_organization_name"],
                        supporters=res_sales_support_organization_supporter_totals_list,
                    )
                )

            res_man_hours.append(
                SummaryManHours(
                    man_hour_type_name=SummaryManHourType.SALES_SUPPORT.value,
                    sub_name=f"{customer_name}／{k_project}",
                    supporter_organization_total=res_sales_support_organization_totals_list,
                    supporter_organization_man_hours=res_sales_support_organization_supporters_list,
                )
            )

        # 内部業務工数
        for k_category, v_organizations in summary_ssap_man_hours.items():
            res_ssap_organization_totals_list: List[SupporterOrganizationTotal] = []
            res_ssap_organization_supporters_list: List[
                SupporterOrganizationMonHours
            ] = []

            for k_organization, v2 in v_organizations.items():
                res_ssap_organization_supporter_totals_list: List[Supporters] = []

                res_ssap_organization_totals_list.append(
                    SupporterOrganizationTotal(
                        supporter_organization_id=k_organization,
                        supporter_organization_name=v2["supporter_organization_name"],
                        man_hour=v2["total"],
                    )
                )

                for k_supporter_user_id, v3 in v2["supporter"].items():
                    res_ssap_organization_supporter_totals_list.append(
                        Supporters(
                            id=k_supporter_user_id,
                            name=v3["supporter_name"],
                            man_hour=v3["total_s"],
                        )
                    )

                res_ssap_organization_supporters_list.append(
                    SupporterOrganizationMonHours(
                        supporter_organization_id=k_organization,
                        supporter_organization_name=v2["supporter_organization_name"],
                        supporters=res_ssap_organization_supporter_totals_list,
                    )
                )

            res_man_hours.append(
                SummaryManHours(
                    man_hour_type_name=SummaryManHourType.SSAP.value,
                    sub_name=k_category,
                    supporter_organization_total=res_ssap_organization_totals_list,
                    supporter_organization_man_hours=res_ssap_organization_supporters_list,
                )
            )

        # 休憩その他工数
        for k_category, v_organizations in summary_holidays_man_hours.items():
            res_holidays_organization_totals_list: List[SupporterOrganizationTotal] = []
            res_holidays_organization_supporters_list: List[
                SupporterOrganizationMonHours
            ] = []

            for k_organization, v2 in v_organizations.items():
                res_holidays_organization_supporter_totals_list: List[Supporters] = []

                res_holidays_organization_totals_list.append(
                    SupporterOrganizationTotal(
                        supporter_organization_id=k_organization,
                        supporter_organization_name=v2["supporter_organization_name"],
                        man_hour=v2["total"],
                    )
                )

                for k_supporter_user_id, v3 in v2["supporter"].items():
                    res_holidays_organization_supporter_totals_list.append(
                        Supporters(
                            id=k_supporter_user_id,
                            name=v3["supporter_name"],
                            man_hour=v3["total_s"],
                        )
                    )

                res_holidays_organization_supporters_list.append(
                    SupporterOrganizationMonHours(
                        supporter_organization_id=k_organization,
                        supporter_organization_name=v2["supporter_organization_name"],
                        supporters=res_holidays_organization_supporter_totals_list,
                    )
                )

            res_man_hours.append(
                SummaryManHours(
                    man_hour_type_name=SummaryManHourType.HOLIDAYS.value,
                    sub_name=k_category,
                    supporter_organization_total=res_holidays_organization_totals_list,
                    supporter_organization_man_hours=res_holidays_organization_supporters_list,
                )
            )

        # ヘッダー
        res_header_organization_totals_list: List[SupporterOrganizationTotal] = []
        res_header_organization_supporters_list: List[
            SupporterOrganizationMonHours
        ] = []

        for k_organization, v in summary_man_hour_header.items():
            res_header_organization_supporter_totals_list: List[Supporters] = []

            res_header_organization_totals_list.append(
                SupporterOrganizationTotal(
                    supporter_organization_id=k_organization,
                    supporter_organization_name=v["supporter_organization_name"],
                    man_hour=v["header_total_o"],
                )
            )

            for k_supporter_user_id, v2 in v["supporters"].items():
                res_header_organization_supporter_totals_list.append(
                    Supporters(
                        id=k_supporter_user_id,
                        name=v2["supporter_name"],
                        man_hour=v2["header_total_s"],
                    )
                )

            res_header_organization_supporters_list.append(
                SupporterOrganizationMonHours(
                    supporter_organization_id=k_organization,
                    supporter_organization_name=v["supporter_organization_name"],
                    supporters=res_header_organization_supporter_totals_list,
                )
            )

        # ####################################
        # ソート
        # ####################################
        # 分類1, 分類2, カテゴリ、役割の順で昇順でソート
        res_man_hours_sorted = sorted(
            res_man_hours,
            key=lambda x: (
                x.man_hour_type_name,
                x.sub_name,
                x.service_type_name,
                x.role_name,
            ),
        )

        # 支援者組織名で昇順でソート
        res_header_organization_totals_list_sorted = sorted(
            res_header_organization_totals_list,
            key=lambda x: x.supporter_organization_name,
        )

        res_header_organization_supporters_list_sorted = sorted(
            res_header_organization_supporters_list,
            key=lambda x: x.supporter_organization_name,
        )

        return GetSummaryManHourTypeResponse(
            year_month=year_month,
            header=GetSummaryManHourTypeHeader(
                supporter_organization_total=res_header_organization_totals_list_sorted,
                supporter_organization_man_hours=res_header_organization_supporters_list_sorted,
            ),
            man_hours=res_man_hours_sorted,
        )

    @staticmethod
    def get_summary_supporter_man_hours(
        year: int,
        month: int,
        supporter_organization_id: str,
        current_user: AdminModel,
    ) -> GetSummarySupporterManHoursResponse:
        """Get /api/man-hours/summary/supporter 月次担当者別工数一覧取得API

        Args:
            year (int): 対象年
            month (int): 対象月
            supporter_organization_id (str): 支援者組織ID
            current_user (Behavior, optional): 認証済みのユーザー

        Returns:
            GetSummarySupporterManHoursResponse: 取得結果
        """

        data_type_prefix = "supporter#"

        month_str = str(month)
        year_month = str(year) + "/" + month_str.zfill(2)

        filter_condition = None

        if supporter_organization_id:
            # カンマ区切りで分割
            supporter_organization_iter = iter(supporter_organization_id.split(","))

            current_id = next(supporter_organization_iter)
            filter_condition = (
                ManHourSupporterModel.supporter_organization_id == current_id
            )
            for current_id in supporter_organization_iter:
                filter_condition |= (
                    ManHourSupporterModel.supporter_organization_id == current_id
                )

        # 支援者別工数を取得
        supporter_man_hours = ManHourSupporterModel.year_month_data_type_index.query(
            hash_key=year_month,
            range_key_condition=ManHourSupporterModel.data_type.startswith(
                data_type_prefix
            ),
            filter_condition=filter_condition,
        )

        # ####################################
        # 集計
        # ####################################
        man_hours_list: List[SummarySupporterManHours] = []

        for supporter_man_hour in supporter_man_hours:
            # 契約時間の集計(対象は対面支援工数と支援仕込工数に関連付けられている案件)
            producer_contract_time: List[float] = []
            accelerator_contract_time: List[float] = []
            this_month_total_contract_time: float = None

            # 支援者責任者のアクセス判定、自分の組織以外の案件の場合はスキップ
            if not ManHourService.is_visible_man_hour(
                current_user=current_user,
                supporter_organization_id=supporter_man_hour.supporter_organization_id,
            ):
                continue

            # 対面支援工数
            for direct_support in supporter_man_hour.direct_support_man_hours.items:
                # 役割別の当月契約時間の集計
                try:
                    if direct_support.role == SupporterRoleTypeName.PRODUCER:
                        producer_contract_time.append(direct_support.input_man_hour)

                    elif direct_support.role == SupporterRoleTypeName.ACCELERATOR:
                        accelerator_contract_time.append(direct_support.input_man_hour)
                except KeyError:
                    pass

            # 支援仕込工数
            for pre_support in supporter_man_hour.pre_support_man_hours.items:
                # 役割別の当月契約時間の集計
                try:
                    if pre_support.role == SupporterRoleTypeName.PRODUCER:
                        producer_contract_time.append(pre_support.input_man_hour)

                    elif pre_support.role == SupporterRoleTypeName.ACCELERATOR:
                        accelerator_contract_time.append(pre_support.input_man_hour)
                except KeyError:
                    pass

            # 契約時間の合計を集計
            this_month_total_contract_time = round(
                sum(producer_contract_time) + sum(accelerator_contract_time), 2
            )

            man_hours_list.append(
                SummarySupporterManHours(
                    supporter_organization_id=supporter_man_hour.supporter_organization_id,
                    supporter_organization_name=supporter_man_hour.supporter_organization_name,
                    supporter_id=supporter_man_hour.supporter_user_id,
                    supporter_name=supporter_man_hour.supporter_name,
                    summary_man_hour=SummarySupporterManHour(
                        direct=round(supporter_man_hour.summary_man_hour.direct, 2),
                        pre=round(supporter_man_hour.summary_man_hour.pre, 2),
                        sales=round(supporter_man_hour.summary_man_hour.sales, 2),
                        ssap=round(supporter_man_hour.summary_man_hour.ssap, 2),
                        others=round(supporter_man_hour.summary_man_hour.others, 2),
                        total=round(supporter_man_hour.summary_man_hour.total, 2),
                    ),
                    contract_time=SummarySupporterContractTime(
                        producer=round(sum(producer_contract_time), 2),
                        accelerator=round(sum(accelerator_contract_time), 2),
                        total=this_month_total_contract_time,
                    ),
                    is_confirm=supporter_man_hour.is_confirm,
                )
            )

        # ####################################
        # ソート
        # ####################################
        # 支援者組織名、支援者名の順でで昇順でソート
        man_hours_list_sorted = sorted(
            man_hours_list,
            key=lambda x: (x.supporter_organization_name, x.supporter_name),
        )

        return GetSummarySupporterManHoursResponse(
            year_month=year_month, man_hours=man_hours_list_sorted
        )
