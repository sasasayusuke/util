(function($){

    function replaceText() {
        // ボタン
        $('button[data-from-site-id="' + SuppliesConsumablesID + '"]').each(function() {
            var text = $(this).text();
            if (text.indexOf('予約・使用一覧') !== -1) {
                $(this).text(text.replace('予約・使用一覧', '予約'));
            }
        });

        // タブ
        $('li[aria-controls="FieldSetHistories"] > a.ui-tabs-anchor').each(function() {
            var text = $(this).text();
            if (text.indexOf('変更履歴の一覧') !== -1) {
                $(this).text(text.replace('変更履歴の一覧', '照合履歴'));
            }
        });
    }

    // 初回
    $(function() {
        replaceText();
    });

    // 保存・更新後
    if (window.$p && $p.events) {
        var originalAfterSet = $p.events.after_set;
        $p.events.after_set = function () {
            if (typeof originalAfterSet === 'function') {
                originalAfterSet.apply(this, arguments);
            }
            replaceText();
        };
    }

})(jQuery);
