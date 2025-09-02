
import os
import re
import datetime
import json
import config
import sys
from itertools import tee

def is_duplicates(list):
    """
    指定されたリストに重複があるかどうか

    :param list: チェックするリスト
    :return: 真偽値
    """
    return len(list) != len(set(list))

def is_empty(obj):
    """
    オブジェクトが空であるかどうか

    :param obj:     オブジェクト
    :return:        真偽値
    """
    if obj is None:
        return True
    elif isinstance(obj, (list, tuple)):
        # リスト、タプルの場合
        return not obj
    elif isinstance(obj, (int, float)):
        # 数値の場合、0を空とみなさない
        return False
    elif isinstance(obj, str):
        # 文字列の場合、全角スペースを含む空白文字を省いた後、空かどうかを評価
        return not obj.strip(' \t\n\r\f\v\u3000')
    else:
        return not bool(obj)

def print_color(message, color):
    """
    色付きのメッセージをターミナルに出力する関数

    :param message:     出力メッセージ
    :param color:       色
    """
    defaultColor = "\033[0m"

    if color == "red":
        color = "\033[31m"
    elif color == "yellow":
        color = "\033[33m"
    elif color == "gray":
        color = "\033[90m"
    else:
        color = defaultColor
    print(f"{color}{message}{defaultColor}", file=sys.stderr)

def is_numeric(value):
    """
    文字列が数値（整数または浮動小数点数）であるかどうかを判定する関数

    :param value:           値
    :return:                真偽値
    """
    try:
        float(value)
        return True
    except ValueError:
        return False

def find_file(file_name="", directory=""):
    # file_name が直接アクセス可能な場合
    if os.path.isfile(file_name):
        return os.path.abspath(file_name)

    # directory + file_name でアクセス可能な場合
    if directory:
        full_path = os.path.join(directory, file_name)
        if os.path.isfile(full_path):
            return os.path.abspath(full_path)

    # スクリプト実行場所 + file_name でアクセス可能な場合
    current_dir = os.path.dirname(os.path.abspath(__file__))
    full_path = os.path.join(current_dir, file_name)
    if os.path.isfile(full_path):
        return os.path.abspath(full_path)

    # ファイルが見つからなかった場合
    return ""


def find_right_edge(sheet, cell_address, scan_direction = True):
    """
    指定された開始セルから右方向に移動し右端を探索する関数

    :param sheet:           openpyxlのシートオブジェクト
    :param cell_address:    開始セルアドレス
    :param scan_direction:  普通の走査か逆走査かを指定するフラグ
    :return:                右端セルの列番号
    """
    cell = sheet[cell_address]
    start_row = cell.row
    start_column = cell.column

    if scan_direction:
        # 次の列から右方向に走査
        for col in range(start_column + 1, sheet.max_column + 1):
            if is_empty(sheet.cell(row=start_row, column=col).value):
                return col - 1
    else:
        # 最後の列から逆方向に走査
        for col in range(sheet.max_column, start_column, -1):
            if not is_empty(sheet.cell(row=start_row, column=col).value):
                return col
    return sheet.max_column + 1

def find_down_edge(sheet, cell_address, scan_direction = True):
    """
    指定された開始セルから下方向に移動し下端を探索する関数

    :param sheet:           openpyxlのシートオブジェクト
    :param cell_address:    開始セルアドレス
    :param scan_direction:  普通の走査か逆走査かを指定するフラグ
    :return:                下端セルの行番号
    """
    cell = sheet[cell_address]
    start_row = cell.row
    start_column = cell.column

    if scan_direction:
        # 次の行から下方向に走査
        for row in range(start_row + 1, sheet.max_row + 1):
            if is_empty(sheet.cell(row=row, column=start_column).value):
                return row - 1
    else:
        # 最後の行から逆方向に走査
        for row in range(sheet.max_row, start_row, -1):
            if not is_empty(sheet.cell(row=row, column=start_column).value):
                return row
    return sheet.max_row + 1

