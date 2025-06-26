import json
import pytest
from app.main import app
from app.utils.platform import PlatformApiOperator
from fastapi.testclient import TestClient

client = TestClient(app)


class TestGetMasterKartenSelectBox:
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker):
        """正常系"""

        json_open = open("mock/pf/get_code_master.json", "r")
        get_code_master_mock = json.load(json_open)
        pf_api_operator_mock = mocker.patch.object(
            PlatformApiOperator, "get_code_master"
        )
        pf_api_operator_mock.return_value = (200, get_code_master_mock)

        response = client.get("/api/master-karten/select-box")
        actual = response.json()

        expected = [
            {
                "name": "customerSegment",
                "items": [
                    {"value": "value1", "label": "大企業"},
                    {"value": "value2", "label": "中小企業"},
                    {"value": "value3", "label": "ベンチャー"},
                ],
            },
            {
                "name": "industrySegment",
                "items": [
                    {"value": "value1", "label": "東証33業種分類不可"},
                    {"value": "value2", "label": "電気機器"},
                    {"value": "value3", "label": "精密機器"},
                    {"value": "value4", "label": "食料品"},
                    {"value": "value5", "label": "輸送用機器"},
                    {"value": "value6", "label": "化学"},
                    {"value": "value7", "label": "非鉄金属"},
                    {"value": "value8", "label": "水産農林業"},
                    {"value": "value9", "label": "その他製品"},
                    {"value": "value10", "label": "情報・通信業"},
                    {"value": "value11", "label": "医薬品"},
                    {"value": "value12", "label": "不動産業"},
                    {"value": "value13", "label": "機械"},
                    {"value": "value14", "label": "電機機器"},
                    {"value": "value15", "label": "サービス業"},
                    {"value": "value16", "label": "卸売業"},
                    {"value": "value17", "label": "ガラス・土石製品"},
                    {"value": "value18", "label": "繊維製品"},
                    {"value": "value19", "label": "医療品"},
                    {"value": "value20", "label": "電気・ガス業"},
                    {"value": "value21", "label": "銀行"},
                    {"value": "value22", "label": "農業"},
                    {"value": "value23", "label": "鉱業"},
                    {"value": "value24", "label": "建設業"},
                    {"value": "value25", "label": "パルプ紙"},
                    {"value": "value26", "label": "石油石炭製品"},
                    {"value": "value27", "label": "ゴム製品"},
                    {"value": "value28", "label": "鉄鋼"},
                    {"value": "value29", "label": "金属製品"},
                    {"value": "value30", "label": "陸運業"},
                    {"value": "value31", "label": "海運業"},
                    {"value": "value32", "label": "空運業"},
                    {"value": "value33", "label": "倉庫運輸関連業"},
                    {"value": "value34", "label": "小売業"},
                    {"value": "value35", "label": "証券商品先物取引業"},
                    {"value": "value36", "label": "保険業"},
                    {"value": "value37", "label": "その他金融業"},
                ],
            },
            {
                "name": "lineup",
                "items": [
                    {"value": "value1", "label": "ライナップ1"},
                    {"value": "value2", "label": "ライナップ2"},
                    {"value": "value3", "label": "ライナップ3"},
                    {"value": "value4", "label": "ライナップ4"},
                    {"value": "value5", "label": "ライナップ5"},
                ],
            },
            {
                "name": "serviceType",
                "items": [
                    {"value": "value1", "label": "アイディア可視化"},
                    {"value": "value2", "label": "組織開発"},
                    {"value": "value3", "label": "事業育成＆拡大"},
                    {"value": "value4", "label": "その他"},
                ],
            },
            {
                "name": "projectResult",
                "items": [
                    {"value": "value1", "label": "事業化"},
                    {"value": "value2", "label": "継続"},
                    {"value": "value3", "label": "中止"},
                    {"value": "value4", "label": "その他"},
                ],
            },
            {
                "name": "supportResult",
                "items": [
                    {"value": "value1", "label": "完了"},
                    {"value": "value2", "label": "継続"},
                    {"value": "value3", "label": "打ち切り"},
                    {"value": "value4", "label": "その他"},
                ],
            },
        ]

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_supporter_user")
    def test_pf_api_exeption(self, mocker):
        """異常系: プラットフォームAPIからエラーが返ってきた時"""
        pf_api_operator_mock = mocker.patch.object(
            PlatformApiOperator, "get_code_master"
        )
        pf_api_response_detail = {"detail": {"code": 401, "message": "Unauthorized"}}
        pf_api_operator_mock.return_value = (401, pf_api_response_detail)

        response = client.get("/api/master-karten/select-box")
        actual = response.json()

        expected = {"detail": '{"detail": {"code": 401, "message": "Unauthorized"}}'}

        assert response.status_code == 401
        assert actual == expected
