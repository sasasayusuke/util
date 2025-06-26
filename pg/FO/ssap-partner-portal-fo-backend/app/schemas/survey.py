from datetime import datetime
from typing import List

from fastapi import HTTPException, Query
from fastapi import status as api_status
from pydantic import Field, root_validator

from app.resources.const import (
    GetSurveysByMineSortType,
    SurveyQuestionsSummaryType,
    SurveyType,
    SurveyTypeForGetSurveys,
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
        title="集計タイプ（ラジオボタンのみ）（normal: 通常の設問、point:点数形式、satisfaction:満足度、continuation:継続意思、recommended:紹介可能性、sales:営業評価、survey_satisfaction:カルテ機能満足度、man_hour_satisfaction:工数機能満足度、karte_satisfaction:カルテ機能満足度、master_karte_satisfaction:マスターカルテ機能満足度）",
        example="normal",
    )
    other_input: str = Field(None, title="回答任意入力", example="任意入力")


class GetSurveysByMineQuery(CustomBaseModel):
    type: SurveyType = Query(
        None,
        title="アンケート種別（service:サービス、completion:修了、service_and_completion:サービス修了合算、quick:クイック、pp:PartnerPortal）指定なしは全て)",
        example="pp",
    )
    date_from: int = Query(None, title=" 回答（送信）依頼実績日時（From）", example=202204)
    date_to: int = Query(None, title=" 回答（送信）依頼実績日時（To）", example=202209)
    sort: GetSurveysByMineSortType = Query(
        ..., title="ソート", example="actual_survey_request_datetime"
    )
    project_id: str = Query(
        None, title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    limit: int = Query(None, title="取得件数", example=5)


class GetSurveysByMine(CustomBaseModel):
    id: str = Field(..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    survey_type: SurveyType = Field(..., title="アンケート種別", example="pp")
    project_name: str = Field(None, title="案件名", example="○○プロジェクト")
    summary_month: str = Field(..., title="集計月（yyyy/mm）", example="2022/01")
    actual_survey_request_datetime: str = Field(
        None, title="回答（送信）依頼実績日時（送信実績）", example="2022/09/10"
    )
    plan_survey_response_datetime: str = Field(
        None, title="回答（受信）予定日時", example="2022/09/15"
    )
    actual_survey_response_datetime: str = Field(
        None, title="回答（受信）実績日時", example="2022/09/12"
    )
    answer_user_name: str = Field(..., title="回答ユーザー名", example="田中太郎")
    company: str = Field(None, title="所属会社", example="株式会社○○")
    customer_name: str = Field(None, title="取引先名", example="株式会社○○")
    is_finished: bool = Field(..., title="回答済", example=True)


class GetSurveysByMineResponse(CustomBaseModel):
    total: int = Field(..., title="合計件数", example=5)
    surveys: List[GetSurveysByMine] = Field(..., title="自身のアンケート情報")


class GetSurveysQuery(CustomBaseModel):
    summary_month: str = Query(None, title="集計月", example="202201")
    summary_month_from: str = Query(None, title="集計月（From）", example="202204")
    summary_month_to: str = Query(None, title="集計月（To）", example="202303")
    plan_survey_response_date_from: str = Query(
        None, title="回答予定日（From）", example="20220401"
    )
    plan_survey_response_date_to: str = Query(
        None, title="回答予定日（To）", example="20230331"
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
        title="アンケート種別（service:サービス、completion:修了、service_and_completion:サービス修了合算、quick:クイック、pp:PartnerPortal、non_pp:PartnerPortal以外）",
        description="指定なしは全て",
        example="pp",
    )
    is_disclosure: bool = Query(
        False, title="非公開アンケートを取得するか（Trueの場合は取得する）", example=False
    )
    organization_ids: str = Query(
        None,
        title="支援者組織（課）ID",
        description="カンマ区切りで複数可",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d,89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )


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
    answers: List[Answer] = Field(None, title="設問ごとの回答")
    summary_month: str = Field(..., title="集計月（yyyy/mm）", example="2022/01")
    is_not_summary: bool = Field(..., title="集計対象外にする", example="")
    is_solver_project: bool = Field(..., title="ソルバー担当の案件か", example=False)
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


class UpdateSurveyByIdQuery(CustomBaseModel):
    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class UpdateSurveyByIdRequest(CustomBaseModel):
    answers: List[Answer] = Field(..., title="設問ごとの回答")
    is_finished: bool = Field(..., title="回答済", example=True)
    is_disclosure: bool = Field(..., title="支援者へ開示OK", example=True)


class UpdateSurveyByIdResponse(CustomBaseModel):

    message: str = Field(..., title="メッセージ", example="OK")


class Service(CustomBaseModel):
    satisfaction_average: float = Field(..., title="総合満足度平均")
    total_receive: int = Field(..., title="N数")


class Completion(CustomBaseModel):
    satisfaction_average: float = Field(..., title="総合満足度平均")
    continuation_positive_percent: int = Field(..., title="継続意思（%）")
    recommended_average: float = Field(..., title="紹介可能性平均")
    sales_average: float = Field(..., title="営業評価平均")
    total_receive: int = Field(..., title="N数")


class ServiceAndCompletion(CustomBaseModel):
    satisfaction_average: float = Field(..., title="総合満足度平均")
    total_receive: int = Field(..., title="N数")


class Quick(CustomBaseModel):
    total_receive: int = Field(..., title="N数")


class TermSummaryResult(CustomBaseModel):
    service: Service = Field(..., title="サービスアンケート集計")
    completion: Completion = Field(..., title="修了アンケート")
    service_and_completion: ServiceAndCompletion = Field(..., title="サービスアンケート／修了アンケート")
    quick: Quick = Field(..., title="クイック")


class CompletionContinuation(CustomBaseModel):
    positive_count: int = Field(0, title="継続意思あり数")
    negative_count: int = Field(0, title="継続意思なし数")
    positive_percent: int = Field(0, title="継続意思（％）")


class GetSurveySummaryByMineQuery(CustomBaseModel):
    year_month_from: str = Query(None, title="集計月（From）", example="202204")
    year_month_to: str = Query(None, title="集計月（To）", example="202303")

    @root_validator
    def validate_year_month(cls, v):
        # yearMonthFrom,yearMonthTo
        try:
            from_date = (
                datetime.strptime(str(v.get("year_month_from")), "%Y%m")
                if v.get("year_month_from")
                else None
            )
            to_date = (
                datetime.strptime(str(v.get("year_month_to")), "%Y%m")
                if v.get("year_month_to")
                else None
            )
        except ValueError:
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail="yearMonthFrom and yearMonthTo should be 6-digit numbers(yyyymm).",
            )
        if from_date and to_date:
            if from_date > to_date:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="yearMonthTo is greater than or equal to yearMonthFrom.",
                )

        return v


