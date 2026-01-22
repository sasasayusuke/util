(function () {
    /**
     * キッチンカーリンクフィールド制御
     * Results_ClassA → Results_Source253143Field
     */

    /**
     * トリガー判定
     * ClassA(イベントカテゴリー)が
     * 「キッチンカー」または「キッチンカー（自主取扱）」の場合
     */
    function checkClassATrigger(select) {
        if (!select) return;

        // 選択中 option の表示文字列を取得
        var text = select.options[select.selectedIndex]?.text || '';

        
        if (
            text === 'キッチンカー' ||
            text === 'キッチンカー（自主取扱）'
        ) {
            setVisibility(true);
        } else {
            setVisibility(false);
        }
    }

    /**
     * 表示制御
     */
    function setVisibility(isVisible) {
        var field = document.getElementById('Results_Source253143Field');
        if (!field) return;
        field.style.display = isVisible ? 'block' : 'none';
    }

    /**
     * 初期バインド
     */
    function bind() {
        var select = document.getElementById('Results_ClassA');
        if (!select || select.__classA_bound__) return;

        select.addEventListener('change', function () {
            checkClassATrigger(select);
        });

        select.__classA_bound__ = true;

        // 初期表示判定
        checkClassATrigger(select);
    }

    document.addEventListener('DOMContentLoaded', bind);

    if (window.AppTriggers) {
        AppTriggers.registerAfterSet(function () {
            setTimeout(bind, 50);
        });
    }
})();
