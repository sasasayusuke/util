from datetime import datetime
from typing import List

from fastapi import Query
from pydantic import Field

from app.schemas.base import CustomBaseModel


class GetKartenResponse(CustomBaseModel):
    """案件カルテの一覧取得レスポンスクラス"""

    karte_id: str = Field(
        ..., title="案件カルテID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    date: str = Field(..., title="支援日", example="2022/05/20")
    start_time: str = Field(..., title="開始時間", example="10:00")
    end_time: str = Field(..., title="終了時間", example="13:00")
    is_draft: bool = Field(..., title="下書きか否か", example=False)
    last_update_datetime: datetime = Field(
        None, title="最終更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    update_user: str = Field(..., title="最終更新者", example="テスト ユーザー")


class GetKartenQuery(CustomBaseModel):
    """案件カルテの一覧取得クエリパラメータクラス"""

    project_id: str = Query(..., title="案件ID")


class DocumentInfo(CustomBaseModel):
    """documents情報"""

    file_name: str = Field(..., title="ファイル名")
    path: str = Field(..., title="パス ※S3保存パス")


class DeliverablesInfo(CustomBaseModel):
    """deliverables情報"""

    file_name: str = Field(..., title="ファイル名")
    path: str = Field(..., title="パス ※S3保存パス")


class Location(CustomBaseModel):
    type: str = Field(None, title="タイプ", example="online")
    detail: str = Field(None, title="場所の詳細", example="")


class GetKartenByIdResponse(CustomBaseModel):
    """IDで取得した案件カルテのレスポンスクラス"""

    karte_id: str = Field(
        ..., title="案件カルテID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    project_id: str = Field(
        ..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    date: str = Field(..., title="支援日", example="2022/05/20")
    start_datetime: str = Field(..., title="支援開始日時", example="2022/01/20 10:00")
    start_time: str = Field(..., title="開始時間", example="10:00")
    end_time: str = Field(..., title="終了時間", example="13:00")
    draft_supporter_id: str = Field(
        ..., title="起票支援者ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    draft_supporter_name: str = Field(..., title="起票支援者名")
    last_update_datetime: datetime = Field(
        None, title="最終更新日時", example="2020-10-23T03:21:39.356+09:00"
    )
    start_support_actual_time: str = Field(
        None, title="支援実績時間（自）", example="10:00"
    )
    end_support_actual_time: str = Field(
        None, title="支援実績時間（至）", example="13:00"
    )
    man_hour: float = Field(0, title="実施時間（工数h）")
    customers: str = Field(None, title="お客様（敬称略）")
    support_team: str = Field(None, title="SAP支援チーム（敬称略）")
    detail: str = Field(None, title="支援実施内容")
    feedback: str = Field(None, title="フィードバック")
    homework: str = Field(None, title="宿題")
    documents: List[DocumentInfo] = Field(..., title="資料")
    deliverables: List[DeliverablesInfo] = Field(..., title="成果物")
    memo: str = Field(None, title="申し送り")
    human_resource_needed_for_customer: str = Field(
        None, title="お客様に不足している人的リソース"
    )
    company_and_industry_recommended_to_customer: str = Field(
        None, title="お客様に紹介したい企業や産業"
    )
    human_resource_needed_for_support: str = Field(
        None, title="SAP支援チームに補充したい人的リソース"
    )
    task: str = Field(None, title="現状の課題")
    is_draft: bool = Field(..., title="下書きか否か")
    location: Location = Field(None, title="場所")
    update_id: str = Field(
        ..., title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_user_name: str = Field(None, title="更新ユーザー名", example="ソニー太郎")
    update_at: datetime = Field(
        None, title="更新日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(
        None, title="ロックキー（楽観ロック制御）", minimum=1, example=1
    )
