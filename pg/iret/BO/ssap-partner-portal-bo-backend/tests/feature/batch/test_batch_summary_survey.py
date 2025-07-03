from datetime import datetime

import freezegun
import pytest
from moto import mock_dynamodb
from pytz import timezone

from app.models.master import (
    BatchControlAttribute,
    MasterBatchControlModel,
    MasterSupporterOrganizationModel,
)
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
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
from app.models.survey_summary import (
    SurveySummaryAllModel,
    SurveySummarySupporterOrganizationModel,
    SurveySummaryUserModel,
)
from app.models.user import UserModel
from functions.batch_const import (
    BatchSettingConst,
    BatchStatus,
    DataType,
    MasterDataType,
    SurveyType,
)
from functions.batch_summary_survey import handler


@mock_dynamodb
@freezegun.freeze_time("2022-07-05T12:00:00.000+0900")
class TestBatchSummarySurvey:
    @pytest.mark.parametrize(
        "project_survey_models, project_karte_models, user_models, project_models, expected_survey_data",
        [
            (
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/06/06",
                        start_datetime="2022/06/06 00:00",
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
                                karte_update_date="2022/06/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                        date="2022/06/13",
                        start_datetime="2022/06/13 00:00",
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
                                karte_update_date="2022/06/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/07/01",
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
                        date="2022/07/01",
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
                                karte_update_date="2022/06/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                [
                    UserModel(
                        id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        data_type=DataType.USER,
                        name="田中 太郎",
                        email="user@example.com",
                        role="sales",
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
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                ],
                [],
                # 案件アンケート情報のアンケート評価対象者の更新後データの確認
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # アクセラレーターは変更されない（plan_survey_response_datetimeを過ぎている）
                {
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7076": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7086": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a9911": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                },
            ),
            (
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/06/06",
                        start_datetime="2022/06/06 00:00",
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
                                karte_update_date="2022/06/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                        date="2022/06/13",
                        start_datetime="2022/06/13 00:00",
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
                                karte_update_date="2022/06/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/07/05",
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
                        date="2022/07/01",
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
                                karte_update_date="2022/06/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                [
                    UserModel(
                        id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        data_type=DataType.USER,
                        name="田中 太郎",
                        email="user@example.com",
                        role="sales",
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
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                ],
                [],
                # 案件アンケート情報のアンケート評価対象者の更新後データの確認
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # 2件：アクセラレーターは変更される（plan_survey_response_datetimeを過ぎていない）
                # 1件：変更されない（plan_survey_response_datetimeを過ぎている）
                {
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7076": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7086": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a9911": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                },
            ),
            (
                [
                    # main_supporter_userが未設定のケース
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
                        main_supporter_user=None,
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/06/06",
                        start_datetime="2022/06/06 00:00",
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
                                karte_update_date="2022/06/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                        date="2022/06/13",
                        start_datetime="2022/06/13 00:00",
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
                                karte_update_date="2022/06/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/07/05",
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
                        date="2022/07/01",
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
                                karte_update_date="2022/06/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                [
                    UserModel(
                        id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        data_type=DataType.USER,
                        name="田中 太郎",
                        email="user@example.com",
                        role="sales",
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
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                ],
                [],
                # 案件アンケート情報のアンケート評価対象者の更新後データの確認
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # 2件：アクセラレーターは変更される（plan_survey_response_datetimeを過ぎていない）
                # 1件：変更されない（plan_survey_response_datetimeを過ぎている）
                {
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7076": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7086": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a9911": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                },
            ),
            (
                [
                    # アンケート評価対象者の更新が初回でない場合
                    # supporter_users_before_update が更新されないこと
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
                        main_supporter_user=None,
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                        ],
                        evaluation_supporters=[
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/06/06",
                        start_datetime="2022/06/06 00:00",
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
                                karte_update_date="2022/06/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                        date="2022/06/13",
                        start_datetime="2022/06/13 00:00",
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
                                karte_update_date="2022/06/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/07/05",
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
                        date="2022/07/01",
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
                                karte_update_date="2022/06/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                [
                    UserModel(
                        id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        data_type=DataType.USER,
                        name="田中 太郎",
                        email="user@example.com",
                        role="sales",
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
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                ],
                [],
                # 案件アンケート情報のアンケート評価対象者の更新後データの確認
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # 2件：アクセラレーターは変更される（plan_survey_response_datetimeを過ぎていない）
                # 1件：変更されない（plan_survey_response_datetimeを過ぎている）
                {
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7076": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7086": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a9911": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                },
            ),
            (
                # アクセラレーターにソルバーが登録されている場合（個別カルテを更新していなくても評価対象者とする）
                # アンケート評価対象者の更新が初回の場合
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
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/06/06",
                        start_datetime="2022/06/06 00:00",
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
                                karte_update_date="2022/06/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                        date="2022/06/13",
                        start_datetime="2022/06/13 00:00",
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
                                karte_update_date="2022/06/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/07/05",
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
                        date="2022/07/01",
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
                                karte_update_date="2022/06/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                [
                    UserModel(
                        id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        data_type=DataType.USER,
                        name="田中 太郎",
                        email="user@example.com",
                        role="sales",
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
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                ],
                [],
                # 案件アンケート情報のアンケート評価対象者の更新後データの確認
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # 2件：アクセラレーターは変更される（plan_survey_response_datetimeを過ぎていない）
                # 1件：変更されない（plan_survey_response_datetimeを過ぎている）
                {
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7076": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
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
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7086": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
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
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
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
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a9911": {
                        "supporter_users": [
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
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                },
            ),
            (
                # アクセラレーターにソルバーが登録されている場合（個別カルテを更新していなくても評価対象者とする）
                # ソルバーの判定確認
                # ・アクセラレーターの名前の先頭がソルバーを識別する文字列 -> ソルバー
                # ・アクセラレーターの名前の途中にソルバーを識別する文字列を含む -> ソルバーと判定しない
                # アンケート評価対象者の更新が初回の場合
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
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                            ),
                            # ソルバーを識別する文字列が名前の途中にあるため、ソルバーと判定しない
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="佐藤 （ソルバー） 太郎",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/06/06",
                        start_datetime="2022/06/06 00:00",
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
                                karte_update_date="2022/06/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                        date="2022/06/13",
                        start_datetime="2022/06/13 00:00",
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
                                karte_update_date="2022/06/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/07/05",
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
                        date="2022/07/01",
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
                                karte_update_date="2022/06/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                [
                    UserModel(
                        id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        data_type=DataType.USER,
                        name="田中 太郎",
                        email="user@example.com",
                        role="sales",
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
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                ],
                [],
                # 案件アンケート情報のアンケート評価対象者の更新後データの確認
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # 2件：アクセラレーターは変更される（plan_survey_response_datetimeを過ぎていない）
                # 1件：変更されない（plan_survey_response_datetimeを過ぎている）
                {
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7076": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
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
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7086": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="6f24a6c1-985a-46d7-8850-7dc8053d2be4",
                                name="（ソルバー）佐藤 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="佐藤 （ソルバー） 太郎",
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
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a9911": {
                        "supporter_users": [
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
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                },
            ),
            (
                [
                    # アクセラレーターにソルバーが登録されている場合（個別カルテを更新していなくても評価対象者とする）
                    # アンケート評価対象者の更新が初回でない場合
                    # supporter_users_before_update を元にソルバーを抽出
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
                        main_supporter_user=None,
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                        ],
                        evaluation_supporters=[
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime="2022/06/15 01:10",
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime="2022/07/01 10:00",
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
                [
                    ProjectKarteModel(
                        karte_id="dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                        project_id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        customer_id="d6121808-341d-4883-8e2c-69462acf6ccb",
                        date="2022/06/06",
                        start_datetime="2022/06/06 00:00",
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
                                karte_update_date="2022/06/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                        date="2022/06/13",
                        start_datetime="2022/06/13 00:00",
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
                                karte_update_date="2022/06/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/07/05",
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
                        date="2022/07/01",
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
                                karte_update_date="2022/06/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                [
                    UserModel(
                        id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        data_type=DataType.USER,
                        name="田中 太郎",
                        email="user@example.com",
                        role="sales",
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
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                ],
                [],
                # 案件アンケート情報のアンケート評価対象者の更新後データの確認
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # 2件：アクセラレーターは変更される（plan_survey_response_datetimeを過ぎていない）
                # 1件：変更されない（plan_survey_response_datetimeを過ぎている）
                {
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7076": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
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
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7086": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
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
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
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
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a9911": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            )
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                },
            ),
            (
                # アクセラレーターにソルバーが登録されている場合（個別カルテを更新していなくても評価対象者とする）
                # アンケート未送信の場合
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime=None,
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/07/04 23:59",
                        actual_survey_response_datetime=None,
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
                        summary_month="2022/06",
                        plan_survey_request_datetime="2022/06/15 01:00",
                        actual_survey_request_datetime=None,
                        plan_survey_response_datetime="2022/07/03 23:59",
                        actual_survey_response_datetime=None,
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
                        date="2022/06/06",
                        start_datetime="2022/06/06 00:00",
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
                                karte_update_date="2022/06/07",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                user_name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                        date="2022/06/13",
                        start_datetime="2022/06/13 00:00",
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
                                karte_update_date="2022/06/14",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                user_name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/31",
                            ),
                            # 対象外（karte_update_dateがアンケート情報のplan_survey_response_datetimeを過ぎている）
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="bb5020db-c954-48af-8142-ab2089b36158",
                                user_name="カルテ更新者X1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/07/05",
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
                        date="2022/07/01",
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
                                karte_update_date="2022/06/03",
                            ),
                            KarteNotifyUpdateHistoryAttribute(
                                user_id="b44a53df-c3eb-4dbd-9173-33df43be8346",
                                user_name="カルテ更新者X2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                                karte_update_date="2022/06/10",
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
                [
                    UserModel(
                        id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        data_type=DataType.USER,
                        name="田中 太郎",
                        email="user@example.com",
                        role="sales",
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
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                    UserModel(
                        id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                        data_type=DataType.USER,
                        name="（ソルバー）佐藤 太郎",
                        email="user@example.com",
                        role="supporter",
                        customer_id=None,
                        customer_name=None,
                        job="部長",
                        company=None,
                        supporter_organization_id=["de40733f-6be9-4fef-8229-01052f43c1e2"],
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                    UserModel(
                        id="ef6151e8-6dcd-4db0-88eb-252ec1141142",
                        data_type=DataType.USER,
                        name="（ソルバー）佐藤 三郎",
                        email="user@example.com",
                        role="supporter_mgr",
                        customer_id=None,
                        customer_name=None,
                        job="部長",
                        company=None,
                        supporter_organization_id=[
                            "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "556a3144-9650-4a34-8a23-3b02f3b9aeac"
                        ],
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at=datetime.now(),
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at=datetime.now(),
                    ),
                ],
                [
                    ProjectModel(
                        id="111a46d3-1d56-428b-ac6d-a51e263b54a8",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
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
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(
                                [
                                    "4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                    "ef6151e8-6dcd-4db0-88eb-252ec1141142"
                                ]
                            )
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at=datetime.now(),
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at=datetime.now(),
                    ),
                ],
                # 案件アンケート情報のアンケート評価対象者の更新後データの確認
                # （ProjectSurveyModel.plan_survey_response_datetime と現在日の比較）
                # 2件：アクセラレーターは変更される（plan_survey_response_datetimeを過ぎていない）
                # 1件：変更されない（plan_survey_response_datetimeを過ぎている）
                {
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7076": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                            SupporterUserAttribute(
                                id="ef6151e8-6dcd-4db0-88eb-252ec1141142",
                                name="（ソルバー）佐藤 三郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2;556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="AST;IST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a7086": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                name="カルテ更新者1",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                name="カルテ更新者2",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="53e094dc-0028-4e95-a5be-4b3629555183",
                                name="カルテ更新者3",
                                organization_id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="IST",
                            ),
                            SupporterUserAttribute(
                                id="4fb385b4-597d-4c61-92e5-7c82348e18e2",
                                name="（ソルバー）佐藤 太郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                            SupporterUserAttribute(
                                id="ef6151e8-6dcd-4db0-88eb-252ec1141142",
                                name="（ソルバー）佐藤 三郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2;556a3144-9650-4a34-8a23-3b02f3b9aeac",
                                organization_name="AST;IST",
                            ),
                        ],
                        "evaluation_supporters": [
                            EvaluationSupporterAttribute(
                                supporter_id="4501aef6-f068-41ef-ad99-1b709d4eb810",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                    "69695430-8dd9-472a-8940-d537145f311e",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="dd22b48d-485e-46fd-8c81-b4fdbd6e8c21",
                                karte_ids=[
                                    "dbf4cbb7-dcd1-4f6c-b2bd-cbe2cb1b136a",
                                ],
                            ),
                            EvaluationSupporterAttribute(
                                supporter_id="53e094dc-0028-4e95-a5be-4b3629555183",
                                karte_ids=["69695430-8dd9-472a-8940-d537145f311e"],
                            ),
                        ],
                        "supporter_users_before_update": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                        ],
                        "is_updated_evaluation_supporters": True,
                    },
                    # survey_id
                    "11111449-5d63-42d4-ae1b-f0faf65a9911": {
                        "supporter_users": [
                            SupporterUserAttribute(
                                id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                                name="田中 次郎",
                                organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                                organization_name="AST",
                            ),
                        ],
                        "evaluation_supporters": None,
                        "supporter_users_before_update": None,
                        "is_updated_evaluation_supporters": False,
                    },
                },
            ),
        ],
    )
    @freezegun.freeze_time("2022-07-05T12:00:00.000+0900")
    def test_ok_user(
        self,
        mocker,
        project_survey_models,
        project_karte_models,
        user_models,
        project_models,
        expected_survey_data,
    ):
        """支援者（営業担当者）別集計作成:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="BOアンケート情報集計データ作成処理",
                value="partnerportal-backoffice-dev-batch_summary_survey#user",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # ###########################
        # モック化
        # ###########################
        ProjectSurveyModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        for survey_model in project_survey_models:
            survey_model.save()

        ProjectKarteModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        for karte_model in project_karte_models:
            karte_model.save()

        SurveySummaryUserModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        UserModel.create_table(read_capacity_units=5, write_capacity_units=5, wait=True)
        for user_model in user_models:
            user_model.save()

        ProjectModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        for project_model in project_models:
            project_model.save()

        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        MasterSupporterOrganizationModel(
            id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION,
            name="Ideation Service Team",
            value="IST",
            attributes=None,
            order=1,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime.now(),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime.now(),
        ).save()
        MasterSupporterOrganizationModel(
            id="de40733f-6be9-4fef-8229-01052f43c1e2",
            data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION,
            name="Acceleration Service Team​",
            value="AST",
            attributes=None,
            order=1,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime.now(),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime.now(),
        ).save()

        expected = "Normal end."
        param = {"mode": "user", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

        # 検証
        for k, v in expected_survey_data.items():
            survey = ProjectSurveyModel.get(hash_key=k, range_key=DataType.SURVEY)
            if survey.supporter_users is None:
                assert v["supporter_users"] is None
            else:
                assert len(survey.supporter_users) == len(v["supporter_users"])
                for survey_sup in survey.supporter_users:
                    for expected_sup in v["supporter_users"]:
                        if survey_sup.id == expected_sup.id:
                            print(f"survey_sup: {survey_sup}")
                            print(f"expected_sup: {expected_sup}")
                            assert survey_sup.name == expected_sup.name
                            assert (
                                survey_sup.organization_id
                                == expected_sup.organization_id
                            )
                            assert (
                                survey_sup.organization_name
                                == expected_sup.organization_name
                            )
                            break
            if survey.evaluation_supporters is None:
                assert v["evaluation_supporters"] is None
            else:
                assert len(survey.evaluation_supporters) == len(
                    v["evaluation_supporters"]
                )
                for survey_eva in survey.evaluation_supporters:
                    for expected_eva in v["evaluation_supporters"]:
                        if survey_eva.supporter_id == expected_eva.supporter_id:
                            print(f"survey_eva: {survey_eva}")
                            print(f"expected_eva: {expected_eva}")
                            assert set(survey_eva.karte_ids) == set(
                                expected_eva.karte_ids
                            )
                            break
            if survey.supporter_users_before_update is None:
                assert v["supporter_users_before_update"] is None
            else:
                assert len(survey.supporter_users_before_update) == len(
                    v["supporter_users_before_update"]
                )
                for survey_sup in survey.supporter_users_before_update:
                    for expected_sup in v["supporter_users_before_update"]:
                        if survey_sup.id == expected_sup.id:
                            print(f"survey_sup: {survey_sup}")
                            print(f"expected_sup: {expected_sup}")
                            assert survey_sup.name == expected_sup.name
                            assert (
                                survey_sup.organization_id
                                == expected_sup.organization_id
                            )
                            assert (
                                survey_sup.organization_name
                                == expected_sup.organization_name
                            )
                            break

            if survey.is_updated_evaluation_supporters is None:
                assert v["is_updated_evaluation_supporters"] is False
            else:
                assert (
                    survey.is_updated_evaluation_supporters
                    == v["is_updated_evaluation_supporters"]
                )

    @pytest.mark.parametrize(
        "project_survey_models, project_models, project_karte_models",
        [
            (
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type=SurveyType.COMPLETION,
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        project_name="サンプルプロジェクト１",
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
                ],
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
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
                ],
                [
                    ProjectKarteModel(
                        karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        date="2022/07/01",
                        start_datetime="2022/07/01 13:00",
                        start_time="13:00",
                        end_time="14:00",
                        supporter_ids=[
                            "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                            "b9b67094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                            "8a990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=5,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                    ProjectKarteModel(
                        karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        date="2022/07/02",
                        start_datetime="2022/07/02 13:00",
                        start_time="13:00",
                        end_time="17:00",
                        supporter_ids=[
                            "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                            "45667094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="",
                        customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "4561b7e4-5625-4361-be0a-65f0e49829ea",
                            "78990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=0,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                ],
            ),
        ],
    )
    def test_ok_supporter_organization(
        self,
        mocker,
        project_survey_models,
        project_models,
        project_karte_models,
    ):
        """支援者組織（粗利メイン課）別集計作成:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="BOアンケート情報集計データ作成処理",
                value="partnerportal-backoffice-dev-batch_summary_survey#supporter_organization",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # TODO dynamodbモック未対応（moto.mock_dynamodbライブラリ
        mock_survey_summary_user = mocker.patch.object(SurveySummaryUserModel, "query")
        mock_survey_summary_user.return_value = []
        mock_survey_summary_supporterOrganization = mocker.patch.object(
            SurveySummarySupporterOrganizationModel, "query"
        )
        mock_survey_summary_supporterOrganization.return_value = []
        mock_survey_summary_all = mocker.patch.object(SurveySummaryAllModel, "query")
        mock_survey_summary_all.return_value = []

        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.data_type_summary_month_index, "query"
        )
        mock_project_survey.return_value = project_survey_models

        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = project_models
        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = project_karte_models

        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = []
        mock_master_supporter_organization = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master_supporter_organization.return_value = [
            MasterSupporterOrganizationModel(
                id="de40733f-6be9-4fef-8229-01052f43c1e2",
                data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION,
                name="Ideation Service Team",
                value="IST",
                attributes=None,
                order=1,
                use=True,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T04:21:39.356000+0000",
                version=1,
            ),
            MasterSupporterOrganizationModel(
                id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION,
                name="Acceleration Service Team​",
                value="AST",
                attributes=None,
                order=1,
                use=True,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T04:21:39.356000+0000",
                version=1,
            ),
        ]

        mocker.patch.object(SurveySummaryUserModel, "batch_write")
        mocker.patch.object(SurveySummarySupporterOrganizationModel, "batch_write")
        mocker.patch.object(SurveySummaryAllModel, "batch_write")
        mocker.patch.object(SurveySummaryAllModel, "save")

        expected = "Normal end."
        param = {"mode": "supporter_organization", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_survey_models, project_models, project_karte_models",
        [
            (
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type=SurveyType.COMPLETION,
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        project_name="サンプルプロジェクト１",
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
                ],
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
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
                ],
                [
                    ProjectKarteModel(
                        karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        date="2022/07/01",
                        start_datetime="2022/07/01 13:00",
                        start_time="13:00",
                        end_time="14:00",
                        supporter_ids=[
                            "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                            "b9b67094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                            "8a990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=5,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                    ProjectKarteModel(
                        karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        date="2022/07/02",
                        start_datetime="2022/07/02 13:00",
                        start_time="13:00",
                        end_time="17:00",
                        supporter_ids=[
                            "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                            "45667094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="",
                        customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "4561b7e4-5625-4361-be0a-65f0e49829ea",
                            "78990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=0,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                ],
            ),
        ],
    )
    def test_ok_all(
        self,
        mocker,
        project_survey_models,
        project_models,
        project_karte_models,
    ):
        """全集計作成:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="BOアンケート情報集計データ作成処理",
                value="partnerportal-backoffice-dev-batch_summary_survey#all",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # TODO dynamodbモック未対応（moto.mock_dynamodbライブラリ
        mock_survey_summary_user = mocker.patch.object(SurveySummaryUserModel, "query")
        mock_survey_summary_user.return_value = []
        mock_survey_summary_supporterOrganization = mocker.patch.object(
            SurveySummarySupporterOrganizationModel, "query"
        )
        mock_survey_summary_supporterOrganization.return_value = []
        mock_survey_summary_all = mocker.patch.object(SurveySummaryAllModel, "query")
        mock_survey_summary_all.return_value = []

        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.data_type_summary_month_index, "query"
        )
        mock_project_survey.return_value = project_survey_models

        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = project_models
        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = project_karte_models

        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = []

        mocker.patch.object(SurveySummaryUserModel, "batch_write")
        mocker.patch.object(SurveySummarySupporterOrganizationModel, "batch_write")
        mocker.patch.object(SurveySummaryAllModel, "batch_write")
        mocker.patch.object(SurveySummaryAllModel, "save")

        expected = "Normal end."
        param = {"mode": "all", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    def test_error(
        self,
        mocker,
    ):
        """エラー"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="BOアンケート情報集計データ作成処理",
                value="partnerportal-backoffice-dev-batch_summary_survey#all",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # TODO dynamodbモック未対応（moto.mock_dynamodbライブラリ
        mock_survey_summary_user = mocker.patch.object(SurveySummaryUserModel, "query")
        mock_survey_summary_user.return_value = []
        mock_survey_summary_supporterOrganization = mocker.patch.object(
            SurveySummarySupporterOrganizationModel, "query"
        )
        mock_survey_summary_supporterOrganization.return_value = []
        mock_survey_summary_all = mocker.patch.object(SurveySummaryAllModel, "query")
        mock_survey_summary_all.return_value = []

        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.data_type_summary_month_index, "query"
        )
        mock_project_survey.return_value = []

        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = []
        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = []

        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = []

        mocker.patch.object(SurveySummaryUserModel, "batch_write")
        mocker.patch.object(SurveySummarySupporterOrganizationModel, "batch_write")
        mocker.patch.object(SurveySummaryAllModel, "batch_write")
        mocker.patch.object(SurveySummaryAllModel, "save")

        expected = "Error end."
        param = {"mode": "xxxx", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_survey_models, project_models, project_karte_models",
        [
            (
                [
                    ProjectSurveyModel(
                        id="11111449-5d63-42d4-ae1b-f0faf65a7076",
                        data_type=DataType.SURVEY,
                        survey_master_id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        survey_master_revision=4,
                        survey_type=SurveyType.COMPLETION,
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        project_name="サンプルプロジェクト１",
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
                ],
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
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
                ],
                [
                    ProjectKarteModel(
                        karte_id="9992b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        date="2022/07/01",
                        start_datetime="2022/07/01 13:00",
                        start_time="13:00",
                        end_time="14:00",
                        supporter_ids=[
                            "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                            "b9b67094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                            "8a990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=5,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                    ProjectKarteModel(
                        karte_id="9982b0c4-496b-41c2-8273-9db2c95fe927",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        date="2022/07/02",
                        start_datetime="2022/07/02 13:00",
                        start_time="13:00",
                        end_time="17:00",
                        supporter_ids=[
                            "12316c2-a938-46b5-a1de-e7c74bbd18fa",
                            "45667094-cdab-494c-818e-d4845088269b",
                        ],
                        draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                        last_update_datetime="",
                        customer_id="12348558-c450-42d7-9c4b-62e0f22dc0ff",
                        customer_user_ids=[
                            "4561b7e4-5625-4361-be0a-65f0e49829ea",
                            "78990e25-43da-49a3-ae76-863b5219fe6a",
                        ],
                        man_hour=0,
                        is_draft=False,
                        update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    ),
                ],
            ),
        ],
    )
    def test_ok_skip_end(
        self,
        mocker,
        project_survey_models,
        project_models,
        project_karte_models,
    ):
        """処理途中（スキップ）終了"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=5, hour=12, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="BOアンケート情報集計データ作成処理",
                value="partnerportal-backoffice-dev-batch_summary_survey#user",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # TODO dynamodbモック未対応（moto.mock_dynamodbライブラリ
        mock_survey_summary_user = mocker.patch.object(SurveySummaryUserModel, "query")
        mock_survey_summary_user.return_value = []
        mock_survey_summary_supporterOrganization = mocker.patch.object(
            SurveySummarySupporterOrganizationModel, "query"
        )
        mock_survey_summary_supporterOrganization.return_value = []
        mock_survey_summary_all = mocker.patch.object(SurveySummaryAllModel, "query")
        mock_survey_summary_all.return_value = []

        mock_project_survey = mocker.patch.object(
            ProjectSurveyModel.data_type_summary_month_index, "query"
        )
        mock_project_survey.return_value = project_survey_models

        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = project_models
        mock_project_karte = mocker.patch.object(
            ProjectKarteModel.project_id_start_datetime_index, "query"
        )
        mock_project_karte.return_value = project_karte_models

        mock_user = mocker.patch.object(UserModel, "batch_get")
        mock_user.return_value = []

        mocker.patch.object(SurveySummaryUserModel, "batch_write")
        mocker.patch.object(SurveySummarySupporterOrganizationModel, "batch_write")
        mocker.patch.object(SurveySummaryAllModel, "batch_write")
        mocker.patch.object(SurveySummaryAllModel, "save")

        expected = "Skipped processing."
        param = {"mode": "user", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected
