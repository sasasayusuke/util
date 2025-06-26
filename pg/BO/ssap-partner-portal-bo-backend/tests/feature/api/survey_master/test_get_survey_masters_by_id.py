import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.survey_master import (
    SurveyMasterModel,
    QuestionsAttribute,
    ChoicesSubAttribute,
    GroupSubAttribute,
)
from app.resources.const import UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetSurveyMastersById:
    @pytest.mark.parametrize(
        "id, model_list, expected",
        [
            (
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                [
                    SurveyMasterModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        revision=2,
                        name="修了アンケート",
                        type="quick",
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
                                choices=[
                                    ChoicesSubAttribute(
                                        description="支援者の対応姿勢",
                                        group=[
                                            GroupSubAttribute(
                                                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                                title="string",
                                                disabled=False,
                                            )
                                        ],
                                    )
                                ],
                                other_description="その他記載欄（任意）",
                                disabled=False,
                            )
                        ],
                        question_flow=[
                            {
                                "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "condition_choice_ids": [
                                    "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                ],
                            }
                        ],
                        is_latest=1,
                        memo="string",
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2022-04-23T03:21:39.356Z",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2022-04-23T03:21:39.356Z",
                        version="1",
                    ),
                    SurveyMasterModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        revision=1,
                        name="修了アンケート",
                        type="quick",
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
                                choices=[
                                    ChoicesSubAttribute(
                                        description="支援者の対応姿勢",
                                        group=[
                                            GroupSubAttribute(
                                                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                                title="string",
                                                disabled=False,
                                            )
                                        ],
                                    )
                                ],
                                other_description="その他記載欄（任意）",
                                disabled=False,
                            )
                        ],
                        question_flow=[
                            {
                                "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "condition_choice_ids": [
                                    "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                ],
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
                    SurveyMasterModel(
                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        revision=0,
                        name="修了アンケート",
                        type="quick",
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
                                choices=[
                                    ChoicesSubAttribute(
                                        description="支援者の対応姿勢",
                                        group=[
                                            GroupSubAttribute(
                                                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                                title="string",
                                                disabled=False,
                                            )
                                        ],
                                    )
                                ],
                                other_description="その他記載欄（任意）",
                                disabled=False,
                            )
                        ],
                        question_flow=[
                            {
                                "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "condition_choice_ids": [
                                    "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                ],
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
                ],
                {
                    "total": 3,
                    "masters": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "revision": 0,
                            "name": "修了アンケート",
                            "type": "quick",
                            "timing": "monthly",
                            "initSendDaySetting": 20,
                            "initAnswerLimitDaySetting": 5,
                            "isDisclosure": True,
                            "isLatest": 0,
                            "memo": "string",
                            "status": "draft",
                            "questionCount": 1,
                            "updateAt": "2022-04-23T03:21:39.356+09:00",
                        },
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "revision": 2,
                            "name": "修了アンケート",
                            "type": "quick",
                            "timing": "monthly",
                            "initSendDaySetting": 20,
                            "initAnswerLimitDaySetting": 5,
                            "isDisclosure": True,
                            "isLatest": 1,
                            "memo": "string",
                            "status": "in_operation",
                            "questionCount": 1,
                            "updateAt": "2022-04-23T03:21:39.356+09:00",
                        },
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "revision": 1,
                            "name": "修了アンケート",
                            "type": "quick",
                            "timing": "monthly",
                            "initSendDaySetting": 20,
                            "initAnswerLimitDaySetting": 5,
                            "isDisclosure": True,
                            "isLatest": 0,
                            "memo": "string",
                            "status": "archive",
                            "questionCount": 1,
                            "updateAt": "2022-04-23T03:21:39.356+09:00",
                        },
                    ],
                },
            )
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, id, model_list, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(SurveyMasterModel.id_is_latest_index, "query")
        mock.return_value = [model_list[0]]
        mock = mocker.patch.object(SurveyMasterModel, "query")
        mock.return_value = model_list

        response = client.get(f"/api/survey-masters/{id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    def test_survey_master_not_found(self, mocker, mock_auth_admin):
        """アンケートマスタが存在しない時のテスト"""
        id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(SurveyMasterModel.id_is_latest_index, "query")
        mock.return_value = []
        mock = mocker.patch.object(SurveyMasterModel, "query")
        mock.return_value = []

        response = client.get(f"/api/survey-masters/{id}", headers=REQUEST_HEADERS)

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not Found"
