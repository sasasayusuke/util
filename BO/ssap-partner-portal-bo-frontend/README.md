# Sony Acceleration Platform Partner Portal BackOffice Frontend

[![Job Status](https://inspecode.rocro.com/badges/github.com/staqct/ssap-partner-portal-bo-frontend/status?token=Ei4mbKRIbtgyoFtDG_qpfISmTEXiDfrKyBdy0QcmO7g)](https://inspecode.rocro.com/jobs/github.com/staqct/ssap-partner-portal-bo-frontend/latest?completed=true)
[![Report](https://inspecode.rocro.com/badges/github.com/staqct/ssap-partner-portal-bo-frontend/report?token=Ei4mbKRIbtgyoFtDG_qpfISmTEXiDfrKyBdy0QcmO7g&branch=develop)](https://inspecode.rocro.com/reports/github.com/staqct/ssap-partner-portal-bo-frontend/branch/develop/summary)
[![develop_deploy](https://github.com/staqct/ssap-partner-portal-bo-frontend/actions/workflows/develop_deploy.yml/badge.svg?branch=develop)](https://github.com/staqct/ssap-partner-portal-bo-frontend/actions/workflows/develop_deploy.yml)
![Node.js](https://img.shields.io/badge/Node.js-14-1384C5.svg)
![Nuxt.js](https://img.shields.io/badge/Nuxt.js-2.15.7-1384C5.svg)

## プロジェクトのセットアップ

```
git clone https://github.com/staqct/ssap-partner-portal-bo-frontend.git
cd ssap-partner-portal-bo-frontend/
npm ci
```

nuxt コマンドを未インストールなら事前にインストール。

```
npm install -g nuxt
```

## 推奨の拡張機能のインストール

拡張機能の検索バーに `@recommended` と入力して出てくる拡張機能のインストールをおすすめします。

### ローカル環境としてコンテナを使う場合
※コンテナを利用しなくても可
1. Docker を起動させておく
2. [Remote Development](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.vscode-remote-extensionpack) VSCode 拡張機能をインストール
3. ~Remote-Container を起動~ 動作が重いため、ローカルで作業する。

### ローカルサーバーで起動

```
npm run [local|dev|sqa|evs|sup|prd]
```

うまくいかない場合は[こちら](https://github.com/staqct/ssap-partner-portal-bo-frontend/wiki/%E7%92%B0%E5%A2%83%E6%A7%8B%E7%AF%89)を参照。

### ビルド

```
npm run generate:[local|dev|sqa|evs|sup|prd]
```

※省略時は dev

### 静的解析

```
npm run lint
```

### テスト

```
npm run test
```

# デバッグ

## Vue.js devtools によるデバッグ

[Vue.js devtools](https://chrome.google.com/webstore/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd?hl=ja) を使うことでフロントの Chrome を使って要素や Vuex ストアのデバッグが可能。

![image](https://user-images.githubusercontent.com/49630732/146368053-ae79e444-37f7-474c-b5e4-264d53c6dc19.png)

## VS Code デバッグ

ローカルサーバーを立ち上げた状態で、ソースコードの任意位置にブレークポイントを打ち。
デバッガを起動する。

![image](https://user-images.githubusercontent.com/49630732/146368260-15f680eb-6874-4bf8-920e-01177f2f8404.png)

# コーディング規約

- [スタイルガイド（Vue.js）](https://jp.vuejs.org/v2/style-guide/index.html)
- [Atomic Design](https://github.com/staqct/ssap-partner-portal-bo-frontend/wiki/Atomic-Design)
- [コンポーネント設計](https://github.com/staqct/ssap-partner-portal-bo-frontend/wiki/%E3%82%B3%E3%83%B3%E3%83%9D%E3%83%BC%E3%83%8D%E3%83%B3%E3%83%88%E8%A8%AD%E8%A8%88)
- [ディレクトリ構成](https://github.com/staqct/ssap-partner-portal-bo-frontend/wiki/%E3%83%87%E3%82%A3%E3%83%AC%E3%82%AF%E3%83%88%E3%83%AA%E6%A7%8B%E6%88%90)
- [コーディング規約](https://github.com/staqct/ssap-partner-portal-bo-frontend/wiki/%E3%82%B3%E3%83%BC%E3%83%87%E3%82%A3%E3%83%B3%E3%82%B0%E8%A6%8F%E7%B4%84)
- [ファイル・コンポーネント命名規則](https://github.com/staqct/ssap-partner-portal-bo-frontend/wiki/%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB%E3%83%BB%E3%82%B3%E3%83%B3%E3%83%9D%E3%83%BC%E3%83%8D%E3%83%B3%E3%83%88%E5%91%BD%E5%90%8D%E8%A6%8F%E5%89%87)

# デプロイ

[GitHub Actions](https://github.com/staqct/ssap-partner-portal-bo-frontend/actions) によって実行される。

# その他

- [ブランチ運用フロー](https://github.com/staqct/ssap-partner-portal-tools/wiki/%E3%83%96%E3%83%A9%E3%83%B3%E3%83%81%E9%81%8B%E7%94%A8)

## BackOffice Backend API仕様書

- [Swagger UI](https://openapi.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/bo/swagger-ui/index.html)
- [Redoc](https://openapi.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/bo/redoc.html)

## Tips

- [Nuxt.js 公式サイト](https://nuxtjs.org/ja/)
- [Nuxt TypeScript](https://typescript.nuxtjs.org/ja/)
- [Vuetify](https://vuetifyjs.com/ja/)
- [TypeScript Deep Dive](https://typescript-jp.gitbook.io/deep-dive/)
- [Type Search](https://www.typescriptlang.org/dt/search?search=)
- [Vue.js devtools](https://chrome.google.com/webstore/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd?hl=ja)
- [Vue/Nuxt 開発効率を 3 倍にする VSCode 拡張機能セット](https://qiita.com/newt0/items/aeddc6a179ea3a464ed5)
