from app.auth.jwt import JWTAuthorizationCredentials, JWTBearer
from app.main import app
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from fastapi.testclient import TestClient
from moto import mock_dynamodb

client = TestClient(app)


@mock_dynamodb
class TestAuthLogin:
    def setup_method(self, method):
        UserModel.create_table(read_capacity_units=1, write_capacity_units=1, wait=True)

    def teardown_method(self, method):
        UserModel.delete_table()

    def test_ok(self, mocker):
        """通常ログイン"""

        # #######################################
        # テストデータ
        #########################################
        user = UserModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.USER,
            name="山田太郎",
            email="taro.yamada@example.com",
            role=UserRoleType.SYSTEM_ADMIN.key,
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        )
        user.save()

        # #######################################
        # モック作成
        # #######################################

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

        # #######################################
        # 検証
        # #######################################

        response = client.post("/api/auth/login")
        actual = response.json()

        assert response.status_code == 200
        assert actual["message"] == "OK"

        actual_user = UserModel.get(
            hash_key="906a3144-9650-4a34-8a23-3b02f3b9aeac", range_key=DataType.USER
        )
        assert credentials.claims["sub"] == actual_user.cognito_id

    def test_ok_upper(self, mocker):
        """通常ログイン"""

        # #######################################
        # テストデータ
        #########################################
        user = UserModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.USER,
            name="山田太郎",
            email="taro.yamada@example.com",
            role=UserRoleType.SYSTEM_ADMIN.key,
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        )
        user.save()

        # #######################################
        # モック作成
        # #######################################

        credentials = JWTAuthorizationCredentials(
            jwt_token="",
            header={},
            claims={
                "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                "cognito:username": "taro.yamada@Example.com",
                "email": "taro.yamada@Example.com",
            },
            signature="",
            message="",
        )
        mock = mocker.patch.object(JWTBearer, "__call__")
        mock.return_value = credentials

        # #######################################
        # 検証
        # #######################################

        response = client.post("/api/auth/login")
        actual = response.json()

        assert response.status_code == 200
        assert actual["message"] == "OK"

        actual_user = UserModel.get(
            hash_key="906a3144-9650-4a34-8a23-3b02f3b9aeac", range_key=DataType.USER
        )
        assert credentials.claims["sub"] == actual_user.cognito_id

    def test_fist_login(self, mocker):
        """初回ログイン時のテスト"""
        # #######################################
        # テストデータ
        #########################################
        user = UserModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.USER,
            name="山田太郎",
            email="taro.yamada@example.com",
            role=UserRoleType.SYSTEM_ADMIN.key,
        )
        user.save()

        # #######################################
        # モック作成
        # #######################################

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

        # #######################################
        # 検証
        # #######################################

        response = client.post("/api/auth/login")
        actual = response.json()

        # レスポンス検証
        assert response.status_code == 200
        assert actual["message"] == "OK"

        # データ検証
        actual_user = UserModel.get(
            hash_key="906a3144-9650-4a34-8a23-3b02f3b9aeac", range_key=DataType.USER
        )
        assert credentials.claims["sub"] == actual_user.cognito_id

    def test_email_updated(self, mocker):
        """メールアドレスが変更されていた時のテスト"""
        # #######################################
        # テストデータ
        #########################################
        user = UserModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.USER,
            name="山田太郎",
            email="taro.yamada@example.com",
            role=UserRoleType.SYSTEM_ADMIN.key,
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        )
        user.save()

        # #######################################
        # モック作成
        # #######################################

        credentials = JWTAuthorizationCredentials(
            jwt_token="",
            header={},
            claims={
                "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                "cognito:username": "taro.yamada@example.com",
                "email": "taro.yamadasan@example.com",
            },
            signature="",
            message="",
        )
        mock = mocker.patch.object(JWTBearer, "__call__")
        mock.return_value = credentials

        # #######################################
        # 検証
        # #######################################

        # リクエスト検証
        response = client.post("/api/auth/login")
        actual = response.json()

        assert response.status_code == 200
        assert actual["message"] == "OK"

        # データ検証
        actual_user = UserModel.get(
            hash_key="906a3144-9650-4a34-8a23-3b02f3b9aeac", range_key=DataType.USER
        )
        assert actual_user.email == credentials.claims["email"]

    def test_jwt_defects(self, mocker):
        """JWTからユーザー情報が取れない時のテスト"""
        # #######################################
        # テストデータ
        #########################################
        user = UserModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.USER,
            name="山田太郎",
            email="taro.yamada@example.com",
            role=UserRoleType.SYSTEM_ADMIN.key,
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        )
        user.save()

        # #######################################
        # モック作成
        # #######################################

        credentials = JWTAuthorizationCredentials(
            jwt_token="",
            header={},
            claims={
                "sub": "itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                "cognito:username": "taro.yamada@example.com",
            },
            signature="",
            message="",
        )
        mock = mocker.patch.object(JWTBearer, "__call__")
        mock.return_value = credentials

        # #######################################
        # 検証
        # #######################################

        response = client.post("/api/auth/login")

        assert response.status_code == 401

    def test_not_found_user(self, mocker):
        """ユーザーがDBに登録されていない時のテスト"""
        # #######################################
        # モック作成
        # #######################################

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

        # #######################################
        # 検証
        # #######################################

        response = client.post("/api/auth/login")

        assert response.status_code == 403
