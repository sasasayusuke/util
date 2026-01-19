(function(){
  // --- 設定 ---
  // 対象テーブルを特定するセレクタ（必要ならここを変更）
  const TABLE_SELECTOR = 'table#Results_Source2832813, table.grid[data-id="2832813"], table.grid[data-name="Source"]';
  // true = 「表示されている最初の行」を許可、false = DOM上の最初の行（display:none含む）を許可
  const useFirstVisibleRow = true;

  // --- helper ---
  const isRowVisible = (tr) => {
    if(!tr) return false;
    // 見た目上の表示判定（display:none や visibility:hidden を考慮）
    return !!(tr.offsetWidth || tr.offsetHeight || tr.getClientRects().length);
  };

  // Apply clickable state: only the first (visible) row gets data-sdt-clickable="1"
  function applyOnlyFirstClickable(table){
    if(!table) return;
    const tbody = table.querySelector('tbody');
    if(!tbody) return;
    const rows = Array.from(tbody.querySelectorAll('tr.grid-row, tr'));
    // find target index
    let firstIndex = -1;
    if(useFirstVisibleRow){
      firstIndex = rows.findIndex(r => isRowVisible(r));
    } else {
      firstIndex = rows.length ? 0 : -1;
    }
    rows.forEach((r, i) => {
      if(i === firstIndex){
        r.setAttribute('data-sdt-clickable', '1');
        r.classList.add('sdt-first-clickable');
        r.classList.remove('sdt-not-clickable');
      } else {
        r.setAttribute('data-sdt-clickable', '0');
        r.classList.remove('sdt-first-clickable');
        r.classList.add('sdt-not-clickable');
      }
    });
  }

  // Intercept clicks on the table to block non-clickable rows
  function attachClickInterceptor(table){
    if(!table) return;
    if(table.__sdt_first_row_interceptor) return;
    table.__sdt_first_row_interceptor = true;

    table.addEventListener('click', function(ev){
      // find the tr.grid-row target
      const tr = ev.target && ev.target.closest ? ev.target.closest('tr.grid-row, tr') : null;
      if(!tr) return; // click outside rows — let it pass
      // if row is hidden, block default and stop propagation (safety)
      if(!isRowVisible(tr)){
        ev.preventDefault();
        ev.stopImmediatePropagation && ev.stopImmediatePropagation();
        return false;
      }
      const clickable = tr.getAttribute('data-sdt-clickable');
      if(clickable !== '1'){
        // block navigation/click handlers
        ev.preventDefault();
        ev.stopImmediatePropagation && ev.stopImmediatePropagation();
        return false;
      }
      // allow default for the one clickable row
    }, true); // use capture phase to block upstream handlers
  }

  // inject styles for visual cue
  function injectStyles(){
    if(document.getElementById('sdt-firstrow-styles')) return;
    const s = document.createElement('style');
    s.id = 'sdt-firstrow-styles';
    s.textContent = `
      /* クリック許可のある行はポインタ表示 */
      tr[data-sdt-clickable="1"] { cursor: pointer; }
      /* クリック不可行の見た目（任意） */
      tr[data-sdt-clickable="0"] { cursor: default; opacity: 0.85; }
    `;
    document.head.appendChild(s);
  }

  // orchestrator for one or multiple tables
  function refreshForAllTables(){
    injectStyles();
    const tables = Array.from(document.querySelectorAll(TABLE_SELECTOR));
    tables.forEach(tbl => {
      applyOnlyFirstClickable(tbl);
      attachClickInterceptor(tbl);
    });
  }

  // Observe DOM changes (table re-render) and refresh with debounce
  const mo = new MutationObserver((mutations) => {
    let shouldRefresh = false;
    for(const m of mutations){
      // if rows added/removed or attributes changed inside target tables, refresh
      if(m.type === 'childList' && m.addedNodes.length) { shouldRefresh = true; break; }
      if(m.type === 'attributes') { shouldRefresh = true; break; }
    }
    if(shouldRefresh){
      if(window.__sdt_firstrow_timeout) clearTimeout(window.__sdt_firstrow_timeout);
      window.__sdt_firstrow_timeout = setTimeout(refreshForAllTables, 60);
    }
  });
  mo.observe(document.body, { childList: true, subtree: true, attributes: true });

  // initial run after DOM ready
  if(document.readyState === 'loading'){
    document.addEventListener('DOMContentLoaded', refreshForAllTables);
  } else {
    refreshForAllTables();
  }

  // debug hook
  window.__sdt_refresh_firstrow = refreshForAllTables;

})();
