/* ============================================================
   一覧画面表示時：派遣者 / 日付 を取得
============================================================ */

// DOMが読み込まれたタイミングで実行（
document.addEventListener("DOMContentLoaded", function () {
  apiGet(); // 派遣者テーブル + 日付テーブル を API取得
});

/* ============================================================
   グローバル（状態）
   ------------------------------------------------------------
     classAList：縦軸に出す「表示名」の配列
     StateMap  ：{ 派遣者ID: 表示名 } のマップ（A列に表示）
     DatesOK   ：単日データ（〇を付ける用）
     PeriodsNG ：期間データ（使用不可で上書き用）
============================================================ */
let classAList = [];     // 縦軸チェック用：表示名配列（空なら未取得の可能性が高い）
let StateMap = {};       // { 派遣者ID: 表示名 }（Excelの縦軸作成に使用）
let DatesOK = [];        // [{ stateId, date }]  ← 単日（〇）
let PeriodsNG = [];      // [{ stateId, start, end }] ← 使用不可（期間）

/* ============================================================
   apiGet：2つのサイトからデータを取得し、必要形に加工して保持
============================================================ */
function apiGet() {
  /* ============================================================
     派遣者テーブル：ID→名前 + 使用不可期間（dateB/dateC）
  ============================================================ */
  $p.apiGet({
    id: DISPATCHER_SITE_ID, // 派遣者テーブルのサイトID
    data: {},               // 条件なし（全件取得想定）
    done: function (data) { // API取得完了（非同期）
      const rows = fnPickRows(data); // Pleasanter返却形式を吸収して「行配列」を取得
      const map = {};               // { 派遣者ID: 表示名 } を作る
      const unavailable = [];       // 使用不可期間（[{ stateId, start, end }]）を作る

      // 派遣者テーブルの全行を走査して map / unavailable を構築する
      for (let i = 0; i < rows.length; i++) {
        const r = rows[i]; // i行目のレコード
        const rid = r?.ResultId ?? r?.Id ?? r?.id;// 派遣者テーブルのレコードID
        const name = r?.ClassHash?.ClassA;// 表示名（縦軸）＝ ClassA

        // ------- ID→名前マップ -------
        // rid と name が揃うものだけ採用（片方欠けると縦軸が作れない）
        if (rid && name) {
          map[String(rid).trim()] = String(name).trim(); // 空白混入を避ける
        }

        // ------- 使用不可期間（dateB/dateC） -------
        // DateB/DateC の表記ゆれ（大文字/小文字）両対応
        const rawB = r?.DateHash?.DateB ?? r?.DateHash?.dateB; // 開始
        const rawC = r?.DateHash?.DateC ?? r?.DateHash?.dateC; // 終了
        // 文字列化して trim（null/undefinedでも落ちない）
        const bStr = String(rawB ?? "").trim();
        const cStr = String(rawC ?? "").trim();
        // rid または日付が揃っていない場合は「期間」として扱えないので除外
        if (!rid || !bStr || !cStr) continue;
        // Pleasanterの未入力日付（1899-12-30...）は除外
        if (bStr.startsWith("1899-12-30")) continue;
        if (cStr.startsWith("1899-12-30")) continue;

        // 使用不可期間として登録（Excelでは後で「使用不可」で上書きする）
        unavailable.push({
          stateId: String(rid).trim(), // 派遣者ID（縦軸行割当キー）
          start: bStr,                 // 使用不可開始
          end: cStr,                   // 使用不可終了
        });
      }

      // ------- 取得結果をグローバルへ反映 -------
      StateMap = map;        // 派遣者ID→表示名
      PeriodsNG = unavailable; // 使用不可期間

      // 取得済みチェック用：表示名だけの配列を作る（空なら未取得の可能性）
      classAList = Object.keys(map).map(function (k) {
        return map[k];
      });
    },
  });

  /* ============================================================
     日付テーブル：単日（dateA） + 派遣者ID（ClassC：単体/複数）
  ============================================================ */
  $p.apiGet({
    id: DATE_SITE_ID, // 日付テーブルのサイトID
    data: {},         // 条件なし（全件取得想定）
    done: function (data) {
      const rows = fnPickRows(data); // 行配列を取得
      const list = [];               // DatesOK の作成用バッファ

      // 日付テーブルの全行を走査して「単日×派遣者ID」の組を作る
      for (let i = 0; i < rows.length; i++) {
        const r = rows[i];
        const rawA = r?.DateHash?.DateA ?? r?.DateHash?.dateA; // 単日：dateA（表記ゆれ両対応）
        const aStr = String(rawA ?? "").trim();
        if (!aStr) continue;// 日付が空なら意味を持たないので除外
        if (aStr.startsWith("1899-12-30")) continue;// 未入力日付（1899-12-30...）は除外
        const ids = fnPickIdList(r?.ClassHash?.ClassC);// 派遣者ID：ClassC（単体/複数/JSON文字列/配列の揺れを吸収して配列化）
        if (!ids.length) continue;// IDが空なら紐付けが無いので除外

        // 1レコードで複数派遣者が紐づく可能性があるため、IDごとに行を作る
        for (let j = 0; j < ids.length; j++) {
          const id = ids[j];
          if (!id || id === "0") continue; // 空や "0" は無効値として除外
          list.push({ stateId: id, date: aStr }); // 「派遣者ID × 日付」の組を登録（Excelで単日に〇を打つ）
        }
      }
      DatesOK = list; // 作成結果をグローバルへ反映
    },
  });
}

