// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝コード値系＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

const SUCCESS_MESSAGE_CODE = {
    // 成功メッセージ
    'SUCCESS_CREATE_FILE': 'ファイルの作成に成功しました',
    'SUCCESS_UPDATE': '更新に成功しました',
    'SUCCESS_DELETE': '削除に成功しました',
    'SUCCESS_UPLOAD': 'アップロードに成功しました',
    'SUCCESS_DOWNLOAD': 'ダウンロードに成功しました',
    'SUCCESS_SAVE': '保存に成功しました',
    'SUCCESS_CHECK': 'チェックが完了しました',
    'NOT_FOUND': '該当データ無し',
}
// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝判定系＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

/**
 * Null判定する関数です。
 * @param {object} obj オブジェクト
 * @returns {boolean} 判定結果
 * @throws {Error} 処理中に発生したエラー
 */
function commonIsNull(obj) {
    if (Array.isArray(obj)) {
        return obj.filter(v => String(v).trim() !== '').length == 0;
    } else if (typeof obj === 'object') {
        return !obj || Object.keys(obj).length === 0 && obj.constructor === Object;
    } else {
        return !obj && obj !== 0 || String(obj).trim() == '';
    }
}

/**
 * ステータスをチェックする関数です。
 * @param {Array|string} applyStatuses 適用するステータス
 * @returns {boolean} チェック結果
 * @throws {Error} 処理中に発生したエラー
 */
function commonCheckStatus(applyStatuses = "all") {
    if ($p.action() !== "edit" && $p.action() !== "new") {
        let message = "編集画面で使用を想定"
        alert(message)
        throw new Error(message)
    }
    let allFlg = false;
    if (!Array.isArray(applyStatuses)) {
        if (applyStatuses === "all") {
            allFlg = true;
        } else {
            applyStatuses = [applyStatuses];
        }
    }
    return allFlg || applyStatuses.map(v => +v).includes(+commonGetValue("Status", false));
}

// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝値取得&値設定系＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

/**
 * 日付を出力する関数です。
 *
 * yyyy引数なしで現在日付
 * 月末はddに0を入力
 *
 * @param {Number} yyyy   年(またはDateオブジェクト)
 * @param {Number} mm     月
 * @param {Number} dd     日
 * @param {String} format フォーマット
 *
 * @return {String} フォーマット加工された日付文字列
 *
 * 例1. yyyy = 2022, mm = 4, dd = 18, format = 'YYYY-MM-DD'
 *            ⇓
 *     '2022-04-18'
 * 例2. yyyy = 2021, mm = 13, dd = 10, format = 'YYYY-MM-DD'
 *            ⇓
 *     '2022-01-10'
 * 例3. yyyy = 2021, mm = 11, dd = 0, format = 'YYYY-MM-DD'
 *            ⇓
 *     '2022-10-31'
 * 例4. yyyy = 2020
 *            ⇓
 *     '2020-01-01T00:00:00'
 * 例5. 引数なし
 *            ⇓
 *     現在日付の'YYYY-MM-DD'
 * 例6. yyyy = "", mm = "", dd = "", format = 'YYYYMM'
 *            ⇓
 *     現在日付の'YYYYMM'
 */
function commonGetDate(yyyy, mm = 1, dd = 1, format = 'YYYY-MM-DD') {
    let date
    if (typeof yyyy === 'undefined' || commonIsNull(yyyy)) {
        date = new Date()
    } else if (typeof yyyy === 'object') {
        date = yyyy
    } else {
        date = new Date(yyyy, mm - 1, dd)
    }
    return format
        .replace(/YYYY/, String(date.getFullYear()).padStart(4, "0"))
        .replace(/MM/, String(date.getMonth() + 1).padStart(2, "0"))
        .replace(/DD/, String(date.getDate()).padStart(2, "0"))
        .replace(/hh/, String(date.getHours()).padStart(2, "0"))
        .replace(/mm/, String(date.getMinutes()).padStart(2, "0"))
        .replace(/ss/, String(date.getSeconds()).padStart(2, "0"))

}

/**
 * 区分文字列から値に応じた区分名を返す
 * @param {string} input - "0:新規 1:修正" 形式の文字列
 * @param {number} value - 判定する値（0または1）
 * @returns {string} 区分名
 */
