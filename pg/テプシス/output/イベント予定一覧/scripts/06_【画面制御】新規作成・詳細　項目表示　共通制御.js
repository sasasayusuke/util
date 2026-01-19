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
})();