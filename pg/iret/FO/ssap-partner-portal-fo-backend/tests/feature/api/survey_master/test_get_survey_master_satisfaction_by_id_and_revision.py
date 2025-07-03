import pytest
from jose import jwt
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.survey_master import GroupSubAttribute, ChoicesSubAttribute, QuestionsAttribute, QuestionFlowAttribute, SurveyMasterModel
from app.models.project_survey import ProjectSurveyModel
from app.service.survey_master_service import SurveyMasterService
from app.service.survey_service import SurveyService
from app.schemas.survey import PostCheckSurveyByIdResponse
from functions.batch_const import JwtSettingInfo, CipherAES
from datetime import datetime, timedelta

client = TestClient(app)


class TestGetSurveyMasterSatisfactionByIdAndRevision:
    @pytest.mark.parametrize(
        "survey_master_id, revision, survey_master_model, project_survey_model, expected",
        [
            (
                "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                1,
                SurveyMasterModel(
                    id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    questions=[
                        QuestionsAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            required=True,
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            format="checkbox",
                            summary_type="satisfaction",
                            choices=[
                                ChoicesSubAttribute(
                                    description="支援者の対応姿勢",
                                    group=[
                                        GroupSubAttribute(
                                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            title="string",
                                            disabled=False,
                                            is_new=True,
                                        )
                                    ],
                                    is_new=True,
                                )
                            ],
                            other_description="その他記載欄（任意）",
                            disabled=False,
                            is_new=True,
                        )
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
                    version=1,
                ),
                ProjectSurveyModel(
                    project_name="○○プロジェクト",
                    customer_name="ソニーグループ株式会社",
                ),
                {
                    "id": "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    "projectName": "○○プロジェクト",
                    "customerName": "ソニーグループ株式会社",
                    "questions": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "required": True,
                            "description": "当月のSSAP支援にはご満足いただけましたか？",
                            "format": "checkbox",
                            "summaryType": "satisfaction",
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
                    "version": 1,
                },
            )
        ],
    )
    def test_ok(
        self,
        mocker,
        survey_master_id,
        revision,
        survey_master_model,
        project_survey_model,
        expected,
    ):
        survey_id = "7aeebd16-17b6-4d6a-a922-29f514212b87",
        """アンケートマスター単一取得の成功"""
        mock = mocker.patch.object(SurveyMasterService, "decompress_questions")
        mock.return_value = {
            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            "required": True,
            "description": "当月のSSAP支援にはご満足いただけましたか？",
            "format": "checkbox",
            "summaryType": "satisfaction",
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
        },
        mock = mocker.patch.object(SurveyMasterService, "decompress_question_flows")
        mock.return_value = [
            {
                "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "conditionId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                "conditionChoiceIds": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
            }
        ]
        mock = mocker.patch.object(SurveyMasterModel, "get")
        mock.return_value = survey_master_model
        mock = mocker.patch.object(ProjectSurveyModel, "get")
        mock.return_value = project_survey_model
        mock_check_auth = mocker.patch.object(SurveyService, "post_check_survey_by_id")
        mock_check_auth.return_value = PostCheckSurveyByIdResponse(
            id="7aeebd16-17b6-4d6a-a922-29f514212b87"
        )

        # 動的にトークンを生成
        # JWT(アンケートIDと有効期限（3日）から生成された)で認証
        current_datetime = datetime.now()
        expiration_datetime = current_datetime + timedelta(days=3)

        payload = {
            "survey_id": survey_id,
            "iat": current_datetime.timestamp(),
            "exp": expiration_datetime.timestamp(),
        }
        token = jwt.encode(payload, JwtSettingInfo.SECRET_KEY, algorithm=JwtSettingInfo.ALGORITHM)

        body = {
            "token": token,
        }
        response = client.post(
            f"/api/survey-masters/satisfaction/{survey_master_id}/{revision}", json=body
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "survey_master_id, revision", [("9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf", 1)]
    )
    def test_survey_master_not_found(self, mocker, survey_master_id, revision):
        survey_id = "7aeebd16-17b6-4d6a-a922-29f514212b87"
        """アンケートマスターが存在しない時のテスト"""
        survey_master_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        mock_check_auth = mocker.patch.object(SurveyService, "post_check_survey_by_id")
        mock_check_auth.return_value = PostCheckSurveyByIdResponse(
            id="7aeebd16-17b6-4d6a-a922-29f514212b87"
        )
        mock = mocker.patch.object(SurveyMasterModel, "get")
        mock.side_effect = DoesNotExist()

        # 動的にトークンを生成
        # JWT(アンケートIDと有効期限（3日）から生成された)で認証
        current_datetime = datetime.now()
        expiration_datetime = current_datetime + timedelta(days=3)

        payload = {
            "survey_id": survey_id,
            "iat": current_datetime.timestamp(),
            "exp": expiration_datetime.timestamp(),
        }
        token = jwt.encode(payload, JwtSettingInfo.SECRET_KEY, algorithm=JwtSettingInfo.ALGORITHM)

        body = {
            "token": token,
        }
        response = client.post(
            f"/api/survey-masters/satisfaction/{survey_master_id}/{revision}", json=body
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"

    @pytest.mark.parametrize(
        "survey_master_id, revision, survey_master_model",
        [
            (
                "9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                1,
                SurveyMasterModel(
                    id="9fb8c7c1-fa03-48ce-b0b1-be8fe638fabf",
                    questions=[
                        QuestionsAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            required=True,
                            description="当月のSSAP支援にはご満足いただけましたか？",
                            format="checkbox",
                            summary_type="satisfaction",
                            choices=[
                                ChoicesSubAttribute(
                                    description="支援者の対応姿勢",
                                    group=GroupSubAttribute(
                                        id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                        title="string",
                                        disabled=False,
                                        is_new=True,
                                    )
                                )
                            ],
                            other_description="その他記載欄（任意）",
                            disabled=False,
                            is_new=True,
                        )
                    ],
                    question_flow=[
                        QuestionFlowAttribute(
                            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            condition_choice_ids=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                        )
                    ],
                    version=1,
                ),
            )
        ],
    )
    def test_project_survey_not_found(
        self, mocker, survey_master_id, revision, survey_master_model
    ):
        survey_id = "7aeebd16-17b6-4d6a-a922-29f514212b87"
        """案件アンケートが存在しない時のテスト"""
        mock_check_auth = mocker.patch.object(SurveyService, "post_check_survey_by_id")
        mock_check_auth.return_value = PostCheckSurveyByIdResponse(
            id="7aeebd16-17b6-4d6a-a922-29f514212b87"
        )
        mock = mocker.patch.object(SurveyMasterModel, "get")
        mock.return_value = survey_master_model
        mock = mocker.patch.object(ProjectSurveyModel, "get")
        mock.side_effect = DoesNotExist()
        # 動的にトークンを生成
        # JWT(アンケートIDと有効期限（3日）から生成された)で認証
        current_datetime = datetime.now()
        expiration_datetime = current_datetime + timedelta(days=3)

        payload = {
            "survey_id": survey_id,
            "iat": current_datetime.timestamp(),
            "exp": expiration_datetime.timestamp(),
        }
        token = jwt.encode(payload, JwtSettingInfo.SECRET_KEY, algorithm=JwtSettingInfo.ALGORITHM)

        body = {
            "token": token,
        }
        response = client.post(
            f"/api/survey-masters/satisfaction/{survey_master_id}/{revision}", json=body
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not found"