/* ============================================================
   一覧画面：ボタン追加
============================================================ */
(function () {
  const el = document.getElementById("MainCommands"); // コマンド領域
  if (!el) return; // 対象画面以外なら何もしない
  if (document.getElementById("fn-download-usage-excel")) return; // 二重追加防止
  const btn = document.createElement("button"); // ボタン生成
  btn.id = "fn-download-usage-excel";           // ボタンID
  btn.className = "button ui-button ui-widget applied"; // Pleasanter既存UIに合わせたclass
  btn.textContent = "利用状況をExcelでダウンロード";    // 表示文言
  btn.addEventListener("click", fnOpenModal);    // クリックでモーダルを開く
  el.appendChild(btn);                           // 画面に追加
})();

/* ============================================================
   モーダル（年月選択 + ダウンロード）
============================================================ */
function fnOpenModal() {
  if (document.getElementById("sdt-usage-excel-modal")) return; // 二重生成防止
  const wrap = document.createElement("div"); // 背景＋中央寄せのラッパ
  wrap.id = "sdt-usage-excel-modal";          // 閉じる時の参照用ID
  wrap.className = "sdt-modal-wrap";          // 背景・中央寄せ用CSSクラス
  // モーダルのHTML構造（UI）
  wrap.innerHTML = `
    <div class="sdt-modal">
      <button type="button" class="sdt-modal__close" aria-label="close">×</button>

      <div class="sdt-modal__header">
        <div class="sdt-modal__title">利用状況Excelダウンロード</div>
      </div>

      <div class="sdt-modal__content">
        <div class="sdt-field">
          <div class="sdt-field__label">
            ダウンロード対象の年月 <span class="sdt-field__required">*</span>
          </div>
          <input id="fn-target-month" class="sdt-field__input" type="month" />
        </div>

        <div class="sdt-actions">
          <button id="fn-modal-download" type="button" class="sdt-btn sdt-btn--primary">
            ダウンロード
          </button>
        </div>
      </div>
    </div>
  `;

  document.body.appendChild(wrap); // DOMに追加して表示
  const now = new Date();// 初期値：当月（type=month は "YYYY-MM" 形式）
  document.getElementById("fn-target-month").value = now.getFullYear() + "-" + String(now.getMonth() + 1).padStart(2, "0"); // 月は0始まりなので+1
  document.getElementById("fn-modal-download").onclick = fnDownloadExcel;// ダウンロード押下でExcel作成開始
  wrap.querySelector(".sdt-modal__close").onclick = fnCloseModal;// ×ボタン押下でモーダルを閉じる
}

/* ============================================================
   ExcelJS 待機
============================================================ */
function fnWaitExcelJS(cb) {
  if (window.ExcelJS) return cb();                 // 使用可能なら即実行
  setTimeout(function () { fnWaitExcelJS(cb); }, 50); // 50ms毎に再チェック
}

/* ============================================================
   ダウンロード開始
============================================================ */
function fnDownloadExcel() {
  // 派遣者マスタが未取得（縦軸が作れない）
  if (!classAList.length) {
    alert("派遣者データがまだ取得できていません");
    return;
  }
  // 日付テーブルが未取得（〇が打てない）
  if (!DatesOK.length) {
    alert("日付データがまだ取得できていません");
    return;
  }
  // 選択年月（YYYY-MM）
  const ym = document.getElementById("fn-target-month").value;
  if (!ym) {
    alert("年月を選択してください");
    return;
  }
  // 年/月に分解（数値化）
  const [year, month] = ym.split("-").map(Number);
  // ExcelJS読み込み完了後にExcel作成
  fnWaitExcelJS(function () {
    fnBuildExcel(year, month); // Excel作成→DL
    fnCloseModal();            // モーダル閉じる
  });
}

/* ============================================================
   日付を「日単位」に正規化
============================================================ */
function fnToDateOnly(dateStr) {
  const s = String(dateStr).split("T")[0]; // 日付部分だけ抽出
  const p = s.split("-").map(Number);      // [YYYY, MM, DD]
  return new Date(p[0], p[1] - 1, p[2]);   // 月は0始まりなので -1
}

