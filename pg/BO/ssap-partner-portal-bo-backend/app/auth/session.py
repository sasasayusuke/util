from datetime import datetime, timedelta
from typing import List

from fastapi import HTTPException
from pytz import timezone
from starlette.status import HTTP_401_UNAUTHORIZED

from app.core.config import get_app_settings
from app.models.session import SessionModel


class Session:
    # セッション管理対象外のメソッド及びパス
    NO_MANAGED_SESSION_PATHS: List = [
        ("GET", "/auth/otp"),
        ("POST", "/auth/otp"),
        ("GET", "/misc/health-check"),
    ]

    @staticmethod
    def manage_session(cognito_id: str, method: str, path: str):
        """セッションの管理を行います

        Raises:
            HTTPException: 401
        """
        if not Session.is_request_to_manage_session(method=method, path=path):
            return

        if Session.check_session_validity(cognito_id):
            Session.extend_session(cognito_id)
        else:
            raise HTTPException(status_code=HTTP_401_UNAUTHORIZED)

    @staticmethod
    def create_session(cognito_id: str):
        """新規にセッションを開始します

        Args:
            cognito_id (str): ログインユーザの Cognito ID
        """
        now = datetime.now()
        one_week_later = now + timedelta(days=7)
        session = SessionModel(
            cognito_id=cognito_id,
            latest_access_at=now,
            ttl=one_week_later.timestamp(),
        )
        session.save()

    @staticmethod
    def is_request_to_manage_session(method: str, path: str) -> bool:
        """特定のメソッドおよびパスがセッション管理対象かどうかをチェックします

        Args:
            method (str): HTTP リクエストのメソッド
            path (str): HTTP リクエストのパス

        Returns:
            bool: 引数で渡されたメソッドおよびパスがセッション管理対象であるときは True を、管理対象でないときは False を返します
        """
        return (method, path) not in Session.NO_MANAGED_SESSION_PATHS

    @staticmethod
    def check_session_validity(cognito_id: str) -> bool:
        """セッションが有効か無効かを判定します

        セッションがタイムアウトする日時と現在日時とを比較して、対象ユーザのセッションの有効性をチェックします。

        Args:
            cognito_id (str): ログインユーザの Cognito ID

        Returns:
            bool: セッションが有効であるときは True を、無効であるときは False を返します
        """
        session_model = SessionModel.get(hash_key=cognito_id)
        now = datetime.now(timezone("Asia/Tokyo"))
        delta: timedelta = now - session_model.latest_access_at

        return delta.seconds <= get_app_settings().session_expiration_seconds

    @staticmethod
    def extend_session(cognito_id: str):
        """セッションの有効期限を延長します

        内部処理としてはcreate_sessionと同じ

        Args:
            cognito_id (str): ログインユーザーの Cognito ID
        """

        Session.create_session(cognito_id)
