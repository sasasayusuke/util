import freezegun
import pytest
from datetime import datetime
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist
from moto import mock_dynamodb

from app.main import app
from app.models.master import MasterSupporterOrganizationModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_survey import ProjectSurveyModel, PointsAttribute
from app.models.survey_master import (
    SurveyMasterModel,
    QuestionsAttribute,
    QuestionFlowAttribute,
)
from app.models.user import UserModel
from app.resources.const import DataType, MasterDataType, SurveyType, UserRoleType
from app.service.common_service.cached_db_items import CachedDbItems

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
@freezegun.freeze_time("2022-08-01 22:42:05", tz_offset=+9)
class TestUpdateMultipleSurveySchedules:
    def setup_method(self, method):
        ProjectModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        project_models = [
            ProjectModel(
                id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                data_type=DataType.PROJECT,
                salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                name="サンプルプロジェクト",
                customer_name="ソニーグループ株式会社",
                service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                create_new=True,
                continued=True,
                main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                contract_date="2021/01/30",
                phase="プラン提示(D)",
                customer_success="DXの実現",
                support_date_from="2022/06/01",
                support_date_to="2022/12/31",
                profit=GrossProfitAttribute(
                    monthly=[],
                    quarterly=[],
                    half=[],
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
                customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                salesforce_main_supporter_user_name="山田太郎",
                supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                survey_group_id="111-456-789",
                is_count_man_hour=True,
                is_karte_remind=True,
                contract_type="有償",
                is_secret=False,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                # version=1,
            ),
            ProjectModel(
                id="afba46d3-1d56-428b-ac6d-a51e263b54a8",
                data_type=DataType.PROJECT,
                salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                name="サンプルプロジェクト",
                customer_name="ソニーグループ株式会社",
                service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                create_new=True,
                continued=True,
                main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                contract_date="2021/01/30",
                phase="プラン提示(D)",
                customer_success="DXの実現",
                support_date_from="2022/06/01",
                support_date_to="2022/12/31",
                profit=GrossProfitAttribute(
                    monthly=[],
                    quarterly=[],
                    half=[],
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
                customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                salesforce_main_supporter_user_name="山田太郎",
                supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                survey_group_id="222-456-789",
                is_count_man_hour=True,
                is_karte_remind=True,
                contract_type="有償",
                is_secret=False,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                # version=1,
            ),
            ProjectModel(
                id="bfba46d3-1d56-428b-ac6d-a51e263b54a8",
                data_type=DataType.PROJECT,
                salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                name="サンプルプロジェクト",
                customer_name="ソニーグループ株式会社",
                service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                create_new=True,
                continued=True,
                main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                contract_date="2021/01/30",
                phase="プラン提示(D)",
                customer_success="DXの実現",
                support_date_from="2022/06/01",
                support_date_to="2022/12/31",
                profit=GrossProfitAttribute(
                    monthly=[],
                    quarterly=[],
                    half=[],
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
                customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                salesforce_main_supporter_user_name="山田太郎",
                supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                survey_group_id="111-456-789",
                is_count_man_hour=True,
                is_karte_remind=True,
                contract_type="有償",
                is_secret=False,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                # version=1,
            ),
            ProjectModel(
                id="cfba46d3-1d56-428b-ac6d-a51e263b54a8",
                data_type=DataType.PROJECT,
                salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                name="サンプルプロジェクト",
                customer_name="ソニーグループ株式会社",
                service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                create_new=True,
                continued=True,
                main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                contract_date="2021/01/30",
                phase="プラン提示(D)",
                customer_success="DXの実現",
                support_date_from="2022/06/01",
                support_date_to="2022/12/31",
                profit=GrossProfitAttribute(
                    monthly=[],
                    quarterly=[],
                    half=[],
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
                customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                salesforce_main_supporter_user_name="山田太郎",
                supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                survey_group_id="111-456-789",
                is_count_man_hour=True,
                is_karte_remind=True,
                contract_type="有償",
                is_secret=False,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                # version=1,
            ),
        ]

        for project_model in project_models:
            project_model.save()

        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        survey_models = [
            ProjectSurveyModel(
                id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SURVEY,
                survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                survey_master_revision=123,
                survey_type=SurveyType.SERVICE,
                project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                project_name="サンプルプロジェクト",
                customer_success="",
                supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                service_type_name="quick",
                answer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                answer_user_name="田中 次郎",
                customer_id="ddd67094-cdab-494c-818e-d4845088269b",
                customer_name="テスト カスタマー",
                points=PointsAttribute(
                    satisfaction=0,
                    continuation=0,
                    recommended=0,
                    sales=0,
                    survey_satisfaction=0,
                    man_hour_satisfaction=0,
                    karte_satisfaction=0,
                    master_karte_satisfaction=0,
                ),
                summary_month="2022/08",
                plan_survey_request_datetime="2022/08/20 09:00",
                plan_survey_response_datetime="2022/08/30 09:00",
                survey_group_id="1259c946-2748-4b15-b374-c159266c0617",
                create_id="bcb67094-cdab-494c-818e-d4845088269b",
                create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                update_id="bcb67094-cdab-494c-818e-d4845088269b",
                update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                # version=1,
            ),
            ProjectSurveyModel(
                id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SURVEY,
                survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                survey_master_revision=123,
                survey_type=SurveyType.SERVICE,
                project_id="afba46d3-1d56-428b-ac6d-a51e263b54a8",
                project_name="サンプルプロジェクト",
                customer_success="",
                supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                service_type_name="quick",
                answer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                answer_user_name="田中 次郎",
                customer_id="ddd67094-cdab-494c-818e-d4845088269b",
                customer_name="テスト カスタマー",
                points=PointsAttribute(
                    satisfaction=0,
                    continuation=0,
                    recommended=0,
                    sales=0,
                    survey_satisfaction=0,
                    man_hour_satisfaction=0,
                    karte_satisfaction=0,
                    master_karte_satisfaction=0,
                ),
                summary_month="2022/08",
                plan_survey_request_datetime="2022/08/20 09:00",
                plan_survey_response_datetime="2022/08/30 09:00",
                create_id="bcb67094-cdab-494c-818e-d4845088269b",
                create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                update_id="bcb67094-cdab-494c-818e-d4845088269b",
                update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                # version=1,
            ),
        ]

        for survey_model in survey_models:
            survey_model.save()

        SurveyMasterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        survey_manster_models = [
            SurveyMasterModel(
                id="298cfbb7-af80-4263-bd8e-71992516f9d8",
                revision=1,
                name="サービスアンケート",
                type="service",
                timing="monthly",
                init_send_day_setting=20,
                init_answer_limit_day_setting=5,
                is_disclosure=True,
                questions=[
                    QuestionsAttribute(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        required=False,
                        description="当月のSSAP支援にはご満足いただけましたか？",
                        format="checkbox",
                        disabled=False,
                        is_new=True,
                    )
                ],
                question_flow=[
                    QuestionFlowAttribute(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        condition_choice_ids=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        ],
                    )
                ],
                is_latest=1,
                create_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
            ),
            SurveyMasterModel(
                id="6eceb76d-b0dc-464d-94c2-ab0af8e765ba",
                revision=1,
                name="修了アンケート",
                type="completion",
                timing="completion_month",
                init_send_day_setting=24,
                init_answer_limit_day_setting=10,
                is_disclosure=True,
                questions=[
                    QuestionsAttribute(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        required=False,
                        description="当月のSSAP支援にはご満足いただけましたか？",
                        format="checkbox",
                        disabled=False,
                        is_new=True,
                    )
                ],
                question_flow=[
                    QuestionFlowAttribute(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        condition_choice_ids=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        ],
                    )
                ],
                is_latest=1,
                create_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
            ),
        ]

        for survey_manster_model in survey_manster_models:
            survey_manster_model.save()

        UserModel.create_table(read_capacity_units=5, write_capacity_units=5, wait=True)

        user_models = [
            UserModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="田中太郎",
                email="user@example.com",
                role=UserRoleType.SUPPORTER.key,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company=None,
                supporter_organization_id=["180a3597-b7e7-42c8-902c-a29016afa662"],
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                # version=1,
            ),
            UserModel(
                id="b9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="田中次郎",
                email="user@example.com",
                role=UserRoleType.SUPPORTER.key,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company=None,
                supporter_organization_id=["180a3597-b7e7-42c8-902c-a29016afa662"],
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                # version=1,
            ),
            UserModel(
                id="c9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="田中三郎",
                email="user@example.com",
                role=UserRoleType.SUPPORTER.key,
                customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                customer_name="ソニーグループ株式会社",
                job="部長",
                company=None,
                supporter_organization_id=["180a3597-b7e7-42c8-902c-a29016afa662"],
                is_input_man_hour=None,
                project_ids=None,
                cognito_id=None,
                agreed=False,
                last_login_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                # version=1,
            ),
        ]

        for user_model in user_models:
            user_model.save()

        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        master_supporter_organization_models = [
            MasterSupporterOrganizationModel(
                id="180a3597-b7e7-42c8-902c-a29016afa662",
                data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                name="Business Incubation Team",
                value="BIT",
                order=4,
                use=True,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                # version=1,
            ),
            MasterSupporterOrganizationModel(
                id="de40733f-6be9-4fef-8229-01052f43c1e2",
                data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
                name="Visualization Service Team 2 Group",
                value="VST2",
                order=3,
                use=True,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                # version=1,
            ),
            MasterSupporterOrganizationModel(
                id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                name="組織開発",
                value="1",
                order=1,
                use=True,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                # version=1,
            ),
            MasterSupporterOrganizationModel(
                id="bad0b57c-de29-48e0-9deb-94be98437441",
                data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                name="アイデア可視化",
                value="2",
                order=2,
                use=True,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                # version=1,
            ),
            MasterSupporterOrganizationModel(
                id="1cdf118f-662d-4ae1-8765-f87f94ba345b",
                data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
                name="事業育成＆検証",
                value="3",
                order=3,
                use=True,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at=datetime(2022, 4, 23, 16, 34, 21, 523),
                # version=1,
            ),
        ]

        for master_supporter_organization_model in master_supporter_organization_models:
            master_supporter_organization_model.save()

    def teardown_method(self, method):
        ProjectModel.delete_table()
        ProjectSurveyModel.delete_table()
        SurveyMasterModel.delete_table()
        UserModel.delete_table()
        MasterSupporterOrganizationModel.delete_table()

    @pytest.mark.parametrize(
        "body, expected",
        [
            (
                {
                    # 特定日(e.g. 2022/04/01)
                    "sendDate": "2022/08/21",
                    # 99:月末最終営業日
                    "surveyLimitDate": 99,
                    "add": [
                        {"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        {"projectId": "cfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                    ],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                {"message": "OK"},
            ),
            (
                {
                    # 特定日(e.g. 2022/04/01)
                    "sendDate": "2022/08/21",
                    # 1～30: ○営業日後
                    "surveyLimitDate": 1,
                    "add": [
                        {"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        {"projectId": "cfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                    ],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                {"message": "OK"},
            ),
            (
                {
                    # 特定日(e.g. 2022/04/01)
                    "sendDate": "2022/08/21",
                    # 1～30: ○営業日後
                    "surveyLimitDate": 30,
                    "add": [
                        {"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        {"projectId": "cfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                    ],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                {"message": "OK"},
            ),
            (
                {
                    # 特定日(e.g. 2022/04/01)
                    "sendDate": "2022/08/21",
                    # 0: なし
                    "surveyLimitDate": 0,
                    "add": [
                        {"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        {"projectId": "cfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                    ],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                {"message": "OK"},
            ),
            (
                {
                    # 特定日(e.g. 2022/04/01)
                    "sendDate": "2022/08/21",
                    # 101～130: 翌月月初○営業日(営業日に+100した数値)
                    "surveyLimitDate": 101,
                    "add": [
                        {"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        {"projectId": "cfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                    ],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                {"message": "OK"},
            ),
            (
                {
                    # 特定日(e.g. 2022/04/01)
                    "sendDate": "2022/08/21",
                    # 101～130: 翌月月初○営業日(営業日に+100した数値)
                    "surveyLimitDate": 130,
                    "add": [
                        {"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        {"projectId": "cfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                    ],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                {"message": "OK"},
            ),
            (
                {
                    # 回答期限日から○営業日前:-30～-1
                    "sendDate": "-1",
                    # 101～130: 翌月月初○営業日(営業日に+100した数値)
                    "surveyLimitDate": 101,
                    "add": [
                        {"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        {"projectId": "cfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                    ],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                {"message": "OK"},
            ),
            (
                {
                    # 回答期限日から○営業日前:-30～-1
                    "sendDate": "-30",
                    # 101～130: 翌月月初○営業日(営業日に+100した数値)
                    "surveyLimitDate": 130,
                    "add": [
                        {"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        {"projectId": "cfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                    ],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                {"message": "OK"},
            ),
            (
                {
                    "sendDate": "2022/08/21",
                    "surveyLimitDate": 99,
                    "add": [],
                    # 更新のみ
                    "update": [{"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"}],
                },
                {"message": "OK"},
            ),
            (
                {
                    "sendDate": "2022/08/21",
                    "surveyLimitDate": 99,
                    # 追加のみ
                    "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                    "update": [],
                },
                {"message": "OK"},
            ),
            (
                {
                    "sendDate": "2022/08/21",
                    "surveyLimitDate": 99,
                    # 追加、更新共に指定なし
                    "add": [],
                    "update": [],
                },
                {"message": "OK"},
            ),
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        body,
        expected,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            "/api/schedules/survey/multiple",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "role",
        [
            (UserRoleType.SYSTEM_ADMIN.key),
            (UserRoleType.SURVEY_OPS.key),
        ],
    )
    def test_auth_ok(
        self,
        mocker,
        mock_auth_admin,
        role,
    ):
        """ロール別権限テスト：制限なし"""
        mock_auth_admin([role])

        body = {
            "sendDate": "2022/08/21",
            "surveyLimitDate": 99,
            "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
            "update": [
                {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
            ],
        }

        response = client.put(
            "/api/schedules/survey/multiple",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "role",
        [
            (UserRoleType.SALES.key),
            (UserRoleType.SALES_MGR.key),
            (UserRoleType.SUPPORTER_MGR.key),
            (UserRoleType.MAN_HOUR_OPS.key),
        ],
    )
    def test_auth_ng_403(
        self,
        mocker,
        mock_auth_admin,
        role,
    ):
        """ロール別権限テスト：アクセス不可"""
        mock_auth_admin([role])

        body = {
            "sendDate": "2022/08/21",
            "surveyLimitDate": 99,
            "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
            "update": [
                {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
            ],
        }

        response = client.put(
            "/api/schedules/survey/multiple",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body",
        [
            (
                {
                    "sendDate": "2022/08/21",
                    "surveyLimitDate": 99,
                    # 存在しない案件ID
                    "add": [
                        {"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        {"projectId": "xxxx46d3-1d56-428b-ac6d-a51e263b54a8"},
                    ],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                }
            ),
        ],
    )
    def test_ng_not_found_of_project_id(
        self,
        mocker,
        mock_auth_admin,
        body,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            "/api/schedules/survey/multiple",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        error_project_id = "xxxx46d3-1d56-428b-ac6d-a51e263b54a8"
        assert response.status_code == 404
        assert (
            actual["detail"] == f"Project id not found. project_id:{error_project_id}"
        )

    @pytest.mark.parametrize(
        "body",
        [
            (
                {
                    "sendDate": "2022/08/21",
                    "surveyLimitDate": 99,
                    "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        # 存在しないアンケートID
                        {"surveyId": "xxxxx2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                }
            ),
        ],
    )
    def test_ng_not_found_of_survey_id(
        self,
        mocker,
        mock_auth_admin,
        body,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            "/api/schedules/survey/multiple",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        error_survey_id = "xxxxx2ed-f44c-4a1c-9408-c67b0ca2270d"
        assert response.status_code == 404
        assert actual["detail"] == f"Survey id not found. survey_id:{error_survey_id}"

    @pytest.mark.parametrize(
        "body",
        [
            (
                {
                    # エラー（送信日が実行日以前）
                    "sendDate": "2022/07/31",
                    "surveyLimitDate": 99,
                    "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                }
            ),
        ],
    )
    def test_ng_bad_request_of_send_date_that_is_earlier_than_the_current_date(
        self,
        mocker,
        mock_auth_admin,
        body,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            "/api/schedules/survey/multiple",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        error_calc_date = "2022/07/31"
        assert response.status_code == 400
        assert (
            actual["detail"]
            == f"You cannot set a send date that is earlier than the current date. send_date(or calc_date):{error_calc_date}"
        )

    @pytest.mark.parametrize(
        "body, survey_model_list",
        [
            (
                {
                    "sendDate": "2022/08/21",
                    "surveyLimitDate": 99,
                    "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                [
                    ProjectSurveyModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                        survey_master_revision=123,
                        survey_type=SurveyType.SERVICE,
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_name="quick",
                        answer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="田中 次郎",
                        customer_id="ddd67094-cdab-494c-818e-d4845088269b",
                        customer_name="テスト カスタマー",
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=0,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        summary_month="2022/08",
                        plan_survey_request_datetime="2022/08/20 09:00",
                        plan_survey_response_datetime="2022/08/30 09:00",
                        create_id="bcb67094-cdab-494c-818e-d4845088269b",
                        create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        update_id="bcb67094-cdab-494c-818e-d4845088269b",
                        update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        # version=1,
                    ),
                    ProjectSurveyModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                        survey_master_revision=123,
                        survey_type=SurveyType.SERVICE,
                        project_id="afba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_name="quick",
                        answer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="田中 次郎",
                        customer_id="ddd67094-cdab-494c-818e-d4845088269b",
                        customer_name="テスト カスタマー",
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=0,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        summary_month="2022/08",
                        plan_survey_request_datetime="2022/08/20 09:00",
                        plan_survey_response_datetime="2022/08/30 09:00",
                        # 送信済
                        actual_survey_request_datetime="2022/08/20 09:00",
                        create_id="bcb67094-cdab-494c-818e-d4845088269b",
                        create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        update_id="bcb67094-cdab-494c-818e-d4845088269b",
                        update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        # version=1,
                    ),
                ],
            ),
        ],
    )
    def test_ng_bad_request_of_surveys_have_been_sent(
        self,
        mocker,
        mock_auth_admin,
        body,
        survey_model_list,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        ProjectSurveyModel.delete_table()
        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for survey_model in survey_model_list:
            survey_model.save()

        response = client.put(
            "/api/schedules/survey/multiple",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        error_survey_id = "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        assert response.status_code == 400
        assert (
            actual["detail"]
            == f"Surveys that have been sent cannot be updated. survey_id:{error_survey_id}"
        )

    @pytest.mark.parametrize(
        "body, survey_model_list",
        [
            (
                {
                    "sendDate": "2022/08/21",
                    "surveyLimitDate": 99,
                    "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                    "update": [
                        {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    ],
                },
                [
                    ProjectSurveyModel(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                        survey_master_revision=123,
                        survey_type=SurveyType.SERVICE,
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_name="quick",
                        answer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="田中 次郎",
                        customer_id="ddd67094-cdab-494c-818e-d4845088269b",
                        customer_name="テスト カスタマー",
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=0,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        summary_month="2022/08",
                        plan_survey_request_datetime="2022/08/20 09:00",
                        plan_survey_response_datetime="2022/08/30 09:00",
                        create_id="bcb67094-cdab-494c-818e-d4845088269b",
                        create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        update_id="bcb67094-cdab-494c-818e-d4845088269b",
                        update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        # version=1,
                    ),
                    ProjectSurveyModel(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                        survey_master_revision=123,
                        # 対象外のアンケート種別
                        survey_type=SurveyType.QUICK,
                        project_id="afba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_name="quick",
                        answer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="田中 次郎",
                        customer_id="ddd67094-cdab-494c-818e-d4845088269b",
                        customer_name="テスト カスタマー",
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=0,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        summary_month="2022/08",
                        plan_survey_request_datetime="2022/08/20 09:00",
                        plan_survey_response_datetime="2022/08/30 09:00",
                        create_id="bcb67094-cdab-494c-818e-d4845088269b",
                        create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        update_id="bcb67094-cdab-494c-818e-d4845088269b",
                        update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        # version=1,
                    ),
                ],
            ),
        ],
    )
    def test_ng_bad_request_of_survey_type(
        self,
        mocker,
        mock_auth_admin,
        body,
        survey_model_list,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        ProjectSurveyModel.delete_table()
        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for survey_model in survey_model_list:
            survey_model.save()

        response = client.put(
            "/api/schedules/survey/multiple",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        error_survey_id = "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        error_survey_type = SurveyType.QUICK
        assert response.status_code == 400
        assert (
            actual["detail"]
            == f"Only surveys whose survey type is service and completion can be specified. survey_id:{error_survey_id}, survey_type:{error_survey_type.value}"
        )

    @pytest.mark.parametrize(
        "body",
        [
            #####################
            # sendDate
            #####################
            # 必須
            {
                # "sendDate": "2022/08/21",
                "surveyLimitDate": 99,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            # None
            {
                "sendDate": None,
                "surveyLimitDate": 99,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            # 型エラー
            {
                "sendDate": -1,
                "surveyLimitDate": 101,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            # 回答期限日から○営業日前:-30～-1, 特定日(e.g. 2022/04/01) 以外はエラー
            {
                "sendDate": "0",
                "surveyLimitDate": 99,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            {
                "sendDate": "-31",
                "surveyLimitDate": 99,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            #####################
            # surveyLimitDate
            #####################
            # 必須
            {
                "sendDate": "2022/08/21",
                # "surveyLimitDate": 99,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            # None
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": None,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            # 99:月末最終営業日、1～30: ○営業日後, 0: なし, 101～130: 翌月月初○営業日(営業日に+100した数値)以外はエラー
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": -1,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 31,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 98,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 100,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 131,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            #####################
            # add
            #####################
            # 必須
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 99,
                # "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            # None
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 99,
                "add": None,
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            #####################
            # add.projectId
            #####################
            # None
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 99,
                "add": [{"projectId": None}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            #####################
            # update
            #####################
            # 必須
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 99,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                # "update": [
                #     {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                #     {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                # ],
            },
            # None
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 99,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": None,
            },
            #####################
            # update.surveyId
            #####################
            # None
            {
                "sendDate": "2022/08/21",
                "surveyLimitDate": 99,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": None},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            #####################
            # 関連チェック
            #####################
            # sendDate(アンケート送信日時)の「回答期限日から○営業日前」とsurveyLimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
            {
                "sendDate": "-10",
                "surveyLimitDate": 5,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            # sendDate(アンケート送信日時)の「回答期限日から○営業日前」とsurveyLimitDate(回答期限日)の「なし」の同時指定は不可
            {
                "sendDate": "-10",
                "surveyLimitDate": 0,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
            # sendDate(アンケート送信日時)の「回答期限日から○営業日前」とsurveyLimitDate(回答期限日)の「月末最終営業日」の同時指定は不可
            {
                "sendDate": "-10",
                "surveyLimitDate": 99,
                "add": [{"projectId": "bfba46d3-1d56-428b-ac6d-a51e263b54a8"}],
                "update": [
                    {"surveyId": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    {"surveyId": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                ],
            },
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            "/api/schedules/survey/multiple",
            json=body,
            headers=REQUEST_HEADERS,
        )
        assert response.status_code == 422
