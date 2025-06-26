from app.core.settings.app import AppSettings


class SupAppSettings(AppSettings):
    class Config:
        env_file = ".env.sup"
