import pytest
from app.main import app
from app.models.survey_master import SurveyMasterModel
from app.service.survey_master_service import SurveyMasterService
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

client = TestClient(app)


class TestGetSurveyMastersByIdAndRevison:
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
    @pytest.mark.usefixtures("auth_supporter_user")
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

        response = client.get(f"/api/survey-masters/{survey_master_id}/{revision}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_master_id, revision", [("9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf", 1)]
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_survey_master_not_found(self, mocker, survey_master_id, revision):
        """アンケートマスターが存在しない時のテスト"""
        survey_master_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock = mocker.patch.object(SurveyMasterModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.get(f"/api/survey-masters/{survey_master_id}/{revision}")

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