def get_address(row, col):
    return to_alphabet_from_num(col) + str(row)


def to_alphabet_from_num(num):
    # アルファベットのリストを作成
    alphabets = [chr(i) for i in range(65, 91)]
    result = ""

    while num > 0:
        num -= 1
        result = alphabets[num % 26] + result
        num //= 26

    return result

def to_N_digits(num, digits):
    return f"{{0:0{digits}d}}".format(num)

def save_json(json_data, file_name="", directory="", add_timestamp_dir=False):
    # ディレクトリが指定されていない場合は、現在のディレクトリを使用
    if not directory:
        directory = os.path.dirname(os.path.abspath(__file__))

    # ファイル名に .json 拡張子がない場合は追加
    if not file_name.endswith('.json'):
        file_name = f'{file_name}.json'

    # オプションがTrueの場合、ディレクトリ名に現在の日時を追加
    if add_timestamp_dir:
        directory = os.path.join(directory, datetime.datetime.now().strftime("%Y-%m-%d_%H-%M-%S"))

    # ディレクトリが存在しない場合は作成
    if not os.path.exists(directory):
        os.makedirs(directory)

    # ファイルパスを設定
    file_path = os.path.join(directory, file_name)

    with open(file_path, 'w', encoding='utf-8') as f:
        json.dump(json_data, f, ensure_ascii=False, indent=4)

    return file_path


def save_js_object(data, variable_name="params", file_name="変数.js", directory=""):
    # ディレクトリが指定されていない場合は、現在のディレクトリを使用
    if not directory:
        directory = os.path.dirname(os.path.abspath(__file__))

    # ディレクトリが存在しない場合は作成
    if not os.path.exists(directory):
        os.makedirs(directory)

    # ファイルパスを設定
    file_path = os.path.join(directory, file_name)

    # データをJSON形式の文字列に変換
    json_string = json.dumps(data, ensure_ascii=False, indent=2)

    # JavaScriptの変数宣言を作成
    js_variable = f"const {variable_name} = {json_string};"

    # ファイルに保存
    with open(file_path, "w", encoding="utf-8") as f:
        f.write(js_variable)

    return file_path



def read_json(file_name="", directory=""):
    # ファイルが存在するか確認
    full_path = find_file(file_name, directory)
    if not os.path.exists(full_path):
        raise FileNotFoundError(f"File not found: {full_path}")

    # JSONファイルを読み込む
    with open(full_path, 'r', encoding='utf-8-sig') as f:
        json_data = json.load(f)

    return json_data

def process_site_info(input_string, site_info):
    """
    入力文字列内のsite_info参照を実際の値に置換する関数

    :param input_string: site_info参照を含む入力文字列
    :param site_info:    参照される値を含む辞書
    :return:             site_info参照が置換された出力文字列
    """
    pattern = r'site_info\["([^"]+)"\]\["([^"]+)"\]'
    return re.sub(pattern, lambda m: json.dumps(site_info.get(m.group(1), {}).get(m.group(2), m.group(0))), input_string)




def pairwise(iterable):
    """
    現在の要素と次の要素を一緒にループする関数

    :param iterable: イテレータ
    :param pattern:  ペアのパターン
        s -> (s0,s1), (s1,s2), (s2, s3), ..., (sn-1, sn)
    :return 現在の要素と次の要素 を組みにしたイテレータ
    """
    a, b = tee(iterable)
    next(b, None)
    # パターン1: 最後の要素のペアは生成されない
    return zip(a, b)



if __name__ == "__main__":
    # テストケース
    test_values = [12, 0.444, "int", "float", "03", "2345", "-123.456", "0.56", "dsd", "100L", "5.2f"]
    results = {val: is_numeric(val) for val in test_values}
    print(results)
    for p in config.PARAMETERS:
        print(p)