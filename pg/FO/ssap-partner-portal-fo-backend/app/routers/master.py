from app.auth.jwt import JWTBearer
from fastapi import APIRouter, Depends, Path, status
from fastapi.security import HTTPAuthorizationCredentials

from app.auth.auth import AuthUser, get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.master import (
    GetBatchControlResponse,
    GetServiceTypesQuery,
    GetServiceTypesResponse,
    GetSupporterOrganizationsQuery,
    GetSupporterOrganizationsResponse,
    GetSelectItemsQuery,
    GetSelectItemsResponse,
)
from app.service.master_service import MasterService

router = APIRouter()


@router.get(
    "/masters/service-types",
    tags=["Master"],
    description="サービス種別の一覧を取得します。",
    response_model=GetServiceTypesResponse,
    status_code=status.HTTP_200_OK,
)
def get_service_types(
    query_params: GetServiceTypesQuery = Depends(),
    current_user: AuthUser = Depends(
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
) -> GetServiceTypesResponse:
    """Get /masters/service-types サービス種別一覧取得API
    Returns:
        GetServiceTypesResponse: 取得結果
    """
    return MasterService.get_service_types(
        current_user=current_user, disabled=query_params.disabled
    )


@router.get(
    "/masters/batch-control/{id}",
    tags=["Master"],
    description="各種集計バッチ処理の最終完了日時を取得します。",
    response_model=GetBatchControlResponse,
    status_code=status.HTTP_200_OK,
)
def get_batch_control_by_id(
    id: str = Path(...),
    current_user: AuthUser = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.CUSTOMER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetBatchControlResponse:
    """Get /masters/batch-control/{id} バッチ処理最終完了日時取得API
    Args:
        id (str): バッチ処理関数ID パスパラメータで指定

    Returns:
        GetBatchControlResponse: 取得結果
    """
    return MasterService.get_batch_control_by_id(batch_id=id, current_user=current_user)


@router.get(
    "/masters/supporter-organizations",
    tags=["Master"],
    description="支援者組織の一覧を取得します。",
    response_model=GetSupporterOrganizationsResponse,
    status_code=status.HTTP_200_OK,
)
def get_supporter_organization(
    query_params: GetSupporterOrganizationsQuery = Depends(),
    current_user: AuthUser = Depends(
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
) -> GetSupporterOrganizationsResponse:
    """Get /masters/supporter-organizations 支援者組織一覧取得API

    Returns:
        GetSupporterOrganizationsResponse: 取得結果
    """
    return MasterService.get_supporter_organizations(
        current_user=current_user, disabled=query_params.disabled
    )


@router.get(
    "/masters/select-items",
    tags=["Master"],
    description="セレクトの選択肢を取得する",
    response_model=GetSelectItemsResponse,
    status_code=status.HTTP_200_OK,
)
def get_select_items(
    query_params: GetSelectItemsQuery = Depends(),
    current_user: AuthUser = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> GetSelectItemsResponse:
    """Get /masters/select-items 選択肢取得API
    Args:
        current_user (AuthUser): 認証済みのユーザー
        data_type (str): データ区分
    Returns:
        GetSelectItemsResponse: 取得結果
    """
    return MasterService.get_select_items(
        current_user=current_user,
        data_type=query_params.data_type,
    )
