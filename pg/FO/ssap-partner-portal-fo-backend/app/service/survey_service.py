import copy
from datetime import datetime
from typing import List

import pandas as pd
from fastapi import HTTPException, status
from pynamodb.exceptions import DoesNotExist
from pynamodb.expressions.condition import size as condition_size
from pynamodb.expressions.update import Action

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.master import (
    MasterSupporterOrganizationModel,
)
from app.models.project import ProjectModel
from app.models.project_survey import (
    AnswersAttribute,
    PointsAttribute,
    ProjectSurveyModel,
)
from app.models.survey_master import SurveyMasterModel
from app.models.survey_summary import (
    SurveySummarySupporterOrganizationModel,
    SurveySummaryUserModel,
)
from app.models.user import UserModel
from app.resources.const import (
    BoAppUrl,
    DataType,
    DateTime,
    DateTimeHourMinutes,
    FoAppUrl,
    GetSurveysByMineSortType,
    HourMinutes,
    MailType,
    MasterDataType,
    NotificationType,
    SurveyQuestionsSummaryType,
    SurveyType,
    SurveyTypeForGetSurveys,
    SurveyTypeName,
    UserRoleType,
)
from app.schemas.base import OKResponse
from app.schemas.survey import (
    Answer,
    CheckAndGetSurveyAnonymousByIdRequest,
    CheckAndGetSurveyAnonymousByIdResponse,
    CheckAndUpdateSurveyAnonymousByIdRequest,
    Completion,
    CompletionContinuation,
    GetSurveyByIdResponse,
    GetSurveyOfSatisfactionByIdRequest,
    GetSurveyOfSatisfactionByIdResponse,
    GetSurveysByMine,
    GetSurveysByMineQuery,
    GetSurveysByMineResponse,
    GetSurveysQuery,
    GetSurveysResponse,
    GetSurveySummaryByMineQuery,
    GetSurveySummaryByMineResponse,
    GetSurveySummarySupporterOrganizationByMineQuery,
    GetSurveySummarySupporterOrganizationByMineResponse,
    MonthlyResultsBySupporterOrganization,
    PostCheckSurveyByIdPasswordRequest,
    PostCheckSurveyByIdPasswordResponse,
    PostCheckSurveyByIdRequest,
    PostCheckSurveyByIdResponse,
    Quick,
    Service,
    ServiceAndCompletion,
    SummaryInfoForGetSurveysResponse,
    SupporterUser,
    SurveyInfoForGetSurveysResponse,
    Surveys,
    SurveysForMonthlyResultsBySupporterOrganization,
    TermSummaryResult,
    TermSummaryResultForSupporterOrganizations,
    UpdateSurveyByIdQuery,
    UpdateSurveyByIdRequest,
    UpdateSurveyByIdResponse,
    UpdateSurveyOfSatisfactionByIdRequest,
    monthlySurveySummaryForGetSurveysResponse,
    surveySummaryForGetSurveysResponse,
    surveySummaryHalfTotalForGetSurveysResponse,
    surveySummaryQuotaTotalForGetSurveysResponse,
    surveySummaryTotalForGetSurveysResponse,
    totalSummaryResultForSupporterOrganizations,
)
from app.service.notification_service import NotificationService
from app.service.project_service import ProjectService
from app.utils.cipher_aes import AesCipherUtils
from app.utils.date import get_datetime_now
from app.utils.encryption import decode_jwt
from app.utils.format import round_off

logger = CustomLogger.get_logger()


