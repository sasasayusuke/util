import os

from pydantic import BaseSettings


class BaseAppSettings(BaseSettings):
    env = os.getenv("STAGE", "local")
