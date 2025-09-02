import os
import json
import pyperclip
from typing import Dict
from pathlib import Path
from collections import deque

MAX_SIZE = 5 * 1024 * 1024  # 5MB

HEAD_LINES = 100  # 先頭の行数
TAIL_LINES = 50  # 末尾の行数
MAX_LINES = HEAD_LINES + TAIL_LINES  # 最大行数の制限

SIZE_LIMIT_MESSAGE = f"\n... （{MAX_SIZE}Byteを超えたため残りは省略）"
LINE_LIMIT_MESSAGE = f"\n... （{MAX_LINES}行を超えたため中間の行は省略）...\n"

# 対象パス
# TARGET_PATH = r"C:\svn\trunk\src\sanwa\FastAPI\tests"
TARGET_PATH = r"C:\svn\trunk\src\sanwa\FastAPI"


# 対象フォルダ（空リストの場合はすべて対象）
TARGET_FOLDERS = []

# 対象拡張子（空リストの場合はすべて対象）
TARGET_EXTENSIONS = []

# 除外設定
EXCLUDED_FOLDERS = [
    '.history',
    '.zip',
    '.git',
    '.svn',
    '.next',
    '__pycache__',
    'node_modules',
    'gitlab',
    'venv',
    'log',
    'history',
    'dist',
    'Htmls',
    'Scripts',
    'Styles'
]

EXCLUDED_EXTENSIONS = [
    ".png",
    ".jpg",
    ".mp3",
    ".mp4",
    ".LICENSE.txt",
    ".map",
    ".exe",
    ".vbw",
    ".frx"
]

EXCLUDED_FILES = [
    "package-lock.json",
    "bundle.js",
    "app.log"
]

def read_file_content(file_path: str) -> str:
    """
    ファイルの先頭500行と末尾500行を読み込む
    複数のエンコーディングに対応し、エラー処理を強化
    """

    encodings = [
        ('utf-8', {'errors': 'strict'}),
        ('utf-8-sig', {'errors': 'strict'}),
        ('shift-jis', {'errors': 'strict'}),
        ('cp932', {'errors': 'strict'}),
        ('utf-16', {'errors': 'strict'}),
        ('utf-16le', {'errors': 'strict'}),
        ('utf-16be', {'errors': 'strict'}),
        ('ascii', {'errors': 'ignore'}),
    ]

    def process_file(f) -> str:
        """ファイルを読み込んで先頭と末尾の行を処理"""
        head_lines = []
        tail_buffer = deque(maxlen=TAIL_LINES)
        total_lines = 0
        total_size = 0

        # 全行を走査
        for line in f:
            total_lines += 1
            line = line.rstrip()
            line_size = len(line.encode('utf-8'))

            # サイズ制限のチェック
            if total_size + line_size > MAX_SIZE:
                return f"ファイルサイズが制限（{MAX_SIZE}バイト）を超えたため、処理を中止しました。"

            total_size += line_size

            # 先頭部分の処理
            if len(head_lines) < HEAD_LINES:
                head_lines.append(line)

            # 末尾部分の処理（常に最新のTAIL_LINES行を保持）
            tail_buffer.append(line)

        # 行数に応じて適切な出力を生成
        if total_lines <= HEAD_LINES + TAIL_LINES:
            # ファイルが十分に小さい場合は全行を返す
            return '\n'.join(head_lines + list(tail_buffer)[-(total_lines - len(head_lines)):])
        else:
            # 大きいファイルの場合は先頭と末尾を結合
            skipped_lines = total_lines - HEAD_LINES - TAIL_LINES
            middle_message = f"{LINE_LIMIT_MESSAGE}（{skipped_lines:,}行省略）{LINE_LIMIT_MESSAGE}"
            return '\n'.join(head_lines) + middle_message + '\n'.join(list(tail_buffer))

    # テキストとして読み込みを試行
    for encoding, kwargs in encodings:
        try:
            with open(file_path, 'r', encoding=encoding, **kwargs) as f:
                return process_file(f)
        except (UnicodeError, UnicodeDecodeError):
            continue
        except Exception as e:
            print(f"Warning: Error reading {file_path} with {encoding}: {str(e)}")
            continue

    # テキスト読み込みが全て失敗した場合はバイナリとして読み込み
    try:
        with open(file_path, 'rb') as f:
            content = f.read()
            return f"バイナリファイル: {len(content):,} バイト"
    except Exception as e:
        return f"ファイル読み込みエラー: {str(e)}"

