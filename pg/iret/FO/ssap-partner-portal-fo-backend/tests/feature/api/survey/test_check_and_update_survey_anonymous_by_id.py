import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.admin import AdminModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_survey import (
    AnswersAttribute,
    EvaluationSupporterAttribute,
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.models.survey_master import (
    ChoicesSubAttribute,
    GroupSubAttribute,
    QuestionFlowAttribute,
    QuestionsAttribute,
    SurveyMasterModel,
)
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from app.schemas.survey import PostCheckSurveyByIdPasswordResponse
from app.service.notification_service import NotificationService
from app.service.project_service import ProjectService
from app.service.survey_service import SurveyService
from app.utils.aws.sqs import SqsHelper

client = TestClient(app)


class TestCheckAndUpdateSurveyById:
    @pytest.mark.parametrize(
        "body, survey_model, expected, admin_models, user_supporter_mgr_models, user_models, survey_master_model, project_model",
        [
            (
                {
                    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                    "password": "]UHY4^ITo,RU",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "isFinished": True,
                    "isDisclosure": True,
                },
                ProjectSurveyModel(
                    id="3361530c-1f7a-43f7-b306-6e88f88f92c3",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type="quick",
                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="AST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        name="田中 三郎",
                        organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        organization_name="AST",
                    ),
                    supporter_users=[
                        SupporterUserAttribute(
                            id="b9b67094-cdab-494c-818e-d4845088269b",
                            name="田中 次郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        )
                    ],
                    sales_user_id="7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                    service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    service_type_name="組織開発",
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
                {"message": "OK"},
                [
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type=DataType.ADMIN,
                        name="田中太郎",
                        email="user@example.com",
                        roles={UserRoleType.SYSTEM_ADMIN.key},
                        company=None,
                        job="部長",
                        supporter_organization_id={
                            "180a3597-b7e7-42c8-902c-a29016afa662"
                        },
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
                    ),
                ],
                [
                    UserModel(
                        id="d9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER_MGR,
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
                ],
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                    UserModel(
                        id="b9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中次郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                ],
                SurveyMasterModel(
                    id="e1823e1c-f9b3-4982-9e34-e34f5e5e2a37",
                    revision=0,
                    name="修了アンケート",
                    type="completion",
                    timing="monthly",
                    init_send_day_setting=0,
                    init_answer_limit_day_setting=0,
                    is_disclosure=True,
                    questions=[
                        QuestionsAttribute(
                            other_description="その他記載欄（任意）",
                            format="checkbox",
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            disabled=False,
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            choices=[
                                ChoicesSubAttribute(
                                    description="支援者の対応姿勢",
                                    group=[
                                        GroupSubAttribute(
                                            title="string",
                                            disabled=False,
                                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            is_new=True,
                                        )
                                    ],
                                    is_new=True,
                                )
                            ],
                            summary_type="point",
                            required=True,
                            is_new=True,
                        ),
                    ],
                    question_flow=[
                        QuestionFlowAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_choice_ids=set(
                                ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
                            ),
                        )
                    ],
                    is_latest=0,
                    memo="メモ",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
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
            # 案件アンケートのアンケート評価対象者（アクセラレータ）が更新されている場合
            # お知らせメール通知先のアクセラレータは、supporter_users_before_update を使用する
            (
                {
                    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                    "password": "]UHY4^ITo,RU",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "isFinished": True,
                    "isDisclosure": True,
                },
                ProjectSurveyModel(
                    id="3361530c-1f7a-43f7-b306-6e88f88f92c3",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type="quick",
                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="AST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        name="田中 三郎",
                        organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        organization_name="AST",
                    ),
                    supporter_users=[
                        SupporterUserAttribute(
                            id="b9b67094-cdab-494c-818e-d4845088269b",
                            name="田中 次郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        )
                    ],
                    evaluation_supporters=[
                        EvaluationSupporterAttribute(
                            supporter_id="b9b67094-cdab-494c-818e-d4845088269b",
                            karte_ids=["karte_A"],
                        )
                    ],
                    supporter_users_before_update=[
                        SupporterUserAttribute(
                            id="3bda6069-503a-4874-b27d-243197018a3c",
                            name="田中 テスト次郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        )
                    ],
                    is_updated_evaluation_supporters=True,
                    sales_user_id="7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                    service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    service_type_name="組織開発",
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
                {"message": "OK"},
                [
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type=DataType.ADMIN,
                        name="田中太郎",
                        email="user@example.com",
                        roles={UserRoleType.SYSTEM_ADMIN.key},
                        company=None,
                        job="部長",
                        supporter_organization_id={
                            "180a3597-b7e7-42c8-902c-a29016afa662"
                        },
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
                    ),
                ],
                [
                    UserModel(
                        id="d9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER_MGR,
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
                ],
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                    UserModel(
                        id="b9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中次郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                ],
                SurveyMasterModel(
                    id="e1823e1c-f9b3-4982-9e34-e34f5e5e2a37",
                    revision=0,
                    name="修了アンケート",
                    type="completion",
                    timing="monthly",
                    init_send_day_setting=0,
                    init_answer_limit_day_setting=0,
                    is_disclosure=True,
                    questions=[
                        QuestionsAttribute(
                            other_description="その他記載欄（任意）",
                            format="checkbox",
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            disabled=False,
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            choices=[
                                ChoicesSubAttribute(
                                    description="支援者の対応姿勢",
                                    group=[
                                        GroupSubAttribute(
                                            title="string",
                                            disabled=False,
                                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            is_new=True,
                                        )
                                    ],
                                    is_new=True,
                                )
                            ],
                            summary_type="point",
                            required=True,
                            is_new=True,
                        ),
                    ],
                    question_flow=[
                        QuestionFlowAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_choice_ids=set(
                                ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
                            ),
                        )
                    ],
                    is_latest=0,
                    memo="メモ",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
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
    def test_ok(
        self,
        mocker,
        body,
        survey_model,
        expected,
        admin_models,
        user_supporter_mgr_models,
        user_models,
        survey_master_model,
        project_model,
    ):
        survey_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = admin_models
        mock_user_supporter_mgr = mocker.patch.object(UserModel.role_index, "query")
        mock_user_supporter_mgr.return_value = user_supporter_mgr_models
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = user_models

        mock_survey_master = mocker.patch.object(SurveyMasterModel, "get")
        mock_survey_master.return_value = survey_master_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mocker.patch.object(UserModel, "get")

        mocker.patch.object(
            SurveyService,
            "is_visible_survey_by_supporter_mgr_for_survey_answer_provided",
        )
        mocker.patch.object(ProjectSurveyModel, "update")
        mocker.patch.object(NotificationService, "save_notification")
        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(ProjectService, "send_mail")

        mock_check_auth = mocker.patch.object(
            SurveyService, "post_check_survey_by_id_password"
        )
        mock_check_auth.return_value = PostCheckSurveyByIdPasswordResponse(
            id="12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        )

        response = client.put(
            f"/api/surveys/anonymous/{survey_id}?version={version}", json=body
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    def test_survey_not_found(self, mocker):
        """案件アンケートが存在しない時のテスト"""
        body = {
            "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
            "password": "]UHY4^ITo,RU",
            "answers": [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "answer": "問題なし",
                    "point": 3,
                    "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                    "summaryType": "normal",
                    "otherInput": "任意入力",
                }
            ],
            "isFinished": True,
            "isDisclosure": True,
        }

        survey_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.side_effect = DoesNotExist()
        mock_check_auth = mocker.patch.object(
            SurveyService, "post_check_survey_by_id_password"
        )
        mock_check_auth.return_value = PostCheckSurveyByIdPasswordResponse(
            id="12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        )

        response = client.put(
            f"/api/surveys/anonymous/{survey_id}?version={version}", json=body
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "body, survey_model, expected, admin_models, user_supporter_mgr_models, user_models, survey_master_model, project_model",
        [
            (
                {
                    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                    "password": "]UHY4^ITo,RU",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "isFinished": True,
                    "isDisclosure": True,
                },
                ProjectSurveyModel(
                    id="3361530c-1f7a-43f7-b306-6e88f88f92c3",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type="quick",
                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="AST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        name="田中 三郎",
                        organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        organization_name="AST",
                    ),
                    supporter_users=[
                        SupporterUserAttribute(
                            id="b9b67094-cdab-494c-818e-d4845088269b",
                            name="田中 次郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        )
                    ],
                    sales_user_id="7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                    service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    service_type_name="組織開発",
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
                        )
                    ],
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime=None,
                    plan_survey_response_datetime="2022/01/31 17:00",
                    actual_survey_response_datetime="2022/01/30 10:00",
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
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                {"message": "OK"},
                [
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type=DataType.ADMIN,
                        name="田中太郎",
                        email="user@example.com",
                        roles={UserRoleType.SYSTEM_ADMIN.key},
                        company=None,
                        job="部長",
                        supporter_organization_id={
                            "180a3597-b7e7-42c8-902c-a29016afa662"
                        },
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
                    ),
                ],
                [
                    UserModel(
                        id="d9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER_MGR,
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
                ],
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                    UserModel(
                        id="b9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中次郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                ],
                SurveyMasterModel(
                    id="e1823e1c-f9b3-4982-9e34-e34f5e5e2a37",
                    revision=0,
                    name="修了アンケート",
                    type="completion",
                    timing="monthly",
                    init_send_day_setting=0,
                    init_answer_limit_day_setting=0,
                    is_disclosure=True,
                    questions=[
                        QuestionsAttribute(
                            other_description="その他記載欄（任意）",
                            format="checkbox",
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            disabled=False,
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            choices=[
                                ChoicesSubAttribute(
                                    description="支援者の対応姿勢",
                                    group=[
                                        GroupSubAttribute(
                                            title="string",
                                            disabled=False,
                                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            is_new=True,
                                        )
                                    ],
                                    is_new=True,
                                )
                            ],
                            summary_type="point",
                            required=True,
                            is_new=True,
                        ),
                    ],
                    question_flow=[
                        QuestionFlowAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_choice_ids=set(
                                ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
                            ),
                        )
                    ],
                    is_latest=0,
                    memo="メモ",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
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
    def test_validate_error_non_survey_request(
        self,
        mocker,
        body,
        survey_model,
        expected,
        admin_models,
        user_supporter_mgr_models,
        user_models,
        survey_master_model,
        project_model,
    ):
        survey_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = admin_models
        mock_user_supporter_mgr = mocker.patch.object(UserModel.role_index, "query")
        mock_user_supporter_mgr.return_value = user_supporter_mgr_models
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = user_models

        mock_survey_master = mocker.patch.object(SurveyMasterModel, "get")
        mock_survey_master.return_value = survey_master_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mocker.patch.object(UserModel, "get")

        mocker.patch.object(ProjectSurveyModel, "update")
        mocker.patch.object(NotificationService, "save_notification")
        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(ProjectService, "send_mail")

        mock_check_auth = mocker.patch.object(
            SurveyService, "post_check_survey_by_id_password"
        )
        mock_check_auth.return_value = PostCheckSurveyByIdPasswordResponse(
            id="12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        )

        response = client.put(
            f"/api/surveys/anonymous/{survey_id}?version={version}", json=body
        )

        actual = response.json()
        assert response.status_code == 400
        assert (
            actual["detail"]
            == "Cannot get a survey for which the response request has not been sent."
        )

    @pytest.mark.parametrize(
        "body, survey_model, expected, admin_models, user_supporter_mgr_models, user_models, survey_master_model, project_model",
        [
            (
                {
                    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                    "password": "]UHY4^ITo,RU",
                    "answers": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "answer": "問題なし",
                            "point": 3,
                            "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                            "summaryType": "normal",
                            "otherInput": "任意入力",
                        }
                    ],
                    "isFinished": True,
                    "isDisclosure": True,
                },
                ProjectSurveyModel(
                    id="3361530c-1f7a-43f7-b306-6e88f88f92c3",
                    data_type=DataType.SURVEY,
                    survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    survey_master_revision=4,
                    survey_type="quick",
                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                    project_name="サンプルプロジェクト",
                    customer_success="DXの実現",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="AST",
                    support_date_from="2021/06/01",
                    support_date_to="2022/12/31",
                    main_supporter_user=SupporterUserAttribute(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        name="田中 三郎",
                        organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        organization_name="AST",
                    ),
                    supporter_users=[
                        SupporterUserAttribute(
                            id="b9b67094-cdab-494c-818e-d4845088269b",
                            name="田中 次郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="AST",
                        )
                    ],
                    sales_user_id="7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                    service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    service_type_name="組織開発",
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
                        )
                    ],
                    summary_month="2022/01",
                    plan_survey_request_datetime="2022/01/15 01:00",
                    actual_survey_request_datetime=None,
                    plan_survey_response_datetime="2022/01/31 17:00",
                    actual_survey_response_datetime="2022/01/30 10:00",
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
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                {"message": "OK"},
                [
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type=DataType.ADMIN,
                        name="田中太郎",
                        email="user@example.com",
                        roles={UserRoleType.SYSTEM_ADMIN.key},
                        company=None,
                        job="部長",
                        supporter_organization_id={
                            "180a3597-b7e7-42c8-902c-a29016afa662"
                        },
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
                    ),
                ],
                [
                    UserModel(
                        id="d9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER_MGR,
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
                ],
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                    UserModel(
                        id="b9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中次郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                    UserModel(
                        id="c9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中三郎",
                        email="user@example.com",
                        role=UserRoleType.SUPPORTER,
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
                ],
                SurveyMasterModel(
                    id="e1823e1c-f9b3-4982-9e34-e34f5e5e2a37",
                    revision=0,
                    name="修了アンケート",
                    type="completion",
                    timing="monthly",
                    init_send_day_setting=0,
                    init_answer_limit_day_setting=0,
                    is_disclosure=True,
                    questions=[
                        QuestionsAttribute(
                            other_description="その他記載欄（任意）",
                            format="checkbox",
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            disabled=False,
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            choices=[
                                ChoicesSubAttribute(
                                    description="支援者の対応姿勢",
                                    group=[
                                        GroupSubAttribute(
                                            title="string",
                                            disabled=False,
                                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            is_new=True,
                                        )
                                    ],
                                    is_new=True,
                                )
                            ],
                            summary_type="point",
                            required=True,
                            is_new=True,
                        ),
                    ],
                    question_flow=[
                        QuestionFlowAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_choice_ids=set(
                                ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]
                            ),
                        )
                    ],
                    is_latest=0,
                    memo="メモ",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2020-10-23T03:21:39.356000+0000",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2020-10-23T03:21:39.356000+0000",
                    version=1,
                ),
                ProjectModel(
                    id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
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
    def test_error_conflict(
        self,
        mocker,
        body,
        survey_model,
        expected,
        admin_models,
        user_supporter_mgr_models,
        user_models,
        survey_master_model,
        project_model,
    ):
        survey_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 3

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = survey_model

        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = admin_models
        mock_user_supporter_mgr = mocker.patch.object(UserModel.role_index, "query")
        mock_user_supporter_mgr.return_value = user_supporter_mgr_models
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = user_models

        mock_survey_master = mocker.patch.object(SurveyMasterModel, "get")
        mock_survey_master.return_value = survey_master_model

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mocker.patch.object(UserModel, "get")

        mocker.patch.object(ProjectSurveyModel, "update")
        mocker.patch.object(NotificationService, "save_notification")
        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(ProjectService, "send_mail")

        mock_check_auth = mocker.patch.object(
            SurveyService, "post_check_survey_by_id_password"
        )
        mock_check_auth.return_value = PostCheckSurveyByIdPasswordResponse(
            id="12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        )

        response = client.put(
            f"/api/surveys/anonymous/{survey_id}?version={version}", json=body
        )

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"

    @pytest.mark.parametrize(
        "body",
        [
            ###################
            # token : 必須
            ###################
            {
                # "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                "password": "]UHY4^ITo,RU",
                "answers": [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "answer": "問題なし",
                        "point": 3,
                        "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                        "summaryType": "normal",
                        "otherInput": "任意入力",
                    }
                ],
                "isFinished": True,
                "isDisclosure": True,
            },
            ###################
            # password : 必須
            ###################
            {
                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                # "password": "]UHY4^ITo,RU",
                "answers": [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "answer": "問題なし",
                        "point": 3,
                        "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                        "summaryType": "normal",
                        "otherInput": "任意入力",
                    }
                ],
                "isFinished": True,
                "isDisclosure": True,
            },
            ###################
            # answers : 必須
            ###################
            {
                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                "password": "]UHY4^ITo,RU",
                # "answers": [
                #     {
                #         "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                #         "answer": "問題なし",
                #         "point": 3,
                #         "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                #         "summaryType": "normal",
                #         "otherInput": "任意入力",
                #     }
                # ],
                "isFinished": True,
                "isDisclosure": True,
            },
            ###################
            # answers.id : 必須
            ###################
            {
                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                "password": "]UHY4^ITo,RU",
                "answers": [
                    {
                        # "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "answer": "問題なし",
                        "point": 3,
                        "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                        "summaryType": "normal",
                        "otherInput": "任意入力",
                    }
                ],
                "isFinished": True,
                "isDisclosure": True,
            },
            ##########################
            # answers.answer : 必須
            #########################
            {
                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                "password": "]UHY4^ITo,RU",
                "answers": [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        # "answer": "問題なし",
                        "point": 3,
                        "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                        "summaryType": "normal",
                        "otherInput": "任意入力",
                    }
                ],
                "isFinished": True,
                "isDisclosure": True,
            },
            #############################
            # answers.isFinished : 必須
            ############################
            {
                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                "password": "]UHY4^ITo,RU",
                "answers": [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "answer": "問題なし",
                        "point": 3,
                        "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                        "summaryType": "normal",
                        "otherInput": "任意入力",
                    }
                ],
                # "isFinished": True,
                "isDisclosure": True,
            },
            #############################
            # answers.isDisclosure : 必須
            ############################
            {
                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
                "password": "]UHY4^ITo,RU",
                "answers": [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "answer": "問題なし",
                        "point": 3,
                        "choiceIds": ["93bcdbae-4a53-4faa-b32e-85c086d09152"],
                        "summaryType": "normal",
                        "otherInput": "任意入力",
                    }
                ],
                "isFinished": True,
                # "isDisclosure": True,
            },
        ],
    )
    def test_validation(self, mocker, body):
        survey_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        mock_survey = mocker.patch.object(ProjectSurveyModel, "get")
        mock_survey.return_value = ProjectSurveyModel(
            id="3361530c-1f7a-43f7-b306-6e88f88f92c3",
            data_type=DataType.SURVEY,
            survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
            survey_master_revision=4,
            survey_type="quick",
            project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
            project_name="サンプルプロジェクト",
            customer_success="DXの実現",
            supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
            supporter_organization_name="AST",
            support_date_from="2021/06/01",
            support_date_to="2022/12/31",
            main_supporter_user=SupporterUserAttribute(
                id="c9b67094-cdab-494c-818e-d4845088269b",
                name="田中 三郎",
                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                organization_name="AST",
            ),
            supporter_users=[
                SupporterUserAttribute(
                    id="b9b67094-cdab-494c-818e-d4845088269b",
                    name="田中 次郎",
                    organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    organization_name="AST",
                )
            ],
            sales_user_id="7bf1b7e4-5625-4361-be0a-65f0e49829ea",
            service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            service_type_name="組織開発",
            answer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            answer_user_name="田中 次郎",
            customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
            customer_name="取引先株式会社",
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
            create_at="2022-05-23T16:34:21.523000+0000",
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at="2022-05-23T16:34:21.523000+0000",
            version=1,
        )

        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = [
            AdminModel(
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
        ]
        mock_user_supporter_mgr = mocker.patch.object(UserModel.role_index, "query")
        mock_user_supporter_mgr.return_value = [
            UserModel(
                id="d9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="田中三郎",
                email="user@example.com",
                role=UserRoleType.SUPPORTER_MGR,
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
        ]
        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = [
            UserModel(
                id="a9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="田中太郎",
                email="user@example.com",
                role=UserRoleType.SUPPORTER,
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
            UserModel(
                id="b9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="田中次郎",
                email="user@example.com",
                role=UserRoleType.SUPPORTER,
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
            UserModel(
                id="c9b67094-cdab-494c-818e-d4845088269b",
                data_type=DataType.USER,
                name="田中三郎",
                email="user@example.com",
                role=UserRoleType.SUPPORTER,
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
        ]

        mock_survey_master = mocker.patch.object(SurveyMasterModel, "get")
        mock_survey_master.return_value = SurveyMasterModel(
            id="e1823e1c-f9b3-4982-9e34-e34f5e5e2a37",
            revision=0,
            name="修了アンケート",
            type="completion",
            timing="monthly",
            init_send_day_setting=0,
            init_answer_limit_day_setting=0,
            is_disclosure=True,
            questions=[
                QuestionsAttribute(
                    other_description="その他記載欄（任意）",
                    format="checkbox",
                    description="当月のSSAP支援にはご満足いただけましたか？",
                    disabled=False,
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    choices=[
                        ChoicesSubAttribute(
                            description="支援者の対応姿勢",
                            group=[
                                GroupSubAttribute(
                                    title="string",
                                    disabled=False,
                                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    is_new=True,
                                )
                            ],
                            is_new=True,
                        )
                    ],
                    summary_type="point",
                    required=True,
                    is_new=True,
                ),
            ],
            question_flow=[
                QuestionFlowAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    condition_choice_ids=set(["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"]),
                )
            ],
            is_latest=0,
            memo="メモ",
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at="2020-10-23T03:21:39.356000+0000",
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at="2020-10-23T03:21:39.356000+0000",
            version=1,
        )

        mocker.patch.object(ProjectSurveyModel, "update")
        mocker.patch.object(NotificationService, "save_notification")
        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(ProjectService, "send_mail")

        mock_check_auth = mocker.patch.object(
            SurveyService, "post_check_survey_by_id_password"
        )
        mock_check_auth.return_value = PostCheckSurveyByIdPasswordResponse(
            id="12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        )

        response = client.put(
            f"/api/surveys/anonymous/{survey_id}?version={version}", json=body
        )

        assert response.status_code == 422
