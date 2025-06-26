import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb

from app.main import app
from app.models.survey_master import SurveyMasterModel
from app.resources.const import UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestUpdateSurveyMasterDraftById:
    @pytest.mark.parametrize(
        "create_body, update_body, expected",
        [
            (
                {
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
                                        }
                                    ],
                                }
                            ],
                            "otherDescription": "その他記載欄（任意）",
                            "disabled": False,
                        }
                    ],
                    "questionFlow": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition_choice_ids": [
                                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                            ],
                        }
                    ],
                    "memo": "これはメモです",
                },
                {
                    "name": "修了アンケート2",
                    "timing": "monthly_not_completion_month",
                    "initSendDaySetting": 25,
                    "initAnswerLimitDaySetting": 6,
                    "isDisclosure": True,
                    "questions": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "required": True,
                            "description": "当月のSSAP支援にはご満足いただけましたでしょうか？",
                            "format": "checkbox",
                            "summaryType": "point",
                            "choices": [
                                {
                                    "description": "支援者の対応姿勢2",
                                    "group": [
                                        {
                                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            "title": "見出し1",
                                            "disabled": False,
                                        }
                                    ],
                                }
                            ],
                            "otherDescription": "その他記載欄（任意）",
                            "disabled": False,
                        }
                    ],
                    "questionFlow": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition_choice_ids": [
                                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                            ],
                        }
                    ],
                    "memo": "これはメモです2",
                },
                {
                    "revision": 0,
                    "name": "修了アンケート2",
                    "type": "quick",
                    "timing": "monthly_not_completion_month",
                    "initSendDaySetting": 25,
                    "initAnswerLimitDaySetting": 6,
                    "isDisclosure": True,
                    "questions": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "required": True,
                            "description": "当月のSSAP支援にはご満足いただけましたでしょうか？",
                            "format": "checkbox",
                            "summaryType": "point",
                            "choices": [
                                {
                                    "description": "支援者の対応姿勢2",
                                    "group": [
                                        {
                                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            "title": "見出し1",
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
                    "isLatest": 1,
                    "memo": "これはメモです2",
                    "version": 2,
                },
            )
        ],
    )
    def test_ok(self, mock_auth_admin, create_body, update_body, expected):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        SurveyMasterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        # バージョン1を作成
        create_response = client.post(
            "/api/survey-masters", json=create_body, headers=REQUEST_HEADERS
        ).json()
        id = create_response["id"]

        # 下書きを作成
        client.put(
            f"/api/survey-masters/{id}?originRevision=1", headers=REQUEST_HEADERS
        )

        response = client.put(
            f"/api/survey-masters/{id}/draft?version=1",
            json=update_body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        actual.pop("id")
        actual.pop("updateId")
        actual.pop("updateAt")
        assert response.status_code == 200
        assert actual == expected

        SurveyMasterModel.delete_table()

    @pytest.mark.parametrize(
        "create_body, update_body, expected",
        [
            (
                {
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
                                        }
                                    ],
                                }
                            ],
                            "otherDescription": "その他記載欄（任意）",
                            "disabled": False,
                        }
                    ],
                    "questionFlow": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition_choice_ids": [
                                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                            ],
                        }
                    ],
                    "memo": "これはメモです",
                },
                {
                    "name": "修了アンケート2",
                    "timing": "monthly_not_completion_month",
                    "initSendDaySetting": 25,
                    "initAnswerLimitDaySetting": 6,
                    "isDisclosure": True,
                    "questions": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "required": True,
                            "description": "当月のSSAP支援にはご満足いただけましたでしょうか？",
                            "format": "checkbox",
                            "summaryType": "point",
                            "choices": [
                                {
                                    "description": "支援者の対応姿勢2",
                                    "group": [
                                        {
                                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            "title": "見出し1",
                                            "disabled": False,
                                        }
                                    ],
                                }
                            ],
                            "otherDescription": "その他記載欄（任意）",
                            "disabled": False,
                        }
                    ],
                    "questionFlow": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "condition_choice_ids": [
                                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                            ],
                        }
                    ],
                    "memo": "これはメモです2",
                },
                {
                    "revision": 0,
                    "name": "修了アンケート2",
                    "type": "quick",
                    "timing": "monthly_not_completion_month",
                    "initSendDaySetting": 25,
                    "initAnswerLimitDaySetting": 6,
                    "isDisclosure": True,
                    "questions": [
                        {
                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "required": True,
                            "description": "当月のSSAP支援にはご満足いただけましたでしょうか？",
                            "format": "checkbox",
                            "summaryType": "point",
                            "choices": [
                                {
                                    "description": "支援者の対応姿勢2",
                                    "group": [
                                        {
                                            "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                            "title": "見出し1",
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
                            "prevId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270b",
                            "condition": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                            "defaultNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "matchNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "isFirst": True,
                            "isEnd": True,
                        }
                    ],
                    "isLatest": 0,
                    "memo": "これはメモです2",
                    "version": 2,
                },
            )
        ],
    )
    def test_conflict(self, mock_auth_admin, create_body, update_body, expected):
        """楽観ロック制御のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        SurveyMasterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        # バージョン1を作成
        create_response = client.post(
            "/api/survey-masters", json=create_body, headers=REQUEST_HEADERS
        ).json()
        id = create_response["id"]

        # 下書きを作成
        client.put(
            f"/api/survey-masters/{id}?originRevision=1", headers=REQUEST_HEADERS
        )

        response = client.put(
            f"/api/survey-masters/{id}/draft?version=2",
            json=update_body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 409

        SurveyMasterModel.delete_table()

    def test_draft_not_exist(self, mock_auth_admin):
        """下書きが存在しない場合のテスト"""
        body = {
            "revision": 0,
            "name": "修了アンケート2",
            "type": "quick",
            "timing": "monthly_not_completion_month",
            "initSendDaySetting": 25,
            "initAnswerLimitDaySetting": 6,
            "isDisclosure": True,
            "questions": [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "required": True,
                    "description": "当月のSSAP支援にはご満足いただけましたでしょうか？",
                    "format": "checkbox",
                    "summaryType": "point",
                    "choices": [
                        {
                            "description": "支援者の対応姿勢2",
                            "group": [
                                {
                                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                                    "title": "見出し1",
                                    "disabled": False,
                                }
                            ],
                        }
                    ],
                    "otherDescription": "その他記載欄（任意）",
                    "disabled": False,
                }
            ],
            "questionFlow": [
                {
                    "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    "condition_choice_ids": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                }
            ],
            "isLatest": 0,
            "memo": "これはメモです2",
            "version": "2",
        }
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        SurveyMasterModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        id = "0f8c995e-db5e-450e-b073-9df81c1ede0f"

        response = client.put(
            f"/api/survey-masters/{id}/draft?version=1",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 404

        SurveyMasterModel.delete_table()
