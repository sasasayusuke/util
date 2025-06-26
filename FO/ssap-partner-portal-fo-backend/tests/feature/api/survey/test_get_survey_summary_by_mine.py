import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.survey_summary import (
    CompletionContinuationSubAttribute,
    SurveySummaryUserModel,
)

client = TestClient(app)


class TestGetSurveySummaryByMine:
    @pytest.mark.parametrize(
        "year_month_from, year_month_to, survey_summary_models, expected",
        [
            (
                "202204",
                "202303",
                [
                    SurveySummaryUserModel(
                        summary_month="2022/04",
                        data_type="user#906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        name="山田太郎",
                        service_satisfaction_summary=3,
                        service_satisfaction_average=0.6,
                        service_satisfaction_unanswered=1,
                        service_receive=6,
                        completion_satisfaction_summary=4,
                        completion_satisfaction_average=0.5,
                        completion_satisfaction_unanswered=2,
                        completion_continuation=CompletionContinuationSubAttribute(
                            positive_count=8, negative_count=2
                        ),
                        completion_continuation_unanswered=1,
                        completion_recommended_summary=3,
                        completion_recommended_average=0.3,
                        completion_recommended_unanswered=1,
                        completion_sales_summary=5,
                        completion_sales_average=0.6,
                        completion_sales_unanswered=1,
                        completion_receive=10,
                        quick_receive=4,
                    ),
                    SurveySummaryUserModel(
                        summary_month="2022/05",
                        data_type="user#906a3144-9650-4a34-8a23-3b02f3b9aeac",
                        name="山田太郎",
                        service_satisfaction_summary=6,
                        service_satisfaction_average=0.7,
                        service_satisfaction_unanswered=1,
                        service_receive=10,
                        completion_satisfaction_summary=5,
                        completion_satisfaction_average=0.6,
                        completion_satisfaction_unanswered=2,
                        completion_continuation=CompletionContinuationSubAttribute(
                            positive_count=3, negative_count=7
                        ),
                        completion_continuation_unanswered=1,
                        completion_recommended_summary=4,
                        completion_recommended_average=0.4,
                        completion_recommended_unanswered=1,
                        completion_sales_summary=2,
                        completion_sales_average=0.2,
                        completion_sales_unanswered=1,
                        completion_receive=10,
                        quick_receive=8,
                    ),
                ],
                {
                    "termSummaryResult": {
                        "service": {"satisfactionAverage": 0.6, "totalReceive": 16},
                        "completion": {
                            "satisfactionAverage": 0.6,
                            "continuationPositivePercent": 61,
                            "recommendedAverage": 0.4,
                            "salesAverage": 0.4,
                            "totalReceive": 20,
                        },
                        "serviceAndCompletion": {
                            "satisfactionAverage": 0.6,
                            "totalReceive": 36,
                        },
                        "quick": {"totalReceive": 12},
                    },
                    "surveys": [
                        {
                            "summaryMonth": "2022/04",
                            "serviceSatisfactionSummary": 3,
                            "serviceSatisfactionAverage": 0.6,
                            "serviceReceive": 6,
                            "completionSatisfactionSummary": 4,
                            "completionSatisfactionAverage": 0.5,
                            "completionContinuation": {
                                "positiveCount": 8,
                                "negativeCount": 2,
                                "positivePercent": 89,
                            },
                            "completionSalesSummary": 5,
                            "completionSalesAverage": 0.6,
                            "completionRecommendedSummary": 3,
                            "completionRecommendedAverage": 0.3,
                            "completionReceive": 10,
                            "quickReceive": 4,
                        },
                        {
                            "summaryMonth": "2022/05",
                            "serviceSatisfactionSummary": 6,
                            "serviceSatisfactionAverage": 0.7,
                            "serviceReceive": 10,
                            "completionSatisfactionSummary": 5,
                            "completionSatisfactionAverage": 0.6,
                            "completionContinuation": {
                                "positiveCount": 3,
                                "negativeCount": 7,
                                "positivePercent": 33,
                            },
                            "completionSalesSummary": 2,
                            "completionSalesAverage": 0.2,
                            "completionRecommendedSummary": 4,
                            "completionRecommendedAverage": 0.4,
                            "completionReceive": 10,
                            "quickReceive": 8,
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(
        self, mocker, year_month_from, year_month_to, survey_summary_models, expected
    ):
        survey_summary_mock = mocker.patch.object(
            SurveySummaryUserModel.data_type_summary_month_index, "query"
        )
        survey_summary_mock.return_value = survey_summary_models
        response = client.get(
            f"/api/surveys/summary/me?yearMonthFrom={year_month_from}&yearMonthTo={year_month_to}"
        )

        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_supporter_user")
    def test_period_order(self):
        response = client.get(
            "/api/surveys/summary/me?yearMonthFrom=202206&yearMonthTo=202204"
        )
        assert response.status_code == 400
