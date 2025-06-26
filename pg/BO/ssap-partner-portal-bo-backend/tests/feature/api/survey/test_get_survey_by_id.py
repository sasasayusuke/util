import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.admin import AdminModel
from app.models.project import ProjectModel
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


class TestGetSurveyById:
    @pytest.mark.parametrize(
        "survey_model, project_model, admin_model, user_model, expected",
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
                    is_solver_project=True,
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
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    name="サンプルプロジェクト",
                    continued=True,
                    main_sales_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    is_secret=True,
                    is_solver_project=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles=[UserRoleType.SALES.key],
                    otp_verified_token="111111",
                ),
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
                    "isSolverProject": True,
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
            ),
            # 案件アンケートのsupportersが設定されていない場合
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
                    is_solver_project=True,
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
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    name="サンプルプロジェクト",
                    continued=True,
                    main_sales_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    is_secret=True,
                    is_solver_project=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles=[UserRoleType.SALES.key],
                    otp_verified_token="111111",
                ),
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
                    "isSolverProject": True,
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
            ),
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        survey_model,
        project_model,
        admin_model,
        user_model,
        expected,
    ):
        """正常"""
        mock_auth_admin([UserRoleType.SALES.key])
        mock_admin = mocker.patch.object(AdminModel, "get")
        mock_admin.return_value = admin_model

        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, project_model, admin_model, user_model, expected",
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
                    actual_survey_request_datetime=None,
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
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    name="サンプルプロジェクト",
                    continued=True,
                    main_sales_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles=[UserRoleType.SALES.key],
                    otp_verified_token="111111",
                ),
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
            ),
        ],
    )
    def test_validate_error_non_survey_request(
        self,
        mocker,
        mock_auth_admin,
        survey_model,
        project_model,
        admin_model,
        user_model,
        expected,
    ):
        """正常"""
        mock_auth_admin([UserRoleType.SALES.key])

        mock_admin = mocker.patch.object(AdminModel, "get")
        mock_admin.return_value = admin_model

        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 400
        assert (
            actual["detail"]
            == "Cannot get a survey for which the response request has not been sent."
        )

    @pytest.mark.parametrize(
        "survey_model, project_model, admin_model, user_model",
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
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime="2022/01/15 01:00",
                    plan_survey_response_datetime="2022/01/31 17:00",
                    # actual_survey_response_datetime="2022/01/30 10:00",
                    is_finished=False,
                    is_disclosure=True,
                    is_not_summary=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                    name="サンプルプロジェクト",
                    continued=True,
                    main_sales_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles=[UserRoleType.SALES.key],
                    otp_verified_token="111111",
                ),
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
            ),
        ],
    )
    def test_validate_error_unanswwerd_survey(
        self,
        mocker,
        mock_auth_admin,
        survey_model,
        project_model,
        admin_model,
        user_model,
    ):
        """未回答のアンケートを取得しようとした時のテスト"""
        mock_auth_admin([UserRoleType.SALES.key])

        mock_admin = mocker.patch.object(AdminModel, "get")
        mock_admin.return_value = admin_model

        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = user_model

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 400
        assert actual["detail"] == "Unanswered surveys cannot be retrieved."

    def test_survey_not_found(self, mocker, mock_auth_admin):
        """案件アンケート情報が存在しない時のテスト"""
        mock_auth_admin([UserRoleType.SALES_MGR.key])

        mock = mocker.patch.object(ProjectSurveyModel, "get")
        mock.side_effect = DoesNotExist()

        survey_id = "11111449-5d63-42d4-ae1b-f0faf65a7076"
        response = client.get(f"/api/surveys/{survey_id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
