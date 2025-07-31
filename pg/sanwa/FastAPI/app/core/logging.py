import sys
import logging
import contextvars
from logging.handlers import RotatingFileHandler
from urllib.parse import unquote
from app.core.config import settings


# リクエストIDを保持するコンテキスト変数
request_id_ctx = contextvars.ContextVar('request_id', default='no-request-id')

class RequestIDFilter(logging.Filter):
    """ログにリクエストIDを追加するフィルター"""
    def filter(self, record):
        record.request_id = request_id_ctx.get()
        return True

def setup_logging():
    """環境に応じたロギング設定を行う関数"""

    # 共通のログフォーマット
    log_format = '%(asctime)s - %(name)s - [RequestID:%(request_id)s] - %(levelname)s - %(message)s'

    # 環境に応じた設定
    if settings.ENVIRONMENT == "product":
        config = {
            'level': logging.INFO,
            'log_file': "log/app_prod.log",
            'max_bytes': 100 * 1024 * 1024,
            'backup_count': 10,
        }
    else:
        config = {
            'level': logging.DEBUG,
            'log_file': "log/app_dev.log",
            'max_bytes': 10 * 1024 * 1024,
            'backup_count': 3,
        }

    logger = logging.getLogger(settings.APP_NAME)
    logger.setLevel(config['level'])
    logger.handlers.clear()

    # リクエストIDフィルターを追加
    logger.addFilter(RequestIDFilter())

    # ハンドラの設定
    handlers = [
        logging.StreamHandler(sys.stdout),
        RotatingFileHandler(
            config['log_file'],
            maxBytes=config['max_bytes'],
            backupCount=config['backup_count'],
            encoding='utf-8'
        )
    ]

    for handler in handlers:
        handler.setFormatter(logging.Formatter(log_format))
        logger.addHandler(handler)

    return logger