function commonGetDivisionName(input, value) {
    // 文字列を分割して配列に変換
    const pairs = input.split(' ').map(pair => {
        const [code, name] = pair.split(':');
        return { code: parseInt(code), name };
    });

    // 該当する値の区分名を返す
    const found = pairs.find(pair => pair.code == value);
    return found ? found.name : '未定義';
}

/**
 * 入力されたラベルに一致する項目の選択した値を返却する。
 * @param {String} label ラベル
 * @param {Boolean} valueFlg true : value値 false : display値
 * @returns {*} 選択された値または表示テキスト
 * @throws {Error} 処理中に発生したエラー
 */
function commonGetValue(label, valueFlg = true) {
    if ($p.action() !== "edit" && $p.action() !== "new") {
        let message = "編集画面で使用を想定"
        alert(message)
        throw new Error(message)
    }
    const control = $p.getControl($p.getColumnName(label));
    const tagName = control.prop("tagName");

    let value;

    switch (tagName) {
        case "SELECT":
            value = commonHandleSelect(control, valueFlg);
            break;
        case "INPUT":
            value = commonHandleInput(control, label, valueFlg);
            break;
        case "TEXTAREA":
            value = document.getElementById(commonGetId(label) + ".viewer").innerText;
            break;
        default:
            value = commonHandleDefault(control, valueFlg);
    }

    return commonParseIfJson(value);
}

/**
 * 入力されたラベルに一致する項目の選択した値を入力する。
 * @param {String} label ラベル
 * @param {object} value 値
 */
function commonSetValue(label, value) {
    if ($p.action() !== "edit" && $p.action() !== "new") {
        let message = "編集画面で使用を想定"
        alert(message)
        throw new Error(message)
    }

    // 値が変更されている場合
    if (commonGetValue(label, true) != value) {
        if ($p.getColumnName(label).startsWith("Check")) {
            // changeイベントを動かす
            $p.getControl(label).click()
        } else {
            value = (value == commonGetDateEmpty()) ? "" : value
            $p.set($p.getControl(label), value)
        }
    }
}

/**
 * 入力されたラベルのIDを返却する。
 * @param {String} label ラベル
 * @param {Boolean} [prefix=true] プレフィックス（テーブル名）を付けるかどうか
 * @param {Boolean} [suffix=false] サフィックス（'Field'）を付けるかどうか
 * @returns {String} 生成されたID
 * @throws {Error} ラベルが不正な場合やその他のエラー
 */
function commonGetId(label, prefix = true, suffix = false) {
    if ($p.action() !== "edit" && $p.action() !== "new") {
        let message = "編集画面で使用を想定"
        alert(message)
        throw new Error(message)
    }

    let id = $p.getColumnName(label);
    if (commonIsNull(id)) {
        let message = `commonGetId ：ラベル不正。${label}`
        alert(message)
        throw new Error(message)
    }
    id = prefix ? `${$p.tableName()}_${id}` : id;
    id = suffix ? `${id}Field` : id;
    return id;

}

/**
 * SELECT要素の値を処理する。
 * @param {jQuery} control jQuery対象のSELECT要素
 * @param {Boolean} valueFlg true : value値 false : display値
 * @returns {*} 選択された値または表示テキスト
 * @throws {Error} 処理中に発生したエラー
 */
function commonHandleSelect(control, valueFlg) {
    if (control.attr("multiple")) {
        return valueFlg ? control.val() : control.next().children().last().text();
    }
    const selectedOption = control.children(':selected');
    return valueFlg ? selectedOption.val() : selectedOption.text();
}

/**
 * INPUT要素の値を処理する。
 * @param {jQuery} control jQuery対象のINPUT要素
 * @param {String} label ラベル
 * @param {Boolean} valueFlg true : value値 false : display値
 * @returns {*} 入力された値または表示テキスト
 * @throws {Error} 処理中に発生したエラー
 */
function commonHandleInput(control, label, valueFlg) {
    const inputId = commonGetId(label);
    if (inputId.indexOf("Check") > 0) {
        const checked = document.getElementById(inputId).checked;
        return valueFlg ? +checked : checked;
    }
    let value = valueFlg ? control.attr('data-value') : control[0].innerHTML;
    return commonIsNull(value) ? control.val() : value;

}

