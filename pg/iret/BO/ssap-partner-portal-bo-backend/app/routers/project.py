from typing import List

from fastapi import APIRouter, Depends, Path
from fastapi import status as api_status
from fastapi.security import HTTPAuthorizationCredentials

from app.auth.jwt import JWTBearer
from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.base import OKResponse
from app.schemas.project import (
    CreateProjectRequest,
    CreateProjectResponse,
    DeleteProjectByIdQuery,
    GetMonthlyProjectsReponse,
    GetProjectByIdResponse,
    GetProjectsQuery,
    GetProjectsResponse,
    ImportProjectsRequest,
    ImportProjectsResponse,
    SuggestProjectsQuery,
    SuggestProjectsResponse,
    UpdateProjectByIdQuery,
    UpdateProjectByIdRequest,
)
from app.service.project_service import ProjectService

router = APIRouter()


@router.post(
    "/projects",
    tags=["Project"],
    description="案件を一件作成します",
    response_model=CreateProjectResponse,
    status_code=api_status.HTTP_200_OK,
)
def create_project(
    body: CreateProjectRequest,
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
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
) -> CreateProjectResponse:
    """Post /projects 案件新規個別登録API

    Args:
        body (CreateProjectRequest): 登録内容
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        CreateProjectResponse: 登録結果
    """
    return ProjectService.create_project(
        item=body, current_user=current_user, authorization=authorization
    )


@router.post(
    "/projects/import",
    tags=["Project"],
    description="案件のCSVデータをエラーチェックまたは取り込み実行します",
    response_model=ImportProjectsResponse,
    status_code=api_status.HTTP_200_OK,
)
def import_project(
    body: ImportProjectsRequest,
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
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
) -> ImportProjectsResponse:
    """Post /projects/import 案件情報一括登録API
    Args:
        body (ImportProjectsRequest): 登録内容
        current_user (Behavior, optional): 認証済みのユーザー
    Returns:
        ImportProjectsResponse: 登録結果
    """
    return ProjectService.import_project(
        item=body, current_user=current_user, authorization=authorization
    )


@router.get(
    "/projects/suggest",
    tags=["Project"],
    description="案件のサジェスト用情報を取得します。 フロントでサジェストするためのリストを取得。",
    response_model=List[SuggestProjectsResponse],
    status_code=api_status.HTTP_200_OK,
)
def suggest_projects(
    query_params: SuggestProjectsQuery = Depends(),
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
) -> List[SuggestProjectsResponse]:
    """Get /projects/suggest 案件サジェスト用データ取得API

    Args:
        customerId (str): 取引先ID（結果の絞り込み）
        sort (SuggestProjectsSortType): ソート クエリパラメータで指定（'name:asc'）
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        SuggestProjectsResponse: 取得結果
    """
    return ProjectService.suggest_projects(
        customer_id=query_params.customer_id,
        sort=query_params.sort,
        current_user=current_user,
    )


@router.get(
    "/projects/summary/{year}/{month}",
    tags=["Project"],
    description="月次案件情報を取得します。",
    response_model=GetMonthlyProjectsReponse,
    status_code=api_status.HTTP_200_OK,
)
def get_monthly_projects(
    year: int = Path(..., title="対象年", ge=2000, le=3000, example=2022),
    month: int = Path(..., title="対象月", ge=1, le=12, example=6),
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
) -> GetMonthlyProjectsReponse:
    """Get /projects/summary/{year}/{month} 月次案件情報取得API

    Args:
        year (int): 対象年 パスパラメータで指定
        month (int): 対象月 パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetMonthlyProjectsReponse: 取得結果
    """
    return ProjectService.get_monthly_projects(
        year=year, month=month, current_user=current_user
    )


