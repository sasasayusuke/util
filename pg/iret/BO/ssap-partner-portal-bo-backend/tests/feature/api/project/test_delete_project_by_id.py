import json
import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist
from pynamodb.models import BatchWrite

from app.main import app
from app.models.admin import AdminModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_karte import ProjectKarteModel
from app.models.project_survey import (
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from app.utils.platform import PlatformApiOperator

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestDeleteProjectById:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_not_access(
        self,
        mock_auth_admin,
        role_types,
    ):
        """権限のないロールはアクセス不可"""
        mock_auth_admin(role_types)
        response = client.delete(
            "/api/projects/1?version=1", headers=REQUEST_HEADERS
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "project_id, version, project_model, expected, project_karte_model_list, project_survey_model_list, user_model_list",
        [
            (
                "efba46d3-1d56-428b-ac6d-a51e263b54a8",
                1,
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
                ),
                {"message": "OK"},
                [
                    ProjectKarteModel(
                        karte_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        start_datetime="2022/01/20 10:00",
                        start_time=600,
                        end_time=780,
                        supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        last_update_datetime="2020-10-23T03:21:39.356000+0000",
                        customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        man_hour=0,
                        detail=None,
                        feedback=None,
                        homework=None,
                        documents=None,
                        deliverables=None,
                        memo=None,
                        task=None,
                        is_draft=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectKarteModel(
                        karte_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        start_datetime="2022/01/20 10:00",
                        start_time=600,
                        end_time=780,
                        supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        last_update_datetime="2020-10-23T03:21:39.356000+0000",
                        customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        man_hour=0,
                        detail=None,
                        feedback=None,
                        homework=None,
                        documents=None,
                        deliverables=None,
                        memo=None,
                        task=None,
                        is_draft=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="c9b67094-cdab-494c-818e-d4845088269b",
                            name="山田太郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="c9b67094-cdab-494c-818e-d4845088269b",
                                name="山田太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        service_type_name="組織開発",
                        answer_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="山田太郎",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=None,
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=True,
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectSurveyModel(
                        id="22221449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="c9b67094-cdab-494c-818e-d4845088269b",
                            name="山田太郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="c9b67094-cdab-494c-818e-d4845088269b",
                                name="山田太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        service_type_name="組織開発",
                        answer_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="山田太郎",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=None,
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=True,
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        company="テスト株式会社",
                        is_input_man_hour=True,
                        project_ids={"efba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.SALES.key,
                        version=1,
                    ),
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                        customer_name="取引先株式会社",
                        job="部長",
                        organization_name="コンサルティング事業部",
                        project_ids={"886a3144-9650-4a34-8a23-3b02f3b9aeac"},
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.CUSTOMER.key,
                        version=1,
                    ),
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        company="テスト株式会社",
                        supporter_organization_id=[
                            "556a3144-9650-4a34-8a23-3b02f3b9aeac"
                        ],
                        is_input_man_hour=True,
                        project_ids={
                            "886a3144-9650-4a34-8a23-3b02f3b9aeac",
                            "efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        },
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.SUPPORTER.key,
                        version=1,
                    ),
                ],
            ),
            (
                "efba46d3-1d56-428b-ac6d-a51e263b54a8",
                1,
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
                    is_man_hour_input=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                {"message": "OK"},
                [
                    ProjectKarteModel(
                        karte_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        start_datetime="2022/01/20 10:00",
                        start_time=600,
                        end_time=780,
                        supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        last_update_datetime="2020-10-23T03:21:39.356000+0000",
                        customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        man_hour=0,
                        detail=None,
                        feedback=None,
                        homework=None,
                        documents=None,
                        deliverables=None,
                        memo=None,
                        task=None,
                        is_draft=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectKarteModel(
                        karte_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        start_datetime="2022/01/20 10:00",
                        start_time=600,
                        end_time=780,
                        supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        last_update_datetime="2020-10-23T03:21:39.356000+0000",
                        customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        man_hour=0,
                        detail=None,
                        feedback=None,
                        homework=None,
                        documents=None,
                        deliverables=None,
                        memo=None,
                        task=None,
                        is_draft=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="c9b67094-cdab-494c-818e-d4845088269b",
                            name="山田太郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="c9b67094-cdab-494c-818e-d4845088269b",
                                name="山田太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        service_type_name="組織開発",
                        answer_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="山田太郎",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=None,
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=True,
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectSurveyModel(
                        id="22221449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="c9b67094-cdab-494c-818e-d4845088269b",
                            name="山田太郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="c9b67094-cdab-494c-818e-d4845088269b",
                                name="山田太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        service_type_name="組織開発",
                        answer_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="山田太郎",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=None,
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=True,
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        company="テスト株式会社",
                        is_input_man_hour=True,
                        project_ids={"efba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.SALES.key,
                        version=1,
                    ),
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                        customer_name="取引先株式会社",
                        job="部長",
                        organization_name="コンサルティング事業部",
                        project_ids={"886a3144-9650-4a34-8a23-3b02f3b9aeac"},
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.CUSTOMER.key,
                        version=1,
                    ),
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        company="テスト株式会社",
                        supporter_organization_id=[
                            "556a3144-9650-4a34-8a23-3b02f3b9aeac"
                        ],
                        is_input_man_hour=True,
                        project_ids={
                            "886a3144-9650-4a34-8a23-3b02f3b9aeac",
                            "efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        },
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.SUPPORTER.key,
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        project_id,
        version,
        project_model,
        expected,
        project_karte_model_list,
        project_survey_model_list,
        user_model_list,
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mocker.patch.object(ProjectModel, "delete")
        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = project_karte_model_list
        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_project_survey.return_value = project_survey_model_list
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = user_model_list

        mocker.patch.object(BatchWrite, "delete")
        mocker.patch.object(BatchWrite, "save")

        mock_npf_get_projects = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock_npf_get_projects.return_value = (
            200,
            json.load(open("mock/pf/get_projects.json", "r")),
        )
        mock_npf_delete_project = mocker.patch.object(
            PlatformApiOperator, "delete_project_by_pf_id"
        )
        mock_npf_delete_project.return_value = (200, {"message": "OK"})
        response = client.delete(
            f"/api/project/{project_id}/{version}",
            headers=REQUEST_HEADERS,
        )

        response = client.delete(
            f"/api/projects/{project_id}?version={version}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    def test_project_not_found(self, mocker, mock_auth_admin):
        """案件が存在しない時のテスト"""
        project_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )
        mock = mocker.patch.object(ProjectModel, "get")
        mock.side_effect = DoesNotExist()

        mocker.patch.object(ProjectModel, "delete")

        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = []
        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_project_survey.return_value = []
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = []

        mocker.patch.object(BatchWrite, "delete")
        mocker.patch.object(BatchWrite, "save")

        response = client.delete(
            f"/api/projects/{project_id}?version={version}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "admin_model_get",
        [
            # 営業担当者
            AdminModel(
                id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                data_type=DataType.ADMIN,
                name="田中太郎",
                email="user@example.com",
                roles={UserRoleType.SALES.key},
                company=None,
                job="部長",
                supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
                organization_name=None,
                cognito_id=None,
                last_login_at="2020-10-23T03:21:39.356000+0000",
                otp_secret=None,
                otp_verified_token="111111",
                otp_verified_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
            # 営業責任者
            AdminModel(
                id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                data_type=DataType.ADMIN,
                name="田中太郎",
                email="user@example.com",
                roles={UserRoleType.SALES_MGR.key},
                company=None,
                job="部長",
                supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
                organization_name=None,
                cognito_id=None,
                last_login_at="2020-10-23T03:21:39.356000+0000",
                otp_secret=None,
                otp_verified_token="111111",
                otp_verified_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
            # 支援者責任者
            AdminModel(
                id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                data_type=DataType.ADMIN,
                name="田中太郎",
                email="user@example.com",
                roles={UserRoleType.SUPPORTER_MGR.key},
                company=None,
                job="部長",
                supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
                organization_name=None,
                cognito_id=None,
                last_login_at="2020-10-23T03:21:39.356000+0000",
                otp_secret=None,
                otp_verified_token="111111",
                otp_verified_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
        ],
    )
    def test_auth_error(
        self,
        mocker,
        mock_auth_admin,
        admin_model_get,
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model_get

        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = admin_model_get
        project_id = "efba46d3-1d56-428b-ac6d-a51e263b54a8"
        version = 1

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

        mocker.patch.object(ProjectModel, "delete")

        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = []
        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_project_survey.return_value = []
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = []

        mocker.patch.object(BatchWrite, "delete")
        mocker.patch.object(BatchWrite, "save")

        response = client.delete(
            f"/api/projects/{project_id}?version={version}", headers=REQUEST_HEADERS
        )
        assert response.status_code == 403

    @pytest.mark.parametrize(
        "admin_model_get",
        [
            # システム管理者
            AdminModel(
                id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                data_type=DataType.ADMIN,
                name="田中太郎",
                email="user@example.com",
                roles={UserRoleType.SYSTEM_ADMIN.key},
                company=None,
                job="部長",
                supporter_organization_id=None,
                organization_name=None,
                cognito_id=None,
                last_login_at="2020-10-23T03:21:39.356000+0000",
                otp_secret=None,
                otp_verified_token="111111",
                otp_verified_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
            # アンケート事務局
            AdminModel(
                id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                data_type=DataType.ADMIN,
                name="田中太郎",
                email="user@example.com",
                roles={UserRoleType.SURVEY_OPS.key},
                company=None,
                job="部長",
                supporter_organization_id=None,
                organization_name=None,
                cognito_id=None,
                last_login_at="2020-10-23T03:21:39.356000+0000",
                otp_secret=None,
                otp_verified_token="111111",
                otp_verified_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
            # 稼働率調査事務局
            AdminModel(
                id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                data_type=DataType.ADMIN,
                name="田中太郎",
                email="user@example.com",
                roles={UserRoleType.MAN_HOUR_OPS.key},
                company=None,
                job="部長",
                supporter_organization_id=None,
                organization_name=None,
                cognito_id=None,
                last_login_at="2020-10-23T03:21:39.356000+0000",
                otp_secret=None,
                otp_verified_token="111111",
                otp_verified_at=None,
                disabled=False,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            ),
        ],
    )
    def test_auth_ok(
        self,
        mocker,
        mock_auth_admin,
        admin_model_get,
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        # mock_auth_adminのユーザ部分を上書き
        mock = mocker.patch.object(AdminModel, "get_admin_by_cognito_id")
        mock.return_value = admin_model_get

        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = admin_model_get
        project_id = "efba46d3-1d56-428b-ac6d-a51e263b54a8"
        version = 1

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

        mocker.patch.object(ProjectModel, "delete")

        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = []
        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_project_survey.return_value = []
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = []

        mocker.patch.object(BatchWrite, "delete")
        mocker.patch.object(BatchWrite, "save")

        mock_npf_get_projects = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock_npf_get_projects.return_value = (
            200,
            json.load(open("mock/pf/get_projects.json", "r")),
        )
        mock_npf_delete_project = mocker.patch.object(
            PlatformApiOperator, "delete_project_by_pf_id"
        )
        mock_npf_delete_project.return_value = (200, {"message": "OK"})

        response = client.delete(
            f"/api/project/{project_id}/{version}",
            headers=REQUEST_HEADERS,
        )

        response = client.delete(
            f"/api/projects/{project_id}?version={version}", headers=REQUEST_HEADERS
        )
        assert response.status_code == 200

    def test_project_version_conflict(self, mocker, mock_auth_admin):
        project_id = "efba46d3-1d56-428b-ac6d-a51e263b54a8"
        version = 1

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )
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
            version=2,
        )

        mocker.patch.object(ProjectModel, "delete")

        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = []
        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_project_survey.return_value = []
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = []

        mocker.patch.object(BatchWrite, "delete")
        mocker.patch.object(BatchWrite, "save")

        response = client.delete(
            f"/api/projects/{project_id}?version={version}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"

    @pytest.mark.parametrize(
        "project_id, version, project_model, project_karte_model_list, project_survey_model_list, user_model_list",
        [
            (
                "efba46d3-1d56-428b-ac6d-a51e263b54a8",
                1,
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
                ),
                [
                    ProjectKarteModel(
                        karte_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        start_datetime="2022/01/20 10:00",
                        start_time=600,
                        end_time=780,
                        supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        last_update_datetime="2020-10-23T03:21:39.356000+0000",
                        customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        man_hour=0,
                        detail=None,
                        feedback=None,
                        homework=None,
                        documents=None,
                        deliverables=None,
                        memo=None,
                        task=None,
                        is_draft=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectKarteModel(
                        karte_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        start_datetime="2022/01/20 10:00",
                        start_time=600,
                        end_time=780,
                        supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        last_update_datetime="2020-10-23T03:21:39.356000+0000",
                        customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        man_hour=0,
                        detail=None,
                        feedback=None,
                        homework=None,
                        documents=None,
                        deliverables=None,
                        memo=None,
                        task=None,
                        is_draft=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="c9b67094-cdab-494c-818e-d4845088269b",
                            name="山田太郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="c9b67094-cdab-494c-818e-d4845088269b",
                                name="山田太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        service_type_name="組織開発",
                        answer_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="山田太郎",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=None,
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=True,
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectSurveyModel(
                        id="22221449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="c9b67094-cdab-494c-818e-d4845088269b",
                            name="山田太郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="c9b67094-cdab-494c-818e-d4845088269b",
                                name="山田太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        service_type_name="組織開発",
                        answer_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="山田太郎",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=None,
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:00",
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=True,
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        company="テスト株式会社",
                        is_input_man_hour=True,
                        project_ids={"efba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.SALES.key,
                        version=1,
                    ),
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                        customer_name="取引先株式会社",
                        job="部長",
                        organization_name="コンサルティング事業部",
                        project_ids={"886a3144-9650-4a34-8a23-3b02f3b9aeac"},
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.CUSTOMER.key,
                        version=1,
                    ),
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        company="テスト株式会社",
                        supporter_organization_id=[
                            "556a3144-9650-4a34-8a23-3b02f3b9aeac"
                        ],
                        is_input_man_hour=True,
                        project_ids={
                            "886a3144-9650-4a34-8a23-3b02f3b9aeac",
                            "efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        },
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.SUPPORTER.key,
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_error_400_surveys_has_already_been_sent(
        self,
        mocker,
        mock_auth_admin,
        project_id,
        version,
        project_model,
        project_karte_model_list,
        project_survey_model_list,
        user_model_list,
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mocker.patch.object(ProjectModel, "delete")
        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = project_karte_model_list
        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_project_survey.return_value = project_survey_model_list
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = user_model_list

        mocker.patch.object(BatchWrite, "delete")
        mocker.patch.object(BatchWrite, "save")

        response = client.delete(
            f"/api/projects/{project_id}?version={version}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 400
        assert (
            actual["detail"]
            == "The project cannot be deleted because surveys has already been sent."
        )

    @pytest.mark.parametrize(
        "project_id, version, project_model, project_karte_model_list, project_survey_model_list, user_model_list",
        [
            (
                "efba46d3-1d56-428b-ac6d-a51e263b54a8",
                1,
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
                    is_man_hour_input=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                [
                    ProjectKarteModel(
                        karte_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        start_datetime="2022/01/20 10:00",
                        start_time=600,
                        end_time=780,
                        supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        last_update_datetime="2020-10-23T03:21:39.356000+0000",
                        customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        man_hour=0,
                        detail=None,
                        feedback=None,
                        homework=None,
                        documents=None,
                        deliverables=None,
                        memo=None,
                        task=None,
                        is_draft=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectKarteModel(
                        karte_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        start_datetime="2022/01/20 10:00",
                        start_time=600,
                        end_time=780,
                        supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        last_update_datetime="2020-10-23T03:21:39.356000+0000",
                        customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                        man_hour=0,
                        detail=None,
                        feedback=None,
                        homework=None,
                        documents=None,
                        deliverables=None,
                        memo=None,
                        task=None,
                        is_draft=False,
                        create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="c9b67094-cdab-494c-818e-d4845088269b",
                            name="山田太郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="c9b67094-cdab-494c-818e-d4845088269b",
                                name="山田太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        service_type_name="組織開発",
                        answer_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="山田太郎",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=None,
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=True,
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectSurveyModel(
                        id="22221449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="c9b67094-cdab-494c-818e-d4845088269b",
                            name="山田太郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="c9b67094-cdab-494c-818e-d4845088269b",
                                name="山田太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                        service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        service_type_name="組織開発",
                        answer_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                        answer_user_name="山田太郎",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        customer_name="あああ研究所",
                        company="ソニーグループ株式会社",
                        answers=None,
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=True,
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        company="テスト株式会社",
                        is_input_man_hour=True,
                        project_ids={"efba46d3-1d56-428b-ac6d-a51e263b54a8"},
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.SALES.key,
                        version=1,
                    ),
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                        customer_name="取引先株式会社",
                        job="部長",
                        organization_name="コンサルティング事業部",
                        project_ids={"886a3144-9650-4a34-8a23-3b02f3b9aeac"},
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.CUSTOMER.key,
                        version=1,
                    ),
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        company="テスト株式会社",
                        supporter_organization_id=[
                            "556a3144-9650-4a34-8a23-3b02f3b9aeac"
                        ],
                        is_input_man_hour=True,
                        project_ids={
                            "886a3144-9650-4a34-8a23-3b02f3b9aeac",
                            "efba46d3-1d56-428b-ac6d-a51e263b54a8",
                        },
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        agreed=True,
                        role=UserRoleType.SUPPORTER.key,
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_error_400_manhours_have_already_been_entered(
        self,
        mocker,
        mock_auth_admin,
        project_id,
        version,
        project_model,
        project_karte_model_list,
        project_survey_model_list,
        user_model_list,
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock_user_admin_get = mocker.patch.object(AdminModel, "get")
        mock_user_admin_get.return_value = AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at="2020-10-23T03:21:39.356000+0000",
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model
        mocker.patch.object(ProjectModel, "delete")
        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = project_karte_model_list
        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_project_survey.return_value = project_survey_model_list
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = user_model_list

        mocker.patch.object(BatchWrite, "delete")
        mocker.patch.object(BatchWrite, "save")

        response = client.delete(
            f"/api/projects/{project_id}?version={version}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 400
        assert (
            actual["detail"]
            == "The project cannot be deleted because man-hours have already been entered."
        )
