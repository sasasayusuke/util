from fastapi import Query
from pydantic import Field

from app.resources.const import (
    SuggestSolverCorporationsSortType,
)
from app.schemas.base import CustomBaseModel


class SuggestSolverCorporationsQuery(CustomBaseModel):
    """法人名サジェスト用データ取得クエリクラス"""

    sort: SuggestSolverCorporationsSortType = Query(..., title="ソート")
    disabled: bool = Query(
        False,
        title="trueならば無効なユーザーを含む",
    )


class SuggestSolverCorporationsResponse(CustomBaseModel):
    """法人名サジェスト用データ取得レスポンスクラス"""

    id: str = Field(..., title="法人ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    name: str = Field(..., title="法人名", example="ソニーグループ株式会社")
