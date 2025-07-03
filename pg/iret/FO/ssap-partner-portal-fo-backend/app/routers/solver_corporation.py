from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.models.user import UserModel
from app.resources.const import UserRoleType
from app.schemas.solver_corporation import (
    DeleteSolverCorporationByIdQuery,
    GetSolverCorporationByIdResponse,
    GetSolverCorporationsResponse,
    GetSolverCorporationsQuery,
    UpdateSolverCorporationByIdQuery,
    UpdateSolverCorporationByIdRequest,
)
from app.service.solver_corporation_service import SolverCorporationService
from app.schemas.base import OKResponse

router = APIRouter()


@router.get(
    "/solver-corporations",
    tags=["SolverCorporation"],
    description="法人ソルバーの一覧を取得します。",
    response_model=GetSolverCorporationsResponse,
    status_code=status.HTTP_200_OK,
)
def get_solver_corporations(
    query_params: GetSolverCorporationsQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
            }
        )
    ),
) -> GetSolverCorporationsResponse:
    """Get /solver-corporations 法人ソルバー一覧取得API
    Args:
        sort (GetSolverCorporationsSortType): ソート
        disabled (bool): trueならば無効の法人ソルバーのみを取得 クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー
    Returns:
        GetSolverCorporationsResponse: 取得結果
    """
    return SolverCorporationService.get_solver_corporations(
        disabled=query_params.disabled,
        sort=query_params.sort,
        current_user=current_user,
    )


@router.get(
    "/solver-corporations/{solver_corporation_id}",
    tags=["SolverCorporation"],
    description="法人ソルバーをIDで一意に取得します。",
    response_model=GetSolverCorporationByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_solver_corporation_by_id(
    solver_corporation_id: str = Path(..., title="法人ソルバーID"),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> GetSolverCorporationByIdResponse:
    """Get /solver-corporations/{solver_corporation_id} 法人ソルバー情報取得API

    Args:
        solver_corporation_id (str): 法人ソルバーID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSolverCorporationByIdResponse: 取得結果
    """
    return SolverCorporationService.get_solver_corporation_by_id(
        current_user, solver_corporation_id
    )


@router.put(
    "/solver-corporations/{solver_corporation_id}",
    tags=["SolverCorporation"],
    description="法人ソルバーをIDで一意に更新します。",
    status_code=status.HTTP_200_OK,
)
def update_solver_corporation_by_id(
    body: UpdateSolverCorporationByIdRequest,
    solver_corporation_id: str = Path(..., title="法人ソルバーID"),
    query_params: UpdateSolverCorporationByIdQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> OKResponse:
    """Put /solver-corporations/{solver_corporation_id} 法人ソルバー登録内容更新API

    Args:
        body (UpdateSolverCorporationByIdRequest): 更新内容
        solver_corporation_id (str): 法人ソルバーID パスパラメータで指定
        query_params (UpdateSolverCorporationByIdQuery): クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        OKResponse: 更新後の取得結果
    """
    return SolverCorporationService.update_solver_corporation_by_id(
        current_user, solver_corporation_id, query_params.version, item=body
    )


@router.delete(
    "/solver-corporations/{solver_corporation_id}",
    tags=["SolverCorporation"],
    description="指定した法人ソルバーを削除します。",
    status_code=status.HTTP_200_OK,
)
def delete_solver_corporation_by_id(
    solver_corporation_id: str = Path(..., title="法人ソルバーID"),
    query_params: DeleteSolverCorporationByIdQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
            }
        )
    ),
):
    return SolverCorporationService.delete_solver_corporation_by_id(
        current_user, solver_corporation_id, query_params.version
    )
