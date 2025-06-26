from datetime import datetime

from fastapi import APIRouter, Depends, HTTPException, Path, status

from app.auth.auth import get_current_user_factory
from app.models.user import UserModel
from app.resources.const import UserRoleType
from app.schemas.base import OKResponse
from app.schemas.survey import (
    CheckAndGetSurveyAnonymousByIdRequest,
    CheckAndGetSurveyAnonymousByIdResponse,
    CheckAndUpdateSurveyAnonymousByIdRequest,
    GetSurveyByIdResponse,
    GetSurveysByMineQuery,
    GetSurveysByMineResponse,
    GetSurveysQuery,
    GetSurveysResponse,
    GetSurveyOfSatisfactionByIdRequest,
    GetSurveyOfSatisfactionByIdResponse,
    GetSurveySummaryByMineQuery,
    GetSurveySummarySupporterOrganizationByMineQuery,
    GetSurveySummarySupporterOrganizationByMineResponse,
    PostCheckSurveyByIdPasswordRequest,
    PostCheckSurveyByIdPasswordResponse,
    UpdateSurveyByIdQuery,
    UpdateSurveyByIdRequest,
    UpdateSurveyByIdResponse,
    UpdateSurveyOfSatisfactionByIdRequest,
)
from app.service.survey_service import SurveyService

router = APIRouter()


