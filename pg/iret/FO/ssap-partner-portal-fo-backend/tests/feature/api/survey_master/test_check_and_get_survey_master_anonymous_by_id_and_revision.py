import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.survey_master import SurveyMasterModel
from app.service.survey_master_service import SurveyMasterService
from app.service.survey_service import SurveyService
from app.schemas.survey import PostCheckSurveyByIdPasswordResponse

client = TestClient(app)


class TestCheckAndGetSurveyMastersByIdAndRevison:
    @pytest.mark.parametrize(
        "survey_master_id, revision, model, expected",
        [
            (
                "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                1,
                SurveyMasterModel(
                    id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    revision=1,
                    name="修了アンケート",
                    type="quick",
                    timing="monthly",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "required": True,
                            "description": "当月のSSAP支援にはご満足いただけましたか？",
                            "format": "checkbox",
                            "summaryType": "point",
                            "choices": [
                                {
                                    "description": "支援者の対応姿勢",
                                    "group": [
                                        {
                                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            "title": "string",
                                            "disabled": False,
                                            "isNew": True,
                                        }
                                    ],
                                    "isNew": True,
                                }
                            ],
                            "otherDescription": "その他記載欄（任意）",
                            "disabled": False,
                            "isNew": True,
                        }
                    ],
                    question_flow=[
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "conditionId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "conditionChoiceIds": [
                                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                            ],
                        }
                    ],
                    is_latest=0,
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-06-01T11:20:04.332Z",
                    version=1,
                ),
                {
                    "id": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "revision": 1,
                    "name": "修了アンケート",
                    "type": "quick",
                    "timing": "monthly",
                    "initSendDaySetting": 20,
                    "initAnswerLimitDaySetting": 5,
                    "isDisclosure": True,
                    "questions": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "required": True,
                            "description": "当月のSSAP支援にはご満足いただけましたか？",
                            "format": "checkbox",
                            "summaryType": "point",
                            "choices": [
                                {
                                    "description": "支援者の対応姿勢",
                                    "group": [
                                        {
                                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            "title": "string",
                                            "disabled": False,
                                            "isNew": True,
                                        }
                                    ],
                                    "isNew": True,
                                }
                            ],
                            "otherDescription": "その他記載欄（任意）",
                            "disabled": False,
                            "isNew": True,
                        }
                    ],
                    "questionFlow": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "conditionId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "conditionChoiceIds": [
                                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                            ],
                        }
                    ],
                    "isLatest": 0,
                    "updateId": "998c3cd7-63b5-453f-9de2-5af21d565b98",
                    "updateAt": "2022-06-01T11:20:04.332+09:00",
                    "version": 1,
                },
            )
        ],
    )
    def test_ok(self, mocker, survey_master_id, revision, model, expected):
        """アンケートマスター単一取得の成功"""
        mock = mocker.patch.object(SurveyMasterService, "decompress_questions")
        mock.return_value = [
            {
                "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "required": True,
                "description": "当月のSSAP支援にはご満足いただけましたか？",
                "format": "checkbox",
                "summaryType": "point",
                "choices": [
                    {
                        "description": "支援者の対応姿勢",
                        "group": [
                            {
                                "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                "title": "string",
                                "disabled": False,
                                "isNew": True,
                            }
                        ],
                        "isNew": True,
                    }
                ],
                "otherDescription": "その他記載欄（任意）",
                "disabled": False,
                "isNew": True,
            }
        ]
        mock = mocker.patch.object(SurveyMasterService, "decompress_question_flows")
        mock.return_value = [
            {
                "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "conditionId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "conditionChoiceIds": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            }
        ]
        mock = mocker.patch.object(SurveyMasterModel, "get")
        mock.return_value = model
        mock_check_auth = mocker.patch.object(
            SurveyService, "post_check_survey_by_id_password"
        )
        mock_check_auth.return_value = PostCheckSurveyByIdPasswordResponse(
            id="7aeebd16-17b6-4d6a-a922-29f514212b87"
        )

        body = {
            "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
            "password": "]UHY4^ITo,RU",
        }
        response = client.post(
            f"/api/survey-masters/anonymous/{survey_master_id}/{revision}", json=body
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_master_id, revision", [("9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf", 1)]
    )
    def test_survey_master_not_found(self, mocker, survey_master_id, revision):
        """アンケートマスターが存在しない時のテスト"""
        survey_master_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock_check_auth = mocker.patch.object(
            SurveyService, "post_check_survey_by_id_password"
        )
        mock_check_auth.return_value = PostCheckSurveyByIdPasswordResponse(
            id="7aeebd16-17b6-4d6a-a922-29f514212b87"
        )
        mock = mocker.patch.object(SurveyMasterModel, "get")
        mock.side_effect = DoesNotExist()

        body = {
            "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdXJ2ZXlfaWQiOiIxMTExMTQ0OS01ZDYzLTQyZDQtYWUxYi1mMGZhZjY1YTcwNzYiLCJpYXQiOjE2Nzg0MjQ0NTEuNzk4MjM1LCJleHAiOjE2ODM2NDQzOTkuNzk4MjM1fQ.w3r1YOB9mUs6x2pLkavzmyyY7m9mxQqUzA2adUt5-Ts",
            "password": "]UHY4^ITo,RU",
        }
        response = client.post(
            f"/api/survey-masters/anonymous/{survey_master_id}/{revision}", json=body
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
