// ----------------- メイン処理 -----------------
// モーダル内：バリデーションあり
$(document).on('click', '.fn-sdt-modal-export', function (e) {
  formValidate.call(this, e);
});


// 直接エクスポート：バリデーションなし
$(document).on('click', '.fn-export', async function (e) {
  e.preventDefault();
  const button = this;
  const buttonId = this.id; // htmlのbuttonのid

  const sqlFileName = exportWithoutParameter[buttonId]

  if (!sqlFileName) {
    alert("対応するSQLが定義されていません");
    return;
  }

  try {
    alert('Excelファイルを出力します');
    const extendSqlResult = await getExtendSqlWithoutParameter(sqlFileName);
    await exportExcel(extendSqlResult, button);
  } catch (err) {
    window.force && console.error('エクスポートエラー:', err);
    alert('エラー: ' + err.message);
  }
});


// ----------------- ヘルパー関数 -----------------
/**
 * getExportTitle
 * - Excelファイル名に使うタイトルを取得
 * - モーダル経由 / 直接エクスポート 両対応
 *
 * @param {HTMLElement} button
 * @returns {string}
 */
function getExportTitle(button) {
  // ① モーダル内の「出力」ボタンか？
  const modal = button.closest('.fn-modal');

  if (modal) {
    const modalId = modal.dataset.modal;
    if (!modalId) return '';

    const openModalButton = document.querySelector(
      `.fn-open-modal[data-target="${modalId}"]`
    );

    return openModalButton
      ? openModalButton.innerText.trim()
      : '';
  }

  // ② 直接エクスポート（fn-export）
  let title = button.innerText.trim();

  // ▼ 例外対応：イベント写真（写真名のみ）
  if (button.id === 'fn-eventphoto-export') {
    title = 'イベント写真';
  }

  return title;
}


/**
 * createExcelFileName
 * - クリックされたボタンのテキストを元にExcelファイル名を生成
 * - 日付inputがない場合は本日の日付を使用
 *
 * @param {HTMLElement} button クリックされたボタン要素
 * @returns {string}
 */
