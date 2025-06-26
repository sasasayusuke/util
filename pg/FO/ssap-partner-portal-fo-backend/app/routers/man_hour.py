from app.auth.auth import AuthUser, get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.man_hour import (
    GetManHourByMineQuery,
    GetManHourByMineResponse,
    GetSummaryProjectSupporterManHourStatusQuery,
    GetSummaryProjectSupporterManHourStatusResponse,
    UpdateManHourByMineQuery,
    UpdateManHourByMineRequest,
    UpdateManHourByMineResponse,
)
from app.service.man_hour_service import ManHourService
from fastapi import APIRouter, Depends, Path, status

router = APIRouter()


@router.get(
    "/man-hours/me",
    tags=["ManHour"],
    description="支援者自身の支援工数を取得します。",
    response_model=GetManHourByMineResponse,
    status_code=status.HTTP_200_OK,
)
def get_man_hour_by_mine(
    query_params: GetManHourByMineQuery = Depends(),
    current_user: AuthUser = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
            }
        )
    ),
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
    return ManHourService.get_man_hour_by_mine(
        year=query_params.year, month=query_params.month, current_user=current_user
    )


@router.get(
    "/man-hours/summary/project/{id}/supporter",
    tags=["ManHour"],
    description="案件別の支援工数状況を取得します。 工数アラートテーブルの係数を乗じる",
    response_model=GetSummaryProjectSupporterManHourStatusResponse,
    status_code=status.HTTP_200_OK,
)
def get_summary_project_supporter_man_hour_status(
    query_params: GetSummaryProjectSupporterManHourStatusQuery = Depends(),
    id: str = Path(..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"),
    current_user: AuthUser = Depends(
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
    return ManHourService.get_summary_project_supporter_man_hour_status(
        summary_month=query_params.summary_month,
        supporter_user_id=query_params.supporter_user_id,
        project_id=id,
        current_user=current_user,
    )


@router.put(
    "/man-hours/me",
    tags=["ManHour"],
    description="支援者自身の支援工数を更新します。",
    response_model=UpdateManHourByMineResponse,
    status_code=status.HTTP_200_OK,
)
def update_man_hour_by_mine(
    body: UpdateManHourByMineRequest,
    query_params: UpdateManHourByMineQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
            }
        )
    ),
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

    return ManHourService.update_man_hour_by_mine(
        year=query_params.year,
        month=query_params.month,
        version=query_params.version,
        body=body,
        current_user=current_user,
    )
