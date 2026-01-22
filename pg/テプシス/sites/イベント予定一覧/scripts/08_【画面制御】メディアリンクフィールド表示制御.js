(function () {
    /**
     * メディアリンクフィールド制御
     * Results_CheckB → Results_Source253139Field
     */

    /**
     * トリガー判定
     * CheckB(メディア掲載有無)がの場合
     * メディアリンクフィールドを表示する
     */
    function checkCheckBTrigger(isChecked) {
        if (isChecked) {
            setVisibility(true);
        } else {
            setVisibility(false);
        }
    }


    /**
     * 表示制御
     */
    function setVisibility(isVisible) {
        var field = document.getElementById('Results_Source253139Field');
        if (!field) return;
        field.style.display = isVisible ? 'block' : 'none';
    }


    /**
     * 初期バインド
     */
    function bind() {
        var checkbox = document.getElementById('Results_CheckB');
        if (!checkbox || checkbox.__checkB_bound__) return;

        checkbox.addEventListener('change', function () {
            checkCheckBTrigger(checkbox.checked);
        });

        checkbox.__CheckB_bound__ = true;
        checkCheckBTrigger(checkbox.checked);
    }

    document.addEventListener('DOMContentLoaded', bind);

    if (window.AppTriggers) {
        AppTriggers.registerAfterSet(function () {
            setTimeout(bind, 50);
        });
    }
})();
