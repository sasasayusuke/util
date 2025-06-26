import time
from functools import lru_cache
from typing import Any, Dict, List, Optional, Tuple

import requests
from fastapi import HTTPException, Request, status
from fastapi.security import HTTPAuthorizationCredentials, HTTPBearer
from jose import JWTError, jwk, jwt
from jose.utils import base64url_decode
from pydantic import BaseModel

from app.core.config import get_app_settings


class JWKS(BaseModel):
    """JWK の配列を表すオブジェクト"""

    keys: List[Dict[str, str]]


class JWTAuthorizationCredentials(BaseModel):
    """JWTの内容"""

    jwt_token: str
    header: Dict[str, Any]
    claims: Dict[str, Any]
    signature: str
    message: str


@lru_cache
def get_user_pool_jwks():
    """CognitoのUserPoolからJWKを取得して整形する

    Ref: https://docs.aws.amazon.com/ja_jp/cognito/latest/developerguide/amazon-cognito-user-pools-using-tokens-verifying-a-jwt.html

    Returns:
        Dict: 整形されたJWK
    """
    jwk_url_base = get_app_settings().jwk_url_base
    jwks = JWKS.parse_obj(
        requests.get(
            jwk_url_base.format(
                cognito_region=get_app_settings().aws_region,
                cognito_pool_id=get_app_settings().cognito_pool_id,
            )
        ).json()
    )
    return {jwk["kid"]: jwk for jwk in jwks.keys}


def get_authorization_scheme_param(authorization_header_value: str) -> Tuple[str, str]:
    """JWTを分割する

    Args:
        authorization_header_value (str): JWT

    Returns:
        Tuple[str, str]: 分割されたJWT
    """
    if not authorization_header_value:
        return "", ""
    scheme, _, param = authorization_header_value.partition(" ")
    return scheme, param


class JWTBearer(HTTPBearer):
    def __init__(self, auto_error: bool = True):
        super().__init__(auto_error=auto_error)

    def verify_jwk_token(self, jwt_credentials: JWTAuthorizationCredentials) -> bool:
        """JWK Tokenを検証する

        Ref: https://python-jose.readthedocs.io/en/latest/jwk/index.html#examples

        Args:
            jwt_credentials (JWTAuthorizationCredentials): JWTの内容

        Raises:
            HTTPException: 401

        Returns:
            bool: 検証結果
        """
        try:
            public_key = get_user_pool_jwks()[jwt_credentials.header["kid"]]
        except KeyError:
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="JWK public key not found",
            )
        key = jwk.construct(public_key)
        decode_signature = base64url_decode(jwt_credentials.signature.encode())

        return key.verify(jwt_credentials.message.encode(), decode_signature)

    def __call__(self, request: Request) -> Optional[HTTPAuthorizationCredentials]:
        """インスタンス作成時に呼ばれるメソッド

        認証系のレスポンスステータスコードを401で合わせたかったため親クラスをオーバーライド

        Args:
            request (Request): _description_

        Raises:
            HTTPException: 401

        Returns:
            Optional[HTTPAuthorizationCredentials]: 認証済みのトークン情報
        """

        authorization: str = request.headers.get("Authorization")
        scheme, jwt_token = get_authorization_scheme_param(authorization)

        # トークンの形式チェック
        if not (authorization and scheme and jwt_token):
            if self.auto_error:
                raise HTTPException(
                    status_code=status.HTTP_401_UNAUTHORIZED, detail="Not authenticated"
                )
            else:
                return None
        if scheme.lower() != "bearer":
            if self.auto_error:
                raise HTTPException(
                    status_code=status.HTTP_401_UNAUTHORIZED,
                    detail="Invalid authentication credentials",
                )
            else:
                return None

        message, signature = jwt_token.rsplit(".", 1)
        try:
            # 各項目デコード
            header = jwt.get_unverified_header(jwt_token)
            claims = jwt.get_unverified_claims(jwt_token)

            jwt_credentials = JWTAuthorizationCredentials(
                jwt_token=jwt_token,
                header=header,
                claims=claims,
                signature=signature,
                message=message,
            )

        except JWTError:
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED, detail="JWK invalid"
            )

        if not self.verify_jwk_token(jwt_credentials):
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED, detail="JWK invalid"
            )

        # ローカル環境でのみトークンの有効期限チェックを入れいない
        if not get_app_settings().enable_token_expiration:
            return jwt_credentials

        # 有効期限チェック
        exp = jwt_credentials.claims["exp"]
        now = time.time()
        if exp < int(now):
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="Token has been expired",
            )

        return jwt_credentials
