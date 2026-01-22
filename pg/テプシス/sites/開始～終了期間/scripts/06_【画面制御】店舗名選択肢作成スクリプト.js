(function () {
    'use strict';
    // ---------- jQuery 安全参照 ----------
    var $jq = (typeof window !== 'undefined' && typeof window.jQuery !== 'undefined')
        ? window.jQuery
        : (typeof window !== 'undefined' && typeof window.$ === 'function' ? window.$ : null);
    if ($jq) {
        console.info('LinkChecker: using jQuery (version ' + ($jq.fn && $jq.fn.jquery ? $jq.fn.jquery : 'unknown') + ')');
    } else {
        console.info('LinkChecker: jQuery not available, using vanilla DOM API as fallback.');
    }
    // ---------- siteId 決定（event_Site_Id を優先、無ければ外部変数 TARGET_LINK_TABLE_DATA_ID） ----------
    function getEffectiveSiteId() {
        if (typeof event_Site_Id !== 'undefined' && event_Site_Id !== null) return String(event_Site_Id);
        if (typeof window !== 'undefined' && typeof window.TARGET_LINK_TABLE_DATA_ID !== 'undefined') return String(window.TARGET_LINK_TABLE_DATA_ID);
        return undefined;
    }
    // ---------- DOM / 要素ユーティリティ ----------
    function getResultsClassAElement() {
        if ($jq) {
            var $el = $jq('#Results_ClassA');
            return ($el && $el.length) ? $el.get(0) : null;
        } else {
            return document.getElementById('Results_ClassA');
        }
    }
    function preventDefaultHandler(e) { e.preventDefault(); }
    // ---------- テーブル検索と ID 抽出 ----------
    function getLinkTableByDataId(siteId) {
        if ($jq) {
            var $t = $jq('table[data-id="' + siteId + '"]').first();
            return ($t && $t.length) ? $t.get(0) : null;
        } else {
            return document.querySelector('table[data-id="' + siteId + '"]');
        }
    }
    function extractIdFromHref(href) {
        if (!href) return null;
        var m = href.match(/\/items\/(\d+)/);
        if (m && m[1]) return m[1];
        var searchMatch = href.match(/[?&]LinkId=(\d+)/);
        if (searchMatch && searchMatch[1]) return searchMatch[1];
        return null;
    }
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
    // ---------- tableElem から ResultId 列の td 値を取得 ----------
    function getResultIdsFromTable(tableElem) {
        var ids = [];
        if (!tableElem) return ids;
        if ($jq) {
            var $table = $jq(tableElem);
            var $th = $table.find('thead tr th[data-name="ResultId"]').first();
            if (!$th || !$th.length) {
                console.warn('LinkChecker: th[data-name="ResultId"] が見つかりません。');
                return ids;
            }
            var colIndex = $th.index();
            $table.find('tbody tr').each(function () {
                var $tr = $jq(this);
                var $td = $tr.find('td').eq(colIndex);
                if ($td && $td.length) {
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
                }
            });
        } else {
            var thead = tableElem.querySelector('thead');
            if (!thead) {
                console.warn('LinkChecker: thead が見つかりません。');
                return ids;
            }
            var th = thead.querySelector('tr th[data-name="ResultId"]');
            if (!th) {
                console.warn('LinkChecker: th[data-name="ResultId"] が見つかりません。');
                return ids;
            }
            var headers = Array.prototype.slice.call(th.parentNode.children);
            var colIndex = headers.indexOf(th);
            var rows = tableElem.querySelectorAll('tbody tr');
            Array.prototype.forEach.call(rows, function (row) {
                var tds = row.querySelectorAll('td');
                var td = tds[colIndex];
                if (td) {
                    var text = td.textContent.trim();
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
                }
            });
        }
        // 一意化して返却
        ids = ids.filter(function (v, i, self) { return self.indexOf(v) === i; });
        return ids;
    }
    // ---------- ドロップダウン管理（body に直接追加する実装、外部化不要） ----------
    // data-lc-dropdown-id 属性で input と body の dropdown を結びつける
    function removeExistingDropdown(inputEl) {
        if (!inputEl) return;
        // wrapper が input の親であれば wrapper を解体して input を元に戻す
        var parent = inputEl.parentNode;
        if (parent && parent.classList && parent.classList.contains('linkchecker-wrapper')) {
            var wrapper = parent;
            if (wrapper.parentNode) {
                wrapper.parentNode.insertBefore(inputEl, wrapper);
                wrapper.parentNode.removeChild(wrapper);
            }
        } else {
            // もし input の直上に wrapper がないが親内に wrapper があればそれを削除
            if (parent) {
                var maybeWrapper = parent.querySelector && parent.querySelector('.linkchecker-wrapper');
                if (maybeWrapper) {
                    if (maybeWrapper.contains(inputEl) && maybeWrapper.parentNode) {
                        maybeWrapper.parentNode.insertBefore(inputEl, maybeWrapper);
                    }
                    if (maybeWrapper.parentNode) maybeWrapper.parentNode.removeChild(maybeWrapper);
                }
            }
        }
        // body に追加された dropdown を削除
        var ddId = inputEl.getAttribute('data-lc-dropdown-id');
        if (ddId) {
            var existingUl = document.getElementById(ddId);
            if (existingUl && existingUl.parentNode) existingUl.parentNode.removeChild(existingUl);
            inputEl.removeAttribute('data-lc-dropdown-id');
        }
    }
    function setInputValue(inputEl, value) {
        if (!inputEl) return;
        inputEl.value = value;
        try {
            if ($jq) $jq(inputEl).trigger('change');
            else {
                var ev = document.createEvent('HTMLEvents');
                ev.initEvent('change', true, false);
                inputEl.dispatchEvent(ev);
            }
        } catch (e) { /* noop */ }
    }
    // ドロップダウンを body に作って表示する（options: string[]）
    function createDropdownForInput(inputEl, options) {
        if (!inputEl) return;
        // まず既存削除
        removeExistingDropdown(inputEl);
        // wrapper を作って input を内包（見た目維持）
        var wrapper = document.createElement('span');
        wrapper.className = 'linkchecker-wrapper';
        wrapper.style.display = 'inline-block';
        wrapper.style.position = 'relative';
        wrapper.style.verticalAlign = 'middle';
        var computedWidth = (window.getComputedStyle) ? getComputedStyle(inputEl).width : '';
        if (computedWidth) wrapper.style.width = computedWidth;
        inputEl.parentNode.insertBefore(wrapper, inputEl);
        wrapper.appendChild(inputEl);
        // input スタイル調整（ボタン分の余白）
        inputEl.style.boxSizing = 'border-box';
        inputEl.style.width = '100%';
        inputEl.style.paddingRight = '36px';
        inputEl.setAttribute('readonly', 'readonly');
        // ブロック系イベントで直接入力を阻止
        inputEl.addEventListener('keydown', preventDefaultHandler);
        inputEl.addEventListener('paste', preventDefaultHandler);
        // toggle ボタン（wrapper 内）
        var btn = document.createElement('button');
        btn.type = 'button';
        btn.className = 'linkchecker-toggle';
        btn.setAttribute('aria-haspopup', 'true');
        btn.setAttribute('aria-expanded', 'false');
        btn.style.position = 'absolute';
        btn.style.right = '2px';
        btn.style.top = '50%';
        btn.style.transform = 'translateY(-50%)';
        btn.style.height = '24px';
        btn.style.padding = '0 6px';
        btn.style.cursor = 'pointer';
        btn.style.border = '1px solid #ccc';
        btn.style.background = '#fff';
        btn.style.fontSize = '12px';
        btn.textContent = '▼';
        wrapper.appendChild(btn);
        // dropdown を body に作成
        var ul = document.createElement('ul');
        var ddId = 'linkchecker_dd_' + (Date.now()) + '_' + (Math.floor(Math.random() * 10000));
        ul.id = ddId;
        ul.className = 'linkchecker-dropdown';
        ul.style.position = 'absolute';
        ul.style.zIndex = '99999';
        ul.style.listStyle = 'none';
        ul.style.margin = '0';
        ul.style.padding = '4px 0';
        ul.style.border = '1px solid #ccc';
        ul.style.background = '#fff';
        ul.style.maxHeight = '240px';
        ul.style.overflowY = 'auto';
        ul.style.boxShadow = '0 2px 10px rgba(0,0,0,0.12)';
        ul.style.display = 'none';
        document.body.appendChild(ul);
        // li を追加
        options.forEach(function (opt) {
            var li = document.createElement('li');
            li.style.padding = '6px 10px';
            li.style.cursor = 'pointer';
            li.style.whiteSpace = 'nowrap';
            li.textContent = opt;
            li.addEventListener('mouseenter', function () { li.style.background = '#f0f0f0'; });
            li.addEventListener('mouseleave', function () { li.style.background = ''; });
            li.addEventListener('click', function (e) {
                e.stopPropagation();
                setInputValue(inputEl, opt);
                hideDropdown();
            });
            ul.appendChild(li);
        });
        // input と dropdown を紐づけるための属性
        inputEl.setAttribute('data-lc-dropdown-id', ddId);
        // 位置決め関数（表示前に必ず呼ぶ）
        function positionDropdown() {
            var rect = inputEl.getBoundingClientRect();
            var width = rect.width;
            var left = rect.left + window.pageXOffset;
            var top = rect.bottom + window.pageYOffset + 4; // 少し空ける
            ul.style.left = left + 'px';
            ul.style.top = top + 'px';
            ul.style.width = width + 'px';
        }
        function showDropdown() {
            positionDropdown();
            ul.style.display = 'block';
            btn.setAttribute('aria-expanded', 'true');
        }
        function hideDropdown() {
            ul.style.display = 'none';
            btn.setAttribute('aria-expanded', 'false');
        }
        // toggle と input クリックで開閉
        btn.addEventListener('click', function (e) {
            e.stopPropagation();
            if (ul.style.display === 'block') hideDropdown(); else showDropdown();
        });
        inputEl.addEventListener('click', function (e) {
            e.stopPropagation();
            if (ul.style.display === 'block') hideDropdown(); else showDropdown();
        });
        // グローバルクリックで閉じる（多重登録防止）
        if (!window._linkchecker_global_click_attached) {
            window._linkchecker_global_click_attached = true;
            document.addEventListener('click', function () {
                var list = document.querySelectorAll('.linkchecker-dropdown');
                Array.prototype.forEach.call(list, function (u) { u.style.display = 'none'; });
                var toggles = document.querySelectorAll('.linkchecker-toggle');
                Array.prototype.forEach.call(toggles, function (t) { t.setAttribute('aria-expanded', 'false'); });
            });
        }
        // スクロール・リサイズ時は表示中なら位置更新（passive リスナ）
        window.addEventListener('resize', function () {
            if (ul.style.display === 'block') positionDropdown();
        });
        window.addEventListener('scroll', function () {
            if (ul.style.display === 'block') positionDropdown();
        }, { passive: true });
    }
    // ---------- API 取得（ページング対応） ----------
    function fetchSiteKeyValues(siteId) {
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
                        "GridColumns": ["ClassI", "ClassJ", "ClassK"]
                    },
                    Offset: offset
                };
                $p.apiGet({
                    id: siteId,
                    data: postData,
                    done: function (data) {
                        var resp = (data && data.Response) ? data.Response : null;
                        if (!resp) {
                            try { showCommonError(500); } catch (e) { /* noop */ }
                            return reject(500);
                        }
                        if (!firstResponse) firstResponse = data;
                        var pageData = Array.isArray(resp.Data) ? resp.Data : [];
                        Array.prototype.push.apply(allData, pageData);
                        var pageSize = (typeof resp.PageSize === 'number' && resp.PageSize > 0) ? resp.PageSize : pageData.length;
                        var totalCount = (typeof resp.TotalCount === 'number') ? resp.TotalCount : allData.length;
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
                        try { showCommonError(err); } catch (e) { /* noop */ }
                        reject(err);
                    }
                });
            }
            getPage();
        });
    }
    // ---------- 取得データを #Results_ClassA に反映する処理（あなたの要件に沿う） ----------
    function applyDataToResultsClassA(data) {
        // 安全に Data 配列を取得
        var rows = (data && data.Response && Array.isArray(data.Response.Data)) ? data.Response.Data : [];
        if (!rows.length) {
            console.warn('LinkChecker: 取得データが空です。');
            return;
        }
        var rec = rows[0];
        // ClassI が '店舗数' のときは ClassK、'店舗ごと入力' のときは ClassJ を使用
        var chosenRaw = '';
        if (rec.ClassI === '店舗数') {
            chosenRaw = rec.ClassK || '';
        } else if (rec.ClassI === '店舗ごと入力') {
            chosenRaw = rec.ClassJ || '';
        } else {
            console.warn('LinkChecker: 未対応の ClassI 値:', rec.ClassI);
            return;
        }
        // カンマ区切りで分解、トリムし空要素除去
        var options = ('' + chosenRaw).split(',').map(function (s) { return s.trim(); }).filter(function (s) { return s.length > 0; });
        var inputEl = getResultsClassAElement();
        if (!inputEl) {
            console.warn('LinkChecker: 対象 input (#Results_ClassA) が見つかりません。');
            return;
        }
        // 直接入力禁止（readonly）と基本的な阻止
        inputEl.setAttribute('readonly', 'readonly');
        inputEl.addEventListener('keydown', preventDefaultHandler);
        inputEl.addEventListener('paste', preventDefaultHandler);
        // 分岐: 0,1,複数
        if (options.length === 0) {
            removeExistingDropdown(inputEl);
            setInputValue(inputEl, '');
            return;
        }
        if (options.length === 1) {
            removeExistingDropdown(inputEl);
            setInputValue(inputEl, options[0]);
            return;
        }
        // 複数ならプルダウンを作る（クリックで選択）
        createDropdownForInput(inputEl, options);
    }
    // ---------- sdtApifunction（API 呼び出し後の制御を行う） ----------
    function sdtApifunction(saiteID) {
        try { window.force && console.log('index開始'); } catch (e) { /* noop */ }
        try { if (typeof incLoading === 'function') incLoading(); } catch (e) { /* noop */ }
        fetchSiteKeyValues(saiteID).then(function (data) {
            // ここが「次の処理」の起点
            console.log('次の処理', data);
            // データをフォームに反映
            try {
                applyDataToResultsClassA(data);
            } catch (e) {
                console.error('LinkChecker: 値反映処理でエラー', e);
            }
            // ロード表示解除（無ければ無視）
            try { if (typeof decLoading === 'function') decLoading(); } catch (e) { /* noop */ }
            // 一覧への反映（FilterButton を押す挙動を試みる）
            try {
                if ($jq) {
                    $p.send($jq('#FilterButton'));
                } else {
                    var fb = document.getElementById('FilterButton');
                    if (fb) $p.send(fb);
                }
            } catch (e) {
                console.warn('LinkChecker: $p.send によるフィルタ適用でエラー', e);
            }
        }).catch(function (err) {
            console.error('LinkChecker: fetchSiteKeyValues エラー', err);
            try { if (typeof decLoading === 'function') decLoading(); } catch (e) { /* noop */ }
        });
    }
    // ---------- トリガ（画面読み込み・レコード登録・更新後） ----------
    function processFlow() {
        var siteId = getEffectiveSiteId();
        if (!siteId) {
            console.warn('LinkChecker: event_Site_Id が指定されていません。window.TARGET_LINK_TABLE_DATA_ID を設定してください。');
            return;
        }
        var tableElem = getLinkTableByDataId(siteId);
        if (tableElem) {
            var ids = getResultIdsFromTable(tableElem);
            if (ids && ids.length) {
                sdtApifunction(ids[0]);
            } else {
                // テーブルはあるが ID が取れなかった場合は URL を参照
                var linkId = getQueryParamFromLocation('LinkId');
                if (linkId) sdtApifunction(linkId);
            }
        } else {
            var linkId = getQueryParamFromLocation('LinkId');
            if (linkId) sdtApifunction(linkId);
            else console.info('LinkChecker: 対象テーブル／LinkId が見つかりませんでした。siteId=' + siteId);
        }
    }
    // Pleasanter のイベントに接続（編集画面読み込み／更新後）
    if (typeof $p !== 'undefined' && $p && $p.events) {
        $p.events.on_editor_load = function () { processFlow(); };
        $p.events.after_set = function () { processFlow(); };
    }
    // DOM ready でも一度実行（保険）
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
