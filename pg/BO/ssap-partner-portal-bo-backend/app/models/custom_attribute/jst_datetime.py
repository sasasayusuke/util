from datetime import datetime

from pynamodb.attributes import UTCDateTimeAttribute
from pytz import timezone

date_format = "%Y-%m-%dT%H:%M:%S.%f"


class JSTDateTimeAttribute(UTCDateTimeAttribute):
    """
    An attribute for storing a JST Datetime
    """

    def serialize(self, value):
        """
        Takes a datetime object and returns a string
        """
        if value.tzinfo is None:
            value = value.replace(tzinfo=timezone("Asia/Tokyo"))
        # Padding of years under 1000 is inconsistent and depends on system strftime:
        # https://bugs.python.org/issue13305
        fmt = value.strftime(date_format).zfill(26)
        return fmt

    def deserialize(self, value):
        """
        Takes a JST datetime string and returns a datetime object
        """
        return self._fast_parse_jst_date_string(value)

    @staticmethod
    def _fast_parse_jst_date_string(date_string: str) -> datetime:
        # Method to quickly parse strings formatted with '%Y-%m-%dT%H:%M:%S.%f+0000'.
        # This is ~5.8x faster than using strptime and 38x faster than dateutil.parser.parse.
        _int = int  # Hack to prevent global lookups of int, speeds up the function ~10%
        try:
            # Fix pre-1000 dates serialized on systems where strftime doesn't pad w/older PynamoDB versions.
            date_string = date_string.zfill(26)
            if (
                # len(date_string) != 26
                date_string[4] != "-"
                or date_string[7] != "-"
                or date_string[10] != "T"
                or date_string[13] != ":"
                or date_string[16] != ":"
                or date_string[19] != "."
                # or date_string[26:31] != "+0900"
            ):
                raise ValueError(
                    "Datetime string '{}' does not match format '{}'".format(
                        date_string, date_format
                    )
                )
            return timezone("Asia/Tokyo").localize(
                datetime(
                    _int(date_string[0:4]),
                    _int(date_string[5:7]),
                    _int(date_string[8:10]),
                    _int(date_string[11:13]),
                    _int(date_string[14:16]),
                    _int(date_string[17:19]),
                    _int(date_string[20:26]),
                )
            )
        except (TypeError, ValueError):
            raise ValueError(
                "Datetime string '{}' does not match format '{}'".format(
                    date_string, date_format
                )
            )
