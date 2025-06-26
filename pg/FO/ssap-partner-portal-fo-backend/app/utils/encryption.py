import os
import secrets
import string
from datetime import datetime, timedelta

from jose import jwt

from app.resources.const import JwtSettingInfo


def create_random_survey_password(length) -> str:
    """アンケート用パスワードを生成して返却

    Args:
        length (int): パスワード長
    Returns:
        str: 生成したアンケート用パスワード
    """
    # 半角英数(大文字、小文字)、数字、記号のランダム文字列
    # string.punctuationは、!"#$%&'()*+,-./:;<=>?@[\]^_`{|}~
    pass_chars = string.ascii_letters + string.digits + string.punctuation
    password = "".join(secrets.choice(pass_chars) for x in range(length))
    return password


def create_jwt_survey_payload(survey_id: str, current_datetime: datetime) -> dict:
    """匿名回答アンケート用JWTのpayloadを作成.
    payload内の有効期限は現在日時にアンケート回答期限日数を加算して算出.

    Args:
        survey_id (str): アンケートID
        current_datetime (datetime): 現在日時
    Returns:
        dict: payload
            "survey_id": アンケートID
            "iat": JWT発行日時 (UNIX時間)
            "exp": 有効期限 (UNIX時間)
        e.g. {survey_id: "aaa", "iat": 1675728000, "exp": 1680879599}
    """
    # 環境変数からアンケート有効期限日数を取得
    env_valid_period = os.getenv("ANONYMOUS_SURVEY_VALID_PERIOD")
    valid_period: int = None
    try:
        valid_period = int(env_valid_period)
    except Exception:
        # 取得できない場合はデフォルトの60日を設定
        valid_period = 60

    # 有効期限
    # 現在日時にアンケート有効期限日数を加算し、時分秒を再設定
    expiration_datetime = current_datetime + timedelta(days=valid_period)
    expiration_datetime = expiration_datetime.replace(hour=23, minute=59, second=59)

    ret_dict: dict = {}
    ret_dict["survey_id"] = survey_id
    ret_dict["iat"] = current_datetime.timestamp()
    ret_dict["exp"] = expiration_datetime.timestamp()

    return ret_dict


def create_jwt(payload: dict) -> str:
    """JWTの生成

    Args:
        payload (dict): JWT claims
    Returns:
        str: JWT

    """

    return jwt.encode(
        payload, JwtSettingInfo.SECRET_KEY, algorithm=JwtSettingInfo.ALGORITHM
    )


def decode_jwt(jwt_str: str) -> dict:
    """JWTのデコード

    Args:
        jwt_str (str): JWT
    Returns:
        str: デコード内容
        e.g. {survey_id: "aaa", "iat": 1675728000, "exp": 1680879599}
    """
    return jwt.decode(
        jwt_str, JwtSettingInfo.SECRET_KEY, algorithms=JwtSettingInfo.ALGORITHM
    )
