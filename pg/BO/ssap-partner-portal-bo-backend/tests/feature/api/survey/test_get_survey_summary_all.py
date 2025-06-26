import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.survey_summary import (
    CompletionAttribute,
    ContinuationSubAttribute,
    KarteAttribute,
    PpAttribute,
    QuickAttribute,
    ServiceAttribute,
    SurveySummaryAllModel,
)
from app.resources.const import SurveySummaryType, UserRoleType

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


class TestGetSurveySummaryAll:
    @pytest.mark.parametrize(
        "survey_summary_all_models, expected",
        [
            (
                [
                    SurveySummaryAllModel(
                        summary_month="2022/04",
                        data_type=SurveySummaryType.ALL,
                        service=ServiceAttribute(
                            project_count=3,
                            receive_count=3,
                            receive_percent=1,
                            satisfaction_summary=9,
                            satisfaction_average=4.5,
                            satisfaction_unanswered=1,
                        ),
                        completion=CompletionAttribute(
                            project_count=3,
                            receive_count=3,
                            receive_percent=1,
                            satisfaction_summary=12,
                            satisfaction_average=4,
                            satisfaction_unanswered=0,
                            continuation=ContinuationSubAttribute(
                                positive_count=2,
                                negative_count=1,
                                percent=0.667,
                                unanswered=0,
                            ),
                            recommended_summary=13,
                            recommended_average=6.5,
                            recommended_unanswered=1,
                            sales_summary=9,
                            sales_average=3,
                            sales_unanswered=0,
                        ),
                        karte=KarteAttribute(
                            project_count=3, karte_count=12, use_percent=0.25
                        ),
                        quick=QuickAttribute(
                            send_count=2,
                            receive_count=2,
                        ),
                        pp=PpAttribute(
                            survey_satisfaction_summary=4,
                            survey_satisfaction_average=2,
                            man_hour_satisfaction_summary=5,
                            man_hour_satisfaction_average=2.5,
                            karte_satisfaction_summary=9,
                            karte_satisfaction_average=4.5,
                            master_karte_satisfaction_summary=6,
                            master_karte_satisfaction_average=3,
                            send_count=2,
                            receive_count=2,
                            receive_percent=1,
                        ),
                    ),
                    SurveySummaryAllModel(
                        summary_month="2022/05",
                        data_type=SurveySummaryType.ALL,
                        service=ServiceAttribute(
                            project_count=3,
                            receive_count=3,
                            receive_percent=1,
                            satisfaction_summary=9,
                            satisfaction_average=4.5,
                            satisfaction_unanswered=1,
                        ),
                        completion=CompletionAttribute(
                            project_count=3,
                            receive_count=3,
                            receive_percent=1,
                            satisfaction_summary=12,
                            satisfaction_average=4,
                            satisfaction_unanswered=0,
                            continuation=ContinuationSubAttribute(
                                positive_count=2,
                                negative_count=1,
                                percent=0.667,
                                unanswered=0,
                            ),
                            recommended_summary=13,
                            recommended_average=6.5,
                            recommended_unanswered=1,
                            sales_summary=9,
                            sales_average=3,
                            sales_unanswered=0,
                        ),
                        karte=KarteAttribute(
                            project_count=3, karte_count=12, use_percent=0.25
                        ),
                        quick=QuickAttribute(
                            send_count=2,
                            receive_count=2,
                        ),
                        pp=PpAttribute(
                            survey_satisfaction_summary=4,
                            survey_satisfaction_average=2,
                            man_hour_satisfaction_summary=5,
                            man_hour_satisfaction_average=2.5,
                            karte_satisfaction_summary=9,
                            karte_satisfaction_average=4.5,
                            master_karte_satisfaction_summary=6,
                            master_karte_satisfaction_average=3,
                            send_count=2,
                            receive_count=2,
                            receive_percent=1,
                        ),
                    ),
                    SurveySummaryAllModel(
                        summary_month="2022/06",
                        data_type=SurveySummaryType.ALL,
                        service=ServiceAttribute(
                            project_count=3,
                            receive_count=3,
                            receive_percent=1,
                            satisfaction_summary=9,
                            satisfaction_average=4.5,
                            satisfaction_unanswered=1,
                        ),
                        completion=CompletionAttribute(
                            project_count=3,
                            receive_count=3,
                            receive_percent=1,
                            satisfaction_summary=12,
                            satisfaction_average=4,
                            satisfaction_unanswered=0,
                            continuation=ContinuationSubAttribute(
                                positive_count=2,
                                negative_count=1,
                                percent=0.667,
                                unanswered=0,
                            ),
                            recommended_summary=13,
                            recommended_average=6.5,
                            recommended_unanswered=1,
                            sales_summary=9,
                            sales_average=3,
                            sales_unanswered=0,
                        ),
                        karte=KarteAttribute(
                            project_count=3, karte_count=12, use_percent=0.25
                        ),
                        quick=QuickAttribute(
                            send_count=2,
                            receive_count=2,
                        ),
                        pp=PpAttribute(
                            survey_satisfaction_summary=4,
                            survey_satisfaction_average=2,
                            man_hour_satisfaction_summary=5,
                            man_hour_satisfaction_average=2.5,
                            karte_satisfaction_summary=9,
                            karte_satisfaction_average=4.5,
                            master_karte_satisfaction_summary=6,
                            master_karte_satisfaction_average=3,
                            send_count=2,
                            receive_count=2,
                            receive_percent=1,
                        ),
                    ),
                ],
                {
                    "termSummaryResult": {
                        "service": {
                            "satisfactionAverage": 4.5,
                            "totalReceive": 9,
                        },
                        "completion": {
                            "satisfactionAverage": 4.0,
                            "continuationPositivePercent": 67,
                            "recommendedAverage": 6.5,
                            "salesAverage": 3.0,
                            "totalReceive": 9,
                        },
                        "serviceAndCompletion": {
                            "satisfactionAverage": 4.2,
                            "totalReceive": 18,
                        },
                        "quick": {"totalReceive": 6},
                        "pp": {
                            "surveySatisfactionAverage": 2.0,
                            "manHourSatisfactionAverage": 2.5,
                            "karteSatisfactionAverage": 4.5,
                            "masterKarteSatisfactionAverage": 3.0,
                            "totalReceive": 6,
                        },
                    },
                    "surveys": [
                        {
                            "summaryMonth": "2022/04",
                            "service": {
                                "projectCount": 3,
                                "receiveCount": 3,
                                "receivePercent": 100,
                                "satisfactionSummary": 9,
                                "satisfactionAverage": 4.5,
                            },
                            "completion": {
                                "projectCount": 3,
                                "receiveCount": 3,
                                "receivePercent": 100,
                                "satisfactionSummary": 12,
                                "satisfactionAverage": 4.0,
                                "continuation": {
                                    "positiveCount": 2,
                                    "negativeCount": 1,
                                    "positivePercent": 66,
                                },
                                "recommendedSummary": 13,
                                "recommendedAverage": 6.5,
                                "salesSummary": 9,
                                "salesAverage": 3.0,
                            },
                            "karte": {
                                "projectCount": 3,
                                "karteCount": 12,
                                "usePercent": 25,
                            },
                            "quick": {
                                "receiveCount": 2,
                                "receivePercent": 0,
                            },
                            "pp": {
                                "surveySatisfactionSummary": 4,
                                "surveySatisfactionAverage": 2.0,
                                "manHourSatisfactionSummary": 5,
                                "manHourSatisfactionAverage": 2.5,
                                "karteSatisfactionSummary": 9,
                                "karteSatisfactionAverage": 4.5,
                                "masterKarteSatisfactionSummary": 6,
                                "masterKarteSatisfactionAverage": 3,
                                "sendCount": 2,
                                "receiveCount": 2,
                                "receivePercent": 100,
                            },
                        },
                        {
                            "summaryMonth": "2022/05",
                            "service": {
                                "projectCount": 3,
                                "receiveCount": 3,
                                "receivePercent": 100,
                                "satisfactionSummary": 9,
                                "satisfactionAverage": 4.5,
                            },
                            "completion": {
                                "projectCount": 3,
                                "receiveCount": 3,
                                "receivePercent": 100,
                                "satisfactionSummary": 12,
                                "satisfactionAverage": 4.0,
                                "continuation": {
                                    "positiveCount": 2,
                                    "negativeCount": 1,
                                    "positivePercent": 66,
                                },
                                "recommendedSummary": 13,
                                "recommendedAverage": 6.5,
                                "salesSummary": 9,
                                "salesAverage": 3.0,
                            },
                            "karte": {
                                "projectCount": 3,
                                "karteCount": 12,
                                "usePercent": 25,
                            },
                            "quick": {
                                "receiveCount": 2,
                                "receivePercent": 0,
                            },
                            "pp": {
                                "surveySatisfactionSummary": 4,
                                "surveySatisfactionAverage": 2.0,
                                "manHourSatisfactionSummary": 5,
                                "manHourSatisfactionAverage": 2.5,
                                "karteSatisfactionSummary": 9,
                                "karteSatisfactionAverage": 4.5,
                                "masterKarteSatisfactionSummary": 6,
                                "masterKarteSatisfactionAverage": 3,
                                "sendCount": 2,
                                "receiveCount": 2,
                                "receivePercent": 100,
                            },
                        },
                        {
                            "summaryMonth": "2022/06",
                            "service": {
                                "projectCount": 3,
                                "receiveCount": 3,
                                "receivePercent": 100,
                                "satisfactionSummary": 9,
                                "satisfactionAverage": 4.5,
                            },
                            "completion": {
                                "projectCount": 3,
                                "receiveCount": 3,
                                "receivePercent": 100,
                                "satisfactionSummary": 12,
                                "satisfactionAverage": 4.0,
                                "continuation": {
                                    "positiveCount": 2,
                                    "negativeCount": 1,
                                    "positivePercent": 66,
                                },
                                "recommendedSummary": 13,
                                "recommendedAverage": 6.5,
                                "salesSummary": 9,
                                "salesAverage": 3.0,
                            },
                            "karte": {
                                "projectCount": 3,
                                "karteCount": 12,
                                "usePercent": 25,
                            },
                            "quick": {
                                "receiveCount": 2,
                                "receivePercent": 0,
                            },
                            "pp": {
                                "surveySatisfactionSummary": 4,
                                "surveySatisfactionAverage": 2.0,
                                "manHourSatisfactionSummary": 5,
                                "manHourSatisfactionAverage": 2.5,
                                "karteSatisfactionSummary": 9,
                                "karteSatisfactionAverage": 4.5,
                                "masterKarteSatisfactionSummary": 6,
                                "masterKarteSatisfactionAverage": 3,
                                "sendCount": 2,
                                "receiveCount": 2,
                                "receivePercent": 100,
                            },
                        },
                    ],
                },
            )
        ],
    )
    def test_ok(self, mocker, mock_auth_admin, survey_summary_all_models, expected):

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        survey_summary_mock = mocker.patch.object(
            SurveySummaryAllModel.data_type_summary_month_index, "query"
        )
        survey_summary_mock.return_value = survey_summary_all_models

        response = client.get(
            "/api/surveys/summary/all?yearMonthFrom=202204&yearMonthTo=202303",
            headers=REQUEST_HEADERS,
        )

        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "query_params",
        [
            "?yearMonthFrom=202212&yearMonthTo=202203",
            "?yearMonthFrom=2022&yearMonthTo=2022",
            "?yearMonthFrom=20220301&yearMonthTo=20221231",
            "?yearMonthFrom=test&yearMonthTo=test",
        ],
    )
    def test_validation(self, mock_auth_admin, query_params):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        response = client.get(
            f"/api/surveys/summary/all{query_params}", headers=REQUEST_HEADERS
        )
        assert response.status_code == 400
