from datetime import datetime

from app.core.config import get_app_settings
from app.models.custom_attribute.jst_datetime import JSTDateTimeAttribute
from pynamodb.attributes import (BooleanAttribute, UnicodeAttribute,
                                 UnicodeSetAttribute, VersionAttribute)
from pynamodb.indexes import AllProjection, GlobalSecondaryIndex
from pynamodb.models import Model
from app.resources.const import DataType
from pynamodb.exceptions import DoesNotExist
from fastapi import HTTPException, status


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
    email = UnicodeAttribute(range_key=True)


class AdminModel(Model):
    """管理者ユーザーTBLモデル定義"""

    class Meta:
        table_name = get_app_settings().table_prefix + "Admin"
        region = get_app_settings().aws_region

    id = UnicodeAttribute(hash_key=True, null=False)
    data_type = UnicodeAttribute(range_key=True, null=False)
    name = UnicodeAttribute(null=False)
    email = UnicodeAttribute(null=False)
    roles = UnicodeSetAttribute(null=False)
    company = UnicodeAttribute(null=True)
    job = UnicodeAttribute(null=False)
    supporter_organization_id = UnicodeSetAttribute(null=False)
    organization_name = UnicodeAttribute(null=True)
    cognito_id = UnicodeAttribute(null=True)
    last_login_at = JSTDateTimeAttribute(null=True, default=datetime.now())
    otp_secret = UnicodeAttribute(null=True)
    otp_verified_token = UnicodeAttribute(null=True)
    otp_verified_at = JSTDateTimeAttribute(null=True)
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

    @staticmethod
    def get_user_count_by_email(data_type: str, email: str) -> int:
        return AdminModel.data_type_email_index.count(
            hash_key=data_type, range_key_condition=AdminModel.email == email
        )

    @staticmethod
    def get_update_user_name(user_id: str) -> str:
        update_user_name = ""
        if not user_id:
            return update_user_name
        try:
            admin = AdminModel.get(hash_key=user_id, range_key=DataType.ADMIN)
            update_user_name = admin.name
        except DoesNotExist:
            raise HTTPException(status.HTTP_404_NOT_FOUND, detail="User not found.")

        return update_user_name

    # @staticmethod
    # def get_auth_user(cognito_id: str, email: str):
    #     try:
