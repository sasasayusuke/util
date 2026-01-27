(function () {
    /**
     * アプリ共通の名前空間
     * 他スクリプトはすべてここ経由で after_set に登録する
     */
    window.AppTriggers = window.AppTriggers || {};

    /**
     * after_set に安全に処理を追加するユーティリティ
     * - 既存処理を壊さない
     * - 複数スクリプト共存可能
     */
    AppTriggers.afterSetHandlers = AppTriggers.afterSetHandlers || [];

    /**
     * after_set 登録関数
     */
    AppTriggers.registerAfterSet = function (fn) {
        if (typeof fn === 'function') {
            AppTriggers.afterSetHandlers.push(fn);
        }
    };

    /**
     * Pleasanter after_set を1回だけラップ
     */
    if (window.$p && $p.events && !$p.events.__appTriggersWrapped__) {
        var originalAfterSet = $p.events.after_set;

        $p.events.after_set = function (args) {
            // 既存 after_set を先に実行
            if (typeof originalAfterSet === 'function') {
                try {
                    originalAfterSet(args);
                } catch (e) {
                    console.error('original after_set error', e);
                }
            }

            // 登録されたハンドラを順番に実行
            AppTriggers.afterSetHandlers.forEach(function (handler) {
                try {
                    handler(args);
                } catch (e) {
                    console.error('after_set handler error', e);
                }
            });
        };

        $p.events.__appTriggersWrapped__ = true;
    }

    // =====================================================
    // 主管箇所変更時に管理番号(NumA)にMAX+1をセットする機能
    // =====================================================

    /**
     * 主管箇所(ClassN)の変更を監視し、管理番号(NumA)を自動採番
     */
    function setupAutoNumbering() {
        var classNSelect = document.getElementById('Results_ClassN');
        if (!classNSelect || classNSelect.__classN_autoNumber_bound__) return;

        classNSelect.addEventListener('change', function () {
            var selectedValue = classNSelect.value;
            if (!selectedValue) return;

            fetchMaxNumAAndSet(selectedValue);
        });

        classNSelect.__classN_autoNumber_bound__ = true;
    }

    /**
     * 指定した主管箇所のNumA最大値を取得し、MAX+1をセット
     * @param {string} classNValue - 主管箇所の値
     */
    async function fetchMaxNumAAndSet(classNValue) {
        try {
            var siteId = typeof SuppliesConsumablesID !== 'undefined' ? SuppliesConsumablesID : $p.id();
            var api = new PleasanterAPI(location.origin, { logging: window.force });

            // 全件取得してJSでフィルタリング
            var records = await api.getRecords(siteId, {
                columns: ['NumA', 'ClassN'],
                setLabelText: false,
                setDisplayValue: 'Value'
            });

            // 主管箇所でフィルタリング
            var filtered = records.filter(function (r) {
                return r.ClassN === classNValue;
            });

            // NumAの最大値を取得
            var maxNumA = 0;
            filtered.forEach(function (r) {
                var num = Number(r.NumA) || 0;
                if (num > maxNumA) {
                    maxNumA = num;
                }
            });

            var newNumA = maxNumA + 1;

            var numAInput = document.getElementById('Results_NumA');
            if (numAInput) {
                numAInput.value = newNumA;
                // Pleasanterの変更検知をトリガー
                var event = new Event('change', { bubbles: true });
                numAInput.dispatchEvent(event);
            }

            window.force && console.log('主管箇所「' + classNValue + '」の管理番号MAX:', maxNumA, '→ 新番号:', newNumA);

        } catch (err) {
            console.error('管理番号自動採番エラー:', err);
            window.force && console.log('管理番号自動採番に失敗しました', err);
        }
    }

    // =====================================================
    // 編集画面で取得/移管日(DateA)を読み取り専用にする機能
    // =====================================================

    /**
     * 取得/移管日(DateA)を読み取り専用にする（編集画面のみ）
     */
    function setDateAReadOnly() {
        if ($p.action().toUpperCase() !== 'EDIT') return;

        var dateAField = document.getElementById('Results_DateAField');
        if (!dateAField) return;

        // フィールド全体にreadonly用クラスを付与
        dateAField.classList.add('field-readonly');

        // 入力欄をreadonly
        var dateAInput = document.getElementById('Results_DateA');
        if (dateAInput) {
            dateAInput.setAttribute('readonly', 'readonly');
        }
    }

    // DOMContentLoadedで初期バインド
    document.addEventListener('DOMContentLoaded', function () {
        setupAutoNumbering();
        setDateAReadOnly();
    });

    // after_setでも再バインド（動的に要素が再生成される場合に対応）
    if (window.AppTriggers) {
        AppTriggers.registerAfterSet(function () {
            setTimeout(function () {
                setupAutoNumbering();
                setDateAReadOnly();
            }, 50);
        });
    }
})();