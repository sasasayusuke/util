import json
import os
import uuid
from dataclasses import dataclass, field
from datetime import datetime, timedelta
from decimal import ROUND_HALF_UP, Decimal
from logging import getLogger
from typing import Any, List

from distutils.util import strtobool
from pynamodb.attributes import MapAttribute
from pynamodb.models import Model
from pytz import timezone

from app.core.config import get_app_settings
from app.models.man_hour import SupportManHourAttribute
from app.models.master import BatchControlAttribute, MasterBatchControlModel
from app.utils.aws.sqs import SqsHelper
from functions.batch_const import BatchSettingConst, BatchStatus, MasterDataType
from functions.batch_exceptions import ExitHandler

# ログ出力設定をロードする
get_app_settings().configure_logging()
logger = getLogger(__name__)


def send_mail(
    template, to_addr_list: List[str], cc_addr_list: List[str], payload: dict
):
    """メール送信(SqsHelper.send_message_to_queue()を使用)"""
    queue_name = get_app_settings().sqs_email_queue_name
    message_body = {
        "template": template,
        "to": to_addr_list,
        "cc": cc_addr_list,
        "payload": payload,
    }
    sqs_message_body = json.dumps(message_body)
    SqsHelper().send_message_to_queue(
        queue_name=queue_name, message_body=sqs_message_body
    )


def send_mail_batch(entries: List[dict]):
    """メール送信(SqsHelper.send_message_batch_to_queue()を使用)"""
    queue_name = get_app_settings().sqs_email_queue_name
    SqsHelper().send_message_batch_to_queue(queue_name=queue_name, entries=entries)


def get_operation_datetime() -> datetime:
    """起動日時の取得

    テストを効率よく行えるようLambda環境変数(OPERATION_DATETIME)から起動日時を取得する。
    ・「yyyy/mm/dd hh:mm」形式の値が入っている場合 : 左記を起動日時として利用
    ・上記以外の場合 : 現在日時

    Returns:
        datetime: 起動日時(JST)
    """
    try:
        env_datetime = os.getenv("OPERATION_DATETIME")
        if env_datetime:
            dt = datetime.strptime(env_datetime, "%Y/%m/%d %H:%M")
            return timezone("Asia/Tokyo").localize(dt)
    except ValueError:
        logger.debug("Incorrect format of datetime specified in environment variable")

    return datetime.now(timezone("Asia/Tokyo"))


def model_to_json(model: Model, indent=2):
    return json.dumps(model_to_dict(model), indent=indent)


def model_to_dict(model: Model):
    _dict = {}
    for name, attr in model.attribute_values.items():
        _dict[name] = _attr_to_obj(attr)

    return _dict


def _attr_to_obj(attr):
    if isinstance(attr, list):
        _list = []
        for el in attr:
            _list.append(_attr_to_obj(el))
        return _list
    elif isinstance(attr, MapAttribute):
        _dict = {}
        for k, v in attr.attribute_values.items():
            _dict[k] = _attr_to_obj(v)
        return _dict
    elif isinstance(attr, datetime):
        return attr.isoformat()
    else:
        return attr


def decimal_round(value: float, digit: float = 0) -> float:
    """小数点以下を任意の桁数で四捨五入します
    Args:
        value (float): 対象の値
        digit (int): 四捨五入する位。0 → 小数点第一位, 0.1 → 小数点第二位
    Returns:
        float: 四捨五入された値
    """
    decimal_value = Decimal(str(value))
    return float(decimal_value.quantize(Decimal(str(digit)), rounding=ROUND_HALF_UP))


