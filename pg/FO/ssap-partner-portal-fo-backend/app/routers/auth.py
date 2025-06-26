from datetime import datetime
from typing import Dict, List

from app.auth.jwt import JWTAuthorizationCredentials, JWTBearer
from app.models.user import UserModel
from app.resources.const import DataType
from fastapi import APIRouter, Depends, HTTPException, status

router = APIRouter()


@router.post(
    "/auth/login",
    tags=["Authentication"],
    description="SSAP公式サイトから取得したJWTトークンの妥当性チェック及びトークンの情報をもとにユーザー情報の最新化を行います。",
)
def auth_login(credentials: JWTAuthorizationCredentials = Depends(JWTBearer())) -> Dict:
    """JWTの妥当性チェックとユーザー情報の最新化を行います。

    Args:
        credentials (JWTAuthorizationCredentials): JWT情報

    Raises:
        HTTPException: 401

    Returns:
        Dict: 成功を示すメッセージ
    """

    try:
        # JWTからユーザー情報取得
        sub = credentials.claims["sub"]
        email = credentials.claims["email"]
    except KeyError:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED, detail="Unauthorized"
        )

    try:
        users = UserModel.cognito_id_index.query(hash_key=sub)
        user: UserModel = next(users)

        # メールアドレスが変更された場合
        if user.email != email:
            user.update(actions=[UserModel.email.set(email)])

    except StopIteration:
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

        if user:
            user.update(actions=[UserModel.cognito_id.set(sub)])
        else:
            # DBに登録されていない場合
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN,
                detail="Authenticated user not found.",
            )

    # ユーザの有効/無効判定
    if user.disabled:
        raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden")

    now = datetime.now()
    actions = [
        UserModel.last_login_at.set(now),
        UserModel.update_id.set(user.id),
        UserModel.update_at.set(now),
    ]
    user.update(actions=actions)

    return {"message": "OK"}
