from datetime import datetime

import pytest
from fastapi.testclient import TestClient
from moto import mock_dynamodb
from pynamodb.exceptions import PutError
from pynamodb.models import BatchWrite
from pytz import timezone

from app.main import app
from app.models.admin import AdminModel
from app.models.customer import CustomerModel
from app.models.project import ProjectModel
from app.models.user import UserModel
from app.resources.const import DataType, TimezoneType, UserRoleType
from app.service.customer_service import CustomerService
from app.service.notification_service import NotificationService
from app.utils.aws.s3 import S3Helper
from app.utils.aws.sqs import SqsHelper

client = TestClient(app)
jst = timezone(TimezoneType.UTC)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@mock_dynamodb
class TestImportCustomers:
    @pytest.mark.parametrize(
        "role_types",
        [
            [UserRoleType.SYSTEM_ADMIN.key],
            [UserRoleType.SALES.key],
            [UserRoleType.SALES_MGR.key],
            [UserRoleType.SUPPORTER_MGR.key],
            [UserRoleType.SURVEY_OPS.key],
            [UserRoleType.MAN_HOUR_OPS.key],
            [UserRoleType.BUSINESS_MGR.key],
        ]
    )
    def test_access_ok(
        self,
        mock_auth_admin,
        role_types,
    ):
        mock_auth_admin(role_types)
        response = client.post(
            "/api/customers/import", headers=REQUEST_HEADERS
        )

        assert response.status_code == 422

    def setup_method(self, method):
        AdminModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        CustomerModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        ProjectModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        UserModel.create_table(read_capacity_units=5, write_capacity_units=5, wait=True)

        AdminModel(
            id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            data_type=DataType.ADMIN,
            name="田中太郎",
            email="user@example.com",
            roles={UserRoleType.SYSTEM_ADMIN.key},
            company=None,
            job="部長",
            supporter_organization_id={"180a3597-b7e7-42c8-902c-a29016afa662"},
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

        CustomerModel(
            id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.CUSTOMER,
            name="ソニーグループ株式会社",
            category="ソニーグループ",
            salesforce_customer_id="9999a99999RYLbr",
            salesforce_update_at=CustomerService.jst.localize(
                datetime.strptime(
                    "2022/01/16 8:07",
                    "%Y/%m/%d %H:%M",
                )
            ),
            salesforce_target=None,
            salesforce_credit_limit=None,
            salesforce_credit_get_month=None,
            salesforce_credit_manager=None,
            salesforce_credit_no_retry=None,
            salesforce_paws_credit_number=None,
            salesforce_customer_owner=None,
            salesforce_customer_segment=None,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 2, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 4, 21, 39, 356000),
        ).save()

        UserModel(
            id="a9b67094-cdab-494c-818e-d4845088269b",
            data_type=DataType.USER,
            name="田中太郎",
            email="user@example.com",
            role=UserRoleType.CUSTOMER.value[1],
            customer_id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()

        ProjectModel(
            id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            data_type=DataType.PROJECT,
            customer_id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            salesforce_customer_id=None,
            salesforce_opportunity_id=None,
            salesforce_update_at=None,
            name="サンプルプロジェクト",
            customer_name="ソニーグループ株式会社",
            service_type=None,
            create_new=None,
            continued=None,
            main_sales_user_id="c67b0ca2270d",
            contract_date=None,
            phase=None,
            customer_success=None,
            support_date_from="2021/03/01",
            support_date_to="2022/02/28",
            profit=None,
            gross=None,
            total_contract_time=200,
            main_customer_user_id="c67b0ca2270d",
            salesforce_main_customer=None,
            customer_user_ids=None,
            main_supporter_user_id=None,
            supporter_organization_id=None,
            salesforce_main_supporter_user_name=None,
            supporter_user_ids=None,
            salesforce_supporter_user_names=None,
            is_count_man_hour=True,
            is_karte_remind=True,
            contract_type=None,
            is_secret=False,
            salesforce_use_package=None,
            salesforce_via_pr=None,
            update_history=None,
            create_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            create_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
            update_id="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7",
            update_at=datetime(2020, 10, 23, 3, 21, 39, 356000),
        ).save()

    def teardown_method(self, method):
        AdminModel.delete_table()
        CustomerModel.delete_table()
        ProjectModel.delete_table()
        UserModel.delete_table()

    @pytest.mark.parametrize(
        "body, s3_value, expected",
        [
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","","","0","","ソニー 太郎",""\n"9999a99999pSlqQ","2022/05/19 12:23","アアアアア大学","0","0","教育/研究開発機関","","","0","","ソニー 太郎",""',
                {
                    "mode": "check",
                    "result": "ok",
                    "customers": [
                        {
                            "name": "株式会社アアアアア",
                            "category": "ソニーグループ",
                            "salesforceCustomerId": "9999a99999S3XYn",
                            "salesforceUpdateAt": "2022/02/16 8:07",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "90009099",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "情報・通信業",
                            "errorMessage": [],
                        },
                        {
                            "name": "一般社団法人アアアアア",
                            "category": "事業/人材開発機関",
                            "salesforceCustomerId": "9999a99999RYLbr",
                            "salesforceUpdateAt": "2021/12/16 9:28",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": [],
                        },
                        {
                            "name": "アアアアア大学",
                            "category": "教育/研究開発機関",
                            "salesforceCustomerId": "9999a99999pSlqQ",
                            "salesforceUpdateAt": "2022/05/19 12:23",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": [],
                        },
                    ],
                },
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","","","0","","ソニー 太郎",""\n"9999a99999pSlqQ","2022/05/19 12:23","アアアアア大学","0","0","教育/研究開発機関","","","0","","ソニー 太郎",""',
                {
                    "mode": "execute",
                    "result": "done",
                    "customers": [
                        {
                            "name": "株式会社アアアアア",
                            "category": "ソニーグループ",
                            "salesforceCustomerId": "9999a99999S3XYn",
                            "salesforceUpdateAt": "2022/02/16 8:07",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "90009099",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "情報・通信業",
                            "errorMessage": None,
                        },
                        # 更新されないデータ(CSVのSF最終更新日がDBのSF最終更新日より古いため)
                        # {
                        #     "name": "一般社団法人アアアアア",
                        #     "category": "事業/人材開発機関",
                        #     "salesforceCustomerId": "9999a99999RYLbr",
                        #     "salesforceUpdateAt": "2021/12/16 9:28",
                        #     "salesforceTarget": "0",
                        #     "salesforceCreditLimit": "0",
                        #     "salesforceCreditGetMonth": "",
                        #     "salesforceCreditManager": "",
                        #     "salesforceCreditNoRetry": "0",
                        #     "salesforcePawsCreditNumber": "",
                        #     "salesforceCustomerOwner": "ソニー 太郎",
                        #     "salesforceCustomerSegment": "",
                        #     "errorMessage": None,
                        # },
                        {
                            "name": "アアアアア大学",
                            "category": "教育/研究開発機関",
                            "salesforceCustomerId": "9999a99999pSlqQ",
                            "salesforceUpdateAt": "2022/05/19 12:23",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": None,
                        },
                    ],
                },
            ),
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        body,
        s3_value,
        expected,
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_s3 = mocker.patch.object(S3Helper, "get_object_content")
        mock_s3.return_value = s3_value

        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(NotificationService, "save_notification")

        response = client.post(
            "/api/customers/import", json=body, headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "role",
        [
            (UserRoleType.SALES.key),
            (UserRoleType.SUPPORTER_MGR.key),
            (UserRoleType.SALES_MGR.key),
            (UserRoleType.SURVEY_OPS.key),
            (UserRoleType.MAN_HOUR_OPS.key),
        ],
    )
    def test_auth_ok(
        self,
        mocker,
        mock_auth_admin,
        role,
    ):
        """ロール別権限テスト"""
        mock_auth_admin([role])

        s3_value = '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","","","0","","ソニー 太郎",""\n"9999a99999pSlqQ","2022/05/19 12:23","アアアアア大学","0","0","教育/研究開発機関","","","0","","ソニー 太郎",""'

        mock_s3 = mocker.patch.object(S3Helper, "get_object_content")
        mock_s3.return_value = s3_value

        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(NotificationService, "save_notification")

        body = {"file": "取引先情報_テストデータ.csv", "mode": "check"}

        response = client.post(
            "/api/customers/import", json=body, headers=REQUEST_HEADERS
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "body, s3_value",
        [
            ({"file": "取引先情報_テストデータ.csv", "mode": "check"}, ""),
            ({"file": "取引先情報_テストデータ.csv", "mode": "execute"}, ""),
        ],
    )
    def test_ng_400_csv_blank_data(self, mocker, mock_auth_admin, body, s3_value):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_s3 = mocker.patch.object(S3Helper, "get_object_content")
        mock_s3.return_value = s3_value

        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(NotificationService, "save_notification")

        response = client.post(
            "/api/customers/import", json=body, headers=REQUEST_HEADERS
        )

        assert response.status_code == 400

    @pytest.mark.parametrize(
        "body, s3_value",
        [
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"',
            ),
        ],
    )
    def test_ng_400_csv_illegal_header(self, mocker, mock_auth_admin, body, s3_value):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_s3 = mocker.patch.object(S3Helper, "get_object_content")
        mock_s3.return_value = s3_value

        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(NotificationService, "save_notification")

        response = client.post(
            "/api/customers/import", json=body, headers=REQUEST_HEADERS
        )

        assert response.status_code == 400

    @pytest.mark.parametrize(
        "body, s3_value, expected",
        [
            (
                # 取引先ID（必須チェック）
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","","","0","","ソニー 太郎",""',
                {
                    "mode": "check",
                    "result": "ng",
                    "customers": [
                        {
                            "name": "株式会社アアアアア",
                            "category": "ソニーグループ",
                            "salesforceCustomerId": "",
                            "salesforceUpdateAt": "2022/02/16 8:07",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "90009099",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "情報・通信業",
                            "errorMessage": ["取引先 IDを入力してください。"],
                        },
                        {
                            "name": "一般社団法人アアアアア",
                            "category": "事業/人材開発機関",
                            "salesforceCustomerId": "9999a99999RYLbr",
                            "salesforceUpdateAt": "2021/12/16 9:28",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": [],
                        },
                    ],
                },
            ),
            (
                # 最終更新日時（必須チェック、書式チェック）
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021-12-16 09:28:00","一般社団法人アアアアア","0","0","事業/人材開発機関","","","0","","ソニー 太郎",""\n"9999a99999RYLbr","2021/13/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","2022/02/18","","0","","ソニー 太郎",""',
                {
                    "mode": "check",
                    "result": "ng",
                    "customers": [
                        {
                            "name": "株式会社アアアアア",
                            "category": "ソニーグループ",
                            "salesforceCustomerId": "9999a99999S3XYn",
                            "salesforceUpdateAt": "",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "90009099",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "情報・通信業",
                            "errorMessage": [
                                "PP最終更新日時(取引先)を入力してください。",
                                "PP最終更新日時(取引先)は「yyyy/mm/dd h:mm」形式で入力してください。",
                                "PP最終更新日時(取引先)には正しい日時を入力してください。",
                            ],
                        },
                        {
                            "name": "一般社団法人アアアアア",
                            "category": "事業/人材開発機関",
                            "salesforceCustomerId": "9999a99999RYLbr",
                            "salesforceUpdateAt": "2021-12-16 09:28:00",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": [
                                "PP最終更新日時(取引先)は「yyyy/mm/dd h:mm」形式で入力してください。",
                                "PP最終更新日時(取引先)には正しい日時を入力してください。",
                            ],
                        },
                        {
                            "name": "一般社団法人アアアアア",
                            "category": "事業/人材開発機関",
                            "salesforceCustomerId": "9999a99999RYLbr",
                            "salesforceUpdateAt": "2021/13/16 9:28",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "2022/02/18",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": ["PP最終更新日時(取引先)には正しい日時を入力してください。"],
                        },
                    ],
                },
            ),
            (
                # 取引先名（必須チェック）
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","","","0","","ソニー 太郎",""',
                {
                    "mode": "check",
                    "result": "ng",
                    "customers": [
                        {
                            "name": "",
                            "category": "ソニーグループ",
                            "salesforceCustomerId": "9999a99999S3XYn",
                            "salesforceUpdateAt": "2022/02/16 8:07",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "90009099",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "情報・通信業",
                            "errorMessage": ["取引先名を入力してください。"],
                        },
                        {
                            "name": "一般社団法人アアアアア",
                            "category": "事業/人材開発機関",
                            "salesforceCustomerId": "9999a99999RYLbr",
                            "salesforceUpdateAt": "2021/12/16 9:28",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": [],
                        },
                    ],
                },
            ),
            (
                # 戦略ターゲット・コアターゲット（真理値変換可否チェック）
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","A","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","","","0","","ソニー 太郎",""',
                {
                    "mode": "check",
                    "result": "ng",
                    "customers": [
                        {
                            "name": "株式会社アアアアア",
                            "category": "ソニーグループ",
                            "salesforceCustomerId": "9999a99999S3XYn",
                            "salesforceUpdateAt": "2022/02/16 8:07",
                            "salesforceTarget": "A",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "90009099",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "情報・通信業",
                            "errorMessage": ["戦略ターゲット・コアターゲットは真理値（1または0）を入力してください。"],
                        },
                        {
                            "name": "一般社団法人アアアアア",
                            "category": "事業/人材開発機関",
                            "salesforceCustomerId": "9999a99999RYLbr",
                            "salesforceUpdateAt": "2021/12/16 9:28",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": [],
                        },
                    ],
                },
            ),
            (
                # 月与信上限（数値チェック）
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","A","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","","","0","","ソニー 太郎",""',
                {
                    "mode": "check",
                    "result": "ng",
                    "customers": [
                        {
                            "name": "株式会社アアアアア",
                            "category": "ソニーグループ",
                            "salesforceCustomerId": "9999a99999S3XYn",
                            "salesforceUpdateAt": "2022/02/16 8:07",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "A",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "90009099",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "情報・通信業",
                            "errorMessage": ["月与信上限は数値を入力してください。"],
                        },
                        {
                            "name": "一般社団法人アアアアア",
                            "category": "事業/人材開発機関",
                            "salesforceCustomerId": "9999a99999RYLbr",
                            "salesforceUpdateAt": "2021/12/16 9:28",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": [],
                        },
                    ],
                },
            ),
            (
                # 与信決裁取得年月（書式チェック）
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","2021-01-01","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","2022/13/18","","0","","ソニー 太郎",""',
                {
                    "mode": "check",
                    "result": "ng",
                    "customers": [
                        {
                            "name": "株式会社アアアアア",
                            "category": "ソニーグループ",
                            "salesforceCustomerId": "9999a99999S3XYn",
                            "salesforceUpdateAt": "2022/02/16 8:07",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "2021-01-01",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "90009099",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "情報・通信業",
                            "errorMessage": [
                                "与信決裁取得年月は「yyyy/mm/dd」形式で入力してください。",
                                "与信決裁取得年月には正しい日付を入力してください。",
                            ],
                        },
                        {
                            "name": "一般社団法人アアアアア",
                            "category": "事業/人材開発機関",
                            "salesforceCustomerId": "9999a99999RYLbr",
                            "salesforceUpdateAt": "2021/12/16 9:28",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "2022/13/18",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": ["与信決裁取得年月には正しい日付を入力してください。"],
                        },
                    ],
                },
            ),
            (
                # 与信再取得不要（真理値変換可否チェック）
                {"file": "取引先情報_テストデータ.csv", "mode": "check"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","2021/01/01","","A","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","2021/01/01","","0","","ソニー 太郎",""',
                {
                    "mode": "check",
                    "result": "ng",
                    "customers": [
                        {
                            "name": "株式会社アアアアア",
                            "category": "ソニーグループ",
                            "salesforceCustomerId": "9999a99999S3XYn",
                            "salesforceUpdateAt": "2022/02/16 8:07",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "2021/01/01",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "A",
                            "salesforcePawsCreditNumber": "90009099",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "情報・通信業",
                            "errorMessage": ["与信再取得不要は真理値（1または0）を入力してください。"],
                        },
                        {
                            "name": "一般社団法人アアアアア",
                            "category": "事業/人材開発機関",
                            "salesforceCustomerId": "9999a99999RYLbr",
                            "salesforceUpdateAt": "2021/12/16 9:28",
                            "salesforceTarget": "0",
                            "salesforceCreditLimit": "0",
                            "salesforceCreditGetMonth": "2021/01/01",
                            "salesforceCreditManager": "",
                            "salesforceCreditNoRetry": "0",
                            "salesforcePawsCreditNumber": "",
                            "salesforceCustomerOwner": "ソニー 太郎",
                            "salesforceCustomerSegment": "",
                            "errorMessage": [],
                        },
                    ],
                },
            ),
        ],
    )
    def test_check_ng(
        self,
        mocker,
        mock_auth_admin,
        body,
        s3_value,
        expected,
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_s3 = mocker.patch.object(S3Helper, "get_object_content")
        mock_s3.return_value = s3_value

        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(NotificationService, "save_notification")

        response = client.post(
            "/api/customers/import", json=body, headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "body, s3_value, expected",
        [
            (
                {"file": "取引先情報_テストデータ.csv", "mode": "execute"},
                '"取引先 ID","PP最終更新日時(取引先)","取引先名","戦略ターゲット・コアターゲット","月与信上限","カテゴリ","与信決裁取得年月","与信取得担当者","与信再取得不要","PAWS決裁番号(与信)","取引先 所有者","業界セグメント"\n"9999a99999S3XYn","2022/02/16 8:07","株式会社アアアアア","0","0","ソニーグループ","","","0","90009099","ソニー 太郎","情報・通信業"\n"9999a99999RYLbr","2021/12/16 9:28","一般社団法人アアアアア","0","0","事業/人材開発機関","","","0","","ソニー 太郎",""\n"9999a99999pSlqQ","2022/05/19 12:23","アアアアア大学","0","0","教育/研究開発機関","","","0","","ソニー 太郎",""',
                {
                    "mode": "execute",
                    "result": "error",
                    "customers": [],
                },
            ),
        ],
    )
    def test_execute_error(
        self,
        mocker,
        mock_auth_admin,
        body,
        s3_value,
        expected,
    ):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_s3 = mocker.patch.object(S3Helper, "get_object_content")
        mock_s3.return_value = s3_value

        mock_batch_write = mocker.patch.object(BatchWrite, "save")
        mock_batch_write.side_effect = PutError()

        mocker.patch.object(SqsHelper, "send_message_to_queue")
        mocker.patch.object(NotificationService, "save_notification")

        response = client.post(
            "/api/customers/import", json=body, headers=REQUEST_HEADERS
        )

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
