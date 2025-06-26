from datetime import datetime

import freezegun
import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.admin import AdminModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_survey import (
    AnswersAttribute,
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@freezegun.freeze_time("2022/10/31")
@mock_dynamodb
class TestGetSurveys:
    def setup_method(self, method):
        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        ProjectModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        UserModel.create_table(read_capacity_units=5, write_capacity_units=5, wait=True)

        ProjectSurveyModel(
            id="11111449-5d63-42d4-ae1b-f0faf65a7076",
            data_type=DataType.SURVEY,
            survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
            survey_master_revision=4,
            survey_type="quick",
            project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
            project_name="サンプルプロジェクト",
            customer_success="DXの実現",
            supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
            supporter_organization_name="IST",
            support_date_from="2021/06/01",
            support_date_to="2022/12/31",
            main_supporter_user=SupporterUserAttribute(
                id="51ff73f4-2414-421c-b64c-9981f988b331",
                name="田中 三郎",
                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                organization_name="AST",
            ),
            supporter_users=[
                SupporterUserAttribute(
                    id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    name="田中 次郎",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    organization_name="AST",
                )
            ],
            sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
            service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            service_type_name="組織開発",
            answer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
            answer_user_name="田中 次郎",
            customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
            customer_name="あああ研究所",
            company="ソニーグループ株式会社",
            answers=[
                AnswersAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    answer="問題なし",
                    point=3,
                )
            ],
            summary_month="2022/01",
            plan_survey_request_datetime="2022/01/15 01:00",
            actual_survey_request_datetime="2022/01/15 01:10",
            plan_survey_response_datetime="2022/01/31 17:00",
            actual_survey_response_datetime="2022/01/30 10:00",
            is_finished=False,
            is_disclosure=True,
            is_not_summary=False,
            points=PointsAttribute(
                satisfaction=3,
                continuation=False,
                recommended=3,
                sales=0,
                survey_satisfaction=4,
                man_hour_satisfaction=4,
                karte_satisfaction=4,
                master_karte_satisfaction=0,
            ),
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at=datetime.now(),
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at=datetime.now(),
        ).save()

        ProjectSurveyModel(
            id="22222449-5d63-42d4-ae1b-f0faf65a7076",
            data_type=DataType.SURVEY,
            survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
            survey_master_revision=4,
            survey_type="quick",
            project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
            project_name="サンプルプロジェクト",
            customer_success="DXの実現",
            supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
            supporter_organization_name="IST",
            support_date_from="2021/06/01",
            support_date_to="2022/12/31",
            main_supporter_user=SupporterUserAttribute(
                id="51ff73f4-2414-421c-b64c-9981f988b331",
                name="田中 三郎",
                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                organization_name="AST",
            ),
            supporter_users=[
                SupporterUserAttribute(
                    id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    name="田中 次郎",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    organization_name="AST",
                )
            ],
            sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
            service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            service_type_name="組織開発",
            answer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
            answer_user_name="田中 次郎",
            customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
            customer_name="あああ研究所",
            company="ソニーグループ株式会社",
            answers=[
                AnswersAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    answer="問題なし",
                    point=3,
                )
            ],
            summary_month="2022/03",
            plan_survey_request_datetime="2022/03/15 01:00",
            actual_survey_request_datetime="2022/03/15 01:10",
            plan_survey_response_datetime="2022/03/30 17:00",
            actual_survey_response_datetime="2022/03/30 10:00",
            is_finished=True,
            is_disclosure=True,
            is_not_summary=False,
            points=PointsAttribute(
                satisfaction=3,
                continuation=False,
                recommended=3,
                sales=0,
                survey_satisfaction=4,
                man_hour_satisfaction=4,
                karte_satisfaction=4,
                master_karte_satisfaction=0,
            ),
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at=datetime.now(),
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at=datetime.now(),
        ).save()

        ProjectSurveyModel(
            id="33333449-5d63-42d4-ae1b-f0faf65a7076",
            data_type=DataType.SURVEY,
            survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
            survey_master_revision=4,
            survey_type="service",
            project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
            project_name="サンプルプロジェクト",
            customer_success="DXの実現",
            supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
            supporter_organization_name="IST",
            support_date_from="2021/06/01",
            support_date_to="2022/12/31",
            main_supporter_user=SupporterUserAttribute(
                id="51ff73f4-2414-421c-b64c-9981f988b331",
                name="田中 三郎",
                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                organization_name="AST",
            ),
            supporter_users=[
                SupporterUserAttribute(
                    id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    name="田中 次郎",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    organization_name="AST",
                )
            ],
            sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
            service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            service_type_name="組織開発",
            answer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
            answer_user_name="田中 次郎",
            customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
            customer_name="あああ研究所",
            company="ソニーグループ株式会社",
            answers=[
                AnswersAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    answer="問題なし",
                    point=3,
                )
            ],
            summary_month="2022/04",
            plan_survey_request_datetime="2022/04/15 01:00",
            actual_survey_request_datetime="2022/04/15 01:10",
            plan_survey_response_datetime="2022/04/30 17:00",
            actual_survey_response_datetime="2022/04/30 10:00",
            is_finished=True,
            is_disclosure=True,
            is_not_summary=False,
            points=PointsAttribute(
                satisfaction=3,
                continuation=False,
                recommended=3,
                sales=0,
                survey_satisfaction=4,
                man_hour_satisfaction=4,
                karte_satisfaction=4,
                master_karte_satisfaction=0,
            ),
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at=datetime.now(),
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at=datetime.now(),
        ).save()

        ProjectModel(
            id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
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
            customer_user_ids=set(list(["51ff73f4-2414-421c-b64c-9981f988b331"])),
            main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["51ff73f4-2414-421c-b64c-9981f988b331"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at=datetime.now(),
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at=datetime.now(),
        ).save()
        ProjectModel(
            id="222a46d3-1d56-428b-ac6d-a51e263b54a8",
            data_type=DataType.PROJECT,
            salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            name="サンプルプロジェクト２",
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
            customer_user_ids=set(list(["51ff73f4-2414-421c-b64c-9981f988b331"])),
            main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["51ff73f4-2414-421c-b64c-9981f988b331"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
        ).save()

        UserModel(
            id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
            data_type=DataType.USER,
            name="田中 太郎",
            email="user@example.com",
            role=UserRoleType.CUSTOMER.key,
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
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
        ).save()

        UserModel(
            id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
            data_type=DataType.USER,
            name="田中 次郎",
            email="user@example.com",
            role=UserRoleType.CUSTOMER.key,
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
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
        ).save()
        UserModel(
            id="51ff73f4-2414-421c-b64c-9981f988b331",
            data_type=DataType.USER,
            name="田中 三郎",
            email="user@example.com",
            role=UserRoleType.CUSTOMER.key,
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
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
        ).save()
        UserModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.USER,
            name="山田太郎",
            role=UserRoleType.SUPPORTER.key,
            email="taro.yamada@example.com",
            job="部長",
            supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            organization_name="IST",
            cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
        ).save()

    def teardown_method(self, method):
        ProjectSurveyModel.delete_table()
        ProjectModel.delete_table()
        UserModel.delete_table()

    # TODO: 権限テスト
    @pytest.mark.parametrize(
        "admin_model, expected",
        [
            (
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                {
                    "summary": {
                        "monthly": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                        ],
                        "accumulation": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                        ],
                        "total": {
                            "quota": {
                                "quota1": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota2": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota3": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota4": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "half": {
                                "half1": {"satisfactionAverage": 3.0, "receive": 1},
                                "half2": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "year": {"satisfactionAverage": 3.0, "receive": 1},
                        },
                    },
                    "surveys": [
                        {
                            "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/01",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/01/15 01:00",
                            "actualSurveyRequestDatetime": "2022/01/15 01:10",
                            "planSurveyResponseDatetime": "2022/01/31 17:00",
                            "actualSurveyResponseDatetime": "2022/01/30 10:00",
                            "isFinished": False,
                            "isDisclosure": True,
                        },
                        {
                            "id": "22222449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/03",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/03/15 01:00",
                            "actualSurveyRequestDatetime": "2022/03/15 01:10",
                            "planSurveyResponseDatetime": "2022/03/30 17:00",
                            "actualSurveyResponseDatetime": "2022/03/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                        {
                            "id": "33333449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "service",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/04",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/04/15 01:00",
                            "actualSurveyRequestDatetime": "2022/04/15 01:10",
                            "planSurveyResponseDatetime": "2022/04/30 17:00",
                            "actualSurveyResponseDatetime": "2022/04/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_no_query_parameters_specified(
        self,
        mocker,
        mock_auth_admin,
        admin_model,
        expected,
    ):
        """クエリパラメータなし"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model

        response = client.get("/api/surveys", headers=REQUEST_HEADERS)
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "admin_model, expected",
        [
            (
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                {
                    "summary": {
                        "monthly": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                        ],
                        "accumulation": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                        ],
                        "total": {
                            "quota": {
                                "quota1": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota2": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota3": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota4": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "half": {
                                "half1": {"satisfactionAverage": 3.0, "receive": 1},
                                "half2": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "year": {"satisfactionAverage": 3.0, "receive": 1},
                        },
                    },
                    "surveys": [
                        {
                            "id": "22222449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/03",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/03/15 01:00",
                            "actualSurveyRequestDatetime": "2022/03/15 01:10",
                            "planSurveyResponseDatetime": "2022/03/30 17:00",
                            "actualSurveyResponseDatetime": "2022/03/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                        {
                            "id": "33333449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "service",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/04",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/04/15 01:00",
                            "actualSurveyRequestDatetime": "2022/04/15 01:10",
                            "planSurveyResponseDatetime": "2022/04/30 17:00",
                            "actualSurveyResponseDatetime": "2022/04/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                    ],
                },
            )
        ],
    )
    def test_query_parameters_contains_summary_month(
        self,
        mocker,
        mock_auth_admin,
        admin_model,
        expected,
    ):
        """集計月絞り込み"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])

        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model

        response = client.get(
            "/api/surveys?summaryMonthFrom=202203&summaryMonthTo=202206",
            headers=REQUEST_HEADERS,
        )
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "admin_model, expected",
        [
            (
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                {
                    "summary": {
                        "monthly": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                        ],
                        "accumulation": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                        ],
                        "total": {
                            "quota": {
                                "quota1": {"satisfactionAverage": 0.0, "receive": 0},
                                "quota2": {"satisfactionAverage": 0.0, "receive": 0},
                                "quota3": {"satisfactionAverage": 0.0, "receive": 0},
                                "quota4": {"satisfactionAverage": 0.0, "receive": 0},
                            },
                            "half": {
                                "half1": {"satisfactionAverage": 0.0, "receive": 0},
                                "half2": {"satisfactionAverage": 0.0, "receive": 0},
                            },
                            "year": {"satisfactionAverage": 0.0, "receive": 0},
                        },
                    },
                    "surveys": [
                        {
                            "id": "22222449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/03",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/03/15 01:00",
                            "actualSurveyRequestDatetime": "2022/03/15 01:10",
                            "planSurveyResponseDatetime": "2022/03/30 17:00",
                            "actualSurveyResponseDatetime": "2022/03/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                    ],
                },
            )
        ],
    )
    def test_query_parameters_contains_actual_survey_response_date(
        self,
        mocker,
        mock_auth_admin,
        admin_model,
        expected,
    ):
        """回答日絞り込み"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model

        response = client.get(
            "/api/surveys?actualSurveyResponseDateFrom=20220301&actualSurveyResponseDateTo=20220401",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "admin_model, expected",
        [
            (
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                {
                    "summary": {
                        "monthly": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                        ],
                        "accumulation": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                        ],
                        "total": {
                            "quota": {
                                "quota1": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota2": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota3": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota4": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "half": {
                                "half1": {"satisfactionAverage": 3.0, "receive": 1},
                                "half2": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "year": {"satisfactionAverage": 3.0, "receive": 1},
                        },
                    },
                    "surveys": [
                        {
                            "id": "33333449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "service",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/04",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/04/15 01:00",
                            "actualSurveyRequestDatetime": "2022/04/15 01:10",
                            "planSurveyResponseDatetime": "2022/04/30 17:00",
                            "actualSurveyResponseDatetime": "2022/04/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_query_parameters_contains_plan_survey_response_date(
        self,
        mocker,
        mock_auth_admin,
        admin_model,
        expected,
    ):
        """回答受信予定日絞り込み"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model

        response = client.get(
            "/api/surveys?planSurveyResponseDateFrom=20220429&planSurveyResponseDateTo=20220505",
            headers=REQUEST_HEADERS,
        )
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, admin_model, expected",
        [
            (
                "?projectId=111a46d3-1d56-428b-ac6d-a51e263b54a8",
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                {
                    "summary": {
                        "monthly": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                        ],
                        "accumulation": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                        ],
                        "total": {
                            "quota": {
                                "quota1": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota2": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota3": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota4": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "half": {
                                "half1": {"satisfactionAverage": 3.0, "receive": 1},
                                "half2": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "year": {"satisfactionAverage": 3.0, "receive": 1},
                        },
                    },
                    "surveys": [
                        {
                            "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/01",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/01/15 01:00",
                            "actualSurveyRequestDatetime": "2022/01/15 01:10",
                            "planSurveyResponseDatetime": "2022/01/31 17:00",
                            "actualSurveyResponseDatetime": "2022/01/30 10:00",
                            "isFinished": False,
                            "isDisclosure": True,
                        },
                        {
                            "id": "22222449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/03",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/03/15 01:00",
                            "actualSurveyRequestDatetime": "2022/03/15 01:10",
                            "planSurveyResponseDatetime": "2022/03/30 17:00",
                            "actualSurveyResponseDatetime": "2022/03/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                        {
                            "id": "33333449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "service",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/04",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/04/15 01:00",
                            "actualSurveyRequestDatetime": "2022/04/15 01:10",
                            "planSurveyResponseDatetime": "2022/04/30 17:00",
                            "actualSurveyResponseDatetime": "2022/04/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_specified_project_id(
        self, mocker, mock_auth_admin, query_param, admin_model, expected
    ):
        """案件ID絞り込み"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model

        response = client.get(f"/api/surveys{query_param}", headers=REQUEST_HEADERS)
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, admin_model, expected",
        [
            (
                "?customerId=d6121808-341d-4883-8e2c-69462acf6ccb",
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                {
                    "summary": {
                        "monthly": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                        ],
                        "accumulation": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                        ],
                        "total": {
                            "quota": {
                                "quota1": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota2": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota3": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota4": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "half": {
                                "half1": {"satisfactionAverage": 3.0, "receive": 1},
                                "half2": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "year": {"satisfactionAverage": 3.0, "receive": 1},
                        },
                    },
                    "surveys": [
                        {
                            "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/01",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/01/15 01:00",
                            "actualSurveyRequestDatetime": "2022/01/15 01:10",
                            "planSurveyResponseDatetime": "2022/01/31 17:00",
                            "actualSurveyResponseDatetime": "2022/01/30 10:00",
                            "isFinished": False,
                            "isDisclosure": True,
                        },
                        {
                            "id": "22222449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/03",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/03/15 01:00",
                            "actualSurveyRequestDatetime": "2022/03/15 01:10",
                            "planSurveyResponseDatetime": "2022/03/30 17:00",
                            "actualSurveyResponseDatetime": "2022/03/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                        {
                            "id": "33333449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "service",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/04",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/04/15 01:00",
                            "actualSurveyRequestDatetime": "2022/04/15 01:10",
                            "planSurveyResponseDatetime": "2022/04/30 17:00",
                            "actualSurveyResponseDatetime": "2022/04/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_specified_customer_id(
        self, mocker, mock_auth_admin, query_param, admin_model, expected
    ):
        """顧客ID絞り込み"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model

        response = client.get(f"/api/surveys{query_param}", headers=REQUEST_HEADERS)
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, admin_model, expected",
        [
            (
                "?serviceTypeId=7ac8bddf-88da-46c9-a504-a03d1661ad58",
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                {
                    "summary": {
                        "monthly": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                        ],
                        "accumulation": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                        ],
                        "total": {
                            "quota": {
                                "quota1": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota2": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota3": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota4": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "half": {
                                "half1": {"satisfactionAverage": 3.0, "receive": 1},
                                "half2": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "year": {"satisfactionAverage": 3.0, "receive": 1},
                        },
                    },
                    "surveys": [
                        {
                            "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/01",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/01/15 01:00",
                            "actualSurveyRequestDatetime": "2022/01/15 01:10",
                            "planSurveyResponseDatetime": "2022/01/31 17:00",
                            "actualSurveyResponseDatetime": "2022/01/30 10:00",
                            "isFinished": False,
                            "isDisclosure": True,
                        },
                        {
                            "id": "22222449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/03",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/03/15 01:00",
                            "actualSurveyRequestDatetime": "2022/03/15 01:10",
                            "planSurveyResponseDatetime": "2022/03/30 17:00",
                            "actualSurveyResponseDatetime": "2022/03/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                        {
                            "id": "33333449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "service",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/04",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/04/15 01:00",
                            "actualSurveyRequestDatetime": "2022/04/15 01:10",
                            "planSurveyResponseDatetime": "2022/04/30 17:00",
                            "actualSurveyResponseDatetime": "2022/04/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_specified_service_type_id(
        self, mocker, mock_auth_admin, query_param, admin_model, expected
    ):
        """サービス種別ID絞り込み"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model

        response = client.get(f"/api/surveys{query_param}", headers=REQUEST_HEADERS)
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, admin_model, expected",
        [
            (
                "?type=service",
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                {
                    "summary": {
                        "monthly": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 0.0,
                                "receive": 0,
                            },
                        ],
                        "accumulation": [
                            {
                                "month": "2022年4月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年5月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年6月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年7月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年8月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年9月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年10月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年11月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2022年12月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年1月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年2月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                            {
                                "month": "2023年3月",
                                "satisfactionAverage": 3.0,
                                "receive": 1,
                            },
                        ],
                        "total": {
                            "quota": {
                                "quota1": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota2": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota3": {"satisfactionAverage": 3.0, "receive": 1},
                                "quota4": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "half": {
                                "half1": {"satisfactionAverage": 3.0, "receive": 1},
                                "half2": {"satisfactionAverage": 3.0, "receive": 1},
                            },
                            "year": {"satisfactionAverage": 3.0, "receive": 1},
                        },
                    },
                    "surveys": [
                        {
                            "id": "33333449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "service",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/04",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/04/15 01:00",
                            "actualSurveyRequestDatetime": "2022/04/15 01:10",
                            "planSurveyResponseDatetime": "2022/04/30 17:00",
                            "actualSurveyResponseDatetime": "2022/04/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_specified_type(
        self, mocker, mock_auth_admin, query_param, admin_model, expected
    ):
        """アンケート種別絞り込み"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model

        response = client.get(f"/api/surveys{query_param}", headers=REQUEST_HEADERS)
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param, admin_model, expected",
        [
            (
                "?type=quick",
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                {
                    "summary": None,
                    "surveys": [
                        {
                            "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/01",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/01/15 01:00",
                            "actualSurveyRequestDatetime": "2022/01/15 01:10",
                            "planSurveyResponseDatetime": "2022/01/31 17:00",
                            "actualSurveyResponseDatetime": "2022/01/30 10:00",
                            "isFinished": False,
                            "isDisclosure": True,
                        },
                        {
                            "id": "22222449-5d63-42d4-ae1b-f0faf65a7076",
                            "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "surveyRevision": 4,
                            "surveyType": "quick",
                            "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                            "projectName": "サンプルプロジェクト",
                            "customerSuccess": "DXの実現",
                            "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "supporterOrganizationName": "IST",
                            "supportDateFrom": "2021/06/01",
                            "supportDateTo": "2022/12/31",
                            "mainSupporterUser": {
                                "id": "51ff73f4-2414-421c-b64c-9981f988b331",
                                "name": "田中 三郎",
                                "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                "organizationName": "AST",
                            },
                            "supporterUsers": [
                                {
                                    "id": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                    "name": "田中 次郎",
                                    "organizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                                    "organizationName": "AST",
                                }
                            ],
                            "salesUserId": "1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                            "salesUserName": "田中 太郎",
                            "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            "serviceTypeName": "組織開発",
                            "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                            "answerUserName": "田中 次郎",
                            "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                            "customerName": "あああ研究所",
                            "company": "ソニーグループ株式会社",
                            "summaryMonth": "2022/03",
                            "isNotSummary": False,
                            "planSurveyRequestDatetime": "2022/03/15 01:00",
                            "actualSurveyRequestDatetime": "2022/03/15 01:10",
                            "planSurveyResponseDatetime": "2022/03/30 17:00",
                            "actualSurveyResponseDatetime": "2022/03/30 10:00",
                            "isFinished": True,
                            "isDisclosure": True,
                        },
                    ],
                },
            )
        ],
    )
    def test_ok_specified_type_quick(
        self, mocker, mock_auth_admin, query_param, admin_model, expected
    ):
        """アンケート種別絞り込み"""
        mock_auth_admin([UserRoleType.MAN_HOUR_OPS.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model

        response = client.get(f"/api/surveys{query_param}", headers=REQUEST_HEADERS)
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_param",
        [
            "?summaryMonthFrom=2022013",
            "?summaryMonthTo=2022013",
            "?actualSurveyResponseDateFrom=202203011",
            "?actualSurveyResponseDateTo=202203011",
            "?planSurveyResponseDateFrom=202203011",
            "?planSurveyResponseDateTo=202203011",
            "?summaryMonthFrom=202212&actualSurveyResponseDateFrom=20221231&planSurveyResponseDateFrom=20221231",
            "?summaryMonthFrom=202303&summaryMonthTo=202204",
            "?actualSurveyResponseDateFrom=20230301&actualSurveyResponseDateTo=20220401",
            "?planSurveyResponseDateFrom=20230301&planSurveyResponseDateTo=20220401",
        ],
    )
    def test_validation(self, mock_auth_admin, query_param):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(f"/api/surveys{query_param}", headers=REQUEST_HEADERS)
        assert response.status_code == 400
