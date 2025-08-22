# 経費取込処理 SQL比較（新旧システム）

## 1. TD経費テーブルへのINSERT処理比較

### 旧システム（VB6）
```vb
sql = "select * from TD経費 where 経費番号 = " & NUMBR
Set rs = OpenRs(sql, Cn, adOpenKeyset, adLockPessimistic)

With rs
    .AddNew
    ![経費番号] = NUMBR
    ![経費日付] = gRsA![経費日付]
    ![初期登録日] = Date
    ![登録変更日] = Date
    .Update
End With
```

### 新システム（Python）
```python
query = "INSERT INTO TD経費 \
    (経費番号, 経費日付, 初期登録日, 登録変更日) \
    VALUES ({}, '{}', '{}', '{}')".format(
        Number,
        insert_data['経費日付'],
        datetime.date.today(),
        datetime.date.today()
    )
```

### TD経費の比較結果
| フィールド | 旧システム | 新システム | 差異 |
|-----------|-----------|-----------|------|
| 経費番号 | NUMBR | Number | ✓ 同じ |
| 経費日付 | gRsA![経費日付] | insert_data['経費日付'] | ✓ 同じ |
| 初期登録日 | Date | datetime.date.today() | ✓ 同じ |
| 登録変更日 | Date | datetime.date.today() | ✓ 同じ |

**結論**: TD経費テーブルへの挿入は同一。値のずれは発生しない。

---

## 2. TD経費明細テーブルへのINSERT処理比較

### 旧システム（VB6）
```vb
sql = "select * from TD経費明細 where 0 <> 0 "
Set rs = OpenRs(sql, Cn, adOpenKeyset, adLockPessimistic)

With rs
    rs.AddNew
    ![経費番号] = NUMBR
    ![枝番] = Mcnt
    ![科目CD] = gRsA![科目CD]
    ![科目名] = gRsA![科目名]
    ![金額] = gRsA![金額]
    ![担当者CD] = gRsA![担当者CD]
    ![担当者名] = gRsA![担当者名]
    ![購入先名] = ""
    ![科目摘要cd] = Null
    ![科目摘要名] = gRsA![科目摘要名]
    ![初期登録日] = Date
    ![登録変更日] = Date
    ![消費税額] = gRsA![消費税額]
    ![補助CD] = gRsA![補助CD]
    .Update
End With
```

### 新システム（Python）
```python
query = "INSERT INTO TD経費明細 \
    (経費番号, 枝番, 科目CD, 科目名, 金額, 担当者CD, 担当者名, 購入先名, 科目摘要名, 初期登録日, 登録変更日, 消費税額, 補助CD) \
    VALUES ({}, {}, {}, '{}', {}, null, '{}', '{}', '{}', '{}', '{}', {}, {})".format(
        Number,
        Mcnt,
        insert_data['科目CD'],
        insert_data['科目名'],
        insert_data['金額'],
        insert_data['担当者名'],
        "",
        insert_data['科目摘要名'],
        datetime.date.today(),
        datetime.date.today(),
        insert_data['消費税額'],
        insert_data['補助CD']
    )
```

### TD経費明細の比較結果
| フィールド | 旧システム | 新システム | 差異 |
|-----------|-----------|-----------|------|
| 経費番号 | NUMBR | Number | ✓ 同じ |
| 枝番 | Mcnt | Mcnt | ✓ 同じ |
| 科目CD | gRsA![科目CD] | insert_data['科目CD'] | ✓ 同じ |
| 科目名 | gRsA![科目名] | insert_data['科目名'] | ✓ 同じ |
| 金額 | gRsA![金額] | insert_data['金額'] | ✓ 同じ |
| 担当者CD | gRsA![担当者CD] | null（固定） | ⚠️ **差異あり** |
| 担当者名 | gRsA![担当者名] | insert_data['担当者名'] | ✓ 同じ |
| 購入先名 | ""（空文字） | ""（空文字） | ✓ 同じ |
| 科目摘要cd | Null | （省略） | ⚠️ **差異あり** |
| 科目摘要名 | gRsA![科目摘要名] | insert_data['科目摘要名'] | ✓ 同じ |
| 初期登録日 | Date | datetime.date.today() | ✓ 同じ |
| 登録変更日 | Date | datetime.date.today() | ✓ 同じ |
| 消費税額 | gRsA![消費税額] | insert_data['消費税額'] | ✓ 同じ |
| 補助CD | gRsA![補助CD] | insert_data['補助CD'] | ✓ 同じ |

