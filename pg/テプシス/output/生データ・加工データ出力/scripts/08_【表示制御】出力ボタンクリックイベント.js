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

  alert('Excelファイルを出力します');

  const extendSqlResult = await getExtendSqlWithoutParameter(sqlFileName);
  if (!extendSqlResult) return;

  exportExcel(extendSqlResult, button);
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
  return button.innerText.trim();
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

  // console.log(rows.length);
  // console.table(`拡張SQL(${sqlFileName})取得結果：`, rows);

  let obj = JSON.parse(rows);
  try {
    obj = JSON.parse(rows);
  } catch (e) {
    console.log("拡張sqlのjsonパースに失敗しました。");
    console.log(rows);
    return;
  };

  const resultSql = obj?.Response?.Data?.Table;  // JSON生データ

  if (!resultSql) {
    console.log("拡張sqlで取得されたjsonがありません");
    return;
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
    console.log("拡張sqlで取得されたjsonがありません");
    return;
  }

  let obj = JSON.parse(rows);
  try {
    obj = JSON.parse(rows);
  } catch (e) {
    console.log("拡張sqlのjsonパースに失敗しました。");
    console.log(rows);
    return;
  }

  const resultSql = obj?.Response?.Data?.Table;

  if (!resultSql) {
    console.log("拡張sqlで取得されたjsonがありません");
    return;
  }

  return resultSql;
}



/**
 * exportExcel
 * - 拡張sqlで取得したjson配列をExcelファイルとして出力する関数
 * - 引数: jsonFromSql - 拡張sqlで取得したjson配列
 * - 動作: 引数で渡されたjson配列をExcelファイルに変換してダウンロードさせる
 */
// function exportExcel(jsonFromSql, button) {
//   const workbook = new ExcelJS.Workbook();
//   const worksheet = workbook.addWorksheet('Sheet1');

//   const header = Object.keys(jsonFromSql[0]);
//   worksheet.addRow(header);

//   jsonFromSql.forEach(row => {
//     worksheet.addRow(Object.values(row));
//   });

//   const fileName = createExcelFileName(button);

//   workbook.xlsx.writeBuffer().then(buffer => {
//     const blob = new Blob(
//       [buffer],
//       { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }
//     );

//     const url = URL.createObjectURL(blob);

//     const a = document.createElement('a');
//     a.href = url;
//     a.download = fileName;
//     document.body.appendChild(a);
//     a.click();

//     document.body.removeChild(a);
//     URL.revokeObjectURL(url);
//   });
// }

function exportExcel(jsonFromSql, button) {

  // ★ 追加：データなしチェック
  if (!Array.isArray(jsonFromSql) || jsonFromSql.length === 0) {
    alert('出力するデータがありません。');
    return;
  }

  // ★ 追加：日付フォーマット変換
  const formattedData = formatDateColumns(jsonFromSql);

  const workbook = new ExcelJS.Workbook();
  const worksheet = workbook.addWorksheet('Sheet1');

  const header = Object.keys(formattedData[0]);
  worksheet.addRow(header);

  formattedData.forEach(row => {
    worksheet.addRow(Object.values(row));
  });

  const fileName = createExcelFileName(button);

  workbook.xlsx.writeBuffer().then(buffer => {
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

  alert('Excelファイルを出力します');

  const extendSqlResult = await getExtendSqlWithParameter(sqlFileName, dateRange);
  // this = クリックされた .fn-sdt-modal-export ボタン
  exportExcel(extendSqlResult, button);
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
