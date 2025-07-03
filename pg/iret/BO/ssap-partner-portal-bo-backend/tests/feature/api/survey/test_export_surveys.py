import csv
import io
import os
from datetime import datetime

import boto3
import freezegun
import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb, mock_s3

from app.main import app
from app.models.admin import AdminModel
from app.models.project_karte import (
    KarteNotifyUpdateHistoryAttribute,
    ProjectKarteModel,
)
from app.models.project_survey import (
    AnswersAttribute,
    EvaluationSupporterAttribute,
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
)
from app.models.survey_master import QuestionsAttribute, SurveyMasterModel
from app.resources.const import DataType, UserRoleType
from app.service.project_service import ProjectService
from app.service.survey_service import SurveyService

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
@mock_s3
class TestExportSurveys:
    @pytest.mark.parametrize(
        "summary_month_from, admin_model, survey_manster_model, models",
        [
            (
                202207,
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=4,
                    name="サービスアンケート",
                    type="service",
                    timing="monthly",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[
                        QuestionsAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            required=True,
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            format="checkbox",
                            summary_type="point",
                            other_description="その他記載欄（任意）",
                            disabled=False,
                        )
                    ],
                    question_flow=[
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "prevId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                            "defaultNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "matchNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "isFirst": True,
                            "isEnd": True,
                        }
                    ],
                    is_latest=0,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7077",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=5,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/09",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
            ),
            # アンケート評価対象者（supporter_users）が存在しない場合 ※2件全て
            (
                202207,
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=4,
                    name="サービスアンケート",
                    type="service",
                    timing="monthly",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[
                        QuestionsAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            required=True,
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            format="checkbox",
                            summary_type="point",
                            other_description="その他記載欄（任意）",
                            disabled=False,
                        )
                    ],
                    question_flow=[
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "prevId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                            "defaultNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "matchNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "isFirst": True,
                            "isEnd": True,
                        }
                    ],
                    is_latest=0,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                [
                    # supporter_users が存在しない
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # supporter_users が存在しない
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7077",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=5,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/09",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
            ),
            # アンケート評価対象者（supporter_users）が存在しない場合 ※存在するデータと存在しないデータの混在
            (
                202207,
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=4,
                    name="サービスアンケート",
                    type="service",
                    timing="monthly",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[
                        QuestionsAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            required=True,
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            format="checkbox",
                            summary_type="point",
                            other_description="その他記載欄（任意）",
                            disabled=False,
                        )
                    ],
                    question_flow=[
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "prevId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                            "defaultNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "matchNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "isFirst": True,
                            "isEnd": True,
                        }
                    ],
                    is_latest=0,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                [
                    # supporter_users が存在しない
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # supporter_users が存在しない
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7077",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=5,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/09",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # supporter_users が存在する
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7078",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # supporter_users が存在する
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7079",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=5,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/09",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
            ),
            # アンケート評価対象者（supporter_users）が存在しない場合 ※2件全て
            # かつ、案件アンケートのサービス責任者が設定されていない場合
            # アクセラレーター列は空で出力される
            (
                202207,
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=4,
                    name="サービスアンケート",
                    type="service",
                    timing="monthly",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[
                        QuestionsAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            required=True,
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            format="checkbox",
                            summary_type="point",
                            other_description="その他記載欄（任意）",
                            disabled=False,
                        )
                    ],
                    question_flow=[
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "prevId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                            "defaultNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "matchNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "isFirst": True,
                            "isEnd": True,
                        }
                    ],
                    is_latest=0,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                [
                    # supporter_users が存在しない
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                        service_manager_id=None,
                        service_manager_name=None,
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # supporter_users が存在しない
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7077",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=5,
                        survey_type="service",
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
                        service_manager_id=None,
                        service_manager_name=None,
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
                            )
                        ],
                        summary_month="2022/09",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
            ),
        ],
    )
    def test_summary_month_from_ok(
        self,
        mock_auth_admin,
        mocker,
        admin_model,
        survey_manster_model,
        summary_month_from,
        models,
    ):
        """RAW CSVアンケートの集計月(From)指定"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        admin_mock = mocker.patch.object(AdminModel, "get")
        admin_mock.return_value = admin_model

        # ###########################
        # モック化
        # ###########################
        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for model in models:
            model.save()

        mock = mocker.patch.object(
            SurveyService, "is_visible_survey_for_get_survey_by_id"
        )
        mock.return_value = True

        mock = mocker.patch.object(SurveyService, "get_survey_master_name_from_cache")
        mock.return_value = "アンケートマスタ1"

        mock = mocker.patch.object(SurveyService, "get_accelerator_display_names")
        mock.return_value = "IST_田中太郎"

        mock = mocker.patch.object(SurveyService, "get_user_name")
        mock.return_value = "山田太郎"

        mock = mocker.patch.object(SurveyMasterModel, "get")
        mock.return_value = survey_manster_model

        mock = mocker.patch.object(os, "getenv")
        mock.return_value = "dev"

        mock = mocker.patch.object(SurveyService, "get_survey_master_questions_number")
        mock.return_value = 1

        s3 = boto3.client("s3")
        s3.create_bucket(
            Bucket="partnerportal-dev-common-upload",
            CreateBucketConfiguration={"LocationConstraint": "ap-northeast-1"},
        )

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/surveys/export?summaryMonthFrom={summary_month_from}&mode=raw&type=service",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 200

        # オブジェクトのキーの一覧取得
        s3_list_object_v2_response = s3.list_objects_v2(
            Bucket="partnerportal-dev-common-upload",
            Prefix="export/survey/",
        )
        file_key_list = [
            obj["Key"] for obj in s3_list_object_v2_response.get("Contents", [])
        ]
        print(f"file_key_list: {file_key_list}")
        assert len(file_key_list) > 0

        # CSVファイルの取得
        for key in file_key_list:
            print(f"key: {key}")
            # get_objectで取得
            s3_get_object_response = s3.get_object(
                Bucket="partnerportal-dev-common-upload", Key=key
            )
            content = s3_get_object_response["Body"].read()
            csv_content = content.decode("utf-8")
            print(f"=== csv: {key} start. ===")
            print(csv_content)
            print(f"=== csv: {key} end. ===")

    @pytest.mark.parametrize(
        "summary_month_to, admin_model, survey_manster_model, models",
        [
            (
                202307,
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=4,
                    name="サービスアンケート",
                    type="service",
                    timing="monthly",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[
                        QuestionsAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            required=True,
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            format="checkbox",
                            summary_type="point",
                            other_description="その他記載欄（任意）",
                            disabled=False,
                        )
                    ],
                    question_flow=[
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "prevId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                            "defaultNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "matchNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "isFirst": True,
                            "isEnd": True,
                        }
                    ],
                    is_latest=0,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7077",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=5,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/09",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
            )
        ],
    )
    def test_summary_month_to_ok(
        self,
        mock_auth_admin,
        mocker,
        admin_model,
        survey_manster_model,
        summary_month_to,
        models,
    ):
        """RAW CSVアンケートの集計月(To)指定"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        admin_mock = mocker.patch.object(AdminModel, "get")
        admin_mock.return_value = admin_model

        # ###########################
        # モック化
        # ###########################
        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for model in models:
            model.save()

        mock = mocker.patch.object(
            SurveyService, "is_visible_survey_for_get_survey_by_id"
        )
        mock.return_value = True

        mock = mocker.patch.object(SurveyService, "get_survey_master_name_from_cache")
        mock.return_value = "アンケートマスタ1"

        mock = mocker.patch.object(SurveyService, "get_accelerator_display_names")
        mock.return_value = "IST_田中太郎"

        mock = mocker.patch.object(SurveyService, "get_user_name")
        mock.return_value = "山田太郎"

        mock = mocker.patch.object(SurveyMasterModel, "get")
        mock.return_value = survey_manster_model

        mock = mocker.patch.object(os, "getenv")
        mock.return_value = "dev"

        mock = mocker.patch.object(SurveyService, "get_survey_master_questions_number")
        mock.return_value = 1

        s3 = boto3.client("s3")
        s3.create_bucket(
            Bucket="partnerportal-dev-common-upload",
            CreateBucketConfiguration={"LocationConstraint": "ap-northeast-1"},
        )

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/surveys/export?summaryMonthTo={summary_month_to}&mode=raw&type=service",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 200

        # オブジェクトのキーの一覧取得
        s3_list_object_v2_response = s3.list_objects_v2(
            Bucket="partnerportal-dev-common-upload",
            Prefix="export/survey/",
        )
        file_key_list = [
            obj["Key"] for obj in s3_list_object_v2_response.get("Contents", [])
        ]
        print(f"file_key_list: {file_key_list}")
        assert len(file_key_list) > 0

        # CSVファイルの取得
        for key in file_key_list:
            print(f"key: {key}")
            # get_objectで取得
            s3_get_object_response = s3.get_object(
                Bucket="partnerportal-dev-common-upload", Key=key
            )
            content = s3_get_object_response["Body"].read()
            csv_content = content.decode("utf-8")
            print(f"=== csv: {key} start. ===")
            print(csv_content)
            print(f"=== csv: {key} end. ===")

    @pytest.mark.parametrize(
        "plan_survey_request_date_from, admin_model, models, karte_models, exec_ref_datetime_str, expected_data_list",
        [
            (
                202201,
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7086",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="completion",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a9911",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/30 23:59",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 集計対象外(service, is_not_summary=True)
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7096",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
                        actual_survey_response_datetime="2022/01/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=True,
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 集計対象外(completion, is_not_summary=True)
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7106",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="completion",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
                        actual_survey_response_datetime="2022/01/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=True,
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/01/06",
                        start_datetime="2022/01/06 00:00",
                        start_time="00:00",
                        end_time="23:00",
                        supporter_ids=set(["7b36cc66-fe66-495f-b416-16ecc879fbaa"]),
                        draft_supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                        last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_update_history=[
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                user_name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/10",
                            ),
                        ],
                        customer_user_ids=None,
                        start_support_actual_time="00:00",
                        end_support_actual_time="23:00",
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectKarteModel(
                        karte_id="69695430-8dd9-472a-8940-d537145f311e",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/01/13",
                        start_datetime="2022/01/13 00:00",
                        start_time="00:00",
                        end_time="23:00",
                        supporter_ids=set(["7b36cc66-fe66-495f-b416-16ecc879fbaa"]),
                        draft_supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                        last_update_datetime=datetime(2022, 1, 31),
                        karte_notify_last_update_datetime=datetime(2022, 1, 31),
                        karte_notify_update_history=[
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                user_name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/02/01",
                            ),
                        ],
                        customer_user_ids=None,
                        start_support_actual_time="00:00",
                        end_support_actual_time="23:00",
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 対象外（dateが基準月以外）
                    ProjectKarteModel(
                        karte_id="2e560bda-4fc6-4ef2-8d0e-363b44c3c4ab",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/02/01",
                        start_datetime="2022/02/01 00:00",
                        start_time="00:00",
                        end_time="23:00",
                        supporter_ids=set(["7b36cc66-fe66-495f-b416-16ecc879fbaa"]),
                        draft_supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                        last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_update_history=[
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                user_name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/10",
                            ),
                        ],
                        customer_user_ids=None,
                        start_support_actual_time="00:00",
                        end_support_actual_time="23:00",
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
                # アンケート評価対象者の変更
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # アクセラレーターは変更されない（plan_survey_response_datetimeを過ぎている）
                "2022-02-01 00:00:00",
                [
                    # 想定結果(出力されるCSVのデータを辞書形式にしたもの)
                    {
                        "アクセラレーター氏名": "田中 三郎",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "2",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "3",
                    },
                    {
                        "アクセラレーター氏名": "田中 次郎",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "2",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "3",
                    },
                ],
            ),
            (
                202201,
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7086",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="completion",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a9911",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/30 23:59",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 集計対象外(service, is_not_summary=True)
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7096",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
                        actual_survey_response_datetime="2022/01/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=True,
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 集計対象外(completion, is_not_summary=True)
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7106",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="completion",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
                        actual_survey_response_datetime="2022/01/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=True,
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/01/06",
                        start_datetime="2022/01/06 00:00",
                        start_time="00:00",
                        end_time="23:00",
                        supporter_ids=set(["7b36cc66-fe66-495f-b416-16ecc879fbaa"]),
                        draft_supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                        last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_update_history=[
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                user_name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/10",
                            ),
                        ],
                        customer_user_ids=None,
                        start_support_actual_time="00:00",
                        end_support_actual_time="23:00",
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectKarteModel(
                        karte_id="69695430-8dd9-472a-8940-d537145f311e",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/01/13",
                        start_datetime="2022/01/13 00:00",
                        start_time="00:00",
                        end_time="23:00",
                        supporter_ids=set(["7b36cc66-fe66-495f-b416-16ecc879fbaa"]),
                        draft_supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                        last_update_datetime=datetime(2022, 1, 31),
                        karte_notify_last_update_datetime=datetime(2022, 1, 31),
                        karte_notify_update_history=[
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                user_name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/02/01",
                            ),
                        ],
                        customer_user_ids=None,
                        start_support_actual_time="00:00",
                        end_support_actual_time="23:00",
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 対象外（dateが基準月以外）
                    ProjectKarteModel(
                        karte_id="2e560bda-4fc6-4ef2-8d0e-363b44c3c4ab",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/02/01",
                        start_datetime="2022/02/01 00:00",
                        start_time="00:00",
                        end_time="23:00",
                        supporter_ids=set(["7b36cc66-fe66-495f-b416-16ecc879fbaa"]),
                        draft_supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                        last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_update_history=[
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                user_name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/10",
                            ),
                        ],
                        customer_user_ids=None,
                        start_support_actual_time="00:00",
                        end_support_actual_time="23:00",
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
                # アンケート評価対象者の変更
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # 2件：アクセラレーターは変更される（plan_survey_response_datetimeを過ぎていない）
                # 1件：変更されない（plan_survey_response_datetimeを過ぎている）
                "2022-01-31 00:00:00",
                [
                    # 想定結果(出力されるCSVのデータを辞書形式にしたもの)
                    {
                        "アクセラレーター氏名": "カルテ更新者1",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                    {
                        "アクセラレーター氏名": "カルテ更新者2",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                    {
                        "アクセラレーター氏名": "カルテ更新者3",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                    {
                        "アクセラレーター氏名": "田中 三郎",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "2",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "3",
                    },
                    {
                        "アクセラレーター氏名": "田中 次郎",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "",
                        "修了アンケート継続意思": "",
                        "修了アンケート紹介可能性": "",
                        "修了アンケートN数": "",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "1",
                    },
                ],
            ),
            (
                202201,
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                [
                    # アンケート評価対象者が更新されていないデータ
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                            SupporterUserAttribute(
                                id="6f24a6c1-985a-46d7-8850-7dc8053d2be4",
                                name="（ソルバー）佐藤 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                            # ソルバーと判定されないデータ（ソルバーを識別する文字列が名前の途中にあるため）
                            SupporterUserAttribute(
                                id="6f24a6c1-985a-46d7-8850-7dc8053d2be4",
                                name="佐藤（ソルバー）三郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # アンケート評価対象者が更新済みのデータ
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7086",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="completion",
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
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                        ],
                        evaluation_supporters=[
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                        ],
                        supporter_users_before_update=[
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                            SupporterUserAttribute(
                                id="6f24a6c1-985a-46d7-8850-7dc8053d2be4",
                                name="（ソルバー）佐藤 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                        ],
                        is_updated_evaluation_supporters=True,
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a9911",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/30 23:59",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 集計対象外(service, is_not_summary=True)
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7096",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
                        actual_survey_response_datetime="2022/01/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=True,
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 集計対象外(completion, is_not_summary=True)
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7106",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="completion",
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
                            )
                        ],
                        summary_month="2022/01",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 23:59",
                        actual_survey_response_datetime="2022/01/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=True,
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/01/06",
                        start_datetime="2022/01/06 00:00",
                        start_time="00:00",
                        end_time="23:00",
                        supporter_ids=set(["7b36cc66-fe66-495f-b416-16ecc879fbaa"]),
                        draft_supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                        last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_update_history=[
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                user_name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/10",
                            ),
                        ],
                        customer_user_ids=None,
                        start_support_actual_time="00:00",
                        end_support_actual_time="23:00",
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectKarteModel(
                        karte_id="69695430-8dd9-472a-8940-d537145f311e",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/01/13",
                        start_datetime="2022/01/13 00:00",
                        start_time="00:00",
                        end_time="23:00",
                        supporter_ids=set(["7b36cc66-fe66-495f-b416-16ecc879fbaa"]),
                        draft_supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                        last_update_datetime=datetime(2022, 1, 31),
                        karte_notify_last_update_datetime=datetime(2022, 1, 31),
                        karte_notify_update_history=[
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                user_name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/02/01",
                            ),
                        ],
                        customer_user_ids=None,
                        start_support_actual_time="00:00",
                        end_support_actual_time="23:00",
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 対象外（dateが基準月以外）
                    ProjectKarteModel(
                        karte_id="2e560bda-4fc6-4ef2-8d0e-363b44c3c4ab",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/02/01",
                        start_datetime="2022/02/01 00:00",
                        start_time="00:00",
                        end_time="23:00",
                        supporter_ids=set(["7b36cc66-fe66-495f-b416-16ecc879fbaa"]),
                        draft_supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                        last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_last_update_datetime=datetime(2022, 1, 10),
                        karte_notify_update_history=[
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                user_name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/01/10",
                            ),
                        ],
                        customer_user_ids=None,
                        start_support_actual_time="00:00",
                        end_support_actual_time="23:00",
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
                # アンケート評価対象者の変更
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # アクセラレーターにソルバーが登録されている場合（個別カルテを更新していなくても評価対象者とする）
                # 2件：アクセラレーターは変更される（plan_survey_response_datetimeを過ぎていない）
                # 1件：変更されない（plan_survey_response_datetimeを過ぎている）
                "2022-01-31 00:00:00",
                [
                    # 想定結果(出力されるCSVのデータを辞書形式にしたもの)
                    {
                        "アクセラレーター氏名": "（ソルバー）佐藤 太郎",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                    {
                        "アクセラレーター氏名": "（ソルバー）佐藤 次郎",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                    {
                        "アクセラレーター氏名": "カルテ更新者1",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                    {
                        "アクセラレーター氏名": "カルテ更新者2",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                    {
                        "アクセラレーター氏名": "カルテ更新者3",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                    {
                        "アクセラレーター氏名": "田中 三郎",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "2",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "3",
                    },
                    {
                        "アクセラレーター氏名": "田中 次郎",
                        "課名 （支援者が属する課）": "IST",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "",
                        "修了アンケート継続意思": "",
                        "修了アンケート紹介可能性": "",
                        "修了アンケートN数": "",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "1",
                    },
                ],
            ),
        ],
    )
    def test_summary_summary_ok(
        self,
        mock_auth_admin,
        mocker,
        admin_model,
        plan_survey_request_date_from,
        models,
        karte_models,
        exec_ref_datetime_str,
        expected_data_list,
    ):
        """支援者別CSVアンケート"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        admin_mock = mocker.patch.object(AdminModel, "get")
        admin_mock.return_value = admin_model

        # ###########################
        # モック化
        # ###########################
        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for model in models:
            model.save()

        ProjectKarteModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for karte_model in karte_models:
            karte_model.save()

        mock = mocker.patch.object(
            SurveyService, "is_visible_survey_for_get_survey_by_id"
        )
        mock.return_value = True

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        mock = mocker.patch.object(os, "getenv")
        mock.return_value = "dev"

        s3 = boto3.client("s3")
        s3.create_bucket(
            Bucket="partnerportal-dev-common-upload",
            CreateBucketConfiguration={"LocationConstraint": "ap-northeast-1"},
        )

        # 現在時間の固定(freezegun)
        with freezegun.freeze_time(exec_ref_datetime_str, tz_offset=+9):
            # ###########################
            # テスト実行
            # ###########################
            response = client.get(
                f"/api/surveys/export?planSurveyRequestDateFrom={plan_survey_request_date_from}&mode=supporter",
                headers=REQUEST_HEADERS,
            )

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 200

        # オブジェクトのキーの一覧取得
        s3_list_object_v2_response = s3.list_objects_v2(
            Bucket="partnerportal-dev-common-upload",
            Prefix="export/survey_supporter/",
        )
        file_key_list = [
            obj["Key"] for obj in s3_list_object_v2_response.get("Contents", [])
        ]
        print(f"file_key_list: {file_key_list}")
        assert len(file_key_list) > 0

        # CSVファイルの取得
        for key in file_key_list:
            print(f"key: {key}")
            # get_objectで取得
            s3_get_object_response = s3.get_object(
                Bucket="partnerportal-dev-common-upload", Key=key
            )
            content = s3_get_object_response["Body"].read()
            csv_content = content.decode("utf-8")
            print(f"=== csv: {key} start. ===")
            print(csv_content)
            print(f"=== csv: {key} end. ===")
            row_list = [
                row
                for row in csv.DictReader(
                    io.TextIOWrapper(
                        io.BytesIO(content), encoding="utf-8-sig", newline=""
                    )
                )
            ]
            print(f"row_list: {row_list}")
            print(f"expected_data_list: {expected_data_list}")
            assert row_list == expected_data_list

    @pytest.mark.parametrize(
        "plan_survey_request_date_to, admin_model, models, expected_data_list",
        [
            (
                202301,
                AdminModel(
                    id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    data_type=DataType.ADMIN,
                    name="ソニー太郎",
                    email="taro.sony@example.com",
                    job="部長",
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    organization_name="IST",
                    cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                    roles={UserRoleType.SYSTEM_ADMIN.key},
                    otp_verified_token="111111",
                ),
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7077",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="51ff73f4-2414-421c-b64c-9981f988b331",
                            name="佐藤 三郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="IST",
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 集計対象外(service, is_not_summary=True)
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7091",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="service",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="51ff73f4-2414-421c-b64c-9981f988b331",
                            name="佐藤 三郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="IST",
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
                            )
                        ],
                        summary_month="2022/08",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime="2022/01/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=True,
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7086",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="completion",
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7087",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="completion",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="51ff73f4-2414-421c-b64c-9981f988b331",
                            name="佐藤 三郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="IST",
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
                            )
                        ],
                        summary_month="2022/08",
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                    # 集計対象外(completion, is_not_summary=True)
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7092",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type="completion",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        project_name="サンプルプロジェクト",
                        customer_success="DXの実現",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        supporter_organization_name="IST",
                        support_date_from="2021/06/01",
                        support_date_to="2022/12/31",
                        main_supporter_user=SupporterUserAttribute(
                            id="51ff73f4-2414-421c-b64c-9981f988b331",
                            name="佐藤 三郎",
                            organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                            organization_name="IST",
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
                            )
                        ],
                        summary_month="2022/08",
                        plan_survey_request_datetime="2022/01/15 01:00",
                        actual_survey_request_datetime="2022/01/15 01:10",
                        plan_survey_response_datetime="2022/01/31 17:00",
                        actual_survey_response_datetime="2022/01/30 10:00",
                        is_finished=True,
                        is_disclosure=True,
                        is_not_summary=True,
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
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
                [
                    # 想定結果(出力されるCSVのデータを辞書形式にしたもの)
                    {
                        "課名 （粗利メイン課）": "IST",
                        "プロデューサー氏名": "佐藤 三郎",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                    {
                        "課名 （粗利メイン課）": "IST",
                        "プロデューサー氏名": "田中 三郎",
                        "サービスアンケート総合満足度": "3.00",
                        "サービスアンケートN数": "1",
                        "修了アンケート総合満足度": "3.00",
                        "修了アンケート継続意思": "0%",
                        "修了アンケート紹介可能性": "3.00",
                        "修了アンケートN数": "1",
                        "総合満足度評価": "3.00",
                        "総合満足度N数": "2",
                    },
                ],
            )
        ],
    )
    def test_summary_organization_ok(
        self,
        mock_auth_admin,
        mocker,
        admin_model,
        plan_survey_request_date_to,
        models,
        expected_data_list,
    ):
        """組織別CSVアンケート"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        admin_mock = mocker.patch.object(AdminModel, "get")
        admin_mock.return_value = admin_model

        # ###########################
        # モック化
        # ###########################
        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        for model in models:
            model.save()

        mock = mocker.patch.object(
            SurveyService, "is_visible_survey_for_get_survey_by_id"
        )
        mock.return_value = True

        mock = mocker.patch.object(
            ProjectService, "cached_get_supporter_organization_name"
        )
        mock.return_value = "IST"

        mock = mocker.patch.object(os, "getenv")
        mock.return_value = "dev"

        s3 = boto3.client("s3")
        s3.create_bucket(
            Bucket="partnerportal-dev-common-upload",
            CreateBucketConfiguration={"LocationConstraint": "ap-northeast-1"},
        )

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/surveys/export?planSurveyRequestDateTo={plan_survey_request_date_to}&mode=organization",
            headers=REQUEST_HEADERS,
        )

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 200

        # オブジェクトのキーの一覧取得
        s3_list_object_v2_response = s3.list_objects_v2(
            Bucket="partnerportal-dev-common-upload",
            Prefix="export/survey_organization/",
        )
        file_key_list = [
            obj["Key"] for obj in s3_list_object_v2_response.get("Contents", [])
        ]
        print(f"file_key_list: {file_key_list}")
        assert len(file_key_list) > 0

        # CSVファイルの取得
        for key in file_key_list:
            print(f"key: {key}")
            # get_objectで取得
            s3_get_object_response = s3.get_object(
                Bucket="partnerportal-dev-common-upload", Key=key
            )
            content = s3_get_object_response["Body"].read()
            csv_content = content.decode("utf-8")
            print(f"=== csv: {key} start. ===")
            print(csv_content)
            print(f"=== csv: {key} end. ===")
            row_list = [
                row
                for row in csv.DictReader(
                    io.TextIOWrapper(
                        io.BytesIO(content), encoding="utf-8-sig", newline=""
                    )
                )
            ]
            print(f"row_list: {row_list}")
            print(f"expected_data_list: {expected_data_list}")
            assert row_list == expected_data_list
