

/**
 * Plesanterメッセージを利用する関数です。
 * @param {String} type 深刻度
 * @param {String} message メッセージ内容
 */
function commonMessage(message = '', type = 'success') {
    $p.clearMessage()

    switch (type) {
        case 'success':
            $p.setMessage(
                '#Message',
                JSON.stringify({
                    Css: 'alert-success',
                    Text: message
                })
            )
            break
        case 'warning':
            $p.setMessage(
                '#Message',
                JSON.stringify({
                    Css: 'alert-warning',
                    Text: message
                })
            )
            break
        case 'error':
            $p.setMessage(
                '#Message',
                JSON.stringify({
                    Css: 'alert-error',
                    Text: message
                })
            )
            break
        default:
            throw new Error(message)

    }
}
/**
 * 入力されたラベルのIDを返却する。
 * @param {String} label ラベル
 */
function commonGetId(label, prefix = true, suffix = false) {
    try {
        let id = $p.getColumnName(label)
        if (commonIsNull(id)) {
            let message = `共通関数commonGetId：ラベル不正。${label}`
            commonMessage(message, 'error')
            throw new Error(message)
        } else {
            id = prefix ? $p.tableName() + '_' + id : id
            id = suffix ? id + 'Field' : id
        }
        return id
    } catch (err) {
        // 再スロー
        throw err
    }

}

/**
 * Null判定する関数です。
 * @param {object} obj オブジェクト
 *
 * @return {boolean} 判定結果
 */
function commonIsNull(obj) {
    if (Array.isArray(obj)) {
        return obj.filter(v => String(v).trim() !== '').length == 0
    } else if (typeof obj === 'object') {
        return !obj || Object.keys(obj).length === 0 && obj.constructor === Object
    } else {
        return !obj && obj !== 0 || String(obj).trim() == ''
    }
}

/**
 * 入力されたラベルに一致する項目の選択した値を返却する。
 * @param {String} label ラベル
 * @param {String} flg true: value false: text
 */
function commonGetVal(label, valueFlg = false) {
    let value = ""
    try {
        let tagName = $p.getControl($p.getColumnName(label)).prop("tagName")
        if (tagName === "SELECT") {
            if ($p.getControl($p.getColumnName(label)).attr("multiple")) {
                value = valueFlg ? $p.getControl($p.getColumnName(label)).val() : $p.getControl($p.getColumnName(label)).next().children().last().text()
            } else {
                value = valueFlg ? $p.getControl($p.getColumnName(label)).children(':selected').val() : $p.getControl($p.getColumnName(label)).children(':selected').text()
            }
        } else if (tagName === "INPUT") {
            if (commonGetId(label).indexOf("Check") > 0) {
                value = document.getElementById(commonGetId(label)).checked
                value = valueFlg ? +value : value
            } else {
                // 選択系 読み取り専用
                value = valueFlg ? $p.getControl($p.getColumnName(label)).attr('data-value') : $p.getControl($p.getColumnName(label))[0].innerHTML
                if (commonIsNull(value)) {
                    // 選択系以外
                    value = $p.getControl($p.getColumnName(label)).val()
                }
            }
        } else if (tagName === "TEXTAREA") {
            value = document.getElementById(commonGetId(label) + ".viewer").innerText
        } else {
            // 選択系 読み取り専用
            value = valueFlg ? ($p.getControl($p.getColumnName(label)).attr('data-value') ?? $p.getControl($p.getColumnName(label))[0].innerHTML) : $p.getControl($p.getColumnName(label))[0].innerHTML
            if (commonIsNull(value)) {
                // 選択系以外
                value = $p.getControl($p.getColumnName(label)).val()
            }
        }
    } catch (e) {
        console.log(label)
        console.log(e)
        value = ""
    } finally {
        try {
            // JSON.parseを試みる
            return typeof value === 'string' ? JSON.parse(value) : value
        } catch (error) {
            // エラーが発生した場合、元の値を返す
            return value;
        }
    }
}

/**
 * ローディングオーバーレイを表示する共通関数
 * @param {String} title タイトル（デフォルト: '処理中...'）
 * @param {String} message メッセージ（デフォルト: '処理を実行しています'）
 * @param {Boolean} darkMode 濃い半透明にするかどうか（デフォルト: false）
 */
function commonShowLoadingOverlay(title = '処理中...', message = '処理を実行しています', darkMode = false) {
    if ($('#loading-overlay').length > 0) return; // 既に表示されている場合は何もしない

    const overlayHTML = `
        <div id="loading-overlay" class="loading-overlay">
            <div class="loading-content">
                <div class="spinner-large"></div>
                <h3>${title}</h3>
                <p>${message}</p>
                <p class="loading-note">しばらくお待ちください</p>
            </div>
        </div>

        <style>
            .loading-overlay {
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: ${darkMode ? 'rgba(0, 0, 0, 0.7)' : 'rgba(0, 0, 0, 0.3)'};
                ${darkMode ? 'backdrop-filter: blur(5px);' : ''}
                display: flex;
                justify-content: center;
                align-items: center;
                z-index: 9999;
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            }

            .loading-content {
                background: white;
                padding: 3rem;
                border-radius: 20px;
                text-align: center;
                box-shadow: 0 20px 40px rgba(0, 0, 0, 0.3);
                max-width: 400px;
                width: 90%;
            }

            .spinner-large {
                width: 80px;
                height: 80px;
                border: 6px solid #f3f3f3;
                border-top: 6px solid #3498db;
                border-radius: 50%;
                margin: 0 auto 2rem;
                animation: spin-large 1s linear infinite;
            }

            @keyframes spin-large {
                0% { transform: rotate(0deg); }
                100% { transform: rotate(360deg); }
            }

            .loading-content h3 {
                color: #333;
                margin: 0 0 1rem;
                font-size: 1.5rem;
            }

            .loading-content p {
                color: #666;
                margin: 0.5rem 0;
                line-height: 1.5;
            }

            .loading-note {
                font-size: 0.9rem;
                opacity: 0.8;
            }
        </style>
    `;

    $('body').append(overlayHTML);
}

/**
 * ローディングオーバーレイを非表示にする共通関数
 */
function commonHideLoadingOverlay() {
    $('#loading-overlay').fadeOut(300, function() {
        $(this).remove();
    });
}

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
 * 複数行文字列の各行の先頭にあるスペース・タブなどの空白文字を削除します。
 *
 * 主にテンプレートリテラルで記述された文字列の整形に使用します。
 * テンプレートの見た目を整えつつ、出力では余分なインデントを除去したい場面に便利です。
 *
 * @param {string} text - 整形対象の複数行文字列
 * @returns {string} 各行の先頭空白を削除した文字列
 *
 * @example
 * const raw = `
 *     Hello
 *     World
 * `;
 * const result = commonTrimLines(raw);
 * // "Hello\nWorld"
 */
function commonTrimLines(text) {
    // 各行の先頭スペース数を取得（0は除外）
    const spaceCounts = text
        .split('\n')
        .map(line => line.length - line.trimStart().length)
        .filter(count => count > 0);

    // 最小スペース数を取得
    const minSpaces = spaceCounts.length > 0
        ? Math.min(...spaceCounts)
        : 0;

    // 最小スペース数分だけ各行から削除
    return text
        .split('\n')
        .map(line => line.slice(minSpaces))
        .join('\n');
}