function createExcelFileName(exportButton) {
  let year;
  let month;

  const dateInput = exportButton
    .closest('form')
    ?.querySelector('.fn-modal-start-date');

  if (dateInput && dateInput.value) {
    const [y, m] = dateInput.value.split('-');
    year = y;
    month = m;
  } else {
    const today = new Date();
    year = today.getFullYear();
    month = String(today.getMonth() + 1).padStart(2, '0');
  }

  const yearMonth = `${year}年${month}月`;

  const title = getExportTitle(exportButton);
  const safeTitle = title.replace(/[\\/:*?"<>|]/g, '');

  return `${yearMonth}_${safeTitle}.xlsx`;
}


/**
 * getExtendSqlWithoutParameter
 * - 拡張sqlを実行しjson生データを取得するする関数
 * - 引数: jsonFromSql - 拡張sqlで取得したjson配列
 * - 動作: 引数で渡されたjson配列をExcelファイルに変換してダウンロードさせる
 */
async function getExtendSqlWithoutParameter(sqlFileName) {
  if (!sqlFileName) {
    throw new Error('sqlFileNameが指定されていません');
  }

  //パラメーターあり
  const getEvents = sql(sqlFileName, { DateFrom: null, DateTo: null });

  // 12/1〜12/31で絞り込み
  const rows = await getEvents();

  window.force && console.log(rows.length);
  window.force && console.table(`拡張SQL(${sqlFileName})取得結果：`, rows);

  let obj;
  try {
    obj = JSON.parse(rows);
  } catch (e) {
    throw new Error('サーバーからのレスポンスが不正です');
  }

  const resultSql = obj?.Response?.Data?.Table;

  if (!resultSql) {
    throw new Error('該当するデータがありません');
  }
  return resultSql;
}


/**
 * getExtendSqlWithParameter
 * - 拡張sqlを実行しjson生データを取得するする関数
 * - 引数: sqlFileName - 拡張sqlファイル名
 * - 引数：dateRange - パラメーターとして渡す期間
 * - 動作: 引数で渡されたsqlを実行し、取得したjson生データをjsonパースして返す(resultSql)
 */
async function getExtendSqlWithParameter(sqlFileName, dateRange) {
  if (!sqlFileName) {
    throw new Error('sqlFileNameが指定されていません');
  }

  const getEvents = sql(sqlFileName, dateRange);

  const rows = await getEvents();

  if (!rows) {
    throw new Error('サーバーからレスポンスがありません');
  }

  let obj;
  try {
    obj = JSON.parse(rows);
  } catch (e) {
    throw new Error('サーバーからのレスポンスが不正です');
  }

  const resultSql = obj?.Response?.Data?.Table;

  if (!resultSql) {
    throw new Error('該当するデータがありません');
  }

  return resultSql;
}



/**
 * exportExcel
 * - 拡張sqlで取得したjson配列をExcelファイルとして出力する関数
 * - 引数: jsonFromSql - 拡張sqlで取得したjson配列
 * - 動作: 引数で渡されたjson配列をExcelファイルに変換してダウンロードさせる
 */
async function exportExcel(jsonFromSql, button) {

  // ===== イベント店舗の時データ生成が必要 =====
  const modal = button.closest('.fn-modal');
  const isEventShop = modal?.dataset.modal === 'modal-B';

  let data = jsonFromSql;

  if (isEventShop) {
    data = convertEventShopData(jsonFromSql);
    if (!Array.isArray(data) || data.length === 0) {
        throw new Error('出力するデータがありません');
    }

    // 日付フォーマット変換
    const formattedData = formatDateColumns(data);

    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Sheet1');

    const header = Object.keys(formattedData[0]);
    worksheet.addRow(header);

    formattedData.forEach(row => {
        worksheet.addRow(Object.values(row));
    });

    const fileName = createExcelFileName(button);

    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob(
        [buffer],
        { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }
    );

    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);

    return; 
  }

  // データなしチェック
  if (!Array.isArray(jsonFromSql) || jsonFromSql.length === 0) {
    throw new Error('出力するデータがありません');
  }

  // 日付フォーマット変換
  const formattedData = formatDateColumns(jsonFromSql);

  const workbook = new ExcelJS.Workbook();
  const worksheet = workbook.addWorksheet('Sheet1');

  const header = Object.keys(formattedData[0]);
  worksheet.addRow(header);

  formattedData.forEach(row => {
    worksheet.addRow(Object.values(row));
  });

  const fileName = createExcelFileName(button);

  const buffer = await workbook.xlsx.writeBuffer();
  const blob = new Blob(
    [buffer],
    { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }
  );

  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = fileName;
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
  URL.revokeObjectURL(url);
}

//イベント店舗データ生成用
function convertEventShopData(rows) {

  const meaningItems = [
    { id: "1", label: "参加意義・期待効果-流通拡大・販路開拓" },
    { id: "2", label: "参加意義・期待効果-認知度向上・魅力発信" },
    { id: "3", label: "参加意義・期待効果-関係構築・強化" },
    { id: "4", label: "参加意義・期待効果-ブランド価値向上" },
    { id: "5", label: "参加意義・期待効果-定番化・自由化" },
    { id: "6", label: "参加意義・期待効果-その他" },
  ];

  const prItems = [
    { id: "1", label: "販促方法（PR）-Facebook" },
    { id: "2", label: "販促方法（PR）-Instagram" },
    { id: "3", label: "販促方法（PR）-X" },
    { id: "4", label: "販促方法（PR）-店舗HP" },
    { id: "5", label: "販促方法（PR）-店内チラシ" },
    { id: "6", label: "販促方法（PR）-新聞折込" },
    { id: "7", label: "販促方法（PR）-その他" },
  ];

  const INSERT_MEANING_AFTER = "参加概要・予定事業者・委託先候補";
  const INSERT_PR_AFTER = "販促方法（現場）";

  return rows.map(row => {

    // --- パース ---
    let meaningVals = [];
    let prVals = [];

    try {
      meaningVals = JSON.parse(row["参加意義・期待効果"] || "[]");
    } catch {}

    try {
      prVals = JSON.parse(row["販促方法（PR）"] || "[]");
    } catch {}

    const result = {};

    Object.keys(row).forEach(key => {

      // 不要な元列は除外
      if (
        key !== "参加意義・期待効果" &&
        key !== "販促方法（PR）"
      ) {
        result[key] = row[key];
      }

      // ① 参加意義・期待効果 → 展開
      if (key === INSERT_MEANING_AFTER) {
        meaningItems.forEach(item => {
          result[item.label] = meaningVals.includes(item.id);
        });
      }

      // ② 販促方法（PR） → 展開
      if (key === INSERT_PR_AFTER) {
        prItems.forEach(item => {
          result[item.label] = prVals.includes(item.id);
        });
      }
    });

    return result;
  });
}


/**
 * formValidate
 * - フォームのバリデーションを行う関数
 * - 引数: form - バリデーションを行うフォーム要素
 * - 動作: フォームの有効性をチェックし、無効な場合は処理を中断する。有効な場合はExcel出力を行う。
 */
async function formValidate(e) {
  e.preventDefault();

  const button = this;
  const form = this.closest('form');
  if (!form) return;

  if (!form.reportValidity()) return;

  const buttonId = button.id;
  const sqlFileName = exportWithParameter[buttonId];

  if (!sqlFileName) {
    alert("対応するSQLが定義されていません");
    return;
  }

  let dateRange;
  try {
    dateRange = getDateRangeFromModal(button);
  } catch (err) {
    alert(err.message);
    return;
  }

  try {
    alert('Excelファイルを出力します');
    const extendSqlResult = await getExtendSqlWithParameter(sqlFileName, dateRange);
    await exportExcel(extendSqlResult, button);
  } catch (err) {
    window.force && console.error('エクスポートエラー:', err);
    alert('エラー: ' + err.message);
  }
}

/**
 * getDateRangeFromModal
 * - モーダル内の入力値から DateFrom / DateTo を生成
 *
 * @param {HTMLElement} button クリックされたボタン
 * @returns {{ DateFrom: string, DateTo: string }}
 */
function getDateRangeFromModal(button) {
  const modal = button.closest('.fn-modal');
  if (!modal) {
    throw new Error('モーダルが見つかりません');
  }

  const startInput = modal.querySelector('.fn-modal-start-date');
  const finishInput = modal.querySelector('.fn-modal-finish-date');

  if (!startInput || !startInput.value) {
    throw new Error('開始日が入力されていません');
  }

  const buttonId = button.id;

  // ===== 加工データ（月指定） =====
  if (buttonId === 'fn-processed-data-export') {
    // YYYY-MM
    const [year, month] = startInput.value.split('-').map(Number);

    const firstDay = `${year}-${String(month).padStart(2, '0')}-01`;

    // 月末日を取得
    const lastDate = new Date(year, month, 0); // 次月0日 = 月末
    const lastDay = `${year}-${String(month).padStart(2, '0')}-${String(lastDate.getDate()).padStart(2, '0')}`;

    return {
      DateFrom: firstDay,
      DateTo: lastDay
    };
  }

  // ===== 通常（開始日〜終了日） =====
  if (!finishInput || !finishInput.value) {
    throw new Error('終了日が入力されていません');
  }

  return {
    DateFrom: startInput.value,
    DateTo: finishInput.value
  };
}

/**
 * ISO日付文字列を指定フォーマットに変換
 *
 * @param {string} value ISO文字列 (例: 2026-01-16T04:20:46.948)
 * @param {string} format YYYY-MM-DD or YYYY-MM-DD HH:mm:ss
 * @returns {string}
 */
function formatDate(value, format) {
  const date = new Date(value);
  if (isNaN(date)) return value;

  const yyyy = date.getFullYear();
  const mm = String(date.getMonth() + 1).padStart(2, '0');
  const dd = String(date.getDate()).padStart(2, '0');
  const hh = String(date.getHours()).padStart(2, '0');
  const mi = String(date.getMinutes()).padStart(2, '0');
  const ss = String(date.getSeconds()).padStart(2, '0');

  if (format === 'YYYY-MM-DD') {
    return `${yyyy}-${mm}-${dd}`;
  }

  if (format === 'YYYY-MM-DD HH:mm:ss') {
    return `${yyyy}-${mm}-${dd} ${hh}:${mi}:${ss}`;
  }

  if (format === 'HH:mm:ss') {
    return `${hh}:${mi}:${ss}`;
  }

  return value;
}



/**
 * 日付項目を定義に従って整形する
 *
 * @param {Array<Object>} rows
 * @returns {Array<Object>}
 */
function formatDateColumns(rows) {
  return rows.map(row => {
    const formattedRow = { ...row };

    Object.keys(formattedRow).forEach(key => {
      const format = DATE_FORMAT_MAP[key];
      const value = formattedRow[key];

      if (format && value) {
        formattedRow[key] = formatDate(value, format);
      }
    });

    return formattedRow;
  });
}
