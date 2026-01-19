// ===== トリガー ======
// Pleasanter のグリッド読み込みイベントで一度だけ実行される入口。
// ここでは sdtIndex() を呼ぶだけにしておく（sdtIndex はオーケストレーションを行う）。
$p.events.on_grid_load_arr.push(() => {
    sdtIndex();
});

// ==== index関数 (オーケストレーション) ====
// - 役割: 各一覧取得の Promise を待ち、解析→比較関数に渡して判定結果を受け取る。
// - 戻り値: jQuery.Promise を返す（resolve: boolean or null / reject: err）
function sdtIndex() {
    const deferred = $.Deferred();

    // ログインIDを同期的に取得
    const loginId = getLoginId();

    // 各管理者リストを非同期で取得（取得関数は「取得のみ」を担当）
    const systemAdministratorPromise = sysAdministratorList();
    const businessAdministratorPromise = busAdministratorList();

    // 両方の取得が完了したらパースして比較へ渡す
    $.when(systemAdministratorPromise, businessAdministratorPromise)
        .done(function (sysRawData, busRawData) {
            // 取得した生データを解析して配列に変換（parseUserArray は解析のみ担当）
            const systemAdministrator = parseUserArray(sysRawData);
            const businessAdministrator = parseUserArray(busRawData);

            // 比較関数（比較のみ）で loginId が配列内に存在するかチェック
            const isAdmin = isLoginIdInAdminLists(loginId, systemAdministrator, businessAdministrator);

            // 管理者の時実行
            if (isAdmin) {
                // 承認ボタンの表示
                buttonDisplay()
            }

            deferred.resolve(isAdmin);
        })
        .fail(function (err) {
            deferred.reject(err);
        });

    return deferred.promise();
}

// === getLoginId: ログインID を取得（同期処理） ===
// - 戻り値: 取得できればログインID (string)、取得不可なら null
// - 実装: 環境差を吸収するため $p.loginId() を優先、その後 $p.userInfo() を参照
function getLoginId(){ 
    try {
        if (typeof $p.loginId === "function") {
            return $p.loginId();
        }
        if (typeof $p.userInfo === "function") {
            const ui = $p.userInfo() || {};
            return ui.Id || ui.id || ui.LoginId || ui.loginId || null;
        }
    } catch (e) {
        console.warn("getLoginId error", e);
    }
    return null;
}

// === sysAdministratorList: システム管理者リストを取得（取得のみ） ===
// - 役割: API を叩いて生データを取得し、そのまま resolve する。
// - 戻り値: jQuery.Promise（resolve: rawData, reject: err）
function sysAdministratorList() {
    const deferred = $.Deferred();

    $p.apiGet({
        id: adminListsID,
        data: {
            View: {
                ApiDataType: "KeyValues",
                GridColumns: ["ClassA"]
            }
        },
        done: function (data) {
            deferred.resolve(data);
        },
        fail: function (err) {
            deferred.reject(err);
        }
    });

    return deferred.promise();
}

// === busAdministratorList: 業務管理者リストを取得（取得のみ） ===
// - 役割: API を叩いて生データを取得し、そのまま resolve する。
// - 戻り値: jQuery.Promise（resolve: rawData, reject: err）
function busAdministratorList() {
    const deferred = $.Deferred();

    $p.apiGet({
        id: businessAdministratorID,
        data: {
            View: {
                ApiDataType: "KeyValues",
                GridColumns: ["ClassA"]
            }
        },
        done: function (data) {
            deferred.resolve(data);
        },
        fail: function (err) {
            deferred.reject(err);
        }
    });

    return deferred.promise();
}

// === parseUserArray: API 生データからユーザー配列を取り出す（解析のみ） ===
// - 引数: rawData (object) - API から返ってきた生データ
// - 戻り値: Array - 例: [{ Username: 'oohira' }, ...]（見つからなければ空配列）
// - 想定される構造例: { StatusCode:200, Response:{ Data: [...] } }
function parseUserArray(rawData) {
    if (!rawData || typeof rawData !== "object") return [];
    const list = (rawData.Response && rawData.Response.Data) || rawData.Data || rawData.Items || [];
    return Array.isArray(list) ? list : [];
}

// === isLoginIdInAdminLists: loginId がどちらかの配列に完全一致で存在するか判定（比較のみ） ===
// - 引数: 
//    loginId (string) - チェック対象のログインID
//    systemAdministratorArray (Array) - sys 管理者配列（parseUserArray の戻り値）
//    businessAdministratorArray (Array) - bus 管理者配列（parseUserArray の戻り値）
// - 戻り値: boolean (true: 存在する / false: 存在しない)
// - 注意: この関数は副作用を持たず純粋に判定だけを行う（alert 等は呼び出し元で行う）
function isLoginIdInAdminLists(loginId, systemAdministratorArray, businessAdministratorArray) {
    if (!loginId) return false;

    function arrayContainsLoginId(arr) {
        if (!Array.isArray(arr)) return false;
        return arr.some(function (item) {
            // item.Username が loginId と完全一致するかをチェック
            return item && item.Username === loginId;
        });
    }

    return arrayContainsLoginId(systemAdministratorArray) || arrayContainsLoginId(businessAdministratorArray);
}


// // ===== ボタン表示 =====
function buttonDisplay() {
    $('.fn-verification').css('display', '');
}