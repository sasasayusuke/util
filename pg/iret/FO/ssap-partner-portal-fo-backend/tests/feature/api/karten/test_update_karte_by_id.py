import json

import pytest
from fastapi.testclient import TestClient
from pynamodb.models import BatchWrite

from app.main import app
from app.models.master import MasterSupporterOrganizationModel, MasterUpdateKarteRecord
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.user import UserModel
from app.resources.const import DataType, MasterDataType
from app.service.karten_service import KartenService
from app.utils.platform import PlatformApiOperator

client = TestClient(app)


class TestUpdateKarteById:
    @pytest.mark.parametrize(
        "karte_id, version, model, body,"
        + "project_customer_name, platform_get_projects_status, is_empty_response_of_platform_get_projects,"
        + "platform_get_project_by_id_status, platform_category, platform_department_name, expected",
        [
            (
                "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                2,
                ProjectKarteModel(
                    karte_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    project_id="2854fc20-caea-44bb-ada5",
                    date="2022/08/31",
                    start_datetime="2022/08/31 13:00",
                    start_time="13:00",
                    end_time="14:00",
                    draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    last_update_datetime="2022-7-T02:21:39.356000+0000",
                    karte_notify_last_update_datetime="2022-7-T02:21:39.356000+0000",
                    customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    start_support_actual_time="13:00",
                    end_support_actual_time="14:00",
                    man_hour=5,
                    customers="田中様、加藤様",
                    support_team="山田、佐藤",
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[],
                    deliverables=[],
                    memo="memo",
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-7-12T:21:39.356000+0000",
                    version=2,
                ),
                {
                    "startSupportActualTime": "13:00",
                    "endSupportActualTime": "14:00",
                    "manHour": 0,
                    "customers": "田中様、加藤様",
                    "supportTeam": "山田、佐藤",
                    "detail": "detail",
                    "feedback": "feedback",
                    "homework": "homework",
                    "documents": [{"fileName": "fileName1", "path": "path1"}],
                    "deliverables": [{"fileName": "fileName2", "path": "path2"}],
                    "memo": "memo",
                    "humanResourceNeededForCustomer": "テストお客様に不足している人的リソース",
                    "companyAndIndustryRecommendedToCustomer": "テストお客様に紹介したい企業や産業",
                    "humanResourceNeededForSupport": "テスト支援に不足している人的リソース",
                    "task": "task",
                    "isDraft": True,
                    "isNotifyUpdateKarte": True,
                    "karteId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    "projectId": "2854fc20-caea-44bb-ada5",
                    "date": "2022/08/31",
                    "startDatetime": "2022/08/31 13:00",
                    "startTime": "13:00",
                    "endTime": "14:00",
                    "draftSupporterId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "draftSupporterName": "テストユーザー",
                    "lastUpdateDatetime": "2022-7-T02:21:39.356000+0000",
                    "updateId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "updateUserName": "テストユーザー",
                    "updateAt": "2022-7-12T:21:39.356000+0000",
                    "version": 2,
                    "location": {"type": 'online', "detail": ''}
                },
                "ソニーグループ株式会社１",
                200,
                False,
                200,
                # 顧客セグメントが「ソニーグループ」: メール本文に部署名が表示される
                "ソニーグループ",
                "テスト部署",
                {"message": "OK"},
            ),
            (
                "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                2,
                ProjectKarteModel(
                    karte_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    project_id="2854fc20-caea-44bb-ada5",
                    date="2022/08/31",
                    start_datetime="2022/08/31 13:00",
                    start_time="13:00",
                    end_time="14:00",
                    draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    last_update_datetime="2022-7-T02:21:39.356000+0000",
                    karte_notify_last_update_datetime="2022-7-T02:21:39.356000+0000",
                    customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    start_support_actual_time="13:00",
                    end_support_actual_time="14:00",
                    man_hour=5,
                    customers="田中様、加藤様",
                    support_team="山田、佐藤",
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[],
                    deliverables=[],
                    memo="memo",
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-7-12T:21:39.356000+0000",
                    version=2,
                ),
                {
                    "startSupportActualTime": "13:00",
                    "endSupportActualTime": "14:00",
                    "manHour": 0,
                    "customers": "田中様、加藤様",
                    "supportTeam": "山田、佐藤",
                    "detail": "detail",
                    "feedback": "feedback",
                    "homework": "homework",
                    "documents": [{"fileName": "fileName1", "path": "path1"}],
                    "deliverables": [{"fileName": "fileName2", "path": "path2"}],
                    "memo": "memo",
                    "humanResourceNeededForCustomer": "テストお客様に不足している人的リソース",
                    "companyAndIndustryRecommendedToCustomer": "テストお客様に紹介したい企業や産業",
                    "humanResourceNeededForSupport": "テスト支援に不足している人的リソース",
                    "task": "task",
                    "isDraft": True,
                    "isNotifyUpdateKarte": True,
                    "karteId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    "projectId": "2854fc20-caea-44bb-ada5",
                    "date": "2022/08/31",
                    "startDatetime": "2022/08/31 13:00",
                    "startTime": "13:00",
                    "endTime": "14:00",
                    "draftSupporterId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "draftSupporterName": "テストユーザー",
                    "lastUpdateDatetime": "2022-7-T02:21:39.356000+0000",
                    "updateId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "updateUserName": "テストユーザー",
                    "updateAt": "2022-7-12T:21:39.356000+0000",
                    "version": 2,
                    "location": {"type": 'other', "detail": 'その他'}
                },
                "ソニーグループ株式会社２",
                200,
                False,
                200,
                # 顧客セグメントが「ソニーグループ」以外: メール本文に部署名は表示されない
                "大企業内新規事業",
                "テスト部署",
                {"message": "OK"},
            ),
            (
                "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                2,
                ProjectKarteModel(
                    karte_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    project_id="2854fc20-caea-44bb-ada5",
                    date="2022/08/31",
                    start_datetime="2022/08/31 13:00",
                    start_time="13:00",
                    end_time="14:00",
                    draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    last_update_datetime="2022-7-T02:21:39.356000+0000",
                    karte_notify_last_update_datetime="2022-7-T02:21:39.356000+0000",
                    customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    start_support_actual_time="13:00",
                    end_support_actual_time="14:00",
                    man_hour=5,
                    customers="田中様、加藤様",
                    support_team="山田、佐藤",
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[],
                    deliverables=[],
                    memo="memo",
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-7-12T:21:39.356000+0000",
                    version=2,
                ),
                {
                    "startSupportActualTime": "13:00",
                    "endSupportActualTime": "14:00",
                    "manHour": 0,
                    "customers": "田中様、加藤様",
                    "supportTeam": "山田、佐藤",
                    "detail": "detail",
                    "feedback": "feedback",
                    "homework": "homework",
                    "documents": [{"fileName": "fileName1", "path": "path1"}],
                    "deliverables": [{"fileName": "fileName2", "path": "path2"}],
                    "memo": "memo",
                    "humanResourceNeededForCustomer": "テストお客様に不足している人的リソース",
                    "companyAndIndustryRecommendedToCustomer": "テストお客様に紹介したい企業や産業",
                    "humanResourceNeededForSupport": "テスト支援に不足している人的リソース",
                    "task": "task",
                    "isDraft": True,
                    "isNotifyUpdateKarte": True,
                    "karteId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    "projectId": "2854fc20-caea-44bb-ada5",
                    "date": "2022/08/31",
                    "startDatetime": "2022/08/31 13:00",
                    "startTime": "13:00",
                    "endTime": "14:00",
                    "draftSupporterId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "draftSupporterName": "テストユーザー",
                    "lastUpdateDatetime": "2022-7-T02:21:39.356000+0000",
                    "updateId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "updateUserName": "テストユーザー",
                    "updateAt": "2022-7-12T:21:39.356000+0000",
                    "version": 2,
                },
                "ソニーグループ株式会社３",
                200,
                False,
                200,
                "ソニーグループ",
                # 部署名の設定なし: メール本文に部署名は表示されない
                "",
                {"message": "OK"},
            ),
            (
                "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                2,
                ProjectKarteModel(
                    karte_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    project_id="2854fc20-caea-44bb-ada5",
                    date="2022/08/31",
                    start_datetime="2022/08/31 13:00",
                    start_time="13:00",
                    end_time="14:00",
                    draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    last_update_datetime="2022-7-T02:21:39.356000+0000",
                    karte_notify_last_update_datetime="2022-7-T02:21:39.356000+0000",
                    customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    start_support_actual_time="13:00",
                    end_support_actual_time="14:00",
                    man_hour=5,
                    customers="田中様、加藤様",
                    support_team="山田、佐藤",
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[],
                    deliverables=[],
                    memo="memo",
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-7-12T:21:39.356000+0000",
                    version=2,
                ),
                {
                    "startSupportActualTime": "13:00",
                    "endSupportActualTime": "14:00",
                    "manHour": 0,
                    "customers": "田中様、加藤様",
                    "supportTeam": "山田、佐藤",
                    "detail": "detail",
                    "feedback": "feedback",
                    "homework": "homework",
                    "documents": [{"fileName": "fileName1", "path": "path1"}],
                    "deliverables": [{"fileName": "fileName2", "path": "path2"}],
                    "memo": "memo",
                    "humanResourceNeededForCustomer": "テストお客様に不足している人的リソース",
                    "companyAndIndustryRecommendedToCustomer": "テストお客様に紹介したい企業や産業",
                    "humanResourceNeededForSupport": "テスト支援に不足している人的リソース",
                    "task": "task",
                    "isDraft": True,
                    "isNotifyUpdateKarte": True,
                    "karteId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    "projectId": "2854fc20-caea-44bb-ada5",
                    "date": "2022/08/31",
                    "startDatetime": "2022/08/31 13:00",
                    "startTime": "13:00",
                    "endTime": "14:00",
                    "draftSupporterId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "draftSupporterName": "テストユーザー",
                    "lastUpdateDatetime": "2022-7-T02:21:39.356000+0000",
                    "updateId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "updateUserName": "テストユーザー",
                    "updateAt": "2022-7-12T:21:39.356000+0000",
                    "version": 2,
                },
                "ソニーグループ株式会社４",
                # PF案件一覧取得エラー: メール本文に部署名は表示されない
                400,
                False,
                200,
                "ソニーグループ",
                "テスト部署",
                {"message": "OK"},
            ),
            (
                "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                2,
                ProjectKarteModel(
                    karte_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    project_id="2854fc20-caea-44bb-ada5",
                    date="2022/08/31",
                    start_datetime="2022/08/31 13:00",
                    start_time="13:00",
                    end_time="14:00",
                    draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    last_update_datetime="2022-7-T02:21:39.356000+0000",
                    karte_notify_last_update_datetime="2022-7-T02:21:39.356000+0000",
                    customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    start_support_actual_time="13:00",
                    end_support_actual_time="14:00",
                    man_hour=5,
                    customers="田中様、加藤様",
                    support_team="山田、佐藤",
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[],
                    deliverables=[],
                    memo="memo",
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-7-12T:21:39.356000+0000",
                    version=2,
                ),
                {
                    "startSupportActualTime": "13:00",
                    "endSupportActualTime": "14:00",
                    "manHour": 0,
                    "customers": "田中様、加藤様",
                    "supportTeam": "山田、佐藤",
                    "detail": "detail",
                    "feedback": "feedback",
                    "homework": "homework",
                    "documents": [{"fileName": "fileName1", "path": "path1"}],
                    "deliverables": [{"fileName": "fileName2", "path": "path2"}],
                    "memo": "memo",
                    "humanResourceNeededForCustomer": "テストお客様に不足している人的リソース",
                    "companyAndIndustryRecommendedToCustomer": "テストお客様に紹介したい企業や産業",
                    "humanResourceNeededForSupport": "テスト支援に不足している人的リソース",
                    "task": "task",
                    "isDraft": True,
                    "isNotifyUpdateKarte": True,
                    "karteId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    "projectId": "2854fc20-caea-44bb-ada5",
                    "date": "2022/08/31",
                    "startDatetime": "2022/08/31 13:00",
                    "startTime": "13:00",
                    "endTime": "14:00",
                    "draftSupporterId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "draftSupporterName": "テストユーザー",
                    "lastUpdateDatetime": "2022-7-T02:21:39.356000+0000",
                    "updateId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "updateUserName": "テストユーザー",
                    "updateAt": "2022-7-12T:21:39.356000+0000",
                    "version": 2,
                },
                "ソニーグループ株式会社５",
                200,
                # PF案件一覧取得が0件（レスポンスのprojectsが空リスト）: メール本文に部署名は表示されない
                True,
                200,
                "ソニーグループ",
                "テスト部署",
                {"message": "OK"},
            ),
            (
                "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                2,
                ProjectKarteModel(
                    karte_id="8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    project_id="2854fc20-caea-44bb-ada5",
                    date="2022/08/31",
                    start_datetime="2022/08/31 13:00",
                    start_time="13:00",
                    end_time="14:00",
                    draft_supporter_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    last_update_datetime="2022-7-T02:21:39.356000+0000",
                    karte_notify_last_update_datetime="2022-7-T02:21:39.356000+0000",
                    customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
                    start_support_actual_time="13:00",
                    end_support_actual_time="14:00",
                    man_hour=5,
                    customers="田中様、加藤様",
                    support_team="山田、佐藤",
                    detail="detail",
                    feedback="feedback",
                    homework="homework",
                    documents=[],
                    deliverables=[],
                    memo="memo",
                    task="task",
                    is_draft=True,
                    update_id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    update_at="2022-7-12T:21:39.356000+0000",
                    version=2,
                ),
                {
                    "startSupportActualTime": "13:00",
                    "endSupportActualTime": "14:00",
                    "manHour": 0,
                    "customers": "田中様、加藤様",
                    "supportTeam": "山田、佐藤",
                    "detail": "detail",
                    "feedback": "feedback",
                    "homework": "homework",
                    "documents": [{"fileName": "fileName1", "path": "path1"}],
                    "deliverables": [{"fileName": "fileName2", "path": "path2"}],
                    "memo": "memo",
                    "humanResourceNeededForCustomer": "テストお客様に不足している人的リソース",
                    "companyAndIndustryRecommendedToCustomer": "テストお客様に紹介したい企業や産業",
                    "humanResourceNeededForSupport": "テスト支援に不足している人的リソース",
                    "task": "task",
                    "isDraft": True,
                    "isNotifyUpdateKarte": True,
                    "karteId": "8dbeeeb9-8096-4489-b497-6e33e4dabb1d",
                    "projectId": "2854fc20-caea-44bb-ada5",
                    "date": "2022/08/31",
                    "startDatetime": "2022/08/31 13:00",
                    "startTime": "13:00",
                    "endTime": "14:00",
                    "draftSupporterId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "draftSupporterName": "テストユーザー",
                    "lastUpdateDatetime": "2022-7-T02:21:39.356000+0000",
                    "updateId": "48501bd7-4c28-40bc-8251-3aecbf4a70ca",
                    "updateUserName": "テストユーザー",
                    "updateAt": "2022-7-12T:21:39.356000+0000",
                    "version": 2,
                },
                "ソニーグループ株式会社６",
                200,
                False,
                # PF案件詳細エラー: メール本文に部署名は表示されない
                400,
                "ソニーグループ",
                "テスト部署",
                {"message": "OK"},
            ),
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(
        self,
        mocker,
        model,
        karte_id,
        version,
        body,
        project_customer_name,
        platform_get_projects_status,
        is_empty_response_of_platform_get_projects,
        platform_get_project_by_id_status,
        platform_category,
        platform_department_name,
        expected,
    ):
        """正常系のテスト"""
        mock = mocker.patch.object(ProjectKarteModel, "get")
        mock.return_value = model

        mocker.patch.object(ProjectKarteModel, "update")

        mock_user = mocker.patch.object(UserModel, "get")
        mock_user.return_value = UserModel(
            id="48501bd7-4c28-40bc-8251-3aecbf4a70ca",
            name="テストユーザー",
            email="sato@example.com",
            role="customer",
            customer_id="98cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
            customer_name="〇〇株式会社",
            job="部長",
            company=None,
            supporter_organization_id=[],
            is_input_man_hour=None,
            project_ids=[],
            agreed=True,
            last_login_at="2022-04-25T03:21:39.356Z",
            disabled=False,
        )

        # メソッド
        mock_project_get = mocker.patch.object(ProjectModel, "get")
        mock_project_get.return_value = ProjectModel(
            id="2854fc20-caea-44bb-ada5",
            data_type=DataType.PROJECT,
            salesforce_customer_id="c9b67094-cdab-494c-818e-d4845088269b",
            salesforce_opportunity_id="a03d1661-cdab-494c-818e-d4845088269b",
            salesforce_update_at="2020/10/23 03:21",
            customer_id="106a3144-9650-4a34-8a23-3b02f3b9aeac",
            name="サンプルプロジェクト",
            customer_name=project_customer_name,
            service_type="7ac8bddf-88da-46c9-a504-a03d1661ad58",
            create_new=True,
            continued=True,
            main_sales_user_id="a9b67094-cdab-494c-818e-d4845088269b",
            contract_date="2021/01/30",
            phase="プラン提示(D)",
            customer_success="DXの実現",
            support_date_from="2021/01/30",
            support_date_to="2021/02/28",
            profit=[],
            total_contract_time=200,
            main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
            salesforce_main_customer={},
            customer_user_ids=set(list(["c9b67094-cdab-494c-818e-d4845088269b"])),
            main_supporter_user_id="906a3144-9650-4a34-8a23-3b02f3b9aeac",
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

        mock_master_organization = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_index, "query"
        )
        mock_master_organization.return_value = [
            MasterSupporterOrganizationModel(
                id="556a3144-9650-4a34-8a23-3b02f3b9aeac",
                data_type=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
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
            ),
        ]

        # =================================================================
        # メールやお知らせ通知の確認をしたい場合は、以下のモックをコメントアウト
        # =================================================================
        # 汎用マスター 個別カルテ更新記録のモック
        mocker.patch.object(MasterUpdateKarteRecord, "update")
        # メール送信のモック
        mocker.patch.object(KartenService, "send_mail")
        # お知らせ通知のモック
        mocker.patch.object(BatchWrite, "save")
        # =================================================================

        # PF案件一覧取得のモック
        if is_empty_response_of_platform_get_projects:
            # 0件（レスポンスのprojectsが空リスト）の場合
            json_load_get_projects = {"offsetPage": 0, "total": 0, "projects": []}
        else:
            json_open_get_projects = open("mock/pf/get_projects.json", "r")
            json_load_get_projects = json.load(json_open_get_projects)
        mock_pf_get_projects = mocker.patch.object(PlatformApiOperator, "get_projects")
        mock_pf_get_projects.return_value = (
            platform_get_projects_status,
            json_load_get_projects,
        )

        # PF案件詳細取得のモック
        json_open_get_project_by_id = open("mock/pf/get_project_by_id.json", "r")
        json_load_get_project_by_id = json.load(json_open_get_project_by_id)
        # 顧客セグメント
        json_load_get_project_by_id["customer"]["category"] = platform_category
        # 部署名
        json_load_get_project_by_id["department"]["departmentName"] = (
            platform_department_name
        )
        mock_pf_get_project_by_pf_id = mocker.patch.object(
            PlatformApiOperator, "get_project_by_pf_id"
        )
        mock_pf_get_project_by_pf_id.return_value = (
            platform_get_project_by_id_status,
            json_load_get_project_by_id,
        )

        # 実行
        response = client.put(f"/api/karten/{karte_id}?version={version}", json=body)
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "body",
        [
            #####################
            # is_draft
            #####################
            # 必須チェック
            {
                # "is_draft": False,
                "is_notify_update_karte": True,
            },
            # Noneチェック
            {
                "is_draft": None,
                "is_notify_update_karte": True,
            },
            #####################
            # is_notify_update_karte
            #####################
            # 必須チェック
            {
                "is_draft": False,
                # "is_notify_update_karte": True,
            },
            # Noneチェック
            {
                "is_draft": False,
                "is_notify_update_karte": None,
            },
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_validation(self, body):
        karte_id = "89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
        version = 1

        response = client.put(f"/api/karten/{karte_id}?version={version}", json=body)

        assert response.status_code == 422
