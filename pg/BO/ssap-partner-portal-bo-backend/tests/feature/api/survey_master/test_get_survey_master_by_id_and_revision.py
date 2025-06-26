from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.admin import AdminModel
from app.models.survey_master import (
    QuestionFlowAttribute,
    QuestionsAttribute,
    SurveyMasterModel,
)
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestGetSurveyMasterByIdAndRevision:
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
            is_latest=0,
            create_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()

        AdminModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        AdminModel(
            id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
            organization_name=None,
            cognito_id=None,
            last_login_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            otp_secret=None,
            otp_verified_token="111111",
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()

        UserModel.create_table(read_capacity_units=5, write_capacity_units=5, wait=True)

    def teardown_method(self, method):
        SurveyMasterModel.delete_table()
        AdminModel.delete_table()
        UserModel.delete_table()

    @pytest.mark.parametrize(
        "id, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "revision": 1,
                    "name": "クイックアンケート1",
                    "type": "quick",
                    "timing": "monthly",
                    "initSendDaySetting": 20,
                    "initAnswerLimitDaySetting": 5,
                    "isDisclosure": True,
                    "questions": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "required": False,
                            "description": "当月のSSAP支援にはご満足いただけましたか？",
                            "format": "checkbox",
                            "summaryType": None,
                            "choices": [],
                            "otherDescription": None,
                            "disabled": False,
                            "isNew": True,
                        }
                    ],
                    "questionFlow": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "conditionId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "conditionChoiceIds": [
                                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            ],
                        }
                    ],
                    "isLatest": 0,
                    "memo": None,
                    "version": 1,
                    "createId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "createAt": "2022-04-23T03:21:39.356+09:00",
                    "updateId": "906a3144-9650-4a34-8a23-3b02f3b9aeac",
                    "updateUserName": "田中太郎",
                    "updateAt": "2022-04-23T03:21:39.356+09:00",
                },
            )
        ],
    )
    def test_ok(self, mock_auth_admin, id, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(f"/api/survey-masters/{id}/1", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    def test_survey_master_not_found(self, mock_auth_admin):
        """アンケートマスタが存在しない時のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        revision = 1

        response = client.get(
            f"/api/survey-masters/{id}/{revision}", headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not Found"
