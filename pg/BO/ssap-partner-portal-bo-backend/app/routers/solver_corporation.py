from typing import List

from fastapi import APIRouter, Depends, status

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.solver_corporation import (
    SuggestSolverCorporationsQuery,
    SuggestSolverCorporationsResponse,
)
from app.service.solver_corporation import SolversCorporationService

router = APIRouter()


@router.get(
    "/solvers/corporations/suggest",
    tags=["SolversCorporation"],
    description="法人名のサジェスト用情報を取得します。 フロントでサジェストするためのリストを取得。",
    response_model=List[SuggestSolverCorporationsResponse],
    status_code=status.HTTP_200_OK,
)
def suggest_solver_corporations(
    query_params: SuggestSolverCorporationsQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
            }
        )
    ),
) -> List[SuggestSolverCorporationsResponse]:
    """Get /solvers/corporations/suggest 法人名サジェスト用データ取得API

    Args:
        sort (SuggestSolversCorporationsSortType): ソート クエリパラメータで指定（'name:asc'）
        disabled (bool): trueならば無効なユーザーを含む クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        SuggestSolverCorporationsResponse: 取得結果
    """
    return SolversCorporationService.suggest_solver_corporations(
        sort=query_params.sort,
        disabled=query_params.disabled,
        current_user=current_user
    )
