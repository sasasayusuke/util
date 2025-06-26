import json
from datetime import datetime

import freezegun
import pytest
from pytz import timezone

from app.models.admin import AdminModel
from app.models.man_hour import (
    DirectSupportManHoursAttribute,
    HolidaysManHoursAttribute,
    ManHourSupporterModel,
    PreSupportManHoursAttribute,
    SalesSupportManHoursAttribute,
    SsapManHoursAttribute,
)
from app.models.master import (
    BatchControlAttribute,
    MasterBatchControlModel,
    MasterServiceManagerModel,
    MasterSupporterOrganizationModel,
)
from app.models.notification import NotificationModel
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_karte import ProjectKarteModel
from app.models.project_survey import (
    PointsAttribute,
    ProjectSurveyModel,
    SupporterUserAttribute,
    EvaluationSupporterAttribute,
)
from app.models.survey_master import (
    QuestionFlowAttribute,
    QuestionsAttribute,
    SurveyMasterModel,
)
from app.models.user import UserModel
from app.utils.aws.sqs import SqsHelper
from app.utils.platform import PlatformApiOperator
from functions.batch_const import BatchSettingConst, BatchStatus, UserRoleType
from functions.batch_remind import handler


@freezegun.freeze_time("2022-07-05T12:00:00.000+0900")
class TestBatchRemind:
    @pytest.mark.parametrize(
        "project_models, user_models, karte_models",
        [
            (
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
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
                        main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ],
                [
                    UserModel(
                        id="deb0c0e9-b5ec-49d0-b8da-839fa82e15b1",
                        data_type="user",
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter_mgr",
                    ),
                ],
                [
                    ProjectKarteModel(
                        karte_id="112d68a1-eb5b-4686-b5c5-ac5aaba830ce",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        customer_id="b11846b6-21a2-479a-81f8-9e12581f92fd",
                        date="2022/07/14",
                        start_datetime="2022/07/14 10:00",
                        start_time="10:00",
                        end_time="11:00",
                        supporter_ids=set(["deb0c0e9-b5ec-49d0-b8da-839fa82e15b1"]),
                        is_draft=True,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_ok_remind_karte(
        self,
        mocker,
        project_models,
        user_models,
        karte_models,
    ):
        """カルテ書き忘れリマインド通知:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="FO各種リマインド通知処理",
                value="partnerportal-frontoffice-dev-batch_remind#remind_karte",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # mock: カルテ書き忘れリマインド通知処理
        mock_karte = mocker.patch.object(ProjectKarteModel.date_index, "query")
        mock_karte.return_value = karte_models
        mock_project = mocker.patch.object(ProjectModel, "batch_get")
        mock_project.return_value = project_models
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models
        mocker.patch.object(NotificationModel, "batch_write")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        expected = "Normal end."
        param = {"mode": "remind_karte", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, user_models",
        [
            (
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
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
                        main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ],
                [
                    UserModel(
                        id="deb0c0e9-b5ec-49d0-b8da-839fa82e15b1",
                        data_type="user",
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter_mgr",
                    ),
                ],
            )
        ],
    )
    def test_ok_remind_master_karten_current_program(
        self,
        mocker,
        project_models,
        user_models,
    ):
        """マスターカルテ当期支援入力催促通知:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="FO各種リマインド通知処理",
                value="partnerportal-frontoffice-dev-batch_remind#remind_karte",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # mock: マスターカルテ当期支援入力催促通知処理
        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = project_models
        json_open = open("mock/pf/get_projects.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock.return_value = (200, json_load)
        json_open = open("mock/pf/get_project_by_id.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_project_by_pf_id")
        mock.return_value = (200, json_load)
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models
        mocker.patch.object(NotificationModel, "batch_write")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        expected = "Normal end."
        param = {"mode": "remind_karte", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, user_models",
        [
            (
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
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
                        main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ],
                [
                    UserModel(
                        id="deb0c0e9-b5ec-49d0-b8da-839fa82e15b1",
                        data_type="user",
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter_mgr",
                    ),
                ],
            )
        ],
    )
    def test_ok_remind_master_karten_next_program(
        self,
        mocker,
        project_models,
        user_models,
    ):
        """マスターカルテ次期支援入力催促通知:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="FO各種リマインド通知処理",
                value="partnerportal-frontoffice-dev-batch_remind#remind_karte",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # mock: マスターカルテ次期支援入力催促通知処理
        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = project_models
        json_open = open("mock/pf/get_projects.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock.return_value = (200, json_load)
        json_open = open("mock/pf/get_project_by_id.json", "r")
        json_load = json.load(json_open)
        mock = mocker.patch.object(PlatformApiOperator, "get_project_by_pf_id")
        mock.return_value = (200, json_load)
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models
        mocker.patch.object(NotificationModel, "batch_write")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        expected = "Normal end."
        param = {"mode": "remind_karte", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, user_models, survey_models, survey_master_models, admin_models",
        [
            (
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
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
                        main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                        is_count_man_hour=True,
                        is_karte_remind=True,
                        contract_type="有償",
                        is_secret=False,
                        is_solver_project=False,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    UserModel(
                        id="51ff73f4-2414-421c-b64c-9981f988b331",
                        data_type="user",
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter_mgr",
                    ),
                    UserModel(
                        id="79ac0833-bf58-4f12-b566-1b1ae3c45abf",
                        data_type="user",
                        name="田中次郎",
                        email="jiro.tanaka@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter_mgr",
                    ),
                ],
                [
                    ProjectSurveyModel(
                        id="ce2a4882-2f11-467c-80a4-3acb2ff4992a",
                        data_type="survey",
                        survey_master_id="d0c742e4-de71-47a6-a38f-731ccab2099c",
                        survey_master_revision=1,
                        survey_type="service",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        project_name="サンプルプロジェクト１",
                        main_supporter_user=SupporterUserAttribute(
                            id="51ff73f4-2414-421c-b64c-9981f988b331"
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="79ac0833-bf58-4f12-b566-1b1ae3c45abf"
                            )
                        ],
                        sales_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        answer_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        answer_user_name="山田太郎",
                        customer_id="4d4464cc-1b81-42d6-94a6-ece8ffa7b187",
                        customer_name="取引先株式会社",
                        summary_month="2022/07",
                        plan_survey_request_datetime="2022/07/10 09:00",
                        actual_survey_request_datetime="2022/07/10 09:00",
                        plan_survey_response_datetime="2022/07/20 09:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=False,
                        is_not_summary=False,
                        is_solver_project=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                    ProjectSurveyModel(
                        id="b62c5993-0975-4c88-8698-fbf818bc0c13",
                        data_type="survey",
                        survey_master_id="d0c742e4-de71-47a6-a38f-731ccab2099c",
                        survey_master_revision=1,
                        survey_type="service",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        project_name="サンプルプロジェクト１",
                        main_supporter_user=SupporterUserAttribute(
                            id="51ff73f4-2414-421c-b64c-9981f988b331"
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="51ff73f4-2414-421c-b64c-9981f988b331"
                            )
                        ],
                        is_updated_evaluation_supporters=True,
                        evaluation_supporters=[
                            EvaluationSupporterAttribute(
                                supporter_id="51ff73f4-2414-421c-b64c-9981f988b331",
                                karte_ids=["8897a39d-8567-4b12-a3ef-cfcde3cbf642"]
                            )
                        ],
                        supporter_users_before_update=[
                            SupporterUserAttribute(
                                id="79ac0833-bf58-4f12-b566-1b1ae3c45abf"
                            )
                        ],
                        sales_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        answer_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        answer_user_name="山田太郎",
                        customer_id="4d4464cc-1b81-42d6-94a6-ece8ffa7b187",
                        customer_name="取引先株式会社",
                        summary_month="2022/07",
                        plan_survey_request_datetime="2022/07/10 09:00",
                        actual_survey_request_datetime="2022/07/10 09:00",
                        plan_survey_response_datetime="2022/07/20 09:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=False,
                        is_not_summary=False,
                        is_solver_project=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                [
                    SurveyMasterModel(
                        id="d0c742e4-de71-47a6-a38f-731ccab2099c",
                        revision=1,
                        name="テンプレート",
                        type="service",
                        timing="毎月",
                        init_send_day_setting=20,
                        init_answer_limit_day_setting=5,
                        is_disclosure=True,
                        questions=[QuestionsAttribute()],
                        question_flow=[QuestionFlowAttribute()],
                        is_latest=1,
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    )
                ],
                [
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SYSTEM_ADMIN.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SURVEY_OPS.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.MAN_HOUR_OPS.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SUPPORTER_MGR.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_ok_schedule_survey(
        self,
        mocker,
        project_models,
        user_models,
        survey_models,
        survey_master_models,
        admin_models,
    ):
        """アンケート回答依頼通知:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="FO各種リマインド通知処理",
                value="partnerportal-frontoffice-dev-batch_remind#schedule_survey",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # mock: アンケート回答依頼通知処理
        mock_survey = mocker.patch.object(
            ProjectSurveyModel.data_type_summary_month_index, "query"
        )
        mock_survey.return_value = survey_models
        mock_survey_master = mocker.patch.object(
            SurveyMasterModel.is_latest_name_index, "query"
        )
        mock_survey_master.return_value = survey_master_models

        mock_project = mocker.patch.object(ProjectModel, "batch_get")
        mock_project.return_value = project_models
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models
        mock_master_supporter_org = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master_supporter_org.return_value = [
            MasterSupporterOrganizationModel(
                id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                data_type="master_supporter_organization",
                name="Ideation Service Team",
                value="IST",
                attributes=None,
                order=1,
                use=True,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T04:21:39.356000+0000",
                version=1,
            )
        ]
        mock_master_service_manager = mocker.patch.object(
            MasterServiceManagerModel.data_type_index, "query"
        )
        mock_master_service_manager.return_value = [
            MasterServiceManagerModel(
                id="8e394235-fde1-4546-a7e2-1f76e7ead6ac",
                data_type="service_manager",
                name="テスト太郎（サービス責任者）",
                supporter_organization_id="de40733f-6be9-4fef-8229-01052f43c1e2",
                supporter_organization_name="IST",
            ),
        ]
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = admin_models
        mock_survey_by_project_id = mocker.patch.object(
            ProjectSurveyModel.project_id_summary_month_index, "query"
        )
        mock_survey_by_project_id.return_value = survey_models
        mocker.patch.object(ProjectSurveyModel, "batch_write")
        mocker.patch.object(NotificationModel, "batch_write")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        expected = "Normal end."
        param = {"mode": "schedule_survey", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, user_models, survey_models, survey_master_models, admin_models",
        [
            (
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
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
                        main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ],
                [
                    UserModel(
                        id="51ff73f4-2414-421c-b64c-9981f988b331",
                        data_type="user",
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter_mgr",
                    ),
                ],
                [
                    ProjectSurveyModel(
                        id="ce2a4882-2f11-467c-80a4-3acb2ff4992a",
                        data_type="survey",
                        survey_master_id="d0c742e4-de71-47a6-a38f-731ccab2099c",
                        survey_master_revision=1,
                        survey_type="service",
                        project_id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        project_name="サンプルプロジェクト１",
                        main_supporter_user=SupporterUserAttribute(
                            id="51ff73f4-2414-421c-b64c-9981f988b331"
                        ),
                        supporter_users=[
                            SupporterUserAttribute(
                                id="51ff73f4-2414-421c-b64c-9981f988b331"
                            )
                        ],
                        sales_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        answer_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        answer_user_name="山田太郎",
                        customer_id="4d4464cc-1b81-42d6-94a6-ece8ffa7b187",
                        customer_name="取引先株式会社",
                        summary_month="2022/07",
                        plan_survey_request_datetime="2022/07/10 09:00",
                        actual_survey_request_datetime="2022/07/10 09:00",
                        plan_survey_response_datetime="2022/07/01 09:00",
                        actual_survey_response_datetime=None,
                        is_finished=False,
                        is_disclosure=False,
                        is_not_summary=False,
                        points=PointsAttribute(
                            satisfaction=0,
                            continuation=False,
                            recommended=0,
                            sales=0,
                            survey_satisfaction=0,
                            man_hour_satisfaction=0,
                            karte_satisfaction=0,
                            master_karte_satisfaction=0,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    ),
                ],
                SurveyMasterModel(
                    id="d0c742e4-de71-47a6-a38f-731ccab2099c",
                    revision=1,
                    name="テンプレート",
                    type="service",
                    timing="毎月",
                    init_send_day_setting=20,
                    init_answer_limit_day_setting=5,
                    is_disclosure=True,
                    questions=[QuestionsAttribute()],
                    question_flow=[QuestionFlowAttribute()],
                    is_latest=1,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                ),
                [
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SYSTEM_ADMIN.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SURVEY_OPS.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.MAN_HOUR_OPS.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SUPPORTER_MGR.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_ok_remind_survey(
        self,
        mocker,
        project_models,
        user_models,
        survey_models,
        survey_master_models,
        admin_models,
    ):
        """アンケート催促通知:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="FO各種リマインド通知処理",
                value="partnerportal-frontoffice-dev-batch_remind#remind_survey",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # mock: アンケート催促通知処理
        mock_survey = mocker.patch.object(
            ProjectSurveyModel.data_type_summary_month_index, "query"
        )
        mock_survey.return_value = survey_models

        mock_project = mocker.patch.object(ProjectModel, "batch_get")
        mock_project.return_value = project_models
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = admin_models
        mock_survey_master = mocker.patch.object(SurveyMasterModel, "get")
        mock_survey_master.return_value = survey_master_models
        mocker.patch.object(ProjectSurveyModel, "batch_write")
        mocker.patch.object(NotificationModel, "batch_write")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        expected = "Normal end."
        param = {"mode": "remind_survey", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, user_models, man_hour_models, admin_models",
        [
            (
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
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
                        main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ],
                [
                    UserModel(
                        id="51ff73f4-2414-421c-b64c-9981f988b331",
                        data_type="user",
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "7ac8bddf-88da-46c9-a504-a03d1661ad58"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter_mgr",
                    ),
                    UserModel(
                        id="eec73b2a-2a28-492f-b748-8e0e5ab55f26",
                        data_type="user",
                        name="山田次郎",
                        email="taro.yamada@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "7ac8bddf-88da-46c9-a504-a03d1661ad58"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter",
                    ),
                ],
                [
                    ManHourSupporterModel(
                        data_type="supporter#003772df-0b35-4b8c-a9d5-24b91a0936ca",
                        year_month="2022/06",
                        supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_name="山田太郎",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        supporter_organization_name="IST",
                        direct_support_man_hours=DirectSupportManHoursAttribute(),
                        pre_support_man_hours=PreSupportManHoursAttribute(),
                        sales_support_man_hours=SalesSupportManHoursAttribute(),
                        ssap_man_hours=SsapManHoursAttribute(),
                        holidays_man_hours=HolidaysManHoursAttribute(),
                        is_confirm=False,
                    ),
                ],
                [
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SYSTEM_ADMIN.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SURVEY_OPS.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.MAN_HOUR_OPS.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SUPPORTER_MGR.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_ok_remind_manhour(
        self,
        mocker,
        project_models,
        user_models,
        man_hour_models,
        admin_models,
    ):
        """工数提出漏れ通知:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="FO各種リマインド通知処理",
                value="partnerportal-frontoffice-dev-batch_remind#remind_manhour",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # mock: 工数提出漏れ通知処理
        mock_man_hour = mocker.patch.object(
            ManHourSupporterModel.year_month_data_type_index, "query"
        )
        mock_man_hour.return_value = man_hour_models

        mock_project = mocker.patch.object(ProjectModel, "batch_get")
        mock_project.return_value = project_models
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models
        mock_master_supporter_org = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master_supporter_org.return_value = [
            MasterSupporterOrganizationModel(
                id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                data_type="master_supporter_organization",
                name="Ideation Service Team",
                value="IST",
                attributes=None,
                order=1,
                use=True,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T04:21:39.356000+0000",
                version=1,
            )
        ]
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = admin_models
        mocker.patch.object(NotificationModel, "batch_write")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        expected = "Normal end."
        param = {"mode": "remind_manhour", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    def test_error(
        self,
        mocker,
    ):
        """エラー"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="FO各種リマインド通知処理",
                value="partnerportal-frontoffice-dev-batch_remind#remind_manhour",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        mocker.patch.object(NotificationModel, "batch_write")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        expected = "Error end."
        param = {"mode": "xxxx", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, user_models, man_hour_models, admin_models",
        [
            (
                [
                    ProjectModel(
                        id="a0137afe-13d2-46ba-bc8d-53829a770934",
                        data_type="project",
                        salesforce_customer_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                        name="サンプルプロジェクト１",
                        customer_name="ソニーグループ株式会社",
                        service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        create_new=True,
                        continued=True,
                        main_sales_user_id="1b5e955f-4c1a-405d-b9ba-b618c8ba1edd",
                        contract_date="2021/01/30",
                        phase="プラン提示(D)",
                        customer_success="DXの実現",
                        support_date_from="2021/01/30",
                        support_date_to="2021/02/28",
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
                        main_customer_user_id="7b36cc66-fe66-495f-b416-16ecc879fbaa",
                        salesforce_main_customer=SalesforceMainCustomerAttribute(
                            name="山田太郎",
                            email="yamada@example.com",
                            organization_name="IST",
                            job="部長",
                        ),
                        customer_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
                        ),
                        main_supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        salesforce_main_supporter_user_name="山田太郎",
                        supporter_user_ids=set(
                            list(["51ff73f4-2414-421c-b64c-9981f988b331"])
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
                ],
                [
                    UserModel(
                        id="51ff73f4-2414-421c-b64c-9981f988b331",
                        data_type="user",
                        name="山田太郎",
                        email="taro.yamada@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "7ac8bddf-88da-46c9-a504-a03d1661ad58"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter_mgr",
                    ),
                    UserModel(
                        id="eec73b2a-2a28-492f-b748-8e0e5ab55f26",
                        data_type="user",
                        name="山田次郎",
                        email="taro.yamada@example.com",
                        job="部長",
                        supporter_organization_id=[
                            "7ac8bddf-88da-46c9-a504-a03d1661ad58"
                        ],
                        organization_name="IST",
                        cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                        role="supporter",
                    ),
                ],
                [
                    ManHourSupporterModel(
                        data_type="supporter#003772df-0b35-4b8c-a9d5-24b91a0936ca",
                        year_month="2022/06",
                        supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_name="山田太郎",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        supporter_organization_name="IST",
                        direct_support_man_hours=DirectSupportManHoursAttribute(),
                        pre_support_man_hours=PreSupportManHoursAttribute(),
                        sales_support_man_hours=SalesSupportManHoursAttribute(),
                        ssap_man_hours=SsapManHoursAttribute(),
                        holidays_man_hours=HolidaysManHoursAttribute(),
                        is_confirm=False,
                    ),
                ],
                [
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SYSTEM_ADMIN.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SURVEY_OPS.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.MAN_HOUR_OPS.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                    AdminModel(
                        id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        data_type="admin",
                        name="田中太郎",
                        email="user@example.com",
                        roles=UserRoleType.SUPPORTER_MGR.key,
                        company=None,
                        job="部長",
                        supporter_organization_id="180a3597-b7e7-42c8-902c-a29016afa662",
                        organization_name=None,
                        cognito_id=None,
                        last_login_at=datetime.now(),
                        otp_secret=None,
                        otp_verified_token=None,
                        otp_verified_at=None,
                        disabled=False,
                        create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        create_at="2020-10-23T03:21:39.356000+0000",
                        update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                        update_at="2020-10-23T03:21:39.356000+0000",
                        version=1,
                    ),
                ],
            ),
        ],
    )
    def test_ok_skip_end(
        self,
        mocker,
        project_models,
        user_models,
        man_hour_models,
        admin_models,
    ):
        """工数提出漏れ通知:正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2022, month=7, day=5, hour=12, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="FO各種リマインド通知処理",
                value="partnerportal-frontoffice-dev-batch_remind#remind_manhour",
                update_at=update_datetime,
                attributes=BatchControlAttribute(
                    batch_start_at=update_datetime,
                    batch_status=BatchStatus.RUNNING,
                    batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
                ),
            )
        ]
        mocker.patch.object(MasterBatchControlModel, "save")
        mocker.patch.object(MasterBatchControlModel, "update")

        # mock: 工数提出漏れ通知処理
        mock_man_hour = mocker.patch.object(
            ManHourSupporterModel.year_month_data_type_index, "query"
        )
        mock_man_hour.return_value = man_hour_models

        mock_project = mocker.patch.object(ProjectModel, "batch_get")
        mock_project.return_value = project_models
        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = user_models
        mock_master_supporter_org = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master_supporter_org.return_value = [
            MasterSupporterOrganizationModel(
                id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                data_type="master_supporter_organization",
                name="Ideation Service Team",
                value="IST",
                attributes=None,
                order=1,
                use=True,
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T04:21:39.356000+0000",
                version=1,
            )
        ]
        mock_admin = mocker.patch.object(AdminModel, "scan")
        mock_admin.return_value = admin_models
        mocker.patch.object(NotificationModel, "batch_write")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        expected = "Skipped processing."
        param = {"mode": "remind_manhour", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected
