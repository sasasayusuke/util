(function(){
  if(window.__pleasanter_date_sync_client_loaded) return;
  window.__pleasanter_date_sync_client_loaded = true;

  const log = (...a) => { try{ console.info('[date-sync-cl]', ...a); }catch(e){} };
  const visibleSelector = 'input[data-action="openSetDateRangeDialog"], input[id$="_DateRange"], input[name$="_DateRange"]';
  let lastTarget = null;
  let syncing = false;

  // フォーカス／クリックで「どのフィルタ用のモーダルか」を記録
  $(document).on('focus click', visibleSelector, function(){
    lastTarget = $(this).attr('id') || $(this).attr('name') || null;
    log('focused/clicked target ->', lastTarget);
  });

  // helper: get hidden element by name/id fallback
  function findHiddenByBase(baseName){
    if(!baseName) return null;
    // baseName 例: ViewFilters__ClassC~~2832814,DateC_DateRange -> strip _DateRange
    const base = baseName.replace(/_DateRange$/, '');
    let el = document.getElementById(base) || document.getElementsByName(base)[0];
    if(el) return el;
    // fallback: scan hidden inputs for substring match of last token (e.g. DateC)
    const token = base.split(',').slice(-1)[0];
    const hid = document.querySelectorAll('input[type="hidden"], input[data-method], input[id]');
    for(let i=0;i<hid.length;i++){
      const h = hid[i];
      if((h.id && h.id.indexOf(token) !== -1) || (h.name && h.name.indexOf(token) !== -1)) return h;
    }
    return null;
  }

  // normalizer: input value may be JSON like ["2025...","2025..."] or comma separated
  function normalizeHiddenString(s){
    if(!s && s !== '') return '';
    try {
      // if JSON array string
      const t = (''+s).trim();
      if(t.startsWith('[')) {
        try {
          const arr = JSON.parse(t);
          if(Array.isArray(arr)) return arr.map(x => (''+x).trim().replace(/,$/,'')).filter(x=>x!=='').slice(0,2);
        } catch(e){}
      }
      // strip surrounding quotes/brackets and trailing commas
      const cleaned = t.replace(/^\[|]$/g,'').replace(/^["']|["']$/g,'').trim();
      // split on commas that separate tokens
      const parts = cleaned.split(',').map(p=>p.trim()).filter(p=>p!=='');
      return parts.slice(0,2);
    } catch(e){ return []; }
  }

  // format visible text from from/to tokens (simple)
  function visibleTextFromRange(parts){
    if(!parts || parts.length===0) return '';
    if(parts.length===1) return parts[0];
    return parts[0] + ' - ' + parts[1];
  }

  // main: DateRangeOK click handler
  $(document).on('click', '#DateRangeOK', function(){
    // delay a bit so Pleasanter's own handler writes hidden values first
    setTimeout(function(){
      if(syncing) { log('already syncing - skip'); return; }
      syncing = true;
      try {
        if(!lastTarget){
          log('no lastTarget recorded - abort');
          syncing = false;
          return;
        }
        log('OK pressed for target ->', lastTarget);

        const sourceHidden = findHiddenByBase(lastTarget);
        if(!sourceHidden){
          log('source hidden not found for', lastTarget);
          syncing = false; return;
        }
        const raw = sourceHidden.value;
        const parts = normalizeHiddenString(raw);
        log('source hidden value ->', raw, 'normalized ->', parts);

        // copy to other visible inputs & their hidden counterparts
        $(visibleSelector).each(function(){
          const $vis = $(this);
          const name = $vis.attr('name') || $vis.attr('id') || '';
          if(!name) return;
          if(name === lastTarget) return; // same filter skip
          const otherHidden = findHiddenByBase(name);
          if(otherHidden){
            // set hidden value as JSON array string like ["from","to"] if 2 parts, else ["from"]
            if(parts.length === 0){
              otherHidden.value = '';
            } else if(parts.length === 1){
              otherHidden.value = JSON.stringify([parts[0]]);
            } else {
              otherHidden.value = JSON.stringify([parts[0], parts[1]]);
            }
            // update visible input text for UX
            try { $vis.val(visibleTextFromRange(parts)); } catch(e){}
            log('copied to', name, '-> hidden', otherHidden.name || otherHidden.id, otherHidden.value);
          } else {
            // no hidden found — still update visible for UX
            try { $vis.val(visibleTextFromRange(parts)); log('updated visible only', name); } catch(e){}
          }
        });

        // trigger search / grid refresh
        try {
          const useFilter = (document.getElementById('UseFilterButton') || document.getElementsByName('UseFilterButton')[0] || {}).value;
          if(String(useFilter) === '1'){
            // try find visible search button then click; fallback to $p.send
            const btn = $('button[data-action="Search"], button:contains("検索"), button[id*="Search"], button#Search').filter(':visible').first();
            if(btn && btn.length){
              log('clicking search button');
              try { btn.click(); }
              catch(e){ log('search button click failed', e); try { $p.send($('#Grid')); } catch(e2){ log('p.send failed', e2); } }
            } else {
              log('search button not found -> $p.send');
              try { $p.send($('#Grid')); } catch(e){ log('p.send failed', e); }
            }
          } else {
            // auto-search mode
            log('auto-search mode -> $p.send');
            try { $p.send($('#Grid')); } catch(e){ log('p.send failed', e); }
          }
        } catch(e){ log('search trigger failed', e); }
      } catch(err){
        log('unexpected sync error', err);
      } finally {
        syncing = false;
      }
    }, 80);
  });

  log('date-range client sync installed');
})();