@router.get(
    "/surveys/me",
    tags=["Survey"],
    description="自身の回答対象の案件アンケートの一覧を取得します。",
    response_model=GetSurveysByMineResponse,
)
def get_surveys_by_mine(
    query_params: GetSurveysByMineQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.CUSTOMER.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSurveysByMineResponse:
    """Get /surveys/me 自身の回答対象のアンケート取得

    Args:
        query_params (GetSurveysByMineQuery): クエリパラメータ
        current_user (UserModel): API呼び出しユーザー

    Returns:
        List[GetSurveysByMineResponse]: 取得結果
    """
    return SurveyService.get_survey_by_mine(
        query_params=query_params, current_user=current_user
    )


@router.post(
    "/surveys/auth",
    tags=["Survey"],
    description="パスワードとJWT(アンケートIDと有効期限から生成された)で認証する",
    response_model=PostCheckSurveyByIdPasswordResponse,
    status_code=status.HTTP_200_OK,
)
def post_check_survey_by_id_password(
    body: PostCheckSurveyByIdPasswordRequest,
) -> PostCheckSurveyByIdPasswordResponse:
    """Post /surveys/auth パスワードとJWT(アンケートIDと有効期限から生成された)で認証するAPI

    Args:
        body (PostCheckSurveyByIdPasswordRequest): アンケート認証リクエスト内容

    Returns:
        PostCheckSurveyByIdPasswordResponse: 匿名アンケート認証結果
    """
    return SurveyService.post_check_survey_by_id_password(item=body)


@router.get(
    "/surveys/{survey_id}",
    tags=["Survey"],
    description="案件アンケートをIDで一意に取得します。",
    response_model=GetSurveyByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_by_id(
    survey_id: str = Path(
        ..., title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.CUSTOMER.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSurveyByIdResponse:
    """Get /surveys/{survey_id} 案件アンケート詳細取得API

    Args:
        survey_id (str): 案件アンケートID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSurveyByIdResponse: 取得結果
    """
    return SurveyService.get_survey_by_id(
        survey_id=survey_id, current_user=current_user
    )


@router.get(
    "/surveys",
    tags=["Survey"],
    description="案件アンケートの一覧を取得します。",
    response_model=GetSurveysResponse,
)
def get_surveys(
    query_params: GetSurveysQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.CUSTOMER.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSurveysResponse:
    """Get /surveys 自身の回答対象のアンケート取得
    Args:
        query_params (GetSurveysQuery): クエリパラメータ
        current_user (UserModel): API呼び出しユーザー
    Returns:
        GetSurveysResponse: 取得結果
    """

    """集計月、集計月From～Toは、複合指定NG"""
    # 複合指定チェック
    specified_summary_month: bool = (
        True if query_params.summary_month is not None else False
    )
    specified_range_of_summary_month: bool = (
        True
        if (
            query_params.summary_month_from is not None
            or query_params.summary_month_to is not None
        )
        else False
    )
    specified_range_of_plan_survey_response_date: bool = (
        True
        if (
            query_params.plan_survey_response_date_from is not None
            or query_params.plan_survey_response_date_to is not None
        )
        else False
    )

    if [
        specified_summary_month,
        specified_range_of_summary_month,
        specified_range_of_plan_survey_response_date,
    ].count(True) > 1:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="SummaryMonth, range of summaryMonth, range of planSurveyResponseDate cannot be specified together.",
        )

    # 日付チェック、FromTo逆転チェック
    try:
        _ = (
            datetime.strptime((query_params.summary_month), "%Y%m")
            if query_params.summary_month
            else None
        )
        from_date_ym = (
            datetime.strptime((query_params.summary_month_from), "%Y%m")
            if query_params.summary_month_from
            else None
        )
        to_date_ym = (
            datetime.strptime((query_params.summary_month_to), "%Y%m")
            if query_params.summary_month_to
            else None
        )
        from_datetime_ym = (
            datetime.strptime((query_params.plan_survey_response_date_from), "%Y%m%d")
            if query_params.plan_survey_response_date_from
            else None
        )
        to_datetime_ym = (
            datetime.strptime((query_params.plan_survey_response_date_to), "%Y%m%d")
            if query_params.plan_survey_response_date_to
            else None
        )
    except ValueError:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="SummaryMonth, summaryMonthFrom, summaryMonthTo should be 6-digit numbers(yyyymm). PlanSurveyResponseDateFrom, planSurveyResponseDateTo should be 8-digit numbers(yyyymmdd).",
        )
    if from_date_ym and to_date_ym:
        if from_date_ym > to_date_ym:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="SummaryMonthTo is greater than or equal to summaryMonthFrom.",
            )
    if from_datetime_ym and to_datetime_ym:
        if from_datetime_ym > to_datetime_ym:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="PlanSurveyResponseDateTo is greater than or equal to planSurveyResponseDateFrom.",
            )

    return SurveyService.get_surveys(
        query_params=query_params, current_user=current_user
    )


@router.put(
    "/surveys/{survey_id}",
    tags=["Survey"],
    description="案件アンケートをIDで一意に更新します。",
    response_model=UpdateSurveyByIdResponse,
    status_code=status.HTTP_200_OK,
)
def update_survey_by_id(
    body: UpdateSurveyByIdRequest,
    query_params: UpdateSurveyByIdQuery = Depends(),
    survey_id: str = Path(
        ..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.CUSTOMER.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> UpdateSurveyByIdResponse:
    """Put /surveys/{survey_id} 案件アンケート情報更新API

    Args:
        body (UpdateSurveyByIdRequest): 更新内容
        query_params (UpdateSurveyByIdQuery): クエリパラメータ
        survey_id (str): 案件アンケートID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        UpdateSurveyByIdResponse: 取得結果
    """
    return SurveyService.update_survey_by_id(
        item=body,
        query_params=query_params,
        survey_id=survey_id,
        current_user=current_user,
    )


@router.post(
    "/surveys/anonymous/{survey_id}",
    tags=["Survey"],
    description="匿名用の案件アンケートをIDで一意に取得します。 パスワードとJWT(アンケートIDと有効期限から生成された)で認証します。",
    response_model=CheckAndGetSurveyAnonymousByIdResponse,
    status_code=status.HTTP_200_OK,
)
def check_and_get_survey_anonymous_by_id(
    body: CheckAndGetSurveyAnonymousByIdRequest,
    survey_id: str = Path(
        ..., title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
) -> CheckAndGetSurveyAnonymousByIdResponse:
    """Get /surveys/anonymous/{survey_id} 匿名用の案件アンケートをIDで一意取得API

    Args:
        body (CheckAndGetSurveyAnonymousByIdRequest): 認証情報
        survey_id (str): 案件アンケートID パスパラメータで指定

    Returns:
        CheckAndGetSurveyAnonymousByIdResponse: 取得結果
    """
    return SurveyService.check_and_get_survey_anonymous_by_id(
        item=body, survey_id=survey_id
    )


@router.put(
    "/surveys/anonymous/{survey_id}",
    tags=["Survey"],
    description="匿名用の案件アンケートをIDで一意に更新。",
    response_model=OKResponse,
    status_code=status.HTTP_200_OK,
)
def check_and_update_survey_anonymous_by_id(
    body: CheckAndUpdateSurveyAnonymousByIdRequest,
    query_params: UpdateSurveyByIdQuery = Depends(),
    survey_id: str = Path(
        ..., title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
) -> OKResponse:
    """Put /surveys/anonymous/{survey_id} 匿名用の案件アンケートをIDで一意に更新API

    Args:
        survey_id (str): 案件アンケートID パスパラメータで指定

    Returns:
        OKResponse: 更新結果
    """
    return SurveyService.check_and_update_survey_anonymous_by_id(
        item=body, query_params=query_params, survey_id=survey_id
    )


@router.post(
    "/surveys/satisfaction",
    tags=["Survey"],
    description="満足度評価のみ回答アンケート用のアンケートをIDで一意に取得します。",
    response_model=GetSurveyOfSatisfactionByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_of_satisfaction_by_id(
    body: GetSurveyOfSatisfactionByIdRequest,
) -> GetSurveyOfSatisfactionByIdResponse:
    """Get /surveys/satisfaction 満足度評価のみ回答アンケート用のアンケートIDで一意取得API

    Args:
        body (GetSurveyOfSatisfactionByIdRequest): 認証情報

    Returns:
        GetSurveyOfSatisfactionByIdResponse: 取得結果
    """
    return SurveyService.get_survey_of_satisfaction_by_id(item=body)


@router.put(
    "/surveys/satisfaction/{survey_id}",
    tags=["Survey"],
    description="満足度評価のみ回答アンケート用の案件アンケートをIDで一意に更新。",
    response_model=OKResponse,
    status_code=status.HTTP_200_OK,
)
def update_survey_of_satisfaction_by_id(
    body: UpdateSurveyOfSatisfactionByIdRequest,
    query_params: UpdateSurveyByIdQuery = Depends(),
) -> OKResponse:
    """Put /surveys/satisfaction/{survey_id} 満足度評価のみ回答用の案件アンケートをIDで一意に更新API

    Args:
        body (UpdateSurveyOfSatisfactionByIdRequest): 更新内容
        query_params (UpdateSurveyByIdQuery): クエリパラメータ

    Returns:
        OKResponse: 更新結果
    """
    return SurveyService.update_survey_of_satisfaction_by_id(
        item=body, query_params=query_params
    )


@router.get("/surveys/summary/me", tags=["Survey"])
def get_survey_summary_me(
    query_params: GetSurveySummaryByMineQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
):
    return SurveyService.get_survey_summary_by_mine(
        query_params=query_params, current_user=current_user
    )


@router.get("/surveys/summary/supporter-organizations/me")
def get_survey_summary_supporter_organizations(
    query_params: GetSurveySummarySupporterOrganizationByMineQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSurveySummarySupporterOrganizationByMineResponse:
    """Get /surveys/summary/supporter-organizations/me

    Args:
        query_params (GetSurveySummarySupporterOrganizationByMineQuery, optional): クエリパラメータ
        current_user (_type_, optional): 認証済みユーザー

    Returns:
        GetSurveySummarySupporterOrganizationByMineResponse: _description_
    """
    return SurveyService.get_survey_summary_supporter_organizations_by_mine(
        query_params=query_params, current_user=current_user
    )
