import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.master import MasterSelectItems

client = TestClient(app)


class TestGetServiceTypes:
    @pytest.mark.parametrize(
        "data_type, model_list, expected",
        [
            (
                "issue_map50",
                [
                    MasterSelectItems(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="issue_map50",
                        category="人材育成",
                        name="新規事業開発を担うリーダーが不足している",
                        use=True,
                    ),
                    MasterSelectItems(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="issue_map50",
                        category="経営改善",
                        name="新規事業創出を促す体制がない",
                        use=True,
                    ),
                    MasterSelectItems(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="issue_map50",
                        category="組織体制",
                        name="部門間連携がスムーズに行われていない",
                        use=True,
                    ),
                    MasterSelectItems(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="issue_map50",
                        category="ITシステム",
                        name="社内システムが老朽化し、業務効率が低下している",
                        use=True,
                    ),
                ],
                {
                    "masters": [
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": "人材育成",
                            "name": "新規事業開発を担うリーダーが不足している",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": "経営改善",
                            "name": "新規事業創出を促す体制がない",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": "組織体制",
                            "name": "部門間連携がスムーズに行われていない",
                        },
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": "ITシステム",
                            "name": "社内システムが老朽化し、業務効率が低下している",
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_issue_map50_for_apt(self, mocker, data_type, model_list, expected):
        """課題マップ50一覧情報の取得成功（APT）"""
        mock = mocker.patch.object(MasterSelectItems.data_type_order_index, "query")
        mock.return_value = model_list

        response = client.get(f"/api/masters/select-items?dataType={data_type}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "data_type, model_list, expected",
        [
            (
                "issue_map50",
                [
                    MasterSelectItems(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="issue_map50",
                        category="人材育成",
                        name="新規事業開発を担うリーダーが不足している",
                        order=1,
                        use=True,
                    ),
                    MasterSelectItems(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="issue_map50",
                        category="経営改善",
                        name="新規事業創出を促す体制がない",
                        order=2,
                        use=True,
                    ),
                    MasterSelectItems(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="issue_map50",
                        category="組織体制",
                        name="部門間連携がスムーズに行われていない",
                        order=3,
                        use=True,
                    ),
                    MasterSelectItems(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="issue_map50",
                        category="ITシステム",
                        name="社内システムが老朽化し、業務効率が低下している",
                        order=4,
                        use=True,
                    ),
                ],
                {
                    "masters": [
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": "人材育成",
                            "name": "新規事業開発を担うリーダーが不足している",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": "経営改善",
                            "name": "新規事業創出を促す体制がない",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": "組織体制",
                            "name": "部門間連携がスムーズに行われていない",
                        },
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": "ITシステム",
                            "name": "社内システムが老朽化し、業務効率が低下している",
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_ok_issue_map50_for_solver_staff(self, mocker, data_type, model_list, expected):
        """課題マップ50一覧情報の取得成功（法人ソルバー）"""
        mock = mocker.patch.object(MasterSelectItems.data_type_order_index, "query")
        mock.return_value = model_list

        response = client.get(f"/api/masters/select-items?dataType={data_type}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "data_type, model_list, expected",
        [
            (
                "industry_segment",
                [
                    MasterSelectItems(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="industry_segment",
                        name="食料品",
                        use=True,
                    ),
                    MasterSelectItems(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="industry_segment",
                        name="機械",
                        use=True,
                    ),
                    MasterSelectItems(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="industry_segment",
                        name="海運業",
                        use=True,
                    ),
                    MasterSelectItems(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="industry_segment",
                        name="医薬品",
                        use=True,
                    ),
                ],
                {
                    "masters": [
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": None,
                            "name": "食料品",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": None,
                            "name": "機械",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": None,
                            "name": "海運業",
                        },
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": None,
                            "name": "医薬品",
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_apt_user")
    def test_ok_industry_segment_for_apt(self, mocker, data_type, model_list, expected):
        """東証33業種経験/対応可能領域一覧情報の取得成功（APT）"""
        mock = mocker.patch.object(MasterSelectItems.data_type_index, "query")
        mock.return_value = model_list

        response = client.get(f"/api/masters/select-items?dataType={data_type}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.parametrize(
        "data_type, model_list, expected",
        [
            (
                "industry_segment",
                [
                    MasterSelectItems(
                        id="11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="industry_segment",
                        name="食料品",
                        use=True,
                    ),
                    MasterSelectItems(
                        id="22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="industry_segment",
                        name="機械",
                        use=True,
                    ),
                    MasterSelectItems(
                        id="44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="industry_segment",
                        name="海運業",
                        use=True,
                    ),
                    MasterSelectItems(
                        id="55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                        data_type="industry_segment",
                        name="医薬品",
                        use=True,
                    ),
                ],
                {
                    "masters": [
                        {
                            "id": "11cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": None,
                            "name": "食料品",
                        },
                        {
                            "id": "22cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": None,
                            "name": "機械",
                        },
                        {
                            "id": "44cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": None,
                            "name": "海運業",
                        },
                        {
                            "id": "55cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                            "category": None,
                            "name": "医薬品",
                        },
                    ],
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_solver_staff_user")
    def test_ok_industry_segment_for_solver_staff(self, mocker, data_type, model_list, expected):
        """東証33業種経験/対応可能領域一覧情報の取得成功（法人ソルバー）"""
        mock = mocker.patch.object(MasterSelectItems.data_type_index, "query")
        mock.return_value = model_list

        response = client.get(f"/api/masters/select-items?dataType={data_type}")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
