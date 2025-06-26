from typing import List

from app.schemas.base import CustomBaseModel
from pydantic import Field


class HealthCheckResponse(CustomBaseModel):
    """死活監視レスポンスクラス"""

    results: List[str] = Field(..., title="結果", example=["Status OK"])
