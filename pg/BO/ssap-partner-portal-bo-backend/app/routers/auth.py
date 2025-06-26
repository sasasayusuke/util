from typing import Dict

from fastapi import APIRouter, Depends, HTTPException, status
from fastapi.security import HTTPAuthorizationCredentials

from app.auth.jwt import JWTBearer
from app.schemas.auth import VerifyOTPRequest, VerifyOTPResponse
from app.service.auth_service import AuthService

router = APIRouter()


@router.get(
    "/auth/otp",
    tags=["Authentication"],
    description="SSAP公式サイトから取得したJWTトークンからユーザ情報を取り出し、そのユーザに対してメールでワンタイムパスワードを送信します。",
)
def issue_otp(credentials: HTTPAuthorizationCredentials = Depends(JWTBearer())) -> Dict:
    """OTPを発行しユーザーに送信する

    Returns:
       dict: OTP送信成功のメッセージ
    """
    try:
        cognito_id = credentials.claims["sub"]
        email = credentials.claims["email"]
    except KeyError:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED)

    return AuthService.issue_otp(cognito_id=cognito_id, email=email)


@router.post(
    "/auth/otp",
    tags=["Authentication"],
    description="ユーザが入力したワンタイムパスワードの妥当性を検証します。\n\nユーザが入力したワンタイムパスワードの妥当性が正しい場合、ユーザに対してトークンを払い出します。 ここで払い出したトークンは実際の API 呼び出しのときに X-Otp-Verified-Token ヘッダに設定します。 サーバサイドではヘッダを検証することで、API 呼び出しを実行できるかどうかを判断します。",
    response_model=VerifyOTPResponse,
)
def verify_otp(
    body: VerifyOTPRequest,
    credentials: HTTPAuthorizationCredentials = Depends(JWTBearer()),
) -> VerifyOTPResponse:
    try:
        cognito_id = credentials.claims["sub"]
        email = credentials.claims["email"]
    except KeyError:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED)

    return AuthService.verify_otp(
        cognito_id=cognito_id, email=email, otp=body.one_time_password
    )
