# VBA→JavaScript変換手順書
双日ライフワン光熱費管理システム Pleasanter移行

## 📋 概要

1,800行以上のVBAマクロを効率的なJavaScriptに変換するための段階的手順書です。

**対象ファイル**: 2ファイル・25機能  
**変換後予想行数**: 約300-400行のJavaScript  
**削減率**: 約80%

---

## 🎯 変換優先順位

### 【Phase 1: 高優先度】重要なデータ処理マクロ

#### 1-1. `計算用管理変動費Master.xls` → `dataProcessor.js`

| マクロ名 | 行数 | 複雑度 | 変換順序 | 推定JS行数 |
|---------|------|--------|----------|------------|
| **FDに値のコピー** | 150行 | 高 | 1番目 | 30行 |
| **管理変動費master用FD作成** | 194行 | 高 | 2番目 | 40行 |
| **電気合計チェック** | 46行 | 中 | 3番目 | 15行 |
| **Macro_test_1** | 20行 | 低 | 4番目 | 10行 |

#### 1-2. `MO報告光熱費.xls` → `reportGenerator.js`

| マクロ名 | 行数 | 複雑度 | 変換順序 | 推定JS行数 |
|---------|------|--------|----------|------------|
| **準備1** | 200行 | 高 | 5番目 | 35行 |
| **テナント検針データコピー** | 62行 | 高 | 6番目 | 25行 |

---

## 🔧 Phase 1 詳細変換手順

### Step 1: `FDに値のコピー` (最重要)

#### **元VBAコード分析**
```vba
' 問題のあるVBAパターン
Columns("E:E").Select
Selection.Copy
Windows("管理変動費Master.xls").Activate
Range("E1").Select
Selection.PasteSpecial Paste:=xlValues
```

#### **JavaScript変換方針**
1. **ファイル間データコピー** → Pleasanter API + SheetJS
2. **手動ウィンドウ操作** → 自動化されたデータ転送
3. **大量の重複コード** → ループ処理で簡素化

#### **変換後JavaScript構造**
```javascript
// dataProcessor.js
class DataProcessor {
  async copyColumnData(sourceTable, targetTable, columnMap) {
    // 1. ソースデータ取得
    const sourceData = await this.getPleasanterData(sourceTable);
    
    // 2. データ変換・検証
    const transformedData = this.transformData(sourceData, columnMap);
    
    // 3. ターゲットに書き込み
    await this.updatePleasanterData(targetTable, transformedData);
  }
  
  async processAllSheets() {
    const sheetMappings = [
      { source: '電気使用料_一般', target: '電気使用料_一般_Master', column: 'E' },
      { source: '電気使用料_動力店舗', target: '電気使用料_動力店舗_Master', column: 'E' },
      // ... 他のシート定義
    ];
    
    for (const mapping of sheetMappings) {
      await this.copyColumnData(mapping.source, mapping.target, mapping.column);
    }
  }
}
```

### Step 2: `管理変動費master用FD作成` 

#### **VBA問題点**
- 194行の重複コード
- フロッピーディスク前提
- エラーハンドリング皆無

#### **JavaScript変換**
```javascript
class MasterFileGenerator {
  async generateMasterFile() {
    try {
      // 1. 複数シートのデータを並行取得
      const [electricGeneral, electricPower, airConGas, water] = await Promise.all([
        this.getSheetData('電気使用料_一般'),
        this.getSheetData('電気使用料_動力'),
        this.getSheetData('空調用ガス'),
        this.getSheetData('水道使用料')
      ]);
      
      // 2. マスターファイル生成
      const masterData = this.consolidateData({
        electricGeneral, electricPower, airConGas, water
      });
      
      // 3. Pleasanterに保存 or Excelダウンロード
      await this.saveMasterFile(masterData);
      
      return { success: true, recordCount: masterData.length };
    } catch (error) {
      console.error('マスターファイル生成エラー:', error);
      throw error;
    }
  }
}
```

### Step 3: `テナント検針データコピー`

#### **VBA問題点**
```vba
MsgBox "Ａドライブに｢テナント検針データ｣ＦＤをセットしてください"
ChDir "A:\"
Workbooks.Open Filename:="A:\テナント検針.csv"
```

#### **JavaScript変換**
```javascript
class TenantDataImporter {
  async importTenantData(file) {
    // 1. ファイルアップロード処理
    const csvData = await this.parseCSVFile(file);
    
    // 2. データ検証
    const validatedData = this.validateTenantData(csvData);
    
    // 3. Pleasanterに一括インポート
    const result = await this.batchImportToPleasanter(validatedData);
    
    return {
      imported: result.successCount,
      errors: result.errors,
      total: csvData.length
    };
  }
  
  parseCSVFile(file) {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = (e) => {
        const csv = e.target.result;
        const data = Papa.parse(csv, {
          header: true,
          skipEmptyLines: true,
          encoding: 'Shift_JIS' // 元ファイルの文字コード対応
        });
        resolve(data.data);
      };
      reader.onerror = reject;
      reader.readAsText(file, 'Shift_JIS');
    });
  }
}
```

