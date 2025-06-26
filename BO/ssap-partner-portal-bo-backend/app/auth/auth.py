from typing import Set

from fastapi import Depends, Header, HTTPException, Request
from fastapi.security import HTTPAuthorizationCredentials
from starlette.status import HTTP_401_UNAUTHORIZED, HTTP_403_FORBIDDEN

from app.auth.jwt import JWTBearer
from app.auth.session import Session
from app.core.common_logging import CustomLogger
from app.models.admin import AdminModel

logger = CustomLogger.get_logger()


def is_accessible_method(
    current_user_roles: Set[str], accessible_roles: Set[str]
) -> bool:
    """メソッド（API）に対してアクセスできるかを判定

    Args:
        current_user_roles (Set[str]): メソッド（API）を呼び出したユーザーのロール
        accessible_roles (Set[str]): メソッド（API）にアクセスできるロール

    Returns:
        bool: 判定結果
    """
    for role in current_user_roles:
        if role in accessible_roles:
            return True
    return False


def get_current_user_factory(accessible_roles: Set = None) -> AdminModel:
    def get_current_user(
        request: Request,
        credentials: HTTPAuthorizationCredentials = Depends(JWTBearer()),
        x_otp_verified_token: str = Header(default=None),
    ) -> AdminModel:

        try:
            sub = credentials.claims["sub"]

            if x_otp_verified_token:
                admin: AdminModel = AdminModel.get_admin_by_cognito_id(sub)
                logger.info(f"api execution user: id={admin.id}")

                if x_otp_verified_token != admin.otp_verified_token:
                    raise HTTPException(status_code=HTTP_401_UNAUTHORIZED)

                Session.manage_session(
                    cognito_id=sub, method=request.method, path=request.url.path
                )

            else:
                raise HTTPException(status_code=HTTP_401_UNAUTHORIZED)

        except KeyError:
            raise HTTPException(status_code=HTTP_401_UNAUTHORIZED)

        if Session.is_request_to_manage_session(request.method, request.url.path):
            # ユーザの有効/無効判定
            if admin.disabled:
                raise HTTPException(status_code=HTTP_403_FORBIDDEN, detail="Forbidden")
            # APIへのアクセス権判定
            if not is_accessible_method(
                current_user_roles=admin.roles, accessible_roles=accessible_roles
            ):
                raise HTTPException(status_code=HTTP_403_FORBIDDEN, detail="Forbidden")

        return admin

    return get_current_user
