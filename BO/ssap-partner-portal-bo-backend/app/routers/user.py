from typing import List

from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.user import (
    CreateUserRequest,
    CreateUserResponse,
    GetUserByIdResponse,
    GetUsersResponse,
    GetUsersUserQuery,
    PatchUserStatusByIdResponse,
    SuggestUsersQuery,
    SuggestUsersResponse,
    UpdateUserByIdQuery,
    UpdateUserByIdRequest,
    UpdateUserByIdResponse,
)
from app.service.user_service import UserService

router = APIRouter()


@router.post(
    "/users",
    tags=["User"],
    description="一般ユーザーを1件作成します",
    response_model=CreateUserResponse,
    status_code=status.HTTP_200_OK,
)
def create_user(
    body: CreateUserRequest,
    current_user=Depends(
        get_current_user_factory(accessible_roles={UserRoleType.SYSTEM_ADMIN.key})
    ),
) -> CreateUserResponse:

    return UserService.create_user(
        body=body,
        current_user=current_user,
    )


@router.get(
    "/users",
    tags=["User"],
    description="Front Officeにサインインできる一般ユーザーの検索・一覧を取得します。",
    response_model=GetUsersResponse,
    status_code=status.HTTP_200_OK,
)
def get_users(
    query_params: GetUsersUserQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetUsersResponse:
    """Get /users 一般ユーザー一覧取得API

    Args:
        email (str): メールアドレス
        name (str): 氏名
        role (enum): ロール
        sort (str): ソート
        offset_page (int): リストの中で何ページ目を取得するか
        limit (int): 最大取得件数（省略時は限度なし）

    Raises:
            HTTPException: 404

    Returns:
        GetUsersResponse: 取得結果
    """

    return UserService.get_users(
        email=query_params.email,
        name=query_params.name,
        role=query_params.role,
        sort=query_params.sort,
        offset_page=query_params.offset_page,
        limit=query_params.limit,
        current_user=current_user,
    )


@router.get(
    "/users/suggest",
    tags=["User"],
    description="一般ユーザーのサジェスト用情報を取得します。 フロントでサジェストするためのリストを取得。",
    response_model=List[SuggestUsersResponse],
    status_code=status.HTTP_200_OK,
)
def suggest_users(
    query_params: SuggestUsersQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
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
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
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

    return UserService.get_user_by_id(user_id=user_id, current_user=current_user)


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
    current_user=Depends(
        get_current_user_factory(accessible_roles={UserRoleType.SYSTEM_ADMIN.key})
    ),
) -> UpdateUserByIdResponse:
    """Put /users/{user_id} 一般ユーザー登録内容更新API

    Args:
        user_id (str): 一般ユーザーID クエリパラメータで指定
        body (UpdateUserByIdRequest): リクエストボディ
        version (int): 楽観ロック制御
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        UpdateUserByIdResponse: 更新後の取得結果
    """

    return UserService.update_user_by_id(
        version=query_params.version,
        body=body,
        user_id=user_id,
        current_user=current_user,
    )


@router.patch(
    "/users/{user_id}",
    tags=["User"],
    description="一般ユーザーをIDで一意に有効化/無効化します。 物理削除は行わずdisableの値を制御します。",
    response_model=PatchUserStatusByIdResponse,
    status_code=status.HTTP_200_OK,
)
def patch_user_status_by_id(
    version: int,
    enable: bool,
    user_id: str = Path(...),
    current_user=Depends(
        get_current_user_factory(accessible_roles={UserRoleType.SYSTEM_ADMIN.key})
    ),
) -> PatchUserStatusByIdResponse:
    """Patch /users/{user_id} 一般ユーザー有効化/無効化API

    Args:
        version (int): ロックキー（楽観ロック制御） クエリパラメータで指定
        enable (bool): 有効化 クエリパラメータで指定
        user_id (str): 一般ユーザーID クエリパラメータで指定

    Returns:
        PatchUserByIdResponse: 有効化/無効化後の取得結果
    """

    return UserService.patch_user_status_by_id(
        version=version, enable=enable, user_id=user_id, current_user=current_user
    )