/**
 * デフォルトの要素処理を行う。
 * @param {jQuery} control jQuery対象の要素
 * @param {Boolean} valueFlg true : value値 false : display値
 * @returns {*} 要素の値または表示テキスト
 * @throws {Error} 処理中に発生したエラー
 */
function commonHandleDefault(control, valueFlg) {
    let value = valueFlg ? (control.attr('data-value') ?? control[0].innerHTML) : control[0].innerHTML;
    return commonIsNull(value) ? control.val() : value;
}

/**
 * 空日付を出力する関数です。
 *
 * @return {String} 空日付
 */
function commonGetDateEmpty() {
    return '1899-12-30T00:00:00'
}

// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝値変換系＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
/**
 * 値がJSON文字列の場合はパースし、そうでない場合は元の値を返す。
 * @param {*} value パース対象の値
 * @returns {*} パースされた値または元の値
 */
function commonParseIfJson(value) {
    if (typeof value !== 'string') return value;
    try {
        return JSON.parse(value);
    } catch (err) {
        return value;
    }
}

/**
 * file を Base64 文字列に変換する。
 * @param {ArrayBuffer} file 変換する file
 * @returns {string} Base64 エンコードされた文字列
 */
async function commonToBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => {
            const base64String = reader.result.split(",")[1];
            resolve(base64String);
        };
        reader.onerror = (err) => reject(err);
        reader.readAsDataURL(file);
    });
}

/**
   * 引数の2次元配列をUTF-8のCSVに変換する関数です。
   * @param {Array} array 2次元配列
   *
   * @return {String} csvData
   * 例. [
   *      ['数量', '単価', '合計'],
   *      ['1', '2', '2'],
   *      ['4', '5', '20'],
   *      ['7', '8', '56'],
   *     ]
   *            ⇓
   * 'data:text/csvcharset=utf-8,"数量","単価","合計"\r\n"1","2","2"\r\n"4","5","20"\r\n"7","8","56"\r\n'
   */
function commonConvert2DToCsv(d2array) {
    // csvDataに出力方法を追加
    let csvOutput = 'data:text/csvcharset=utf-8,'
    let csvData = csvOutput
    d2array.forEach(v => {
        const row = '"' + v.join('","') + '"'
        csvData += row + '\r\n'
    })
    return csvData
}

/**
   * 引数の2次元配列をUTF-8のCSVに変換する関数です。
   * @param {String} csvData
   *
   * @return {Array} array 2次元配列
   * 例. 'data:text/csvcharset=utf-8,"数量","単価","合計"\r\n"1","2","2"\r\n"4","5","20"\r\n"7","8","56"\r\n'
   *            ⇓
   *     [
   *      ['数量', '単価', '合計'],
   *      ['1', '2', '2'],
   *      ['4', '5', '20'],
   *      ['7', '8', '56'],
   *     ]
   */
function commonConvertCsvTo2D(csvData) {
    // 行の分割
    let rows = csvData.split(/\r?\n/);

    // 空行の除去
    let filteredRows = rows.filter(row => row.trim() !== '');

    // 各行を処理
    let processedData = filteredRows.map(row => {
        // カラムに分割
        let columns = row.split(',');

        // 各値ごとに型変換処理
        return columns.map(value => {
            let trimmed = value.trim();
            let num = Number(trimmed);

            if (!isNaN(num) && trimmed !== '') {
                return num;
            }
            return trimmed;
        });
    });

    return processedData;
}

// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝要素追加&要素編集系＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
/**
 * 指定された文言と完全に一致する最初のリンクを検索し、href属性を'#'に変更して、クリック時にカスタム関数を実行する関数
 * @param {string} searchText - リンクを検索するための文言（完全一致）
 * @param {function} customClickHandler - クリック時に実行されるカスタム関数
 * @returns {boolean} - リンクが見つかって修正されたかどうか
 * @throws {Error} 処理中に発生したエラー
 */
