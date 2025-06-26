import copy
import itertools
import os
import re
from datetime import date, datetime, timedelta
from typing import Dict, List, Union

import numpy as np
import pandas as pd
from dateutil.relativedelta import relativedelta
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
from app.models.project_karte import ProjectKarteModel
from app.models.project_survey import ProjectSurveyModel, SupporterUserAttribute
from app.models.survey_master import SurveyMasterModel
from app.models.survey_summary import (
    SurveySummaryAllModel,
    SurveySummarySupporterOrganizationModel,
)
from app.models.user import UserModel
from app.resources.const import (
    CsvFormatName,
    DataType,
    Date,
    DateTime,
    DateTimeHourMinutes,
    ExportSurveysModeType,
    FoAppUrl,
    HourMinutes,
    JwtSettingInfo,
    MailType,
    MasterDataType,
    NotificationType,
    S3PresignedExpire,
    SendSurveyTypeName,
    SurveyAnswerSummaryType,
    SolverIdentifier,
    SurveyCsvAttributeOfOrganization,
    SurveyCsvAttributeOfRaw,
    SurveyCsvAttributeOfSupporter,
    SurveyQuestionsSummaryType,
    SurveySummaryType,
    SurveyType,
    SurveyTypeForGetSurveys,
    SurveyTypeName,
    SurveyUserType,
    UserRoleType,
)
from app.schemas.base import OKResponse
from app.schemas.survey import (
    Answer,
    CompletionContinuation,
    CompletionSummaryResult,
    ExportSurveysQuery,
    ExportSurveysResponse,
    GetSurveyByIdResponse,
    GetSurveyPlansQuery,
    GetSurveyPlansResponse,
    GetSurveysQuery,
    GetSurveysResponse,
    GetSurveySummaryAllQuery,
    GetSurveySummaryAllResponse,
    GetSurveySummaryReportsQuery,
    GetSurveySummaryReportsResponse,
    GetSurveySummarySupporterOrganizationsQuery,
    GetSurveySummarySupporterOrganizationsResponse,
    MonthlyResultsBySupporterOrganization,
    MonthlySurveySummaryAllResults,
    PPSummaryResult,
    QuickSummaryResult,
    SummaryInfoForGetSurveysResponse,
    SummaryResult,
    SupporterUser,
    SurveyInfoForGetSurveyPlansResponse,
    SurveyInfoForGetSurveysResponse,
    SurveysForMonthlyResultsBySupporterOrganization,
    SurveySummaryReports,
    TermSummaryResultForSummaryAll,
    TermSummaryResultForSupporterOrganizations,
    UpdateSurveyByIdRequest,
    monthlySurveySummaryForGetSurveysResponse,
    surveySummaryForGetSurveysResponse,
    surveySummaryHalfTotalForGetSurveysResponse,
    surveySummaryQuotaTotalForGetSurveysResponse,
    surveySummaryTotalForGetSurveysResponse,
    totalSummaryResultForSupporterOrganizations,
)
from app.service.common_service.cached_db_items import CachedDbItems
from app.service.notification_service import NotificationService
from app.service.project_service import ProjectService
from app.utils.aws.s3 import S3Helper
from app.utils.cipher_aes import AesCipherUtils
from app.utils.date import get_datetime_now, get_day_of_week
from app.utils.encryption import create_jwt, create_jwt_survey_payload
from app.utils.format import round_off

logger = CustomLogger.get_logger()


