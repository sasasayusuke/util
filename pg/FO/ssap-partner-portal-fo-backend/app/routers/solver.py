from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.models.user import UserModel
from app.resources.const import UserRoleType
from app.schemas.base import OKResponse
from app.schemas.solver import (
    CreateSolverRequest,
    DeleteSolverByIdQuery,
    GetSolverByIdResponse,
    GetSolversQuery,
    GetSolversResponse,
    PatchSolverStatusByIdQuery,
    UpdateSolverByIdQuery,
    UpdateSolverByIdRequest,
    UpdateSolverUtilizationRateQuery,
    UpdateSolverUtilizationRateRequest,
    UpdateSolverUtilizationRateResponse,
)
from app.service.solver_service import SolverService

router = APIRouter()


@router.post(
    "/solvers",
    tags=["Solver"],
    description="1人もしくは複数人のソルバーを作成します。",
    status_code=status.HTTP_200_OK,
)
def create_solver(
    body: CreateSolverRequest,
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> OKResponse:
    """Post /solvers 新規ソルバー候補申請・新規個人ソルバー登録申請API

    Args:
        body (CreateSolverRequest): 登録内容
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        OKResponse: 登録結果
    """
    return SolverService.create_solver(
        items=body,
        current_user=current_user,
    )


@router.get(
    "/solvers",
    tags=["Solver"],
    description="1つの法人に紐づくソルバーの一覧を取得します。",
    response_model=GetSolversResponse,
    status_code=status.HTTP_200_OK,
)
def get_solvers(
    query_params: GetSolversQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> GetSolversResponse:
    """Get /solvers 個人ソルバー一覧取得API

    Args:
        query_params (GetSolversQuery): クエリパラメータ
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSolversResponse: 取得結果
    """
    return SolverService.get_solvers(
        query_params=query_params,
        current_user=current_user,
    )


@router.get(
    "/solvers/{solver_id}",
    tags=["Solver"],
    description="ソルバーをIDで一意に取得します。",
    response_model=GetSolverByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_solver_by_id(
    solver_id: str = Path(..., title="個人ソルバーID"),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> GetSolverByIdResponse:
    """Get /solvers/{solver_id} ソルバー情報取得API

    Args:
        solver_id (str): 個人ソルバーID パスパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSolverByIdResponse: 取得結果
    """
    return SolverService.get_solver_by_id(current_user, solver_id)


@router.put(
    "/solvers/utilization-rate/{solver_corporation_id}",
    tags=["Solver"],
    description="1つの法人に紐づくソルバーの稼働率と単価を一括更新します。",
    response_model=UpdateSolverUtilizationRateResponse,
    status_code=status.HTTP_200_OK,
)
def update_solver_utilization_rate(
    body: UpdateSolverUtilizationRateRequest,
    solver_corporation_id: str = Path(..., title="法人ソルバーID"),
    query_params: UpdateSolverUtilizationRateQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> UpdateSolverUtilizationRateResponse:
    return SolverService.update_solver_utilization_rate(
        item=body, solver_corporation_id=solver_corporation_id, version=query_params.version, current_user=current_user
    )


@router.put(
    "/solvers/{solver_id}",
    tags=["Solver"],
    description="ソルバーをIDで一意に更新します。",
    status_code=status.HTTP_200_OK,
)
def update_solver_by_id(
    body: UpdateSolverByIdRequest,
    solver_id: str = Path(..., title="個人ソルバーID"),
    query_params: UpdateSolverByIdQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> OKResponse:
    """Put /solvers/{solver_id} ソルバー情報更新API

    Args:
        body (UpdateSolverByIdRequest): 更新内容
        solver_id (str): 個人ソルバーID パスパラメータで指定
        query_params (UpdateSolverByIdQuery): クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        OKResponse: 更新後の結果
    """
    return SolverService.update_solver_by_id(
        current_user,
        solver_id,
        query_params.version,
        item=body
    )


@router.delete(
    "/solvers/{solver_id}",
    tags=["Solver"],
    description="指定したソルバーを削除します。",
    status_code=status.HTTP_200_OK,
)
def delete_solver_by_id(
    solver_id: str = Path(..., title="個人ソルバーID"),
    query_params: DeleteSolverByIdQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
):
    return SolverService.delete_solver_by_id(current_user, solver_id, query_params)


@router.patch(
    "/solvers/{solver_id}",
    tags=["Solver"],
    description="ソルバー候補から個人ソルバーに変更します。",
    status_code=status.HTTP_200_OK,
)
def patch_solver_status_by_id(
    query_params: PatchSolverStatusByIdQuery = Depends(),
    solver_id: str = Path(..., title="個人ソルバーID"),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> OKResponse:
    """Patch /solvers/{solver_id} ソルバー認定/非認定更新API

    Args:
        query_params (PatchSolverStatusByIdQuery): 更新内容
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        OKResponse: 結果
    """

    return SolverService.patch_solver_status_by_id(
        current_user=current_user, item=query_params, solver_id=solver_id
    )
