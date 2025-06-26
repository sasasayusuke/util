import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
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
from app.resources.const import DataType, SurveyType, UserRoleType

client = TestClient(app)


class TestGetSurveyById:
    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
            # 案件アンケートのsupportersが設定されていない場合
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
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
                    supporter_users=None,
                    sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    service_type_name="組織開発",
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
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
                            "id": "8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                            "name": "テスト太郎（サービス責任者）",
                            "organizationId": None,
                            "organizationName": None,
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_ok(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """正常"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
                        )
                    ],
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime=None,
                    plan_survey_response_datetime="2022/01/31 17:00",
                    actual_survey_response_datetime="2022/01/30 10:00",
                    is_finished=False,
                    is_disclosure=True,
                    is_not_summary=False,
                    is_solver_project=False,
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
                ),
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_validate_error_non_survey_request(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """正常"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 400
        assert (
            actual["detail"]
            == "Cannot get a survey for which the response request has not been sent."
        )

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
                    project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="IST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        name="山田 太郎",
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
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
                        "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        "name": "山田 太郎",
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_auth_ok_supporter(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 支援者: 担当案件かつ支援者への開示OK"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
                    project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="IST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        name="山田 太郎",
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
                        )
                    ],
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime="2022/01/15 01:10",
                    plan_survey_response_datetime="2022/01/31 17:00",
                    actual_survey_response_datetime="2022/01/30 10:00",
                    is_finished=False,
                    is_disclosure=False,
                    is_not_summary=False,
                    is_solver_project=False,
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
                ),
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
                        "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        "name": "山田 太郎",
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": False,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_auth_error_supporter_403(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 支援者: 担当案件かつ支援者への開示OK以外"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.PP,
                    project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="IST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        name="山田 太郎",
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
                    answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    answer_user_name="田中 次郎",
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    customer_name="あああ研究所",
                    company="ソニーグループ株式会社",
                    answers=[
                        AnswersAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            answer="問題なし",
                            point=3,
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
                {
                    "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                    "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "surveyRevision": 4,
                    "surveyType": "pp",
                    "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    "projectName": "サンプルプロジェクト",
                    "customerSuccess": "DXの実現",
                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                    "supporterOrganizationName": "IST",
                    "supportDateFrom": "2021/06/01",
                    "supportDateTo": "2022/12/31",
                    "mainSupporterUser": {
                        "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        "name": "山田 太郎",
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
                    "answerUserId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "answerUserName": "田中 次郎",
                    "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                    "customerName": "あああ研究所",
                    "company": "ソニーグループ株式会社",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_auth_ok_supporter_pp(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 支援者: 制限なし（PPアンケート）"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
                    project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                    supporter_organization_name="IST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        name="山田 太郎",
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
                {
                    "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                    "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "surveyRevision": 4,
                    "surveyType": "quick",
                    "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    "projectName": "サンプルプロジェクト",
                    "customerSuccess": "DXの実現",
                    "supporterOrganizationId": "556a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "supporterOrganizationName": "IST",
                    "supportDateFrom": "2021/06/01",
                    "supportDateTo": "2022/12/31",
                    "mainSupporterUser": {
                        "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        "name": "山田 太郎",
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_auth_ok_supporter_mgr(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 支援者責任者: 非所属課かつ非公開案件を除いた全案件"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
                    project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="IST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        name="山田 太郎",
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
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
                        "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        "name": "山田 太郎",
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ),
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_auth_error_supporter_mgr_403(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 支援者責任者: 非所属課かつ非公開案件"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.PP,
                    project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="IST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        name="山田 太郎",
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
                    answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    answer_user_name="田中 次郎",
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    customer_name="あああ研究所",
                    company="ソニーグループ株式会社",
                    answers=[
                        AnswersAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            answer="問題なし",
                            point=3,
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
                {
                    "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                    "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "surveyRevision": 4,
                    "surveyType": "pp",
                    "projectId": "111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    "projectName": "サンプルプロジェクト",
                    "customerSuccess": "DXの実現",
                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                    "supporterOrganizationName": "IST",
                    "supportDateFrom": "2021/06/01",
                    "supportDateTo": "2022/12/31",
                    "mainSupporterUser": {
                        "id": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        "name": "山田 太郎",
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
                    "answerUserId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "answerUserName": "田中 次郎",
                    "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                    "customerName": "あああ研究所",
                    "company": "ソニーグループ株式会社",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ),
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_auth_ok_supporter_mgr_pp(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 支援者責任者: 制限なし（PPアンケート）"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
                    project_id="886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                    sales_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    service_type_name="組織開発",
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
                        )
                    ],
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime="2022/01/15 01:10",
                    plan_survey_response_datetime="2022/01/31 17:00",
                    actual_survey_response_datetime="2022/01/30 10:00",
                    is_finished=True,
                    is_disclosure=True,
                    is_not_summary=False,
                    is_solver_project=False,
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
                ),
                {
                    "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                    "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "surveyRevision": 4,
                    "surveyType": "quick",
                    "projectId": "886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                    "salesUserId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "salesUserName": "田中 太郎",
                    "serviceTypeId": "7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    "serviceTypeName": "組織開発",
                    "answerUserId": "7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    "answerUserName": "田中 次郎",
                    "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                    "customerName": "あああ研究所",
                    "company": "ソニーグループ株式会社",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": True,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_sales_user")
    def test_auth_ok_sales(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 営業担当者: 非担当・非公開案件を除いた全案件"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
                    project_id="886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
                        )
                    ],
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime="2022/01/15 01:10",
                    plan_survey_response_datetime="2022/01/31 17:00",
                    actual_survey_response_datetime="2022/01/30 10:00",
                    is_finished=True,
                    is_disclosure=True,
                    is_not_summary=False,
                    is_solver_project=False,
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
                ),
                {
                    "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                    "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "surveyRevision": 4,
                    "surveyType": "quick",
                    "projectId": "886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": True,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ),
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_sales_user")
    def test_auth_error_sales_403(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 営業担当者: 非担当・非公開案件"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.PP,
                    project_id="886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
                    answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    answer_user_name="田中 次郎",
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    customer_name="あああ研究所",
                    company="ソニーグループ株式会社",
                    answers=[
                        AnswersAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            answer="問題なし",
                            point=3,
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
                        )
                    ],
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime="2022/01/15 01:10",
                    plan_survey_response_datetime="2022/01/31 17:00",
                    actual_survey_response_datetime="2022/01/30 10:00",
                    is_finished=True,
                    is_disclosure=True,
                    is_not_summary=False,
                    is_solver_project=False,
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
                ),
                {
                    "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                    "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "surveyRevision": 4,
                    "surveyType": "pp",
                    "projectId": "886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                    "answerUserId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "answerUserName": "田中 次郎",
                    "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                    "customerName": "あああ研究所",
                    "company": "ソニーグループ株式会社",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": True,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ),
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_sales_user")
    def test_auth_ok_sales_pp(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 営業担当者: 制限なし（PPアンケート）"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.QUICK,
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_sales_mgr_user")
    def test_auth_ok_sales_mgr(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 営業責任者: 制限なし"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type=SurveyType.PP,
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
                    answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    answer_user_name="田中 次郎",
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    customer_name="あああ研究所",
                    company="ソニーグループ株式会社",
                    answers=[
                        AnswersAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            answer="問題なし",
                            point=3,
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
                {
                    "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                    "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "surveyRevision": 4,
                    "surveyType": "pp",
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
                    "answerUserId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "answerUserName": "田中 次郎",
                    "customerId": "d6121808-341d-4883-8e2c-69462acf6ccb",
                    "customerName": "あああ研究所",
                    "company": "ソニーグループ株式会社",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_sales_mgr_user")
    def test_auth_ok_sales_mgr_pp(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 営業責任者: 制限なし（PPアンケート）"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
                    answer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    answer_user_name="山田太郎",
                    customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    customer_name="取引先株式会社",
                    company="ソニーグループ株式会社",
                    answers=[
                        AnswersAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            answer="問題なし",
                            point=3,
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
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
                    "answerUserId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "answerUserName": "山田太郎",
                    "customerId": "106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "customerName": "取引先株式会社",
                    "company": "ソニーグループ株式会社",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    main_customer_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_customer_user")
    def test_auth_ok_customer(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 顧客: 自身の案件"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_customer_user")
    def test_auth_error_customer_403(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """権限確認: 顧客: 自身の案件以外"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        assert response.status_code == 403

    @pytest.mark.usefixtures("auth_user")
    def test_survey_not_found(self, mocker):
        """案件アンケート情報が存在しない時のテスト"""

        mock = mocker.patch.object(ProjectSurveyModel, "get")
        mock.side_effect = DoesNotExist()

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.usefixtures("auth_user")
    def test_project_not_found(self, mocker):
        """案件が存在しない時のテスト"""
        project_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock = mocker.patch.object(ProjectModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.get(f"/api/projects/{project_id}")

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "survey_model, expected, user_model, project_model",
        [
            (
                # 所属していない案件の未回答アンケート
                ProjectSurveyModel(
                    id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type="quick",
                    project_id="99911449-5d63-42d4-ae1b-f0faf65a7076",
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
                    service_manager_id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    service_manager_name="テスト太郎（サービス責任者）",
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
                            choice_ids=set(
                                [
                                    "93bcdbae-4a53-4faa-b32e-85c086d09152",
                                ]
                            ),
                            summary_type="normal",
                            other_input="任意入力",
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
                    is_solver_project=False,
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
                ),
                {
                    "id": "11111449-5d63-42d4-ae1b-f0faf65a7076",
                    "surveyMasterId": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "surveyRevision": 4,
                    "surveyType": "quick",
                    "projectId": "886a3144-9650-4a34-8a23-3b02f3b9aeac",
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
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": [
                                "93bcdbae-4a53-4faa-b32e-85c086d09152",
                            ],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "summaryMonth": "2022/01",
                    "isNotSummary": False,
                    "isSolverProject": False,
                    "planSurveyRequestDatetime": "2022/01/15 01:00",
                    "actualSurveyRequestDatetime": "2022/01/15 01:10",
                    "planSurveyResponseDatetime": "2022/01/31 17:00",
                    "actualSurveyResponseDatetime": "2022/01/30 10:00",
                    "isFinished": False,
                    "isDisclosure": True,
                    "createId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "createAt": "2022-05-23T16:34:21.523+09:00",
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-05-23T16:34:21.523+09:00",
                    "version": 1,
                },
                UserModel(
                    id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                    data_type=DataType.USER,
                    name="田中 太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
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
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
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
                    customer_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                    ),
                    main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_sales_user")
    def test_validate_error_sales_get_unassigned_and_unanswered_survey(
        self,
        mocker,
        survey_model,
        expected,
        user_model,
        project_model,
    ):
        """営業担当者は所属していない案件の未回答アンケートの取得不可（PPアンケート除く）"""
        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}")

        actual = response.json()
        assert response.status_code == 403
        assert (
            actual["detail"]
            == "Sales users cannot get unanswered surveys for unassigned projects."
        )
