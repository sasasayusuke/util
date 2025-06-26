import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.admin import AdminModel
from app.models.master import MasterSupporterOrganizationModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.resources.const import DataType, MasterDataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestSuggestProjects:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.SURVEY_OPS.key],
            [UserRoleType.MAN_HOUR_OPS.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_access_ok(
        self,
        mock_auth_admin,
        role_types,
    ):
        mock_auth_admin(role_types)
        response = client.get(
            "/api/projects/suggest",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 422

    @pytest.mark.parametrize(
        "project_model_list, expected, master_model",
        [
            (
                [
                    ProjectModel(
                        id="afba46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社１",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/01",
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
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        id="bfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト２",
                        customer_name="ソニーグループ株式会社２",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/03/01",
                        support_date_to="2021/04/30",
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
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        id="cfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト３",
                        customer_name="ソニーグループ株式会社３",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/05/01",
                        support_date_to="2021/06/30",
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
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                [
                    {
                        "id": "afba46d3-1d56-428b-ac6d-a51e263b54a8",
                        "name": "サンプルプロジェクト１",
                        "displayName": "サンプルプロジェクト１：組織開発：2021/01/01 ～ 2021/02/28：ソニーグループ株式会社１",
                    },
                    {
                        "id": "bfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        "name": "サンプルプロジェクト２",
                        "displayName": "サンプルプロジェクト２：組織開発：2021/03/01 ～ 2021/04/30：ソニーグループ株式会社２",
                    },
                    {
                        "id": "cfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        "name": "サンプルプロジェクト３",
                        "displayName": "サンプルプロジェクト３：組織開発：2021/05/01 ～ 2021/06/30：ソニーグループ株式会社３",
                    },
                ],
                MasterSupporterOrganizationModel(
                    id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                    name="組織開発",
                    value=None,
                    attributes=None,
                    order=1,
                    use=True,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_ok_specified_customer_id(
        self, mocker, mock_auth_admin, project_model_list, expected, master_model
    ):
        """正常系（取引先の指定あり）のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_master_supporter = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_order_index, "query"
        )
        mock_master_supporter.return_value = iter([master_model])

        mock_project = mocker.patch.object(ProjectModel.customer_id_name_index, "query")
        mock_project.return_value = project_model_list
        # mock_project = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        # mock_project.return_value = project_model_list

        customer_id = "3c648558-c450-42d7-9c4b-62e0f22dc0ff"
        sort = "name:asc"

        response = client.get(
            f"/api/projects/suggest?customerId={customer_id}&sort={sort}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_model_list, expected, master_model",
        [
            (
                [
                    ProjectModel(
                        id="afba46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社１",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/01",
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
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        id="bfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト２",
                        customer_name="ソニーグループ株式会社２",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/03/01",
                        support_date_to="2021/04/30",
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
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                        id="cfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト３",
                        customer_name="ソニーグループ株式会社３",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/05/01",
                        support_date_to="2021/06/30",
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
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                [
                    {
                        "id": "afba46d3-1d56-428b-ac6d-a51e263b54a8",
                        "name": "サンプルプロジェクト１",
                        "displayName": "サンプルプロジェクト１：組織開発：2021/01/01 ～ 2021/02/28：ソニーグループ株式会社１",
                    },
                    {
                        "id": "bfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        "name": "サンプルプロジェクト２",
                        "displayName": "サンプルプロジェクト２：組織開発：2021/03/01 ～ 2021/04/30：ソニーグループ株式会社２",
                    },
                    {
                        "id": "cfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        "name": "サンプルプロジェクト３",
                        "displayName": "サンプルプロジェクト３：組織開発：2021/05/01 ～ 2021/06/30：ソニーグループ株式会社３",
                    },
                ],
                MasterSupporterOrganizationModel(
                    id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                    name="組織開発",
                    value=None,
                    attributes=None,
                    order=1,
                    use=True,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_ok_no_specified_customer_id(
        self, mocker, mock_auth_admin, project_model_list, expected, master_model
    ):
        """正常系（取引先の指定なし）のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_master_supporter = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_order_index, "query"
        )
        mock_master_supporter.return_value = iter([master_model])

        # mock_project = mocker.patch.object(ProjectModel.customer_id_name_index, "query")
        # mock_project.return_value = project_model_list
        mock_project = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        mock_project.return_value = project_model_list

        # customer_id = "3c648558-c450-42d7-9c4b-62e0f22dc0ff"
        sort = "name:asc"

        response = client.get(
            f"/api/projects/suggest?sort={sort}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_model_list, expected, master_model",
        [
            (
                [
                    ProjectModel(
                        id="afba46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社１",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/01",
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
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                    # 支援責任者、自分の課以外の非公開案件（取得不可）
                    ProjectModel(
                        id="bfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト２",
                        customer_name="ソニーグループ株式会社２",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/03/01",
                        support_date_to="2021/04/30",
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
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["c9b67094-cdab-494c-818e-d4845088269b"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectModel(
                        id="cfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト３",
                        customer_name="ソニーグループ株式会社３",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/05/01",
                        support_date_to="2021/06/30",
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
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
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
                [
                    {
                        "id": "afba46d3-1d56-428b-ac6d-a51e263b54a8",
                        "name": "サンプルプロジェクト１",
                        "displayName": "サンプルプロジェクト１：組織開発：2021/01/01 ～ 2021/02/28：ソニーグループ株式会社１",
                    },
                    {
                        "id": "cfba46d3-1d56-428b-ac6d-a51e263b54a8",
                        "name": "サンプルプロジェクト３",
                        "displayName": "サンプルプロジェクト３：組織開発：2021/05/01 ～ 2021/06/30：ソニーグループ株式会社３",
                    },
                ],
                MasterSupporterOrganizationModel(
                    id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                    name="組織開発",
                    value=None,
                    attributes=None,
                    order=1,
                    use=True,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_ok_visible_only(
        self, mocker, mock_auth_admin, project_model_list, expected, master_model
    ):
        """正常系(支援責任者でアクセス可能な情報のみ取得)のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        # mock_auth_adminのユーザ部分を上書き
        # 支援者責任者
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SUPPORTER_MGR.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token="111111",
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_master_supporter = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_order_index, "query"
        )
        mock_master_supporter.return_value = iter([master_model])

        mock_project = mocker.patch.object(ProjectModel.customer_id_name_index, "query")
        mock_project.return_value = project_model_list
        # mock_project = mocker.patch.object(ProjectModel.data_type_name_index, "query")
        # mock_project.return_value = project_model_list

        customer_id = "3c648558-c450-42d7-9c4b-62e0f22dc0ff"
        sort = "name:asc"

        response = client.get(
            f"/api/projects/suggest?customerId={customer_id}&sort={sort}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "master_model",
        [
            (
                MasterSupporterOrganizationModel(
                    id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                    name="組織開発",
                    value=None,
                    attributes=None,
                    order=1,
                    use=True,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T04:21:39.356000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_project_zero(self, mocker, mock_auth_admin, master_model):
        """取引先が１件も存在しない時"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_master_supporter = mocker.patch.object(
            MasterSupporterOrganizationModel, "get"
        )
        mock_master_supporter.return_value = master_model

        mock_project = mocker.patch.object(ProjectModel.customer_id_name_index, "query")
        mock_project.return_value = []

        customer_id = "3c648558-c450-42d7-9c4b-62e0f22dc0ff"
        sort = "name:asc"

        response = client.get(
            f"/api/projects/suggest?customerId={customer_id}&sort={sort}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == []

    def test_validation_sort(self, mock_auth_admin):
        """sort指定のバリデーションチェック"""

        sort = "name:desc"
        customer_id = "3c648558-c450-42d7-9c4b-62e0f22dc0ff"

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(
            f"/api/projects/suggest?customerId={customer_id}&sort={sort}",
            headers=REQUEST_HEADERS,
        )
        assert response.status_code == 422
