from app.core.settings.app import AppSettings


class EvsAppSettings(AppSettings):
    class Config:
        env_file = ".env.evs"
