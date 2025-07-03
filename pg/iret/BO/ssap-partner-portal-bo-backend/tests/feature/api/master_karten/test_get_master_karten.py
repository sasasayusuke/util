import json
from datetime import datetime

import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.customer import CustomerModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from app.utils.platform import PlatformApiOperator

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetMasterKarten:
    @pytest.mark.parametrize(
        "project_models, customer_model, project_model, user_model, expected",
        [
            (
                [
                    ProjectModel(
                        id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="222a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト２",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="333a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト３",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="444a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト４",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    # SF商談IDなし
                    ProjectModel(
                        id="555a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト５",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                CustomerModel(
                    id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="12345678-aaaa-bbbb-cccc-0123456789ab",
                    salesforce_update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                    salesforce_target=None,
                    salesforce_credit_limit=None,
                    salesforce_credit_get_month=None,
                    salesforce_credit_manager=None,
                    salesforce_credit_no_retry=None,
                    salesforce_paws_credit_number=None,
                    salesforce_customer_owner=None,
                    salesforce_customer_segment=None,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト１",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="taro.sony@example.com",
                        role=UserRoleType.SALES,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=None,
                        is_input_man_hour=None,
                        project_ids="555a46d3-1d56-428b-ac6d-a51e263b54a8",
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
                ],
                # expected
                {
                    "offsetPage": 1,
                    "total": 1,
                    "karten": [
                        {
                            "npfProjectId": "100",
                            "ppProjectId": "555a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "project": "サンプルプロジェクト５",
                            "serviceName": "string",
                            "client": "ソニーグループ株式会社",
                            "supportDateFrom": "2021/01/30",
                            "supportDateTo": "2021/02/28",
                            "isAccessibleKarten": True,
                            'isAccessibleMasterKarten': True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_sales(
        self,
        mocker,
        mock_auth_admin,
        project_models,
        customer_model,
        project_model,
        user_model,
        expected,
    ):
        """正常1ページ目 total:5件,limit=6,offsetPage=1（営業担当担当者）"""

        mock_auth_admin([UserRoleType.SALES.key])

        mock_project = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project.return_value = project_models

        mock_customer = mocker.patch.object(CustomerModel, "get")
        mock_customer.return_value = customer_model

        # 案件情報へアクセス可能か判定
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mock_user = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user.return_value = user_model

        json_open = open("mock/pf/get_projects.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock.return_value = (200, json_load)

        # SF商談IDが一致するPP案件情報を取得
        mock_project_2 = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project_2.return_value = project_models

        response = client.get(
            "/api/master-karten?offsetPage=1&customerId=12345678-aaaa-bbbb-cccc-0123456789ab",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, customer_model, project_model, user_model, expected",
        [
            (
                [
                    ProjectModel(
                        id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="222a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト２",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="333a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト３",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="444a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト４",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    # SF商談IDなし
                    ProjectModel(
                        id="555a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト５",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                CustomerModel(
                    id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="12345678-aaaa-bbbb-cccc-0123456789ab",
                    salesforce_update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                    salesforce_target=None,
                    salesforce_credit_limit=None,
                    salesforce_credit_get_month=None,
                    salesforce_credit_manager=None,
                    salesforce_credit_no_retry=None,
                    salesforce_paws_credit_number=None,
                    salesforce_customer_owner=None,
                    salesforce_customer_segment=None,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト１",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.CUSTOMER,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
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
                ],
                # expected
                {
                    "offsetPage": 1,
                    "total": 1,
                    "karten": [
                        {
                            "npfProjectId": "100",
                            "ppProjectId": "555a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "project": "サンプルプロジェクト５",
                            "serviceName": "string",
                            "client": "ソニーグループ株式会社",
                            "supportDateFrom": "2021/01/30",
                            "supportDateTo": "2021/02/28",
                            "isAccessibleKarten": True,
                            'isAccessibleMasterKarten': True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_supporter_mgr(
        self,
        mocker,
        mock_auth_admin,
        project_models,
        customer_model,
        project_model,
        user_model,
        expected,
    ):
        """正常1ページ目 total:5件,limit=6,offsetPage=1（支援者責任者）"""

        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        mock_project = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project.return_value = project_models

        mock_customer = mocker.patch.object(CustomerModel, "get")
        mock_customer.return_value = customer_model

        # 案件情報へアクセス可能か判定
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mock_user = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user.return_value = user_model

        json_open = open("mock/pf/get_projects.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock.return_value = (200, json_load)

        # SF商談IDが一致するPP案件情報を取得
        mock_project_2 = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project_2.return_value = project_models

        response = client.get(
            "/api/master-karten?offsetPage=1&customerId=c9b67094-cdab-494c-818e-d4845088269b",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, customer_model, project_model, user_model, expected",
        [
            (
                [
                    ProjectModel(
                        id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="222a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト２",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="333a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト３",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="444a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト４",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    # SF商談IDなし
                    ProjectModel(
                        id="555a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト５",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                CustomerModel(
                    id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="12345678-aaaa-bbbb-cccc-0123456789ab",
                    salesforce_update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                    salesforce_target=None,
                    salesforce_credit_limit=None,
                    salesforce_credit_get_month=None,
                    salesforce_credit_manager=None,
                    salesforce_credit_no_retry=None,
                    salesforce_paws_credit_number=None,
                    salesforce_customer_owner=None,
                    salesforce_customer_segment=None,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト１",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.CUSTOMER,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
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
                ],
                # expected
                {
                    "offsetPage": 1,
                    "total": 1,
                    "karten": [
                        {
                            "npfProjectId": "100",
                            "ppProjectId": "555a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "project": "サンプルプロジェクト５",
                            "serviceName": "string",
                            "client": "ソニーグループ株式会社",
                            "supportDateFrom": "2021/01/30",
                            "supportDateTo": "2021/02/28",
                            "isAccessibleKarten": True,
                            'isAccessibleMasterKarten': True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_2page(
        self,
        mocker,
        mock_auth_admin,
        project_models,
        customer_model,
        project_model,
        user_model,
        expected,
    ):
        """正常2ページ目 total:5件,limit=2,offsetPage=2"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # 営業又は支援者責任者がアクセス可能な案件情報を取得
        mock_projects = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_projects.return_value = project_models

        # 取引先IDからSF取引先IDを取得
        mock_customer = mocker.patch.object(CustomerModel, "get")
        mock_customer.return_value = customer_model

        # 案件情報へアクセス可能か判定
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mock_user = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user.return_value = user_model

        json_open = open("mock/pf/get_projects.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock.return_value = (200, json_load)

        # SF商談IDが一致するPP案件情報を取得
        mock_projects_2 = mocker.patch.object(
            ProjectModel.data_type_name_index, "query"
        )
        mock_projects_2.return_value = project_models

        response = client.get(
            "/api/master-karten?offsetPage=1&customerId=c9b67094-cdab-494c-818e-d4845088269b",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, customer_model, project_model, user_model, expected",
        [
            (
                [],
                CustomerModel(
                    id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="12345678-aaaa-bbbb-cccc-0123456789ab",
                    salesforce_update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                    salesforce_target=None,
                    salesforce_credit_limit=None,
                    salesforce_credit_get_month=None,
                    salesforce_credit_manager=None,
                    salesforce_credit_no_retry=None,
                    salesforce_paws_credit_number=None,
                    salesforce_customer_owner=None,
                    salesforce_customer_segment=None,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト１",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.CUSTOMER,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
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
                ],
                {
                    "offsetPage": 1,
                    "total": 0,
                    "karten": [],
                },
            )
        ],
    )
    def test_ok_zero(
        self,
        mocker,
        mock_auth_admin,
        project_models,
        customer_model,
        project_model,
        user_model,
        expected,
    ):
        """正常 0件  total:0件,limit=2,offsetPage=1"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project.return_value = project_models

        mock_customer = mocker.patch.object(CustomerModel, "get")
        mock_customer.return_value = customer_model

        # 案件情報へアクセス可能か判定
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mock_user = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user.return_value = user_model

        json_open = open("mock/pf/get_project_zero.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock.return_value = (200, json_load)

        # SF商談IDが一致するPP案件情報を取得
        mock_project_2 = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project_2.return_value = project_models

        response = client.get(
            "/api/master-karten?offsetPage=3&limit=2&customerId=c9b67094-cdab-494c-818e-d48450",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, customer_model, project_model, user_model, expected",
        [
            (
                [
                    ProjectModel(
                        id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="222a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト２",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="333a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト３",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="444a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト４",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    # SF商談IDなし
                    ProjectModel(
                        id="555a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト５",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                CustomerModel(
                    id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="12345678-aaaa-bbbb-cccc-0123456789ab",
                    salesforce_update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                    salesforce_target=None,
                    salesforce_credit_limit=None,
                    salesforce_credit_get_month=None,
                    salesforce_credit_manager=None,
                    salesforce_credit_no_retry=None,
                    salesforce_paws_credit_number=None,
                    salesforce_customer_owner=None,
                    salesforce_customer_segment=None,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト１",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.CUSTOMER,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
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
                ],
                # expected
                {
                    "offsetPage": 1,
                    "total": 1,
                    "karten": [
                        {
                            "npfProjectId": "100",
                            "ppProjectId": "555a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "project": "サンプルプロジェクト５",
                            "serviceName": "string",
                            "client": "ソニーグループ株式会社",
                            "supportDateFrom": "2021/01/30",
                            "supportDateTo": "2021/02/28",
                            "isAccessibleKarten": True,
                            'isAccessibleMasterKarten': True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_all_query(
        self,
        mocker,
        mock_auth_admin,
        project_models,
        customer_model,
        project_model,
        user_model,
        expected,
    ):
        """正常 全てのクエリパラメータ"""

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project.return_value = project_models

        mock_customer = mocker.patch.object(CustomerModel, "get")
        mock_customer.return_value = customer_model

        # 案件情報へアクセス可能か判定
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mock_user = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user.return_value = user_model

        json_open = open("mock/pf/get_projects.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock.return_value = (200, json_load)

        # SF商談IDが一致するPP案件情報を取得
        mock_project_2 = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project_2.return_value = project_models

        response = client.get(
            "/api/master-karten?offsetPage=1&limit=10&customerId=c9b67094-cdab-494c-818e-d4845088269b&supportDateFrom=2021/01/30&supportDateTo=2021/02/28&isCurrentProgram=true&customerSegment=ソニーグループ1&customerSegment=ソニーグループ2&industrySegment=電気機器1&industrySegment=電気機器2&industrySegment=電気機器3&departmentName=IT戦略事業部&currentSituation=&issue=メンバー不足&customerSuccess=売上120%&lineup=ラインナップ1&lineup=ラインナップ2&lineup=ラインナップ3&requiredPersonalSkill=十分なマーケティング能力を備えていること&requiredPartner=潤沢な資金があること&strength=マーケティング能力&projectResult=成功&supportResult=成功",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, customer_model, project_model, user_model, expected",
        [
            (
                [
                    ProjectModel(
                        id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="222a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト２",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="333a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト３",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="444a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト４",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    # SF商談IDなし
                    ProjectModel(
                        id="555a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト５",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
                        profit=GrossProfitAttribute(
                            monthly=[
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                                400000,
                            ],
                            quarterly=[1200000, 1200000, 1200000, 1200000],
                            half=[2400000, 2400000],
                            year=4800000,
                        ),
                        total_contract_time=200,
                        main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                CustomerModel(
                    id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                    data_type=DataType.CUSTOMER,
                    name="ソニーグループ株式会社",
                    category="ソニーグループ",
                    salesforce_customer_id="12345678-aaaa-bbbb-cccc-0123456789ab",
                    salesforce_update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
                    salesforce_target=None,
                    salesforce_credit_limit=None,
                    salesforce_credit_get_month=None,
                    salesforce_credit_manager=None,
                    salesforce_credit_no_retry=None,
                    salesforce_paws_credit_number=None,
                    salesforce_customer_owner=None,
                    salesforce_customer_segment=None,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト１",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.CUSTOMER,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
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
                ],
                # expected
                {
                    "offsetPage": 1,
                    "total": 1,
                    "karten": [
                        {
                            "client": "ソニーグループ株式会社",
                            "ppProjectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "npfProjectId": "100",
                            "project": "サンプルプロジェクト１",
                            "serviceName": "string",
                            "supportDateFrom": "2021/01/30",
                            "supportDateTo": "2021/02/28",
                            "isAccessibleKarten": True,
                            'isAccessibleMasterKarten': True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_no_sf_opportunity_id(
        self,
        mocker,
        mock_auth_admin,
        project_models,
        customer_model,
        project_model,
        user_model,
        expected,
    ):
        """正常 PF案件取得APIレスポンスにSF商談IDがない場合"""

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project.return_value = project_models

        mock_customer = mocker.patch.object(CustomerModel, "get")
        mock_customer.return_value = customer_model

        # 案件情報へアクセス可能か判定
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mock_user = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user.return_value = user_model

        json_open = open("mock/pf/get_projetcts_no_sf_opportunity_id.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock.return_value = (200, json_load)

        # SF商談IDがない場合はPP案件IDを利用して、PP案件情報を取得
        mock_project_2 = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project_2.return_value = project_models

        response = client.get(
            "/api/master-karten?offsetPage=1&customerId=c9b67094-cdab-494c-818e-d4845088269b",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
