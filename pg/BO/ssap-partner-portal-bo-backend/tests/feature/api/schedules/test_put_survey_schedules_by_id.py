from datetime import datetime

import freezegun
import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_survey import ProjectSurveyModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@freezegun.freeze_time("2022-07-01T22:42:05.033Z")
class TestUpdateSurveySchedulesById:
    @pytest.mark.parametrize(
        "survey_id, version, body, expected, survey_model",
        [
            (
                "99c5bc68-246f-4450-8a50-2f23f9518025",
                1,
                {"sendDate": "2022/08/10", "surveyLimitDate": 10},
                {"message": "OK"},
                ProjectSurveyModel(
                    id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                    data_type=DataType.SURVEY,
                    plan_survey_request_datetime="2022/8/21 9:00",
                    plan_survey_response_datetime="2022/8/31 9:00",
                    version=1,
                ),
            ),
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        survey_id,
        version,
        body,
        expected,
        survey_model,
    ):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = ProjectModel(
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
            contract_date="2023/03/30",
            phase="プラン提示（D）",
            customer_success="DXの実現",
            support_date_from="2022/04/01",
            support_date_to="2022/05/31",
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
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model
        mocker.patch.object(ProjectSurveyModel, "update")

        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.put(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200

        assert actual == expected

    @pytest.mark.parametrize(
        "survey_id, version, body, expected, survey_model",
        [
            (
                "99c5bc68-246f-4450-8a50-2f23f9518025",
                1,
                {"sendDate": "2022/08/10", "surveyLimitDate": 99},
                {"message": "OK"},
                ProjectSurveyModel(
                    id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                    data_type=DataType.SURVEY,
                    plan_survey_request_datetime="2022/8/21 9:00",
                    plan_survey_response_datetime="2022/8/31 9:00",
                    version=1,
                ),
            ),
            (
                "99c5bc68-246f-4450-8a50-2f23f9518025",
                1,
                {"sendDate": "2022/08/10", "surveyLimitDate": 3},
                {"message": "OK"},
                ProjectSurveyModel(
                    id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                    data_type=DataType.SURVEY,
                    plan_survey_request_datetime="2022/8/21 9:00",
                    plan_survey_response_datetime="2022/8/31 9:00",
                    version=1,
                ),
            ),
            (
                "99c5bc68-246f-4450-8a50-2f23f9518025",
                1,
                {"sendDate": "2022/08/10", "surveyLimitDate": 0},
                {"message": "OK"},
                ProjectSurveyModel(
                    id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                    data_type=DataType.SURVEY,
                    plan_survey_request_datetime="2022/8/21 9:00",
                    plan_survey_response_datetime="2022/8/31 9:00",
                    version=1,
                ),
            ),
            (
                "99c5bc68-246f-4450-8a50-2f23f9518025",
                1,
                {"sendDate": "2022/08/10", "surveyLimitDate": 101},
                {"message": "OK"},
                ProjectSurveyModel(
                    id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                    data_type=DataType.SURVEY,
                    plan_survey_request_datetime="2022/8/21 9:00",
                    plan_survey_response_datetime="2022/8/31 9:00",
                    version=1,
                ),
            ),
        ],
    )
    def test_last_date_ok(
        self,
        mocker,
        mock_auth_admin,
        survey_id,
        version,
        body,
        expected,
        survey_model,
    ):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = ProjectModel(
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
            contract_date="2023/03/30",
            phase="プラン提示（D）",
            customer_success="DXの実現",
            support_date_from="2022/04/01",
            support_date_to="2022/05/31",
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
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        mocker_method_date = mocker.patch("app.utils.date.get_last_date_of_month")
        mocker_method_date.return_value = datetime(2022, 8, 31)

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model
        mocker.patch.object(ProjectSurveyModel, "update")

        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.put(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200

        assert actual == expected

    def test_schedules_not_found(
        self,
        mocker,
        mock_auth_admin,
    ):
        """案件スケジュールが存在しない(該当のproject_idが存在しない)時のテスト"""
        survey_id = "99c5bc68-246f-4450-8a50-2f23f9518025"
        version = 1
        body = {"sendDate": "2022/08/10", "surveyLimitDate": 10}

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(ProjectSurveyModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.put(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not Found"

    @pytest.mark.parametrize(
        "role",
        [
            (UserRoleType.SYSTEM_ADMIN.key),
            (UserRoleType.SALES.key),
            (UserRoleType.SALES_MGR.key),
            (UserRoleType.SURVEY_OPS.key),
            (UserRoleType.MAN_HOUR_OPS.key),
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

        survey_id = "99c5bc68-246f-4450-8a50-2f23f9518025"
        version = 1
        body = {"sendDate": "2022/08/10", "surveyLimitDate": 10}

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = ProjectModel(
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
            contract_date="2023/03/30",
            phase="プラン提示（D）",
            customer_success="DXの実現",
            support_date_from="2022/04/01",
            support_date_to="2022/05/31",
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
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = ProjectSurveyModel(
            id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
            data_type=DataType.SURVEY,
            plan_survey_request_datetime="2022/8/21 9:00",
            plan_survey_response_datetime="2022/8/31 9:00",
            version=1,
        )
        mocker.patch.object(ProjectSurveyModel, "update")

        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.put(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "project_model",
        [
            (
                # 自身の課の案件、公開案件
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
                    contract_date="2023/03/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/05/31",
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
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                )
            ),
            (
                # 自身の課の案件、非公開案件
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
                    contract_date="2023/03/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/05/31",
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
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                )
            ),
            (
                # 自身の課以外の案件、公開案件
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
                    contract_date="2023/03/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/05/31",
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
                )
            ),
        ],
    )
    def test_auth_supporter_mgr_ok(
        self,
        mocker,
        mock_auth_admin,
        project_model,
    ):
        """権限確認：支援者責任者、OKパターン"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        survey_id = "99c5bc68-246f-4450-8a50-2f23f9518025"
        version = 1
        body = {"sendDate": "2022/08/10", "surveyLimitDate": 10}

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = ProjectSurveyModel(
            id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
            data_type=DataType.SURVEY,
            plan_survey_request_datetime="2022/8/21 9:00",
            plan_survey_response_datetime="2022/8/31 9:00",
            version=1,
        )
        mocker.patch.object(ProjectSurveyModel, "update")

        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.put(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "project_model",
        [
            (
                # 自身の課以外の案件、非公開案件
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
                    contract_date="2023/03/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/05/31",
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
                )
            ),
            (
                # 自身の案件でない（支援者組織が未設定）、非公開案件
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
                    contract_date="2023/03/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/05/31",
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
                    supporter_organization_id="",
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
                )
            ),
        ],
    )
    def test_auth_supporter_mgr_403(
        self,
        mocker,
        mock_auth_admin,
        project_model,
    ):
        """権限確認：支援者責任者、403エラー"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        survey_id = "99c5bc68-246f-4450-8a50-2f23f9518025"
        version = 1
        body = {"sendDate": "2022/08/10", "surveyLimitDate": 10}

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = ProjectSurveyModel(
            id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
            data_type=DataType.SURVEY,
            plan_survey_request_datetime="2022/8/21 9:00",
            plan_survey_response_datetime="2022/8/31 9:00",
            version=1,
        )
        mocker.patch.object(ProjectSurveyModel, "update")

        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.put(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body",
        [
            #####################
            # sendDate
            #####################
            # 必須チェック
            {
                # "sendDate": "2022/08/18",
                "surveyLimitDate": 9,
            },
            # Noneチェック
            {
                "sendDate": None,
                "surveyLimitDate": 9,
            },
            # 形式チェック
            {
                "sendDate": "2022-08-18",
                "surveyLimitDate": 9,
            },
            # 型エラー
            {
                "sendDate": -1,
                "surveyLimitDate": 101,
            },
            #####################
            # surveyLimitDate
            #####################
            # 必須チェック
            {
                "sendDate": "2022/08/18",
                # "surveyLimitDate": 9,
            },
            # Noneチェック
            {
                "sendDate": "2022/08/18",
                "surveyLimitDate": None,
            },
            # 値チェック
            {
                "sendDate": "2022/08/18",
                "surveyLimitDate": 150,
            },
            #####################
            # 関連チェック
            #####################
            # 送信日チェック 回答期限日から○営業日前:-30～-1, 特定日(e.g. 2022/04/01) 以外はエラー
            {
                "sendDate": "-31",
                "surveyLimitDate": 9,
            },
            {
                "sendDate": "0",
                "surveyLimitDate": 9,
            },
            # sendDate(アンケート送信日時)の「回答期限日から○営業日前」とsurveyLimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
            {
                "sendDate": "-5",
                "surveyLimitDate": 9,
            },
            # sendDate(アンケート送信日時)の「回答期限日から○営業日前」とsurveyLimitDate(回答期限日)の「なし」の同時指定は不可
            {
                "sendDate": "-5",
                "surveyLimitDate": 0,
            },
            # sendDate(アンケート送信日時)の「回答期限日から○営業日前」とsurveyLimitDate(回答期限日)の「月末最終営業日」の同時指定は不可
            {
                "sendDate": "-5",
                "surveyLimitDate": 99,
            },
            # 回答期限日チェック 99:月末最終営業日、1～30: ○営業日後, 0: なし, 101～130: 翌月月初○営業日(営業日に+100した数値)以外はエラー
            {
                "sendDate": "2022/08/18",
                "surveyLimitDate": 31,
            },
            {
                "sendDate": "2022/08/18",
                "surveyLimitDate": 100,
            },
            {
                "sendDate": "2022/08/18",
                "surveyLimitDate": 131,
            },
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        survey_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        response = client.put(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 422
