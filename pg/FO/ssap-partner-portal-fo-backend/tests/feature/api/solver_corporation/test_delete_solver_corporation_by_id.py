import uuid
from app.utils.aws.ses import SesHelper
import pytest
from app.main import app
from app.models.user import UserModel
from app.models.admin import AdminModel
from app.models.solver_corporation import SolverCorporationModel
from app.resources.const import DataType, UserRoleType
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


@pytest.mark.usefixtures("auth_apt_user")
class TestDeleteSolverCorporationById:
    def test_APTロールで正常に削除ができること(
        self,
        mocker,
    ):
        solver_corporation_id = uuid.uuid4()
        """正常系のテスト（APT）"""
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = SolverCorporationModel(
            id=solver_corporation_id,
            data_type=DataType.SOLVER_CORPORATION,
            name="法人ソルバー1株式会社",
            update_at=None,
            version=1
        )
        mocker.patch.object(SolverCorporationModel, "update")

        mock_admin = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin.return_value = [
            AdminModel(
                id=uuid.uuid4(),
                data_type=DataType.ADMIN,
                name="テストシステム管理者",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                job="部長",
                supporter_organization_id=None,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]

        mock_user = mocker.patch.object(UserModel, "data_type_name_index")
        mock_user.return_value = [
            UserModel(
                id=uuid.uuid4(),
                data_type=DataType.USER,
                name="山田太郎",
                email="taro.yamada@example.com",
                company="テスト株式会社",
                solver_corporation_id=solver_corporation_id,
                is_input_man_hour=True,
                project_ids=[],
                cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                agreed=True,
                role=UserRoleType.SOLVER_STAFF.key,
                version=1,
            ),
            UserModel(
                id=uuid.uuid4(),
                data_type=DataType.USER,
                name="田中花子",
                email="hanako.tanaka@example.com",
                company="テスト株式会社",
                solver_corporation_id=solver_corporation_id,
                is_input_man_hour=True,
                project_ids=[],
                cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                agreed=True,
                role=UserRoleType.SOLVER_STAFF.key,
                version=1,
            )
        ]
        mocker.patch.object(UserModel, "batch_write")

        mocker.patch.object(SesHelper, "send_mail")

        response = client.delete(f"/api/solver-corporations/{solver_corporation_id}?version=1")

        actual = response.json()
        assert response.status_code == 200
        assert actual == {"message": "OK"}

    def test_排他チェックがされていること(self, mocker):
        solver_corporation_id = uuid.uuid4()
        """正常系のテスト（APT）"""
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = SolverCorporationModel(
            id=solver_corporation_id,
            data_type=DataType.SOLVER_CORPORATION,
            name="法人ソルバー1株式会社",
            update_at=None,
            version=2
        )

        response = client.delete(f"/api/solver-corporations/{solver_corporation_id}?version=1")

        assert response.status_code == 409

    def test_該当の法人ソルバーが存在しない場合に404を返すこと(self, mocker):
        solver_corporation_id = uuid.uuid4()
        """正常系のテスト（APT）"""
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.delete(f"/api/solver-corporations/{solver_corporation_id}?version=1")

        assert response.status_code == 404
