import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.survey_master import SurveyMasterModel

client = TestClient(app)


class TestGetSurveyMasters:
    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    SurveyMasterModel(
                        id="0f8c995e-db5e-450e-b073-9df81c1ede0f",
                        revision=3,
                        name="修了アンケート",
                        type="quick",
                        timing="monthly",
                        init_send_day_setting=20,
                        init_answer_limit_day_setting=5,
                        is_disclosure=True,
                        is_latest=1,
                    ),
                    SurveyMasterModel(
                        id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                        revision=4,
                        name="修了アンケート",
                        type="quick",
                        timing="monthly",
                        init_send_day_setting=10,
                        init_answer_limit_day_setting=6,
                        is_disclosure=True,
                        is_latest=0,
                    ),
                ],
                {
                    "total": 2,
                    "masters": [
                        {
                            "id": "0f8c995e-db5e-450e-b073-9df81c1ede0f",
                            "revision": 3,
                            "name": "修了アンケート",
                            "type": "quick",
                            "timing": "monthly",
                            "initSendDaySetting": 20,
                            "initAnswerLimitDaySetting": 5,
                            "isDisclosure": True,
                            "isLatest": 1
                        },
                        {
                            "id": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                            "revision": 4,
                            "name": "修了アンケート",
                            "type": "quick",
                            "timing": "monthly",
                            "initSendDaySetting": 10,
                            "initAnswerLimitDaySetting": 6,
                            "isDisclosure": True,
                            "isLatest": 0
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, model_list, expected):
        """アンケートひな形一覧情報の取得成功"""
        mock = mocker.patch.object(
            SurveyMasterModel.is_latest_name_index, "query"
        )
        mock.return_value = model_list

        response = client.get("/api/survey-masters")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