/* ============================================================
   Excel 作成
============================================================ */
function fnBuildExcel(year, month) {
  const stateMap = StateMap;  // { 派遣者ID: 表示名 }
  const okDates = DatesOK;    // [{ stateId, date }]
  const unavail = PeriodsNG;  // [{ stateId, start, end }]
  const monthStart = new Date(year, month - 1, 1); // 対象月の開始日（1日）
  const monthEnd = new Date(year, month, 0);// 対象月の終了日（月末）
  const lastDay = monthEnd.getDate();// 月末日（28〜31）
  const wb = new ExcelJS.Workbook();// ExcelJS：ブック生成
  const sh = wb.addWorksheet("利用状況");// ExcelJS：シート生成（この sh に対して getCell を呼ぶ）

  // ===== ヘッダ（1行目）=====
  sh.getCell(1, 1).value = year + "年"; // A1：年

  // B1〜：日付ラベル（1日→B列）
  for (let d = 1; d <= lastDay; d++) {
    sh.getCell(1, d + 1).value = month + "月" + d + "日";
  }

  // ===== 行割当（id→行）=====
  const rowMap = {}; // { 派遣者ID: 行番号 }
  // マスタ順に 2行目から割り当て、A列に表示名を入れる
  Object.keys(stateMap).forEach(function (id, i) {
    const row = i + 2;              // 1行目はヘッダなので +2
    rowMap[id] = row;               // 行番号を保持
    sh.getCell(row, 1).value = stateMap[id]; // A列：表示名
  });

  // ===== 単日：〇 =====
  okDates.forEach(function (u) {
    const row = rowMap[u.stateId]; // 対象派遣者の行
    const dt = fnToDateOnly(u.date); // 日付を Date 化
    const col = 1 + dt.getDate(); // 1日→B列なので 1 + 日
    if (!row) return;              // マスタに無いIDは除外
    if (dt < monthStart || dt > monthEnd) return; // 対象月以外は除外
    sh.getCell(row, col).value = "〇"; // 単日のセルに〇
  });

  // ===== 使用不可：期間上書き =====
  unavail.forEach(function (p) {
    const row = rowMap[p.stateId]; // 対象派遣者の行
    if (!row) return;              // マスタに無いIDは除外

    // 開始/終了を Date 化
    const start = fnToDateOnly(p.start);
    const end = fnToDateOnly(p.end);

    // 対象月に収まるように切り詰める
    const fillStart = start > monthStart ? start : monthStart;
    const fillEnd = end < monthEnd ? end : monthEnd;
    if (fillStart > fillEnd) return; // 対象月にかからない
    const cur = new Date(fillStart); // fillStart〜fillEnd を1日ずつ進めて「使用不可」で上書き
    while (cur <= fillEnd) {
      const col = 1 + cur.getDate(); // 1日→B列
      sh.getCell(row, col).value = "使用不可"; // 〇があっても上書き
      cur.setDate(cur.getDate() + 1); // 次の日へ
    }
  });

  // ===== ダウンロード =====
  const file = `利用状況_${year}_${String(month).padStart(2, "0")}.xlsx`; // 保存名
  // ExcelJS：バッファ生成 → Blob化 → a.click() で保存
  wb.xlsx.writeBuffer().then(function (buf) {
    const blob = new Blob([buf], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });
    const url = URL.createObjectURL(blob); // 一時URL
    const a = document.createElement("a"); // ダウンロード用リンク
    a.href = url;                          // Blob URL
    a.download = file;                     // 保存ファイル名
    document.body.appendChild(a);          // クリックできるようDOMに追加
    a.click();                             // ダウンロード開始
    a.remove();                            // DOMから削除
    // URL解放（メモリリーク対策）
    setTimeout(function () { URL.revokeObjectURL(url); }, 3000);
  });
}

/* ============================================================
   Pleasanter返却の「行配列」を取り出す
============================================================ */
function fnPickRows(data) {
  return (data && data.Response && data.Response.Data) ? data.Response.Data :
         (data && data.Data) ? data.Data :
         (Array.isArray(data) ? data : []);
}

/* ============================================================
   ClassC（単体/配列/JSON配列文字列）→ ID配列に正規化
============================================================ */
function fnPickIdList(v) {
  if (v === undefined || v === null) return []; // null/undefined は空配列
  // 既に配列なら、文字列化→trim→空除外
  if (Array.isArray(v)) {
    return v.map(String).map(function (s) { return s.trim(); }).filter(Boolean);
  }
  const s = String(v).trim(); // 文字列化してtrim
  if (!s) return [];

  // JSON配列文字列なら parse して配列にする
  if (s[0] === "[") {
    try {
      const arr = JSON.parse(s);
      if (Array.isArray(arr)) {
        return arr.map(String).map(function (x) { return x.trim(); }).filter(Boolean);
      }
    } catch (e) {
      alert("予期せぬエラーが発生しました。", e);
      return [];
    }
  }
  // 単体IDなら配列化して返す
  return [s];
}

/* ============================================================
   モーダルを閉じる
============================================================ */
function fnCloseModal() {
  document.getElementById("sdt-usage-excel-modal")?.remove();
}
