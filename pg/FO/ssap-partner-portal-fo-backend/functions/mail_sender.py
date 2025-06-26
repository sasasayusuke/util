import copy
import json
from logging import getLogger
from typing import Any, Dict, List, Optional, Union

import boto3
import yaml
from jinja2 import Template

from app.core.config import get_app_settings

from email.header import Header

try:
    import unzip_requirements
except ImportError:
    pass

# ログ出力設定をロードする
get_app_settings().configure_logging()
logger = getLogger(__name__)


def _send_mail(
    template_name: str,
    to: List[str],
    cc: List[str],
    bcc: List[str],
    payload: Dict[str, Union[str, int]],
) -> Optional[str]:
    """メール本文を組み立てて、メールを送信します
    Args:
        template_name (str): メール本文を組み立てる際に使用するテンプレート名 (同名のテンプレートファイルが存在する必要がある)
        to (List[str]): メール送信先（TO）
        cc (List[str]): メール送信先（CC）
        bcc (List[str]): メール送信先（BCC）
        payload (Dict[str, Union[str, int]]): テンプレートの変数部分に流し込む実際の値 (ネストのない JSON 形式)
    Returns:
        str: メール送信時に発行される Message ID
    """
    # メールテンプレートの読み込み
    tmpl = {}

    with open(
        get_app_settings().email_template.format(template_name=template_name)
    ) as file:
        tmpl = yaml.safe_load(file)

    # メール件名のテンプレート処理
    subject_template = Template(tmpl["message"]["subject"]["data"])
    subject_data = subject_template.render(payload)

    # メール本文のテンプレート処理
    text_template = Template(tmpl["message"]["body"]["text"]["data"])
    text_data = text_template.render(payload)

    # メール送信者名をエンコード
    sender_name = Header(get_app_settings().sender_name.encode('iso-2022-jp'), 'iso-2022-jp').encode()
    sender_address = get_app_settings().sender_address

    ses = boto3.client("ses")
    response = ses.send_email(
        Source=f"{sender_name} <{sender_address}>",
        Destination={"ToAddresses": to, "CcAddresses": cc, "BccAddresses": bcc},
        Message={
            "Subject": {
                "Data": subject_data,
                "Charset": tmpl["message"]["subject"]["charset"],
            },
            "Body": {
                "Text": {
                    "Data": text_data,
                    "Charset": tmpl["message"]["body"]["text"]["charset"],
                },
            },
        },
    )

    logger.info(f"Email has been sent, MessageId: {response['MessageId']}")
    return response["MessageId"]


def handler(event: Dict[str, Any], context) -> List[str]:
    """SQS メッセージをトリガとして起動するハンドラ
    このハンドラでは SES を利用してメールを送信します。メールを送信したいユースケースにおいて、実装内で
    EmailQueue という SQS Queue に対してメッセージを送信すると、そのメッセージをトリガとしてこのハンドラが起動、
    そして SES を使用したメール送信が実行されます。
    SQS のメッセージボディ部分には次のような値が設定されることを期待しています。
    なお、bcc は任意項目ですので、必要の時のみ設定してください。
    ```python
    {
        "template": "registration_completed",
        "to": ["to@example.com"],
        "cc": ["cc@example.com"],
        "bcc": ["bcc@example.com"],
        "payload": {
            "mail_address": "to@example.com",
            "foo": "bar",
        },
    }
    ```
    boto3 を使用して SQS にメッセージを送信します。
    ```python
    sqs = boto3.resource("sqs")
    queue = sqs.get_queue_by_name(QueueName="EmailQueue")
    response = queue.send_message(MessageBody=json.dumps(data))
    ```
    Args:
        event (Dict[str, Any]): SQS メッセージをトリガとして起動したときの event
        context ([type]): Lambda 実行におけるコンテキスト情報
    Returns:
        List[str]: SES でのメール送信に成功したときに発行される Message ID の配列
    """
    logger.debug(event)

    # SES 送信成功時の Message ID を格納するための空の配列
    message_ids: List[str] = []

    for record in event["Records"]:
        sqs_message_id = record["messageId"]
        logger.info(f"messageId: {sqs_message_id}")

        try:
            # メッセージ本文を JSON 文字列として取り出す
            body = json.loads(record["body"])

            # メッセージ本文 (JSON) から独自ペイロードを取り出す
            payload = body["payload"]
            logger.debug(payload)

            # SESの宛先上限(50)単位のグループに分割し送信する
            to_addresses = body["to"]
            cc_addresses = body["cc"]
            bcc_addresses = body.get("bcc", [])
            temp_to_addresses = copy.deepcopy(to_addresses)
            temp_cc_addresses = copy.deepcopy(cc_addresses)
            temp_bcc_addresses = copy.deepcopy(bcc_addresses)
            # 50毎のセット数
            loop_count = len(to_addresses + cc_addresses + bcc_addresses) // 50 + 1
            for i in range(loop_count):
                to_addresses_splitted = []
                cc_addresses_splitted = []
                bcc_addresses_splitted = []

                for j in range(50):
                    if temp_to_addresses:
                        to_addresses_splitted.append(temp_to_addresses.pop(0))
                    elif temp_cc_addresses:
                        cc_addresses_splitted.append(temp_cc_addresses.pop(0))
                    elif temp_bcc_addresses:
                        bcc_addresses_splitted.append(temp_bcc_addresses.pop(0))

                if (
                    to_addresses_splitted
                    or cc_addresses_splitted
                    or bcc_addresses_splitted
                ):
                    message_id: Optional[str] = _send_mail(
                        template_name=body["template"],
                        to=to_addresses_splitted,
                        cc=cc_addresses_splitted,
                        bcc=bcc_addresses_splitted,
                        payload=payload,
                    )
                    if message_id:
                        message_ids.append(message_id)

        except Exception as ex:
            logger.error(ex)

    return message_ids
