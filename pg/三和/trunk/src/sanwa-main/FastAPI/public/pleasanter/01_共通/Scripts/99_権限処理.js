

const permittionStyleElement = document.createElement('style');

const STATUS_NONE = 0
const STATUS_VIEW = 50
const STATUS_EXEC = 100

permittionStyleElement.textContent = `
    .disabled-navi {
        pointer-events: none;
        opacity: 0.3;
    }
    .unavailable-button {
        opacity: 0.3;
        pointer-events: none;
        cursor: not-allowed;
        background-color: #cccccc !important;
    }
`;
// スタイルを追加
document.head.appendChild(permittionStyleElement);

// ロード時はいったん権限なしで設定
document.querySelectorAll(".nav-site.ui-sortable-handle").forEach(elem => elem.classList.remove('disabled-navi'))

// 権限読み込み
async function loadPermittion(masterTableFlag=false, copyOnFlag=false) {

    // 特権ユーザー
    if ($p.userName() == "Administrator") {
        return
    }

    const filter = {
        "Owner": $p.userId()
    }
        // 権限情報読み込み
    let data = await getData(MASTER_TABLES['権限設定'], [], filter)

    // 権限登録なし
    if (!data || data.length == 0) {
        // HTML要素を全て削除
        document.body.innerHTML = ''

        setTimeout(() => {
            // アラートメッセージの表示
            alert("[AUTH-003] 権限登録が設定されていないので、ホーム画面へリダイレクト致します。\n何度もこの画面が表示されるようであれば、サポートへご連絡ください");
            // OKをクリックするとリダイレクト
            window.location.href = HOME_URL;
        }, 100)
    // 権限登録あり (マスターテーブル系)
    } else if (masterTableFlag) {
        let tableName = getTableName($p.siteId())
        window.permittion = data[0][tableName]
    // 権限登録あり (ダイアログ系)
    } else {
        window.permittion = data[0];
    }

    // マスタテーブル系の閲覧制御
    if (masterTableFlag) {
        // 削除機能
        removeElems = []

        // 管理権限
        if (window.permittion == STATUS_EXEC) {
            removeElems = [
                "EditOutgoingMail",
                "OpenCopyDialogCommand",
            ]

            // コピー機能をONにする（製品マスタのみ）
            if (!copyOnFlag) {
                removeElems.push("ReferenceCopyCommand");
            }
        // 一般権限
        } else if (window.permittion == STATUS_VIEW) {
            removeElems = [
                "Navigations",
                "EditOutgoingMail",
                "OpenCopyDialogCommand",
                "BulkDeleteCommand",
                "OriginBulkDeleteCommand",
                "EditImportSettings",
                "UpdateCommand",
                "DeleteCommand",
                "Process_1",
                "Process_2",
                "EditOnGridCommand",
                "ReferenceCopyCommand",
            ]


        // 権限なし
        } else if (window.permittion == STATUS_NONE) {
            // HTML要素を全て削除
            document.body.innerHTML = ''

            setTimeout(() => {
            // アラートメッセージの表示
                alert("[AUTH-001] 一般権限が設定されていないので、ホーム画面へリダイレクト致します。\n何度もこの画面が表示されるようであれば、サポートへご連絡ください");
                // OKをクリックするとリダイレクト
                window.location.href = HOME_URL;
            }, 100);
        } else {
            // HTML要素を全て削除
            document.body.innerHTML = ''
            setTimeout(() => {
                // アラートメッセージの表示
                alert("[AUTH-002] 設定された権限が不正なので、ホーム画面へリダイレクト致します。\n何度もこの画面が表示されるようであれば、サポートへご連絡ください");
                // OKをクリックするとリダイレクト
                window.location.href = HOME_URL;
            }, 100);
        }
        removeElems.forEach(id => document.getElementById(id)?.remove());

    // ダイアログ系の閲覧制御
    } else {
        let exec_buttons = ["削除", "Excel出力", "戻し"]

        // 権限なしのダッシュボードをクリックできなくする
        Array.from(document.getElementsByClassName("nav-site ui-sortable-handle"))
            .filter(elem => +window.permittion[elem.innerText.split("\n")[0]] < STATUS_VIEW)
            .forEach(elem => elem.classList.add('disabled-navi'))

        // ダッシュボードの権限制御
        document.querySelectorAll(".nav-site.ui-sortable-handle")
            .forEach(elem => {
                if (+window.permittion[elem.innerText.split("\n")[0]] >= STATUS_VIEW) {
                    elem.classList.remove('disabled-navi')
                } else {
                    elem.classList.add('disabled-navi')
                }
            });

        // 権限以下の（100以下）ダイアログ
        Array.from(document.getElementsByClassName("dialog"))
            .filter(d => +window.permittion[d.getAttribute("aria-label")] < STATUS_EXEC)
            .map(d => {
                // その中のカスタマイズボタン
                return Array.from(d.getElementsByClassName("customized"))
                    // その中の実行系ボタン
                    .filter(b => exec_buttons.includes(b.innerText.trim()))
            })
            // クリックできなくする
            .flat()
            .forEach(elem => elem.classList.add('unavailable-button'))


    }
}

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
async function getData(id = $p.siteId(), columns = ["ClassA", "NumA"], filterHash = {}, sorterHash = { "ResultId": "asc" }, filterSearchTypes = {}, keyFlg = true, valueFlg = true, addFunc) {
    let offset = 0
    let data = []
    let temp = {}
    do {
        temp = await getInnner(id, columns, filterHash, sorterHash, filterSearchTypes, keyFlg, valueFlg, offset, addFunc)
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
    async function getInnner(id, columns, filterHash, sorterHash, filterSearchTypes, keyFlg, valueFlg, offset, addFunc) {
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
