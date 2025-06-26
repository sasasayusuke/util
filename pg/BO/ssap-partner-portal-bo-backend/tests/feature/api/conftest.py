from typing import List

import pytest

from app.auth.jwt import JWTAuthorizationCredentials, JWTBearer
from app.auth.session import Session
from app.models.admin import AdminModel
from app.resources.const import DataType


@pytest.fixture
def mock_auth_admin(mocker):
    """認証済み管理ユーザーのモック化を行う"""

    def _mock_auth_admin(roles: List):

        # #######################################
        # テスト用データ
        # #######################################
        admin = AdminModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.ADMIN,
            name="ソニー太郎",
            email="taro.sony@example.com",
            job="部長",
            supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            organization_name="IST",
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            roles=set(roles),
            otp_verified_token="111111",
        )

        # #######################################
        # モック作成
        # #######################################

        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin

        credentials = JWTAuthorizationCredentials(
            jwt_token="",
            header={},
            claims={
                "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                "cognito:username": "taro.yamada@example.com",
                "email": "taro.yamada@example.com",
            },
            signature="",
            message="",
        )
        mock = mocker.patch.object(JWTBearer, "__call__")
        mock.return_value = credentials

        mocker.patch.object(Session, "manage_session")

        return admin

    return _mock_auth_admin