class Surveys(CustomBaseModel):
    summary_month: str = Field(..., title="集計月")
    service_satisfaction_summary: int = Field(..., title="サービスアンケート総合満足度合計")
    service_satisfaction_average: float = Field(..., title="サービスアンケート総合満足度平均")
    service_receive: int = Field(..., title="サービスアンケートＮ数")
    completion_satisfaction_summary: int = Field(..., title="修了アンケート総合満足度合計")
    completion_satisfaction_average: float = Field(..., title="修了アンケート総合満足度平均")
    completion_continuation: CompletionContinuation = Field(..., title="修了アンケート継続意思")
    completion_recommended_summary: int = Field(..., title="修了アンケート紹介可能性合計")
    completion_recommended_average: float = Field(..., title="修了アンケート紹介可能性平均")
    completion_sales_summary: float = Field(..., title="営業評価合計")
    completion_sales_average: float = Field(..., title="営業評価平均")
    completion_receive: int = Field(..., title="修了アンケートＮ数")
    quick_receive: int = Field(..., title="クイックアンケートＮ数")


class GetSurveySummaryByMineResponse(CustomBaseModel):
    term_summary_result: TermSummaryResult = Field(..., title="期間集計結果")
    surveys: List[Surveys] = Field(..., title="月間アンケート集計結果")


