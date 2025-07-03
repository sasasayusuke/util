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
    SupportItemsSubAttribute,
)
from app.resources.const import UserRoleType
from app.service.common_service.cached_db_items import CachedDbItems
from app.service.man_hour_service import ManHourService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetSummaryManHourType:
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
        mocker
    ):
        mock_auth_admin(role_types)

        ManHourSupporterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        # アクセス制御
        mock = mocker.patch.object(ManHourService, "is_visible_man_hour")
        mock.return_value = True

        year = 2021
        month = 7
        response = client.get(f"/api/man-hours/summary/type?year={year}&month={month}", headers=REQUEST_HEADERS)

        assert response.status_code == 200

        ManHourSupporterModel.delete_table()

    @pytest.mark.parametrize(
        "year, month, man_hour_models, expected",
        [
            (
                2021,
                7,
                [
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
                        summary_man_hour={},
                        is_confirm=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ),
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
                        summary_man_hour={},
                        is_confirm=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ),
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
                        summary_man_hour={},
                        is_confirm=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ),
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
                        summary_man_hour={},
                        is_confirm=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ),
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
                        summary_man_hour={},
                        is_confirm=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ),
                ],
                {
                    "yearMonth": "2021/07",
                    "header": {
                        "supporterOrganizationTotal": [
                            {
                                "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                "supporterOrganizationName": "IST",
                                "manHour": 690,
                            },
                            {
                                "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "supporterOrganizationName": "KYT",
                                "manHour": 440,
                            },
                        ],
                        "supporterOrganizationManHours": [
                            {
                                "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                "supporterOrganizationName": "IST",
                                "supporters": [
                                    {
                                        "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                        "name": "井上太郎",
                                        "manHour": 250,
                                    },
                                    {
                                        "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                        "name": "出川太郎",
                                        "manHour": 220,
                                    },
                                    {
                                        "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                        "name": "鬼塚太郎",
                                        "manHour": 220,
                                    },
                                ],
                            },
                            {
                                "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "supporterOrganizationName": "KYT",
                                "supporters": [
                                    {
                                        "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                        "name": "山本太郎",
                                        "manHour": 230,
                                    },
                                    {
                                        "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                        "name": "山下太郎",
                                        "manHour": 210,
                                    },
                                ],
                            },
                        ],
                    },
                    "manHours": [
                        {
                            "manHourTypeName": "1. 対面支援",
                            "subName": "顧客1234／案件1234",
                            "serviceTypeName": "サービスタイプ1234",
                            "roleName": "アクセラレーター",
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                },
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "1. 対面支援",
                            "subName": "顧客1234／案件1234",
                            "serviceTypeName": "サービスタイプ1234",
                            "roleName": "プロデューサー",
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 20,
                                }
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 20,
                                        }
                                    ],
                                }
                            ],
                        },
                        {
                            "manHourTypeName": "1. 対面支援",
                            "subName": "顧客5678／案件5678",
                            "serviceTypeName": "サービスタイプ5678",
                            "roleName": "アクセラレーター",
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 10,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "1. 対面支援",
                            "subName": "顧客5678／案件5678",
                            "serviceTypeName": "サービスタイプ5678",
                            "roleName": "プロデューサー",
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                }
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                }
                            ],
                        },
                        {
                            "manHourTypeName": "2. 支援仕込",
                            "subName": "顧客1234／案件1234",
                            "serviceTypeName": "サービスタイプ1234",
                            "roleName": "アクセラレーター",
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                },
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "2. 支援仕込",
                            "subName": "顧客1234／案件1234",
                            "serviceTypeName": "サービスタイプ1234",
                            "roleName": "プロデューサー",
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 10,
                                }
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                }
                            ],
                        },
                        {
                            "manHourTypeName": "2. 支援仕込",
                            "subName": "顧客5678／案件5678",
                            "serviceTypeName": "サービスタイプ5678",
                            "roleName": "アクセラレーター",
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 10,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "2. 支援仕込",
                            "subName": "顧客5678／案件5678",
                            "serviceTypeName": "サービスタイプ5678",
                            "roleName": "プロデューサー",
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                }
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                }
                            ],
                        },
                        {
                            "manHourTypeName": "3. 商談/提案準備",
                            "subName": "顧客1／案件1",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 60,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 20,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 20,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 20,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "3. 商談/提案準備",
                            "subName": "顧客2／案件2",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "3. 商談/提案準備",
                            "subName": "顧客3／案件3",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                }
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                }
                            ],
                        },
                        {
                            "manHourTypeName": "3. 商談/提案準備",
                            "subName": "顧客4／案件4",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 10,
                                }
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        }
                                    ],
                                }
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "accounting",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "improvement",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "learning",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "management",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "meeting",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "new_service",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "office_work",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "others",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "qc",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "ssap",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "startdash",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "4. 内部業務",
                            "subName": "study",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "5. 休憩その他",
                            "subName": "department_others",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "5. 休憩その他",
                            "subName": "holiday",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "5. 休憩その他",
                            "subName": "others",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "5. 休憩その他",
                            "subName": "paid_holiday",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
                        },
                        {
                            "manHourTypeName": "5. 休憩その他",
                            "subName": "private",
                            "serviceTypeName": None,
                            "roleName": None,
                            "supporterOrganizationTotal": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "manHour": 30,
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "manHour": 20,
                                },
                            ],
                            "supporterOrganizationManHours": [
                                {
                                    "supporterOrganizationId": "c6ce787c-90d7-4bd6-a9a9-566ffb174062",
                                    "supporterOrganizationName": "IST",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d241",
                                            "name": "井上太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d243",
                                            "name": "出川太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d245",
                                            "name": "鬼塚太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                                {
                                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "supporterOrganizationName": "KYT",
                                    "supporters": [
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d242",
                                            "name": "山本太郎",
                                            "manHour": 10,
                                        },
                                        {
                                            "id": "3e0894d9-d8c4-4651-b160-d9396221d244",
                                            "name": "山下太郎",
                                            "manHour": 10,
                                        },
                                    ],
                                },
                            ],
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
        man_hour_models,
        expected,
    ):
        """支援者別工数の取得成功"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        # ###########################
        # モック化
        # ###########################
        ManHourSupporterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        for model in man_hour_models:
            model.save()

        # 案件名
        mock = mocker.patch.object(CachedDbItems, "get_projects")
        mock.return_value = [
            {
                "id": "2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                "name": "案件1234",
                "customer_id": "0749ee34-f1b4-403d-b722-a8e7917f0418",
            },
            {
                "id": "2bb85e8d-7560-48b0-82c7-d8ba134a9290",
                "name": "案件5678",
                "customer_id": "92e00e16-cd68-487e-a382-53ea474cb807",
            },
        ]
        # 顧客名
        mock = mocker.patch.object(CachedDbItems, "get_customers")
        mock.return_value = [
            {
                "id": "0749ee34-f1b4-403d-b722-a8e7917f0418",
                "name": "顧客1234",
            },
            {
                "id": "92e00e16-cd68-487e-a382-53ea474cb807",
                "name": "顧客5678",
            },
        ]
        # サービス種別名
        mock = mocker.patch.object(CachedDbItems, "ReturnServiceTypes")
        mock.return_value = [
            {
                "id": "06f65c34-1570-4c02-a892-b2cf6392451a",
                "name": "サービスタイプ1234",
            },
            {
                "id": "bad0b57c-de29-48e0-9deb-94be98437441",
                "name": "サービスタイプ5678",
            },
        ]
        # アクセス制御
        mock = mocker.patch.object(ManHourService, "is_visible_man_hour")
        mock.return_value = True

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/type?year={year}&month={month}",
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
            "header": {
                "supporterOrganizationManHours": [],
                "supporterOrganizationTotal": [],
            },
            "manHours": [],
        }

        response = client.get(
            f"/api/man-hours/summary/type?year={year}&month={month}",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
