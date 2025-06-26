from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.survey_master import (
    QuestionFlowAttribute,
    QuestionsAttribute,
    SurveyMasterModel,
)
from app.resources.const import UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetSurveyMasters:
    def setup_method(self, method):
        SurveyMasterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        SurveyMasterModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            revision=1,
            name="クイックアンケート1",
            type="quick",
            timing="monthly",
            init_send_day_setting=20,
            init_answer_limit_day_setting=5,
            is_disclosure=True,
            questions=[
                QuestionsAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    required=False,
                    description="当月のSSAP支援にはご満足いただけましたか？",
                    format="checkbox",
                    disabled=False,
                    is_new=True,
                )
            ],
            question_flow=[
                QuestionFlowAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    condition_choice_ids=[
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ],
                )
            ],
            is_latest=1,
            create_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()

        SurveyMasterModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
            revision=1,
            name="クイックアンケート2",
            type="quick",
            timing="monthly",
            init_send_day_setting=20,
            init_answer_limit_day_setting=5,
            is_disclosure=True,
            questions=[
                QuestionsAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    required=False,
                    description="当月のSSAP支援にはご満足いただけましたか？",
                    format="checkbox",
                    disabled=False,
                    is_new=True,
                )
            ],
            question_flow=[
                QuestionFlowAttribute(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    condition_choice_ids=[
                        "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    ],
                )
            ],
            is_latest=1,
            create_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()

    def teardown_method(self, method):
        SurveyMasterModel.delete_table()

    @pytest.mark.parametrize(
        "expected",
        [
            {
                "total": 2,
                "masters": [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "revision": 1,
                        "name": "クイックアンケート1",
                        "type": "quick",
                        "timing": "monthly",
                        "initSendDaySetting": 20,
                        "initAnswerLimitDaySetting": 5,
                        "isDisclosure": True,
                        "isLatest": 1,
                        "memo": None,
                        "status": "in_operation",
                        "questionCount": 1,
                        "updateAt": "2022-04-23T03:21:39.356+09:00",
                    },
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270e",
                        "revision": 1,
                        "name": "クイックアンケート2",
                        "type": "quick",
                        "timing": "monthly",
                        "initSendDaySetting": 20,
                        "initAnswerLimitDaySetting": 5,
                        "isDisclosure": True,
                        "isLatest": 1,
                        "memo": None,
                        "status": "in_operation",
                        "questionCount": 1,
                        "updateAt": "2022-04-23T03:21:39.356+09:00",
                    },
                ],
            },
        ],
    )
    def test_ok(self, mock_auth_admin, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get("/api/survey-masters", headers=REQUEST_HEADERS)

        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    def test_survey_master_not_exist(self, mock_auth_admin):
        """空配列の返却チェック"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        SurveyMasterModel.delete_table()
        SurveyMasterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        expected = {"total": 0, "masters": []}

        response = client.get("/api/survey-masters", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
