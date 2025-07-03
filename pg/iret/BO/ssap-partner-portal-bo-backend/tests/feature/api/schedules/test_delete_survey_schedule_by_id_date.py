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
from app.resources.const import DataType, SurveyType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@freezegun.freeze_time("2022-08-01T22:42:05.033Z")
class TestDeleteSupportScheduleByIdDate:
    @pytest.mark.parametrize(
        "version, survey_id, project_model, expected, project_survey_model",
        [
            (
                1,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                    survey_group_id="123-456-789",
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                {"message": "OK"},
                ProjectSurveyModel(
                    id="88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SURVEY,
                    survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                    survey_master_revision=123,
                    survey_type=SurveyType.QUICK,
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
                    points="",
                    summary_month="2022/07",
                    plan_survey_request_datetime="2022/09/20 09:00",
                    plan_survey_response_datetime="2022/10/20 09:00",
                    create_id="bcb67094-cdab-494c-818e-d4845088269b",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        version,
        survey_id,
        project_model,
        expected,
        project_survey_model,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        # delete 実行
        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = project_survey_model
        mocker.patch.object(ProjectSurveyModel, "delete")

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.delete(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "version, survey_id, project_model, expected, project_survey_model",
        [
            (
                1,
                "cccbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                ),
                {"message": "OK"},
                ProjectSurveyModel(
                    id="88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SURVEY,
                    survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                    survey_master_revision=123,
                    survey_type=SurveyType.QUICK,
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
                    points="",
                    summary_month="2022/07",
                    plan_survey_request_datetime="2022/09/20 09:00",
                    plan_survey_response_datetime="2022/10/20 09:00",
                    create_id="bcb67094-cdab-494c-818e-d4845088269b",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_schedule_not_found(
        self,
        mocker,
        mock_auth_admin,
        version,
        survey_id,
        project_model,
        expected,
        project_survey_model,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        # delete 実行
        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.side_effect = DoesNotExist()
        mocker.patch.object(ProjectSurveyModel, "delete")

        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.delete(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not Found"

    @pytest.mark.parametrize(
        "version, survey_id, project_model, expected, project_survey_model",
        [
            (
                3,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                ),
                {"message": "OK"},
                ProjectSurveyModel(
                    id="88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SURVEY,
                    survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                    survey_master_revision=123,
                    survey_type=SurveyType.QUICK,
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
                    points="",
                    summary_month="2022/07",
                    plan_survey_request_datetime="2022/09/20 09:00",
                    plan_survey_response_datetime="2022/10/20 09:00",
                    create_id="bcb67094-cdab-494c-818e-d4845088269b",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_version_conflict(
        self,
        mocker,
        mock_auth_admin,
        version,
        survey_id,
        project_model,
        expected,
        project_survey_model,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        # delete 実行
        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = project_survey_model
        mocker.patch.object(ProjectSurveyModel, "delete")

        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.delete(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"

    @pytest.mark.parametrize(
        "version, survey_id, project_model, expected, project_survey_model",
        [
            (
                1,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                ),
                {"message": "OK"},
                ProjectSurveyModel(
                    id="88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    data_type=DataType.SURVEY,
                    survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
                    survey_master_revision=123,
                    survey_type=SurveyType.QUICK,
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
                    points="",
                    summary_month="2022/07",
                    plan_survey_request_datetime="2022/07/20 09:00",
                    plan_survey_response_datetime="2022/10/20 09:00",
                    create_id="bcb67094-cdab-494c-818e-d4845088269b",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
        ],
    )
    def test_bad_request_date(
        self,
        mocker,
        mock_auth_admin,
        version,
        survey_id,
        project_model,
        expected,
        project_survey_model,
    ):
        # 権限チェック用
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        # delete 実行
        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = project_survey_model
        mocker.patch.object(ProjectSurveyModel, "delete")

        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.delete(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 400
        assert actual["detail"] == "Bad Request"

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

        version = 1
        survey_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

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
            survey_group_id="123-456-789",
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

        # delete 実行
        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = ProjectSurveyModel(
            id="88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SURVEY,
            survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
            survey_master_revision=123,
            survey_type=SurveyType.QUICK,
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
            points="",
            summary_month="2022/07",
            plan_survey_request_datetime="2022/09/20 09:00",
            plan_survey_response_datetime="2022/10/20 09:00",
            create_id="bcb67094-cdab-494c-818e-d4845088269b",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )
        mocker.patch.object(ProjectSurveyModel, "delete")

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
            survey_group_id="123-456-789",
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
        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.delete(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
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
                    survey_group_id="9d6849b6-6214-4fb5-8bc6-3675a6bc2409",
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
                    survey_group_id="9d6849b6-6214-4fb5-8bc6-3675a6bc2409",
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

        version = 1
        survey_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        # delete 実行
        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = ProjectSurveyModel(
            id="88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SURVEY,
            survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
            survey_master_revision=123,
            survey_type=SurveyType.QUICK,
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
            points="",
            summary_month="2022/07",
            plan_survey_request_datetime="2022/09/20 09:00",
            plan_survey_response_datetime="2022/10/20 09:00",
            create_id="bcb67094-cdab-494c-818e-d4845088269b",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )
        mocker.patch.object(ProjectSurveyModel, "delete")

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.delete(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "project_model",
        [
            (
                # 自身の課以外の案件
                ProjectModel(
                    id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                    survey_group_id="9d6849b6-6214-4fb5-8bc6-3675a6bc2409",
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
                # 自身の案件でない（支援者組織が未設定）
                ProjectModel(
                    id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                    survey_group_id="9d6849b6-6214-4fb5-8bc6-3675a6bc2409",
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

        version = 1
        survey_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        # delete 実行
        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = mock_survey.return_value = ProjectSurveyModel(
            id="88cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.SURVEY,
            survey_master_id="123456-f44c-4a1c-9408-c67b0ca2270d",
            survey_master_revision=123,
            survey_type=SurveyType.QUICK,
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
            points="",
            summary_month="2022/07",
            plan_survey_request_datetime="2022/09/20 09:00",
            plan_survey_response_datetime="2022/10/20 09:00",
            create_id="bcb67094-cdab-494c-818e-d4845088269b",
            create_at="2022-05-23T16:34:21.523000+0000",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )
        mocker.patch.object(ProjectSurveyModel, "delete")

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mock_survey_group_id_check = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_group_id_check.return_value = []
        mocker.patch.object(ProjectModel, "update")

        response = client.delete(
            f"/api/schedules/survey?surveyId={survey_id}&version={version}",
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403
