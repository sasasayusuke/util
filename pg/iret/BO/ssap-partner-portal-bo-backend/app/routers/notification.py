from typing import List

from fastapi import APIRouter, Depends, status

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.notification import (
    GetNotificationsByMineResponse,
    PatchCheckNotificationsResponse,
)
from app.service.notification_service import NotificationService

router = APIRouter()


@router.get(
    "/notifications/me",
    tags=["Notification"],
    description="自身の管理ユーザーIDに紐づくお知らせ情報を取得します。",
    response_model=List[GetNotificationsByMineResponse],
    status_code=status.HTTP_200_OK,
)
def get_notification_by_mine(
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
    )
) -> GetNotificationsByMineResponse:
    """Get /notifications/me お知らせ情報取得API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetNotificationsByMineResponse: 取得結果
    """
    return NotificationService.get_notification_by_me(current_user)


@router.patch(
    "/notifications",
    tags=["Notification"],
    description="お知らせ情報を既読に更新します。",
    response_model=PatchCheckNotificationsResponse,
    status_code=status.HTTP_200_OK,
)
def patch_check_notifications(
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
    )
) -> PatchCheckNotificationsResponse:
    """Patch /notifications お知らせ情報を既読更新API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        PatchCheckNotificationsResponse: 結果
    """
    return NotificationService.patch_check_notifications(current_user)
