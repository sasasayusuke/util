/* ============================================================
   一覧画面表示時：State / Kitchen を取得
============================================================ */
// DOMが読み込まれたタイミングで実行（Pleasanter一覧画面表示直後の想定）
document.addEventListener("DOMContentLoaded", function () {
  apiGet(); // kitchen と date の2サイトを API 取得する
});

let classAList = []; // Excel縦軸（A列）に並べる「キッチン名」一覧（取得できているかのチェックにも使う）
let StateMap = {}; // State：{ "253262": "Aさん", ... } 形式の「レコードID → 表示名」マップ
let PeriodsOK = []; // 利用期間（〇を付ける期間）
let PeriodsNG = []; // 使用不可期間（使用不可で上書きする期間）

/* ============================================================
   apiGet：2つのサイトからデータを取得し、必要形に加工して保持
============================================================ */
function apiGet() {
  /* ============================================================
     kitchen_Site_Id：ID → 名前 マッピング + 使用不可期間（DateC/DateD）
  ============================================================ */
  $p.apiGet({
    id: kitchen_Site_Id, // PleasanterのサイトID（kitchen側）
    data: {},
    done: function (data) {
      // Pleasanter返却形式の差を吸収して rows（行配列）を取り出す
      // - data.Response.Data があるパターン
      // - data.Data があるパターン
      // - data 自体が配列のパターン
      const rows =
        (data && data.Response && data.Response.Data) ? data.Response.Data :
        (data && data.Data) ? data.Data :
        (Array.isArray(data) ? data : []);
      const map = {};          // kitchenのレコードID → 表示名 のマップ
      const unavailable = [];  // 使用不可期間の配列
      // rows を1行ずつ処理してマップ・期間配列を作成する
      for (let i = 0; i < rows.length; i++) {
        const r = rows[i]; // i行目のレコード
        const rid = r?.ResultId ?? r?.Id ?? r?.id; // kitchenのレコードID（ResultId / Id / id の表記ゆれ吸収）
        const name = r?.ClassHash?.ClassA; // キッチン名を ClassA から取得

        // rid と name がそろっている場合のみ、ID→名前マップに登録
        if (rid && name) {
          map[String(rid).trim()] = String(name).trim(); // 空白混入を避けるため trim
        }

        // ======== 使用不可期間（DateC/DateD）========
        // DateC/DateD の表記ゆれ（DateC/dateC）両対応で取り出す
        const rawC = r?.DateHash?.DateC ?? r?.DateHash?.dateC; // 開始
        const rawD = r?.DateHash?.DateD ?? r?.DateHash?.dateD; // 終了
        // 文字列に変換して trim（null/undefinedでも落ちない）
        const cStr = String(rawC ?? "").trim();
        const dStr = String(rawD ?? "").trim();

        // rid または日付が空なら「使用不可期間なし」と判断してスキップ
        if (!rid || !cStr || !dStr) continue;
        // Pleasanterの未入力日付（1899-12-30…）は期間として扱わない
        if (cStr.startsWith("1899-12-30")) continue;
        if (dStr.startsWith("1899-12-30")) continue;

        // 使用不可期間として採用（Excelではこの期間を「使用不可」で上書きする）
        unavailable.push({
          stateId: String(rid).trim(), // kitchenレコードID（行割当のキー）
          start: cStr,                 // 使用不可開始
          end: dStr,                   // 使用不可終了
        });
      }
      StateMap = map; // 作成した ID→名前マップを保存

      // Excel縦軸用（表示名一覧）を作成
      classAList = Object.keys(map).map(function (k) {
        return map[k]; // id から name のみ取り出す
      });
      // 使用不可期間を window に保存（Excel作成時に参照するため）
      window.PeriodsNG = unavailable;
    },
  });

  /* ============================================================
    date_Site_Id：利用期間（DateA/DateB）
  ============================================================ */
  $p.apiGet({
    id: date_Site_Id, // PleasanterのサイトID（date側）
    data: {},         // 検索条件なし（全件取得の想定）
    done: function (data) { // 取得完了時コールバック（非同期）

      // Pleasanter返却形式の差を吸収して rows（行配列）を取り出す
      const rows =
        (data && data.Response && data.Response.Data) ? data.Response.Data :
        (data && data.Data) ? data.Data :
        (Array.isArray(data) ? data : []);

      const periods = []; // 利用期間の格納先（〇を付ける期間）

      // rows を1行ずつ処理して periods を作成する
      for (let i = 0; i < rows.length; i++) {
        const r = rows[i]; // i行目のレコード

        // date側のClassAは kitchenのレコードID（配列/JSON/文字列があり得る）
        // → 先頭1件を取り出す（行割当キーとして使うため）
        const stateId = fnPickFirstId(r?.ClassHash?.ClassA);

        // DateA/DateB（表記ゆれ両対応）で開始/終了を取得
        const startRaw = r?.DateHash?.DateA ?? r?.DateHash?.dateA; // 利用開始
        const endRaw = r?.DateHash?.DateB ?? r?.DateHash?.dateB;   // 利用終了

        // stateId が無い/0 の場合は不正データとして除外
        if (!stateId || stateId === "0") continue;

        // 日付は文字列として扱う（T以降の時刻は後で捨てる）
        const startStr = String(startRaw ?? "").trim();
        const endStr = String(endRaw ?? "").trim();

        // 日付が揃っていないなら期間として扱えないので除外
        if (!startStr || !endStr) continue;

        // 未入力日付（1899-12-30…）は除外（Pleasanterの仕様対策）
        if (startStr.startsWith("1899-12-30")) continue;
        if (endStr.startsWith("1899-12-30")) continue;

        // 利用期間として採用（Excelでこの範囲に「〇」を入れる）
        periods.push({
          stateId: stateId,  // kitchenレコードID（行割当キー）
          start: startStr,   // 利用開始
          end: endStr,       // 利用終了
        });
      }

      // 利用期間を window に保存（Excel作成時に参照するため）
      window.PeriodsOK = periods;

      // デバッグログ：利用期間が作れているか確認
      console.log("[Kitchen] 利用期間配列 作成OK", {
        count: periods.length,
        sample: periods.slice(0, 5),
      });
    },
  });
}

