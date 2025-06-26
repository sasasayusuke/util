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
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.resources.const import UserRoleType
from app.service.man_hour_service import ManHourService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetManHourBySupporterUserId:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
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
        supporter_user_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        year = 2021
        month = 7
        response = client.get(f"/api/man-hours/{supporter_user_id}?year={year}&month={month}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

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
                        input_man_hour=10.25,
                    ),
                ]
            ),
            pre_support_man_hours=PreSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        role="プロデューサー",
                        service_type="service-1",
                        input_man_hour=20.5,
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
            customer_name="SGC",
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

    def teardown_method(self, method):
        ManHourSupporterModel.delete_table()
        ProjectModel.delete_table()
        ProjectKarteModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, supporter_user_id, expected",
        [
            (
                2021,
                7,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                {
                    "yearMonth": "2021/07",
                    "supporterUserId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "supporterName": "田中太郎",
                    "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "supporterOrganizationName": "ソニー株式会社",
                    "directSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "projectName": "案件1",
                                "role": "プロデューサー",
                                "customerId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "customerName": "SGC",
                                "serviceType": "service-1",
                                "karteManHour": 20,
                                "inputManHour": 10.25,
                            },
                        ],
                        "memo": None,
                    },
                    "preSupportManHours": {
                        "items": [
                            {
                                "projectId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "projectName": "案件1",
                                "role": "プロデューサー",
                                "customerId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "customerName": "SGC",
                                "serviceType": "service-1",
                                "inputManHour": 20.5,
                            },
                        ],
                        "memo": None,
                    },
                    "salesSupportManHours": {
                        "items": [
                            {
                                "projectName": "案件1",
                                "customerName": "顧客1",
                                "type": "new",
                                "inputManHour": 30,
                            },
                        ],
                        "memo": None,
                    },
                    "ssapManHours": {
                        "meeting": 0,
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
                        "memo": None,
                    },
                    "holidaysManHours": {
                        "paidHoliday": 0,
                        "holiday": 0,
                        "private": 0,
                        "others": 0,
                        "departmentOthers": 0,
                        "memo": None,
                    },
                    "summaryManHour": {
                        "direct": 0,
                        "pre": 0,
                        "sales": 0,
                        "ssap": 0,
                        "others": 0,
                        "total": 0,
                    },
                    "isConfirm": False,
                    "createId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "updateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "version": 1,
                },
            )
        ],
    )
    def test_ok(
        self,
        mock_auth_admin,
        mocker,
        year,
        month,
        supporter_user_id,
        expected,
    ):
        """支援者別工数の取得成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        mock = mocker.patch.object(ManHourService, "is_visible_project")
        mock.return_value = True

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/{supporter_user_id}?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        actual.pop("createAt")
        actual.pop("updateAt")
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "year, month, supporter_user_id, expected",
        [
            (
                2021,
                7,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                {
                    "yearMonth": "2021/07",
                    "supporterUserId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "supporterName": "田中太郎",
                    "supporterOrganizationId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "supporterOrganizationName": "ソニー株式会社",
                    "directSupportManHours": {
                        "items": [],
                        "memo": None,
                    },
                    "preSupportManHours": {
                        "items": [],
                        "memo": None,
                    },
                    "salesSupportManHours": {
                        "items": [
                            {
                                "projectName": "案件1",
                                "customerName": "顧客1",
                                "type": "new",
                                "inputManHour": 30,
                            },
                        ],
                        "memo": None,
                    },
                    "ssapManHours": {
                        "meeting": 0,
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
                        "memo": None,
                    },
                    "holidaysManHours": {
                        "paidHoliday": 0,
                        "holiday": 0,
                        "private": 0,
                        "others": 0,
                        "departmentOthers": 0,
                        "memo": None,
                    },
                    "summaryManHour": {
                        "direct": 0,
                        "pre": 0,
                        "sales": 0,
                        "ssap": 0,
                        "others": 0,
                        "total": 0,
                    },
                    "isConfirm": False,
                    "createId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "updateId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "version": 1,
                },
            )
        ],
    )
    def test_supporter_mgr_empty(
        self,
        mock_auth_admin,
        mocker,
        year,
        month,
        supporter_user_id,
        expected,
    ):
        """支援者別工数の取得成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        mock = mocker.patch.object(ManHourService, "is_visible_project")
        mock.return_value = False

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/{supporter_user_id}?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        actual.pop("createAt")
        actual.pop("updateAt")
        assert response.status_code == 200
        assert actual == expected
