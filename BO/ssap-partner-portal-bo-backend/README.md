# Sony Acceleration Platform PartnerPortal BackOffice Backend

[![Job Status](https://inspecode.rocro.com/badges/github.com/staqct/ssap-partner-portal-bo-backend/status?token=Qzapzgixb0zcuitkcOsqLtDHNRGNMu8WKyokwEdDbtE)](https://inspecode.rocro.com/jobs/github.com/staqct/ssap-partner-portal-bo-backend/latest?completed=true)
[![Report](https://inspecode.rocro.com/badges/github.com/staqct/ssap-partner-portal-bo-backend/report?token=Qzapzgixb0zcuitkcOsqLtDHNRGNMu8WKyokwEdDbtE&branch=develop)](https://inspecode.rocro.com/reports/github.com/staqct/ssap-partner-portal-bo-backend/branch/develop/summary)
[![develop_deploy](https://github.com/staqct/ssap-partner-portal-bo-backend/actions/workflows/develop_deploy.yml/badge.svg?branch=develop)](https://github.com/staqct/ssap-partner-portal-bo-backend/actions/workflows/develop_deploy.yml)
[![Open API deploy](https://github.com/staqct/ssap-partner-portal-bo-backend/actions/workflows/open_api_deploy.yml/badge.svg?branch=develop)](https://github.com/staqct/ssap-partner-portal-bo-backend/actions/workflows/open_api_deploy.yml)
![Version](https://img.shields.io/badge/ServerlessFramework-3.2.1-1384C5.svg)
![Python](https://img.shields.io/badge/Python-3.12-1384C5.svg)

## AWS に関する設定

ホスト OS の環境で、以下の設定を行ってください。
`~/.aws/config`

```
[profile ssap-partner-portal-dev]
region = ap-northeast-1
output = json
```

`~/.aws/credentials`

```
[ssap-partner-portal-dev]
aws_access_key_id=Parther Portal開発用AWSアカウントのアクセスキーID
aws_secret_access_key=Parther Portal開発用AWSアカウントのシークレットキー
```

上記の設定をすることにより、[devcontainer.json](https://github.com/staqct/ssap-partner-portal-bo-backend/blob/develop/.devcontainer/devcontainer.json#L13)でホスト OS 上の`ssap-partner-portal-dev`のプロファイルを参照するように設定してあるので、Parther Portal の開発 AWS 環境にアクセスできるようになります。<br>
<br>

## プロジェクトのセットアップ

[Rancher Desktop](https://rancherdesktop.io/)をインストールします。

リポジトリをクローンし、/ssap-partner-portal-bo-backend に移動します。

```
git clone https://github.com/staqct/ssap-partner-portal-bo-backend
cd ssap-partner-portal-bo-backend/
```

※Prisma Access に接続した状態でコンテナを利用する場合は「Prisma Access の設定」を対応してください。

ローカル開発環境としてコンテナを利用します（下記 「AWS に関する設定」を行ってから実施してください）。

1. 利用 OS に沿って[こちら](https://github.com/staqct/ssap-partner-portal-bo-backend/blob/develop/.devcontainer/devcontainer.json#L242-L247)をコメントイン、コメントアウトする（※macOS の場合、対応不要）
2. Rancher Desktop を起動させておく
3. [Remote Development](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.vscode-remote-extensionpack) VSCode 拡張機能をインストール
4. Remote-Container を起動

起動したコンテナ内のシェルで下記が実行できるか確認します。

```
npm install -g serverless@3
serverless -v
OR
sls -v
```

[API が呼び出せるか確認](https://github.com/staqct/ssap-partner-portal-bo-backend/wiki/%E8%AA%8D%E8%A8%BC)します。

## Prisma Access の設定

Prisma Access に接続した状態でコンテナを利用するには、下記の設定が必要になります。

1. .devcontainer/ssl フォルダを作成し、その中に[最新の証明書](https://cloudpack.account.box.com/login?redirect_url=%2Ffile%2F1417536964016&logout=true)を入れる
2. vscode を再読み込み
3. vscode 画面左下の緑の矢印をクリックした後に「再度コンテナで開く」を選択し、コンテナを起動する
4. コンテナ上で　`sudo vi /etc/pip.conf`　を実施
5. 下記をコピペして保存

```
[global]
trusted-host = pypi.python.org
               pypi.org
               files.pythonhosted.org
```

7. 下記のコマンドを実施

```
$ pip install -r requirements-dev.txt

$ echo -en 'openssl_conf = openssl_init\n\n[openssl_init]\nssl_conf = ssl_sect\n\n[ssl_sect]\nsystem_default = system_default_sect\n\n[system_default_sect]\nOptions = UnsafeLegacyRenegotiation' | sudo tee /etc/ssl/openssl.cnf

$ export OPENSSL_CONF=/etc/ssl/openssl.cnf
```

## デバッグ

1. サイドメニューのデバッグを選択し、デバッグ名が`Python: FastAPI`が選択されていることを確認し、デバッグ開始ボタンを押下する。
2. ローカルサーバーが立ち上がる。
3. ブレイクポイントを設定し、API をコール。

## コーディング規約

- [コーディングスタイル（PEP 8）](https://pep8-ja.readthedocs.io/ja/latest/)
- [コメントスタイル（Google Style Python Docstrings）](https://sphinxcontrib-napoleon.readthedocs.io/en/latest/example_google.html)

## デプロイ

[GitHub Actions](https://github.com/staqct/ssap-partner-portal-fo-backend/actions) によって実行される。

### 手動デプロイ

```
serverless deploy --stage dev --verbose
OR
sls deploy --stage dev --verbose
```

# その他

- [ブランチ運用フロー](https://github.com/staqct/ssap-partner-portal-tools/wiki/%E3%83%96%E3%83%A9%E3%83%B3%E3%83%81%E9%81%8B%E7%94%A8)

## API 仕様書

- [Open API](https://github.com/staqct/ssap-partner-portal-bo-backend/blob/develop/docs/api/openapi.yaml)
- [Swagger UI](https://openapi.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/bo/swagger-ui/index.html)
- [Redoc](https://openapi.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/bo/redoc.html)

## Tips

- [公式サイト](https://www.serverless.com/)
- [FastAPI](https://fastapi.tiangolo.com/ja/)
- [FastAPI 入門](https://zenn.dev/sh0nk/books/537bb028709ab9)
