from decimal import ROUND_HALF_UP, Decimal
from typing import Any


def round_off(value: float, digit: float = 0) -> str:
    """小数点以下を任意の桁数で四捨五入します

    Args:
        value (float): 対象の値
        digit (int): 四捨五入する位。0 → 小数点第一位, 0.1 → 小数点第二位

    Returns:
        str: 四捨五入された値
    """
    decimal_value = Decimal(str(value))
    return decimal_value.quantize(Decimal(str(digit)), rounding=ROUND_HALF_UP)


def conv_blank(target: Any) -> str:
    """Noneの場合、空文字に変換します

    Args:
        target (Any): 対象のオブジェクト

    Returns:
        str: 変換後の文字列
    """
    if target is None:
        return ""
    return target
