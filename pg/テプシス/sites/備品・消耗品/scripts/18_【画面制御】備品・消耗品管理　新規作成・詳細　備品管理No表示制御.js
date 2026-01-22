(function () {
    /**
     * 管理No制御
     * Results_ClassA → Results_NumB
     */

    /**
     * トリガー判定
     */
    function checkClassATrigger(value) {
        if (value === '備品') {
            onSelected();
        } else {
            onUnselected();
        }
    }

    /**
     * 備品選択時
     */
    function onSelected() {
        setVisibility(true);
        adjustValidation(true);
        toggleRequiredLabel(true);
    }

    /**
     * 備品以外
     */
    function onUnselected() {
        setVisibility(false);
        adjustValidation(false);
        toggleRequiredLabel(false);
    }

    /**
     * 表示制御
     */
    function setVisibility(isVisible) {
        var field = document.getElementById('Results_NumBField');
        if (!field) return;
        field.style.display = isVisible ? 'block' : 'none';
    }

    /**
     * バリデーション制御
     */
    function adjustValidation(isRequired) {
        var el = document.getElementById('Results_NumB');
        if (!el) return;

        if (isRequired) {
            el.removeAttribute('data-validate-date');
            el.setAttribute('data-validate-required', '1');
        } else {
            el.removeAttribute('data-validate-required');
            el.setAttribute('data-validate-date', '1');
        }
    }

    /**
     * ラベルの required クラス制御
     * <label for="Results_NumB">
     */
    function toggleRequiredLabel(isRequired) {
        var label = document.querySelector('label[for="Results_NumB"]');
        if (!label) return;

        if (isRequired) {
            label.classList.add('required');
        } else {
            label.classList.remove('required');
        }
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
