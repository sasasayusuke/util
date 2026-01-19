(function () {
    // ============================ 一括現品照合承認 ============================
    // メイン処理
    $p.events.on_grid_load_arr.push(() => {
        genpinshogoShoninButton();
    });


    // 現品照合承認ボタンを追加
    function genpinshogoShoninButton() {
        const $btn = $(
            '<button id="genpinshogoShoninButton" class="sdt-verification__btn fn-verification button button-icon button-positive ui-button ui-corner-all ui-widget applied" style="display:none;" type="button"><span class="ui-button-icon ui-icon ui-icon-disk"></span><span class="ui-button-icon-space"> </span>現品照合承認</button>'
        );

        $("#MainCommands button:last-child").after($btn);

        $btn.on("click", function () {
            genpinshogoShonin();
        });
    }


    // $p.apiGet を Promise 化
    function apiGetPromise(id) {
        return new Promise(function (resolve, reject) {
            $p.apiGet({
                id: id,
                data: {
                    View: { ApiDataType: "KeyValues", ApiColumnKeyDisplayType: "ColumnName", GridColumns: ["DateD"] }
                },
                done: function (res) { resolve(res); },
                fail: function (err) { reject(err); }
            });
        });
    }



    /**
     * チェックされたレコードが全て現品照合済みか確認する（詳細オブジェクトを返す）
     * @param {Array} ids
     * @returns {Promise<{ allDone: boolean, hasUnprocessed: boolean, failedFetchIds: Array<string>, unprocessedIds: Array<string> }>}
     */
    async function isGenpinshogoDone(ids) {
        if (!ids || !Array.isArray(ids) || ids.length === 0) return { allDone: false, hasUnprocessed: false, failedFetchIds: [], unprocessedIds: ids };

        const doneIds = []; // 現品照合済みのIDを格納する配列
        const failedFetchIds = []; // 取得失敗したID
        const unprocessedIds = []; // 取得成功だが未照合のID

        try {
            const results = await Promise.allSettled(ids.map(apiGetPromise));

            for (let i = 0; i < results.length; i++) {
                const r = results[i];

                if (r.status !== "fulfilled") {
                    // 取得失敗は記録しておく
                    failedFetchIds.push(ids[i]);
                    continue;
                }

                const res = r.value;
                const dateVal = res?.Response?.Data?.[0]?.DateD; // 現品照合日(DateD) の取得

                if (dateVal && String(dateVal).trim() !== "") {
                    doneIds.push(ids[i]); // 現品照合日(DateD) が空でなければ登録
                } else {
                    unprocessedIds.push(ids[i]);
                }
            }

            const hasFetchFailures = failedFetchIds.length > 0;
            const hasUnprocessed = unprocessedIds.length > 0;
            const allDone = !hasFetchFailures && !hasUnprocessed && doneIds.length === ids.length;

            return {
                allDone: allDone,
                hasUnprocessed: hasUnprocessed,
                failedFetchIds: failedFetchIds,
                unprocessedIds: unprocessedIds
            };

        } catch (err) {
            console.error("現品照合実施状況確認エラー(isGenpinshogoDone error):", err);
            return { allDone: false, hasUnprocessed: false, failedFetchIds: ids, unprocessedIds: [] };
        }
    }



    // $p.apiUpdate を Promise にラップ
    function apiUpdatePromise(opts) {
        return new Promise(function (resolve, reject) {
            try {
                const ret = $p.apiUpdate(Object.assign({}, opts, {
                    success: function (res) { resolve(res); },
                    error: function (err) { reject(err); }
                }));
                if (ret && typeof ret.then === "function") {
                    ret.then(resolve).catch(reject);
                }
            } catch (err) {
                reject(err);
            }
        });
    }


    // レコードの一括更新（shogoShoninDate を受け取る）
    function updateShoninRireki(ids, shogoShoninDate) {
        const promises = ids.map(function (id) {
            return apiUpdatePromise({
                id: id,
                data: {
                    ClassHash: { ClassH: $p.userName(), ClassP: "承認済" },
                    DateHash: { DateB: shogoShoninDate }
                }
            });
        });
        return Promise.all(promises);
    }


    // 成功メッセージ
    function sendSuccessMessage() {
        $p.setMessage('#Message', JSON.stringify({ Css: "alert-success", Text: "現品照合承認が完了しました。" }));
    }

    // エラーメッセージ
    function sendErrorMessage() {
        $p.setMessage('#Message', JSON.stringify({ Css: "alert-error", Text: "現品照合未実施の項目があります。" }));
    }

    // 取得失敗用メッセージ
    function sendFetchErrorMessage(failedIds) {
        const cnt = Array.isArray(failedIds) ? failedIds.length : 0;
        const text = cnt > 0 ? `レコードの取得に失敗しました（${cnt} 件）。通信状況を確認して再試行してください。` : `レコードの取得に失敗しました。通信状況を確認して再試行してください。`;
        $p.setMessage('#Message', JSON.stringify({ Css: "alert-error", Text: text }));
    }

    // レコード未選択エラーメッセージ
    function sendSelectErrorMessage() {
        $p.setMessage('#Message', JSON.stringify({ Css: "alert-error", Text: "選択中のレコードがありません。" }));
    }

    // 承認日時を作成（フォーマット: YYYY/MM/DD HH:mm:ss）
    function getShogoShoninDate() {
        const d = new Date();
        const z = (n) => (n < 10 ? '0' + n : n);
        return d.getFullYear() + '/' + z(d.getMonth() + 1) + '/' + z(d.getDate()) + ' ' + z(d.getHours()) + ':' + z(d.getMinutes()) + ':' + z(d.getSeconds());
    }

    // 照合承認ボタン押下時の処理
    async function genpinshogoShonin() {
        window.force && console.log("現品照合承認処理開始");    // デバッグログ
        const selectedIds = $p.selectedIds();
        // 選択レコードなしの場合はエラーメッセージを表示して処理中断
        if (!selectedIds || !Array.isArray(selectedIds) || selectedIds.length === 0) {

            sendSelectErrorMessage();
            window.force && console.warn("レコードが選択されていないため、承認処理中断");    // デバッグログ
            return;

        } else {
            window.force && console.log("レコードが選択されています。");    // デバッグログ
        }

        // 選択レコードが現品照合実施済み確認し、全件実施済みなら承認処理
        isGenpinshogoDone(selectedIds).then(async function (result) {
            // result: { allDone: boolean, hasUnprocessed: boolean, failedFetchIds: [], unprocessedIds: [] }
            if (result.failedFetchIds && result.failedFetchIds.length > 0) {
                // 取得失敗がある場合は取得失敗のメッセージを出して中断
                sendFetchErrorMessage(result.failedFetchIds);
                window.force && console.warn("レコード取得失敗のため、承認処理中断。失敗ID:", result.failedFetchIds);
                return;
            }

            if (result.hasUnprocessed) {
                // 未実施のレコードがある場合
                sendErrorMessage();
                window.force && console.warn("現品照合未実施のレコードがあるため、承認処理中断。未実施ID:", result.unprocessedIds);
                return;
            }

            if (result.allDone) {
                // 全件入力済み（TRUE の処理）
                try {

                    window.force && console.log("チェックしたレコードがすべて現品照合実施済みであるか確認");    // デバッグログ
                    const shogoShoninDate = getShogoShoninDate();   // 承認日時取得
                    await updateShoninRireki(selectedIds, shogoShoninDate); // レコード更新
                    sendSuccessMessage();   // 成功メッセージ表示
                    window.force && console.log("現品照合承認成功");    // デバッグログ

                } catch (err) {

                    window.force && console.error("updateShoninRireki failed:", err);
                    alert(`現品照合承認中にエラーが発生しました。${err.message}`);

                }
            }
        });
    }
})();