function commonModifyLink(searchText, customClickHandler) {
    const link = Array.from(document.getElementsByTagName('a')).find(a =>
        a.textContent.trim() === searchText.trim()
    );

    if (link) {
        const dataValue = link.closest('[data-value]')?.getAttribute('data-value');
        link.setAttribute('href', '#');
        link.style.color = 'black';
        link.style.textDecoration = 'none';
        link.onclick = function (event) {
            event.preventDefault();
            customClickHandler(dataValue, link.textContent);
            return false;
        };
        return true;
    } else {
        console.warn(`"${searchText}" と完全に一致するリンクが見つかりませんでした。`);
        return false;
    }
}

/**
 * メインコマンドエリアにボタンを追加する関数です。
 * @param {String} buttonId     ボタンID
 * @param {String} buttonLabel  ラベル
 * @param {Function} clickFunc  click時関数
 * @param {String} icon         アイコン（empty指定でアイコンなし）
 * @throws {Error}              処理中に発生したエラー
 */
function commonAddButtonToMainCommands(buttonId, buttonLabel, clickFunc, icon = "ui-icon-disk") {
    const target = document.getElementById("MainCommands");
    const elem = document.createElement('button');
    elem.id = buttonId;
    elem.className = 'button button-icon ui-button ui-corner-all ui-widget applied customized';
    elem.onclick = clickFunc;
    elem.innerText = buttonLabel;
    if (icon !== "empty") {
        const span = document.createElement('span');
        span.className = `ui-button-icon ui-icon ${icon}`;
        elem.appendChild(span);
    }
    const space = document.createElement('span');
    space.className = "ui-button-icon-space";
    elem.appendChild(space);

    target.appendChild(elem);
}

/**
 * 指定項目横にボタンを追加する関数です。
 * @param {String} buttonId     ボタンID
 * @param {String} buttonLabel  ラベル
 * @param {Function} clickFunc  click時関数
 * @param {String} icon         アイコン（empty指定でアイコンなし）
 * @param {Array} applys        適用するステータス
 * @throws {Error}              処理中に発生したエラー
 */
function commonAddButtonToNextField(ItemLabel, buttonId, buttonLabel, clickFunc, icon = "ui-icon-disk", applys = "all") {
    if (!commonCheckStatus(applys)) return;

    // 対象のフィールドを取得
    const targetField = document.getElementById(commonGetId(ItemLabel));
    if (!targetField) {
        let message = `commonAddButtonToNextField :対象項目が見つかりません。${ItemLabel}`
        alert(message)
        throw new Error(message)
    }

    // container-normal クラスを持つ親要素を見つける
    const containerElement = targetField.closest('.container-normal');
    if (!containerElement) {
        let message = `commonAddButtonToNextField :container-normalクラスを持つ親要素が見つかりません。`
        alert(message)
        throw new Error(message)
    }

    // 新しいボタン要素を作成
    const elem = document.createElement('button');
    elem.id = buttonId;
    elem.className = 'button button-icon ui-button ui-corner-all ui-widget applied customized';
    elem.onclick = clickFunc;

    if (icon !== "empty") {
        const span = document.createElement('span');
        span.className = `ui-button-icon ui-icon ${icon}`;
        elem.insertBefore(span, elem.firstChild);
    }

    const space = document.createElement('span');
    space.className = "ui-button-icon-space";
    elem.appendChild(space);

    const textNode = document.createTextNode(buttonLabel);
    elem.appendChild(textNode);

    // ボタンをcontainer-normal要素内に挿入
    containerElement.appendChild(elem);

    // レイアウトとスタイルを調整
    containerElement.style.display = 'flex';
    containerElement.style.alignItems = 'center';
    containerElement.style.flexWrap = 'nowrap';

    targetField.style.flexGrow = '1';
    targetField.style.flexShrink = '1';
    targetField.style.flexBasis = 'auto';
    targetField.style.minWidth = '0';

    elem.style.flexGrow = '0';
    elem.style.flexShrink = '0';
    elem.style.flexBasis = 'auto';
    elem.style.marginLeft = '10px';
    elem.style.whiteSpace = 'nowrap';

}

// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝API系＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
let API_VERSION = "1.0"

