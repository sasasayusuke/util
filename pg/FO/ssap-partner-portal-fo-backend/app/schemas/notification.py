from datetime import datetime

from app.schemas.base import CustomBaseModel
from pydantic import Field


class GetNotificationsByMineResponse(CustomBaseModel):
    """お知らせ情報取得レスポンスクラス"""

    id: str = Field(..., title="お知らせID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    user_id: str = Field(
        ..., title="ユーザーID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    summary: str = Field(..., title="お知らせタイトル", example="カルテを記入してください。")
    url: str = Field(
        None,
        title="お知らせリンク",
        example="https://example.com/project-survey/89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    message: str = Field(None, title="お知らせ文言", example="支援が完了したのでカルテを記入してください。")
    confirmed: bool = Field(..., title="確認済フラグ")
    noticed_at: datetime = Field(
        ..., title="お知らせ日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )


class PatchCheckNotificationsResponse(CustomBaseModel):
    """お知らせ情報を既読に更新レスポンスクラス"""

    message: str = Field(..., title="メッセージ", example="OK")
