from app.core.settings.app import AppSettings


class DevAppSettings(AppSettings):
    docs_url = "/docs"
    redoc_url = "/redoc"
    openapi_url = "/openapi.json"

    class Config:
        env_file = ".env.dev"
