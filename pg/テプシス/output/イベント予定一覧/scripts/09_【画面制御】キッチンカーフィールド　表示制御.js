(function () {
    /**
     * キッチンカーリンクフィールド制御
     * Results_ClassA → Results_Source253143Field
     */

    /**
     * トリガー判定
     * ClassA(イベントカテゴリー)が「3:キッチンカー」または「10:キッチンカー（自主取扱）」の場合
     * キッチンカーリンクフィールドを表示する
     */
    function checkClassATrigger(value) {
        if (value === '3' || value === '10') {
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
            checkClassATrigger(select.value);
        });

        select.__classA_bound__ = true;
        checkClassATrigger(select.value);
    }

    document.addEventListener('DOMContentLoaded', bind);

    if (window.AppTriggers) {
        AppTriggers.registerAfterSet(function () {
            setTimeout(bind, 50);
        });
    }
})();
