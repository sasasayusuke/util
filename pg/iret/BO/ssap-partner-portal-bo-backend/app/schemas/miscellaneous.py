from typing import List

from pydantic import Field

from app.schemas.base import CustomBaseModel


class HealthCheckResponse(CustomBaseModel):
    """死活監視レスポンスクラス"""

    results: List[str] = Field(..., title="結果", example=["Status OK"])
