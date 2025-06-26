import pytest
from app.main import app
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.resources.const import DataType
from app.service.schedule_service import SchedulesService
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist
from pynamodb.models import BatchWrite

client = TestClient(app)


class TestCreateSchedules:
    @pytest.mark.parametrize(
        "project_id, body, project_model",
        [
            (
                "2ed6e959-c216-46a8-92c3-3d7d3f951d00",
                {
                    "timing": "monthly",
                    "supportDate": 5,
                    "startTime": "13:00",
                    "endTime": "14:00",
                },
                ProjectModel(
                    id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2023/03/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/05/31",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_ok_monthly(
        self,
        mocker,
        body,
        project_id,
        project_model,
    ):
        # 権限チェック用
        mocker_method = mocker.patch.object(SchedulesService, "is_visible_project")
        mocker_method.return_value = True

        """毎月のスケジュール登録"""
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mocker.patch.object(BatchWrite, "save")

        response = client.post(f"/api/schedules/support/{project_id}", json=body)

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "project_id, body, project_model",
        [
            (
                "2ed6e959-c216-46a8-92c3-3d7d3f951d00",
                {
                    "timing": "weekly",
                    "supportDate": 1,
                    "startTime": "13:00",
                    "endTime": "14:00",
                },
                ProjectModel(
                    id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/04/30",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_ok_weekly(
        self,
        mocker,
        body,
        project_id,
        project_model,
    ):
        """毎週のスケジュール登録"""
        # 権限チェック用
        mocker_method = mocker.patch.object(SchedulesService, "is_visible_project")
        mocker_method.return_value = True

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mocker.patch.object(BatchWrite, "save")

        response = client.post(f"/api/schedules/support/{project_id}", json=body)

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "project_id, body, project_model",
        [
            (
                "2ed6e959-c216-46a8-92c3-3d7d3f951d00",
                {
                    "timing": "once",
                    "supportDate": "2022/08/04",
                    "startTime": "13:00",
                    "endTime": "14:00",
                },
                ProjectModel(
                    id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                    data_type=DataType.PROJECT,
                    salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    name="サンプルプロジェクト",
                    customer_name="ソニーグループ株式会社",
                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    create_new=True,
                    continued=True,
                    main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
                    contract_date="2021/01/30",
                    phase="プラン提示（D）",
                    customer_success="DXの実現",
                    support_date_from="2022/04/01",
                    support_date_to="2022/09/30",
                    profit=GrossProfitAttribute(
                        monthly=[
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                            400000,
                        ],
                        quarterly=[1200000, 1200000, 1200000, 1200000],
                        half=[2400000, 2400000],
                        year=4800000,
                    ),
                    total_contract_time=200,
                    main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                    salesforce_main_customer=SalesforceMainCustomerAttribute(
                        name="山田太郎",
                        email="yamada@example.com",
                        organization_name="IST",
                        job="部長",
                    ),
                    customer_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
                    supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=False,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_ok_once(
        self,
        mocker,
        body,
        project_id,
        project_model,
    ):
        """1回のスケジュール登録"""
        # 権限チェック用
        mocker_method = mocker.patch.object(SchedulesService, "is_visible_project")
        mocker_method.return_value = True
        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mocker.patch.object(BatchWrite, "save")

        response = client.post(f"/api/schedules/support/{project_id}", json=body)

        assert response.status_code == 200

    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_not_found_project(self, mocker):
        """プロジェクトが存在しない場合"""
        # 権限チェック用
        mocker_method = mocker.patch.object(SchedulesService, "is_visible_project")
        mocker_method.return_value = True

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.side_effect = DoesNotExist()

        project_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        body = {
            "timing": "monthly",
            "supportDate": 5,
            "startTime": "13:00",
            "endTime": "14:00",
        }

        response = client.post(f"/api/schedules/support/{project_id}", json=body)

        assert response.status_code == 404

    @pytest.mark.parametrize(
        "body",
        [
            #####################
            # timing
            #####################
            # 必須チェック
            {
                # "timing": "monthly",
                "supportDate": 5,
                "startTime": "13:00",
                "endTime": "14:00",
            },
            # Noneチェック
            {
                "timing": None,
                "supportDate": 5,
                "startTime": "13:00",
                "endTime": "14:00",
            },
            # enum
            {
                "timing": "year",
                "supportDate": 5,
                "startTime": "13:00",
                "endTime": "14:00",
            },
            #####################
            # support_date
            #####################
            # 必須チェック
            {
                "timing": "once",
                # "supportDate": '2022/04/01',
                "startTime": "13:00",
                "endTime": "14:00",
            },
            # Noneチェック
            {
                "timing": "once",
                "supportDate": None,
                "startTime": "13:00",
                "endTime": "14:00",
            },
            # 組み合わせテスト
            {
                "timing": "monthly",
                "supportDate": 0,
                "startTime": "13:00",
                "endTime": "14:00",
            },
            {
                "timing": "monthly",
                "supportDate": 32,
                "startTime": "13:00",
                "endTime": "14:00",
            },
            {
                "timing": "monthly",
                "supportDate": "2022/04/01",
                "startTime": "13:00",
                "endTime": "14:00",
            },
            {
                "timing": "weekly",
                "supportDate": 7,
                "startTime": "13:00",
                "endTime": "14:00",
            },
            {
                "timing": "weekly",
                "supportDate": "2022/04/06",
                "startTime": "13:00",
                "endTime": "14:00",
            },
            {
                "timing": "once",
                "supportDate": 5,
                "startTime": "13:00",
                "endTime": "14:00",
            },
            {
                "timing": "once",
                "supportDate": 0,
                "startTime": "13:00",
                "endTime": "14:00",
            },
            #####################
            # start_time
            #####################
            # 必須チェック
            {
                "timing": "once",
                "supportDate": "2022/04/01",
                # "startTime": "13:00",
                "endTime": "14:00",
            },
            # Noneチェック
            {
                "timing": "once",
                "supportDate": "2022/04/01",
                "startTime": None,
                "endTime": "14:00",
            },
            #####################
            # end_time
            #####################
            # 必須チェック
            {
                "timing": "once",
                "supportDate": "2022/04/01",
                "startTime": "13:00",
                # "endTime": "14:00",
            },
            # Noneチェック
            {
                "timing": "once",
                "supportDate": "2022/04/01",
                "startTime": "13:00",
                "endTime": None,
            },
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_mgr_user")
    def test_validation(self, body):

        project_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"

        response = client.post(f"/api/schedules/support/{project_id}", json=body)
        assert response.status_code == 422
