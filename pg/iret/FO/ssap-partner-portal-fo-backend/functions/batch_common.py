import os
from datetime import datetime, timedelta

from functions.batch_const import JwtSettingInfo, CipherAES
from jose import jwt

import base64

from Crypto import Random
from Crypto.Cipher import AES
from Crypto.Hash import SHA256


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


def create_aes(iv: bytes):
    sha = SHA256.new()
    sha.update(CipherAES.SECRET_KEY.encode())
    key = sha.digest()

    return AES.new(key, AES.MODE_CFB, iv)


def encrypt(raw_date: str) -> str:
    """AES256暗号化.
        ※初期化ベクトルは動的に生成しているため、同じ平文でも毎回、暗号化データは異なります.
        2つの暗号化データを比較する場合は復号化して比較してください.

    Args:
        raw_date (str): 暗号化する文字列
    Returns:
        str: 暗号化データ(Base64)
    """
    if not raw_date:
        return raw_date
    iv = Random.new().read(AES.block_size)
    ret = iv + create_aes(iv).encrypt(raw_date.encode())
    return base64.b64encode(ret).decode()


def decrypt(encrypted_data: str) -> str:
    """AES256復号化

    Args:
        encrypted_data (str): 暗号化データ(Base64)
    Returns:
        str: 復号化した文字列
    """
    if not encrypted_data:
        return encrypted_data
    enc_data = base64.b64decode(encrypted_data)
    iv, cipher = enc_data[: AES.block_size], enc_data[AES.block_size :]
    return create_aes(iv).decrypt(cipher).decode()


def get_day_of_week(dt: datetime) -> str:
    """曜日を取得する

    Args:
        dt (datetime): 現在日時 (datetime)

    Returns:
        day: day_listの曜日
    """
    day_list = ["月", "火", "水", "木", "金", "土", "日"]
    return day_list[dt.weekday()]
