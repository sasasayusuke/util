import pytest
from app.main import app
from app.models.user import UserModel
from app.models.admin import AdminModel
from app.models.solver_corporation import SolverCorporationModel
from app.resources.const import DataType, UserRoleType
from app.schemas.solver_corporation import (
    ValueAndMemo,
    Address,
    CorporatePhoto,
    CorporateInfoDocument,
    MainCharge,
    DeputyCharge,
    OtherCharge,
)
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestGetSolverCorporationById:
    @pytest.mark.parametrize(
        "solver_corporation_id, solver_corporation_model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                SolverCorporationModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER_CORPORATION,
                    name="株式会社A",
                    company_abbreviation="A",
                    industry="IT",
                    established="2024/10/30",
                    management_team="山田太郎",
                    employee=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    capital=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    earnings=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    listing_exchange="東京証券取引所",
                    business_content="ITソリューションの提供",
                    address=Address(
                        postal_code="000-0000",
                        state="東京都",
                        city="千代田区",
                        street="丸の内1-1-1",
                        building="丸の内ビルディング",
                    ),
                    corporate_photo=CorporatePhoto(
                        file_name="ファイルA",
                        path="http://www.example.com",
                    ),
                    corporate_info_document=[
                        CorporateInfoDocument(
                            file_name="ファイルA",
                            path="http://www.example.com",
                        )
                    ],
                    issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    vision="新規事業開発",
                    mission="新規事業開発",
                    main_charge=MainCharge(
                        name="山田太郎",
                        kana="やまだたろう",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    deputy_charge=DeputyCharge(
                        name="山田太郎",
                        kana="やまだたろう",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    other_charge=OtherCharge(
                        name="山田太郎",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    notes="特になし",
                    disabled=True,
                    create_id="a9b67094-cdab-494c-818e-d4845088269b",
                    create_at="2022-07-12T02:21:39.356000+0000",
                    price_and_operating_rate_update_at="2022-07-12T02:21:39.356000+0000",
                    price_and_operating_rate_update_by="ソニー太郎",
                    update_id="b9b67094-cdab-494c-818e-d4845088269b",
                    update_at="2022-07-12T02:21:39.356000+0000",
                    version="1",
                    utilization_rate_version=1,
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        "building": "丸の内ビルディング",
                    },
                    "corporatePhoto": {
                        "fileName": "ファイルA",
                        "path": "http://www.example.com",
                    },
                    "corporateInfoDocument": [
                        {
                            "fileName": "ファイルA",
                            "path": "http://www.example.com",
                        },
                    ],
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "vision": "新規事業開発",
                    "mission": "新規事業開発",
                    "mainCharge": {
                        "name": "山田太郎",
                        "kana": "やまだたろう",
                        "title": "リーダー",
                        "email": "連絡先メールアドレス",
                        "department": "経営企画部",
                        "phone": "00-0000-0000",
                    },
                    "deputyCharge": {
                        "name": "山田太郎",
                        "kana": "やまだたろう",
                        "title": "リーダー",
                        "email": "連絡先メールアドレス",
                        "department": "経営企画部",
                        "phone": "00-0000-0000",
                    },
                    "otherCharge": {
                        "name": "山田太郎",
                        "title": "リーダー",
                        "email": "連絡先メールアドレス",
                        "department": "経営企画部",
                        "phone": "00-0000-0000",
                    },
                    "notes": "特になし",
                    "disabled": True,
                    "createId": "a9b67094-cdab-494c-818e-d4845088269b",
                    "createUserName": "テストシステム管理者",
                    "createAt": "2022-07-12T02:21:39.356+09:00",
                    "priceAndOperatingRateUpdateAt": "2022-07-12T02:21:39.356+09:00",
                    "priceAndOperatingRateUpdateBy": "ソニー太郎",
                    "updateId": "b9b67094-cdab-494c-818e-d4845088269b",
                    "updateUserName": "テスト法人ソルバー",
                    "updateAt": "2022-07-12T02:21:39.356+09:00",
                    "version": 1,
                    "utilizationRateVersion": 1
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_auth_ok_apt(
        self,
        mocker,
        solver_corporation_id,
        solver_corporation_model,
        expected,
    ):
        """正常系のテスト（APT）"""
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = solver_corporation_model

        mock_admin = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin.return_value = "テストシステム管理者"

        mock_user = mocker.patch.object(UserModel, "get_update_user_name")
        mock_user.return_value = "テスト法人ソルバー"

        response = client.get(f"/api/solver-corporations/{solver_corporation_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "solver_corporation_id, solver_corporation_model, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                SolverCorporationModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9a999",
                    data_type=DataType.SOLVER_CORPORATION,
                    name="株式会社A",
                    company_abbreviation="A",
                    industry="IT",
                    established="2024/10/30",
                    management_team="山田太郎",
                    employee=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    capital=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    earnings=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    listing_exchange="東京証券取引所",
                    business_content="ITソリューションの提供",
                    address=Address(
                        postal_code="000-0000",
                        state="東京都",
                        city="千代田区",
                        street="丸の内1-1-1",
                        building="丸の内ビルディング",
                    ),
                    corporate_photo=CorporatePhoto(
                        file_name="ファイルA",
                        path="http://www.example.com",
                    ),
                    corporate_info_document=[
                        CorporateInfoDocument(
                            file_name="ファイルA",
                            path="http://www.example.com",
                        )
                    ],
                    issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    vision="新規事業開発",
                    mission="新規事業開発",
                    main_charge=MainCharge(
                        name="山田太郎",
                        kana="やまだたろう",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    deputy_charge=DeputyCharge(
                        name="山田太郎",
                        kana="やまだたろう",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    other_charge=OtherCharge(
                        name="山田太郎",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    notes="特になし",
                    disabled=True,
                    create_id="a9b67094-cdab-494c-818e-d4845088269b",
                    create_at="2022-07-12T02:21:39.356000+0000",
                    price_and_operating_rate_update_at="2022-07-12T02:21:39.356000+0000",
                    price_and_operating_rate_update_by="ソニー太郎",
                    update_id="b9b67094-cdab-494c-818e-d4845088269b",
                    update_at="2022-07-12T02:21:39.356000+0000",
                    version="1",
                    utilization_rate_version=1,
                ),
                {
                    "id": "906a3144-9650-4a34-8a23-3b02f3b9a999",
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
                        "building": "丸の内ビルディング",
                    },
                    "corporatePhoto": {
                        "fileName": "ファイルA",
                        "path": "http://www.example.com",
                    },
                    "corporateInfoDocument": [
                        {
                            "fileName": "ファイルA",
                            "path": "http://www.example.com",
                        },
                    ],
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "vision": "新規事業開発",
                    "mission": "新規事業開発",
                    "mainCharge": {
                        "name": "山田太郎",
                        "kana": "やまだたろう",
                        "title": "リーダー",
                        "email": "連絡先メールアドレス",
                        "department": "経営企画部",
                        "phone": "00-0000-0000",
                    },
                    "deputyCharge": {
                        "name": "山田太郎",
                        "kana": "やまだたろう",
                        "title": "リーダー",
                        "email": "連絡先メールアドレス",
                        "department": "経営企画部",
                        "phone": "00-0000-0000",
                    },
                    "otherCharge": {
                        "name": "山田太郎",
                        "title": "リーダー",
                        "email": "連絡先メールアドレス",
                        "department": "経営企画部",
                        "phone": "00-0000-0000",
                    },
                    "notes": "特になし",
                    "disabled": True,
                    "createId": "a9b67094-cdab-494c-818e-d4845088269b",
                    "createUserName": "テストシステム管理者",
                    "createAt": "2022-07-12T02:21:39.356+09:00",
                    "priceAndOperatingRateUpdateAt": "2022-07-12T02:21:39.356+09:00",
                    "priceAndOperatingRateUpdateBy": "ソニー太郎",
                    "updateId": "b9b67094-cdab-494c-818e-d4845088269b",
                    "updateUserName": "テスト法人ソルバー",
                    "updateAt": "2022-07-12T02:21:39.356+09:00",
                    "version": 1,
                    "utilizationRateVersion": 1,
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_auth_ok_solver_staff(
        self,
        mocker,
        solver_corporation_id,
        solver_corporation_model,
        expected,
    ):
        """正常系のテスト（法人ソルバー）"""
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = solver_corporation_model

        mock_admin = mocker.patch.object(AdminModel, "get_update_user_name")
        mock_admin.return_value = "テストシステム管理者"

        mock_user = mocker.patch.object(UserModel, "get_update_user_name")
        mock_user.return_value = "テスト法人ソルバー"

        response = client.get(f"/api/solver-corporations/{solver_corporation_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "admin_model, solver_corporation_id, solver_corporation_model, expected",
        [
            (
                AdminModel(
                    id="a9b67094-cdab-494c-818e-d4845088269b",
                    data_type=DataType.ADMIN,
                    name="テストシステム管理者",
                    email="user@example.com",
                    job="部長",
                    supporter_organization_id=None,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                SolverCorporationModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER_CORPORATION,
                    name="株式会社A",
                    company_abbreviation="A",
                    industry="IT",
                    established="2024/10/30",
                    management_team="山田太郎",
                    employee=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    capital=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    earnings=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    listing_exchange="東京証券取引所",
                    business_content="ITソリューションの提供",
                    address=Address(
                        postal_code="000-0000",
                        state="東京都",
                        city="千代田区",
                        street="丸の内1-1-1",
                        building="丸の内ビルディング",
                    ),
                    corporate_photo=CorporatePhoto(
                        file_name="ファイルA",
                        path="http://www.example.com",
                    ),
                    corporate_info_document=[
                        CorporateInfoDocument(
                            file_name="ファイルA",
                            path="http://www.example.com",
                        )
                    ],
                    issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    vision="新規事業開発",
                    mission="新規事業開発",
                    main_charge=MainCharge(
                        name="山田太郎",
                        kana="やまだたろう",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    deputy_charge=DeputyCharge(
                        name="山田太郎",
                        kana="やまだたろう",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    other_charge=OtherCharge(
                        name="山田太郎",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    notes="特になし",
                    disabled=True,
                    create_id="a9b67094-cdab-494c-818e-d4845088269b",
                    create_at="2022-07-12T02:21:39.356000+0000",
                    price_and_operating_rate_update_at="2022-07-12T02:21:39.356000+0000",
                    price_and_operating_rate_update_by="ソニー太郎",
                    update_id=None,
                    update_at=None,
                    version="1",
                    utilization_rate_version=1,
                ),
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        "building": "丸の内ビルディング",
                    },
                    "corporatePhoto": {
                        "fileName": "ファイルA",
                        "path": "http://www.example.com",
                    },
                    "corporateInfoDocument": [
                        {
                            "fileName": "ファイルA",
                            "path": "http://www.example.com",
                        },
                    ],
                    "issueMap50": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    "vision": "新規事業開発",
                    "mission": "新規事業開発",
                    "mainCharge": {
                        "name": "山田太郎",
                        "kana": "やまだたろう",
                        "title": "リーダー",
                        "email": "連絡先メールアドレス",
                        "department": "経営企画部",
                        "phone": "00-0000-0000",
                    },
                    "deputyCharge": {
                        "name": "山田太郎",
                        "kana": "やまだたろう",
                        "title": "リーダー",
                        "email": "連絡先メールアドレス",
                        "department": "経営企画部",
                        "phone": "00-0000-0000",
                    },
                    "otherCharge": {
                        "name": "山田太郎",
                        "title": "リーダー",
                        "email": "連絡先メールアドレス",
                        "department": "経営企画部",
                        "phone": "00-0000-0000",
                    },
                    "notes": "特になし",
                    "disabled": True,
                    "createId": "a9b67094-cdab-494c-818e-d4845088269b",
                    "createUserName": "テストシステム管理者",
                    "createAt": "2022-07-12T02:21:39.356+09:00",
                    "priceAndOperatingRateUpdateAt": "2022-07-12T02:21:39.356+09:00",
                    "priceAndOperatingRateUpdateBy": "ソニー太郎",
                    "updateId": "",
                    "updateUserName": "",
                    "updateAt": None,
                    "version": 1,
                    "utilizationRateVersion": 1,
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_only_create_ok(
        self,
        mocker,
        admin_model,
        solver_corporation_id,
        solver_corporation_model,
        expected,
    ):
        """正常系のテスト（APT）"""
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = solver_corporation_model

        mock_user = mocker.patch.object(AdminModel, "get")
        mock_user.return_value = admin_model

        response = client.get(f"/api/solver-corporations/{solver_corporation_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "admin_model, user_model, solver_corporation_id, solver_corporation_model, expected",
        [
            (
                AdminModel(
                    id="a9b67094-cdab-494c-818e-d4845088269b",
                    data_type=DataType.ADMIN,
                    name="テストシステム管理者",
                    email="user@example.com",
                    job="部長",
                    supporter_organization_id=None,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                UserModel(
                    id="b9b67094-cdab-494c-818e-d4845088269b",
                    data_type=DataType.USER,
                    name="テスト法人ソルバー",
                    email="user@example.com",
                    role=UserRoleType.SOLVER_STAFF,
                    customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                    customer_name="ソニーグループ株式会社",
                    job="部長",
                    company="ソニーグループ株式会社",
                    solver_corporation_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    supporter_organization_id=None,
                    is_input_man_hour=None,
                    project_ids=None,
                    cognito_id=None,
                    agreed=False,
                    last_login_at=None,
                    disabled=False,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                "66cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                SolverCorporationModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SOLVER_CORPORATION,
                    name="株式会社A",
                    company_abbreviation="A",
                    industry="IT",
                    established="2024/10/30",
                    management_team="山田太郎",
                    employee=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    capital=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    earnings=ValueAndMemo(
                        value="100",
                        memo="2024年度",
                    ),
                    listing_exchange="東京証券取引所",
                    business_content="ITソリューションの提供",
                    address=Address(
                        postal_code="000-0000",
                        state="東京都",
                        city="千代田区",
                        street="丸の内1-1-1",
                        building="丸の内ビルディング",
                    ),
                    corporate_photo=CorporatePhoto(
                        file_name="ファイルA",
                        path="http://www.example.com",
                    ),
                    corporate_info_document=[
                        CorporateInfoDocument(
                            file_name="ファイルA",
                            path="http://www.example.com",
                        )
                    ],
                    issue_map50=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                    vision="新規事業開発",
                    mission="新規事業開発",
                    main_charge=MainCharge(
                        name="山田太郎",
                        kana="やまだたろう",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    deputy_charge=DeputyCharge(
                        name="山田太郎",
                        kana="やまだたろう",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    other_charge=OtherCharge(
                        name="山田太郎",
                        title="リーダー",
                        email="連絡先メールアドレス",
                        department="経営企画部",
                        phone="00-0000-0000",
                    ),
                    notes="特になし",
                    disabled=True,
                    create_id="a9b67094-cdab-494c-818e-d4845088269b",
                    create_at="2022-07-12T02:21:39.356000+0000",
                    price_and_operating_rate_update_at="2022-07-12T02:21:39.356000+0000",
                    price_and_operating_rate_update_by="ソニー太郎",
                    update_id="b9b67094-cdab-494c-818e-d4845088269b",
                    update_at="2022-07-12T02:21:39.356000+0000",
                    version="1",
                ),
                {
                    "detail": "Forbidden",
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_auth_error_solver_staff_403(
        self,
        mocker,
        admin_model,
        user_model,
        solver_corporation_id,
        solver_corporation_model,
        expected,
    ):
        """権限確認: 法人ソルバー: アクセス不可"""
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.return_value = solver_corporation_model

        mock_admin_user = mocker.patch.object(AdminModel, "get")
        mock_admin_user.return_value = admin_model

        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        response = client.get(f"/api/solver-corporations/{solver_corporation_id}")

        actual = response.json()
        assert response.status_code == 403
        assert actual == expected

    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_solver_corporation_not_found(
        self,
        mocker,
    ):
        """法人ソルバーが存在しない時のテスト"""
        solver_corporation_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        mock = mocker.patch.object(SolverCorporationModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.get(f"/api/solver-corporations/{solver_corporation_id}")

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
