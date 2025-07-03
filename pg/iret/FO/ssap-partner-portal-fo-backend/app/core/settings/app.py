import pathlib
from logging.config import fileConfig
from typing import Dict

from app.core.settings.base import BaseAppSettings
from app.resources.const import AppEnvTypes


class AppSettings(BaseAppSettings):
    docs_url = ""
    redoc_url = ""
    openapi_url = ""
    servers = []

    host: str

    aws_region: str
    sqs_email_queue_name: str
    table_prefix: str
    upload_s3_bucket_name: str
    cognito_pool_id: str
    jwk_url_base: str = "https://cognito-idp.{cognito_region}.amazonaws.com/{cognito_pool_id}/.well-known/jwks.json"
    secret_name: str

    # メールテンプレート
    email_template: str
    # メール送信元アドレス
    sender_name: str
    sender_address: str

    # ログ設定
    logging_settings: str

    # URL
    fo_site_url: str
    bo_site_url: str

    # PF API
    pf_api_base_url: str

    # Salesforce連携メールアドレス
    salesforce_address_for_solver_corporation: str
    salesforce_address_for_solver: str

    #############################
    #  開発用
    #############################
    # ローカルでトークンの有効期限の設定を有効化したい場合は、LocalAppSettingsをTrueに設定してください。
    enable_token_expiration: bool = True
    # ローカルでトークン認証を有効化したい場合は、LocalAppSettingsをTrueに設定してください。
    enable_token_authentication: bool = True

    @property
    def fastapi_kwargs(self) -> Dict[str, str]:
        return {
            "docs_url": self.docs_url,
            "openapi_url": self.openapi_url,
            "redoc_url": self.redoc_url,
            "title": "SSAP Partner Portal FrontOffice API",
            "version": "1.3.0",
            "servers": self.servers,
        }

    @property
    def openapi_kwargs(self) -> Dict[str, str]:
        return {
            "title": "SSAP Partner Portal FrontOffice API",
            "version": "1.3.0",
            "summary": "SSAP Partner Portal FrontOffice API",
            "openapi_version": "3.0.2",
        }

    def configure_logging(self) -> None:
        if self.env == AppEnvTypes.LOCAL.value:
            pathlib.Path(".logs/").mkdir(parents=True, exist_ok=True)
        fileConfig(self.logging_settings, disable_existing_loggers=False)
