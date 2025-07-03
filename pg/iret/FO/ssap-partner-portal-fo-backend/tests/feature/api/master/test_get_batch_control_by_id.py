import pytest
from app.main import app
from app.models.master import BatchControlAttribute, MasterBatchControlModel
from app.resources.const import MasterDataType
from fastapi.testclient import TestClient

client = TestClient(app)


class TestGetBatchControlById:
    @pytest.mark.parametrize(
        "batch_id, model, expected",
        [
            (
                "2ed6e959-c216-46a8-92c3-3d7d3f951d00",
                [
                    MasterBatchControlModel(
                        id="2ed6e959-c216-46a8-92c3-3d7d3f951d00",
                        data_type=MasterDataType.BATCH_CONTROL.value,
                        name="BO工数情報集計データ作成処理",
                        value="partnerportal-backoffice-dev-batch_summary_man_hour#project",
                        attributes=BatchControlAttribute(
                            batch_start_at="2022-08-02T22:42:05.033Z",
                            batch_end_at="2022-08-03T22:42:05.033Z",
                            batch_status="executed",
                            batch_rerun_span=10,
                        ),
                        update_at="2022-08-01T22:42:05.033Z",
                    )
                ],
                {
                    "batchEndAt": "2022-08-03T22:42:05.033+09:00",
                },
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, batch_id, model, expected):
        """正常系のテスト"""
        mock = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        mock.return_value = iter(model)

        response = client.get(f"api/masters/batch-control/{batch_id}")
        actual = response.json()

        assert response.status_code == 200
        assert actual == expected

    @pytest.mark.usefixtures("auth_supporter_user")
    def test_not_found(
        self,
        mocker,
    ):
        """id が存在しないときのテスト"""
        batch_id = "222-c216-46a8-92c3-3d7d3f951d00"
        mock = mocker.patch.object(
            MasterBatchControlModel.data_type_value_index, "query"
        )
        mock.side_effect = StopIteration()

        response = client.get(f"api/masters/batch-control/{batch_id}")
        actual = response.json()

        assert response.status_code == 404
        assert actual["detail"] == "Not Found"
