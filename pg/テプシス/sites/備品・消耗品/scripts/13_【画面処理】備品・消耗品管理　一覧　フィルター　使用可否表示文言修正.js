(function($){
    // ---------- 関数1: 表示テキストを置換する ----------
    function replaceDisplayText(container) {
        // ログ（指定形式）
        window.force && console.log('処理開始', { id: 'MainForm', name: '表示置換処理' });

        // container が未指定なら document を対象
        var $root = container ? $(container) : $(document);

        // 対象となる span 要素を走査し、表示のみ置換する
        $root.find('ul.ui-multiselect-checkboxes span').each(function () {
            var $span = $(this);
            var txt = $.trim($span.text());

            if (txt === '可能') {
                $span.text('使用可'); // 表示だけ変更（input.value 等は触らない）
            } else if (txt === '不可') {
                $span.text('使用不可');
            }
            // (未設定) 等は変更しない
        });
    }

    // ---------- 関数2: MutationObserver をセットアップして変更時に処理を呼ぶ ----------
    function setupMainFormObserver() {
        window.force && console.log('処理開始', { id: 'MainForm', name: 'MutationObserverセット' });

        var target = document.getElementById('MainForm');
        if (!target) {
            // #MainForm が存在しない場合はログのみ出して終了（存在するまで待つ設計にしたい場合は別実装）
            window.force && console.log('処理開始', { id: 'MainForm', name: '#MainForm が見つかりません' });
            return;
        }

        var debounceTimer = null;
        var observer = new MutationObserver(function (mutationsList) {
            // 変更をまとめてデバウンス（連続変更で何度も走らせない）
            if (debounceTimer) {
                clearTimeout(debounceTimer);
            }
            debounceTimer = setTimeout(function () {
                replaceDisplayText('#MainForm');
            }, 100); // 100ms デバウンス（必要なら調整）
        });

        observer.observe(target, {
            childList: true,
            subtree: true,
            characterData: true
        });

        // 参照を外部に保持したければ返す（今回は不要）
        return observer;
    }

    // ---------- 関数3: 初期化（画面を開いたタイミングで実行） ----------
    function initReplaceOnLoad() {
        window.force && console.log('処理開始', { id: 'MainForm', name: '初期化処理' });

        // ドキュメント読み込み完了時に初回置換
        $(function () {
            // 初回置換（画面表示時）
            replaceDisplayText(document);

            // #MainForm の変化監視を開始
            setupMainFormObserver();
        });
    }

    // 実行
    initReplaceOnLoad();

})(jQuery);
