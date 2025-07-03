import os
import sys
import time
import traceback
from datetime import date, datetime
from logging import getLogger
from typing import List

import pandas as pd
from dateutil.relativedelta import relativedelta

from app.core.config import get_app_settings
from app.models.man_hour import (
    ManHourProjectSummaryModel,
    ManHourServiceTypeSummaryModel,
    ManHourSupporterModel,
    ManHourSupporterOrganizationSummaryModel,
    SupporterUsersAttribute,
    SupportManHourAttribute,
)
from app.models.master import MasterSupporterOrganizationModel
from app.models.master_alert_setting import MasterAlertSettingModel
from app.models.project import ProjectModel
from app.models.user import UserModel
from functions.batch_common import (
    SummaryInfoForProjectSummary,
    batch_end_common_procedure,
    batch_init_common_procedure,
    decimal_round,
    get_operation_datetime,
    model_to_dict,
    send_mail,
)
from functions.batch_const import (
    BatchFunctionId,
    BatchFunctionName,
    BatchInputParameterModeForSummaryManHour,
    BatchStatus,
    ContractType,
    DataType,
    MailType,
    ManHourDataTypePrefix,
    MasterDataType,
    RoundSetting,
    SupporterRoleType,
)
from functions.batch_exceptions import ExitHandler

try:
    import unzip_requirements
except ImportError:
    pass

# ログ出力設定をロードする
get_app_settings().configure_logging()
logger = getLogger(__name__)

# 汎用マスタ：支援工数アラート設定
master_alert_setting_list: List[MasterAlertSettingModel] = None
# 汎用マスタ:支援者組織情報
master_supporter_organization_list: List[MasterSupporterOrganizationModel] = None
# 汎用マスタ:サービス種別
master_service_type_list: List[MasterSupporterOrganizationModel] = None


def get_count_greater_than_zero(s: pd.Series):
    """0より大きい要素のカウントを返す"""
    return len(s[s > 0])


def edit_summary_info_for_project_summary(
    summary_info_list: List[SummaryInfoForProjectSummary],
    add_info_list: List[SummaryInfoForProjectSummary],
):
    """
        プロジェクト別工数集計：累積項目編集
        ・累積情報に追加情報を追加（更新）
        ・同一案件IDの情報がある場合は更新、ない場合は追加

    Args:
        event:
        summary_info_list (List[SummaryInfoForProjectSummary]): 累積値が格納された累積情報クラスのリスト
        add_info_list (List[SummaryInfoForProjectSummary]): 既存の累積情報に追加する累積情報クラスのリスト
    """
    for add_info in add_info_list:
        if add_info.project_id in [x.project_id for x in summary_info_list]:
            # 累積情報に存在する場合
            for summary in summary_info_list:
                if add_info.project_id == summary.project_id:
                    # 累積に追加分を加算
                    summary.summary_direct_support_man_hour += (
                        add_info.summary_direct_support_man_hour
                    )
                    summary.summary_pre_support_man_hour += (
                        add_info.summary_pre_support_man_hour
                    )
                    if add_info.summary_supporter_direct_support_man_hours:
                        for (
                            support_man_hour
                        ) in add_info.summary_supporter_direct_support_man_hours:
                            if (
                                summary.summary_supporter_direct_support_man_hours
                                and support_man_hour.supporter_user_id
                                in [
                                    x.supporter_user_id
                                    for x in summary.summary_supporter_direct_support_man_hours
                                ]
                            ):
                                # 累積の累積支援者別直接支援工数に存在する場合
                                for (
                                    summary_support_man_hour
                                ) in summary.summary_supporter_direct_support_man_hours:
                                    if (
                                        support_man_hour.supporter_user_id
                                        == summary_support_man_hour.supporter_user_id
                                    ):
                                        summary_support_man_hour.input_man_hour += (
                                            support_man_hour.input_man_hour
                                        )
                                        break
                            else:

                                summary.summary_supporter_direct_support_man_hours = (
                                    []
                                    if summary.summary_supporter_direct_support_man_hours
                                    is None
                                    else summary.summary_supporter_direct_support_man_hours
                                )
                                # 累積に追加
                                summary.summary_supporter_direct_support_man_hours.append(
                                    support_man_hour
                                )
                    if add_info.summary_supporter_pre_support_man_hours:
                        for (
                            support_pre_man_hour
                        ) in add_info.summary_supporter_pre_support_man_hours:
                            if (
                                summary.summary_supporter_pre_support_man_hours
                                and support_pre_man_hour.supporter_user_id
                                in [
                                    x.supporter_user_id
                                    for x in summary.summary_supporter_pre_support_man_hours
                                ]
                            ):
                                # 累積の累積支援者別仕込支援工数に存在する場合
                                for (
                                    summary_support_pre_man_hour
                                ) in summary.summary_supporter_pre_support_man_hours:
                                    if (
                                        support_pre_man_hour.supporter_user_id
                                        == summary_support_pre_man_hour.supporter_user_id
                                    ):
                                        summary_support_pre_man_hour.input_man_hour += (
                                            support_pre_man_hour.input_man_hour
                                        )
                                        break
                            else:
                                summary.summary_supporter_pre_support_man_hours = (
                                    []
                                    if summary.summary_supporter_pre_support_man_hours
                                    is None
                                    else summary.summary_supporter_pre_support_man_hours
                                )
                                # 累積に追加
                                summary.summary_supporter_pre_support_man_hours.append(
                                    support_pre_man_hour
                                )

                    break

        else:
            # 新規に追加
            summary_info_list.append(add_info)


def calc_monthly_average_gross_profit_and_contract_time(
    project: ProjectModel, summary_month_from_date: date, summary_month_to_date: date
) -> dict:
    """
        平均（当月）粗利額、当月契約時間を算出。
        契約形態が無償の場合、平均（当月）粗利額はゼロで計算される。

    Args:
        project (ProjectModel): 案件情報
        summary_month_from_date (date): 集計月の初日(1日)の年月日
        summary_month_to_date (date): 集計月の末尾(月末日)の年月日

    Returns:
        dict: 平均（当月）粗利額、当月契約時間
          key:
            monthly_average_gross_profit: 平均（当月）粗利額
            monthly_contract_time: 当月契約時間
    """
    ret_dict = {}
    # 契約日数
    from_date = datetime.strptime(project.support_date_from, "%Y/%m/%d").date()
    to_date = datetime.strptime(project.support_date_to, "%Y/%m/%d").date()
    support_days: int = (to_date - from_date).days + 1

    # 当月契約日数
    summary_month_support_days: int = 0
    if from_date <= summary_month_from_date:
        # 支援期間中
        if summary_month_to_date <= to_date:
            summary_month_support_days = (
                summary_month_to_date - summary_month_from_date
            ).days + 1
        else:
            summary_month_support_days = (to_date - summary_month_from_date).days + 1
    else:
        # 月中に支援開始の場合
        if summary_month_to_date <= to_date:
            summary_month_support_days = (summary_month_to_date - from_date).days + 1
        else:
            summary_month_support_days = (to_date - from_date).days + 1

    # 平均（当月）粗利額
    if project.contract_type == ContractType.FOR_VALUE:
        # 契約形態：有償の場合
        ret_dict["monthly_average_gross_profit"] = decimal_round(
            project.profit.year * (summary_month_support_days / support_days)
            if project.profit.year and summary_month_support_days and support_days
            else 0,
            RoundSetting.DECIMAL_DIGITS,
        )
    else:
        # 契約形態：無償の場合
        ret_dict["monthly_average_gross_profit"] = float(0)

    # 当月契約時間
    ret_dict["monthly_contract_time"] = decimal_round(
        project.total_contract_time * (summary_month_support_days / support_days)
        if project.total_contract_time and summary_month_support_days and support_days
        else 0,
        RoundSetting.DECIMAL_DIGITS,
    )

    return ret_dict


def get_direct_and_pre_support_man_hour_of_alert_setting(service_type_id: str) -> float:
    """
        工数アラート設定の対面仕込支援工数係数(%)を取得

    Args:
        service_type_id (str): サービス種別ID
    Returns:
        float: 対面仕込支援工数係数(%)
    """
    ret_zero: float = float(0)
    # 工数アラート設定
    if not master_alert_setting_list:
        # 工数アラート設定が取得できない場合
        error_message = "Unable to get a alert setting info."
        logger.warning(error_message)
        return ret_zero
    else:
        for factor in master_alert_setting_list[0].attributes.factor_setting:
            if factor.service_type_id == service_type_id:
                return factor.direct_and_pre_support_man_hour

    # 指定されたサービス種別の設定が取得できない場合
    error_message = f"Alert settings of service_type_id '{service_type_id}' not found."
    logger.warning(error_message)
    return ret_zero


