import urllib.parse
from typing import Any, Dict, List


def url_decode_data(data: Any) -> Any:
    """
    再帰的にURLデコードを行う
    - dict: 全キー・値に対して再帰
    - list: 全要素に対して再帰
    - str : urllib.parse.unquote でデコード
    """
    try:
        if isinstance(data, dict):
            return {k: url_decode_data(v) for k, v in data.items()}
        elif isinstance(data, list):
            return [url_decode_data(v) for v in data]
        elif isinstance(data, str):
            return urllib.parse.unquote(data)
        else:
            return data
    except Exception:
        return data


def url_encode_data(data: Any) -> Any:
    """
    再帰的にURLエンコードを行う
    - dict: 全キー・値に対して再帰
    - list: 全要素に対して再帰
    - str : urllib.parse.quote でエンコード
    """
    try:
        if isinstance(data, dict):
            return {k: url_encode_data(v) for k, v in data.items()}
        elif isinstance(data, list):
            return [url_encode_data(v) for v in data]
        elif isinstance(data, str):
            return urllib.parse.quote(data, safe='')
        else:
            return data
    except Exception:
        return data
