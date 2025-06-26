import logging


class CustomLoggerAdapter(logging.LoggerAdapter):
    def __init__(self, logger):
        super().__init__(logger, extra={})
        self.aws_request_id = ""

    def inject_aws_context(self, lambda_handler):
        def wrapper(event, context):
            self.aws_request_id = context.aws_request_id
            return lambda_handler(event, context)

        return wrapper

    # logging.LoggerAdapterのprocessをオーバーライド
    # メッセージの前にaws_request_id を付与
    def process(self, msg, kwargs):
        kwargs["extra"] = self.extra
        return "[%s] %s" % (self.aws_request_id, msg), kwargs


class CustomLogger:
    __instance: CustomLoggerAdapter = None

    @classmethod
    def get_logger(cls):
        if not cls.__instance:
            cls.__instance = CustomLoggerAdapter(logging.getLogger(__name__))

        return cls.__instance