def save_project_summary_of_last_or_current_month(summary_year_month: str) -> None:
    """
        プロジェクト別工数集計の編集・登録処理（前月または当月用）
        ・指定された基準年月のプロジェクト別工数集計を支援工数情報に登録

    Args:
        summary_year_month (str): 基準年月(YYYY/MM)
    """
    summary_month_datetime: datetime = datetime.strptime(summary_year_month, "%Y/%m")
    # 累積値情報
    summary_info_list: List[SummaryInfoForProjectSummary] = []

    ########################################################
    # データ洗い替えの為、処理前に前回作成分の
    # プロジェクト別工数集計の処理対象の年月のデータを削除
    ########################################################
    logger.info(f"集計月{summary_year_month}の前回作成データ削除")
    del_year_month = summary_month_datetime.strftime("%Y/%m")
    del_project_summary_range_key_condition = (
        ManHourProjectSummaryModel.data_type.startswith(
            ManHourDataTypePrefix.PROJECT_SUMMARY
        )
    )
    del_man_hour_project_summary_list: List[ManHourProjectSummaryModel] = list(
        ManHourProjectSummaryModel.year_month_data_type_index.query(
            hash_key=del_year_month,
            range_key_condition=del_project_summary_range_key_condition,
        )
    )
    with ManHourProjectSummaryModel.batch_write() as del_project_summary_batch:
        count = 0
        for del_summary in del_man_hour_project_summary_list:
            count += 1
            logger.info(
                f"集計月{summary_year_month}の前回作成データ削除 - 処理開始 ({count}/{len(del_man_hour_project_summary_list)})"
            )
            del_project_summary_batch.delete(del_summary)

    ######################################################
    # 案件情報取得
    # 抽出条件
    # ・集計月時点で支援期間中の案件
    # ・工数管理設定あり(is_count_man_hour=True)
    ######################################################
    summary_month_from_datetime: datetime = summary_month_datetime + relativedelta(
        day=1
    )
    summary_month_to_datetime: datetime = summary_month_datetime + relativedelta(
        months=+1, day=1, days=-1
    )
    summary_month_from = summary_month_from_datetime.strftime("%Y/%m/%d")
    summary_month_to = summary_month_to_datetime.strftime("%Y/%m/%d")
    project_range_key_condition = ProjectModel.support_date_to >= summary_month_from
    bool_true: bool = True
    project_filter_condition = ProjectModel.support_date_from <= summary_month_to
    project_filter_condition &= ProjectModel.is_count_man_hour == bool_true
    project_model_list: List[ProjectModel] = list(
        ProjectModel.data_type_support_date_to_index.query(
            hash_key=DataType.PROJECT,
            range_key_condition=project_range_key_condition,
            filter_condition=project_filter_condition,
        )
    )
    logger.info(f"集計月{summary_year_month}の案件情報取得件数: {len(project_model_list)}件")
    for project in project_model_list:
        logger.info(f"処理対象の案件ID: {project.id}")

    # 一般ユーザ情報の取得
    user_list: List[UserModel] = list(UserModel.scan())

    ###########################
    # 累積値の取得
    ###########################
    # 基準月の前月のプロジェクト別工数集計の取得
    last_summary_month_datetime: datetime = summary_month_datetime + relativedelta(
        months=-1
    )
    last_year_month = last_summary_month_datetime.strftime("%Y/%m")
    last_project_summary_range_key_condition = (
        ManHourSupporterModel.data_type.startswith(
            ManHourDataTypePrefix.PROJECT_SUMMARY
        )
    )
    last_man_hour_project_summary_list: List[ManHourProjectSummaryModel] = list(
        ManHourProjectSummaryModel.year_month_data_type_index.query(
            hash_key=last_year_month,
            range_key_condition=last_project_summary_range_key_condition,
        )
    )
    # 累積情報の追加
    add_info_list: List[SummaryInfoForProjectSummary] = []
    for project_summary in last_man_hour_project_summary_list:
        add_info_list.append(
            SummaryInfoForProjectSummary(
                project_id=project_summary.project_id,
                summary_direct_support_man_hour=project_summary.summary_direct_support_man_hour,
                summary_pre_support_man_hour=project_summary.summary_pre_support_man_hour,
                summary_supporter_direct_support_man_hours=project_summary.summary_supporter_direct_support_man_hours,
                summary_supporter_pre_support_man_hours=project_summary.summary_supporter_pre_support_man_hours,
            )
        )

    edit_summary_info_for_project_summary(
        summary_info_list=summary_info_list, add_info_list=add_info_list
    )

    ###########################
    # 基準月データの作成
    ###########################
    # 支援者別工数の取得
    man_hour_supporter_range_key_condition = ManHourSupporterModel.data_type.startswith(
        ManHourDataTypePrefix.SUPPORTER
    )
    man_hour_supporter_list: List[ManHourSupporterModel] = list(
        ManHourSupporterModel.year_month_data_type_index.query(
            hash_key=summary_year_month,
            range_key_condition=man_hour_supporter_range_key_condition,
        )
    )
    logger.info(
        f"集計月{summary_year_month}の支援者別工数情報の取得件数: {len(man_hour_supporter_list)}件"
    )

    if not man_hour_supporter_list:
        # 基準月の支援者別工数情報が0件の場合
        # プロジェクト別工数集計の編集
        current_datetime = datetime.now()
        count = 0
        with ManHourProjectSummaryModel.batch_write() as project_summary_batch:
            for project in project_model_list:
                count += 1
                logger.info(
                    f"data_type:{ManHourDataTypePrefix.PROJECT_SUMMARY + project.id},year_month:{summary_year_month} - 処理開始 ({count}/{len(project_model_list)})"
                )

                project_supporter_organization_id = (
                    project.supporter_organization_id
                    if project.supporter_organization_id
                    else ""
                )

                # メイン支援者（プロデューサー）
                main_supporter_user: SupporterUsersAttribute = None
                if project.main_supporter_user_id:
                    name = ""
                    org_id = ""
                    for user in user_list:
                        if user.id == project.main_supporter_user_id:
                            name = user.name
                            # 支援者責任者の場合、支援者組織を複数持つため先頭の組織IDを取得
                            if user.supporter_organization_id:
                                org_id = user.supporter_organization_id[0]
                            break
                    main_supporter_user = SupporterUsersAttribute(
                        id=project.main_supporter_user_id,
                        organization_id=org_id,
                        name=name,
                        is_confirm=False,
                    )

                # 支援者メンバー（アクセラレーター）
                supporter_users: List[SupporterUsersAttribute] = None
                if project.supporter_user_ids:
                    supporter_users = []
                    for supporter_user_id in project.supporter_user_ids:
                        name = ""
                        org_id = ""
                        for user in user_list:
                            if user.id == supporter_user_id:
                                name = user.name
                                # 支援者責任者の場合、支援者組織を複数持つため先頭の組織IDを取得
                                if user.supporter_organization_id:
                                    org_id = user.supporter_organization_id[0]
                                break
                        supporter_user = SupporterUsersAttribute(
                            id=supporter_user_id,
                            organization_id=org_id,
                            name=name,
                            is_confirm=False,
                        )
                        supporter_users.append(supporter_user)

                monthly_calc_info = calc_monthly_average_gross_profit_and_contract_time(
                    project=project,
                    summary_month_from_date=summary_month_from_datetime.date(),
                    summary_month_to_date=summary_month_to_datetime.date(),
                )

                # 平均（当月）粗利額
                monthly_average_gross_profit = monthly_calc_info[
                    "monthly_average_gross_profit"
                ]

                # 当月契約時間
                monthly_contract_time = monthly_calc_info["monthly_contract_time"]

                this_month_direct_support_man_hour: float = float(0)
                this_month_direct_support_man_hour_main: float = float(0)
                this_month_direct_support_man_hour_sub: float = float(0)
                this_month_pre_support_man_hour: float = float(0)

                # 当月支援者別直接支援工数
                this_month_supporter_direct_support_man_hours: List[
                    SupportManHourAttribute
                ] = None
                # 工数未入力の支援者は、案件情報を基に追加
                # メイン支援者
                if project.main_supporter_user_id:
                    this_month_supporter_direct_support_man_hours = []
                    this_month_supporter_direct_support_man_hours.append(
                        SupportManHourAttribute(
                            supporter_user_id=project.main_supporter_user_id,
                            input_man_hour=float(0),
                        )
                    )
                # 支援者メンバー
                # メイン支援者と支援者メンバーに同じユーザーが設定されることはないが、念のため再度リスト取得
                if project.supporter_user_ids:
                    this_month_supporter_direct_list: List = (
                        [
                            x.supporter_user_id
                            for x in this_month_supporter_direct_support_man_hours
                        ]
                        if this_month_supporter_direct_support_man_hours is not None
                        else []
                    )
                    for supporter_id in project.supporter_user_ids:
                        if supporter_id not in this_month_supporter_direct_list:
                            if this_month_supporter_direct_support_man_hours is None:
                                this_month_supporter_direct_support_man_hours = []
                            this_month_supporter_direct_support_man_hours.append(
                                SupportManHourAttribute(
                                    supporter_user_id=supporter_id,
                                    input_man_hour=float(0),
                                )
                            )

                # 当月支援者別仕込支援工数
                this_month_supporter_pre_support_man_hours: List[
                    SupportManHourAttribute
                ] = None
                # 工数未入力の支援者は、案件情報を基に追加
                # メイン支援者
                if project.main_supporter_user_id:
                    this_month_supporter_pre_support_man_hours = []
                    this_month_supporter_pre_support_man_hours.append(
                        SupportManHourAttribute(
                            supporter_user_id=project.main_supporter_user_id,
                            input_man_hour=float(0),
                        )
                    )
                # 支援者メンバー
                # メイン支援者と支援者メンバーに同じユーザーが設定されることはないが、念のため再度リスト取得
                if project.supporter_user_ids:
                    this_month_supporter_pre_list: List = (
                        [
                            x.supporter_user_id
                            for x in this_month_supporter_pre_support_man_hours
                        ]
                        if this_month_supporter_pre_support_man_hours is not None
                        else []
                    )
                    for supporter_id in project.supporter_user_ids:
                        if supporter_id not in this_month_supporter_pre_list:
                            if this_month_supporter_pre_support_man_hours is None:
                                this_month_supporter_pre_support_man_hours = []
                            this_month_supporter_pre_support_man_hours.append(
                                SupportManHourAttribute(
                                    supporter_user_id=supporter_id,
                                    input_man_hour=float(0),
                                )
                            )

                # DB編集項目に累積項目をセット
                summary_direct_support_man_hour: float = float(0)
                summary_pre_support_man_hour: float = float(0)
                summary_supporter_direct_support_man_hours: List[
                    SupportManHourAttribute
                ] = None
                summary_supporter_pre_support_man_hours: List[
                    SupportManHourAttribute
                ] = None
                if project.id in [x.project_id for x in summary_info_list]:
                    for summary_info in summary_info_list:
                        if project.id == summary_info.project_id:
                            summary_direct_support_man_hour = (
                                summary_info.summary_direct_support_man_hour
                            )
                            summary_pre_support_man_hour = (
                                summary_info.summary_pre_support_man_hour
                            )
                            summary_supporter_direct_support_man_hours = (
                                summary_info.summary_supporter_direct_support_man_hours
                            )
                            summary_supporter_pre_support_man_hours = (
                                summary_info.summary_supporter_pre_support_man_hours
                            )

                man_hour_project_summary = ManHourProjectSummaryModel(
                    data_type=ManHourDataTypePrefix.PROJECT_SUMMARY + project.id,
                    year_month=summary_year_month,
                    project_id=project.id,
                    project_name=project.name,
                    customer_id=project.customer_id,
                    customer_name=project.customer_name,
                    supporter_organization_id=project_supporter_organization_id,
                    service_type=project.service_type,
                    support_date_from=project.support_date_from,
                    support_date_to=project.support_date_to,
                    contract_type=project.contract_type,
                    main_supporter_user=main_supporter_user,
                    supporter_users=supporter_users,
                    total_contract_time=project.total_contract_time,
                    this_month_contract_time=monthly_contract_time,
                    total_profit=project.profit.year,
                    this_month_profit=monthly_average_gross_profit,
                    this_month_direct_support_man_hour=this_month_direct_support_man_hour,
                    this_month_direct_support_man_hour_main=this_month_direct_support_man_hour_main,
                    this_month_direct_support_man_hour_sub=this_month_direct_support_man_hour_sub,
                    this_month_pre_support_man_hour=this_month_pre_support_man_hour,
                    summary_direct_support_man_hour=summary_direct_support_man_hour,
                    summary_pre_support_man_hour=summary_pre_support_man_hour,
                    summary_theoretical_direct_support_man_hour=0,
                    summary_theoretical_pre_support_man_hour=0,
                    this_month_theoretical_direct_support_man_hour=0,
                    this_month_theoretical_pre_support_man_hour=0,
                    this_month_supporter_direct_support_man_hours=this_month_supporter_direct_support_man_hours,
                    this_month_supporter_pre_support_man_hours=this_month_supporter_pre_support_man_hours,
                    summary_supporter_direct_support_man_hours=summary_supporter_direct_support_man_hours,
                    summary_supporter_pre_support_man_hours=summary_supporter_pre_support_man_hours,
                    create_at=current_datetime,
                    update_at=current_datetime,
                )
                project_summary_batch.save(man_hour_project_summary)
        # 終了
        return

    # modelのリスト -> pandasのDataFrame (Map等は子要素も展開.但し、リスト部分は展開不可)
    # e.g. 以下を1行とするDataFrame
    #  df:
    #  create_at                                                2022-07-25T12:07:27.014841+09:00
    #  update_at                                                2022-07-25T12:07:27.014860+09:00
    #  create_id                                            fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7
    #  data_type                                  supporter#278be7f8-c46f-4e2d-b3df-ffdf12278ad1
    #  is_confirm                                                                           True
    #  supporter_name                                                                   支援者１
    #  supporter_organization_id                            180a3597-b7e7-42c8-902c-a29016afa662
    #  supporter_organization_name                                                           IST
    #  supporter_user_id                                    278be7f8-c46f-4e2d-b3df-ffdf12278ad1
    #  update_id                                            fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7
    #  version                                                                                 1
    #  year_month                                                                        2022/06
    #  direct_support_man_hours.items          [{'input_man_hour': 30, 'project_id': '2bb85e8...
    #  direct_support_man_hours.memo                                                        メモ
    #  holidays_man_hours.department_others                                                    0
    #  .........
    df = pd.json_normalize([model_to_dict(x) for x in man_hour_supporter_list])
    # 粗利メイン課が設定されていない場合、追加
    if "supporter_organization_id" not in df.columns:
        df["supporter_organization_id"] = ""
    if "supporter_organization_name" not in df.columns:
        df["supporter_organization_name"] = ""
    # 粗利メイン課がNaNの場合、空文字に置換
    df = df.fillna({"supporter_organization_id": "", "supporter_organization_name": ""})

    # DataFrameのリスト要素 -> 別のDataFrameに展開
    # 直接支援工数(1.対面支援)
    # e.g. 以下を1行とするDataFrame
    #  df_direct:
    #  input_man_hour                                         30
    #  project_id           2bb85e8d-7560-48b0-82c7-d8ba134a9266
    #  role                                               主担当
    #  service_type         7ac8bddf-88da-46c9-a504-a03d1661ad58
    #  supporter_user_id    278be7f8-c46f-4e2d-b3df-ffdf12278ad1
    df_direct = pd.json_normalize(
        df.to_dict("records"),
        record_path="direct_support_man_hours.items",
        meta="supporter_user_id",
    )
    # 仕込支援工数(2.支援仕込)
    df_pre = pd.json_normalize(
        df.to_dict("records"),
        record_path="pre_support_man_hours.items",
        meta="supporter_user_id",
    )

    # 案件別工数集計
    # 直接支援工数(1.対面支援)
    # e.g.
    #  pivot_table_direct:
    #  role                                 プロデューサー  アクセラレーター  total
    #  project_id
    #  2854fc20-caea-44bb-ada5-d5ec3595f4c0   20              110            130
    #  2bb85e8d-7560-48b0-82c7-d8ba134a9266   30              120            150
    #  ca57d2cd-a898-4344-bdbd-9811fb26e19f   50              120            170
    #  total                                 100              350            450
    if not df_direct.empty:
        pivot_table_direct = pd.pivot_table(
            data=df_direct,
            index=["project_id"],
            columns="role",
            values="input_man_hour",
            margins=True,
            margins_name="total",
            aggfunc="sum",
        ).fillna(0)
    else:
        # df_directがemptyの場合
        pivot_table_direct = pd.DataFrame(
            index=["project_id"],
            columns=[
                "role",
                "total",
            ],
        )

    for x in range(pivot_table_direct.shape[0]):
        logger.debug(
            f"案件別工数集計: 直接支援工数(1.対面支援) : pivot_table_direct.iloc[index]:{pivot_table_direct.iloc[x]}"
        )

    # 仕込支援工数(2.支援仕込)
    if not df_pre.empty:
        pivot_table_pre = pd.pivot_table(
            data=df_pre,
            index=["project_id"],
            columns="role",
            values="input_man_hour",
            margins=True,
            margins_name="total",
            aggfunc="sum",
        ).fillna(0)
    else:
        # df_preがemptyの場合
        pivot_table_pre = pd.DataFrame(
            index=["project_id"],
            columns=[
                "role",
                "total",
            ],
        )

    for x in range(pivot_table_pre.shape[0]):
        logger.debug(
            f"案件別工数集計: 仕込支援工数(2.支援仕込) : pivot_table_pre.iloc[index]:{pivot_table_pre.iloc[x]}"
        )

    # 案件別支援者別工数集計
    # 直接支援工数(1.対面支援)
    # e.g.
    #  pivot_table_direct:
    #  supporter_user_id                    aaaaa    bbbbb  ... total
    #  project_id
    #  2854fc20-caea-44bb-ada5-d5ec3595f4c0   20       50   ...  130
    #  2bb85e8d-7560-48b0-82c7-d8ba134a9266   30       40   ...  150
    #  ca57d2cd-a898-4344-bdbd-9811fb26e19f   50       50   ...  170
    #  total                                 100      140   ...  450
    if not df_direct.empty:
        pivot_table_direct_by_supporter = pd.pivot_table(
            data=df_direct,
            index=["project_id"],
            columns="supporter_user_id",
            values="input_man_hour",
            margins=True,
            margins_name="total",
            aggfunc="sum",
        )
    else:
        # df_directがemptyの場合
        pivot_table_direct_by_supporter = pd.DataFrame(
            index=["project_id"],
            columns=[
                "supporter_user_id",
                "total",
            ],
        )

    for x in range(pivot_table_direct_by_supporter.shape[0]):
        logger.debug(
            f"案件別支援者別工数集計: 直接支援工数(1.対面支援) : pivot_table_direct_by_supporter.iloc[index]:{pivot_table_direct_by_supporter.iloc[x]}"
        )

    # 仕込支援工数(2.支援仕込)
    if not df_pre.empty:
        pivot_table_pre_by_supporter = pd.pivot_table(
            data=df_pre,
            index=["project_id"],
            columns="supporter_user_id",
            values="input_man_hour",
            margins=True,
            margins_name="total",
            aggfunc="sum",
        )
    else:
        # df_preがemptyの場合
        pivot_table_pre_by_supporter = pd.DataFrame(
            index=["project_id"],
            columns=[
                "supporter_user_id",
                "total",
            ],
        )

    for x in range(pivot_table_pre_by_supporter.shape[0]):
        logger.debug(
            f"案件別支援者別工数集計: 仕込支援工数(2.支援仕込) : pivot_table_pre_by_supporter.iloc[index]:{pivot_table_pre_by_supporter.iloc[x]}"
        )

    # プロジェクト別工数集計の編集
    current_datetime = datetime.now()
    count = 0
    with ManHourProjectSummaryModel.batch_write() as project_summary_batch:
        for project in project_model_list:
            count += 1
            logger.info(
                f"data_type:{ManHourDataTypePrefix.PROJECT_SUMMARY + project.id},year_month:{summary_year_month} - 処理開始 ({count}/{len(project_model_list)})"
            )

            project_supporter_organization_id = (
                project.supporter_organization_id
                if project.supporter_organization_id
                else ""
            )

            # メイン支援者（プロデューサー）
            # 直接支援工数(1.対面支援)、(2.支援仕込)のDataFrameから取得
            producer_list_direct = []
            if not df_direct.empty:
                producer_list_direct = df_direct[
                    (df_direct.role == SupporterRoleType.PRODUCER)
                    & (df_direct.project_id == project.id)
                ].supporter_user_id.values.tolist()
            producer_list_pre = []
            if not df_pre.empty:
                producer_list_pre = df_pre[
                    (df_pre.role == SupporterRoleType.PRODUCER)
                    & (df_pre.project_id == project.id)
                ].supporter_user_id.values.tolist()
            producer_list = list(set(producer_list_direct + producer_list_pre))
            # DB登録用に編集
            main_supporter_user: SupporterUsersAttribute = None
            if producer_list:
                temp_df = df[df.supporter_user_id == producer_list[0]]
                main_supporter_user = SupporterUsersAttribute(
                    id=temp_df.supporter_user_id.iloc[-1],
                    organization_id=temp_df.supporter_organization_id.iloc[-1],
                    name=temp_df.supporter_name.iloc[-1],
                    is_confirm=temp_df.is_confirm.iloc[-1],
                )
            else:
                # 工数未入力のメイン支援者は案件情報を基に追加
                if project.main_supporter_user_id:
                    name = ""
                    org_id = ""
                    for user in user_list:
                        if user.id == project.main_supporter_user_id:
                            name = user.name
                            # 支援者責任者の場合、支援者組織を複数持つため先頭の組織IDを取得
                            if user.supporter_organization_id:
                                org_id = user.supporter_organization_id[0]
                            break
                    main_supporter_user = SupporterUsersAttribute(
                        id=project.main_supporter_user_id,
                        organization_id=org_id,
                        name=name,
                        is_confirm=False,
                    )

            # 支援者メンバー（アクセラレーター）
            # 直接支援工数(1.対面支援)、(2.支援仕込)のDataFrameから取得
            accelerator_list_direct = []
            if not df_direct.empty:
                accelerator_list_direct = df_direct[
                    (df_direct.role == SupporterRoleType.ACCELERATOR)
                    & (df_direct.project_id == project.id)
                ].supporter_user_id.values.tolist()
            accelerator_list_pre = []
            if not df_pre.empty:
                accelerator_list_pre = df_pre[
                    (df_pre.role == SupporterRoleType.ACCELERATOR)
                    & (df_pre.project_id == project.id)
                ].supporter_user_id.values.tolist()
            accelerator_list = list(set(accelerator_list_direct + accelerator_list_pre))
            # DB登録用に編集
            supporter_users: List[SupporterUsersAttribute] = None
            if accelerator_list:
                supporter_users = []
                for accelerator_id in accelerator_list:
                    temp_df = df[df.supporter_user_id == accelerator_id]
                    supporter_user = SupporterUsersAttribute(
                        id=temp_df.supporter_user_id.iloc[-1],
                        organization_id=temp_df.supporter_organization_id.iloc[-1],
                        name=temp_df.supporter_name.iloc[-1],
                        is_confirm=temp_df.is_confirm.iloc[-1],
                    )
                    supporter_users.append(supporter_user)
            # 工数未入力の支援者メンバーは案件情報を基に追加
            if project.supporter_user_ids:
                if supporter_users is None:
                    supporter_users = []
                for supporter_user_id in project.supporter_user_ids:
                    enter_flag = False
                    # 工数入力済(リストに含まれているか)の判定
                    if supporter_users:
                        for exist_user in supporter_users:
                            if exist_user.id == supporter_user_id:
                                enter_flag = True
                                break
                    # 工数未入力の場合は追加
                    if not enter_flag:
                        name = ""
                        org_id = ""
                        for user in user_list:
                            if user.id == supporter_user_id:
                                name = user.name
                                # 支援者責任者の場合、支援者組織を複数持つため先頭の組織IDを取得
                                if user.supporter_organization_id:
                                    org_id = user.supporter_organization_id[0]
                                break
                        supporter_user = SupporterUsersAttribute(
                            id=supporter_user_id,
                            organization_id=org_id,
                            name=name,
                            is_confirm=False,
                        )
                        supporter_users.append(supporter_user)

            monthly_calc_info = calc_monthly_average_gross_profit_and_contract_time(
                project=project,
                summary_month_from_date=summary_month_from_datetime.date(),
                summary_month_to_date=summary_month_to_datetime.date(),
            )

            # 平均（当月）粗利額
            monthly_average_gross_profit = monthly_calc_info[
                "monthly_average_gross_profit"
            ]

            # 当月契約時間
            monthly_contract_time = monthly_calc_info["monthly_contract_time"]

            # 当月全体直接支援工数
            this_month_direct_support_man_hour = (
                decimal_round(
                    float(pivot_table_direct.loc[project.id, "total"]),
                    RoundSetting.DECIMAL_DIGITS,
                )
                if project.id in pivot_table_direct.index.values.tolist()
                else float(0)
            )

            # 当月主担当直接支援工数
            this_month_direct_support_man_hour_main = (
                decimal_round(
                    float(
                        pivot_table_direct.loc[project.id, SupporterRoleType.PRODUCER]
                    ),
                    RoundSetting.DECIMAL_DIGITS,
                )
                if project.id in pivot_table_direct.index.values.tolist()
                and SupporterRoleType.PRODUCER
                in pivot_table_direct.columns.values.tolist()
                else float(0)
            )

            # 当月副担当直接支援工数
            this_month_direct_support_man_hour_sub = (
                decimal_round(
                    float(
                        pivot_table_direct.loc[
                            project.id, SupporterRoleType.ACCELERATOR
                        ]
                    ),
                    RoundSetting.DECIMAL_DIGITS,
                )
                if project.id in pivot_table_direct.index.values.tolist()
                and SupporterRoleType.ACCELERATOR
                in pivot_table_direct.columns.values.tolist()
                else float(0)
            )

            # 当月全体仕込支援工数
            this_month_pre_support_man_hour = (
                decimal_round(
                    float(pivot_table_pre.loc[project.id, "total"]),
                    RoundSetting.DECIMAL_DIGITS,
                )
                if project.id in pivot_table_pre.index.values.tolist()
                else float(0)
            )

            # 当月支援者別直接支援工数
            this_month_supporter_direct_support_man_hours: List[
                SupportManHourAttribute
            ] = None
            if project.id in pivot_table_direct_by_supporter.index.values.tolist():
                this_month_supporter_direct_support_man_hours = []
                # ピボットテーブルpivot_table_direct_by_supporterからproject_idの該当行を取得
                # 欠損値NaN、合計行totalを除去
                # Seriesオブジェクト (index: supporter_user_id, value: man_hour)
                series_supporter_main_man_hour = (
                    pivot_table_direct_by_supporter.loc[project.id]
                    .dropna()
                    .drop("total")
                    .astype(float)
                )

                for i, v in series_supporter_main_man_hour.items():
                    this_month_supporter_direct_support_man_hours.append(
                        SupportManHourAttribute(
                            supporter_user_id=i,
                            input_man_hour=v,
                        )
                    )

            # 工数未入力の支援者は、案件情報を基に追加
            # メイン支援者
            this_month_supporter_direct_list: List = (
                [
                    x.supporter_user_id
                    for x in this_month_supporter_direct_support_man_hours
                ]
                if this_month_supporter_direct_support_man_hours is not None
                else []
            )
            if project.main_supporter_user_id:
                if (
                    project.main_supporter_user_id
                    not in this_month_supporter_direct_list
                ):
                    if this_month_supporter_direct_support_man_hours is None:
                        this_month_supporter_direct_support_man_hours = []
                    this_month_supporter_direct_support_man_hours.append(
                        SupportManHourAttribute(
                            supporter_user_id=project.main_supporter_user_id,
                            input_man_hour=float(0),
                        )
                    )
            # 支援者メンバー
            # メイン支援者と支援者メンバーに同じユーザーが設定されることはないが、念のため再度リスト取得
            this_month_supporter_direct_list: List = (
                [
                    x.supporter_user_id
                    for x in this_month_supporter_direct_support_man_hours
                ]
                if this_month_supporter_direct_support_man_hours is not None
                else []
            )
            if project.supporter_user_ids:
                for supporter_id in project.supporter_user_ids:
                    if supporter_id not in this_month_supporter_direct_list:
                        if this_month_supporter_direct_support_man_hours is None:
                            this_month_supporter_direct_support_man_hours = []
                        this_month_supporter_direct_support_man_hours.append(
                            SupportManHourAttribute(
                                supporter_user_id=supporter_id,
                                input_man_hour=float(0),
                            )
                        )

            # 当月支援者別仕込支援工数
            this_month_supporter_pre_support_man_hours: List[
                SupportManHourAttribute
            ] = None
            if project.id in pivot_table_pre_by_supporter.index.values.tolist():
                this_month_supporter_pre_support_man_hours = []
                # ピボットテーブルpivot_table_pre_by_supporterからproject_idの該当行を取得
                # 欠損値NaN、合計行totalを除去
                # Seriesオブジェクト (index: supporter_user_id, value: man_hour)
                series_supporter_pre_man_hour = (
                    pivot_table_pre_by_supporter.loc[project.id]
                    .dropna()
                    .drop("total")
                    .astype(float)
                )

                for i, v in series_supporter_pre_man_hour.items():
                    this_month_supporter_pre_support_man_hours.append(
                        SupportManHourAttribute(
                            supporter_user_id=i,
                            input_man_hour=v,
                        )
                    )

            # 工数未入力の支援者は、案件情報を基に追加
            # メイン支援者
            if project.main_supporter_user_id:
                this_month_supporter_pre_list: List = (
                    [
                        x.supporter_user_id
                        for x in this_month_supporter_pre_support_man_hours
                    ]
                    if this_month_supporter_pre_support_man_hours is not None
                    else []
                )
                if project.main_supporter_user_id not in this_month_supporter_pre_list:
                    if this_month_supporter_pre_support_man_hours is None:
                        this_month_supporter_pre_support_man_hours = []
                    this_month_supporter_pre_support_man_hours.append(
                        SupportManHourAttribute(
                            supporter_user_id=project.main_supporter_user_id,
                            input_man_hour=float(0),
                        )
                    )
            # 支援者メンバー
            # メイン支援者と支援者メンバーに同じユーザーが設定されることはないが、念のため再度リスト取得
            if project.supporter_user_ids:
                this_month_supporter_pre_list: List = (
                    [
                        x.supporter_user_id
                        for x in this_month_supporter_pre_support_man_hours
                    ]
                    if this_month_supporter_pre_support_man_hours is not None
                    else []
                )
                for supporter_id in project.supporter_user_ids:
                    if supporter_id not in this_month_supporter_pre_list:
                        if this_month_supporter_pre_support_man_hours is None:
                            this_month_supporter_pre_support_man_hours = []
                        this_month_supporter_pre_support_man_hours.append(
                            SupportManHourAttribute(
                                supporter_user_id=supporter_id,
                                input_man_hour=float(0),
                            )
                        )

            # 累積項目
            # 累積情報に当月分を追加
            add_info_list: List[SummaryInfoForProjectSummary] = [
                SummaryInfoForProjectSummary(
                    project_id=project.id,
                    summary_direct_support_man_hour=this_month_direct_support_man_hour,
                    summary_pre_support_man_hour=this_month_pre_support_man_hour,
                    summary_supporter_direct_support_man_hours=this_month_supporter_direct_support_man_hours,
                    summary_supporter_pre_support_man_hours=this_month_supporter_pre_support_man_hours,
                )
            ]
            edit_summary_info_for_project_summary(
                summary_info_list=summary_info_list, add_info_list=add_info_list
            )

            # DB編集項目に累積項目をセット
            summary_direct_support_man_hour: float = float(0)
            summary_pre_support_man_hour: float = float(0)
            summary_supporter_direct_support_man_hours: List[
                SupportManHourAttribute
            ] = None
            summary_supporter_pre_support_man_hours: List[
                SupportManHourAttribute
            ] = None
            if project.id in [x.project_id for x in summary_info_list]:
                for summary_info in summary_info_list:
                    if project.id == summary_info.project_id:
                        summary_direct_support_man_hour = (
                            summary_info.summary_direct_support_man_hour
                        )
                        summary_pre_support_man_hour = (
                            summary_info.summary_pre_support_man_hour
                        )
                        summary_supporter_direct_support_man_hours = (
                            summary_info.summary_supporter_direct_support_man_hours
                        )
                        summary_supporter_pre_support_man_hours = (
                            summary_info.summary_supporter_pre_support_man_hours
                        )

            man_hour_project_summary = ManHourProjectSummaryModel(
                data_type=ManHourDataTypePrefix.PROJECT_SUMMARY + project.id,
                year_month=summary_year_month,
                project_id=project.id,
                project_name=project.name,
                customer_id=project.customer_id,
                customer_name=project.customer_name,
                supporter_organization_id=project_supporter_organization_id,
                service_type=project.service_type,
                support_date_from=project.support_date_from,
                support_date_to=project.support_date_to,
                contract_type=project.contract_type,
                main_supporter_user=main_supporter_user,
                supporter_users=supporter_users,
                total_contract_time=project.total_contract_time,
                this_month_contract_time=monthly_contract_time,
                total_profit=project.profit.year,
                this_month_profit=monthly_average_gross_profit,
                this_month_direct_support_man_hour=this_month_direct_support_man_hour,
                this_month_direct_support_man_hour_main=this_month_direct_support_man_hour_main,
                this_month_direct_support_man_hour_sub=this_month_direct_support_man_hour_sub,
                this_month_pre_support_man_hour=this_month_pre_support_man_hour,
                summary_direct_support_man_hour=summary_direct_support_man_hour,
                summary_pre_support_man_hour=summary_pre_support_man_hour,
                summary_theoretical_direct_support_man_hour=0,
                summary_theoretical_pre_support_man_hour=0,
                this_month_theoretical_direct_support_man_hour=0,
                this_month_theoretical_pre_support_man_hour=0,
                this_month_supporter_direct_support_man_hours=this_month_supporter_direct_support_man_hours,
                this_month_supporter_pre_support_man_hours=this_month_supporter_pre_support_man_hours,
                summary_supporter_direct_support_man_hours=summary_supporter_direct_support_man_hours,
                summary_supporter_pre_support_man_hours=summary_supporter_pre_support_man_hours,
                create_at=current_datetime,
                update_at=current_datetime,
            )
            project_summary_batch.save(man_hour_project_summary)


