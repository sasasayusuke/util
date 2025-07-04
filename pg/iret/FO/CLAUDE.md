# Claude開発支援ルール

## 📋 概要

このファイルは、Claude Code使用時のフローチャート作成支援における標準ルールとテンプレートを定義します。

---

## 🎯 フローチャート作成支援ルール

### 基本原則
1. **新人でも理解可能** - 専門用語の説明、具体的な手順記載
2. **既存資料の活用** - ルートディレクトリのPDF/SVGファイルを読み取り、参考にする
3. **ソースコード準拠** - 画面操作の記載はソースコードのラベル名に従う
4. **実装調査を優先** - フローチャート作成前にソースコードを解析
5. **視覚的にわかりやすく** - Mermaid記法等を使用して見やすいフローチャートを作成

### フローチャート作成手順

#### 1. 既存資料の確認
```
1. ルートディレクトリのPDF/SVGファイルを読み取る
2. 既存のフロー図やシステム構成図を参考にする
3. 業務フローや画面遷移パターンを把握する
```

#### 2. ソースコード解析
```
1. 関連する画面コンポーネントを特定
2. ボタンやリンクのラベル名を確認（ja.jsonから取得）
3. 画面遷移ロジックを解析
4. API呼び出しの流れを確認
```

#### 3. フローチャート作成
```
1. 開始・終了点を明確にする
2. 各ステップで「どの画面の」「どのボタン/操作」かを明記
3. 条件分岐は具体的な判定条件を記載
4. エラー処理も含める
```

---

## 📊 フローチャートテンプレート

### ファイル命名規則
```
[機能名]フローチャート.md
例: ユーザー登録フローチャート.md
    案件作成フローチャート.md
```

### 必須セクション構成

```markdown
# [機能名]フローチャート

## 📋 概要
[機能の概要、業務上の目的、対象ユーザー]

---

## 🔍 参考資料
- 参照したPDF: [ファイル名]
- 参照したSVG: [ファイル名]
- 関連ソースコード: [ファイルパス]

---

## 📊 フローチャート（プレビュー用）

```mermaid
flowchart TD
    Start([開始]) --> A[ステップ1の説明<br/>画面: ○○画面<br/>操作: 「○○」ボタンクリック]
    A --> B{条件分岐<br/>判定: ○○の場合}
    B -->|Yes| C[ステップ2A]
    B -->|No| D[ステップ2B]
    C --> End([終了])
    D --> End
```

---

## 📁 draw.ioファイル
- **ファイル名**: `[機能名]フローチャート.drawio`
- **配置場所**: ルートディレクトリ
- draw.ioで開いて編集可能です

---

## 🖥️ 画面別操作詳細

### 1. [画面名]（URL: /path/to/screen）
**ソースファイル**: `src/views/[ComponentName].vue`

#### 操作可能な要素:
| ラベル名（表示） | ソース上の名称 | 動作 | 次の遷移先 |
|----------------|--------------|------|-----------|
| [表示ラベル] | [変数名/ID] | [動作説明] | [遷移先] |

#### 入力項目:
| 項目名（表示） | フィールド名 | 必須/任意 | バリデーション |
|--------------|-------------|----------|--------------|
| [表示名] | [field_name] | 必須/任意 | [制約] |

---

## 🔄 処理フロー詳細

### STEP 1: [処理名]
**実行画面**: [画面名]
**トリガー**: [ボタン名]クリック

1. **画面操作**
   - 場所: [画面上の位置]
   - 操作: [具体的な操作方法]
   - ラベル: 「[実際の表示テキスト]」

2. **内部処理**
   - API呼び出し: `[エンドポイント]`
   - データ処理: [処理内容]

3. **結果**
   - 成功時: [次の動作]
   - エラー時: [エラー処理]

---

## ⚠️ 注意事項・エラー処理

### よくあるエラーパターン
| エラー種別 | 発生条件 | 表示メッセージ | 対処法 |
|-----------|---------|--------------|--------|
| [エラー名] | [条件] | [メッセージ] | [対処法] |

---

## 👥 ロール別の違い
| ロール | 利用可能機能 | 制限事項 |
|--------|------------|---------|
| [ロール名] | [機能リスト] | [制限] |

---

## 📝 補足情報
- [その他の重要な情報]
- [新人向けの説明]
- [業務上の注意点]
```

---

## 🛠️ ソースコード解析ガイドライン

