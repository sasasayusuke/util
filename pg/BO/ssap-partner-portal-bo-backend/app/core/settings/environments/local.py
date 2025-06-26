from app.core.settings.app import AppSettings


class LocalAppSettings(AppSettings):
    docs_url = "/docs"
    redoc_url = "/redoc"
    openapi_url = "/openapi.json"
    servers = [
        {"url": "http://localhost:9000", "description": "Local Development Server"},
        {
            "url": "https://bo-api.dev.partner-portal.inhouse-sony-startup-acceleration-program.com",
            "description": "Development Server",
        },
    ]

    # トークンの有効期限設定の有効化・無効化
    enable_token_expiration: bool = False
    # トークン認証の有効化・無効化
    enable_token_authentication: bool = False

    class Config:
        env_file = ".env.local"