def save_project_summary_of_future(summary_year_month: str) -> None:
    """
        先日付分のプロジェクト別工数集計の編集・登録処理
        ・指定された基準年月のプロジェクト別工数集計を支援工数情報に登録

    Args:
        summary_year_month (str): 基準年月(YYYY/MM)
    """
    summary_month_datetime: datetime = datetime.strptime(summary_year_month, "%Y/%m")

    ########################################################
    # データ洗い替えの為、処理前に前回作成分の
    # プロジェクト別工数集計の処理対象の年月のデータを削除
    ########################################################
    logger.info(f"集計月{summary_year_month}の前回作成データ削除")
    del_year_month = summary_month_datetime.strftime("%Y/%m")
    del_project_summary_range_key_condition = (
        ManHourProjectSummaryModel.data_type.startswith(
            ManHourDataTypePrefix.PROJECT_SUMMARY
        )
    )
    del_man_hour_project_summary_list: List[ManHourProjectSummaryModel] = list(
        ManHourProjectSummaryModel.year_month_data_type_index.query(
            hash_key=del_year_month,
            range_key_condition=del_project_summary_range_key_condition,
        )
    )
    with ManHourProjectSummaryModel.batch_write() as del_project_summary_batch:
        count = 0
        for del_summary in del_man_hour_project_summary_list:
            count += 1
            logger.info(
                f"集計月{summary_year_month}の前回作成データ削除 - 処理開始 ({count}/{len(del_man_hour_project_summary_list)})"
            )
            del_project_summary_batch.delete(del_summary)

    ######################################################
    # 案件情報取得
    # 抽出条件
    # ・集計月時点で支援期間中の案件
    # ・工数管理設定あり(is_count_man_hour=True)
    ######################################################
    summary_month_from_datetime: datetime = summary_month_datetime + relativedelta(
        day=1
    )
    summary_month_to_datetime: datetime = summary_month_datetime + relativedelta(
        months=+1, day=1, days=-1
    )
    summary_month_from = summary_month_from_datetime.strftime("%Y/%m/%d")
    summary_month_to = summary_month_to_datetime.strftime("%Y/%m/%d")
    project_range_key_condition = ProjectModel.support_date_to >= summary_month_from
    bool_true: bool = True
    project_filter_condition = ProjectModel.support_date_from <= summary_month_to
    project_filter_condition &= ProjectModel.is_count_man_hour == bool_true
    project_model_list: List[ProjectModel] = list(
        ProjectModel.data_type_support_date_to_index.query(
            hash_key=DataType.PROJECT,
            range_key_condition=project_range_key_condition,
            filter_condition=project_filter_condition,
        )
    )
    logger.info(f"集計月{summary_year_month}の案件情報取得件数: {len(project_model_list)}件")
    for project in project_model_list:
        logger.info(f"処理対象の案件ID: {project.id}")
    user_list: List[UserModel] = list(UserModel.scan())

    # プロジェクト別工数集計の編集
    current_datetime = datetime.now()
    count = 0
    with ManHourProjectSummaryModel.batch_write() as project_summary_batch:
        for project in project_model_list:
            count += 1
            logger.info(
                f"data_type:{ManHourDataTypePrefix.PROJECT_SUMMARY + project.id},year_month:{summary_year_month} - 処理開始 ({count}/{len(project_model_list)})"
            )

            project_supporter_organization_id = (
                project.supporter_organization_id
                if project.supporter_organization_id
                else ""
            )

            # メイン支援者（プロデューサー）
            main_supporter_user: SupporterUsersAttribute = None
            if project.main_supporter_user_id:
                name = ""
                org_id = ""
                for user in user_list:
                    if user.id == project.main_supporter_user_id:
                        name = user.name
                        # 支援者責任者の場合、支援者組織を複数持つため先頭の組織IDを取得
                        if user.supporter_organization_id:
                            org_id = user.supporter_organization_id[0]
                        break
                main_supporter_user = SupporterUsersAttribute(
                    id=project.main_supporter_user_id,
                    organization_id=org_id,
                    name=name,
                    is_confirm=False,
                )

            # 支援者メンバー（アクセラレーター）
            supporter_users: List[SupporterUsersAttribute] = None
            if project.supporter_user_ids:
                supporter_users = []
                for supporter_user_id in project.supporter_user_ids:
                    name = ""
                    org_id = ""
                    for user in user_list:
                        if user.id == supporter_user_id:
                            name = user.name
                            # 支援者責任者の場合、支援者組織を複数持つため先頭の組織IDを取得
                            if user.supporter_organization_id:
                                org_id = user.supporter_organization_id[0]
                            break
                    supporter_user = SupporterUsersAttribute(
                        id=supporter_user_id,
                        organization_id=org_id,
                        name=name,
                        is_confirm=False,
                    )
                    supporter_users.append(supporter_user)

            monthly_calc_info = calc_monthly_average_gross_profit_and_contract_time(
                project=project,
                summary_month_from_date=summary_month_from_datetime.date(),
                summary_month_to_date=summary_month_to_datetime.date(),
            )

            # 平均（当月）粗利額
            monthly_average_gross_profit = monthly_calc_info[
                "monthly_average_gross_profit"
            ]

            # 当月契約時間
            monthly_contract_time = monthly_calc_info["monthly_contract_time"]

            man_hour_project_summary = ManHourProjectSummaryModel(
                data_type=ManHourDataTypePrefix.PROJECT_SUMMARY + project.id,
                year_month=summary_year_month,
                project_id=project.id,
                project_name=project.name,
                customer_id=project.customer_id,
                customer_name=project.customer_name,
                supporter_organization_id=project_supporter_organization_id,
                service_type=project.service_type,
                support_date_from=project.support_date_from,
                support_date_to=project.support_date_to,
                contract_type=project.contract_type,
                main_supporter_user=main_supporter_user,
                supporter_users=supporter_users,
                total_contract_time=project.total_contract_time,
                this_month_contract_time=monthly_contract_time,
                total_profit=project.profit.year,
                this_month_profit=monthly_average_gross_profit,
                create_at=current_datetime,
                update_at=current_datetime,
            )
            project_summary_batch.save(man_hour_project_summary)