### 解析対象
1. **画面コンポーネント**
   - Vue.jsファイル（.vue）
   - 画面表示ラベルの取得元

2. **多言語ファイル**
   - `locales/ja.json` - 日本語ラベル
   - 実際の画面表示テキスト

3. **ルーティング**
   - `router/index.js` - 画面遷移定義
   - URLパスと画面の対応

4. **API定義**
   - APIエンドポイント
   - リクエスト/レスポンス形式

### 解析時の注意点
- **ソースコードは変更しない**
- 実際のラベル名を正確に取得
- コメントやドキュメントも参考にする
- 不明な点は推測せず、調査可能な範囲で記載

---

## 📁 既存資料の読み取り方法

### PDF/SVGファイルの活用
1. **フロー図の確認**
   - 業務フロー全体像の把握
   - 各ステップの関係性理解

2. **画面イメージの確認**
   - 画面レイアウトの理解
   - ボタン配置の確認

3. **業務ルールの抽出**
   - 条件分岐の理由
   - エラーケースの把握

### 読み取り時の注意
- 最新版かどうか確認
- ソースコードとの差異に注意
- 不明瞭な部分は質問形式で記載

---

## 🎯 成果物品質基準

### フローチャートの品質基準
1. **新人が業務フローを理解できる**
2. **画面操作が具体的に分かる**
3. **エラー時の対処が明確**
4. **ソースコードと一致している**
5. **draw.ioで編集可能**

### チェックリスト
- [ ] 既存PDF/SVGを参照したか
- [ ] ソースコード解析を実施したか
- [ ] ラベル名は正確か（ja.json確認）
- [ ] 全ての分岐を網羅しているか
- [ ] エラーケースを記載したか
- [ ] 新人でも理解できる説明か
- [ ] draw.ioファイル（.drawio）を作成したか
- [ ] draw.ioで開いて編集可能か確認したか

---

## 📐 draw.ioファイル作成手順

### 1. 基本テンプレートの作成
```xml
<!-- [機能名]フローチャート.drawio -->
<mxfile host="app.diagrams.net" modified="2024-01-01T00:00:00.000Z" agent="Claude" version="24.0.0">
  <diagram name="[機能名]フロー" id="unique-id">
    <mxGraphModel dx="1422" dy="762" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="1169" pageHeight="827" math="0" shadow="0">
      <root>
        <mxCell id="0" />
        <mxCell id="1" parent="0" />
        <!-- ここにフローチャート要素を追加 -->
      </root>
    </mxGraphModel>
  </diagram>
</mxfile>
```

### 2. よく使うパターン

#### ログイン処理の例
```xml
<!-- 開始 -->
<mxCell id="start" value="開始" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#dae8fc;strokeColor=#6c8ebf;" vertex="1" parent="1">
  <mxGeometry x="380" y="40" width="120" height="60" as="geometry" />
</mxCell>

<!-- ログイン画面 -->
<mxCell id="login" value="画面: ログイン画面&lt;br&gt;操作: ID/パスワード入力&lt;br&gt;「ログイン」ボタンクリック" style="rounded=0;whiteSpace=wrap;html=1;fillColor=#e1d5e7;strokeColor=#9673a6;" vertex="1" parent="1">
  <mxGeometry x="330" y="140" width="220" height="80" as="geometry" />
</mxCell>

<!-- 認証判定 -->
<mxCell id="auth" value="認証成功？" style="rhombus;whiteSpace=wrap;html=1;fillColor=#fff2cc;strokeColor=#d6b656;" vertex="1" parent="1">
  <mxGeometry x="370" y="260" width="140" height="100" as="geometry" />
</mxCell>

<!-- エラー表示 -->
<mxCell id="error" value="エラー表示&lt;br&gt;「認証に失敗しました」" style="rounded=0;whiteSpace=wrap;html=1;fillColor=#f8cecc;strokeColor=#b85450;" vertex="1" parent="1">
  <mxGeometry x="140" y="270" width="180" height="80" as="geometry" />
</mxCell>
```

