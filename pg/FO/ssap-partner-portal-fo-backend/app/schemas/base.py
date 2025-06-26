from datetime import datetime

from pydantic import BaseModel, Field


def to_camel(snake_str: str) -> str:
    components = snake_str.split("_")
    return components[0] + "".join(x.title() for x in components[1:])


def convert_datetime_to_iso_8601(dt: datetime) -> str:
    return dt.strftime("%Y-%m-%dT%H:%M:%S.%f")[:-3] + "+09:00"


class CustomBaseModel(BaseModel):
    class Config:
        alias_generator = to_camel
        allow_population_by_field_name = True
        json_encoders = {datetime: convert_datetime_to_iso_8601}


class OKResponse(CustomBaseModel):
    """API実行成功レスポンス"""

    message: str = Field("OK", title="メッセージ", example="OK")
