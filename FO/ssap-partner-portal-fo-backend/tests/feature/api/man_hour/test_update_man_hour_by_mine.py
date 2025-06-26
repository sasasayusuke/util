import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.man_hour import (
    AcquirementItemsSubAttribute,
    DirectSupportManHoursAttribute,
    ManHourSupporterModel,
    PreSupportManHoursAttribute,
    SalesSupportManHoursAttribute,
    SupportItemsSubAttribute,
)
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.user import UserModel
from app.resources.const import DataType

client = TestClient(app)


@mock_dynamodb
class TestUpdateManHourBySupporterUserId:
    @pytest.mark.parametrize(
        "year, month, version, body, project_models, man_hour_model, expected",
        [
            (
                2021,
                7,
                1,
                {
                    "supporterName": "田中太郎",
                    "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "supporterOrganizationName": "ソニー株式会社",
                    "directSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "role": "supporter",
                                "serviceType": "service-1",
                                "inputManHour": 20,
                            },
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                                "role": "supporter",
                                "serviceType": "service-2",
                                "inputManHour": 10,
                            },
                        ]
                    },
                    "preSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "role": "supporter",
                                "serviceType": "service-1",
                                "inputManHour": 20,
                            },
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                                "role": "supporter",
                                "serviceType": "service-2",
                                "inputManHour": 10,
                            },
                        ]
                    },
                    "salesSupportManHours": {
                        "items": [
                            {
                                "projectName": "案件1",
                                "customerName": "顧客1",
                                "type": "new",
                                "inputManHour": 50,
                            },
                            {
                                "projectName": "案件2",
                                "customerName": "顧客2",
                                "type": "new",
                                "inputManHour": 10,
                            },
                        ]
                    },
                    "ssapManHours": {
                        "meeting": 10,
                        "study": 0,
                        "learning": 0,
                        "newService": 0,
                        "startdash": 0,
                        "improvement": 0,
                        "ssap": 0,
                        "qc": 0,
                        "accounting": 0,
                        "management": 0,
                        "officeWork": 0,
                        "others": 0,
                    },
                    "holidaysManHours": {
                        "paidHoliday": 10,
                        "holiday": 0,
                        "private": 0,
                        "others": 0,
                        "departmentOthers": 0,
                    },
                    "isConfirm": False,
                },
                [
                    ProjectModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
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
                        main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    ),
                    ProjectModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                        data_type=DataType.PROJECT,
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
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
                        main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_organization_id="2d82eff6-0af4-48a6-8036-9bec43db33e1",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    ),
                ],
                ManHourSupporterModel(
                    data_type="supporter#48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    year_month="2021/07",
                    supporter_user_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    supporter_name="田中太郎",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    supporter_organization_name="ソニー株式会社",
                    direct_support_man_hours=DirectSupportManHoursAttribute(
                        items=[
                            SupportItemsSubAttribute(
                                project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                role="supporter",
                                service_type="service-1",
                                input_man_hour=10,
                            ),
                        ]
                    ),
                    pre_support_man_hours=PreSupportManHoursAttribute(
                        items=[
                            SupportItemsSubAttribute(
                                project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                role="supporter",
                                service_type="service-1",
                                input_man_hour=20,
                            ),
                        ]
                    ),
                    sales_support_man_hours=SalesSupportManHoursAttribute(
                        items=[
                            AcquirementItemsSubAttribute(
                                project_name="案件1",
                                customer_name="顧客1",
                                type="new",
                                input_man_hour=30,
                            ),
                        ]
                    ),
                    ssap_man_hours={},
                    holidays_man_hours={},
                    summary_man_hour={},
                    is_confirm=False,
                    create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                ),
                {"message": "OK"},
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(
        self,
        mocker,
        year,
        month,
        version,
        body,
        project_models,
        man_hour_model,
        expected,
    ):
        """支援者別工数の更新成功"""
        # ###########################
        # モック化
        # ###########################
        ProjectModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for project_model in project_models:
            project_model.save()

        ManHourSupporterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        man_hour_model.save()

        mock_user = mocker.patch.object(UserModel.cognito_id_index, "query")
        mock_user.return_value = iter(
            [
                UserModel(
                    id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    name="テストユーザー",
                    email="sato@example.com",
                    role="supporter",
                    customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    customer_name="ソニーグループ株式会社",
                    job="",
                    company=None,
                    supporter_organization_id=[],
                    is_input_man_hour=None,
                    project_ids=[],
                    agreed=True,
                    last_login_at="2022-04-25T03:21:39.356Z",
                    disabled=False,
                )
            ]
        )

        # ###########################
        # テスト実行
        # ###########################
        response = client.put(
            f"/api/man-hours/me?year={year}&month={month}&version={version}",
            json=body,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "year, month, version, body, man_hour_model",
        [
            (
                2021,
                7,
                2,
                {
                    "supporterName": "田中太郎",
                    "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "supporterOrganizationName": "ソニー株式会社",
                    "directSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "role": "supporter",
                                "serviceType": "service-1",
                                "inputManHour": 20,
                            },
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                                "role": "supporter",
                                "serviceType": "service-2",
                                "inputManHour": 10,
                            },
                        ]
                    },
                    "preSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "role": "supporter",
                                "serviceType": "service-1",
                                "inputManHour": 20,
                            },
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                                "role": "supporter",
                                "serviceType": "service-2",
                                "inputManHour": 10,
                            },
                        ]
                    },
                    "salesSupportManHours": {
                        "items": [
                            {
                                "projectName": "案件1",
                                "customerName": "顧客1",
                                "type": "new",
                                "inputManHour": 50,
                            },
                            {
                                "projectName": "案件2",
                                "customerName": "顧客2",
                                "type": "new",
                                "inputManHour": 10,
                            },
                        ]
                    },
                    "ssapManHours": {
                        "meeting": 10,
                        "study": 0,
                        "learning": 0,
                        "newService": 0,
                        "startdash": 0,
                        "improvement": 0,
                        "ssap": 0,
                        "qc": 0,
                        "accounting": 0,
                        "management": 0,
                        "officeWork": 0,
                        "others": 0,
                    },
                    "holidaysManHours": {
                        "paidHoliday": 10,
                        "holiday": 0,
                        "private": 0,
                        "others": 0,
                        "departmentOthers": 0,
                    },
                    "isConfirm": False,
                },
                ManHourSupporterModel(
                    data_type="supporter#48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    year_month="2021/07",
                    supporter_user_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    supporter_name="田中太郎",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    supporter_organization_name="ソニー株式会社",
                    direct_support_man_hours=DirectSupportManHoursAttribute(
                        items=[
                            SupportItemsSubAttribute(
                                project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                role="supporter",
                                service_type="service-1",
                                input_man_hour=10,
                            ),
                        ]
                    ),
                    pre_support_man_hours=PreSupportManHoursAttribute(
                        items=[
                            SupportItemsSubAttribute(
                                project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                role="supporter",
                                service_type="service-1",
                                input_man_hour=20,
                            ),
                        ]
                    ),
                    sales_support_man_hours=SalesSupportManHoursAttribute(
                        items=[
                            AcquirementItemsSubAttribute(
                                project_name="案件1",
                                customer_name="顧客1",
                                type="new",
                                input_man_hour=30,
                            ),
                        ]
                    ),
                    ssap_man_hours={},
                    holidays_man_hours={},
                    summary_man_hour={},
                    is_confirm=False,
                    create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                ),
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_conflict(
        self,
        mocker,
        year,
        month,
        version,
        body,
        man_hour_model,
    ):
        """コンフリクトのテスト"""
        # ###########################
        # モック化
        # ###########################
        ManHourSupporterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        man_hour_model.save()

        mock_user = mocker.patch.object(UserModel.cognito_id_index, "query")
        mock_user.return_value = iter(
            [
                UserModel(
                    id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    name="テストユーザー",
                    email="sato@example.com",
                    role="supporter",
                    customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    customer_name="ソニーグループ株式会社",
                    job="",
                    company=None,
                    supporter_organization_id=[],
                    is_input_man_hour=None,
                    project_ids=[],
                    agreed=True,
                    last_login_at="2022-04-25T03:21:39.356Z",
                    disabled=False,
                )
            ]
        )

        # ###########################
        # テスト実行
        # ###########################
        response = client.put(
            f"/api/man-hours/me?year={year}&month={month}&version={version}",
            json=body,
        )

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 409
