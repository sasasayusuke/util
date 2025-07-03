from typing import Dict, List, Optional, Union

import botocore.exceptions
import os
import boto3
import mimetypes
import yaml
from jinja2 import Template

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.resources.const import MailType

from email.header import Header
from email.mime.application import MIMEApplication
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText

from fastapi import HTTPException, status

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
        sender_name = Header(
            get_app_settings().sender_name.encode("iso-2022-jp"), "iso-2022-jp"
        ).encode()
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

    def send_mail_with_file(
        self,
        template_name: str,
        to: List[str],
        cc: List[str],
        payload: Dict[str, Union[str, int]],
        payload_error: Dict[str, Union[str, int]],
        files: List[str] = [],
        bcc: List[str] = [],
    ) -> Optional[str]:
        """メール本文を組み立てて、添付ファイル付きメールを送信します
        Args:
            template_name (str): メール本文を組み立てる際に使用するテンプレート名 (同名のテンプレートファイルが存在する必要がある)
            to (List[str]): メール送信先（TO）
            cc (List[str]): メール送信先（CC）
            payload (Dict[str, Union[str, int]]): テンプレートの変数部分に流し込む実際の値 (ネストのない JSON 形式)
            payload_error (Dict[str, Union[str, int]]): エラーメールテンプレートの変数部分に流し込む実際の値 (ネストのない JSON 形式)
            files (List[str]): 画像ファイル名・パス・内容
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
        sender_name = Header(
            get_app_settings().sender_name.encode("iso-2022-jp"), "iso-2022-jp"
        ).encode()
        sender_address = get_app_settings().sender_address

        # MIMEメッセージの作成
        msg = MIMEMultipart("mixed")
        msg["Subject"] = Header(
            subject_data.encode(template["message"]["subject"]["charset"]),
            template["message"]["subject"]["charset"],
        )
        msg['From'] = f"{sender_name} <{sender_address}>"
        msg['To'] = ', '.join(to)

        # メール本文の追加
        msg_body = MIMEMultipart("alternative")
        text_part = MIMEText(
            text_data.encode(template["message"]["body"]["text"]["charset"]),
            "plain",
            template["message"]["body"]["text"]["charset"],
        )
        msg_body.attach(text_part)
        msg.attach(msg_body)

        # メールに画像ファイルを添付
        for file in files:
            ext = os.path.splitext(file["path"])
            # テキストファイルの場合はエンコードした上で添付、バイナリファイルの場合はそのまま添付
            if ext[1].lower() == ".txt":
                att = MIMEText(file["file_content"].encode("utf-8"), "plain", "utf-8")
            else:
                att = MIMEApplication(file["file_content"])
            att.add_header(
                "Content-Disposition", "attachment", filename=file["file_name"]
            )
            # ファイル名からMIMEタイプを推測
            mime_type = mimetypes.guess_type(file["file_name"])
            att.add_header('Content-Type', mime_type[0])
            msg.attach(att)

        # メールサイズを計算（MB）
        email_size = len(str(msg).encode('utf-8')) / 1024 / 1024
        # メールサイズが25MBを超えた場合、エラーメールを送信
        if email_size > 25:
            payload_error["error_message"] = f"The email size exceeds the 25MB limit. Current size: {email_size}MB."
            response = self.send_mail(
                template_name=MailType.MAIL_ERROR,
                # エラーメール通知の場合、宛先TOにシステム管理者を設定
                to=cc,
                cc=[],
                payload=payload_error
            )
            response["email_type"] = "error"
            logger.info(f"Error email has been sent, MessageId: {response["MessageId"]}")

            return response

        # 添付ファイル付きメールの送信
        retries = 0
        while retries < 2:
            try:
                ses_v2 = boto3.client("sesv2")
                response = ses_v2.send_email(
                    FromEmailAddress=f"{sender_name} <{sender_address}>",
                    Destination={"ToAddresses": to, "CcAddresses": cc, "BccAddresses": bcc},
                    Content={
                        "Raw": {"Data": msg.as_string()},
                    },
                )
                response["email_type"] = "normal"
                logger.info(f"Email has been sent, MessageId: {response["MessageId"]}")
                break
            except botocore.exceptions.ClientError as e:
                logger.error(f"Failed to send email, attempt {retries + 1}/2. Error: {e}")
                retries += 1

                # メールのリトライに失敗した場合、エラーメール通知を送信
                if retries >= 2:
                    try:
                        payload_error["error_message"] = e.response['Error']['Message']
                        response = self.send_mail(
                            template_name=MailType.MAIL_ERROR,
                            # エラーメール通知の場合、宛先TOにシステム管理者を設定
                            to=cc,
                            cc=[],
                            payload=payload_error
                        )
                        response["email_type"] = "error"
                        logger.info(f"Error email has been sent, MessageId: {response["MessageId"]}")
                    except Exception as send_error:
                        logger.error(f"Failed to send error email: {send_error}")
                        response = None

        return response
