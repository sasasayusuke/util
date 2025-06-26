from datetime import datetime

from moto import mock_dynamodb

from app.models.session import SessionModel


@mock_dynamodb
def test_model_session():
    """セッションTBL SessionModelクラス テスト定義
    """

    SessionModel.create_table(
        read_capacity_units=5, write_capacity_units=5, wait=True
    )

    cognito_id = 'cognito-id1'

    session = SessionModel(cognito_id)

    session.latest_access_at = datetime.now()

    session.save()

    session_item = SessionModel.get(cognito_id)

    assert str(session) == str(session_item)

    SessionModel.delete_table()
