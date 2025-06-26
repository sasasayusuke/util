import os
import json
import pyperclip
from typing import Dict
from pathlib import Path
from collections import deque
import sys

# 標準出力のエンコードを utf-8 に設定
sys.stdout.reconfigure(encoding='utf-8') # type: ignore
MAX_SIZE = 5 * 1024 * 1024  # 5MB
MAX_SIZE_LIMIT_MESSAGE = f"\n... （{MAX_SIZE}Byteを超えたため残りは省略）"

MODULE_HEAD_SIZE = 700  # ファイルごとの先頭の文字数
MODULE_TAIL_SIZE = 300  # ファイルごとの末尾の文字数
MODULE_SIZE = MODULE_HEAD_SIZE + MODULE_TAIL_SIZE  # ファイルごとの最大文字数の制限
MODULE_SIZE_LIMIT_MESSAGE = f"\n... （{MODULE_SIZE}文字数を超えるため中間は省略）...\n"

TARGET_PATH = r"/root/services/1on1"


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
]

EXCLUDED_EXTENSIONS = [
    ".xls",
    ".xlsx",
    ".xlsm",
    ".ico",
    ".svg",
    ".png",
    ".jpg",
    ".mp3",
    ".mp4",
    ".LICENSE.txt",
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
    ファイルの先頭と末尾を読み込み、MODULE_SIZE を超える場合は中間部分を省略
    複数のエンコーディングに対応し、エラー処理を強化
    """
    encodings = [
        ('utf-8', {'errors': 'replace'}),
        ('utf-8-sig', {'errors': 'replace'}),
        ('shift-jis', {'errors': 'replace'}),
        ('cp932', {'errors': 'replace'}),
        ('utf-16', {'errors': 'replace'}),
        ('utf-16le', {'errors': 'replace'}),
        ('utf-16be', {'errors': 'replace'}),
        ('ascii', {'errors': 'ignore'}),
    ]

    def process_file(f) -> str:
        """ファイルを読み込んで先頭と末尾を返す"""
        content = f.read()

        if len(content) > MODULE_SIZE:
            head_content = content[:MODULE_HEAD_SIZE]
            tail_content = content[-MODULE_TAIL_SIZE:]
            return head_content + MODULE_SIZE_LIMIT_MESSAGE + tail_content
        return content

    # テキストとして読み込みを試行
    for encoding, kwargs in encodings:
        try:
            with open(file_path, 'r', encoding=encoding, **kwargs) as f: # type: ignore
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
    print(f"デバッグ: 出力サイズ: {output_size} バイト (最大: {MAX_SIZE} バイト)")

    if output_size > MAX_SIZE:
        print(f"警告: 出力サイズ ({output_size} バイト) が最大許容サイズ ({MAX_SIZE} バイト) を超えています。")
        output = output[:MAX_SIZE] + MAX_SIZE_LIMIT_MESSAGE

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
