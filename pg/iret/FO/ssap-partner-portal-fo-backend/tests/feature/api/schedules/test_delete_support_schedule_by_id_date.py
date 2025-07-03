import pytest
from pynamodb.exceptions import DoesNotExist
from app.main import app
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_karte import ProjectKarteModel
from app.resources.const import DataType
from app.service.schedule_service import SchedulesService
from fastapi.testclient import TestClient

client = TestClient(app)


class TestDeleteSupportScheduleByIdDate:
    @pytest.mark.parametrize(
        "version, karte_id, project_model, expected, project_karte_model",
        [
            (
                1,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[],
                        quarterly=[],
                        half=[],
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
                {"message": "OK"},
                ProjectKarteModel(
                    karte_id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                    project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                    customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    start_datetime="2022/01/20 10:00",
                    start_time=600,
                    end_time=780,
                    supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    last_update_datetime="2020-10-23T03:21:39.356000+0000",
                    customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    man_hour=0,
                    detail=None,
                    feedback=None,
                    homework=None,
                    documents=None,
                    deliverables=None,
                    memo=None,
                    task=None,
                    is_draft=False,
                    create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(
        self,
        mocker,
        version,
        karte_id,
        project_model,
        expected,
        project_karte_model,
    ):
        """正常系のテスト"""
        # 権限チェック用
        mocker_method = mocker.patch.object(SchedulesService, "is_visible_project")
        mocker_method.return_value = True

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        # delete 実行
        mock_karte = mocker.patch.object(ProjectKarteModel, "get")
        mock_karte.return_value = project_karte_model
        mocker.patch.object(ProjectKarteModel, "delete")

        response = client.delete(
            f"/api/schedules/support?karteId={karte_id}&version={version}"
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "version, karte_id, project_model, project_karte_model",
        [
            (
                1,
                "888be2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[],
                        quarterly=[],
                        half=[],
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
                ProjectKarteModel(
                    karte_id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                    project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                    customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    start_datetime="2022/01/20 10:00",
                    start_time=600,
                    end_time=780,
                    supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    last_update_datetime="2020-10-23T03:21:39.356000+0000",
                    customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    man_hour=0,
                    detail=None,
                    feedback=None,
                    homework=None,
                    documents=None,
                    deliverables=None,
                    memo=None,
                    task=None,
                    is_draft=False,
                    create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_schedule_not_found(
        self,
        mocker,
        version,
        karte_id,
        project_model,
        project_karte_model,
    ):
        """案件スケジュールが存在しない時"""
        # 権限チェック用
        mocker_method = mocker.patch.object(SchedulesService, "is_visible_project")
        mocker_method.return_value = True

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        # delete 実行
        mock_karte = mocker.patch.object(ProjectKarteModel, "get")
        mock_karte.side_effect = DoesNotExist()
        mocker.patch.object(ProjectKarteModel, "delete")

        response = client.delete(
            f"/api/schedules/support?karteId={karte_id}&version={version}"
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not Found"

    @pytest.mark.parametrize(
        "version, karte_id, project_model, project_karte_model",
        [
            (
                3,
                "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                    phase="プラン提示(D)",
                    customer_success="DXの実現",
                    support_date_from="2021/01/30",
                    support_date_to="2021/02/28",
                    profit=GrossProfitAttribute(
                        monthly=[],
                        quarterly=[],
                        half=[],
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
                ProjectKarteModel(
                    karte_id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                    project_id="efba46d3-1d56-428b-ac6d-a51e263b54a8",
                    customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    date="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    start_datetime="2022/01/20 10:00",
                    start_time=600,
                    end_time=780,
                    supporter_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    draft_supporter_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    last_update_datetime="2020-10-23T03:21:39.356000+0000",
                    customer_user_ids={"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"},
                    man_hour=0,
                    detail=None,
                    feedback=None,
                    homework=None,
                    documents=None,
                    deliverables=None,
                    memo=None,
                    task=None,
                    is_draft=False,
                    create_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_version_conflict(
        self,
        mocker,
        version,
        karte_id,
        project_model,
        project_karte_model,
    ):
        # 権限チェック用
        mocker_method = mocker.patch.object(SchedulesService, "is_visible_project")
        mocker_method.return_value = True

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        # delete 実行
        mock_karte = mocker.patch.object(ProjectKarteModel, "get")
        mock_karte.return_value = project_karte_model
        mocker.patch.object(ProjectKarteModel, "delete")

        response = client.delete(
            f"/api/schedules/support?karteId={karte_id}&version={version}"
        )

        actual = response.json()
        assert response.status_code == 409
        assert actual["detail"] == "Conflict"