class GetSurveySummarySupporterOrganizationByMineQuery(CustomBaseModel):
    year_month_from: str = Query(..., title="集計月（From）", example="202204")
    year_month_to: str = Query(..., title="集計月（To）", example="202303")

    @root_validator
    def validate_year_month(cls, v):
        # yearMonthFrom,yearMonthTo
        try:
            from_date = (
                datetime.strptime(str(v.get("year_month_from")), "%Y%m")
                if v.get("year_month_from")
                else None
            )
            to_date = (
                datetime.strptime(str(v.get("year_month_to")), "%Y%m")
                if v.get("year_month_to")
                else None
            )
        except ValueError:
            raise HTTPException(
                status_code=api_status.HTTP_400_BAD_REQUEST,
                detail="yearMonthFrom and yearMonthTo should be 6-digit numbers(yyyymm).",
            )
        if from_date and to_date:
            if from_date > to_date:
                raise HTTPException(
                    status_code=api_status.HTTP_400_BAD_REQUEST,
                    detail="yearMonthTo is greater than or equal to yearMonthFrom.",
                )

        return v


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
    service_satisfaction_average: float = Field(0, title="サービスアンケート総合満足度平均")
    service_satisfaction_unanswered: int = Field(0, title="サービスアンケート総合満足度未回答数")
    service_receive: int = Field(0, title="サービスアンケートN数")
    completion_satisfaction_summary: int = Field(0, title="修了アンケート総合満足度合計")
    completion_satisfaction_average: float = Field(0, title="修了アンケート総合満足度平均")
    completion_satisfaction_unanswered: int = Field(0, title="修了アンケート総合満足度未回答数")
    completion_continuation: CompletionContinuation = Field(..., title="修了アンケート継続意思")
    completion_continuation_unanswered: int = Field(0, title="修了アンケート継続意思未回答数")
    completion_recommended_summary: int = Field(0, title="修了アンケート紹介可能性合計")
    completion_recommended_average: float = Field(0, title="修了アンケート紹介可能性平均")
    completion_recommended_unanswered: int = Field(0, title="修了アンケート紹介可能性未回答数")
    completion_receive: int = Field(0, title="修了アンケートN数")
    total_satisfaction_summary: int = Field(0, title="総合満足度合計")
    total_satisfaction_average: float = Field(0, title="総合満足度平均")
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


class GetSurveySummarySupporterOrganizationByMineResponse(CustomBaseModel):
    total_summary_result: totalSummaryResultForSupporterOrganizations = Field(
        ..., title="全ての課の集計結果"
    )
    term_summary_result: List[TermSummaryResultForSupporterOrganizations] = Field(
        ..., title="期間集計結果"
    )
    surveys: List[SurveysForMonthlyResultsBySupporterOrganization] = Field(
        ..., title="課別の集計月別結果"
    )


class PostCheckSurveyByIdPasswordRequest(CustomBaseModel):
    token: str = Field(
        ...,
        title="JWT(アンケートIDと有効期限から生成された)",
        example="eyJhbGciOi12JIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0X2lkIjoiMTIzNDU2Nzg5MCIsImV4cGlyZWQiOiIyMDIzLzA0LzAxIn0.OVFrpwjo_HhSyN4aI5y4jVAw06EkyCPi-sv0vEeMJ",
    )
    password: str = Field(
        ...,
        title="案件IDと有効期限から生成されたJWT",
        example="ABcd12*345Ef",
    )


class PostCheckSurveyByIdPasswordResponse(CustomBaseModel):
    id: str = Field(
        ...,
        title="案件アンケートID",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )


class PostCheckSurveyByIdRequest(CustomBaseModel):
    token: str = Field(
        ...,
        title="JWT(アンケートIDと有効期限から生成された)",
        example="eyJhbGciOi12JIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0X2lkIjoiMTIzNDU2Nzg5MCIsImV4cGlyZWQiOiIyMDIzLzA0LzAxIn0.OVFrpwjo_HhSyN4aI5y4jVAw06EkyCPi-sv0vEeMJ",
    )


