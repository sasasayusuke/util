import calendar
import copy
import os
from datetime import date, datetime, timedelta
from typing import List

import pandas
from dateutil.relativedelta import relativedelta
from fastapi import status
from fastapi.exceptions import HTTPException
from pytz import timezone

from app.core.common_logging import CustomLogger

logger = CustomLogger.get_logger()


days_of_week = ["月曜", "火曜", "水曜", "木曜", "金曜", "土曜", "日曜"]
freq_week = ["W-MON", "W-TUE", "W-WED", "W-THU", "W-FRI", "W-SUT", "W-SUN"]


def get_datetime_now() -> datetime:
    """現在に日時を取得するための関数

    Lambdaの環境変数を設定してテストをしやすくするため、現在日時取得に関してはこちらの関数を使用してください。

    Returns:
        datetime: JSTでの現在日時
    """
    try:
        env_datetime = os.getenv("datetime")
        if env_datetime:
            dt = datetime.strptime(env_datetime, "%Y/%m/%d %H:%M")
            return timezone("Asia/Tokyo").localize(dt)
    except ValueError:
        logger.debug("Incorrect format of datetime specified in environment variable")

    return datetime.now(timezone("Asia/Tokyo"))


def get_day_of_week(dt: datetime) -> str:
    """FO Home用に、現在日時(datetime)から曜日を取得する

    Args:
        dt (datetime): 現在日時 (datetime)

    Returns:
        day: day_listの曜日
    """
    day_list = ["月", "火", "水", "木", "金", "土", "日"]
    return day_list[dt.weekday()]


def get_last_date_of_month(year: int, month: int) -> datetime:
    """対象月の最終日を取得します

    Args:
        year (int): 年
        month (int): 月

    Returns:
        datetime: 月の最終日
    """
    last_day_of_month = calendar.monthrange(year, month)[1]
    return date(year, month, last_day_of_month)


def get_continuous_date_for_each_month(
    start_date: datetime, end_date: datetime, offset_day: int
) -> List:
    """指定期間中の月ごとの連続した日時データリストを取得します。

    Args:
        start_date (datetime): 日時データ生成の開始日時
        end_date (datetime): 日時データ生成の終了日時
        offset_day (int): 毎月○日の指定

    Returns:
        List: 連続日時データ ex) ['2022/01/05', '2022/02/05', '2022/03/05']
    """

    # start_dateが生成する日時データの対象に入るのかをチェック
    # 開始日時が2022/06/05で、毎月の4日のリストを生成する場合は2022/07/05の日付から開始する
    if offset_day < start_date.day:
        start_date += relativedelta(months=1)

    # リスト生成のために月の最終日をセット -> 送信日作成時に支援期間終了月の日付チェックを行う
    range_end_date = copy.deepcopy(end_date)
    range_end_date = get_last_date_of_month(int(end_date.year), int(end_date.month))

    # 指定期間の年月リストを作成
    range_list = pandas.date_range(
        start_date.strftime("%Y/%m/%d"),
        range_end_date.strftime("%Y/%m/%d"),
        freq="M",
    ).strftime("%Y/%m")

    return_list = []

    for index, range_list_itr in enumerate(range_list):
        # 年月の月最終日を取得
        year_month = range_list_itr.split("/")
        last_date = calendar.monthrange(int(year_month[0]), int(year_month[1]))[1]

        if index == (len(range_list) - 1):
            # 支援期間の支援終了月
            if (last_date > end_date.day) and (offset_day > end_date.day):
                # 最終月のレスポンスデータ作成なし
                break

        if last_date >= offset_day:
            # 年月日をoffset_dayで作成する
            return_list.append(range_list_itr + "/" + str(offset_day).zfill(2))
        else:
            # last_date < offset -> 月末最終日とする
            return_list.append(range_list_itr + "/" + str(last_date).zfill(2))

    return return_list


