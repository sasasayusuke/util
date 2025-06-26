from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.admin import AdminModel, CognitoIdIndex
from app.resources.const import DataType, UserRoleType
from app.utils.aws.ses import SesHelper

# https://github.com/pynamodb/PynamoDB/issues/569


client = TestClient(app)


@mock_dynamodb
class TestIssueOTP:
    def setup_method(self, method):
        AdminModel.create_table(
            read_capacity_units=1, write_capacity_units=1, wait=True
        )

    def teardown_method(self, method):
        AdminModel.delete_table()

    def test_issue_otp_not_found_user(self, mocker, mock_auth_admin):
        # #######################################
        # モック化
        # #######################################
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(CognitoIdIndex, "query")
        mock.return_value = iter([])

        response = client.get("/api/auth/otp")
        actual = response.json()
        assert response.status_code == 401
        assert actual["detail"] == "Auth user not found."

    def test_issue_otp(self, mocker, mock_auth_admin):
        # #######################################
        # モック化
        # #######################################
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mocker.patch.object(SesHelper, "send_mail")

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
        # 検証
        # #######################################

        # レスポンス検証
        response = client.get("/api/auth/otp")
        actual = response.json()

        assert response.status_code == 200
        assert actual["result"] == "One-time password was sent successfully."

        # データを検証
        actual_admin = AdminModel.get(
            "906a3144-9650-4a34-8a23-3b02f3b9aeac", DataType.ADMIN
        )
        assert actual_admin.otp_secret is not None
        assert len(actual_admin.otp_secret) == 32

    def test_issue_otp_upper(self, mocker, mock_auth_admin):
        # #######################################
        # モック化
        # #######################################
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mocker.patch.object(SesHelper, "send_mail")

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
        )
        admin.save()

        # #######################################
        # 検証
        # #######################################

        # レスポンス検証
        response = client.get("/api/auth/otp")
        actual = response.json()

        assert response.status_code == 200
        assert actual["result"] == "One-time password was sent successfully."

        # データを検証
        actual_admin = AdminModel.get(
            "906a3144-9650-4a34-8a23-3b02f3b9aeac", DataType.ADMIN
        )
        assert actual_admin.otp_secret is not None
        assert len(actual_admin.otp_secret) == 32
