from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.models.admin import AdminModel
from app.models.user import UserModel
from app.resources.const import (
    BatchFunctionId,
    DataType,
    MasterKartenUpdateUserType,
)
from pynamodb.exceptions import DoesNotExist

logger = CustomLogger.get_logger()


def get_update_user_name(user_id: str) -> str:
    """
        更新ユーザ名称の取得に使用.
        一般ユーザ情報、管理ユーザ情報の順でユーザ情報を検索する.
        ユーザ情報が取得できない場合は、エラーログを出力し、空文字を返却。
    Args:
        user_id (str): ユーザID
    Returns:
        str: ユーザ名称
    """
    update_user_name = ""
    if not user_id:
        return update_user_name

    try:
        user = UserModel.get(hash_key=user_id, range_key=DataType.USER)
        update_user_name = user.name
    except DoesNotExist:
        try:
            admin = AdminModel.get(hash_key=user_id, range_key=DataType.ADMIN)
            update_user_name = admin.name
        except DoesNotExist:
            pass

    if not update_user_name:
        # 更新ユーザIDが自動連携バッチのバッチIDの場合、固定値を返却する
        env = get_app_settings().env
        if user_id == BatchFunctionId.AUTOMATIC_LINK_BATCH.format(landscape=env):
            update_user_name = MasterKartenUpdateUserType.SALESFORCE

    if not update_user_name:
        logger.error(f"Update user not found. user_id: {user_id}")

    return update_user_name