def get_continuous_business_date_for_each_month(
    start_date: datetime,
    end_date: datetime,
    ref_day: int,
    current_datetime: datetime,
    offset_day: int = 0,
) -> tuple:
    """指定期間中の月ごとの連続した営業日の日時データリストを取得します。
       なお、月ごとに基準日から営業日分をoffsetした日時リスト、及び基準日の日時リストをタプルで返却します。

    Args:
        start_date (datetime): 日時データ生成の開始日時
        end_date (datetime): 日時データ生成の終了日時
        ref_day (int): 基準日 (月末最終営業日:99, 翌月月初○営業日:101～130(営業日に+100した数値))
        current_datetime: 実行日時
        offset_day (int): 基準日からの日数(営業日)のoffsetの指定
          e.g. 基準日(月末最終営業日:99)から3営業日前:
              get_continuous_business_date_for_each_month(
                  start_date=start_date,
                  end_date=end_date,
                  ref_day=99,
                  current_datetime=current_datetime,
                  offset_day=-3,
              )

    Returns:
        Tuple:
            List: 連続日時データ(生成した日時) e.g. ['2022/01/26', '2022/02/22', '2022/03/28']
            List: 連続日時データ(生成した基準日) e.g. ['2022/01/31', '2022/02/28', '2022/03/31']
    """

    if not (ref_day == 99 or (101 <= ref_day <= 130)):
        return []

    # リスト生成のために月の最終日を取得
    range_end_date = copy.deepcopy(end_date)
    range_end_date = get_last_date_of_month(int(end_date.year), int(end_date.month))
    # 指定期間の年月リストを作成
    range_list = (
        pandas.date_range(
            start_date.strftime("%Y/%m/%d"),
            range_end_date.strftime("%Y/%m/%d"),
            freq="M",
        )
        .strftime("%Y/%m")
        .tolist()
    )

    calc_business_day_datetime_list: List[datetime] = []
    ref_business_day_datetime_list: List[datetime] = []
    for year_month_item in range_list:
        year_month = year_month_item.split("/")
        # 基準日時の算出
        ref_datetime: datetime = None
        if ref_day == 99:
            # 月末最終営業日
            ref_datetime = get_last_business_date_of_month(
                year=int(year_month[0]), month=int(year_month[1])
            )
        elif 101 <= ref_day <= 130:
            # 翌月月初○営業日
            # 月初1営業日はoffsetを0とするため、101を減算
            temp_offset: int = ref_day - 101
            ref_datetime = get_first_business_day_of_the_next_month(
                year=int(year_month[0]),
                month=int(year_month[1]),
                offset=temp_offset,
            )

        if offset_day < 0:
            calc_business_day_datetime_list.append(
                get_before_business_day(ref_datetime, offset_day * -1)
            )
        else:
            calc_business_day_datetime_list.append(
                get_after_business_day(ref_datetime, offset_day)
            )
        ref_business_day_datetime_list.append(ref_datetime)

    # get_last_business_date_of_month()の戻り値がdatetime型かdate型のどちらか不明確
    # 日時比較を行うためdate型に変換して型を統一
    calc_date_list = [
        i.date() if type(i) is datetime else i for i in calc_business_day_datetime_list
    ]
    ref_date_list = [
        j.date() if type(j) is datetime else j for j in ref_business_day_datetime_list
    ]

    ret_calc_item = []
    ret_ref_item = []
    for calc_item, ref_item in zip(calc_date_list, ref_date_list):
        if (
            start_date.date() <= calc_item
            and calc_item <= end_date.date()
            and current_datetime.date() < calc_item
        ):
            # 日時データ生成の開始日、終了日に収まる日時データのみ格納（現在日を含まない）
            ret_calc_item.append(calc_item.strftime("%Y/%m/%d"))
            ret_ref_item.append(ref_item.strftime("%Y/%m/%d"))

    return ret_calc_item, ret_ref_item


