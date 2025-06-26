from datetime import datetime

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute
from app.resources.const import DataType
from fastapi import HTTPException, status
from pynamodb.attributes import (
    BooleanAttribute,
    ListAttribute,
    UnicodeAttribute,
    UnicodeSetAttribute,
    VersionAttribute,
)
from pynamodb.exceptions import DoesNotExist
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model


class CognitoIdIndex(GlobalSecondaryIndex):
    """GSI：CognitoID検索"""

    class Meta:
        index_name = "cognito_id-index"
        projection = AllProjection()

    cognito_id = UnicodeAttribute(hash_key=True)


class DataTypeEmailIndex(GlobalSecondaryIndex):
    """GSI：データ区分メールアドレス検索"""

    class Meta:
        index_name = "data_type-email-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    email = UnicodeAttribute(range_key=True)


class DataTypeLastLoginAtIndex(GlobalSecondaryIndex):
    """GSI：データ区分最終ログイン日時ソート"""

    class Meta:
        index_name = "data_type-last_login_at-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    last_login_at = JSTDateTimeAttribute(range_key=True)


class DataTypeNameIndex(GlobalSecondaryIndex):
    """GSI：データ区分氏名検索"""

    class Meta:
        index_name = "data_type-name-index"
        projection = AllProjection()

    data_type = UnicodeAttribute(hash_key=True)
    name = UnicodeAttribute(range_key=True)


class CustomerIdEmailIndex(GlobalSecondaryIndex):
    """GSI：取引先ID、メールアドレス検索"""

    class Meta:
        index_name = "customer_id-email-index"
        projection = AllProjection()

    customer_id = UnicodeAttribute(hash_key=True)
    email = UnicodeAttribute(range_key=True)


class RoleIndex(GlobalSecondaryIndex):
    class Meta:
        index_name = "role-index"
        projection = AllProjection()

    role = UnicodeAttribute(hash_key=True)


class UserModel(Model):
    """一般ユーザーTBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "User"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    email = UnicodeAttribute(null=False)
    role = UnicodeAttribute(null=False)
    customer_id = UnicodeAttribute(null=True)
    customer_name = UnicodeAttribute(null=True)
    job = UnicodeAttribute(null=True)
    company = UnicodeAttribute(null=True)
    solver_corporation_id = UnicodeAttribute(null=True)
    supporter_organization_id = ListAttribute(null=True)
    organization_name = UnicodeAttribute(null=True)
    is_input_man_hour = BooleanAttribute(null=True)
    project_ids = UnicodeSetAttribute(null=True)
    cognito_id = UnicodeAttribute(null=True)
    agreed = BooleanAttribute(null=False, default=False)
    last_login_at = JSTDateTimeAttribute(null=True)
    disabled = BooleanAttribute(null=False, default=False)
    create_id = UnicodeAttribute(null=True)
    create_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    update_id = UnicodeAttribute(null=True)
    update_at = JSTDateTimeAttribute(null=False, default=datetime.now())
    version = VersionAttribute()

    cognito_id_index = CognitoIdIndex()
    data_type_email_index = DataTypeEmailIndex()
    data_type_last_login_at_index = DataTypeLastLoginAtIndex()
    data_type_name_index = DataTypeNameIndex()
    customer_id_email_index = CustomerIdEmailIndex()
    role_index = RoleIndex()

    @staticmethod
    def get_user_count_by_email(data_type: str, email: str) -> int:
        return UserModel.data_type_email_index.count(
            hash_key=data_type, range_key_condition=UserModel.email == email
        )

    @staticmethod
    def get_auth_user(cognito_id: str, email: str):
        users = UserModel.cognito_id_index.query(hash_key=cognito_id)
        try:
            user = next(users)
        except StopIteration:
            users = UserModel.data_type_email_index.query(
                hash_key=DataType.USER, range_key_condition=UserModel.email == email
            )
            user = next(users)

        return user

    @staticmethod
    def get_user_by_id(user_id: str):
        try:
            return UserModel.get(hash_key=user_id, range_key=DataType.USER)
        except DoesNotExist:
            raise HTTPException(status.HTTP_404_NOT_FOUND, detail="User not found.")

    @staticmethod
    def get_update_user_name(user_id: str) -> str:
        update_user_name = ""
        if not user_id:
            return update_user_name
        try:
            user = UserModel.get(hash_key=user_id, range_key=DataType.USER)
            update_user_name = user.name
        except DoesNotExist:
            raise HTTPException(status.HTTP_404_NOT_FOUND, detail="User not found.")

        return update_user_name
