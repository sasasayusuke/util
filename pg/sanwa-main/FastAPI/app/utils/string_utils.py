import os
import io
import logging
import base64
from sqlalchemy import null
from datetime import date, datetime
from typing import Union, List, Optional
from app.core.config import settings

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

def decode_str_value(value, encodings=['shift_jis', 'cp932', 'euc_jp', 'iso2022_jp', 'utf-8']):
    try:
        byte_string = value.encode('latin-1')
        return byte_string.decode('shift_jis').replace("\u3000", " ")
    except Exception:
        for encoding in encodings:
            try:
                return byte_string.decode(encoding)
            except Exception:
                continue
    # どのエンコーディングでもデコードできない場合、そのまま返す
    return value

def decode_bytes_value(value, encodings=['shift_jis', 'cp932', 'euc_jp', 'iso2022_jp', 'utf-8']):
    for encoding in encodings:
        try:
            return value.decode(encoding)
        except UnicodeDecodeError:
            continue
    # どのエンコーディングでもデコードできない場合、エラーを置換してUTF-8でデコード
    return value.decode('utf-8', errors='replace')

def decode_texts(arr):
    returnArr = []
    for row in arr:
        for key, value in row.items():
            if isinstance(value, str):
                row[key] = decode_str_value(value)
            elif isinstance(value, bytes):
                row[key] = decode_bytes_value(value)
        returnArr.append(row)
    return returnArr

def convert_rc_to_cell(row, col):
    """
    行番号と列番号をExcelのセルアドレスに変換する関数
        例：
            (1, 1) -> "A1"
            (23, 55) -> "BC23"

    Args:
        row (int): 行番号
        col (int): 列番号

    Returns:
        str: A1形式のセル参照
    """
    # 列番号をアルファベットに変換
    alpha = convert_1_to_a(col)

    # 行番号と結合
    return f"{alpha}{row}"

def convert_cell_to_rc(cell):
    """
    Excelのセルアドレスを行番号と列番号に変換する関数
        例：
            "A1" -> (1, 1)
            "BC23" -> (23, 55)

    Args:
        cell (str): A1形式のセル参照(例："A1", "BC23")

    Returns:
        tuple: (行番号, 列番号)
    """

    # アルファベット部分と数字部分を分離
    alpha = ""
    num = ""

    for char in cell:
        if char.isalpha():
            alpha += char
        elif char.isdigit():
            num += char

    # アルファベットを数値に変換
    col = convert_a_to_1(alpha)
    # 文字列の数字をintに変換
    row = int(num)

    return row, col

def convert_a_to_1(string):
    """
    アルファベット文字列を26進数的な数値に変換する関数
        例:
            A→1, B→2, Z→26, AA→27, BC→55
    Args:
        string: 変換するアルファベット文字列
    Returns:
        変換後の数値
    """
    count = 0
    for i in range(len(string)):
        j = len(string) - i - 1
        count += (alphabet.index(string.upper()[j]) + 1) * (len(alphabet) ** i)
    return count

def convert_1_to_a(number):
    """
    数値をアルファベット文字列に変換する関数
        例:
            1→A, 2→B, 26→Z, 27→AA, 55→BC
    Args:
        number: 変換する数値
    Returns:
        変換後のアルファベット文字列
    """
    if number <= 0:
        return ""

    base = len(alphabet)
    result = ""

    while number > 0:
        number -= 1
        result = alphabet[number % base] + result
        number //= base

    return result


def encode_string(text: str) -> str:
    """
    日本語文字列をBase64でエンコードする関数

        js 側でデコードする場合の例
        // Base64文字列をデコードしてUint8Arrayを取得
        const binaryData = Uint8Array.from(atob(encodedText), c => c.charCodeAt(0));

        // Uint8ArrayをUTF-8文字列にデコード
        const decoder = new TextDecoder('utf-8');
        return decoder.decode(binaryData);

    Args:
        text: エンコードする文字列（日本語可）

    Returns:
        エンコードされた文字列

    使用例:
        >>> encode_string("ささきゆうすけ")
        '44GV44GV44GN44KG44GG44GZ44GR'
    """
    # 文字列をUTF-8でバイト列に変換し、Base64エンコード
    encoded = base64.b64encode(text.encode('utf-8'))
    # バイト列を文字列に変換して返す
    return encoded.decode('ascii')


