(function (document, $, $p) {
    "use strict";

    // =====================================================
    // Pleasanter 用スクリプト（IIFE 内に完全カプセル化）
    // - 表示（画面読み込み）時とレコード更新時に動作する
    // - 既存の $p.events ハンドラがあれば保持して併用
    // =====================================================

    // 既存ハンドラを破壊せず安全に追加するユーティリティ
    function bindPEventSafe(eventName, handler) {
        try {
            if (!$p || !$p.events) {
                window.force && console.warn("bindPEventSafe: $p.events が存在しません。イベント登録をスキップします。");
                return;
            }

            var prev = $p.events[eventName];

            $p.events[eventName] = function () {
                var args = arguments;

                // 既存ハンドラがあれば先に呼ぶ（安全に）
                if (typeof prev === "function") {
                    try { prev.apply(this, args); } catch (e) { window.force && console.warn("既存ハンドラ実行中のエラー:", e); }
                }

                // 今回の処理を実行（失敗しても影響を最小化）
                try { handler.apply(this, args); } catch (e) { window.force && console.error("バインドされた handler 実行中のエラー:", e); }
            };
        } catch (e) {
            window.force && console.warn("bindPEventSafe error:", e);
        }
    }

    // =====================================================
    // 関数群（1タスク = 1関数）
    // =====================================================

    // オーケストレーション（取得→解析→比較→描画）
    function sdtWrappedShowSettingsIndex() {
        var deferred = $.Deferred();

        try {
            var loginId = getCurrentLoginId();
            var fetchPromise = fetchSystemAdministratorsView();

            $.when(fetchPromise)
                .done(function (rawData) {
                    var adminArray = normalizeUserArray(rawData);
                    var isAdmin = isLoginInAdminList(loginId, adminArray);

                    if (isAdmin) {
                        showSettingsMenuIfHidden();
                    }

                    deferred.resolve(isAdmin);
                })
                .fail(function (err) {
                    window.force && console.warn("fetchSystemAdministratorsView failed:", err);
                    deferred.reject(err);
                });
        } catch (e) {
            window.force && console.error("sdtWrappedShowSettingsIndex error:", e);
            deferred.reject(e);
        }

        return deferred.promise();
    }

    // ログイン ID 取得（取得だけ）
    function getCurrentLoginId() {
        try {
            if (typeof $p.loginId === "function") {
                return $p.loginId();
            }
            if (typeof $p.userInfo === "function") {
                var ui = $p.userInfo() || {};
                return ui.Id || ui.id || ui.LoginId || ui.loginId || null;
            }
        } catch (e) {
            window.force && console.warn("getCurrentLoginId error:", e);
        }
        return null;
    }

    // 管理者リストを API から取得（取得だけ）
    function fetchSystemAdministratorsView() {
        var deferred = $.Deferred();

        try {
            $p.apiGet({
                id: adminListsID ,
                data: {
                    View: {
                        ApiDataType: "KeyValues",
                        GridColumns: ["ClassA"] // 必要に応じて変更
                    }
                },
                done: function (data) { deferred.resolve(data); },
                fail: function (err) { deferred.reject(err); }
            });
        } catch (e) {
            deferred.reject(e);
        }

        return deferred.promise();
    }

    // rawData を配列に正規化（解析だけ）
    function normalizeUserArray(rawData) {
        if (!rawData || typeof rawData !== "object") return [];
        var list = (rawData.Response && rawData.Response.Data) || rawData.Data || rawData.Items || [];
        return Array.isArray(list) ? list : [];
    }

    // 判定：ログイン ID が 管理者配列 に含まれているか（比較だけ）
    function isLoginInAdminList(loginId, adminArray) {
        if (!loginId) return false;
        if (!Array.isArray(adminArray)) return false;

        return adminArray.some(function (item) {
            if (!item) return false;
            var username = item.Username || item.username || item.UserName || item.User || item.LoginId || item.loginId || null;
            return username === loginId;
        });
    }

    // 描画 / 副作用（表示だけ）
    function showSettingsMenuIfHidden() {
        try {
            var $el = $("#SettingsMenuContainer");

            if (!$el || $el.length === 0) {
                window.force && console.info("showSettingsMenuIfHidden: #SettingsMenuContainer が見つかりません。");
                return;
            }

            if (!$el.is(":visible")) {
                $el.show();
                window.force && console.info("showSettingsMenuIfHidden: 設定メニューを表示しました。");
            } else {
                window.force && console.info("showSettingsMenuIfHidden: 既に表示されています。");
            }
        } catch (e) {
            window.force && console.warn("showSettingsMenuIfHidden error:", e);
        }
    }

    // =====================================================
    // トリガー登録（画面読み込み時 + レコード更新時）
    // - on_grid_load / on_view_load を追随して登録
    // - レコード更新時に動くよう after_set / on_form_save も登録
    // =====================================================
    try {
        bindPEventSafe("on_grid_load", function () {
            sdtWrappedShowSettingsIndex().fail(function () {
                window.force && console.info("sdtWrappedShowSettingsIndex: on_grid_load 実行中にエラーが発生しました。");
            });
        });

        bindPEventSafe("on_view_load", function () {
            sdtWrappedShowSettingsIndex().fail(function () {
                window.force && console.info("sdtWrappedShowSettingsIndex: on_view_load 実行中にエラーが発生しました。");
            });
        });

        // レコード取得・保存後に動くイベント（これを追加）
        bindPEventSafe("after_set", function () {
            sdtWrappedShowSettingsIndex().fail(function () {
                window.force && console.info("sdtWrappedShowSettingsIndex: after_set 実行中にエラーが発生しました。");
            });
        });

        // フォーム保存時のイベント（存在すれば確実にトリガー）
        if ($p && $p.events && ("on_form_save" in $p.events)) {
            bindPEventSafe("on_form_save", function () {
                sdtWrappedShowSettingsIndex().fail(function () {
                    window.force && console.info("sdtWrappedShowSettingsIndex: on_form_save 実行中にエラーが発生しました。");
                });
            });
        }
    } catch (e) {
        window.force && console.warn("トリガー登録時のエラー:", e);
    }

    // ドキュメント読み込み完了時に一度だけ実行（画面読み込み時）
    try {
        $(document).ready(function () {
            sdtWrappedShowSettingsIndex().fail(function () {
                window.force && console.info("sdtWrappedShowSettingsIndex: document.ready 実行中にエラーが発生しました。");
            });
        });
    } catch (e) {
        window.force && console.warn("初期実行時のエラー:", e);
    }

    // モジュール完了（グローバルへの汚染なし）
})(document, jQuery, $p);
