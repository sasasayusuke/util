(function () {
    // ============================ 一括現品照合 ============================ 

    let selectedIds

    // ============================ メイン処理 ============================ 
    $p.events.on_grid_load_arr.push(function () {
        genpinshogoButton();
    })



    // =================================== 現品照合ボタンを追加 ===================================
    function genpinshogoButton() {
        // 現品照合ボタンのhtml
        const $btn = $('<button id="genpinshogoButton" class="button button-icon button-positive ui-button ui-corner-all ui-widget applied" type="button"><span class="ui-button-icon ui-icon ui-icon-disk"></span><span class="ui-button-icon-space"> </span>現品照合</button>');

        // Maincommandsの一番最後のボタンの次に、現品照合ボタンを作成
        $("#MainCommands button:last-child").after($btn);

        // 現品照合ボタンのクリックイベント：モーダルを表示
        $btn.on("click", function (e) {
            e.preventDefault();
            openBulkMatchModal();
        });
    }


    // =================================== モーダルを表示する関数（現品照合ボタンの onclick に割り当てる）===================================
    function openBulkMatchModal() {
        if ($("#bulkMatchModalOverlay").length) return;

        // モーダル画面のhtml
        const modalHtml = `
    <style id="bulkMatchModalStyles">
      #bulkMatchModalOverlay { position: fixed; inset: 0; background: rgba(0,0,0,0.4); display: flex; align-items: center; justify-content: center; z-index: 9999; }
      #bulkMatchModal { background: #fff; width: 520px; max-width: 92%; border-radius: 6px; box-shadow: 0 8px 24px rgba(0,0,0,0.2); position: relative; padding: 18px 20px; font-family: "Segoe UI", Roboto, "Helvetica Neue", Arial; }
      #bulkMatchModal .closeBtn { position: absolute; right: 10px; top: 8px; border: none; background: transparent; font-size: 20px; cursor: pointer; }
      #bulkMatchModal h2 { margin: 4px 0 12px 0; font-size: 16px; }
      #bulkMatchModal .field { margin-bottom: 10px; }
      #bulkMatchModal label { display:block; margin-bottom:4px; font-size:13px; }
      #bulkMatchModal select, #bulkMatchModal input[type="date"], #bulkMatchModal textarea { width:100%; padding:8px; box-sizing: border-box; font-size:13px; border:1px solid #ccc; border-radius:4px; }
      #bulkMatchModal textarea { resize: vertical; min-height:70px; }
      #bulkMatchModal .actions { text-align: right; margin-top:12px; }
      #bulkMatchModal .actions button { margin-left:8px; padding:6px 12px; }
    </style>

    <div id="bulkMatchModalOverlay">
      <div id="bulkMatchModal" role="dialog" aria-modal="true" aria-labelledby="bulkMatchTitle">
        <button class="closeBtn" title="閉じる" aria-label="閉じる">&times;</button>
        <h2 id="bulkMatchTitle">現品照合</h2>

        <div class="fielsd">
          <label for="bm_result">照合結果 <span style="color:#d00">*</span></label>
          <select id="bm_result" required>
            <option value="OK">OK</option>
            <option value="NG">NG</option>
            <option value="保留">保留</option>
          </select>
        </div>

        <div class="field">
          <label for="bm_date">照合日 <span style="color:#d00">*</span></label>
          <input id="bm_date" type="date" min="1900-01-01" max="9999-12-31" required/>
        </div>

        <div class="field">
          <label for="bm_comment">コメント</label>
          <textarea id="bm_comment" placeholder="コメントを入力"></textarea>
        </div>

        <div class="actions">
          <button id="bm_submit" type="button" class="button button-positive">照合</button>
        </div>
      </div>
    </div>
    `;

        $("body").append(modalHtml);

        // 初期フォーカス
        $("#bm_result").focus();

        // 照合日の選択可能期間を設定 & 初期値補正
        setDateRange();

        // 照合日はカレンダー選択のみ（キーボード入力禁止）
        $("#bm_date")
            .on("keydown", function (e) {
                e.preventDefault();
            })
            .on("paste", function (e) {
                e.preventDefault();
            });

        // モーダルを閉じる処理
        function closeModal() {
            $("#bulkMatchModalOverlay").remove();
            $("#bulkMatchModalStyles").remove();
            $(document).off("keydown.bulkMatch");
        }

        // × ボタンでモーダルを閉じる
        $("#bulkMatchModal .closeBtn").on("click", function (e) {
            e.preventDefault();
            closeModal();
        });

        // モーダルの外側クリックでモーダル画面を閉じる
        $("#bulkMatchModalOverlay").on("click", function (e) {
            if (e.target.id === "bulkMatchModalOverlay") closeModal();
        });

        // Escキー押下でモーダル画面を閉じる
        $(document).on("keydown.bulkMatch", function (e) {
            if (e.key === "Escape") closeModal();
        });

        // 照合ボタン押下時の処理
        $("#bm_submit").on("click", async function () {
            const shogoResult = $("#bm_result").val();   // 照合結果
            const shogoDate = $("#bm_date").val();      // 照合日
            const shogoComment = $("#bm_comment").val();    // 照合コメント
            const ids = $p.selectedIds() || []; // 選択されているレコードIDの配列

            // レコードを選択のバリデーション
            window.force && console.log("レコード選択チェック開始");    // デバッグログ
            if (!ids.length) {
                alert("選択中のレコードがありません。");
                return;
            } else {
                window.force && console.log("レコードが選択されています。");    // デバッグログ
            }

            // 照合結果のバリデーション
            window.force && console.log("照合結果入力チェック");    // デバッグログ
            if (!shogoResult) {
                window.force && console.warn("注意：照合結果が未入力です");
                alert("照合結果を選択してください。");
                $("#bm_result").focus();
                return;
            } else {
                window.force && console.log("照合結果入力済み");    // デバッグログ
            }


            // 照合日のバリデーション
            window.force && console.log("照合日入力チェック開始");  // デバッグログ
            if (!shogoDate) {
                window.force && console.warn("注意：照合日が未入力です");
                alert("照合日を入力してください。");
                $("#bm_date").focus();
                return;
            } else {
                window.force && console.log("照合日入力済み");  // デバッグログ
            }

            // 二重送信防止・処理中表示
            const $submit = $("#bm_submit");
            $submit.prop("disabled", true).text("処理中...");

            try {
                // レコードの一括更新が全て成功してからメッセージ表示
                await updateShogoRireki(ids, shogoResult, shogoDate, shogoComment);

                // 現品照合完了メッセージ表示（すべて成功した場合のみ）
                sendSuccessMessage();

                // 照合完了したら、モーダルを閉じる
                closeModal();

                // // 少し待ってから画面リロード（メッセージを一瞬表示させるため）
                // setTimeout(function () { location.reload(); }, 800);

            } catch (err) {
                console.error("updateShogoRireki failed:", err);
                alert("更新に失敗しました。詳細はコンソールを確認してください。");
            } finally {
                $submit.prop("disabled", false).text("照合");
            }
        });
    }


    // =================================== 照合日の選択可能期間を設定する関数 =================================== 
    function setDateRange() {
        const $date = $("#bm_date");
        if (!$date.length) return;

        const today = new Date();
        const year = today.getFullYear();
        const month = today.getMonth() + 1; // 1-12
        const day = today.getDate();

        let startYear;
        let endYear;

        // 1/1〜4/9 → 前年4/10 〜 当年4/9
        if (month < 4 || (month === 4 && day <= 9)) {
            startYear = year - 1;
            endYear = year;
        }
        // 4/10〜12/31 → 当年4/10 〜 翌年4/9
        else {
            startYear = year;
            endYear = year + 1;
        }

        const minDate = `${startYear}-04-10`;
        const maxDate = `${endYear}-04-09`;

        $date.attr("min", minDate);
        $date.attr("max", maxDate);

        // 既存値が範囲外なら今日 or min に補正
        const currentVal = $date.val();
        if (!currentVal || currentVal < minDate || currentVal > maxDate) {
            const todayStr = today.toISOString().slice(0, 10);
            if (todayStr >= minDate && todayStr <= maxDate) {
                $date.val(todayStr);
            } else {
                $date.val(minDate);
            }
        }
    }


    // =================================== $p.apiUpdateをPromiseにラップする関数 ===================================
    function apiUpdatePromise(opts) {
        return new Promise(function (resolve, reject) {
            try {
                const ret = $p.apiUpdate(Object.assign({}, opts, {
                    success: function (res) { resolve(res); },
                    error: function (err) { reject(err); }
                }));
                // apiUpdate が Promise を返す場合はそれも利用
                if (ret && typeof ret.then === "function") {
                    ret.then(resolve).catch(reject);
                }
            } catch (err) {
                reject(err);
            }
        });
    }


    // =================================== レコードの一括更新（Promise を返す）=================================== 
    function updateShogoRireki(ids, shogoResult, shogoDate, shogoComment) {
        // 各IDの ClassH を取得してから条件分岐で更新
        const classHPromises = ids.map(function (id) {
            return new Promise(function (resolve, reject) {
                $p.apiGet({
                    id: id,
                    data: {
                        View: {
                            ApiDataType: 'KeyValues',
                            GridColumns: ["ClassH"],
                            ApiColumnKeyDisplayType: "ColumnName"
                        }
                    },
                    done: function (response) {
                        const classH = response.Response.Data[0].ClassH || "";
                        resolve({ id: id, classH: classH });
                    },
                    error: function (err) {
                        reject(err);
                    }
                });
            });
        });

        // 全ての 照合承認者(ClassH) 取得完了後に更新処理を実行
        return Promise.all(classHPromises).then(function (classHResults) {
            const updatePromises = classHResults.map(function (result) {
                const id = result.id;
                const classH = result.classH;
                let updateData;

                // 1. 承認済みの備品である場合、現品照合日 /現品照合結果 / 現品照合コメント / 現品照合実施者が入力される
                if (!classH || classH.trim() === "") {

                    window.force && console.log("承認済みではない備品の更新処理開始");  // デバッグログ

                    // 照合承認者(ClassH) が空白 -> 従来通りの更新（ClassH/DateB は触らない）
                    updateData = {
                        ClassHash: {
                            ClassK: shogoResult,
                            ClassG: shogoComment,
                            ClassI: $p.userName(),
                        },
                        DateHash: {
                            DateD: shogoDate,
                        }
                    };

                } else { // 2. 承認済みの備品である場合、現品照合日 /現品照合結果 / 現品照合コメント / 現品照合実施者 に加え、照合承認者 / 照合切替日 をクリアする

                    window.force && console.log("承認済みの備品の更新処理開始");  // デバッグログ

                    // 照合承認者(ClassH) に値がある -> ClassH と DateB を空白にして更新
                    updateData = {
                        ClassHash: {
                            ClassK: shogoResult,
                            ClassG: shogoComment,
                            ClassI: $p.userName(),
                            ClassH: "",
                            ClassP: "未承認"
                        },
                        DateHash: {
                            DateD: shogoDate,
                            DateB: "1899/12/31",
                        }
                    };
                }

                return apiUpdatePromise({
                    id: id,
                    data: updateData
                });
            });

            return Promise.all(updatePromises);
        });
    }

    // =================================== 現品照合完了メッセージ表示 =================================== 
    function sendSuccessMessage() {
        $p.setMessage('#Message', JSON.stringify({
            Css: "alert-success",
            Text: "現品照合が完了しました。"
        }));
    }
})();
