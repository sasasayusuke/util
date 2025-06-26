import copy
import os
import re
import sys
import time
import traceback
from datetime import datetime
from logging import getLogger
from typing import List

import pandas as pd
from dateutil.relativedelta import relativedelta
from pynamodb.expressions.condition import size

from app.core.config import get_app_settings
from app.models.master import MasterSupporterOrganizationModel
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.project_survey import (
    EvaluationSupporterAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.models.survey_summary import (
    CompletionAttribute,
    CompletionContinuationSubAttribute,
    ContinuationSubAttribute,
    KarteAttribute,
    PpAttribute,
    QuickAttribute,
    ServiceAttribute,
    SurveySummaryAllModel,
    SurveySummarySupporterOrganizationModel,
    SurveySummaryUserModel,
)
from app.models.user import UserModel
from functions.batch_common import (
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
    BatchInputParameterModeForSummarySurvey,
    BatchStatus,
    DataType,
    HourMinutes,
    MailType,
    MasterDataType,
    RoundSetting,
    SolverIdentifier,
    SurveySummaryDataTypePrefix,
    SurveyType,
    SurveyUnansweredColumn,
)
from functions.batch_exceptions import ExitHandler

try:
    import unzip_requirements
except ImportError:
    pass

# ログ出力設定をロードする
get_app_settings().configure_logging()
logger = getLogger(__name__)


def calc_unanswered_column(data, unanswered_col: SurveyUnansweredColumn) -> int:
    """
        案件アンケート情報の未回答項目(Set)から項目毎の列の生成に使用.

    Args:
        data: 未回答項目(Set). 未回答なしの場合、SetでなくNaN
        unanswered_col: 集計する項目名.
            e.g. "satisfaction", "continuation"
    Returns:
        0: 指定されたunanswered_colが未回答項目に存在しない場合
        1: 指定されたunanswered_colが未回答項目に存在する場合

    """
    if pd.isnull(data):
        return 0
    elif type(data) is not set:
        return 0
    else:
        for unanswered in data:
            if unanswered_col in unanswered:
                return 1

    return 0


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
                    for supporter_organization in master_supporter_organization_list:
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


def update_survey_evaluators(
    summary_year_month: str,
    operation_start_datetime: datetime,
    batch_control_id: str,
) -> None:
    """
        アンケート評価対象者の更新
        ・指定された基準年月を集計月とするアンケート情報のアンケート評価対象者を更新
    Args:
        summary_year_month (str): 基準年月(YYYY/MM)
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    summary_month_datetime: datetime = datetime.strptime(summary_year_month, "%Y/%m")

    ###############################
    # 処理対象のアンケートの取得
    ###############################
    # 抽出条件:
    #  集計月が処理年月と一致するもの
    #  Partner Portalアンケート以外
    #  回答期限日が設定されており、回答期限を過ぎていない
    # ※補足
    #  ・回答期限を過ぎたアンケート情報（過去のアンケート）のアンケート評価対象者を更新しないよう
    #    「回答期限日が設定されており、回答期限を過ぎていない」条件を追加
    project_survey_range_key_condition = (
        ProjectSurveyModel.summary_month == summary_year_month
    )

    # バッチが夜間(AM1:00)に実行されるため、1日前の日時で判定
    ref_datetime = operation_start_datetime + relativedelta(days=-1)
    ref_datetime_str = ref_datetime.strftime("%Y/%m/%d") + " " + HourMinutes.MINIMUM

    project_survey_filter_condition = None
    project_survey_filter_condition &= ProjectSurveyModel.survey_type != SurveyType.PP
    # 回答（受信）予定日時（回答期限）が空（未設定）でなく、かつ回答期限日を過ぎていない
    project_survey_filter_condition &= (
        ProjectSurveyModel.plan_survey_response_datetime.exists()
        & (size(ProjectSurveyModel.plan_survey_response_datetime) != 0)
        & (ProjectSurveyModel.plan_survey_response_datetime >= ref_datetime_str)
    )

    project_survey_list: list[ProjectSurveyModel] = list(
        ProjectSurveyModel.data_type_summary_month_index.query(
            hash_key=DataType.SURVEY,
            range_key_condition=project_survey_range_key_condition,
            filter_condition=project_survey_filter_condition,
        )
    )
    logger.info(
        f"集計月{summary_year_month}のアンケート情報の取得件数: {len(project_survey_list)}件"
    )

    if not project_survey_list:
        # 対象データ0件の場合、処理終了
        logger.info(
            f"アンケート情報が0件のため、集計月{summary_year_month}のアンケート評価対象者の更新処理終了"
        )
        return

    # アンケート情報に含まれる案件IDの集合
    target_project_id_set: set = {x.project_id for x in project_survey_list}
    # 案件IDごとの案件カルテ情報リスト
    project_id_karte_map: dict[str, list[ProjectKarteModel]] = {}

    for target_project_id in target_project_id_set:
        ###############################
        # 案件カルテ情報取得
        ###############################
        # 抽出条件
        # ・案件ID
        # ・支援日が集計月のもの

        # 集計月の月初
        karte_from_date_str = summary_year_month + "/01"
        # 集計月の月末 (翌月の1日から1日分戻す)
        karte_to_date = summary_month_datetime + relativedelta(
            months=+1, day=1, days=-1
        )
        karte_to_date_str = karte_to_date.strftime("%Y/%m/%d")

        karte_filter_condition = None
        karte_filter_condition &= ProjectKarteModel.date.between(
            karte_from_date_str, karte_to_date_str
        )

        project_karte_list: list[ProjectKarteModel] = list(
            ProjectKarteModel.project_id_start_datetime_index.query(
                hash_key=target_project_id,
                filter_condition=karte_filter_condition,
            )
        )
        logger.info(
            f"案件ID({target_project_id})の案件カルテ情報取得件数: {len(project_karte_list)}件"
        )

        project_id_karte_map[target_project_id] = project_karte_list

    # 案件情報の取得
    item_keys = [(x, DataType.PROJECT) for x in target_project_id_set]
    project_list = list(ProjectModel.batch_get(item_keys))
    project_id_model_map = {x.id: x for x in project_list}
    # 一般ユーザ情報の取得
    user_list: List[UserModel] = list(UserModel.scan())
    # 汎用マスタ:支援者組織情報
    master_supporter_organization_list: List[MasterSupporterOrganizationModel] = list(
        MasterSupporterOrganizationModel.data_type_index.query(
            hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION
        )
    )

    # アンケート情報の更新
    with ProjectSurveyModel.batch_write() as project_survey_batch:
        for idx, survey in enumerate(project_survey_list, start=1):
            logger.info(
                f"集計月{summary_year_month}のアンケート評価対象者の更新処理 - 処理開始 ({idx}/{len(project_survey_list)})"
            )
            # プロジェクトIDに紐づく案件カルテ情報リスト
            target_project_karte_list = project_id_karte_map.get(survey.project_id, [])

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

            # 辞書（key: アクセラレーターのユーザID、value: 更新したカルテのカルテIDリスト）
            supporter_id_karte_dict: dict[str, list[str]] = {}
            # 辞書（key: アクセラレーターのユーザID、value: SupporterUserAttribute）
            supporter_id_attribute_dict: dict[str, SupporterUserAttribute] = {}

            if survey.plan_survey_response_datetime:
                # 回答期限日が設定されている場合
                for target_karte in target_project_karte_list:
                    if target_karte.karte_notify_update_history:
                        for history in target_karte.karte_notify_update_history:
                            comp_date_str = datetime.strptime(
                                survey.plan_survey_response_datetime, "%Y/%m/%d %H:%M"
                            ).strftime("%Y/%m/%d")

                            if history.karte_update_date <= comp_date_str and (
                                not survey.main_supporter_user
                                or history.user_id != survey.main_supporter_user.id
                            ):
                                # アンケート集計月の個別カルテをアンケート回答期限日までに記入したアクセラレーターが評価対象者
                                # プロデューサーがカルテ更新した場合は、アクセラレーターに含めない

                                # アクセラレーターごとのカルテIDリストの編集
                                if history.user_id in supporter_id_karte_dict.keys():
                                    temp_list = supporter_id_karte_dict[history.user_id]
                                    # カルテIDをリストに追加、重複を排除
                                    temp_list.append(target_karte.karte_id)
                                    supporter_id_karte_dict[history.user_id] = list(
                                        set(temp_list)
                                    )
                                else:
                                    supporter_id_karte_dict[history.user_id] = [
                                        target_karte.karte_id
                                    ]

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

                    supporter_user_list.append(value)

                # アクセラレーターとして登録されたソルバーは、個別カルテを更新していなくても評価対象者とする
                target_supporter_users: list[SupporterUserAttribute] = None
                # 当該バッチ処理は夜間（AM 1:00）、アンケート送信バッチはAM 9:00（アンケート情報が最新化される）のため、
                # アンケート情報のアクセラレーターを使用すると、アンケート送信日当日は評価対象者に設定するソルバーが最新でない可能性がある。
                # （アンケート情報作成（未送信）後に、アクセラレーターとして登録されたソルバーが変更になった場合など）
                # アンケート回答画面のアクセラレーター表示を考慮し、アンケート未送信の場合は、案件情報のアクセラレータを元にソルバーを抽出する
                if not survey.actual_survey_request_datetime:
                    # アンケート未送信（回答（送信）依頼実績日時が設定されていない）の場合
                    latest_project: ProjectModel = project_id_model_map[
                        survey.project_id
                    ]
                    if latest_project.supporter_user_ids:
                        latest_project_supporter_users = []
                        for supporter_user_id in latest_project.supporter_user_ids:
                            latest_project_supporter_users.append(
                                edit_supporter_user_attribute(
                                    supporter_user_id=supporter_user_id,
                                    user_list=user_list,
                                    master_supporter_organization_list=master_supporter_organization_list,
                                )
                            )
                    target_supporter_users = latest_project_supporter_users
                else:
                    # アンケート送信済みの場合
                    if survey.is_updated_evaluation_supporters:
                        # アンケート評価対象者が以前に更新済みの場合
                        target_supporter_users = copy.deepcopy(
                            survey.supporter_users_before_update
                        )
                    else:
                        # アンケート評価対象者の更新が初回の場合
                        target_supporter_users = copy.deepcopy(
                            survey.supporter_users
                        )

                # アクセラレーターとして登録されたソルバーを抽出し、supporter_user_listに追加
                if target_supporter_users:
                    for supporter_user in target_supporter_users:
                        if re.match(SolverIdentifier.NAME_PREFIX, supporter_user.name):
                            # アクセラレーターの名前の先頭がソルバーを識別する文字列である場合
                            if supporter_user_list is None:
                                supporter_user_list = []
                            # 存在チェック
                            supporter_user_ids = [supporter_user.id for supporter_user in supporter_user_list]
                            if supporter_user.id not in supporter_user_ids:
                                supporter_user_list.append(copy.deepcopy(supporter_user))

                evaluation_user_list: list[EvaluationSupporterAttribute] = None
                for key, val in supporter_id_karte_dict.items():
                    if evaluation_user_list is None:
                        evaluation_user_list = []

                    temp_evaluation_supporter_user = EvaluationSupporterAttribute(
                        supporter_id=key, karte_ids=val
                    )
                    evaluation_user_list.append(temp_evaluation_supporter_user)

                if not survey.is_updated_evaluation_supporters:
                    # アンケート評価対象者の更新が初回の場合
                    # 更新前のアクセラレータ情報をsupporter_users_before_updateに格納
                    survey.supporter_users_before_update = copy.deepcopy(
                        survey.supporter_users
                    )

                survey.supporter_users = supporter_user_list
                survey.evaluation_supporters = evaluation_user_list
                survey.is_updated_evaluation_supporters = True
                survey.update_id = batch_control_id
                survey.update_at = operation_start_datetime
                survey.version += 1
                project_survey_batch.save(survey)


def save_user_summary_of_summary_month(
    summary_year_month: str,
) -> None:
    """
        支援者（営業担当者）別集計の編集・登録
        ・指定された基準年月の支援者（営業担当者）別集計をアンケートサマリ情報に登録
    Args:
        summary_year_month (str): 基準年月(YYYY/MM)
    """
    summary_month_datetime: datetime = datetime.strptime(summary_year_month, "%Y/%m")

    ########################################################
    # データ洗い替えの為、処理前に前回作成分の
    # 支援者（営業担当者）別集計の処理対象の年月のデータを削除
    ########################################################
    logger.info(f"集計月{summary_year_month}の前回作成データ削除")
    del_year_month = summary_month_datetime.strftime("%Y/%m")
    del_survey_summary_user_range_key_condition = (
        SurveySummaryUserModel.data_type.startswith(SurveySummaryDataTypePrefix.USER)
    )
    del_survey_summary_user_list: List[SurveySummaryUserModel] = list(
        SurveySummaryUserModel.query(
            hash_key=del_year_month,
            range_key_condition=del_survey_summary_user_range_key_condition,
        )
    )
    with SurveySummaryUserModel.batch_write() as del_survey_summary_user_batch:
        count = 0
        for del_summary in del_survey_summary_user_list:
            count += 1
            logger.info(
                f"集計月{summary_year_month}の前回作成データ削除 - 処理開始 ({count}/{len(del_survey_summary_user_list)})"
            )
            del_survey_summary_user_batch.delete(del_summary)

    ###############################
    # 処理対象のアンケートの取得
    ###############################
    # 抽出条件:
    #  集計月が処理年月と一致するもの
    #  Partner Portalアンケート以外
    #  集計対象外でない
    #  回答済
    project_survey_range_key_condition = (
        ProjectSurveyModel.summary_month == summary_year_month
    )
    # flake8でエラーが出るため、boolを変数で定義
    true_bool: bool = True
    false_bool: bool = False
    project_survey_filter_condition = None
    project_survey_filter_condition &= ProjectSurveyModel.survey_type != SurveyType.PP
    project_survey_filter_condition &= ProjectSurveyModel.is_not_summary == false_bool
    project_survey_filter_condition &= ProjectSurveyModel.is_finished == true_bool

    project_survey_list: List[ProjectSurveyModel] = list(
        ProjectSurveyModel.data_type_summary_month_index.query(
            hash_key=DataType.SURVEY,
            range_key_condition=project_survey_range_key_condition,
            filter_condition=project_survey_filter_condition,
        )
    )
    logger.info(
        f"集計月{summary_year_month}のアンケート情報の取得件数: {len(project_survey_list)}件"
    )

    if not project_survey_list:
        # 対象データ0件の場合、処理終了
        logger.info(f"アンケート情報が0件のため、集計月{summary_year_month}の処理終了")
        return

    # modelのリスト -> pandasのDataFrame (Map等は子要素も展開.但し、リスト部分は展開不可)
    # e.g. 以下を1行とするDataFrame
    #  df:
    #   create_at                                                 2022-08-03T20:14:25.437270+09:00
    #   is_disclosure                                                                         True
    #   is_finished                                                                           True
    #   is_not_summary                                                                       False
    #   ....中略.....
    #   answer_user_id                                        b9b67094-cdab-494c-818e-d4845088269b
    #   answer_user_name                                                                 田中 次郎
    #   answers                              [{'answer': '問題なし', 'id': '89cbe2ed-f44c-4a1c-...
    #   company                                                              ソニーグループ株式会社
    #   ....中略.....
    #   customer_id                                           d6121808-341d-4883-8e2c-69462acf6ccb
    #   customer_name                                                                 あああ研究所
    #   customer_success                                                                  DXの実現
    #   data_type                                                                           survey
    #   id                                                    668ac0a7-b0fb-4970-9753-8fceb59d02a9
    #   ....中略.....
    #   project_id                                            96103959-e438-4293-8b8b-3a990bb18de9
    #   project_name                                                           サンプルプロジェクト
    #   sales_user_id                                         7bf1b7e4-5625-4361-be0a-65f0e49829ea
    #   service_type_id                                       7ac8bddf-88da-46c9-a504-a03d1661ad58
    #   service_type_name                                                                 組織開発
    #   summary_month                                                                      2022/06
    #   support_date_from                                                               2021/06/01
    #   support_date_to                                                                 2022/12/31
    #   supporter_organization_id                             de40733f-6be9-4fef-8229-01052f43c1e2
    #   supporter_organization_name                                                            AST
    #   supporter_users                          [{'id': 'b9b67094-cdab-494c-818e-d4845088269b'...
    #   survey_master_id                                      9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf
    #   survey_master_revision                                                                   4
    #   survey_type                                                                        service
    #   ....中略.....
    #   main_supporter_user.id                                c9b67094-cdab-494c-818e-d4845088269b
    #   main_supporter_user.name                                                         田中 三郎
    #   main_supporter_user.organization_id                   de40733f-6be9-4fef-8229-01052f43c1e2
    #   main_supporter_user.organization_name                                                  AST
    #   points.continuation                                                                  False
    #   points.master_karte_satisfaction                                                         0
    #   points.karte_satisfaction                                                                3
    #   points.man_hour_satisfaction                                                             3
    #   points.recommended                                                                       4
    #   points.sales                                                                             0
    #   points.satisfaction                                                                      4
    #   points.survey_satisfaction                                                               3
    #   points.unanswered                                                                  {sales}
    #   .........
    df = pd.json_normalize([model_to_dict(x) for x in project_survey_list])

    # 上記のDataFrameに集計用の未回答項目の列を追加（points.unansweredの値を基に生成）
    # e.g.
    #  unanswered.satisfaction                                                                  1
    #  unanswered.continuation                                                                  0
    #  unanswered.recommended                                                                   0
    #  unanswered.sales                                                                         0
    #  unanswered.survey_satisfaction                                                           0
    #  unanswered.man_hour_satisfaction                                                         0
    #  unanswered.karte_satisfaction                                                            0
    #  unanswered.master_karte_satisfaction                                                     0
    add_unanswered_column: dict = {
        "unanswered.satisfaction": SurveyUnansweredColumn.SATISFACTION,
        "unanswered.continuation": SurveyUnansweredColumn.CONTINUATION,
        "unanswered.recommended": SurveyUnansweredColumn.RECOMMENDED,
        "unanswered.sales": SurveyUnansweredColumn.SALES,
        "unanswered.survey_satisfaction": SurveyUnansweredColumn.SURVEY_SATISFACTION,
        "unanswered.man_hour_satisfaction": SurveyUnansweredColumn.MAN_HOUR_SATISFACTION,
        "unanswered.karte_satisfaction": SurveyUnansweredColumn.KARTE_SATISFACTION,
        "unanswered.master_karte_satisfaction": SurveyUnansweredColumn.MASTER_KARTE_SATISFACTION,
    }
    if "points.unanswered" not in df.columns:
        df["points.unanswered"] = [set() for _ in range(df.shape[0])]
    for k, v in add_unanswered_column.items():
        df[k] = df["points.unanswered"].apply(calc_unanswered_column, unanswered_col=v)

    # dfの副担当支援者(supporter_users)部分を展開。
    # * プレフィックスは "supporter_user." とした。
    # e.g.
    # df_supporter_users:
    #                       supporter_user.id  ... unanswered.karte_satisfaction
    # 0  278be7f8-c46f-4e2d-b3df-ffdf12278ad1  ...                             0
    # 1  8a990e25-43da-49a3-ae76-863b5219fe6a  ...                             0
    # 2  58cbdca8-f210-4345-be15-884d21f6b834  ...                             0
    # 3  8a990e25-43da-49a3-ae76-863b5219fe6a  ...                             0
    # 4  58cbdca8-f210-4345-be15-884d21f6b834  ...                             0
    # 5  278be7f8-c46f-4e2d-b3df-ffdf12278ad1  ...                             0
    # 6  8a990e25-43da-49a3-ae76-863b5219fe6a  ...                             0
    # 7  58cbdca8-f210-4345-be15-884d21f6b834  ...                             0
    # 8  58cbdca8-f210-4345-be15-884d21f6b834  ...                             0
    # 9  8a990e25-43da-49a3-ae76-863b5219fe6a  ...                             0
    #
    # e.g. 1行のイメージ()
    # df_supporter_users.iloc[0]:
    #   supporter_user.id                   278be7f8-c46f-4e2d-b3df-ffdf12278ad1
    #   supporter_user.name                                            福島 支援
    #   supporter_user.organization_id      180a3597-b7e7-42c8-902c-a29016afa662
    #   supporter_user.organization_name                                     IST
    #   id                                  668ac0a7-b0fb-4970-9753-8fceb59d02a9
    #   survey_type                                                      service
    #   points.continuation                                                False
    #   points.master_karte_satisfaction                                       0
    #   points.karte_satisfaction                                              3
    #   points.man_hour_satisfaction                                           3
    #   points.recommended                                                     4
    #   points.sales                                                           4
    #   points.satisfaction                                                    0
    #   points.survey_satisfaction                                             3
    #   points.unanswered                                         {satisfaction}
    #   unanswered.satisfaction                                                1
    #   unanswered.continuation                                                0
    #   unanswered.recommended                                                 0
    #   unanswered.sales                                                       0
    #   unanswered.survey_satisfaction                                         0
    #   unanswered.man_hour_satisfaction                                       0
    #   unanswered.karte_satisfaction                                          0
    #   unanswered.master_karte_satisfaction                                   0
    if "supporter_users" in df.columns:
        df_supporter_users = pd.json_normalize(
            df.to_dict("records"),
            record_path="supporter_users",
            meta=[
                "id",
                "survey_type",
                "points.continuation",
                "points.master_karte_satisfaction",
                "points.karte_satisfaction",
                "points.man_hour_satisfaction",
                "points.recommended",
                "points.sales",
                "points.satisfaction",
                "points.survey_satisfaction",
                "points.unanswered",
                "unanswered.satisfaction",
                "unanswered.continuation",
                "unanswered.recommended",
                "unanswered.sales",
                "unanswered.survey_satisfaction",
                "unanswered.man_hour_satisfaction",
                "unanswered.karte_satisfaction",
                "unanswered.master_karte_satisfaction",
            ],
            record_prefix="supporter_user.",
        )
        # metaで追加した項目の型を変換(object -> bool or int64)
        df_supporter_users = df_supporter_users.astype(
            {
                "points.continuation": "bool",
                "points.master_karte_satisfaction": "int64",
                "points.karte_satisfaction": "int64",
                "points.man_hour_satisfaction": "int64",
                "points.recommended": "int64",
                "points.sales": "int64",
                "points.satisfaction": "int64",
                "points.survey_satisfaction": "int64",
                "unanswered.satisfaction": "int64",
                "unanswered.continuation": "int64",
                "unanswered.recommended": "int64",
                "unanswered.sales": "int64",
                "unanswered.survey_satisfaction": "int64",
                "unanswered.man_hour_satisfaction": "int64",
                "unanswered.karte_satisfaction": "int64",
                "unanswered.master_karte_satisfaction": "int64",
            }
        )
    else:
        # supporter_usersが設定されていない場合
        df_supporter_users = pd.DataFrame()

    # 後続のデータ合算のため、集計の軸となる項目をリネーム
    # 支援者(主担当)用
    #  main_supporter_user.id -> target_user_id
    # 支援者(副担当)用
    #  supporter_user.id -> target_user_id
    # 営業担当者用
    #  sales_user_id -> target_user_id
    if "main_supporter_user.id" in df.columns:
        df_main_supporter_user = df.rename(
            columns={"main_supporter_user.id": "target_user_id"}
        )
    else:
        # main_supporter_user.idが設定されていない場合
        df_main_supporter_user = pd.DataFrame()
    if not df_supporter_users.empty:
        df_supporter_users = df_supporter_users.rename(
            columns={"supporter_user.id": "target_user_id"}
        )
    df_sales_user_id = df.rename(columns={"sales_user_id": "target_user_id"})

    # 支援者(主担当)単位で集計
    # e.g.
    #  pivot_table_point_by_main_supporter:
    #                                                                    sum  ...                   count
    #                                                    points.continuation  ... unanswered.satisfaction
    #   target_user_id                       survey_type                      ...
    #   278be7f8-c46f-4e2d-b3df-ffdf12278ad1 completion                    1  ...                       1
    #                                        service                       0  ...                       1
    #   d9f416c2-a938-46b5-a1de-e7c74bbd18fa completion                    1  ...                       2
    #                                        service                       0  ...                       2
    #
    # e.g. 1行のイメージ(index: (d9f416c2-a938-46b5-a1de-e7c74bbd18fa, completion))
    #  pivot_table_point_by_main_supporter:
    #   sum    points.continuation         1
    #          points.recommended         10
    #          points.sales                7
    #          points.satisfaction         9
    #          unanswered.continuation     0
    #          unanswered.recommended      1
    #          unanswered.sales            0
    #          unanswered.satisfaction     0
    #   count  id                          2
    #          points.continuation         2
    #          points.recommended          2
    #          points.sales                2
    #          points.satisfaction         2
    #          unanswered.continuation     2
    #          unanswered.recommended      2
    #          unanswered.sales            2
    #          unanswered.satisfaction     2
    if not df_main_supporter_user.empty:
        pivot_table_point_by_main_supporter = pd.pivot_table(
            data=df_main_supporter_user,
            index=["target_user_id", "survey_type"],
            values=[
                "points.satisfaction",
                "points.continuation",
                "points.recommended",
                "points.sales",
                "unanswered.satisfaction",
                "unanswered.continuation",
                "unanswered.recommended",
                "unanswered.sales",
            ],
            aggfunc=["sum", "count"],
        ).fillna(0)

        pivot_table_id_by_main_supporter = pd.pivot_table(
            data=df_main_supporter_user,
            index=["target_user_id", "survey_type"],
            values="id",
            aggfunc="count",
        ).fillna(0)
    else:
        # df_main_supporter_userがemptyの場合
        pivot_table_point_by_main_supporter = pd.DataFrame()

    for x in range(pivot_table_point_by_main_supporter.shape[0]):
        logger.debug(
            f"支援者(主担当)単位: pivot_table_point_by_main_supporter.iloc[index]:{pivot_table_point_by_main_supporter.iloc[x]}"
        )

    # 支援者(副担当)単位で集計
    if not df_supporter_users.empty:
        pivot_table_point_by_supporter_users = pd.pivot_table(
            data=df_supporter_users,
            index=["target_user_id", "survey_type"],
            values=[
                "points.satisfaction",
                "points.continuation",
                "points.recommended",
                "points.sales",
                "unanswered.satisfaction",
                "unanswered.continuation",
                "unanswered.recommended",
                "unanswered.sales",
            ],
            aggfunc=["sum", "count"],
        ).fillna(0)

        pivot_table_id_by_supporter_users = pd.pivot_table(
            data=df_supporter_users,
            index=["target_user_id", "survey_type"],
            values="id",
            aggfunc="count",
        ).fillna(0)
    else:
        # df_supporter_usersがemptyの場合
        pivot_table_point_by_supporter_users = pd.DataFrame()

    for x in range(pivot_table_point_by_supporter_users.shape[0]):
        logger.debug(
            f"支援者(副担当)単位: pivot_table_point_by_supporter_users.iloc[index]:{pivot_table_point_by_supporter_users.iloc[x]}"
        )

    # 営業担当者単位で集計
    pivot_table_point_by_sales_user = pd.pivot_table(
        data=df_sales_user_id,
        index=["target_user_id", "survey_type"],
        values=[
            "points.satisfaction",
            "points.continuation",
            "points.recommended",
            "points.sales",
            "unanswered.satisfaction",
            "unanswered.continuation",
            "unanswered.recommended",
            "unanswered.sales",
        ],
        aggfunc=["sum", "count"],
    ).fillna(0)

    pivot_table_id_by_sales_user = pd.pivot_table(
        data=df_sales_user_id,
        index=["target_user_id", "survey_type"],
        values="id",
        aggfunc="count",
    ).fillna(0)

    for x in range(pivot_table_point_by_sales_user.shape[0]):
        logger.debug(
            f"営業担当者単位: pivot_table_point_by_sales_user.iloc[index]:{pivot_table_point_by_sales_user.iloc[x]}"
        )

    # 支援者(主担当)、支援者(副担当)、営業担当者の集計結果を合算
    df_point_by_user = pivot_table_point_by_sales_user.copy(deep=True)
    if not pivot_table_point_by_main_supporter.empty:
        df_point_by_user = df_point_by_user.add(
            pivot_table_point_by_main_supporter, fill_value=0
        )
    if not pivot_table_point_by_supporter_users.empty:
        df_point_by_user = df_point_by_user.add(
            pivot_table_point_by_supporter_users, fill_value=0
        )

    # 支援者(主担当)、支援者(副担当)、営業担当者のアンケート数を合算
    df_id_by_user = pivot_table_id_by_sales_user.copy(deep=True)
    if not pivot_table_point_by_main_supporter.empty:
        df_id_by_user = df_id_by_user.add(
            pivot_table_id_by_main_supporter, fill_value=0
        )
    if not pivot_table_point_by_supporter_users.empty:
        df_id_by_user = df_id_by_user.add(
            pivot_table_id_by_supporter_users, fill_value=0
        )

    for x in range(df_point_by_user.shape[0]):
        logger.debug(
            f"支援者(主担当)、支援者(副担当)、営業担当者の集計結果を合算後: df_point_by_user.iloc[index]:{df_point_by_user.iloc[x]}"
        )

    # 処理対象のユーザID(重複を排除)
    target_user_id_list: list = list(
        set([str(index[0]) for index in df_point_by_user.index])
    )

    logger.debug(f"処理対象のユーザID: {target_user_id_list}")

    # 営業担当者のユーザ情報の取得
    user_model_list: List[UserModel] = []
    item_keys = [
        (project_survey.sales_user_id, DataType.USER)
        for project_survey in project_survey_list
    ]
    for item in UserModel.batch_get(item_keys):
        user_model_list.append(item)

    # アンケートサマリ：支援者（営業担当者）別集計の編集
    current_datetime = datetime.now()
    count = 0
    with SurveySummaryUserModel.batch_write() as survey_summary_user_batch:
        for user_id in target_user_id_list:
            count += 1
            logger.info(
                f"data_type:{SurveySummaryDataTypePrefix.USER + user_id},year_month:{summary_year_month} - 処理開始 ({count}/{len(target_user_id_list)})"
            )
            # DB編集
            name = ""
            for project_survey in project_survey_list:
                if project_survey.main_supporter_user:
                    if project_survey.main_supporter_user.id == user_id:
                        # 支援者(主担当)の場合
                        name = project_survey.main_supporter_user.name
                        break
                if project_survey.supporter_users:
                    for supporter in project_survey.supporter_users:
                        if supporter.id == user_id:
                            # 支援者(副担当)の場合
                            name = supporter.name
                            break
                if project_survey.sales_user_id == user_id:
                    # 営業担当者の場合
                    for user in user_model_list:
                        if user.id == user_id:
                            name = user.name
                            break

            # サービスアンケート
            service_receive = 0
            service_satisfaction_summary = 0
            service_satisfaction_unanswered = 0
            service_satisfaction_average = 0
            if (user_id, SurveyType.SERVICE) in df_point_by_user.index:
                df_service_point: pd.Series = df_point_by_user.loc[
                    (user_id, SurveyType.SERVICE), :
                ]
                df_service: pd.Series = df_id_by_user.loc[
                    (user_id, SurveyType.SERVICE), :
                ]
                service_receive = df_service.loc["id"].item()
                service_satisfaction_summary = df_service_point.loc[
                    ("sum", "points.satisfaction")
                ].item()
                service_satisfaction_unanswered = df_service_point.loc[
                    ("sum", "unanswered.satisfaction")
                ].item()
                service_satisfaction_average = decimal_round(
                    service_satisfaction_summary
                    / (service_receive - service_satisfaction_unanswered)
                    if service_satisfaction_summary
                    and (service_receive - service_satisfaction_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS,
                )

            # 修了アンケート
            completion_receive = 0
            completion_satisfaction_summary = 0
            completion_satisfaction_unanswered = 0
            completion_satisfaction_average = 0
            completion_continuation = CompletionContinuationSubAttribute(
                positive_count=0, negative_count=0
            )
            completion_continuation_unanswered = 0
            completion_continuation_percent = 0
            completion_recommended_summary = 0
            completion_recommended_unanswered = 0
            completion_recommended_average = 0
            completion_sales_summary = 0
            completion_sales_unanswered = 0
            completion_sales_average = 0
            if (user_id, SurveyType.COMPLETION) in df_point_by_user.index:
                df_completion_point: pd.Series = df_point_by_user.loc[
                    (user_id, SurveyType.COMPLETION), :
                ]
                df_completion: pd.Series = df_id_by_user.loc[
                    (user_id, SurveyType.COMPLETION), :
                ]
                completion_receive = df_completion.loc["id"].item()
                completion_satisfaction_summary = df_completion_point.loc[
                    ("sum", "points.satisfaction")
                ].item()
                completion_satisfaction_unanswered = df_completion_point.loc[
                    ("sum", "unanswered.satisfaction")
                ].item()
                completion_satisfaction_average = decimal_round(
                    completion_satisfaction_summary
                    / (completion_receive - completion_satisfaction_unanswered)
                    if completion_satisfaction_summary
                    and (completion_receive - completion_satisfaction_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS,
                )
                completion_continuation = CompletionContinuationSubAttribute(
                    positive_count=df_completion_point.loc[
                        ("sum", "points.continuation")
                    ].item(),
                    negative_count=(
                        df_completion_point.loc[("count", "points.continuation")]
                        - df_completion_point.loc[("sum", "points.continuation")]
                    ).item(),
                )
                completion_continuation_unanswered = df_completion_point.loc[
                    ("sum", "unanswered.continuation")
                ].item()
                completion_continuation_percent = decimal_round(
                    completion_continuation.positive_count
                    / (completion_receive - completion_continuation_unanswered)
                    if completion_continuation.positive_count
                    and (completion_receive - completion_continuation_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
                )
                completion_recommended_summary = df_completion_point.loc[
                    ("sum", "points.recommended")
                ].item()
                completion_recommended_unanswered = df_completion_point.loc[
                    ("sum", "unanswered.recommended")
                ].item()
                completion_recommended_average = decimal_round(
                    completion_recommended_summary
                    / (completion_receive - completion_recommended_unanswered)
                    if completion_recommended_summary
                    and (completion_receive - completion_recommended_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS,
                )
                completion_sales_summary = df_completion_point.loc[
                    ("sum", "points.sales")
                ].item()
                completion_sales_unanswered = df_completion_point.loc[
                    ("sum", "unanswered.sales")
                ].item()
                completion_sales_average = decimal_round(
                    completion_sales_summary
                    / (completion_receive - completion_sales_unanswered)
                    if completion_sales_summary
                    and (completion_receive - completion_sales_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS,
                )

            # クイックアンケート
            quick_receive = 0
            if (user_id, SurveyType.QUICK) in df_point_by_user.index:
                df_quick: pd.Series = df_id_by_user.loc[(user_id, SurveyType.QUICK), :]
                quick_receive = df_quick.loc["id"].item()

            new_summary = SurveySummaryUserModel(
                summary_month=summary_year_month,
                data_type=SurveySummaryDataTypePrefix.USER + user_id,
                name=name,
                service_satisfaction_summary=service_satisfaction_summary,
                service_satisfaction_average=service_satisfaction_average,
                service_satisfaction_unanswered=service_satisfaction_unanswered,
                service_receive=service_receive,
                completion_satisfaction_summary=completion_satisfaction_summary,
                completion_satisfaction_average=completion_satisfaction_average,
                completion_satisfaction_unanswered=completion_satisfaction_unanswered,
                completion_continuation=completion_continuation,
                completion_continuation_percent=completion_continuation_percent,
                completion_continuation_unanswered=completion_continuation_unanswered,
                completion_recommended_summary=completion_recommended_summary,
                completion_recommended_average=completion_recommended_average,
                completion_recommended_unanswered=completion_recommended_unanswered,
                completion_sales_summary=completion_sales_summary,
                completion_sales_average=completion_sales_average,
                completion_sales_unanswered=completion_sales_unanswered,
                completion_receive=completion_receive,
                quick_receive=quick_receive,
                create_at=current_datetime,
                update_at=current_datetime,
            )
            survey_summary_user_batch.save(new_summary)


def save_supporter_organization_summary_of_summary_month(
    summary_year_month: str,
) -> None:
    """
        支援者組織（粗利メイン課）別集計の編集・登録
        ・指定された基準年月の支援者組織（粗利メイン課）別集計をアンケートサマリ情報に登録
    Args:
        summary_year_month (str): 基準年月(YYYY/MM)
    """
    summary_month_datetime: datetime = datetime.strptime(summary_year_month, "%Y/%m")

    ########################################################
    # データ洗い替えの為、処理前に前回作成分の
    # 支援者組織（粗利メイン課）別集計の処理対象の年月のデータを削除
    ########################################################
    logger.info(f"集計月{summary_year_month}の前回作成データ削除")
    del_year_month = summary_month_datetime.strftime("%Y/%m")
    del_survey_summary_supporter_organization_range_key_condition = (
        SurveySummarySupporterOrganizationModel.data_type.startswith(
            SurveySummaryDataTypePrefix.SUPPORTER_ORGANIZATION
        )
    )
    del_survey_summary_supporter_organization_list: List[
        SurveySummarySupporterOrganizationModel
    ] = list(
        SurveySummarySupporterOrganizationModel.query(
            hash_key=del_year_month,
            range_key_condition=del_survey_summary_supporter_organization_range_key_condition,
        )
    )
    with (
        SurveySummarySupporterOrganizationModel.batch_write() as del_survey_summary_supporter_organization_batch
    ):
        count = 0
        for del_summary in del_survey_summary_supporter_organization_list:
            count += 1
            logger.info(
                f"集計月{summary_year_month}の前回作成データ削除 - 処理開始 ({count}/{len(del_survey_summary_supporter_organization_list)})"
            )
            del_survey_summary_supporter_organization_batch.delete(del_summary)

    ###############################
    # 処理対象のアンケートの取得
    ###############################
    # 抽出条件:
    #  集計月が処理年月と一致するもの
    #  Partner Portalアンケート以外
    #  集計対象外でない
    #  回答済
    project_survey_range_key_condition = (
        ProjectSurveyModel.summary_month == summary_year_month
    )
    # flake8でエラーが出るため、boolを変数で定義
    true_bool: bool = True
    false_bool: bool = False
    project_survey_filter_condition = None
    project_survey_filter_condition &= ProjectSurveyModel.survey_type != SurveyType.PP
    project_survey_filter_condition &= ProjectSurveyModel.is_not_summary == false_bool
    project_survey_filter_condition &= ProjectSurveyModel.is_finished == true_bool

    project_survey_list: List[ProjectSurveyModel] = list(
        ProjectSurveyModel.data_type_summary_month_index.query(
            hash_key=DataType.SURVEY,
            range_key_condition=project_survey_range_key_condition,
            filter_condition=project_survey_filter_condition,
        )
    )
    logger.info(
        f"集計月{summary_year_month}のアンケート情報の取得件数: {len(project_survey_list)}件"
    )

    if not project_survey_list:
        # 対象データ0件の場合、処理修了
        logger.info(f"アンケート情報が0件のため、集計月{summary_year_month}の処理終了")
        return

    # 汎用マスタ:支援者組織情報
    master_supporter_organization_list: List[MasterSupporterOrganizationModel] = list(
        MasterSupporterOrganizationModel.data_type_index.query(
            hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION
        )
    )

    # modelのリスト -> pandasのDataFrame (Map等は子要素も展開.但し、リスト部分は展開不可)
    # e.g. 以下を1行とするDataFrame
    #  df:
    #   create_at                                                 2022-08-03T20:14:25.437270+09:00
    #   is_disclosure                                                                         True
    #   is_finished                                                                           True
    #   is_not_summary                                                                       False
    #   ....中略.....
    #   answer_user_id                                        b9b67094-cdab-494c-818e-d4845088269b
    #   answer_user_name                                                                 田中 次郎
    #   answers                              [{'answer': '問題なし', 'id': '89cbe2ed-f44c-4a1c-...
    #   company                                                              ソニーグループ株式会社
    #   ....中略.....
    #   customer_id                                           d6121808-341d-4883-8e2c-69462acf6ccb
    #   customer_name                                                                 あああ研究所
    #   customer_success                                                                  DXの実現
    #   data_type                                                                           survey
    #   id                                                    668ac0a7-b0fb-4970-9753-8fceb59d02a9
    #   ....中略.....
    #   project_id                                            96103959-e438-4293-8b8b-3a990bb18de9
    #   project_name                                                           サンプルプロジェクト
    #   sales_user_id                                         7bf1b7e4-5625-4361-be0a-65f0e49829ea
    #   service_type_id                                       7ac8bddf-88da-46c9-a504-a03d1661ad58
    #   service_type_name                                                                 組織開発
    #   summary_month                                                                      2022/06
    #   support_date_from                                                               2021/06/01
    #   support_date_to                                                                 2022/12/31
    #   supporter_organization_id                             de40733f-6be9-4fef-8229-01052f43c1e2
    #   supporter_organization_name                                                            AST
    #   supporter_users                          [{'id': 'b9b67094-cdab-494c-818e-d4845088269b'...
    #   survey_master_id                                      9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf
    #   survey_master_revision                                                                   4
    #   survey_type                                                                        service
    #   ....中略.....
    #   main_supporter_user.id                                c9b67094-cdab-494c-818e-d4845088269b
    #   main_supporter_user.name                                                         田中 三郎
    #   main_supporter_user.organization_id                   de40733f-6be9-4fef-8229-01052f43c1e2
    #   main_supporter_user.organization_name                                                  AST
    #   points.continuation                                                                  False
    #   points.master_karte_satisfaction                                                         0
    #   points.karte_satisfaction                                                                3
    #   points.man_hour_satisfaction                                                             3
    #   points.recommended                                                                       4
    #   points.sales                                                                             0
    #   points.satisfaction                                                                      4
    #   points.survey_satisfaction                                                               3
    #   points.unanswered                                                                  {sales}
    #   .........
    df = pd.json_normalize([model_to_dict(x) for x in project_survey_list])
    # 粗利メイン課が設定されていない場合、追加
    if "supporter_organization_id" not in df.columns:
        df["supporter_organization_id"] = ""
    if "supporter_organization_name" not in df.columns:
        df["supporter_organization_name"] = ""
    # 粗利メイン課がNaNの場合、空文字に置換
    df = df.fillna({"supporter_organization_id": "", "supporter_organization_name": ""})

    # 上記のDataFrameに集計用の未回答項目の列を追加（points.unansweredの値を基に生成）
    # e.g.
    #  unanswered.satisfaction                                                                  1
    #  unanswered.continuation                                                                  0
    #  unanswered.recommended                                                                   0
    #  unanswered.sales                                                                         0
    #  unanswered.survey_satisfaction                                                           0
    #  unanswered.man_hour_satisfaction                                                         0
    #  unanswered.karte_satisfaction                                                            0
    #  unanswered.master_karte_satisfaction                                                     0
    add_unanswered_column: dict = {
        "unanswered.satisfaction": SurveyUnansweredColumn.SATISFACTION,
        "unanswered.continuation": SurveyUnansweredColumn.CONTINUATION,
        "unanswered.recommended": SurveyUnansweredColumn.RECOMMENDED,
        "unanswered.sales": SurveyUnansweredColumn.SALES,
        "unanswered.survey_satisfaction": SurveyUnansweredColumn.SURVEY_SATISFACTION,
        "unanswered.man_hour_satisfaction": SurveyUnansweredColumn.MAN_HOUR_SATISFACTION,
        "unanswered.karte_satisfaction": SurveyUnansweredColumn.KARTE_SATISFACTION,
        "unanswered.master_karte_satisfaction": SurveyUnansweredColumn.MASTER_KARTE_SATISFACTION,
    }
    if "points.unanswered" not in df.columns:
        df["points.unanswered"] = [set() for _ in range(df.shape[0])]
    for k, v in add_unanswered_column.items():
        df[k] = df["points.unanswered"].apply(calc_unanswered_column, unanswered_col=v)

    # 支援者組織（粗利メイン課）で集計
    # e.g.
    #  pivot_table_supporter_organization:
    #                                                                   sum  ...                   count
    #                                                   points.continuation  ... unanswered.satisfaction
    #  supporter_organization_id            survey_type                      ...
    #  180a3597-b7e7-42c8-902c-a29016afa662 completion                    1  ...                       1
    #                                       quick                         1  ...                       1
    #                                       service                       0  ...                       1
    #  de40733f-6be9-4fef-8229-01052f43c1e2 completion                    1  ...                       2
    #                                       quick                         0  ...                       1
    #                                       service                       0  ...                       2
    #
    # e.g. 以下を1行とするDataFrame
    #  name  (180a3597-b7e7-42c8-902c-a29016afa662, completion)
    #  pivot_table_supporter_organization.iloc[0]:
    #   sum    points.continuation        1
    #          points.recommended         3
    #          points.sales               2
    #          points.satisfaction        3
    #          unanswered.continuation    0
    #          unanswered.recommended     0
    #          unanswered.sales           0
    #          unanswered.satisfaction    0
    #   count  id                         1
    #          points.continuation        1
    #          points.recommended         1
    #          points.sales               1
    #          points.satisfaction        1
    #          unanswered.continuation    1
    #          unanswered.recommended     1
    #          unanswered.sales           1
    #          unanswered.satisfaction    1
    pivot_table_supporter_organization = pd.pivot_table(
        data=df,
        index=[
            "supporter_organization_id",
            "survey_type",
        ],
        values=[
            "points.satisfaction",
            "points.continuation",
            "points.recommended",
            "points.sales",
            "unanswered.satisfaction",
            "unanswered.continuation",
            "unanswered.recommended",
            "unanswered.sales",
            "id",
        ],
        aggfunc=["sum", "count"],
    ).fillna(0)

    for x in range(pivot_table_supporter_organization.shape[0]):
        logger.debug(
            f"支援者組織（粗利メイン課）単位: pivot_table_supporter_organization.iloc[index]:{pivot_table_supporter_organization.iloc[x]}"
        )

    # 処理対象の粗利メイン課IDを取得
    target_supporter_organization_id_list = [
        str(index[0]) for index in pivot_table_supporter_organization.index
    ]
    # 重複排除
    target_supporter_organization_id_list = list(
        set(target_supporter_organization_id_list)
    )

    logger.debug(f"処理対象の粗利メイン課ID: {target_supporter_organization_id_list}")

    # アンケートサマリ：支援者組織（粗利メイン課）別集計の編集
    current_datetime = datetime.now()
    count = 0
    with (
        SurveySummarySupporterOrganizationModel.batch_write() as survey_summary_supporter_organization_batch
    ):
        for supporter_organization_id in target_supporter_organization_id_list:
            count += 1
            logger.info(
                f"data_type:{SurveySummaryDataTypePrefix.SUPPORTER_ORGANIZATION + supporter_organization_id},year_month:{summary_year_month} - 処理開始 ({count}/{len(target_supporter_organization_id_list)})"
            )

            supporter_organization_name = ""
            for master_supporter_organization in master_supporter_organization_list:
                if master_supporter_organization.id == supporter_organization_id:
                    supporter_organization_name = master_supporter_organization.value
                    break

            # DB編集
            # サービスアンケート
            service_receive = 0
            service_satisfaction_summary = 0
            service_satisfaction_unanswered = 0
            service_satisfaction_average = 0
            if (
                supporter_organization_id,
                SurveyType.SERVICE,
            ) in pivot_table_supporter_organization.index:
                df_service_point: pd.Series = pivot_table_supporter_organization.loc[
                    (
                        supporter_organization_id,
                        SurveyType.SERVICE,
                    ),
                    :,
                ]
                # pynamodbで、int64のjson serializeエラーが発生するため、item()でスカラーを取得(以降、同様)
                service_receive = df_service_point.loc[("count", "id")].item()
                service_satisfaction_summary = df_service_point.loc[
                    ("sum", "points.satisfaction")
                ].item()
                service_satisfaction_unanswered = df_service_point.loc[
                    ("sum", "unanswered.satisfaction")
                ].item()
                service_satisfaction_average = decimal_round(
                    service_satisfaction_summary
                    / (service_receive - service_satisfaction_unanswered)
                    if service_satisfaction_summary
                    and (service_receive - service_satisfaction_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS,
                )

            # 修了アンケート
            completion_receive = 0
            completion_satisfaction_summary = 0
            completion_satisfaction_unanswered = 0
            completion_satisfaction_average = 0
            completion_continuation = CompletionContinuationSubAttribute(
                positive_count=0, negative_count=0
            )
            completion_continuation_unanswered = 0
            completion_continuation_percent = 0
            completion_recommended_summary = 0
            completion_recommended_unanswered = 0
            completion_recommended_average = 0
            completion_sales_summary = 0
            completion_sales_unanswered = 0
            completion_sales_average = 0
            if (
                supporter_organization_id,
                SurveyType.COMPLETION,
            ) in pivot_table_supporter_organization.index:
                df_completion_point: pd.Series = pivot_table_supporter_organization.loc[
                    (
                        supporter_organization_id,
                        SurveyType.COMPLETION,
                    ),
                    :,
                ]
                completion_receive = df_completion_point.loc[("count", "id")].item()
                completion_satisfaction_summary = df_completion_point.loc[
                    ("sum", "points.satisfaction")
                ].item()
                completion_satisfaction_unanswered = df_completion_point.loc[
                    ("sum", "unanswered.satisfaction")
                ].item()
                completion_satisfaction_average = decimal_round(
                    completion_satisfaction_summary
                    / (completion_receive - completion_satisfaction_unanswered)
                    if completion_satisfaction_summary
                    and (completion_receive - completion_satisfaction_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS,
                )
                completion_continuation = CompletionContinuationSubAttribute(
                    positive_count=df_completion_point.loc[
                        ("sum", "points.continuation")
                    ].item(),
                    negative_count=(
                        df_completion_point.loc[("count", "points.continuation")]
                        - df_completion_point.loc[("sum", "points.continuation")]
                    ).item(),
                )
                completion_continuation_unanswered = df_completion_point.loc[
                    ("sum", "unanswered.continuation")
                ].item()
                completion_continuation_percent = decimal_round(
                    completion_continuation.positive_count
                    / (completion_receive - completion_continuation_unanswered)
                    if completion_continuation.positive_count
                    and (completion_receive - completion_continuation_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
                )
                completion_recommended_summary = df_completion_point.loc[
                    ("sum", "points.recommended")
                ].item()
                completion_recommended_unanswered = df_completion_point.loc[
                    ("sum", "unanswered.recommended")
                ].item()
                completion_recommended_average = decimal_round(
                    completion_recommended_summary
                    / (completion_receive - completion_recommended_unanswered)
                    if completion_recommended_summary
                    and (completion_receive - completion_recommended_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS,
                )
                completion_sales_summary = df_completion_point.loc[
                    ("sum", "points.sales")
                ].item()
                completion_sales_unanswered = df_completion_point.loc[
                    ("sum", "unanswered.sales")
                ].item()
                completion_sales_average = decimal_round(
                    completion_sales_summary
                    / (completion_receive - completion_sales_unanswered)
                    if completion_sales_summary
                    and (completion_receive - completion_sales_unanswered)
                    else float(0),
                    RoundSetting.DECIMAL_DIGITS,
                )

            # 総合満足度
            total_satisfaction_summary = (
                service_satisfaction_summary + completion_satisfaction_summary
            )
            total_receive = service_receive + completion_receive
            total_satisfaction_average = decimal_round(
                total_satisfaction_summary
                / (
                    total_receive
                    - (
                        service_satisfaction_unanswered
                        + completion_satisfaction_unanswered
                    )
                )
                if total_satisfaction_summary
                and (
                    total_receive
                    - (
                        service_satisfaction_unanswered
                        + completion_satisfaction_unanswered
                    )
                )
                else float(0),
                RoundSetting.DECIMAL_DIGITS,
            )

            new_summary = SurveySummarySupporterOrganizationModel(
                summary_month=summary_year_month,
                data_type=SurveySummaryDataTypePrefix.SUPPORTER_ORGANIZATION
                + supporter_organization_id,
                supporter_organization_name=supporter_organization_name,
                service_satisfaction_summary=service_satisfaction_summary,
                service_satisfaction_average=service_satisfaction_average,
                service_satisfaction_unanswered=service_satisfaction_unanswered,
                service_receive=service_receive,
                completion_satisfaction_summary=completion_satisfaction_summary,
                completion_satisfaction_average=completion_satisfaction_average,
                completion_satisfaction_unanswered=completion_satisfaction_unanswered,
                completion_continuation=completion_continuation,
                completion_continuation_percent=completion_continuation_percent,
                completion_continuation_unanswered=completion_continuation_unanswered,
                completion_recommended_summary=completion_recommended_summary,
                completion_recommended_average=completion_recommended_average,
                completion_recommended_unanswered=completion_recommended_unanswered,
                completion_sales_summary=completion_sales_summary,
                completion_sales_average=completion_sales_average,
                completion_sales_unanswered=completion_sales_unanswered,
                completion_receive=completion_receive,
                total_satisfaction_summary=total_satisfaction_summary,
                total_satisfaction_average=total_satisfaction_average,
                total_receive=total_receive,
                create_at=current_datetime,
                update_at=current_datetime,
            )
            survey_summary_supporter_organization_batch.save(new_summary)


def save_all_summary_of_summary_month(
    summary_year_month: str,
) -> None:
    """
        全集計の編集・登録
        ・指定された基準年月の全集計をアンケートサマリ情報に登録
    Args:
        summary_year_month (str): 基準年月(YYYY/MM)
    """
    summary_month_datetime: datetime = datetime.strptime(summary_year_month, "%Y/%m")

    ########################################################
    # データ洗い替えの為、処理前に前回作成分の
    # 全集計の処理対象の年月のデータを削除
    ########################################################
    logger.info(f"集計月{summary_year_month}の前回作成データ削除")
    del_year_month = summary_month_datetime.strftime("%Y/%m")
    del_survey_all_summary_range_key_condition = (
        SurveySummaryAllModel.data_type.startswith(SurveySummaryDataTypePrefix.ALL)
    )
    del_survey_all_summary_list: List[SurveySummaryAllModel] = list(
        SurveySummaryAllModel.query(
            hash_key=del_year_month,
            range_key_condition=del_survey_all_summary_range_key_condition,
        )
    )
    with SurveySummaryAllModel.batch_write() as del_survey_all_summary_batch:
        count = 0
        for del_summary in del_survey_all_summary_list:
            count += 1
            logger.info(
                f"集計月{summary_year_month}の前回作成データ削除 - 処理開始 ({count}/{len(del_survey_all_summary_list)})"
            )
            del_survey_all_summary_batch.delete(del_summary)

    # flake8でエラーが出るため、boolを変数で定義
    true_bool: bool = True
    false_bool: bool = False
    ###############################
    # 集計月のアンケート送信数の取得
    ###############################
    # 抽出条件:
    #  集計月が処理年月と一致するもの
    #  集計対象外でない
    #  アンケート送信済（回答（送信）依頼実績日時が入力されているもの）
    project_survey_actual_request_range_key_condition = (
        ProjectSurveyModel.summary_month == summary_year_month
    )
    project_survey_actual_request_filter_condition = None
    project_survey_actual_request_filter_condition &= (
        ProjectSurveyModel.is_not_summary == false_bool
    )
    project_survey_actual_request_filter_condition &= (
        ProjectSurveyModel.actual_survey_request_datetime.exists()
        & (size(ProjectSurveyModel.actual_survey_request_datetime) > 0)
    )

    project_survey_actual_request_list: List[ProjectSurveyModel] = list(
        ProjectSurveyModel.data_type_summary_month_index.query(
            hash_key=DataType.SURVEY,
            range_key_condition=project_survey_actual_request_range_key_condition,
            filter_condition=project_survey_actual_request_filter_condition,
        )
    )
    logger.info(
        f"集計月{summary_year_month}の送信済アンケート情報（未回答含む）の取得件数: {len(project_survey_actual_request_list)}件"
    )
    actual_request_survey_count_of_service = 0
    actual_request_survey_count_of_completion = 0
    actual_request_survey_count_of_quick = 0
    actual_request_survey_count_of_pp = 0
    for actual_request_survey in project_survey_actual_request_list:
        if actual_request_survey.survey_type == SurveyType.SERVICE:
            actual_request_survey_count_of_service += 1
        elif actual_request_survey.survey_type == SurveyType.COMPLETION:
            actual_request_survey_count_of_completion += 1
        elif actual_request_survey.survey_type == SurveyType.QUICK:
            actual_request_survey_count_of_quick += 1
        elif actual_request_survey.survey_type == SurveyType.PP:
            actual_request_survey_count_of_pp += 1

    logger.info(
        f"集計月{summary_year_month}のサービスアンケート送信数: {str(actual_request_survey_count_of_service)}件"
    )
    logger.info(
        f"集計月{summary_year_month}の修了アンケート送信数: {str(actual_request_survey_count_of_completion)}件"
    )
    logger.info(
        f"集計月{summary_year_month}のクイックアンケート送信数: {str(actual_request_survey_count_of_quick)}件"
    )
    logger.info(
        f"集計月{summary_year_month}のPPアンケート送信数: {str(actual_request_survey_count_of_pp)}件"
    )

    ######################################################
    # カルテ集計
    ######################################################
    ######################################################
    # 案件情報取得
    # 抽出条件
    # ・集計月時点で支援期間中の案件
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
    project_filter_condition = ProjectModel.support_date_from <= summary_month_to
    project_model_list: List[ProjectModel] = list(
        ProjectModel.data_type_support_date_to_index.query(
            hash_key=DataType.PROJECT,
            range_key_condition=project_range_key_condition,
            filter_condition=project_filter_condition,
        )
    )
    logger.info(
        f"集計月{summary_year_month}の支援期間中の案件情報取得件数: {len(project_model_list)}件"
    )
    for project in project_model_list:
        logger.info(f"案件ID: {project.id}")

    # カルテ当月支援期間中の案件数
    karte_project_count: int = len(project_model_list)

    logger.debug(f"カルテ当月支援期間中の案件数: {karte_project_count}")

    ######################################################
    # 案件カルテ情報取得
    # 抽出条件
    # ・案件ID
    # ・公開済（is_draft=False）
    # ・支援日が集計月のもの
    ######################################################
    # 当月一回でもカルテを利用した案件数（公開済みのカルテがあるかで判断する）
    karte_use_count: int = 0

    for project in project_model_list:
        karte_filter_condition = None
        karte_filter_condition &= ProjectKarteModel.is_draft == false_bool
        karte_filter_condition &= ProjectKarteModel.date >= summary_month_from
        karte_filter_condition &= ProjectKarteModel.date <= summary_month_to
        project_karte_list: List[ProjectKarteModel] = list(
            ProjectKarteModel.project_id_start_datetime_index.query(
                hash_key=project.id,
                filter_condition=karte_filter_condition,
            )
        )
        logger.info(
            f"案件ID({project.id})の案件カルテ(公開済)情報取得件数: {len(project_karte_list)}件"
        )
        if len(project_karte_list) > 0:
            # 利用したカルテが1件でもある場合、カウントアップ
            karte_use_count += 1

    logger.debug(f"当月一回でもカルテを利用した案件数: {karte_use_count}")

    # カルテ利用率
    karte_use_percent = decimal_round(
        karte_use_count / karte_project_count
        if karte_use_count and karte_project_count
        else float(0),
        RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
    )

    logger.debug(f"カルテ利用率: {karte_use_percent}")

    ###############################
    # 処理対象のアンケートの取得
    ###############################
    # 抽出条件:
    #  集計月が処理年月と一致するもの
    #  集計対象外でない
    #  回答済
    project_survey_range_key_condition = (
        ProjectSurveyModel.summary_month == summary_year_month
    )
    project_survey_filter_condition = None
    project_survey_filter_condition &= ProjectSurveyModel.is_not_summary == false_bool
    project_survey_filter_condition &= ProjectSurveyModel.is_finished == true_bool

    project_survey_list: List[ProjectSurveyModel] = list(
        ProjectSurveyModel.data_type_summary_month_index.query(
            hash_key=DataType.SURVEY,
            range_key_condition=project_survey_range_key_condition,
            filter_condition=project_survey_filter_condition,
        )
    )
    logger.info(
        f"集計月{summary_year_month}のアンケート情報の取得件数: {len(project_survey_list)}件"
    )

    ###############################
    # 対象データ0件の場合
    ###############################
    # 0件の場合も集計データを作成する
    if not project_survey_list:
        # アンケートサマリ：全集計の編集
        current_datetime = datetime.now()
        logger.info(
            f"data_type:{SurveySummaryDataTypePrefix.ALL},year_month:{summary_year_month} - 処理開始 (1/1)"
        )
        # DB編集
        # サービスアンケート
        service_project_count = actual_request_survey_count_of_service
        service_receive_count = 0
        service_receive_percent = 0
        service_satisfaction_summary = 0
        service_satisfaction_average = 0
        service_satisfaction_unanswered = 0
        # 修了アンケート
        completion_project_count = actual_request_survey_count_of_completion
        completion_receive_count = 0
        completion_receive_percent = 0
        completion_satisfaction_summary = 0
        completion_satisfaction_average = 0
        completion_satisfaction_unanswered = 0
        completion_continuation = ContinuationSubAttribute(
            positive_count=0,
            negative_count=0,
            percent=0,
            unanswered=0,
        )
        completion_recommended_summary = 0
        completion_recommended_average = 0
        completion_recommended_unanswered = 0
        completion_sales_summary = 0
        completion_sales_average = 0
        completion_sales_unanswered = 0
        # クイックアンケート
        quick_send_count = actual_request_survey_count_of_quick
        quick_receive_count = 0
        # PPアンケート
        pp_survey_satisfaction_summary = 0
        pp_survey_satisfaction_average = 0
        pp_man_hour_satisfaction_summary = 0
        pp_man_hour_satisfaction_average = 0
        pp_karte_satisfaction_summary = 0
        pp_karte_satisfaction_average = 0
        pp_master_karte_satisfaction_summary = 0
        pp_master_karte_satisfaction_average = 0
        pp_send_count = actual_request_survey_count_of_pp
        pp_receive_count = 0
        pp_receive_percent = 0

        new_summary = SurveySummaryAllModel(
            summary_month=summary_year_month,
            data_type=SurveySummaryDataTypePrefix.ALL,
            service=ServiceAttribute(
                project_count=service_project_count,
                receive_count=service_receive_count,
                receive_percent=service_receive_percent,
                satisfaction_summary=service_satisfaction_summary,
                satisfaction_average=service_satisfaction_average,
                satisfaction_unanswered=service_satisfaction_unanswered,
            ),
            completion=CompletionAttribute(
                project_count=completion_project_count,
                receive_count=completion_receive_count,
                receive_percent=completion_receive_percent,
                satisfaction_summary=completion_satisfaction_summary,
                satisfaction_average=completion_satisfaction_average,
                satisfaction_unanswered=completion_satisfaction_unanswered,
                continuation=completion_continuation,
                recommended_summary=completion_recommended_summary,
                recommended_average=completion_recommended_average,
                recommended_unanswered=completion_recommended_unanswered,
                sales_summary=completion_sales_summary,
                sales_average=completion_sales_average,
                sales_unanswered=completion_sales_unanswered,
            ),
            karte=KarteAttribute(
                project_count=karte_project_count,
                karte_count=karte_use_count,
                use_percent=karte_use_percent,
            ),
            quick=QuickAttribute(
                send_count=quick_send_count,
                receive_count=quick_receive_count,
            ),
            pp=PpAttribute(
                survey_satisfaction_summary=pp_survey_satisfaction_summary,
                survey_satisfaction_average=pp_survey_satisfaction_average,
                man_hour_satisfaction_summary=pp_man_hour_satisfaction_summary,
                man_hour_satisfaction_average=pp_man_hour_satisfaction_average,
                karte_satisfaction_summary=pp_karte_satisfaction_summary,
                karte_satisfaction_average=pp_karte_satisfaction_average,
                master_karte_satisfaction_summary=pp_master_karte_satisfaction_summary,
                master_karte_satisfaction_average=pp_master_karte_satisfaction_average,
                send_count=pp_send_count,
                receive_count=pp_receive_count,
                receive_percent=pp_receive_percent,
            ),
            create_at=current_datetime,
            update_at=current_datetime,
        )
        new_summary.save()

        # 終了
        return

    # modelのリスト -> pandasのDataFrame (Map等は子要素も展開.但し、リスト部分は展開不可)
    # e.g. 以下を1行とするDataFrame
    #  df:
    #   create_at                                                 2022-08-03T20:14:25.437270+09:00
    #   is_disclosure                                                                         True
    #   is_finished                                                                           True
    #   is_not_summary                                                                       False
    #   ....中略.....
    #   answer_user_id                                        b9b67094-cdab-494c-818e-d4845088269b
    #   answer_user_name                                                                 田中 次郎
    #   answers                              [{'answer': '問題なし', 'id': '89cbe2ed-f44c-4a1c-...
    #   company                                                              ソニーグループ株式会社
    #   ....中略.....
    #   customer_id                                           d6121808-341d-4883-8e2c-69462acf6ccb
    #   customer_name                                                                 あああ研究所
    #   customer_success                                                                  DXの実現
    #   data_type                                                                           survey
    #   id                                                    668ac0a7-b0fb-4970-9753-8fceb59d02a9
    #   ....中略.....
    #   project_id                                            96103959-e438-4293-8b8b-3a990bb18de9
    #   project_name                                                           サンプルプロジェクト
    #   sales_user_id                                         7bf1b7e4-5625-4361-be0a-65f0e49829ea
    #   service_type_id                                       7ac8bddf-88da-46c9-a504-a03d1661ad58
    #   service_type_name                                                                 組織開発
    #   summary_month                                                                      2022/06
    #   support_date_from                                                               2021/06/01
    #   support_date_to                                                                 2022/12/31
    #   supporter_organization_id                             de40733f-6be9-4fef-8229-01052f43c1e2
    #   supporter_organization_name                                                            AST
    #   supporter_users                          [{'id': 'b9b67094-cdab-494c-818e-d4845088269b'...
    #   survey_master_id                                      9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf
    #   survey_master_revision                                                                   4
    #   survey_type                                                                        service
    #   ....中略.....
    #   main_supporter_user.id                                c9b67094-cdab-494c-818e-d4845088269b
    #   main_supporter_user.name                                                         田中 三郎
    #   main_supporter_user.organization_id                   de40733f-6be9-4fef-8229-01052f43c1e2
    #   main_supporter_user.organization_name                                                  AST
    #   points.continuation                                                                  False
    #   points.master_karte_satisfaction                                                         0
    #   points.karte_satisfaction                                                                3
    #   points.man_hour_satisfaction                                                             3
    #   points.recommended                                                                       4
    #   points.sales                                                                             0
    #   points.satisfaction                                                                      4
    #   points.survey_satisfaction                                                               3
    #   points.unanswered                                                                  {sales}
    #   .........
    df = pd.json_normalize([model_to_dict(x) for x in project_survey_list])

    # 上記のDataFrameに集計用の未回答項目の列を追加（points.unansweredの値を基に生成）
    # e.g.
    #  unanswered.satisfaction                                                                  1
    #  unanswered.continuation                                                                  0
    #  unanswered.recommended                                                                   0
    #  unanswered.sales                                                                         0
    #  unanswered.survey_satisfaction                                                           0
    #  unanswered.man_hour_satisfaction                                                         0
    #  unanswered.karte_satisfaction                                                            0
    #  unanswered.master_karte_satisfaction                                                     0
    add_unanswered_column: dict = {
        "unanswered.satisfaction": SurveyUnansweredColumn.SATISFACTION,
        "unanswered.continuation": SurveyUnansweredColumn.CONTINUATION,
        "unanswered.recommended": SurveyUnansweredColumn.RECOMMENDED,
        "unanswered.sales": SurveyUnansweredColumn.SALES,
        "unanswered.survey_satisfaction": SurveyUnansweredColumn.SURVEY_SATISFACTION,
        "unanswered.man_hour_satisfaction": SurveyUnansweredColumn.MAN_HOUR_SATISFACTION,
        "unanswered.karte_satisfaction": SurveyUnansweredColumn.KARTE_SATISFACTION,
        "unanswered.master_karte_satisfaction": SurveyUnansweredColumn.MASTER_KARTE_SATISFACTION,
    }
    if "points.unanswered" not in df.columns:
        df["points.unanswered"] = [set() for _ in range(df.shape[0])]
    for k, v in add_unanswered_column.items():
        df[k] = df["points.unanswered"].apply(calc_unanswered_column, unanswered_col=v)

    # 全集計
    # e.g.
    #  pivot_table_survey_type:
    #                               sum                            ...            count
    #               points.continuation points.karte_satisfaction  ... unanswered.sales unanswered.satisfaction
    #   survey_type                                                ...
    #   completion                    2                         9  ...                3                       3
    #   quick                         1                         8  ...                2                       2
    #   service                       0                         9  ...                3                       3

    #
    # e.g. 以下を1行とするDataFrame
    #  pivot_table_survey_type.loc['completion']:
    #   sum    points.continuation              2
    #          points.master_karte_satisfaction 9
    #          points.karte_satisfaction        9
    #          points.man_hour_satisfaction     9
    #          points.recommended              13
    #          points.sales                     9
    #          points.satisfaction             12
    #          points.survey_satisfaction       0
    #          unanswered.continuation          0
    #          unanswered.recommended           1
    #          unanswered.sales                 0
    #          unanswered.satisfaction          0
    #   count  id                               3
    #          points.continuation              3
    #          points.master_karte_satisfaction 3
    #          points.karte_satisfaction        3
    #          points.man_hour_satisfaction     3
    #          points.recommended               3
    #          points.sales                     3
    #          points.satisfaction              3
    #          points.survey_satisfaction       3
    #          unanswered.continuation          3
    #          unanswered.recommended           3
    #          unanswered.sales                 3
    #          unanswered.satisfaction          3
    pivot_table_survey_type = pd.pivot_table(
        data=df,
        index=[
            "survey_type",
        ],
        values=[
            "points.satisfaction",
            "points.continuation",
            "points.recommended",
            "points.sales",
            "points.survey_satisfaction",
            "points.man_hour_satisfaction",
            "points.karte_satisfaction",
            "points.master_karte_satisfaction",
            "unanswered.satisfaction",
            "unanswered.continuation",
            "unanswered.recommended",
            "unanswered.sales",
            "id",
        ],
        aggfunc=["sum", "count"],
    ).fillna(0)

    for x in range(pivot_table_survey_type.shape[0]):
        logger.debug(
            f"アンケート種類単位: pivot_table_survey_type.iloc[index]:{pivot_table_survey_type.iloc[x]}"
        )

    # アンケートサマリ：全集計の編集
    current_datetime = datetime.now()
    logger.info(
        f"data_type:{SurveySummaryDataTypePrefix.ALL},year_month:{summary_year_month} - 処理開始 (1/1)"
    )
    # DB編集
    # サービスアンケート
    service_project_count = actual_request_survey_count_of_service
    service_receive_count = 0
    service_receive_percent = 0
    service_satisfaction_summary = 0
    service_satisfaction_average = 0
    service_satisfaction_unanswered = 0

    if SurveyType.SERVICE in pivot_table_survey_type.index:
        df_service_point: pd.Series = pivot_table_survey_type.loc[
            SurveyType.SERVICE,
            :,
        ]
        # pynamodbで、int64のjson serializeエラーが発生するため、item()でスカラーを取得(以降、同様)
        service_receive_count = df_service_point.loc[("count", "id")].item()
        service_receive_percent = decimal_round(
            service_receive_count / service_project_count
            if service_receive_count and service_project_count
            else float(0),
            RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
        )
        service_satisfaction_summary = df_service_point.loc[
            ("sum", "points.satisfaction")
        ].item()
        service_satisfaction_unanswered = df_service_point.loc[
            ("sum", "unanswered.satisfaction")
        ].item()
        service_satisfaction_average = decimal_round(
            service_satisfaction_summary
            / (service_receive_count - service_satisfaction_unanswered)
            if service_satisfaction_summary
            and (service_receive_count - service_satisfaction_unanswered)
            else float(0),
            RoundSetting.DECIMAL_DIGITS,
        )

    # 修了アンケート
    completion_project_count = actual_request_survey_count_of_completion
    completion_receive_count = 0
    completion_receive_percent = 0
    completion_satisfaction_summary = 0
    completion_satisfaction_average = 0
    completion_satisfaction_unanswered = 0
    completion_continuation = ContinuationSubAttribute(
        positive_count=0,
        negative_count=0,
        percent=0,
        unanswered=0,
    )
    completion_recommended_summary = 0
    completion_recommended_average = 0
    completion_recommended_unanswered = 0
    completion_sales_summary = 0
    completion_sales_average = 0
    completion_sales_unanswered = 0

    if SurveyType.COMPLETION in pivot_table_survey_type.index:
        df_completion_point: pd.Series = pivot_table_survey_type.loc[
            SurveyType.COMPLETION,
            :,
        ]
        completion_receive_count = df_completion_point.loc[("count", "id")].item()
        completion_receive_percent = decimal_round(
            completion_receive_count / completion_project_count
            if completion_receive_count and completion_project_count
            else float(0),
            RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
        )
        completion_satisfaction_summary = df_completion_point.loc[
            ("sum", "points.satisfaction")
        ].item()
        completion_satisfaction_unanswered = df_completion_point.loc[
            ("sum", "unanswered.satisfaction")
        ].item()
        completion_satisfaction_average = decimal_round(
            completion_satisfaction_summary
            / (completion_receive_count - completion_satisfaction_unanswered)
            if completion_satisfaction_summary
            and (completion_receive_count - completion_satisfaction_unanswered)
            else float(0),
            RoundSetting.DECIMAL_DIGITS,
        )

        completion_continuation_positive_count = df_completion_point.loc[
            ("sum", "points.continuation")
        ].item()
        completion_continuation_unanswered = df_completion_point.loc[
            ("sum", "unanswered.continuation")
        ].item()
        completion_continuation_percent = decimal_round(
            completion_continuation_positive_count
            / (completion_receive_count - completion_continuation_unanswered)
            if completion_satisfaction_summary
            and (completion_receive_count - completion_continuation_unanswered)
            else float(0),
            RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
        )
        completion_continuation = ContinuationSubAttribute(
            positive_count=completion_continuation_positive_count,
            negative_count=df_completion_point.loc[
                ("count", "points.continuation")
            ].item()
            - completion_continuation_positive_count,
            percent=completion_continuation_percent,
            unanswered=completion_continuation_unanswered,
        )

        completion_recommended_summary = df_completion_point.loc[
            ("sum", "points.recommended")
        ].item()
        completion_recommended_unanswered = df_completion_point.loc[
            ("sum", "unanswered.recommended")
        ].item()
        completion_recommended_average = decimal_round(
            completion_recommended_summary
            / (completion_receive_count - completion_recommended_unanswered)
            if completion_recommended_summary
            and (completion_receive_count - completion_recommended_unanswered)
            else float(0),
            RoundSetting.DECIMAL_DIGITS,
        )
        completion_sales_summary = df_completion_point.loc[
            ("sum", "points.sales")
        ].item()
        completion_sales_unanswered = df_completion_point.loc[
            ("sum", "unanswered.sales")
        ].item()
        completion_sales_average = decimal_round(
            completion_sales_summary
            / (completion_receive_count - completion_sales_unanswered)
            if completion_sales_summary
            and (completion_receive_count - completion_sales_unanswered)
            else float(0),
            RoundSetting.DECIMAL_DIGITS,
        )

    # クイックアンケート
    quick_send_count = actual_request_survey_count_of_quick
    quick_receive_count = 0
    if SurveyType.QUICK in pivot_table_survey_type.index:
        df_quick_point: pd.Series = pivot_table_survey_type.loc[
            SurveyType.QUICK,
            :,
        ]
        quick_receive_count = df_quick_point.loc[("count", "id")].item()

    # PPアンケート
    pp_survey_satisfaction_summary = 0
    pp_survey_satisfaction_average = 0
    pp_man_hour_satisfaction_summary = 0
    pp_man_hour_satisfaction_average = 0
    pp_karte_satisfaction_summary = 0
    pp_karte_satisfaction_average = 0
    pp_master_karte_satisfaction_summary = 0
    pp_master_karte_satisfaction_average = 0
    pp_send_count = actual_request_survey_count_of_pp
    pp_receive_count = 0
    pp_receive_percent = 0
    if SurveyType.PP in pivot_table_survey_type.index:
        df_pp_point: pd.Series = pivot_table_survey_type.loc[
            SurveyType.PP,
            :,
        ]
        pp_receive_count = df_pp_point.loc[("count", "id")].item()
        pp_receive_percent = decimal_round(
            pp_receive_count / pp_send_count
            if pp_receive_count and pp_send_count
            else float(0),
            RoundSetting.DECIMAL_DIGITS_RATE_COLUMN,
        )
        pp_survey_satisfaction_summary = df_pp_point.loc[
            ("sum", "points.survey_satisfaction")
        ].item()
        pp_survey_satisfaction_average = decimal_round(
            pp_survey_satisfaction_summary / pp_receive_count
            if pp_survey_satisfaction_summary and pp_receive_count
            else float(0),
            RoundSetting.DECIMAL_DIGITS,
        )
        pp_man_hour_satisfaction_summary = df_pp_point.loc[
            ("sum", "points.man_hour_satisfaction")
        ].item()
        pp_man_hour_satisfaction_average = decimal_round(
            pp_man_hour_satisfaction_summary / pp_receive_count
            if pp_man_hour_satisfaction_summary and pp_receive_count
            else float(0),
            RoundSetting.DECIMAL_DIGITS,
        )
        pp_karte_satisfaction_summary = df_pp_point.loc[
            ("sum", "points.karte_satisfaction")
        ].item()
        pp_karte_satisfaction_average = decimal_round(
            pp_karte_satisfaction_summary / pp_receive_count
            if pp_karte_satisfaction_summary and pp_receive_count
            else float(0),
            RoundSetting.DECIMAL_DIGITS,
        )
        pp_master_karte_satisfaction_summary = df_pp_point.loc[
            ("sum", "points.master_karte_satisfaction")
        ].item()
        pp_master_karte_satisfaction_average = decimal_round(
            pp_master_karte_satisfaction_summary / pp_receive_count
            if pp_master_karte_satisfaction_summary and pp_receive_count
            else float(0),
            RoundSetting.DECIMAL_DIGITS,
        )

    new_summary = SurveySummaryAllModel(
        summary_month=summary_year_month,
        data_type=SurveySummaryDataTypePrefix.ALL,
        service=ServiceAttribute(
            project_count=service_project_count,
            receive_count=service_receive_count,
            receive_percent=service_receive_percent,
            satisfaction_summary=service_satisfaction_summary,
            satisfaction_average=service_satisfaction_average,
            satisfaction_unanswered=service_satisfaction_unanswered,
        ),
        completion=CompletionAttribute(
            project_count=completion_project_count,
            receive_count=completion_receive_count,
            receive_percent=completion_receive_percent,
            satisfaction_summary=completion_satisfaction_summary,
            satisfaction_average=completion_satisfaction_average,
            satisfaction_unanswered=completion_satisfaction_unanswered,
            continuation=completion_continuation,
            recommended_summary=completion_recommended_summary,
            recommended_average=completion_recommended_average,
            recommended_unanswered=completion_recommended_unanswered,
            sales_summary=completion_sales_summary,
            sales_average=completion_sales_average,
            sales_unanswered=completion_sales_unanswered,
        ),
        karte=KarteAttribute(
            project_count=karte_project_count,
            karte_count=karte_use_count,
            use_percent=karte_use_percent,
        ),
        quick=QuickAttribute(
            send_count=quick_send_count,
            receive_count=quick_receive_count,
        ),
        pp=PpAttribute(
            survey_satisfaction_summary=pp_survey_satisfaction_summary,
            survey_satisfaction_average=pp_survey_satisfaction_average,
            man_hour_satisfaction_summary=pp_man_hour_satisfaction_summary,
            man_hour_satisfaction_average=pp_man_hour_satisfaction_average,
            karte_satisfaction_summary=pp_karte_satisfaction_summary,
            karte_satisfaction_average=pp_karte_satisfaction_average,
            master_karte_satisfaction_summary=pp_master_karte_satisfaction_summary,
            master_karte_satisfaction_average=pp_master_karte_satisfaction_average,
            send_count=pp_send_count,
            receive_count=pp_receive_count,
            receive_percent=pp_receive_percent,
        ),
        create_at=current_datetime,
        update_at=current_datetime,
    )
    new_summary.save()


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
        BatchInputParameterModeForSummarySurvey.USER,
        BatchInputParameterModeForSummarySurvey.SUPPORTER_ORGANIZATION,
        BatchInputParameterModeForSummarySurvey.ALL,
    ]:
        raise Exception(f"Invalid parameter: {mode}")

    batch_init_common_procedure(
        event=event,
        operation_start_datetime=operation_start_datetime,
        batch_control_id=batch_control_id,
        batch_control_name=BatchFunctionName.SUMMARY_SURVEY_BATCH,
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


def create_survey_summary_by_user(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        支援者（営業担当者）別集計作成
    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    # 前々月(yyyy/mm)
    two_months_ago_datetime: datetime = operation_start_datetime + relativedelta(
        months=-2
    )
    two_months_ago_year_month = two_months_ago_datetime.strftime("%Y/%m")
    # 前月(yyyy/mm)
    last_month_datetime: datetime = operation_start_datetime + relativedelta(months=-1)
    last_year_month = last_month_datetime.strftime("%Y/%m")
    # 当月(yyyy/mm)
    current_year_month = operation_start_datetime.strftime("%Y/%m")

    ###############################
    # アンケート情報の更新
    ###############################
    ###########################
    # 前々月データ
    ###########################
    logger.info(f"前々月アンケート評価対象者の更新開始:{two_months_ago_year_month}")
    # アンケート評価対象者の更新
    update_survey_evaluators(
        two_months_ago_year_month, operation_start_datetime, batch_control_id
    )
    logger.info("前々月アンケート評価対象者の更新完了")

    ###########################
    # 前月分データ
    ###########################
    logger.info(f"前月アンケート評価対象者の更新開始:{last_year_month}")
    # アンケート評価対象者の更新
    update_survey_evaluators(
        last_year_month, operation_start_datetime, batch_control_id
    )
    logger.info("前月アンケート評価対象者の更新完了")

    ###########################
    # 当月分データ
    ###########################
    logger.info(f"当月アンケート評価対象者の更新開始:{current_year_month}")
    # アンケート評価対象者の更新
    update_survey_evaluators(
        current_year_month, operation_start_datetime, batch_control_id
    )
    logger.info("当月アンケート評価対象者の更新完了")

    ###############################
    # 支援者（営業担当者）別集計作成
    ###############################
    ###########################
    # 前々月データ
    ###########################
    logger.info(f"前々月データ作成開始:{two_months_ago_year_month}")
    # 支援者（営業担当者）別集計の編集・登録
    save_user_summary_of_summary_month(two_months_ago_year_month)
    logger.info("前々月データ作成完了")

    ###########################
    # 前月分データ
    ###########################
    logger.info(f"前月データ作成開始:{last_year_month}")
    # 支援者（営業担当者）別集計の編集・登録
    save_user_summary_of_summary_month(last_year_month)
    logger.info("前月データ作成完了")

    ###########################
    # 当月分データ
    ###########################
    logger.info(f"当月データ作成開始:{current_year_month}")
    # 支援者（営業担当者）別集計の編集・登録
    save_user_summary_of_summary_month(current_year_month)
    logger.info("当月データ作成完了")


def create_survey_summary_by_supporter_organization(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        支援者組織（粗利メイン課）別集計作成
    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    ###########################
    # 前々月データ
    ###########################
    # 前々月(yyyy/mm)
    two_months_ago_datetime: datetime = operation_start_datetime + relativedelta(
        months=-2
    )
    two_months_ago_year_month = two_months_ago_datetime.strftime("%Y/%m")
    logger.info(f"前々月データ作成開始:{two_months_ago_year_month}")
    # 支援者組織（粗利メイン課）別集計の編集・登録
    save_supporter_organization_summary_of_summary_month(two_months_ago_year_month)
    logger.info("前々月データ作成完了")

    ###########################
    # 前月分データ
    ###########################
    # 前月(yyyy/mm)
    last_month_datetime: datetime = operation_start_datetime + relativedelta(months=-1)
    last_year_month = last_month_datetime.strftime("%Y/%m")
    logger.info(f"前月データ作成開始:{last_year_month}")
    # 支援者組織（粗利メイン課）別集計の編集・登録
    save_supporter_organization_summary_of_summary_month(last_year_month)
    logger.info("前月データ作成完了")

    ###########################
    # 当月分データ
    ###########################
    # 当月(yyyy/mm)
    current_year_month = operation_start_datetime.strftime("%Y/%m")
    logger.info(f"当月データ作成開始:{current_year_month}")
    # 支援者組織（粗利メイン課）別集計の編集・登録
    save_supporter_organization_summary_of_summary_month(current_year_month)
    logger.info("当月データ作成完了")


def create_survey_all_summary(
    event, operation_start_datetime: datetime, batch_control_id: str
):
    """
        全集計作成
    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    ###########################
    # 前々月データ
    ###########################
    # 前々月(yyyy/mm)
    two_months_ago_datetime: datetime = operation_start_datetime + relativedelta(
        months=-2
    )
    two_months_ago_year_month = two_months_ago_datetime.strftime("%Y/%m")
    logger.info(f"前々月データ作成開始:{two_months_ago_year_month}")
    # 全集計の編集・登録
    save_all_summary_of_summary_month(two_months_ago_year_month)
    logger.info("前々月データ作成完了")

    ###########################
    # 前月分データ
    ###########################
    # 前月(yyyy/mm)
    last_month_datetime: datetime = operation_start_datetime + relativedelta(months=-1)
    last_year_month = last_month_datetime.strftime("%Y/%m")
    logger.info(f"前月データ作成開始:{last_year_month}")
    # 全集計の編集・登録
    save_all_summary_of_summary_month(last_year_month)
    logger.info("前月データ作成完了")

    ###########################
    # 当月分データ
    ###########################
    # 当月(yyyy/mm)
    current_year_month = operation_start_datetime.strftime("%Y/%m")
    logger.info(f"当月データ作成開始:{current_year_month}")
    # 全集計の編集・登録
    save_all_summary_of_summary_month(current_year_month)
    logger.info("当月データ作成完了")


def main_procedure(event, operation_start_datetime: datetime, batch_control_id: str):
    """
        主処理
    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    if event["mode"] == BatchInputParameterModeForSummarySurvey.USER:
        # 支援者（営業担当者）別集計作成
        logger.info("create_survey_summary_by_user start.")
        create_survey_summary_by_user(event, operation_start_datetime, batch_control_id)
        logger.info("create_survey_summary_by_user end.")

    elif (
        event["mode"] == BatchInputParameterModeForSummarySurvey.SUPPORTER_ORGANIZATION
    ):
        # 支援者組織（粗利メイン課）別集計作成
        logger.info("create_survey_summary_by_supporter_organization start.")
        create_survey_summary_by_supporter_organization(
            event, operation_start_datetime, batch_control_id
        )
        logger.info("create_survey_summary_by_supporter_organization end.")

    elif event["mode"] == BatchInputParameterModeForSummarySurvey.ALL:
        # 全集計作成
        logger.info("create_survey_all_summary start.")
        create_survey_all_summary(event, operation_start_datetime, batch_control_id)
        logger.info("create_survey_all_summary end.")


def handler(event, context):
    """
    バッチ処理 : BOアンケート情報集計データ作成処理
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
        BatchFunctionId.SUMMARY_SURVEY_BATCH.format(
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
                    "error_function": BatchFunctionName.SUMMARY_SURVEY_BATCH,
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
