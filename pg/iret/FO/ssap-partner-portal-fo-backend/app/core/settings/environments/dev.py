from app.core.settings.app import AppSettings


class DevAppSettings(AppSettings):
    class Config:
        env_file = ".env.dev"