def save_service_type_summary_of_last_or_current_month(summary_year_month: str) -> None:
    """
        サービス別工数集計の編集・登録処理（前月または当月用）
        ・指定された基準年月のサービス別工数集計を支援工数情報に登録

    Args:
        summary_year_month (str): 基準年月(YYYY/MM)
    """
    summary_month_datetime: datetime = datetime.strptime(summary_year_month, "%Y/%m")

    ########################################################
    # データ洗い替えの為、処理前に前回作成分の
    # サービス別工数集計の処理対象の年月のデータを削除
    ########################################################
    logger.info(f"集計月{summary_year_month}の前回作成データ削除")
    del_year_month = summary_month_datetime.strftime("%Y/%m")
    del_service_type_summary_range_key_condition = (
        ManHourServiceTypeSummaryModel.data_type.startswith(
            ManHourDataTypePrefix.SERVICE_TYPE_SUMMARY
        )
    )
    del_man_hour_service_type_summary_list: List[ManHourServiceTypeSummaryModel] = list(
        ManHourServiceTypeSummaryModel.year_month_data_type_index.query(
            hash_key=del_year_month,
            range_key_condition=del_service_type_summary_range_key_condition,
        )
    )
    with ManHourServiceTypeSummaryModel.batch_write() as del_service_type_summary_batch:
        count = 0
        for del_summary in del_man_hour_service_type_summary_list:
            count += 1
            logger.info(
                f"集計月{summary_year_month}の前回作成データ削除 - 処理開始 ({count}/{len(del_man_hour_service_type_summary_list)})"
            )
            del_service_type_summary_batch.delete(del_summary)

    # 支援者別工数の取得
    man_hour_supporter_range_key_condition = ManHourSupporterModel.data_type.startswith(
        ManHourDataTypePrefix.SUPPORTER
    )
    man_hour_supporter_list: List[ManHourSupporterModel] = list(
        ManHourSupporterModel.year_month_data_type_index.query(
            hash_key=summary_year_month,
            range_key_condition=man_hour_supporter_range_key_condition,
        )
    )
    logger.info(
        f"集計月{summary_year_month}の支援者別工数情報の取得件数: {len(man_hour_supporter_list)}件"
    )

    ###########################
    # 総工数の算出
    ###########################
    logger.info("総工数の算出 - 処理開始")
    # 総工数
    total_man_hour = 0
    for man_hour in man_hour_supporter_list:
        total_man_hour = (
            total_man_hour
            + man_hour.summary_man_hour.direct
            + man_hour.summary_man_hour.pre
            + man_hour.summary_man_hour.sales
            + man_hour.summary_man_hour.ssap
        )

    logger.debug(f"総工数: {total_man_hour}")

    ######################################################
    # 案件情報取得
    # 抽出条件
    # ・集計月時点で支援期間中の案件
    # ・工数管理設定あり(is_count_man_hour=True)
    ######################################################
    summary_month_from_datetime: datetime = summary_month_datetime + relativedelta(
        day=1
    )
    summary_month_to_datetime: datetime = summary_month_datetime + relativedelta(
        months=+1, day=1, days=-1
    )
    summary_month_from = summary_month_from_datetime.strftime("%Y/%m/%d")
    summary_month_to = summary_month_to_datetime.strftime("%Y/%m/%d")
    project_range_key_condition = ProjectModel.support_date_to >= summary_month_from
    bool_true: bool = True
    project_filter_condition = ProjectModel.support_date_from <= summary_month_to
    project_filter_condition &= ProjectModel.is_count_man_hour == bool_true
    project_model_list: List[ProjectModel] = list(
        ProjectModel.data_type_support_date_to_index.query(
            hash_key=DataType.PROJECT,
            range_key_condition=project_range_key_condition,
            filter_condition=project_filter_condition,
        )
    )
    logger.info(f"集計月{summary_year_month}の案件情報取得件数: {len(project_model_list)}件")

    if not project_model_list:
        # 集計月の案件情報が0件の場合
        # 終了
        logger.info(f"案件情報が0件のため、集計月{summary_year_month}の処理終了")
        return

    #######################################
    # 当月契約時間・支援者数の算出
    #######################################
    calc_info_list: List[dict] = []
    supporter_map_by_service_type: dict[str, set] = {}
    count = 0
    for project in project_model_list:
        count += 1
        logger.info(f"当月契約時間・支援者数の算出 - 処理開始 ({count}/{len(project_model_list)})")

        # 案件単位の算出情報(dict)
        # key:
        #  service_type_id
        #  project_id
        #  contract_type: 契約形態（有償/無償）
        #  contract_time_only_for_value: 当月契約時間（有償のみ）
        #  contract_time_include_free: 当月契約時間（無償含む）
        calc_info_by_project: dict = {}
        calc_info_by_project["service_type_id"] = project.service_type
        calc_info_by_project["project_id"] = project.id
        calc_info_by_project["contract_type"] = project.contract_type

        monthly_calc_info = calc_monthly_average_gross_profit_and_contract_time(
            project=project,
            summary_month_from_date=summary_month_from_datetime.date(),
            summary_month_to_date=summary_month_to_datetime.date(),
        )

        # 当月契約時間（無償含む）
        calc_info_by_project["contract_time_include_free"] = monthly_calc_info[
            "monthly_contract_time"
        ]

        # 当月契約時間（有償のみ）
        calc_info_by_project["contract_time_only_for_value"] = (
            monthly_calc_info["monthly_contract_time"]
            if project.contract_type == ContractType.FOR_VALUE
            else float(0)
        )

        calc_info_list.append(calc_info_by_project)

        # サービス種別単位の支援者情報（重複は排除）をdictに追加
        # 支援者のユーザIDはset型に格納するため、重複は排除される
        # key:
        #  service_type_id
        # value:
        #  支援者ID（プロデューサー、アクセラレーターのユーザID）の集合（Set型）
        if supporter_map_by_service_type.get(project.service_type):
            temp_supporter_set: set = supporter_map_by_service_type[
                project.service_type
            ]
        else:
            temp_supporter_set: set = set()
        # プロデューサーを追加
        if project.main_supporter_user_id:
            temp_supporter_set.add(project.main_supporter_user_id)
        # アクセラレーターを追加
        if project.supporter_user_ids:
            temp_supporter_set.update(project.supporter_user_ids)
        # 辞書に格納
        supporter_map_by_service_type[project.service_type] = temp_supporter_set

    # pandas.DataFrameに展開
    # 以下情報のリスト
    # pd_service_info:
    #  service_type_id                   6722577d-1af8-487c-abe4-f327eb1d6006
    #  project_id                        60b8274c-69ce-4b43-8e3b-51e4d33bdffa
    #  contract_type                                                     有償
    #  contract_time_include_free                                        33.0
    #  contract_time_only_for_value                                      33.0
    pd_service_info = pd.DataFrame(calc_info_list)

    # 当月契約時間
    # サービス種別単位で集計
    # pivot_table_service_by_service_type:
    #                                       contract_time_include_free  contract_time_only_for_value
    # service_type_id
    # 43699ec5-4154-454b-af35-5cd5069e3e96                       125.0                         125.0
    # 46ccde4e-1ec1-43cb-900c-5399a454f37c                       479.2                         479.2
    # 6722577d-1af8-487c-abe4-f327eb1d6006                       412.2                         411.8
    pivot_table_service_by_service_type = pd.pivot_table(
        data=pd_service_info,
        index=["service_type_id"],
        aggfunc="sum",
    )

    # サービス種別単位の支援者数
    supporter_count_list_by_service_type: List[dict] = []
    for service_type_id, supporter_set in supporter_map_by_service_type.items():
        temp_supporter_count_dict: dict = {}
        temp_supporter_count_dict["service_type_id"] = service_type_id
        temp_supporter_count_dict["supporter_count"] = float(len(supporter_set))
        supporter_count_list_by_service_type.append(temp_supporter_count_dict)

    # pandas.DataFrameに展開
    # pd_supporter_count:
    #                                       supporter_count
    # service_type_id
    # 6722577d-1af8-487c-abe4-f327eb1d6006              9.0
    # 46ccde4e-1ec1-43cb-900c-5399a454f37c             11.0
    # 43699ec5-4154-454b-af35-5cd5069e3e96              7.0
    pd_supporter_count = pd.DataFrame(supporter_count_list_by_service_type)
    pd_supporter_count = pd_supporter_count.set_index("service_type_id")

    # サービス種別単位の当月契約時間の集計DataFrameにサービス種別単位の支援者数DataFrameを外部結合(left outer join). NaNは0に置換.
    # pivot_table_service_by_service_type:
    #                                       contract_time_include_free  ...  supporter_count
    # service_type_id                                                   ...
    # 43699ec5-4154-454b-af35-5cd5069e3e96                       125.0  ...              7.0
    # 46ccde4e-1ec1-43cb-900c-5399a454f37c                       479.2  ...             11.0
    # 6722577d-1af8-487c-abe4-f327eb1d6006                       412.2  ...              9.0
    #
    # e.g. 以下を1行とするDataFrame
    # pivot_table_service_by_service_type:
    #  service_type_id   -> index
    #  contract_time_include_free           412.2
    #  contract_time_only_for_value         411.8
    #  supporter_count                        9.0
    pivot_table_service_by_service_type = pd.merge(
        pivot_table_service_by_service_type,
        pd_supporter_count,
        how="left",
        on="service_type_id",
    ).fillna(0)

    # 「按分比率」の列を追加
    if pivot_table_service_by_service_type["supporter_count"].sum() > 0:
        pivot_table_service_by_service_type["man_hour_rate"] = (
            pivot_table_service_by_service_type["supporter_count"]
            / pivot_table_service_by_service_type["supporter_count"].sum()
        )
        # ゼロ除算の場合は値がinfになるため、0に置換
        pivot_table_service_by_service_type.loc[
            pivot_table_service_by_service_type["supporter_count"] == 0,
            "man_hour_rate",
        ] = 0.0
    else:
        pivot_table_service_by_service_type["man_hour_rate"] = 0

    # 追加した列の四捨五入
    pivot_table_service_by_service_type[
        "man_hour_rate"
    ] = pivot_table_service_by_service_type["man_hour_rate"].apply(
        decimal_round, digit=RoundSetting.DECIMAL_DIGITS_RATE_COLUMN
    )

    # 「サービス毎の総工数」の列を追加
    pivot_table_service_by_service_type["total_man_hour_by_service"] = (
        total_man_hour * pivot_table_service_by_service_type["man_hour_rate"]
    )
    # 追加した列の四捨五入
    pivot_table_service_by_service_type[
        "total_man_hour_by_service"
    ] = pivot_table_service_by_service_type["total_man_hour_by_service"].apply(
        decimal_round, digit=RoundSetting.DECIMAL_DIGITS
    )

    # 「Y%」の列を追加 : 工数アラート設定の「対面支援工数＋支援仕込工数」
    rate_y_percent_list: dict = {}
    for service_type_id in pivot_table_service_by_service_type.index:
        rate_y_percent_list[
            service_type_id
        ] = get_direct_and_pre_support_man_hour_of_alert_setting(service_type_id)

    pivot_table_service_by_service_type["alert_y_percent"] = pd.Series(
        rate_y_percent_list
    )

    # 「総工数のY%」の列を追加
    pivot_table_service_by_service_type["total_mon_hour_y_percent"] = (
        pivot_table_service_by_service_type["total_man_hour_by_service"]
        * pivot_table_service_by_service_type["alert_y_percent"]
        / 100
    )
    # ゼロ除算の場合は値がinfになるため、0に置換
    pivot_table_service_by_service_type.loc[
        (pivot_table_service_by_service_type["total_man_hour_by_service"] == 0)
        | (pivot_table_service_by_service_type["alert_y_percent"] == 0),
        "total_mon_hour_y_percent",
    ] = 0.0
    # 追加した列の四捨五入
    pivot_table_service_by_service_type[
        "total_mon_hour_y_percent"
    ] = pivot_table_service_by_service_type["total_mon_hour_y_percent"].apply(
        decimal_round, digit=RoundSetting.DECIMAL_DIGITS
    )

    # 「契約時間1時間あたりの係数」の列を追加
    pivot_table_service_by_service_type["factor"] = (
        pivot_table_service_by_service_type["total_mon_hour_y_percent"]
        / pivot_table_service_by_service_type["contract_time_include_free"]
    )
    # ゼロ除算の場合は値がinfになるため、0に置換
    pivot_table_service_by_service_type.loc[
        (pivot_table_service_by_service_type["total_mon_hour_y_percent"] == 0)
        | (pivot_table_service_by_service_type["contract_time_include_free"] == 0),
        "factor",
    ] = 0.0
    # 追加した列の四捨五入
    pivot_table_service_by_service_type["factor"] = pivot_table_service_by_service_type[
        "factor"
    ].apply(decimal_round, digit=RoundSetting.DECIMAL_DIGITS)

    # 上記で編集したDataFramの構造は以下の通り.
    # e.g.
    #  pivot_table_service_by_service_type:
    #    * 以下を1行とするDataFrame
    #    service_type_id   -> index
    #    contract_time_include_free      25.800
    #    contract_time_only_for_value    25.800
    #    supporter_count                  3.000
    #    man_hour_rate                    0.136
    #    total_man_hour_by_service        9.100
    #    alert_y_percent                 60.000
    #    total_mon_hour_y_percent         5.500
    #    factor                           0.200

    for x in range(pivot_table_service_by_service_type.shape[0]):
        logger.debug(
            f"サービス種別単位集計 : pivot_table_service_by_service_type.iloc[index]:{pivot_table_service_by_service_type.iloc[x]}"
        )

    # サービス種別別工数集計の登録
    logger.info(
        f"集計月{summary_year_month}のサービス種別件数: {len(pivot_table_service_by_service_type)}件"
    )
    current_datetime = datetime.now()
    count = 0
    with ManHourServiceTypeSummaryModel.batch_write() as service_type_summary_batch:
        for row in pivot_table_service_by_service_type.itertuples():
            count += 1
            logger.info(
                f"year_month:{summary_year_month} - サービス種別別工数集計の登録 ({count}/{len(pivot_table_service_by_service_type)})"
            )

            service_type_name = ""
            for master_service_type in master_service_type_list:
                if master_service_type.id == row.Index:
                    service_type_name = master_service_type.name
                    break

            # pandas.DataFrameに列追加する際に端数処理(四捨五入)しているが、
            # そのままDB登録すると端数処理(四捨五入)されていないため、再度処理する
            service_type_summary = ManHourServiceTypeSummaryModel(
                data_type=ManHourDataTypePrefix.SERVICE_TYPE_SUMMARY + row.Index,
                year_month=summary_year_month,
                service_type_id=row.Index,
                service_type_name=service_type_name,
                this_month_contract_time=decimal_round(
                    row.contract_time_include_free, RoundSetting.DECIMAL_DIGITS
                ),
                this_month_paid_contract_time=decimal_round(
                    row.contract_time_only_for_value, RoundSetting.DECIMAL_DIGITS
                ),
                number_of_supporters=decimal_round(
                    row.supporter_count, RoundSetting.DECIMAL_DIGITS
                ),
                man_hour_rate=decimal_round(
                    row.man_hour_rate, RoundSetting.DECIMAL_DIGITS_RATE_COLUMN
                ),
                total_man_hour=decimal_round(
                    row.total_man_hour_by_service, RoundSetting.DECIMAL_DIGITS
                ),
                total_man_hour_y_percent=decimal_round(
                    row.total_mon_hour_y_percent, RoundSetting.DECIMAL_DIGITS
                ),
                factor=decimal_round(row.factor, RoundSetting.DECIMAL_DIGITS),
                create_at=current_datetime,
                update_at=current_datetime,
            )
            service_type_summary_batch.save(service_type_summary)


