# フォルダ構成
FastAPI
├── start_service.ps1                       サービス起動スクリプト（環境変数も保持）
├── requirements.txt                        Pythonの必要ライブラリバージョンも記載
├── create_service.bat                      本番環境でサービスに登録するバッチ
│
├── app/								バックエンド
│   ├── api/								APIエンドポイントの実装
│   ├── core/								設定、例外処理、ログ設定などの機能　　
│   ├── models/								入出力等のデータモデルの定義
│   ├── services/							ビジネスロジックの実装
│   │   ├── mitsumori_hacchu/
│   │   ├── seikyu_urikake/
│   │   ├── shiharai_kaikake/
│   │   :        :
│   │   └── uriage_nyukin/
│   ├── templates/							HTMLテンプレートやExcelテンプレートの格納
│   │   ├── html/
│   │   ├── img/
│   │   └── xlsx/
│   └── utils/								共通関数
│
├── public/								フロントエンド
│   ├── assets/								画像などの静的アセット
│   └── pleasanter/							Pleasanter関連のスクリプトなど（バックエンドを通じて配信される）
│       ├── 00_動作未保障/
│       ├── 01_共通/
│       ├── 02_見積・発注/
│       ├── 03_仕入・支払/
│       ├── 04_売上・入金/
│       ├── 05_請求・売掛/
│       ├── 06_支払・買掛/
│       ├── 07_統計/
│       ├── 08_経費/
│       ├── 09_保守/
│       └── 10_マスタ/
│
├── log/								ログ管理
│   ├── start_log.txt						起動までのログファイル（start_service.ps1を実行するとできる：毎回刷新）
│   ├── app_dev.log							開発モードで起動中のログファイル（start_service.ps1を実行するとできる：追記モード）
│   └── app_prod.log						本番モードで起動中のログファイル（start_service.ps1を実行するとできる：追記モード）　
│
├── venv/								仮想環境内にpip installを行う
│
└── cert/								HTTPSで動かすための証明書などを格納


# 開発手順
## 1. ローカル環境での作業
### 1.1 ローカルでのファイルの修正
- 修正や追加が必要なファイルをローカル環境で修正する

### 1.2 ローカルでの動作確認
- ローカル環境でできる動作確認を行う
　APIの確認等は、開発環境へいれてから動作確認でもOK
### 1.3 SVN へのソースコミット

## 2. 開発環境での作業
### 2.1 開発環境へのRDP接続
IP：192.168.10.54

- ユーザ：adinistrator
PW：1qaz!QAZ

- ユーザ：sdt_op
PW：1qaz!QAZ

*基本的には 下記の役割で使い分ける
- adinistrator：  開発サーバー内のVScodeでデバッグモードを使って処理を止めながら確認したりしたいとき
- sdt_op：	SVNの更新したいだけのとき


### 2.2 開発環境でのSVN の更新
- C:\svn へ移動してSNV更新

### 2.3 開発環境でのファイルの修正
- VScode を開き対象ファイルの修正

### 2.4 デバッグモードの確立
- VScode にて Run and Debug(F5)を押下
- デバッグモードを使って処理を止めたりしながら修正

### 2.5 開発環境での動作確認
- ローカル環境からの動作確認
- 開発環境の各エンドポイントにアクセスし、正常に動作することを確認するなど

### 2.6 修正完了後
- ファイル修正があった場合 忘れずにSVNへコミットしローカルでSVN更新する
- デバッグモードを使用した場合、サーバー再起動
（デバッグモードのままだと、リクエストがそこで止まり続けるので）
```bash
C:\svn\trunk\src\sanwa\FastAPI\start_service.ps1
```

# 本番環境でのサービスの登録
## 1 start_service.ps1 内の環境変数を変更し起動
- $env:ENVIRONMENT = "product"

## 2 コマンドプロンプト を管理者で開く
sc create sanwa binPath= "C:\svn\trunk\src\sanwa\create_service.bat" start= auto
- OS再起動時に自動起動するようにする




うまくいかなかったら笹木まで
