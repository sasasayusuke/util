import os
import time
import uuid
import locale
import asyncio
from fastapi import FastAPI, Request, HTTPException
from fastapi.responses import JSONResponse
from fastapi.staticfiles import StaticFiles
from fastapi.middleware.cors import CORSMiddleware
from app.api import config_routes, sql_routes, excel_routes, service_routes, sync_routes,lock_routes
from app.core.config import settings
from app.core.logging import setup_logging, request_id_ctx


# ロケールを日本語に設定
locale.setlocale(locale.LC_TIME, 'ja_JP')


# ロギングを設定する
logger = setup_logging()

app = FastAPI(
    title=settings.APP_NAME,
    docs_url="/docs",               # Swagger UIのエンドポイント
    redoc_url="/redoc",             # ReDocのエンドポイント
    openapi_url="/openapi.json",    # OpenAPI仕様のJSONエンドポイント
)

# CORSミドルウェアの追加
app.add_middleware(
    CORSMiddleware,
    allow_origins=[
        f"http://{settings.PLEASANTER_HOST}",             # ポート80
        f"http://{settings.PLEASANTER_HOST}:80",          # 明示的なポート80
        f"http://{settings.PLEASANTER_HOST}:8000",        # API自身のポート
        f"https://{settings.PLEASANTER_HOST}",            # HTTPS
        f"https://{settings.PLEASANTER_HOST}:443"         # 明示的なHTTPSポート
    ],
    allow_credentials=True,
    allow_methods=["GET", "POST"],
    allow_headers=["*"],
    expose_headers=["Content-Type", "X-Custom-Header", "Content-Disposition", "Success-Message-Code", "Download-Skip"],
)


# 静的ファイルのマウント
# 1) pleasanter 用
static_dir_pleasanter = os.path.join(settings.APP_ROOT, 'public', 'pleasanter')
app.mount("/pleasanter", StaticFiles(directory=static_dir_pleasanter), name="pleasanter_static")

# 2) react 用
static_dir_react = os.path.join(settings.APP_ROOT, 'public', 'react')
app.mount("/react", StaticFiles(directory=static_dir_react), name="react_static")

# APIルーターの追加
app.include_router(config_routes.router, tags=["コンフィグ"])
app.include_router(service_routes.router, tags=["サービス"])
app.include_router(sql_routes.router, tags=["SQL"])
app.include_router(excel_routes.router, tags=["Excel"])
app.include_router(sync_routes.router, tags=["マスタ同期"])
app.include_router(lock_routes.router, tags=["ロック"])


settings.initialize_pleasanter_info()
settings.initialize_excel_info()

logger.info(settings.get_env())

@app.middleware("http")
async def log_requests(request: Request, call_next):
    """リクエストのログ記録ミドルウェア"""

    # リクエストIDを生成
    request_id = str(uuid.uuid4())
    # コンテキストに設定
    request_id_ctx.set(request_id)

    start_time = time.time()

    # リクエスト開始ログ
    logger.info(f"リクエスト受信: {request.method} {request.url}")

    # リクエスト処理
    response = await call_next(request)

    # 処理時間を計算
    process_time = time.time() - start_time

    # レスポンスヘッダーにトレース情報を追加
    response.headers.update({
        "X-Process-Time": f"{process_time:.3f}",
        "X-Request-ID": request_id
    })

    # 完了ログ
    logger.info(f"リクエスト完了: {request.method} {request.url}")

    return response

# HTTPExceptionのエラーハンドラー
@app.exception_handler(HTTPException)
async def http_exception_handler(request: Request, exc: HTTPException):
    message = exc.detail
    logger.exception(message)
    return JSONResponse(
        status_code=exc.status_code,
        content={
            "message": message,
            "code": exc.status_code,
            "path": str(request.url)
        }
    )