/**
 * 取得APIを呼び出す関数です。
 *
 * @param {Number}    id テーブルID
 * @param {Array}     columns 取得列
 * @param {Object}    filterHash フィルター条件
 * @param {Object}    sorterHash ソート条件
 * @param {Object}    filterSearchTypes 検索条件
 * @param {Boolean}   keyFlg true : 表示名 false : カラム名
 * @param {Boolean}   valueFlg true : value値 false : display値
 * @param {Function}  addFunc 最後に実行したい関数
 */
async function commonGetData(id = $p.siteId(), columns = ["ClassA", "NumA"], filterHash = {}, sorterHash = { "ResultId": "asc" }, filterSearchTypes = {}, keyFlg = true, valueFlg = true, addFunc) {
    let offset = 0
    let data = []
    let temp = {}
    do {
        temp = await commonGetInnner(id, columns, filterHash, sorterHash, filterSearchTypes, keyFlg, valueFlg, offset, addFunc)
        data = [...data, ...temp.Response.Data]
        offset += temp.Response.PageSize
    } while (offset < temp.Response.TotalCount)
    return data

    /**
     * 取得APIを呼び出す関数内関数です。(TotalCount 200)
     *
     * @param {Number}    id テーブルID
     * @param {Array}     columns 取得列
     * @param {Object}    filterHash フィルター条件
     * @param {Object}    sorterHash ソート条件
     * @param {Object}    filterSearchTypes 検索条件
     * @param {Boolean}   keyFlg true : 表示名 false : カラム名
     * @param {Boolean}   valueFlg true : value値 false : display値
     * @param {Number}    offset オフセット条件
     * @param {Function}  addFunc 最後に実行したい関数
     */
    async function commonGetInnner(id, columns, filterHash, sorterHash, filterSearchTypes, keyFlg, valueFlg, offset, addFunc) {
        let payload = {
            'id': id,
            'data': {
                'Offset': offset,
                'View': {
                    'ApiDataType': "KeyValues",
                    'ApiColumnKeyDisplayType': keyFlg ? 'LabelText' : 'ColumnName',
                    'ApiColumnValueDisplayType': valueFlg ? "Value" : "DisplayValue",
                    "ColumnFilterHash": filterHash,
                    "ColumnSorterHash": sorterHash,
                    "ColumnFilterSearchTypes": filterSearchTypes,
                }
            },
            'done': function (data) {
                if (addFunc && typeof addFunc === 'function') {
                    // 渡されたオブジェクトが関数なら実行する
                    addFunc(data)
                }
                return data.Response.Data
            }
        }

        if (!Array.isArray(columns)) {
            columns = [columns]
        }
        if (columns.length > 0) {
            payload['data']['View']['GridColumns'] = columns
        }



        return await $p.apiGet(payload)
    }
}

/**
 * エクスポートAPIを呼び出す関数です。
 *
 * @param {String}    tableId 取得テーブルID
 * @param {Array}     columns 取得列
 * @param {Object}    filterHash フィルター条件
 * @param {Object}    sorterHash ソート条件
 * @param {Object}    filterSearchTypes 検索条件
 * @param {Boolean}   jsonFlg trueならjson　falseならcsvで取得
 * @param {Boolean}   headerFlg trueならheaderも取得
 * @param {Boolean}   overFlg trueなら超過分のみ取得
 * @param {Function}  addFunc 最後に実行したい関数
 */
