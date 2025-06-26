from typing import List

from fastapi import APIRouter, Depends, Path
from fastapi import status as api_status

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.man_hour import (
    GetManHourBySupporterUserIdResponse,
    GetSummaryManHourTypeResponse,
    GetSummaryProjectManHourAlertQuery,
    GetSummaryProjectManHourAlertResponse,
    GetSummaryProjectManHourAlertsQuery,
    GetSummaryProjectManHourAlertsResponse,
    GetSummaryServiceTypesManHoursResponse,
    GetSummarySupporterManHoursQuery,
    GetSummarySupporterManHoursResponse,
    GetSummarySupporterOrganizationsManHoursQuery,
    GetSummarySupporterOrganizationsManHoursResponse,
    UpdateManHourBySupporterUserIdQuery,
    UpdateManHourBySupporterUserIdRequest,
    UpdateManHourBySupporterUserIdResponse,
    YaerMonthQuery,
)
from app.service.man_hour_service import ManHourService

router = APIRouter()


@router.get(
    "/man-hours/summary/supporter-organizations",
    tags=["ManHour"],
    description="支援者組織（課）別工数指標を取得します。 月別実績 SSAP粗利シミレーションとキーとなる指標 用",
    response_model=List[GetSummarySupporterOrganizationsManHoursResponse],
    status_code=api_status.HTTP_200_OK,
)
def get_summary_supporter_organizations_man_hours(
    query_params: GetSummarySupporterOrganizationsManHoursQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> List[GetSummarySupporterOrganizationsManHoursResponse]:
    """Get /api/man-hours/summary/supporter-organizations 支援者組織(課)別工数取得API

    Args:
        year (int): 対象年
        month (int): 対象月
        supporterOrganizationId (str): 支援者組織ID
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSummarySupporterOrganizationsManHoursResponse: 取得結果
    """
    return ManHourService.get_summary_supporter_organizations_man_hours(
        year=query_params.year,
        month=query_params.month,
        supporter_organization_id=query_params.supporter_organization_id,
        current_user=current_user,
    )


@router.get(
    "/man-hours/summary/service-types",
    tags=["ManHour"],
    description="サービス種別別工数指標を取得します。 月別実績 案件別契約時間と直接寄与工数（対面時間）内訳 描画用",
    response_model=List[GetSummaryServiceTypesManHoursResponse],
    status_code=api_status.HTTP_200_OK,
)
def get_summary_service_types_man_hours(
    query_params: YaerMonthQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> List[GetSummaryServiceTypesManHoursResponse]:
    """Get /api/man-hours/summary/service-types サービス種別別工数指標一覧取得API

    Args:
        year (int): 対象年
        month (int): 対象月
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        List[GetSummaryServiceTypesManHoursResponse]: 取得結果
    """

    return ManHourService.get_summary_service_types_man_hours(
        year=query_params.year,
        month=query_params.month,
        current_user=current_user,
    )


@router.get(
    "/man-hours/summary/project-man-hour-alerts",
    tags=["ManHour"],
    description="案件毎の工数状況とアラート一覧を取得します。",
    response_model=GetSummaryProjectManHourAlertsResponse,
    status_code=api_status.HTTP_200_OK,
)
def get_summary_project_man_hour_alerts(
    query_params: GetSummaryProjectManHourAlertsQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
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

    return ManHourService.get_summary_project_man_hour_alerts(
        year=query_params.year,
        month=query_params.month,
        supporter_organization_id=query_params.supporter_organization_id,
        service_type_id=query_params.service_type_id,
        current_user=current_user,
    )


@router.get(
    "/man-hours/summary/project-man-hour-alert",
    tags=["ManHour"],
    description="案件指定の工数状況とアラートを取得します。",
    response_model=GetSummaryProjectManHourAlertResponse,
    status_code=api_status.HTTP_200_OK,
)
def get_summary_project_man_hour_alert(
    query_params: GetSummaryProjectManHourAlertQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
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

    return ManHourService.get_summary_project_man_hour_alert(
        year=query_params.year,
        month=query_params.month,
        project_id=query_params.project_id,
        current_user=current_user,
    )


@router.get(
    "/man-hours/{supporter_user_id}",
    tags=["ManHour"],
    description="支援者単位で支援工数を取得します。",
    response_model=GetManHourBySupporterUserIdResponse,
    status_code=api_status.HTTP_200_OK,
)
def get_man_hour_by_supporter_user_id(
    query_params: YaerMonthQuery = Depends(),
    supporter_user_id: str = Path(..., title="支援者ユーザーID"),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
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

    return ManHourService.get_man_hour_by_supporter_user_id(
        year=query_params.year,
        month=query_params.month,
        supporter_user_id=supporter_user_id,
        current_user=current_user,
    )


@router.put(
    "/man-hours/{supporter_user_id}",
    tags=["ManHour"],
    description="支援者単位で支援工数を更新します。",
    response_model=UpdateManHourBySupporterUserIdResponse,
    status_code=api_status.HTTP_200_OK,
)
def update_man_hour_by_supporter_user_id(
    body: UpdateManHourBySupporterUserIdRequest,
    query_params: UpdateManHourBySupporterUserIdQuery = Depends(),
    supporter_user_id: str = Path(..., title="支援者ユーザーID"),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.MAN_HOUR_OPS.key,
            }
        )
    ),
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

    return ManHourService.update_man_hour_by_supporter_user_id(
        year=query_params.year,
        month=query_params.month,
        version=query_params.version,
        body=body,
        supporter_user_id=supporter_user_id,
        current_user=current_user,
    )


@router.get(
    "/man-hours/summary/type",
    tags=["ManHour"],
    description="月次工数分類別工数一覧（月次工数集計表）を取得します。",
    response_model=GetSummaryManHourTypeResponse,
    status_code=api_status.HTTP_200_OK,
)
def get_summary_man_hour_type(
    query_params: YaerMonthQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSummaryManHourTypeResponse:
    """Get /api/man-hours/summary/type 月次工数分類別工数一覧取得API

    Args:
        year (int): 対象年
        month (int): 対象月
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSummaryManHourTypeResponse: 取得結果
    """

    return ManHourService.get_summary_man_hour_type(
        year=query_params.year,
        month=query_params.month,
        current_user=current_user,
    )


@router.get(
    "/man-hours/summary/supporter",
    tags=["ManHour"],
    description="月次担当者別工数一覧を取得します。 担当者別月次工数集計で利用",
    response_model=GetSummarySupporterManHoursResponse,
    status_code=api_status.HTTP_200_OK,
)
def get_summary_supporter_man_hours(
    query_params: GetSummarySupporterManHoursQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSummaryManHourTypeResponse:
    """Get /api/man-hours/summary/supporter 月次担当者別工数一覧取得API

    Args:
        year (int): 対象年
        month (int): 対象月
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSummarySupporterManHoursResponse: 取得結果
    """

    return ManHourService.get_summary_supporter_man_hours(
        year=query_params.year,
        month=query_params.month,
        supporter_organization_id=query_params.supporter_organization_id,
        current_user=current_user,
    )
