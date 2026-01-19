(function () {
    // ===================== 照合履歴一行化 =====================

    // 現品照合実施日の取得
    const genpinshogoDate = $p.getControl("現品照合日").text();


    // 監視対象:照合履歴タブのフィールドセット
    const target = document.getElementById('FieldSetHistories');


    // ================================= メイン処理 =================================
    $p.events.on_editor_load_arr.push(function () {
        applyRowVisibility();
    })



    // 照合履歴表示の制御関数
    function applyRowVisibility() {

        // 現品照合日が空白の場合
        if (!genpinshogoDate) {

            window.force && console.log("現品照合実施日が空であるため、履歴の全行非表示処理を実行します。");    // デバッグログ

            // 履歴の全行非表示
            $(".grid.history").hide();

            // 照合履歴がない場合のメッセージ表示
            $('#FieldSetHistories').after('<p class="no-history-message" style="padding: 10px; text-align: left; color: #666;">照合履歴がありません</p>');


        } else {  // 現品照合日が存在する場合

            window.force && console.log("現品照合実施済であるため、1行だけ表示されます。");    // デバッグログ

            // 最新の行以外を非表示にする
            $('#FieldSetHistories .grid-row:not([data-latest="1"])').hide();
        }
    }

    // MutationObserver の設定
    const observer = new MutationObserver(function (mutations) {
        window.force && console.log("MutationObserver 開始");   // デバッグログ
        applyRowVisibility();
    });

    // 画面変化があった時
    if (target) {
        observer.observe(target, {
            childList: true,
            subtree: true
        });
    }
})();