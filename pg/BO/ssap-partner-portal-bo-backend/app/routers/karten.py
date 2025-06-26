from typing import List

from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.karten import GetKartenByIdResponse, GetKartenQuery, GetKartenResponse
from app.service.karten_service import KartenService

router = APIRouter()


@router.get(
    "/karten",
    tags=["Karten"],
    description="案件カルテの一覧を取得します。",
    response_model=List[GetKartenResponse],
    status_code=status.HTTP_200_OK,
)
def get_karten(
    query_params: GetKartenQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetKartenResponse:
    """Get /karten 案件カルテの一覧取得API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetKartenResponse: 取得結果
    """
    return KartenService.get_karten(
        query_params=query_params, current_user=current_user
    )


@router.get(
    "/karten/{id}",
    tags=["Karten"],
    description="案件カルテをIDで取得します。",
    response_model=GetKartenByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_karten_by_id(
    id: str = Path(..., title="カルテID"),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetKartenByIdResponse:
    """Get /karten 案件カルテの一覧取得API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetKartenResponse: 取得結果
    """
    return KartenService.get_karten_by_id(karte_id=id, current_user=current_user)
