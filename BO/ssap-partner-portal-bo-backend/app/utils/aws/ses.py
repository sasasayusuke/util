from typing import Dict, List, Optional, Union

import boto3
import yaml
from jinja2 import Template

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings

from email.header import Header

logger = CustomLogger.get_logger()


class SesHelper:
    def __init__(self):
        self.resource = boto3.client("ses")

    def send_mail(
        self,
        template_name: str,
        to: List[str],
        cc: List[str],
        payload: Dict[str, Union[str, int]],
        bcc: List[str] = [],
    ) -> Optional[str]:
        """メール本文を組み立てて、メールを送信します
        Args:
            template_name (str): メール本文を組み立てる際に使用するテンプレート名 (同名のテンプレートファイルが存在する必要がある)
            to (List[str]): メール送信先（TO）
            cc (List[str]): メール送信先（CC）
            payload (Dict[str, Union[str, int]]): テンプレートの変数部分に流し込む実際の値 (ネストのない JSON 形式)
            bcc (List[str]): メール送信先（BCC）※任意. デフォルト:空リスト
        Returns:
            str: メール送信時に発行される Message ID
        """

        with open(
            get_app_settings().email_template.format(template_name=template_name)
        ) as file:
            template = yaml.safe_load(file)

        # メール件名のテンプレート処理
        subject_template = Template(template["message"]["subject"]["data"])
        subject_data = subject_template.render(payload)

        # メール本文のテンプレート処理
        text_template = Template(template["message"]["body"]["text"]["data"])
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
                    "Charset": template["message"]["subject"]["charset"],
                },
                "Body": {
                    "Text": {
                        "Data": text_data,
                        "Charset": template["message"]["body"]["text"]["charset"],
                    },
                },
            },
        )

        logger.info(f"Email has been sent, MessageId: {response['MessageId']}")

        return response
