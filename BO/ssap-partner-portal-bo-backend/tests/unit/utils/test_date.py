from datetime import datetime

import pytest

from app.utils.date import (
    get_continuous_date_for_each_month,
    get_continuous_date_for_each_week_old,
)


def test_get_continuous_date_for_each_month_ok():
    start_date = datetime(2022, 4, 1)
    end_date = datetime(2022, 6, 1)
    expected = ["2022/04/05", "2022/05/05"]

    actual = get_continuous_date_for_each_month(
        start_date=start_date, end_date=end_date, offset_day=5
    )

    assert actual == expected


def test_get_continuous_date_for_each_month_offset_day_after_start_day():
    start_date = datetime(2022, 4, 15)
    end_date = datetime(2022, 6, 15)
    expected = ["2022/05/05", "2022/06/05"]

    actual = get_continuous_date_for_each_month(
        start_date=start_date, end_date=end_date, offset_day=5
    )

    assert actual == expected


def test_get_continuous_date_for_each_week():
    start_date = datetime(2022, 4, 1)
    end_date = datetime(2022, 6, 1)
    day_of_week = "火曜"

    expected = [
        "2022/04/05",
        "2022/04/12",
        "2022/04/19",
        "2022/04/26",
        "2022/05/03",
        "2022/05/10",
        "2022/05/17",
        "2022/05/24",
        "2022/05/31",
    ]

    actual = get_continuous_date_for_each_week_old(
        start_date=start_date, end_date=end_date, day_of_week=day_of_week
    )

    assert actual == expected


def test_get_continuous_date_for_each_week_threshold():
    with pytest.raises(Exception) as e:
        get_continuous_date_for_each_week_old(
            start_date=datetime(2022, 4, 1),
            end_date=datetime(2022, 6, 1),
            day_of_week="3月",
        )

    assert e.value.detail == "Please specify the day of the week."
