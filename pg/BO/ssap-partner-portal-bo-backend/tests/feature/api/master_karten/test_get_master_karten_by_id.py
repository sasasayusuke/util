import json
from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist
from pytz import timezone

from app.main import app
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_karte import ProjectKarteModel
from app.models.project_survey import (
    AnswersAttribute,
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.models.survey_master import (
    ChoicesSubAttribute,
    GroupSubAttribute,
    QuestionsAttribute,
    SurveyMasterModel,
)
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from app.models.admin import AdminModel
from app.utils.platform import PlatformApiOperator

client = TestClient(app)
REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetMasterKartenById:
    def test_ok(self, mocker, mock_auth_admin):
        mock_auth_admin([UserRoleType.SALES.key])
        pp_project_mock = mocker.patch.object(ProjectModel, "get")
        pp_project_mock.return_value = ProjectModel(
            id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at=timezone("Asia/Tokyo").localize(
                datetime.strptime(
                    "2020/10/23 03:21",
                    "%Y/%m/%d %H:%M",
                )
            ),
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
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="a9b67094-cdab-494c-818e-d4845088269b",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )
        mock_user = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user.return_value = iter(
            [
                UserModel(
                    id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    role="sales",
                    company="ソニー株式会社",
                    is_input_man_hour=None,
                    project_ids=["efba46d3-1d56-428b-ac6d-a51e263b54a8"],
                    agreed=True,
                    last_login_at="2022-04-25T03:21:39.356Z",
                    disabled=False,
                )
            ]
        )
        mock_admin = mocker.patch.object(AdminModel.data_type_email_index, "query")
        mock_admin.return_value = iter(
            [
                AdminModel(
                    id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="営業",
                    supporter_organization_id=[],
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SALES.key},
                    otp_verified_token="111111",
                )
            ]
        )
        pp_mock_projet_2 = mocker.patch.object(
            ProjectModel.customer_id_name_index, "query"
        )
        pp_mock_projet_2.return_value = [
            ProjectModel(
                id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                data_type=DataType.PROJECT,
                salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
                salesforce_update_at=timezone("Asia/Tokyo").localize(
                    datetime.strptime(
                        "2020/10/23 03:21",
                        "%Y/%m/%d %H:%M",
                    )
                ),
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
                customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                salesforce_main_supporter_user_name="山田太郎",
                supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
                salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                is_count_man_hour=True,
                is_karte_remind=True,
                contract_type="有償",
                is_secret=False,
                create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                create_at="2022-05-23T16:34:21.523000+0000",
                update_id="a9b67094-cdab-494c-818e-d4845088269b",
                update_at="2022-05-23T16:34:21.523000+0000",
                version=1,
            )
        ]
        survey_master_mock = mocker.patch.object(SurveyMasterModel, "get")
        survey_master_mock.return_value = SurveyMasterModel(
            id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
            revision=1,
            name="修了アンケート",
            type="completion",
            timing="monthly",
            init_send_day_setting=20,
            init_answer_limit_day_setting=5,
            is_disclosure=True,
            questions=[
                QuestionsAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    required=True,
                    description="今回利用されたプログラムについての総合的な満足度はいかがですか？",
                    format="radio",
                    summary_type="satisfaction",
                    choices=[
                        ChoicesSubAttribute(
                            description=None,
                            group=[
                                GroupSubAttribute(
                                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[5]]5：とても満足",
                                    disabled=False,
                                    is_new=False,
                                ),
                                GroupSubAttribute(
                                    id="79cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[4]]4：満足",
                                    disabled=False,
                                    is_new=False,
                                ),
                                GroupSubAttribute(
                                    id="69cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[3]]：どちらともいえない",
                                    disabled=False,
                                    is_new=False,
                                ),
                                GroupSubAttribute(
                                    id="59cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[2]]：不満",
                                    disabled=False,
                                    is_new=False,
                                ),
                                GroupSubAttribute(
                                    id="49cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[1]]1：とても不満",
                                    disabled=False,
                                    is_new=False,
                                ),
                            ],
                        ),
                    ],
                )
            ],
            question_flow=[
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "conditionId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "conditionChoiceIds": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                }
            ],
            is_latest=1,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-04-23T03:21:39.356Z",
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-06-01T11:20:04.332Z",
            version="1",
        )
        json_open = open("mock/pf/get_projects.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock.return_value = (200, json_load)
        project_survey_mock = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        project_survey_mock.return_value = [
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
                        answer="4：満足",
                        point=4,
                        choice_ids=set(
                            [
                                "79cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            ]
                        ),
                        summary_type="satisfaction",
                        other_input="",
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
                create_at="2022-05-23T16:34:21.523000+0000",
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at="2022-05-23T16:34:21.523000+0000",
                version=1,
            )
        ]

        npf_project_id = "2854fc20-caea-44bb-ada5"
        json_open = open("mock/pf/get_project_by_id.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_project_by_pf_id")
        mock.return_value = (200, json_load)
        response = client.get(
            f"/api/master-karten/{npf_project_id}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    def test_not_found(self, mock_auth_admin, mocker):
        """NPFマスターカルテ詳細取得APIで404"""

        mock_auth_admin([UserRoleType.SALES.key])
        npf_project_id = "2854fc20-caea-44bb-ada5"
        mock = mocker.patch.object(PlatformApiOperator, "get_project_by_pf_id")
        mock.return_value = (404, {"detail": {"code": 404, "message": "Not found"}})
        response = client.get(
            f"/api/master-karten/{npf_project_id}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 404

    def test_lack_of_authority(self, mock_auth_admin, mocker):
        """権限が足りないケース（担当していない非公開案件）"""
        mock_auth_admin([UserRoleType.SALES.key])
        npf_project_id = "2854fc20-caea-44bb-ada5"

        json_open = open("mock/pf/get_project_by_id.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_project_by_pf_id")
        mock.return_value = (200, json_load)

        pp_project_mock = mocker.patch.object(ProjectModel, "get")
        pp_project_mock.return_value = ProjectModel(
            id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at=timezone("Asia/Tokyo").localize(
                datetime.strptime(
                    "2020/10/23 03:21",
                    "%Y/%m/%d %H:%M",
                )
            ),
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
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            salesforce_supporter_user_names=["山田太郎", "山田次郎"],
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type="有償",
            is_secret=True,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="a9b67094-cdab-494c-818e-d4845088269b",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )
        survey_master_mock = mocker.patch.object(SurveyMasterModel, "get")
        survey_master_mock.return_value = SurveyMasterModel(
            id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
            revision=1,
            name="修了アンケート",
            type="completion",
            timing="monthly",
            init_send_day_setting=20,
            init_answer_limit_day_setting=5,
            is_disclosure=True,
            questions=[
                QuestionsAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    required=True,
                    description="今回利用されたプログラムについての総合的な満足度はいかがですか？",
                    format="radio",
                    summary_type="satisfaction",
                    choices=[
                        ChoicesSubAttribute(
                            description=None,
                            group=[
                                GroupSubAttribute(
                                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[5]]5：とても満足",
                                    disabled=False,
                                    is_new=False,
                                ),
                                GroupSubAttribute(
                                    id="79cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[4]]4：満足",
                                    disabled=False,
                                    is_new=False,
                                ),
                                GroupSubAttribute(
                                    id="69cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[3]]：どちらともいえない",
                                    disabled=False,
                                    is_new=False,
                                ),
                                GroupSubAttribute(
                                    id="59cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[2]]：不満",
                                    disabled=False,
                                    is_new=False,
                                ),
                                GroupSubAttribute(
                                    id="49cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    title="[[1]]1：とても不満",
                                    disabled=False,
                                    is_new=False,
                                ),
                            ],
                        ),
                    ],
                )
            ],
            question_flow=[
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "conditionId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "conditionChoiceIds": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                }
            ],
            is_latest=1,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2022-04-23T03:21:39.356Z",
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-06-01T11:20:04.332Z",
            version="1",
        )
        project_survey_mock = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        project_survey_mock.return_value = [
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
                        answer="4：満足",
                        point=4,
                        choice_ids=set(
                            [
                                "79cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            ]
                        ),
                        summary_type="satisfaction",
                        other_input="",
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
                create_at="2022-05-23T16:34:21.523000+0000",
                update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                update_at="2022-05-23T16:34:21.523000+0000",
                version=1,
            )
        ]

        mock_user_auth = mocker.patch.object(UserModel.data_type_email_index, "query")
        mock_user_auth.return_value = []
        response = client.get(
            f"/api/master-karten/{npf_project_id}",
            headers=REQUEST_HEADERS,
        )
        assert response.status_code == 403
