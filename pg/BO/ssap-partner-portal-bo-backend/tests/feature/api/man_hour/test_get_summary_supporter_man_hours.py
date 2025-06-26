import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.man_hour import (
    AcquirementItemsSubAttribute,
    DirectSupportManHoursAttribute,
    HolidaysManHoursAttribute,
    ManHourSupporterModel,
    PreSupportManHoursAttribute,
    SalesSupportManHoursAttribute,
    SsapManHoursAttribute,
    SummaryManHourAllowNullAttribute,
    SupportItemsSubAttribute,
)
from app.resources.const import UserRoleType
from app.service.common_service.cached_db_items import CachedDbItems
from app.service.man_hour_service import ManHourService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetSummarySupporterManHours:
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
        year = 2021
        month = 7
        response = client.get(f"/api/man-hours/summary/supporter?year={year}&month={month}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

    def setup_method(self, method):
        ManHourSupporterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        ManHourSupporterModel(
            data_type="supporter#3e0894d9-d8c4-4651-b160-d9396221d241",
            year_month="2021/07",
            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d241",
            supporter_name="井上太郎",
            supporter_organization_id="c6ce787c-90d7-4bd6-a9a9-566ffb174062",
            supporter_organization_name="IST",
            direct_support_man_hours=DirectSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                        role="プロデューサー",
                        service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                        input_man_hour=10,
                    ),
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                        role="プロデューサー",
                        service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                        input_man_hour=10,
                    ),
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9290",
                        role="アクセラレーター",
                        service_type="bad0b57c-de29-48e0-9deb-94be98437441",
                        input_man_hour=10,
                    ),
                ]
            ),
            pre_support_man_hours=PreSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                        role="プロデューサー",
                        service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                        input_man_hour=10,
                    ),
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9290",
                        role="アクセラレーター",
                        service_type="bad0b57c-de29-48e0-9deb-94be98437441",
                        input_man_hour=10,
                    ),
                ]
            ),
            sales_support_man_hours=SalesSupportManHoursAttribute(
                items=[
                    AcquirementItemsSubAttribute(
                        project_name="案件1",
                        customer_name="顧客1",
                        type="new",
                        input_man_hour=10,
                    ),
                    AcquirementItemsSubAttribute(
                        project_name="案件1",
                        customer_name="顧客1",
                        type="new",
                        input_man_hour=10,
                    ),
                    AcquirementItemsSubAttribute(
                        project_name="案件2",
                        customer_name="顧客2",
                        type="new",
                        input_man_hour=10,
                    ),
                ]
            ),
            ssap_man_hours=SsapManHoursAttribute(
                meeting=10,
                study=10,
                learning=10,
                new_service=10,
                startdash=10,
                improvement=10,
                ssap=10,
                qc=10,
                accounting=10,
                management=10,
                office_work=10,
                others=10,
                memo="内部業務工数",
            ),
            holidays_man_hours=HolidaysManHoursAttribute(
                paid_holiday=10,
                holiday=10,
                private=10,
                others=10,
                department_others=10,
            ),
            summary_man_hour=SummaryManHourAllowNullAttribute(
                direct=10, pre=10, sales=10, ssap=10, others=10, total=50
            ),
            is_confirm=False,
            create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        ).save()
        ManHourSupporterModel(
            data_type="supporter#3e0894d9-d8c4-4651-b160-d9396221d242",
            year_month="2021/07",
            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d242",
            supporter_name="山本太郎",
            supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
            supporter_organization_name="KYT",
            direct_support_man_hours=DirectSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                        role="アクセラレーター",
                        service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                        input_man_hour=10,
                    ),
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9290",
                        role="プロデューサー",
                        service_type="bad0b57c-de29-48e0-9deb-94be98437441",
                        input_man_hour=10,
                    ),
                ]
            ),
            pre_support_man_hours=PreSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                        role="アクセラレーター",
                        service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                        input_man_hour=10,
                    ),
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9290",
                        role="プロデューサー",
                        service_type="bad0b57c-de29-48e0-9deb-94be98437441",
                        input_man_hour=10,
                    ),
                ]
            ),
            sales_support_man_hours=SalesSupportManHoursAttribute(
                items=[
                    AcquirementItemsSubAttribute(
                        project_name="案件1",
                        customer_name="顧客1",
                        type="new",
                        input_man_hour=10,
                    ),
                    AcquirementItemsSubAttribute(
                        project_name="案件2",
                        customer_name="顧客2",
                        type="new",
                        input_man_hour=10,
                    ),
                ]
            ),
            ssap_man_hours=SsapManHoursAttribute(
                meeting=10,
                study=10,
                learning=10,
                new_service=10,
                startdash=10,
                improvement=10,
                ssap=10,
                qc=10,
                accounting=10,
                management=10,
                office_work=10,
                others=10,
                memo="内部業務工数",
            ),
            holidays_man_hours=HolidaysManHoursAttribute(
                paid_holiday=10,
                holiday=10,
                private=10,
                others=10,
                department_others=10,
            ),
            summary_man_hour=SummaryManHourAllowNullAttribute(
                direct=10, pre=10, sales=10, ssap=10, others=10, total=50
            ),
            is_confirm=False,
            create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        ).save()
        ManHourSupporterModel(
            data_type="supporter#3e0894d9-d8c4-4651-b160-d9396221d243",
            year_month="2021/07",
            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d243",
            supporter_name="出川太郎",
            supporter_organization_id="c6ce787c-90d7-4bd6-a9a9-566ffb174062",
            supporter_organization_name="IST",
            direct_support_man_hours=DirectSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                        role="アクセラレーター",
                        service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                        input_man_hour=10,
                    ),
                ]
            ),
            pre_support_man_hours=PreSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                        role="アクセラレーター",
                        service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                        input_man_hour=10,
                    ),
                ]
            ),
            sales_support_man_hours=SalesSupportManHoursAttribute(
                items=[
                    AcquirementItemsSubAttribute(
                        project_name="案件1",
                        customer_name="顧客1",
                        type="new",
                        input_man_hour=10,
                    ),
                    AcquirementItemsSubAttribute(
                        project_name="案件1",
                        customer_name="顧客1",
                        type="new",
                        input_man_hour=10,
                    ),
                    AcquirementItemsSubAttribute(
                        project_name="案件2",
                        customer_name="顧客2",
                        type="new",
                        input_man_hour=10,
                    ),
                ]
            ),
            ssap_man_hours=SsapManHoursAttribute(
                meeting=10,
                study=10,
                learning=10,
                new_service=10,
                startdash=10,
                improvement=10,
                ssap=10,
                qc=10,
                accounting=10,
                management=10,
                office_work=10,
                others=10,
                memo="内部業務工数",
            ),
            holidays_man_hours=HolidaysManHoursAttribute(
                paid_holiday=10,
                holiday=10,
                private=10,
                others=10,
                department_others=10,
            ),
            summary_man_hour=SummaryManHourAllowNullAttribute(
                direct=10, pre=10, sales=10, ssap=10, others=10, total=50
            ),
            is_confirm=False,
            create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        ).save()
        ManHourSupporterModel(
            data_type="supporter#3e0894d9-d8c4-4651-b160-d9396221d244",
            year_month="2021/07",
            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d244",
            supporter_name="山下太郎",
            supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
            supporter_organization_name="KYT",
            direct_support_man_hours=DirectSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9290",
                        role="アクセラレーター",
                        service_type="bad0b57c-de29-48e0-9deb-94be98437441",
                        input_man_hour=10,
                    ),
                ]
            ),
            pre_support_man_hours=PreSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9290",
                        role="アクセラレーター",
                        service_type="bad0b57c-de29-48e0-9deb-94be98437441",
                        input_man_hour=10,
                    ),
                ]
            ),
            sales_support_man_hours=SalesSupportManHoursAttribute(
                items=[
                    AcquirementItemsSubAttribute(
                        project_name="案件3",
                        customer_name="顧客3",
                        type="new",
                        input_man_hour=10,
                    ),
                    AcquirementItemsSubAttribute(
                        project_name="案件4",
                        customer_name="顧客4",
                        type="new",
                        input_man_hour=10,
                    ),
                ]
            ),
            ssap_man_hours=SsapManHoursAttribute(
                meeting=10,
                study=10,
                learning=10,
                new_service=10,
                startdash=10,
                improvement=10,
                ssap=10,
                qc=10,
                accounting=10,
                management=10,
                office_work=10,
                others=10,
                memo="内部業務工数",
            ),
            holidays_man_hours=HolidaysManHoursAttribute(
                paid_holiday=10,
                holiday=10,
                private=10,
                others=10,
                department_others=10,
            ),
            summary_man_hour=SummaryManHourAllowNullAttribute(
                direct=10, pre=10, sales=10, ssap=10, others=10, total=50
            ),
            is_confirm=False,
            create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        ).save()
        ManHourSupporterModel(
            data_type="supporter#3e0894d9-d8c4-4651-b160-d9396221d245",
            year_month="2021/07",
            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d245",
            supporter_name="鬼塚太郎",
            supporter_organization_id="c6ce787c-90d7-4bd6-a9a9-566ffb174062",
            supporter_organization_name="IST",
            direct_support_man_hours=DirectSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                        role="アクセラレーター",
                        service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                        input_man_hour=10,
                    ),
                ]
            ),
            pre_support_man_hours=PreSupportManHoursAttribute(
                items=[
                    SupportItemsSubAttribute(
                        project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                        role="アクセラレーター",
                        service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                        input_man_hour=10,
                    ),
                ]
            ),
            sales_support_man_hours=SalesSupportManHoursAttribute(
                items=[
                    AcquirementItemsSubAttribute(
                        project_name="案件1",
                        customer_name="顧客1",
                        type="new",
                        input_man_hour=10,
                    ),
                    AcquirementItemsSubAttribute(
                        project_name="案件1",
                        customer_name="顧客1",
                        type="new",
                        input_man_hour=10,
                    ),
                    AcquirementItemsSubAttribute(
                        project_name="案件2",
                        customer_name="顧客2",
                        type="new",
                        input_man_hour=10,
                    ),
                ]
            ),
            ssap_man_hours=SsapManHoursAttribute(
                meeting=10,
                study=10,
                learning=10,
                new_service=10,
                startdash=10,
                improvement=10,
                ssap=10,
                qc=10,
                accounting=10,
                management=10,
                office_work=10,
                others=10,
                memo="内部業務工数",
            ),
            holidays_man_hours=HolidaysManHoursAttribute(
                paid_holiday=10,
                holiday=10,
                private=10,
                others=10,
                department_others=10,
            ),
            summary_man_hour=SummaryManHourAllowNullAttribute(
                direct=10, pre=10, sales=10, ssap=10, others=10, total=50
            ),
            is_confirm=False,
            create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        ).save()

    def teardown_method(self, method):
        ManHourSupporterModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, expected",
        [
            (
                2021,
                7,
                {
                    "yearMonth": "2021/07",
                    "manHours": [
                        {
                            "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                            "supporterOrganizationName": "IST",
                            "supporterId": "3e0894d9-d8c4-4651-b160-d9396221d241",
                            "supporterName": "井上太郎",
                            "summaryManHour": {
                                "direct": 10.0,
                                "pre": 10.0,
                                "sales": 10.0,
                                "ssap": 10.0,
                                "others": 10.0,
                                "total": 50.0,
                            },
                            "contractTime": {
                                "producer": 30.0,
                                "accelerator": 20.0,
                                "total": 50.0,
                            },
                            "isConfirm": False,
                        },
                        {
                            "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                            "supporterOrganizationName": "IST",
                            "supporterId": "3e0894d9-d8c4-4651-b160-d9396221d243",
                            "supporterName": "出川太郎",
                            "summaryManHour": {
                                "direct": 10.0,
                                "pre": 10.0,
                                "sales": 10.0,
                                "ssap": 10.0,
                                "others": 10.0,
                                "total": 50.0,
                            },
                            "contractTime": {
                                "producer": 0.0,
                                "accelerator": 20.0,
                                "total": 20.0,
                            },
                            "isConfirm": False,
                        },
                        {
                            "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                            "supporterOrganizationName": "IST",
                            "supporterId": "3e0894d9-d8c4-4651-b160-d9396221d245",
                            "supporterName": "鬼塚太郎",
                            "summaryManHour": {
                                "direct": 10.0,
                                "pre": 10.0,
                                "sales": 10.0,
                                "ssap": 10.0,
                                "others": 10.0,
                                "total": 50.0,
                            },
                            "contractTime": {
                                "producer": 0.0,
                                "accelerator": 20.0,
                                "total": 20.0,
                            },
                            "isConfirm": False,
                        },
                        {
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "KYT",
                            "supporterId": "3e0894d9-d8c4-4651-b160-d9396221d244",
                            "supporterName": "山下太郎",
                            "summaryManHour": {
                                "direct": 10.0,
                                "pre": 10.0,
                                "sales": 10.0,
                                "ssap": 10.0,
                                "others": 10.0,
                                "total": 50.0,
                            },
                            "contractTime": {
                                "producer": 0.0,
                                "accelerator": 20.0,
                                "total": 20.0,
                            },
                            "isConfirm": False,
                        },
                        {
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "KYT",
                            "supporterId": "3e0894d9-d8c4-4651-b160-d9396221d242",
                            "supporterName": "山本太郎",
                            "summaryManHour": {
                                "direct": 10.0,
                                "pre": 10.0,
                                "sales": 10.0,
                                "ssap": 10.0,
                                "others": 10.0,
                                "total": 50.0,
                            },
                            "contractTime": {
                                "producer": 20.0,
                                "accelerator": 20.0,
                                "total": 40.0,
                            },
                            "isConfirm": False,
                        },
                    ],
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
        expected,
    ):
        """支援者別工数の取得成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################

        # 案件名
        mock = mocker.patch.object(CachedDbItems, "get_projects")
        mock.return_value = [
            {
                "id": "2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                "supporter_organization_id": "de40733f-6be9-4fef-8229-01052f43c1e2",
                "support_date_from": "2021/05/30",
                "support_date_to": "2021/12/31",
                "total_contract_time": 200,
            },
            {
                "id": "2bb85e8d-7560-48b0-82c7-d8ba134a9290",
                "supporter_organization_id": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                "support_date_from": "2021/04/30",
                "support_date_to": "2021/12/31",
                "total_contract_time": 150,
            },
        ]

        # アクセス制御
        mock = mocker.patch.object(ManHourService, "is_visible_project")
        mock.return_value = True

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/supporter?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    def test_supporter_mgr_ng_and_empty_ok(self, mock_auth_admin, mocker):
        """支援者責任者のアクセス制御がNGでレスポンスに空配列が返ることを確認"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        # ###########################
        # モック化
        # ###########################
        ManHourSupporterModel.delete_table()
        ManHourSupporterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        # アクセス制御
        mock = mocker.patch.object(ManHourService, "is_visible_man_hour")
        mock.return_value = False

        # ###########################
        # テスト実行
        # ###########################
        year = 2021
        month = 7
        expected = {
            "yearMonth": "2021/07",
            "manHours": [],
        }

        response = client.get(
            f"/api/man-hours/summary/supporter?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
