from datetime import datetime
from typing import List

from fastapi import Query
from pydantic import Field, conint, validator

from app.resources.const import (
    SurveyQuestionsFormat,
    SurveyQuestionsSummaryType,
    SurveyRevisionStatus,
    SurveyTiming,
    SurveyType,
)
from app.schemas.base import CustomBaseModel


class OriginRevisionQuery(CustomBaseModel):
    origin_revision: int = Query(..., title="コピー元revision")


class QuestionsChoicesGroup(CustomBaseModel):
    """アンケートマスタ クエスチョンチョイスグループ クラス"""

    id: str = Field(None, title="選択肢ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    title: str = Field(None, title="選択肢グループ")
    disabled: bool = Field(False, title="無効（出題時に表示しない）")
    is_new: bool = Field(True, title="今バージョンで初めて追加された選択肢か（運用中になったらfalseに更新）")


class QuestionsChoices(CustomBaseModel):
    """アンケートマスタ クエスチョンチョイス クラス"""

    description: str = Field(None, title="回答選択肢グループ見出し", example="支援者の対応姿勢")
    group: List[QuestionsChoicesGroup] = Field(None, title="選択肢グループ")
    is_new: bool = Field(True, title="今バージョンで初めて追加された選択肢グループか（運用中になったらfalseに更新）")


class SurveyMastersQuestions(CustomBaseModel):
    """アンケートマスタ クエスチョン クラス"""

    id: str = Field(..., title="設問ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    required: bool = Field(..., title="必須か否か")
    description: str = Field(..., title="設問説明", example="当月のSSAP支援にはご満足いただけましたか？")
    format: SurveyQuestionsFormat = Field(
        ...,
        title="入力フォーマット（checkbox: チェックボックス、radio:ラジオボタン、selectbox:プルダウンリスト、textarea:テキストエリア）",
        example="checkbox",
    )
    summary_type: SurveyQuestionsSummaryType = Field(
        None,
        title="集計タイプ（ラジオボタンのみ）<br> （normal: 通常の設問、point:点数形式、satisfaction:満足度、continuation:継続意思、recommended:紹介可能性、sales:営業評価、survey_satisfaction:カルテ機能満足度、man_hour_satisfaction:工数機能満足度、karte_satisfaction:カルテ機能満足度、master_karte_satisfaction:マスターカルテ満足度）",
        example="point",
    )
    choices: List[QuestionsChoices] = Field(None, title="選択肢")
    other_description: str = Field(None, title="回答任意入力見出し", example="その他記載欄（任意）")
    disabled: bool = Field(
        False,
        title="無効（出題時に表示しない）",
    )
    is_new: bool = Field(True, title="今バージョンで初めて追加された設問か（運用中になったらfalseに更新）")


class SurveyMastersQuestionsFlow(CustomBaseModel):
    """アンケートマスタ クエスチョンフロー クラス"""

    id: str = Field(..., title="設問ID")
    condition_id: str = Field(..., title="条件分岐元設問ID")
    condition_choice_ids: set = Field(None, title="条件分岐選択肢ID（空配列:条件なし、選択肢ID（複数可））")


class CreateSurveyMastersRequest(CustomBaseModel):
    """アンケートマスタ作成リクエストクラス"""

    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    type: SurveyType = Field(
        ...,
        title="アンケート種別（service:サービス、completion:修了、quick:クイック、pp:PartnarPortal）",
        example="quick",
    )
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(
        ..., title="送信日初期設定（0:なし、1～31:毎月○日、-30～-1:回答期限日から○営業日前）", example=20
    )
    init_answer_limit_day_setting: int = Field(
        ...,
        title="回答期限初期設定（0:なし、99:月末最終営業日、1～30:○営業日後, 101～130:翌月月初○営業日(営業日に+100した数値)）",
        example=5,
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    questions: List[SurveyMastersQuestions] = Field(..., title="設問")
    question_flow: List[SurveyMastersQuestionsFlow] = Field(..., title="出題フロー")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")

    @validator("type")
    def check_type(cls, v):
        if SurveyType.QUICK != v:
            raise ValueError("Only quick can be specified.")
        return v


class CreateSurveyMastersResponse(CustomBaseModel):
    """アンケートマスタ作成レスポンスクラス"""

    id: str = Field(
        ..., title="アンケートマスタID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    type: SurveyType = Field(
        ...,
        title="アンケート種別（service:サービス、completion:修了、quick:クイック、pp:PartnarPortal）",
        example="quick",
    )
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(
        ..., title="送信日初期設定（0:なし、1～31:毎月○日、-30～-1:回答期限日から○営業日前）", example=20
    )
    init_answer_limit_day_setting: int = Field(
        ...,
        title="回答期限初期設定（0:なし、99:月末最終営業日、1～30:○営業日後, 101～130:翌月月初○営業日(営業日に+100した数値)）",
        example=5,
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    questions: List[SurveyMastersQuestions] = Field(..., title="設問")
    question_flow: List[SurveyMastersQuestionsFlow] = Field(..., title="出題フロー")
    is_latest: int = Field(..., title="最新バージョンだと1それ以外は0")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時（JST）", example="020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class GetSurveyMastersQuery(CustomBaseModel):
    name: str = Query(None, title="名称")
    survey_type: SurveyType = Query(None, title="アンケート種別")
    latest: bool = Query(True, title="最新バージョンのみ取得")
    in_operation: bool = Query(True, title="下書きを除く運用中のアンケートマスタのみを取得するか")


class SurveyMaster(CustomBaseModel):
    id: str = Field(..., title="データID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d")
    revision: int = Field(..., title="バージョン番号（大きいものがより最新）", example="1")
    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    type: SurveyType = Field(
        ...,
        title="アンケート種別（service:サービス、completion:修了、quick:クイック、pp:PartnarPortal）",
        example="quick",
    )
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(
        ..., title="送信日初期設定（0:なし、1～31:毎月○日、-30～-1:回答期限日から○営業日前）", example=20
    )
    init_answer_limit_day_setting: int = Field(
        ...,
        title="回答期限初期設定（0:なし、99:月末最終営業日、1～30:○営業日後, 101～130:翌月月初○営業日(営業日に+100した数値)）",
        example=5,
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    is_latest: int = Field(..., title="最新バージョンだと1それ以外は0")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")
    status: SurveyRevisionStatus = Field(
        ..., title="ステータス (draft: 下書き、archive: アーカイブ、in_operation: 運用中"
    )
    question_count: int = Field(..., title="設問数")
    update_at: datetime = Field(
        ..., title="更新日時", example="2020-10-23T03:21:39.356+09:00"
    )


class GetSurveyMastersResponse(CustomBaseModel):
    """アンケートマスタ一覧取得レスポンスクラス"""

    total: int = Field(..., title="件数", example=10)
    masters: List[SurveyMaster]


class GetSurveyMastersByIdSurveyMasterResponse(CustomBaseModel):
    """アンケートマスタ単一取得 アンケートマスタレスポンスクラス"""

    id: str = Field(
        ..., title="アンケートマスタID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    revision: int = Field(..., title="バージョン番号（大きいものがより最新）", example=1)
    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    type: SurveyType = Field(
        ...,
        title="アンケート種別（service:サービス、completion:修了、quick:クイック、pp:PartnarPortal）",
        example="service",
    )
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(
        ..., title="送信日初期設定（0:なし、1～31:毎月○日、-30～-1:回答期限日から○営業日前）", example=20
    )
    init_answer_limit_day_setting: int = Field(
        ...,
        title="回答期限初期設定（0:なし、99:月末最終営業日、1～30:○営業日後, 101～130:翌月月初○営業日(営業日に+100した数値)）",
        example=5,
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    is_latest: int = Field(..., title="最新バージョンだと1それ以外は0")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")
    status: SurveyRevisionStatus = Field(
        ...,
        title="ステータス (draft: 下書き、archive: アーカイブ、in_operation: 運用中)",
        example="in_operation",
    )
    question_count: int = Field(..., title="設問数", example=1)
    update_at: datetime = Field(
        ..., title="更新日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )


class GetSurveyMastersByIdResponse(CustomBaseModel):
    """アンケートマスタ単一取得レスポンスクラス"""

    total: conint(ge=0) = Field(..., title="件数")
    masters: List[GetSurveyMastersByIdSurveyMasterResponse]


class GetSurveyMasterByIdAndRevisionResponse(CustomBaseModel):
    """アンケートマスタバージョン単一取得レスポンスクラス"""

    id: str = Field(
        ..., title="アンケートマスタID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    revision: int = Field(..., title="バージョン番号（大きいものがより最新）", example=1)
    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    type: SurveyType = Field(
        ...,
        title="アンケート種別（service:サービス、completion:修了、quick:クイック、pp:PartnarPortal）",
        example="quick",
    )
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(
        ..., title="送信日初期設定（0:なし、1～31:毎月○日、-30～-1:回答期限日から○営業日前）", example=20
    )
    init_answer_limit_day_setting: int = Field(
        ...,
        title="回答期限初期設定（0:なし、99:月末最終営業日、1～30:○営業日後, 101～130:翌月月初○営業日(営業日に+100した数値)）",
        example=5,
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    questions: List[SurveyMastersQuestions] = Field(..., title="設問")
    question_flow: List[SurveyMastersQuestionsFlow] = Field(..., title="出題フロー")
    is_latest: int = Field(..., title="最新バージョンだと1それ以外は0")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時（JST）", example="020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_user_name: str = Field(None, title="更新ユーザ名", example="ソニー太郎")
    update_at: datetime = Field(
        None, title="更新日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class CopySurveyMastersByIdResponse(CustomBaseModel):
    """アンケートマスタ詳細変更レスポンスクラス"""

    id: str = Field(
        ..., title="アンケートマスタID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    revision: int = Field(..., title="バージョン番号（大きいものがより最新）", example=1)
    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    type: SurveyType = Field(
        ...,
        title="アンケート種別（service:サービス、completion:修了、quick:クイック、pp:PartnarPortal）",
        example="quick",
    )
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(..., title="送信日初期設定（0:なし）", example=20)
    init_answer_limit_day_setting: int = Field(
        ..., title="回答期限初期設定（0:なし、99:月末日）", example=5
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    questions: List[SurveyMastersQuestions] = Field(..., title="設問")
    question_flow: List[SurveyMastersQuestionsFlow] = Field(..., title="出題フロー")
    is_latest: int = Field(..., title="最新バージョンだと1それ以外は0")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class PatchSurveyMasterRevisionByIdQuery(CustomBaseModel):
    """アンケートマスタステータス変更クエリクラス"""

    version: int = Query(..., title="ロックキー（楽観ロック制御）", example="1")


class PatchSurveyMasterRevisionByIdResponse(CustomBaseModel):
    """アンケートマスタステータス変更レスポンスクラス"""

    status: SurveyRevisionStatus = Field(
        ...,
        title="ステータス (draft: 下書き、archive: アーカイブ、in_operation: 運用中)",
        example="in_operation",
    )
    revision: int = Field(..., title="バージョン番号（大きいものがより最新）", example=1)
    memo: str = Field(None, title="バージョンメモ（更新メモ）")
    question_count: int = Field(..., title="設問数", example=1)
    update_at: datetime = Field(
        None, title="更新日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )


class UpdateSurveyMasterLatestByIdRequest(CustomBaseModel):
    """アンケートマスタ詳細情報更新リクエストクラス"""

    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    type: SurveyType = Field(
        ...,
        title="アンケート種別（service:サービス、completion:修了、quick:クイック、pp:PartnarPortal）",
        example="quick",
    )
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(
        ..., title="送信日初期設定（0:なし、1～31:毎月○日、-30～-1:回答期限日から○営業日前）", example=20
    )
    init_answer_limit_day_setting: int = Field(
        ...,
        title="回答期限初期設定（0:なし、99:月末最終営業日、1～30:○営業日後, 101～130:翌月月初○営業日(営業日に+100した数値)）",
        example=5,
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")


class UpdateSurveyMasterLatestByIdResponse(CustomBaseModel):
    """アンケートマスタ詳細情報更新レスポンスクラス"""

    id: str = Field(
        ..., title="アンケートマスタID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    revision: int = Field(..., title="バージョン番号（大きいものがより最新）", example=1)
    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    type: SurveyType = Field(
        ...,
        title="アンケート種別（service:サービス、completion:修了、quick:クイック、pp:PartnarPortal）",
        example="quick",
    )
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(
        ..., title="送信日初期設定（0:なし、1～31:毎月○日、-30～-1:回答期限日から○営業日前）", example=20
    )
    init_answer_limit_day_setting: int = Field(
        ...,
        title="回答期限初期設定（0:なし、99:月末最終営業日、1～30:○営業日後, 101～130:翌月月初○営業日(営業日に+100した数値)）",
        example=5,
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    is_latest: int = Field(..., title="最新バージョンだと1それ以外は0")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example="1")


class UpdateSurveyMasterDraftByIdRequest(CustomBaseModel):
    """アンケートマスタ下書き更新リクエストクラス"""

    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(
        ..., title="送信日初期設定（0:なし、1～31:毎月○日、-30～-1:回答期限日から○営業日前）", example=20
    )
    init_answer_limit_day_setting: int = Field(
        ...,
        title="回答期限初期設定（0:なし、99:月末最終営業日、1～30:○営業日後, 101～130:翌月月初○営業日(営業日に+100した数値)）",
        example=5,
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    questions: List[SurveyMastersQuestions] = Field(..., title="設問")
    question_flow: List[SurveyMastersQuestionsFlow] = Field(..., title="出題フロー")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")


class UpdateSurveyMasterDraftByIdResponse(CustomBaseModel):
    """アンケートマスタ下書き更新レスポンスクラス"""

    id: str = Field(
        ..., title="アンケートマスタID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    revision: int = Field(..., title="バージョン番号（大きいものがより最新）", example=1)
    name: str = Field(..., title="テンプレート名", example="修了アンケート")
    type: SurveyType = Field(
        ...,
        title="アンケート種別（service:サービス、completion:修了、quick:クイック、pp:PartnarPortal）",
        example="quick",
    )
    timing: SurveyTiming = Field(
        ...,
        title="アンケート頻度（monthly:毎月、monthly_not_completion_month:毎月（修了月除く）、completion_month:修了月、anytime:任意）",
        example="monthly",
    )
    init_send_day_setting: int = Field(
        ..., title="送信日初期設定（0:なし、1～31:毎月○日、-30～-1:回答期限日から○営業日前）", example=20
    )
    init_answer_limit_day_setting: int = Field(
        ...,
        title="回答期限初期設定（0:なし、99:月末最終営業日、1～30:○営業日後, 101～130:翌月月初○営業日(営業日に+100した数値)）",
        example=5,
    )
    is_disclosure: bool = Field(..., title="支援者への開示の問があるか")
    questions: List[SurveyMastersQuestions] = Field(..., title="設問")
    question_flow: List[SurveyMastersQuestionsFlow] = Field(..., title="出題フロー")
    is_latest: int = Field(..., title="最新バージョンだと1それ以外は0")
    memo: str = Field(None, title="バージョンメモ（更新メモ）")
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時（JST）", example="2020-10-23T03:21:39.356+09:00"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example="1")