def batch_init_common_procedure(
    event,
    operation_start_datetime: datetime,
    batch_control_id: str,
    batch_control_name: str,
):
    """
        初期処理(共通)

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID (汎用マスター バッチ処理制御に使用)
          e.g. "partnerportal-backoffice-dev-batch_summary_man_hour#service_type"
        batch_control_name (str): バッチ関数名 (汎用マスター バッチ処理制御に使用)
          e.g. "BO工数情報集計データ作成処理"
    """

    # 汎用マスター バッチ処理制御の取得
    range_key_condition = MasterBatchControlModel.value == batch_control_id
    batch_control_list: List[MasterBatchControlModel] = list(
        MasterBatchControlModel.data_type_value_index.query(
            hash_key=MasterDataType.BATCH_CONTROL,
            range_key_condition=range_key_condition,
        )
    )

    # 現在日時
    datetime_now = datetime.now()

    if batch_control_list:
        batch_control_model = batch_control_list[0]
        env_datetime = os.getenv("OPERATION_DATETIME")
        # Lambda環境変数(OPERATION_DATETIME)が指定されていない場合のみ起動時刻をチェックする
        if not env_datetime:
            # 起動時刻が前回更新日時＋再処理可能期間を経過していない場合は処理終了
            rerun_limit = batch_control_model.update_at + timedelta(
                minutes=int(batch_control_model.attributes.batch_rerun_span)
            )
            if operation_start_datetime <= rerun_limit:
                # 処理終了
                logger.info(
                    "起動時刻が前回更新日時＋再処理可能期間を経過していないため、処理終了"
                )
                raise ExitHandler()

        batch_control_model.update(
            actions=[
                MasterBatchControlModel.attributes.batch_start_at.set(datetime_now),
                MasterBatchControlModel.attributes.batch_status.set(
                    BatchStatus.RUNNING
                ),
                MasterBatchControlModel.update_at.set(datetime_now),
            ]
        )

    else:
        # 汎用マスター バッチ処理制御の項目が存在しない場合、新規作成
        new_batch_control = MasterBatchControlModel(
            id=str(uuid.uuid4()),
            data_type=MasterDataType.BATCH_CONTROL,
            name=batch_control_name,
            value=batch_control_id,
            attributes=BatchControlAttribute(
                batch_start_at=datetime_now,
                batch_status=BatchStatus.RUNNING,
                batch_rerun_span=BatchSettingConst.BATCH_RERUN_SPAN,
            ),
            update_at=datetime_now,
        )
        new_batch_control.save()


def batch_end_common_procedure(event, batch_status: str, batch_control_id: str):
    """
        終了処理(共通)

    Args:
        event:
        operation_start_datetime (datetime): 起動日時
        batch_control_id (str): バッチ関数ID
    """
    # 汎用マスター バッチ処理制御の項目取得
    range_key_condition = MasterBatchControlModel.value == batch_control_id
    batch_control_list: List[MasterBatchControlModel] = list(
        MasterBatchControlModel.data_type_value_index.query(
            hash_key=MasterDataType.BATCH_CONTROL,
            range_key_condition=range_key_condition,
        )
    )
    datetime_now = datetime.now()
    batch_control_list[0].update(
        actions=[
            MasterBatchControlModel.attributes.batch_end_at.set(datetime_now),
            MasterBatchControlModel.attributes.batch_status.set(batch_status),
            MasterBatchControlModel.update_at.set(datetime_now),
        ]
    )


def is_int(target: str) -> bool:
    """数値チェック"""
    try:
        int(target)
    except ValueError:
        return False
    else:
        return True


def is_float(target: str) -> bool:
    """数値チェック"""
    try:
        float(target)
    except ValueError:
        return False
    else:
        return True


def is_bool(target: str) -> bool:
    """真理値チェック"""
    try:
        strtobool(target)
    except ValueError:
        return False
    else:
        return True


def conv_blank(target: Any) -> str:
    """Noneの場合、空文字に変換します

    Args:
        target (Any): 対象のオブジェクト

    Returns:
        str: 変換後の文字列
    """
    if target is None:
        return ""
    return target


@dataclass
class SummaryInfoForProjectSummary:
    """プロジェクト別工数集計：累積情報クラス"""

    project_id: str
    summary_direct_support_man_hour: float = 0
    summary_pre_support_man_hour: float = 0
    summary_supporter_direct_support_man_hours: List[SupportManHourAttribute] = field(
        default_factory=list
    )
    summary_supporter_pre_support_man_hours: List[SupportManHourAttribute] = field(
        default_factory=list
    )
