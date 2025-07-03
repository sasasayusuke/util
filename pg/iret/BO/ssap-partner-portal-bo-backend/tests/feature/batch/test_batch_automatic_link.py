import json
from datetime import datetime

import freezegun
from botocore.auth import SigV4Auth
from moto import mock_dynamodb
from pytz import timezone

import app.utils.platform
from app.models.admin import AdminModel
from app.models.customer import CustomerModel
from app.models.master import (
    BatchControlAttribute,
    MasterBatchControlModel,
    MasterSupporterOrganizationModel,
)
from app.models.notification import NotificationModel
from app.models.project import (
    ProjectModel,
)
from app.models.user import UserModel
from app.resources.const import MasterDataType
from app.utils.aws.sqs import SqsHelper
from app.utils.platform import PlatformApiOperator
from functions.batch_automatic_link import handler
from functions.batch_const import (
    BatchSettingConst,
    BatchStatus,
    DataType,
    UserRoleType,
)


@mock_dynamodb
@freezegun.freeze_time("2025-02-01T12:00:00.000+0900")
class TestBatchAutomaticLink:
    def setup_method(self, method):
        MasterSupporterOrganizationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        AdminModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        UserModel.create_table(read_capacity_units=5, write_capacity_units=5, wait=True)
        NotificationModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        ProjectModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )

        MasterSupporterOrganizationModel(
            id="1ed63a11-c0fc-4d78-bd5f-58bb8f23b1d0",
            data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
            name="Ideation Service Team",
            value="IST",
            order=3,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()
        MasterSupporterOrganizationModel(
            id="9129c1e6-03fe-4199-99a5-d8accd7d60b7",
            data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
            name="Acceleration Service Team",
            value="AST",
            order=2,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()
        MasterSupporterOrganizationModel(
            id="d5b60da9-3094-47b9-be7d-6aff0ff6bc3e",
            data_type=MasterDataType.MASTER_SERVICE_TYPE.value,
            name="組織開発",
            value="1",
            order=1,
            use=True,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2022, 4, 23, 3, 21, 39, 356000),
        ).save()

        AdminModel(
            id="98d7327a-3fe4-4cf4-a24c-12c97abb1b13",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"1ed63a11-c0fc-4d78-bd5f-58bb8f23b1d0"},
            organization_name=None,
            cognito_id=None,
            last_login_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()
        AdminModel(
            id="ea2f9eb3-c5f6-4763-bdda-02a4df4a00fd",
            data_type=DataType.ADMIN,
            name="田中次郎",
            email="user@example.com",
            roles={UserRoleType.MAN_HOUR_OPS.key},
            company=None,
            job="部長",
            supporter_organization_id={"1ed63a11-c0fc-4d78-bd5f-58bb8f23b1d0"},
            organization_name=None,
            cognito_id=None,
            last_login_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()
        AdminModel(
            id="40ceda56-6d3d-4c2e-940f-4da15bba1747",
            data_type=DataType.ADMIN,
            name="田中三郎",
            email="user@example.com",
            roles={UserRoleType.SURVEY_OPS.key},
            company=None,
            job="部長",
            supporter_organization_id={"1ed63a11-c0fc-4d78-bd5f-58bb8f23b1d0"},
            organization_name=None,
            cognito_id=None,
            last_login_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            otp_secret=None,
            otp_verified_token=None,
            otp_verified_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()

        UserModel(
            id="59ce2c10-86c0-4809-a80e-f0012c0ea02a",
            data_type=DataType.USER,
            name="佐藤太郎",
            email="user@example.com",
            role=UserRoleType.SUPPORTER.key,
            customer_id="6f1baf31-dd27-458f-8fac-cb36d8ff24b9",
            customer_name="ソニーグループ株式会社",
            job="部長",
            company=None,
            supporter_organization_id=["1ed63a11-c0fc-4d78-bd5f-58bb8f23b1d0"],
            is_input_man_hour=None,
            project_ids=None,
            cognito_id=None,
            agreed=False,
            last_login_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()
        UserModel(
            id="001ee3c4-58ed-4483-bc60-e9fbb7e6d35a",
            data_type=DataType.USER,
            name="佐藤次郎",
            email="user@example.com",
            role=UserRoleType.SUPPORTER.key,
            customer_id="6f1baf31-dd27-458f-8fac-cb36d8ff24b9",
            customer_name="ソニーグループ株式会社",
            job="部長",
            company=None,
            supporter_organization_id=["1ed63a11-c0fc-4d78-bd5f-58bb8f23b1d0"],
            is_input_man_hour=None,
            project_ids=None,
            cognito_id=None,
            agreed=False,
            last_login_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()
        UserModel(
            id="38d05324-be3a-4534-9fed-38bd78e89405",
            data_type=DataType.USER,
            name="佐藤三郎",
            email="user@example.com",
            role=UserRoleType.SUPPORTER_MGR.key,
            customer_id="6f1baf31-dd27-458f-8fac-cb36d8ff24b9",
            customer_name="ソニーグループ株式会社",
            job="部長",
            company=None,
            supporter_organization_id=[
                "1ed63a11-c0fc-4d78-bd5f-58bb8f23b1d0",
                "9129c1e6-03fe-4199-99a5-d8accd7d60b7",
            ],
            is_input_man_hour=None,
            project_ids=None,
            cognito_id=None,
            agreed=False,
            last_login_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()
        UserModel(
            id="b828e53d-3e53-4d4e-96fb-0754c97a7be2",
            data_type=DataType.USER,
            name="鈴木太郎",
            email="user@example.com",
            role=UserRoleType.SALES.key,
            customer_id="6f1baf31-dd27-458f-8fac-cb36d8ff24b9",
            customer_name="ソニーグループ株式会社",
            job="部長",
            company=None,
            supporter_organization_id=None,
            is_input_man_hour=None,
            project_ids={"baf7a172-b005-4b2c-a8c1-f8d6669c2548"},
            cognito_id=None,
            agreed=False,
            last_login_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()
        UserModel(
            id="75524d75-5441-4a93-bdd9-6bafffebb6dd",
            data_type=DataType.USER,
            name="山本太郎",
            email="user@example.com",
            role=UserRoleType.CUSTOMER.key,
            customer_id="6f1baf31-dd27-458f-8fac-cb36d8ff24b9",
            customer_name="ソニーグループ株式会社",
            job="部長",
            company=None,
            supporter_organization_id=None,
            is_input_man_hour=None,
            project_ids={"baf7a172-b005-4b2c-a8c1-f8d6669c2548"},
            cognito_id=None,
            agreed=False,
            last_login_at=None,
            disabled=False,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()

        CustomerModel(
            id="6f1baf31-dd27-458f-8fac-cb36d8ff24b9",
            data_type=DataType.CUSTOMER,
            name="ソニーグループ株式会社",
            category="ソニーグループ",
            salesforce_customer_id="47ee3e76-cc79-4d25-a813-91efd70f84bc",
            salesforce_update_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
            salesforce_target=None,
            salesforce_credit_limit=None,
            salesforce_credit_get_month=None,
            salesforce_credit_manager=None,
            salesforce_credit_no_retry=None,
            salesforce_paws_credit_number=None,
            salesforce_customer_owner=None,
            salesforce_customer_segment=None,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
        ).save()

        ProjectModel(
            id="baf7a172-b005-4b2c-a8c1-f8d6669c2548",
            data_type=DataType.PROJECT,
            salesforce_customer_id="47ee3e76-cc79-4d25-a813-91efd70f84bc",
            salesforce_opportunity_id="577fd5ca-08d9-4d3e-824c-fd04a54e7cdc",
            salesforce_update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
            customer_id="6f1baf31-dd27-458f-8fac-cb36d8ff24b9",
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type="d5b60da9-3094-47b9-be7d-6aff0ff6bc3e",
            create_new=True,
            continued=True,
            main_sales_user_id="b828e53d-3e53-4d4e-96fb-0754c97a7be2",
            contract_date="2021/01/30",
            phase="プラン提示(D)",
            customer_success="DXの実現",
            support_date_from="2021/01/30",
            support_date_to="2021/02/28",
            total_contract_time=200,
            profit=None,
            gross=None,
            main_customer_user_id="75524d75-5441-4a93-bdd9-6bafffebb6dd",
            salesforce_main_customer=None,
            customer_user_ids=None,
            main_supporter_user_id=None,
            supporter_organization_id="1ed63a11-c0fc-4d78-bd5f-58bb8f23b1d0",
            salesforce_main_supporter_user_name=None,
            supporter_user_ids=None,
            salesforce_supporter_user_names=None,
            is_count_man_hour=True,
            is_karte_remind=True,
            is_master_karte_remind=True,
            contract_type="有償",
            is_secret=False,
            create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            create_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
            update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
            update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
        ).save()

    def teardown_method(self, method):
        MasterSupporterOrganizationModel.delete_table()
        AdminModel.delete_table()
        UserModel.delete_table()
        NotificationModel.delete_table()
        CustomerModel.delete_table()
        ProjectModel.delete_table()

    def test_ok(
        self,
        mocker,
    ):
        """BO取引先・案件情報自動連携: 正常"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2025, month=2, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="BO取引先・案件情報自動連携処理",
                value="partnerportal-backoffice-dev-batch_automatic_link",
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

        # ###########################
        # モック化
        # ###########################
        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        # PlatformApiOperatorのコンストラクタ用のモック
        mocker.patch.object(app.utils.platform, "get_secret")
        mocker.patch.object(SigV4Auth, "add_auth")

        json_open = open("mock/pf/get_projects.json", "r")
        json_load = json.load(json_open)
        pf_get_projects_mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        pf_get_projects_mock.return_value = (200, json_load)
        pf_put_project_publication_mock = mocker.patch.object(
            PlatformApiOperator, "put_project_publication"
        )
        pf_put_project_publication_mock.return_value = (200, "")

        expected = "Normal end."
        param = {"stageParams": {"stage": "dev"}}
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
            datetime(year=2025, month=2, day=1, hour=10, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="BO取引先・案件情報自動連携処理",
                value="partnerportal-backoffice-dev-batch_automatic_link",
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

        # ###########################
        # モック化
        # ###########################
        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        # PlatformApiOperatorのコンストラクタ用のモック
        mocker.patch.object(app.utils.platform, "get_secret")
        mocker.patch.object(SigV4Auth, "add_auth")

        pf_get_projects_mock = mocker.patch.object(PlatformApiOperator, "get_projects")
        # エラー(500)
        pf_get_projects_mock.return_value = (500, [])
        pf_put_project_publication_mock = mocker.patch.object(
            PlatformApiOperator, "put_project_publication"
        )
        pf_put_project_publication_mock.return_value = (200, "")

        expected = "Error end."
        param = {"stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected

    def test_ok_skip_end(
        self,
        mocker,
    ):
        """処理途中（スキップ）終了"""
        # mock: 前処理・後処理
        mock_batch_control = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        # 前回実行時刻から15分未経過 -> スキップ
        update_datetime = timezone("Asia/Tokyo").localize(
            datetime(year=2025, month=2, day=1, hour=12, minute=0, second=0)
        )
        mock_batch_control.return_value = [
            MasterBatchControlModel(
                id="1965ab22-7594-41d6-8b61-6bb1601a237e",
                data_type="batch_control",
                name="BO取引先・案件情報自動連携処理",
                value="partnerportal-backoffice-dev-batch_automatic_link",
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

        # ###########################
        # モック化
        # ###########################
        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(SqsHelper, "send_message_batch_to_queue")

        expected = "Skipped processing."
        param = {"stageParams": {"stage": "dev"}}
        actual = handler(param, {})

        assert actual == expected
