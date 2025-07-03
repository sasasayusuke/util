import freezegun
import pytest
from fastapi.testclient import TestClient
from pynamodb.models import BatchWrite

from app.main import app
from app.models.master import MasterServiceManagerModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.survey_master import SurveyMasterModel
from app.models.user import UserModel
from app.resources.const import DataType, UserRoleType
from app.service.common_service.cached_db_items import CachedDbItems
from app.service.project_service import ProjectService
from app.service.schedule_service import SchedulesService

client = TestClient(app)


@freezegun.freeze_time("2022/08/31")
class TestBulkCreateSurveySchedules:
    @pytest.mark.parametrize(
        "project_id, body, project_model, user_model, survey_master_service_model, survey_master_completion_model, user_model_db",
        [
            (
                "2ed6e959-c216-46a8-92c3-3d7d3f951d00",
                {
                    "service": {"requestDate": 10, "limitDate": 99},
                    "completion": {"requestDate": 10, "limitDate": 99},
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
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.CUSTOMER,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=None,
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    )
                ],
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=1,
                    name="サービスアンケート",
                    type="service",
                    timing="monthly",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[{}],
                    question_flow=[{}],
                    is_latest=1,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=1,
                    name="修了アンケート",
                    type="completion",
                    timing="once",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[{}],
                    question_flow=[{}],
                    is_latest=1,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                UserModel(
                    id="c9b67094-cdab-494c-818e-d4845088269b",
                    data_type=DataType.USER,
                    name="田中太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
                    supporter_organization_id=["7ac8bddf-88da-46c9-a504-a03d1661ad58"],
                ),
            ),
            (
                "2ed6e959-c216-46a8-92c3-3d7d3f951d00",
                {
                    "service": {"requestDate": -5, "limitDate": 99},
                    "completion": {"requestDate": -5, "limitDate": 99},
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
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.CUSTOMER,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=None,
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    )
                ],
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=1,
                    name="サービスアンケート",
                    type="service",
                    timing="monthly",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[{}],
                    question_flow=[{}],
                    is_latest=1,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=1,
                    name="修了アンケート",
                    type="completion",
                    timing="once",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[{}],
                    question_flow=[{}],
                    is_latest=1,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                UserModel(
                    id="c9b67094-cdab-494c-818e-d4845088269b",
                    data_type=DataType.USER,
                    name="田中太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
                    supporter_organization_id=["7ac8bddf-88da-46c9-a504-a03d1661ad58"],
                ),
            ),
            (
                "2ed6e959-c216-46a8-92c3-3d7d3f951d00",
                {
                    "service": {"requestDate": -5, "limitDate": 103},
                    "completion": {"requestDate": -5, "limitDate": 103},
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
                [
                    UserModel(
                        id="a9b67094-cdab-494c-818e-d4845088269b",
                        data_type=DataType.USER,
                        name="田中太郎",
                        email="user@example.com",
                        role=UserRoleType.CUSTOMER,
                        customer_id="033bd0b5-c2c7-4778-a58d-76a46500f7d9",
                        customer_name="ソニーグループ株式会社",
                        job="部長",
                        company=None,
                        supporter_organization_id=None,
                        is_input_man_hour=None,
                        project_ids=None,
                        cognito_id=None,
                        agreed=False,
                        last_login_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    )
                ],
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=1,
                    name="サービスアンケート",
                    type="service",
                    timing="monthly",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[{}],
                    question_flow=[{}],
                    is_latest=1,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                SurveyMasterModel(
                    id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    revision=1,
                    name="修了アンケート",
                    type="completion",
                    timing="once",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[{}],
                    question_flow=[{}],
                    is_latest=1,
                    memo="string",
                    create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    create_at="2022-04-23T03:21:39.356Z",
                    update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                    update_at="2022-04-23T03:21:39.356Z",
                    version="1",
                ),
                UserModel(
                    id="c9b67094-cdab-494c-818e-d4845088269b",
                    data_type=DataType.USER,
                    name="田中太郎",
                    email="user@example.com",
                    role=UserRoleType.CUSTOMER,
                    supporter_organization_id=["7ac8bddf-88da-46c9-a504-a03d1661ad58"],
                ),
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_sales_user")
    def test_ok(
        self,
        mocker,
        project_id,
        body,
        project_model,
        user_model,
        survey_master_service_model,
        survey_master_completion_model,
        user_model_db,
    ):
        """正常系のテスト"""
        mocker_method = mocker.patch.object(SchedulesService, "is_visible_project")
        mocker_method.return_value = True

        project_mock = mocker.patch.object(ProjectModel, "get_project")
        project_mock.return_value = project_model

        mocker.patch.object(BatchWrite, "save")

        user_mock = mocker.patch.object(UserModel, "batch_get")
        user_mock.return_value = user_model

        supporter_organization_mock = mocker.patch.object(
            ProjectService, "get_supporter_organization_name"
        )
        supporter_organization_mock.return_value = "IST"

        survey_master_service_mock = mocker.patch.object(
            SurveyMasterModel, "get_latest_survey_masters"
        )
        survey_master_service_mock.return_value = survey_master_service_model

        survey_master_completion_mock = mocker.patch.object(
            SurveyMasterModel, "get_latest_survey_masters"
        )
        survey_master_completion_mock.return_value = survey_master_completion_model

        service_types_mock = mocker.patch.object(CachedDbItems, "ReturnServiceTypes")
        service_types_mock.return_value = [
            {
                "id": "06f65c34-1570-4c02-a892-b2cf6392451a",
                "name": "サービスタイプ1234",
            },
            {
                "id": "bad0b57c-de29-48e0-9deb-94be98437441",
                "name": "サービスタイプ5678",
            },
        ]

        user_info_mock = mocker.patch.object(UserModel, "get_user_by_id")
        user_info_mock.return_value = user_model_db

        organization_name_mock = mocker.patch.object(
            ProjectService, "get_supporter_organization_name"
        )
        organization_name_mock.return_value = "IST"

        user_mock = mocker.patch.object(ProjectModel, "update")

        master_service_manager_mock = mocker.patch.object(
            MasterServiceManagerModel.data_type_index, "query"
        )
        master_service_manager_mock.return_value = iter(
            [
                MasterServiceManagerModel(
                    id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                    data_type="service_manager",
                    name="テスト太郎（サービス責任者）",
                    supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                    supporter_organization_name="IST",
                ),
            ]
        )

        # 検証
        response = client.post(f"/api/schedules/survey/bulk/{project_id}", json=body)

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "body",
        [
            #####################
            # requestDate
            #####################
            # 必須
            {
                "service": {"limitDate": 99},
                "completion": {"requestDate": 10, "limitDate": 99},
            },
            {
                "service": {"requestDate": 10, "limitDate": 99},
                "completion": {"limitDate": 99},
            },
            # None
            {
                "service": {"requestDate": None, "limitDate": 99},
                "completion": {"requestDate": 10, "limitDate": 99},
            },
            {
                "service": {"requestDate": 10, "limitDate": 99},
                "completion": {"requestDate": None, "limitDate": 99},
            },
            # 値チェック
            {
                "service": {"requestDate": 0, "limitDate": 99},
                "completion": {"requestDate": 10, "limitDate": 99},
            },
            {
                "service": {"requestDate": 10, "limitDate": 99},
                "completion": {"requestDate": 0, "limitDate": 99},
            },
            #####################
            # limitDate
            #####################
            # 必須
            {
                "service": {"requestDate": 10},
                "completion": {"requestDate": 10, "limitDate": 99},
            },
            {
                "service": {"requestDate": 10, "limitDate": 99},
                "completion": {"requestDate": 10},
            },
            # None
            {
                "service": {"requestDate": 10, "limitDate": None},
                "completion": {"requestDate": 10, "limitDate": 99},
            },
            {
                "service": {"requestDate": 10, "limitDate": 99},
                "completion": {"requestDate": 10, "limitDate": None},
            },
            # 値チェック
            {
                "service": {"requestDate": 10, "limitDate": 150},
                "completion": {"requestDate": 10, "limitDate": 99},
            },
            {
                "service": {"requestDate": 10, "limitDate": 99},
                "completion": {"requestDate": 10, "limitDate": 150},
            },
            # 組み合わせ
            {
                "service": {"requestDate": -3, "limitDate": 10},
                "completion": {"requestDate": 10, "limitDate": 99},
            },
            {
                "service": {"requestDate": 10, "limitDate": 99},
                "completion": {"requestDate": -3, "limitDate": 10},
            },
            {
                "service": {"requestDate": -3, "limitDate": 0},
                "completion": {"requestDate": 10, "limitDate": 99},
            },
            {
                "service": {"requestDate": 10, "limitDate": 99},
                "completion": {"requestDate": -3, "limitDate": 0},
            },
        ],
    )
    @pytest.mark.usefixtures("auth_sales_user")
    def test_validation(self, body):
        project_id = "2ed6e959-c216-46a8-92c3-3d7d3f951d00"

        # 検証
        response = client.post(f"/api/schedules/survey/bulk/{project_id}", json=body)

        assert response.status_code == 422
