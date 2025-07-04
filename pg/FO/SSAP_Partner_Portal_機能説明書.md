# SSAP Partner Portal Front Office System 機能説明書

## 目次

1. [システム概要](#1-システム概要)
2. [アーキテクチャ構成](#2-アーキテクチャ構成)
3. [主要機能](#3-主要機能)
4. [API エンドポイント](#4-api-エンドポイント)
5. [データモデル](#5-データモデル)
6. [認証・認可](#6-認証認可)
7. [外部連携](#7-外部連携)
8. [開発・デバッグ手順](#8-開発デバッグ手順)
9. [運用・保守](#9-運用保守)

---

## 1. システム概要

### 1.1 システム名
**SSAP Partner Portal Front Office System**

### 1.2 目的
ソニーのスタートアップ支援プラットフォーム（SSAP）において、ソルバー（専門家）の管理、案件管理、顧客管理、アンケート機能などを提供する統合パートナーポータルシステム。

### 1.3 主要ユーザー
- **APT（管理者）**: システム全体の管理権限
- **SOLVER_STAFF**: ソルバー担当者
- **その他業務ロール**: 各種業務担当者

### 1.4 システム特徴
- **サーバーレスアーキテクチャ**: AWS Lambda ベースの完全サーバーレス
- **NoSQL データベース**: DynamoDB による高性能・高可用性
- **マイクロサービス指向**: 機能ごとの独立したサービス層
- **型安全性**: TypeScript（フロントエンド）+ Pydantic（バックエンド）

---

## 2. アーキテクチャ構成

### 2.1 システム構成図

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   フロントエンド    │    │   バックエンド API  │    │   外部システム    │
│                 │    │                 │    │                 │
│   Nuxt.js 3     │◄──►│   FastAPI       │◄──►│   Salesforce    │
│   TypeScript    │    │   Python 3.12   │    │   AWS Services  │
│   Vuetify       │    │   Lambda        │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                        │                        │
         │              ┌─────────────────┐                │
         │              │   データベース   │                │
         │              │                 │                │
         └──────────────►│   DynamoDB     │◄───────────────┘
                        │   NoSQL        │
                        └─────────────────┘
```

### 2.2 技術スタック

#### バックエンド
- **フレームワーク**: FastAPI 0.111.0
- **ランタイム**: Python 3.12
- **デプロイメント**: AWS Lambda + Serverless Framework
- **データベース**: DynamoDB（PynamoDB 5.5.1）
- **認証**: JWT + AWS Cognito
- **API Gateway**: AWS API Gateway

#### フロントエンド
- **フレームワーク**: Nuxt.js 3
- **言語**: TypeScript
- **UIフレームワーク**: Vuetify
- **状態管理**: Vuex
- **HTTP クライアント**: Axios

#### インフラストラクチャ
- **AWS Lambda**: サーバーレス実行環境
- **AWS DynamoDB**: NoSQL データベース
- **AWS Cognito**: 認証サービス
- **AWS S3**: ファイルストレージ
- **AWS SES**: メール送信
- **AWS SQS**: メッセージキュー

---

## 3. 主要機能

### 3.1 ソルバー管理機能

#### 3.1.1 個人ソルバー管理
- **ソルバー登録**: 新規ソルバーの登録と基本情報管理
- **ソルバー認定**: 候補者から正式認定への変更処理
- **プロフィール管理**: 経歴、スキル、専門分野の管理
- **稼働率・単価管理**: 個別およびファイル一括更新
- **スクリーニング機能**: ソルバーの適正評価

#### 3.1.2 生年月日不明データ対応
- **不明チェックボックス**: 生年月日が不明な場合の設定
- **データ連携**: Salesforce への不明フラグ連携
- **入力制御**: 不明設定時の入力欄グレーアウト

#### 3.1.3 法人ソルバー管理
- **法人情報管理**: 法人ソルバーの基本情報
- **契約管理**: 契約条件・料金体系の管理

### 3.2 案件管理機能

#### 3.2.1 プロジェクト管理
- **案件登録**: 新規案件の登録と基本情報管理
- **案件詳細**: 案件の詳細情報とステータス管理
- **案件検索**: 各種条件による案件検索・絞り込み

#### 3.2.2 案件カルテ機能
- **カルテ作成**: 案件進捗の記録・管理
- **履歴管理**: 案件の変更履歴・コメント管理
- **ステータス追跡**: 案件の進行状況可視化

#### 3.2.3 スケジュール管理
- **スケジュール登録**: 案件のスケジュール管理
- **リマインダー**: 期限通知・アラート機能

### 3.3 顧客管理機能

#### 3.3.1 顧客情報管理
- **顧客登録**: 取引先企業の基本情報管理
- **連絡先管理**: 担当者情報・連絡先の管理
- **取引履歴**: 過去の取引・案件履歴

### 3.4 アンケート機能

#### 3.4.1 アンケート管理
- **アンケート作成**: 案件満足度調査の作成
- **回答収集**: アンケート回答の収集・集計
- **匿名アンケート**: 匿名回答機能

#### 3.4.2 アンケート分析
- **結果分析**: 回答結果の統計分析
- **レポート生成**: 分析結果のレポート出力

### 3.5 工数管理機能

#### 3.5.1 支援工数管理
- **工数登録**: 支援工数の記録・管理
- **工数集計**: 期間別・案件別の工数集計
- **工数分析**: 工数データの分析・可視化

### 3.6 通知機能

#### 3.6.1 お知らせ管理
- **通知作成**: システム通知の作成・管理
- **通知配信**: ユーザー別通知の配信
- **通知履歴**: 通知の送信履歴・確認状況

### 3.7 マスター管理機能

#### 3.7.1 マスターデータ管理
- **コードマスター**: 各種コードマスターの管理
- **設定管理**: システム設定の管理
- **データメンテナンス**: マスターデータの更新・削除

---

## 4. API エンドポイント

### 4.1 認証 API

| メソッド | エンドポイント | 説明 |
|---------|---------------|------|
| POST | `/auth/login` | JWT認証・ログイン |

### 4.2 ソルバー管理 API

| メソッド | エンドポイント | 説明 |
|---------|---------------|------|
| POST | `/solvers` | ソルバー作成 |
| GET | `/solvers` | ソルバー一覧取得 |
| GET | `/solvers/{solver_id}` | ソルバー詳細取得 |
| PUT | `/solvers/{solver_id}` | ソルバー更新 |
| DELETE | `/solvers/{solver_id}` | ソルバー削除 |
| PATCH | `/solvers/{solver_id}` | ソルバー認定状態変更 |
| PUT | `/solvers/utilization-rate/{solver_corporation_id}` | 稼働率・単価一括更新 |

### 4.3 ユーザー管理 API

| メソッド | エンドポイント | 説明 |
|---------|---------------|------|
| GET | `/users` | ユーザー一覧取得 |
| GET | `/users/{user_id}` | ユーザー詳細取得 |
| POST | `/users` | ユーザー作成 |
| PUT | `/users/{user_id}` | ユーザー更新 |
| DELETE | `/users/{user_id}` | ユーザー削除 |

### 4.4 案件管理 API

| メソッド | エンドポイント | 説明 |
|---------|---------------|------|
| GET | `/projects` | 案件一覧取得 |
| GET | `/projects/{project_id}` | 案件詳細取得 |
| POST | `/projects` | 案件作成 |
| PUT | `/projects/{project_id}` | 案件更新 |
| DELETE | `/projects/{project_id}` | 案件削除 |

### 4.5 顧客管理 API

| メソッド | エンドポイント | 説明 |
|---------|---------------|------|
| GET | `/customers` | 顧客一覧取得 |
| GET | `/customers/{customer_id}` | 顧客詳細取得 |
| POST | `/customers` | 顧客作成 |
| PUT | `/customers/{customer_id}` | 顧客更新 |
| DELETE | `/customers/{customer_id}` | 顧客削除 |

### 4.6 アンケート API

| メソッド | エンドポイント | 説明 |
|---------|---------------|------|
| GET | `/surveys` | アンケート一覧取得 |
| GET | `/surveys/{survey_id}` | アンケート詳細取得 |
| POST | `/surveys` | アンケート作成 |
| PUT | `/surveys/{survey_id}` | アンケート更新 |
| DELETE | `/surveys/{survey_id}` | アンケート削除 |

### 4.7 その他 API

| メソッド | エンドポイント | 説明 |
|---------|---------------|------|
| GET | `/health` | ヘルスチェック |
| GET | `/masters` | マスターデータ取得 |
| GET | `/notifications` | 通知一覧取得 |
| GET | `/man-hours` | 工数データ取得 |
| GET | `/schedules` | スケジュール取得 |

---

## 5. データモデル

### 5.1 ソルバーモデル (SolverModel)

```python
class SolverModel(Model):
    # 基本情報
    solver_id: str              # ソルバーID（プライマリキー）
    solver_name: str            # ソルバー名
    solver_email: str           # メールアドレス
    birth_date: datetime        # 生年月日
    birth_date_unknown: bool    # 生年月日不明フラグ
    
    # 認定情報
    certification_status: str   # 認定状態（候補者/正式認定）
    certification_date: datetime # 認定日
    
    # 業務情報
    utilization_rate: float     # 稼働率
    unit_price: int            # 単価
    specialty_areas: List[str] # 専門分野
    
    # GSI
    GSI1PK: str                # 検索用インデックス
    GSI1SK: str                # 検索用ソートキー
```

### 5.2 プロジェクトモデル (ProjectModel)

```python
class ProjectModel(Model):
    # 基本情報
    project_id: str             # 案件ID（プライマリキー）
    project_name: str           # 案件名
    project_status: str         # 案件ステータス
    start_date: datetime        # 開始日
    end_date: datetime          # 終了日
    
    # 顧客情報
    customer_id: str            # 顧客ID
    customer_name: str          # 顧客名
    
    # 担当者情報
    assigned_solvers: List[str] # 担当ソルバーID一覧
    project_manager: str        # プロジェクトマネージャー
```

### 5.3 ユーザーモデル (UserModel)

```python
class UserModel(Model):
    # 基本情報
    user_id: str                # ユーザーID（プライマリキー）
    cognito_user_id: str        # Cognito ユーザーID
    username: str               # ユーザー名
    email: str                  # メールアドレス
    
    # 権限情報
    role: str                   # ユーザーロール
    permissions: List[str]      # 権限一覧
    
    # 状態情報
    is_active: bool             # アクティブ状態
    last_login: datetime        # 最終ログイン日時
```

### 5.4 アンケートモデル (SurveyModel)

```python
class SurveyModel(Model):
    # 基本情報
    survey_id: str              # アンケートID（プライマリキー）
    survey_title: str           # アンケートタイトル
    survey_type: str            # アンケート種別
    
    # 期間情報
    start_date: datetime        # 開始日
    end_date: datetime          # 終了日
    
    # 対象情報
    target_projects: List[str]  # 対象案件一覧
    target_users: List[str]     # 対象ユーザー一覧
    
    # 設定情報
    is_anonymous: bool          # 匿名フラグ
    is_active: bool             # アクティブ状態
```

---

## 6. 認証・認可

### 6.1 認証方式

#### 6.1.1 JWT認証
- **発行元**: SSAP公式サイト
- **検証方式**: JWK（JSON Web Key）による署名検証
- **トークン有効期限**: 設定可能
- **リフレッシュトークン**: 対応

#### 6.1.2 AWS Cognito連携
- **ユーザープール**: AWS Cognito によるユーザー管理
- **フェデレーション**: 外部ID プロバイダーとの連携
- **MFA**: 多要素認証対応

### 6.2 認可システム

#### 6.2.1 ロールベースアクセス制御（RBAC）

| ロール | 権限 | 説明 |
|--------|------|------|
| APT | 全権限 | システム管理者 |
| SOLVER_STAFF | ソルバー管理権限 | ソルバー担当者 |
| PROJECT_MANAGER | 案件管理権限 | プロジェクト管理者 |
| SURVEY_ADMIN | アンケート管理権限 | アンケート管理者 |

#### 6.2.2 権限チェック機能
- **リクエストレベル**: API エンドポイントごとの権限チェック
- **データレベル**: データアクセス時の権限チェック
- **UIレベル**: フロントエンドでの表示制御

### 6.3 セキュリティ対策

#### 6.3.1 API セキュリティ
- **CORS設定**: Cross-Origin Resource Sharing の適切な設定
- **レート制限**: API 呼び出し回数の制限
- **入力検証**: Pydantic による厳密な入力検証

#### 6.3.2 データ保護
- **暗号化**: 機密データの暗号化
- **アクセスログ**: 全アクセスのログ記録
- **監査証跡**: データ変更の監査ログ

---

## 7. 外部連携

### 7.1 Salesforce連携

#### 7.1.1 データ同期
- **ソルバー情報同期**: 個人ソルバーデータの Salesforce 連携
- **生年月日不明フラグ**: 不明設定の連携
- **認定状態同期**: ソルバー認定状態の同期

#### 7.1.2 メール連携
- **通知メール**: Salesforce 経由でのメール送信
- **リマインダー**: 期限通知メールの送信

### 7.2 AWS Services連携

#### 7.2.1 S3連携
- **ファイルアップロード**: ドキュメント・画像のアップロード
- **ファイルダウンロード**: 登録ファイルのダウンロード
- **バックアップ**: データベースバックアップの保存

#### 7.2.2 SES連携
- **メール送信**: システムメールの送信
- **メール配信**: 一斉メール配信機能
- **配信ログ**: メール送信履歴の記録

#### 7.2.3 SQS連携
- **非同期処理**: バックグラウンドジョブの実行
- **バッチ処理**: 定期処理・データ処理の実行
- **メッセージキュー**: 処理キューの管理

---

## 8. 開発・デバッグ手順

### 8.1 DevContainer を使用したデバッグ

#### 8.1.1 DevContainer 起動
```bash
# 1. VSCodeでプロジェクトを開く
code /home/sdt_op/projects/util/pg/FO/ssap-partner-portal-fo-backend

# 2. VSCodeコマンドパレットで「Dev Containers: Reopen in Container」を実行
```

#### 8.1.2 デバッグ環境構築
```bash
# 環境変数設定
export STAGE=local
export PYTHONPATH=/workspaces/ssap-partner-portal-fo-backend

# FastAPIサーバーを起動
uvicorn app.main:app --reload --host 0.0.0.0 --port 8000
```

#### 8.1.3 VSCodeデバッグ設定
`.vscode/launch.json`:
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "FastAPI DevContainer Debug",
            "type": "python",
            "request": "launch",
            "program": "/usr/local/pip-global/bin/uvicorn",
            "args": ["app.main:app", "--reload", "--host", "0.0.0.0", "--port", "8000"],
            "cwd": "/workspaces/ssap-partner-portal-fo-backend",
            "env": {
                "STAGE": "local",
                "PYTHONPATH": "/workspaces/ssap-partner-portal-fo-backend"
            },
            "console": "integratedTerminal"
        }
    ]
}
```

### 8.2 SwaggerUI を使用したデバッグ

#### 8.2.1 ブレークポイント設置
```python
# app/routers/solver.py の例
@router.get("/solvers")
async def get_solvers():
    breakpoint()  # ここで停止
    return {"solvers": []}
```

#### 8.2.2 SwaggerUI でのテスト
1. **SwaggerUIアクセス**: `http://localhost:8000/docs`
2. **API実行**: 対象エンドポイントで「Try it out」→「Execute」
3. **デバッグ**: ブレークポイントで処理停止・変数確認

### 8.3 テスト実行

#### 8.3.1 単体テスト
```bash
# pytest実行
pytest tests/

# カバレッジ付きテスト
pytest --cov=app tests/
```

#### 8.3.2 統合テスト
```bash
# 統合テスト実行
pytest tests/integration/
```

---

## 9. 運用・保守

### 9.1 環境構成

#### 9.1.1 環境一覧
- **Local**: ローカル開発環境
- **Dev**: 開発環境
- **SQA**: 品質保証環境
- **EVS**: 脆弱性試験環境
- **SUP**: サポート環境
- **PRD**: 本番環境

#### 9.1.2 デプロイメント
```bash
# Serverless デプロイ
serverless deploy --stage dev

# 特定機能のデプロイ
serverless deploy function --function app --stage dev
```

### 9.2 監視・ログ

#### 9.2.1 CloudWatch Logs
- **アプリケーションログ**: `/aws/lambda/partner-portal-frontoffice-dev-app`
- **バッチログ**: `/aws/lambda/partner-portal-frontoffice-batch_remind`
- **メール送信ログ**: `/aws/lambda/partner-portal-frontoffice-dev-mail_sender`

#### 9.2.2 ログ確認
```bash
# アプリケーションログ
tail -f logs/application.log

# エラーログ
tail -f logs/error.log

# Salesforce連携ログ
tail -f logs/salesforce.log
```

### 9.3 バックアップ・リストア

#### 9.3.1 DynamoDB バックアップ
- **自動バックアップ**: Point-in-time Recovery 設定
- **手動バックアップ**: 定期的な手動バックアップ
- **クロスリージョンバックアップ**: 災害対策

#### 9.3.2 S3 バックアップ
- **バージョニング**: ファイルのバージョン管理
- **レプリケーション**: クロスリージョンレプリケーション
- **ライフサイクル管理**: 古いファイルの自動削除

### 9.4 トラブルシューティング

#### 9.4.1 よくある問題と対処法

| 問題 | 原因 | 対処法 |
|------|------|--------|
| API エラー 500 | Lambda 関数のエラー | CloudWatch Logs 確認 |
| 認証エラー | JWT トークンの問題 | トークンの再発行 |
| データ取得エラー | DynamoDB 接続エラー | 接続設定確認 |
| Salesforce連携エラー | API認証の問題 | 認証情報の確認 |

#### 9.4.2 パフォーマンス調整
- **Lambda メモリ設定**: 処理量に応じたメモリ調整
- **DynamoDB 設定**: Read/Write キャパシティの調整
- **API Gateway 設定**: スロットリング設定の調整

---

## 付録

### A. 用語集

| 用語 | 説明 |
|------|------|
| SSAP | ソニーのスタートアップ支援プラットフォーム |
| ソルバー | 専門的な知識・技術を持つ支援者 |
| APT | 管理者権限を持つユーザーロール |
| カルテ | 案件の進捗・履歴を記録する機能 |
| GSI | Global Secondary Index (DynamoDB) |
| JWK | JSON Web Key |
| RBAC | Role-Based Access Control |

### B. 関連リンク

- [AWS Lambda Documentation](https://docs.aws.amazon.com/lambda/)
- [FastAPI Documentation](https://fastapi.tiangolo.com/)
- [DynamoDB Documentation](https://docs.aws.amazon.com/dynamodb/)
- [Nuxt.js Documentation](https://nuxt.com/)
- [Serverless Framework Documentation](https://www.serverless.com/framework/docs/)

### C. 更新履歴

| バージョン | 更新日 | 更新内容 |
|-----------|--------|----------|
| 1.0 | 2025-07-03 | 初版作成 |

---

**文書作成者**: Claude Code  
**最終更新日**: 2025-07-03  
**承認者**: [TBD]