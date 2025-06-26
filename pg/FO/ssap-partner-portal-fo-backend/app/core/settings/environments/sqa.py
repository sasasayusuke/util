from app.core.settings.app import AppSettings


class SqaAppSettings(AppSettings):
    class Config:
        env_file = ".env.sqa"
