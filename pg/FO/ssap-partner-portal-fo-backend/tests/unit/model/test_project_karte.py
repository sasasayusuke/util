from datetime import datetime

from moto import mock_dynamodb

from app.models.project_karte import ProjectKarteModel


@mock_dynamodb
def test_model_project_karte():
    """案件カルテTBL ProjectKarteModelクラス テスト定義
    """

    ProjectKarteModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    karte_id = 'karte1'

    project_karte = ProjectKarteModel(karte_id)

    project_karte.project_id = 'project1'
    project_karte.customer_id = 'customer1'
    project_karte.date = datetime.now().strftime("%Y/%m/%d")
    project_karte.is_draft = False
    project_karte.create_at = datetime.now()
    project_karte.update_at = datetime.now()

    project_karte.save()

    project_karte_item = ProjectKarteModel.get(karte_id)
    project_karte_item_by_query = ProjectKarteModel.date_index.query(
        project_karte.date
    )

    assert str(project_karte) == str(project_karte_item)
    assert project_karte_item_by_query is not None

    ProjectKarteModel.delete_table()