### 3. 接続線の追加
```xml
<!-- 開始からログイン画面へ -->
<mxCell id="e1" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;" edge="1" parent="1" source="start" target="login">
  <mxGeometry relative="1" as="geometry" />
</mxCell>

<!-- ログインから認証判定へ -->
<mxCell id="e2" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;" edge="1" parent="1" source="login" target="auth">
  <mxGeometry relative="1" as="geometry" />
</mxCell>

<!-- 認証失敗時 -->
<mxCell id="e3" value="No" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;" edge="1" parent="1" source="auth" target="error">
  <mxGeometry relative="1" as="geometry" />
</mxCell>
```

---

## 📝 ドキュメント作成ルール

### 作成場所
- **必ずルートディレクトリに作成**
- マークダウン形式（.md）で保存
- draw.io形式（.drawio）も併せて作成

### draw.io形式での出力

#### 基本的なdraw.ioファイル構造
```xml
<mxfile host="app.diagrams.net" modified="2024-01-01T00:00:00.000Z" agent="5.0" etag="xxx" version="24.0.0" type="device">
  <diagram name="ページ1" id="xxx">
    <mxGraphModel dx="1422" dy="762" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="827" pageHeight="1169" math="0" shadow="0">
      <root>
        <mxCell id="0" />
        <mxCell id="1" parent="0" />
        <!-- フローチャートの要素をここに配置 -->
      </root>
    </mxGraphModel>
  </diagram>
</mxfile>
```

#### フローチャート要素のテンプレート

**開始/終了（角丸）**
```xml
<mxCell id="node1" value="開始" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#dae8fc;strokeColor=#6c8ebf;" vertex="1" parent="1">
  <mxGeometry x="380" y="40" width="120" height="60" as="geometry" />
</mxCell>
```

**処理（四角形）**
```xml
<mxCell id="node2" value="画面: ○○画面&lt;br&gt;操作: 「○○」ボタン" style="rounded=0;whiteSpace=wrap;html=1;fillColor=#e1d5e7;strokeColor=#9673a6;" vertex="1" parent="1">
  <mxGeometry x="350" y="140" width="180" height="80" as="geometry" />
</mxCell>
```

**判定（ひし形）**
```xml
<mxCell id="node3" value="条件判定&lt;br&gt;○○の場合？" style="rhombus;whiteSpace=wrap;html=1;fillColor=#fff2cc;strokeColor=#d6b656;" vertex="1" parent="1">
  <mxGeometry x="370" y="260" width="140" height="100" as="geometry" />
</mxCell>
```

**矢印（エッジ）**
```xml
<mxCell id="edge1" value="Yes" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;" edge="1" parent="1" source="node3" target="node4">
  <mxGeometry relative="1" as="geometry" />
</mxCell>
```

### ファイル出力形式
1. **マークダウンファイル（.md）**
   - 説明文書として作成
   - Mermaid形式のプレビュー用フローチャート含む

2. **draw.ioファイル（.drawio）**
   - draw.ioで直接編集可能
   - XML形式で保存
   - 日本語フォント対応

### draw.ioファイル作成のガイドライン
- **色分け規則**
  - 開始/終了: 青系（#dae8fc）
  - 処理: 紫系（#e1d5e7）
  - 判定: 黄系（#fff2cc）
  - エラー処理: 赤系（#f8cecc）

- **配置規則**
  - グリッドサイズ: 10
  - 要素間隔: 40px以上
  - 中央揃えを基本とする

- **テキスト規則**
  - フォント: デフォルト（日本語対応）
  - HTMLタグ使用可（&lt;br&gt;で改行）
  - 重要な情報は太字（&lt;b&gt;タグ）

---

*このルールは開発効率と品質向上を目的として継続的に更新されます*
*フローチャート作成時は、必ず最新のソースコードと既存資料を確認してください*

---

## 🔄 実際の作成フロー

### STEP 1: 資料確認
1. ルートディレクトリのPDF/SVGを確認
2. 業務フローの全体像を把握

### STEP 2: ソース解析
1. 関連する画面コンポーネントを特定
2. ja.jsonから実際のラベル名を取得
3. 画面遷移とAPI呼び出しを確認

### STEP 3: ドキュメント作成
1. マークダウンファイル（.md）作成
   - 概要説明
   - Mermaidでのプレビュー用フローチャート
   - 詳細な操作手順

2. draw.ioファイル（.drawio）作成
   - XML形式で編集可能なフローチャート
   - 色分けと配置ルールに従う
   - 日本語表示に対応

### STEP 4: 品質確認
1. draw.ioで開いて編集可能か確認
2. 新人視点でのレビュー
3. ソースコードとの整合性チェック