def get_pair_business_date_for_once(
    current_datetime: datetime, ref_day: int, offset_day: int = 0
) -> tuple:
    """指定条件の営業日の日時データ(1組)を取得します。
       なお、基準日から営業日分をoffsetした日時リスト、及び基準日の日時リストをタプルで返却します。

    Args:
        current_datetime (datetime): 現在日時
        ref_day (int): 基準日 (月末最終営業日:99, 翌月月初○営業日:101～130(営業日に+100した数値))
        offset_day (int): 基準日からの日数(営業日)のoffsetの指定
          e.g. 基準日(月末最終営業日:99)から3営業日前:
              get_continuous_business_date_for_each_month(
                  current_datetime=current_datetime,
                  ref_day=99,
                  offset_day=-3,
              )

    Returns:
        Tuple:
            List: 連続日時データ(生成した日時) e.g. ['2022/01/26']
            List: 連続日時データ(生成した基準日) e.g. ['2022/01/31']
    Exceptions:
        ValueError: 算出された日時が現在日以前の場合
    """

    if not (ref_day == 99 or (101 <= ref_day <= 130)):
        return []

    # 基準日時の算出
    calc_datetime: datetime = None
    ref_datetime: datetime = None
    if ref_day == 99:
        # 月末最終営業日
        ref_datetime = get_last_business_date_of_month(
            year=current_datetime.year, month=current_datetime.month
        )
    elif 101 <= ref_day <= 130:
        # 翌月月初○営業日
        # 月初1営業日はoffsetを0とするため、101を減算
        temp_offset: int = ref_day - 101
        ref_datetime = get_first_business_day_of_the_next_month(
            year=current_datetime.year,
            month=current_datetime.month,
            offset=temp_offset,
        )

    if offset_day < 0:
        calc_datetime = get_before_business_day(ref_datetime, offset_day * -1)
    else:
        calc_datetime = get_after_business_day(ref_datetime, offset_day)

    ret_calc_item = []
    ret_ref_item = []
    # get_last_business_date_of_month()の戻り値がdatetime型かdate型のどちらか不明確
    # 日時比較を行うためdate型に変換して型を統一
    calc_date = (
        calc_datetime.date() if type(calc_datetime) is datetime else calc_datetime
    )
    if current_datetime.date() >= calc_date:
        # 算出された日時が現在日以前の場合
        raise ValueError(
            "You cannot set a send date that is earlier than the current date."
        )
    ret_calc_item.append(calc_datetime.strftime("%Y/%m/%d"))
    ret_ref_item.append(ref_datetime.strftime("%Y/%m/%d"))

    return ret_calc_item, ret_ref_item


def get_completion_request_date(year: int, month: int, offset_day: int) -> datetime:
    """年月日を作成: カレンダー上で取りうる日付にする

    Args:
        year (datetime): 日時データ生成の年
        month (datetime): 日時データ生成の月
        offset_day (int): ○日の指定

    Returns:
        datetime: カレンダー上での日付
    """

    # 指定した年月の最終日を取得
    last_date = calendar.monthrange(year, month)[1]

    if last_date >= offset_day:
        # 年月日をoffset_dayで作成する
        return datetime(year, month, offset_day)
    else:
        # last_date < offset -> 月末最終日とする
        return datetime(year, month, last_date)


def get_continuous_date_for_each_week(
    start_date: datetime, end_date: datetime, day_of_week_index: int
) -> List:
    """指定期間中の週ごとの連続した日時データリストを取得します。
    Args:
        start_date (datetime): 日時データ生成の開始日時
        end_date (datetime): 日時データ生成の終了日時
        day_of_week_week_index (int): 0:月曜、1:火曜、2:水曜, 3:木曜, 4:金曜, 5:土曜, 6:日曜
    Returns:
       List: 連続日時データ ex) ['2022/01/05', '2022/01/12', '2022/01/19']
    """
    if not 0 <= day_of_week_index <= 6:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="Please specify the day of the week.",
        )
    for _ in days_of_week:
        if start_date.weekday() == day_of_week_index:
            break
        start_date += timedelta(days=1)

    return (
        (
            pandas.date_range(
                start_date.strftime("%Y/%m/%d"),
                end_date.strftime("%Y/%m/%d"),
                freq=freq_week[day_of_week_index],
            )
        )
        .strftime("%Y/%m/%d")
        .tolist()
    )