class PostCheckSurveyByIdResponse(CustomBaseModel):
    id: str = Field(
        ...,
        title="案件アンケートID",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )


class CheckAndGetSurveyAnonymousByIdRequest(CustomBaseModel):
    token: str = Field(
        ...,
        title="JWT(アンケートIDと有効期限から生成された)",
        example="eyJhbGciOi12JIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0X2lkIjoiMTIzNDU2Nzg5MCIsImV4cGlyZWQiOiIyMDIzLzA0LzAxIn0.OVFrpwjo_HhSyN4aI5y4jVAw06EkyCPi-sv0vEeMJ",
    )
    password: str = Field(
        ...,
        title="案件IDと有効期限から生成されたJWT",
        example="ABcd12*345Ef",
    )


class CheckAndGetSurveyAnonymousByIdResponse(CustomBaseModel):
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
    answer_user_name: str = Field(..., title="回答ユーザ名", example="ソニー太郎")
    customer_id: str = Field(
        None, title="取引先ID（顧客のみ）", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    customer_name: str = Field(None, title="取引先名（顧客のみ）", example="○○株式会社")
    company: str = Field(None, title="所属会社（顧客以外）", example="ソニーグループ株式会社")
    answers: List[Answer] = Field(None, title="設問ごとの回答")
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


class CheckAndUpdateSurveyAnonymousByIdRequest(CustomBaseModel):
    token: str = Field(
        ...,
        title="JWT(アンケートIDと有効期限から生成された)",
        example="eyJhbGciOi12JIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0X2lkIjoiMTIzNDU2Nzg5MCIsImV4cGlyZWQiOiIyMDIzLzA0LzAxIn0.OVFrpwjo_HhSyN4aI5y4jVAw06EkyCPi-sv0vEeMJ",
    )
    password: str = Field(
        ...,
        title="案件IDと有効期限から生成されたJWT",
        example="ABcd12*345Ef",
    )
    answers: List[Answer] = Field(..., title="設問ごとの回答")
    is_finished: bool = Field(..., title="回答済", example=True)
    is_disclosure: bool = Field(..., title="支援者へ開示OK", example=True)


class GetSurveyOfSatisfactionByIdRequest(CustomBaseModel):
    token: str = Field(
        ...,
        title="JWT(アンケートIDと有効期限から生成された)",
        example="eyJhbGciOi12JIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0X2lkIjoiMTIzNDU2Nzg5MCIsImV4cGlyZWQiOiIyMDIzLzA0LzAxIn0.OVFrpwjo_HhSyN4aI5y4jVAw06EkyCPi-sv0vEeMJ",
    )


class GetSurveyOfSatisfactionByIdResponse(CustomBaseModel):
    is_finished: bool = Field(..., title="回答済", example=True)
    survey_master_id: str = Field(
        None, title="アンケートマスターID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    survey_id: str = Field(
        None, title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    project_id: str = Field(
        None, title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    project_name: str = Field(None, title="案件名", example="○○プロジェクト")
    customer_name: str = Field(None, title="取引先名", example="○○株式会社")
    survey_revision: int = Field(None, title="アンケートマスターのバージョン番号", example="1")
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)


class UpdateSurveyOfSatisfactionByIdRequest(CustomBaseModel):
    token: str = Field(
        ...,
        title="JWT(アンケートIDと有効期限から生成された)",
        example="eyJhbGciOi12JIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0X2lkIjoiMTIzNDU2Nzg5MCIsImV4cGlyZWQiOiIyMDIzLzA0LzAxIn0.OVFrpwjo_HhSyN4aI5y4jVAw06EkyCPi-sv0vEeMJ",
    )
    answers: List[Answer] = Field(..., title="設問ごとの回答")
    is_finished: bool = Field(..., title="回答済", example=True)
    is_disclosure: bool = Field(..., title="支援者へ開示OK", example=True)