/* ============================================================
    一覧画面：ボタン追加
============================================================ */
(function () {
  const el = document.getElementById("MainCommands"); // Pleasanterのコマンド領域DOM
  if (!el) return; // 見つからない画面では何もしない
  if (document.getElementById("fn-download-usage-excel")) return; // 既にあれば二重追加しない
  const btn = document.createElement("button"); // ボタンDOMを生成
  btn.id = "fn-download-usage-excel";           // ボタンID（重複判定にも使用）
  btn.className = "button ui-button ui-widget applied"; // Pleasanter既存の見た目に寄せる
  btn.textContent = "利用状況をExcelでダウンロード";    // 表示文言
  btn.addEventListener("click", fnOpenModal);    // クリックでモーダルを開く
  el.appendChild(btn);                           // コマンド領域に追加
})();

/* ============================================================
    モーダル（年月選択 + ダウンロード）
============================================================ */
function fnOpenModal() {
  if (document.getElementById("sdt-usage-excel-modal")) return; // 既に開いていれば何もしない
  const wrap = document.createElement("div"); // ラッパ要素を生成
  wrap.id = "sdt-usage-excel-modal";          // ラッパID（閉じる時にremoveする）
  wrap.className = "sdt-modal-wrap";
  // モーダルUI（シンプル構造）
  wrap.innerHTML = `
  <div class="sdt-modal">
    <!-- 閉じるボタン -->
    <button
      type="button"
      class="sdt-modal__close"
      aria-label="close"
    >×</button>

    <!-- ヘッダー -->
    <div class="sdt-modal__header">
      <div class="sdt-modal__title">
        利用状況Excelダウンロード
      </div>
    </div>

    <!-- コンテンツ -->
    <div class="sdt-modal__content">

      <!-- 入力フィールド -->
      <div class="sdt-field">
        <div class="sdt-field__label">
          ダウンロード対象の年月
          <span class="sdt-field__required">*</span>
        </div>

        <input
          id="fn-target-month"
          class="sdt-field__input"
          type="month"
        />
      </div>

      <!-- アクション -->
      <div class="sdt-actions">
        <button
          id="fn-modal-download"
          type="button"
          class="sdt-btn sdt-btn--primary"
        >
          ダウンロード
        </button>
      </div>

    </div>
  </div>
`;

  document.body.appendChild(wrap); // 画面に追加して表示
  const now = new Date(); // 現在日時
  // input[type=month] は YYYY-MM 形式なので組み立てる
  document.getElementById("fn-target-month").value = now.getFullYear() + "-" + String(now.getMonth() + 1).padStart(2, "0"); // 月は0始まりなので+1
  document.getElementById("fn-modal-download").onclick = fnDownloadExcel; // ダウンロードボタン押下でExcel作成開始
  wrap.querySelector(".sdt-modal__close").onclick = fnCloseModal; // ×ボタン押下でモーダルを閉じる
}

