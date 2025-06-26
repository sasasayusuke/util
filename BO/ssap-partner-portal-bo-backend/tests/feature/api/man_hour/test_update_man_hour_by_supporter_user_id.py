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
from app.models.master import MasterSupporterOrganizationModel
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.user import UserModel
from app.resources.const import DataType, MasterDataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestUpdateManHourBySupporterUserId:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.SURVEY_OPS.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_not_access(
        self,
        mock_auth_admin,
        role_types,
    ):
        mock_auth_admin(role_types)
        supporter_user_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        response = client.put(f"/api/man-hours/{supporter_user_id}?year=2021&month=7&version=1", headers=REQUEST_HEADERS)

        assert response.status_code == 403

    def setup_method(self, method):
        ManHourSupporterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        ProjectModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        ProjectKarteModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        UserModel.create_table(read_capacity_units=5, write_capacity_units=5, wait=True)
        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        ManHourSupporterModel(
            data_type="supporter#89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            year_month="2021/07",
            supporter_user_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            supporter_name="田中太郎",
            supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            supporter_organization_name="ソニー株式会社",
            direct_support_man_hours=DirectSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        role="プロデューサー",
                        service_type="service-1",
                        input_man_hour=10,
                    ),
                ]
            ),
            pre_support_man_hours=PreSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        role="プロデューサー",
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
        ).save()
        ProjectModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type="project",
            customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            name="案件1",
            main_sales_user_id="main_sales_user_id",
            support_date_from="2021/05/30",
            support_date_to="2021/12/28",
            total_contract_time=200,
            main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            is_karte_remind=True,
            is_secret=False,
        ).save()

        ProjectKarteModel(
            karte_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            date="2021/07/02",
            start_datetime="2021/07/02 13:00",
            man_hour=10,
            is_draft=True,
        ).save()
        ProjectKarteModel(
            karte_id="7ac8bddf-88da-46c9-a504-a03d1661ad59",
            project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            date="2021/07/03",
            start_datetime="2021/07/02 13:00",
            man_hour=10,
            is_draft=True,
        ).save()
        ProjectKarteModel(
            karte_id="7ac8bddf-88da-46c9-a504-a03d1661ad60",
            project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
            customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            date="2021/07/03",
            start_datetime="2021/07/02 13:00",
            man_hour=10,
            is_draft=True,
        ).save()

        UserModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.USER,
            name="田中太郎",
            email="yamada@example.com",
            role="customer",
            customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_name="〇〇株式会社",
            job="部長",
            company=None,
            supporter_organization_id=[],
            is_input_man_hour=None,
            project_ids=[],
            agreed=True,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
        ).save()

        MasterSupporterOrganizationModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
            name="Ideation Service Team",
            value="IST",
            order=3,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
        ).save()

    def teardown_method(self, method):
        ManHourSupporterModel.delete_table()
        ProjectModel.delete_table()
        ProjectKarteModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, supporter_user_id, version, body, expected",
        [
            (
                2021,
                7,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                1,
                {
                    "supporterName": "田中太郎",
                    "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "supporterOrganizationName": "ソニー株式会社",
                    "directSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "role": "プロデューサー",
                                "serviceType": "service-1",
                                "inputManHour": 20,
                            },
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                                "role": "プロデューサー",
                                "serviceType": "service-2",
                                "inputManHour": 10,
                            },
                        ]
                    },
                    "preSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "role": "プロデューサー",
                                "serviceType": "service-1",
                                "inputManHour": 20,
                            },
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                                "role": "プロデューサー",
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
                    "summaryManHour": {
                        "direct": 10,
                        "pre": 0,
                        "sales": 0,
                        "ssap": 0,
                        "others": 0,
                        "total": 0,
                    },
                    "isConfirm": False,
                },
                {"message": "OK"},
            )
        ],
    )
    def test_ok(
        self,
        mock_auth_admin,
        year,
        month,
        supporter_user_id,
        version,
        body,
        expected,
    ):
        """支援者別工数の更新成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # テスト実行
        # ###########################
        response = client.put(
            f"/api/man-hours/{supporter_user_id}?year={year}&month={month}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "year, month, supporter_user_id, version, body",
        [
            (
                2021,
                7,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                2,
                {
                    "supporterName": "田中太郎",
                    "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "supporterOrganizationName": "ソニー株式会社",
                    "directSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "role": "プロデューサー",
                                "serviceType": "service-1",
                                "inputManHour": 20,
                            },
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                                "role": "プロデューサー",
                                "serviceType": "service-2",
                                "inputManHour": 10,
                            },
                        ]
                    },
                    "preSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "role": "プロデューサー",
                                "serviceType": "service-1",
                                "inputManHour": 20,
                            },
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                                "role": "プロデューサー",
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
                    "summaryManHour": {
                        "direct": 10,
                        "pre": 0,
                        "sales": 0,
                        "ssap": 0,
                        "others": 0,
                        "total": 0,
                    },
                    "isConfirm": False,
                },
            )
        ],
    )
    def test_conflict(
        self,
        mock_auth_admin,
        year,
        month,
        supporter_user_id,
        version,
        body,
    ):
        """支援者別工数の更新成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################

        # ###########################
        # テスト実行
        # ###########################
        response = client.put(
            f"/api/man-hours/{supporter_user_id}?year={year}&month={month}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 409
