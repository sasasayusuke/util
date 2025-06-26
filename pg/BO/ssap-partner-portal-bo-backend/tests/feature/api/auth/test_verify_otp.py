import pyotp
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.admin import AdminModel
from app.models.session import SessionModel
from app.resources.const import DataType, UserRoleType
from app.service.auth_service import AuthService

client = TestClient(app)


@mock_dynamodb
class TestVerifyOTP:
    def setup_method(self, method):
        AdminModel.create_table(
            read_capacity_units=1, write_capacity_units=1, wait=True
        )

    def teardown_method(self, method):
        AdminModel.delete_table()

    def test_otp_verified_ok(self, mocker, mock_auth_admin):
        # #######################################
        # テストデータ
        # #######################################
        admin = AdminModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.ADMIN,
            name="山田太郎",
            email="taro.yamada@example.com",
            job="部長",
            supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            organization_name="IST",
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            roles=["system_admin"],
            otp_verified_token="111111",
            otp_secret="FZYFQUGBUYLR3ISSI6ZH5JE3AMPFZH5R",
        )
        admin.save()

        # #######################################
        # モック化
        # #######################################
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock = mocker.patch.object(AuthService, "get_auth_user")
        mock.return_value = admin

        mocker.patch.object(AdminModel, "update")
        mocker.patch.object(SessionModel, "save")

        mock = mocker.patch.object(pyotp.TOTP, "verify")
        mock.return_value = True

        # #######################################
        # 検証
        # #######################################

        # レスポンス検証
        response = client.post("/api/auth/otp", json={"one_time_password": 99999999})

        assert response.status_code == 200

    def test_otp_verified_ok_upper(self, mocker, mock_auth_admin):
        # #######################################
        # テストデータ
        # #######################################
        admin = AdminModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.ADMIN,
            name="山田太郎",
            email="taro.yamada@Example.com",
            job="部長",
            supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            organization_name="IST",
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            roles=["system_admin"],
            otp_verified_token="111111",
            otp_secret="FZYFQUGBUYLR3ISSI6ZH5JE3AMPFZH5R",
        )
        admin.save()

        # #######################################
        # モック化
        # #######################################
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock = mocker.patch.object(AuthService, "get_auth_user")
        mock.return_value = admin

        mocker.patch.object(AdminModel, "update")
        mocker.patch.object(SessionModel, "save")

        mock = mocker.patch.object(pyotp.TOTP, "verify")
        mock.return_value = True

        # #######################################
        # 検証
        # #######################################

        # レスポンス検証
        response = client.post("/api/auth/otp", json={"one_time_password": 99999999})

        assert response.status_code == 200

    def test_otp_invalid(self, mocker, mock_auth_admin):
        # #######################################
        # テストデータ
        # #######################################
        admin = AdminModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.ADMIN,
            name="山田太郎",
            email="taro.yamada@example.com",
            job="部長",
            supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            organization_name="IST",
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            roles=["system_admin"],
            otp_verified_token="111111",
            otp_secret="ABCDEFGHIJKLMNOPQRSTUVWXXZ",
        )
        admin.save()

        # #######################################
        # モック化
        # #######################################
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mocker.patch.object(AdminModel, "update")
        mocker.patch.object(SessionModel, "save")

        mock = mocker.patch.object(pyotp.TOTP, "verify")
        mock.return_value = False

        # #######################################
        # 検証
        # #######################################

        response = client.post("/api/auth/otp", json={"one_time_password": 99999999})

        assert response.status_code == 401

    def test_not_otp_secret(self, mocker, mock_auth_admin):
        # #######################################
        # テストデータ
        # #######################################
        admin = AdminModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.ADMIN,
            name="山田太郎",
            email="taro.yamada@example.com",
            job="部長",
            supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            organization_name="IST",
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
            roles=["system_admin"],
            otp_verified_token="111111",
        )
        admin.save()

        # #######################################
        # モック化
        # #######################################
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock = mocker.patch.object(AuthService, "get_auth_user")
        mock.return_value = admin

        # #######################################
        # 検証
        # #######################################

        response = client.post("/api/auth/otp", json={"one_time_password": 99999999})

        assert response.status_code == 401
