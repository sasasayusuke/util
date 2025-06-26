import freezegun
import pytest
from fastapi.testclient import TestClient
from pynamodb.exceptions import DoesNotExist

from app.main import app
from app.models.project import (
    GrossProfitAttribute,
    ProjectModel,
    SalesforceMainCustomerAttribute,
)
from app.models.project_karte import ProjectKarteModel
from app.resources.const import DataType, UserRoleType
from app.schemas.karten import DeliverablesInfo, DocumentInfo

client = TestClient(app)

REQUEST_HEADERS = {"x-otp-verified-token": "111111"}


@freezegun.freeze_time("2022-07-01T22:42:05.033Z")
class TestSupportSchedulesById:
    @pytest.mark.parametrize(
        "karte_id, version, body, karte_model, expected",
        [
            (
                "99c5bc68-246f-4450-8a50-2f23f9518025",
                1,
                {
                    "supportDate": "2022/08/18",
                    "supportStartTime": "10:00",
                    "supportEndTime": "17:00",
                },
                ProjectKarteModel(
                    karte_id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
                    project_id="2854fc20-caea-44bb-ada5",
                    date="2022/08/31",
                    start_datetime="2022/08/31 13:00",
                    start_time="13:00",
                    end_time="14:00",
                    supporter_ids=[
                        "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                        "b9b67094-cdab-494c-818e-d4845088269b",
                    ],
                    draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    last_update_datetime="2022-7-12T02:21:39.356000+0000",
                    customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
                    customer_user_ids=[
                        "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                        "8a990e25-43da-49a3-ae76-863b5219fe6a",
                    ],
                    man_hour=5,
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[
                        DocumentInfo(
                            fileName="document",
                            path="path/aaa/bbb...",
                        )
                    ],
                    deliverables=[
                        DeliverablesInfo(
                            fileName="deliverables",
                            path="path/aaa/ccc...",
                        )
                    ],
                    memo="memo",
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-7-12T02:21:39.356000+0000",
                    version=1,
                ),
                {"message": "OK"},
            ),
        ],
    )
    def test_ok(
        self,
        mocker,
        mock_auth_admin,
        karte_id,
        version,
        body,
        karte_model,
        expected,
    ):
        """正常系のテスト"""
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = ProjectModel(
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
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
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
        )

        mock_karte = mocker.patch.object(ProjectKarteModel, "get")
        mock_karte.return_value = karte_model
        mocker.patch.object(ProjectKarteModel, "update")

        response = client.put(
            f"/api/schedules/support?karteId={karte_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    def test_schedules_not_found(
        self,
        mocker,
        mock_auth_admin,
    ):
        """案件スケジュールが存在しない(該当のproject_idが存在しない)時のテスト"""
        karte_id = "99c5bc68-246f-4450-8a50-2f23f9518025"
        version = 1
        body = {
            "supportDate": "2022/08/18",
            "supportStartTime": "10:00",
            "supportEndTime": "17:00",
        }

        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])
        mock = mocker.patch.object(ProjectKarteModel, "get")
        mock.side_effect = DoesNotExist()

        response = client.put(
            f"/api/schedules/support?karteId={karte_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        actual = response.json()
        assert response.status_code == 404
        assert actual["detail"] == "Not Found"

    @pytest.mark.parametrize(
        "role",
        [
            (UserRoleType.SYSTEM_ADMIN.key),
            (UserRoleType.SALES.key),
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
        """ロール別権限テスト：制限なし"""
        mock_auth_admin([role])

        karte_id = "99c5bc68-246f-4450-8a50-2f23f9518025"
        version = 1
        body = {
            "supportDate": "2022/08/18",
            "supportStartTime": "10:00",
            "supportEndTime": "17:00",
        }

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = ProjectModel(
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
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="c9b67094-cdab-494c-818e-d4845088269b",
            supporter_organization_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            salesforce_main_supporter_user_name="山田太郎",
            supporter_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
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
        )

        mock_karte = mocker.patch.object(ProjectKarteModel, "get")
        mock_karte.return_value = ProjectKarteModel(
            karte_id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
            project_id="2854fc20-caea-44bb-ada5",
            date="2022/08/31",
            start_datetime="2022/08/31 13:00",
            start_time="13:00",
            end_time="14:00",
            supporter_ids=[
                "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                "b9b67094-cdab-494c-818e-d4845088269b",
            ],
            draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            last_update_datetime="2022-7-12T02:21:39.356000+0000",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            customer_user_ids=[
                "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                "8a990e25-43da-49a3-ae76-863b5219fe6a",
            ],
            man_hour=5,
            detail="detail",
            feedback="feedback",
            homework="homework",
            documents=[
                DocumentInfo(
                    fileName="document",
                    path="path/aaa/bbb...",
                )
            ],
            deliverables=[
                DeliverablesInfo(
                    fileName="deliverables",
                    path="path/aaa/ccc...",
                )
            ],
            memo="memo",
            task="task",
            is_draft=True,
            update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            update_at="2022-7-12T02:21:39.356000+0000",
            version=1,
        )
        mocker.patch.object(ProjectKarteModel, "update")

        response = client.put(
            f"/api/schedules/support?karteId={karte_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "project_model",
        [
            (
                # 自身の課の案件、公開案件
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
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
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
                )
            ),
            (
                # 自身の課の案件、非公開案件
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
                    supporter_organization_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                )
            ),
            (
                # 自身の課以外の案件、公開案件
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
                )
            ),
        ],
    )
    def test_auth_supporter_mgr_ok(
        self,
        mocker,
        mock_auth_admin,
        project_model,
    ):
        """権限確認：支援者責任者、OKパターン"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        karte_id = "99c5bc68-246f-4450-8a50-2f23f9518025"
        version = 1
        body = {
            "supportDate": "2022/08/18",
            "supportStartTime": "10:00",
            "supportEndTime": "17:00",
        }

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mock_karte = mocker.patch.object(ProjectKarteModel, "get")
        mock_karte.return_value = ProjectKarteModel(
            karte_id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
            project_id="2854fc20-caea-44bb-ada5",
            date="2022/08/31",
            start_datetime="2022/08/31 13:00",
            start_time="13:00",
            end_time="14:00",
            supporter_ids=[
                "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                "b9b67094-cdab-494c-818e-d4845088269b",
            ],
            draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            last_update_datetime="2022-7-12T02:21:39.356000+0000",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            customer_user_ids=[
                "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                "8a990e25-43da-49a3-ae76-863b5219fe6a",
            ],
            man_hour=5,
            detail="detail",
            feedback="feedback",
            homework="homework",
            documents=[
                DocumentInfo(
                    fileName="document",
                    path="path/aaa/bbb...",
                )
            ],
            deliverables=[
                DeliverablesInfo(
                    fileName="deliverables",
                    path="path/aaa/ccc...",
                )
            ],
            memo="memo",
            task="task",
            is_draft=True,
            update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            update_at="2022-7-12T02:21:39.356000+0000",
            version=1,
        )
        mocker.patch.object(ProjectKarteModel, "update")

        response = client.put(
            f"/api/schedules/support?karteId={karte_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 200

    @pytest.mark.parametrize(
        "project_model",
        [
            (
                # 自身の課以外の案件、非公開案件
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
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                )
            ),
            (
                # 自身の案件でない（支援者組織が未設定）、非公開案件
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
                    supporter_organization_id="",
                    salesforce_main_supporter_user_name="山田太郎",
                    supporter_user_ids=set(
                        list(["c9b67094-cdab-494c-818e-d4845088269b"])
                    ),
                    salesforce_supporter_user_names=["山田太郎", "山田次郎"],
                    is_count_man_hour=True,
                    is_karte_remind=True,
                    contract_type="有償",
                    is_secret=True,
                    create_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    create_at="2022-05-23T16:34:21.523000+0000",
                    update_id="998c3cd7-63b5-453f-9de2-5af21d565b98",
                    update_at="2022-05-23T16:34:21.523000+0000",
                    version=1,
                )
            ),
        ],
    )
    def test_auth_supporter_mgr_403(
        self,
        mocker,
        mock_auth_admin,
        project_model,
    ):
        """権限確認：支援者責任者、403エラー"""
        mock_auth_admin([UserRoleType.SUPPORTER_MGR.key])

        karte_id = "99c5bc68-246f-4450-8a50-2f23f9518025"
        version = 1
        body = {
            "supportDate": "2022/08/18",
            "supportStartTime": "10:00",
            "supportEndTime": "17:00",
        }

        mock_project = mocker.patch.object(ProjectModel, "get")
        mock_project.return_value = project_model

        mock_karte = mocker.patch.object(ProjectKarteModel, "get")
        mock_karte.return_value = ProjectKarteModel(
            karte_id="9904d11d-b53e-4aee-acc9-a08e1daab69d",
            project_id="2854fc20-caea-44bb-ada5",
            date="2022/08/31",
            start_datetime="2022/08/31 13:00",
            start_time="13:00",
            end_time="14:00",
            supporter_ids=[
                "d9f416c2-a938-46b5-a1de-e7c74bbd18fa",
                "b9b67094-cdab-494c-818e-d4845088269b",
            ],
            draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            last_update_datetime="2022-7-12T02:21:39.356000+0000",
            customer_id="3c648558-c450-42d7-9c4b-62e0f22dc0ff",
            customer_user_ids=[
                "7bf1b7e4-5625-4361-be0a-65f0e49829ea",
                "8a990e25-43da-49a3-ae76-863b5219fe6a",
            ],
            man_hour=5,
            detail="detail",
            feedback="feedback",
            homework="homework",
            documents=[
                DocumentInfo(
                    fileName="document",
                    path="path/aaa/bbb...",
                )
            ],
            deliverables=[
                DeliverablesInfo(
                    fileName="deliverables",
                    path="path/aaa/ccc...",
                )
            ],
            memo="memo",
            task="task",
            is_draft=True,
            update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            update_at="2022-7-12T02:21:39.356000+0000",
            version=1,
        )
        mocker.patch.object(ProjectKarteModel, "update")

        response = client.put(
            f"/api/schedules/support?karteId={karte_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 403

    @pytest.mark.parametrize(
        "body",
        [
            #####################
            # supportDate
            #####################
            # 必須チェック
            {
                # "supportDate": "2022/08/18",
                "supportStartTime": "10:00",
                "supportEndTime": "17:00",
            },
            # Noneチェック
            {
                "supportDate": None,
                "supportStartTime": "10:00",
                "supportEndTime": "17:00",
            },
            #####################
            # supportStartTime
            #####################
            # 必須チェック
            {
                "supportDate": "2022/08/18",
                # "supportStartTime": "10:00",
                "supportEndTime": "17:00",
            },
            # Noneチェック
            {
                "supportDate": "2022/08/18",
                "supportStartTime": None,
                "supportEndTime": "17:00",
            },
            #####################
            # supportEndTime
            #####################
            # 必須チェック
            {
                "supportDate": "2022/08/18",
                "supportStartTime": "10:00",
                # "supportEndTime": "17:00",
            },
            # Noneチェック
            {
                "supportDate": "2022/08/18",
                "supportStartTime": "10:00",
                "supportEndTime": None,
            },
        ],
    )
    def test_validation(self, mock_auth_admin, body):
        mock_auth_admin([UserRoleType.SYSTEM_ADMIN.key])

        karte_id = "12cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        response = client.put(
            f"/api/schedules/support?karteId={karte_id}&version={version}",
            json=body,
            headers=REQUEST_HEADERS,
        )

        assert response.status_code == 422