/* ============================================================
    ExcelJS 待機
============================================================ */
function fnWaitExcelJS(cb) {
  if (window.ExcelJS) return cb(); // 既に使えるなら即実行
  setTimeout(function () { fnWaitExcelJS(cb); }, 50); // 50msごとに再チェック
}

/* ============================================================
    ダウンロード開始
    ------------------------------------------------------------
      1) 取得済みチェック
      2) 年月入力チェック
      3) ExcelJS待機
      4) Excel作成 → DL
      5) モーダルを閉じる
============================================================ */
function fnDownloadExcel() {
  // kitchen 側のマップが取れていない（縦軸が作れない）場合は中断
  if (!classAList.length) {
    alert("Stateデータがまだ取得できていません");
    return;
  }
  // date 側の利用期間が取れていない（〇が付けられない）場合は中断
  if (!window.PeriodsOK.length) {
    alert("利用期間データがまだ取得できていません");
    return;
  }
  // 使用不可は 0件でもOK（取得待ちにしない）＝この設計は妥当
  const ym = document.getElementById("fn-target-month").value; // YYYY-MM を取得
  if (!ym) {
    alert("年月を選択してください");
    return;
  }

  // "YYYY-MM" を year, month に分解して数値化
  const [year, month] = ym.split("-").map(Number);
  // ExcelJS が読み込まれてから Excel を作成する
  fnWaitExcelJS(function () {
    fnBuildExcel(year, month); // Excel作成・DL
    fnCloseModal();            // 完了後モーダルを閉じる
  });
}

/* ============================================================
    日付を「日単位」に正規化
    "2026-01-10T00:00:00" のような文字列から
    "2026-01-10" を取り出して Date オブジェクト化する
============================================================ */
function fnToDateOnly(dateStr) {
  const s = String(dateStr).split("T")[0]; // "YYYY-MM-DD" のみ抽出
  const p = s.split("-").map(Number);      // [YYYY, MM, DD] に分解して数値化
  return new Date(p[0], p[1] - 1, p[2]);   // JSの月は0始まりなので -1
}

