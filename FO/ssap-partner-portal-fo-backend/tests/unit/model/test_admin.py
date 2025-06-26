from datetime import datetime

from moto import mock_dynamodb

from app.models.admin import AdminModel


@mock_dynamodb
def test_model_admin():
    """管理者ユーザーTBL AdminModelクラス テスト定義
    """

    AdminModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    id = 'id1'
    data_type = 'admin'

    admin = AdminModel(id, data_type)
    admin.name = 'user1'
    admin.email = 'foo@example.com'
    admin.roles = ['sales', 'sales_mgr']
    admin.job = 'mgr'
    admin.supporter_organization_id = 'organization1'
    admin.cognito_id = 'congnito-1234'
    admin.disabled = False
    admin.create_at = datetime.now()
    admin.update_at = datetime.now()

    admin.save()

    admin_item = AdminModel.get(id, data_type)
    admin_item_by_query_cognito = AdminModel.cognito_id_index.query(
        data_type, id
    )
    admin_item_by_query_email = AdminModel.data_type_email_index.query(
        data_type, admin.email
    )
    admin_item_by_query_name = AdminModel.data_type_name_index.query(
        data_type, admin.name
    )

    assert str(admin) == str(admin_item)
    assert admin_item_by_query_cognito is not None
    assert admin_item_by_query_email is not None
    assert admin_item_by_query_name is not None

    AdminModel.delete_table()
