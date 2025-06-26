import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.survey_master import SurveyMasterModel
from app.resources.const import UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestSurveyMasters:
    @pytest.mark.parametrize(
        "body, expected",
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
                    "memo": "string",
                },
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
                    "memo": "string",
                },
            )
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, body, expected):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mocker.patch.object(SurveyMasterModel, "save")

        response = client.post(
            "/api/survey-masters", json=body, headers=REQUEST_HEADERS
        )
        actual = response.json()
        actual.pop("id")
        actual.pop("createId")
        actual.pop("createAt")
        actual.pop("version")
        actual.pop("isLatest")
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "body",
        [
            # ###########################
            # name
            # ###########################
            # 必須チェック
            {
                # "name": "修了アンケート",
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
                "memo": "string",
            },
            # Noneチェック
            {
                "name": None,
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
                "memo": "string",
            },
            # ###########################
            # type
            # ###########################
            # 必須チェック
            {
                "name": "修了アンケート",
                # "type": "quick",
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
                "memo": "string",
            },
            # Noneチェック
            {
                "name": "修了アンケート",
                "type": None,
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
                "memo": "string",
            },
            # ###########################
            # timing
            # ###########################
            # 必須チェック
            {
                "name": "修了アンケート",
                "type": "quick",
                # "timing": "monthly",
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
                "memo": "string",
            },
            # Noneチェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": None,
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
                "memo": "string",
            },
            # ###########################
            # init_send_day_setting
            # ###########################
            # 必須チェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                # "initSendDaySetting": 20,
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
                "memo": "string",
            },
            # Noneチェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                "initSendDaySetting": None,
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
                "memo": "string",
            },
            # ###########################
            # init_answer_limit_day_setting
            # ###########################
            # 必須チェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                "initSendDaySetting": 20,
                # "initAnswerLimitDaySetting": 5,
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
                "memo": "string",
            },
            # Noneチェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                "initSendDaySetting": 20,
                "initAnswerLimitDaySetting": None,
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
                "memo": "string",
            },
            # ###########################
            # is_disclosure
            # ###########################
            # 必須チェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                "initSendDaySetting": 20,
                "initAnswerLimitDaySetting": 5,
                # "isDisclosure": True,
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
                "memo": "string",
            },
            # Noneチェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                "initSendDaySetting": 20,
                "initAnswerLimitDaySetting": 5,
                "isDisclosure": None,
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
                "memo": "string",
            },
            # ###########################
            # questions
            # ###########################
            # 必須チェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                "initSendDaySetting": 20,
                "initAnswerLimitDaySetting": 5,
                "isDisclosure": True,
                "questionFlow": [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "prevId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "condition": ["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                        "defaultNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "matchNextId": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "isFirst": True,
                        "isEnd": True,
                    }
                ],
                "memo": "string",
            },
            # Noneチェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                "initSendDaySetting": 20,
                "initAnswerLimitDaySetting": 5,
                "isDisclosure": True,
                "questions": None,
                "questionFlow": [
                    {
                        "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "condition_choice_ids": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                    }
                ],
                "memo": "string",
            },
            # ###########################
            # questions.id
            # ###########################
            # 必須チェック
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                "initSendDaySetting": 20,
                "initAnswerLimitDaySetting": 5,
                "isDisclosure": True,
                "questions": [
                    {
                        # "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                "memo": "string",
            },
            {
                "name": "修了アンケート",
                "type": "quick",
                "timing": "monthly",
                "initSendDaySetting": 20,
                "initAnswerLimitDaySetting": 5,
                "isDisclosure": True,
                "questions": [
                    {
                        "id": None,
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
                "memo": "string",
            },
            # ###########################
            # questions.required
            # ###########################
            # 必須チェック
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
                        # "required": True,
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
                "memo": "string",
            },
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
                        "required": None,
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
                "memo": "string",
            },
            # ###########################
            # questions.description
            # ###########################
            # 必須チェック
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
                        # "description": "当月のSSAP支援にはご満足いただけましたか？",
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
                "memo": "string",
            },
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
                        "description": None,
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
                "memo": "string",
            },
            # ###########################
            # questions.format
            # ###########################
            # 必須チェック
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
                        # "format": "checkbox",
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
                "memo": "string",
            },
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
                        "format": None,
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
                "memo": "string",
            },
            # ###########################
            # question_flow
            # ###########################
            # 必須チェック
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
                "memo": "string",
            },
            # Noneチェック
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
                "questionFlow": None,
                "memo": "string",
            },
            # ###########################
            # questions_flow.id
            # ###########################
            # 必須チェック
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
                        # "id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "condition_choice_ids": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                    }
                ],
                "memo": "string",
            },
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
                        "id": None,
                        "condition_id": "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        "condition_choice_ids": [
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                    }
                ],
                "memo": "string",
            },
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        response = client.post(
            "/api/survey-masters", json=body, headers=REQUEST_HEADERS
        )
        assert response.status_code == 422