class SurveyService:
    @staticmethod
    def is_visible_survey_for_get_surveys(
        user: UserModel,
        survey: ProjectSurveyModel,
    ) -> bool:
        """案件アンケート情報がアクセス可能か判定.
            1.お客様
              自身が参加してる案件のアンケート
            2.支援者
              担当案件の顧客開示OKされたアンケート
            3.営業担当
              担当案件
              その他公開案件
            4.支援者責任者
              担当案件
              所属課案件
              その他公開案件
            5.営業責任者
              全案件
            6.事業者責任者
              全案件
        Args:
            user (UserModel): ユーザ情報
            survey (ProjectSurveyModel): アンケート情報
            project_list (List[ProjectModel]): 案件情報
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(
            hash_key=survey.project_id, range_key=DataType.PROJECT
        )
        if (
            user.role == UserRoleType.SALES_MGR.key
            or user.role == UserRoleType.BUSINESS_MGR.key
        ):
            # 全案件
            return True
        elif user.role == UserRoleType.SUPPORTER.key:
            # 担当案件かつ支援者への開示OK
            if (
                (
                    project.main_supporter_user_id
                    and project.main_supporter_user_id == user.id
                )
                or (
                    project.supporter_user_ids
                    and user.id
                    in [
                        supporter_user_id
                        for supporter_user_id in project.supporter_user_ids
                    ]
                )
            ) and survey.is_disclosure:
                return True
        elif user.role == UserRoleType.SALES.key:
            # 担当案件／その他公開案件
            if project.main_sales_user_id == user.id:
                return True
            elif not project.is_secret:
                return True
        elif user.role == UserRoleType.SUPPORTER_MGR.key:
            # 担当案件かつ支援者への開示OK／所属課案件／その他公開案件
            if (
                project.main_supporter_user_id
                and project.main_supporter_user_id == user.id
            ) or (
                project.supporter_user_ids
                and user.id
                in [
                    supporter_user_id
                    for supporter_user_id in project.supporter_user_ids
                ]
            ):
                # 支援者（担当者）としての権限
                if survey.is_disclosure:
                    return True
            else:
                # 支援者責任者としての権限
                if (
                    user.supporter_organization_id
                    and project.supporter_organization_id
                    in user.supporter_organization_id
                ):
                    return True
                elif not project.is_secret:
                    return True

        elif user.role == UserRoleType.CUSTOMER.key:
            # 自身が参加してる案件のアンケート
            if project.main_customer_user_id == user.id:
                return True

        return False

    @staticmethod
    def is_visible_survey_for_get_survey_by_id(
        user: UserModel,
        survey: ProjectSurveyModel,
        project: ProjectModel,
    ) -> bool:
        """案件アンケート情報がアクセス可能か判定.
            1. 営業責任者
                制限なし（アクセス可）
            2. 営業担当者
                担当案件／その他公開案件（アクセス可）
            3. 支援者責任者
                担当案件かつ支援者への開示OK／所属課案件／その他公開案件（アクセス可）
            3. 支援者
                担当案件かつ支援者への開示OK : アクセス可
            4. 顧客
                自身の情報：アクセス可
            6.事業者責任者
                制限なし（アクセス可）
        Args:
            user (UserModel): ユーザー
            survey (ProjectSurveyModel): アンケート情報
            project (ProjectModel): 案件情報
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """

        if (
            user.role == UserRoleType.SALES_MGR.key
            or user.role == UserRoleType.BUSINESS_MGR.key
        ):
            return True

        elif user.role == UserRoleType.SALES.key:
            # 担当案件／その他公開案件
            if project.main_sales_user_id == user.id:
                return True
            elif not project.is_secret:
                return True

        elif user.role == UserRoleType.SUPPORTER_MGR.key:
            # 担当案件かつ支援者への開示OK／所属課案件／その他公開案件
            if (
                project.main_supporter_user_id
                and project.main_supporter_user_id == user.id
            ) or (
                project.supporter_user_ids
                and user.id
                in [
                    supporter_user_id
                    for supporter_user_id in project.supporter_user_ids
                ]
            ):
                if survey.is_disclosure:
                    return True
            else:
                if (
                    user.supporter_organization_id
                    and project.supporter_organization_id
                    in user.supporter_organization_id
                ):
                    # 所属課案件
                    return True
                elif not project.is_secret:
                    # その他公開案件
                    return True
        elif user.role == UserRoleType.SUPPORTER.key:
            # 担当案件かつ支援者への開示OK
            if (
                project.main_supporter_user_id
                and project.main_supporter_user_id == user.id
            ) or (
                project.supporter_user_ids
                and user.id
                in [
                    supporter_user_id
                    for supporter_user_id in project.supporter_user_ids
                ]
            ):
                # 担当案件かつ支援者への開示OK
                if survey.is_disclosure:
                    return True

        elif user.role == UserRoleType.CUSTOMER.key:
            # 自身の情報はアクセス可
            if project.main_customer_user_id == user.id:
                return True

        return False

    @staticmethod
    def is_visible_survey_for_update_survey_by_id(
        user_id: str,
        role: str,
        survey: ProjectSurveyModel,
    ) -> bool:
        """案件アンケート情報がアクセス可能か判定.
            1.支援者、支援者責任者
              PPアンケート：アクセス可
              上記以外：アクセス不可
            2.営業担当者、営業責任者
              所属案件：アクセス可
              PPアンケート：アクセス可
              上記以外：アクセス不可
            3.事業者責任者
              所属案件：アクセス可
              PPアンケート：アクセス可
            4.顧客
              自身の情報：アクセス可
              上記以外：アクセス不可
        Args:
            user_id (str): ユーザID
            role (str): ロール
            survey (ProjectSurveyModel): アンケート情報
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(
            hash_key=survey.project_id, range_key=DataType.PROJECT
        )
        if role in [
            UserRoleType.SUPPORTER.key,
            UserRoleType.SUPPORTER_MGR.key,
        ]:
            if survey.survey_type == SurveyType.PP:
                # PPアンケート：アクセス可
                return True

        elif role in [
            UserRoleType.SALES.key,
            UserRoleType.SALES_MGR.key,
        ]:
            if project.main_sales_user_id == user_id:
                # 所属案件：アクセス可
                return True
            if survey.survey_type == SurveyType.PP:
                # PPアンケート：アクセス可
                return True

        elif role == UserRoleType.BUSINESS_MGR.key:
            if project.main_supporter_user_id == user_id:
                # プロデューサー担当案件：アクセス可
                return True
            if user_id in project.supporter_user_ids:
                # アクセレーター担当案件：アクセス可
                return True
            if survey.survey_type == SurveyType.PP:
                # PPアンケート：アクセス可
                return True

        elif role == UserRoleType.CUSTOMER.key:
            if project.main_customer_user_id == user_id:
                # 自身の情報はアクセス可
                return True

        return False

    @staticmethod
    def is_visible_survey_by_supporter_mgr_for_survey_answer_provided(
        user: UserModel,
        survey: ProjectSurveyModel,
        project: ProjectModel,
    ) -> bool:
        """支援者責任者が案件アンケート情報にアクセス可能か判定(アンケート回答通知用).
            判定1:
                所属していない非公開案件：アクセス不可
                上記以外：アクセス可
            判定2:
                案件にアサインされている支援者の組織IDに含まれていない支援者責任者はアクセス不可
        Args:
            user (UserModel) : 支援者責任者
            survey (ProjectSurveyModel)
            project (ProjectModel)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        if (
            not project.main_supporter_user_id
            or project.main_supporter_user_id != user.id
        ) and (
            not project.supporter_user_ids
            or user.id
            not in [
                supporter_user_id for supporter_user_id in project.supporter_user_ids
            ]
        ):
            # 所属していない非公開案件はアクセス不可
            if project and project.is_secret:
                return False

        # プロデューサー、アクセラレータの所属課を取得
        supporter_organization_id_list = []
        if project.main_supporter_user_id:
            main_supporter_user_info = UserModel.get(
                hash_key=project.main_supporter_user_id, range_key=DataType.USER
            )
            supporter_organization_id_list.extend(
                main_supporter_user_info.supporter_organization_id
            )

        if project.supporter_user_ids:
            for supporter_user in project.supporter_user_ids:
                supporter_user_info = UserModel.get(
                    hash_key=supporter_user, range_key=DataType.USER
                )
                supporter_organization_id_list.extend(
                    supporter_user_info.supporter_organization_id
                )

        # 支援者責任者の所属課がプロデューサー、アクセラレータの所属課と異なる場合はアクセス不可
        include_flag = False
        if user.supporter_organization_id:
            for org_id in user.supporter_organization_id:
                if org_id in supporter_organization_id_list:
                    include_flag = True
                    break

        if not include_flag:
            return False

        return True

    @staticmethod
    def calc_satisfaction_average_and_receive(temp_series: pd.Series) -> tuple:
        """
            対象年月の顧客満足度とその未回答数の集計情報を基に顧客満足度平均とN数を算出

        Args:
            temp_series (pd.Series): pivot_tableから取得した1行の情報 (対象年月の情報)
        Returns:
            tuple 以下情報のタプル
                - 顧客満足度平均
                - N数
        """
        logger.info("start calc_satisfaction_average_and_receive method")

        satisfaction_sum = temp_series.loc[("sum", "points_satisfaction")].item()
        receive = (
            temp_series.loc[("count", "points_satisfaction")].item()
            - temp_series.loc[("sum", "unanswered_satisfaction")].item()
        )
        satisfaction_average = round_off(
            SurveyService.divide(satisfaction_sum, receive), 0.1
        )

        return satisfaction_average, receive

    @staticmethod
    def edit_summary_of_get_surveys_response(
        target_list: List[ProjectSurveyModel], aggregation_year: str
    ) -> SummaryInfoForGetSurveysResponse:
        """GetSurveysレスポンスのsummary項目を編集

        Args:
            target_list (List[ProjectSurveyModel]): 集計の元データ
            aggregation_year (str): 集計年度

        Returns:
            SummaryInfoForGetSurveysResponse: GetSurveysレスポンスのsummary項目
        """
        logger.info(f"サマリー項目の集計年度:{aggregation_year}")

        # 集計年度の年月(指定年の4月～翌年3月)
        datetime_index = pd.date_range(
            start=datetime(year=int(aggregation_year), month=4, day=1),
            freq="M",
            periods=12,
        )
        summary_month_list: List[str] = datetime_index.strftime("%Y/%m").tolist()
        # 四半期末の年月
        quota_month_list: List[str] = [
            summary_month_list[2],
            summary_month_list[5],
            summary_month_list[8],
            summary_month_list[11],
        ]
        # 半期末の年月
        half_month_list: List[str] = [summary_month_list[5], summary_month_list[11]]
        # 年度末の年月
        year_last_month: str = summary_month_list[11]

        logger.debug(f"summary_month_list:{summary_month_list}")
        logger.debug(f"quota_month_list:{quota_month_list}")
        logger.debug(f"half_month_list:{half_month_list}")
        logger.debug(f"year_last_month:{year_last_month}")

        # DataFrameに取り込む情報を編集
        df_index: list = []
        target_dict_list = []
        for target in target_list:
            # 集計対象年度以外のデータはスキップ
            if target.summary_month not in summary_month_list:
                continue
            # アンケート情報の集計対象外フラグがTrueの場合はスキップ
            if target.is_not_summary:
                continue

            df_index.append(target.id)
            unanswered_satisfaction_count: int = 0
            if target.points.unanswered and "satisfaction" in target.points.unanswered:
                unanswered_satisfaction_count = 1

            target_dict = {
                "summary_month": target.summary_month,
                "points_satisfaction": target.points.satisfaction,
                "unanswered_satisfaction": unanswered_satisfaction_count,
            }
            target_dict_list.append(target_dict)

        # 対象データが None または 空 の場合
        if not target_dict_list:
            monthly_zero_list: List[monthlySurveySummaryForGetSurveysResponse] = []
            accumulation_zero_list: List[monthlySurveySummaryForGetSurveysResponse] = []
            for year_month in summary_month_list:
                year_month_split_str = year_month.split("/")
                year_str = year_month_split_str[0]
                month_str = year_month_split_str[1].lstrip("0")
                monthly_zero_info = monthlySurveySummaryForGetSurveysResponse(
                    month=year_str + "年" + month_str + "月",
                    satisfaction_average=0,
                    receive=0,
                )
                monthly_zero_list.append(monthly_zero_info)
                accumulation_zero_list.append(monthly_zero_info)

            total_child_zero_info = surveySummaryForGetSurveysResponse(
                satisfaction_average=0, receive=0
            )
            total_zero_info = surveySummaryTotalForGetSurveysResponse(
                quota=surveySummaryQuotaTotalForGetSurveysResponse(
                    quota1=total_child_zero_info,
                    quota2=total_child_zero_info,
                    quota3=total_child_zero_info,
                    quota4=total_child_zero_info,
                ),
                half=surveySummaryHalfTotalForGetSurveysResponse(
                    half1=total_child_zero_info, half2=total_child_zero_info
                ),
                year=total_child_zero_info,
            )

            # 項目は作成して返却（ゼロで作成）
            return SummaryInfoForGetSurveysResponse(
                monthly=monthly_zero_list,
                accumulation=accumulation_zero_list,
                total=total_zero_info,
            )

        # DataFrameのイメージ
        # df:                                  summary_month  points_satisfaction  unanswered_satisfaction
        # 995-e2eb-4712-9825-2bf3e97d7e79            2022/09                    0                        0
        # 98c3a5ed-d517-147a-7282-50b88abf0000       2022/09                    5                        0
        # 333-e2eb-4712-9825-2bf3e97d7e79            2022/09                    0                        0
        df = pd.DataFrame(data=target_dict_list, index=df_index)
        logger.debug(f"df.head(10):{df.head(10)}")
        logger.debug(f"df.shape:{df.shape}")

        # month(単月)の編集
        # pivot_tableのイメージ
        # pivot_table_monthly:
        #                               sum                                       count
        #               points_satisfaction unanswered_satisfaction points_satisfaction unanswered_satisfaction
        # summary_month
        # 2022/04                         0                       0                   0                       0
        # 2022/05                         0                       0                   0                       0
        # 2022/06                        37                       1                  10                      10
        # 2022/07                        37                       1                  10                      10
        # 2022/08                        37                       1                  11                      11
        # 2022/09                        12                       0                  10                      10
        # 2022/10                         0                       0                   0                       0
        # 2022/11                         0                       0                   0                       0
        # 2022/12                         0                       0                   0                       0
        # 2023/01                         0                       0                   0                       0
        # 2023/02                         0                       0                   0                       0
        # 2023/03                         0                       0                   0                       0
        pivot_table_monthly = pd.pivot_table(
            data=df,
            index=["summary_month"],
            aggfunc=["sum", "count"],
        ).fillna(0)

        # 対象年度でデータが存在しない月は、値0で追加
        for year_month in summary_month_list:
            if year_month not in pivot_table_monthly.index:
                pivot_table_monthly.loc[year_month] = 0
        # summary_monthで昇順ソート
        pivot_table_monthly = pivot_table_monthly.sort_index()
        logger.debug(f"pivot_table_monthly:{pivot_table_monthly}")

        monthly_list: List[monthlySurveySummaryForGetSurveysResponse] = []
        for year_month in summary_month_list:
            temp_monthly: pd.Series = pivot_table_monthly.loc[year_month, :]
            (
                satisfaction_average,
                receive,
            ) = SurveyService.calc_satisfaction_average_and_receive(temp_monthly)
            year_month_split_str = year_month.split("/")
            year_str = year_month_split_str[0]
            month_str = year_month_split_str[1].lstrip("0")
            monthly_info = monthlySurveySummaryForGetSurveysResponse(
                month=year_str + "年" + month_str + "月",
                satisfaction_average=satisfaction_average,
                receive=receive,
            )
            monthly_list.append(monthly_info)

        # accumulation(累積)の編集
        # pivot_tableのイメージ
        # pivot_table_accumulation:
        #                               sum                                       count
        #               points_satisfaction unanswered_satisfaction points_satisfaction unanswered_satisfaction
        # summary_month
        # 2022/04                         0                       0                   0                       0
        # 2022/05                         0                       0                   0                       0
        # 2022/06                        37                       1                  10                      10
        # 2022/07                        74                       2                  20                      20
        # 2022/08                       111                       3                  31                      31
        # 2022/09                       123                       3                  41                      41
        # 2022/10                       123                       3                  41                      41
        # 2022/11                       123                       3                  41                      41
        # 2022/12                       123                       3                  41                      41
        # 2023/01                       123                       3                  41                      41
        # 2023/02                       123                       3                  41                      41
        # 2023/03                       123                       3                  41                      41
        pivot_table_accumulation = pivot_table_monthly.copy()
        # 累積和の算出
        pivot_table_accumulation = pivot_table_accumulation.cumsum(axis=0)
        logger.debug(f"pivot_table_accumulation:{pivot_table_accumulation}")

        accumulation_list: List[monthlySurveySummaryForGetSurveysResponse] = []
        for year_month in summary_month_list:
            temp_accumulation: pd.Series = pivot_table_accumulation.loc[year_month, :]
            (
                satisfaction_average,
                receive,
            ) = SurveyService.calc_satisfaction_average_and_receive(temp_accumulation)
            year_month_split_str = year_month.split("/")
            year_str = year_month_split_str[0]
            month_str = year_month_split_str[1].lstrip("0")
            accumulation_info = monthlySurveySummaryForGetSurveysResponse(
                month=year_str + "年" + month_str + "月",
                satisfaction_average=satisfaction_average,
                receive=receive,
            )
            accumulation_list.append(accumulation_info)

        # total(期間合計)の編集
        # 四半期
        quota_calc_list: List = []
        # 四半期末の累積データを基に算出
        for year_month in quota_month_list:
            temp_total_quota: pd.Series = pivot_table_accumulation.loc[year_month, :]
            (
                satisfaction_average,
                receive,
            ) = SurveyService.calc_satisfaction_average_and_receive(temp_total_quota)
            quota_calc_list.append(
                surveySummaryForGetSurveysResponse(
                    satisfaction_average=satisfaction_average,
                    receive=receive,
                )
            )

        quota_info = surveySummaryQuotaTotalForGetSurveysResponse(
            quota1=quota_calc_list[0],
            quota2=quota_calc_list[1],
            quota3=quota_calc_list[2],
            quota4=quota_calc_list[3],
        )

        # 半期
        half_calc_list: List = []
        # 半期末の累積データを基に算出
        for year_month in half_month_list:
            temp_total_half: pd.Series = pivot_table_accumulation.loc[year_month, :]
            (
                satisfaction_average,
                receive,
            ) = SurveyService.calc_satisfaction_average_and_receive(temp_total_half)
            half_calc_list.append(
                surveySummaryForGetSurveysResponse(
                    satisfaction_average=satisfaction_average,
                    receive=receive,
                )
            )

        half_info = surveySummaryHalfTotalForGetSurveysResponse(
            half1=half_calc_list[0],
            half2=half_calc_list[1],
        )

        # 期
        # 年度末の累積データを基に算出
        temp_total_year: pd.Series = pivot_table_accumulation.loc[year_last_month, :]
        (
            satisfaction_average,
            receive,
        ) = SurveyService.calc_satisfaction_average_and_receive(temp_total_year)
        year_info: surveySummaryForGetSurveysResponse = (
            surveySummaryForGetSurveysResponse(
                satisfaction_average=satisfaction_average,
                receive=receive,
            )
        )

        total_info = surveySummaryTotalForGetSurveysResponse(
            quota=quota_info, half=half_info, year=year_info
        )

        return SummaryInfoForGetSurveysResponse(
            monthly=monthly_list, accumulation=accumulation_list, total=total_info
        )

    @staticmethod
    def get_survey_by_mine(
        query_params: GetSurveysByMineQuery, current_user: UserModel
    ) -> GetSurveysByMineResponse:
        """自身のアンケート情報を取得

        Args:
            query_params (GetSurveysByMineQuery): クエリパラメータ
            current_user (UserModel): API呼出ユーザー

        Returns:
            GetSurveysByMineResponse: 自身のアンケートリスト
        """
        ##########################
        # クエリ条件組み立て
        ##########################

        logger.info("start creating filter condition")

        if query_params.date_from:
            str_date_from = (
                datetime.strptime(str(query_params.date_from), "%Y%m%d").strftime(
                    "%Y/%m/%d"
                )
                + " "
                + HourMinutes.MINIMUM
            )
        else:
            str_date_from = DateTimeHourMinutes.MINIMUM

        if query_params.date_to:
            str_date_to = (
                datetime.strptime(str(query_params.date_to), "%Y%m%d").strftime(
                    "%Y/%m/%d"
                )
                + " "
                + HourMinutes.MAXIMUM
            )
        else:
            str_date_to = DateTimeHourMinutes.MAXIMUM

        filter_condition = None
        if query_params.type:
            filter_condition &= ProjectSurveyModel.survey_type == query_params.type

        scan_index_forward = (
            query_params.sort
            == GetSurveysByMineSortType.ACTUAL_SURVEY_REQUEST_DATETIME_DESC
        )

        ##############################
        # レスポンス組み立て
        ##############################

        logger.info("start creating API response")

        surveys_list: List[GetSurveysByMine] = []

        if query_params.project_id:
            if query_params.project_id in current_user.project_ids:
                filter_condition &= ProjectSurveyModel.answer_user_id == current_user.id
                surveys = ProjectSurveyModel.project_id_actual_survey_request_datetime_index.query(
                    hash_key=query_params.project_id,
                    range_key_condition=ProjectSurveyModel.actual_survey_request_datetime.between(
                        str_date_from, str_date_to
                    ),
                    filter_condition=filter_condition,
                    scan_index_forward=scan_index_forward,
                    limit=query_params.limit,
                )

                for survey in surveys:
                    surveys_list.append(GetSurveysByMine(**survey.attribute_values))
        elif query_params.type == SurveyType.PP:
            # 自身のPPアンケートを取得（顧客以外）
            if current_user.role != UserRoleType.CUSTOMER.key:
                pp_surveys = ProjectSurveyModel.survey_type_summary_month_index.query(
                    hash_key=SurveyType.PP,
                    filter_condition=(
                        ProjectSurveyModel.actual_survey_request_datetime.between(
                            str_date_from, str_date_to
                        )
                        & (ProjectSurveyModel.answer_user_id == current_user.id)
                    ),
                )
                for pp_survey in pp_surveys:
                    surveys_list.append(GetSurveysByMine(**pp_survey.attribute_values))
        else:
            filter_condition &= ProjectSurveyModel.answer_user_id == current_user.id
            surveys = ProjectSurveyModel.data_type_actual_survey_request_datetime_index.query(
                hash_key=DataType.SURVEY,
                range_key_condition=ProjectSurveyModel.actual_survey_request_datetime.between(
                    str_date_from, str_date_to
                ),
                filter_condition=filter_condition,
            )
            for survey in surveys:
                try:
                    project = ProjectModel.get(
                        hash_key=survey.project_id, range_key=DataType.PROJECT
                    )
                except DoesNotExist:
                    continue
                if project.main_customer_user_id == current_user.id:
                    # 最新の案件情報のお客様代表者に自身が設定されている場合アクセス可能
                    surveys_list.append(GetSurveysByMine(**survey.attribute_values))

        # 第1ソート: 未回答（昇順）, 第2ソート: 回答依頼日（降順）または回答期限（降順）
        if (
            query_params.sort
            == GetSurveysByMineSortType.ACTUAL_SURVEY_REQUEST_DATETIME_DESC
        ):
            surveys_list.sort(
                key=lambda x: x.actual_survey_request_datetime, reverse=True
            )
        elif (
            query_params.sort
            == GetSurveysByMineSortType.PLAN_SURVEY_RESPONSE_DATETIME_DESC
        ):
            surveys_list.sort(
                key=lambda x: (
                    x.plan_survey_response_datetime is not None,
                    x.plan_survey_response_datetime,
                ),
                reverse=True,
            )
        surveys_list.sort(
            key=lambda x: x.is_finished,
        )

        # 全案件分でソートした後に指定件数分取得
        if query_params.limit:
            surveys_list = surveys_list[: query_params.limit]

        return GetSurveysByMineResponse(total=len(surveys_list), surveys=surveys_list)

    @staticmethod
    def post_check_survey_by_id_password(
        item: PostCheckSurveyByIdPasswordRequest,
    ) -> PostCheckSurveyByIdPasswordResponse:
        """パスワード、及び案件IDと有効期限から生成されたJWTをもとに認証する。

        Args:
            body (PostCheckSurveyByIdPasswordRequest): チェックする内容
        Raise:

        Returns:
            PostCheckSurveyByIdPasswordResponse: 認証結果
        """

        # token(JWT)をデコード
        try:
            decode_str = decode_jwt(item.token)

            # 有効期限検証
            exp_time = float(decode_str["exp"])
            if exp_time < get_datetime_now().timestamp():
                logger.warning(
                    "post_check_survey_by_id_password password_date forbidden."
                )
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )

            # 案件アンケートアンケートidをもとに、案件とパスワードを取得
            survey_id = decode_str["survey_id"]
            try:
                survey = ProjectSurveyModel.get(
                    hash_key=survey_id, range_key=DataType.SURVEY
                )
            except DoesNotExist:
                logger.warning(
                    f"post_check_survey_by_id_password survey not found. survey_id: {survey_id}"
                )
                raise HTTPException(
                    status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
                )
        except Exception:
            logger.warning(
                f"post_check_survey_by_id_password . item.token: {item.token}"
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST, detail="Bad Request"
            )

        try:
            project = ProjectModel.get(
                hash_key=survey.project_id, range_key=DataType.PROJECT
            )
        except DoesNotExist:
            logger.warning(
                f"post_check_survey_by_id_password survey.project_id not found. survey.project_id:{survey.project_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 取得したパスワードの暗号化のデコード
        if project.survey_password is not None or len(project.survey_password) > 0:
            decrypt_password = AesCipherUtils.decrypt(project.survey_password)
        else:
            logger.warning(
                f"post_check_survey_by_id_password forbidden. project_id:{survey.project_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # パスワードをチェックする
        if decrypt_password != item.password:
            logger.warning(
                f"post_check_survey_by_id_password password forbidden. password:{item.password}"
            )
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        else:
            # アンケートIDをレスポンス
            return PostCheckSurveyByIdPasswordResponse(id=survey_id)

    @staticmethod
    def post_check_survey_by_id(
        item: PostCheckSurveyByIdRequest,
    ) -> PostCheckSurveyByIdResponse:
        """案件IDと有効期限から生成されたJWTをもとに認証する。

        Args:
            body (PostCheckSurveyByIdRequest): チェックする内容
        Raise:

        Returns:
            PostCheckSurveyByIdResponse: 認証結果
        """

        # token(JWT)をデコード
        try:
            decode_str = decode_jwt(item.token)

            # 有効期限検証
            exp_time = float(decode_str["exp"])
            if exp_time < get_datetime_now().timestamp():
                logger.warning("post_check_survey_by_id password_date forbidden.")
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )

            survey_id = decode_str["survey_id"]
            return PostCheckSurveyByIdResponse(id=survey_id)

        except Exception:
            logger.warning(f"post_check_survey_by_id . item.token: {item.token}")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST, detail="Bad Request"
            )

    @staticmethod
    def get_survey_by_id(survey_id: str, current_user: UserModel):
        """案件アンケートIDを指定して案件アンケート情報を取得する

        Args:
            survey_id (str): 案件アンケートID
            current_user (Behavior, optional): 認証済みのユーザー
        Raises:
            HTTPException: 400 Bad request
            HTTPException: 404 Not found

        Returns:
            GetSurveyByIdResponse: 取得結果
        """
        try:
            survey = ProjectSurveyModel.get(
                hash_key=survey_id, range_key=DataType.SURVEY
            )
        except DoesNotExist:
            logger.warning(f"GetSurveyById survey not found. survey_id: {survey_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if survey.survey_type == SurveyType.PP:
            # PPアンケートの場合
            # 顧客はアクセス不可
            # 自身以外のアンケートはアクセス不可
            if (
                current_user.role == UserRoleType.CUSTOMER.key
                or current_user.id != survey.answer_user_id
            ):
                logger.warning(
                    f"GetSurveyById forbidden. survey.project_id: {survey.project_id}"
                )
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )
        else:
            # サービス・修了・クイックアンケートの場合
            try:
                project = ProjectModel.get(
                    hash_key=survey.project_id, range_key=DataType.PROJECT
                )
            except DoesNotExist:
                logger.warning(
                    f"GetSurveyById survey not found. project_id: {survey.project_id}"
                )
                raise HTTPException(
                    status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
                )

            # 権限チェック
            if not SurveyService.is_visible_survey_for_get_survey_by_id(
                user=current_user,
                survey=survey,
                project=project,
            ):
                logger.warning(
                    f"GetSurveyById forbidden. survey.project_id: {survey.project_id}"
                )
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )

        # バリデーションチェック
        # 回答依頼送信済でないアンケートは取得不可
        if not survey.actual_survey_request_datetime:
            logger.warning(
                "GetSurveyById bad request. Cannot get a survey for which the response request has not been sent."
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Cannot get a survey for which the response request has not been sent.",
            )
        # 営業担当者は所属していない案件の未回答アンケートの取得不可（PPアンケート除く）
        if current_user.role == UserRoleType.SALES.key:
            if survey.survey_type != SurveyType.PP:
                if (
                    survey.project_id not in current_user.project_ids
                    and not survey.is_finished
                ):
                    logger.warning(
                        "GetSurveyById cannot get the survey.Sales users cannot get unanswered surveys for unassigned projects."
                    )
                    raise HTTPException(
                        status_code=status.HTTP_403_FORBIDDEN,
                        detail="Sales users cannot get unanswered surveys for unassigned projects.",
                    )

        # レスポンス編集
        response_sales_user_name = ""
        if survey.sales_user_id:
            try:
                user: UserModel = UserModel.get(
                    hash_key=survey.sales_user_id, range_key=DataType.USER
                )
                response_sales_user_name = user.name
            except DoesNotExist:
                pass
        # レスポンスとmodelで定義が異なる項目を編集
        response_main_supporter_user: SupporterUser = None
        if survey.main_supporter_user:
            response_main_supporter_user = SupporterUser(
                id=survey.main_supporter_user.id,
                name=survey.main_supporter_user.name,
                organization_id=survey.main_supporter_user.organization_id,
                organization_name=survey.main_supporter_user.organization_name,
            )
        response_supporter_users: List[SupporterUser] = None
        if survey.supporter_users:
            response_supporter_users = []
            for supporter_user in survey.supporter_users:
                response_supporter_users.append(
                    SupporterUser(
                        id=supporter_user.id,
                        name=supporter_user.name,
                        organization_id=supporter_user.organization_id,
                        organization_name=supporter_user.organization_name,
                    )
                )
        else:
            # アクセラレーターが設定されていない場合、サービス責任者をセット
            if survey.service_manager_id and survey.service_manager_name:
                response_supporter_users = [
                    SupporterUser(
                        id=survey.service_manager_id,
                        name=survey.service_manager_name,
                        organization_id=None,
                        organization_name=None,
                    )
                ]

        response_answers: List[Answer] = None
        if survey.answers:
            response_answers = []
            for answer in survey.answers:
                choice_ids = None
                if answer.choice_ids is not None:
                    choice_ids = list(answer.choice_ids)
                response_answers.append(
                    Answer(
                        id=answer.id,
                        answer=answer.answer,
                        point=answer.point,
                        choice_ids=choice_ids,
                        summary_type=answer.summary_type,
                        other_input=answer.other_input,
                    )
                )
        response_answer_user_name = ""
        if survey.answer_user_id:
            # 通常アンケートの場合
            response_answer_user_name = survey.answer_user_name
        elif survey.dedicated_survey_user_email:
            # 匿名アンケートの場合
            response_answer_user_name = survey.dedicated_survey_user_name

        response_survey: dict = copy.deepcopy(survey.attribute_values)
        # SurveyInfoForGetSurveysResponse生成時にmultiple valueエラーとなる為、元の項目は削除
        if "main_supporter_user" in response_survey.keys():
            del response_survey["main_supporter_user"]
        if "supporter_users" in response_survey.keys():
            del response_survey["supporter_users"]
        if "answers" in response_survey.keys():
            del response_survey["answers"]
        if "answer_user_name" in response_survey.keys():
            del response_survey["answer_user_name"]

        return GetSurveyByIdResponse(
            survey_revision=survey.survey_master_revision,
            main_supporter_user=response_main_supporter_user,
            supporter_users=response_supporter_users,
            sales_user_name=response_sales_user_name,
            answers=response_answers,
            answer_user_name=response_answer_user_name,
            **response_survey,
        )

    @staticmethod
    def get_surveys(
        query_params: GetSurveysQuery, current_user: UserModel
    ) -> GetSurveysResponse:
        """案件アンケートの一覧を取得
        Args:
            query_params (GetSurveysQuery): クエリパラメータ
            current_user (UserModel): API呼出ユーザー
        Returns:
            GetSurveysResponse: 自身のアンケートリスト
        """
        range_key_condition_data_type_plan_survey_response = None
        filter_condition_data_type_plan_survey_response = None
        range_key_condition_project_id_month = None
        filter_condition_project_id_month = None
        range_key_condition_survey_type_month = None
        filter_condition_survey_type_month = None
        range_key_condition_data_type_month = None
        filter_condition_data_type_month = None

        # 検索条件の編集
        # 共通条件： 回答（送信）依頼実績日時が空でない情報
        filter_condition_data_type_plan_survey_response &= (
            ProjectSurveyModel.actual_survey_request_datetime.exists()
            & (condition_size(ProjectSurveyModel.actual_survey_request_datetime) > 0)
        )
        filter_condition_project_id_month &= (
            ProjectSurveyModel.actual_survey_request_datetime.exists()
            & (condition_size(ProjectSurveyModel.actual_survey_request_datetime) > 0)
        )
        filter_condition_survey_type_month &= (
            ProjectSurveyModel.actual_survey_request_datetime.exists()
            & (condition_size(ProjectSurveyModel.actual_survey_request_datetime) > 0)
        )
        filter_condition_data_type_month &= (
            ProjectSurveyModel.actual_survey_request_datetime.exists()
            & (condition_size(ProjectSurveyModel.actual_survey_request_datetime) > 0)
        )

        # summary_month
        summary_month_separated = (
            datetime.strptime(query_params.summary_month, "%Y%m").strftime("%Y/%m")
            if query_params.summary_month
            else None
        )
        if summary_month_separated:
            filter_condition_data_type_plan_survey_response &= (
                ProjectSurveyModel.summary_month == summary_month_separated
            )
            range_key_condition_project_id_month &= (
                ProjectSurveyModel.summary_month == summary_month_separated
            )
            range_key_condition_survey_type_month &= (
                ProjectSurveyModel.summary_month == summary_month_separated
            )
            range_key_condition_data_type_month &= (
                ProjectSurveyModel.summary_month == summary_month_separated
            )

        # summary_month_from, summary_month_to
        month_from_separated = (
            datetime.strptime(query_params.summary_month_from, "%Y%m").strftime("%Y/%m")
            if query_params.summary_month_from
            else DateTime.MINIMUM
        )
        month_to_separated = (
            datetime.strptime(query_params.summary_month_to, "%Y%m").strftime("%Y/%m")
            if query_params.summary_month_to
            else DateTime.MAXIMUM
        )
        if query_params.summary_month_from or query_params.summary_month_to:
            filter_condition_data_type_plan_survey_response &= (
                ProjectSurveyModel.summary_month.between(
                    month_from_separated, month_to_separated
                )
            )
            range_key_condition_project_id_month &= (
                ProjectSurveyModel.summary_month.between(
                    month_from_separated, month_to_separated
                )
            )
            range_key_condition_survey_type_month &= (
                ProjectSurveyModel.summary_month.between(
                    month_from_separated, month_to_separated
                )
            )
            range_key_condition_data_type_month &= (
                ProjectSurveyModel.summary_month.between(
                    month_from_separated, month_to_separated
                )
            )

        # plan_survey_response_date_from,plan_survey_response_date_to
        plan_survey_response_date_from_separated = (
            datetime.strptime(
                query_params.plan_survey_response_date_from, "%Y%m%d"
            ).strftime("%Y/%m/%d")
            + " "
            + HourMinutes.MINIMUM
            if query_params.plan_survey_response_date_from
            else DateTimeHourMinutes.MINIMUM
        )
        plan_survey_response_date_to_separated = (
            datetime.strptime(
                query_params.plan_survey_response_date_to, "%Y%m%d"
            ).strftime("%Y/%m/%d")
            + " "
            + HourMinutes.MAXIMUM
            if query_params.plan_survey_response_date_to
            else DateTimeHourMinutes.MAXIMUM
        )
        if (
            query_params.plan_survey_response_date_from
            or query_params.plan_survey_response_date_to
        ):
            range_key_condition_data_type_plan_survey_response &= (
                ProjectSurveyModel.plan_survey_response_datetime.between(
                    plan_survey_response_date_from_separated,
                    plan_survey_response_date_to_separated,
                )
            )
            filter_condition_project_id_month &= (
                ProjectSurveyModel.plan_survey_response_datetime.between(
                    plan_survey_response_date_from_separated,
                    plan_survey_response_date_to_separated,
                )
            )
            filter_condition_survey_type_month &= (
                ProjectSurveyModel.plan_survey_response_datetime.between(
                    plan_survey_response_date_from_separated,
                    plan_survey_response_date_to_separated,
                )
            )
            filter_condition_data_type_month &= (
                ProjectSurveyModel.plan_survey_response_datetime.between(
                    plan_survey_response_date_from_separated,
                    plan_survey_response_date_to_separated,
                )
            )

        # project_id
        if query_params.project_id:
            filter_condition_data_type_plan_survey_response &= (
                ProjectSurveyModel.project_id == query_params.project_id
            )
            filter_condition_survey_type_month &= (
                ProjectSurveyModel.project_id == query_params.project_id
            )
            filter_condition_data_type_month &= (
                ProjectSurveyModel.project_id == query_params.project_id
            )

        # customer_id
        if query_params.customer_id:
            filter_condition_data_type_plan_survey_response &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )
            filter_condition_project_id_month &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )
            filter_condition_survey_type_month &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )
            filter_condition_data_type_month &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )

        # service_type_id
        if query_params.service_type_id:
            filter_condition_data_type_plan_survey_response &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )
            filter_condition_project_id_month &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )
            filter_condition_survey_type_month &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )
            filter_condition_data_type_month &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )

        # is_finished
        if query_params.is_finished:
            filter_condition_data_type_plan_survey_response &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )
            filter_condition_project_id_month &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )
            filter_condition_survey_type_month &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )
            filter_condition_data_type_month &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )

        # type
        if query_params.type:
            if query_params.type == SurveyTypeForGetSurveys.SERVICE_AND_COMPLETION:
                filter_condition_data_type_plan_survey_response &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
                ) | (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
                )
                filter_condition_project_id_month &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
                ) | (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
                )
                filter_condition_data_type_month &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
                ) | (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
                )
            elif query_params.type == SurveyTypeForGetSurveys.NON_PP:
                filter_condition_data_type_plan_survey_response &= (
                    ProjectSurveyModel.survey_type != SurveyTypeForGetSurveys.PP
                )
                filter_condition_project_id_month &= (
                    ProjectSurveyModel.survey_type != SurveyTypeForGetSurveys.PP
                )
                filter_condition_data_type_month &= (
                    ProjectSurveyModel.survey_type != SurveyTypeForGetSurveys.PP
                )
            else:
                filter_condition_data_type_plan_survey_response &= (
                    ProjectSurveyModel.survey_type == query_params.type
                )
                filter_condition_project_id_month &= (
                    ProjectSurveyModel.survey_type == query_params.type
                )
                filter_condition_data_type_month &= (
                    ProjectSurveyModel.survey_type == query_params.type
                )

        # organization_ids
        supporter_organization_id_list: List[str] = (
            query_params.organization_ids.split(",")
            if query_params.organization_ids
            else None
        )
        if supporter_organization_id_list:
            filter_condition_data_type_plan_survey_response &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )
            filter_condition_project_id_month &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )
            filter_condition_survey_type_month &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )
            filter_condition_data_type_month &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )

        # 案件アンケート情報の取得
        project_survey_list: List[ProjectSurveyModel] = []
        if (
            query_params.plan_survey_response_date_from
            or query_params.plan_survey_response_date_to
        ):
            # クエリパラメータに回答予定日From,またはToを含む場合
            project_survey_list.extend(
                list(
                    ProjectSurveyModel.data_type_plan_survey_response_datetime_index.query(
                        hash_key=DataType.SURVEY,
                        range_key_condition=range_key_condition_data_type_plan_survey_response,
                        filter_condition=filter_condition_data_type_plan_survey_response,
                    )
                )
            )

        elif query_params.project_id:
            # クエリパラメータにproject_idを含む場合
            project_survey_list.extend(
                list(
                    ProjectSurveyModel.project_id_summary_month_index.query(
                        hash_key=query_params.project_id,
                        range_key_condition=range_key_condition_project_id_month,
                        filter_condition=filter_condition_project_id_month,
                    )
                )
            )

        elif query_params.type:
            # クエリパラメータにtypeを含む場合
            if query_params.type == SurveyTypeForGetSurveys.SERVICE_AND_COMPLETION:
                # サービスと修了の両方を取得する場合
                project_survey_list.extend(
                    list(
                        ProjectSurveyModel.survey_type_summary_month_index.query(
                            hash_key=SurveyTypeForGetSurveys.SERVICE,
                            range_key_condition=range_key_condition_survey_type_month,
                            filter_condition=filter_condition_survey_type_month,
                        )
                    )
                )
                project_survey_list.extend(
                    list(
                        ProjectSurveyModel.survey_type_summary_month_index.query(
                            hash_key=SurveyTypeForGetSurveys.COMPLETION,
                            range_key_condition=range_key_condition_survey_type_month,
                            filter_condition=filter_condition_survey_type_month,
                        )
                    )
                )
            elif query_params.type == SurveyTypeForGetSurveys.NON_PP:
                # non_ppを指定した場合は、pp以外すべてを取得するため、
                # hash_keyがsurvey_typeのGSIでなく別のGSIを使用する
                project_survey_list.extend(
                    list(
                        ProjectSurveyModel.data_type_summary_month_index.query(
                            hash_key=DataType.SURVEY,
                            range_key_condition=range_key_condition_data_type_month,
                            filter_condition=filter_condition_data_type_month,
                        )
                    )
                )
            else:
                project_survey_list.extend(
                    list(
                        ProjectSurveyModel.survey_type_summary_month_index.query(
                            hash_key=query_params.type,
                            range_key_condition=range_key_condition_survey_type_month,
                            filter_condition=filter_condition_survey_type_month,
                        )
                    )
                )

        else:
            # 上記以外の場合
            project_survey_list.extend(
                list(
                    ProjectSurveyModel.data_type_summary_month_index.query(
                        hash_key=DataType.SURVEY,
                        range_key_condition=range_key_condition_data_type_month,
                        filter_condition=filter_condition_data_type_month,
                    )
                )
            )

        # 第1ソート: 未回答（昇順）, 第2ソート: 回答依頼日（降順）
        project_survey_list.sort(
            key=lambda x: x.actual_survey_request_datetime, reverse=True
        )
        project_survey_list.sort(
            key=lambda x: x.is_finished,
        )

        # レスポンス編集
        user_list: List[UserModel] = list(
            UserModel.data_type_name_index.query(hash_key=DataType.USER)
        )
        survey_info_response_list: List[SurveyInfoForGetSurveysResponse] = []
        target_summary_survey_model_list: List[ProjectSurveyModel] = []
        for survey in project_survey_list:
            # 権限チェック
            if not SurveyService.is_visible_survey_for_get_surveys(
                user=current_user,
                survey=survey,
            ):
                # アクセス不可の案件情報はスキップ
                continue

            # アクセス可の情報のみ格納
            target_summary_survey_model_list.append(survey)

            response_sales_user_name = ""
            for user in user_list:
                if survey.sales_user_id == user.id:
                    response_sales_user_name = user.name
                    break
            # レスポンスとmodelで定義が異なる項目を編集
            response_main_supporter_user: SupporterUser = None
            if survey.main_supporter_user:
                response_main_supporter_user = SupporterUser(
                    id=survey.main_supporter_user.id,
                    name=survey.main_supporter_user.name,
                    organization_id=survey.main_supporter_user.organization_id,
                    organization_name=survey.main_supporter_user.organization_name,
                )
            response_supporter_users: List[SupporterUser] = None
            if survey.supporter_users:
                response_supporter_users = []
                for supporter_user in survey.supporter_users:
                    response_supporter_users.append(
                        SupporterUser(
                            id=supporter_user.id,
                            name=supporter_user.name,
                            organization_id=supporter_user.organization_id,
                            organization_name=supporter_user.organization_name,
                        )
                    )
            response_answer_user_name = ""
            if survey.answer_user_id:
                # 通常アンケートの場合
                response_answer_user_name = survey.answer_user_name
            elif survey.dedicated_survey_user_email:
                # 匿名アンケートの場合
                response_answer_user_name = survey.dedicated_survey_user_name

            response_survey: dict = copy.deepcopy(survey.attribute_values)
            # SurveyInfoForGetSurveysResponse生成時にmultiple valueエラーとなる為、元の項目は削除
            if response_survey.get("main_supporter_user") is not None:
                del response_survey["main_supporter_user"]
            if response_survey.get("supporter_users") is not None:
                del response_survey["supporter_users"]
            if response_survey.get("answer_user_name") is not None:
                del response_survey["answer_user_name"]
            survey_info_response_list.append(
                SurveyInfoForGetSurveysResponse(
                    survey_revision=survey.survey_master_revision,
                    main_supporter_user=response_main_supporter_user,
                    supporter_users=response_supporter_users,
                    sales_user_name=response_sales_user_name,
                    answer_user_name=response_answer_user_name,
                    **response_survey,
                )
            )

        # レスポンスのsummary項目の編集
        # ・顧客の場合はサマリー項目は付与しない
        # ・アンケート種別がクイックアンケートの場合はサマリー項目の返却は不要
        summary: SummaryInfoForGetSurveysResponse = None
        if (
            current_user.role != UserRoleType.CUSTOMER.key
            and query_params.type != SurveyTypeForGetSurveys.QUICK
        ):
            # 集計年度の算出
            if query_params.summary_month:
                # summary_monthが指定されている場合、summary_monthから年度を算出
                summary_month_datetime = datetime.strptime(
                    query_params.summary_month, "%Y%m"
                )
                aggregation_year = (
                    str(summary_month_datetime.year - 1).zfill(4)
                    if summary_month_datetime.month < 4
                    else str(summary_month_datetime.year).zfill(4)
                )
            elif query_params.summary_month_to:
                # summary_month_toが指定されている場合、summary_month_toから年度を算出
                summary_month_to_datetime = datetime.strptime(
                    query_params.summary_month_to, "%Y%m"
                )
                aggregation_year = (
                    str(summary_month_to_datetime.year - 1).zfill(4)
                    if summary_month_to_datetime.month < 4
                    else str(summary_month_to_datetime.year).zfill(4)
                )
            else:
                # 現在日時を基に年度を算出
                now_datetime = get_datetime_now()
                aggregation_year = (
                    str(now_datetime.year - 1).zfill(4)
                    if now_datetime.month < 4
                    else str(now_datetime.year).zfill(4)
                )

            # summary項目の編集
            summary = SurveyService.edit_summary_of_get_surveys_response(
                target_list=target_summary_survey_model_list,
                aggregation_year=aggregation_year,
            )

        return GetSurveysResponse(summary=summary, surveys=survey_info_response_list)

    @staticmethod
    def update_survey_by_id(
        item: UpdateSurveyByIdRequest,
        query_params: UpdateSurveyByIdQuery,
        survey_id: str,
        current_user: UserModel,
    ) -> UpdateSurveyByIdResponse:
        """案件アンケートIDを指定して案件アンケート情報を更新する

        Args:
            item (UpdateSurveyByIdRequest): 更新内容
            query_params (UpdateSurveyByIdQuery): クエリパラメータ
            survey_id (str): 案件アンケートID
            current_user (UserModel): API呼出ユーザー
        Raises:
            HTTPException: 400 Bad request
            HTTPException: 404 Not found
            HTTPException: 409 Conflict

        Returns:
            UpdateSurveyByIdResponse: 取得結果
        """
        try:
            survey = ProjectSurveyModel.get(
                hash_key=survey_id, range_key=DataType.SURVEY
            )
        except DoesNotExist:
            logger.warning(f"UpdateSurveyById not found. survey_id:{survey_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if survey.survey_type == SurveyType.PP:
            # PPアンケートの場合
            # 顧客はアクセス不可
            if current_user.role == UserRoleType.CUSTOMER.key:
                logger.warning(
                    f"UpdateSurveyById forbidden. survey.project_id: {survey.project_id}"
                )
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )
        else:
            # サービス・修了・クイックアンケートの場合
            # 権限チェック
            if not SurveyService.is_visible_survey_for_update_survey_by_id(
                user_id=current_user.id,
                role=current_user.role,
                survey=survey,
            ):
                logger.warning(
                    f"UpdateSurveyById forbidden. survey.project_id: {survey.project_id}"
                )
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
                )

        # 排他チェック
        if query_params.version != survey.version:
            logger.warning(
                f"UpdateSurveyById conflict. request_ver:{query_params.version} survey_ver: {survey.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # バリデーションチェック
        # 回答依頼送信済でないアンケートは取得不可
        if not survey.actual_survey_request_datetime:
            logger.warning(
                "UpdateSurveyById bad request. Cannot get a survey for which the response request has not been sent."
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Cannot get a survey for which the response request has not been sent.",
            )

        # 更新処理
        update_action: List[Action] = []
        update_answers: List[AnswersAttribute] = []
        satisfaction: int = 0
        continuation: bool = False
        recommended: int = 0
        sales: int = 0
        survey_satisfaction: int = 0
        man_hour_satisfaction: int = 0
        karte_satisfaction: int = 0
        master_karte_satisfaction: int = 0

        for answer in item.answers:
            choice_ids = None
            if answer.choice_ids is not None:
                choice_ids = set(answer.choice_ids)
            update_answers.append(
                AnswersAttribute(
                    id=answer.id,
                    answer=answer.answer,
                    point=answer.point,
                    choice_ids=choice_ids,
                    summary_type=answer.summary_type,
                    other_input=answer.other_input,
                )
            )
            # points(指標となる数値合計)編集
            if answer.summary_type == SurveyQuestionsSummaryType.SATISFACTION:
                satisfaction += answer.point
            elif answer.summary_type == SurveyQuestionsSummaryType.CONTINUATION:
                if answer.point > 0:
                    continuation = True
            elif answer.summary_type == SurveyQuestionsSummaryType.RECOMMENDED:
                recommended += answer.point
            elif answer.summary_type == SurveyQuestionsSummaryType.SALES:
                sales += answer.point
            elif answer.summary_type == SurveyQuestionsSummaryType.SURVEY_SATISFACTION:
                survey_satisfaction += answer.point
            elif (
                answer.summary_type == SurveyQuestionsSummaryType.MAN_HOUR_SATISFACTION
            ):
                man_hour_satisfaction += answer.point
            elif answer.summary_type == SurveyQuestionsSummaryType.KARTE_SATISFACTION:
                karte_satisfaction += answer.point
            elif (
                answer.summary_type
                == SurveyQuestionsSummaryType.MASTER_KARTE_SATISFACTION
            ):
                master_karte_satisfaction += answer.point

        update_action.append(ProjectSurveyModel.answers.set(update_answers))
        update_points = PointsAttribute(
            satisfaction=satisfaction,
            continuation=continuation,
            recommended=recommended,
            sales=sales,
            survey_satisfaction=survey_satisfaction,
            man_hour_satisfaction=man_hour_satisfaction,
            karte_satisfaction=karte_satisfaction,
            master_karte_satisfaction=master_karte_satisfaction,
        )
        update_action.append(ProjectSurveyModel.points.set(update_points))
        update_action.append(
            ProjectSurveyModel.actual_survey_response_datetime.set(
                get_datetime_now().strftime("%Y/%m/%d %H:%M")
            )
        )
        update_action.append(ProjectSurveyModel.is_finished.set(item.is_finished))
        update_action.append(ProjectSurveyModel.is_disclosure.set(item.is_disclosure))

        # クイックアンケートまたはPPアンケート以外は連続未回答数の減算処理
        if survey.survey_type not in [SurveyType.QUICK, SurveyType.PP]:
            SurveyService.decreased_unanswered_surveys_number(survey)
            update_action.append(ProjectSurveyModel.unanswered_surveys_number.set(0))

        survey.update(actions=update_action)

        # PPアンケート以外はメール・お知らせを通知
        if survey.survey_type != SurveyType.PP:
            # メール・お知らせ通知
            bcc_address_list: List[str] = []
            fo_notification_id_list: List[str] = []
            bo_notification_id_list: List[str] = []
            # 案件情報
            try:
                project: ProjectModel = ProjectModel.get(
                    hash_key=survey.project_id, range_key=DataType.PROJECT
                )
            except DoesNotExist:
                logger.warning(
                    f"UpdateSurveyById survey.project_id not found. survey.project_id:{survey.project_id}"
                )
                project = None

            # 宛先設定
            # 営業担当責任者（全員）、システム管理者（全員）、アンケート事務局スタッフ（全員）、事業者責任者（全員）
            filter_condition = (
                AdminModel.roles.contains(UserRoleType.SYSTEM_ADMIN.key)
                | AdminModel.roles.contains(UserRoleType.SURVEY_OPS.key)
                | AdminModel.roles.contains(UserRoleType.SALES_MGR.key)
                | AdminModel.roles.contains(UserRoleType.BUSINESS_MGR.key)
            )
            admin_iter = AdminModel.scan(filter_condition=filter_condition)
            for admin in admin_iter:
                if not admin.disabled:
                    bcc_address_list.append(admin.email)
                    bo_notification_id_list.append(admin.id)

            # 支援者責任者（ユーザー権限マトリクス表に準ずる）
            # ・ユーザー権限マトリクス表より、所属していない非公開案件のアンケートは通知しない
            # ・案件にアサインされている支援者の組織IDに含まれていない支援者責任者には通知しない
            user_filter_condition = UserModel.disabled == False  # NOQA
            supporter_mgr_user_iter = UserModel.role_index.query(
                hash_key=UserRoleType.SUPPORTER_MGR.key,
                filter_condition=user_filter_condition,
            )
            for user in supporter_mgr_user_iter:
                if SurveyService.is_visible_survey_by_supporter_mgr_for_survey_answer_provided(
                    user=user,
                    survey=survey,
                    project=project,
                ):
                    bcc_address_list.append(user.email)
                    fo_notification_id_list.append(user.id)

            assign_user_id_list: List[str] = []
            # 案件にアサインされた営業担当者
            if survey.sales_user_id:
                assign_user_id_list.append(survey.sales_user_id)

            if survey.is_disclosure:
                # 開示OKの場合
                # 案件にアサインされた支援者
                if survey.main_supporter_user:
                    assign_user_id_list.append(survey.main_supporter_user.id)

                if survey.is_updated_evaluation_supporters:
                    if survey.supporter_users_before_update:
                        # アンケート評価対象者（アクセラレータ）が更新されている場合、更新前のアクセラレータを使用
                        for supporter in survey.supporter_users_before_update:
                            assign_user_id_list.append(supporter.id)
                else:
                    if survey.supporter_users:
                        for supporter in survey.supporter_users:
                            assign_user_id_list.append(supporter.id)

            item_keys = [(id, DataType.USER) for id in assign_user_id_list]
            for user in UserModel.batch_get(item_keys):
                if user.role in [
                    UserRoleType.SALES.key,
                    UserRoleType.SALES_MGR.key,
                    UserRoleType.SUPPORTER.key,
                    UserRoleType.SUPPORTER_MGR.key,
                ]:
                    if not user.disabled:
                        bcc_address_list.append(user.email)
                        fo_notification_id_list.append(user.id)

            # 本文の編集
            # URL
            fo_site_url = get_app_settings().fo_site_url
            fo_survey_detail_url = fo_site_url + FoAppUrl.SURVEY_DETAIL.format(
                surveyId=survey.id
            )
            bo_site_url = get_app_settings().bo_site_url
            bo_survey_detail_url = bo_site_url + BoAppUrl.SURVEY_ANSWER_RESULT.format(
                surveyId=survey.id
            )
            # 回答内容
            survey_answer_info_list: List[dict] = []
            survey_master = SurveyMasterModel.get(
                hash_key=survey.survey_master_id,
                range_key=survey.survey_master_revision,
            )
            for answer in survey.answers:
                answer_info: dict = {}
                description = ""
                for question in survey_master.questions:
                    if question.id == answer.id:
                        description = question.description
                        break
                answer_info["description"] = description
                answer_info["answer"] = answer.answer
                answer_info["other_input"] = (
                    answer.other_input if answer.other_input else ""
                )
                survey_answer_info_list.append(answer_info)

            customer_name_project_name = (
                (survey.customer_name or "") + "／" + (survey.project_name or "")
            )
            survey_month_str = "{year}年{month}月".format(
                year=survey.summary_month[0:4],
                month=survey.summary_month[5:],
            )
            survey_type_name = ""
            if survey.survey_type == SurveyType.SERVICE:
                survey_type_name = SurveyTypeName.SERVICE
            elif survey.survey_type == SurveyType.COMPLETION:
                survey_type_name = SurveyTypeName.COMPLETION
            elif survey.survey_type == SurveyType.QUICK:
                survey_type_name = SurveyTypeName.QUICK

            disclosure_display = "可" if survey.is_disclosure else "不可"

            # プロデューサー、アクセラレータ、営業担当、サービス責任者の名前
            main_supporter_user_name: str = ""
            supporter_users_names: List[str] = []
            sales_user_name: str = ""
            if survey.main_supporter_user:
                main_supporter_user_name = survey.main_supporter_user.name

            if survey.is_updated_evaluation_supporters:
                if survey.supporter_users_before_update:
                    # アンケート評価対象者（アクセラレータ）が更新されている場合、更新前のアクセラレータを使用
                    supporter_users_names = [
                        supporter_user.name
                        for supporter_user in survey.supporter_users_before_update
                    ]
            else:
                if survey.supporter_users:
                    supporter_users_names = [
                        supporter_user.name for supporter_user in survey.supporter_users
                    ]

            project: ProjectModel = ProjectModel.get(
                hash_key=survey.project_id, range_key=DataType.PROJECT
            )

            service_manager_name: str = ""
            if project.service_manager_name:
                service_manager_name = project.service_manager_name

            # [sales_user_id]から営業担当をDBから取得
            user: UserModel = UserModel.get(
                hash_key=survey.sales_user_id, range_key=DataType.USER
            )
            sales_user_name = user.name

            payload: dict = {
                "customer_name_project_name": customer_name_project_name,
                "fo_survey_detail_url": fo_survey_detail_url,
                "bo_survey_detail_url": bo_survey_detail_url,
                "survey_month": survey_month_str,
                "survey_type_name": survey_type_name,
                "disclosure": disclosure_display,
                "survey_answer_info_list": survey_answer_info_list,
                "service_manager_name": service_manager_name,
                "main_supporter_user_name": main_supporter_user_name,
                "supporter_users_names": supporter_users_names,
                "sales_user_name": sales_user_name,
            }
            # メール送信
            bcc_address_list = list(set(bcc_address_list))
            ProjectService.send_mail(
                template=MailType.SURVEY_ANSWER_PROVIDED,
                to_addr_list=[],
                cc_addr_list=[],
                bcc_addr_list=bcc_address_list,
                payload=payload,
            )

            # お知らせ通知
            notification_noticed_at = datetime.now()
            fo_notification_id_list = list(set(fo_notification_id_list))
            bo_notification_id_list = list(set(bo_notification_id_list))
            # FO
            NotificationService.save_notification(
                notification_type=NotificationType.SURVEY_ANSWER_PROVIDED,
                user_id_list=fo_notification_id_list,
                message_param=payload,
                url=payload.get("fo_survey_detail_url"),
                noticed_at=notification_noticed_at,
                create_id=current_user.id,
                update_id=current_user.id,
            )
            # BO
            NotificationService.save_notification(
                notification_type=NotificationType.SURVEY_ANSWER_PROVIDED,
                user_id_list=bo_notification_id_list,
                message_param=payload,
                url=payload.get("bo_survey_detail_url"),
                noticed_at=notification_noticed_at,
                create_id=current_user.id,
                update_id=current_user.id,
            )

        return UpdateSurveyByIdResponse(message="OK")

    @staticmethod
    def check_and_get_survey_anonymous_by_id(
        item: CheckAndGetSurveyAnonymousByIdRequest,
        survey_id: str,
    ) -> CheckAndGetSurveyAnonymousByIdResponse:
        """Get /surveys/anonymous/{survey_id} 匿名用の案件アンケートをIDで一意取得API

        Args:
            item (CheckAndGetSurveyAnonymousByIdRequest): 認証情報
            survey_id (str): 案件アンケートID パスパラメータで指定
        Raises:
            HTTPException: 400 Bad request
            HTTPException: 404 Not found

        Returns:
            CheckAndGetSurveyAnonymousByIdResponse: 取得結果
        """

        # 匿名アンケート認証
        auth_check_result: PostCheckSurveyByIdPasswordResponse = (
            SurveyService.post_check_survey_by_id_password(
                PostCheckSurveyByIdPasswordRequest(
                    token=item.token, password=item.password
                )
            )
        )
        # 認証情報のJWTに含まれるアンケートIDとパスパラメータのアンケートIDが異なる場合
        if auth_check_result.id != survey_id:
            logger.warning(
                f"GetSurveyAnonymousById survey bad request. SurveyId and token do not match. survey_id: {survey_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="SurveyId and token do not match",
            )

        try:
            survey = ProjectSurveyModel.get(
                hash_key=survey_id, range_key=DataType.SURVEY
            )
        except DoesNotExist:
            logger.warning(
                f"GetSurveyAnonymousById survey not found. survey_id: {survey_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # バリデーションチェック
        # 回答依頼送信済でないアンケートは取得不可
        if not survey.actual_survey_request_datetime:
            logger.warning(
                "GetSurveyAnonymousById bad request. Cannot get a survey for which the response request has not been sent."
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Cannot get a survey for which the response request has not been sent.",
            )

        # レスポンス編集
        response_sales_user_name = ""
        if survey.sales_user_id:
            try:
                user: UserModel = UserModel.get(
                    hash_key=survey.sales_user_id, range_key=DataType.USER
                )
                response_sales_user_name = user.name
            except DoesNotExist:
                pass
        # レスポンスとmodelで定義が異なる項目を編集
        response_main_supporter_user: SupporterUser = None
        if survey.main_supporter_user:
            response_main_supporter_user = SupporterUser(
                id=survey.main_supporter_user.id,
                name=survey.main_supporter_user.name,
                organization_id=survey.main_supporter_user.organization_id,
                organization_name=survey.main_supporter_user.organization_name,
            )
        response_supporter_users: List[SupporterUser] = None
        if survey.supporter_users:
            response_supporter_users = []
            for supporter_user in survey.supporter_users:
                response_supporter_users.append(
                    SupporterUser(
                        id=supporter_user.id,
                        name=supporter_user.name,
                        organization_id=supporter_user.organization_id,
                        organization_name=supporter_user.organization_name,
                    )
                )
        else:
            # アクセラレーターが設定されていない場合、サービス責任者をセット
            if survey.service_manager_id and survey.service_manager_name:
                response_supporter_users = [
                    SupporterUser(
                        id=survey.service_manager_id,
                        name=survey.service_manager_name,
                        organization_id=None,
                        organization_name=None,
                    )
                ]
        response_answers: List[Answer] = None
        if survey.answers:
            response_answers = []
            for answer in survey.answers:
                choice_ids = None
                if answer.choice_ids is not None:
                    choice_ids = list(answer.choice_ids)
                response_answers.append(
                    Answer(
                        id=answer.id,
                        answer=answer.answer,
                        point=answer.point,
                        choice_ids=choice_ids,
                        summary_type=answer.summary_type,
                        other_input=answer.other_input,
                    )
                )
        response_answer_user_name = ""
        if survey.dedicated_survey_user_name:
            response_answer_user_name = survey.dedicated_survey_user_name

        response_survey: dict = copy.deepcopy(survey.attribute_values)
        # SurveyInfoForGetSurveysResponse生成時にmultiple valueエラーとなる為、元の項目は削除
        if "main_supporter_user" in response_survey.keys():
            del response_survey["main_supporter_user"]
        if "supporter_users" in response_survey.keys():
            del response_survey["supporter_users"]
        if "answer_user_name" in response_survey.keys():
            del response_survey["answer_user_name"]
        if "answers" in response_survey.keys():
            del response_survey["answers"]

        return CheckAndGetSurveyAnonymousByIdResponse(
            survey_revision=survey.survey_master_revision,
            main_supporter_user=response_main_supporter_user,
            supporter_users=response_supporter_users,
            sales_user_name=response_sales_user_name,
            answer_user_name=response_answer_user_name,
            answers=response_answers,
            **response_survey,
        )

    @staticmethod
    def check_and_update_survey_anonymous_by_id(
        item: CheckAndUpdateSurveyAnonymousByIdRequest,
        query_params: UpdateSurveyByIdQuery,
        survey_id: str,
    ) -> OKResponse:
        """Put /surveys/anonymous/{survey_id} 匿名用の案件アンケートをIDで一意に更新API

        Args:
            item (CheckAndUpdateSurveyAnonymousByIdRequest): 認証情報
            query_params (UpdateSurveyByIdQuery): クエリパラメータ
            survey_id (str): 案件アンケートID パスパラメータで指定

        Returns:
            OKResponse: 更新結果
        """

        # 匿名アンケート認証
        auth_check_result: PostCheckSurveyByIdPasswordResponse = (
            SurveyService.post_check_survey_by_id_password(
                PostCheckSurveyByIdPasswordRequest(
                    token=item.token, password=item.password
                )
            )
        )
        # 認証情報のJWTに含まれるアンケートIDとパスパラメータのアンケートIDが異なる場合
        if auth_check_result.id != survey_id:
            logger.warning(
                f"UpdateSurveyAnonymousById survey bad request. SurveyId and token do not match. survey_id: {survey_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="SurveyId and token do not match",
            )

        try:
            survey = ProjectSurveyModel.get(
                hash_key=survey_id, range_key=DataType.SURVEY
            )
        except DoesNotExist:
            logger.warning(
                f"UpdateSurveyAnonymousById not found. survey_id:{survey_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 排他チェック
        if query_params.version != survey.version:
            logger.warning(
                f"UpdateSurveyAnonymousById conflict. request_ver:{query_params.version} survey_ver: {survey.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # バリデーションチェック
        # 回答依頼送信済でないアンケートは取得不可
        if not survey.actual_survey_request_datetime:
            logger.warning(
                "UpdateSurveyAnonymousById bad request. Cannot get a survey for which the response request has not been sent."
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Cannot get a survey for which the response request has not been sent.",
            )

        # 更新処理
        update_action: List[Action] = []
        update_answers: List[AnswersAttribute] = []
        satisfaction: int = 0
        continuation: bool = False
        recommended: int = 0
        sales: int = 0
        survey_satisfaction: int = 0
        man_hour_satisfaction: int = 0
        karte_satisfaction: int = 0
        master_karte_satisfaction: int = 0

        for answer in item.answers:
            choice_ids = None
            if answer.choice_ids is not None:
                choice_ids = set(answer.choice_ids)
            update_answers.append(
                AnswersAttribute(
                    id=answer.id,
                    answer=answer.answer,
                    point=answer.point,
                    choice_ids=choice_ids,
                    summary_type=answer.summary_type,
                    other_input=answer.other_input,
                )
            )
            # points(指標となる数値合計)編集
            if answer.summary_type == SurveyQuestionsSummaryType.SATISFACTION:
                satisfaction += answer.point
            elif answer.summary_type == SurveyQuestionsSummaryType.CONTINUATION:
                if answer.point > 0:
                    continuation = True
            elif answer.summary_type == SurveyQuestionsSummaryType.RECOMMENDED:
                recommended += answer.point
            elif answer.summary_type == SurveyQuestionsSummaryType.SALES:
                sales += answer.point
            elif answer.summary_type == SurveyQuestionsSummaryType.SURVEY_SATISFACTION:
                survey_satisfaction += answer.point
            elif (
                answer.summary_type == SurveyQuestionsSummaryType.MAN_HOUR_SATISFACTION
            ):
                man_hour_satisfaction += answer.point
            elif answer.summary_type == SurveyQuestionsSummaryType.KARTE_SATISFACTION:
                karte_satisfaction += answer.point
            elif (
                answer.summary_type
                == SurveyQuestionsSummaryType.MASTER_KARTE_SATISFACTION
            ):
                master_karte_satisfaction += answer.point

        update_action.append(ProjectSurveyModel.answers.set(update_answers))
        update_points = PointsAttribute(
            satisfaction=satisfaction,
            continuation=continuation,
            recommended=recommended,
            sales=sales,
            survey_satisfaction=survey_satisfaction,
            man_hour_satisfaction=man_hour_satisfaction,
            karte_satisfaction=karte_satisfaction,
            master_karte_satisfaction=master_karte_satisfaction,
        )
        update_action.append(ProjectSurveyModel.points.set(update_points))
        update_action.append(
            ProjectSurveyModel.actual_survey_response_datetime.set(
                get_datetime_now().strftime("%Y/%m/%d %H:%M")
            )
        )
        update_action.append(ProjectSurveyModel.is_finished.set(item.is_finished))
        update_action.append(ProjectSurveyModel.is_disclosure.set(item.is_disclosure))

        # クイックアンケートまたはPPアンケート以外は連続未回答数の減算処理
        if survey.survey_type not in [SurveyType.QUICK, SurveyType.PP]:
            SurveyService.decreased_unanswered_surveys_number(survey)
            update_action.append(ProjectSurveyModel.unanswered_surveys_number.set(0))

        survey.update(actions=update_action)

        # PPアンケート以外はメール・お知らせを通知
        if survey.survey_type != SurveyType.PP:
            # メール・お知らせ通知
            bcc_address_list: List[str] = []
            fo_notification_id_list: List[str] = []
            bo_notification_id_list: List[str] = []
            # 案件情報
            try:
                project: ProjectModel = ProjectModel.get(
                    hash_key=survey.project_id, range_key=DataType.PROJECT
                )
            except DoesNotExist:
                logger.warning(
                    f"UpdateSurveyById survey.project_id not found. survey.project_id:{survey.project_id}"
                )
                project = None

            # 宛先設定
            # 営業担当責任者（全員）、システム管理者（全員）、アンケート事務局スタッフ（全員）、事業者責任者（全員）
            filter_condition = (
                AdminModel.roles.contains(UserRoleType.SYSTEM_ADMIN.key)
                | AdminModel.roles.contains(UserRoleType.SURVEY_OPS.key)
                | AdminModel.roles.contains(UserRoleType.SALES_MGR.key)
                | AdminModel.roles.contains(UserRoleType.BUSINESS_MGR.key)
            )
            admin_iter = AdminModel.scan(filter_condition=filter_condition)
            for admin in admin_iter:
                if not admin.disabled:
                    bcc_address_list.append(admin.email)
                    bo_notification_id_list.append(admin.id)

            # 支援者責任者（ユーザー権限マトリクス表に準ずる）
            # ・ユーザー権限マトリクス表より、所属していない非公開案件のアンケートは通知しない
            # ・案件にアサインされている支援者の組織IDに含まれていない支援者責任者には通知しない
            user_filter_condition = UserModel.disabled == False  # NOQA
            supporter_mgr_user_iter = UserModel.role_index.query(
                hash_key=UserRoleType.SUPPORTER_MGR.key,
                filter_condition=user_filter_condition,
            )
            for user in supporter_mgr_user_iter:
                if SurveyService.is_visible_survey_by_supporter_mgr_for_survey_answer_provided(
                    user=user,
                    survey=survey,
                    project=project,
                ):
                    bcc_address_list.append(user.email)
                    fo_notification_id_list.append(user.id)

            assign_user_id_list: List[str] = []
            # 案件にアサインされた営業担当者
            if survey.sales_user_id:
                assign_user_id_list.append(survey.sales_user_id)

            if survey.is_disclosure:
                # 開示OKの場合
                # 案件にアサインされた支援者
                if survey.main_supporter_user:
                    assign_user_id_list.append(survey.main_supporter_user.id)

                if survey.is_updated_evaluation_supporters:
                    if survey.supporter_users_before_update:
                        # アンケート評価対象者（アクセラレータ）が更新されている場合、更新前のアクセラレータを使用
                        for supporter in survey.supporter_users_before_update:
                            assign_user_id_list.append(supporter.id)
                else:
                    if survey.supporter_users:
                        for supporter in survey.supporter_users:
                            assign_user_id_list.append(supporter.id)

            item_keys = [(id, DataType.USER) for id in assign_user_id_list]
            for user in UserModel.batch_get(item_keys):
                if user.role in [
                    UserRoleType.SALES.key,
                    UserRoleType.SALES_MGR.key,
                    UserRoleType.SUPPORTER.key,
                    UserRoleType.SUPPORTER_MGR.key,
                ]:
                    if not user.disabled:
                        bcc_address_list.append(user.email)
                        fo_notification_id_list.append(user.id)

            # 本文の編集
            # URL
            fo_site_url = get_app_settings().fo_site_url
            fo_survey_detail_url = fo_site_url + FoAppUrl.SURVEY_DETAIL.format(
                surveyId=survey.id
            )
            bo_site_url = get_app_settings().bo_site_url
            bo_survey_detail_url = bo_site_url + BoAppUrl.SURVEY_ANSWER_RESULT.format(
                surveyId=survey.id
            )
            # 回答内容
            survey_answer_info_list: List[dict] = []
            survey_master = SurveyMasterModel.get(
                hash_key=survey.survey_master_id,
                range_key=survey.survey_master_revision,
            )
            for answer in survey.answers:
                answer_info: dict = {}
                description = ""
                for question in survey_master.questions:
                    if question.id == answer.id:
                        description = question.description
                        break
                answer_info["description"] = description
                answer_info["answer"] = answer.answer
                answer_info["other_input"] = (
                    answer.other_input if answer.other_input else ""
                )
                survey_answer_info_list.append(answer_info)

            customer_name_project_name = (
                (survey.customer_name or "") + "／" + (survey.project_name or "")
            )
            survey_month_str = "{year}年{month}月".format(
                year=survey.summary_month[0:4],
                month=survey.summary_month[5:],
            )
            survey_type_name = ""
            if survey.survey_type == SurveyType.SERVICE:
                survey_type_name = SurveyTypeName.SERVICE
            elif survey.survey_type == SurveyType.COMPLETION:
                survey_type_name = SurveyTypeName.COMPLETION
            elif survey.survey_type == SurveyType.QUICK:
                survey_type_name = SurveyTypeName.QUICK

            disclosure_display = "可" if survey.is_disclosure else "不可"

            # プロデューサー、アクセラレータ、営業担当、サービス責任者の名前
            main_supporter_user_name: str = ""
            supporter_users_names: List[str] = []
            sales_user_name: str = ""
            if survey.main_supporter_user:
                main_supporter_user_name = survey.main_supporter_user.name

            if survey.is_updated_evaluation_supporters:
                if survey.supporter_users_before_update:
                    # アンケート評価対象者（アクセラレータ）が更新されている場合、更新前のアクセラレータを使用
                    supporter_users_names = [
                        supporter_user.name
                        for supporter_user in survey.supporter_users_before_update
                    ]
            else:
                if survey.supporter_users:
                    supporter_users_names = [
                        supporter_user.name for supporter_user in survey.supporter_users
                    ]

            project: ProjectModel = ProjectModel.get(
                hash_key=survey.project_id, range_key=DataType.PROJECT
            )

            service_manager_name: str = ""
            if project.service_manager_name:
                service_manager_name = project.service_manager_name

            # [sales_user_id]から営業担当をDBから取得
            user: UserModel = UserModel.get(
                hash_key=survey.sales_user_id, range_key=DataType.USER
            )
            sales_user_name = user.name

            payload: dict = {
                "customer_name_project_name": customer_name_project_name,
                "fo_survey_detail_url": fo_survey_detail_url,
                "bo_survey_detail_url": bo_survey_detail_url,
                "survey_month": survey_month_str,
                "survey_type_name": survey_type_name,
                "disclosure": disclosure_display,
                "survey_answer_info_list": survey_answer_info_list,
                "service_manager_name": service_manager_name,
                "main_supporter_user_name": main_supporter_user_name,
                "supporter_users_names": supporter_users_names,
                "sales_user_name": sales_user_name,
            }
            # メール送信
            bcc_address_list = list(set(bcc_address_list))
            ProjectService.send_mail(
                template=MailType.SURVEY_ANSWER_PROVIDED,
                to_addr_list=[],
                cc_addr_list=[],
                bcc_addr_list=bcc_address_list,
                payload=payload,
            )

            # 匿名アンケートであるため、create_id & update_id は更新しない
            # お知らせ通知
            notification_noticed_at = datetime.now()
            fo_notification_id_list = list(set(fo_notification_id_list))
            bo_notification_id_list = list(set(bo_notification_id_list))
            # FO
            NotificationService.save_notification(
                notification_type=NotificationType.SURVEY_ANSWER_PROVIDED,
                user_id_list=fo_notification_id_list,
                message_param=payload,
                url=payload.get("fo_survey_detail_url"),
                noticed_at=notification_noticed_at,
                create_id="",
                update_id="",
            )
            # BO
            NotificationService.save_notification(
                notification_type=NotificationType.SURVEY_ANSWER_PROVIDED,
                user_id_list=bo_notification_id_list,
                message_param=payload,
                url=payload.get("bo_survey_detail_url"),
                noticed_at=notification_noticed_at,
                create_id="",
                update_id="",
            )

        return OKResponse()

    @staticmethod
    def get_survey_of_satisfaction_by_id(
        item: GetSurveyOfSatisfactionByIdRequest,
    ) -> GetSurveyOfSatisfactionByIdResponse:
        """Get /surveys/satisfaction/{survey_id} 満足度評価のみ回答アンケートをIDで一意に取得API

        Args:
            item (GetSurveyOfSatisfactionByIdRequest): 認証情報
        Raises:
            HTTPException: 400 Bad request
            HTTPException: 404 Not found

        Returns:
            GetSurveyOfSatisfactionByIdResponse: 取得結果
        """

        # 満足度評価のみ回答アンケート認証
        SurveyService.post_check_survey_by_id(
            PostCheckSurveyByIdRequest(token=item.token)
        )

        survey_id: str = decode_jwt(item.token)["survey_id"]

        try:
            survey = ProjectSurveyModel.get(
                hash_key=survey_id, range_key=DataType.SURVEY
            )
        except DoesNotExist:
            logger.warning(
                f"GetSurveyOfSatisfactionById survey not found. survey_id: {survey_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # バリデーションチェック
        # 回答依頼送信済でないアンケートは取得不可
        if not survey.actual_survey_request_datetime:
            logger.warning(
                "GetSurveyOfSatisfactionById bad request. Cannot get a survey for which the response request has not been sent."
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Cannot get a survey for which the response request has not been sent.",
            )

        if survey.is_finished:
            return GetSurveyOfSatisfactionByIdResponse(
                is_finished=True,
                project_id=survey.project_id,
                project_name=survey.project_name,
                customer_name=survey.customer_name,
            )
        return GetSurveyOfSatisfactionByIdResponse(
            is_finished=False,
            survey_master_id=survey.survey_master_id,
            survey_id=survey_id,
            survey_revision=survey.survey_master_revision,
            project_id=survey.project_id,
            project_name=survey.project_name,
            customer_name=survey.customer_name,
            version=survey.version,
        )

    @staticmethod
    def update_survey_of_satisfaction_by_id(
        item: UpdateSurveyOfSatisfactionByIdRequest,
        query_params: UpdateSurveyByIdQuery,
    ) -> OKResponse:
        """Put /surveys/satisfaction/{survey_id} 満足度評価のみ回答用の案件アンケートをIDで一意に更新API

        Args:
            item (UpdateSurveyOfSatisfactionByIdRequest): 更新内容
            query_params (UpdateSurveyByIdQuery): クエリパラメータ
            current_user (UserModel): API呼出ユーザー

        Returns:
            OKResponse: 更新結果
        """

        # 満足度評価のみ回答アンケート認証
        SurveyService.post_check_survey_by_id(
            PostCheckSurveyByIdRequest(token=item.token)
        )

        survey_id: str = decode_jwt(item.token)["survey_id"]

        try:
            survey = ProjectSurveyModel.get(
                hash_key=survey_id, range_key=DataType.SURVEY
            )
        except DoesNotExist:
            logger.warning(
                f"UpdateSurveyOfSatisfactionById not found. survey_id:{survey_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 排他チェック
        if query_params.version != survey.version:
            logger.warning(
                f"UpdateSurveyOfSatisfactionById conflict. request_ver:{query_params.version} survey_ver: {survey.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        # バリデーションチェック
        # 回答依頼送信済でないアンケートは取得不可
        if not survey.actual_survey_request_datetime:
            logger.warning(
                "UpdateSurveyOfSatisfactionById bad request. Cannot get a survey for which the response request has not been sent."
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Cannot get a survey for which the response request has not been sent.",
            )

        # 更新処理
        update_action: List[Action] = []
        update_answers: List[AnswersAttribute] = []
        satisfaction: int = 0
        # 満足度評価設問以外の設問を未回答指標にセット
        unanswered = set(
            [
                SurveyQuestionsSummaryType.CONTINUATION,
                SurveyQuestionsSummaryType.RECOMMENDED,
                SurveyQuestionsSummaryType.SALES,
            ]
        )

        for answer in item.answers:
            choice_ids = None
            if answer.choice_ids is not None:
                choice_ids = set(answer.choice_ids)
            update_answers.append(
                AnswersAttribute(
                    id=answer.id,
                    answer=answer.answer,
                    point=answer.point,
                    choice_ids=choice_ids,
                    summary_type=answer.summary_type,
                    other_input=answer.other_input,
                )
            )
            # points(指標となる数値合計)編集
            if answer.summary_type == SurveyQuestionsSummaryType.SATISFACTION:
                satisfaction += answer.point

        update_action.append(ProjectSurveyModel.answers.set(update_answers))
        update_points = PointsAttribute(
            satisfaction=satisfaction,
            unanswered=unanswered,
        )
        update_action.append(ProjectSurveyModel.points.set(update_points))
        update_action.append(
            ProjectSurveyModel.actual_survey_response_datetime.set(
                get_datetime_now().strftime("%Y/%m/%d %H:%M")
            )
        )
        update_action.append(ProjectSurveyModel.is_finished.set(item.is_finished))
        update_action.append(ProjectSurveyModel.is_disclosure.set(item.is_disclosure))

        # クイックアンケートまたはPPアンケート以外は連続未回答数の減算処理
        if survey.survey_type not in [SurveyType.QUICK, SurveyType.PP]:
            SurveyService.decreased_unanswered_surveys_number(survey)
            update_action.append(ProjectSurveyModel.unanswered_surveys_number.set(0))

        survey.update(actions=update_action)

        # PPアンケート以外はメール・お知らせを通知
        if survey.survey_type != SurveyType.PP:
            # メール・お知らせ通知
            bcc_address_list: List[str] = []
            fo_notification_id_list: List[str] = []
            bo_notification_id_list: List[str] = []
            # 案件情報
            try:
                project: ProjectModel = ProjectModel.get(
                    hash_key=survey.project_id, range_key=DataType.PROJECT
                )
            except DoesNotExist:
                logger.warning(
                    f"UpdateSurveyById survey.project_id not found. survey.project_id:{survey.project_id}"
                )
                project = None

            # 宛先設定
            # 営業担当責任者（全員）、システム管理者（全員）、アンケート事務局スタッフ（全員）、事業者責任者（全員）
            filter_condition = (
                AdminModel.roles.contains(UserRoleType.SYSTEM_ADMIN.key)
                | AdminModel.roles.contains(UserRoleType.SURVEY_OPS.key)
                | AdminModel.roles.contains(UserRoleType.SALES_MGR.key)
                | AdminModel.roles.contains(UserRoleType.BUSINESS_MGR.key)
            )
            admin_iter = AdminModel.scan(filter_condition=filter_condition)
            for admin in admin_iter:
                if not admin.disabled:
                    bcc_address_list.append(admin.email)
                    bo_notification_id_list.append(admin.id)

            # 支援者責任者（ユーザー権限マトリクス表に準ずる）
            # ・ユーザー権限マトリクス表より、所属していない非公開案件のアンケートは通知しない
            # ・案件にアサインされている支援者の組織IDに含まれていない支援者責任者には通知しない
            user_filter_condition = UserModel.disabled == False  # NOQA
            supporter_mgr_user_iter = UserModel.role_index.query(
                hash_key=UserRoleType.SUPPORTER_MGR.key,
                filter_condition=user_filter_condition,
            )
            for user in supporter_mgr_user_iter:
                if SurveyService.is_visible_survey_by_supporter_mgr_for_survey_answer_provided(
                    user=user,
                    survey=survey,
                    project=project,
                ):
                    bcc_address_list.append(user.email)
                    fo_notification_id_list.append(user.id)

            assign_user_id_list: List[str] = []
            # 案件にアサインされた営業担当者
            if survey.sales_user_id:
                assign_user_id_list.append(survey.sales_user_id)

            if survey.is_disclosure:
                # 開示OKの場合
                # 案件にアサインされた支援者
                if survey.main_supporter_user:
                    assign_user_id_list.append(survey.main_supporter_user.id)

                if survey.is_updated_evaluation_supporters:
                    if survey.supporter_users_before_update:
                        # アンケート評価対象者（アクセラレータ）が更新されている場合、更新前のアクセラレータを使用
                        for supporter in survey.supporter_users_before_update:
                            assign_user_id_list.append(supporter.id)
                else:
                    if survey.supporter_users:
                        for supporter in survey.supporter_users:
                            assign_user_id_list.append(supporter.id)

            item_keys = [(id, DataType.USER) for id in assign_user_id_list]
            for user in UserModel.batch_get(item_keys):
                if user.role in [
                    UserRoleType.SALES.key,
                    UserRoleType.SALES_MGR.key,
                    UserRoleType.SUPPORTER.key,
                    UserRoleType.SUPPORTER_MGR.key,
                ]:
                    if not user.disabled:
                        bcc_address_list.append(user.email)
                        fo_notification_id_list.append(user.id)

            # 本文の編集
            # URL
            fo_site_url = get_app_settings().fo_site_url
            fo_survey_detail_url = fo_site_url + FoAppUrl.SURVEY_DETAIL.format(
                surveyId=survey.id
            )
            bo_site_url = get_app_settings().bo_site_url
            bo_survey_detail_url = bo_site_url + BoAppUrl.SURVEY_ANSWER_RESULT.format(
                surveyId=survey.id
            )
            # 回答内容
            survey_answer_info_list: List[dict] = []
            survey_master = SurveyMasterModel.get(
                hash_key=survey.survey_master_id,
                range_key=survey.survey_master_revision,
            )
            for answer in survey.answers:
                answer_info: dict = {}
                description = ""
                for question in survey_master.questions:
                    if question.id == answer.id:
                        description = question.description
                        break
                answer_info["description"] = description
                answer_info["answer"] = answer.answer
                answer_info["other_input"] = (
                    answer.other_input if answer.other_input else ""
                )
                survey_answer_info_list.append(answer_info)

            customer_name_project_name = (
                (survey.customer_name or "") + "／" + (survey.project_name or "")
            )
            survey_month_str = "{year}年{month}月".format(
                year=survey.summary_month[0:4],
                month=survey.summary_month[5:],
            )
            survey_type_name = ""
            if survey.survey_type == SurveyType.SERVICE:
                survey_type_name = SurveyTypeName.SERVICE
            elif survey.survey_type == SurveyType.COMPLETION:
                survey_type_name = SurveyTypeName.COMPLETION
            elif survey.survey_type == SurveyType.QUICK:
                survey_type_name = SurveyTypeName.QUICK

            disclosure_display = "可" if survey.is_disclosure else "不可"

            # プロデューサー、アクセラレータ、営業担当、サービス責任者の名前
            main_supporter_user_name: str = ""
            supporter_users_names: List[str] = []
            sales_user_name: str = ""
            if survey.main_supporter_user:
                main_supporter_user_name = survey.main_supporter_user.name

            if survey.is_updated_evaluation_supporters:
                if survey.supporter_users_before_update:
                    # アンケート評価対象者（アクセラレータ）が更新されている場合、更新前のアクセラレータを使用
                    supporter_users_names = [
                        supporter_user.name
                        for supporter_user in survey.supporter_users_before_update
                    ]
            else:
                if survey.supporter_users:
                    supporter_users_names = [
                        supporter_user.name for supporter_user in survey.supporter_users
                    ]

            project: ProjectModel = ProjectModel.get(
                hash_key=survey.project_id, range_key=DataType.PROJECT
            )

            service_manager_name: str = ""
            if project.service_manager_name:
                service_manager_name = project.service_manager_name

            # [sales_user_id]から営業担当をDBから取得
            user: UserModel = UserModel.get(
                hash_key=survey.sales_user_id, range_key=DataType.USER
            )
            sales_user_name = user.name

            payload: dict = {
                "customer_name_project_name": customer_name_project_name,
                "fo_survey_detail_url": fo_survey_detail_url,
                "bo_survey_detail_url": bo_survey_detail_url,
                "survey_month": survey_month_str,
                "survey_type_name": survey_type_name,
                "disclosure": disclosure_display,
                "survey_answer_info_list": survey_answer_info_list,
                "service_manager_name": service_manager_name,
                "main_supporter_user_name": main_supporter_user_name,
                "supporter_users_names": supporter_users_names,
                "sales_user_name": sales_user_name,
            }
            # メール送信
            bcc_address_list = list(set(bcc_address_list))
            ProjectService.send_mail(
                template=MailType.SURVEY_ANSWER_PROVIDED,
                to_addr_list=[],
                cc_addr_list=[],
                bcc_addr_list=bcc_address_list,
                payload=payload,
            )

            # お知らせ通知
            notification_noticed_at = datetime.now()
            fo_notification_id_list = list(set(fo_notification_id_list))
            bo_notification_id_list = list(set(bo_notification_id_list))
            # 案件に設定されたアンケート送信先のお客様が回答した場合、create_id & update_id は更新しない
            if survey.dedicated_survey_user_email:
                # FO
                NotificationService.save_notification(
                    notification_type=NotificationType.SURVEY_ANSWER_PROVIDED,
                    user_id_list=fo_notification_id_list,
                    message_param=payload,
                    url=payload.get("fo_survey_detail_url"),
                    noticed_at=notification_noticed_at,
                    create_id="",
                    update_id="",
                )
                # BO
                NotificationService.save_notification(
                    notification_type=NotificationType.SURVEY_ANSWER_PROVIDED,
                    user_id_list=bo_notification_id_list,
                    message_param=payload,
                    url=payload.get("bo_survey_detail_url"),
                    noticed_at=notification_noticed_at,
                    create_id="",
                    update_id="",
                )
            else:
                # FO
                NotificationService.save_notification(
                    notification_type=NotificationType.SURVEY_ANSWER_PROVIDED,
                    user_id_list=fo_notification_id_list,
                    message_param=payload,
                    url=payload.get("fo_survey_detail_url"),
                    noticed_at=notification_noticed_at,
                    create_id=survey.customer_id,
                    update_id=survey.customer_id,
                )
                # BO
                NotificationService.save_notification(
                    notification_type=NotificationType.SURVEY_ANSWER_PROVIDED,
                    user_id_list=bo_notification_id_list,
                    message_param=payload,
                    url=payload.get("bo_survey_detail_url"),
                    noticed_at=notification_noticed_at,
                    create_id=survey.customer_id,
                    update_id=survey.customer_id,
                )

        return OKResponse()

    @staticmethod
    def get_survey_summary_by_mine(
        query_params: GetSurveySummaryByMineQuery, current_user: UserModel
    ):
        data_type = f"{DataType.USER}#{current_user.id}"

        ##########################
        # クエリ条件組み立て
        ##########################

        logger.info("start creating query condition")

        if query_params.year_month_from:
            str_year_month_from = datetime.strptime(
                str(query_params.year_month_from), "%Y%m"
            ).strftime("%Y/%m")
        else:
            str_year_month_from = DateTime.MINIMUM

        if query_params.year_month_to:
            str_year_month_to = datetime.strptime(
                str(query_params.year_month_to), "%Y%m"
            ).strftime("%Y/%m")
        else:
            str_year_month_to = DateTime.MAXIMUM

        survey_summaries: List[SurveySummaryUserModel] = (
            SurveySummaryUserModel.data_type_summary_month_index.query(
                hash_key=data_type,
                range_key_condition=SurveySummaryUserModel.summary_month.between(
                    str_year_month_from, str_year_month_to
                ),
            )
        )
        service_satisfaction_summary = 0
        service_satisfaction_unanswered = 0
        service_receive = 0
        completion_satisfaction_summary = 0
        completion_satisfaction_unanswered = 0
        quick_receive = 0
        completion_continuation_positive = 0
        completion_continuation_unanswered = 0
        completion_sales_summary = 0
        completion_sales_unanswered = 0
        completion_receive = 0
        completion_recommended_summary = 0
        completion_recommended_unanswered = 0

        surveys: List[Surveys] = []

        for survey_summary in survey_summaries:
            #########################################
            # 期間中の値の算出
            #########################################
            service_satisfaction_summary += survey_summary.service_satisfaction_summary
            service_satisfaction_unanswered += (
                survey_summary.service_satisfaction_unanswered
            )
            service_receive += survey_summary.service_receive
            completion_satisfaction_summary += (
                survey_summary.completion_satisfaction_summary
            )
            completion_satisfaction_unanswered += (
                survey_summary.completion_satisfaction_unanswered
            )
            quick_receive += survey_summary.quick_receive
            completion_continuation_positive += (
                survey_summary.completion_continuation.positive_count
            )
            completion_continuation_unanswered += (
                survey_summary.completion_continuation_unanswered
            )
            completion_sales_summary += survey_summary.completion_sales_summary
            completion_sales_unanswered += survey_summary.completion_sales_unanswered
            completion_receive += survey_summary.completion_receive
            completion_recommended_summary += (
                survey_summary.completion_recommended_summary
            )
            completion_recommended_unanswered += (
                survey_summary.completion_recommended_unanswered
            )

            ########################################
            # 月別集計
            ########################################
            positive_percent = (
                SurveyService.divide(
                    survey_summary.completion_continuation.positive_count,
                    survey_summary.completion_receive
                    - survey_summary.completion_continuation_unanswered,
                )
                * 100
            )

            survey_summary.attribute_values["completion_continuation"] = (
                CompletionContinuation(
                    positive_count=survey_summary.completion_continuation.positive_count,
                    negative_count=survey_summary.completion_continuation.negative_count,
                    positive_percent=round_off(positive_percent),
                )
            )

            surveys.append(
                Surveys(
                    **survey_summary.attribute_values,
                )
            )

        #########################################
        # 期間集計結果
        #########################################
        service_satisfaction_average = SurveyService.divide(
            service_satisfaction_summary,
            service_receive - service_satisfaction_unanswered,
        )

        completion_satisfaction_average = SurveyService.divide(
            completion_satisfaction_summary,
            completion_receive - completion_satisfaction_unanswered,
        )

        completion_positive_percent = (
            SurveyService.divide(
                completion_continuation_positive,
                completion_receive - completion_continuation_unanswered,
            )
            * 100
        )
        completion_position_percent_str = round_off(completion_positive_percent)

        completion_sales_average = SurveyService.divide(
            completion_sales_summary, completion_receive - completion_sales_unanswered
        )
        completion_recommended_average = SurveyService.divide(
            completion_recommended_summary,
            completion_receive - completion_recommended_unanswered,
        )
        service_and_completion_receive = service_receive + completion_receive
        service_and_completion_unanswered = (
            service_satisfaction_unanswered + completion_satisfaction_unanswered
        )
        service_and_completion_satisfaction_average = SurveyService.divide(
            (service_satisfaction_summary + completion_satisfaction_summary),
            (service_and_completion_receive - service_and_completion_unanswered),
        )

        return GetSurveySummaryByMineResponse(
            term_summary_result=TermSummaryResult(
                service=Service(
                    satisfaction_average=round_off(service_satisfaction_average, 0.1),
                    total_receive=service_receive,
                ),
                completion=Completion(
                    satisfaction_average=round_off(
                        completion_satisfaction_average, 0.1
                    ),
                    continuation_positive_percent=completion_position_percent_str,
                    recommended_average=round_off(completion_recommended_average, 0.1),
                    sales_average=round_off(completion_sales_average, 0.1),
                    total_receive=completion_receive,
                ),
                service_and_completion=ServiceAndCompletion(
                    satisfaction_average=round_off(
                        service_and_completion_satisfaction_average, 0.1
                    ),
                    total_receive=service_and_completion_receive,
                ),
                quick=Quick(total_receive=quick_receive),
            ),
            surveys=surveys,
        )

    @staticmethod
    def get_survey_summary_supporter_organizations_by_mine(
        query_params: GetSurveySummarySupporterOrganizationByMineQuery,
        current_user: UserModel,
    ) -> GetSurveySummarySupporterOrganizationByMineResponse:
        """課別のアンケート集計結果を取得する

        Args:
            query_params (GetSurveySummarySupporterOrganizationByMineQuery): クエリパラメータ
            current_user (UserModel): 認証済みユーザー

        Returns:
            GetSurveySummarySupporterOrganizationByMineResponse: 取得結果
        """
        # 絞り込み期間整形
        if query_params.year_month_from:
            str_year_month_from = datetime.strptime(
                str(query_params.year_month_from), "%Y%m"
            ).strftime("%Y/%m")
        else:
            str_year_month_from = DateTime.MINIMUM

        if query_params.year_month_to:
            str_year_month_to = datetime.strptime(
                str(query_params.year_month_to), "%Y%m"
            ).strftime("%Y/%m")
        else:
            str_year_month_to = DateTime.MAXIMUM

        monthly_survey_organization: List[
            SurveysForMonthlyResultsBySupporterOrganization
        ] = []
        monthly_surveys = {}
        term_summary_result_list: List[TermSummaryResultForSupporterOrganizations] = []

        filter_condition = None

        if current_user.role in [
            UserRoleType.SUPPORTER.key,
            UserRoleType.SUPPORTER_MGR.key,
        ]:
            # 支援者・支援者責任者の場合は所属課の集計結果を取得
            if current_user.supporter_organization_id:
                filter_condition = MasterSupporterOrganizationModel.id.is_in(
                    *current_user.supporter_organization_id
                )

        master_supporter_organization_list: List[MasterSupporterOrganizationModel] = (
            list(
                MasterSupporterOrganizationModel.data_type_index.query(
                    hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                    filter_condition=filter_condition,
                )
            )
        )

        # すべての課の集計用
        completion_satisfaction_summary = 0
        completion_satisfaction_unanswered = 0
        service_satisfaction_summary = 0
        service_satisfaction_unanswered = 0
        service_total_receive = 0
        completion_total_receive = 0
        completion_continuation_positive_count = 0
        completion_continuation_unanswered = 0
        completion_recommended_summary = 0
        completion_recommended_unanswered = 0
        total_satisfaction_summary = 0
        total_satisfaction_unanswered = 0
        total_receive = 0

        for supporter_organization in master_supporter_organization_list:
            survey_summary_list: List[SurveySummarySupporterOrganizationModel] = list(
                SurveySummarySupporterOrganizationModel.data_type_summary_month_index.query(
                    hash_key=f"supporter_organization#{supporter_organization.id}",
                    range_key_condition=SurveySummarySupporterOrganizationModel.summary_month.between(
                        str_year_month_from, str_year_month_to
                    ),
                )
            )

            term_summary_result = TermSummaryResultForSupporterOrganizations(
                supporter_organization_id=supporter_organization.id,
                supporter_organization_name=supporter_organization.value,
                completion_continuation=CompletionContinuation(),
            )
            for survey_summary in survey_summary_list:
                term_summary_result.service_satisfaction_summary += (
                    survey_summary.service_satisfaction_summary
                )
                term_summary_result.service_satisfaction_unanswered += (
                    survey_summary.service_satisfaction_unanswered
                )
                term_summary_result.service_receive += survey_summary.service_receive
                term_summary_result.completion_satisfaction_summary += (
                    survey_summary.completion_satisfaction_summary
                )
                term_summary_result.completion_satisfaction_unanswered += (
                    survey_summary.completion_satisfaction_unanswered
                )
                term_summary_result.completion_continuation.positive_count += (
                    survey_summary.completion_continuation.positive_count
                )
                term_summary_result.completion_continuation.negative_count += (
                    survey_summary.completion_continuation.negative_count
                )
                term_summary_result.completion_continuation_unanswered += (
                    survey_summary.completion_continuation_unanswered
                )
                term_summary_result.completion_recommended_summary += (
                    survey_summary.completion_recommended_summary
                )
                term_summary_result.completion_recommended_unanswered += (
                    survey_summary.completion_recommended_unanswered
                )
                term_summary_result.completion_receive += (
                    survey_summary.completion_receive
                )
                term_summary_result.total_satisfaction_summary += (
                    survey_summary.total_satisfaction_summary
                )
                term_summary_result.total_satisfaction_unanswered += (
                    survey_summary.service_satisfaction_unanswered
                    + survey_summary.completion_satisfaction_unanswered
                )
                term_summary_result.total_receive += survey_summary.total_receive

                completion_continuation_percent = (
                    survey_summary.completion_continuation_percent * 100
                )

                # 集計月レスポンスの生成
                monthly_surveys.setdefault(survey_summary.summary_month, [])
                monthly_surveys[survey_summary.summary_month].append(
                    MonthlyResultsBySupporterOrganization(
                        supporter_organization_name=supporter_organization.value,
                        service_satisfaction_average=survey_summary.service_satisfaction_average,
                        service_receive=survey_summary.service_receive,
                        completion_satisfaction_average=survey_summary.completion_satisfaction_average,
                        completion_continuation_percent=completion_continuation_percent,
                        completion_recommended_average=survey_summary.completion_recommended_average,
                        completion_receive=survey_summary.completion_receive,
                        total_satisfaction_average=survey_summary.total_satisfaction_average,
                        total_receive=survey_summary.total_receive,
                    )
                )

            else:
                # 対象のレコードがある場合のみ平均を算出
                if survey_summary_list:
                    # 各種平均算出
                    SurveyService.calculate_survey_average(term_summary_result)
                    term_summary_result_list.append(term_summary_result)

                    # すべての課の合計・平均算出用の数値
                    service_satisfaction_summary += (
                        term_summary_result.service_satisfaction_summary
                    )
                    service_satisfaction_unanswered += (
                        term_summary_result.service_satisfaction_unanswered
                    )
                    completion_satisfaction_summary += (
                        term_summary_result.completion_satisfaction_summary
                    )
                    completion_satisfaction_unanswered += (
                        term_summary_result.completion_satisfaction_unanswered
                    )
                    total_satisfaction_summary += (
                        term_summary_result.total_satisfaction_summary
                    )
                    total_satisfaction_unanswered += (
                        term_summary_result.total_satisfaction_unanswered
                    )
                    service_total_receive += term_summary_result.service_receive
                    completion_total_receive += term_summary_result.completion_receive
                    total_receive += term_summary_result.total_receive

                    completion_continuation_positive_count += (
                        term_summary_result.completion_continuation.positive_count
                    )
                    completion_continuation_unanswered += (
                        term_summary_result.completion_continuation_unanswered
                    )
                    completion_recommended_summary += (
                        term_summary_result.completion_recommended_summary
                    )
                    completion_recommended_unanswered += (
                        term_summary_result.completion_recommended_unanswered
                    )

        total_service_satisfaction_average = 0
        total_completion_satisfaction_average = 0
        total_satisfaction_average = 0
        completion_continuation_percent = 0
        completion_recommended_average = 0

        # すべての課の合計・平均
        if (service_total_receive - service_satisfaction_unanswered) != 0:
            # サービスアンケート総合満足度
            total_service_satisfaction_average = round_off(
                service_satisfaction_summary
                / (service_total_receive - service_satisfaction_unanswered),
                0.1,
            )

        if (completion_total_receive - completion_satisfaction_unanswered) != 0:
            # 修了アンケート総合満足度
            total_completion_satisfaction_average = round_off(
                completion_satisfaction_summary
                / (completion_total_receive - completion_satisfaction_unanswered),
                0.1,
            )

        if (total_receive - total_satisfaction_unanswered) != 0:
            # 総合満足度評価
            total_satisfaction_average = round_off(
                total_satisfaction_summary
                / (total_receive - total_satisfaction_unanswered),
                0.1,
            )

        if (completion_total_receive - completion_continuation_unanswered) != 0:
            # 修了アンケート継続意向
            completion_continuation_percent = round_off(
                completion_continuation_positive_count
                / (completion_total_receive - completion_continuation_unanswered)
                * 100,
                0,
            )

        if (completion_total_receive - completion_recommended_unanswered) != 0:
            # 修了アンケート紹介可能性
            completion_recommended_average = round_off(
                completion_recommended_summary
                / (completion_total_receive - completion_recommended_unanswered),
                0.1,
            )

        for (
            summary_month,
            monthly_survey_summary,
        ) in monthly_surveys.items():
            monthly_survey_organization.append(
                SurveysForMonthlyResultsBySupporterOrganization(
                    summary_month=summary_month,
                    supporter_organizations=monthly_survey_summary,
                )
            )

        # ソート
        # 課名の昇順（文字コード順）
        term_summary_result_list.sort(key=lambda x: x.supporter_organization_name)
        for monthly_item in monthly_survey_organization:
            monthly_item.supporter_organizations.sort(
                key=lambda x: x.supporter_organization_name
            )

        return GetSurveySummarySupporterOrganizationByMineResponse(
            total_summary_result=totalSummaryResultForSupporterOrganizations(
                service_satisfaction_average=total_service_satisfaction_average,
                service_receive=service_total_receive,
                completion_satisfaction_average=total_completion_satisfaction_average,
                completion_continuation_percent=completion_continuation_percent,
                completion_recommended_average=completion_recommended_average,
                completion_receive=completion_total_receive,
                total_satisfaction_average=total_satisfaction_average,
                total_receive=total_receive,
            ),
            term_summary_result=term_summary_result_list,
            surveys=monthly_survey_organization,
        )

    @staticmethod
    def calculate_survey_average(
        term_summary_result: TermSummaryResultForSupporterOrganizations,
    ):
        """各種アンケートサマリの平均の算出

        Args:
            term_summary_result (TermSummaryResultForSupporterOrganizations): 集計月ごとの項目
        """

        if (
            term_summary_result.service_receive
            - term_summary_result.service_satisfaction_unanswered
        ) != 0:
            term_summary_result.service_satisfaction_average = round_off(
                term_summary_result.service_satisfaction_summary
                / (
                    term_summary_result.service_receive
                    - term_summary_result.service_satisfaction_unanswered
                ),
                0.1,
            )
        if (
            term_summary_result.completion_receive
            - term_summary_result.completion_satisfaction_unanswered
        ) != 0:
            term_summary_result.completion_satisfaction_average = round_off(
                term_summary_result.completion_satisfaction_summary
                / (
                    term_summary_result.completion_receive
                    - term_summary_result.completion_satisfaction_unanswered
                ),
                0.1,
            )
        if (
            term_summary_result.completion_receive
            - term_summary_result.completion_recommended_unanswered
        ) != 0:
            term_summary_result.completion_recommended_average = round_off(
                term_summary_result.completion_recommended_summary
                / (
                    term_summary_result.completion_receive
                    - term_summary_result.completion_recommended_unanswered
                ),
                0.1,
            )
        if (
            term_summary_result.total_receive
            - term_summary_result.total_satisfaction_unanswered
        ) != 0:
            term_summary_result.total_satisfaction_average = round_off(
                term_summary_result.total_satisfaction_summary
                / (
                    term_summary_result.total_receive
                    - term_summary_result.total_satisfaction_unanswered
                ),
                0.1,
            )

        if (
            term_summary_result.completion_receive
            - term_summary_result.completion_continuation_unanswered
        ) != 0:
            # 継続可能性のみ％形式
            term_summary_result.completion_continuation.positive_percent = round_off(
                (
                    term_summary_result.completion_continuation.positive_count
                    / (
                        term_summary_result.completion_receive
                        - term_summary_result.completion_continuation_unanswered
                    )
                )
                * 100,
            )

    @staticmethod
    def divide(dividend, divisor):
        try:
            quotient = dividend / divisor
        except ZeroDivisionError:
            quotient = 0

        return quotient

    @staticmethod
    def decreased_unanswered_surveys_number(survey: ProjectSurveyModel):
        """
        連続未回答数を減算する
        """
        # Modelのqueryでflake8(E712)エラーが出るため定義
        is_finished: bool = False
        unanswered_filter_condition = None
        unanswered_filter_condition &= (
            ProjectSurveyModel.actual_survey_request_datetime.exists()
        )
        unanswered_filter_condition &= ProjectSurveyModel.is_finished == is_finished
        unanswered_filter_condition &= (
            ProjectSurveyModel.survey_type != SurveyType.QUICK
        )
        # 同じ案件の回答アンケートの下記の条件の案件アンケートをすべて取得
        # ・送信アンケートの同月以降
        # ・未回答のアンケート
        unanswered_survey_list: List[ProjectSurveyModel] = list(
            ProjectSurveyModel.project_id_summary_month_index.query(
                hash_key=survey.project_id,
                range_key_condition=ProjectSurveyModel.summary_month
                >= survey.summary_month,
                filter_condition=unanswered_filter_condition,
            )
        )

        # 連続未回答数が大きい順に並び替え
        # 取得した際のDBのアンケートの並びがバラバラで連続未回答数の減算にずれが生じるため
        sorted_unanswered_survey_list = sorted(
            unanswered_survey_list, key=lambda x: x.unanswered_surveys_number
        )

        for unanswered_survey in sorted_unanswered_survey_list:
            # 以下の場合、処理をスキップ
            # ・同じアンケートid または 送信日実績日が[古いまたは同じ]
            # 且つ
            # ・連続未回答数が回答したアンケートの連続未回答数より[小さいまたは同じ]
            if (
                (
                    unanswered_survey.id == survey.id
                    or unanswered_survey.actual_survey_request_datetime
                    <= survey.actual_survey_request_datetime
                )
                and unanswered_survey.unanswered_surveys_number
                <= survey.unanswered_surveys_number
            ):
                continue

            # 新しい送信実績日で連続未回答数が回答アンケートの連続未回答数より[小さいまたは同じ]の場合は処理を抜ける
            if (
                (
                    unanswered_survey.actual_survey_request_datetime
                    > survey.actual_survey_request_datetime
                )
                and unanswered_survey.unanswered_surveys_number
                <= survey.unanswered_surveys_number
            ):
                break

            # 取得した各案件アンケートの連続未回答数を回答したアンケートの連続未回答数で減算
            unanswered_survey.unanswered_surveys_number -= (
                survey.unanswered_surveys_number
            )
            unanswered_survey.save()
