(function () {
    'use strict';

    // ============================
    // replaceText: 実際の無効化処理
    // - #Results_ClassY を編集不可にする（マウス・TAB を無効化）
    // - 見た目のスタイルを指定どおりに上書き
    // - 同じ処理を何度呼んでも副作用が出ないようにする
    // ============================
    function replaceText() {
        try {
            // 要素を取得（jQuery があればそれを優先、なければ document.getElementById）
            var el = (typeof window.$ === 'function') ? window.$('#Results_ClassY').get(0) : document.getElementById('Results_ClassY');
            if (!el) return; // 要素が見つからなければ何もしない

            // 既に適用済みなら再適用は不要（副作用回避）
            if (el.dataset && el.dataset.linkCheckerApplied === '1') {
                // ただしスタイルや属性が崩れている可能性に備えて再設定（安全な範囲のみ）
                try {
                    el.setAttribute('readonly', 'readonly');
                    el.setAttribute('tabindex', '-1');
                    el.setAttribute('aria-disabled', 'true');
                    el.style.color = '#000';
                    el.style.background = '#f5f5f5';
                    el.style.border = 'solid 1px #c0c0c0';
                } catch (e) { /* ignore */ }
                return;
            }

            // 1) 編集を禁止（ユーザーが文字を打ち込めないようにする）
            el.setAttribute('readonly', 'readonly');

            // 2) TABキーでフォーカスが移動しないようにする
            el.setAttribute('tabindex', '-1');

            // 3) 支援技術向けに「無効」であることを示す
            el.setAttribute('aria-disabled', 'true');

            // 4) 見た目を指定どおりにセット
            el.style.color = '#000';
            el.style.background = '#f5f5f5';
            el.style.border = 'solid 1px #c0c0c0';

            // 5) マウスクリックでの編集開始を防ぐ（mousedown のデフォルト動作を抑止）
            //    ※ stopPropagation は行わない（他スクリプトへの影響回避）
            var onMouseDown = function (ev) {
                try { ev.preventDefault(); } catch (e) { /* ignore */ }
            };

            // 6) フォーカスが来たらすぐ外す（念のため）
            var onFocus = function () {
                try { el.blur(); } catch (e) { /* ignore */ }
            };

            // イベントの重複登録を避けるため、フラグを使って一度だけ登録する
            // dataset が使えない環境でも name 属性にマークを残す（互換）
            if (el.dataset) {
                el.dataset.linkCheckerApplied = '1';
            } else {
                el.setAttribute('data-linkchecker-applied', '1');
            }

            // addEventListener で登録（既存リスナは残す）
            el.addEventListener('mousedown', onMouseDown, false);
            el.addEventListener('focus', onFocus, false);

        } catch (err) {
            // 何かあっても影響を他に広げない（ログのみ）
            try { console.error('replaceText error:', err); } catch (e) { /* ignore */ }
        }
    }

    // ============================
    // トリガー登録（ご提示の方式に合わせる）
    // - 初回: $(function(){ replaceText(); })
    // - 保存・更新後: $p.events.after_set を上書きせずラップして呼ぶ
    // ※ jQuery が無い場合は代替で DOMContentLoaded を使い、$p.events が無ければ何もしない
    // ============================

    // 初回（jQuery があればその ready を使う）
    if (typeof window.$ === 'function') {
        try {
            // 指定の書き方を忠実に再現
            $(function () {
                replaceText();
            });
        } catch (e) {
            // 念のため DOMContentLoaded にフォールバック
            document.addEventListener('DOMContentLoaded', replaceText);
        }
    } else {
        // jQuery が無ければ DOMContentLoaded で一度だけ実行
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', replaceText);
        } else {
            replaceText();
        }
    }

    // 保存・更新後のイベント追加（既存の after_set を壊さない）
    if (typeof window.$p !== 'undefined' && $p && $p.events) {
        var originalAfterSet = $p.events.after_set;
        $p.events.after_set = function () {
            // まず元の処理をそのまま実行（あれば）
            if (typeof originalAfterSet === 'function') {
                try { originalAfterSet.apply(this, arguments); } catch (e) { console.error(e); }
            }
            // その後に最小の処理を一度だけ呼ぶ
            replaceText();
        };
    } else {
        // $p.events が無ければ何もしない（ユーザー指定のトリガー条件に忠実）
    }

})();
