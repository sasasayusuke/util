from typing import Set, List

from app.auth.jwt import JWTBearer
from app.core.common_logging import CustomLogger
from app.models.user import UserModel
from app.resources.const import DataType
from fastapi import Depends, HTTPException, Request, status
from fastapi.security import HTTPAuthorizationCredentials
from pydantic import BaseModel

logger = CustomLogger.get_logger()


class AuthUser(BaseModel):
    """認証済みユーザー情報"""

    id: str
    cognito_id: str
    email: str
    name: str
    role: str


def get_current_user_factory(accessible_roles: Set = None) -> UserModel:
    def get_current_user(
        request: Request,
        credentials: HTTPAuthorizationCredentials = Depends(JWTBearer()),
    ) -> UserModel:

        sub = credentials.claims["sub"]
        email = credentials.claims["email"]

        try:
            users = UserModel.cognito_id_index.query(hash_key=sub)
            user: UserModel = next(users)
        except StopIteration:
            # ここは初回ログイン時のみ入る
            # 一般ユーザ情報の取得
            # flake8エラーになるため、bool型の変数を定義
            user = None
            bool_false = False
            user_filter_condition = UserModel.disabled == bool_false
            user_list: List[UserModel] = list(UserModel.scan(filter_condition=user_filter_condition))

            for user_info in user_list:
                if user_info.email.lower() == email.lower():
                    user = user_info
                    break

            if not user:
                raise HTTPException(
                    status_code=status.HTTP_403_FORBIDDEN,
                    detail="Authenticated user not found.",
                )

        logger.info(f"api execution user: id={user.id}")

        # ユーザの有効/無効判定
        if user.disabled:
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # APIへのアクセス権判定
        if user.role not in accessible_roles:
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        return user

    return get_current_user