@router.get(
    "/projects/{project_id}",
    tags=["Project"],
    description="案件をIDで一意に取得します。",
    response_model=GetProjectByIdResponse,
    status_code=api_status.HTTP_200_OK,
)
def get_project_by_id(
    project_id: str = Path(
        ..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
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
) -> GetProjectByIdResponse:
    """Get /projects/{project_id} 案件情報詳細取得API

    Args:
        project_id (str): 案件ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetProjectByIdResponse: 取得結果
    """
    return ProjectService.get_project_by_id(
        project_id=project_id, current_user=current_user
    )


@router.get(
    "/projects",
    tags=["Project"],
    description="案件の検索・一覧を取得します。 ",
    response_model=GetProjectsResponse,
    status_code=api_status.HTTP_200_OK,
)
def get_projects(
    query_params: GetProjectsQuery = Depends(),
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
) -> GetProjectsResponse:
    """Get /projects 案件一覧取得API

    Args:
        name (str): 案件名
        status (SupportStatusType): ステータス（支援前、支援中、支援後）
        fromYearMonth (int): 対象年月（From）
        toYearMonth (int): 対象年月（To）
        customerId (str): 取引先ID
        mainSalesUserId (str): 営業担当者ID
        supporterOrganizationId (str): 支援者組織ID. カンマ区切りで複数のIDを渡すことを可能とする.
        serviceTypeId (str): サービス種別ID
        sort (GetProjectsSortType): ソート
        offsetPage (int): リストの中で何ページ目を取得するか Default:1
        limit (int): 最大取得件数 Default:Constの規定値(DefaultPageItemCount.limit)
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetProjectsResponse: 取得結果
    """

    supporter_organization_id_list = (
        query_params.supporter_organization_id.split(",")
        if query_params.supporter_organization_id
        else None
    )

    return ProjectService.get_projects(
        name=query_params.name,
        status=query_params.status,
        from_year_month=query_params.from_year_month,
        to_year_month=query_params.to_year_month,
        customer_id=query_params.customer_id,
        main_sales_user_id=query_params.main_sales_user_id,
        supporter_organization_id_list=supporter_organization_id_list,
        service_type_id=query_params.service_type_id,
        is_karte_usage_project_list_of_sales=query_params.is_karte_usage_project_list_of_sales,
        sort=query_params.sort,
        offset_page=query_params.offset_page,
        limit=query_params.limit,
        current_user=current_user,
    )


@router.put(
    "/projects/{project_id}",
    tags=["Project"],
    description="案件をIDで一意に更新します。",
    response_model=OKResponse,
    status_code=api_status.HTTP_200_OK,
)
def update_project_by_id(
    body: UpdateProjectByIdRequest,
    query_params: UpdateProjectByIdQuery = Depends(),
    project_id: str = Path(
        ..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
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
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
) -> OKResponse:
    """Put /projects/{project_id} 案件情報更新API

    Args:
        body (UpdateProjectByIdRequest): 更新内容
        version: ロックキー（楽観ロック制御） クエリパラメータで指定(>=1)
        project_id (str): 案件ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        OKResponse: 取得結果
    """
    return ProjectService.update_project_by_id(
        item=body,
        version=query_params.version,
        project_id=project_id,
        current_user=current_user,
        authorization=authorization,
    )


@router.delete(
    "/projects/{project_id}",
    tags=["Project"],
    description="案件をIDで一意に削除します。",
    response_model=OKResponse,
    status_code=api_status.HTTP_200_OK,
)
def delete_project_by_id(
    query_params: DeleteProjectByIdQuery = Depends(),
    project_id: str = Path(
        ..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
            }
        )
    ),
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
) -> OKResponse:
    """Delete /projects/{project_id} 案件情報削除API

    Args:
        version: ロックキー（楽観ロック制御） クエリパラメータで指定(>=1)
        project_id (str): 案件ID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        OKResponse: 取得結果
    """
    return ProjectService.delete_project_by_id(
        version=query_params.version,
        project_id=project_id,
        current_user=current_user,
        authorization=authorization,
    )
