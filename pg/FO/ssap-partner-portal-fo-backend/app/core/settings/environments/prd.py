from app.core.settings.app import AppSettings


class PrdAppSettings(AppSettings):
    class Config:
        env_file = ".env.prd"
