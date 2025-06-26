from typing import List

from fastapi import APIRouter, Depends, Path
from fastapi import status as api_status
from fastapi.security import HTTPAuthorizationCredentials

from app.auth.jwt import JWTBearer
from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.base import OKResponse
from app.schemas.project import (
    GetProjectByIdResponse,
    GetProjectsQuery,
    GetProjectsResponse,
    SuggestProjectsQuery,
    SuggestProjectsResponse,
    UpdateProjectByIdQuery,
    UpdateProjectByIdRequest,
)
from app.service.project_service import ProjectService

router = APIRouter()


@router.get(
    "/projects/suggest",
    tags=["Project"],
    description="案件のサジェスト用情報を取得します。 フロントでサジェストするためのリストを取得。 ",
    response_model=List[SuggestProjectsResponse],
    status_code=api_status.HTTP_200_OK,
)
def suggest_projects(
    query_params: SuggestProjectsQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
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
                UserRoleType.CUSTOMER.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
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
    description="案件の検索・一覧を取得します。 顧客の場合は自身のアサイン案件のみ取得。",
    response_model=GetProjectsResponse,
    status_code=api_status.HTTP_200_OK,
)
def get_projects(
    query_params: GetProjectsQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.CUSTOMER.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetProjectsResponse:
    """Get /projects 案件一覧取得API

    Args:
        YearMonthFrom (int): 年月From
        YearMonthTo (int): 年月To
        DateFrom (int): 年月日From
        DateTo (int): 年月日To
        all (bool): 全案件取得（未指定時は担当案件）
        allAssigned (bool): 全担当取得（未指定時は主担当のみ）
        customerId (str): 取引先ID
        sort (GetProjectsSortType): ソート
        limit (int): 最大取得件数
        offsetPage (int): リストの中で何ページ目を取得するか
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetProjectsResponse: 取得結果
    """

    return ProjectService.get_projects(
        year_month_from=query_params.from_year_month,
        year_month_to=query_params.to_year_month,
        date_from=query_params.from_date,
        date_to=query_params.to_date,
        all=query_params.all,
        all_assigned=query_params.all_assigned,
        customer_id=query_params.customer_id,
        sort=query_params.sort,
        limit=query_params.limit,
        offset_page=query_params.offset_page,
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
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
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