def save_supporter_organization_summary_of_last_or_current_month(
    summary_year_month: str,
) -> None:
    """
        支援者組織（課）別工数集計の編集・登録処理（前月または当月用）
        ・指定された基準年月の支援者組織（課）別工数集計を支援工数情報に登録

    Args:
        summary_year_month (str): 基準年月(YYYY/MM)
    """
    summary_month_datetime: datetime = datetime.strptime(summary_year_month, "%Y/%m")

    ########################################################
    # データ洗い替えの為、処理前に前回作成分の
    # 支援者組織（課）別工数集計の処理対象の年月のデータを削除
    ########################################################
    logger.info(f"集計月{summary_year_month}の前回作成データ削除")
    del_year_month = summary_month_datetime.strftime("%Y/%m")
    del_supporter_organization_summary_range_key_condition = (
        ManHourSupporterOrganizationSummaryModel.data_type.startswith(
            ManHourDataTypePrefix.SUPPORTER_ORGANIZATION_SUMMARY
        )
    )
    del_man_hour_supporter_organization_summary_list: List[
        ManHourSupporterOrganizationSummaryModel
    ] = list(
        ManHourSupporterOrganizationSummaryModel.year_month_data_type_index.query(
            hash_key=del_year_month,
            range_key_condition=del_supporter_organization_summary_range_key_condition,
        )
    )
    with ManHourSupporterOrganizationSummaryModel.batch_write() as del_supporter_organization_summary_batch:
        count = 0
        for del_summary in del_man_hour_supporter_organization_summary_list:
            count += 1
            logger.info(
                f"集計月{summary_year_month}の前回作成データ削除 - 処理開始 ({count}/{len(del_man_hour_supporter_organization_summary_list)})"
            )
            del_supporter_organization_summary_batch.delete(del_summary)

    ######################################################
    # 案件情報取得
    # 抽出条件
    # ・集計月時点で支援期間中の案件
    # ・工数管理設定あり(is_count_man_hour=True)
    ######################################################
    summary_month_from_datetime: datetime = summary_month_datetime + relativedelta(
        day=1
    )
    summary_month_to_datetime: datetime = summary_month_datetime + relativedelta(
        months=+1, day=1, days=-1
    )
    summary_month_from = summary_month_from_datetime.strftime("%Y/%m/%d")
    summary_month_to = summary_month_to_datetime.strftime("%Y/%m/%d")
    project_range_key_condition = ProjectModel.support_date_to >= summary_month_from
    bool_true: bool = True
    project_filter_condition = ProjectModel.support_date_from <= summary_month_to
    project_filter_condition &= ProjectModel.is_count_man_hour == bool_true
    project_model_list: List[ProjectModel] = list(
        ProjectModel.data_type_support_date_to_index.query(
            hash_key=DataType.PROJECT,
            range_key_condition=project_range_key_condition,
            filter_condition=project_filter_condition,
        )
    )
    logger.info(f"集計月{summary_year_month}の案件情報取得件数: {len(project_model_list)}件")

    if not project_model_list:
        # 集計月{summary_year_month}の案件情報が0件の場合
        # 終了
        logger.info(f"案件情報が0件のため、集計月{summary_year_month}の処理終了")
        return

    #######################################
    # 当月契約時間・月商（月の粗利）の算出
    #######################################
    calc_info_list: List[dict] = []
    count = 0
    for project in project_model_list:
        count += 1
        logger.info(f"当月契約時間・月商（月の粗利）の算出 - 処理開始 ({count}/{len(project_model_list)})")

        # 案件単位の算出情報(dict)
        # key:
        #  supporter_organization_id: 粗利メイン課
        #  project_id
        #  contract_type: 契約形態（有償/無償）
        #  contract_time_only_for_value: 当月契約時間（有償のみ）
        #  monthly_gross_profit: 月商（月の粗利）
        calc_info_by_project: dict = {}
        calc_info_by_project["supporter_organization_id"] = (
            project.supporter_organization_id
            if project.supporter_organization_id
            else ""
        )
        calc_info_by_project["project_id"] = project.id
        calc_info_by_project["contract_type"] = project.contract_type

        monthly_calc_info = calc_monthly_average_gross_profit_and_contract_time(
            project=project,
            summary_month_from_date=summary_month_from_datetime.date(),
            summary_month_to_date=summary_month_to_datetime.date(),
        )

        # 平均（当月）粗利額
        # 契約形態が無償の場合、0で計算される
        calc_info_by_project["monthly_gross_profit"] = monthly_calc_info[
            "monthly_average_gross_profit"
        ]

        # 当月契約時間（有償のみ）
        calc_info_by_project["contract_time_only_for_value"] = (
            monthly_calc_info["monthly_contract_time"]
            if project.contract_type == ContractType.FOR_VALUE
            else float(0)
        )

        calc_info_list.append(calc_info_by_project)

    # pandas.DataFrameに展開
    # e.g. 以下を1行とするDataFrame
    #  * ここに含まれるsupporter_organization_idは粗利メイン課であり、所属課ではない。
    #  pd_supporter_organization_info:
    #   supporter_organization_id       180a3597-b7e7-42c8-902c-a29016afa662
    #   project_id                      2854fc20-caea-44bb-ada5-d5ec3595f4c0
    #   contract_type                                                   有償
    #   monthly_gross_profit                                        978947.4
    #   contract_time_only_for_value                                    61.2
    pd_supporter_organization_info = pd.DataFrame(calc_info_list)

    #######################################
    # 課毎の稼働人数、粗利、契約時間の算出
    #######################################
    logger.info("課毎の稼働人数、粗利、契約時間の算出 - 処理開始")

    # 支援者別工数の取得
    man_hour_supporter_range_key_condition = ManHourSupporterModel.data_type.startswith(
        ManHourDataTypePrefix.SUPPORTER
    )
    man_hour_supporter_list: List[ManHourSupporterModel] = list(
        ManHourSupporterModel.year_month_data_type_index.query(
            hash_key=summary_year_month,
            range_key_condition=man_hour_supporter_range_key_condition,
        )
    )
    logger.info(
        f"集計月{summary_year_month}の支援者別工数情報の取得件数: {len(man_hour_supporter_list)}件"
    )

    if man_hour_supporter_list:
        # 集計月の支援者別工数情報が存在する場合

        # modelのリスト -> pandasのDataFrame (Map等は子要素も展開.但し、リスト部分は展開不可)
        # e.g. 以下を1行とするDataFrame
        #  df:
        #  create_at                                                2022-07-25T12:07:27.014841+09:00
        #  update_at                                                2022-07-25T12:07:27.014860+09:00
        #  create_id                                            fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7
        #  data_type                                  supporter#278be7f8-c46f-4e2d-b3df-ffdf12278ad1
        #  is_confirm                                                                           True
        #  supporter_name                                                                   支援者１
        #  supporter_organization_id                            180a3597-b7e7-42c8-902c-a29016afa662
        #  supporter_organization_name                                                           IST
        #  supporter_user_id                                    278be7f8-c46f-4e2d-b3df-ffdf12278ad1
        #  update_id                                            fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7
        #  version                                                                                 1
        #  year_month                                                                        2022/06
        #  direct_support_man_hours.items          [{'input_man_hour': 30, 'project_id': '2bb85e8...
        #  direct_support_man_hours.memo                                                        メモ
        #  holidays_man_hours.department_others                                                    0
        #  .........
        df = pd.json_normalize([model_to_dict(x) for x in man_hour_supporter_list])
        # 粗利メイン課が設定されていない場合、追加
        if "supporter_organization_id" not in df.columns:
            df["supporter_organization_id"] = ""
        if "supporter_organization_name" not in df.columns:
            df["supporter_organization_name"] = ""
        # 粗利メイン課がNaNの場合、空文字に置換
        df = df.fillna(
            {"supporter_organization_id": "", "supporter_organization_name": ""}
        )

        # DataFrameのリスト要素 -> 別のDataFrameに展開
        # 直接支援工数(1.対面支援)
        # e.g. 以下を1行とするDataFrame
        #  df_direct:
        #   input_man_hour                                               30.0
        #   project_id                   2bb85e8d-7560-48b0-82c7-d8ba134a9266
        #   role                                                プロデューサー
        #   service_type                 06f65c34-1570-4c02-a892-b2cf6392451a
        #   supporter_user_id            278be7f8-c46f-4e2d-b3df-ffdf12278ad1
        #   supporter_organization_id    180a3597-b7e7-42c8-902c-a29016afa662
        df_direct = pd.json_normalize(
            df.to_dict("records"),
            record_path="direct_support_man_hours.items",
            meta=["supporter_user_id", "supporter_organization_id"],
        )
        # 仕込支援工数(2.支援仕込)
        df_pre = pd.json_normalize(
            df.to_dict("records"),
            record_path="pre_support_man_hours.items",
            meta=["supporter_user_id", "supporter_organization_id"],
        )

        # 1.対面支援、2.支援仕込の稼働人数を集計するため、1と2をマージ
        direct_and_pre_list: List[dict] = []
        for data in [df_direct, df_pre]:
            for row in data.itertuples():
                add_flag = True
                direct_and_pre_man_hour_items: dict = {}
                for direct_and_pre in direct_and_pre_list:
                    if (
                        direct_and_pre["supporter_organization_id"]
                        == row.supporter_organization_id
                        and direct_and_pre["supporter_user_id"] == row.supporter_user_id
                        and direct_and_pre["project_id"] == row.project_id
                        and direct_and_pre["service_type"] == row.service_type
                    ):
                        # 存在する場合は工数を加算
                        direct_and_pre["input_man_hour"] += row.input_man_hour
                        add_flag = False
                        break

                if add_flag:
                    direct_and_pre_man_hour_items[
                        "supporter_organization_id"
                    ] = row.supporter_organization_id
                    direct_and_pre_man_hour_items[
                        "supporter_user_id"
                    ] = row.supporter_user_id
                    direct_and_pre_man_hour_items["project_id"] = row.project_id
                    direct_and_pre_man_hour_items["service_type"] = row.service_type
                    direct_and_pre_man_hour_items["input_man_hour"] = row.input_man_hour
                    direct_and_pre_list.append(direct_and_pre_man_hour_items)

        if direct_and_pre_list:
            # 直接支援工数(1.対面支援)、または仕込支援工数(2.支援仕込)が入力されている場合

            # df_directとdf_preをマージしたリスト -> DataFrameに展開
            # 直接支援工数(1.対面支援)、仕込支援工数(2.支援仕込)
            # e.g. 以下を1行とするDataFrame
            #  df_direct_pre_input_man_hour:
            #   supporter_organization_id    180a3597-b7e7-42c8-902c-a29016afa662
            #   supporter_user_id            278be7f8-c46f-4e2d-b3df-ffdf12278ad1
            #   project_id                   2bb85e8d-7560-48b0-82c7-d8ba134a9266
            #   service_type                 06f65c34-1570-4c02-a892-b2cf6392451a
            #   input_man_hour                                               50.0
            df_direct_pre_input_man_hour = pd.DataFrame(direct_and_pre_list)

            # 所属課別の稼働人数集計
            # 直接支援工数(1.対面支援)、仕込支援工数(2.支援仕込)
            # e.g.
            #  pivot_table_direct:
            #   supporter_organization_id           manpower_180a3597-b7e7-42c8-902c-a29016afa662  ...  manpower_total
            #   project_id                                                                         ...
            #   2854fc20-caea-44bb-ada5-d5ec3595f4c0                                            2  ...      4
            #   2bb85e8d-7560-48b0-82c7-d8ba134a9266                                            1  ...      4
            #   ca57d2cd-a898-4344-bdbd-9811fb26e19f                                            2  ...      5
            #   total                                                                           5  ...     13
            pivot_table_direct_pre_input_count = pd.pivot_table(
                data=df_direct_pre_input_man_hour,
                index=["project_id"],
                columns="supporter_organization_id",
                values="input_man_hour",
                margins=True,
                margins_name="total",
                aggfunc=get_count_greater_than_zero,
            ).fillna(0)
            pivot_table_direct_pre_input_count = (
                pivot_table_direct_pre_input_count.add_prefix("manpower_")
            )

            # DataFrameに所属課別の稼働人数集計を外部結合(left outer join). NaNは0に置換.
            # e.g. 以下を1行とするDataFrame
            #  pd_supporter_organization_info:
            #   supporter_organization_id                        180a3597-b7e7-42c8-902c-a29016afa662
            #   project_id                                       2854fc20-caea-44bb-ada5-d5ec3595f4c0
            #   contract_type                                                                    有償
            #   monthly_gross_profit                                                         978947.4
            #   contract_time_only_for_value                                                     61.2
            #   manpower_180a3597-b7e7-42c8-902c-a29016afa662                                     2.0
            #   manpower_de40733f-6be9-4fef-8229-01052f43c1e2                                     3.0
            #   manpower_total                                                                    5.0
            pd_supporter_organization_info = pd.merge(
                pd_supporter_organization_info,
                pivot_table_direct_pre_input_count,
                how="left",
                on="project_id",
            ).fillna(0)

        else:
            # 直接支援工数(1.対面支援)、および仕込支援工数(2.支援仕込)が共に未入力の場合

            # DataFrameに所属課別の稼働人数集計を外部結合(left outer join). NaNは0に置換.
            # e.g. 以下を1行とするDataFrame
            #  pd_supporter_organization_info:
            #   supporter_organization_id                        180a3597-b7e7-42c8-902c-a29016afa662
            #   project_id                                       2854fc20-caea-44bb-ada5-d5ec3595f4c0
            #   contract_type                                                                    有償
            #   monthly_gross_profit                                                         978947.4
            #   contract_time_only_for_value                                                     61.2
            #   manpower_total                                                                    5.0
            pd_supporter_organization_info = pd_supporter_organization_info.fillna(0)
            # 集計月の支援者別工数情報が0件のため、manpower_total列のみ追加する
            pd_supporter_organization_info["manpower_total"] = 0

    else:
        # 集計月の支援者別工数情報が0件の場合

        # DataFrameに所属課別の稼働人数集計を外部結合(left outer join). NaNは0に置換.
        # e.g. 以下を1行とするDataFrame
        #  pd_supporter_organization_info:
        #   supporter_organization_id                        180a3597-b7e7-42c8-902c-a29016afa662
        #   project_id                                       2854fc20-caea-44bb-ada5-d5ec3595f4c0
        #   contract_type                                                                    有償
        #   monthly_gross_profit                                                         978947.4
        #   contract_time_only_for_value                                                     61.2
        #   manpower_total                                                                    5.0
        pd_supporter_organization_info = pd_supporter_organization_info.fillna(0)
        # 集計月の支援者別工数情報が0件のため、manpower_total列のみ追加する
        pd_supporter_organization_info["manpower_total"] = 0

    # 誰も工数入力していない場合は粗利メイン課を１人としてカウント
    add_column_to_supporter_organization_info: List[dict] = []
    for index, row in pd_supporter_organization_info.iterrows():
        temp_add_column: dict = {}
        temp_add_column_value: dict = {}
        if row.manpower_total == 0:
            add_id = "manpower_" + row.supporter_organization_id
            # 誰も工数入力していない場合
            if add_id in pd_supporter_organization_info.columns:
                # 稼働人数集計の列に粗利メイン課の列が存在する場合
                # 粗利メイン課を１人としてカウント
                pd_supporter_organization_info.at[index, add_id] = 1.0
                pd_supporter_organization_info.at[index, "manpower_total"] = 1.0
            elif add_id in [
                x["column_name"] for x in add_column_to_supporter_organization_info
            ]:
                # 追加予定の列に含む場合
                for info in add_column_to_supporter_organization_info:
                    if info["column_name"] == add_id:
                        temp_add_column_value["index"] = index
                        temp_add_column_value["value"] = 1.0
                        info["data"].append(temp_add_column_value)
                        break
            else:
                # 稼働人数集計の列に粗利メイン課の列が存在しない場合、列を追加
                temp_add_column["column_name"] = add_id
                temp_add_column_value["index"] = index
                temp_add_column_value["value"] = 1.0
                temp_add_column["data"] = [temp_add_column_value]
                add_column_to_supporter_organization_info.append(temp_add_column)

    for info in add_column_to_supporter_organization_info:
        # 上記で編集した列を追加(合計行の前に追加)
        idx = pd_supporter_organization_info.columns.get_loc("manpower_total")
        pd_supporter_organization_info.insert(
            loc=idx, column=info["column_name"], value=0.0
        )
        # 必要な部分のみ値を編集
        for value_row in info["data"]:
            pd_supporter_organization_info.at[
                value_row["index"], info["column_name"]
            ] = value_row["value"]
            pd_supporter_organization_info.at[
                value_row["index"], "manpower_total"
            ] = 1.0

    manpower_columns_list: List[str] = pd_supporter_organization_info.filter(
        like="manpower_", axis=1
    ).columns.tolist()
    gross_profit_columns_list = [
        x.replace("manpower_", "gross_profit_") for x in manpower_columns_list
    ]
    contract_time_columns_list = [
        x.replace("manpower_", "contract_time_") for x in manpower_columns_list
    ]

    # 「課毎の粗利」列の追加
    for manpower, profit in zip(manpower_columns_list, gross_profit_columns_list):
        pd_supporter_organization_info[profit] = (
            pd_supporter_organization_info["monthly_gross_profit"]
            * pd_supporter_organization_info[manpower]
            / pd_supporter_organization_info["manpower_total"]
        )
        # ゼロ除算の場合は値がinfになるため、0に置換
        pd_supporter_organization_info.loc[
            (pd_supporter_organization_info["monthly_gross_profit"] == 0)
            | (pd_supporter_organization_info[manpower] == 0)
            | (pd_supporter_organization_info["manpower_total"] == 0),
            profit,
        ] = 0.0
        # 追加した列の四捨五入
        pd_supporter_organization_info[profit] = pd_supporter_organization_info[
            profit
        ].apply(decimal_round, digit=RoundSetting.DECIMAL_DIGITS)

    # 「課毎の契約時間」列の追加
    for manpower, contract_time in zip(
        manpower_columns_list, contract_time_columns_list
    ):
        pd_supporter_organization_info[contract_time] = (
            pd_supporter_organization_info["contract_time_only_for_value"]
            * pd_supporter_organization_info[manpower]
            / pd_supporter_organization_info["manpower_total"]
        )
        # ゼロ除算の場合は値がinfになるため、0に置換
        pd_supporter_organization_info.loc[
            (pd_supporter_organization_info["contract_time_only_for_value"] == 0)
            | (pd_supporter_organization_info[manpower] == 0)
            | (pd_supporter_organization_info["manpower_total"] == 0),
            contract_time,
        ] = 0.0
        # 追加した列の四捨五入
        pd_supporter_organization_info[contract_time] = pd_supporter_organization_info[
            contract_time
        ].apply(decimal_round, digit=RoundSetting.DECIMAL_DIGITS)

    # 「課毎の粗利」「課毎の契約時間」列の追加後のデータイメージ
    # e.g. 以下を1行とするDataFrame
    #  pd_supporter_organization_info:
    #   supporter_organization_id                             180a3597-b7e7-42c8-902c-a29016afa662
    #   project_id                                            2bb85e8d-7560-48b0-82c7-d8ba134a9266
    #   contract_type                                                                         有償
    #   monthly_gross_profit                                                              744000.0
    #   contract_time_only_for_value                                                          31.0
    #   manpower_180a3597-b7e7-42c8-902c-a29016afa662                                          2.0
    #   manpower_de40733f-6be9-4fef-8229-01052f43c1e2                                          3.0
    #   manpower_c6ce787c-90d7-4bd6-a9a9-566ffb174062                                          0.0
    #   manpower_total                                                                         5.0
    #   gross_profit_180a3597-b7e7-42c8-902c-a29016afa662                                 297600.0
    #   gross_profit_de40733f-6be9-4fef-8229-01052f43c1e2                                 446400.0
    #   gross_profit_c6ce787c-90d7-4bd6-a9a9-566ffb174062                                      0.0
    #   gross_profit_total                                                                744000.0
    #   contract_time_180a3597-b7e7-42c8-902c-a29016afa662                                    12.4
    #   contract_time_de40733f-6be9-4fef-8229-01052f43c1e2                                    18.6
    #   contract_time_c6ce787c-90d7-4bd6-a9a9-566ffb174062                                     0.0
    #   contract_time_total                                                                   31.0

    for x in range(pd_supporter_organization_info.shape[0]):
        logger.debug(
            f"所属課別集計 : pd_supporter_organization_info.iloc[index]:{pd_supporter_organization_info.iloc[x]}"
        )

    # 課別の平均粗利額
    # key: supporter_organization_id(所属課ID), value: 平均粗利額
    gross_profit_summary_for_value: dict = {}
    # 課別の契約時間（有償分）合計
    # key: supporter_organization_id(所属課ID), value: 契約時間（有償分）合計
    contract_time_summary_for_value: dict = {}
    # 課ごとの契約時間単価
    # key: supporter_organization_id(所属課ID), value: 契約時間単価
    contract_hourly_unit_price_for_value: dict = {}
    # 有償のみ抽出
    pd_supporter_organization_info_only_for_value = pd_supporter_organization_info[
        pd_supporter_organization_info["contract_type"] == "有償"
    ]

    for profit_column, contract_time_column in zip(
        gross_profit_columns_list, contract_time_columns_list
    ):
        if profit_column == "gross_profit_total":
            # 合計行はスキップ
            continue
        column_name_supporter_organization_id = profit_column.replace(
            "gross_profit_", ""
        )
        # 課別の平均粗利額
        gross_profit_summary_for_value[
            column_name_supporter_organization_id
        ] = pd_supporter_organization_info_only_for_value[profit_column].sum()
        # 課別の契約時間（有償分）合計
        contract_time_summary_for_value[
            column_name_supporter_organization_id
        ] = pd_supporter_organization_info_only_for_value[contract_time_column].sum()
        # 契約時間単価
        contract_hourly_unit_price_for_value[column_name_supporter_organization_id] = (
            gross_profit_summary_for_value[column_name_supporter_organization_id]
            / contract_time_summary_for_value[column_name_supporter_organization_id]
            if gross_profit_summary_for_value[column_name_supporter_organization_id]
            and contract_time_summary_for_value[column_name_supporter_organization_id]
            else 0
        )

    # 課毎の有償案件の月稼働時間直接寄与
    # key: supporter_organization_id(所属課ID), value: 課毎の月稼働時間の直接寄与分
    direct_contribution_to_monthly_manpower: dict = {}
    # 課毎の月人員数
    # key: supporter_organization_id(所属課ID), value: 課毎の月人員数
    supporter_count_monthly: dict = {}
    # 課毎の月総工数
    # key: supporter_organization_id(所属課ID), value: 月総工数
    monthly_all_man_hour_total: dict = {}
    # 課毎の月稼働時間仕込含
    # key: supporter_organization_id(所属課ID), value: 月稼働時間仕込含
    monthly_man_hour_total_direct_and_pre: dict = {}

    if man_hour_supporter_list:
        # 集計月の支援者別工数情報が存在する場合

        #######################################
        # 課毎の有償案件の月稼働時間直接寄与の算出
        #######################################
        logger.info("課毎の有償案件の月稼働時間直接寄与の算出 - 処理開始")

        if not df_direct.empty:
            # 直接支援工数(1.対面支援)の入力がある場合

            # 所属課別の対面支援工数集計
            # 直接支援工数(1.対面支援)
            # e.g.
            #  pivot_table_direct_man_hour:
            #   supporter_organization_id             180a3597-b7e7-42c8-902c-a29016afa662  ...
            #   project_id                                                                  ...
            #   2854fc20-caea-44bb-ada5-d5ec3595f4c0                                 60.75  ...
            #   2bb85e8d-7560-48b0-82c7-d8ba134a9266                                 80.50  ...
            #   ca57d2cd-a898-4344-bdbd-9811fb26e19f                                100.00  ...
            pivot_table_direct_man_hour = pd.pivot_table(
                data=df_direct,
                index=["project_id"],
                columns="supporter_organization_id",
                values="input_man_hour",
                aggfunc="sum",
            ).fillna(0)

            # 案件情報から有償案件の案件IDを取得
            for_value_project_id_list = [
                x.id
                for x in project_model_list
                if x.contract_type == ContractType.FOR_VALUE
            ]
            # 課毎の有償案件の月稼働時間直接寄与
            # 集計を有償案件でフィルタ
            df_direct_man_hour_only_for_value = pivot_table_direct_man_hour[
                pivot_table_direct_man_hour.index.isin(for_value_project_id_list)
            ]
            for supporter_organization_id in pivot_table_direct_man_hour.columns:
                direct_contribution_to_monthly_manpower[
                    supporter_organization_id
                ] = float(
                    df_direct_man_hour_only_for_value[supporter_organization_id].sum()
                )

        #######################################
        # 課毎の月人員数、月総工数の算出
        #######################################
        logger.info("課毎の月人員数、月総工数の算出 - 処理開始")
        # 課毎の月人員数（その月に支援工数入力を行ったそのチーム（課）に所属する支援者の人数）
        # e.g.
        #  pivot_table_df_supporter_count:
        #                                        supporter_user_id
        #  supporter_organization_id
        #  180a3597-b7e7-42c8-902c-a29016afa662                  2
        #  de40733f-6be9-4fef-8229-01052f43c1e2                  3
        pivot_table_df_supporter_count = pd.pivot_table(
            data=df,
            index=["supporter_organization_id"],
            values="supporter_user_id",
            aggfunc="count",
        ).fillna(0)

        for supporter_organization_id in pivot_table_df_supporter_count.index:
            supporter_count_monthly[supporter_organization_id] = int(
                pivot_table_df_supporter_count.loc[
                    supporter_organization_id, "supporter_user_id"
                ]
            )

        # 月総工数（その月に支援工数入力を行ったその課に所属する支援者の工数総合計（1～4）※5は除く）
        # e.g.
        # pivot_table_df_all_man_hour_total:
        #                                       summary_man_hour.direct  ...  summary_man_hour.ssap
        # supporter_organization_id                                      ...
        # 180a3597-b7e7-42c8-902c-a29016afa662                      240  ...                     30
        # de40733f-6be9-4fef-8229-01052f43c1e2                      190  ...                     45
        pivot_table_df_all_man_hour_total = pd.pivot_table(
            data=df,
            index=["supporter_organization_id"],
            values=[
                "summary_man_hour.direct",
                "summary_man_hour.pre",
                "summary_man_hour.sales",
                "summary_man_hour.ssap",
            ],
            aggfunc="sum",
        )
        # 課毎の月総工数
        series_all_man_hour_total_sum = pivot_table_df_all_man_hour_total.sum(axis=1)
        # key: supporter_organization_id(所属課ID), value: 月総工数
        monthly_all_man_hour_total = {
            index: value for index, value in series_all_man_hour_total_sum.items()
        }

        ################################################################
        # 課毎の月稼働時間仕込含（時間）（無償案件の対応を含む）の算出
        ################################################################
        logger.info("課毎の月稼働時間仕込含（時間）（無償案件の対応を含む）の算出 - 処理開始")
        # 月稼働時間仕込含（時間）（無償案件の対応を含む）
        # その月に支援工数入力を行ったその課に所属する支援者の工数総合計（1～2）
        # e.g.
        # pivot_table_df_all_man_hour_total_direct_and_pre:
        #                                       summary_man_hour.direct  ...  summary_man_hour.pre
        # supporter_organization_id                                      ...
        # 180a3597-b7e7-42c8-902c-a29016afa662                      240  ...                     30
        # de40733f-6be9-4fef-8229-01052f43c1e2                      190  ...                     45
        pivot_table_df_all_man_hour_total_direct_and_pre = pd.pivot_table(
            data=df,
            index=["supporter_organization_id"],
            values=[
                "summary_man_hour.direct",
                "summary_man_hour.pre",
            ],
            aggfunc="sum",
        )
        # 課毎の月稼働時間仕込含
        series_man_hour_total_sum_direct_and_pre = (
            pivot_table_df_all_man_hour_total_direct_and_pre.sum(axis=1)
        )
        # key: supporter_organization_id(所属課ID), value: 月稼働時間仕込含
        monthly_man_hour_total_direct_and_pre = {
            index: value
            for index, value in series_man_hour_total_sum_direct_and_pre.items()
        }

    logger.debug(f"課別の平均粗利額: {gross_profit_summary_for_value}")
    logger.debug(f"課別の契約時間（有償分）合計: {contract_time_summary_for_value}")
    logger.debug(f"課ごとの契約時間単価: {contract_hourly_unit_price_for_value}")
    logger.debug(f"課毎の有償案件の月稼働時間直接寄与: {direct_contribution_to_monthly_manpower}")
    logger.debug(f"課毎の月人員数: {supporter_count_monthly}")
    logger.debug(f"課毎の月総工数: {monthly_all_man_hour_total}")
    logger.debug(f"課毎の月稼働時間仕込含: {monthly_man_hour_total_direct_and_pre}")

    #####################################
    # 支援者組織（課）別工数集計の作成
    #####################################
    supporter_organization_id_list = [
        str(x) for x in gross_profit_summary_for_value.keys()
    ]
    # 支援者組織（課）別工数集計の登録
    logger.info(
        f"集計月{summary_year_month}の支援者組織（課）件数: {len(supporter_organization_id_list)}件"
    )
    current_datetime = datetime.now()
    count = 0
    with ManHourSupporterOrganizationSummaryModel.batch_write() as supporter_organization_summary_batch:
        for org_id in supporter_organization_id_list:
            count += 1
            logger.info(
                f"year_month:{summary_year_month} - 支援者組織（課）別工数集計の登録 ({count}/{len(supporter_organization_id_list)})"
            )
            supporter_organization_name = ""
            for master_supporter_organization in master_supporter_organization_list:
                if master_supporter_organization.id == org_id:
                    supporter_organization_name = master_supporter_organization.value
                    break

            # 月商（百万円）
            monthly_sales = decimal_round(
                gross_profit_summary_for_value.get(org_id, 0) / 1000000
                if gross_profit_summary_for_value.get(org_id, 0)
                else 0,
                RoundSetting.DECIMAL_DIGITS,
            )
            # 有償案件の月契約時間（時間）
            monthly_contract_time = decimal_round(
                contract_time_summary_for_value.get(org_id, 0),
                RoundSetting.DECIMAL_DIGITS,
            )
            # 有償案件の月時間単価（万円/1時間）
            monthly_project_price = decimal_round(
                contract_hourly_unit_price_for_value.get(org_id, 0) / 10000
                if contract_hourly_unit_price_for_value.get(org_id, 0)
                else 0,
                RoundSetting.DECIMAL_DIGITS,
            )
            # 年商（億円）
            annual_sales = decimal_round(
                monthly_sales * 12 / 100 if monthly_sales else 0,
                RoundSetting.DECIMAL_DIGITS,
            )

            # 有償案件の月稼働時間直接寄与（時間）
            monthly_work_time = decimal_round(
                direct_contribution_to_monthly_manpower.get(org_id, 0),
                RoundSetting.DECIMAL_DIGITS,
            )
            # 月契約時間当たりの直接寄与工数の比率
            monthly_work_time_rate = decimal_round(
                monthly_work_time / monthly_contract_time
                if monthly_work_time and monthly_contract_time
                else 0,
                RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
            )
            # 月人員数（人）
            monthly_supporters = supporter_count_monthly.get(org_id, 0)
            # 月総工数（時間）
            monthly_man_hour = decimal_round(
                monthly_all_man_hour_total.get(org_id, 0), RoundSetting.DECIMAL_DIGITS
            )
            # 月稼働率直接寄与（有償案件分のみ）
            monthly_occupancy_rate = decimal_round(
                monthly_work_time / monthly_man_hour
                if monthly_work_time and monthly_man_hour
                else 0,
                RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
            )
            # 月稼働時間仕込含（時間）
            monthly_occupancy_total_time = decimal_round(
                monthly_man_hour_total_direct_and_pre.get(org_id, 0),
                RoundSetting.DECIMAL_DIGITS,
            )
            # 月稼働仕込含
            monthly_occupancy_total_rate = decimal_round(
                monthly_occupancy_total_time / monthly_man_hour
                if monthly_occupancy_total_time and monthly_man_hour
                else 0,
                RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
            )

            supporter_organization_summary = ManHourSupporterOrganizationSummaryModel(
                data_type=ManHourDataTypePrefix.SUPPORTER_ORGANIZATION_SUMMARY + org_id,
                year_month=summary_year_month,
                supporter_organization_id=org_id,
                supporter_organization_name=supporter_organization_name,
                annual_sales=annual_sales,
                monthly_sales=monthly_sales,
                monthly_project_price=monthly_project_price,
                monthly_contract_time=monthly_contract_time,
                monthly_work_time=monthly_work_time,
                monthly_work_time_rate=monthly_work_time_rate,
                monthly_supporters=monthly_supporters,
                monthly_man_hour=monthly_man_hour,
                monthly_occupancy_rate=monthly_occupancy_rate,
                monthly_occupancy_total_time=monthly_occupancy_total_time,
                monthly_occupancy_total_rate=monthly_occupancy_total_rate,
                create_at=current_datetime,
                update_at=current_datetime,
            )
            supporter_organization_summary_batch.save(supporter_organization_summary)