def is_business_day(date: datetime) -> bool:
    """平日であるかを判定します

    Args:
        date (datetime):

    Returns:
        bool: 判定結果
    """
    return date.weekday() < 5


def get_last_business_date_of_month(year: int, month: int) -> datetime:
    """月末最終営業日を取得する（祝日は考慮していない）

    Args:
        year (int): 年
        month (int): 月

    Returns:
        datetime: 月末最終営業日
    """
    last_date = get_last_date_of_month(year, month)

    while not is_business_day(last_date):
        last_date = last_date - timedelta(days=1)

    return last_date


def get_before_business_day(dt: datetime, past_day_count: int) -> datetime:
    """
        指定日時から○営業日前の日時を取得（祝日考慮なし）

    Args:
        dt (datetime): 指定日時
        past_day_count (int): 取得したい過去の営業日までの日数
    Returns
        datetime: ○営業日前の日時
    """
    temp_dt = dt
    for _ in range(past_day_count):
        temp_dt = temp_dt + timedelta(days=-1)
        while temp_dt.weekday() >= 5:
            # 土・日の場合
            temp_dt = temp_dt + timedelta(days=-1)
    return temp_dt


def get_after_business_day(dt: datetime, past_day_count: int) -> datetime:
    """
        指定日時から○営業日後の日時を取得（祝日考慮なし）

    Args:
        dt (datetime): 指定日時
        past_day_count (int): 取得したい未来の営業日までの日数
    Returns
        datetime: ○営業日後の日時
    """
    temp_dt = dt
    for _ in range(past_day_count):
        temp_dt = temp_dt + timedelta(days=1)
        while temp_dt.weekday() >= 5:
            # 土・日の場合
            temp_dt = temp_dt + timedelta(days=1)
    return temp_dt


def get_first_bussiness_day_of_the_month(
    year: int, month: int, offset: int = 0
) -> datetime:
    """
    月初の営業日を取得。（祝日考慮なし）
    offsetを指定することで、取得する営業日を変えることが可能。
      e.g. 月初3営業日を取得したい場合 -> offsetに2をセット

    Args:
        year (int): 年
        month (int): 月
        offset (int): オフセット値
            e.g. 月初3営業日を取得したい場合 -> offsetに2をセット
    Returns
        datetime: 月初の営業日（オフセット指定時は、オフセットした営業日）
    """
    temp_datetime = datetime(year=year, month=month, day=1)
    count = 0
    while True:
        if temp_datetime.weekday() < 5:
            # 月～金の場合
            if count >= offset:
                return temp_datetime
            else:
                count += 1
        temp_datetime = temp_datetime + timedelta(days=1)


def get_first_business_day_of_the_next_month(
    year: int, month: int, offset: int = 0
) -> datetime:
    """
    翌月の月初の営業日を取得。（祝日考慮なし）
    offsetを指定することで、取得する営業日を変えることが可能。
      e.g. 月初3営業日を取得したい場合 -> offsetに2をセット

    Args:
        year (int): 年
        month (int): 月
        offset (int): オフセット値
            e.g. 翌月の月初3営業日を取得したい場合 -> offsetに2をセット
    Returns
        datetime: 翌月の月初の営業日（オフセット指定時は、オフセットした営業日）
    """
    temp_datetime: datetime = datetime(year=year, month=month, day=1) + relativedelta(
        months=1
    )
    return get_first_bussiness_day_of_the_month(
        year=temp_datetime.year, month=temp_datetime.month, offset=offset
    )
