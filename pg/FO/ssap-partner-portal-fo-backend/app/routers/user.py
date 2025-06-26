from typing import Dict, List

from app.auth.auth import get_current_user_factory
from app.models.user import UserModel
from app.resources.const import UserRoleType
from app.schemas.user import (
    CreateUserRequest,
    CreateUserResponse,
    GetUserByIdResponse,
    GetUserByMineResponse,
    GetUsersResponse,
    GetUsersUserQuery,
    PatchUserByMineRequest,
    SuggestUsersQuery,
    SuggestUsersResponse,
    UpdateUserByIdQuery,
    UpdateUserByIdRequest,
    UpdateUserByIdResponse,
)
from app.service.user_service import UserService
from fastapi import APIRouter, Depends, Path, status

router = APIRouter()


@router.post(
    "/users",
    tags=["User"],
    description="Front Officeにサインインできる一般ユーザーを一件作成します。",
    response_model=CreateUserResponse,
)
def create_user(
    body: CreateUserRequest,
    current_user: UserModel = Depends(
        get_current_user_factory(
            {
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> CreateUserResponse:
    """Post /users ユーザー登録API

    ユーザーの種別は顧客のみ登録可

    Args:
        body (CreateUserRequest): 登録内容
        current_user (UserModel, optional): 認証済みのユーザー

    Returns:
        CreateUserResponse: 登録結果
    """
    return UserService.create_user(current_user=current_user, item=body)


@router.get(
    "/users",
    tags=["User"],
    description="Front Officeにサインインできる一般ユーザーの検索・一覧を取得します。",
    response_model=GetUsersResponse,
    status_code=status.HTTP_200_OK,
)
def get_users(
    query_params: GetUsersUserQuery = Depends(),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetUsersResponse:
    """Get /users 一般ユーザー一覧取得API

    Args:
        email (str): メールアドレス
        role (enum): ロール
        sort (str): ソート

    Returns:
        GetUsersResponse: 取得結果
    """

    return UserService.get_users(
        email=query_params.email,
        role=query_params.role,
        current_user=current_user,
    )


@router.get(
    "/users/me",
    tags=["User"],
    description="ログイン中の自身の一般ユーザー情報を取得します",
    response_model=GetUserByMineResponse,
)
def get_user_by_mine(
    current_user: UserModel = Depends(
        get_current_user_factory(
            {
                UserRoleType.CUSTOMER.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    )
) -> GetUserByMineResponse:
    """Get /users/me 自身の情報取得API
    Args:
        current_user (UserModel, optional): 認証済みユーザー

    Returns:
        GetUserByMineResponse: 取得結果
    """
    return UserService.get_user_by_mine(current_user)


@router.patch("/users/me", tags=["User"], description="ログイン中の一般ユーザー情報を更新します")
def patch_user_by_mine(
    body: PatchUserByMineRequest,
    current_user: UserModel = Depends(
        get_current_user_factory(
            {
                UserRoleType.CUSTOMER.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
                UserRoleType.APT.key,
                UserRoleType.SOLVER_STAFF.key,
            }
        )
    ),
) -> Dict:
    """Patch /users/me 自身の情報更新API
    利用規約同意時に利用

    Args:
        body (PatchUserByMineRequest): 更新内容
        current_user (UserModel, optional): 認証済みユーザー

    Returns:
        Dict: 更新結果を表すメッセージ
    """

    return UserService.patch_user_by_mine(current_user=current_user, item=body)


@router.get(
    "/users/suggest",
    tags=["User"],
    description="一般ユーザーのサジェスト用情報を取得します。 フロントでサジェストするためのリストを取得。",
    response_model=List[SuggestUsersResponse],
    status_code=status.HTTP_200_OK,
)
def suggest_users(
    query_params: SuggestUsersQuery = Depends(),
    current_user: UserModel = Depends(
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
) -> List[SuggestUsersResponse]:
    """Get /users/suggest 一般ユーザーサジェストAPI

    Args:
        role (enum): ロール（結果の絞り込み） クエリパラメータで指定
        disabled (bool): trueならば無効なユーザーを含む クエリパラメータで指定
        user_id (str): ソート クエリパラメータで指定

    Returns:
        SuggestUsersResponse: サジェスト結果
    """

    return UserService.suggest_users(
        role=query_params.role,
        disabled=query_params.disabled,
        sort=query_params.sort,
        current_user=current_user,
    )


@router.get(
    "/users/{user_id}",
    tags=["User"],
    description="一般ユーザーをIDで一意に取得します",
    response_model=GetUserByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_user_by_id(
    user_id: str = Path(...),
    current_user: UserModel = Depends(
        get_current_user_factory(
            {
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetUserByIdResponse:
    """Get /users/{user_id} 一般ユーザー詳細取得API

    Args:
        user_id (str): 一般ユーザーID クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetUserByIdResponse: 取得結果
    """
    return UserService.get_user_by_id(current_user, user_id)


@router.put(
    "/users/{user_id}",
    tags=["User"],
    description="一般ユーザーをIDで一意に更新します",
    response_model=UpdateUserByIdResponse,
    status_code=status.HTTP_200_OK,
)
def update_user_by_id(
    body: UpdateUserByIdRequest,
    query_params: UpdateUserByIdQuery = Depends(),
    user_id: str = Path(...),
    current_user: UserModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> UpdateUserByIdResponse:
    """Put /users/{user_id} 一般ユーザー登録内容更新API

    Args:
        user_id (str): 一般ユーザーID クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        UpdateUserByIdResponse: 更新後の取得結果
    """
    return UserService.update_user_by_id(
        current_user, query_params.version, user_id, item=body
    )
