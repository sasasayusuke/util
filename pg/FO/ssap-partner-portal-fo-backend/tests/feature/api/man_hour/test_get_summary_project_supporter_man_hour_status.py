import pytest
from app.main import app
from app.models.man_hour import ManHourProjectSummaryModel, SupportManHourAttribute
from app.service.man_hour_service import ManHourService
from fastapi.testclient import TestClient
from moto import mock_dynamodb

client = TestClient(app)


@mock_dynamodb
class TestGetSummaryProjectSupporterManHourStatus:
    @pytest.mark.parametrize(
        "project_id, summary_month, supporter_user_id, man_hour_model, expected",
        [
            (
                "2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                "202107",
                "3e0894d9-d8c4-4651-b160-d9396221d241",
                ManHourProjectSummaryModel(
                    data_type="project_summary#2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                    year_month="2021/07",
                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                    project_name="案件1",
                    customer_id="0749ee34-f1b4-403d-b722-a8e7917f0418",
                    customer_name="顧客1",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                    support_date_from="2021/05/30",
                    support_date_to="2021/12/31",
                    contract_type="有償",
                    total_contract_time=200,
                    this_month_contract_time=28.7,
                    this_month_direct_support_man_hour=70,
                    this_month_pre_support_man_hour=50,
                    summary_direct_support_man_hour=200,
                    summary_pre_support_man_hour=150,
                    this_month_supporter_direct_support_man_hours=[
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d241",
                            input_man_hour=40.25,
                        ),
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d242",
                            input_man_hour=10,
                        ),
                    ],
                    this_month_supporter_pre_support_man_hours=[
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d241",
                            input_man_hour=20.5,
                        ),
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d242",
                            input_man_hour=10,
                        ),
                    ],
                    summary_supporter_direct_support_man_hours=[
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d241",
                            input_man_hour=100,
                        ),
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d242",
                            input_man_hour=30,
                        ),
                    ],
                    summary_supporter_pre_support_man_hours=[
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d241",
                            input_man_hour=80,
                        ),
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d242",
                            input_man_hour=20,
                        ),
                    ],
                ),
                {
                    "projectId": "2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                    "projectName": "案件1",
                    "customerId": "0749ee34-f1b4-403d-b722-a8e7917f0418",
                    "customerName": "顧客1",
                    "supporterOrganizationId": "de40733f-6be9-4fef-8229-01052f43c1e2",
                    "serviceType": "06f65c34-1570-4c02-a892-b2cf6392451a",
                    "supportDateFrom": "2021/05/30",
                    "supportDateTo": "2021/12/31",
                    "totalContractTime": 200,
                    "thisMonthContractTime": 28.7,
                    "summarySupporterDirectSupportManHour": 100.0,
                    "thisMonthSupporterDirectSupportManHour": 40.25,
                    "summaryDirectSupportManHour": 200.0,
                    "thisMonthDirectSupportManHour": 70.0,
                    "summaryDirectSupportManHourLimit": 0.0,
                    "thisMonthDirectSupportManHourLimit": 0.0,
                    "summarySupporterPreSupportManHour": 80.0,
                    "thisMonthSupporterPreSupportManHour": 20.5,
                    "summaryPreSupportManHour": 150.0,
                    "thisMonthPreSupportManHour": 50.0,
                    "summaryPreSupportManHourLimit": 0.0,
                    "thisMonthPreSupportManHourLimit": 0.0,
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(
        self,
        mocker,
        project_id,
        summary_month,
        supporter_user_id,
        man_hour_model,
        expected,
    ):
        """支援者別工数の更新成功"""
        # ###########################
        # モック化
        # ###########################
        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        man_hour_model.save()

        mock_user = mocker.patch.object(ManHourService, "is_visible_man_hour")
        mock_user.return_value = True

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/project/{project_id}/supporter?summaryMonth={summary_month}&supporterUserId={supporter_user_id}"
        )

        # ###########################
        # 検証
        # ###########################
        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "project_id, summary_month, supporter_user_id",
        [
            (
                "2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                "202107",
                "3e0894d9-d8c4-4651-b160-d9396221d241",
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_not_fount(
        self,
        mocker,
        project_id,
        summary_month,
        supporter_user_id,
    ):
        """支援者別工数の更新成功"""
        # ###########################
        # モック化
        # ###########################
        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        mock_user = mocker.patch.object(ManHourService, "is_visible_man_hour")
        mock_user.return_value = True

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/project/{project_id}/supporter?summaryMonth={summary_month}&supporterUserId={supporter_user_id}"
        )

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 404

    @pytest.mark.parametrize(
        "project_id, summary_month, supporter_user_id, man_hour_model",
        [
            (
                "2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                "202107",
                "3e0894d9-d8c4-4651-b160-d9396221d241",
                ManHourProjectSummaryModel(
                    data_type="project_summary#2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                    year_month="2021/07",
                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9270",
                    project_name="案件1",
                    customer_id="0749ee34-f1b4-403d-b722-a8e7917f0418",
                    customer_name="顧客1",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    service_type="06f65c34-1570-4c02-a892-b2cf6392451a",
                    support_date_from="2021/05/30",
                    support_date_to="2021/12/31",
                    contract_type="有償",
                    total_contract_time=200,
                    this_month_contract_time=28.7,
                    this_month_direct_support_man_hour=70,
                    this_month_pre_support_man_hour=50,
                    summary_direct_support_man_hour=200,
                    summary_pre_support_man_hour=150,
                    this_month_supporter_direct_support_man_hours=[
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d241",
                            input_man_hour=40.25,
                        ),
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d242",
                            input_man_hour=10,
                        ),
                    ],
                    this_month_supporter_pre_support_man_hours=[
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d241",
                            input_man_hour=20.5,
                        ),
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d242",
                            input_man_hour=10,
                        ),
                    ],
                    summary_supporter_direct_support_man_hours=[
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d241",
                            input_man_hour=100,
                        ),
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d242",
                            input_man_hour=30,
                        ),
                    ],
                    summary_supporter_pre_support_man_hours=[
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d241",
                            input_man_hour=80,
                        ),
                        SupportManHourAttribute(
                            supporter_user_id="3e0894d9-d8c4-4651-b160-d9396221d242",
                            input_man_hour=20,
                        ),
                    ],
                ),
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_403(
        self,
        mocker,
        project_id,
        summary_month,
        supporter_user_id,
        man_hour_model,
    ):
        """支援者別工数の更新成功"""
        # ###########################
        # モック化
        # ###########################
        ManHourProjectSummaryModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        man_hour_model.save()

        mock_user = mocker.patch.object(ManHourService, "is_visible_man_hour")
        mock_user.return_value = False

        # ###########################
        # テスト実行
        # ###########################
        response = client.get(
            f"/api/man-hours/summary/project/{project_id}/supporter?summaryMonth={summary_month}&supporterUserId={supporter_user_id}"
        )

        # ###########################
        # 検証
        # ###########################
        assert response.status_code == 403