def init_procedure(event, operation_start_datetime: datetime, batch_control_id: str):
    """
        初期処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    # 入力パラメータのチェック
    mode = event.get("mode")
    if not mode or mode not in [
        BatchInputParameterModeForSummaryManHour.PROJECT,
        BatchInputParameterModeForSummaryManHour.SERVICE_TYPE,
        BatchInputParameterModeForSummaryManHour.SUPPORTER_ORGANIZATION,
    ]:
        raise Exception(f"Invalid parameter: {mode}")

    batch_init_common_procedure(
        event=event,
        operation_start_datetime=operation_start_datetime,
        batch_control_id=batch_control_id,
        batch_control_name=BatchFunctionName.SUMMARY_MAN_HOUR_BATCH,
    )

    global master_alert_setting_list, master_supporter_organization_list, master_service_type_list
    # 汎用マスタ:支援工数アラート設定
    master_alert_setting_list = list(
        MasterAlertSettingModel.data_type_index.query(
            hash_key=MasterDataType.ALERT_SETTING
        )
    )
    # 汎用マスタ:支援者組織情報
    master_supporter_organization_list = list(
        MasterSupporterOrganizationModel.data_type_index.query(
            hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION
        )
    )
    # 汎用マスタ:サービス種別
    master_service_type_list = list(
        MasterSupporterOrganizationModel.data_type_index.query(
            hash_key=MasterDataType.MASTER_SERVICE_TYPE
        )
    )


def end_procedure(event, batch_status: str, batch_control_id: str):
    """
        終了処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    batch_end_common_procedure(
        event=event, batch_status=batch_status, batch_control_id=batch_control_id
    )


