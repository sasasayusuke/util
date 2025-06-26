from fastapi import APIRouter, Depends, status, Path
from pydantic import Field

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.base import OKResponse
from app.schemas.survey import (
    ExportSurveysQuery,
    ExportSurveysResponse,
    GetSurveyByIdResponse,
    GetSurveyPlansQuery,
    GetSurveyPlansResponse,
    GetSurveysQuery,
    GetSurveySummaryAllQuery,
    GetSurveySummaryAllResponse,
    GetSurveySummaryReportsQuery,
    GetSurveySummarySupporterOrganizationsQuery,
    UpdateSurveyByIdQuery,
    UpdateSurveyByIdRequest,
)
from app.service.survey_service import SurveyService

router = APIRouter()


@router.get(
    "/surveys",
    tags=["Survey"],
    description="案件アンケートの一覧を取得します。",
    status_code=status.HTTP_200_OK,
)
def get_surveys(
    query_params: GetSurveysQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
):
    """Get /surveys 案件アンケート一覧取得

    Args:
        query_params (GetSurveysQuery): クエリパラメータ
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSurveysResponse: 取得結果
    """
    return SurveyService.get_surveys(
        query_params=query_params, current_user=current_user
    )


@router.get(
    "/surveys/export",
    tags=["Survey"],
    description="支援者別アンケート集計CSV、課別アンケート集計CSV、アンケートデータCSVを出力します。",
    response_model=ExportSurveysResponse,
    status_code=status.HTTP_200_OK,
)
def export_surveys(
    query_params: ExportSurveysQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
):
    """Get /surveys/export 案件アンケートCSV出力

    Args:
        query_params (ExportSurveysQuery): クエリパラメータ
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        ExportSurveysResponse: 取得結果
    """
    return SurveyService.export_surveys(
        query_params=query_params, current_user=current_user
    )


@router.get(
    "/surveys/plans",
    tags=["Survey"],
    description="案件アンケートの予実績一覧を取得します。",
    response_model=GetSurveyPlansResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_plans(
    query_params: GetSurveyPlansQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
):
    """Get /surveys/plans 案件アンケート予実績一覧取得

    Args:
        query_params (GetSurveyPlansQuery): クエリパラメータ
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSurveyPlansResponse: 取得結果
    """
    return SurveyService.get_survey_plans(
        query_params=query_params, current_user=current_user
    )


@router.get(
    "/surveys/resend/{survey_id}",
    tags=["Survey"],
    description="指定されたIDのアンケート回答依頼通知（メール、お知らせ）を再送信します。回答依頼送信済みでないアンケート、回答済みのアンケート、PPアンケートは再送信不可。",
    response_model=OKResponse,
    status_code=status.HTTP_200_OK,
)
def resend_survey_by_id(
    survey_id: str = Path(
        ..., title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> OKResponse:
    """Get /surveys/resend/{survey_id} 案件アンケート再送信API

    Args:
        survey_id (str): 案件アンケートID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        OKResponse: 取得結果
    """
    return SurveyService.resend_survey_by_id(
        survey_id=survey_id,
        current_user=current_user,
    )


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
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
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


@router.put(
    "/surveys/{survey_id}",
    tags=["Survey"],
    response_model=OKResponse,
    status_code=status.HTTP_200_OK,
)
def update_survey_by_id(
    body: UpdateSurveyByIdRequest,
    query_params: UpdateSurveyByIdQuery = Depends(),
    survey_id: str = Path(
        ..., title="案件アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> OKResponse:
    """Put /surveys/{survey_id} 案件アンケート更新API

    Args:
        body (UpdateSurveyByIdRequest): 更新内容
        query_params (UpdateSurveyByIdQuery): クエリパラメータ
        survey_id (str): 案件アンケートID
        current_user (_type_, optional): 認証済みのユーザー

    Returns:
        OKResponse: 更新結果
    """
    return SurveyService.update_survey_by_id(
        survey_id=survey_id,
        version=query_params.version,
        item=body,
        current_user=current_user,
    )


@router.get("/surveys/summary/reports", tags=["Survey"], status_code=status.HTTP_200_OK)
def get_summary_reports(
    query_params: GetSurveySummaryReportsQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
):
    """Get /surveys/summary/reports 月次レポート取得API

    Args:
        query_params (GetSurveySummaryReportsQuery, optional): クエリパラメータ
        current_user (_type_, optional): 認証済みのユーザー

    Returns:
        _type_: _description_
    """
    return SurveyService.get_survey_summary_reports(query_params, current_user)


@router.get(
    "/surveys/summary/supporter-organizations",
    tags=["Survey"],
    status_code=status.HTTP_200_OK,
)
def get_survey_summary_supporter_organizations(
    query_params: GetSurveySummarySupporterOrganizationsQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
):
    """Get /surveys/summary/supporter-organizations

    Args:
        query_params (GetSurveySummarySupporterOrganizationsQuery, optional): クエリパラメータ
        current_user (_type_, optional): 認証済みユーザー

    Returns:
        _type_: _description_
    """
    return SurveyService.get_survey_summary_supporter_organizations(
        query_params=query_params, current_user=current_user
    )


@router.get(
    "/surveys/summary/all",
    tags=["Survey"],
    response_model=GetSurveySummaryAllResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_summary_all(
    query_params: GetSurveySummaryAllQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSurveySummaryAllResponse:
    """Get /surveys/summary/all

    Args:
        query_params (GetSurveySummaryAllQuery, optional): クエリパラメータ
        current_user (_type_, optional): 認証済みユーザー

    Returns:
        GetSurveySummaryAllResponse: 取得結果
    """
    return SurveyService.get_survey_summary_all(
        query_params=query_params, current_user=current_user
    )
