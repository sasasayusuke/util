(function($){
    // =====================================
    // Results_ClassD を読み取り専用＋スタイル変更
    // =====================================
    document.addEventListener('DOMContentLoaded', function () {
        let select = document.getElementById('Results_ClassD');
        if (!select) return;

        // フォーカス・クリックを無効化
        select.style.pointerEvents = 'none';

        // キーボードフォーカスも防止
        select.tabIndex = -1;

        // 見た目を変更
        select.style.background = '#f5f5f5';
        select.style.border = 'solid 1px silver';
    });

    /* =====================================================
    ＝＝＝＝＝＝＝＝＝
    チェック関数
    ＝＝＝＝＝＝＝＝＝
    目的：
    DOM の 4 つの日付入力（DateC/DateD/DateA/DateB）を参照し、
    ルールに従って select#Results_ClassD の値を自動選択する。

    実行タイミング：
    - onDateChangeTrigger() 内から呼ぶ（推奨）
    - もしくは DOMContentLoaded 時にイベント監視から呼ばれる

    備考：
    - select は pointer-events:none の疑似読み取り専用になっている想定
    - 値はプログラムでセットしてから change イベントを dispatch（必要な他処理が反応するため）
    ===================================================== */
    function checkAndSetClassD() {
        try {
            window.force && console.log('ClassD 自動判定開始');

            // 対象 input を取得
            let elC = document.getElementById('Results_DateC'); // 予定使用日時
            let elD = document.getElementById('Results_DateD'); // 予定返却日時
            let elA = document.getElementById('Results_DateA'); // 使用日時
            let elB = document.getElementById('Results_DateB'); // 返却日時
            let select = document.getElementById('Results_ClassD');

            // 要素が揃っていなければ何もしない
            if (!select) {
                window.force && console.warn('Results_ClassD が見つかりません');
                return;
            }

            // ユーティリティ：空白判定（null/undefined/空文字/半角スペースのみ を空白とみなす）
            function isBlank(v) {
                if (v === null || v === undefined) return true;
                if (typeof v !== 'string') return false;
                return v.trim() === '';
            }

            // 日時パース（"YYYY/MM/DD HH:mm", "YYYY-MM-DD HH:mm:ss", "YYYY/MM/DD" 等を想定）
            function parseDateTime(raw) {
                if (isBlank(raw)) return null;
                var s = String(raw).trim();
                var re = s.match(/^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})(?:[ T](\d{1,2}):(\d{1,2})(?::(\d{1,2}))?)?$/);
                if (!re) {
                    var d = new Date(s);
                    return isNaN(d.getTime()) ? null : d;
                }
                var y = parseInt(re[1], 10);
                var m = parseInt(re[2], 10) - 1;
                var dday = parseInt(re[3], 10);
                var hh = re[4] !== undefined ? parseInt(re[4], 10) : 0;
                var mm = re[5] !== undefined ? parseInt(re[5], 10) : 0;
                var ss = re[6] !== undefined ? parseInt(re[6], 10) : 0;
                return new Date(y, m, dday, hh, mm, ss);
            }

            // 値の取得（原文ストリング）
            let cRaw = elC ? elC.value : '';
            let dRaw = elD ? elD.value : '';
            let aRaw = elA ? elA.value : '';
            let bRaw = elB ? elB.value : '';

            // 日付変換（存在する文字列のみ）
            let cDate = parseDateTime(cRaw);
            let dDate = parseDateTime(dRaw);
            let aDate = parseDateTime(aRaw);
            let bDate = parseDateTime(bRaw);

            // ミリ秒タイムスタンプ（存在する場合）
            let now = Date.now();
            let cTime = cDate ? cDate.getTime() : null;
            let dTime = dDate ? dDate.getTime() : null;
            let aTime = aDate ? aDate.getTime() : null;
            let bTime = bDate ? bDate.getTime() : null;

            // 条件フラグ
            let allBlank = isBlank(cRaw) && isBlank(dRaw) && isBlank(aRaw) && isBlank(bRaw);
            let futurePlanned = (cTime !== null) && (now < cTime); // 現在 < 予定開始（未来予定）
            let plannedButNotUsed = (cTime !== null && dTime !== null) && isBlank(aRaw) && isBlank(bRaw);

            // 判定ロジック（優先度高→低） — あなたの要件に合わせる
            let resultValue = ''; // デフォルトは空

            // 1) 返却済み：返却日時が入力されている場合のみ
            if (bTime !== null) {
                resultValue = '返却済';
            }
            // 2) 使用中（期限超過）: 使用済み・返却未入力・返却予定時刻 < 現在
            else if (aTime !== null && dTime !== null && bTime === null && dTime < now) {
                resultValue = '使用中（期限超過）';
            }
            // 3) 使用中（使用日時が登録され、返却未入力）
            else if (aTime !== null && bTime === null) {
                // 予定期間の有無に関わらず「使用日時がある & 返却未入力」なら使用中
                resultValue = '使用中';
            }
            // 4) 予定期間中（未使用）：予定があり、未使用、かつ現在が期間内（開始 <= now < 終了 ）
            else if (cTime !== null && dTime !== null && isBlank(aRaw) && isBlank(bRaw) && cTime <= now && now <= dTime) {
                resultValue = '予定期間中（未使用）';
            }
            // 5) 空白カテゴリ（未来予定 or planned but not used or all blank）
            else if (allBlank || futurePlanned || plannedButNotUsed) {
                resultValue = ''; // 空文字（select の空値）
            } else {
                // フォールバックは空（安全側）
                resultValue = '';
            }

            // 現在の select の値と異なれば更新する
            if (select.value !== resultValue) {
                // 値をセット（加工は行わず表示用の文字列と一致させる）
                select.value = resultValue;

                // プログラムで値を入れたのち、change を発火して他のスクリプトと整合を取る
                let evt;
                try {
                    evt = new Event('change', { bubbles: true });
                } catch (e) {
                    // 古いブラウザ fallback
                    evt = document.createEvent('HTMLEvents');
                    evt.initEvent('change', true, false);
                }
                select.dispatchEvent(evt);

                window.force && console.log('ClassD を自動設定しました', { value: resultValue });
            } else {
                window.force && console.log('ClassD 変更なし', { value: resultValue });
            }

        } catch (err) {
            // 万一の例外は共通エラーで通知（既存の showCommonError を利用）
            showCommonError(err);
        }
    }


    // =====================================
    // 使用日時・返却日時 変更監視トリガー
    // =====================================
    document.addEventListener('DOMContentLoaded', function () {
        window.force && console.log('監視トリガー初期化');

        let dateA = document.getElementById('Results_DateA');
        let dateB = document.getElementById('Results_DateB');
        let dateC = document.getElementById('Results_DateC');
        let dateD = document.getElementById('Results_DateD');

        if (!dateA || !dateB|| !dateC|| !dateD) {
            window.force && console.log('監視対象inputが見つかりません');
            return;
        }

        // 共通ハンドラ
        function onDateChanged(e) {
            window.force && console.log('日時変更検知', {
                id: e.target.id,
                value: e.target.value
            });

            onDateChangeTrigger();
        }

        // 直接入力
        dateA.addEventListener('input', onDateChanged);
        dateB.addEventListener('input', onDateChanged);
        dateC.addEventListener('input', onDateChanged);
        dateD.addEventListener('input', onDateChanged);

        // カレンダー選択後（フォーカスアウト時）
        dateA.addEventListener('blur', onDateChanged);
        dateB.addEventListener('blur', onDateChanged);
        dateC.addEventListener('blur', onDateChanged);
        dateD.addEventListener('blur', onDateChanged);
    });


    // =====================================
    // トリガー関数（次処理接続用）
    // =====================================
    function onDateChangeTrigger() {
        window.force && console.log('トリガー関数実行');

        checkAndSetClassD();
    }
})(jQuery);