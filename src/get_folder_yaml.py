from __future__ import annotations

import os
from pathlib import Path

import yaml

TARGET_DIR = Path("/home/sdt_op/projects/gitea")

MAX_SIZE = 1 * 1024 * 1024  # 1MB
MAX_SIZE_LIMIT_MESSAGE = f"\n... （{MAX_SIZE}Byteを超えたため残りは省略）"

MODULE_HEAD_SIZE = 30  # ファイルごとの先頭の文字数
MODULE_TAIL_SIZE = 50  # ファイルごとの末尾の文字数
MODULE_SIZE = MODULE_HEAD_SIZE + MODULE_TAIL_SIZE  # ファイルごとの最大文字数の制限
MODULE_SIZE_LIMIT_MESSAGE = f"\n... （{MODULE_SIZE}文字数を超えるため中間は省略）...\n"

EXCLUDED_FOLDERS = [
    '.history', '.zip', '.git', '.svn', '.next', '__pycache__',
    'node_modules', 'gitlab', 'venv', 'history', 'dist',
]

EXCLUDED_EXTENSIONS = [
    ".xls", ".xlsx", ".xlsm", ".ico", ".svg", ".png", ".jpg",
    ".mp3", ".mp4", ".LICENSE.txt", ".exe", ".vbw", ".frx"
]

EXCLUDED_FILES = [
    "package-lock.json", "bundle.js"
]

def should_exclude(path: str | Path, file: str | None = None) -> bool:
    path_str = str(path)
    if any(part in EXCLUDED_FOLDERS for part in path_str.split(os.sep)):
        return True
    if file:
        if file in EXCLUDED_FILES:
            return True
        if any(file.endswith(ext) for ext in EXCLUDED_EXTENSIONS):
            return True
    return False

def get_file_structure_with_contents(base_path: Path) -> dict[str, any]:
    file_tree: dict[str, any] = {}

    for root, _dirs, files in os.walk(base_path):
        root_path = Path(root)
        if should_exclude(root_path):
            continue

        relative_path = root_path.relative_to(base_path)
        current = file_tree

        if str(relative_path) != ".":
            for part in relative_path.parts:
                current = current.setdefault(part, {})

        for file in files:
            if should_exclude(root_path, file):
                continue

            file_path = root_path / file
            
            # ファイルサイズチェック
            if file_path.stat().st_size > MAX_SIZE:
                current[file] = f"ファイルサイズ制限超過: {file_path.stat().st_size:,} バイト (最大: {MAX_SIZE:,} バイト)"
                continue
            
            try:
                with file_path.open("r", encoding="utf-8") as f:
                    content = f.read()
                
                # コンテンツサイズチェックと切り取り
                if len(content) > MODULE_SIZE:
                    head_content = content[:MODULE_HEAD_SIZE]
                    tail_content = content[-MODULE_TAIL_SIZE:]
                    current[file] = head_content + MODULE_SIZE_LIMIT_MESSAGE + tail_content
                else:
                    current[file] = content
                    
            except Exception as e:
                current[file] = f"<<読み取り失敗: {e}>>"

    return file_tree

def main() -> None:
    """メイン実行関数"""
    structure = get_file_structure_with_contents(TARGET_DIR)
    output_path = Path("output.yml")
    
    with output_path.open("w", encoding="utf-8") as f:
        yaml.dump(structure, f, allow_unicode=True, sort_keys=False, default_flow_style=False)
    
    print("✅ output.yml に保存しました。")


if __name__ == "__main__":
    main()
