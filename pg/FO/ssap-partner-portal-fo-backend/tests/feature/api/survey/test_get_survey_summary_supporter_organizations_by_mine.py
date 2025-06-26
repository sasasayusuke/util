import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.master import MasterSupporterOrganizationModel
from app.models.survey_summary import (
    CompletionContinuationSubAttribute,
    SurveySummarySupporterOrganizationModel,
)

client = TestClient(app)


class TestGetSurveySummarySupporterOrganizationsByMine:
    @pytest.mark.parametrize(
        "master_supporter_organization_model, survey_summary_model, expected",
        [
            (
                [
                    MasterSupporterOrganizationModel(
                        id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                        data_type="master_supporter_organization",
                        name="All Service Team",
                        value="AST",
                        order=1,
                        use=True,
                        version=1,
                    ),
                ],
                [
                    SurveySummarySupporterOrganizationModel(
                        summary_month="2022/06",
                        data_type="supporter_organization#556a3144-9650-4a34-8a23-3b02f3b9aeac",
                        supporter_organization_name="AST",
                        service_satisfaction_summary=4,
                        service_satisfaction_unanswered=1,
                        service_receive=2,
                        completion_satisfaction_summary=9,
                        completion_satisfaction_average=9.0,
                        completion_satisfaction_unanswered=1,
                        completion_continuation=CompletionContinuationSubAttribute(
                            positive_count=0,
                            negative_count=2,
                        ),
                        completion_continuation_percent=0,
                        completion_continuation_unanswered=1,
                        completion_recommended_summary=10,
                        completion_recommended_average=10,
                        completion_recommended_unanswered=1,
                        completion_sales_summary=7,
                        completion_sales_average=3.5,
                        completion_sales_unanswered=0,
                        completion_receive=2,
                        total_satisfaction_summary=13,
                        total_satisfaction_average=4.3,
                        total_receive=4,
                    ),
                    SurveySummarySupporterOrganizationModel(
                        summary_month="2022/07",
                        data_type="supporter_organization#556a3144-9650-4a34-8a23-3b02f3b9aeac",
                        supporter_organization_name="AST",
                        service_satisfaction_summary=4,
                        service_satisfaction_unanswered=0,
                        service_receive=2,
                        completion_satisfaction_summary=9,
                        completion_satisfaction_average=4.5,
                        completion_satisfaction_unanswered=0,
                        completion_continuation=CompletionContinuationSubAttribute(
                            positive_count=1,
                            negative_count=1,
                        ),
                        completion_continuation_percent=0.5,
                        completion_continuation_unanswered=0,
                        completion_recommended_summary=10,
                        completion_recommended_average=10,
                        completion_recommended_unanswered=1,
                        completion_sales_summary=7,
                        completion_sales_average=3.5,
                        completion_sales_unanswered=0,
                        completion_receive=2,
                        total_satisfaction_summary=13,
                        total_satisfaction_average=4.3,
                        total_receive=4,
                    ),
                    SurveySummarySupporterOrganizationModel(
                        summary_month="2022/08",
                        data_type="supporter_organization#556a3144-9650-4a34-8a23-3b02f3b9aeac",
                        supporter_organization_name="AST",
                        service_satisfaction_summary=4,
                        service_satisfaction_unanswered=0,
                        service_receive=2,
                        completion_satisfaction_summary=9,
                        completion_satisfaction_average=4.5,
                        completion_satisfaction_unanswered=0,
                        completion_continuation=CompletionContinuationSubAttribute(
                            positive_count=1,
                            negative_count=1,
                        ),
                        completion_continuation_percent=0.5,
                        completion_continuation_unanswered=0,
                        completion_recommended_summary=10,
                        completion_recommended_average=10,
                        completion_recommended_unanswered=1,
                        completion_sales_summary=7,
                        completion_sales_average=3.5,
                        completion_sales_unanswered=0,
                        completion_receive=2,
                        total_satisfaction_summary=13,
                        total_satisfaction_average=4.3,
                        total_receive=4,
                    ),
                ],
                {
                    "totalSummaryResult": {
                        "serviceSatisfactionAverage": 2.4,
                        "serviceReceive": 6,
                        "completionSatisfactionAverage": 5.4,
                        "completionContinuationPercent": 40,
                        "completionRecommendedAverage": 10.0,
                        "completionReceive": 6,
                        "totalSatisfactionAverage": 3.9,
                        "totalReceive": 12,
                    },
                    "termSummaryResult": [
                        {
                            "supporterOrganizationId": "556a3144-9650-4a34-8a23-3b02f3b9aeac",
                            "supporterOrganizationName": "AST",
                            "serviceSatisfactionSummary": 12,
                            "serviceSatisfactionAverage": 2.4,
                            "serviceSatisfactionUnanswered": 1,
                            "serviceReceive": 6,
                            "completionSatisfactionSummary": 27,
                            "completionSatisfactionAverage": 5.4,
                            "completionSatisfactionUnanswered": 1,
                            "completionContinuation": {
                                "positiveCount": 2,
                                "negativeCount": 4,
                                "positivePercent": 40,
                            },
                            "completionContinuationUnanswered": 1,
                            "completionRecommendedSummary": 30,
                            "completionRecommendedAverage": 10.0,
                            "completionRecommendedUnanswered": 3,
                            "completionReceive": 6,
                            "totalSatisfactionSummary": 39,
                            "totalSatisfactionAverage": 3.9,
                            "totalSatisfactionUnanswered": 2,
                            "totalReceive": 12,
                        }
                    ],
                    "surveys": [
                        {
                            "summaryMonth": "2022/06",
                            "supporterOrganizations": [
                                {
                                    "supporterOrganizationName": "AST",
                                    "serviceSatisfactionAverage": 0.0,
                                    "serviceReceive": 2,
                                    "completionSatisfactionAverage": 9.0,
                                    "completionContinuationPercent": 0,
                                    "completionRecommendedAverage": 10.0,
                                    "completionReceive": 2,
                                    "totalSatisfactionAverage": 4.3,
                                    "totalReceive": 4,
                                }
                            ],
                        },
                        {
                            "summaryMonth": "2022/07",
                            "supporterOrganizations": [
                                {
                                    "supporterOrganizationName": "AST",
                                    "serviceSatisfactionAverage": 0.0,
                                    "serviceReceive": 2,
                                    "completionSatisfactionAverage": 4.5,
                                    "completionContinuationPercent": 50,
                                    "completionRecommendedAverage": 10.0,
                                    "completionReceive": 2,
                                    "totalSatisfactionAverage": 4.3,
                                    "totalReceive": 4,
                                }
                            ],
                        },
                        {
                            "summaryMonth": "2022/08",
                            "supporterOrganizations": [
                                {
                                    "supporterOrganizationName": "AST",
                                    "serviceSatisfactionAverage": 0.0,
                                    "serviceReceive": 2,
                                    "completionSatisfactionAverage": 4.5,
                                    "completionContinuationPercent": 50,
                                    "completionRecommendedAverage": 10.0,
                                    "completionReceive": 2,
                                    "totalSatisfactionAverage": 4.3,
                                    "totalReceive": 4,
                                }
                            ],
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_supporter_ok(self, mocker, master_supporter_organization_model, survey_summary_model, expected):
        survey_summary_mock = mocker.patch.object(
            SurveySummarySupporterOrganizationModel, "query"
        )
        survey_summary_mock.return_value = survey_summary_model

        master_supporter_organization_mock = mocker.patch.object(
            MasterSupporterOrganizationModel, "query"
        )
        master_supporter_organization_mock.return_value = (
            master_supporter_organization_model
        )

        response = client.get(
            "/api/surveys/summary/supporter-organizations/me?yearMonthFrom=202203&yearMonthTo=202212"
        )
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "master_supporter_organization_model, survey_summary_model, expected",
        [
            (
                [
                    MasterSupporterOrganizationModel(
                        id="180a3597-b7e7-42c8-902c-a29016afa662",
                        data_type="master_supporter_organization",
                        name="Ideation Service Team",
                        value="IST",
                        order=1,
                        use=True,
                        version=1,
                    ),
                ],
                [
                    SurveySummarySupporterOrganizationModel(
                        summary_month="2022/06",
                        data_type="supporter_organization#180a3597-b7e7-42c8-902c-a29016afa662",
                        supporter_organization_name="IST",
                        service_satisfaction_summary=4,
                        service_receive=2,
                        completion_satisfaction_summary=9,
                        completion_satisfaction_average=4.5,
                        completion_satisfaction_unanswered=0,
                        completion_continuation=CompletionContinuationSubAttribute(
                            positive_count=1,
                            negative_count=1,
                        ),
                        completion_continuation_percent=0.5,
                        completion_continuation_unanswered=0,
                        completion_recommended_summary=10,
                        completion_recommended_average=10,
                        completion_recommended_unanswered=1,
                        completion_sales_summary=7,
                        completion_sales_average=3.5,
                        completion_sales_unanswered=0,
                        completion_receive=2,
                        total_satisfaction_summary=13,
                        total_satisfaction_average=4.3,
                        total_receive=4,
                    ),
                    SurveySummarySupporterOrganizationModel(
                        summary_month="2022/07",
                        data_type="supporter_organization#180a3597-b7e7-42c8-902c-a29016afa662",
                        supporter_organization_name="IST",
                        service_satisfaction_summary=4,
                        service_receive=2,
                        completion_satisfaction_summary=9,
                        completion_satisfaction_average=4.5,
                        completion_satisfaction_unanswered=0,
                        completion_continuation=CompletionContinuationSubAttribute(
                            positive_count=1,
                            negative_count=1,
                        ),
                        completion_continuation_percent=0.5,
                        completion_continuation_unanswered=0,
                        completion_recommended_summary=10,
                        completion_recommended_average=10,
                        completion_recommended_unanswered=1,
                        completion_sales_summary=7,
                        completion_sales_average=3.5,
                        completion_sales_unanswered=0,
                        completion_receive=2,
                        total_satisfaction_summary=13,
                        total_satisfaction_average=4.3,
                        total_receive=4,
                    ),
                    SurveySummarySupporterOrganizationModel(
                        summary_month="2022/08",
                        data_type="supporter_organization#180a3597-b7e7-42c8-902c-a29016afa662",
                        supporter_organization_name="IST",
                        service_satisfaction_summary=4,
                        service_receive=2,
                        completion_satisfaction_summary=9,
                        completion_satisfaction_average=4.5,
                        completion_satisfaction_unanswered=0,
                        completion_continuation=CompletionContinuationSubAttribute(
                            positive_count=1,
                            negative_count=1,
                        ),
                        completion_continuation_percent=0.5,
                        completion_continuation_unanswered=0,
                        completion_recommended_summary=10,
                        completion_recommended_average=10,
                        completion_recommended_unanswered=1,
                        completion_sales_summary=7,
                        completion_sales_average=3.5,
                        completion_sales_unanswered=0,
                        completion_receive=2,
                        total_satisfaction_summary=13,
                        total_satisfaction_average=4.3,
                        total_receive=4,
                    ),
                ],
                {
                    "totalSummaryResult": {
                        "serviceSatisfactionAverage": 2.0,
                        "serviceReceive": 6,
                        "completionSatisfactionAverage": 4.5,
                        "completionContinuationPercent": 50,
                        "completionRecommendedAverage": 10.0,
                        "completionReceive": 6,
                        "totalSatisfactionAverage": 3.3,
                        "totalReceive": 12,
                    },
                    "termSummaryResult": [
                        {
                            "supporterOrganizationId": "180a3597-b7e7-42c8-902c-a29016afa662",
                            "supporterOrganizationName": "IST",
                            "serviceSatisfactionSummary": 12,
                            "serviceSatisfactionAverage": 2.0,
                            "serviceSatisfactionUnanswered": 0,
                            "serviceReceive": 6,
                            "completionSatisfactionSummary": 27,
                            "completionSatisfactionAverage": 4.5,
                            "completionSatisfactionUnanswered": 0,
                            "completionContinuation": {
                                "positiveCount": 3,
                                "negativeCount": 3,
                                "positivePercent": 50,
                            },
                            "completionContinuationUnanswered": 0,
                            "completionRecommendedSummary": 30,
                            "completionRecommendedAverage": 10.0,
                            "completionRecommendedUnanswered": 3,
                            "completionReceive": 6,
                            "totalSatisfactionSummary": 39,
                            "totalSatisfactionAverage": 3.3,
                            "totalSatisfactionUnanswered": 0,
                            "totalReceive": 12,
                        }
                    ],
                    "surveys": [
                        {
                            "summaryMonth": "2022/06",
                            "supporterOrganizations": [
                                {
                                    "supporterOrganizationName": "IST",
                                    "serviceSatisfactionAverage": 0.0,
                                    "serviceReceive": 2,
                                    "completionSatisfactionAverage": 4.5,
                                    "completionContinuationPercent": 50,
                                    "completionRecommendedAverage": 10.0,
                                    "completionReceive": 2,
                                    "totalSatisfactionAverage": 4.3,
                                    "totalReceive": 4,
                                }
                            ],
                        },
                        {
                            "summaryMonth": "2022/07",
                            "supporterOrganizations": [
                                {
                                    "supporterOrganizationName": "IST",
                                    "serviceSatisfactionAverage": 0.0,
                                    "serviceReceive": 2,
                                    "completionSatisfactionAverage": 4.5,
                                    "completionContinuationPercent": 50,
                                    "completionRecommendedAverage": 10.0,
                                    "completionReceive": 2,
                                    "totalSatisfactionAverage": 4.3,
                                    "totalReceive": 4,
                                }
                            ],
                        },
                        {
                            "summaryMonth": "2022/08",
                            "supporterOrganizations": [
                                {
                                    "supporterOrganizationName": "IST",
                                    "serviceSatisfactionAverage": 0.0,
                                    "serviceReceive": 2,
                                    "completionSatisfactionAverage": 4.5,
                                    "completionContinuationPercent": 50,
                                    "completionRecommendedAverage": 10.0,
                                    "completionReceive": 2,
                                    "totalSatisfactionAverage": 4.3,
                                    "totalReceive": 4,
                                }
                            ],
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_sales_user")
    def test_sales_ok(
        self,
        mocker,
        master_supporter_organization_model,
        survey_summary_model,
        expected,
    ):
        survey_summary_mock = mocker.patch.object(
            SurveySummarySupporterOrganizationModel, "query"
        )
        survey_summary_mock.return_value = survey_summary_model

        master_supporter_organization_mock = mocker.patch.object(
            MasterSupporterOrganizationModel, "query"
        )
        master_supporter_organization_mock.return_value = (
            master_supporter_organization_model
        )

        response = client.get(
            "/api/surveys/summary/supporter-organizations/me?yearMonthFrom=202203&yearMonthTo=202212"
        )
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