---

## 🗂️ Pleasanter連携方法

### Pleasanterテーブル設計

#### 1. 光熱費データテーブル (`utility_data`)
```javascript
const utilityDataSchema = {
  fields: {
    group_name: 'テキスト',        // グループ名
    usage_type: '選択肢',          // 電気/ガス/水道
    usage_amount: '数値',          // 使用量
    unit_price: '数値',            // 単価
    total_cost: '数値',            // 合計料金
    measurement_date: '日付',       // 検針日
    period_start: '日付',           // 期間開始
    period_end: '日付'             // 期間終了
  }
};
```

#### 2. Pleasanter API連携
```javascript
class PleasanterAPI {
  constructor(serverUrl, apiKey) {
    this.baseUrl = serverUrl;
    this.apiKey = apiKey;
  }
  
  async createRecord(siteId, data) {
    const response = await fetch(`${this.baseUrl}/api/items/${siteId}/create`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-API-KEY': this.apiKey
      },
      body: JSON.stringify(data)
    });
    return response.json();
  }
  
  async getRecords(siteId, filter = {}) {
    const response = await fetch(`${this.baseUrl}/api/items/${siteId}/get`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-API-KEY': this.apiKey
      },
      body: JSON.stringify({ View: filter })
    });
    return response.json();
  }
}
```

---

## 📁 ファイル構成

### JavaScript分割方針

```
/pleasanter_custom/
├── modules/
│   ├── dataProcessor.js      # データ処理（FDに値のコピー等）
│   ├── reportGenerator.js    # レポート生成（準備1等）
│   ├── fileImporter.js       # ファイルインポート
│   ├── masterFileManager.js  # マスターファイル管理
│   └── utilityCalculator.js  # 光熱費計算
├── utils/
│   ├── pleasanterAPI.js      # Pleasanter API操作
│   ├── csvParser.js          # CSV解析
│   ├── dataValidator.js      # データ検証
│   └── errorHandler.js       # エラーハンドリング
└── main.js                   # メイン処理
```

---

## 🚀 実装順序

### Week 1-2: 基盤構築
1. **Pleasanter環境設定**
   - テーブル作成
   - API設定
   - 権限設定

2. **共通ユーティリティ開発**
   - `pleasanterAPI.js`
   - `csvParser.js`
   - `dataValidator.js`

### Week 3-4: データ処理マクロ変換
1. **`FDに値のコピー` → `dataProcessor.js`**
   - 最も重要な処理から開始
   - テスト駆動開発

2. **`管理変動費master用FD作成` → `masterFileManager.js`**
   - データ統合ロジック
   - エラーハンドリング強化

### Week 5-6: レポート系マクロ変換
1. **`準備1` → `reportGenerator.js`**
   - レポート生成ロジック
   - UI連携

2. **`テナント検針データコピー` → `fileImporter.js`**
   - ファイルアップロード
   - データ検証

### Week 7: テスト・最適化
1. **結合テスト**
2. **パフォーマンス調整**
3. **ユーザー受入テスト**

---

## 🔧 開発ツール・環境

### 必要なライブラリ
```json
{
  "dependencies": {
    "papaparse": "^5.3.0",          // CSV解析
    "xlsx": "^0.18.0",              // Excel操作
    "lodash": "^4.17.0",            // ユーティリティ
    "moment": "^2.29.0"             // 日付操作
  }
}
```

### Pleasanterカスタムスクリプト
```javascript
// pleasanter_main.js - Pleasanterに配置
$p.events.on_editor_load = function () {
  // カスタムボタン追加
  $p.setCustomButton('データ処理', async function() {
    const processor = new DataProcessor();
    await processor.processAllSheets();
  });
};
```

---

## ✅ 変換完了チェックリスト

### 機能検証
- [ ] データコピー機能正常動作
- [ ] ファイルインポート機能正常動作
- [ ] レポート生成機能正常動作
- [ ] エラーハンドリング正常動作

### パフォーマンス検証
- [ ] 大量データ処理（500件以上）
- [ ] ファイルアップロード（10MB以上）
- [ ] 並行処理動作確認

### ユーザビリティ検証
- [ ] 直感的な操作可能
- [ ] エラーメッセージわかりやすい
- [ ] 処理状況の可視化

---

この手順で**1,800行のVBA → 300-400行のJavaScript**への効率的な変換が可能です。最重要の`FDに値のコピー`から開始し、段階的に移行することでリスクを最小化できます。