#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
入館許可アプリの全画面を一斉に開くスクリプト

Usage:
    python open_all_sites.py
    python open_all_sites.py --base-url https://your-server.com
"""

import re
import webbrowser
import time
import argparse
from pathlib import Path
from configparser import ConfigParser


def parse_system_config(config_path):
    """
    システム設定.jsを解析してサイト情報とBASE_URLを取得

    Args:
        config_path: システム設定.jsのパス

    Returns:
        tuple: (サイト情報の辞書 {サイト名: ID}, BASE_URL)
    """
    with open(config_path, 'r', encoding='utf-8') as f:
        content = f.read()

    # SITE_INFOセクションを抽出（ネストした括弧に対応）
    # SITE_INFO: { から始まり、対応する } まで
    site_info_start = content.find('SITE_INFO:')
    if site_info_start == -1:
        raise ValueError("SITE_INFO セクションが見つかりませんでした")

    # SITE_INFO: { の開始位置を見つける
    brace_start = content.find('{', site_info_start)
    if brace_start == -1:
        raise ValueError("SITE_INFO の開始 { が見つかりませんでした")

    # 対応する閉じ括弧を見つける
    brace_count = 0
    brace_end = -1
    for i in range(brace_start, len(content)):
        if content[i] == '{':
            brace_count += 1
        elif content[i] == '}':
            brace_count -= 1
            if brace_count == 0:
                brace_end = i
                break

    if brace_end == -1:
        raise ValueError("SITE_INFO の終了 } が見つかりませんでした")

    site_info_content = content[brace_start:brace_end + 1]

    # BASE_URLを抽出
    base_url_pattern = r'BASE_URL:\s*["\']([^"\']+)["\']'
    base_url_match = re.search(base_url_pattern, site_info_content)
    base_url = base_url_match.group(1) if base_url_match else None

    # 各サイトのID情報を抽出
    # パターン: サイト名: { ID: "数値", ... }
    # BASE_URLは除外
    site_pattern = r'(\w+):\s*\{\s*ID:\s*["\'](\d+)["\']'
    sites = {}

    for match in re.finditer(site_pattern, site_info_content):
        site_name = match.group(1)
        site_id = match.group(2)
        if site_name != 'BASE_URL':  # BASE_URLは除外
            sites[site_name] = site_id

    return sites, base_url


def build_urls(sites, base_url):
    """
    サイト情報からURLを構築

    Args:
        sites: サイト情報の辞書
        base_url: ベースURL

    Returns:
        dict: {サイト名: URL}
    """
    urls = {}

    site_names_ja = {
        'ENTERE_CONTROL': '入館管理',
        'QR_LANDING': 'QR着地',
        'RECEPTION': '受付',
        'QR_READER': 'QR読取',
        'UNSCHEDULED_VISIT': '予定外来訪者受付',
        'STAFF_MASTER': '社員マスタ'
    }

    for site_name, site_id in sites.items():
        # STAFF_MASTER以外はpublishesを使う想定も可能
        # url = f"{base_url}/publishes/{site_id}/index"
        url = f"{base_url}/items/{site_id}/index"
        display_name = site_names_ja.get(site_name, site_name)
        urls[display_name] = url

    return urls


def open_urls(urls, interval=0.5, only=None):
    """
    URLを順番にブラウザで開く

    Args:
        urls: {サイト名: URL} の辞書
        interval: 各URLを開く間隔（秒）
        only: 開く画面の名前リスト（Noneの場合は全て開く）
    """
    # 開く画面をフィルタリング
    if only:
        filtered_urls = {k: v for k, v in urls.items() if k in only}
        if not filtered_urls:
            print(f"警告: 指定された画面が見つかりません: {only}")
            return
        urls = filtered_urls

    print("=" * 60)
    print(f"入館許可アプリ - {len(urls)}画面を開きます")
    print("=" * 60)
    print()

    for i, (name, url) in enumerate(urls.items(), 1):
        print(f"[{i}/{len(urls)}] {name}")
        print(f"  → {url}")
        webbrowser.open(url)

        # 最後のURL以外は待機
        if i < len(urls):
            time.sleep(interval)

    print()
    print("=" * 60)
    print("すべての画面を開きました")
    print("=" * 60)


def load_config_file(config_file_path):
    """config.iniファイルを読み込む"""
    config = ConfigParser()
    if config_file_path.exists():
        config.read(config_file_path, encoding='utf-8')
        return config
    return None


def main():
    # コマンドライン引数の設定
    parser = argparse.ArgumentParser(
        description='入館許可アプリの全画面を一斉に開きます',
        formatter_class=argparse.RawDescriptionHelpFormatter
    )
    parser.add_argument(
        '--base-url',
        help='PleasanterのベースURL (config.iniまたはデフォルト: http://localhost)'
    )
    parser.add_argument(
        '--interval',
        type=float,
        help='各URLを開く間隔（秒） (config.iniまたはデフォルト: 0.5)'
    )
    parser.add_argument(
        '--config',
        default='システム設定.js',
        help='システム設定.jsのパス (デフォルト: システム設定.js)'
    )
    parser.add_argument(
        '--only',
        nargs='+',
        help='開く画面を指定（例: --only 受付 QR読取）'
    )

    args = parser.parse_args()

    # スクリプトのディレクトリを基準にパスを解決
    script_dir = Path(__file__).parent
    config_path = script_dir / args.config
    config_ini_path = script_dir / 'config.ini'

    # config.iniを読み込む
    config = load_config_file(config_ini_path)

    # 設定値を決定（優先順位: コマンドライン > config.ini > デフォルト）
    if args.base_url:
        base_url = args.base_url
    elif config and config.has_option('server', 'base_url'):
        base_url = config.get('server', 'base_url')
    else:
        base_url = 'http://localhost'

    if args.interval is not None:
        interval = args.interval
    elif config and config.has_option('options', 'interval'):
        interval = config.getfloat('options', 'interval')
    else:
        interval = 0.5

    if args.only:
        only = args.only
    elif config and config.has_option('options', 'open_only'):
        only_str = config.get('options', 'open_only').strip()
        only = [s.strip() for s in only_str.split(',')] if only_str else None
    else:
        only = None

    if not config_path.exists():
        print(f"エラー: {config_path} が見つかりません")
        return 1

    try:
        # システム設定を解析
        print(f"システム設定を読み込み中: {config_path}")
        sites, config_base_url = parse_system_config(config_path)

        # BASE_URLを決定（優先順位: コマンドライン > システム設定.js > config.ini > デフォルト）
        if args.base_url:
            base_url = args.base_url
        elif config_base_url:
            base_url = config_base_url
        # else: base_url は既に config.ini またはデフォルトから設定済み

        # URLを構築
        print(f"ベースURL: {base_url}")
        urls = build_urls(sites, base_url)

        # URLを開く
        open_urls(urls, interval, only)

        return 0

    except Exception as e:
        print(f"エラー: {e}")
        import traceback
        traceback.print_exc()
        return 1


if __name__ == '__main__':
    exit(main())
