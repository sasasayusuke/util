(function () {

    function getClassJValue() {
        const checked = document.querySelector(
            'input[name="Results_ClassJ"]:checked'
        );
        return checked ? checked.value : null;
    }

    function checkClassJTrigger() {
        const value = getClassJValue();
        if (value === '可能') {
            disableReservationButton(false);
        } else {
            disableReservationButton(true);
        }
    }

    function disableReservationButton(isDisabled) {
        if (typeof reservationList === 'undefined') return;

        const btn = $('button[data-to-site-id="' + reservationList + '"]');
        if (btn.length === 0) return;

        isDisabled ? btn.hide() : btn.show();
    }

    function bind() {
        const radios = document.querySelectorAll(
            'input[name="Results_ClassJ"]'
        );
        if (!radios.length || radios[0].__classJ_bound__) return;

        radios.forEach(radio => {
            radio.addEventListener('change', checkClassJTrigger);
            radio.__classJ_bound__ = true;
        });

        // 初期状態反映
        checkClassJTrigger();
    }

    // 編集画面初期表示
    $p.events.on_editor_load_arr.push(bind);

    // after_set（自動セット・再描画対応）
    if (window.AppTriggers) {
        AppTriggers.registerAfterSet(function () {
            setTimeout(bind, 50);
        });
    }

})();