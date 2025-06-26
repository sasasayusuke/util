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
class TestUpdateSurveyMasterLatestById:
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

    def teardown_method(self, method):
        SurveyMasterModel.delete_table()

    @pytest.mark.parametrize(
        "id, body, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                {
                    "name": "クイックアンケート2",
                    "type": "quick",
                    "timing": "monthly",
                    "initSendDaySetting": 21,
                    "initAnswerLimitDaySetting": 6,
                    "isDisclosure": False,
                    "memo": "これはメモ2です",
                },
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "revision": 1,
                    "name": "クイックアンケート2",
                    "type": "quick",
                    "timing": "monthly",
                    "initSendDaySetting": 21,
                    "initAnswerLimitDaySetting": 6,
                    "isDisclosure": False,
                    "isLatest": 1,
                    "memo": "これはメモ2です",
                    "version": 2,
                },
            )
        ],
    )
    def test_ok(self, mock_auth_admin, id, body, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            f"/api/survey-masters/{id}/latest?version=1",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        actual.pop("updateId")
        actual.pop("updateAt")
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "id, body",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                {
                    "name": "修了アンケート2",
                    "type": "quick",
                    "timing": "monthly",
                    "initSendDaySetting": 21,
                    "initAnswerLimitDaySetting": 6,
                    "isDisclosure": False,
                    "memo": "これはメモ2です",
                },
            )
        ],
    )
    def test_conflict(self, mock_auth_admin, id, body):
        """楽観ロック制御のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            f"/api/survey-masters/{id}/latest?version=2",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 409

    @pytest.mark.parametrize(
        "id, body",
        [
            (
                "90cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                {
                    "name": "修了アンケート2",
                    "type": "quick",
                    "timing": "monthly",
                    "initSendDaySetting": 21,
                    "initAnswerLimitDaySetting": 6,
                    "isDisclosure": False,
                    "memo": "これはメモ2です",
                },
            )
        ],
    )
    def test_revision_not_exist(self, mock_auth_admin, id, body):
        """アンケートマスタが存在しない場合のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.put(
            f"/api/survey-masters/{id}/latest?version=1",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 404
