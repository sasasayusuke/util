(function () {
    // ====================== 照合未実施アラート ======================

    // =============== テーブルのレコードを全件取得する関数 ===============
    async function fetchAllRecords(siteId) {
        const all = [];
        let offset = 0;
        while (true) {
            const res = await new Promise((resolve, reject) => {
                $p.apiGet({
                    id: siteId,
                    data: {
                        Offset: offset,
                        View: {
                            ApiDataType: "KeyValues",
                            ApiColumnKeyDisplayType: "ColumnName",
                            GridColumns: ["ClassK", "ClassN"]
                        }
                    },
                    done: function (e) { 
                        resolve(e);
                        window.force && console.log("備品・消耗品全レコード取得成功");
                    },
                    fail: function(err) {
                        reject(err)
                        window.force && console.log("備品・消耗品全レコード取得失敗", err.message);
                        alert("備品・消耗品全レコード取得に失敗しました。", err.message);
                    }
                });
            });
            const resp = res && res.Response ? res.Response : {};   // apiのレスポンス
            const data = Array.isArray(resp.Data) ? resp.Data : []; // レスポンス内のデータ配列
            const pageSize = resp.PageSize || (data.length || 200); // ページサイズ
            const total = (typeof resp.TotalCount === 'number') ? resp.TotalCount : null; // レコード総件数
            all.push(...data);

            if (data.length === 0) break;   // レスポンスデータが0件だったら処理終了
            if (total !== null && (offset + pageSize) >= total) break;  // 総件数に達したら処理終了
            if (data.length < pageSize) break;  // 取得データが200件サイズ未満なら処理終了
            offset += pageSize; // オフセットを次のページへ
        }
        return all;
    }


    // =============== サイト設定(システム管理者以外は操作不可)テーブルからアラート日、切り替え日を取得 ===============
    function getAlertDateAndChangeData(siteId) {
        return new Promise((resolve, reject) => {
            $p.apiGet({
                id: siteId,
                data: {
                    View: {
                        ApiDataType: "KeyValues",
                        ApiColumnKeyDisplayType: "ColumnName",
                        GridColumns: ["DateA", "DateB"]
                    }
                },
                done: function (r) {
                    const record = r.Response.Data[0] || {};   // apiレスポンスからレコード情報取得
                    const alertDate = record.DateA ? new Date(record.DateA) : null;     // アラート日(DateA)取得
                    const changeDate = record.DateB ? new Date(record.DateB) : null;    // 切替日(DateB)取得
                    window.force && console.log("取得した期間設定データ:", { alertDate, changeDate });
                    resolve({ alertDate, changeDate });
                },
                fail: function (err) {
                    reject(err);
                    window.force && console.log("サイト設定(システム管理者以外は操作不可)テーブルからアラート日・切替日の取得に失敗しました。", err.message)
                    alert("サイト設定からアラート日、切替日の取得に失敗しました。", err.message)
                }
            });
        });
    }


    // =================================== メイン処理 ===================================
    $p.events.on_grid_load_arr.push(async function () {

        try {
            // 期間設定データ取得
            const { alertDate, changeDate } = await getAlertDateAndChangeData(SetPeriodID);

            // 本日日付
            const today = new Date();

            // 期間内判定
            const isInPeriod =
                alertDate &&
                changeDate &&
                today >= alertDate &&
                today <= changeDate;

            window.force && console.log("アラート表示期間内か？:", { today, alertDate, changeDate, isInPeriod });

            // 期間外ならメッセージ表示しないで終了
            if (!isInPeriod) return;

            // 期間内なのでアラート処理へ進む
            const allRecords = await fetchAllRecords($p.id());

            // 現品照合未実施レコードチェック
            // 現品照合結果(ClassK) が空白のレコードを抽出
            const genpinUncheckedRecords = allRecords.filter(item => {
                const value = item.ClassK;
                return value === "" || value === null || value === undefined;
            });

            // ================== メッセージ領域作成 ==================
            let messageDiv = document.getElementById("sdt-message-area");
            if (!messageDiv) {
                messageDiv = document.createElement("div");
                messageDiv.id = "sdt-message-area";
                messageDiv.style.marginBottom = "10px";
                messageDiv.style.cursor = "pointer";
                const container = document.getElementById("ViewModeContainer");
                container.prepend(messageDiv);
            }

            // ================== メッセージ表示 ==================
            const hasUnchecked = genpinUncheckedRecords.length > 0; // 照合未実施のレコードが1つでもある場合はtrue、照合未実施のレコードがない場合はfalse

            // 照合未実施のレコードが1つでもある場合
            if (hasUnchecked) {
                window.force && console.log("照合未実施のレコードがあるため、アラートメッセージ表示処理開始");  // デバッグログ
                messageDiv.innerHTML = `
                <div style="font-weight: bold;">
                    現品照合完了していない <span style="color:red; font-weight: bold;">主管箇所</span> があります。
                </div>
            `;
                
                // モーダル作成処理 
                createModalIfNeeded();

                // アラートメッセージ領域クリックでモーダル表示
                messageDiv.onclick = () => {
                    // モーダル内に表示するリスト領域
                    const listArea = document.getElementById("sdt-modal-list");

                    // 現品照合未実施レコードの主管箇所(ClassN)を重複削除した配列
                    const uniqueList = [...new Set(
                        genpinUncheckedRecords.map(r => r.ClassN)
                    )];

                    // uiqueListを改行区切りでモーダル画面内に表示
                    listArea.textContent = uniqueList.join("\n");
                    document.getElementById("sdt-modal").style.display = "block";
                };
            } else {
                // 現品照合未実施のれこどがない場合は、アラートメッセージを表示しない
                messageDiv.innerHTML = "";
            }

        } catch (err) {
            window.force && console.error("現品照合未実施アラート表示エラー:", err);
            alert("照合未実施アラート処理失敗", err.message);
        }
    });


    // ==================== モーダル生成関数 ====================
    function createModalIfNeeded() {
        if (document.getElementById("sdt-modal")) return;

        const modal = document.createElement("div");
        modal.id = "sdt-modal";
        modal.style.position = "fixed";
        modal.style.top = "0";
        modal.style.left = "0";
        modal.style.width = "100%";
        modal.style.height = "100%";
        modal.style.background = "rgba(0,0,0,0.3)";
        modal.style.display = "none";
        modal.style.zIndex = "9999";

        modal.innerHTML = `
        <div style="
            background: white;
            width: 300px;
            margin: 100px auto;
            padding: 20px;
            border-radius: 6px;
            position: relative;
            box-shadow: 0px 0px 8px rgba(0,0,0,0.3);
        ">
            <div style="text-align:right;">
                <span id="sdt-modal-close" style="cursor:pointer; font-size:20px;">✕</span>
            </div>
            <pre id="sdt-modal-list" style="white-space:pre-wrap; margin-top:10px;"></pre>
        </div>
    `;

        document.body.appendChild(modal);

        document.getElementById("sdt-modal-close").onclick = () => {
            modal.style.display = "none";
        };
    }

})();
