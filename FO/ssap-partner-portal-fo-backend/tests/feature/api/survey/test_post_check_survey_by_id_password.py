import freezegun
import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.project_survey import (
    AnswersAttribute,
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from app.utils.cipher_aes import AesCipherUtils

client = TestClient(app)


@freezegun.freeze_time("2023/03/11")
class TestPostCheckSurveyByIdPassword:
    @pytest.mark.parametrize(
        "survey_model, project_model, expected, user_model, request_body, method_return",
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
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/09/30",
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
                    survey_password="qECFxZ9YvwFCpleSBDW9IbVF41AJN6WQFBFOeg==",
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
                {"id": "11111449-5d63-42d4-ae1b-f0faf65a7076"},
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
                    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                    "password": "]UHY4^ITo,RU",
                },
                {
                    "survey_id": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                    "iat": "1678422090.51732",
                    "exp": "1683644399.51732",
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_ok(
        self,
        mocker,
        survey_model,
        project_model,
        expected,
        user_model,
        request_body,
        method_return,
    ):
        """正常"""
        mocker_method = mocker.patch("app.utils.encryption.decode_jwt")
        mocker_method.return_value = method_return

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mocker_method_decode = mocker.patch.object(AesCipherUtils, "decrypt")
        mocker_method_decode.return_value = "]UHY4^ITo,RU"

        response = client.post("/api/surveys/auth", json=request_body)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_model, project_model, expected, user_model, request_body, method_return",
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
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/09/30",
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
                    survey_password="qECFxZ9YvwFCpleSBDW9IbVF41AJN6WQFBFOeg==",
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
                {"id": "11111449-5d63-42d4-ae1b-f0faf65a7076"},
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
                    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                    "password": "]UHY4^ITo,",
                },
                {
                    "survey_id": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                    "iat": "1678422090.51732",
                    "exp": "1683644399.51732",
                },
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_user")
    def test_error_password(
        self,
        mocker,
        survey_model,
        project_model,
        expected,
        user_model,
        request_body,
        method_return,
    ):
        """パスワードエラー"""
        mocker_method = mocker.patch("app.utils.encryption.decode_jwt")
        mocker_method.return_value = method_return

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mocker_method_decode = mocker.patch.object(AesCipherUtils, "decrypt")
        mocker_method_decode.return_value = "]UHY4^ITo,RU"

        response = client.post("/api/surveys/auth", json=request_body)

        actual = response.json()
        assert response.status_code == 403
        assert actual["detail"] == "Forbidden"
