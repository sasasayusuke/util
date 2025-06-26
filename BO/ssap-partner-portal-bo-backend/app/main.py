import yaml

from fastapi import FastAPI, Request, Response
from fastapi.encoders import jsonable_encoder
from fastapi.responses import JSONResponse
from fastapi.openapi.utils import get_openapi
from mangum import Mangum
from starlette.exceptions import HTTPException as StarletteHTTPException
from starlette.middleware.cors import CORSMiddleware

from app.core.common_logging import CustomLogger
from app.core.config import get_app_settings
from app.middleware.logging import Logging
from app.routers import router

try:
    import unzip_requirements
except ImportError:
    pass

logger = CustomLogger.get_logger()


def get_application() -> FastAPI:
    settings = get_app_settings()
    settings.configure_logging()

    application = FastAPI(**settings.fastapi_kwargs)
    application.add_middleware(
        CORSMiddleware,
        allow_origins=get_app_settings().host.split(","),
        allow_credentials=True,
        allow_methods=["*"],
        allow_headers=["*"],
    )
    application.include_router(router)

    return application


app = get_application()


@logger.inject_aws_context
def handler(event, context):
    asgi_handler = Mangum(app)
    return asgi_handler(event, context)


@app.exception_handler(StarletteHTTPException)
async def http_exception_handler(
    request: Request, exc: StarletteHTTPException
) -> Response:
    """HTTP の通常のハンドリングで発生するエラーをハンドリングします。

    Args:
        request (Request): クライアントからのリクエスト
        exc (StarletteHTTPException): HTTP ステータスコードを伴うエラー

    Returns:
        Response: エラー情報が格納された JSON オブジェクト
    """
    logger.info(
        jsonable_encoder(
            {
                "detail": str(exc.detail),
            }
        )
    )
    return JSONResponse(
        status_code=exc.status_code,
        content=jsonable_encoder(
            {
                "detail": str(exc.detail),
            }
        ),
    )


@app.middleware("http")
async def output_log(request: Request, call_next) -> Response:
    """実行対象の関数を実行する直前直後の処理
    Args:
        request (Request): クライアントからのリクエスト
        call_next (Callable): 次に実行する関数

    Returns:
        Response: 実行対象の関数から返されたレスポンス
    """

    logging = Logging(request, call_next)
    response = await logging.log_operation()

    return response


@app.middleware("http")
async def add_my_headers(request: Request, call_next):
    """レスポンスヘッダの文字コードをutf-8で指定"""

    response = await call_next(request)
    if "content-type" in response.headers:
        response.headers["content-type"] = (
            response.headers["content-type"] + "; charset=utf-8"
        )
    return response


def output_openapi() -> None:
    """OpenAPI のスキーマを出力します。Github Actions で実行するために必要です。"""
    openapi_schema = get_openapi(
        routes=app.routes,
        **get_app_settings().openapi_kwargs,
    )
    app.openapi_schema = openapi_schema
    yaml.dump(app.openapi())
    print(yaml.dump(app.openapi(), sort_keys=False))
