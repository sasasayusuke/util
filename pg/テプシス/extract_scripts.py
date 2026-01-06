"""
Pleasanter JSONからScript/ServerScript/HTML/Styleを抽出してtree構成で保存するスクリプト

使い方:
    python extract_scripts.py <JSONファイルパス>
    python extract_scripts.py  # カレントディレクトリのJSONを自動検出

出力:
    output/
    └── {SiteTitle}/
        ├── scripts/
        │   ├── 01_{Title}.js
        │   └── ...
        ├── server_scripts/
        │   ├── 01_{Title}.server.js
        │   └── ...
        ├── htmls/
        │   ├── 01_{Title}.html
        │   └── ...
        └── styles/
            ├── 01_{Title}.css
            └── ...

処理後:
    JSONファイルは old/ フォルダへ移動
"""

import json
import shutil
import sys
import re
from pathlib import Path


# 抽出対象の定義: (JSONキー, サブフォルダ名, 拡張子)
EXTRACT_TARGETS = [
    ('Scripts', 'scripts', '.js'),
    ('ServerScripts', 'server_scripts', '.server.js'),
    ('Htmls', 'htmls', '.html'),
    ('Styles', 'styles', '.css'),
]


def sanitize_filename(name: str) -> str:
    """ファイル名に使えない文字を置換"""
    invalid_chars = r'[<>:"/\\|?*]'
    return re.sub(invalid_chars, '_', name)


def extract_contents(json_path: Path) -> None:
    """JSONファイルからコンテンツを抽出してファイル出力"""

    # JSONファイル読み込み
    with open(json_path, 'r', encoding='utf-8-sig') as f:
        data = json.load(f)

    # Sites配列を処理
    sites = data.get('Sites', [])
    if not sites:
        raise ValueError('Sites が見つかりません')

    # 出力先ディレクトリ（JSONと同じ場所にoutputフォルダ）
    output_base = json_path.parent / 'output'

    for site in sites:
        site_title = site.get('Title', 'Unknown')
        site_settings = site.get('SiteSettings', {})

        # サイト用フォルダ
        site_dir = output_base / sanitize_filename(site_title)

        has_content = False

        # 各種コンテンツを抽出
        for json_key, subfolder, ext in EXTRACT_TARGETS:
            items = site_settings.get(json_key, [])
            if not items:
                continue

            has_content = True

            # サブフォルダ作成
            target_dir = site_dir / subfolder
            target_dir.mkdir(parents=True, exist_ok=True)

            # Id順にソート
            sorted_items = sorted(items, key=lambda x: x.get('Id', 0))

            # 各アイテムをファイル出力
            for idx, item in enumerate(sorted_items, start=1):
                item_title = item.get('Title', f'{json_key}_{idx}')
                item_body = item.get('Body', '')

                # ファイル名: 01_タイトル.ext
                filename = f'{idx:02d}_{sanitize_filename(item_title)}{ext}'
                filepath = target_dir / filename

                with open(filepath, 'w', encoding='utf-8') as f:
                    f.write(item_body)

                print(f'[OK] {filepath}')

        if not has_content:
            print(f'[SKIP] {site_title}: コンテンツなし')

    print(f'\n完了: {output_base}')

    # 処理済みJSONをoldフォルダへ移動
    move_to_old(json_path)


def move_to_old(json_path: Path) -> None:
    """処理済みJSONファイルをoldフォルダへ移動"""
    old_dir = json_path.parent / 'old'
    old_dir.mkdir(parents=True, exist_ok=True)

    dest = old_dir / json_path.name
    shutil.move(str(json_path), str(dest))
    print(f'[MOVE] {json_path.name} -> old/')


def find_json_file() -> Path:
    """カレントディレクトリからJSONファイルを検索"""
    current = Path('.')
    json_files = list(current.glob('*.json'))

    if not json_files:
        raise FileNotFoundError('JSONファイルが見つかりません')

    if len(json_files) == 1:
        return json_files[0]

    # 複数ある場合は最新を選択
    return max(json_files, key=lambda p: p.stat().st_mtime)


def main():
    # 引数からJSONパスを取得、なければ自動検出
    if len(sys.argv) > 1:
        json_path = Path(sys.argv[1])
    else:
        json_path = find_json_file()
        print(f'[INFO] 自動検出: {json_path}')

    if not json_path.exists():
        raise FileNotFoundError(f'ファイルが見つかりません: {json_path}')

    extract_contents(json_path)


if __name__ == '__main__':
    main()
