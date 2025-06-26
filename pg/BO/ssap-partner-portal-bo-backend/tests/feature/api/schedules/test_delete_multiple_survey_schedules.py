import freezegun
import pytest
from datetime import datetime
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_survey import ProjectSurveyModel, PointsAttribute
from app.resources.const import DataType, SurveyType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
@freezegun.freeze_time("2022-08-01 22:42:05", tz_offset=+9)
class TestDeleteMultipleSurveySchedules:
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
                support_date_from="2021/01/30",
                support_date_to="2021/02/28",
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
                supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                support_date_from="2021/01/30",
                support_date_to="2021/02/28",
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
                supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
        ]

        for project_model in project_models:
            project_model.save()

        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        survey_models = [
            ProjectSurveyModel(
                id="88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SURVEY,
                survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                survey_master_revision=123,
                survey_type=SurveyType.SERVICE,
                project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                project_name="サンプルプロジェクト",
                customer_success="",
                supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                summary_month="2022/09",
                plan_survey_request_datetime="2022/09/20 09:00",
                plan_survey_response_datetime="2022/10/20 09:00",
                survey_group_id="1259c946-2748-4b15-b374-c159266c0617",
                create_id="bcb67094-cdab-494c-818e-d4845088269b",
                create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                update_id="bcb67094-cdab-494c-818e-d4845088269b",
                update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                # version=1,
            ),
            ProjectSurveyModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type=DataType.SURVEY,
                survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                survey_master_revision=123,
                survey_type=SurveyType.SERVICE,
                project_id="afba46d3-1d56-428b-ac6d-a51e263b54a8",
                project_name="サンプルプロジェクト",
                customer_success="",
                supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                summary_month="2022/09",
                plan_survey_request_datetime="2022/09/20 09:00",
                plan_survey_response_datetime="2022/10/20 09:00",
                create_id="bcb67094-cdab-494c-818e-d4845088269b",
                create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                update_id="bcb67094-cdab-494c-818e-d4845088269b",
                update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                # version=1,
            ),
        ]

        for survey_model in survey_models:
            survey_model.save()

    def teardown_method(self, method):
        ProjectModel.delete_table()
        ProjectSurveyModel.delete_table()

    @pytest.mark.parametrize(
        "body, expected",
        [
            (
                {
                    "surveyIds": [
                        "88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ]
                },
                {"message": "OK"},
            ),
            (
                # 指定なし
                {"surveyIds": []},
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
            "/api/schedules/survey/multiple/delete",
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
            "surveyIds": [
                "88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ]
        }

        response = client.put(
            "/api/schedules/survey/multiple/delete",
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
            "surveyIds": [
                "88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            ]
        }

        response = client.put(
            "/api/schedules/survey/multiple/delete",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body, survey_model_list",
        [
            (
                {
                    "surveyIds": [
                        "88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ]
                },
                [
                    ProjectSurveyModel(
                        id="88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                        survey_master_revision=123,
                        survey_type=SurveyType.SERVICE,
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="",
                        supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        summary_month="2022/09",
                        plan_survey_request_datetime="2022/09/20 09:00",
                        plan_survey_response_datetime="2022/10/20 09:00",
                        # 送信済
                        actual_survey_request_datetime="2022/09/20 09:00",
                        create_id="bcb67094-cdab-494c-818e-d4845088269b",
                        create_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        update_id="bcb67094-cdab-494c-818e-d4845088269b",
                        update_at=datetime(2022, 5, 23, 16, 34, 21, 523),
                        # version=1,
                    ),
                    ProjectSurveyModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type=DataType.SURVEY,
                        survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                        survey_master_revision=123,
                        survey_type=SurveyType.SERVICE,
                        project_id="afba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="",
                        supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                        summary_month="2022/09",
                        plan_survey_request_datetime="2022/09/20 09:00",
                        plan_survey_response_datetime="2022/10/20 09:00",
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
    def test_ng_bad_request_surveys_have_been_sent(
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
            "/api/schedules/survey/multiple/delete",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 400
