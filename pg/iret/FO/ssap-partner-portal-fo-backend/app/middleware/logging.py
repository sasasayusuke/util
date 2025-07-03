import json
import time

from app.core.common_logging import CustomLogger
from fastapi import Request, Response
from starlette.types import Message

logger = CustomLogger.get_logger()


class Logging:
    """ミドルウェアで使用するログ出力クラス"""

    def __init__(self, request: Request, call_next) -> None:
        self.request = request
        self.call_next = call_next
        self.start_time = None

    def re_masking_instance(log_dict: dict):
        """ログの認証情報を再帰処理をしながらマスキングする"""

        for i in log_dict:
            # dictionary 構造を保ちながら処理をする
            # json 構造のリクエストボディを処理する
            if isinstance(i, str):
                if isinstance(log_dict[i], dict):
                    Logging.re_masking_instance(log_dict[i])
                elif isinstance(log_dict[i], list):
                    for list_itr in log_dict[i]:
                        if isinstance(list_itr, dict):
                            Logging.re_masking_instance(list_itr)
                else:
                    if (
                        i == "email"
                        or i == "name"
                        or i == "salesforceMainSupporterUserName"
                        or i == "salesforceSupporterUserNames"
                    ):
                        # "salesforceMainSupporterUserName"と
                        # "salesforceSupporterUserNames" はprojectAPI 用
                        # 文字数分のアスタリスクでマスク処理。Noneまたは空の場合は、そのままセット。
                        log_dict[i] = (
                            "*" * len(log_dict[i]) if log_dict[i] else log_dict[i]
                        )

    async def log_operation(self) -> Response:
        """ミドルウェアでのログ出力処理を行う

        Returns:
            Response: レスポンス本体
        """
        await self._output_request(self.request)
        self._start_measuring_processing_speed()

        # 実行関数の呼び出し
        response: Response = await self.call_next(self.request)

        self._output_response(response)
        self._output_calculated_processing_time()

        return response

    async def _output_request(self, request: Request) -> None:
        """Request内容をログに出力する

        Args:
            request (Request): Requestの内容
        """

        request_log = {}
        request_log["host"] = request.client.host
        request_log["path"] = request.url.path
        request_log["query_params"] = {k: v for (k, v) in request.query_params.items()}
        request_log["method"] = request.method
        # 認証情報を出力しない
        headers_items = {}
        for (k, v) in request.headers.raw:
            if k.decode("utf-8") != "authorization":
                headers_items[k.decode("utf-8")] = v.decode("utf-8")
        request_log["headers"] = headers_items
        try:
            request_body = {}
            request_body = json.loads(await self._get_body(request))
            # 名前とメールアドレスをログ出力させない
            Logging.re_masking_instance(request_body)

            request_log["request_body"] = request_body
        except json.JSONDecodeError:
            request_log["request_body"] = (await self._get_body(request)).decode(
                "utf-8"
            )

        logger.info(json.dumps(request_log))
        self.start_time = time.time()

    def _output_response(self, response: Response) -> None:
        """Response内容をログに出力する

        Args:
            response (Response): Responseの内容
        """
        process_time = round(time.time() - self.start_time, 4)

        response_log = {}
        response_log["status_code"] = response.status_code
        response_log["headers"] = {
            k.decode("utf-8"): v.decode("utf-8") for (k, v) in response.headers.raw
        }
        logger.info(json.dumps(response_log))

        logger.info("process_time: {0}[sec]".format(str(process_time)))

    def _start_measuring_processing_speed(self) -> None:
        """実行関数の計測を開始する"""
        self.start_time = time.time()

    def _output_calculated_processing_time(self) -> None:
        """実行した関数の処理時間を計算しログに出力する"""
        process_time = time.time() - self.start_time
        logger.info("process_time: {0}[sec]".format(str(process_time)))

    async def _set_body(self, request: Request):
        receive_ = await request._receive()

        body = receive_.get('body')
        # Make all changes to the body object here and return the modified request

        async def receive() -> Message:
            receive_["body"] = body
            return receive_

        request._receive = receive

    async def _get_body(self, request: Request) -> bytes:
        body = await request.body()
        self._set_body(request)
        return body