def create_man_hour_total_by_project(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        プロジェクト別工数集計作成

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """

    ###########################
    # 前月分データ
    ###########################
    # 前月(yyyy/mm)
    last_month_datetime: datetime = operation_start_datetime + relativedelta(months=-1)
    last_year_month = last_month_datetime.strftime("%Y/%m")
    logger.info(f"前月データ作成開始:{last_year_month}")
    # プロジェクト別工数集計の編集・登録
    save_project_summary_of_last_or_current_month(last_year_month)
    logger.info("前月データ作成完了")

    ###########################
    # 当月分データ
    ###########################
    # 当月(yyyy/mm)
    current_year_month = operation_start_datetime.strftime("%Y/%m")
    logger.info(f"当月データ作成開始:{current_year_month}")
    # プロジェクト別工数集計の編集・登録
    save_project_summary_of_last_or_current_month(current_year_month)
    logger.info("当月データ作成完了")

    ####################################
    # 先日付分データ(翌月から6箇月分)
    ####################################
    # 翌月
    next_month_datetime: datetime = operation_start_datetime + relativedelta(months=+1)
    # 翌月から6箇月の年月
    datetime_index = pd.date_range(start=next_month_datetime, freq="M", periods=6)
    future_year_month_list = datetime_index.strftime("%Y/%m").tolist()

    for year_month in future_year_month_list:
        logger.info(f"先日付データ作成開始:{year_month}")
        # プロジェクト別工数集計の編集・登録
        save_project_summary_of_future(year_month)
        logger.info("先日付データ作成完了")


def create_man_hour_total_by_service_type(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        サービス種別別工数集計作成

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    ###########################
    # 前月分データ
    ###########################
    # 前月(yyyy/mm)
    last_month_datetime: datetime = operation_start_datetime + relativedelta(months=-1)
    last_year_month = last_month_datetime.strftime("%Y/%m")
    logger.info(f"前月データ作成開始:{last_year_month}")
    # サービス種別別工数集計の編集・登録
    save_service_type_summary_of_last_or_current_month(last_year_month)
    logger.info("前月データ作成完了")

    ###########################
    # 当月分データ
    ###########################
    # 当月(yyyy/mm)
    current_year_month = operation_start_datetime.strftime("%Y/%m")
    logger.info(f"当月データ作成開始:{current_year_month}")
    # サービス種別別工数集計の編集・登録
    save_service_type_summary_of_last_or_current_month(current_year_month)
    logger.info("当月データ作成完了")


def create_man_hour_total_by_supporter_organization(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        支援者組織（課）別工数集計作成

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    ###########################
    # 前月分データ
    ###########################
    # 前月(yyyy/mm)
    last_month_datetime: datetime = operation_start_datetime + relativedelta(months=-1)
    last_year_month = last_month_datetime.strftime("%Y/%m")
    logger.info(f"前月データ作成開始:{last_year_month}")
    # 支援者組織（課）別工数集計の編集・登録
    save_supporter_organization_summary_of_last_or_current_month(last_year_month)
    logger.info("前月データ作成完了")

    ###########################
    # 当月分データ
    ###########################
    # 当月(yyyy/mm)
    current_year_month = operation_start_datetime.strftime("%Y/%m")
    logger.info(f"当月データ作成開始:{current_year_month}")
    # 支援者組織（課）別工数集計の編集・登録
    save_supporter_organization_summary_of_last_or_current_month(current_year_month)
    logger.info("当月データ作成完了")


def main_procedure(event, operation_start_datetime: datetime, batch_control_id: str):
    """
        主処理

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    if event["mode"] == BatchInputParameterModeForSummaryManHour.PROJECT:
        # プロジェクト別工数集計作成
        logger.info("create_man_hour_total_by_project start.")
        create_man_hour_total_by_project(
            event, operation_start_datetime, batch_control_id
        )
        logger.info("create_man_hour_total_by_project end.")

    elif event["mode"] == BatchInputParameterModeForSummaryManHour.SERVICE_TYPE:
        # サービス別工数集計作成
        logger.info("create_man_hour_total_by_service_type start.")
        create_man_hour_total_by_service_type(
            event, operation_start_datetime, batch_control_id
        )
        logger.info("create_man_hour_total_by_service_type end.")

    elif (
        event["mode"] == BatchInputParameterModeForSummaryManHour.SUPPORTER_ORGANIZATION
    ):
        # 支援者組織（課）別工数集計作成
        logger.info("create_man_hour_total_by_supporter_organization start.")
        create_man_hour_total_by_supporter_organization(
            event, operation_start_datetime, batch_control_id
        )
        logger.info("create_man_hour_total_by_supporter_organization end.")


def handler(event, context):
    """
    バッチ処理 : BO工数情報集計データ作成処理

    Args:
        event:
        context:

    Returns:
        str: 実行結果("Normal end.","Skipped processing.","Error end.")
    """
    logger.debug(event)
    logger.debug(context)

    start_time = time.time()
    # バッチ関数名
    batch_control_id = (
        BatchFunctionId.SUMMARY_MAN_HOUR_BATCH.format(
            landscape=event["stageParams"]["stage"]
        )
        + "#"
        + event["mode"]
    )
    try:
        logger.info(f"Process start: mode: {event.get('mode')}")
        # 起動日時
        operation_start_datetime: datetime = get_operation_datetime()
        logger.info(f"operation_start_datetime: {operation_start_datetime}")

        # 初期処理
        init_procedure(event, operation_start_datetime, batch_control_id)

        # 主処理
        main_procedure(event, operation_start_datetime, batch_control_id)

        # 終了処理
        end_procedure(event, BatchStatus.EXECUTED, batch_control_id)

        process_time = round(time.time() - start_time, 4)
        logger.info("process_time: {0}[sec]".format(str(process_time)))
        logger.info(f"Process normal end: mode: {event['mode']}")
        return "Normal end."

    except ExitHandler:
        # 処理の途中で処理終了する場合
        process_time = round(time.time() - start_time, 4)
        logger.info("process_time: {0}[sec]".format(str(process_time)))
        logger.info(f"Skipped Processing: mode: {event['mode']}")
        return "Skipped processing."
    except Exception:
        error_mail_to = os.getenv("ERROR_MAIL_TO")
        if error_mail_to:
            # 環境変数ERROR_MAIL_TOに宛先が設定されている場合、エラーメール送信
            send_mail(
                template=MailType.BATCH_ERROR_MAIL,
                to_addr_list=[error_mail_to],
                cc_addr_list=[],
                payload={
                    "error_datetime": datetime.now().strftime("%Y/%m/%d %H:%M"),
                    "error_function": BatchFunctionName.SUMMARY_MAN_HOUR_BATCH,
                    "error_message": traceback.format_exc(),
                },
            )
        end_procedure(event, BatchStatus.ERROR, batch_control_id)
        process_time = round(time.time() - start_time, 4)
        logger.info("process_time: {0}[sec]".format(str(process_time)))
        logger.exception("Error end.")
        return "Error end."


if __name__ == "__main__":
    param = {"mode": sys.argv[1], "stageParams": {"stage": sys.argv[2]}}
    handler(param, {})
