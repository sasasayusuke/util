from datetime import datetime
from typing import Any, List

from fastapi import Query
from pydantic import Field, root_validator

from app.resources.const import ScheduleTiming, SurveyTypeExcludingPP
from app.schemas.base import CustomBaseModel


class ProjectSupportScheduleInfo(CustomBaseModel):
    "支援スケジュール情報リストスクラス"

    year_month: str = Field(..., title="対象年月", example="2022/01")
    support_date: str = Field(..., title="支援日", example="2022/04/01")
    support_start_time: str = Field(..., title="支援開始時刻", example="10:00")
    support_end_time: str = Field(..., title="支援終了時刻", example="13:00")
    completed: bool = Field(..., title="カルテが作成済で公開(true:公開 false:非公開)")
    karte_id: str = Field(
        ..., title="カルテID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    is_accessible_karte_detail: bool = Field(..., title="個別カルテ詳細画面にアクセスできるか")
    last_update_datetime: datetime = Field(
        None, title="最終更新日時", example="2020-10-23T03:21:39.356872+09:00"
    )
    update_user: str = Field(..., title="最終更新者", example="テスト ユーザー")
    version: int = Field(..., title="ロックキー（楽観ロック制御）", example=1)


class CreateSupportSchedulesRequest(CustomBaseModel):
    timing: ScheduleTiming = Field(
        ..., title="monthly: 毎月, weekly: 毎週, once: 1回", example="monthly"
    )
    support_date: str = Field(..., title="支援日", example="2022/04/01")
    start_time: str = Field(..., title="支援開始時間", example="13:00")
    end_time: str = Field(..., title="支援終了時間", example="14:00")

    @root_validator
    def check_combination(cls, v):
        timing = v.get("timing")
        support_date = v.get("support_date")

        if timing == ScheduleTiming.MONTHLY:
            if not 1 <= int(support_date) <= 31:
                raise ValueError("specify between 1 and 31.")

        elif timing == ScheduleTiming.WEEKLY:
            if not 0 <= int(support_date) <= 6:
                raise ValueError("specify between 0 and 6.")

        elif timing == ScheduleTiming.ONCE:
            try:
                datetime.strptime(support_date, "%Y/%m/%d")
            except ValueError:
                raise ValueError("specify in the format yyyy/mm/dd.")

        try:
            datetime.strptime(v.get("start_time"), "%H:%M")
            datetime.strptime(v.get("end_time"), "%H:%M")
        except ValueError:
            raise ValueError("specify in the format hh:mm.")

        return v


class CreateSupportSchedulesResponse(CustomBaseModel):
    message: str = Field("OK", title="メッセージ", example="OK")


class GetSupportSchedulesByIdResponse(CustomBaseModel):
    "案件スケジュール一覧レスポンスクラス"

    project_id: str = Field(
        ..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    project_schedules: List[ProjectSupportScheduleInfo] = Field(
        ..., title="プロジェクトスケジュールリスト"
    )


class PutSupportSchedulesByIdDateQuery(CustomBaseModel):
    """支援スケジュール更新クエリパラメータクラス"""

    karte_id: str = Query(..., title="カルテID")
    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class SupportScheduleInfo(CustomBaseModel):
    year_month: str = Field(..., title="対象年月", example="2022/01")
    schedule_id: str = Field(
        ..., title="スケジュールID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    schedule_group_id: str = Field(
        None,
        title="スケジュールグループID（一括変更制御用）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    support_date: str = Field(..., title="支援日", example="2022/04/01")
    support_start_time: str = Field(..., title="支援開始時刻", example="10:00")
    support_end_time: str = Field(..., title="支援終了時刻", example="13:00")
    karte_id: str = Field(
        ..., title="カルテID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    version: int = Field(None, title="ロックキー（楽観ロック制御）", example=1)
    create_id: str = Field(
        ..., title="作成ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    create_at: datetime = Field(
        ..., title="作成日時（date-time）", example="2020-10-23T03:21:39.356+09:00"
    )
    update_id: str = Field(
        None, title="更新ID", example="fa5c9f96-af22-bbc6-ac4e-cbcb25ad18e7"
    )
    update_at: datetime = Field(
        None, title="更新日時（date-time）", example="2020-10-23T03:21:39.356+09:00"
    )


class PutSupportScheduleIdDateRequest(CustomBaseModel):
    """支援スケジュール更新リクエストクラス"""

    support_date: str = Field(..., title="支援日", example="2022/05/01")
    support_start_time: str = Field(..., title="支援開始時間(hh:mm)", example="12:00")
    support_end_time: str = Field(..., title="支援終了時間(hh:mm)", example="15:00")

    @root_validator
    def check_combination(cls, v):
        try:
            datetime.strptime(v.get("support_date"), "%Y/%m/%d")
        except ValueError:
            raise ValueError("specify in the format yyyy/mm/dd.")

        try:
            datetime.strptime(v.get("support_start_time"), "%H:%M")
            datetime.strptime(v.get("support_end_time"), "%H:%M")
        except ValueError:
            raise ValueError("specify in the format hh:mm.")

        return v


class PutSupportScheduleIdDateResponse(CustomBaseModel):
    """支援スケジュール更新結果レスポンスクラス"""

    message: str = Field(..., title="メッセージ", example="OK")


class DeleteSupportSchedulesByIdDateQuery(CustomBaseModel):
    """支援スケジュールクエリパラメータクラス"""

    karte_id: str = Query(..., title="スケジュールID")
    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class DeleteScheduleIdDateResponse(CustomBaseModel):
    """支援スケジュール削除結果レスポンスクラス"""

    message: str = Field(..., title="メッセージ", example="OK")


class ProjectSurveyScheduleInfo(CustomBaseModel):
    "アンケート案件スケジュール情報リストスクラス"

    schedule_group_id: str = Field(
        ...,
        title="スケジュールグループID（一括変更制御用）",
        example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    survey_id: str = Field(
        ..., title="アンケートID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    send_date: str = Field(..., title="送信日", example="2022/01/01")
    survey_name: str = Field(
        ...,
        title="アンケート名（アンケート種別 service:サービス、completion:修了、quick:クイック）",
        example="service",
    )
    survey_limit_date: str = Field(
        None, title="アンケート回答期限日（回答期限なしの場合は空文字）", example="2022/01/31"
    )
    version: int = Field(..., title="ロックキー（楽観ロック制御）", example=1)


class GetSurveySchedulesByIdResponse(CustomBaseModel):
    "アンケート案件スケジュール一覧レスポンスクラス"

    project_id: str = Field(
        ..., title="案件ID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    survey_group_id: str = Field(
        None,
        title="新規一括登録時のスケジュールグループID",
        example="8ddbe2ed-f44c-4a1c-9408-c67b0ca2270d",
    )
    project_schedules: List[ProjectSurveyScheduleInfo] = Field(
        ..., title="プロジェクトスケジュールリスト"
    )


class CreateSurveySchedulesRequest(CustomBaseModel):
    survey_type: SurveyTypeExcludingPP = Field(..., title="アンケートタイプ", example="quick")
    survey_master_id: str = Field(
        None, title="アンケートマスタID", example="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d"
    )
    timing: ScheduleTiming = Field(
        ..., title="monthly: 毎月, weekly: 毎週, once: 1回", example="monthly"
    )
    request_date: Any = Field(
        ...,
        title="送信日(曜日指定:0-6, 毎月○日:1-31, 回答期限日から○営業日前:-30～-1, 特定日:2022/04/01)",
        example="2022/04/01",
    )
    limit_date: int = Field(
        ...,
        title="回答期限日(月末最終営業日:99, なし:0, 〇営業日後:1～30, 翌月月初○営業日:101～130(営業日に+100した数値))",
        example=99,
    )

    @root_validator
    def check_timing(cls, v):
        timing = v.get("timing")
        request_date = v.get("request_date")
        survey_type = v.get("survey_type")
        limit_date = v.get("limit_date")
        if survey_type in [
            SurveyTypeExcludingPP.SERVICE,
            SurveyTypeExcludingPP.COMPLETION,
        ]:
            if v.get("timing") is not ScheduleTiming.ONCE:
                raise ValueError(
                    "service and completion surveys can be specified only once."
                )

        """ 送信日の型チェック"""
        if type(request_date) is not str:
            raise ValueError("specify requestDate in string type.")

        """タイミングと送信日の組み合わせチェック

            timingが毎月の場合、以下の何れか
              ・送信日に1～31を指定
              ・送信日に-30～-1を指定
            timingが毎週の場合、送信日に0～6を指定
            timingが1回の場合、以下の何れか
              ・送信日に2022/04/01の形式で指定
              ・送信日に-30～-1を指定
        """
        if timing == ScheduleTiming.MONTHLY:
            if not ((1 <= int(request_date) <= 31) or (-30 <= int(request_date) <= -1)):
                raise ValueError("specify between 1 and 31 or between -30 and -1.")

        if timing == ScheduleTiming.WEEKLY:
            if not 0 <= int(request_date) <= 6:
                raise ValueError("Specify between 0 and 6.")

        if timing == ScheduleTiming.ONCE:
            try:
                datetime.strptime(str(v.get("request_date")), "%Y/%m/%d")
            except ValueError:
                if not (-30 <= int(request_date) <= -1):
                    # YYYY/MM/DD形式でなく、-30～-1でもない場合
                    raise ValueError(
                        "specify yyyy/mm/dd format or a value between -30 and -1."
                    )

        # 回答期限日チェック 99:月末最終営業日、1～30: ○営業日後, 0: なし, 101～130: 翌月月初○営業日(営業日に+100した数値)以外を弾く
        if not (
            (1 <= limit_date <= 30)
            or (limit_date in [0, 99])
            or (101 <= limit_date <= 130)
        ):
            raise ValueError("specify a valid number.")

        # requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
        try:
            datetime.strptime(str(v.get("request_date")), "%Y/%m/%d")
        except ValueError:
            if (-30 <= int(request_date) <= -1) and (1 <= limit_date <= 30):
                raise ValueError("Invalid combination of requestData and limitDate.")
        # requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の「なし」の同時指定は不可
        try:
            datetime.strptime(str(v.get("request_date")), "%Y/%m/%d")
        except ValueError:
            if (-30 <= int(request_date) <= -1) and (limit_date == 0):
                raise ValueError("Invalid combination of requestData and limitDate.")

        # タイミングが「1回」の場合
        if timing in [ScheduleTiming.ONCE]:
            # requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の「月末最終営業日」の同時指定は不可
            try:
                datetime.strptime(str(v.get("request_date")), "%Y/%m/%d")
            except ValueError:
                if (-30 <= int(request_date) <= -1) and (limit_date == 99):
                    raise ValueError(
                        "Invalid combination of requestData and limitDate."
                    )

        return v

    @root_validator
    def check_survey_master_id(cls, v):
        if v.get("survey_type") == SurveyTypeExcludingPP.QUICK.value:
            survey_master_id = v.get("survey_master_id")

            if survey_master_id is None:
                raise ValueError(
                    "If the survey type is quick, the Survey Master ID must be specified."
                )

        return v


class CreateSurveySchedulesResponse(CustomBaseModel):
    """アンケートスケジュール作成レスポンスクラス"""

    message: str = Field("OK", title="メッセージ", example="OK")


class PutSurveyScheduleIdDateQuery(CustomBaseModel):
    """アンケートスケジュール更新クエリパラメータクラス"""

    survey_id: str = Query(..., title="スケジュールID")
    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class PutSurveyScheduleIdDateRequest(CustomBaseModel):
    """アンケートスケジュール更新リクエストクラス"""

    send_date: Any = Field(
        ..., title="送信日(回答期限日から○営業日前:-30～-1, 特定日:2022/04/01)", example="2022/05/01"
    )
    survey_limit_date: int = Field(
        ...,
        title="回答期限日(99:月末最終営業日、1～30: ○営業日後, 0: なし, 101～130: 翌月月初○営業日(営業日に+100した数値))",
        example=99,
    )

    @root_validator
    def check_date(cls, v):
        send_date = v.get("send_date")
        survey_limit_date = v.get("survey_limit_date")

        """ 送信日の型チェック"""
        if type(send_date) is not str:
            raise ValueError("specify sendDate in string type.")

        # 送信日チェック 回答期限日から○営業日前:-30～-1, 特定日(e.g. 2022/04/01) 以外を弾く
        try:
            datetime.strptime(str(v.get("send_date")), "%Y/%m/%d")
        except ValueError:
            if not (-30 <= int(send_date) <= -1):
                # YYYY/MM/DD形式でなく、-30～-1でもない場合
                raise ValueError(
                    "specify yyyy/mm/dd format or a value between -30 and -1."
                )

        # sendDate(アンケート送信日時)の「回答期限日から○営業日前」とsurveyLimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
        try:
            datetime.strptime(str(v.get("send_date")), "%Y/%m/%d")
        except ValueError:
            if (-30 <= int(send_date) <= -1) and (1 <= survey_limit_date <= 30):
                raise ValueError("Invalid combination of sendDate and surveyLimitDate.")
        # sendDate(アンケート送信日時)の「回答期限日から○営業日前」とsurveyLimitDate(回答期限日)の「なし」の同時指定は不可
        try:
            datetime.strptime(str(v.get("send_date")), "%Y/%m/%d")
        except ValueError:
            if (-30 <= int(send_date) <= -1) and (survey_limit_date == 0):
                raise ValueError("Invalid combination of sendDate and surveyLimitDate.")

        # sendDate(アンケート送信日時)の「回答期限日から○営業日前」とsurveyLimitDate(回答期限日)の「月末最終営業日」の同時指定は不可
        try:
            datetime.strptime(str(v.get("send_date")), "%Y/%m/%d")
        except ValueError:
            if (-30 <= int(send_date) <= -1) and (survey_limit_date == 99):
                raise ValueError("Invalid combination of sendDate and limitDate.")

        # 回答期限日チェック 99:月末最終営業日、1～30: ○営業日後, 0: なし, 101～130: 翌月月初○営業日(営業日に+100した数値)以外を弾く
        if not (
            (1 <= survey_limit_date <= 30)
            or (survey_limit_date in [0, 99])
            or (101 <= survey_limit_date <= 130)
        ):
            raise ValueError("specify a valid number.")

        return v


class PutScheduleIdDateResponse(CustomBaseModel):
    """スケジュール更新結果レスポンスクラス"""

    message: str = Field(..., title="メッセージ", example="OK")


class DeleteSurveySchedulesByIdDateQuery(CustomBaseModel):
    """アンケート スケジュールクエリパラメータクラス"""

    survey_id: str = Query(..., title="スケジュールID")
    version: int = Query(..., title="ロックキー（楽観ロック制御）", example=1, ge=1)


class BulkScheduleInfo(CustomBaseModel):
    """一括登録時アンケートスケジュール情報クラス"""

    request_date: int = Field(
        ..., title="送信日(毎月○日:1-31, 回答期限日から○営業日前:-30～-1)", example="10"
    )
    limit_date: int = Field(
        ...,
        title="回答期限日(月末最終営業日:99, なし:0, 〇営業日後:1～30, 翌月月初○営業日:101～130(営業日に+100した数値))",
        example=99,
    )

    @root_validator
    def check_timing(cls, v):
        request_date = v.get("request_date")
        limit_date = v.get("limit_date")

        # 送信日チェック 毎月○日:1-31, 回答期限日から○営業日前:-30～-1
        if not ((1 <= int(request_date) <= 31) or (-30 <= int(request_date) <= -1)):
            raise ValueError("specify between 1 and 31 or between -30 and -1.")

        # 回答期限日チェック 99:月末最終営業日、1～30: ○営業日後, 0: なし, 101～130: 翌月月初○営業日(営業日に+100した数値)以外を弾く
        if not (
            (1 <= limit_date <= 30)
            or (limit_date in [0, 99])
            or (101 <= limit_date <= 130)
        ):
            raise ValueError("specify a valid number.")

        # requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の(送信日から)「〇営業日後」の同時指定は不可
        try:
            datetime.strptime(str(v.get("request_date")), "%Y/%m/%d")
        except ValueError:
            if (-30 <= int(request_date) <= -1) and (1 <= limit_date <= 30):
                raise ValueError("Invalid combination of requestData and limitDate.")
        # requestDate(アンケート送信日時)の「回答期限日から○営業日前」とlimitDate(回答期限日)の「なし」の同時指定は不可
        try:
            datetime.strptime(str(v.get("request_date")), "%Y/%m/%d")
        except ValueError:
            if (-30 <= int(request_date) <= -1) and (limit_date == 0):
                raise ValueError("Invalid combination of requestData and limitDate.")

        return v


class BulkCreateSurveyScheduleRequest(CustomBaseModel):
    """アンケートスケジュール一括登録リクエストクラス"""

    service: BulkScheduleInfo = Field(..., title="サービスアンケート")
    completion: BulkScheduleInfo = Field(..., title="修了アンケート")


class BulkCreateSurveyScheduleResponse(CustomBaseModel):
    """アンケートスケジュール一括登録結果レスポンスクラス"""

    message: str = Field(..., title="メッセージ", example="OK")


class BulkUpdateSurveyScheduleRequest(CustomBaseModel):
    """アンケートスケジュール一括登録リクエストクラス"""

    service: BulkScheduleInfo = Field(..., title="サービスアンケート")
    completion: BulkScheduleInfo = Field(..., title="修了アンケート")


class BulkUpdateSurveyScheduleResponse(CustomBaseModel):
    """アンケートスケジュール一括登録結果レスポンスクラス"""

    message: str = Field(..., title="メッセージ", example="OK")