def get_folder_structure() -> Dict:
    """フォルダ構造を取得して辞書形式で返す"""
    structure = {}

    for root, dirs, files in os.walk(TARGET_PATH):
        relative_path = os.path.relpath(root, TARGET_PATH)

        if TARGET_FOLDERS and relative_path not in TARGET_FOLDERS and relative_path != '.':
            continue

        current = structure
        if relative_path != '.':
            for part in relative_path.split(os.sep):
                current = current.setdefault(part, {})

        # 除外フォルダの処理
        for d in dirs:
            if d in EXCLUDED_FOLDERS:
                current[f"{d}/ (除外)"] = "除外フォルダ"
        dirs[:] = [d for d in dirs if d not in EXCLUDED_FOLDERS]

        # ファイルの処理
        for file in files:
            file_path = os.path.join(root, file)
            _, file_extension = os.path.splitext(file)

            if TARGET_EXTENSIONS and file_extension.lower() not in TARGET_EXTENSIONS:
                continue

            if file in EXCLUDED_FILES:
                current[f"{file} (除外)"] = "除外ファイル"
                continue

            if any(file.lower().endswith(ext.lower()) for ext in EXCLUDED_EXTENSIONS):
                current[f"{file} (除外)"] = "除外拡張子"
                continue

            if os.path.getsize(file_path) > MAX_SIZE:
                current[file] = f"ファイルサイズ制限超過: {os.path.getsize(file_path)} バイト"
                continue

            current[file] = read_file_content(file_path)

    return structure

def structure_to_markdown(structure: Dict, depth: int = 0) -> str:
    """構造をマークダウン形式に変換"""
    markdown = ""
    for key, value in structure.items():
        if isinstance(value, dict):
            markdown += "  " * depth + f"- {key}\n"
            markdown += structure_to_markdown(value, depth + 1)
        else:
            markdown += "  " * depth + f"- {key}\n"
            if value in ["除外フォルダ", "除外ファイル", "除外拡張子"]:
                markdown += "  " * (depth + 1) + f"{value}\n"
            elif isinstance(value, str) and not value.startswith("ファイルサイズ制限超過") and not value.startswith("ファイル読み込みエラー"):
                markdown += "=" * 97 + "\n"
                markdown += value + "\n"
                markdown += "=" * 97 + "\n"
            else:
                markdown += "  " * (depth + 1) + f"{value}\n"
    return markdown

def output_structure(structure: Dict, output_format: str = 'markdown'):
    """構造を指定された形式で出力"""
    if output_format == 'json':
        output = json.dumps(structure, indent=2, ensure_ascii=False)
    elif output_format == 'markdown':
        output = structure_to_markdown(structure)
    else:
        raise ValueError("無効な出力形式です。'json'または'markdown'を選択してください。")

    output_size = len(output.encode('utf-8'))
    print(f"デバッグ: 出力サイズ: {output_size} バイト (最大: {MAX_SIZE} バイト)")  # デバッグ出力

    if output_size > MAX_SIZE:
        print(f"警告: 出力サイズ ({output_size} バイト) が最大許容サイズ ({MAX_SIZE} バイト) を超えています。")
        output = output[:MAX_SIZE] + SIZE_LIMIT_MESSAGE

    print(f"\nフォルダ構造と内容 ({output_format} 形式):")
    print(output)

    try:
        pyperclip.copy(output)
        print(f"\nフォルダ構造と内容 (サイズ: {output_size} バイト) をクリップボードにコピーしました。")
    except Exception as e:
        print(f"\nクリップボードへのコピーでエラーが発生しました: {str(e)}")
        print("出力はコンソールにのみ表示されます。")

def main():
    """メイン実行関数"""
    try:
        result = get_folder_structure()
        output_structure(result, 'markdown')
    except Exception as e:
        print(f"エラーが発生しました: {str(e)}")
        import traceback
        traceback.print_exc()

if __name__ == "__main__":
    main()