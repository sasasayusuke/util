(function () {
    'use strict';

    // ------------------------------------------------------------------
    // LinkChecker（最適化版）
    // - 挙動は変更せず、以下を反映：
    //   ・プルダウンは店舗名（ClassA）のみ表示（ResultId は表示しない）
    //   ・店舗名 input を直接編集できないようにする（readonly）
    // ------------------------------------------------------------------

    // ---------- jQuery 安全参照 ----------
    var $jq = (typeof window !== 'undefined' && typeof window.jQuery !== 'undefined')
        ? window.jQuery
        : (typeof window !== 'undefined' && typeof window.$ === 'function' ? window.$ : null);

    if ($jq) {
        window.force && console.info('LinkChecker: using jQuery (version ' + ($jq.fn && $jq.fn.jquery ? $jq.fn.jquery : 'unknown') + ')');
    } else {
        window.force && console.info('LinkChecker: jQuery not available, using vanilla DOM API as fallback.');
    }

    // ---------- メイン（画面読み込みなどで実行される入口） ----------
    function processFlow() {
        var siteId = event_Site_Id;

        // テーブルがあるか確認（jQuery優先、なければバニラDOM）
        var tableElem = null;
        if ($jq) {
            var $table = $jq('table[data-id="' + siteId + '"]').first();
            tableElem = ($table && $table.length) ? $table.get(0) : null;
        } else {
            tableElem = document.querySelector('table[data-id="' + siteId + '"]');
        }

        if (tableElem) {
            var ids = getResultIdsFromTable(tableElem);
            handleFoundResultIds(ids);
        } else {
            // テーブルが無ければ URL の LinkId を探して処理
            var linkId = getQueryParamFromLocation('LinkId');
            if (linkId) {
                handleFoundLinkId(linkId);
            } else {
                window.force && console.info('LinkChecker: 対象テーブル／LinkId が見つかりませんでした。event_Site_Id=' + siteId);
            }
        }
    }

    // ---------- テーブルから ResultId 列の値を取り出す ----------
    // tableElem: HTMLTableElement を想定
    function getResultIdsFromTable(tableElem) {
        var ids = [];
        if (!tableElem) return ids;

        if ($jq) {
            var $table = $jq(tableElem);
            var $th = $table.find('thead tr th[data-name="ResultId"]').first();
            if (!$th || !$th.length) {
                window.force && console.warn('LinkChecker: th[data-name="ResultId"] が見つかりません。');
                return ids;
            }
            var colIndex = $th.index();

            $table.find('tbody tr').each(function () {
                var $tr = $jq(this);
                var $td = $tr.find('td').eq(colIndex);
                if (!$td || !$td.length) return;
                var text = $td.text().trim();
                if (text) {
                    ids.push(text);
                } else {
                    var $a = $td.find('a').first();
                    if ($a && $a.length) {
                        var href = $a.attr('href') || '';
                        var fromHref = extractIdFromHref(href);
                        if (fromHref) ids.push(fromHref);
                    }
                }
            });
        } else {
            var thead = tableElem.querySelector('thead');
            if (!thead) {
                window.force && console.warn('LinkChecker: thead が見つかりません。');
                return ids;
            }
            var th = thead.querySelector('tr th[data-name="ResultId"]');
            if (!th) {
                window.force && console.warn('LinkChecker: th[data-name="ResultId"] が見つかりません。');
                return ids;
            }
            var headers = Array.prototype.slice.call(th.parentNode.children);
            var colIndex = headers.indexOf(th);

            var rows = tableElem.querySelectorAll('tbody tr');
            Array.prototype.forEach.call(rows, function (row) {
                var tds = row.querySelectorAll('td');
                var td = tds[colIndex];
                if (!td) return;
                var text = (td.textContent || '').trim();
                if (text) {
                    ids.push(text);
                } else {
                    var a = td.querySelector('a');
                    if (a) {
                        var href = a.getAttribute('href') || '';
                        var fromHref = extractIdFromHref(href);
                        if (fromHref) ids.push(fromHref);
                    }
                }
            });
        }

        // 重複を取り除く（順序は保持）
        ids = ids.filter(function (v, i, self) { return self.indexOf(v) === i; });
        return ids;
    }

    // ---------- href 文字列から id を抜き出す（/items/1234 や ?LinkId=1234） ----------
    function extractIdFromHref(href) {
        if (!href) return null;
        var m = href.match(/\/items\/(\d+)/);
        if (m && m[1]) return m[1];
        var searchMatch = href.match(/[?&]LinkId=(\d+)/);
        if (searchMatch && searchMatch[1]) return searchMatch[1];
        return null;
    }

    // ---------- current URL からクエリパラメータを取る（安全に） ----------
    function getQueryParamFromLocation(paramName) {
        try {
            var url = window.location.href || '';
            var query = url.split('?')[1] || '';
            if (!query) return null;
            var searchParams = new URLSearchParams(query);
            return searchParams.get(paramName);
        } catch (e) {
            var re = new RegExp('[?&]' + paramName + '=([^&]+)');
            var m = window.location.href.match(re);
            return m ? decodeURIComponent(m[1]) : null;
        }
    }

    // ---------- テーブル抽出結果を受け取ったときの処理 ----------
    function handleFoundResultIds(idsArray) {
        window.force && console.log('LinkChecker: テーブルから取得した ResultId 一覧:', idsArray);
        if (idsArray && idsArray.length) {
            // 最初の ID を使って API を叩く（既存ロジックを維持）
            sdtApifunction(idsArray[0]);
        }
    }

    function handleFoundLinkId(linkId) {
        window.force && console.log('LinkChecker: URL の LinkId:', linkId);
        if (linkId) {
            sdtApifunction(linkId);
        }
    }

    // ---------- API 呼び出し周り（最適化した sdtApifunction） ----------
    function sdtApifunction(saiteID) {
        window.force && console.log('index開始');

        // Promise の完了／失敗どちらでも decLoading を呼ぶ構成にする
        fetchSiteKeyValues(saiteID).then(function (data) {
            try {
                // incLoading があれば呼ぶ（無ければ無視）
                try { incLoading(); } catch (e) { /* 無ければスキップ */ }

                // ここで取得した data がこの処理の主役
                window.force && console.log('次の処理', data);
                window.force && console.log('使用状況バッジ処理開始', { fn: 'sdtIndex' });

                // ---------- 取得データを使って UI に選択肢を出す処理 ----------
                // data.Response.Data を元に、#Results_ClassA をクリックした時に選択肢を出す。
                (function () {
                    // jQuery 有無を判定して操作関数を揃える
                    var $local = (typeof $jq !== 'undefined' && $jq) ? $jq : (typeof window.jQuery !== 'undefined' ? window.jQuery : null);
                    var useJq = !!$local;

                    // 簡易 DOM 取得関数（jQuery の時は jQuery オブジェクトを返す）
                    function getEl(selector) { return useJq ? $local(selector) : document.querySelector(selector); }
                    function createEl(tag) { return document.createElement(tag); }

                    // 取得データを安全に取り出す
                    var items = [];
                    try { items = (data && data.Response && Array.isArray(data.Response.Data)) ? data.Response.Data : []; } catch (e) { items = []; }

                    // 対象の入力要素セレクタ（画面の構造に合わせる）
                    var INPUT_A_SELECTOR = '#Results_ClassA'; // 店舗名（表示）
                    var INPUT_Y_SELECTOR = '#Results_ClassY'; // イベント店舗ID（実体ID格納）

                    var inputA = getEl(INPUT_A_SELECTOR);
                    var inputY = getEl(INPUT_Y_SELECTOR);

                    // jQuery の場合は DOM 要素を取り出す（操作は両方対応）
                    var inputAEl = inputA ? (useJq ? inputA.get(0) : inputA) : null;
                    var inputYEl = inputY ? (useJq ? inputY.get(0) : inputY) : null;

                    if (!inputAEl || !inputYEl) {
                        window.force && console.warn('LinkChecker: Results_ClassA または Results_ClassY の input が見つかりません。処理を中止します。');
                        return;
                    }

                    // ★ 要求どおり：店舗名入力欄を直接編集できないようにする（readonly）
                    try {
                        inputAEl.setAttribute('readonly', 'readonly');
                        // 見た目で入力できないことをわかりやすくするため、フォーカス時に選択肢が出るようにする
                        inputAEl.style.cursor = 'pointer';
                    } catch (e) {
                        /* 属性設定が失敗しても動作自体は継続 */
                    }

                    // items をラベル／id の配列に変換し、空や不正なものは除外する
                    var normalized = items.map(function (it) {
                        return {
                            label: (it && typeof it.ClassA !== 'undefined') ? String(it.ClassA) : '',
                            id: (it && typeof it.ResultId !== 'undefined') ? String(it.ResultId) : ''
                        };
                    }).filter(function (it) {
                        return it.label !== '' && it.id !== '';
                    });

                    if (!normalized.length) {
                        window.force && console.info('LinkChecker: 選択肢データがありません（0件）。');
                        return;
                    }

                    // 件数が1件ならプルダウンを出さずに自動で値を入れる（元の挙動を維持）
                    if (normalized.length === 1) {
                        try {
                            if (useJq) {
                                $local(INPUT_A_SELECTOR).val(normalized[0].label).trigger('change');
                                $local(INPUT_Y_SELECTOR).val(normalized[0].id).trigger('change');
                            } else {
                                inputAEl.value = normalized[0].label;
                                inputYEl.value = normalized[0].id;
                                // change イベントを発火して外部のハンドラを動かす
                                var evA = document.createEvent('HTMLEvents'); evA.initEvent('change', true, false); inputAEl.dispatchEvent(evA);
                                var evY = document.createEvent('HTMLEvents'); evY.initEvent('change', true, false); inputYEl.dispatchEvent(evY);
                            }
                        } catch (e) {
                            window.force && console.error('LinkChecker: 単独データ挿入でエラー', e);
                        }
                        return;
                    }

                    // 複数件ある場合はクリックで出すプルダウンを用意
                    var DROPDOWN_ID = 'linkchecker-dropdown-ClassA';

                    // 既存のドロップダウンを消す（再生成に備える）
                    function removeDropdown() {
                        var ex = document.getElementById(DROPDOWN_ID);
                        if (ex && ex.parentNode) ex.parentNode.removeChild(ex);
                        document.removeEventListener('click', onDocClickClose);
                    }

                    // ドキュメント外クリックで閉じる処理
                    function onDocClickClose(evt) {
                        var dd = document.getElementById(DROPDOWN_ID);
                        if (!dd) return;
                        var t = evt.target || evt.srcElement;
                        if (!dd.contains(t) && t !== inputAEl) removeDropdown();
                    }

                    // ドロップダウン作成・表示
                    function showDropdown() {
                        removeDropdown();

                        var container = createEl('div');
                        container.id = DROPDOWN_ID;

                        // 見た目の最低限スタイル（必要に応じて調整してください）
                        container.style.position = 'absolute';
                        container.style.zIndex = 99999;
                        container.style.minWidth = (inputAEl.offsetWidth || 200) + 'px';
                        container.style.maxHeight = '240px';
                        container.style.overflowY = 'auto';
                        container.style.border = '1px solid #ccc';
                        container.style.background = '#fff';
                        container.style.boxShadow = '0 2px 6px rgba(0,0,0,0.15)';
                        container.style.padding = '4px 0';
                        container.style.borderRadius = '4px';

                        // 各選択肢を作る。表示は「店舗名のみ」（ResultIdは表示しない仕様）
                        normalized.forEach(function (item, idx) {
                            var row = createEl('div');
                            row.setAttribute('data-idx', String(idx));
                            row.style.padding = '6px 10px';
                            row.style.cursor = 'pointer';
                            row.style.whiteSpace = 'nowrap';
                            row.style.overflow = 'hidden';
                            row.style.textOverflow = 'ellipsis';
                            row.textContent = item.label; // ← ここで ID は表示しない

                            // マウスオーバーの視覚フィードバック
                            row.addEventListener('mouseenter', function () { row.style.background = '#f5f5f5'; });
                            row.addEventListener('mouseleave', function () { row.style.background = 'transparent'; });

                            // 項目をクリックしたらフォームに値を入れて閉じる
                            row.addEventListener('click', function (ev) {
                                ev.stopPropagation();
                                try {
                                    if (useJq) {
                                        $local(INPUT_A_SELECTOR).val(item.label).trigger('change');
                                        $local(INPUT_Y_SELECTOR).val(item.id).trigger('change');
                                    } else {
                                        inputAEl.value = item.label;
                                        inputYEl.value = item.id;
                                        var evA = document.createEvent('HTMLEvents'); evA.initEvent('change', true, false); inputAEl.dispatchEvent(evA);
                                        var evY = document.createEvent('HTMLEvents'); evY.initEvent('change', true, false); inputYEl.dispatchEvent(evY);
                                    }
                                } catch (e) {
                                    window.force && console.error('LinkChecker: ドロップダウン選択時にエラー', e);
                                } finally {
                                    removeDropdown();
                                }
                            });

                            container.appendChild(row);
                        });

                        // 入力欄の位置下に表示する（スクロールを考慮）
                        var rect = inputAEl.getBoundingClientRect();
                        var scrollLeft = (window.pageXOffset || document.documentElement.scrollLeft || document.body.scrollLeft || 0);
                        var scrollTop = (window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0);
                        container.style.left = (rect.left + scrollLeft) + 'px';
                        container.style.top = (rect.bottom + scrollTop + 4) + 'px';

                        document.body.appendChild(container);

                        // 外側クリックで閉じるためのイベント（直後に登録）
                        setTimeout(function () { document.addEventListener('click', onDocClickClose); }, 0);
                    }

                    // 入力欄クリックでプルダウンをトグル（既にあるなら閉じる）
                    function onInputClick(ev) {
                        ev.stopPropagation();
                        var ex = document.getElementById(DROPDOWN_ID);
                        if (ex) { removeDropdown(); return; }
                        showDropdown();
                    }

                    // キーボードで閉じる（Esc）
                    function onKeyDown(ev) {
                        if (ev.key === 'Escape' || ev.keyCode === 27) removeDropdown();
                    }

                    // イベント登録（jQuery なら off/on で上書き、バニラなら removeEventListener→addEventListener）
                    if (useJq) {
                        $local(INPUT_A_SELECTOR).off('.linkchecker').on('click.linkchecker', onInputClick);
                        $local(INPUT_A_SELECTOR).off('keydown.linkchecker').on('keydown.linkchecker', function (e) {
                            if (e.key === 'Escape' || e.keyCode === 27) { e.preventDefault(); removeDropdown(); }
                        });
                    } else {
                        try { inputAEl.removeEventListener('click', onInputClick); } catch (e) {}
                        inputAEl.addEventListener('click', onInputClick);
                        document.removeEventListener('keydown', onKeyDown);
                        document.addEventListener('keydown', onKeyDown);
                    }
                })();
                // ---------- 取得データを使った UI 処理 終了 ----------

                // ここに追加のデータ処理（grouping 等）を入れても良い

            } catch (err) {
                // エラーが発生しても decLoading を確実に呼ぶためにログだけ出す
                window.force && console.error('LinkChecker: sdtApifunction 内処理エラー', err);
            } finally {
                // 常に decLoading を呼ぶ（無ければ無視）
                try { decLoading(); } catch (e) { /* 無ければスキップ */ }
            }
        }).catch(function (err) {
            // fetchSiteKeyValues が失敗した場合も decLoading を呼ぶ
            try { decLoading(); } catch (e) { /* 無ければスキップ */ }

            try { showCommonError(err); } catch (e) { /* 無ければスキップ */ }
            window.force && console.error('LinkChecker: fetchSiteKeyValues エラー（sdtApifunction catch）', err);
        });
    }

    // ---------- データ取得（ページング対応） ----------
    function fetchSiteKeyValues(siteId) {
        window.force && console.log('処理開始', { siteId: siteId });

        return new Promise(function (resolve, reject) {
            var offset = 0;
            var allData = [];
            var firstResponse = null;

            function getPage() {
                var postData = {
                    View: {
                        "ApiDataType": "KeyValues",
                        "ApiColumnKeyDisplayType": "ColumnName",
                        "ApiColumnValueDisplayType": "DisplayValue",
                        "GridColumns": ["ResultId","ClassA","DateA","DateB"],
                        "ColumnFilterHash": {
                            "ClassB": JSON.stringify([ String(siteId) ])
                        }
                    },
                    Offset: offset
                };

                $p.apiGet({
                    id: event_Term,
                    data: postData,
                    done: function (data) {
                        var resp = (data && data.Response) ? data.Response : null;
                        if (!resp) {
                            showCommonError(500);
                            return reject(500);
                        }

                        if (!firstResponse) firstResponse = data;

                        var pageData = Array.isArray(resp.Data) ? resp.Data : [];
                        Array.prototype.push.apply(allData, pageData);

                        var pageSize = (typeof resp.PageSize === 'number' && resp.PageSize > 0) ? resp.PageSize : pageData.length;
                        var totalCount = (typeof resp.TotalCount === 'number') ? resp.TotalCount : allData.length;

                        window.force && console.log('ページ取得', { offset: offset, pageSize: pageSize, totalCount: totalCount, got: pageData.length });

                        if (offset + pageSize < totalCount) {
                            offset += pageSize;
                            getPage();
                        } else {
                            if (firstResponse && firstResponse.Response) {
                                firstResponse.Response.Data = allData;
                                firstResponse.Response.Offset = 0;
                                firstResponse.Response.PageSize = allData.length;
                                firstResponse.Response.TotalCount = totalCount;
                                resolve(firstResponse);
                            } else {
                                resolve(allData);
                            }
                        }
                    },
                    fail: function (err) {
                        showCommonError(err);
                        reject(err);
                    }
                });
            }

            getPage();
        });
    }

    // ---------- Pleasanter イベント接続 & 起動 ----------
    if (typeof $p !== 'undefined' && $p && $p.events) {
        $p.events.on_editor_load = function () { processFlow(); };
        $p.events.after_set = function () { processFlow(); };
    }

    // DOM ready でも保険で実行
    if ($jq) {
        $jq(function () { processFlow(); });
    } else {
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', function () { processFlow(); });
        } else {
            processFlow();
        }
    }

})();