async function commonExportData(tableId = $p.siteId(), columns = ["ClassA", "NumA"], filterHash = {}, sorterHash = { "ResultId": "asc" }, filterSearchTypes = {}, jsonFlg = true, headerFlg = true, overFlg = false, addFunc) {
    if (!Array.isArray(columns)) {
        columns = [columns]
    }
    columns.unshift("ResultId")
    let data = await commonExportInner(tableId, columns, filterHash, sorterHash, filterSearchTypes, jsonFlg, headerFlg, overFlg, addFunc)
    if (jsonFlg) {
        // json整形
        data = JSON.parse(data.Response.Content)
    } else {
        // csv整形
        data = commonConvertCsvTo2D(data.Response.Content)
    }

    return data
    /**
     * エクスポートAPIを呼び出す関数内関数です。
     *
     * @param {String}    tableId 取得テーブルID
     * @param {Array}     columns 取得列
     * @param {Object}    filterHash フィルター条件
     * @param {Object}    sorterHash ソート条件
     * @param {Object}    filterSearchTypes 検索条件
     * @param {Boolean}   jsonFlg trueならjson　falseならcsvで取得
     * @param {Boolean}   headerFlg trueならheaderも取得
     * @param {Boolean}   overFlg trueなら超過分のみ取得
     * @param {Function}  addFunc 最後に実行したい関数
     */
    async function commonExportInner(tableId, columns, filterHash, sorterHash, filterSearchTypes, jsonFlg, headerFlg, overFlg, addFunc) {

        let col = []
        columns.forEach(v => col.push({ "ColumnName": v }))
        let data = JSON.stringify({
            "ApiVersion": API_VERSION,
            "Export": {
                "Columns": col,
                "Header": headerFlg,
                "Type": jsonFlg ? "json" : "csv"
            },
            "View": {
                "Overdue": overFlg,
                "ColumnFilterHash": filterHash,
                "ColumnSorterHash": sorterHash,
                "ColumnFilterSearchTypes": filterSearchTypes,
            }
        })

        return new Promise((resolve, reject) => {
            $.ajax({
                type: "POST",
                url: `/api/items/${tableId}/export`,
                contentType: 'application/json',
                data: data
            }).then(
                function (result) {
                    if (addFunc && typeof addFunc === 'function') {
                        // 渡されたオブジェクトが関数なら実行する
                        addFunc(data)
                    }
                    // 正常終了
                    resolve(result)
                },
                function () {
                    // エラー
                    reject()
                }
            )
        })
    }
}

/**
 * 登録APIを呼び出す関数です。
 *
 * @param {String}    tableId 登録テーブルID
 * @param {Object}    Hash 更新項目
 * @param {String}    Status 登録ステータス
 * @param {String}    Comments 登録コメント
 * @param {Function}  addFunc 最後に実行したい関数
 */
async function commonCreate(tableId, Hash = {}, Status, Comments, addFunc) {

    // 登録項目
    let ClassHash = {}
    let NumHash = {}
    let DateHash = {}
    let DescriptionHash = {}
    let CheckHash = {}
    for (let key in Hash) {
        if (key.includes("Class")) ClassHash[key] = Hash[key]
        else if (key.includes("Num")) NumHash[key] = commonConvertCTo1(Hash[key])
        else if (key.includes("Date")) DateHash[key] = commonIsNull(Hash[key]) ? commonGetDateEmpty() : Hash[key]
        else if (key.includes("Description")) DescriptionHash[key] = Hash[key]
        else if (key.includes("Check")) CheckHash[key] = Hash[key]
    }

    let data = JSON.stringify({
        "ApiVersion": API_VERSION,
        Status,
        Comments,
        ClassHash,
        NumHash,
        DateHash,
        DescriptionHash,
        CheckHash
    })
    if (!commonIsNull(Status)) {
        delete data["Status"]
    }
    if (!commonIsNull(Comments)) {
        delete data["Comments"]
    }

    return new Promise((resolve, reject) => {
        $.ajax({
            type: "POST",
            url: `/api/items/${tableId}/create`,
            contentType: 'application/json',
            data: data
        }).then(
            function (result) {
                if (addFunc && typeof addFunc === 'function') {
                    // 渡されたオブジェクトが関数なら実行する
                    addFunc(data)
                }
                // 正常終了
                resolve(result)
            },
            function () {
                // エラー
                reject()
            }
        )
    })
}

/**
 * 更新APIを呼び出す関数です。
 *
 * @param {String}    recordId 更新レコードID
 * @param {Object}    Hash 更新項目
 * @param {String}    Status 更新ステータス
 * @param {String}    Comments 更新コメント
 * @param {Function}  addFunc 最後に実行したい関数
 */
