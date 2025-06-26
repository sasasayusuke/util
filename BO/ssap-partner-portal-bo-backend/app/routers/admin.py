from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.admin import (
    CreateAdminRequest,
    CreateAdminResponse,
    GetAdminByIdResponse,
    GetAdminByMineResponse,
    GetAdminsQuery,
    GetAdminsResponse,
    PatchAdminStatusByIdQuery,
    PatchAdminStatusByIdResponse,
    UpdateAdminByIdQuery,
    UpdateAdminByIdRequest,
    UpdateAdminByIdResponse,
)
from app.service.admin_service import AdminService

router = APIRouter()


@router.post(
    "/admins",
    tags=["Admin"],
    description="管理ユーザーを1件作成します",
    response_model=CreateAdminResponse,
    status_code=status.HTTP_200_OK,
)
def create_admin(
    body: CreateAdminRequest,
    current_user=Depends(
        get_current_user_factory(accessible_roles={UserRoleType.SYSTEM_ADMIN.key})
    ),
) -> CreateAdminResponse:
    """Post /admins 管理ユーザー登録API

    Args:
        body (CreateAdminRequest): 登録内容
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        CreateAdminResponse: 登録結果
    """
    return AdminService.create_admin(item=body, current_user=current_user)


@router.get(
    "/admins",
    tags=["Admin"],
    description="Back Officeにログイン可能な管理ユーザーの検索・一覧を取得します。",
    response_model=GetAdminsResponse,
    status_code=status.HTTP_200_OK,
)
def get_admins(
    query_params: GetAdminsQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(accessible_roles={UserRoleType.SYSTEM_ADMIN.key})
    ),
) -> GetAdminsResponse:
    """Get /admins 管理者ユーザー一覧取得API

    Args:
        email (str): メールアドレス
        name (str): 氏名
        sort (str): ソート

    Returns:
        GetAdminsResponse: 取得結果
    """

    return AdminService.get_admins(
        email=query_params.email,
        name=query_params.name,
        sort=query_params.sort,
        current_user=current_user,
    )


@router.get(
    "/admins/me",
    tags=["Admin"],
    description="ログイン中の自身の管理ユーザー情報を取得します。",
    response_model=GetAdminByMineResponse,
    status_code=status.HTTP_200_OK,
)
def get_admin_by_mine(
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
) -> GetAdminByMineResponse:
    """Get /admins/me ログイン中管理者ユーザー情報取得API

    Returns:
        GetAdminByMineResponse: 取得結果
    """
    return AdminService.get_admin_by_mine(current_user)


@router.get(
    "/admins/{admin_id}",
    tags=["Admin"],
    description="管理ユーザーをIDで一意に取得します。",
    response_model=GetAdminByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_admin_by_id(
    admin_id: str = Path(
        ..., title="管理ユーザID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(accessible_roles={UserRoleType.SYSTEM_ADMIN.key})
    ),
) -> GetAdminByIdResponse:
    """Get /admins/{admin_id} 管理ユーザー詳細取得API

    Args:
        admin_id (str): 管理ユーザーID クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetAdminByIdResponse: 取得結果
    """
    return AdminService.get_admin_by_id(admin_id=admin_id, current_user=current_user)


@router.put(
    "/admins/{admin_id}",
    tags=["Admin"],
    description="管理ユーザーをIDで一意に更新します。",
    response_model=UpdateAdminByIdResponse,
    status_code=status.HTTP_200_OK,
)
def update_admin_by_id(
    body: UpdateAdminByIdRequest,
    query_params: UpdateAdminByIdQuery = Depends(),
    admin_id: str = Path(
        ..., title="管理ユーザID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(accessible_roles={UserRoleType.SYSTEM_ADMIN.key})
    ),
) -> UpdateAdminByIdResponse:
    """Put /admins/{admin_id} 管理ユーザー詳細更新API

    Args:
        version (int): ロックキー（楽観ロック制御）
        body (UpdateAdminByIdRequest): 更新内容
        admin_id (str): 管理ユーザーID クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        UpdateAdminByIdResponse: 更新結果
    """

    return AdminService.update_admin_by_id(
        admin_id=admin_id,
        current_user=current_user,
        version=query_params.version,
        item=body,
    )


@router.patch(
    "/admins/{admin_id}",
    tags=["Admin"],
    description="管理ユーザーをIDで一意に有効化/無効化します。 物理削除は行わずdisableの値を制御します。",
    response_model=PatchAdminStatusByIdResponse,
    status_code=status.HTTP_200_OK,
)
def patch_admin_status_by_id(
    query_params: PatchAdminStatusByIdQuery = Depends(),
    admin_id: str = Path(
        ..., title="管理ユーザID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    ),
    current_user=Depends(
        get_current_user_factory(accessible_roles={UserRoleType.SYSTEM_ADMIN.key})
    ),
) -> PatchAdminStatusByIdResponse:
    """Patch /admins/{admin_id} 管理ユーザー削除API

    Args:
        version (int): ロックキー（楽観ロック制御）
        enable (bool): 有効化 クエリパラメータで指定
        admin_id (str): 管理ユーザーID クエリパラメータで指定
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        PatchAdminStatusByIdResponse: 有効化/無効化後の取得結果
    """

    return AdminService.patch_admin_status_by_id(
        version=query_params.version,
        enable=query_params.enable,
        admin_id=admin_id,
        current_user=current_user,
    )
