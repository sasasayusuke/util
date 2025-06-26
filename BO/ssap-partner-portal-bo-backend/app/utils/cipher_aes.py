import base64

from Crypto import Random
from Crypto.Cipher import AES
from Crypto.Hash import SHA256

from app.resources.const import CipherAES


class AesCipherUtils:
    @staticmethod
    def create_aes(iv: bytes):
        sha = SHA256.new()
        sha.update(CipherAES.SECRET_KEY.encode())
        key = sha.digest()

        return AES.new(key, AES.MODE_CFB, iv)

    @staticmethod
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
        ret = iv + AesCipherUtils.create_aes(iv).encrypt(raw_date.encode())
        return base64.b64encode(ret).decode()

    @staticmethod
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
        return AesCipherUtils.create_aes(iv).decrypt(cipher).decode()