---

## 3. CSVデータマッピング比較（配列インデックス）

### データ取得時の配列位置
| データ項目 | 旧システム（VB6） | 新システム（Python） | 配列位置の差 |
|-----------|------------------|-------------------|-------------|
| 年 | cTable(i)(1) | csvData[0] | -1 |
| 月 | cTable(i)(2) | csvData[1] | -1 |
| 日 | cTable(i)(3) | csvData[2] | -1 |
| 借方金額 | cTable(i)(5) | csvData[4] | -1 |
| 借方科目CD | cTable(i)(6) | csvData[5] | -1 |
| 借方補助CD | cTable(i)(11) | csvData[10] | -1 |
| 貸方科目CD | cTable(i)(15) | csvData[14] | -1 |
| 貸方補助CD | cTable(i)(20) | csvData[19] | -1 |
| 貸方金額 | cTable(i)(24) | csvData[23] | -1 |
| 科目摘要名 | cTable(i)(25) | csvData[24] | -1 |

**重要**: VB6は1ベース、Pythonは0ベースのため、すべて-1の差がある

---

## 4. 重要な差異と対応が必要な箇所

### ⚠️ 修正が必要な箇所

1. **担当者CD**
   - 旧: `gRsA![担当者CD]`（変換後の値を使用）
   - 新: `null`（固定値）
   - **影響**: 担当者CDが保存されない

2. **科目摘要cd**
   - 旧: `Null`として明示的に設定
   - 新: INSERT文に含まれていない
   - **影響**: デフォルト値が設定される可能性

### 修正案

```python
# 新システムのINSERT文を以下のように修正
query = "INSERT INTO TD経費明細 \
    (経費番号, 枝番, 科目CD, 科目名, 金額, 担当者CD, 担当者名, 購入先名, 科目摘要cd, 科目摘要名, 初期登録日, 登録変更日, 消費税額, 補助CD) \
    VALUES ({}, {}, {}, '{}', {}, {}, '{}', '{}', null, '{}', '{}', '{}', {}, {})".format(
        Number,
        Mcnt,
        insert_data['科目CD'],
        insert_data['科目名'],
        insert_data['金額'],
        f"'{insert_data['担当者CD']}'" if insert_data.get('担当者CD') else "null",  # 修正
        insert_data['担当者名'],
        "",
        insert_data['科目摘要名'],
        datetime.date.today(),
        datetime.date.today(),
        insert_data['消費税額'],
        insert_data['補助CD']
    )
```

---

## 5. データ変換処理の比較

### 金額処理（貸方）
- 旧: `CCur(NullToZero(cTable(i)(24))) * -1`
- 新: `NullToZero(csvData[23]) * -1`
- **結果**: 同じ（マイナス値で保存）

### 科目摘要名処理
- 旧: `AnsiLeftB(SpcToNull(cTable(i)(25), ""), 30)`
- 新: `AnsiLeftB(SpcToNull(csvData[24], ""), 30).replace('"','')`
- **差異**: 新システムは二重引用符を削除

### 消費税額
- 旧・新とも: 0固定（2015年の仕様変更で税計算削除）

---

## 結論

1. **配列インデックス**: VB6（1ベース）とPython（0ベース）の差により、すべて-1のオフセットがあるが、一貫しているため問題なし

2. **修正必須項目**:
   - 担当者CDがnull固定になっている → 変換後の値を使用するよう修正
   - 科目摘要cdフィールドが省略されている → 明示的にnullを設定

3. **動作に影響しない差異**:
   - 二重引用符の削除処理（新システムのみ）
   - SQL実行方法の違い（ADO vs 直接SQL）

上記2点を修正すれば、新旧システムで同一のデータが登録される。