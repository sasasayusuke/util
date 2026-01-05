# 入館許可アプリ - 一斉起動スクリプト

システム設定.jsから全画面のURLを読み取り、Pythonで一斉にブラウザで開くスクリプトです。

## 必要環境

- Python 3.6以上
- 標準ライブラリのみ使用（追加インストール不要）

## ファイル構成

```
入館許可アプリ/
├── システム設定.js        # システム設定（URL情報）
├── open_all_sites.py     # 一斉起動スクリプト
├── config.ini            # 設定ファイル（オプション）
└── README_open_all_sites.md  # このファイル
```

## 基本的な使い方

### 1. 設定ファイル（config.ini）を編集

```ini
[server]
# PleasanterサーバーのベースURL
base_url = https://your-server.com

[options]
# 各URLを開く間隔（秒）
interval = 0.5

# 開く画面を指定（空白の場合は全て開く）
open_only =
```

### 2. スクリプトを実行

```bash
# 全画面を開く
python open_all_sites.py

# 特定の画面だけ開く
python open_all_sites.py --only 受付 QR読取

# ベースURLを指定して開く
python open_all_sites.py --base-url https://your-server.com
```

## コマンドラインオプション

| オプション | 説明 | 例 |
|----------|------|-----|
| `--base-url` | PleasanterのベースURL | `--base-url https://example.com` |
| `--interval` | 各URLを開く間隔（秒） | `--interval 1.0` |
| `--config` | システム設定.jsのパス | `--config custom_config.js` |
| `--only` | 開く画面を指定 | `--only 受付 QR読取` |

## 使用例

### 例1: 全画面を開く（config.ini使用）

```bash
python open_all_sites.py
```

出力:
```
システム設定を読み込み中: システム設定.js
ベースURL: https://your-server.com
============================================================
入館許可アプリ - 6画面を開きます
============================================================

[1/6] 入館管理
  → https://your-server.com/items/5/index
[2/6] QR着地
  → https://your-server.com/items/6/index
[3/6] 受付
  → https://your-server.com/items/4/index
[4/6] QR読取
  → https://your-server.com/items/2/index
[5/6] 予定外来訪者受付
  → https://your-server.com/items/3/index
[6/6] 社員マスタ
  → https://your-server.com/items/9/index

============================================================
すべての画面を開きました
============================================================
```

### 例2: 特定の画面だけ開く

```bash
python open_all_sites.py --only 受付 QR読取
```

### 例3: config.iniを使わずコマンドラインで全指定

```bash
python open_all_sites.py --base-url https://example.com --interval 1.0
```

### 例4: 特定の画面をconfig.iniで指定

config.iniを編集:
```ini
[options]
open_only = 受付, QR読取, 予定外来訪者受付
```

実行:
```bash
python open_all_sites.py
```

## 開く画面の一覧

スクリプトは以下の画面を開くことができます：

- **入館管理** (ENTERE_CONTROL)
- **QR着地** (QR_LANDING)
- **受付** (RECEPTION)
- **QR読取** (QR_READER)
- **予定外来訪者受付** (UNSCHEDULED_VISIT)
- **社員マスタ** (STAFF_MASTER)

## 設定の優先順位

設定値は以下の優先順位で決定されます：

1. **コマンドライン引数** （最優先）
2. **config.ini** （次点）
3. **デフォルト値** （最後）

例：
```bash
# config.iniにbase_url = http://localhost と書いてあっても、
# コマンドラインのほうが優先される
python open_all_sites.py --base-url https://production-server.com
```

## トラブルシューティング

### エラー: システム設定.js が見つかりません

→ スクリプトと同じディレクトリに `システム設定.js` があることを確認してください。

### URLが正しく開かない

→ `config.ini` の `base_url` が正しいか確認してください。

### 画面が開かない

→ デフォルトブラウザが正しく設定されているか確認してください。

## カスタマイズ

### URLの形式を変更する場合

`open_all_sites.py` の `build_urls()` 関数を編集してください：

```python
def build_urls(sites, base_url):
    # ...
    for site_name, site_id in sites.items():
        # items から publishes に変更する例
        url = f"{base_url}/publishes/{site_id}/index"
        # ...
```

## ライセンス

このスクリプトは入館許可アプリの一部として提供されます。