async function commonUpdate(recordId, Hash = {}, Status, Comments, addFunc) {

    // 更新項目
    let ClassHash = {}
    let NumHash = {}
    let DateHash = {}
    let DescriptionHash = {}
    let CheckHash = {}
    for (let key in Hash) {
        if (key.includes("Class")) ClassHash[key] = Hash[key]
        else if (key.includes("Num")) NumHash[key] = commonConvertCTo1(Hash[key])
        else if (key.includes("Date")) DateHash[key] = commonIsNull(Hash[key]) ? commonGetDateEmpty() : Hash[key]
        else if (key.includes("Description")) DescriptionHash[key] = Hash[key]
        else if (key.includes("Check")) CheckHash[key] = Hash[key]
    }
    let data = JSON.stringify({
        "ApiVersion": API_VERSION,
        Status,
        Comments,
        ClassHash,
        NumHash,
        DateHash,
        DescriptionHash,
        CheckHash
    })
    if (!commonIsNull(Status)) {
        delete data["Status"]
    }
    if (!commonIsNull(Comments)) {
        delete data["Comments"]
    }

    return new Promise((resolve, reject) => {
        $.ajax({
            type: "POST",
            url: `/api/items/${recordId}/update`,
            contentType: 'application/json',
            data: data
        }).then(
            function (result) {
                if (addFunc && typeof addFunc === 'function') {
                    // 渡されたオブジェクトが関数なら実行する
                    addFunc(data)
                }

                // 正常終了
                resolve(result)
            },
            function () {
                // エラー
                reject()
            }
        )
    })
}

/**
 * ユーザー取得APIを呼び出す関数です。
 *
 * @param {Array}     userIds 取得UserId
 * @param {Function}  addFunc 最後に実行したい関数
 */
async function commonExportUser(userIds, addFunc) {
    let users = []
    if (Array.isArray(userIds)) {
        users = userIds
    } else {
        users = [userIds]
    }
    let data = JSON.stringify({
        "ApiVersion": API_VERSION,
        "View": {
            "ColumnFilterHash": {
                "UserId": JSON.stringify(users)
            }
        }
    })
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "POST",
            url: "/api/users/get",
            contentType: 'application/json',
            data: data
        }).then(
            function (result) {
                if (addFunc && typeof addFunc === 'function') {
                    // 渡されたオブジェクトが関数なら実行する
                    addFunc(data)
                }
                // 正常終了
                resolve(result)
            },
            function () {
                // エラー
                reject()
            }
        )
    })
}

/**
 * グループ取得APIを呼び出す関数です。
 *
 * @param {Array}     groupIds 取得GroupId
 * @param {Function}  addFunc 最後に実行したい関数
 */
async function commonExportGroup(groupIds, addFunc) {
    let groups = []
    if (Array.isArray(groupIds)) {
        groups = groupIds
    } else {
        groups = [groupIds]
    }
    let data = JSON.stringify({
        "ApiVersion": API_VERSION,
        "View": {
            "ColumnFilterHash": {
                "GroupId": JSON.stringify(groups)
            }
        }
    })
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "POST",
            url: "/api/groups/get",
            contentType: 'application/json',
            data: data
        }).then(
            function (result) {
                if (addFunc && typeof addFunc === 'function') {
                    // 渡されたオブジェクトが関数なら実行する
                    addFunc(data)
                }
                // 正常終了
                resolve(result)
            },
            function () {
                // エラー
                reject()
            }
        )
    })
}

/**
 * 添付項目にExcelを添付し更新する共通関数です。
 *
 * @param {string} targetID 更新対象のアイテムID
 * @param {string} className 添付ファイルのクラス名
 * @param {Object} excel Excelワークブックオブジェクト
 * @param {string} filename 添付ファイルの名前
 */
async function commonUpdateExcel(targetID, className, workbook, filename) {
    const fileBuffer = await workbook.xlsx.writeBuffer()
    const base64 = commonToBase64(fileBuffer)

    let url = `/api/items/${targetID}/update`
    let method_name = "POST"
    let data = {
        "ApiVersion": API_VERSION,
        "AttachmentsHash": {
            [className]: [
                {
                    "ContentType": "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Name": filename,
                    "Base64": base64
                }
            ]
        }
    }
    await $.ajax({
        type: method_name,
        url: url,
        data: JSON.stringify(data),
        contentType: 'application/json',
        dataType: 'json',
        scriptCharset: 'utf-8',
        success: function (data) {
            // Success
            console.log("success")
            console.log(JSON.stringify(data))
        },
        error: function (data) {
            // Error
            console.log("error")
            console.log(JSON.stringify(data))
        }
    })
}

