from typing import List

from app.auth.auth import get_current_user_factory
from app.auth.jwt import JWTBearer
from app.resources.const import UserRoleType
from app.schemas.karten import (
    GetKartenByIdResponse,
    GetKartenLatestResponse,
    GetKartenQuery,
    GetKartenResponse,
    UpdateKarteByIdQuery,
    UpdateKarteByIdRequest,
    UpdateKarteByIdResponse
)
from app.service.karten_service import KartenService
from fastapi import APIRouter, Depends, Path, status
from fastapi.security import HTTPAuthorizationCredentials

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
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.CUSTOMER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    )
) -> GetKartenResponse:
    """Get /karten 案件カルテの一覧取得API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetKartenResponse: 取得結果
    """
    return KartenService.get_karten(
        query_params=query_params,
        current_user=current_user)


@router.get(
    "/karten/latest",
    tags=["Karten"],
    description="最近更新されたカルテ一覧を取得します。",
    response_model=List[GetKartenLatestResponse],
    status_code=status.HTTP_200_OK,
)
def get_karten_latest(
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.CUSTOMER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    )
) -> GetKartenLatestResponse:
    """Get /karten/latest 最近更新されたカルテ一覧取得API
        ・顧客の場合は自分の取引先に紐づく直近カルテ
        ・顧客以外の場合は当日週から前後一週間に存在する自身のアサイン案件のカルテを表示する
        ・最大5件取得

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetKartenLatestResponse: 取得結果
    """
    return KartenService.get_karten_latest(current_user=current_user)


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
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.CUSTOMER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    )
) -> GetKartenByIdResponse:
    """Get /karten 案件カルテの一覧取得API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetKartenResponse: 取得結果
    """
    return KartenService.get_karten_by_id(
        karte_id=id,
        current_user=current_user)


@router.put(
    "/karten/{id}",
    tags=["Karten"],
    description="案件カルテをIDで更新します。",
    response_model=UpdateKarteByIdResponse,
    status_code=status.HTTP_200_OK,
)
def update_karte_by_id(
    body: UpdateKarteByIdRequest,
    query_params: UpdateKarteByIdQuery = Depends(),
    id: str = Path(..., title="カルテID"),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
    authorization: HTTPAuthorizationCredentials = Depends(JWTBearer()),
) -> UpdateKarteByIdResponse:
    """Get /karten 案件カルテの更新API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー
        version(int): ロックキー(楽観ロック制御) クエリパラメータで指定
        id(str): カルテID パスパラメータで指定
        authorization(HTTPAuthorizationCredentials)

    Returns:
        GetKartenResponse: 更新結果
    """
    return KartenService.update_karte_by_id(
        karte_id=id,
        item=body,
        query_params=query_params,
        current_user=current_user,
        authorization=authorization,
    )
