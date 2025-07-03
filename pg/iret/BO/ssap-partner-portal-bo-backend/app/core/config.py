from functools import lru_cache
from typing import Dict, Type

from app.core.settings.app import AppSettings
from app.core.settings.base import BaseAppSettings
from app.core.settings.environments.dev import DevAppSettings
from app.core.settings.environments.evs import EvsAppSettings
from app.core.settings.environments.local import LocalAppSettings
from app.core.settings.environments.prd import PrdAppSettings
from app.core.settings.environments.sqa import SqaAppSettings
from app.core.settings.environments.sup import SupAppSettings
from app.resources.const import AppEnvTypes

environments: Dict[AppEnvTypes, Type[AppSettings]] = {
    AppEnvTypes.LOCAL.value: LocalAppSettings,
    AppEnvTypes.DEV.value: DevAppSettings,
    AppEnvTypes.SUP.value: SupAppSettings,
    AppEnvTypes.EVS.value: EvsAppSettings,
    AppEnvTypes.SQA.value: SqaAppSettings,
    AppEnvTypes.PRD.value: PrdAppSettings,
}


@lru_cache
def get_app_settings() -> AppSettings:
    env = BaseAppSettings().env
    config = environments[env]
    return config()
