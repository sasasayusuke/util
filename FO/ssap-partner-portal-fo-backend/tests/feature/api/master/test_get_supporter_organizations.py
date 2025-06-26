import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.models.master import MasterSupporterOrganizationModel

client = TestClient(app)


class TestGetSupporterOrganizations:
    @pytest.mark.parametrize(
        "model_list, expected",
        [
            (
                [
                    MasterSupporterOrganizationModel(
                        id="180a3597-b7e7-42c8-902c-a29016afa662",
                        name="Ideation Service Team",
                        value="IST",
                    ),
                    MasterSupporterOrganizationModel(
                        id="de40733f-6be9-4fef-8229-01052f43c1e2",
                        name="Acceleration Service Team",
                        value="AST",
                    ),
                ],
                {
                    "supporterOrganizations": [
                        {
                            "id": "180a3597-b7e7-42c8-902c-a29016afa662",
                            "name": "Ideation Service Team",
                            "shortName": "IST",
                        },
                        {
                            "id": "de40733f-6be9-4fef-8229-01052f43c1e2",
                            "name": "Acceleration Service Team",
                            "shortName": "AST",
                        },
                    ],
                }
            )
        ],
    )
    @pytest.mark.usefixtures("auth_supporter_user")
    def test_ok(self, mocker, model_list, expected):
        """支援者組織の一覧を取得の成功"""
        mock = mocker.patch.object(
            MasterSupporterOrganizationModel.data_type_order_index, "query"
        )
        mock.return_value = model_list
        response = client.get("api/masters/supporter-organizations")

        actual = response.json()
        assert response.status_code == 200
        assert actual == expected
