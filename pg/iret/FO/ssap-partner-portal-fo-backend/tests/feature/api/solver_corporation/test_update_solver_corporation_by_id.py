import pytest
from app.main import app
from app.models.admin import AdminModel
from app.models.solver_corporation import (
    SolverCorporationModel,
    ValueAndMemoAttribute,
    AddressAttribute,
    CorporatePhotoAttribute,
    CorporateInfoDocumentAttribute,
    MainChargeAttribute,
    DeputyChargeAttribute,
    OtherChargeAttribute,
)
from app.resources.const import DataType, UserRoleType
from app.service.master_service import MasterService
from app.service.solver_corporation_service import SolverCorporationService
from app.utils.aws.ses import SesHelper
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestUpdateSolverCorporationById:
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_for_apt(self, mocker):
        """正常系のテスト（APT）"""
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = SolverCorporationModel(
            id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            data_type=DataType.SOLVER_CORPORATION,
            name="株式会社A",
            disabled=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        mocker.patch.object(SolverCorporationModel, "update")

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mock_admin_name = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin_name.return_value = "テストシステム管理者"

        mocker_file_from_s3 = mocker.patch.object(SolverCorporationService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/documents/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.png",
                "file_content": "テスト",
            }
        ]

        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
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

        mocker.patch.object(SesHelper, "send_mail_with_file")

        solver_corporation_id = "8dbeeeb9-8096-4489-b497-6e33e4dabb1d"
        version = 2
        body = {
            "name": "株式会社A",
            "companyAbbreviation": "A",
            "industry": "IT",
            "established": "2024/10/30",
            "managementTeam": "山田太郎",
            "employee": {
                "value": 100,
                "memo": "2024年度",
            },
            "capital": {
                "value": 100,
                "memo": "2024年度",
            },
            "earnings": {
                "value": 100,
                "memo": "2024年度",
            },
            "listingExchange": "東京証券取引所",
            "businessContent": "ITソリューションの提供",
            "address": {
                "postalCode": "000-0000",
                "state": "東京都",
                "city": "千代田区",
                "street": "丸の内1-1-1",
                "building": "丸の内ビルディング"
            },
            "corporatePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.jpeg"
            },
            "corporateInfoDocument": [
                {
                    "fileName": "テスト.txt",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/documents/テスト.txt"
                },
                {
                    "fileName": "テスト.png",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.png"
                },
            ],
            "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            "vision": "新規事業開発",
            "mission": "新規事業開発",
            "mainCharge": {
                "name": "山田太郎",
                "kana": "やまだたろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "deputyCharge": {
                "name": "山田次郎",
                "kana": "やまだじろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "otherCharge": {
                "name": "山田三郎",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "notes": "特になし",
        }

        response = client.put(
            f"/api/solver-corporations/{solver_corporation_id}?version={version}", json=body
        )
        actual = response.json()

        expected = {
            "message": "OK"
        }

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_ok_for_solver_staff(self, mocker):
        """正常系のテスト（法人ソルバー）"""
        solver_corporation_id = "906a3144-9650-4a34-8a23-3b02f3b9a999"
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = SolverCorporationModel(
            id=solver_corporation_id,
            data_type=DataType.SOLVER_CORPORATION,
            name="株式会社A",
            company_abbreviation="A",
            industry="IT",
            established="2024/10/30",
            management_team="山田太郎",
            employee=ValueAndMemoAttribute(
                value=100,
                memo="2024年度",
            ),
            capital=ValueAndMemoAttribute(
                value=100,
                memo="2024年度",
            ),
            earnings=ValueAndMemoAttribute(
                value=100,
                memo="2024年度",
            ),
            listing_exchange="東京証券取引所",
            business_content="ITソリューションの提供",
            address=AddressAttribute(
                postal_code="000-0000",
                state="東京都",
                city="千代田区",
                street="丸の内1-1-1",
                building="丸の内ビルディング"
            ),
            corporate_photo=CorporatePhotoAttribute(
                file_name="テスト.txt",
                path="solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/documents/テスト.txt"
            ),
            corporate_info_document=[
                CorporateInfoDocumentAttribute(
                    file_name="テスト.jpeg",
                    path="solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.jpeg"
                )
            ],
            issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            vision="新規事業開発",
            mission="新規事業開発",
            main_charge=MainChargeAttribute(
                name="山田太郎",
                kana="やまだたろう",
                title="リーダー",
                email="yamada@example.com",
                department="経営企画部",
                phone="00-0000-0000"
            ),
            deputy_charge=DeputyChargeAttribute(
                name="山田太郎",
                kana="やまだたろう",
                title="リーダー",
                email="yamada@example.com",
                department="経営企画部",
                phone="00-0000-0000"
            ),
            other_charge=OtherChargeAttribute(
                name="山田太郎",
                title="リーダー",
                email="yamada@example.com",
                department="経営企画部",
                phone="00-0000-0000"
            ),
            notes="特になし",
            disabled=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            price_and_operating_rate_update_by="山田太郎",
            price_and_operating_rate_update_at="2022-05-23T16:34:21.523000+0000",
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        mocker.patch.object(SolverCorporationModel, "update")

        mocker_issue_map50_name = mocker.patch.object(MasterService, "get_issue_map50_name")
        mocker_issue_map50_name.return_value = [
            "社会課題解決を共に目指すパートナーが不足している"
        ]

        mock_admin_name = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin_name.return_value = "テストシステム管理者"

        mocker_file_from_s3 = mocker.patch.object(SolverCorporationService, "get_file_from_s3")
        mocker_file_from_s3.side_effect = [
            {
                "file_name": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.jpeg",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.txt",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/documents/テスト.txt",
                "file_content": "テスト",
            },
            {
                "file_name": "テスト.png",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.png",
                "file_content": "テスト",
            }
        ]

        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
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

        mocker.patch.object(SesHelper, "send_mail_with_file")

        version = 2
        body = {
            "name": "株式会社A",
            "companyAbbreviation": "A",
            "industry": "IT",
            "established": "2024/10/30",
            "managementTeam": "山田太郎",
            "employee": {
                "value": 100,
                "memo": "2024年度",
            },
            "capital": {
                "value": 100,
                "memo": "2024年度",
            },
            "earnings": {
                "value": 100,
                "memo": "2024年度",
            },
            "listingExchange": "東京証券取引所",
            "businessContent": "ITソリューションの提供",
            "address": {
                "postalCode": "000-0000",
                "state": "東京都",
                "city": "千代田区",
                "street": "丸の内1-1-1",
                "building": "丸の内ビルディング"
            },
            "corporatePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.jpeg"
            },
            "corporateInfoDocument": [
                {
                    "fileName": "テスト.txt",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/documents/テスト.txt"
                },
                {
                    "fileName": "テスト.png",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.png"
                }
            ],
            "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            "vision": "新規事業開発",
            "mission": "新規事業開発",
            "mainCharge": {
                "name": "山田太郎",
                "kana": "やまだたろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "deputyCharge": {
                "name": "山田次郎",
                "kana": "やまだじろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "otherCharge": {
                "name": "山田三郎",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "notes": "特になし",
        }

        response = client.put(
            f"/api/solver-corporations/{solver_corporation_id}?version={version}", json=body
        )
        actual = response.json()

        expected = {
            "message": "OK"
        }

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_apt_user")
    def test_solver_corporation_not_found(self, mocker):
        """法人が存在しない時のテスト"""

        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.side_effect = DoesNotExist()

        solver_corporation_id = "8dbeeeb9-8096-4489-b497-6e33e4dabb1d"
        version = 2
        body = {
            "name": "株式会社A",
            "companyAbbreviation": "A",
            "industry": "IT",
            "established": "2024/10/30",
            "managementTeam": "山田太郎",
            "employee": {
                "value": 100,
                "memo": "2024年度",
            },
            "capital": {
                "value": 100,
                "memo": "2024年度",
            },
            "earnings": {
                "value": 100,
                "memo": "2024年度",
            },
            "listingExchange": "東京証券取引所",
            "businessContent": "ITソリューションの提供",
            "address": {
                "postalCode": "000-0000",
                "state": "東京都",
                "city": "千代田区",
                "street": "丸の内1-1-1",
                "building": "丸の内ビルディング"
            },
            "corporatePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.jpeg"
            },
            "corporateInfoDocument": [
                {
                    "fileName": "テスト.txt",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/documents/テスト.txt"
                },
                {
                    "fileName": "テスト.png",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.png"
                },
            ],
            "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            "vision": "新規事業開発",
            "mission": "新規事業開発",
            "mainCharge": {
                "name": "山田太郎",
                "kana": "やまだたろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "deputyCharge": {
                "name": "山田次郎",
                "kana": "やまだじろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "otherCharge": {
                "name": "山田三郎",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "notes": "特になし",
        }

        response = client.put(
            f"/api/solver-corporations/{solver_corporation_id}?version={version}", json=body
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_auth_error_solver_staff_403(self, mocker):
        """未所属の法人情報にアクセス不可の時のテスト"""

        solver_corporation_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = SolverCorporationModel(
            id=solver_corporation_id,
            data_type=DataType.SOLVER_CORPORATION,
            name="株式会社A",
            disabled=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        version = 2
        body = {
            "name": "株式会社A",
            "companyAbbreviation": "A",
            "industry": "IT",
            "established": "2024/10/30",
            "managementTeam": "山田太郎",
            "employee": {
                "value": 100,
                "memo": "2024年度",
            },
            "capital": {
                "value": 100,
                "memo": "2024年度",
            },
            "earnings": {
                "value": 100,
                "memo": "2024年度",
            },
            "listingExchange": "東京証券取引所",
            "businessContent": "ITソリューションの提供",
            "address": {
                "postalCode": "000-0000",
                "state": "東京都",
                "city": "千代田区",
                "street": "丸の内1-1-1",
                "building": "丸の内ビルディング"
            },
            "corporatePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.jpeg"
            },
            "corporateInfoDocument": [
                {
                    "fileName": "テスト.txt",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/documents/テスト.txt"
                },
                {
                    "fileName": "テスト.png",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.png"
                },
            ],
            "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            "vision": "新規事業開発",
            "mission": "新規事業開発",
            "mainCharge": {
                "name": "山田太郎",
                "kana": "やまだたろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "deputyCharge": {
                "name": "山田次郎",
                "kana": "やまだじろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "otherCharge": {
                "name": "山田三郎",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "notes": "特になし",
        }

        response = client.put(
            f"api/solver-corporations/{solver_corporation_id}?version={version}", json=body
        )

        actual = response.json()
        assert response.status_code == 403
        assert actual["detail"] == "Forbidden"

    @pytest.mark.usefixtures("auth_apt_user")
    def test_project_version_conflict(self, mocker):
        """バージョンが異なる時のテスト"""

        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = SolverCorporationModel(
            id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
            data_type=DataType.SOLVER_CORPORATION,
            name="株式会社A",
            disabled=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            version=2,
        )

        solver_corporation_id = "8dbeeeb9-8096-4489-b497-6e33e4dabb1d"
        version = 1
        body = {
            "name": "株式会社A",
            "companyAbbreviation": "A",
            "industry": "IT",
            "established": "2024/10/30",
            "managementTeam": "山田太郎",
            "employee": {
                "value": 100,
                "memo": "2024年度",
            },
            "capital": {
                "value": 100,
                "memo": "2024年度",
            },
            "earnings": {
                "value": 100,
                "memo": "2024年度",
            },
            "listingExchange": "東京証券取引所",
            "businessContent": "ITソリューションの提供",
            "address": {
                "postalCode": "000-0000",
                "state": "東京都",
                "city": "千代田区",
                "street": "丸の内1-1-1",
                "building": "丸の内ビルディング"
            },
            "corporatePhoto": {
                "fileName": "テスト.jpeg",
                "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.jpeg"
            },
            "corporateInfoDocument": [
                {
                    "fileName": "テスト.txt",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/documents/テスト.txt"
                },
                {
                    "fileName": "テスト.png",
                    "path": "solver-corporation/8dbeeeb9-8096-4489-b497-6e33e4dabb1d/logos/テスト.png"
                },
            ],
            "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            "vision": "新規事業開発",
            "mission": "新規事業開発",
            "mainCharge": {
                "name": "山田太郎",
                "kana": "やまだたろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "deputyCharge": {
                "name": "山田次郎",
                "kana": "やまだじろう",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "otherCharge": {
                "name": "山田三郎",
                "title": "リーダー",
                "email": "yamada@example.com",
                "department": "経営企画部",
                "phone": "00-0000-0000"
            },
            "notes": "特になし",
        }

        response = client.put(
            f"api/solver-corporations/{solver_corporation_id}?version={version}", json=body
        )

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"
