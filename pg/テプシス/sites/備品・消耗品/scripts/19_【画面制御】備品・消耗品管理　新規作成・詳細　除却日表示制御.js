(function () {
    /**
     * 除却日制御
     * Results_ClassF → Results_DateE
     */

    /**
     * トリガー判定
     */
    function checkClassFTrigger(value) {
        if (value === '除却') {
            onSelected();
        } else {
            onUnselected();
        }
    }

    /**
     * 除却選択時
     */
    function onSelected() {
        setVisibility(true);
        adjustValidation(true);
        toggleRequiredLabel(true);
    }

    /**
     * 除却解除時
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
        var field = document.getElementById('Results_DateEField');
        if (!field) return;
        field.style.display = isVisible ? 'block' : 'none';
    }

    /**
     * バリデーション制御
     */
    function adjustValidation(isRequired) {
        var el = document.getElementById('Results_DateE');
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
     * <label for="Results_DateE">
     */
    function toggleRequiredLabel(isRequired) {
        var label = document.querySelector('label[for="Results_DateE"]');
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
        var select = document.getElementById('Results_ClassF');
        if (!select || select.__classF_bound__) return;

        select.addEventListener('change', function () {
            checkClassFTrigger(select.value);
        });

        select.__classF_bound__ = true;
        checkClassFTrigger(select.value);
    }

    document.addEventListener('DOMContentLoaded', bind);

    if (window.AppTriggers) {
        AppTriggers.registerAfterSet(function () {
            setTimeout(bind, 50);
        });
    }
})();
