from datetime import datetime
from typing import List, Literal, Union

from fastapi import HTTPException, Query, status
from pydantic import EmailStr, Field, root_validator, validator

from app.resources.const import (
    ExportSurveysModeType,
    ProjectPhaseType,
    SurveyQuestionsSummaryType,
    SurveyType,
    SurveyTypeForGetSurveys,
    SurveyUserType,
)
from app.schemas.base import CustomBaseModel


class SupporterUser(CustomBaseModel):
    id: str = Field(
        ..., title="支援者ユーザーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="支援者名", example="アクセラレータ太郎")
    organization_id: str = Field(
        None, title="所属課ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    organization_name: str = Field(None, title="所属課名", example="AST")


class Answer(CustomBaseModel):
    id: str = Field(..., title="設問ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    answer: str = Field(..., title="回答（文字列表現）", example="とても満足")
    point: int = Field(None, title="回答点数（数値表現）", example=0)
    choice_ids: List[str] = Field(
        None, title="回答選択肢ID（複数）", example=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
    )
    summary_type: SurveyQuestionsSummaryType = Field(
        None,
        title="集計タイプ（ラジオボタンのみ）（normal: 通常の設問、point:点数形式、satisfaction:満足度、continuation:継続意思、recommended:紹介可能性、sales:営業評価、survey_satisfaction:カルテ機能満足度、man_hour_satisfaction:工数機能満足度、karte_satisfaction:カルテ機能満足度）、master_karte_satisfaction:マスターカルテ満足度",
        example="normal",
    )
    other_input: str = Field(None, title="回答任意入力", example="任意入力")


class GetSurveyByIdQuery(CustomBaseModel):
    survey_id: str = Field(
        ..., title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )


class GetSurveyByIdResponse(CustomBaseModel):
    id: str = Field(
        ..., title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    survey_master_id: str = Field(
        ..., title="アンケートマスターID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    survey_revision: int = Field(..., title="アンケートマスターバージョン番号", example=1)
    survey_type: SurveyType = Field(..., title="アンケート種別", example="pp")
    project_id: str = Field(
        None, title="案件ID（PPアンケート以外）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    project_name: str = Field(None, title="案件名（PPアンケート以外）", example="○○プロジェクト")
    customer_success: str = Field(None, title="カスタマーサクセス（PPアンケート以外）", example="DXの実現")
    supporter_organization_id: str = Field(
        None,
        title="案件担当課ID（粗利メイン課）（PPアンケート以外）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    supporter_organization_name: str = Field(
        None, title="案件担当課名（PPアンケート以外）", example="支援課"
    )
    support_date_from: str = Field(
        None, title="支援開始日（yyyy/mm/dd）", example="2022/06/01"
    )
    support_date_to: str = Field(None, title="支援終了日（yyyy/mm/dd）", example="2023/03/31")
    main_supporter_user: SupporterUser = Field(None, title="主担当支援者（PPアンケート以外）")
    supporter_users: List[SupporterUser] = Field(
        None, title="副担当支援者（PPアンケート以外）", description="IDで取得した場合のみ付与"
    )
    sales_user_id: str = Field(
        None, title="担当営業ID（PPアンケート以外）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    sales_user_name: str = Field(None, title="担当営業名（PPアンケート以外）", example="営業太郎")
    service_type_id: str = Field(
        None, title="サービス種別ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    service_type_name: str = Field(None, title="サービス種別名", example="組織開発")
    answer_user_id: str = Field(
        None, title="回答ユーザーID（顧客or支援者）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    answer_user_name: str = Field(None, title="回答ユーザー名（顧客or支援者）", example="田中太郎")
    customer_id: str = Field(
        None, title="取引先ID（顧客のみ）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    customer_name: str = Field(None, title="取引先名（顧客のみ）", example="○○株式会社")
    company: str = Field(None, title="所属会社（顧客以外）", example="ソニーグループ株式会社")
    answers: List[Answer] = Field(..., title="設問ごとの回答")
    summary_month: str = Field(..., title="集計月（yyyy/mm）", example="2022/01")
    is_not_summary: bool = Field(..., title="集計対象外にする", example="")
    is_solver_project: bool = Field(..., title="ソルバー担当の案件か", example="False")
    plan_survey_request_datetime: str = Field(
        None, title="回答（送信）依頼予定日時", example="2020-10-23T03:21:39.356872Z"
    )
    actual_survey_request_datetime: str = Field(
        None, title="回答（送信）依頼実績日時", example="2020-10-23T03:21:39.356872Z"
    )
    plan_survey_response_datetime: str = Field(
        None, title="回答（受信）予定日時", example="2020-10-23T03:21:39.356872Z"
    )
    actual_survey_response_datetime: str = Field(
        None, title="回答（受信）実績日時", example="2020-10-23T03:21:39.356872Z"
    )
    is_finished: bool = Field(..., title="回答済", example=True)
    is_disclosure: bool = Field(..., title="支援者へ開示OK", example=True)
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)


class GetSurveysQuery(CustomBaseModel):
    summary_month_from: str = Query(None, title="集計月（From）", example="202204")
    summary_month_to: str = Query(None, title="集計月（To）", example="202303")
    actual_survey_response_date_from: str = Query(
        None, title="回答（受信）実績日時（From）", example="20220401"
    )
    actual_survey_response_date_to: str = Query(
        None, title="回答（受信）実績日時（To）", example="20230331"
    )
    plan_survey_response_date_from: str = Query(
        None, title="回答（受信）予定日時（From）", example="20220401"
    )
    plan_survey_response_date_to: str = Query(
        None, title="回答（受信）予定日時（To）", example="20230331"
    )
    project_id: str = Query(
        None, title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    customer_id: str = Query(
        None, title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    service_type_id: str = Query(
        None, title="サービス種別ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    is_finished: bool = Query(None, title="回答済みのみ", example=False)
    type: SurveyTypeForGetSurveys = Query(
        None,
        title="アンケート種別（service:サービス、completion:修了、service_and_completion:サービス修了合算、quick:クイック、pp:PartnarPortal）",
        description="指定なしは全て",
        example="pp",
    )
    organization_ids: str = Query(
        None,
        title="支援者組織（課）ID",
        description="カンマ区切りで複数可",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d,89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )

    @root_validator
    def validate_narrow_down_the_period(cls, v):
        """集計月・回答受信日・受信予定日の同時指定チェック"""
        if (
            (v.get("summary_month_from") or v.get("summary_month_to"))
            and (
                v.get("actual_survey_response_date_from")
                or v.get("actual_survey_response_date_to")
            )
            and (
                v.get("plan_survey_response_date_from")
                or v.get("plan_survey_response_date_from")
            )
        ):
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="summary_month and actual_survey_response_date and plan_survey_response_date cannot be specified together",
            )
        return v

    @root_validator
    def check_summary_month_reversal_and_format(cls, v):
        """集計月のフォーマット・日付け逆転チェック"""
        try:
            summary_month_from = (
                datetime.strptime(v.get("summary_month_from"), "%Y%m")
                if v.get("summary_month_from")
                else None
            )
            summary_month_to = (
                datetime.strptime(v.get("summary_month_to"), "%Y%m")
                if v.get("summary_month_to")
                else None
            )
            if summary_month_from and summary_month_to:
                if summary_month_from > summary_month_to:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="summaryMonthFrom is greater than or equal to summaryMonthTol",
                    )
        except ValueError:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="summaryMonthFrom or summaryMonthTo be 6-digit numbers(yyyymm).",
            )
        return v

    @root_validator
    def check_plan_survey_response_date_reversal_and_format(cls, v):
        """回答受信予定日のフォーマット・日付け逆転チェック"""
        try:
            plan_survey_response_date_from = (
                datetime.strptime(v.get("plan_survey_response_date_from"), "%Y%m%d")
                if v.get("plan_survey_response_date_from")
                else None
            )
            plan_survey_response_date_to = (
                datetime.strptime(v.get("plan_survey_response_date_to"), "%Y%m%d")
                if v.get("plan_survey_response_date_to")
                else None
            )
            if plan_survey_response_date_from and plan_survey_response_date_to:
                if plan_survey_response_date_from > plan_survey_response_date_to:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="planSurveyResponseDateFrom is greater than or equal to planSurveyResponseDateTo.",
                    )
        except ValueError:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="planSurveyResponseDateFrom or planSurveyResponseDateTo be 8-digit numbers(yyyymmdd).",
            )
        return v

    @root_validator
    def check_actual_survey_response_date_reversal_and_format(cls, v):
        # HACK: 日付けのチェックが冗長なのでリファクタ余地あり
        """回答受信日のフォーマット・日付け逆転チェック"""
        try:
            actual_survey_response_date_from = (
                datetime.strptime(v.get("actual_survey_response_date_from"), "%Y%m%d")
                if v.get("actual_survey_response_date_from")
                else None
            )
            actual_survey_response_date_to = (
                datetime.strptime(v.get("actual_survey_response_date_to"), "%Y%m%d")
                if v.get("actual_survey_response_date_to")
                else None
            )
            if actual_survey_response_date_from and actual_survey_response_date_to:
                if actual_survey_response_date_from > actual_survey_response_date_to:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="actualSurveyResponseDateFrom is greater than or equal to actualSurveyResponseDateTo.",
                    )
        except ValueError:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="actualSurveyResponseDateFrom or actualSurveyResponseDateTo be 8-digit numbers(yyyymmdd).",
            )
        return v


class SurveyInfoForGetSurveysResponse(CustomBaseModel):
    id: str = Field(
        ..., title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    survey_master_id: str = Field(
        ..., title="アンケートマスターID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    survey_revision: int = Field(..., title="アンケートマスターバージョン番号", example=1)
    survey_type: SurveyType = Field(..., title="アンケート種別", example="pp")
    project_id: str = Field(
        None, title="案件ID（PPアンケート以外）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    project_name: str = Field(None, title="案件名（PPアンケート以外）", example="○○プロジェクト")
    customer_success: str = Field(None, title="カスタマーサクセス（PPアンケート以外）", example="DXの実現")
    supporter_organization_id: str = Field(
        None,
        title="案件担当課ID（粗利メイン課）（PPアンケート以外）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    supporter_organization_name: str = Field(
        None, title="案件担当課名（PPアンケート以外）", example="支援課"
    )
    support_date_from: str = Field(
        None, title="支援開始日（yyyy/mm/dd）", example="2022/06/01"
    )
    support_date_to: str = Field(None, title="支援終了日（yyyy/mm/dd）", example="2023/03/31")
    main_supporter_user: SupporterUser = Field(None, title="主担当支援者（PPアンケート以外）")
    supporter_users: List[SupporterUser] = Field(
        None, title="副担当支援者（PPアンケート以外）", description="IDで取得した場合のみ付与"
    )
    sales_user_id: str = Field(
        None, title="担当営業ID（PPアンケート以外）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    sales_user_name: str = Field(None, title="担当営業名（PPアンケート以外）", example="営業太郎")
    service_type_id: str = Field(
        None, title="サービス種別ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    service_type_name: str = Field(None, title="サービス種別名", example="組織開発")
    answer_user_id: str = Field(
        None, title="回答ユーザーID（顧客or支援者）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    answer_user_name: str = Field(None, title="回答ユーザー名（顧客or支援者）", example="田中太郎")
    customer_id: str = Field(
        None, title="取引先ID（顧客のみ）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    customer_name: str = Field(None, title="取引先名（顧客のみ）", example="○○株式会社")
    company: str = Field(None, title="所属会社（顧客以外）", example="ソニーグループ株式会社")
    summary_month: str = Field(..., title="集計月（yyyy/mm）", example="2022/01")
    is_not_summary: bool = Field(..., title="集計対象外にする", example="")
    plan_survey_request_datetime: str = Field(
        None, title="回答（送信）依頼予定日時", example="2020-10-23T03:21:39.356872Z"
    )
    actual_survey_request_datetime: str = Field(
        None, title="回答（送信）依頼実績日時", example="2020-10-23T03:21:39.356872Z"
    )
    plan_survey_response_datetime: str = Field(
        None, title="回答（受信）予定日時", example="2020-10-23T03:21:39.356872Z"
    )
    actual_survey_response_datetime: str = Field(
        None, title="回答（受信）実績日時", example="2020-10-23T03:21:39.356872Z"
    )
    is_finished: bool = Field(..., title="回答済", example=True)
    is_disclosure: bool = Field(..., title="支援者へ開示OK", example=True)


class monthlySurveySummaryForGetSurveysResponse(CustomBaseModel):
    month: str = Field(None, title="年月")
    satisfaction_average: float = Field(None, title="満足度平均")
    receive: int = Field(None, title="N数")


class surveySummaryForGetSurveysResponse(CustomBaseModel):
    satisfaction_average: float = Field(None, title="満足度平均")
    receive: int = Field(None, title="N数")


class surveySummaryQuotaTotalForGetSurveysResponse(CustomBaseModel):
    quota1: surveySummaryForGetSurveysResponse = Field(None, title="第１クォーター")
    quota2: surveySummaryForGetSurveysResponse = Field(None, title="第２クォーター")
    quota3: surveySummaryForGetSurveysResponse = Field(None, title="第３クォーター")
    quota4: surveySummaryForGetSurveysResponse = Field(None, title="第４クォーター")


class surveySummaryHalfTotalForGetSurveysResponse(CustomBaseModel):
    half1: surveySummaryForGetSurveysResponse = Field(None, title="上期")
    half2: surveySummaryForGetSurveysResponse = Field(None, title="下期")


class surveySummaryTotalForGetSurveysResponse(CustomBaseModel):
    quota: surveySummaryQuotaTotalForGetSurveysResponse = Field(None, title="四半期")
    half: surveySummaryHalfTotalForGetSurveysResponse = Field(None, title="半期")
    year: surveySummaryForGetSurveysResponse = Field(None, title="年")


class SummaryInfoForGetSurveysResponse(CustomBaseModel):
    monthly: List[monthlySurveySummaryForGetSurveysResponse] = Field(None, title="単月")
    accumulation: List[monthlySurveySummaryForGetSurveysResponse] = Field(
        None, title="累積"
    )
    total: surveySummaryTotalForGetSurveysResponse = Field(None, title="期間合計")


class GetSurveysResponse(CustomBaseModel):
    summary: SummaryInfoForGetSurveysResponse = Field(None, title="サマリ")
    surveys: List[SurveyInfoForGetSurveysResponse] = Field(..., title="アンケート一覧")


class UpdateSurveyByIdQuery(CustomBaseModel):
    version: int = Field(..., title="ロックキー（楽観ロック制御）", example=1)


class UpdateSurveyByIdRequest(CustomBaseModel):
    summary_month: str = Field(..., title="集計月", example="2022/03")
    is_not_summary: bool = Field(..., title="集計対象外にする", example=False)
    is_solver_project: bool = Field(..., title="ソルバー担当の案件か", example="False")

    @validator("summary_month")
    def check_summary_month_format(cls, v):
        try:
            datetime.strptime(v, "%Y/%m")
        except ValueError:
            raise ValueError("Incorrect data format, should be YYYY/MM.")
        return v


class GetSurveyPlansQuery(CustomBaseModel):
    summary_month_from: str = Query(None, title="集計月（From）", example="202204")
    summary_month_to: str = Query(None, title="集計月（To）", example="202303")
    plan_survey_response_date_from: str = Query(
        None, title="受信予定日（From）", example="20220401"
    )
    plan_survey_response_date_to: str = Query(
        None, title="受信予定日（To）", example="20230331"
    )

    @root_validator
    def validate_narrow_down_the_period(cls, v):
        """集計月・受信予定日の同時指定チェック"""
        specified_range_of_summary_month: bool = (
            True
            if (
                v.get("summary_month_from") is not None
                or v.get("summary_month_to") is not None
            )
            else False
        )
        specified_range_of_plan_survey_response_date: bool = (
            True
            if (
                v.get("plan_survey_response_date_from") is not None
                or v.get("plan_survey_response_date_to") is not None
            )
            else False
        )
        if [
            specified_range_of_summary_month,
            specified_range_of_plan_survey_response_date,
        ].count(True) > 1:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="SummaryMonth and planSurveyResponseDate cannot be specified together.",
            )
        return v

    @root_validator
    def check_summary_month_reversal_and_format(cls, v):
        """集計月のフォーマット・日付け逆転チェック"""
        try:
            summary_month_from = (
                datetime.strptime(v.get("summary_month_from"), "%Y%m")
                if v.get("summary_month_from")
                else None
            )
            summary_month_to = (
                datetime.strptime(v.get("summary_month_to"), "%Y%m")
                if v.get("summary_month_to")
                else None
            )
            if summary_month_from and summary_month_to:
                if summary_month_from > summary_month_to:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="SummaryMonthFrom is greater than or equal to summaryMonthTo",
                    )
        except ValueError:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="SummaryMonthFrom or summaryMonthTo be 6-digit numbers(yyyymm).",
            )
        return v

    @root_validator
    def check_plan_survey_response_date_reversal_and_format(cls, v):
        """受信予定日のフォーマット・日付け逆転チェック"""
        try:
            plan_survey_response_date_from = (
                datetime.strptime(v.get("plan_survey_response_date_from"), "%Y%m%d")
                if v.get("plan_survey_response_date_from")
                else None
            )
            plan_survey_response_date_to = (
                datetime.strptime(v.get("plan_survey_response_date_to"), "%Y%m%d")
                if v.get("plan_survey_response_date_to")
                else None
            )
            if plan_survey_response_date_from and plan_survey_response_date_to:
                if plan_survey_response_date_from > plan_survey_response_date_to:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="PlanSurveyResponseDateFrom is greater than or equal to planSurveyResponseDateTo.",
                    )
        except ValueError:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="PlanSurveyResponseDateFrom or planSurveyResponseDateTo be 8-digit numbers(yyyymmdd).",
            )
        return v


class SurveyInfoForGetSurveyPlansResponse(CustomBaseModel):
    project_id: str = Field(
        ..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    project_name: str = Field(..., title="案件名", example="サンプルプロジェクト")
    id: str = Field(
        None, title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    survey_type: SurveyType = Field(None, title="アンケート種別", example="service")
    customer_success: str = Field(None, title="カスタマーサクセス（PPアンケート以外）", example="DXの実現")
    supporter_organization_id: str = Field(
        None,
        title="案件担当課ID（粗利メイン課）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    supporter_organization_name: str = Field(None, title="案件担当課名", example="IST")
    support_date_from: str = Field(..., title="支援開始日（yyyy/mm/dd）", example="2022/06/01")
    support_date_to: str = Field(..., title="支援終了日（yyyy/mm/dd）", example="2023/03/31")
    main_supporter_user: SupporterUser = Field(None, title="主担当支援者（PPアンケート以外）")
    supporter_users: List[SupporterUser] = Field(
        None, title="副担当支援者（PPアンケート以外）", description="IDで取得した場合のみ付与"
    )
    sales_user_id: str = Field(
        ..., title="担当営業ID（PPアンケート以外）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    sales_user_name: str = Field(..., title="担当営業名（PPアンケート以外）", example="営業太郎")
    service_type_id: str = Field(
        None, title="サービス種別ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    service_type_name: str = Field(None, title="サービス種別名", example="組織開発")
    survey_user_type: SurveyUserType = Field(
        None, title="送付先種類（登録済のお客様代表、Salesforce取引先責任者、手動設定）", example="登録済のお客様代表"
    )
    survey_user_name: str = Field(None, title="送付先氏名", example="ソニー太郎")
    survey_user_email: Union[EmailStr, Literal[""]] = Field(
        None, title="送信先メールアドレス", example="user@sample.com"
    )
    customer_id: str = Field(
        ..., title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    customer_name: str = Field(None, title="取引先名", example="○○株式会社")
    summary_month: str = Field(None, title="集計月（yyyy/mm）", example="2022/01")
    plan_survey_request_datetime: str = Field(
        None, title="回答（送信）依頼予定日時", example="2020-10-23T03:21:39.356872Z"
    )
    actual_survey_request_datetime: str = Field(
        None, title="回答（送信）依頼実績日時", example="2020-10-23T03:21:39.356872Z"
    )
    plan_survey_response_datetime: str = Field(
        None, title="回答（受信）予定日時", example="2020-10-23T03:21:39.356872Z"
    )
    actual_survey_response_datetime: str = Field(
        None, title="回答（受信）実績日時", example="2020-10-23T03:21:39.356872Z"
    )
    phase: ProjectPhaseType = Field(
        None,
        title="フェーズ(プラン提示(D)、見積もり提示(C)、内諾(B)、内部決済完了(A)、失注(G)、Open、活動中(E)、Drop(F))",
        example="プラン提示(D)",
    )
    is_count_man_hour: bool = Field(..., title="工数調査するかしないか", example=True)
    unanswered_surveys_number: int = Field(None, title="連続未回答数", example=0)
    this_month_type: str = Field(None, title="今月の種類", example="サービスアンケート")
    no_send_reason: str = Field(None, title="送付しない理由", example="お客様要望")


class GetSurveyPlansResponse(CustomBaseModel):
    surveys: List[SurveyInfoForGetSurveyPlansResponse] = Field(..., title="アンケート予実績一覧")


class GetSurveySummaryReportsQuery(CustomBaseModel):
    summary_month: str = Field(..., title="集計月", example="202201")

    @validator("summary_month")
    def check_summary_month_format(cls, v):
        """集計月のフォーマットチェック"""
        try:
            datetime.strptime(v, "%Y%m")
        except ValueError:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="summaryMonth be 6-digit numbers(yyyymm).",
            )
        return v


class SurveySummaryReports(CustomBaseModel):
    plan: int = Field(0, title="単月 計画（件）")
    collect: int = Field(0, title="単月 回収（件）")
    percent: int = Field(0, title="単月 回収率（%）")
    summary_plan: int = Field(0, title="年度累計 計画（件）")
    summary_collect: int = Field(0, title="年度累計 回収（件）")
    summary_percent: int = Field(0, title="年度累計 回収率（%）")


class GetSurveySummaryReportsResponse(CustomBaseModel):
    summary_month: str = Field(..., title="集計月", example="2022/03")
    service: SurveySummaryReports = Field(..., title="サービスアンケート")
    completion: SurveySummaryReports = Field(..., title="修了アンケート")
    service_and_completion: SurveySummaryReports = Field(
        ..., title="サービスアンケート・修了アンケート合算"
    )
    pp: SurveySummaryReports = Field(..., title="Partner Portalアンケート")


class GetSurveySummarySupporterOrganizationsQuery(CustomBaseModel):
    year_month_from: str = Query(..., title="集計月（From）", example="202204")
    year_month_to: str = Query(..., title="集計月（To）", example="202303")

    @root_validator
    def validate_year_month_from_to(cls, values):
        if values.get("year_month_from") and values.get("year_month_to"):
            try:
                year_month_from = datetime.strptime(
                    values.get("year_month_from"), "%Y%m"
                )
                year_month_to = datetime.strptime(values.get("year_month_to"), "%Y%m")
                if year_month_from > year_month_to:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="yearMonthForm is greater than or equal to yearMonthTo.",
                    )
            except ValueError:
                raise HTTPException(
                    status_code=status.HTTP_400_BAD_REQUEST,
                    detail="yearMonthFrom or yearMonthTo be 6digit numbers(yyyymm).",
                )

        return values


class CompletionContinuation(CustomBaseModel):
    positive_count: int = Field(0, title="継続意思あり数")
    negative_count: int = Field(0, title="継続意思なし数")
    positive_percent: int = Field(0, title="継続意思（％）")

    class Config:
        orm_mode = True


class totalSummaryResultForSupporterOrganizations(CustomBaseModel):
    service_satisfaction_average: float = Field(0, title="サービスアンケート総合満足度平均")
    service_receive: int = Field(0, title="サービスアンケートN数")
    completion_satisfaction_average: float = Field(0, title="修了アンケート総合満足度平均")
    completion_continuation_percent: int = Field(0, title="修了アンケート継続意思（％）")
    completion_recommended_average: float = Field(0, title="修了アンケート紹介可能性平均")
    completion_receive: int = Field(0, title="修了アンケートN数")
    total_satisfaction_average: float = Field(0, title="総合満足度平均")
    total_receive: int = Field(0, title="総合満足度N数")


class TermSummaryResultForSupporterOrganizations(CustomBaseModel):
    supporter_organization_id: str = Field(
        ..., title="支援者組織ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    supporter_organization_name: str = Field(..., title="支援者組織名")
    service_satisfaction_summary: int = Field(0, title="サービスアンケート総合満足度合計")
    service_satisfaction_average: int = Field(0, title="サービスアンケート総合満足度平均")
    service_satisfaction_unanswered: int = Field(0, title="サービスアンケート総合満足度未回答数")
    service_receive: int = Field(0, title="サービスアンケートN数")
    completion_satisfaction_summary: int = Field(0, title="修了アンケート総合満足度合計")
    completion_satisfaction_average: int = Field(0, title="修了アンケート総合満足度平均")
    completion_satisfaction_unanswered: int = Field(0, title="修了アンケート総合満足度未回答数")
    completion_continuation: CompletionContinuation = Field(..., title="修了アンケート継続意思")
    completion_continuation_unanswered: int = Field(0, title="修了アンケート継続意思未回答数")
    completion_recommended_summary: int = Field(0, title="修了アンケート紹介可能性合計")
    completion_recommended_average: int = Field(0, title="修了アンケート紹介可能性平均")
    completion_recommended_unanswered: int = Field(0, title="修了アンケート紹介可能性未回答数")
    completion_receive: int = Field(0, title="修了アンケートN数")
    total_satisfaction_summary: int = Field(0, title="総合満足度合計")
    total_satisfaction_average: int = Field(0, title="総合満足度平均")
    total_satisfaction_unanswered: int = Field(0, title="総合満足度未回答数")
    total_receive: int = Field(0, title="総合満足度N数")


class MonthlyResultsBySupporterOrganization(CustomBaseModel):
    supporter_organization_name: str = Field(..., title="支援者組織名")
    service_satisfaction_average: float = Field(0, title="サービスアンケート総合満足度平均")
    service_receive: int = Field(0, title="サービスアンケートN数")
    completion_satisfaction_average: float = Field(0, title="修了アンケート総合満足度平均")
    completion_continuation_percent: int = Field(0, title="修了アンケート継続意思（％）")
    completion_recommended_average: float = Field(0, title="修了アンケート紹介可能性平均")
    completion_receive: int = Field(0, title="修了アンケートN数")
    total_satisfaction_average: float = Field(0, title="総合満足度平均")
    total_receive: int = Field(0, title="総合満足度N数")


class SurveysForMonthlyResultsBySupporterOrganization(CustomBaseModel):
    summary_month: str = Field(..., title="集計月")
    supporter_organizations: List[MonthlyResultsBySupporterOrganization] = Field(
        ..., title="支援者組織別集計"
    )


class GetSurveySummarySupporterOrganizationsResponse(CustomBaseModel):
    total_summary_result: totalSummaryResultForSupporterOrganizations = Field(
        ..., title="全ての課の集計結果"
    )
    term_summary_result: List[TermSummaryResultForSupporterOrganizations] = Field(
        ..., title="期間集計結果"
    )
    surveys: List[SurveysForMonthlyResultsBySupporterOrganization] = Field(
        ..., title="課別の集計月別結果"
    )


class GetSurveySummaryAllQuery(CustomBaseModel):
    year_month_from: str = Query(..., title="集計月（From）", example="202204")
    year_month_to: str = Query(..., title="集計月（To）", example="202303")

    @root_validator
    def validate_year_month_from_to(cls, values):
        if values.get("year_month_from") and values.get("year_month_to"):
            try:
                year_month_from = datetime.strptime(
                    values.get("year_month_from"), "%Y%m"
                )
                year_month_to = datetime.strptime(values.get("year_month_to"), "%Y%m")
                if year_month_from > year_month_to:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="yearMonthForm is greater than or equal to yearMonthTo.",
                    )
            except ValueError:
                raise HTTPException(
                    status_code=status.HTTP_400_BAD_REQUEST,
                    detail="yearMonthFrom or yearMonthTo be 6digit numbers(yyyymm).",
                )

        return values


class SummaryResult(CustomBaseModel):
    satisfaction_average: float = Field(0, title="総合満足度平均", example=0)
    total_receive: int = Field(0, title="N数", example=0)


class CompletionSummaryResult(CustomBaseModel):
    satisfaction_average: float = Field(0, title="総合満足度平均", example=0)
    continuation_positive_percent: int = Field(0, title="継続意思（％）", example=0)
    recommended_average: float = Field(0, title="紹介可能性平均", example=0)
    sales_average: float = Field(0, title="営業評価平均", example=0)
    total_receive: int = Field(0, title="N数", example=0)


class QuickSummaryResult(CustomBaseModel):
    total_receive: int = Field(0, title="N数", example=0)


class PPSummaryResult(CustomBaseModel):
    survey_satisfaction_average: float = Field(0, title="アンケート機能満足度平均", example=0)
    man_hour_satisfaction_average: float = Field(0, title="工数入力機能満足度平均", example=0)
    karte_satisfaction_average: float = Field(0, title="カルテ機能満足度平均", example=0)
    master_karte_satisfaction_average: float = Field(0, title="マスターカルテ機能満足度平均", example=0)
    total_receive: int = Field(0, title="N数", example=0)


class TermSummaryResultForSummaryAll(CustomBaseModel):
    service: SummaryResult = Field(..., title="サービスアンケート期間集計")
    completion: CompletionSummaryResult = Field(..., title="修了アンケート期間集計")
    service_and_completion: SummaryResult = Field(..., title="サービスアンケート／終了アンケート合算期間集計")
    quick: QuickSummaryResult = Field(..., title="クイックアンケート期間集計")
    pp: PPSummaryResult = Field(..., title="Partner Portal利用アンケート期間集計")


class ServiceSummaryMonthlyResult(CustomBaseModel):
    project_count: int = Field(0, title="当月支援案件数", example=0)
    receive_count: int = Field(0, title="当月回収数（N数）", example=0)
    receive_percent: int = Field(0, title="回収率", example=0)
    satisfaction_summary: int = Field(0, title="総合満足度合計", example=0)
    satisfaction_average: float = Field(0, title="総合満足度平均", example=0)

    class Config:
        orm_mode = True


class CompletionSummaryMonthlyResult(CustomBaseModel):
    project_count: int = Field(0, title="当月支援案件数", example=0)
    receive_count: int = Field(0, title="当月回収数（N数）", example=0)
    receive_percent: int = Field(0, title="回収率", example=0)
    satisfaction_summary: int = Field(0, title="総合満足度合計", example=0)
    satisfaction_average: float = Field(0, title="総合満足度平均", example=0)
    continuation: CompletionContinuation = Field(..., title="継続意思")
    recommended_summary: int = Field(0, title="紹介可能性合計", example=0)
    recommended_average: float = Field(0, title="紹介可能性平均", example=0)
    sales_summary: int = Field(0, title="営業評価合計", example=0)
    sales_average: float = Field(0, title="営業評価平均", example=0)

    class Config:
        orm_mode = True


class KarteSummaryMonthlyResult(CustomBaseModel):
    project_count: int = Field(0, title="当月支援案件数", example=0)
    karte_count: int = Field(0, title="当月起票案件数", example=0)
    use_percent: int = Field(0, title="当月カルテ利用率", example=0)

    class Config:
        orm_mode = True


class QuickSummaryMonthlyResult(CustomBaseModel):
    receive_count: int = Field(0, title="当月回収数（N数）", example=0)
    receive_percent: int = Field(0, title="回収率", example=0)

    class Config:
        orm_mode = True


class PPSummaryMonthlyResult(CustomBaseModel):
    survey_satisfaction_summary: int = Field(0, title="アンケート機能満足度合計", example=0)
    survey_satisfaction_average: float = Field(0, title="アンケート機能満足度平均", example=0)
    man_hour_satisfaction_summary: int = Field(0, title="工数入力機能満足度合計", example=0)
    man_hour_satisfaction_average: float = Field(0, title="工数入力機能満足度平均", example=0)
    karte_satisfaction_summary: int = Field(0, title="カルテ機能満足度合計", example=0)
    karte_satisfaction_average: float = Field(0, title="カルテ機能満足度平均", example=0)
    master_karte_satisfaction_summary: int = Field(0, title="マスターカルテ機能満足度合計", example=0)
    master_karte_satisfaction_average: float = Field(0, title="マスターカルテ機能満足度平均", example=0)
    send_count: int = Field(0, title="当月送信数", example=0)
    receive_count: int = Field(0, title="当月回収数（N数）", example=0)
    receive_percent: int = Field(0, title="回収率", example=0)

    class Config:
        orm_mode = True


class MonthlySurveySummaryAllResults(CustomBaseModel):
    summary_month: str = Field(..., title="集計月")
    service: ServiceSummaryMonthlyResult = Field(..., title="サービスアンケート月別集計結果")
    completion: CompletionSummaryMonthlyResult = Field(..., title="修了アンケート月別集計結果")
    karte: KarteSummaryMonthlyResult = Field(..., title="カルテ月別集計結果")
    quick: QuickSummaryMonthlyResult = Field(..., title="クイックアンケート月別集計結果")
    pp: PPSummaryMonthlyResult = Field(..., title="Parthner Portal利用アンケート月別集計結果")

    class Config:
        orm_mode = True


class GetSurveySummaryAllResponse(CustomBaseModel):
    term_summary_result: TermSummaryResultForSummaryAll = Field(..., title="期間集計結果")
    surveys: List[MonthlySurveySummaryAllResults] = Field(..., title="課別の集計月別結果")


class ExportSurveysQuery(CustomBaseModel):
    summary_month_from: str = Field(None, title="集計月（From）", example="202204")
    summary_month_to: str = Field(None, title="集計月（To）", example="202303")
    plan_survey_request_date_from: str = Field(
        None, title="送信予定日（From）", example="20220401"
    )
    plan_survey_request_date_to: str = Field(
        None, title="送信予定日（To）", example="20230331"
    )
    type: SurveyTypeForGetSurveys = Field(
        None,
        title="アンケート種別\n（service:サービス、completion:修了、service_and_completion:サービス修了合算、quick:クイック、pp:PartnarPortal）\n指定なしは全て",
    )
    project_id: str = Field(
        None, title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    customer_id: str = Field(
        None, title="取引先ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    service_type_id: str = Field(
        None, title="サービス種別ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )

    organization_ids: str = Field(None, title="支援者組織（課）IDカンマ区切りで複数可")
    mode: ExportSurveysModeType = Field(
        ...,
        title="出力モード\nアンケートデータCSV（raw）\n支援者別アンケート集計CSV（supporter）\n課別アンケート集計CSV（organization）",
        example="",
    )

    @root_validator
    def require_narrow_down_the_period(cls, v):
        """集計月・送信予定日の必須チェック"""
        if (not v.get("summary_month_from") and not v.get("summary_month_to")) and (
            not v.get("plan_survey_request_date_from")
            and not v.get("plan_survey_request_date_to")
        ):
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="One of the parameters for the summary month is required",
            )
        return v

    @root_validator
    def validate_narrow_down_the_period(cls, v):
        """集計月・送信予定日の同時指定チェック"""
        if (v.get("summary_month_from") or v.get("summary_month_to")) and (
            (
                v.get("plan_survey_request_date_from")
                or v.get("plan_survey_request_date_to")
            )
        ):
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="summary_month and plan_survey_request_date cannot be specified together",
            )
        return v

    @root_validator
    def check_summary_month_reversal_and_format(cls, v):
        """集計月のフォーマット・日付け逆転・1年超過チェック"""
        try:
            summary_month_from = (
                datetime.strptime(v.get("summary_month_from"), "%Y%m")
                if v.get("summary_month_from")
                else None
            )
            summary_month_to = (
                datetime.strptime(v.get("summary_month_to"), "%Y%m")
                if v.get("summary_month_to")
                else None
            )
            if summary_month_from and summary_month_to:
                if summary_month_from > summary_month_to:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="summaryMonthFrom is greater than or equal to summaryMonthTol",
                    )
            if summary_month_from and summary_month_to:
                td = summary_month_to - summary_month_from
                if td.days > 365:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="Please specify summaryMonthFrom and summaryMonthTo within 1 year",
                    )
        except ValueError:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="summaryMonthFrom or summaryMonthTo be 6-digit numbers(yyyymm).",
            )
        return v

    @root_validator
    def check_plan_survey_request_date_reversal_and_format(cls, v):
        """送信予定日のフォーマット・日付け逆転・1年超過チェック"""
        try:
            plan_survey_request_date_from = (
                datetime.strptime(v.get("plan_survey_request_date_from"), "%Y%m")
                if v.get("plan_survey_request_date_from")
                else None
            )
            plan_survey_request_date_to = (
                datetime.strptime(v.get("plan_survey_request_date_to"), "%Y%m")
                if v.get("plan_survey_request_date_to")
                else None
            )
            if plan_survey_request_date_from and plan_survey_request_date_to:
                if plan_survey_request_date_from > plan_survey_request_date_to:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="planSurveyRequestDateFrom is greater than or equal to planSurveyRequestDateTo",
                    )
            if plan_survey_request_date_from and plan_survey_request_date_to:
                td = plan_survey_request_date_to - plan_survey_request_date_from
                if td.days > 365:
                    raise HTTPException(
                        status_code=status.HTTP_400_BAD_REQUEST,
                        detail="Please specify planSurveyRequestDateFrom and planSurveyRequestDateTo within 1 year",
                    )
        except ValueError:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="planSurveyRequestDateFrom or planSurveyRequestDateTo be 6-digit numbers(yyyymm).",
            )
        return v

    @root_validator
    def check_other_param_for_summary(cls, v):
        """支援者別集計と課別集計のクエリパラメータチェック"""
        if (
            v.get("mode") == ExportSurveysModeType.SUPPORTER.value
            or v.get("mode") == ExportSurveysModeType.ORGANIZATION.value
        ) and (
            v.get("project_id")
            or v.get("customer_id")
            or v.get("type")
            or v.get("service_type_id")
            or v.get("organization_ids")
        ):
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="For summary, parameters other than the aggregation month cannot be specified",
            )
        return v


class ExportSurveysResponse(CustomBaseModel):
    url: str = Field(...)