def decode_base64(base64_str: str) -> io.BytesIO:
    """
    Base64 エンコードされたデータをデコードし、ファイルのように扱えるオブジェクトを返す

    Args:
        base64_str: Base64文字列

    Returns:
        BytesIOオブジェクト
    """
    binary_data = base64.b64decode(base64_str)
    bs = io.BytesIO(binary_data)
    bs.seek(0)
    return bs

def null_to_zero(value, default_value = 0):

    """
    Null(None)または空文字を指定された値に変換する関数

    Args:
        value:          変換対象の値
        default_value:  Null(None)または空文字の場合に設定する値
                        デフォルト: 0

    使用例:
        >>> null_to_default(None)         # 0
        >>> null_to_default("", -1)       # -1
        >>> null_to_default(" abc ")      # "abc"
        >>> null_to_default(123)          # 123
    """

    # Nullまたは空文字チェック
    if value is None or value == "":
        return default_value

    # 文字列の場合は空白を除去
    if isinstance(value, str):
        return value.strip()

    return value

def format_jp_date(
    value: Optional[Union[str, date, datetime]] = None,
    formats: List[str] = ["%Y/%m/%d", "%Y-%m-%d"]
) -> str:
    """日付を日本語形式(YYYY年MM月DD日)に変換する

    Args:
        value:  日付(文字列、date型、datetime型)
                Noneの場合は今日の日付を使用
        formats: 日付文字列のフォーマットリスト（文字列の場合のみ使用）

    Returns:
        str: 日本語形式の日付文字列

    使用例:
        >>> format_jp_date("2023/12/18", ["%Y/%m/%d", "%Y-%m-%d"])
        '2023年12月18日'
        >>> format_jp_date("2023-12-18", ["%Y/%m/%d", "%Y-%m-%d"])
        '2023年12月18日'
        >>> format_jp_date("2023-12-18")
        '2023年12月18日'
        >>> format_jp_date()
        今日の日付が入る
    """
    # valueがNoneの場合は今日の日付を使用
    if value is None:
        value = date.today()

    # date型またはdatetime型の場合は直接変換
    if isinstance(value, (date, datetime)):
        return value.strftime('%Y年%m月%d日')

    # 空文字チェック
    if not value:
        return ''

    # 文字列の場合は指定されたフォーマットで解析
    for fmt in formats:
        try:
            return datetime.strptime(value, fmt).strftime('%Y年%m月%d日')
        except ValueError:
            continue

    raise ValueError(f"日付 '{value}' は指定された形式 {formats} のいずれとも一致しません")

def AnsiLeftB(input_str: str, byte_length: int) -> str:
    """
    指定されたバイト長で文字列を切り取る関数。
    マルチバイト文字を考慮し、途中で切り取られないように調整します。

    :param input_str: 処理対象の文字列
    :param byte_length: 切り取りたいバイト長
    :return: 指定バイト長に収まる文字列
    """
    # バイト列に変換（shift-jisエンコーディングを使用）
    encoded_str = input_str.encode('cp932')

    # バイト数が指定バイト長以下の場合はそのまま返す
    if len(encoded_str) <= byte_length:
        return input_str

    # 指定バイト数まで切り取る
    truncated_bytes = encoded_str[:byte_length]

    # UTF-8デコードでエラーが発生しない最後の文字を確定する
    try:
        decoded_str = truncated_bytes.decode('cp932')
    except UnicodeDecodeError:
        # バイトを減らしてデコードできる位置を探す
        truncated_bytes = truncated_bytes[:-1]
        while True:
            try:
                decoded_str = truncated_bytes.decode('cp932')
                break
            except UnicodeDecodeError:
                truncated_bytes = truncated_bytes[:-1]

    return decoded_str

