from datetime import datetime

from app.models.project import ProjectModel
from moto import mock_dynamodb


@mock_dynamodb
def test_model_project():
    """案件TBL ProjectModelクラス テスト定義
    """

    ProjectModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    id = 'project1'
    data_type = 'project'

    project = ProjectModel(id, data_type)

    project.customer_id = 'customer1'
    project.name = 'opportunity1'
    project.main_sales_user_id = '田中太郎'
    project.support_date_from = datetime.now().strftime("%Y/%m/%d")
    project.support_date_to = datetime.now().strftime("%Y/%m/%d")
    project.total_contract_time = 9
    project.main_customer_user_id = 'user2'
    project.is_survey = False
    project.is_count_man_hour = False
    project.is_karte_remind = False
    project.is_secret = True
    project.create_at = datetime.now()
    project.update_at = datetime.now()

    project.save()

    project_item = ProjectModel.get(id, data_type)
    project_item_by_query_name = ProjectModel.customer_id_name_index.query(
        project.customer_id, project.name
    )
    project_item_by_query_date = ProjectModel.data_type_support_date_to_index.query(
        project.support_date_to
    )

    assert str(project) == str(project_item)
    assert project_item_by_query_name is not None
    assert project_item_by_query_date is not None

    ProjectModel.delete_table()
