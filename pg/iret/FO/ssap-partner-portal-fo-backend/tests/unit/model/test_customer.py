from datetime import datetime

from moto import mock_dynamodb

from app.models.customer import CustomerModel


@mock_dynamodb
def test_model_customer():
    """取引先TBL CustomerModelクラス テスト定義
    """

    CustomerModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    id = 'id1'
    data_type = 'customer'

    customer = CustomerModel(id, data_type)
    customer.name = 'user1'
    customer.category = 'ソニーグループ'
    customer.create_at = datetime.now()
    customer.update_at = datetime.now()

    customer.save()

    customer_item = CustomerModel.get(id, data_type)
    customer_item_by_query_name = CustomerModel.data_type_name_index.query(
        data_type, customer.name
    )
    customer_item_by_query_category = CustomerModel.data_type_category_index.query(
        data_type, customer.category
    )

    assert str(customer) == str(customer_item)
    assert customer_item_by_query_name is not None
    assert customer_item_by_query_category is not None

    CustomerModel.delete_table()
