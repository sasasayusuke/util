from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.models.admin import AdminModel
from app.resources.const import UserRoleType
from app.schemas.base import OKResponse
from app.schemas.schedule import (
    BulkCreateSurveyScheduleRequest,
    BulkCreateSurveyScheduleResponse,
    BulkUpdateSurveyScheduleRequest,
    BulkUpdateSurveyScheduleResponse,
    CreateSupportSchedulesRequest,
    CreateSupportSchedulesResponse,
    CreateSurveySchedulesRequest,
    DeleteMultipleSurveySchedulesRequest,
    DeleteScheduleIdDateResponse,
    DeleteSupportSchedulesByIdDateQuery,
    DeleteSurveySchedulesByIdDateQuery,
    GetSupportSchedulesByIdResponse,
    GetSurveySchedulesByIdResponse,
    PutScheduleIdDateResponse,
    PutSupportScheduleIdDateQuery,
    PutSupportScheduleIdDateRequest,
    PutSupportScheduleIdDateResponse,
    PutSurveyScheduleIdDateQuery,
    PutSurveyScheduleIdDateRequest,
    UpdateMultipleSurveySchedulesRequest,
)
from app.service.schedule_service import SchedulesService

router = APIRouter()


@router.post(
    "/schedules/support/{project_id}",
    tags=["Schedules"],
    description="支援期間内分の案件カルテを作成します。",
    response_model=CreateSupportSchedulesResponse,
    status_code=status.HTTP_200_OK,
)
def create_schedules(
    body: CreateSupportSchedulesRequest,
    project_id: str = Path(...),
    current_user: AdminModel = Depends(
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
) -> CreateSupportSchedulesResponse:
    """Post /schedules/{project_id} スケジュール登録API
    Args:
        body (CreateSupportSchedulesRequest): 登録内容
        current_user (AdminModel): 認証済みユーザー

    Raises:
        HTTPException: 400

    Returns:
        CreateSupportSchedulesResponse: 登録結果
    """
    return SchedulesService.create_support_schedules(
        item=body, project_id=project_id, current_user=current_user
    )


@router.get(
    "/schedules/support/{project_id}",
    tags=["Schedules"],
    description="指定した案件IDの案件カルテを取得します。",
    response_model=GetSupportSchedulesByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_support_schedules_by_id(
    project_id: str = Path(..., title="案件ID"),
    current_user: AdminModel = Depends(
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
) -> GetSupportSchedulesByIdResponse:
    """Get /schedules/support/{project_id} 案件スケジュール取得API

    Args:
        project_id (str): 案件ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSupportSchedulesByIdResponse: 取得結果
    """
    return SchedulesService.get_support_schedules_by_id(
        project_id=project_id, current_user=current_user
    )


@router.put(
    "/schedules/support",
    tags=["Schedules"],
    description="指定したIDの案件カルテを取得・更新します。",
    response_model=PutSupportScheduleIdDateResponse,
    status_code=status.HTTP_200_OK,
)
def put_support_schedules_by_id_date(
    body: PutSupportScheduleIdDateRequest,
    query_params: PutSupportScheduleIdDateQuery = Depends(),
    current_user: AdminModel = Depends(
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
) -> PutSupportScheduleIdDateResponse:
    """Put /schedules/support 「支援」スケジュール更新API

    Args:
        body (PutSupportScheduleIdDateRequest): 更新内容
        karte_id (str): カルテID クエリパラメータで指定
        version (int): ロックキー（楽観ロック制御） クエリパラメータで指定(>=1)
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        PutSupportScheduleIdDateResponse: 更新結果
    """
    return SchedulesService.update_support_schedules_by_id_and_date(
        item=body,
        karte_id=query_params.karte_id,
        version=query_params.version,
        current_user=current_user,
    )


@router.delete(
    "/schedules/support",
    tags=["Schedules"],
    description="指定した案件カルテを削除します。",
    response_model=DeleteScheduleIdDateResponse,
    status_code=status.HTTP_200_OK,
)
def delete_support_schedules_by_id_date(
    query_params: DeleteSupportSchedulesByIdDateQuery = Depends(),
    current_user: AdminModel = Depends(
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
) -> DeleteScheduleIdDateResponse:
    """Delete /schedules/support 「支援」スケジュール削除API

    Args:
        DeleteSupportSchedulesByIdDateQuery(schedule_id, version): クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        DeleteScheduleIdDateResponse: 更新結果
    """
    return SchedulesService.delete_support_schedules_by_id_date(
        query_params=query_params,
        current_user=current_user,
    )


@router.get(
    "/schedules/survey/{project_id}",
    tags=["Schedules"],
    description="指定した案件IDの案件アンケートを取得します。",
    response_model=GetSurveySchedulesByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_schedules_by_id(
    project_id: str = Path(..., title="案件ID"),
    current_user: AdminModel = Depends(
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
) -> GetSurveySchedulesByIdResponse:
    """Get /schedules/survey/{project)id} アンケート案件スケジュール取得API

    Args:
        project_id (str): 案件ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSurveySchedulesByIdResponse: 取得結果
    """
    return SchedulesService.get_survey_schedules_by_id(
        project_id=project_id,
        current_user=current_user,
    )


@router.post("/schedules/survey/{project_id}", tags=["Schedules"])
def create_survey_schedules(
    body: CreateSurveySchedulesRequest,
    project_id: str = Path(..., title="案件ID"),
    current_user: AdminModel = Depends(
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
    """Post /schedules/survey/{project)id} アンケートスケジュール登録API

    Args:
        body (CreateSurveySchedulesRequest): 登録内容
        project_id (str: 案件ID
        current_user (AdminModel): 認証済みユーザー

    Returns:
        OKResponse: 登録結果
    """
    return SchedulesService.create_survey_schedules(
        project_id=project_id,
        item=body,
        current_user=current_user,
    )


@router.put(
    "/schedules/survey",
    tags=["Schedules"],
    description="指定したアンケートIDの案件アンケートを取得・更新します。",
    response_model=PutScheduleIdDateResponse,
    status_code=status.HTTP_200_OK,
)
def put_survey_schedules_by_id_date(
    body: PutSurveyScheduleIdDateRequest,
    query_params: PutSurveyScheduleIdDateQuery = Depends(),
    current_user: AdminModel = Depends(
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
) -> PutScheduleIdDateResponse:
    """Put /schedules/survey 「アンケート」スケジュール更新API

    Args:
        body (PutSurveyScheduleIdDateRequest): 更新内容
        survey_id (str): アンケートID クエリパラメータで指定
        version (int): ロックキー（楽観ロック制御） クエリパラメータで指定(>=1)
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        PutScheduleIdDateResponse: 更新結果
    """
    return SchedulesService.update_survey_schedules_by_id_and_date(
        item=body,
        query_params=query_params,
        current_user=current_user,
    )


@router.delete(
    "/schedules/survey",
    tags=["Schedules"],
    description="指定した案件アンケートを削除します。",
    response_model=DeleteScheduleIdDateResponse,
    status_code=status.HTTP_200_OK,
)
def delete_survey_schedules_by_id_date(
    query_params: DeleteSurveySchedulesByIdDateQuery = Depends(),
    current_user: AdminModel = Depends(
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
) -> DeleteScheduleIdDateResponse:
    """Delete /schedules/survey 「アンケート」スケジュール削除API

    Args:
        DeleteSupportSchedulesByIdDateQuery(survey_id, version): クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        DeleteScheduleIdDateResponse: 更新結果
    """
    return SchedulesService.delete_survey_schedules_by_id_date(
        query_params=query_params,
        current_user=current_user,
    )


@router.put(
    "/schedules/survey/multiple",
    tags=["Schedules"],
    description="リクエストで指定された複数のアンケートを新規登録、更新します。",
    response_model=OKResponse,
    status_code=status.HTTP_200_OK,
)
def update_multiple_survey_schedules(
    body: UpdateMultipleSurveySchedulesRequest,
    current_user: AdminModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SURVEY_OPS.key,
            }
        )
    ),
) -> OKResponse:
    """Put /schedules/survey/multiple 「アンケート」スケジュール複数登録・更新API

    Args:
        body (UpdateMultipleSurveySchedulesRequest): 更新内容
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        OKResponse: 更新結果
    """
    return SchedulesService.update_multiple_survey_schedules(
        item=body,
        current_user=current_user,
    )


@router.put(
    "/schedules/survey/multiple/delete",
    tags=["Schedules"],
    description="リクエストで指定された複数のアンケートを削除します。",
    response_model=OKResponse,
    status_code=status.HTTP_200_OK,
)
def delete_multiple_survey_schedules(
    body: DeleteMultipleSurveySchedulesRequest,
    current_user: AdminModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SURVEY_OPS.key,
            }
        )
    ),
) -> OKResponse:
    """Put /schedules/survey/multiple/delete 「アンケート」スケジュール複数削除API

    Args:
        body (DeleteMultipleSurveySchedulesRequest): 削除情報
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        OKResponse: 削除結果
    """
    return SchedulesService.delete_multiple_survey_schedules(
        item=body,
        current_user=current_user,
    )


@router.post(
    "/schedules/survey/bulk/{project_id}",
    tags=["Schedules"],
    description="アンケートスケジュールを一括登録します。",
    response_model=BulkCreateSurveyScheduleResponse,
    status_code=status.HTTP_200_OK,
)
def bulk_createsurvey_schedules(
    body: BulkCreateSurveyScheduleRequest,
    project_id: str = Path(..., title="案件ID"),
    current_user: AdminModel = Depends(
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
) -> BulkCreateSurveyScheduleResponse:
    """Post /schedules/survey/bulk/{project_id} アンケートスケジュール一括登録API

    Args:
        body (BulkCreateSurveyScheduleRequest): 一括登録内容
        project_id (str): 案件ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        BulkCreateSurveyScheduleResponse: 更新結果
    """
    return SchedulesService.bulk_create_survey_schedules(
        item=body,
        project_id=project_id,
        current_user=current_user,
    )


@router.put(
    "/schedules/survey/bulk/{project_id}",
    tags=["Schedules"],
    description="アンケートスケジュールを一括更新します。",
    response_model=BulkUpdateSurveyScheduleResponse,
    status_code=status.HTTP_200_OK,
)
def bulk_update_survey_schedules(
    body: BulkUpdateSurveyScheduleRequest,
    project_id: str = Path(..., title="案件ID"),
    current_user: AdminModel = Depends(
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
) -> BulkUpdateSurveyScheduleResponse:
    """Put /schedules/survey/bulk/{project_id} アンケートスケジュール一括更新API

    Args:
        body (BulkUpdateSurveyScheduleRequest): 一括更新内容
        project_id (str): 案件ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        BulkUpdateSurveyScheduleResponse: 更新結果
    """
    return SchedulesService.bulk_update_survey_schedules(
        item=body,
        project_id=project_id,
        current_user=current_user,
    )
