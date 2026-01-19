(function(){
  // ラベル文言（必要ならここを変更）
  const CHECKBOX_ID = 'sdt-show-used-toggle';
  const CHECKBOX_LABEL = '過去予約表示'; // チェックで使用日時/返却日時ありも表示

  // テーブルを見つけるセレクタ（環境に応じて必要なら調整）
  const TABLE_SELECTOR = 'table.grid[data-name="Source"], table[id^="Results_Source"]';
  const TABLE_WRAP_SELECTOR = '#Results_Source'+reservationUsageList +'Field, grid-container';

  // helpers
  const isEmptyCell = (td) => {
    if(!td) return true;
    const txt = td.textContent || '';
    return txt.trim().length === 0;
  };

  function findReservationButton(){
    // 予約ボタンは onclick="$p.new(...)" を持つことが多いのでまずそれで探す
    return document.querySelector('button[onclick^="$p.new"], button[data-icon="ui-icon-plus"], button[title^="サンプル"], button:contains("予約")');
  }

  // Insert checkbox next to reservation button (once)
  function ensureCheckboxInserted(){
    // smartly find the button near the top of the provided HTML
    const btn = document.querySelector('button[onclick^="$p.new"], button[data-icon="ui-icon-plus"]');
    if(!btn) return null;
    // avoid double-insert
    if(document.getElementById(CHECKBOX_ID)) return document.getElementById(CHECKBOX_ID);

    const wrapper = document.createElement('div');
    wrapper.style.display = 'inline-flex';
    wrapper.style.alignItems = 'center';
    wrapper.style.marginLeft = '8px';

    const checkbox = document.createElement('input');
    checkbox.type = 'checkbox';
    checkbox.id = CHECKBOX_ID;
    // デフォルト： 未チェック = 「使用日時・返却日時が空の行のみ表示」
    checkbox.checked = false;

    const label = document.createElement('label');
    label.htmlFor = CHECKBOX_ID;
    label.style.marginLeft = '6px';
    label.style.userSelect = 'none';
    label.textContent = CHECKBOX_LABEL;

    wrapper.appendChild(checkbox);
    wrapper.appendChild(label);

    // 挿入位置：ボタンの直後
    btn.parentNode.insertBefore(wrapper, btn.nextSibling);

    return checkbox;
  }

  // Find table and compute column indices for DateC/DateD
  function getTableAndColIndices(){
    const table = document.querySelector(TABLE_SELECTOR);
    if(!table) return null;
    const ths = table.querySelectorAll('thead th');
    let idxDateC = -1, idxDateD = -1;
    ths.forEach((th, i) => {
      const name = th.getAttribute('data-name');
      if(name === 'DateC') idxDateC = i;
      if(name === 'DateD') idxDateD = i;
    });
    // fallback: if not found, attempt to match by header text (予定使用日時 / 予定返却日時)
    if(idxDateC === -1 || idxDateD === -1){
      ths.forEach((th, i) => {
        const txt = (th.textContent || '').trim();
        if(idxDateC === -1 && (txt === '予定使用日時' || txt.includes('予定使用日時'))) idxDateC = i;
        if(idxDateD === -1 && (txt === '予定返却日時' || txt.includes('予定返却日時'))) idxDateD = i;
      });
    }
    return { table, idxDateC, idxDateD };
  }

  // === 改良版: Apply filtering: show only rows based on DateC/DateD (planned start/end) ===
  function applyFilterToTable(tableInfo, showUsed){
    const { table, idxDateC, idxDateD } = tableInfo;
    if(!table) return;
    const tbody = table.querySelector('tbody');
    if(!tbody) return;
    const rows = Array.from(tbody.querySelectorAll('tr.grid-row'));

    // 拡張パーサ：
    // - 「2026/01/01 木」のような末尾の曜日トークンを取り除く
    // - 時刻が無い（= 日付のみ）の場合はその日の終わり (23:59:59) として扱う
    function parseDateFromText(s){
      if(!s) return null;
      let txt = String(s).replace(/\u00A0/g,' ').trim();

      if(txt === '') return null;

      // 末尾の日本語曜日トークンを取り除く（" 木" 等）
      txt = txt.replace(/\s+[月火水木金土日]$/, '');
      // 括弧内注記を削除
      txt = txt.replace(/[（）\(\)]/g, ' ');
      txt = txt.replace(/\s+/g, ' ').trim();

      // 正規表現で "YYYY/MM/DD [HH:MM(:SS)]" や "YYYY-MM-DD ..." を分解
      const m = txt.match(/^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})(?:[ T](\d{1,2}):(\d{1,2})(?::(\d{1,2}))?)?$/);
      if(m){
        const y = +m[1], mo = +m[2]-1, d = +m[3];
        const hh = m[4] ? +m[4] : null;
        const mm = m[5] ? +m[5] : 0;
        const ss = m[6] ? +m[6] : 0;
        if(hh === null){
          // 時刻が無い場合は「その日の終わり」を採用（23:59:59）
          return new Date(y, mo, d, 23, 59, 59);
        } else {
          return new Date(y, mo, d, hh, mm, ss);
        }
      }

      // フォールバック：Date に任せる（環境依存）
      const dt = new Date(txt);
      if(isNaN(dt.getTime())) return null;
      return dt;
    }

    const now = Date.now();

    rows.forEach(row => {
      const tds = row.querySelectorAll('td');
      const tdC = (idxDateC >= 0 && idxDateC < tds.length) ? tds[idxDateC] : null;
      const tdD = (idxDateD >= 0 && idxDateD < tds.length) ? tds[idxDateD] : null;

      const textC = tdC ? (tdC.textContent || '').trim() : '';
      const textD = tdD ? (tdD.textContent || '').trim() : '';

      // 値が空文字かどうか（空白のみ含む場合も空）
      const dateCEmpty = textC.replace(/\s+/g,'') === '';
      const dateDEmpty = textD.replace(/\s+/g,'') === '';

      // 値がある場合は日時をパースして過去かどうかを判定（パース失敗は安全側で "過去ではない" と扱う）
      const parsedC = dateCEmpty ? null : parseDateFromText(textC);
      const parsedD = dateDEmpty ? null : parseDateFromText(textD);

      const dateCIsPast = parsedC ? (parsedC.getTime() <= now) : false;
      const dateDIsPast = parsedD ? (parsedD.getTime() <= now) : false;

      // 「過去」と見なすのは、予定開始（DateC）または予定返却（DateD）が過去/同時刻である場合のみ
      const isPlannedPast = dateCIsPast || dateDIsPast;

      // 表示判定：チェック有なら全部表示、無ければ「過去でない行のみ表示」
      const shouldShow = showUsed ? true : !isPlannedPast;

      row.style.display = shouldShow ? '' : 'none';

      // 補助スタイル：過去扱い行は薄く見せる
      if(isPlannedPast){
        row.classList.add('sdt-used-row');
        row.classList.add('sdt-not-clickable');
        row.setAttribute('data-sdt-clickable', '0');
      } else {
        row.classList.remove('sdt-used-row');
        row.classList.remove('sdt-not-clickable');
        row.setAttribute('data-sdt-clickable', '1');
      }
    });
  }

  // Intercept clicks on rows: allow only rows that have data-sdt-clickable="1"
  function attachClickInterceptor(table){
    if(!table) return;
    // avoid double attaching
    if(table.__sdt_click_interceptor_attached) return;
    table.__sdt_click_interceptor_attached = true;

    // Use capture so we can block other handlers that may run later
    table.addEventListener('click', function(ev){
      const tr = ev.target && ev.target.closest ? ev.target.closest('tr.grid-row') : null;
      if(!tr) return; // not a row click
      // if row is hidden, allow default (nothing)
      if(tr.style.display === 'none') {
        ev.stopPropagation();
        ev.preventDefault();
        return;
      }
      const clickable = tr.getAttribute('data-sdt-clickable');
      if(clickable !== '1'){
        // block navigation/click handlers
        ev.preventDefault();
        // stop other listeners both capture and bubble by stopping immediate propagation
        ev.stopImmediatePropagation && ev.stopImmediatePropagation();
        return false;
      }
      // else allow click
    }, true); // capture = true
  }

  // Add some minimal CSS for visual cue
  function injectStyles(){
    if(document.getElementById('sdt-filter-styles')) return;
    const s = document.createElement('style');
    s.id = 'sdt-filter-styles';
    s.textContent = `
      /* 使用済み行の見た目 */
      tr.sdt-used-row { opacity: 0.7; }
      tr.sdt-not-clickable { cursor: default; }
      /* クリック可能行はポインタ表示 */
      tr[data-sdt-clickable="1"] { cursor: pointer; }
      /* checkbox wrapper spacing */
      #${CHECKBOX_ID} { transform: translateY(1px); }
    `;
    document.head.appendChild(s);
  }

  // Orchestrator: find table, ensure checkbox, apply filter, attach interceptor
  function refreshAll(){
    injectStyles();
    const checkbox = ensureCheckboxInserted();
    const tableInfo = getTableAndColIndices();
    if(!tableInfo || !tableInfo.table) return;
    const showUsed = checkbox ? checkbox.checked : false;
    applyFilterToTable(tableInfo, showUsed);
    attachClickInterceptor(tableInfo.table);

    // bind change handler for checkbox if not bound
    if(checkbox && !checkbox.__sdt_change_attached){
      checkbox.__sdt_change_attached = true;
      checkbox.addEventListener('change', () => {
        applyFilterToTable(tableInfo, checkbox.checked);
      });
    }
  }

  // Observe changes to re-run refreshAll when table is re-rendered
  const observer = new MutationObserver((mutations) => {
    // cheap check: if table added/removed or tbody changed, refresh
    let shouldRefresh = false;
    for(const m of mutations){
      if(m.type === 'childList' && m.addedNodes.length) {
        shouldRefresh = true;
        break;
      }
      if(m.type === 'attributes') {
        shouldRefresh = true;
        break;
      }
    }
    if(shouldRefresh){
      // small debounce
      if(window.__sdt_refresh_timeout) clearTimeout(window.__sdt_refresh_timeout);
      window.__sdt_refresh_timeout = setTimeout(() => {
        refreshAll();
      }, 80);
    }
  });

  // Start observing body for dynamic changes (could be scoped narrower if needed)
  observer.observe(document.body, { childList: true, subtree: true, attributes: false });

  // Initial run (after DOM ready)
  if(document.readyState === 'loading'){
    document.addEventListener('DOMContentLoaded', refreshAll);
  } else {
    refreshAll();
  }

  // Expose a debug function (optional)
  window.__sdt_refresh_filter = refreshAll;

})();