/* ============================================================
    Excel 作成 & 〇埋め + 使用不可
============================================================ */
function fnBuildExcel(year, month) {
  const stateMap = StateMap; // kitchen：レコードID → 表示名
  const periods = window.PeriodsOK; // 利用期間（〇を付ける）
  const unavail = window.PeriodsNG; // 使用不可期間（使用不可で上書き）
  const monthStart = new Date(year, month - 1, 1); // 対象月の開始日（1日）
  const monthEnd = new Date(year, month, 0); // 対象月の終了日（月末）：new Date(year, month, 0) は「前月末」= 対象月末
  const lastDay = monthEnd.getDate(); // 月末日（28〜31）
  const wb = new ExcelJS.Workbook(); // ExcelJS：ワークブック作成
  const sh = wb.addWorksheet("利用状況");
  // ===== ヘッダー行（1行目） =====
  sh.getCell(1, 1).value = year + "年"; // A1に年

  // B1〜に "月D日" を並べる
  for (let d = 1; d <= lastDay; d++) {
    sh.getCell(1, d + 1).value = month + "月" + d + "日"; // d+1：A列の次(B列)から開始
  }

  // ===== 行割当（id → 行番号） =====
  const rowMap = {};
  Object.keys(stateMap).forEach(function (id, i) {
    const row = i + 2; // 2行目から開始（1行目はヘッダー）
    rowMap[id] = row; // id → 行番号 を保存
    sh.getCell(row, 1).value = stateMap[id]; // A列に表示名（キッチン名）
  });

  // ===== 利用期間：〇埋め =====
  periods.forEach(function (p) {
    // この期間のキッチンIDに対応する行番号を取得
    const row = rowMap[p.stateId];
    if (!row) return; // マップに無いIDなら対象外

    // 期間開始/終了を日付に変換
    const start = fnToDateOnly(p.start);
    const end = fnToDateOnly(p.end);

    // 対象月の範囲内に切り詰める
    const fillStart = start > monthStart ? start : monthStart;
    const fillEnd = end < monthEnd ? end : monthEnd;
    // 対象月に一切かからない場合はスキップ
    if (fillStart > fillEnd) return;

    const cur = new Date(fillStart); // fillStart から fillEnd まで 1日ずつ進めながら "〇" を入れる
    while (cur <= fillEnd) {
      const col = 1 + cur.getDate(); // 列番号：1日→B列なので (1 + 日)
      sh.getCell(row, col).value = "〇"; // セルに "〇"
      cur.setDate(cur.getDate() + 1);// 次の日へ
    }
  });

  // ===== 使用不可期間：使用不可で上書き =====
  unavail.forEach(function (p) {
    // この期間のキッチンIDに対応する行番号を取得
    const row = rowMap[p.stateId];
    if (!row) return; // マップに無いIDなら対象外

    // 期間開始/終了を日付に変換
    const start = fnToDateOnly(p.start);
    const end = fnToDateOnly(p.end);

    // 対象月の範囲内に切り詰める
    const fillStart = start > monthStart ? start : monthStart;
    const fillEnd = end < monthEnd ? end : monthEnd;
    // 対象月に一切かからない場合はスキップ
    if (fillStart > fillEnd) return;

    const cur = new Date(fillStart); // fillStart から fillEnd まで 1日ずつ進めながら "使用不可" を入れる
    while (cur <= fillEnd) {
      const col = 1 + cur.getDate(); // 列番号：1日→B列
      sh.getCell(row, col).value = "使用不可"; // "〇" が入っていてもここで上書きする（使用不可が優先）
      cur.setDate(cur.getDate() + 1);// 次の日へ
    }
  });

  // ===== ダウンロード処理 =====
  const file = `利用状況_${year}_${String(month).padStart(2, "0")}.xlsx`;// ファイル名を作成（例：利用状況_2026_01.xlsx）
  // ExcelJS：バッファ生成 → Blob化 → a.click() でダウンロード
  wb.xlsx.writeBuffer().then(function (buf) {
    // Blob作成（xlsxのMIMEタイプ）
    const blob = new Blob([buf], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });
    const url = URL.createObjectURL(blob);// 一時URLを作成
    const a = document.createElement("a");// ダウンロード用 a タグを作成
    a.href = url;       // BlobのURL
    a.download = file;  // 保存名
    document.body.appendChild(a); // DOMに追加（クリックできる状態にする）
    a.click(); // 強制クリックでダウンロード開始
    a.remove(); // DOMから削除
    // URLを解放（メモリリーク対策）
    setTimeout(function () { URL.revokeObjectURL(url); }, 3000);
  });
}

/* ============================================================
    ClassAから「先頭のID」を取り出す
============================================================ */
function fnPickFirstId(v) {
  if (v === undefined || v === null) return ""; // null/undefined 対策：空文字で返す
  if (Array.isArray(v)) return v.length ? String(v[0]).trim() : "";// 既に配列なら先頭を返す
  const s = String(v).trim(); // 文字列化して前後空白を除去
  if (!s) return ""; // 空なら終了
  // JSON配列っぽい場合（先頭が "["）は parse を試す
  if (s[0] === "[") {
    try {
      const arr = JSON.parse(s); // JSON文字列を配列へ
      if (Array.isArray(arr) && arr.length) return String(arr[0]).trim(); // 先頭を返す
    } catch (e) {
      alert("予期せぬエラーが発生しました。" , e);
    }
  }
  // 通常文字列ならそのまま返す
  return s;
}

/* ============================================================
    モーダルを閉じる
============================================================ */
function fnCloseModal() {
  document.getElementById("sdt-usage-excel-modal")?.remove();
}