class SurveyService:
    @staticmethod
    def get_survey_master_name_from_cache(survey_master_id: str) -> Union[str, None]:
        """アンケートマスタIDからアンケートマスタ名を取得.
            survey_master_nameが解決できない場合、Noneを返却.

        Args:
            survey_master_id (str): アンケートマスタID

        Raise:

        Returns:
            str: 案件名
        """
        if not survey_master_id:
            return None

        survey_master_name = None

        # サービスタイプ区分の一覧を取得
        survey_masters = CachedDbItems.get_survey_masters()

        # 一覧に指定されたサービスタイプIDが含まれているか検索
        for survey_master in survey_masters:
            if survey_master_id == survey_master["id"]:
                survey_master_name = survey_master["name"]
                break

        return survey_master_name

    @staticmethod
    def get_survey_master_questions_number(
        survey_master_id: str, survey_master_revision: int
    ) -> int:
        """アンケートマスタIDとリビジョンから設定されている設問数を取得.
            該当のアンケートマスタがない場合は0を返却.

        Args:
            survey_master_id (str): アンケートマスタID
            survey_master_revision (int): アンケートマスタバージョン

        Raise:

        Returns:
            questions_num: 設問数
        """
        # サービスタイプ区分の一覧を取得
        survey_masters = CachedDbItems.get_survey_masters()

        questions_num: int = 0

        # 一覧に指定されたサービスタイプIDが含まれているか検索
        for survey_master in survey_masters:
            if (
                survey_master_id == survey_master["id"]
                and survey_master_revision == survey_master["revision"]
            ):
                questions_num = len(survey_master["questions"])
                break

        return questions_num

    @staticmethod
    def get_service_name(service_type: str) -> Union[str, None]:
        """汎用マスタからサービス区分名を取得.
            取得できない場合は400エラーを発行.
            service_typeがNoneまたは空の場合、Noneを返却.

        Args:
            service_type (str): サービス区分

        Raise:
            HTTP_400_BAD_REQUEST

        Returns:
            str: サービス区分名
        """
        if not service_type:
            return None

        service_type_name = None

        # サービスタイプ区分の一覧を取得
        service_types = CachedDbItems.ReturnServiceTypes()

        # 一覧に指定されたサービスタイプIDが含まれているか検索
        for current_service_type in service_types:
            if service_type == current_service_type["id"]:
                service_type_name = current_service_type["name"]
                break

        return service_type_name

    @staticmethod
    def get_accelerator_display_names(
        accelerators: List[SupporterUserAttribute],
    ) -> str:
        """アクセラレーター情報から支援者組織名と支援者名を連結して返却.
            セミコロン(;)繋ぎの支援者組織名をアンダースコア(_)へ変換.
            連結できない場合はNoneを発行.

        Args:
            accelerators (List[SupporterUserAttribute]): アクセラレーター情報

        Raise:

        Returns:
            str: 支援者組織名_支援者名(支援者責任者の場合は支援者組織名が複数の場合あり)
        """
        if not accelerators:
            return None

        display_names: str = ""

        for accelerator in accelerators:
            supporter_organization_names: str = ""

            if accelerator.organization_name.count(";") > 0:
                accelerator.organization_name += ";"
                supporter_organization_names = accelerator.organization_name.replace(
                    ";", "_"
                )
            else:
                supporter_organization_names = accelerator.organization_name + "_"

            display_names += supporter_organization_names + accelerator.name + ";"

        # 末尾からセミコロンを削除
        display_names = display_names.rstrip(";")

        return display_names

    @staticmethod
    def get_user_name(
        user_id: List[str],
    ) -> str:
        """一般ユーザーからidを条件にnameを取得.
            取得できない場合はNoneを発行.
            idがNoneまたは空の場合、Noneを返却.

        Args:
            admin_user_ids (List[str]): 一般ユーザーID

        Raise:

        Returns:
            str: 一般ユーザー名
        """
        if not user_id:
            return None

        user_name: str = None

        # 一般ユーザーの一覧を取得
        users = CachedDbItems.get_users()

        # 一覧に指定された一般ユーザーIDが含まれているか検索
        for user in users:
            if user_id == user["id"]:
                user_name = user["name"]
                break

        return user_name

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
    def conversion_answer_summary_type_to_japanese(summary_type: str) -> str:
        """回答集計タイプを英語から日本語へ変換する

        Args:
            summary_type (str): 英語の回答集計タイプ
        Returns:
            summary_type_jp (str): 日本語の回答集計タイプ
        """

        summary_type_jp = None

        if summary_type is not None:
            summary_type_jp = getattr(SurveyAnswerSummaryType, summary_type.upper())

        return summary_type_jp

    @staticmethod
    def edit_supporter_user_schema(
        supporter_user_id: str,
        user_list: List[UserModel],
        master_supporter_organization_list: List[MasterSupporterOrganizationModel],
    ) -> SupporterUser:
        """
            SupporterUserの編集

        Args:
            supporter_user_id (str): 支援者ユーザID
            user_list (List[UserModel]): 一般ユーザのリスト
            master_supporter_organization_list (List[MasterSupporterOrganizationModel]):
                汎用マスターの支援者組織のリスト
        Returns
            SupporterUser: 編集したAttribute
        """
        supporter_user: SupporterUser = None
        for user in user_list:
            if supporter_user_id == user.id:
                temp_id = []
                temp_name = []
                if user.supporter_organization_id:
                    for organization_id in user.supporter_organization_id:
                        for (
                            supporter_organization
                        ) in master_supporter_organization_list:
                            if organization_id == supporter_organization.id:
                                temp_id.append(supporter_organization.id)
                                temp_name.append(supporter_organization.value)
                                break
                supporter_organization_id = ";".join(temp_id)
                supporter_organization_name = ";".join(temp_name)

                supporter_user = SupporterUser(
                    id=supporter_user_id,
                    name=user.name,
                    organization_id=supporter_organization_id,
                    organization_name=supporter_organization_name,
                )
                break
        return supporter_user

    @staticmethod
    def edit_supporter_user_attribute(
        supporter_user_id: str,
        user_list: List[UserModel],
        master_supporter_organization_list: List[MasterSupporterOrganizationModel],
    ) -> SupporterUserAttribute:
        """
            SupporterUserAttributeの編集

        Args:
            supporter_user_id (str): 支援者ユーザID
            user_list (List[UserModel]): 一般ユーザのリスト
            master_supporter_organization_list (List[MasterSupporterOrganizationModel]):
                汎用マスターの支援者組織のリスト
        Returns
            SupporterUserAttribute: 編集したAttribute
        """
        supporter_user: SupporterUserAttribute = None
        for user in user_list:
            if supporter_user_id == user.id:
                temp_id = []
                temp_name = []
                if user.supporter_organization_id:
                    for organization_id in user.supporter_organization_id:
                        for (
                            supporter_organization
                        ) in master_supporter_organization_list:
                            if organization_id == supporter_organization.id:
                                temp_id.append(supporter_organization.id)
                                temp_name.append(supporter_organization.value)
                                break
                supporter_organization_id = ";".join(temp_id)
                supporter_organization_name = ";".join(temp_name)

                supporter_user = SupporterUserAttribute(
                    id=supporter_user_id,
                    name=user.name,
                    organization_id=supporter_organization_id,
                    organization_name=supporter_organization_name,
                )
                break
        return supporter_user

    @staticmethod
    def get_user_info(user_id_list: List[str]) -> List[UserModel]:
        user_model_list = []
        item_keys = [(id, DataType.USER) for id in user_id_list]
        for item in UserModel.batch_get(item_keys):
            user_model_list.append(item)
        return user_model_list

    def get_survey_by_id(survey_id: str, current_user: AdminModel):
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

        # 権限チェック
        if not SurveyService.is_visible_survey_for_get_survey_by_id(
            current_user=current_user,
            project_id=survey.project_id,
        ):
            logger.warning(
                f"GetSurveyById forbidden. survey.project_id: {survey.project_id}"
            )
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN)

        # バリデーションチェック
        # 回答依頼送信済でないアンケートは取得不可
        if not survey.actual_survey_request_datetime:
            logger.warning("GetSurveyById bad request.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Cannot get a survey for which the response request has not been sent.",
            )

        # 回答済みでないアンケートは取得不可
        if not survey.actual_survey_response_datetime:
            logger.warning("GetSurveyById bad request.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Unanswered surveys cannot be retrieved.",
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
    def export_surveys(query_params: ExportSurveysQuery, current_user: AdminModel):
        """案件アンケートを集計してCSV出力

        Args:
            query_params (ExportSurveysQuery): クエリパラメータ
            current_user (Behavior, optional): 認証済みのユーザー

        Raises:
            HTTPException: 400 Bad request
            HTTPException: 404 Not found

        Returns:
            ExportSurveysResponse: 取得結果
        """

        # ####################################
        # 共通のクエリパラメータの組み立て
        # ####################################

        # 集計月をソートキー条件へ指定
        range_key_condition = None

        # 集計月From～To指定の場合
        if query_params.summary_month_from or query_params.summary_month_to:
            # Fromのみ指定 (Toは1年後をセット)
            if query_params.summary_month_from and not query_params.summary_month_to:
                summary_month_from = datetime.strptime(
                    query_params.summary_month_from, "%Y%m"
                ).strftime("%Y/%m")
                tmp_summary_month_to = datetime.strptime(
                    query_params.summary_month_from, "%Y%m"
                ) + timedelta(days=365)
                summary_month_to = datetime.strftime(tmp_summary_month_to, "%Y/%m")
            # Toのみ指定 (Fromは1年前をセット)
            elif query_params.summary_month_to and not query_params.summary_month_from:
                summary_month_to = datetime.strptime(
                    query_params.summary_month_to, "%Y%m"
                ).strftime("%Y/%m")
                tmp_summary_month_from = datetime.strptime(
                    query_params.summary_month_to, "%Y%m"
                ) - timedelta(days=365)
                summary_month_from = datetime.strftime(tmp_summary_month_from, "%Y/%m")
            # FromとToの両方を指定
            else:
                summary_month_from = datetime.strptime(
                    query_params.summary_month_from, "%Y%m"
                ).strftime("%Y/%m")
                summary_month_to = datetime.strptime(
                    query_params.summary_month_to, "%Y%m"
                ).strftime("%Y/%m")

            range_key_condition = ProjectSurveyModel.summary_month.between(
                summary_month_from, summary_month_to
            )
            logger.info(f"summary_month_from: {summary_month_from}")
            logger.info(f"summary_month_to: {summary_month_to}")
        # 送信予定日From～To指定の場合
        elif (
            query_params.plan_survey_request_date_from
            or query_params.plan_survey_request_date_to
        ):
            # Fromのみ指定 (Toは1年後をセット)
            if (
                query_params.plan_survey_request_date_from
                and not query_params.plan_survey_request_date_to
            ):
                plan_survey_request_date_from = datetime.strptime(
                    query_params.plan_survey_request_date_from, "%Y%m"
                ).strftime("%Y/%m")
                tmp_plan_survey_request_date_to = datetime.strptime(
                    query_params.plan_survey_request_date_from, "%Y%m"
                ) + timedelta(days=365)
                plan_survey_request_date_to = datetime.strftime(
                    tmp_plan_survey_request_date_to, "%Y/%m"
                )
            # Toのみ指定 (Fromは1年前をセット)
            elif (
                query_params.plan_survey_request_date_to
                and not query_params.plan_survey_request_date_from
            ):
                plan_survey_request_date_to = datetime.strptime(
                    query_params.plan_survey_request_date_to, "%Y%m"
                ).strftime("%Y/%m")
                tmp_plan_survey_request_date_from = datetime.strptime(
                    query_params.plan_survey_request_date_to, "%Y%m"
                ) - timedelta(days=365)
                plan_survey_request_date_from = datetime.strftime(
                    tmp_plan_survey_request_date_from, "%Y/%m"
                )
            # FromとToの両方を指定
            else:
                plan_survey_request_date_from = datetime.strptime(
                    query_params.plan_survey_request_date_from, "%Y%m"
                ).strftime("%Y/%m")
                plan_survey_request_date_to = datetime.strptime(
                    query_params.plan_survey_request_date_to, "%Y%m"
                ).strftime("%Y/%m")

            range_key_condition = (
                ProjectSurveyModel.plan_survey_request_datetime.between(
                    plan_survey_request_date_from, plan_survey_request_date_to
                )
            )
            logger.info(
                f"plan_survey_request_date_from: {plan_survey_request_date_from}"
            )
            logger.info(f"plan_survey_request_date_to: {plan_survey_request_date_to}")

        # ####################################
        # アンケートデータCSV
        # ####################################
        if query_params.mode == ExportSurveysModeType.RAW.value:
            # RAW CSVでアンケートタイプが指定されていない場合は400を返却
            if not query_params.type:
                logger.warning(
                    "ExportSurveys bad request. The survey type must be specified"
                )
                raise HTTPException(
                    status_code=status.HTTP_400_BAD_REQUEST,
                    detail="For raw csv output, the survey type must be specified",
                )

            # ####################################
            # クエリパラメータの組み立て
            # ####################################

            logger.info("start creating filter condition")

            # アンケートタイプをフィルター条件へ指定
            filter_condition = ProjectSurveyModel.is_finished == "true"

            if query_params.type == SurveyTypeForGetSurveys.SERVICE:
                filter_condition &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
                )
            elif query_params.type == SurveyTypeForGetSurveys.COMPLETION:
                filter_condition &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
                )
            elif query_params.type == SurveyTypeForGetSurveys.QUICK:
                filter_condition &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.QUICK
                )
            elif query_params.type == SurveyTypeForGetSurveys.PP:
                filter_condition &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.PP
                )

            if query_params.project_id:
                filter_condition &= (
                    ProjectSurveyModel.project_id == query_params.project_id
                )

            if query_params.customer_id:
                filter_condition &= (
                    ProjectSurveyModel.customer_id == query_params.customer_id
                )

            if query_params.service_type_id:
                filter_condition &= (
                    ProjectSurveyModel.service_type_id == query_params.service_type_id
                )

            if query_params.organization_ids:
                supporter_organization_id_list: List[str] = (
                    query_params.organization_ids.split(",")
                )

                filter_condition &= ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
            logger.info("finish creating filter condition")

            # ####################################
            # クエリの実行
            # ####################################

            logger.info("start executing query")

            # 指定されたパラメータに応じてGSIへクエリを実行
            if query_params.summary_month_from or query_params.summary_month_to:
                if (
                    not query_params.type
                    == SurveyTypeForGetSurveys.SERVICE_AND_COMPLETION
                ):
                    project_survey_iter = (
                        ProjectSurveyModel.data_type_summary_month_index.query(
                            hash_key=DataType.SURVEY,
                            range_key_condition=range_key_condition,
                            filter_condition=filter_condition,
                        )
                    )
                else:
                    filter_condition_service = filter_condition
                    filter_condition_service &= (
                        ProjectSurveyModel.survey_type
                        == SurveyTypeForGetSurveys.SERVICE
                    )

                    filter_condition_completion = filter_condition
                    filter_condition_completion &= (
                        ProjectSurveyModel.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION
                    )

                    project_survey_service_iter = (
                        ProjectSurveyModel.data_type_summary_month_index.query(
                            hash_key=DataType.SURVEY,
                            range_key_condition=range_key_condition,
                            filter_condition=filter_condition_service,
                        )
                    )

                    project_survey_completion_iter = (
                        ProjectSurveyModel.data_type_summary_month_index.query(
                            hash_key=DataType.SURVEY,
                            range_key_condition=range_key_condition,
                            filter_condition=filter_condition_completion,
                        )
                    )

                    project_survey_iter = itertools.chain(
                        project_survey_service_iter, project_survey_completion_iter
                    )

            elif (
                query_params.plan_survey_request_date_from
                or query_params.plan_survey_request_date_to
            ):
                if (
                    not query_params.type
                    == SurveyTypeForGetSurveys.SERVICE_AND_COMPLETION
                ):
                    project_survey_iter = ProjectSurveyModel.data_type_plan_survey_request_datetime_index.query(
                        hash_key=DataType.SURVEY,
                        range_key_condition=range_key_condition,
                        filter_condition=filter_condition,
                    )
                else:
                    filter_condition_service = filter_condition
                    filter_condition_service &= (
                        ProjectSurveyModel.survey_type
                        == SurveyTypeForGetSurveys.SERVICE
                    )

                    filter_condition_completion = filter_condition
                    filter_condition_completion &= (
                        ProjectSurveyModel.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION
                    )

                    project_survey_service_iter = (
                        ProjectSurveyModel.data_type_summary_month_index.query(
                            hash_key=DataType.SURVEY,
                            range_key_condition=range_key_condition,
                            filter_condition=filter_condition_service,
                        )
                    )

                    project_survey_completion_iter = (
                        ProjectSurveyModel.data_type_summary_month_index.query(
                            hash_key=DataType.SURVEY,
                            range_key_condition=range_key_condition,
                            filter_condition=filter_condition_completion,
                        )
                    )

                    project_survey_iter = itertools.chain(
                        project_survey_service_iter, project_survey_completion_iter
                    )

            logger.info("finish executing query")

            # ####################################
            # 集計処理(Pandasへ格納)
            # ####################################

            # 最初のアンケートを取り出してpandasへ格納 (pandasをインスタンス化しappendメソッドを呼び出すため)
            while True:
                try:
                    project_survey = next(project_survey_iter)
                    if query_params.type == SurveyTypeForGetSurveys.PP:
                        # BackOffice全ユーザーPPアンケート閲覧可能
                        break
                    if SurveyService.is_visible_survey_for_get_survey_by_id(
                        current_user=current_user,
                        project_id=project_survey.project_id,
                    ):
                        break
                except StopIteration:
                    logger.warning("ExportSurveys project_survey not found. ")
                    raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

            index: int = 0

            logger.info("start creating panda items for first")

            output_answer_user_name = ""
            if project_survey.answer_user_id:
                # 通常アンケートの場合
                output_answer_user_name = project_survey.answer_user_name
            elif project_survey.dedicated_survey_user_email:
                # 匿名アンケートの場合
                output_answer_user_name = project_survey.dedicated_survey_user_name

            pd_items: Dict = {
                SurveyCsvAttributeOfRaw.DATA_TYPE_D.value[
                    0
                ]: SurveyCsvAttributeOfRaw.DATA_TYPE_D.value[1],
                SurveyCsvAttributeOfRaw.PROJECT_SURVEY_ID.value[0]: project_survey.id,
                SurveyCsvAttributeOfRaw.SURVEY_MASTER_ID.value: project_survey.survey_master_id,
                SurveyCsvAttributeOfRaw.SURVEY_MASTER_NAME.value[
                    0
                ]: SurveyService.get_survey_master_name_from_cache(
                    project_survey.survey_master_id
                ),
                SurveyCsvAttributeOfRaw.SURVEY_MASTER_REVISION.value[
                    0
                ]: project_survey.survey_master_revision,
                SurveyCsvAttributeOfRaw.SURVEY_TYPE_NAME.value[0]: SurveyTypeName[
                    project_survey.survey_type.upper()
                ],
                SurveyCsvAttributeOfRaw.COMPANY.value[0]: project_survey.customer_name
                if project_survey.customer_name
                else project_survey.company,
                SurveyCsvAttributeOfRaw.PROJECT_NAME.value[
                    0
                ]: project_survey.project_name,
                SurveyCsvAttributeOfRaw.SUPPORTER_ORGANIZATION_NAME.value[
                    0
                ]: project_survey.supporter_organization_name,
                SurveyCsvAttributeOfRaw.SERVICE_TYPE_NAME.value[
                    0
                ]: project_survey.service_type_name,
                SurveyCsvAttributeOfRaw.CUSTOMER_SUCCESS.value[
                    0
                ]: project_survey.customer_success,
                SurveyCsvAttributeOfRaw.ANSWER_USER_NAME.value[
                    0
                ]: output_answer_user_name,
                SurveyCsvAttributeOfRaw.MAIN_SUPPORTER_USER.value[
                    0
                ]: project_survey.main_supporter_user.organization_name
                + "_"
                + project_survey.main_supporter_user.name
                if project_survey.main_supporter_user
                and project_survey.main_supporter_user.organization_name
                and project_survey.main_supporter_user.name
                else None,
                SurveyCsvAttributeOfRaw.SUPPORTER_USER.value[
                    0
                ]: SurveyService.get_accelerator_display_names(
                    project_survey.supporter_users
                )
                if project_survey.supporter_users
                else (
                    project_survey.supporter_organization_name
                    + "_"
                    + project_survey.service_manager_name
                )
                if project_survey.service_manager_name
                else None,
                SurveyCsvAttributeOfRaw.IS_SOLVER_PROJECT.value[0]: None
                if query_params.type == SurveyTypeForGetSurveys.PP
                else SurveyCsvAttributeOfRaw.NO_SOLVER.value
                if not project_survey.is_solver_project
                else SurveyCsvAttributeOfRaw.YES_SOLVER.value,
                SurveyCsvAttributeOfRaw.SALES_USER_NAME.value[
                    0
                ]: SurveyService.get_user_name(project_survey.sales_user_id),
                SurveyCsvAttributeOfRaw.SUMMARY_MONTH.value[
                    0
                ]: project_survey.summary_month,
                SurveyCsvAttributeOfRaw.PLAN_SURVEY_REQUEST_DATETIME.value[
                    0
                ]: project_survey.plan_survey_request_datetime,
                SurveyCsvAttributeOfRaw.ACTUAL_SURVEY_REQUEST_DATETIME.value[
                    0
                ]: project_survey.actual_survey_request_datetime,
                SurveyCsvAttributeOfRaw.PLAN_SURVEY_RESPONSE_DATETIME.value[
                    0
                ]: project_survey.plan_survey_response_datetime,
                SurveyCsvAttributeOfRaw.ACTUAL_SURVEY_RESPONSE_DATETIME.value[
                    0
                ]: project_survey.actual_survey_response_datetime,
                SurveyCsvAttributeOfRaw.IS_DISCLOSURE.value[0]: None
                if query_params.type == SurveyTypeForGetSurveys.PP
                else SurveyCsvAttributeOfRaw.APPROVE.value
                if project_survey.is_disclosure
                else SurveyCsvAttributeOfRaw.NOT_APPROVE.value,
                SurveyCsvAttributeOfRaw.IS_NOT_SUMMARY.value[
                    0
                ]: SurveyCsvAttributeOfRaw.INCLUDED.value
                if query_params.type == SurveyTypeForGetSurveys.PP
                else SurveyCsvAttributeOfRaw.INCLUDED.value
                if not project_survey.is_not_summary
                else SurveyCsvAttributeOfRaw.EXCLUDED.value,
                SurveyCsvAttributeOfRaw.ANSWERS_COUNT.value[
                    0
                ]: SurveyService.get_survey_master_questions_number(
                    project_survey.survey_master_id,
                    project_survey.survey_master_revision,
                ),
            }
            logger.info("finish creating panda items for first")

            # 設問ごとの回答の追加
            answer_index = 0

            logger.info("start creating answers for first")
            if project_survey.answers:
                # 子ループ内の設問ずれに対応するため現在のアンケートのアンケートマスタの設問情報を取得
                # question_flowにより回答済みの設問とアンケートマスタの設問のインデックス対応がずれてしまうため
                survey_master_questions = SurveyMasterModel.get(
                    hash_key=project_survey.survey_master_id,
                    range_key=project_survey.survey_master_revision,
                ).questions

                survey_master_question_indexies: List[str] = []
                for survey_master_question in survey_master_questions:
                    survey_master_question_indexies.append(survey_master_question.id)

                for answer in project_survey.answers:
                    try:
                        answerd_question_master_index = (
                            survey_master_question_indexies.index(answer.id)
                        )
                    except ValueError:
                        continue

                    # 現在のアンケートの設問をアンケートマスタの設問のインデックスと同じ位置に登録(ヘッダレコードの設問とずれてしまうため)
                    if answerd_question_master_index == answer_index:
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_ID.value[0]}_{answer_index}"
                        ] = answer.id
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_STR.value}_{answer_index}"
                        ] = answer.answer
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_POINT.value[0]}_{answer_index}"
                        ] = answer.point
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_SUMMARY_TYPE.value[0]}_{answer_index}"
                        ] = SurveyService.conversion_answer_summary_type_to_japanese(
                            answer.summary_type
                        )
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_OTHER_INPUT.value[0]}_{answer_index}"
                        ] = answer.other_input

                        answer_index += 1
                    else:
                        # 現在の回答済みの設問がアンケートマスタの設問位置からいくつずれているかを計算
                        out_alignment_num = answerd_question_master_index - answer_index

                        # ずれた位置の設問にNoneを追加
                        out_alignment_loop_index = 0
                        while out_alignment_loop_index < out_alignment_num:
                            out_alignment_index = (
                                answer_index + out_alignment_loop_index
                            )

                            pd_items[
                                f"{SurveyCsvAttributeOfRaw.ANSWER_ID.value[0]}_{out_alignment_index}"
                            ] = None
                            pd_items[
                                f"{SurveyCsvAttributeOfRaw.ANSWER_STR.value}_{out_alignment_index}"
                            ] = None
                            pd_items[
                                f"{SurveyCsvAttributeOfRaw.ANSWER_POINT.value[0]}_{out_alignment_index}"
                            ] = None
                            pd_items[
                                f"{SurveyCsvAttributeOfRaw.ANSWER_SUMMARY_TYPE.value[0]}_{out_alignment_index}"
                            ] = None
                            pd_items[
                                f"{SurveyCsvAttributeOfRaw.ANSWER_OTHER_INPUT.value[0]}_{out_alignment_index}"
                            ] = None

                            out_alignment_loop_index += 1
                        answer_index += out_alignment_num

                        # 設問位置ずれ解消後にマスタの設問と対応する位置に回答済み設問を追加
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_ID.value[0]}_{answerd_question_master_index}"
                        ] = answer.id
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_STR.value}_{answerd_question_master_index}"
                        ] = answer.answer
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_POINT.value[0]}_{answerd_question_master_index}"
                        ] = answer.point
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_SUMMARY_TYPE.value[0]}_{answerd_question_master_index}"
                        ] = SurveyService.conversion_answer_summary_type_to_japanese(
                            answer.summary_type
                        )
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_OTHER_INPUT.value[0]}_{answerd_question_master_index}"
                        ] = answer.other_input

                        answer_index += 1

            logger.info("finish creating answers for first")

            pd_summary = pd.DataFrame(pd_items, index=[index])

            logger.info("start creating rest of surveys")
            # 残りのアンケートの集計処理
            for project_survey in project_survey_iter:
                if (
                    not query_params.type == SurveyTypeForGetSurveys.PP
                    and not SurveyService.is_visible_survey_for_get_survey_by_id(
                        current_user=current_user,
                        project_id=project_survey.project_id,
                    )
                ):
                    continue

                index += 1
                answer_index = 0

                output_answer_user_name = ""
                if project_survey.answer_user_id:
                    # 通常アンケートの場合
                    output_answer_user_name = project_survey.answer_user_name
                elif project_survey.dedicated_survey_user_email:
                    # 匿名アンケートの場合
                    output_answer_user_name = project_survey.dedicated_survey_user_name

                pd_items: Dict = {
                    SurveyCsvAttributeOfRaw.DATA_TYPE_D.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.DATA_TYPE_D.value[1],
                    SurveyCsvAttributeOfRaw.PROJECT_SURVEY_ID.value[
                        0
                    ]: project_survey.id,
                    SurveyCsvAttributeOfRaw.SURVEY_MASTER_ID.value: project_survey.survey_master_id,
                    SurveyCsvAttributeOfRaw.SURVEY_MASTER_NAME.value[
                        0
                    ]: SurveyService.get_survey_master_name_from_cache(
                        project_survey.survey_master_id
                    ),
                    SurveyCsvAttributeOfRaw.SURVEY_MASTER_REVISION.value[
                        0
                    ]: project_survey.survey_master_revision,
                    SurveyCsvAttributeOfRaw.SURVEY_TYPE_NAME.value[0]: SurveyTypeName[
                        project_survey.survey_type.upper()
                    ],
                    SurveyCsvAttributeOfRaw.COMPANY.value[
                        0
                    ]: project_survey.customer_name
                    if project_survey.customer_name
                    else project_survey.company,
                    SurveyCsvAttributeOfRaw.PROJECT_NAME.value[
                        0
                    ]: project_survey.project_name,
                    SurveyCsvAttributeOfRaw.SUPPORTER_ORGANIZATION_NAME.value[
                        0
                    ]: project_survey.supporter_organization_name,
                    SurveyCsvAttributeOfRaw.SERVICE_TYPE_NAME.value[
                        0
                    ]: project_survey.service_type_name,
                    SurveyCsvAttributeOfRaw.CUSTOMER_SUCCESS.value[
                        0
                    ]: project_survey.customer_success,
                    SurveyCsvAttributeOfRaw.ANSWER_USER_NAME.value[
                        0
                    ]: output_answer_user_name,
                    SurveyCsvAttributeOfRaw.MAIN_SUPPORTER_USER.value[
                        0
                    ]: project_survey.main_supporter_user.organization_name
                    + "_"
                    + project_survey.main_supporter_user.name
                    if project_survey.main_supporter_user
                    and project_survey.main_supporter_user.organization_name
                    and project_survey.main_supporter_user.name
                    else None,
                    SurveyCsvAttributeOfRaw.SUPPORTER_USER.value[
                        0
                    ]: SurveyService.get_accelerator_display_names(
                        project_survey.supporter_users
                    )
                    if project_survey.supporter_users
                    else (
                        project_survey.supporter_organization_name
                        + "_"
                        + project_survey.service_manager_name
                    )
                    if project_survey.service_manager_name
                    else None,
                    SurveyCsvAttributeOfRaw.IS_SOLVER_PROJECT.value[0]: None
                    if query_params.type == SurveyTypeForGetSurveys.PP
                    else SurveyCsvAttributeOfRaw.NO_SOLVER.value
                    if not project_survey.is_solver_project
                    else SurveyCsvAttributeOfRaw.YES_SOLVER.value,
                    SurveyCsvAttributeOfRaw.SALES_USER_NAME.value[
                        0
                    ]: SurveyService.get_user_name(project_survey.sales_user_id),
                    SurveyCsvAttributeOfRaw.SUMMARY_MONTH.value[
                        0
                    ]: project_survey.summary_month,
                    SurveyCsvAttributeOfRaw.PLAN_SURVEY_REQUEST_DATETIME.value[
                        0
                    ]: project_survey.plan_survey_request_datetime,
                    SurveyCsvAttributeOfRaw.ACTUAL_SURVEY_REQUEST_DATETIME.value[
                        0
                    ]: project_survey.actual_survey_request_datetime,
                    SurveyCsvAttributeOfRaw.PLAN_SURVEY_RESPONSE_DATETIME.value[
                        0
                    ]: project_survey.plan_survey_response_datetime,
                    SurveyCsvAttributeOfRaw.ACTUAL_SURVEY_RESPONSE_DATETIME.value[
                        0
                    ]: project_survey.actual_survey_response_datetime,
                    SurveyCsvAttributeOfRaw.IS_DISCLOSURE.value[0]: None
                    if query_params.type == SurveyTypeForGetSurveys.PP
                    else SurveyCsvAttributeOfRaw.APPROVE.value
                    if project_survey.is_disclosure
                    else SurveyCsvAttributeOfRaw.NOT_APPROVE.value,
                    SurveyCsvAttributeOfRaw.IS_NOT_SUMMARY.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.INCLUDED.value
                    if query_params.type == SurveyTypeForGetSurveys.PP
                    else SurveyCsvAttributeOfRaw.INCLUDED.value
                    if not project_survey.is_not_summary
                    else SurveyCsvAttributeOfRaw.EXCLUDED.value,
                    SurveyCsvAttributeOfRaw.ANSWERS_COUNT.value[
                        0
                    ]: SurveyService.get_survey_master_questions_number(
                        project_survey.survey_master_id,
                        project_survey.survey_master_revision,
                    ),
                }

                pd_summary.loc[index] = pd_items

                if project_survey.answers:
                    # 子ループ内の設問ずれに対応するため現在のアンケートのアンケートマスタの設問情報を取得
                    # question_flowにより回答済みの設問とアンケートマスタの設問のインデックス対応がずれてしまうため
                    survey_master_questions = SurveyMasterModel.get(
                        hash_key=project_survey.survey_master_id,
                        range_key=project_survey.survey_master_revision,
                    ).questions

                    survey_master_question_indexies: List[str] = []
                    for survey_master_question in survey_master_questions:
                        survey_master_question_indexies.append(
                            survey_master_question.id
                        )

                    for answer in project_survey.answers:
                        try:
                            answerd_question_master_index = (
                                survey_master_question_indexies.index(answer.id)
                            )
                        except ValueError:
                            continue

                        # 現在のアンケートの設問をアンケートマスタの設問のインデックスと同じ位置に登録(ヘッダレコードの設問とずれてしまうため)
                        if answerd_question_master_index == answer_index:
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_ID.value[0]}_{answer_index}",
                            ] = answer.id
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_STR.value}_{answer_index}",
                            ] = answer.answer
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_POINT.value[0]}_{answer_index}",
                            ] = answer.point
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_SUMMARY_TYPE.value[0]}_{answer_index}",
                            ] = SurveyService.conversion_answer_summary_type_to_japanese(
                                answer.summary_type
                            )
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_OTHER_INPUT.value[0]}_{answer_index}",
                            ] = answer.other_input

                            answer_index += 1
                        else:
                            # 現在の回答済みの設問がアンケートマスタの設問位置からいくつずれているかを計算
                            out_alignment_num = (
                                answerd_question_master_index - answer_index
                            )

                            # ずれた位置の設問にNoneを追加
                            out_alignment_loop_index = 0
                            while out_alignment_loop_index < out_alignment_num:
                                out_alignment_index = (
                                    answer_index + out_alignment_loop_index
                                )

                                pd_summary.loc[
                                    index,
                                    f"{SurveyCsvAttributeOfRaw.ANSWER_ID.value[0]}_{out_alignment_index}",
                                ] = None
                                pd_summary.loc[
                                    index,
                                    f"{SurveyCsvAttributeOfRaw.ANSWER_STR.value}_{out_alignment_index}",
                                ] = None
                                pd_summary.loc[
                                    index,
                                    f"{SurveyCsvAttributeOfRaw.ANSWER_POINT.value[0]}_{out_alignment_index}",
                                ] = None
                                pd_summary.loc[
                                    index,
                                    f"{SurveyCsvAttributeOfRaw.ANSWER_SUMMARY_TYPE.value[0]}_{out_alignment_index}",
                                ] = None
                                pd_summary.loc[
                                    index,
                                    f"{SurveyCsvAttributeOfRaw.ANSWER_OTHER_INPUT.value[0]}_{out_alignment_index}",
                                ] = None

                                out_alignment_loop_index += 1
                            answer_index += out_alignment_num

                            # 設問位置ずれ解消後にマスタの設問と対応する位置に回答済み設問を追加
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_ID.value[0]}_{answerd_question_master_index}",
                            ] = answer.id
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_STR.value}_{answerd_question_master_index}",
                            ] = answer.answer
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_POINT.value[0]}_{answerd_question_master_index}",
                            ] = answer.point
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_SUMMARY_TYPE.value[0]}_{answerd_question_master_index}",
                            ] = SurveyService.conversion_answer_summary_type_to_japanese(
                                answer.summary_type
                            )
                            pd_summary.loc[
                                index,
                                f"{SurveyCsvAttributeOfRaw.ANSWER_OTHER_INPUT.value[0]}_{answerd_question_master_index}",
                            ] = answer.other_input

                            answer_index += 1
            logger.info("finish creating rest of surveys")

            # ソート
            pd_summary_s = pd_summary.sort_values(
                by=[
                    SurveyCsvAttributeOfRaw.SURVEY_MASTER_NAME.value[0],
                    SurveyCsvAttributeOfRaw.SURVEY_MASTER_REVISION.value[0],
                    SurveyCsvAttributeOfRaw.SUMMARY_MONTH.value[0],
                    SurveyCsvAttributeOfRaw.ACTUAL_SURVEY_RESPONSE_DATETIME.value[0],
                ],
                ascending=[True, False, False, False],
            )

            # Indexの再割当て
            pd_summary_r = pd_summary_s.reset_index(drop=True)

            # ####################################
            # ヘッダレコードの追加処理
            # ####################################

            logger.info("start creating record hedder")

            summary_target = pd_summary_r.groupby(
                [
                    SurveyCsvAttributeOfRaw.SURVEY_MASTER_NAME.value[0],
                    SurveyCsvAttributeOfRaw.SURVEY_MASTER_REVISION.value[0],
                ]
            ).survey_master_name.groups

            for current in summary_target:
                target_index = pd_summary_r.query(
                    f'{SurveyCsvAttributeOfRaw.SURVEY_MASTER_NAME.value[0]} == "{current[0]}" & {SurveyCsvAttributeOfRaw.SURVEY_MASTER_REVISION.value[0]} == {current[1]}'
                ).index[0]

                target_survey_master_id = pd_summary_r.query(
                    f'{SurveyCsvAttributeOfRaw.SURVEY_MASTER_NAME.value[0]} == "{current[0]}" & {SurveyCsvAttributeOfRaw.SURVEY_MASTER_REVISION.value[0]} == {current[1]}'
                ).survey_master_id.values[0]

                target_survey_master = SurveyMasterModel.get(
                    hash_key=target_survey_master_id, range_key=current[1]
                )

                pd_items: Dict = {
                    SurveyCsvAttributeOfRaw.DATA_TYPE_H.value[
                        0
                    ]: f"{SurveyCsvAttributeOfRaw.DATA_TYPE_H.value[1]}_{current[0]}",
                    SurveyCsvAttributeOfRaw.PROJECT_SURVEY_ID.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.PROJECT_SURVEY_ID.value[1],
                    SurveyCsvAttributeOfRaw.SURVEY_MASTER_NAME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.SURVEY_MASTER_NAME.value[1],
                    SurveyCsvAttributeOfRaw.SURVEY_MASTER_REVISION.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.SURVEY_MASTER_REVISION.value[1],
                    SurveyCsvAttributeOfRaw.SURVEY_TYPE_NAME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.SURVEY_TYPE_NAME.value[1],
                    SurveyCsvAttributeOfRaw.COMPANY.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.COMPANY.value[1],
                    SurveyCsvAttributeOfRaw.PROJECT_NAME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.PROJECT_NAME.value[1],
                    SurveyCsvAttributeOfRaw.SUPPORTER_ORGANIZATION_NAME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.SUPPORTER_ORGANIZATION_NAME.value[1],
                    SurveyCsvAttributeOfRaw.SERVICE_TYPE_NAME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.SERVICE_TYPE_NAME.value[1],
                    SurveyCsvAttributeOfRaw.CUSTOMER_SUCCESS.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.CUSTOMER_SUCCESS.value[1],
                    SurveyCsvAttributeOfRaw.ANSWER_USER_NAME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.ANSWER_USER_NAME.value[1],
                    SurveyCsvAttributeOfRaw.MAIN_SUPPORTER_USER.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.MAIN_SUPPORTER_USER.value[1],
                    SurveyCsvAttributeOfRaw.SUPPORTER_USER.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.SUPPORTER_USER.value[1],
                    SurveyCsvAttributeOfRaw.IS_SOLVER_PROJECT.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.IS_SOLVER_PROJECT.value[1],
                    SurveyCsvAttributeOfRaw.SALES_USER_NAME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.SALES_USER_NAME.value[1],
                    SurveyCsvAttributeOfRaw.SUMMARY_MONTH.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.SUMMARY_MONTH.value[1],
                    SurveyCsvAttributeOfRaw.PLAN_SURVEY_REQUEST_DATETIME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.PLAN_SURVEY_REQUEST_DATETIME.value[1],
                    SurveyCsvAttributeOfRaw.ACTUAL_SURVEY_REQUEST_DATETIME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.ACTUAL_SURVEY_REQUEST_DATETIME.value[1],
                    SurveyCsvAttributeOfRaw.PLAN_SURVEY_RESPONSE_DATETIME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.PLAN_SURVEY_RESPONSE_DATETIME.value[1],
                    SurveyCsvAttributeOfRaw.ACTUAL_SURVEY_RESPONSE_DATETIME.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.ACTUAL_SURVEY_RESPONSE_DATETIME.value[1],
                    SurveyCsvAttributeOfRaw.IS_DISCLOSURE.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.IS_DISCLOSURE.value[1],
                    SurveyCsvAttributeOfRaw.IS_NOT_SUMMARY.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.IS_NOT_SUMMARY.value[1],
                    SurveyCsvAttributeOfRaw.ANSWERS_COUNT.value[
                        0
                    ]: SurveyCsvAttributeOfRaw.ANSWERS_COUNT.value[1],
                }

                question_index = 0

                if target_survey_master.questions:
                    for question in target_survey_master.questions:
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_ID.value[0]}_{question_index}"
                        ] = SurveyCsvAttributeOfRaw.ANSWER_ID.value[1]
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_STR.value}_{question_index}"
                        ] = question.description
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_POINT.value[0]}_{question_index}"
                        ] = SurveyCsvAttributeOfRaw.ANSWER_POINT.value[1]
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_SUMMARY_TYPE.value[0]}_{question_index}"
                        ] = SurveyCsvAttributeOfRaw.ANSWER_SUMMARY_TYPE.value[1]
                        pd_items[
                            f"{SurveyCsvAttributeOfRaw.ANSWER_OTHER_INPUT.value[0]}_{question_index}"
                        ] = SurveyCsvAttributeOfRaw.ANSWER_OTHER_INPUT.value[1]

                        question_index += 1

                pd_summary_head = pd_summary_r.iloc[:target_index]
                pd_summary_body = pd.DataFrame(pd_items, index=[0])
                pd_summary_tail = pd_summary_r.iloc[target_index:]

                pd_summary_r = pd.concat(
                    [pd_summary_head, pd_summary_body, pd_summary_tail]
                )

                # 結合後のIndexの再割当て
                pd_summary_r = pd_summary_r.reset_index(drop=True)

            # アンケートマスタID列の削除
            pd_summary_r = pd_summary_r.drop(
                columns=SurveyCsvAttributeOfRaw.SURVEY_MASTER_ID.value
            )

            logger.info("finish creating record hedder")

            # ####################################
            # CSV出力
            # ####################################

            logger.info("start exporting CSV")

            now = get_datetime_now()

            csv_file_name = (
                f"{CsvFormatName.RAW_SURVEY.value}_{now.strftime('%Y%m%d-%H%M%S')}.csv"
            )
            save_path = f"/tmp/{csv_file_name}"

            pd_summary_r.to_csv(
                save_path,
                encoding="utf-8_sig",
                quoting=1,  # csv.QUOTE_ALL
                header=False,
                index=False,
            )

            bucket_name = get_app_settings().upload_s3_bucket_name
            object_key = f"export/survey/{csv_file_name}"

            s3 = S3Helper()

            s3.put_object(save_path, bucket_name, object_key)
            url = s3.generate_presigned_url(
                "get_object", bucket_name, object_key, S3PresignedExpire.DEFAULT
            )

            os.remove(save_path)

            logger.info("finish exporting CSV")

            return ExportSurveysResponse(url=url)

        # ####################################
        # 支援者別アンケート集計CSV
        # ####################################

        logger.info("start supporter surveys")
        if query_params.mode == ExportSurveysModeType.SUPPORTER.value:
            # ####################################
            # クエリパラメータの組み立て
            # ####################################

            logger.info("start creating filter condition: support surveys")

            filter_condition_service = ProjectSurveyModel.is_finished == "true"
            filter_condition_service &= ProjectSurveyModel.is_not_summary == False  # noqa: E712
            filter_condition_service &= (
                ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
            )

            filter_condition_completion = ProjectSurveyModel.is_finished == "true"
            filter_condition_completion &= ProjectSurveyModel.is_not_summary == False  # noqa: E712
            filter_condition_completion &= (
                ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
            )

            logger.info("finish creating filter condition: support surveys")

            # ####################################
            # クエリの実行
            # ####################################

            logger.info("start executing query: support surveys")

            # 指定されたパラメータに応じてGSIへクエリを実行
            if query_params.summary_month_from or query_params.summary_month_to:
                project_survey_service_iter = (
                    ProjectSurveyModel.data_type_summary_month_index.query(
                        hash_key=DataType.SURVEY,
                        range_key_condition=range_key_condition,
                        filter_condition=filter_condition_service,
                    )
                )
                project_survey_completion_iter = (
                    ProjectSurveyModel.data_type_summary_month_index.query(
                        hash_key=DataType.SURVEY,
                        range_key_condition=range_key_condition,
                        filter_condition=filter_condition_completion,
                    )
                )
                project_survey_iter = itertools.chain(
                    project_survey_service_iter, project_survey_completion_iter
                )

            elif (
                query_params.plan_survey_request_date_from
                or query_params.plan_survey_request_date_to
            ):
                project_survey_service_iter = ProjectSurveyModel.data_type_plan_survey_request_datetime_index.query(
                    hash_key=DataType.SURVEY,
                    range_key_condition=range_key_condition,
                    filter_condition=filter_condition_service,
                )
                project_survey_completion_iter = ProjectSurveyModel.data_type_plan_survey_request_datetime_index.query(
                    hash_key=DataType.SURVEY,
                    range_key_condition=range_key_condition,
                    filter_condition=filter_condition_completion,
                )
                project_survey_iter = itertools.chain(
                    project_survey_service_iter, project_survey_completion_iter
                )

            logger.info("finish executing query: support surveys")

            logger.info("start changing survey evaluators: support surveys")
            # ####################################
            # アンケート評価対象者の変更
            # ####################################
            # アンケート集計：支援者（営業担当者）別集計作成と同様の処理
            # 但し、当該APIでは、dynamodbのテーブル更新はしない

            # 案件IDごとの案件カルテ情報リスト
            project_id_karte_map: dict[str, list[ProjectKarteModel]] = {}

            project_survey_list_with_changed_evaluators: list[ProjectSurveyModel] = []
            for target_project_survey in project_survey_iter:
                ###############################
                # 処理対象アンケートの判定
                ###############################
                # 条件:
                #  集計月が処理年月と一致するもの  ※前述の抽出条件で絞り込み済み
                #  Partner Portalアンケート以外  ※前述の抽出条件で絞り込み済み
                #  集計対象外でない  ※前述の抽出条件で絞り込み済み
                #  回答依頼済（送信済）  ※前述の抽出条件で回答済を取得しているため、回答依頼済は満たされる
                #  回答期限日が設定されており、回答期限を過ぎていない
                ref_datetime = get_datetime_now()
                ref_datetime_str = (
                    ref_datetime.strftime("%Y/%m/%d") + " " + HourMinutes.MINIMUM
                )
                if (
                    target_project_survey.plan_survey_response_datetime
                    and target_project_survey.plan_survey_response_datetime
                    >= ref_datetime_str
                ):
                    #######################
                    # プロデューサー
                    #######################
                    # アンケート送信時にプロデューサーを担当していたメンバーが評価対象者のため、アンケート情報は更新しない
                    #######################
                    # アクセラレーター
                    #######################
                    # 以下の条件に則って、アンケート情報を更新する
                    # ・アンケート集計月の個別カルテをアンケート回答期限日までに記入したアクセラレーターが評価対象者
                    # ・アクセラレーターとして登録されたソルバーは、個別カルテを記入していなくても評価対象者とする
                    # ・プロデューサーがカルテ更新した場合は、アクセラレーターに含めない
                    # ・個別カルテを記入するタイミングが集計月以前であっても評価対象とする
                    #
                    # なお、回答期限日が設定されていないアンケートは、従来と同様、
                    # アンケート送信時にアクセラレーターを担当していたメンバーが評価対象者となる（アンケート情報は更新しない）

                    # 辞書（key: アクセラレーターのユーザID、value: SupporterUserAttribute）
                    supporter_id_attribute_dict: dict[str, SupporterUserAttribute] = {}

                    if target_project_survey.plan_survey_response_datetime:
                        # 回答期限日が設定されている場合
                        # 案件カルテ情報の取得
                        if (
                            target_project_survey.project_id
                            in project_id_karte_map.keys()
                        ):
                            # 対象の案件カルテ情報が取得済みの場合
                            target_project_karte_list = project_id_karte_map[
                                target_project_survey.project_id
                            ]
                        else:
                            ###############################
                            # 案件カルテ情報取得
                            ###############################
                            # 抽出条件
                            # ・案件ID
                            project_karte_list: list[ProjectKarteModel] = list(
                                ProjectKarteModel.project_id_start_datetime_index.query(
                                    hash_key=target_project_survey.project_id
                                )
                            )
                            project_id_karte_map[target_project_survey.project_id] = (
                                project_karte_list
                            )
                            target_project_karte_list = project_karte_list

                        # 集計月の案件カルテ情報の取得
                        # 集計月の月初
                        karte_from_date_str = (
                            target_project_survey.summary_month + "/01"
                        )
                        # 集計月の月末 (翌月の1日から1日分戻す)
                        summary_month_datetime = datetime.strptime(
                            target_project_survey.summary_month, "%Y/%m"
                        )
                        karte_to_date = summary_month_datetime + relativedelta(
                            months=+1, day=1, days=-1
                        )
                        karte_to_date_str = karte_to_date.strftime("%Y/%m/%d")

                        target_project_summary_month_karte_list = [
                            x
                            for x in target_project_karte_list
                            if karte_from_date_str <= x.date
                            and x.date <= karte_to_date_str
                        ]

                        # アンケート評価対象者の変更
                        for target_karte in target_project_summary_month_karte_list:
                            if target_karte.karte_notify_update_history:
                                for history in target_karte.karte_notify_update_history:
                                    comp_date_str = datetime.strptime(
                                        target_project_survey.plan_survey_response_datetime,
                                        "%Y/%m/%d %H:%M",
                                    ).strftime("%Y/%m/%d")

                                    if (
                                        history.karte_update_date <= comp_date_str
                                        and target_project_survey.main_supporter_user
                                        and history.user_id
                                        != target_project_survey.main_supporter_user.id
                                    ):
                                        # アンケート集計月の個別カルテをアンケート回答期限日までに記入したアクセラレーターが評価対象者
                                        # プロデューサーがカルテ更新した場合は、アクセラレーターに含めない

                                        # アクセラレーターごとのSupporterUserAttributeの編集
                                        supporter_id_attribute_dict[history.user_id] = (
                                            SupporterUserAttribute(
                                                id=history.user_id,
                                                name=history.user_name,
                                                organization_id=history.organization_id,
                                                organization_name=history.organization_name,
                                            )
                                        )

                        # アンケート情報のアクセラレーターの部署ID等は、カルテ更新時のものを使用する
                        supporter_user_list: list[SupporterUserAttribute] = None
                        for key, value in supporter_id_attribute_dict.items():
                            if supporter_user_list is None:
                                supporter_user_list = []

                            temp_supporter_user = value
                            supporter_user_list.append(temp_supporter_user)

                        # アクセラレーターとして登録されたソルバーは、個別カルテを更新していなくても評価対象者とする
                        target_supporter_users: list[SupporterUserAttribute] = None
                        if target_project_survey.is_updated_evaluation_supporters:
                            # アンケート評価対象者が更新済みの場合
                            target_supporter_users = copy.deepcopy(
                                target_project_survey.supporter_users_before_update
                            )
                        else:
                            # アンケート評価対象者が更新されていない場合
                            target_supporter_users = copy.deepcopy(
                                target_project_survey.supporter_users
                            )
                        # アクセラレーターとして登録されたソルバーを抽出し、supporter_user_listに追加
                        if target_supporter_users:
                            for supporter_user in target_supporter_users:
                                if re.match(SolverIdentifier.NAME_PREFIX, supporter_user.name):
                                    # アクセラレーターの名前の先頭がソルバーを識別する文字列である場合
                                    if supporter_user_list is None:
                                        supporter_user_list = []
                                    supporter_user_list.append(copy.deepcopy(supporter_user))

                        target_project_survey.supporter_users = supporter_user_list

                project_survey_list_with_changed_evaluators.append(
                    target_project_survey
                )

            # 変更したアンケート情報をイテレータに戻す
            project_survey_iter = iter(project_survey_list_with_changed_evaluators)

            logger.info("finish changing survey evaluators: support surveys")

            # ####################################
            # 集計処理(Pandasへ格納)
            # ####################################

            # 最初のアンケートを取り出してpandasへ格納 (pandasをインスタンス化しappendメソッドを呼び出すため)
            try:
                project_survey = next(project_survey_iter)
            except StopIteration:
                logger.warning(
                    "ExportSurveys project_survey: support surveys not found."
                )
                raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

            index: int = 0
            # カラム名のみ設定した空のDataFrameを作成
            pd_items_cols: list = [
                SurveyCsvAttributeOfSupporter.ACCELERATOR_NAME.value,
                SurveyCsvAttributeOfSupporter.SUPPORTER_ORGANIZATION_NAME.value,
                SurveyCsvAttributeOfSupporter.SURVEY_TYPE.value,
                SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_N.value,
                SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value,
                SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_RECOMMENDED.value,
                SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_N.value,
                SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_EVALUATION.value,
                SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_N.value,
            ]
            pd_summary = pd.DataFrame(columns=pd_items_cols)

            logger.info("start creating panda item: support surveys for first")
            # Producerの情報を追加
            if project_survey.main_supporter_user:
                pd_items: Dict = {
                    SurveyCsvAttributeOfSupporter.ACCELERATOR_NAME.value: project_survey.main_supporter_user.name,
                    SurveyCsvAttributeOfSupporter.SUPPORTER_ORGANIZATION_NAME.value: ProjectService.cached_get_supporter_organization_name(
                        project_survey.main_supporter_user.organization_id
                    ),
                    SurveyCsvAttributeOfSupporter.SURVEY_TYPE.value: project_survey.survey_type,
                    SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                    if project_survey.survey_type
                    == SurveyTypeForGetSurveys.SERVICE.value
                    and (
                        # 未回答の場合はNoneを登録
                        project_survey.points.satisfaction > 0
                        or project_survey.points.unanswered is None
                        or SurveyQuestionsSummaryType.SATISFACTION.value
                        not in project_survey.points.unanswered
                    )
                    else None,
                    SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_N.value: None,  # アンケート数 * 案件
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                    if project_survey.survey_type
                    == SurveyTypeForGetSurveys.COMPLETION.value
                    and (
                        # 未回答の場合はNoneを登録
                        project_survey.points.satisfaction > 0
                        or project_survey.points.unanswered is None
                        or SurveyQuestionsSummaryType.SATISFACTION.value
                        not in project_survey.points.unanswered
                    )
                    else None,
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value: project_survey.points.continuation
                    if project_survey.survey_type
                    == SurveyTypeForGetSurveys.COMPLETION.value
                    and (
                        # 未回答の場合はNoneを登録
                        project_survey.points.continuation
                        or project_survey.points.unanswered is None
                        or SurveyQuestionsSummaryType.CONTINUATION.value
                        not in project_survey.points.unanswered
                    )
                    else None,
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_RECOMMENDED.value: project_survey.points.recommended
                    if project_survey.survey_type
                    == SurveyTypeForGetSurveys.COMPLETION.value
                    and (
                        # 未回答の場合はNoneを登録
                        project_survey.points.recommended > 0
                        or project_survey.points.unanswered is None
                        or SurveyQuestionsSummaryType.RECOMMENDED.value
                        not in project_survey.points.unanswered
                    )
                    else None,
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_N.value: None,
                    SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_EVALUATION.value: None,
                    SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_N.value: None,
                }
                pd_summary.loc[index] = pd_items
                index += 1
            logger.info("finish creating panda item: support surveys for first")

            logger.info("start creating accelerator item: support surveys for first")
            # Acceleratorの情報を追加
            if project_survey.supporter_users:
                for accelerator in project_survey.supporter_users:
                    pd_items: Dict = {
                        SurveyCsvAttributeOfSupporter.ACCELERATOR_NAME.value: accelerator.name,
                        SurveyCsvAttributeOfSupporter.SUPPORTER_ORGANIZATION_NAME.value: ProjectService.cached_get_supporter_organization_name(
                            accelerator.organization_id
                        ),
                        SurveyCsvAttributeOfSupporter.SURVEY_TYPE.value: project_survey.survey_type,
                        SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.SERVICE.value
                        and (
                            # 未回答の場合はNoneを登録
                            project_survey.points.satisfaction > 0
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.SATISFACTION.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_N.value: None,  # アンケート数 * 案件
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION.value
                        and (
                            # 未回答の場合はNoneを登録
                            project_survey.points.satisfaction > 0
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.SATISFACTION.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value: project_survey.points.continuation
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION.value
                        and (
                            # 未回答の場合はNoneを登録
                            project_survey.points.continuation
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.CONTINUATION.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_RECOMMENDED.value: project_survey.points.recommended
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION.value
                        and (
                            # 未回答の場合はNoneを登録
                            project_survey.points.recommended > 0
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.RECOMMENDED.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_N.value: None,
                        SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_EVALUATION.value: None,
                        SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_N.value: None,
                    }
                    pd_summary.loc[index] = pd_items
                    index += 1
            logger.info("finish creating accelerator item: support surveys for first")

            logger.info("start creating rest of surveys: support surveys")
            # 残りのアンケートの集計処理
            for project_survey in project_survey_iter:
                # Producerの情報を追加
                if project_survey.main_supporter_user:
                    pd_items: Dict = {
                        SurveyCsvAttributeOfSupporter.ACCELERATOR_NAME.value: project_survey.main_supporter_user.name,
                        SurveyCsvAttributeOfSupporter.SUPPORTER_ORGANIZATION_NAME.value: ProjectService.cached_get_supporter_organization_name(
                            project_survey.main_supporter_user.organization_id
                        ),
                        SurveyCsvAttributeOfSupporter.SURVEY_TYPE.value: project_survey.survey_type,
                        SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.SERVICE.value
                        and (
                            # 未回答の場合はNoneを登録
                            project_survey.points.satisfaction > 0
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.SATISFACTION.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_N.value: None,  # アンケート数 * 案件
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION.value
                        and (
                            # 未回答の場合はNoneを登録
                            project_survey.points.satisfaction > 0
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.SATISFACTION.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value: project_survey.points.continuation
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION.value
                        and (
                            # 未回答の場合はNoneを登録
                            project_survey.points.continuation
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.CONTINUATION.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_RECOMMENDED.value: project_survey.points.recommended
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION.value
                        and (
                            # 未回答の場合はNoneを登録
                            project_survey.points.recommended > 0
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.RECOMMENDED.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_N.value: None,
                        SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_EVALUATION.value: None,
                        SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_N.value: None,
                    }
                    pd_summary.loc[index] = pd_items
                    index += 1

                logger.info("start adding rest of accelerator item: support surveys")
                # Acceleratorの情報を追加
                if project_survey.supporter_users:
                    for accelerator in project_survey.supporter_users:
                        pd_items: Dict = {
                            SurveyCsvAttributeOfSupporter.ACCELERATOR_NAME.value: accelerator.name,
                            SurveyCsvAttributeOfSupporter.SUPPORTER_ORGANIZATION_NAME.value: ProjectService.cached_get_supporter_organization_name(
                                accelerator.organization_id
                            ),
                            SurveyCsvAttributeOfSupporter.SURVEY_TYPE.value: project_survey.survey_type,
                            SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                            if project_survey.survey_type
                            == SurveyTypeForGetSurveys.SERVICE.value
                            and (
                                # 未回答の場合はNoneを登録
                                project_survey.points.satisfaction > 0
                                or project_survey.points.unanswered is None
                                or SurveyQuestionsSummaryType.SATISFACTION.value
                                not in project_survey.points.unanswered
                            )
                            else None,
                            SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_N.value: None,  # アンケート数 * 案件
                            SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                            if project_survey.survey_type
                            == SurveyTypeForGetSurveys.COMPLETION.value
                            and (
                                # 未回答の場合はNoneを登録
                                project_survey.points.satisfaction > 0
                                or project_survey.points.unanswered is None
                                or SurveyQuestionsSummaryType.SATISFACTION.value
                                not in project_survey.points.unanswered
                            )
                            else None,
                            SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value: project_survey.points.continuation
                            if project_survey.survey_type
                            == SurveyTypeForGetSurveys.COMPLETION.value
                            and (
                                # 未回答の場合はNoneを登録
                                project_survey.points.continuation
                                or project_survey.points.unanswered is None
                                or SurveyQuestionsSummaryType.CONTINUATION.value
                                not in project_survey.points.unanswered
                            )
                            else None,
                            SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_RECOMMENDED.value: project_survey.points.recommended
                            if project_survey.survey_type
                            == SurveyTypeForGetSurveys.COMPLETION.value
                            and (
                                # 未回答の場合はNoneを登録
                                project_survey.points.recommended > 0
                                or project_survey.points.unanswered is None
                                or SurveyQuestionsSummaryType.RECOMMENDED.value
                                not in project_survey.points.unanswered
                            )
                            else None,
                            SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_N.value: None,
                            SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_EVALUATION.value: None,
                            SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_N.value: None,
                        }
                        pd_summary.loc[index] = pd_items
                        index += 1

            logger.info("finish creating rest of surveys: support surveys")
            logger.info("finish adding rest of accelerator item: support surveys")

            # 支援者と支援者組織課ごとにグルーピングし総合満足度などの素の数値の平均を集計
            pd_summary_g = pd_summary.groupby(
                [
                    SurveyCsvAttributeOfSupporter.ACCELERATOR_NAME.value,
                    SurveyCsvAttributeOfSupporter.SUPPORTER_ORGANIZATION_NAME.value,
                ],
                as_index=False,
            )[
                [
                    SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                    SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_N.value,
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value,
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_RECOMMENDED.value,
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_N.value,
                    SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_EVALUATION.value,
                    SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_N.value,
                ]
            ].mean()

            # NaNをNoneに変換
            pd_summary_g = pd_summary_g.replace([np.nan], [None])

            logger.info("start calculating numbers")
            # N数と総合満足度を集計
            index_s = 0
            index_s_tail = len(pd_summary_g) - 1

            column_list = pd_summary_g.columns.values

            while index_s <= index_s_tail:
                # [ アクセラレータ氏名, 課名 （支援者が属する課）, サービスアンケート満足度, サービスアンケートN数, 修了アンケート総合満足度,
                # 修了アンケート継続意思, 修了アンケート紹介可能性, 修了アンケートN数, 総合満足度評価, 総合満足度N数 ]
                target_summary = pd_summary_g.iloc[index_s]

                target_supporter_name = target_summary.values[0]
                target_organization_name = target_summary.values[1]

                if (
                    SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value
                    in column_list
                ):
                    # サービスアンケート満足度の四捨五入
                    tmp_service_survey_total_satisfaction = pd_summary_g.loc[
                        index_s,
                        SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                    ]
                    if tmp_service_survey_total_satisfaction is not None:
                        pd_summary_g.loc[
                            index_s,
                            SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                        ] = round_off(
                            tmp_service_survey_total_satisfaction,
                            0.01,
                        )

                if (
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value
                    in column_list
                ):
                    # 修了アンケート総合満足度の四捨五入
                    tmp_completion_survey_total_satisfaction = pd_summary_g.loc[
                        index_s,
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                    ]
                    if tmp_completion_survey_total_satisfaction is not None:
                        pd_summary_g.loc[
                            index_s,
                            SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                        ] = round_off(
                            tmp_completion_survey_total_satisfaction,
                            0.01,
                        )

                if (
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value
                    in column_list
                ):
                    # 修了アンケート継続意思の四捨五入
                    tmp_completion_survey_continuation = pd_summary_g.loc[
                        index_s,
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value,
                    ]
                    if tmp_completion_survey_continuation is not None:
                        pd_summary_g.loc[
                            index_s,
                            SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value,
                        ] = round_off(
                            tmp_completion_survey_continuation,
                            0.01,
                        )

                    try:
                        completion_continuation = int(
                            pd_summary_g.loc[
                                index_s,
                                SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value,
                            ]
                            * 100
                        )
                    except TypeError:
                        completion_continuation = None
                else:
                    completion_continuation = None

                if (
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_RECOMMENDED.value
                    in column_list
                ):
                    # 修了アンケート紹介可能性の四捨五入
                    tmp_completion_survey_recommended = pd_summary_g.loc[
                        index_s,
                        SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_RECOMMENDED.value,
                    ]
                    if tmp_completion_survey_recommended is not None:
                        pd_summary_g.loc[
                            index_s,
                            SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_RECOMMENDED.value,
                        ] = round_off(
                            tmp_completion_survey_recommended,
                            0.01,
                        )

                # 現在の支援者の全種別のアンケートを取得
                # 課名の列名の全角スペースでエラーになるのでquery()メソッドは使えない
                pd_summary_i = pd_summary[
                    (
                        pd_summary[SurveyCsvAttributeOfSupporter.ACCELERATOR_NAME.value]
                        == target_supporter_name
                    )
                    & (
                        pd_summary[
                            SurveyCsvAttributeOfSupporter.SUPPORTER_ORGANIZATION_NAME.value
                        ]
                        == target_organization_name
                    )
                ]

                service_n = pd_summary_i[
                    SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value
                ].count()
                completion_n = pd_summary_i[
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value
                ].count()
                total_satisfaction_n = service_n + completion_n

                total_satisfaction = (
                    sum(
                        pd_summary_i[
                            [
                                SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                                SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                            ]
                        ].sum()
                    )
                    / total_satisfaction_n
                )
                total_satisfaction = round_off(total_satisfaction, 0.01)

                pd_summary_g.loc[
                    index_s, SurveyCsvAttributeOfSupporter.SERVICE_SURVEY_N.value
                ] = service_n if service_n > 0 else None
                pd_summary_g.loc[
                    index_s,
                    SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_CONTINUATION.value,
                ] = (
                    str(completion_continuation) + "%"
                    if completion_continuation is not None
                    else None
                )
                pd_summary_g.loc[
                    index_s, SurveyCsvAttributeOfSupporter.COMPLETION_SURVEY_N.value
                ] = completion_n if completion_n > 0 else None
                pd_summary_g.loc[
                    index_s,
                    SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_EVALUATION.value,
                ] = total_satisfaction
                pd_summary_g.loc[
                    index_s, SurveyCsvAttributeOfSupporter.TOTAL_SATISFACTION_N.value
                ] = total_satisfaction_n

                index_s += 1

            logger.info("finish calculating numbers")

            # ####################################
            # CSV出力
            # ####################################

            logger.info("start exporting CSV")

            now = get_datetime_now()

            csv_file_name = f"{CsvFormatName.SUPPORTER_SURVEY.value}_{now.strftime('%Y%m%d-%H%M%S')}.csv"
            save_path = f"/tmp/{csv_file_name}"

            pd_summary_g.to_csv(
                save_path,
                encoding="utf-8_sig",
                quoting=1,  # csv.QUOTE_ALL
                index=False,
            )

            bucket_name = get_app_settings().upload_s3_bucket_name
            object_key = f"export/survey_supporter/{csv_file_name}"

            s3 = S3Helper()

            s3.put_object(save_path, bucket_name, object_key)
            url = s3.generate_presigned_url(
                "get_object", bucket_name, object_key, S3PresignedExpire.DEFAULT
            )

            os.remove(save_path)

            logger.info("finish exporting CSV")

            return ExportSurveysResponse(url=url)

        # ####################################
        # 課別アンケート集計CSV
        # ####################################

        logger.info("start organization surveys")
        if query_params.mode == ExportSurveysModeType.ORGANIZATION.value:
            # ####################################
            # クエリパラメータの組み立て
            # ####################################

            logger.info("start creating filter condition: organization surveys")
            filter_condition_service = ProjectSurveyModel.is_finished == "true"
            filter_condition_service &= ProjectSurveyModel.is_not_summary == False
            filter_condition_service &= (
                ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
            )

            filter_condition_completion = ProjectSurveyModel.is_finished == "true"
            filter_condition_completion &= ProjectSurveyModel.is_not_summary == False
            filter_condition_completion &= (
                ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
            )

            logger.info("finish creating filter condition: organization surveys")

            # ####################################
            # クエリの実行
            # ####################################

            logger.info("start executing query: organization surveys")

            # 指定されたパラメータに応じてGSIへクエリを実行
            if query_params.summary_month_from or query_params.summary_month_to:
                project_survey_service_iter = (
                    ProjectSurveyModel.data_type_summary_month_index.query(
                        hash_key=DataType.SURVEY,
                        range_key_condition=range_key_condition,
                        filter_condition=filter_condition_service,
                    )
                )
                project_survey_completion_iter = (
                    ProjectSurveyModel.data_type_summary_month_index.query(
                        hash_key=DataType.SURVEY,
                        range_key_condition=range_key_condition,
                        filter_condition=filter_condition_completion,
                    )
                )
                project_survey_iter = itertools.chain(
                    project_survey_service_iter, project_survey_completion_iter
                )

            elif (
                query_params.plan_survey_request_date_from
                or query_params.plan_survey_request_date_to
            ):
                project_survey_service_iter = ProjectSurveyModel.data_type_plan_survey_request_datetime_index.query(
                    hash_key=DataType.SURVEY,
                    range_key_condition=range_key_condition,
                    filter_condition=filter_condition_service,
                )
                project_survey_completion_iter = ProjectSurveyModel.data_type_plan_survey_request_datetime_index.query(
                    hash_key=DataType.SURVEY,
                    range_key_condition=range_key_condition,
                    filter_condition=filter_condition_completion,
                )
                project_survey_iter = itertools.chain(
                    project_survey_service_iter, project_survey_completion_iter
                )

            logger.info("finish executing query: organization surveys")

            # ####################################
            # 集計処理(Pandasへ格納)
            # ####################################

            # 最初のアンケートを取り出してpandasへ格納 (pandasをインスタンス化しappendメソッドを呼び出すため)
            try:
                project_survey = next(project_survey_iter)
            except StopIteration:
                logger.warning(
                    "ExportSurveys project_survey: organization surveys not found."
                )
                raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

            index: int = 0
            # カラム名のみ設定した空のDataFrameを作成
            pd_items_cols: list = [
                SurveyCsvAttributeOfOrganization.SUPPORTER_ORGANIZATION_NAME.value,
                SurveyCsvAttributeOfOrganization.PRODUCER_NAME.value,
                SurveyCsvAttributeOfOrganization.SURVEY_TYPE.value,
                SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_N.value,
                SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_CONTINUATION.value,
                SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_RECOMMENDED.value,
                SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_N.value,
                SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_EVALUATION.value,
                SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_N.value,
            ]
            pd_summary = pd.DataFrame(columns=pd_items_cols)

            logger.info("start creating panda item: organization surveys for first")
            # Producerの情報を追加
            if project_survey.main_supporter_user:
                pd_items: Dict = {
                    SurveyCsvAttributeOfOrganization.SUPPORTER_ORGANIZATION_NAME.value: ProjectService.cached_get_supporter_organization_name(
                        project_survey.supporter_organization_id
                    ),
                    SurveyCsvAttributeOfOrganization.PRODUCER_NAME.value: project_survey.main_supporter_user.name,
                    SurveyCsvAttributeOfOrganization.SURVEY_TYPE.value: project_survey.survey_type,
                    SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                    if project_survey.survey_type
                    == SurveyTypeForGetSurveys.SERVICE.value
                    and (
                        project_survey.points.satisfaction > 0
                        or project_survey.points.unanswered is None
                        or SurveyQuestionsSummaryType.SATISFACTION.value
                        not in project_survey.points.unanswered
                    )
                    else None,
                    SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_N.value: None,  # アンケート数 * 案件
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                    if project_survey.survey_type
                    == SurveyTypeForGetSurveys.COMPLETION.value
                    and (
                        project_survey.points.satisfaction > 0
                        or project_survey.points.unanswered is None
                        or SurveyQuestionsSummaryType.SATISFACTION.value
                        not in project_survey.points.unanswered
                    )
                    else None,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_CONTINUATION.value: project_survey.points.continuation
                    if project_survey.survey_type
                    == SurveyTypeForGetSurveys.COMPLETION.value
                    and (
                        project_survey.points.continuation
                        or project_survey.points.unanswered is None
                        or SurveyQuestionsSummaryType.CONTINUATION.value
                        not in project_survey.points.unanswered
                    )
                    else None,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_RECOMMENDED.value: project_survey.points.recommended
                    if project_survey.survey_type
                    == SurveyTypeForGetSurveys.COMPLETION.value
                    and (
                        project_survey.points.recommended > 0
                        or project_survey.points.unanswered is None
                        or SurveyQuestionsSummaryType.RECOMMENDED.value
                        not in project_survey.points.unanswered
                    )
                    else None,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_N.value: None,
                    SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_EVALUATION.value: None,
                    SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_N.value: None,
                }
                pd_summary.loc[index] = pd_items
                index += 1
            logger.info("finish creating panda item: organization surveys for first")

            logger.info("start creating rest of survey item: organization surveys")
            # 残りのアンケートの集計処理
            for project_survey in project_survey_iter:
                # Producerの情報を追加
                if project_survey.main_supporter_user:
                    pd_items: Dict = {
                        SurveyCsvAttributeOfOrganization.SUPPORTER_ORGANIZATION_NAME.value: ProjectService.cached_get_supporter_organization_name(
                            project_survey.supporter_organization_id
                        ),
                        SurveyCsvAttributeOfOrganization.PRODUCER_NAME.value: project_survey.main_supporter_user.name,
                        SurveyCsvAttributeOfOrganization.SURVEY_TYPE.value: project_survey.survey_type,
                        SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.SERVICE.value
                        and (
                            project_survey.points.satisfaction > 0
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.SATISFACTION.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_N.value: None,  # アンケート数 * 案件
                        SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_TOTAL_SATISFACTION.value: project_survey.points.satisfaction
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION.value
                        and (
                            project_survey.points.satisfaction > 0
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.SATISFACTION.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_CONTINUATION.value: project_survey.points.continuation
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION.value
                        and (
                            project_survey.points.continuation
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.CONTINUATION.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_RECOMMENDED.value: project_survey.points.recommended
                        if project_survey.survey_type
                        == SurveyTypeForGetSurveys.COMPLETION.value
                        and (
                            project_survey.points.recommended > 0
                            or project_survey.points.unanswered is None
                            or SurveyQuestionsSummaryType.RECOMMENDED.value
                            not in project_survey.points.unanswered
                        )
                        else None,
                        SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_N.value: None,
                        SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_EVALUATION.value: None,
                        SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_N.value: None,
                    }
                    pd_summary.loc[index] = pd_items
                    index += 1
            logger.info("finish creating rest of survey item: organization surveys")

            logger.info("start calculating numbers")
            # 支援者組織課とプロデューサーごとにグルーピングし総合満足度などの素の数値の平均を集計
            pd_summary_g = pd_summary.groupby(
                [
                    SurveyCsvAttributeOfOrganization.SUPPORTER_ORGANIZATION_NAME.value,
                    SurveyCsvAttributeOfOrganization.PRODUCER_NAME.value,
                ],
                as_index=False,
            )[
                [
                    SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                    SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_N.value,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_CONTINUATION.value,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_RECOMMENDED.value,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_N.value,
                    SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_EVALUATION.value,
                    SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_N.value,
                ]
            ].mean()

            # NaNをNoneに変換
            pd_summary_g = pd_summary_g.replace([np.nan], [None])

            # N数と総合満足度を集計
            index_s = 0
            index_s_tail = len(pd_summary_g) - 1

            while index_s <= index_s_tail:
                # [ 課名 （粗利メイン課）, プロデューサー氏名, サービスアンケート満足度, サービスアンケートN数, 修了アンケート総合満足度,
                # 修了アンケート継続意思, 修了アンケート紹介可能性, 修了アンケートN数, 総合満足度評価, 総合満足度N数 ]
                target_summary = pd_summary_g.iloc[index_s]

                target_organization_name = target_summary.values[0]
                target_supporter_name = target_summary.values[1]

                # サービスアンケート満足度の四捨五入
                tmp_service_survey_total_satisfaction = pd_summary_g.loc[
                    index_s,
                    SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                ]
                if tmp_service_survey_total_satisfaction is not None:
                    pd_summary_g.loc[
                        index_s,
                        SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                    ] = round_off(
                        tmp_service_survey_total_satisfaction,
                        0.01,
                    )

                # 修了アンケート総合満足度の四捨五入
                tmp_completion_survey_total_satisfaction = pd_summary_g.loc[
                    index_s,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                ]
                if tmp_completion_survey_total_satisfaction is not None:
                    pd_summary_g.loc[
                        index_s,
                        SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                    ] = round_off(
                        tmp_completion_survey_total_satisfaction,
                        0.01,
                    )

                # 修了アンケート継続意思の四捨五入
                tmp_completion_survey_continuation = pd_summary_g.loc[
                    index_s,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_CONTINUATION.value,
                ]
                if tmp_completion_survey_continuation is not None:
                    pd_summary_g.loc[
                        index_s,
                        SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_CONTINUATION.value,
                    ] = round_off(
                        tmp_completion_survey_continuation,
                        0.01,
                    )

                # 修了アンケート紹介可能性の四捨五入
                tmp_completion_survey_recommended = pd_summary_g.loc[
                    index_s,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_RECOMMENDED.value,
                ]
                if tmp_completion_survey_recommended is not None:
                    pd_summary_g.loc[
                        index_s,
                        SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_RECOMMENDED.value,
                    ] = round_off(
                        tmp_completion_survey_recommended,
                        0.01,
                    )

                try:
                    completion_continuation = int(
                        pd_summary_g.loc[
                            index_s,
                            SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_CONTINUATION.value,
                        ]
                        * 100
                    )
                except TypeError:
                    completion_continuation = None

                # 現在の支援者の全種別のアンケートを取得
                # 課名の列名の全角スペースでエラーになるのでquery()メソッドは使えない
                pd_summary_i = pd_summary[
                    (
                        pd_summary[SurveyCsvAttributeOfOrganization.PRODUCER_NAME.value]
                        == target_supporter_name
                    )
                    & (
                        pd_summary[
                            SurveyCsvAttributeOfOrganization.SUPPORTER_ORGANIZATION_NAME.value
                        ]
                        == target_organization_name
                    )
                ]

                print(pd_summary_i)

                service_n = pd_summary_i[
                    SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_TOTAL_SATISFACTION.value
                ].count()
                completion_n = pd_summary_i[
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_TOTAL_SATISFACTION.value
                ].count()
                total_satisfaction_n = service_n + completion_n

                total_satisfaction = (
                    sum(
                        pd_summary_i[
                            [
                                SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_TOTAL_SATISFACTION.value,
                                SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_TOTAL_SATISFACTION.value,
                            ]
                        ].sum()
                    )
                    / total_satisfaction_n
                )
                total_satisfaction = round_off(total_satisfaction, 0.01)

                pd_summary_g.loc[
                    index_s, SurveyCsvAttributeOfOrganization.SERVICE_SURVEY_N.value
                ] = service_n if service_n > 0 else None
                pd_summary_g.loc[
                    index_s,
                    SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_CONTINUATION.value,
                ] = (
                    str(completion_continuation) + "%"
                    if completion_continuation is not None
                    else None
                )
                pd_summary_g.loc[
                    index_s, SurveyCsvAttributeOfOrganization.COMPLETION_SURVEY_N.value
                ] = completion_n if completion_n > 0 else None
                pd_summary_g.loc[
                    index_s,
                    SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_EVALUATION.value,
                ] = total_satisfaction
                pd_summary_g.loc[
                    index_s, SurveyCsvAttributeOfOrganization.TOTAL_SATISFACTION_N.value
                ] = total_satisfaction_n

                index_s += 1

            logger.info("finish calculating numbers")

            # ####################################
            # CSV出力
            # ####################################

            logger.info("start exporting CSV")

            now = get_datetime_now()

            csv_file_name = f"{CsvFormatName.SUPPORTER_ORGANIZATION_SURVEY.value}_{now.strftime('%Y%m%d-%H%M%S')}.csv"
            save_path = f"/tmp/{csv_file_name}"

            pd_summary_g.to_csv(
                save_path,
                encoding="utf-8_sig",
                quoting=1,  # csv.QUOTE_ALL
                index=False,
            )

            bucket_name = get_app_settings().upload_s3_bucket_name
            object_key = f"export/survey_organization/{csv_file_name}"

            s3 = S3Helper()

            s3.put_object(save_path, bucket_name, object_key)
            url = s3.generate_presigned_url(
                "get_object", bucket_name, object_key, S3PresignedExpire.DEFAULT
            )

            os.remove(save_path)

            logger.info("finish exporting CSV")

            return ExportSurveysResponse(url=url)

    @staticmethod
    def get_surveys(
        query_params: GetSurveysQuery, current_user: AdminModel
    ) -> GetSurveysResponse:
        """案件アンケートリストを取得

        Args:
            query_params (GetSurveysQuery): クエリパラメータ
            current_user (Behavior, optional): 認証済みのユーザー
        Raises:

        Returns:
            GetSurveysResponse: 取得結果
        """
        # アンケート種別回答受信日GSI用
        range_key_condition_survey_type_actual_response = None
        filter_condition_survey_type_actual_response = None

        # 案件ID回答受信日GSI用
        range_key_condition_project_id_actual_response = None
        filter_condition_project_id_actual_response = None

        # データ区分回答受信日GSI用
        range_key_condition_data_type_actual_response = None
        filter_condition_data_type_actual_response = None

        # 案件ID集計月GSI用
        range_key_condition_project_id_summary_month = None
        filter_condition_project_id_summary_month = None

        # アンケート種別集計月GSI用
        range_key_condition_survey_type_summary_month = None
        filter_condition_survey_type_summary_month = None

        # データ区分回答受信予定日GSI用
        range_key_condition_data_type_plan_response = None
        filter_condition_data_type_plan_response = None

        # データ区分集計月GSI用
        range_key_condition_data_type_summary_month = None
        filter_condition_data_type_summary_month = None

        # 集計月
        summary_month_from = (
            datetime.strptime(query_params.summary_month_from, "%Y%m").strftime("%Y/%m")
            if query_params.summary_month_from
            else DateTime.MINIMUM
        )
        summary_month_to = (
            datetime.strptime(query_params.summary_month_to, "%Y%m").strftime("%Y/%m")
            if query_params.summary_month_to
            else DateTime.MAXIMUM
        )

        # 回答日（回答実績）
        actual_survey_response_date_from = (
            datetime.strptime(
                query_params.actual_survey_response_date_from, "%Y%m%d"
            ).strftime("%Y/%m/%d")
            + " "
            + HourMinutes.MINIMUM
            if query_params.actual_survey_response_date_from
            else DateTimeHourMinutes.MINIMUM
        )

        actual_survey_response_date_to = (
            datetime.strptime(
                query_params.actual_survey_response_date_to, "%Y%m%d"
            ).strftime("%Y/%m/%d")
            + " "
            + HourMinutes.MAXIMUM
            if query_params.actual_survey_response_date_to
            else DateTimeHourMinutes.MAXIMUM
        )

        # 回答受信予定日
        plan_survey_response_date_from = (
            datetime.strptime(
                query_params.plan_survey_response_date_from, "%Y%m%d"
            ).strftime("%Y/%m/%d")
            + " "
            + HourMinutes.MINIMUM
            if query_params.plan_survey_response_date_from
            else DateTimeHourMinutes.MINIMUM
        )

        plan_survey_response_date_to = (
            datetime.strptime(
                query_params.plan_survey_response_date_to, "%Y%m%d"
            ).strftime("%Y/%m/%d")
            + " "
            + HourMinutes.MAXIMUM
            if query_params.plan_survey_response_date_to
            else DateTimeHourMinutes.MAXIMUM
        )

        if query_params.summary_month_from or query_params.summary_month_to:
            range_key_condition_survey_type_summary_month &= (
                ProjectSurveyModel.summary_month.between(
                    summary_month_from, summary_month_to
                )
            )
            range_key_condition_project_id_summary_month &= (
                ProjectSurveyModel.summary_month.between(
                    summary_month_from, summary_month_to
                )
            )
            range_key_condition_data_type_summary_month &= (
                ProjectSurveyModel.summary_month.between(
                    summary_month_from, summary_month_to
                )
            )

        if (
            query_params.actual_survey_response_date_from
            or query_params.actual_survey_response_date_to
        ):
            range_key_condition_project_id_actual_response &= (
                ProjectSurveyModel.actual_survey_response_datetime.between(
                    actual_survey_response_date_from, actual_survey_response_date_to
                )
            )
            range_key_condition_survey_type_actual_response &= (
                ProjectSurveyModel.actual_survey_response_datetime.between(
                    actual_survey_response_date_from, actual_survey_response_date_to
                )
            )
            range_key_condition_data_type_actual_response &= (
                ProjectSurveyModel.actual_survey_response_datetime.between(
                    actual_survey_response_date_from, actual_survey_response_date_to
                )
            )

        if (
            query_params.plan_survey_response_date_from
            or query_params.plan_survey_response_date_to
        ):
            range_key_condition_data_type_plan_response &= (
                ProjectSurveyModel.plan_survey_response_datetime.between(
                    plan_survey_response_date_from, plan_survey_response_date_to
                )
            )

        if query_params.is_finished:
            filter_condition_survey_type_actual_response &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )
            filter_condition_project_id_actual_response &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )
            filter_condition_project_id_summary_month &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )
            filter_condition_survey_type_summary_month &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )
            filter_condition_data_type_plan_response &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )
            filter_condition_data_type_summary_month &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )
            filter_condition_data_type_actual_response &= (
                ProjectSurveyModel.is_finished == query_params.is_finished
            )

        if query_params.project_id:
            filter_condition_survey_type_actual_response &= (
                ProjectSurveyModel.project_id == query_params.project_id
            )
            filter_condition_survey_type_summary_month &= (
                ProjectSurveyModel.project_id == query_params.project_id
            )
            filter_condition_data_type_plan_response &= (
                ProjectSurveyModel.project_id == query_params.project_id
            )
            filter_condition_data_type_summary_month &= (
                ProjectSurveyModel.project_id == query_params.project_id
            )
            filter_condition_data_type_actual_response &= (
                ProjectSurveyModel.project_id == query_params.project_id
            )

        if query_params.customer_id:
            filter_condition_survey_type_actual_response &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )
            filter_condition_project_id_actual_response &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )
            filter_condition_data_type_actual_response &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )

            filter_condition_project_id_summary_month &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )
            filter_condition_survey_type_summary_month &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )
            filter_condition_data_type_plan_response &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )
            filter_condition_data_type_summary_month &= (
                ProjectSurveyModel.customer_id == query_params.customer_id
            )

        if query_params.service_type_id:
            filter_condition_survey_type_actual_response &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )
            filter_condition_project_id_actual_response &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )
            filter_condition_data_type_actual_response &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )

            filter_condition_project_id_summary_month &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )
            filter_condition_survey_type_summary_month &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )
            filter_condition_data_type_plan_response &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )
            filter_condition_data_type_summary_month &= (
                ProjectSurveyModel.service_type_id == query_params.service_type_id
            )

        supporter_organization_id_list: List[str] = (
            query_params.organization_ids.split(",")
            if query_params.organization_ids
            else None
        )

        if query_params.type:
            if query_params.type == SurveyTypeForGetSurveys.SERVICE_AND_COMPLETION:
                filter_condition_project_id_actual_response &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
                ) | (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
                )
                filter_condition_data_type_actual_response &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
                ) | (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
                )

                filter_condition_project_id_summary_month &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
                ) | (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
                )
                filter_condition_data_type_plan_response &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
                ) | (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
                )
                filter_condition_data_type_summary_month &= (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
                ) | (
                    ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION
                )
            else:
                filter_condition_project_id_actual_response &= (
                    ProjectSurveyModel.survey_type == query_params.type
                )
                filter_condition_data_type_actual_response &= (
                    ProjectSurveyModel.survey_type == query_params.type
                )

                filter_condition_project_id_summary_month &= (
                    ProjectSurveyModel.survey_type == query_params.type
                )
                filter_condition_data_type_plan_response &= (
                    ProjectSurveyModel.survey_type == query_params.type
                )
                filter_condition_data_type_summary_month &= (
                    ProjectSurveyModel.survey_type == query_params.type
                )

        if supporter_organization_id_list:
            filter_condition_survey_type_actual_response &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )
            filter_condition_project_id_actual_response &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )
            filter_condition_data_type_actual_response &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )
            filter_condition_project_id_summary_month &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )
            filter_condition_survey_type_summary_month &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )
            filter_condition_data_type_plan_response &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )
            filter_condition_data_type_summary_month &= (
                ProjectSurveyModel.supporter_organization_id.is_in(
                    *supporter_organization_id_list
                )
                | ProjectSurveyModel.supporter_organization_id.does_not_exist()
                | (condition_size(ProjectSurveyModel.supporter_organization_id) == 0)
            )

        """案件アンケート情報の取得

        案件アンケート取得にはクエリパラメータの条件によって下記のGSIを使い分ける
        - アンケート種別回答受信日GSI
        - 案件ID回答受信日GSI
        - データ区分回答受信日GSI
        - 案件ID集計月GSI
        - アンケート種別集計月GSI
        - データ区分回答受信予定日GSI
        - データ区分集計月GSI用
        """

        logger.info("start executing query")
        project_survey_list: List[ProjectSurveyModel] = []

        if (
            query_params.actual_survey_response_date_from
            or query_params.actual_survey_response_date_to
        ):
            # 回答日
            if query_params.project_id:
                # 案件ID
                project_survey_list.extend(
                    list(
                        ProjectSurveyModel.project_id_actual_survey_response_datetime_index.query(
                            hash_key=query_params.project_id,
                            range_key_condition=range_key_condition_project_id_actual_response,
                            filter_condition=filter_condition_project_id_actual_response,
                        )
                    )
                )
            elif query_params.type:
                # アンケート種別
                if query_params.type == SurveyTypeForGetSurveys.SERVICE_AND_COMPLETION:
                    project_survey_list.extend(
                        list(
                            ProjectSurveyModel.survey_type_actual_survey_response_datetime_index.query(
                                hash_key=SurveyTypeForGetSurveys.SERVICE,
                                range_key_condition=range_key_condition_survey_type_actual_response,
                                filter_condition=filter_condition_survey_type_actual_response,
                            )
                        )
                    )
                    project_survey_list.extend(
                        list(
                            ProjectSurveyModel.survey_type_actual_survey_response_datetime_index.query(
                                hash_key=SurveyTypeForGetSurveys.COMPLETION,
                                range_key_condition=range_key_condition_survey_type_actual_response,
                                filter_condition=filter_condition_survey_type_actual_response,
                            )
                        )
                    )
                else:
                    project_survey_list.extend(
                        list(
                            ProjectSurveyModel.survey_type_actual_survey_response_datetime_index.query(
                                hash_key=query_params.type,
                                range_key_condition=range_key_condition_survey_type_actual_response,
                                filter_condition=filter_condition_survey_type_actual_response,
                            )
                        )
                    )
            else:
                project_survey_list.extend(
                    list(
                        ProjectSurveyModel.data_type_actual_survey_response_datetime_index.query(
                            hash_key=DataType.SURVEY,
                            range_key_condition=range_key_condition_data_type_actual_response,
                            filter_condition=filter_condition_data_type_actual_response,
                        )
                    )
                )
        elif query_params.summary_month_from or query_params.summary_month_to:
            # 集計月
            if query_params.project_id:
                # 案件ＩＤ
                project_survey_list.extend(
                    list(
                        ProjectSurveyModel.project_id_summary_month_index.query(
                            hash_key=query_params.project_id,
                            range_key_condition=range_key_condition_project_id_summary_month,
                            filter_condition=filter_condition_project_id_summary_month,
                        )
                    )
                )
            elif query_params.type:
                # アンケート種別
                if query_params.type == SurveyTypeForGetSurveys.SERVICE_AND_COMPLETION:
                    project_survey_list.extend(
                        list(
                            ProjectSurveyModel.survey_type_summary_month_index.query(
                                hash_key=SurveyTypeForGetSurveys.SERVICE,
                                range_key_condition=range_key_condition_survey_type_summary_month,
                                filter_condition=filter_condition_survey_type_summary_month,
                            )
                        )
                    )
                    project_survey_list.extend(
                        list(
                            ProjectSurveyModel.survey_type_summary_month_index.query(
                                hash_key=SurveyTypeForGetSurveys.COMPLETION,
                                range_key_condition=range_key_condition_survey_type_summary_month,
                                filter_condition=filter_condition_survey_type_summary_month,
                            )
                        )
                    )
                else:
                    project_survey_list.extend(
                        list(
                            ProjectSurveyModel.survey_type_summary_month_index.query(
                                hash_key=query_params.type,
                                range_key_condition=range_key_condition_survey_type_summary_month,
                                filter_condition=filter_condition_survey_type_summary_month,
                            )
                        )
                    )
            else:
                # データ区分
                project_survey_list.extend(
                    list(
                        ProjectSurveyModel.data_type_summary_month_index.query(
                            hash_key=DataType.SURVEY,
                            range_key_condition=range_key_condition_data_type_summary_month,
                            filter_condition=filter_condition_data_type_summary_month,
                        )
                    )
                )

        elif (
            query_params.plan_survey_response_date_from
            or query_params.plan_survey_response_date_to
        ):
            # 回答受信予定日の場合は他のクエリパラメータは指定されない。
            # データ区分
            project_survey_list.extend(
                list(
                    ProjectSurveyModel.data_type_plan_survey_response_datetime_index.query(
                        hash_key=DataType.SURVEY,
                        range_key_condition=range_key_condition_data_type_plan_response,
                        filter_condition=filter_condition_data_type_plan_response,
                    )
                )
            )

        else:
            # 期間に指定がない場合は、データ区分集計月GSIで取得
            project_survey_list.extend(
                list(
                    ProjectSurveyModel.data_type_summary_month_index.query(
                        hash_key=DataType.SURVEY,
                        range_key_condition=range_key_condition_data_type_summary_month,
                        filter_condition=filter_condition_data_type_summary_month,
                    )
                )
            )

        logger.info("start editing data for response")
        # レスポンス編集
        user_list: List[UserModel] = list(
            UserModel.data_type_name_index.query(hash_key=DataType.USER)
        )
        project_list: List[ProjectModel] = list(
            ProjectModel.data_type_name_index.query(hash_key=DataType.PROJECT)
        )

        survey_info_response_list: List[SurveyInfoForGetSurveysResponse] = []
        target_summary_survey_model_list: List[ProjectSurveyModel] = []
        for survey in project_survey_list:
            # 権限チェック
            if not SurveyService.is_visible_survey_for_get_surveys(
                current_user=current_user, project_list=project_list, survey=survey
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

            logger.info("start editing survey.attribute_values")
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
        # ・アンケート種別がクイックアンケートの場合はサマリー項目の返却は不要
        logger.info("start editing summary")
        summary: SummaryInfoForGetSurveysResponse = None
        if query_params.type != SurveyTypeForGetSurveys.QUICK:
            # 集計年度の算出
            if query_params.summary_month_to:
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

        return GetSurveysResponse(
            summary=summary,
            surveys=survey_info_response_list,
        )

    def get_survey_summary_reports(
        query_params: GetSurveySummaryReportsQuery, current_user: AdminModel
    ) -> GetSurveySummaryReportsResponse:
        """アンケート月次レポート取得

        Args:
            query_params (GetSurveySummaryReportsQuery):
            current_user (AdminModel): 認証済みユーザー

        Returns:
            取得結果 (GetSurveySummaryReportsResponse): 取得結果
        """
        # 集計範囲の算出
        summary_month_to = datetime.strptime(query_params.summary_month, "%Y%m")

        if summary_month_to.month in [1, 2, 3]:
            fiscal_year = summary_month_to.year - 1
            fiscal_year_start_month = datetime.strftime(
                date(fiscal_year, 4, 1), "%Y/%m"
            )
        else:
            fiscal_year_start_month = datetime.strftime(
                date(summary_month_to.year, 4, 1), "%Y/%m"
            )

        summary_month_to = summary_month_to.strftime("%Y/%m")
        # 案件アンケートを集計月までの年度分を取得
        survey_list: List[ProjectSurveyModel] = list(
            ProjectSurveyModel.data_type_summary_month_index.query(
                hash_key=DataType.SURVEY,
                range_key_condition=ProjectSurveyModel.summary_month.between(
                    fiscal_year_start_month, summary_month_to
                ),
            )
        )

        service_summary = SurveySummaryReports()
        completion_summary = SurveySummaryReports()
        service_and_completion_summary = SurveySummaryReports()
        pp_summary = SurveySummaryReports()

        project_list: List[ProjectModel] = list(
            ProjectModel.data_type_name_index.query(hash_key=DataType.PROJECT)
        )

        for survey in survey_list:
            if not SurveyService.is_visible_survey_for_get_surveys(
                current_user=current_user, project_list=project_list, survey=survey
            ):
                # 案件アクセス権が無ければ集計対象に含まない
                continue
            if survey.survey_type == SurveyType.SERVICE:
                # サービスアンケート、サービスアンケート・修了アンケート合算集計
                if (
                    survey.plan_survey_request_datetime
                    or survey.actual_survey_request_datetime
                ):
                    service_summary.summary_plan += 1
                    service_and_completion_summary.summary_plan += 1

                if survey.actual_survey_response_datetime:
                    service_summary.summary_collect += 1
                    service_and_completion_summary.summary_collect += 1

                if survey.summary_month == summary_month_to:
                    # 単月
                    if (
                        survey.plan_survey_request_datetime
                        or survey.actual_survey_request_datetime
                    ):
                        service_summary.plan += 1
                        service_and_completion_summary.plan += 1
                    if survey.actual_survey_response_datetime:
                        service_summary.collect += 1
                        service_and_completion_summary.collect += 1

            if survey.survey_type == SurveyType.COMPLETION:
                # 修了アンケート、サービスアンケート・修了アンケート合算集計
                if (
                    survey.plan_survey_request_datetime
                    or survey.actual_survey_request_datetime
                ):
                    completion_summary.summary_plan += 1
                    service_and_completion_summary.summary_plan += 1

                if survey.actual_survey_response_datetime:
                    completion_summary.summary_collect += 1
                    service_and_completion_summary.summary_collect += 1

                if survey.summary_month == summary_month_to:
                    # 単月
                    if (
                        survey.plan_survey_request_datetime
                        or survey.actual_survey_request_datetime
                    ):
                        completion_summary.plan += 1
                        service_and_completion_summary.plan += 1

                    if survey.actual_survey_response_datetime:
                        completion_summary.collect += 1
                        service_and_completion_summary.collect += 1

            if survey.survey_type == SurveyType.PP:
                # Partner Portal利用アンケート
                if (
                    survey.plan_survey_request_datetime
                    or survey.actual_survey_request_datetime
                ):
                    pp_summary.summary_plan += 1
                if survey.actual_survey_response_datetime:
                    pp_summary.summary_collect += 1

                if survey.summary_month == summary_month_to:
                    # 単月
                    if (
                        survey.plan_survey_request_datetime
                        or survey.actual_survey_request_datetime
                    ):
                        pp_summary.plan += 1
                    if survey.actual_survey_response_datetime:
                        pp_summary.collect += 1

        # アンケート回収率
        SurveyService.calculate_return_rate(service_summary)
        SurveyService.calculate_return_rate(completion_summary)
        SurveyService.calculate_return_rate(service_and_completion_summary)
        SurveyService.calculate_return_rate(pp_summary)

        return GetSurveySummaryReportsResponse(
            summary_month=summary_month_to,
            service=service_summary,
            completion=completion_summary,
            service_and_completion=service_and_completion_summary,
            pp=pp_summary,
        )

    @staticmethod
    def update_survey_by_id(
        survey_id: str,
        version: int,
        item: UpdateSurveyByIdRequest,
        current_user: AdminModel,
    ) -> OKResponse:
        """案件アンケート更新

        Args:
            survey_id (str): 案件アンケートID
            version (int): バージョン
            item (UpdateSurveyByIdRequest): 更新内容
            current_user (AdminModel): 認証済みのユーザー

        Raises:
            HTTPException: 403 権限なし
            HTTPException: 409 排他チェック

        Returns:
            OKResponse: 更新結果
        """
        # 対象アンケートアンケート項目取得
        project_survey = ProjectSurveyModel.get(
            hash_key=survey_id, range_key=DataType.SURVEY
        )
        # 排他チェック
        if version != project_survey.version:
            logger.warning(
                f"UpdateSurveyById conflict. request_ver:{version} project_survey_ver: {project_survey.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT)

        # 権限チェック
        if not SurveyService.is_visible_survey_for_get_survey_by_id(
            current_user=current_user,
            project_id=project_survey.project_id,
        ):
            logger.warning("UpdateSurveyById forbidden.")
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN)

        # 更新
        project_survey.update(
            actions=[
                ProjectSurveyModel.summary_month.set(item.summary_month),
                ProjectSurveyModel.is_not_summary.set(item.is_not_summary),
                ProjectSurveyModel.is_solver_project.set(item.is_solver_project),
            ]
        )

        return OKResponse()

    @staticmethod
    def get_survey_plans(
        query_params: GetSurveyPlansQuery, current_user: AdminModel
    ) -> GetSurveyPlansResponse:
        """案件アンケート予実績リストを取得

        Args:
            query_params (GetSurveyPlansQuery): クエリパラメータ
            current_user (Behavior, optional): 認証済みのユーザー
        Raises:

        Returns:
            GetSurveyPlansResponse: 取得結果
        """
        # 集計月
        # fromは01日、toに31日を補完
        summary_month_from_ymd = (
            datetime.strptime(query_params.summary_month_from, "%Y%m").strftime(
                "%Y/%m/01"
            )
            if query_params.summary_month_from
            else DateTime.MINIMUM
        )
        summary_month_to_ymd = (
            datetime.strptime(query_params.summary_month_to, "%Y%m").strftime(
                "%Y/%m/31"
            )
            if query_params.summary_month_to
            else DateTime.MAXIMUM
        )

        summary_month_from_ym = (
            datetime.strptime(query_params.summary_month_from, "%Y%m").strftime("%Y/%m")
            if query_params.summary_month_from
            else Date.MINIMUM
        )
        summary_month_to_ym = (
            datetime.strptime(query_params.summary_month_to, "%Y%m").strftime("%Y/%m")
            if query_params.summary_month_to
            else Date.MAXIMUM
        )

        # 受信予定日
        plan_survey_response_date_from_dt = (
            datetime.strptime(
                query_params.plan_survey_response_date_from, "%Y%m%d"
            ).strftime("%Y/%m/%d")
            + " "
            + HourMinutes.MINIMUM
            if query_params.plan_survey_response_date_from
            else DateTimeHourMinutes.MINIMUM
        )
        plan_survey_response_date_to_dt = (
            datetime.strptime(
                query_params.plan_survey_response_date_to, "%Y%m%d"
            ).strftime("%Y/%m/%d")
            + " "
            + HourMinutes.MAXIMUM
            if query_params.plan_survey_response_date_to
            else DateTimeHourMinutes.MAXIMUM
        )

        # 案件情報を範囲検索する為、fromは01日、toに31日を設定
        plan_survey_response_date_from_ymd = (
            datetime.strptime(
                query_params.plan_survey_response_date_from, "%Y%m%d"
            ).strftime("%Y/%m/01")
            if query_params.plan_survey_response_date_from
            else DateTime.MINIMUM
        )
        plan_survey_response_date_to_ymd = (
            datetime.strptime(
                query_params.plan_survey_response_date_to, "%Y%m%d"
            ).strftime("%Y/%m/31")
            if query_params.plan_survey_response_date_to
            else DateTime.MAXIMUM
        )

        # 案件情報の取得
        range_key_condition_project = None
        filter_condition_project = None

        if query_params.summary_month_from or query_params.summary_month_to:
            # from以降に支援終了日がある案件情報が対象
            range_key_condition_project &= (
                ProjectModel.support_date_to >= summary_month_from_ymd
            )
            # to以前に支援開始日がある
            filter_condition_project &= (
                ProjectModel.support_date_from <= summary_month_to_ymd
            )
        elif (
            query_params.plan_survey_response_date_from
            or query_params.plan_survey_response_date_to
        ):
            range_key_condition_project &= (
                ProjectModel.support_date_to >= plan_survey_response_date_from_ymd
            )
            filter_condition_project &= (
                ProjectModel.support_date_from <= plan_survey_response_date_to_ymd
            )

        project_list: List[ProjectModel] = list(
            ProjectModel.data_type_support_date_to_index.query(
                hash_key=DataType.PROJECT,
                range_key_condition=range_key_condition_project,
                filter_condition=filter_condition_project,
            )
        )

        # 一般ユーザ情報の取得
        user_list: List[UserModel] = list(UserModel.scan())
        # 汎用マスタ:支援者組織情報
        master_supporter_organization_list: List[MasterSupporterOrganizationModel] = (
            list(
                MasterSupporterOrganizationModel.data_type_index.query(
                    hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value
                )
            )
        )
        # 汎用マスタ:サービス種別
        master_service_type_list: List[MasterSupporterOrganizationModel] = list(
            MasterSupporterOrganizationModel.data_type_index.query(
                hash_key=MasterDataType.MASTER_SERVICE_TYPE.value
            )
        )

        survey_info_response_list: List[SurveyInfoForGetSurveyPlansResponse] = []

        # 案件アンケート抽出条件
        # アンケート種別: サービスアンケート, 修了アンケート
        filter_condition_survey = None
        filter_condition_survey &= (
            ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.SERVICE
        ) | (ProjectSurveyModel.survey_type == SurveyTypeForGetSurveys.COMPLETION)

        # 案件アンケート取得
        survey_list: List[ProjectSurveyModel] = []
        if query_params.summary_month_from or query_params.summary_month_to:
            # 集計月指定時
            range_key_condition_survey = None
            range_key_condition_survey &= ProjectSurveyModel.summary_month.between(
                summary_month_from_ym, summary_month_to_ym
            )
            survey_list = list(
                ProjectSurveyModel.data_type_summary_month_index.query(
                    hash_key=DataType.SURVEY,
                    range_key_condition=range_key_condition_survey,
                    filter_condition=filter_condition_survey,
                )
            )
        elif (
            query_params.plan_survey_response_date_from
            or query_params.plan_survey_response_date_to
        ):
            # 受信予定日指定時
            range_key_condition_survey = None
            range_key_condition_survey &= (
                ProjectSurveyModel.plan_survey_response_datetime.between(
                    plan_survey_response_date_from_dt,
                    plan_survey_response_date_to_dt,
                )
            )
            survey_list = list(
                ProjectSurveyModel.data_type_plan_survey_response_datetime_index.query(
                    hash_key=DataType.SURVEY,
                    range_key_condition=range_key_condition_survey,
                    filter_condition=filter_condition_survey,
                )
            )
        else:
            # 指定なし
            survey_list = list(
                ProjectSurveyModel.data_type_summary_month_index.query(
                    hash_key=DataType.SURVEY,
                    filter_condition=filter_condition_survey,
                )
            )
        # 案件IDをキーとする辞書を作成
        #  key: 案件ID
        #  value: 案件IDに紐づくアンケート情報リスト
        survey_list_map: dict[str, List[ProjectSurveyModel]] = {}
        for survey in survey_list:
            if survey.project_id in survey_list_map.keys():
                temp_list = survey_list_map[survey.project_id]
                temp_list.append(survey)
                survey_list_map[survey.project_id] = temp_list
            else:
                survey_list_map[survey.project_id] = [survey]

        # 案件単位で処理
        for project in project_list:
            # 権限チェック
            if not SurveyService.is_visible_survey_for_get_survey_plans(
                current_user=current_user,
                project=project,
            ):
                # アクセス不可の場合はスキップ
                continue

            # 案件に紐づくアンケート情報リストを取得
            survey_list_by_project = survey_list_map.get(project.id)

            # レスポンス編集
            if survey_list_by_project:
                # 送信済みアンケートは案件アンケート, 未送信アンケートは案件情報を基に編集
                for survey in survey_list_by_project:
                    if survey.actual_survey_request_datetime:
                        # 送信済みアンケートの場合
                        answer_user_email = ""
                        for user in user_list:
                            if (
                                survey.answer_user_id
                                and user.id == survey.answer_user_id
                            ):
                                answer_user_email = user.email
                                break

                        survey_user_type = None
                        survey_user_name = ""
                        survey_user_email = ""
                        if survey.answer_user_id:
                            survey_user_type = SurveyUserType.REGISTERED_MAIN_CUSTOMER
                            survey_user_name = survey.answer_user_name
                            survey_user_email = answer_user_email
                        elif not survey.dedicated_survey_user_email:
                            # 匿名回答アンケートのメールアドレス設定なし
                            pass
                        elif (
                            survey.dedicated_survey_user_email
                            == project.salesforce_main_customer.email
                        ):
                            survey_user_type = SurveyUserType.SALESFORCE_MAIN_CUSTOMER
                            survey_user_name = survey.dedicated_survey_user_name
                            survey_user_email = survey.dedicated_survey_user_email
                        else:
                            survey_user_type = SurveyUserType.MANUAL_SETTING
                            survey_user_name = survey.dedicated_survey_user_name
                            survey_user_email = survey.dedicated_survey_user_email

                        main_supporter_user: SupporterUser = None
                        if survey.main_supporter_user:
                            main_supporter_user = SupporterUser(
                                id=survey.main_supporter_user.id,
                                name=survey.main_supporter_user.name,
                                organization_id=survey.main_supporter_user.organization_id,
                                organization_name=survey.main_supporter_user.organization_name,
                            )
                        supporter_users: List[SupporterUser] = []
                        if survey.supporter_users:
                            for supporter in survey.supporter_users:
                                temp_supporter_user: SupporterUser = SupporterUser(
                                    id=supporter.id,
                                    name=supporter.name,
                                    organization_id=supporter.organization_id,
                                    organization_name=supporter.organization_name,
                                )
                                supporter_users.append(temp_supporter_user)
                        else:
                            # アクセラレーターが設定されていない場合、サービス責任者をセット
                            if (
                                survey.service_manager_id
                                and survey.service_manager_name
                            ):
                                supporter_users.append(
                                    SupporterUser(
                                        id=survey.service_manager_id,
                                        name=survey.service_manager_name,
                                        organization_id=None,
                                        organization_name=None,
                                    )
                                )

                        sales_user_name = ""
                        for user in user_list:
                            if user.id == survey.sales_user_id:
                                sales_user_name = user.name
                                break

                        # 「単月指定」以外の場合
                        # 「期間指定」の[plan_survey_response_date_from]が含まれている場合
                        # 連続未回答数をNone
                        if (
                            query_params.summary_month_from
                            != query_params.summary_month_to
                            or query_params.plan_survey_response_date_from
                        ):
                            survey.unanswered_surveys_number = None

                        response_item = SurveyInfoForGetSurveyPlansResponse(
                            project_id=survey.project_id,
                            project_name=survey.project_name,
                            id=survey.id,
                            survey_type=survey.survey_type,
                            customer_success=survey.customer_success,
                            supporter_organization_id=survey.supporter_organization_id,
                            supporter_organization_name=survey.supporter_organization_name,
                            support_date_from=survey.support_date_from,
                            support_date_to=survey.support_date_to,
                            main_supporter_user=main_supporter_user,
                            supporter_users=supporter_users,
                            sales_user_id=survey.sales_user_id,
                            sales_user_name=sales_user_name,
                            service_type_id=survey.service_type_id,
                            service_type_name=survey.service_type_name,
                            survey_user_type=survey_user_type,
                            survey_user_name=survey_user_name,
                            survey_user_email=survey_user_email,
                            customer_id=survey.customer_id,
                            customer_name=survey.customer_name,
                            summary_month=survey.summary_month,
                            plan_survey_request_datetime=survey.plan_survey_request_datetime,
                            actual_survey_request_datetime=survey.actual_survey_request_datetime,
                            plan_survey_response_datetime=survey.plan_survey_response_datetime,
                            actual_survey_response_datetime=survey.actual_survey_response_datetime,
                            phase=project.phase,
                            is_count_man_hour=project.is_count_man_hour,
                            unanswered_surveys_number=survey.unanswered_surveys_number,
                            # NPFから取得したもの
                            this_month_type=survey.this_month_type,
                            no_send_reason=survey.no_send_reason,
                        )
                    else:
                        # 未送信アンケートの場合
                        response_item = SurveyService.edit_unsent_response_for_get_survey_plans(
                            project=project,
                            survey=survey,
                            user_list=user_list,
                            master_supporter_organization_list=master_supporter_organization_list,
                            master_service_type_list=master_service_type_list,
                        )

                    # 送信済みアンケート、未送信アンケート分をリストに追加
                    survey_info_response_list.append(response_item)
            else:
                # アンケートスケジュール未登録の場合, 案件情報を基に編集
                response_item = SurveyService.edit_response_based_on_project_info_for_get_survey_plans(
                    project=project,
                    user_list=user_list,
                    master_supporter_organization_list=master_supporter_organization_list,
                    master_service_type_list=master_service_type_list,
                )

                # アンケートスケジュール未登録分をリストに追加
                survey_info_response_list.append(response_item)

        return GetSurveyPlansResponse(
            surveys=survey_info_response_list,
        )

    @staticmethod
    def resend_survey_by_id(
        survey_id: str,
        current_user: UserModel,
    ) -> OKResponse:
        """案件アンケートIDを指定して案件アンケート回答依頼通知（メール、お知らせ）を再送信する

        Args:
            survey_id (str): 案件アンケートID
            current_user (UserModel): API呼出ユーザー
        Raises:
            HTTPException: 400 Bad request
            HTTPException: 404 Not found
            HTTPException: 409 Conflict

        Returns:
            OKResponse: 取得結果
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

        # バリデーションチェック
        # 回答依頼送信済みでないアンケートは再送信不可
        if not survey.actual_survey_request_datetime:
            logger.warning(
                "ResendSurveyById bad request. Surveys that have not been requested to respond cannot be resent."
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Surveys that have not been requested to respond cannot be resent.",
            )
        # 回答済みのアンケートは再送信不可
        if survey.actual_survey_response_datetime:
            logger.warning(
                "ResendSurveyById bad request. The answered survey cannot be resent."
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="The answered survey cannot be resent.",
            )
        # PPアンケートは再送信不可
        if survey.survey_type == SurveyType.PP:
            logger.warning(
                "ResendSurveyById bad request. The PP survey cannot be resent."
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="The PP survey cannot be resent.",
            )

        # 案件情報の取得
        if survey.project_id:
            try:
                project = ProjectModel.get(
                    hash_key=survey.project_id, range_key=DataType.PROJECT
                )
            except DoesNotExist:
                logger.warning(
                    f"Project_id of the survey not found. project_id:{survey.project_id}"
                )
                raise HTTPException(
                    status_code=status.HTTP_400_BAD_REQUEST,
                    detail="Project_id of the survey not found.",
                )
        else:
            logger.warning("Project_id of the survey is empty.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Project_id of the survey is empty.",
            )

        # 権限チェック
        if not SurveyService.is_visible_survey_for_resend_survey_by_id(
            current_user=current_user,
        ):
            logger.warning("ResendSurveyById not found.")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        # 一般ユーザ情報の取得
        user_list: List[UserModel] = list(UserModel.scan())
        # 汎用マスタ:支援者組織情報
        master_supporter_organization_list: List[MasterSupporterOrganizationModel] = (
            list(
                MasterSupporterOrganizationModel.data_type_index.query(
                    hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value
                )
            )
        )
        # 汎用マスタ:サービス種別
        master_service_type_list: List[MasterSupporterOrganizationModel] = list(
            MasterSupporterOrganizationModel.data_type_index.query(
                hash_key=MasterDataType.MASTER_SERVICE_TYPE.value
            )
        )
        # 管理ユーザ
        admin_filter_condition = AdminModel.disabled == False  # NOQA
        admin_list: List[AdminModel] = list(
            AdminModel.scan(filter_condition=admin_filter_condition)
        )
        # アンケートマスタ取得（最新バージョン）
        survey_master_list: List[SurveyMasterModel] = list(
            SurveyMasterModel.is_latest_name_index.query(hash_key=1)
        )
        # 通常回答アンケートの通知関連情報
        notification_data: dict = {}
        # 匿名回答アンケートの通知関連情報
        anonymous_notification_data: dict = {}

        # 通常回答アンケート用
        notification_id_list: List[str] = []
        to_mail_list: List[str] = []
        cc_mail_list: List[str] = []
        bcc_mail_list: List[str] = []
        message_param: dict = {}
        # 匿名回答アンケート用
        anonymous_notification_id_list: List[str] = []
        anonymous_to_mail_list: List[str] = []
        anonymous_cc_mail_list: List[str] = []
        anonymous_bcc_mail_list: List[str] = []
        anonymous_message_param: dict = {}

        # アンケートマスタの最新バージョン取得
        survey_master_revision = ""
        for survey_master in survey_master_list:
            if survey.survey_master_id == survey_master.id:
                survey_master_revision = survey_master.revision
                break
        if not survey_master_revision:
            # アンケートマスタが取得できない場合
            logger.warning(
                f"Unable to get a survey master info. survey_id:{survey.id}, survey_master_id:{survey.survey_master_id}"
            )
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail=f"Unable to get a survey master info. survey_id:{survey.id}, survey_master_id:{survey.survey_master_id}",
            )

        # 案件アンケートの更新
        update_action: List[Action] = []
        # アンケートマスターバージョン番号
        update_action.append(
            ProjectSurveyModel.survey_master_revision.set(survey_master_revision)
        )
        # 上記以外の情報
        update_action.append(ProjectSurveyModel.project_name.set(project.name))
        update_action.append(
            ProjectSurveyModel.customer_success.set(project.customer_success)
        )
        update_action.append(
            ProjectSurveyModel.supporter_organization_id.set(
                project.supporter_organization_id
            )
        )
        supporter_organization_name: str = None
        for supporter_organization in master_supporter_organization_list:
            if survey.supporter_organization_id == supporter_organization.id:
                supporter_organization_name = supporter_organization.value
                break
        update_action.append(
            ProjectSurveyModel.supporter_organization_name.set(
                supporter_organization_name
            )
        )
        update_action.append(
            ProjectSurveyModel.support_date_from.set(project.support_date_from)
        )
        update_action.append(
            ProjectSurveyModel.support_date_to.set(project.support_date_to)
        )
        main_supporter_user: SupporterUserAttribute = (
            SurveyService.edit_supporter_user_attribute(
                supporter_user_id=project.main_supporter_user_id,
                user_list=user_list,
                master_supporter_organization_list=master_supporter_organization_list,
            )
        )
        update_action.append(
            ProjectSurveyModel.main_supporter_user.set(main_supporter_user)
        )
        supporter_users: List[SupporterUserAttribute] = None
        if project.supporter_user_ids:
            supporter_users = []
            for supporter_user_id in project.supporter_user_ids:
                supporter_users.append(
                    SurveyService.edit_supporter_user_attribute(
                        supporter_user_id=supporter_user_id,
                        user_list=user_list,
                        master_supporter_organization_list=master_supporter_organization_list,
                    )
                )
        update_action.append(ProjectSurveyModel.supporter_users.set(supporter_users))
        update_action.append(
            ProjectSurveyModel.sales_user_id.set(project.main_sales_user_id)
        )
        update_action.append(
            ProjectSurveyModel.service_type_id.set(project.service_type)
        )
        service_type_name: str = None
        for master_service_type in master_service_type_list:
            if project.service_type == master_service_type.id:
                service_type_name = master_service_type.name
                break
        update_action.append(
            ProjectSurveyModel.service_type_name.set(service_type_name)
        )
        update_action.append(
            ProjectSurveyModel.dedicated_survey_user_name.set(
                project.dedicated_survey_user_name
            )
        )
        update_action.append(
            ProjectSurveyModel.dedicated_survey_user_email.set(
                project.dedicated_survey_user_email
            )
        )
        update_action.append(
            ProjectSurveyModel.answer_user_id.set(project.main_customer_user_id)
        )
        answer_user_name: str = None
        for user in user_list:
            if project.main_customer_user_id == user.id:
                answer_user_name = user.name
                break
        update_action.append(ProjectSurveyModel.answer_user_name.set(answer_user_name))
        update_action.append(ProjectSurveyModel.customer_id.set(project.customer_id))
        update_action.append(
            ProjectSurveyModel.customer_name.set(project.customer_name)
        )
        update_action.append(ProjectSurveyModel.update_id.set(current_user.id))
        update_action.append(ProjectSurveyModel.update_at.set(datetime.now()))
        survey.update(actions=update_action)

        if not survey.answer_user_id and not survey.dedicated_survey_user_email:
            # 以下がいずれもセットされていない場合、エラー
            # ・アンケート専用メールアドレス（匿名回答アンケート）
            # ・回答ユーザID
            logger.warning("Email of the survey is empty.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Email of the survey is empty.",
            )

        # アンケート宛先の優先順位
        # ・お客様代表: 通常アンケート（入力されている場合）
        # ・匿名アンケート送信宛先: 匿名回答アンケート（入力されている場合）
        if survey.answer_user_id:
            # 通常回答アンケートの場合
            # お知らせ通知ユーザ/メール通知アドレス
            for user in user_list:
                # 有効なユーザーのみに通知を送る
                if not user.disabled:
                    # お客様
                    if survey.answer_user_id == user.id:
                        notification_id_list.append(survey.answer_user_id)
                        to_mail_list.append(user.email)
                    # 営業担当者あるいは営業責任者
                    if survey.sales_user_id == user.id:
                        notification_id_list.append(survey.sales_user_id)
                        cc_mail_list.append(user.email)
            for admin in admin_list:
                if UserRoleType.SURVEY_OPS.key in admin.roles:
                    bcc_mail_list.append(admin.email)

            # お知らせ・メールの本文の編集
            message_param["customer_name_project_name"] = (
                "{customer}／{project}".format(
                    customer=survey.customer_name,
                    project=survey.project_name,
                )
            )
            message_param["customer_name"] = "{customer}".format(
                customer=survey.customer_name,
            )
            message_param["project_name"] = "{project}".format(
                project=survey.project_name,
            )
            message_param["summary_month"] = "{year}年{month}月".format(
                year=survey.summary_month[0:4],
                month=survey.summary_month[5:],
            )
            message_param["formatted_month"] = "{month}".format(
                month=int(survey.summary_month[5:]),
            )
            message_param["answer_user_name"] = answer_user_name
            survey_type_name = ""
            if survey.survey_type == SurveyType.SERVICE:
                survey_type_name = SendSurveyTypeName.SERVICE
            elif survey.survey_type == SurveyType.COMPLETION:
                survey_type_name = SendSurveyTypeName.COMPLETION
            elif survey.survey_type == SurveyType.QUICK:
                survey_type_name = SendSurveyTypeName.QUICK
            message_param["survey_type_name"] = survey_type_name
            # URL
            fo_site_url = get_app_settings().fo_site_url
            fo_survey_detail_url = fo_site_url + FoAppUrl.SURVEY_DETAIL.format(
                surveyId=survey.id
            )
            message_param["fo_survey_detail_url"] = fo_survey_detail_url

            if survey.plan_survey_response_datetime:
                # ○月○日(曜日) ※ゼロ埋めなし
                datetime_due_date = datetime.strptime(
                    survey.plan_survey_response_datetime, "%Y/%m/%d %H:%M"
                )
                message_param["survey_answer_due_date"] = (
                    f"{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日"
                    + f"({get_day_of_week(datetime_due_date)})"
                )
                message_param["survey_answer_due_date_title"] = (
                    f"（回答期限：{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日迄）"
                )
            else:
                message_param["survey_answer_due_date"] = ""
                message_param["survey_answer_due_date_title"] = ""

            notification_data["notification_id_list"] = notification_id_list
            notification_data["to_mail_list"] = to_mail_list
            notification_data["cc_mail_list"] = cc_mail_list
            notification_data["bcc_mail_list"] = bcc_mail_list
            notification_data["message_param"] = message_param

        elif survey.dedicated_survey_user_email:
            # 匿名回答アンケートの場合

            # アンケートパスワード復号化
            survey_password_decrypted = AesCipherUtils.decrypt(project.survey_password)

            # メール通知アドレス
            anonymous_to_mail_list.append(survey.dedicated_survey_user_email)
            for user in user_list:
                # 有効なユーザーのみに通知を送る
                if not user.disabled:
                    # 営業担当者あるいは営業責任者
                    if survey.sales_user_id == user.id:
                        anonymous_cc_mail_list.append(user.email)
                        break
            for admin in admin_list:
                if UserRoleType.SURVEY_OPS.key in admin.roles:
                    anonymous_bcc_mail_list.append(admin.email)

            # お知らせ・メールの本文の編集
            anonymous_message_param["customer_name_project_name"] = (
                "{customer}／{project}".format(
                    customer=survey.customer_name,
                    project=survey.project_name,
                )
            )
            anonymous_message_param["customer_name"] = "{customer}".format(
                customer=survey.customer_name,
            )
            anonymous_message_param["project_name"] = "{project}".format(
                project=survey.project_name,
            )
            anonymous_message_param["summary_month"] = "{year}年{month}月".format(
                year=survey.summary_month[0:4],
                month=survey.summary_month[5:],
            )
            anonymous_message_param["formatted_month"] = "{month}".format(
                month=int(survey.summary_month[5:]),
            )
            anonymous_message_param["answer_user_name"] = (
                survey.dedicated_survey_user_name
            )
            survey_type_name = ""
            if survey.survey_type == SurveyType.SERVICE:
                survey_type_name = SendSurveyTypeName.SERVICE
            elif survey.survey_type == SurveyType.COMPLETION:
                survey_type_name = SendSurveyTypeName.COMPLETION
            elif survey.survey_type == SurveyType.QUICK:
                survey_type_name = SendSurveyTypeName.QUICK
            anonymous_message_param["survey_type_name"] = survey_type_name
            # URL
            fo_site_url = get_app_settings().fo_site_url
            jwt_current_datetime = get_datetime_now()
            jwt_survey_payload = create_jwt_survey_payload(
                survey_id=survey.id, current_datetime=jwt_current_datetime
            )
            jwt = create_jwt(jwt_survey_payload)
            anonymous_fo_survey_detail_url = (
                fo_site_url
                + FoAppUrl.ANONYMOUS_SURVEY
                + JwtSettingInfo.URL_JWT_QUERY.format(jwt=jwt)
            )
            anonymous_message_param["fo_survey_detail_url"] = (
                anonymous_fo_survey_detail_url
            )
            anonymous_message_param["survey_password"] = survey_password_decrypted

            if survey.plan_survey_response_datetime:
                # ○月○日(曜日) ※ゼロ埋めなし
                datetime_due_date = datetime.strptime(
                    survey.plan_survey_response_datetime, "%Y/%m/%d %H:%M"
                )
                anonymous_message_param["survey_answer_due_date"] = (
                    f"{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日"
                    + f"({get_day_of_week(datetime_due_date)})"
                )
                anonymous_message_param["survey_answer_due_date_title"] = (
                    f"（回答期限：{str(datetime_due_date.month)}月{str(datetime_due_date.day)}日迄）"
                )
            else:
                anonymous_message_param["survey_answer_due_date"] = ""
                anonymous_message_param["survey_answer_due_date_title"] = ""

            anonymous_message_param["project_identification_id"] = project.id

            anonymous_notification_data["notification_id_list"] = (
                anonymous_notification_id_list
            )
            anonymous_notification_data["to_mail_list"] = anonymous_to_mail_list
            anonymous_notification_data["cc_mail_list"] = anonymous_cc_mail_list
            anonymous_notification_data["bcc_mail_list"] = anonymous_bcc_mail_list
            anonymous_notification_data["message_param"] = anonymous_message_param

        notification_notice_at = datetime.now()
        if notification_data:
            # お知らせ通知
            # お知らせ「アンケート回答依頼通知」
            # 対象: 通常回答アンケート
            payload_survey_answer_request = {
                "customer_name_project_name": notification_data["message_param"][
                    "customer_name_project_name"
                ],
                "summary_month": notification_data["message_param"]["summary_month"],
                "survey_type_name": notification_data["message_param"][
                    "survey_type_name"
                ],
                "fo_survey_detail_url": notification_data["message_param"][
                    "fo_survey_detail_url"
                ],
                "survey_answer_due_date_title": notification_data["message_param"][
                    "survey_answer_due_date_title"
                ],
                "survey_answer_due_date": notification_data["message_param"][
                    "survey_answer_due_date"
                ],
            }
            notification_user_id_list = list(
                set(notification_data["notification_id_list"])
            )
            NotificationService.save_notification(
                notification_type=NotificationType.SURVEY_ANSWER_REQUEST,
                user_id_list=notification_user_id_list,
                message_param=payload_survey_answer_request,
                url=notification_data["message_param"]["fo_survey_detail_url"],
                noticed_at=notification_notice_at,
                create_id=current_user.id,
                update_id=current_user.id,
            )

            # メール通知
            # 「アンケート回答依頼通知」メール
            # 対象: 通常回答アンケート
            to_addr_list = list(set(notification_data["to_mail_list"]))
            cc_addr_list = list(set(notification_data["cc_mail_list"]))
            bcc_addr_list = list(set(notification_data["bcc_mail_list"]))
            payload_survey_answer_request: dict = {
                "customer_name": notification_data["message_param"]["customer_name"],
                "project_name": notification_data["message_param"]["project_name"],
                "summary_month": notification_data["message_param"]["summary_month"],
                "formatted_month": notification_data["message_param"][
                    "formatted_month"
                ],
                "answer_user_name": notification_data["message_param"][
                    "answer_user_name"
                ],
                "survey_type_name": notification_data["message_param"][
                    "survey_type_name"
                ],
                "fo_survey_detail_url": notification_data["message_param"][
                    "fo_survey_detail_url"
                ],
                "survey_answer_due_date_title": notification_data["message_param"][
                    "survey_answer_due_date_title"
                ],
                "survey_answer_due_date": notification_data["message_param"][
                    "survey_answer_due_date"
                ],
            }
            # 送信
            ProjectService.send_mail(
                template=MailType.SURVEY_ANSWER_REQUEST,
                to_addr_list=to_addr_list,
                cc_addr_list=cc_addr_list,
                bcc_addr_list=bcc_addr_list,
                payload=payload_survey_answer_request,
            )

        if anonymous_notification_data:
            # メール通知
            # 「匿名アンケート回答依頼通知」メール
            # 対象: 匿名回答アンケート
            to_addr_list = list(set(anonymous_notification_data["to_mail_list"]))
            cc_addr_list = list(set(anonymous_notification_data["cc_mail_list"]))
            bcc_addr_list = list(set(anonymous_notification_data["bcc_mail_list"]))
            payload_survey_anonymous_answer_request: dict = {
                "customer_name": anonymous_notification_data["message_param"][
                    "customer_name"
                ],
                "project_name": anonymous_notification_data["message_param"][
                    "project_name"
                ],
                "summary_month": anonymous_notification_data["message_param"][
                    "summary_month"
                ],
                "formatted_month": anonymous_notification_data["message_param"][
                    "formatted_month"
                ],
                "answer_user_name": (
                    anonymous_notification_data["message_param"]["answer_user_name"]
                    if anonymous_notification_data["message_param"]["answer_user_name"]
                    != ""
                    else "担当者"
                ),
                "survey_type_name": anonymous_notification_data["message_param"][
                    "survey_type_name"
                ],
                "fo_survey_detail_url": anonymous_notification_data["message_param"][
                    "fo_survey_detail_url"
                ],
                "survey_answer_due_date_title": anonymous_notification_data[
                    "message_param"
                ]["survey_answer_due_date_title"],
                "survey_answer_due_date": anonymous_notification_data["message_param"][
                    "survey_answer_due_date"
                ],
                "project_identification_id": anonymous_notification_data[
                    "message_param"
                ]["project_identification_id"],
            }
            # 送信
            ProjectService.send_mail(
                template=MailType.SURVEY_ANONYMOUS_ANSWER_REQUEST,
                to_addr_list=to_addr_list,
                cc_addr_list=cc_addr_list,
                bcc_addr_list=bcc_addr_list,
                payload=payload_survey_anonymous_answer_request,
            )

            # メール通知
            # 「匿名回答アンケートパスワード通知」メール
            # 対象: 匿名回答アンケート
            to_addr_list = list(set(anonymous_notification_data["to_mail_list"]))
            cc_addr_list = list(set(anonymous_notification_data["cc_mail_list"]))
            bcc_addr_list = list(set(anonymous_notification_data["bcc_mail_list"]))
            payload_survey_anonymous_password: dict = {
                "customer_name": anonymous_notification_data["message_param"][
                    "customer_name"
                ],
                "project_name": anonymous_notification_data["message_param"][
                    "project_name"
                ],
                "summary_month": anonymous_notification_data["message_param"][
                    "summary_month"
                ],
                "formatted_month": anonymous_notification_data["message_param"][
                    "formatted_month"
                ],
                "answer_user_name": (
                    anonymous_notification_data["message_param"]["answer_user_name"]
                    if anonymous_notification_data["message_param"]["answer_user_name"]
                    != ""
                    else "担当者"
                ),
                "survey_type_name": anonymous_notification_data["message_param"][
                    "survey_type_name"
                ],
                "survey_password": anonymous_notification_data["message_param"][
                    "survey_password"
                ],
                "project_identification_id": anonymous_notification_data[
                    "message_param"
                ]["project_identification_id"],
            }
            # 送信
            ProjectService.send_mail(
                template=MailType.SURVEY_ANONYMOUS_PASSWORD,
                to_addr_list=to_addr_list,
                cc_addr_list=cc_addr_list,
                bcc_addr_list=bcc_addr_list,
                payload=payload_survey_anonymous_password,
            )

        return OKResponse()

    @staticmethod
    def get_survey_summary_supporter_organizations(
        query_params: GetSurveySummarySupporterOrganizationsQuery,
        current_user: AdminModel,
    ) -> GetSurveySummarySupporterOrganizationsResponse:
        """課別のアンケート集計結果を取得する

        Args:
            query_params (GetSurveySummarySupporterOrganizationByMineQuery): クエリパラメータ
            current_user (UserModel): 認証済みユーザー

        Returns:
            GetSurveySummarySupporterOrganizationsResponse: 取得結果
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

        # 営業・営業責任者の場合は全ての課の集計結果を取得
        master_supporter_organizations: List[MasterSupporterOrganizationModel] = list(
            MasterSupporterOrganizationModel.data_type_index.query(
                hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value
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
        total_service_satisfaction_average = 0
        total_completion_satisfaction_average = 0
        total_satisfaction_average = 0
        completion_continuation_percent = 0
        completion_recommended_average = 0

        for supporter_organization in master_supporter_organizations:
            survey_summary_list: List[SurveySummarySupporterOrganizationModel] = list(
                SurveySummarySupporterOrganizationModel.data_type_summary_month_index.query(
                    hash_key=f"supporter_organization#{supporter_organization.id}",
                    range_key_condition=SurveySummarySupporterOrganizationModel.summary_month.between(
                        str_year_month_from, str_year_month_to
                    ),
                )
            )

            # DBに集計データが無い課はスキップ
            if not survey_summary_list:
                continue

            term_summary_result = TermSummaryResultForSupporterOrganizations(
                supporter_organization_id=supporter_organization.id,
                supporter_organization_name="",
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
                # 各種平均算出
                SurveyService.calculate_survey_average(term_summary_result)
                term_summary_result.supporter_organization_name = (
                    supporter_organization.value
                )
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

        return GetSurveySummarySupporterOrganizationsResponse(
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
    def get_survey_summary_all(
        query_params: GetSurveySummaryAllQuery, current_user: AdminModel
    ) -> GetSurveySummaryAllResponse:
        """アンケートサマリ全集計の集計結果を取得します。

        Args:
            query_params (GetSurveySummaryAllQuery): クエリパラメータ
            current_user (AuthUser): 認証済みユーザー

        Returns:
            GetSurveySummaryAllResponse: 取得結果
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

        survey_summary_list: List[SurveySummaryAllModel] = list(
            SurveySummaryAllModel.data_type_summary_month_index.query(
                hash_key=SurveySummaryType.ALL,
                range_key_condition=SurveySummaryAllModel.summary_month.between(
                    str_year_month_from, str_year_month_to
                ),
            )
        )

        service = dict(
            satisfaction_summary=0,
            satisfaction_average=0,
            satisfaction_unanswered=0,
            total_receive=0,
        )
        completion = dict(
            satisfaction_summary=0,
            satisfaction_average=0,
            satisfaction_unanswered=0,
            continuation_positive_count=0,
            continuation_negative_count=0,
            continuation_positive_percent=0,
            continuation_unanswered=0,
            recommended_summary=0,
            recommended_average=0,
            recommended_unanswered=0,
            sales_summary=0,
            sales_average=0,
            sales_unanswered=0,
            total_receive=0,
        )
        service_and_completion = dict(
            satisfaction_summary=0,
            satisfaction_average=0,
            satisfaction_unanswered=0,
            total_receive=0,
        )
        quick = dict(total_receive=0)
        pp = dict(
            survey_satisfaction_summary=0,
            survey_satisfaction_average=0,
            man_hour_satisfaction_summary=0,
            man_hour_satisfaction_average=0,
            karte_satisfaction_summary=0,
            karte_satisfaction_average=0,
            master_karte_satisfaction_summary=0,
            master_karte_satisfaction_average=0,
            total_receive=0,
        )

        monthly_survey_summary_results: List[MonthlySurveySummaryAllResults] = []

        for survey_summary in survey_summary_list:
            service["satisfaction_summary"] += (
                survey_summary.service.satisfaction_summary
            )
            service["satisfaction_unanswered"] += (
                survey_summary.service.satisfaction_unanswered
            )
            service["total_receive"] += survey_summary.service.receive_count

            completion["satisfaction_summary"] += (
                survey_summary.completion.satisfaction_summary
            )
            completion["satisfaction_unanswered"] += (
                survey_summary.completion.satisfaction_unanswered
            )
            completion["continuation_positive_count"] += (
                survey_summary.completion.continuation.positive_count
            )
            completion["continuation_negative_count"] += (
                survey_summary.completion.continuation.negative_count
            )
            completion["continuation_unanswered"] += (
                survey_summary.completion.continuation.unanswered
            )
            completion["total_receive"] += survey_summary.completion.receive_count
            completion["recommended_summary"] += (
                survey_summary.completion.recommended_summary
            )
            completion["recommended_unanswered"] += (
                survey_summary.completion.recommended_unanswered
            )
            completion["sales_summary"] += survey_summary.completion.sales_summary
            completion["sales_unanswered"] += survey_summary.completion.sales_unanswered

            service_and_completion["satisfaction_summary"] += (
                survey_summary.service.satisfaction_summary
                + survey_summary.completion.satisfaction_summary
            )
            service_and_completion["satisfaction_unanswered"] += (
                survey_summary.service.satisfaction_unanswered
                + survey_summary.completion.satisfaction_unanswered
            )
            service_and_completion["total_receive"] += (
                survey_summary.service.receive_count
                + survey_summary.completion.receive_count
            )
            quick["total_receive"] += survey_summary.quick.receive_count
            pp["master_karte_satisfaction_summary"] += (
                survey_summary.pp.master_karte_satisfaction_summary
            )
            pp["karte_satisfaction_summary"] += (
                survey_summary.pp.karte_satisfaction_summary
            )
            pp["man_hour_satisfaction_summary"] += (
                survey_summary.pp.man_hour_satisfaction_summary
            )
            pp["survey_satisfaction_summary"] += (
                survey_summary.pp.survey_satisfaction_summary
            )
            pp["total_receive"] += survey_summary.pp.receive_count

            # DBから取得した回答率をパーセンテージ化
            survey_summary.service.receive_percent = (
                survey_summary.service.receive_percent * 100
            )
            survey_summary.completion.receive_percent = (
                survey_summary.completion.receive_percent * 100
            )
            survey_summary.completion.continuation.positive_percent = (
                survey_summary.completion.continuation.percent * 100
            )
            survey_summary.pp.receive_percent = survey_summary.pp.receive_percent * 100

            survey_summary.karte.use_percent = survey_summary.karte.use_percent * 100

            monthly_survey_summary_results.append(
                MonthlySurveySummaryAllResults(**survey_summary.attribute_values)
            )

        logger.info("start calculating numbers")
        # 各種計算
        # サービスアンケート
        if (service["total_receive"] - service["satisfaction_unanswered"]) != 0:
            service["satisfaction_average"] = service["satisfaction_summary"] / (
                service["total_receive"] - service["satisfaction_unanswered"]
            )

        # 修了アンケート
        if (completion["total_receive"] - completion["satisfaction_unanswered"]) != 0:
            completion["satisfaction_average"] = completion["satisfaction_summary"] / (
                completion["total_receive"] - completion["satisfaction_unanswered"]
            )
        if (completion["total_receive"] - completion["continuation_unanswered"]) != 0:
            completion["continuation_positive_percent"] = round_off(
                completion["continuation_positive_count"]
                / (completion["total_receive"] - completion["continuation_unanswered"])
                * 100
            )
        if (completion["total_receive"] - completion["recommended_unanswered"]) != 0:
            completion["recommended_average"] = completion["recommended_summary"] / (
                completion["total_receive"] - completion["recommended_unanswered"]
            )
        if (completion["total_receive"] - completion["sales_unanswered"]) != 0:
            completion["sales_average"] = completion["sales_summary"] / (
                completion["total_receive"] - completion["sales_unanswered"]
            )

        # サービスアンケート／修了アンケート合算
        if (
            service_and_completion["total_receive"]
            - service_and_completion["satisfaction_unanswered"]
        ) != 0:
            service_and_completion["satisfaction_average"] = service_and_completion[
                "satisfaction_summary"
            ] / (
                service_and_completion["total_receive"]
                - service_and_completion["satisfaction_unanswered"]
            )

        # PP
        if pp["total_receive"] != 0:
            # アンケート機能
            if pp["survey_satisfaction_summary"] != 0:
                pp["survey_satisfaction_average"] = (
                    pp["survey_satisfaction_summary"] / pp["total_receive"]
                )
            # 工数入力機能
            if pp["man_hour_satisfaction_summary"] != 0:
                pp["man_hour_satisfaction_average"] = (
                    pp["man_hour_satisfaction_summary"] / pp["total_receive"]
                )
            # 個別カルテ機能
            if pp["karte_satisfaction_summary"] != 0:
                pp["karte_satisfaction_average"] = (
                    pp["karte_satisfaction_summary"] / pp["total_receive"]
                )
            # マスターカルテ機能
            if pp["master_karte_satisfaction_summary"] != 0:
                pp["master_karte_satisfaction_average"] = (
                    pp["master_karte_satisfaction_summary"] / pp["total_receive"]
                )

        term_summary_result = TermSummaryResultForSummaryAll(
            service=SummaryResult(
                satisfaction_average=round_off(service["satisfaction_average"], 0.1),
                total_receive=service["total_receive"],
            ),
            completion=CompletionSummaryResult(
                satisfaction_average=round_off(completion["satisfaction_average"], 0.1),
                continuation_positive_percent=completion[
                    "continuation_positive_percent"
                ],
                recommended_average=round_off(completion["recommended_average"], 0.1),
                sales_average=round_off(completion["sales_average"], 0.1),
                total_receive=completion["total_receive"],
            ),
            service_and_completion=SummaryResult(
                satisfaction_average=round_off(
                    service_and_completion["satisfaction_average"], 0.1
                ),
                total_receive=service_and_completion["total_receive"],
            ),
            quick=QuickSummaryResult(total_receive=quick["total_receive"]),
            pp=PPSummaryResult(
                survey_satisfaction_average=round_off(
                    pp["survey_satisfaction_average"], 0.1
                ),
                man_hour_satisfaction_average=round_off(
                    pp["man_hour_satisfaction_average"], 0.1
                ),
                karte_satisfaction_average=round_off(
                    pp["karte_satisfaction_average"], 0.1
                ),
                master_karte_satisfaction_average=round_off(
                    pp["master_karte_satisfaction_average"], 0.1
                ),
                total_receive=pp["total_receive"],
            ),
        )

        return GetSurveySummaryAllResponse(
            term_summary_result=term_summary_result,
            surveys=monthly_survey_summary_results,
        )

    @staticmethod
    def is_visible_survey_for_get_survey_by_id(
        current_user: AdminModel,
        project_id: str,
    ) -> bool:
        """案件アンケート情報がアクセス可能か判定.
            ユーザ情報は管理ユーザ情報から取得.
            1.制限なし(アクセス可)
              ・営業担当者
              ・営業責任者
              ・アンケート事務局
              ・システム管理者
              ・事業者責任者
            2.支援者責任者
              所属課案件：アクセス可
              その他公開案件：アクセス可
              上記以外：アクセス不可
        Args:
            current_user (AdminModel): ログインユーザ
            project_id: 案件ID
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """
        project = ProjectModel.get(hash_key=project_id, range_key=DataType.PROJECT)

        for role in current_user.roles:
            if role in [
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True

            elif role == UserRoleType.SUPPORTER_MGR.key:
                if (
                    current_user.supporter_organization_id
                    and project.supporter_organization_id
                    in current_user.supporter_organization_id
                ):
                    # 所属課案件
                    return True

                if not project.is_secret:
                    # その他公開案件
                    return True
        return False

    @staticmethod
    def is_visible_survey_for_get_surveys(
        current_user: AdminModel,
        project_list: List[ProjectModel],
        survey: ProjectSurveyModel,
    ):
        """案件アンケート情報がアクセス可能か判定.
            1.制限なし(アクセス可)
              ・営業責任者
              ・営業担当者
              ・アンケート事務局
              ・システム管理者
              ・事業者責任者
            2.支援者責任者
              所属課案件：アクセス可
              その他公開案件：アクセス可
              上記以外：アクセス不可
        Args:
            current_user (AdminModel): 実行ユーザ情報
            project_list (List[ProjectModel])
            survey (ProjectSurveyModel)
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """

        for role in current_user.roles:
            if role in [
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True

            elif role == UserRoleType.SUPPORTER_MGR.key:
                for project in project_list:
                    if survey.project_id == project.id:
                        if (
                            current_user.supporter_organization_id
                            and project.supporter_organization_id
                            in current_user.supporter_organization_id
                        ):
                            # 所属課案件
                            return True

                        if not project.is_secret:
                            # その他公開案件
                            return True
        return False

    @staticmethod
    def is_visible_survey_for_get_survey_plans(
        current_user: AdminModel,
        project: ProjectModel,
    ):
        """案件アンケート情報がアクセス可能か判定.
            1.制限なし(アクセス可)
              ・営業責任者
              ・営業担当者
              ・アンケート事務局
              ・システム管理者
              ・事業者責任者
            2.支援者責任者
              所属課案件：アクセス可
              その他公開案件：アクセス可
              上記以外：アクセス不可
        Args:
            current_user (AdminModel): 実行ユーザ情報
            project (ProjectModel) : 案件アンケート情報に紐づく案件情報
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """

        for role in current_user.roles:
            if role in [
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True

            elif role == UserRoleType.SUPPORTER_MGR.key:
                if (
                    current_user.supporter_organization_id
                    and project.supporter_organization_id
                    in current_user.supporter_organization_id
                ):
                    # 所属課案件
                    return True

                if not project.is_secret:
                    # その他公開案件
                    return True
        return False

    @staticmethod
    def is_visible_survey_for_resend_survey_by_id(
        current_user: AdminModel,
    ) -> bool:
        """案件アンケート情報がアクセス可能か判定.
            1.制限なし(アクセス可)
              ・営業担当者
              ・営業責任者
              ・アンケート事務局
              ・システム管理者
              ・事業者責任者
            2.アクセス不可
              ・支援者責任者
              ・稼働率調査事務局
        Args:
            current_user (AdminModel): 実行ユーザ情報
        Returns:
            bool: True(アクセス可能)、False(アクセス不可)
        """

        for role in current_user.roles:
            if role in [
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            ]:
                return True
        return False

    def calculate_return_rate(summary_reports: GetSurveySummaryReportsResponse):
        """アンケート回収率を計算する

        Args:
            summary_reports (GetSurveySummaryReportsResponse):
        """
        if summary_reports.plan != 0:
            summary_reports.percent = round_off(
                summary_reports.collect / summary_reports.plan * 100
            )
        if summary_reports.summary_plan != 0:
            summary_reports.summary_percent = round_off(
                summary_reports.summary_collect / summary_reports.summary_plan * 100
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
    def edit_response_based_on_project_info_for_get_survey_plans(
        project: ProjectModel,
        user_list: List[UserModel],
        master_supporter_organization_list: List[MasterSupporterOrganizationModel],
        master_service_type_list: List[MasterSupporterOrganizationModel],
    ) -> SurveyInfoForGetSurveyPlansResponse:
        """案件情報を基に、getSurveyPlansのレスポンスオブジェクトSurveyInfoForGetSurveyPlansResponseを編集.

        Args:
            project (ProjectModel): 案件情報
            user_list (List[UserModel]): 一般ユーザ情報リスト
            master_supporter_organization_list (List[MasterSupporterOrganizationModel]): 汎用マスタ(支援者組織情報)のリスト
            master_service_type_list (List[MasterSupporterOrganizationModel]): 汎用マスタ(サービス種別)のリスト

        Returns:
            SurveyInfoForGetSurveyPlansResponse: GetSurveyPlans用のレスポンスオブジェクト
        """

        survey_user_type = None
        survey_user_name = ""
        survey_user_email = ""
        if project.main_customer_user_id:
            survey_user_type = SurveyUserType.REGISTERED_MAIN_CUSTOMER
            for user in user_list:
                if user.id == project.main_customer_user_id:
                    survey_user_name = user.name
                    survey_user_email = user.email
                    break
        elif not project.dedicated_survey_user_email:
            # 匿名回答アンケートのメールアドレス設定なし
            pass
        elif (
            project.dedicated_survey_user_email
            == project.salesforce_main_customer.email
        ):
            survey_user_type = SurveyUserType.SALESFORCE_MAIN_CUSTOMER
            survey_user_name = project.dedicated_survey_user_name
            survey_user_email = project.dedicated_survey_user_email
        else:
            survey_user_type = SurveyUserType.MANUAL_SETTING
            survey_user_name = project.dedicated_survey_user_name
            survey_user_email = project.dedicated_survey_user_email

        main_supporter_user: SupporterUser = None
        main_supporter_user = SurveyService.edit_supporter_user_schema(
            supporter_user_id=project.main_supporter_user_id,
            user_list=user_list,
            master_supporter_organization_list=master_supporter_organization_list,
        )
        supporter_users: List[SupporterUser] = []
        if project.supporter_user_ids:
            for supporter_user_id in project.supporter_user_ids:
                temp_supporter_user: SupporterUser = SurveyService.edit_supporter_user_schema(
                    supporter_user_id=supporter_user_id,
                    user_list=user_list,
                    master_supporter_organization_list=master_supporter_organization_list,
                )
                supporter_users.append(temp_supporter_user)

        sales_user_name = ""
        for user in user_list:
            if user.id == project.main_sales_user_id:
                sales_user_name = user.name
                break

        supporter_organization_name = ""
        for supporter_organization in master_supporter_organization_list:
            if project.supporter_organization_id == supporter_organization.id:
                supporter_organization_name = supporter_organization.value
                break
        service_type_name = ""
        for master_service_type in master_service_type_list:
            if project.service_type == master_service_type.id:
                service_type_name = master_service_type.name
                break

        return SurveyInfoForGetSurveyPlansResponse(
            project_id=project.id,
            project_name=project.name,
            id=None,
            survey_type=None,
            customer_success=project.customer_success,
            supporter_organization_id=project.supporter_organization_id,
            supporter_organization_name=supporter_organization_name,
            support_date_from=project.support_date_from,
            support_date_to=project.support_date_to,
            main_supporter_user=main_supporter_user,
            supporter_users=supporter_users,
            sales_user_id=project.main_sales_user_id,
            sales_user_name=sales_user_name,
            service_type_id=project.service_type,
            service_type_name=service_type_name,
            survey_user_type=survey_user_type,
            survey_user_name=survey_user_name,
            survey_user_email=survey_user_email,
            customer_id=project.customer_id,
            customer_name=project.customer_name,
            summary_month="",
            plan_survey_request_datetime="",
            actual_survey_request_datetime="",
            plan_survey_response_datetime="",
            actual_survey_response_datetime="",
            phase=project.phase,
            is_count_man_hour=project.is_count_man_hour,
            # NPFから取得したもの
            this_month_type=project.this_month_type,
            no_send_reason=project.no_send_reason,
        )

    @staticmethod
    def edit_unsent_response_for_get_survey_plans(
        project: ProjectModel,
        survey: Union[ProjectSurveyModel, None],
        user_list: List[UserModel],
        master_supporter_organization_list: List[MasterSupporterOrganizationModel],
        master_service_type_list: List[MasterSupporterOrganizationModel],
    ) -> SurveyInfoForGetSurveyPlansResponse:
        """getSurveyPlansのアンケート未送信用のレスポンスオブジェクトSurveyInfoForGetSurveyPlansResponseの編集.
            アンケート情報固有の項目を除き、案件情報を基に編集する.

        Args:
            project (ProjectModel): 案件情報
            survey (ProjectSurveyModel): 案件アンケート情報
            user_list (List[UserModel]): 一般ユーザ情報リスト
            master_supporter_organization_list (List[MasterSupporterOrganizationModel]): 汎用マスタ(支援者組織情報)のリスト
            master_service_type_list (List[MasterSupporterOrganizationModel]): 汎用マスタ(サービス種別)のリスト

        Returns:
            SurveyInfoForGetSurveyPlansResponse: GetSurveyPlans用のレスポンスオブジェクト
        """

        # 案件情報を基にレスポンスオブジェクトを編集
        ret = SurveyService.edit_response_based_on_project_info_for_get_survey_plans(
            project=project,
            user_list=user_list,
            master_supporter_organization_list=master_supporter_organization_list,
            master_service_type_list=master_service_type_list,
        )

        # アンケート関連の情報をセット
        ret.id = survey.id
        ret.survey_type = survey.survey_type
        ret.summary_month = survey.summary_month
        ret.plan_survey_request_datetime = survey.plan_survey_request_datetime
        ret.actual_survey_request_datetime = survey.actual_survey_request_datetime
        ret.plan_survey_response_datetime = survey.plan_survey_response_datetime
        ret.actual_survey_response_datetime = survey.actual_survey_response_datetime
        ret.this_month_type = survey.this_month_type
        ret.no_send_reason = survey.no_send_reason

        return ret
