from datetime import datetime

import freezegun
import pytest
from pytz import timezone

from app.models.man_hour import (
    AcquirementItemsSubAttribute,
    DirectSupportManHoursAttribute,
    HolidaysManHoursAttribute,
    ManHourProjectSummaryModel,
    ManHourServiceTypeSummaryModel,
    ManHourSupporterModel,
    ManHourSupporterOrganizationSummaryModel,
    PreSupportManHoursAttribute,
    SalesSupportManHoursAttribute,
    SsapManHoursAttribute,
    SummaryManHourAllowNullAttribute,
    SupportItemsSubAttribute,
)
from app.models.master import (
    BatchControlAttribute,
    MasterBatchControlModel,
    MasterSupporterOrganizationModel,
)
from app.models.master_alert_setting import (
    AlertSettingAttribute,
    FactorSettingSubAttribute,
    MasterAlertSettingModel,
)
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.user import UserModel
from functions.batch_const import BatchSettingConst, BatchStatus
from functions.batch_summary_man_hour import handler


@freezegun.freeze_time("2022-07-05T12:00:00.000+0900")
class TestBatchSummary:
    @pytest.mark.parametrize(
        "project_models, supporter_models",
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
                    ManHourSupporterModel(
                        data_type="supporter#51ff73f4-2414-421c-b64c-9981f988b331",
                        year_month="2022/07",
                        supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_name="山田太郎",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        supporter_organization_name="IST",
                        direct_support_man_hours=DirectSupportManHoursAttribute(
                            items=[
                                SupportItemsSubAttribute(
                                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9266",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=30,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=20,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                                    role="副担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=50,
                                ),
                            ],
                            memo="メモ",
                        ),
                        pre_support_man_hours=PreSupportManHoursAttribute(
                            items=[
                                SupportItemsSubAttribute(
                                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9266",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=20,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=10,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                                    role="副担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=40,
                                ),
                            ],
                            memo="メモ",
                        ),
                        sales_support_man_hours=SalesSupportManHoursAttribute(
                            items=[
                                AcquirementItemsSubAttribute(
                                    project_name="サンプルプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="continuation",
                                    input_man_hour=8,
                                ),
                                AcquirementItemsSubAttribute(
                                    project_name="SAMPLEプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="new",
                                    input_man_hour=10,
                                ),
                                AcquirementItemsSubAttribute(
                                    project_name="サンプルプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="continuation",
                                    input_man_hour=0,
                                ),
                            ],
                            memo="メモ",
                        ),
                        ssap_man_hours=SsapManHoursAttribute(
                            meeting=10,
                            study=0,
                            learning=5,
                            new_service=0,
                            startdash=0,
                            improvement=0,
                            ssap=0,
                            qc=0,
                            accounting=0,
                            management=0,
                            office_work=0,
                            others=0,
                            memo="メモ",
                        ),
                        holidays_man_hours=HolidaysManHoursAttribute(
                            paid_holiday=0,
                            holiday=0,
                            private=0,
                            others=0,
                            department_others=0,
                            memo="メモ",
                        ),
                        summary_man_hour=SummaryManHourAllowNullAttribute(
                            direct=100,
                            pre=70,
                            sales=18,
                            ssap=15,
                            others=0,
                            total=203,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    )
                ],
            ),
        ],
    )
    def test_ok_project(
        self,
        mocker,
        project_models,
        supporter_models,
    ):
        """プロジェクト別工数集計作成:正常"""
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
                name="BO工数情報集計データ作成処理",
                value="partnerportal-frontoffice-dev-batch_summary_man_hour#project",
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

        mock_master_alert_setting = mocker.patch.object(
            MasterAlertSettingModel.data_type_index, "query"
        )
        mock_master_alert_setting.return_value = [
            MasterAlertSettingModel(
                id="aa82ffc2-4e7b-43cf-b242-dad4bfd8d2e5",
                data_type="alert_setting",
                name="工数アラート設定1",
                attributes=AlertSettingAttribute(
                    factor_setting=[
                        FactorSettingSubAttribute(
                            service_type_id="bad0b57c-de29-48e0-9deb-94be98437441",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="06f65c34-1570-4c02-a892-b2cf6392451a",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="8df630b1-c361-4074-8c89-77f5d9d1815d",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                    ]
                ),
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]
        # 支援者組織とサービス種別
        mock_master = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master.return_value = [
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
        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = project_models
        mock_man_hour_project_summary = mocker.patch.object(
            ManHourProjectSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_project_summary.return_value = []
        mock_man_hour_service_type_summary = mocker.patch.object(
            ManHourServiceTypeSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_service_type_summary.return_value = []
        mock_man_hour_supporter_organization_summary = mocker.patch.object(
            ManHourSupporterOrganizationSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter_organization_summary.return_value = []
        mock_man_hour_supporter = mocker.patch.object(
            ManHourSupporterModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter.return_value = supporter_models

        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = [
            UserModel(
                id="51ff73f4-2414-421c-b64c-9981f988b331",
                data_type="user",
                name="山田太郎",
                email="taro.yamada@example.com",
                job="部長",
                supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                organization_name="IST",
                cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                role="supporter_mgr",
            ),
        ]

        mocker.patch.object(ManHourProjectSummaryModel, "batch_write")
        mocker.patch.object(ManHourServiceTypeSummaryModel, "batch_write")
        mocker.patch.object(ManHourSupporterOrganizationSummaryModel, "batch_write")

        expected = "Normal end."
        param = {"mode": "project", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, supporter_models",
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
                    ManHourSupporterModel(
                        data_type="supporter#51ff73f4-2414-421c-b64c-9981f988b331",
                        year_month="2022/07",
                        supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_name="山田太郎",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        supporter_organization_name="IST",
                        direct_support_man_hours=DirectSupportManHoursAttribute(
                            items=[
                                SupportItemsSubAttribute(
                                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9266",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=30,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=20,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                                    role="副担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=50,
                                ),
                            ],
                            memo="メモ",
                        ),
                        pre_support_man_hours=PreSupportManHoursAttribute(
                            items=[
                                SupportItemsSubAttribute(
                                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9266",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=20,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=10,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                                    role="副担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=40,
                                ),
                            ],
                            memo="メモ",
                        ),
                        sales_support_man_hours=SalesSupportManHoursAttribute(
                            items=[
                                AcquirementItemsSubAttribute(
                                    project_name="サンプルプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="continuation",
                                    input_man_hour=8,
                                ),
                                AcquirementItemsSubAttribute(
                                    project_name="SAMPLEプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="new",
                                    input_man_hour=10,
                                ),
                                AcquirementItemsSubAttribute(
                                    project_name="サンプルプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="continuation",
                                    input_man_hour=0,
                                ),
                            ],
                            memo="メモ",
                        ),
                        ssap_man_hours=SsapManHoursAttribute(
                            meeting=10,
                            study=0,
                            learning=5,
                            new_service=0,
                            startdash=0,
                            improvement=0,
                            ssap=0,
                            qc=0,
                            accounting=0,
                            management=0,
                            office_work=0,
                            others=0,
                            memo="メモ",
                        ),
                        holidays_man_hours=HolidaysManHoursAttribute(
                            paid_holiday=0,
                            holiday=0,
                            private=0,
                            others=0,
                            department_others=0,
                            memo="メモ",
                        ),
                        summary_man_hour=SummaryManHourAllowNullAttribute(
                            direct=100,
                            pre=70,
                            sales=18,
                            ssap=15,
                            others=0,
                            total=203,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    )
                ],
            ),
        ],
    )
    def test_ok_service_type(
        self,
        mocker,
        project_models,
        supporter_models,
    ):
        """サービス種別別工数集計作成:正常"""
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
                name="BO工数情報集計データ作成処理",
                value="partnerportal-frontoffice-dev-batch_summary_man_hour#service_type",
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

        mock_master_alert_setting = mocker.patch.object(
            MasterAlertSettingModel.data_type_index, "query"
        )
        mock_master_alert_setting.return_value = [
            MasterAlertSettingModel(
                id="aa82ffc2-4e7b-43cf-b242-dad4bfd8d2e5",
                data_type="alert_setting",
                name="工数アラート設定1",
                attributes=AlertSettingAttribute(
                    factor_setting=[
                        FactorSettingSubAttribute(
                            service_type_id="bad0b57c-de29-48e0-9deb-94be98437441",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="06f65c34-1570-4c02-a892-b2cf6392451a",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="8df630b1-c361-4074-8c89-77f5d9d1815d",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                    ]
                ),
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]
        # 支援者組織とサービス種別
        mock_master = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master.return_value = [
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
        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = project_models
        mock_man_hour_project_summary = mocker.patch.object(
            ManHourProjectSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_project_summary.return_value = []
        mock_man_hour_service_type_summary = mocker.patch.object(
            ManHourServiceTypeSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_service_type_summary.return_value = []
        mock_man_hour_supporter_organization_summary = mocker.patch.object(
            ManHourSupporterOrganizationSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter_organization_summary.return_value = []
        mock_man_hour_supporter = mocker.patch.object(
            ManHourSupporterModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter.return_value = supporter_models

        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = [
            UserModel(
                id="51ff73f4-2414-421c-b64c-9981f988b331",
                data_type="user",
                name="山田太郎",
                email="taro.yamada@example.com",
                job="部長",
                supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                organization_name="IST",
                cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                role="supporter_mgr",
            ),
        ]

        mocker.patch.object(ManHourProjectSummaryModel, "batch_write")
        mocker.patch.object(ManHourServiceTypeSummaryModel, "batch_write")
        mocker.patch.object(ManHourSupporterOrganizationSummaryModel, "batch_write")

        expected = "Normal end."
        param = {"mode": "service_type", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, supporter_models",
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
                    ManHourSupporterModel(
                        data_type="supporter#51ff73f4-2414-421c-b64c-9981f988b331",
                        year_month="2022/07",
                        supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_name="山田太郎",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        supporter_organization_name="IST",
                        direct_support_man_hours=DirectSupportManHoursAttribute(
                            items=[
                                SupportItemsSubAttribute(
                                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9266",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=30,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=20,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                                    role="副担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=50,
                                ),
                            ],
                            memo="メモ",
                        ),
                        pre_support_man_hours=PreSupportManHoursAttribute(
                            items=[
                                SupportItemsSubAttribute(
                                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9266",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=20,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=10,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                                    role="副担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=40,
                                ),
                            ],
                            memo="メモ",
                        ),
                        sales_support_man_hours=SalesSupportManHoursAttribute(
                            items=[
                                AcquirementItemsSubAttribute(
                                    project_name="サンプルプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="continuation",
                                    input_man_hour=8,
                                ),
                                AcquirementItemsSubAttribute(
                                    project_name="SAMPLEプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="new",
                                    input_man_hour=10,
                                ),
                                AcquirementItemsSubAttribute(
                                    project_name="サンプルプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="continuation",
                                    input_man_hour=0,
                                ),
                            ],
                            memo="メモ",
                        ),
                        ssap_man_hours=SsapManHoursAttribute(
                            meeting=10,
                            study=0,
                            learning=5,
                            new_service=0,
                            startdash=0,
                            improvement=0,
                            ssap=0,
                            qc=0,
                            accounting=0,
                            management=0,
                            office_work=0,
                            others=0,
                            memo="メモ",
                        ),
                        holidays_man_hours=HolidaysManHoursAttribute(
                            paid_holiday=0,
                            holiday=0,
                            private=0,
                            others=0,
                            department_others=0,
                            memo="メモ",
                        ),
                        summary_man_hour=SummaryManHourAllowNullAttribute(
                            direct=100,
                            pre=70,
                            sales=18,
                            ssap=15,
                            others=0,
                            total=203,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    )
                ],
            ),
        ],
    )
    def test_ok_supporter_organization(
        self,
        mocker,
        project_models,
        supporter_models,
    ):
        """支援者組織（課）別工数集計作成:正常"""
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
                name="BO工数情報集計データ作成処理",
                value="partnerportal-frontoffice-dev-batch_summary_man_hour#supporter_organization",
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

        mock_master_alert_setting = mocker.patch.object(
            MasterAlertSettingModel.data_type_index, "query"
        )
        mock_master_alert_setting.return_value = [
            MasterAlertSettingModel(
                id="aa82ffc2-4e7b-43cf-b242-dad4bfd8d2e5",
                data_type="alert_setting",
                name="工数アラート設定1",
                attributes=AlertSettingAttribute(
                    factor_setting=[
                        FactorSettingSubAttribute(
                            service_type_id="bad0b57c-de29-48e0-9deb-94be98437441",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="06f65c34-1570-4c02-a892-b2cf6392451a",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="8df630b1-c361-4074-8c89-77f5d9d1815d",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                    ]
                ),
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]
        # 支援者組織とサービス種別
        mock_master = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master.return_value = [
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
        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = project_models
        mock_man_hour_project_summary = mocker.patch.object(
            ManHourProjectSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_project_summary.return_value = []
        mock_man_hour_service_type_summary = mocker.patch.object(
            ManHourServiceTypeSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_service_type_summary.return_value = []
        mock_man_hour_supporter_organization_summary = mocker.patch.object(
            ManHourSupporterOrganizationSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter_organization_summary.return_value = []
        mock_man_hour_supporter = mocker.patch.object(
            ManHourSupporterModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter.return_value = supporter_models

        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = [
            UserModel(
                id="51ff73f4-2414-421c-b64c-9981f988b331",
                data_type="user",
                name="山田太郎",
                email="taro.yamada@example.com",
                job="部長",
                supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                organization_name="IST",
                cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                role="supporter_mgr",
            ),
        ]

        mocker.patch.object(ManHourProjectSummaryModel, "batch_write")
        mocker.patch.object(ManHourServiceTypeSummaryModel, "batch_write")
        mocker.patch.object(ManHourSupporterOrganizationSummaryModel, "batch_write")

        expected = "Normal end."
        param = {"mode": "supporter_organization", "stageParams": {"stage": "dev"}}
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
                name="BO工数情報集計データ作成処理",
                value="partnerportal-frontoffice-dev-batch_summary_man_hour#supporter_organization",
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

        mock_master_alert_setting = mocker.patch.object(
            MasterAlertSettingModel.data_type_index, "query"
        )
        mock_master_alert_setting.return_value = [
            MasterAlertSettingModel(
                id="aa82ffc2-4e7b-43cf-b242-dad4bfd8d2e5",
                data_type="alert_setting",
                name="工数アラート設定1",
                attributes=AlertSettingAttribute(
                    factor_setting=[
                        FactorSettingSubAttribute(
                            service_type_id="bad0b57c-de29-48e0-9deb-94be98437441",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="06f65c34-1570-4c02-a892-b2cf6392451a",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="8df630b1-c361-4074-8c89-77f5d9d1815d",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                    ]
                ),
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]
        # 支援者組織とサービス種別
        mock_master = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master.return_value = [
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
        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = []
        mock_man_hour_project_summary = mocker.patch.object(
            ManHourProjectSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_project_summary.return_value = []
        mock_man_hour_service_type_summary = mocker.patch.object(
            ManHourServiceTypeSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_service_type_summary.return_value = []
        mock_man_hour_supporter_organization_summary = mocker.patch.object(
            ManHourSupporterOrganizationSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter_organization_summary.return_value = []
        mock_man_hour_supporter = mocker.patch.object(
            ManHourSupporterModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter.return_value = []

        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = [
            UserModel(
                id="51ff73f4-2414-421c-b64c-9981f988b331",
                data_type="user",
                name="山田太郎",
                email="taro.yamada@example.com",
                job="部長",
                supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                organization_name="IST",
                cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                role="supporter_mgr",
            ),
        ]

        mocker.patch.object(ManHourProjectSummaryModel, "batch_write")
        mocker.patch.object(ManHourServiceTypeSummaryModel, "batch_write")
        mocker.patch.object(ManHourSupporterOrganizationSummaryModel, "batch_write")

        expected = "Error end."
        param = {"mode": "xxxx", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    @pytest.mark.parametrize(
        "project_models, supporter_models",
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
                    ManHourSupporterModel(
                        data_type="supporter#51ff73f4-2414-421c-b64c-9981f988b331",
                        year_month="2022/07",
                        supporter_user_id="51ff73f4-2414-421c-b64c-9981f988b331",
                        supporter_name="山田太郎",
                        supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                        supporter_organization_name="IST",
                        direct_support_man_hours=DirectSupportManHoursAttribute(
                            items=[
                                SupportItemsSubAttribute(
                                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9266",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=30,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=20,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                                    role="副担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=50,
                                ),
                            ],
                            memo="メモ",
                        ),
                        pre_support_man_hours=PreSupportManHoursAttribute(
                            items=[
                                SupportItemsSubAttribute(
                                    project_id="2bb85e8d-7560-48b0-82c7-d8ba134a9266",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=20,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="2854fc20-caea-44bb-ada5-d5ec3595f4c0",
                                    role="主担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=10,
                                ),
                                SupportItemsSubAttribute(
                                    project_id="ca57d2cd-a898-4344-bdbd-9811fb26e19f",
                                    role="副担当",
                                    service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                                    input_man_hour=40,
                                ),
                            ],
                            memo="メモ",
                        ),
                        sales_support_man_hours=SalesSupportManHoursAttribute(
                            items=[
                                AcquirementItemsSubAttribute(
                                    project_name="サンプルプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="continuation",
                                    input_man_hour=8,
                                ),
                                AcquirementItemsSubAttribute(
                                    project_name="SAMPLEプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="new",
                                    input_man_hour=10,
                                ),
                                AcquirementItemsSubAttribute(
                                    project_name="サンプルプロジェクト",
                                    customer_name="一般社団法人アアアアア",
                                    type="continuation",
                                    input_man_hour=0,
                                ),
                            ],
                            memo="メモ",
                        ),
                        ssap_man_hours=SsapManHoursAttribute(
                            meeting=10,
                            study=0,
                            learning=5,
                            new_service=0,
                            startdash=0,
                            improvement=0,
                            ssap=0,
                            qc=0,
                            accounting=0,
                            management=0,
                            office_work=0,
                            others=0,
                            memo="メモ",
                        ),
                        holidays_man_hours=HolidaysManHoursAttribute(
                            paid_holiday=0,
                            holiday=0,
                            private=0,
                            others=0,
                            department_others=0,
                            memo="メモ",
                        ),
                        summary_man_hour=SummaryManHourAllowNullAttribute(
                            direct=100,
                            pre=70,
                            sales=18,
                            ssap=15,
                            others=0,
                            total=203,
                        ),
                        create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        create_at="2022-05-23T16:34:21.523000+0000",
                        update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                        update_at="2022-05-23T16:34:21.523000+0000",
                        version=1,
                    )
                ],
            ),
        ],
    )
    def test_ok_skip_end(
        self,
        mocker,
        project_models,
        supporter_models,
    ):
        """処理途中（スキップ）終了"""
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
                name="BO工数情報集計データ作成処理",
                value="partnerportal-frontoffice-dev-batch_summary_man_hour#supporter_organization",
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

        mock_master_alert_setting = mocker.patch.object(
            MasterAlertSettingModel.data_type_index, "query"
        )
        mock_master_alert_setting.return_value = [
            MasterAlertSettingModel(
                id="aa82ffc2-4e7b-43cf-b242-dad4bfd8d2e5",
                data_type="alert_setting",
                name="工数アラート設定1",
                attributes=AlertSettingAttribute(
                    factor_setting=[
                        FactorSettingSubAttribute(
                            service_type_id="bad0b57c-de29-48e0-9deb-94be98437441",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="06f65c34-1570-4c02-a892-b2cf6392451a",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="8df630b1-c361-4074-8c89-77f5d9d1815d",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                        FactorSettingSubAttribute(
                            service_type_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                            direct_support_man_hour=20,
                            direct_and_pre_support_man_hour=60,
                        ),
                    ]
                ),
                create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                create_at="2020-10-23T03:21:39.356000+0000",
                update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
                update_at="2020-10-23T03:21:39.356000+0000",
                version=1,
            )
        ]
        # 支援者組織とサービス種別
        mock_master = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master.return_value = [
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
        mock_project = mocker.patch.object(
            ProjectModel.data_type_support_date_to_index, "query"
        )
        mock_project.return_value = project_models
        mock_man_hour_project_summary = mocker.patch.object(
            ManHourProjectSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_project_summary.return_value = []
        mock_man_hour_service_type_summary = mocker.patch.object(
            ManHourServiceTypeSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_service_type_summary.return_value = []
        mock_man_hour_supporter_organization_summary = mocker.patch.object(
            ManHourSupporterOrganizationSummaryModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter_organization_summary.return_value = []
        mock_man_hour_supporter = mocker.patch.object(
            ManHourSupporterModel.year_month_data_type_index, "query"
        )
        mock_man_hour_supporter.return_value = supporter_models

        mock_user = mocker.patch.object(UserModel, "scan")
        mock_user.return_value = [
            UserModel(
                id="51ff73f4-2414-421c-b64c-9981f988b331",
                data_type="user",
                name="山田太郎",
                email="taro.yamada@example.com",
                job="部長",
                supporter_organization_id=["89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"],
                organization_name="IST",
                cognito_id="itk5s7d0-fj4s-kg05-b238-kld430d2bb97",
                role="supporter_mgr",
            ),
        ]

        mocker.patch.object(ManHourProjectSummaryModel, "batch_write")
        mocker.patch.object(ManHourServiceTypeSummaryModel, "batch_write")
        mocker.patch.object(ManHourSupporterOrganizationSummaryModel, "batch_write")

        expected = "Skipped processing."
        param = {"mode": "supporter_organization", "stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected
