import secrets
from datetime import datetime
from typing import List

import pyotp
from fastapi import HTTPException, status
from pynamodb.expressions.update import Action

from app.auth.session import Session
from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.resources.const import DataType, MailType
from app.schemas.auth import VerifyOTPResponse
from app.utils.aws.ses import SesHelper


class AuthService:
    @staticmethod
    def issue_otp(cognito_id: str, email: str):
        """OTPを発行し、ユーザーにメールでワンタイムパスワードを送信する。

        Args:
            cognito_id (str): cognitoユーザプールとの紐づけ
            email (str): メールアドレス

        Raises:
            HTTPException: 401

        Returns:
            dict: OTP送信成功のメッセージ

        """

        admin = AuthService.get_auth_user(cognito_id=cognito_id, email=email)

        # ユーザの有効/無効判定
        if admin.disabled:
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        # ワンタイムパスワード生成
        secret = pyotp.random_base32()

        update_action: List[Action] = []
        update_action = [
            AdminModel.otp_secret.set(secret),
            AdminModel.update_at.set(datetime.now()),
            AdminModel.update_id.set(admin.id),
        ]
        # メールアドレスが変更された場合
        if admin.email != email:
            update_action.append(AdminModel.email.set(email))

        admin.update(actions=update_action)

        # OTPの桁数と有効期限については環境ごとに設定
        totp = pyotp.TOTP(
            s=secret,
            digits=get_app_settings().totp_digits,
            interval=get_app_settings().totp_expiration_seconds,
        )
        otp = totp.now()

        SesHelper().send_mail(
            template_name=MailType.OTP_PASSWORD,
            to=[admin.email],
            cc=[],
            payload={"otp": otp},
        )

        return {"result": "One-time password was sent successfully."}

    @staticmethod
    def verify_otp(cognito_id: str, email: str, otp: str):
        """ワンタイムパスワードの検証を行い、JWTとは別のセッション管理に使用するトークンを発行します。

        Args:
            cognito_id (str): cognitoユーザプールとの紐づけ
            email (str): メールアドレス
            otp (str): ワンタイムパスワード

        Raises:
            HTTPException: 401

        Returns:
            VerifyOTPResponse: トークン情報
        """

        admin = AuthService.get_auth_user(cognito_id=cognito_id, email=email)

        secret = admin.otp_secret

        if not secret:
            raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED)

        # ユーザの有効/無効判定
        if admin.disabled:
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN, detail="Forbidden"
            )

        totp = pyotp.TOTP(
            secret,
            digits=get_app_settings().totp_digits,
            interval=get_app_settings().totp_expiration_seconds,
        )

        if totp.verify(otp) or (
            get_app_settings().env in ["local", "dev", "evs", "sqa"]
            and otp == "99999999"
        ):
            token = secrets.token_urlsafe(16)
            now = datetime.now()

            actions = [
                AdminModel.otp_verified_token.set(token),
                AdminModel.otp_verified_at.set(now),
                AdminModel.last_login_at.set(now),
                AdminModel.update_id.set(admin.id),
                AdminModel.update_at.set(now),
            ]
            if not admin.cognito_id:
                actions.append(AdminModel.cognito_id.set(cognito_id))

            admin.update(actions=actions)

            # セッション開始
            Session.create_session(cognito_id)

            return VerifyOTPResponse(user_id=admin.id, otp_verified_token=token)

        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED)

    @staticmethod
    def get_auth_user(cognito_id: str, email: str) -> AdminModel:
        try:
            admins = AdminModel.cognito_id_index.query(hash_key=cognito_id)
            admin: AdminModel = next(admins)
        except StopIteration:
            # 一般ユーザ情報の取得
            # flake8エラーになるため、bool型の変数を定義
            admin = None
            bool_false = False
            admin_filter_condition = AdminModel.disabled == bool_false
            admin_list: List[AdminModel] = list(AdminModel.scan(filter_condition=admin_filter_condition))

            for admin_info in admin_list:
                if admin_info.email.lower() == email.lower():
                    admin = admin_info
                    break

            if not admin:
                raise HTTPException(
                    status_code=status.HTTP_401_UNAUTHORIZED,
                    detail="Auth user not found.",
                )

        return admin