def SpcToNull(value, change_value=None):
    """
    入力値が None または空白文字列の場合に、指定された値に置き換える関数。

    :param value: 入力値
    :param change_value: 置き換えたい値（デフォルトは None）
    :return: 処理後の値
    """
    if value is None:  # None チェック (VBの IsNull に相当)
        return change_value
    elif str(value).strip() == "":  # 空白文字列チェック (Trim$ に相当)
        return change_value
    else:
        return str(value).strip()  # 入力値をトリムして返す

def NullToZero(value, change_value=0):
    """
    入力値が None または空文字列の場合に指定された値に置き換える関数。
    文字列の場合はトリム処理を行い、それ以外はそのまま返す。0.

    :param value: 処理対象の値
    :param change_value: 置き換えたい値（デフォルトは 0）
    :return: 処理後の値
    """
    if value is None:  # Null チェック (VBの IsNull に相当)
        return change_value
    elif value == "":  # 空文字列チェック (VBの vbNullString に相当)
        return change_value
    else:
        if isinstance(value, str):  # 文字列型のチェック
            return value.strip()  # 前後の空白を削除
        else:  # 文字列以外はそのまま返す
            return value

def read_asset(path, encoding='utf-8', extensions=['.txt', '.html', '.css', '.js', '.json', '.csv', '.yaml']):
    """
    指定されたパスからアセットを読み込み、テキストとして返す関数
    拡張子が指定されていない場合は、複数の候補を順に試みる

    引数:
        path (str): 読み込むアセットのパス（拡張子があってもなくても可）
        encoding (str): 使用するエンコーディング（デフォルトはutf-8）
        extensions (list): 試す拡張子のリスト（デフォルトは['.txt', '.json', '.csv', '.md', '.html']）

    戻り値:
        str: ファイルから読み込まれたテキストデータ
        None: すべての拡張子でエラーが発生した場合

    例外:
        すべての拡張子で読み込みに失敗した場合はNoneを返す
    """

    # パスから拡張子を取得
    base_path, ext = os.path.splitext(path)

    # 既に拡張子があり、extensionsリストに含まれている場合のみその拡張子で試す
    if ext and ext in extensions:
        try:
            with open(path, 'r', encoding=encoding) as f:
                content = f.read()
                logger.info(f"アセット読み込み成功: {path}")
                return content
        except:
            # 読み込み失敗時は次へ進む
            pass
    # 拡張子がない場合や指定された拡張子で失敗した場合は複数の拡張子を試す
    for ext in extensions:
        try_path = f"{base_path}{ext}"
        try:
            with open(try_path, 'r', encoding=encoding) as f:
                content = f.read()
                logger.info(f"アセット読み込み成功: {try_path}")
                return content
        except:
            # 読み込み失敗時は次の拡張子へ
            continue

    # [sample.js.txt]のように拡張子が2つある場合
    for ext in extensions:
        try_path = f"{path}{ext}"
        try:
            with open(try_path, 'r', encoding=encoding) as f:
                content = f.read()
                logger.info(f"アセット読み込み成功: {try_path}")
                return content
        except:
            # 読み込み失敗時は次の拡張子へ
            continue

    # すべての拡張子で失敗した場合
    logger.error(f"アセット読み込みに失敗: {base_path}")
    return None

def merge_info(self, base_dict, additional_dict, overwrite_existing=True):
    """
    ディクショナリをマージする汎用関数

    引数:
        base_dict (dict): ベースとなる辞書
        additional_dict (dict): 追加する辞書
        overwrite_existing (bool): Trueの場合、既存のキーの値を上書きする。Falseの場合、既存のキーは上書きせず、新しいキーのみ追加する（デフォルトはTrue）

    戻り値:
        dict: マージされた辞書
    """
    result = base_dict.copy()

    for key, value in additional_dict.items():
        if key in result:
            if overwrite_existing:
                # 既存のキーを上書き
                if isinstance(value, dict) and isinstance(result[key], dict):
                    # 両方がディクショナリの場合は再帰的にマージ
                    result[key] = self.merge_info(result[key], value, overwrite_existing)
                else:
                    # ディクショナリでない場合は単純に上書き
                    result[key] = value
        else:
            # 新しいキーを追加
            result[key] = value

    return result

