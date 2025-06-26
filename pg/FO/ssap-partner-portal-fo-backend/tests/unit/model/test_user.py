import uuid
from datetime import datetime

from moto import mock_dynamodb

from app.models.user import UserModel


@mock_dynamodb
def test_model_user():
    """一般ユーザーTBL UserModelクラス テスト定義
    """

    UserModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    id = str(uuid.uuid4())
    data_type = 'user'

    user = UserModel(id, data_type)

    user.name = '田中太郎'
    user.email = 'tanaka@foo.baz'
    user.role = 'customer'
    user.customer_id = 'customer1'
    user.cognito_id = 'cognito-id1'
    user.agreed = True
    user.disabled = True
    user.create_at = datetime.now()
    user.update_at = datetime.now()

    user.save()

    user_item = UserModel.get(id, data_type)
    user_item_by_query_cognito = UserModel.cognito_id_index.query(
        user.cognito_id
    )
    user_item_by_query_email = UserModel.data_type_email_index.query(
        data_type, user.email
    )
    user_item_by_query_name = UserModel.data_type_name_index.query(
        data_type, user.name
    )
    user_item_by_query_id_email = UserModel.customer_id_email_index.query(
        user.customer_id, user.email
    )
    user_item_by_query_role = UserModel.role_index.query(user.role)

    assert str(user) == str(user_item)
    assert user_item_by_query_cognito is not None
    assert user_item_by_query_email is not None
    assert user_item_by_query_name is not None
    assert user_item_by_query_id_email is not None
    assert user_item_by_query_role is not None

    UserModel.delete_table()
