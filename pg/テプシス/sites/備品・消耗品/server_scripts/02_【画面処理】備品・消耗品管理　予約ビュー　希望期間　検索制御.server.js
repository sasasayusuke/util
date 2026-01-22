(function(){
  try {
    // logging helper (server)
    function log(){ try{ if(context && context.Log) context.Log(Array.prototype.join.call(arguments,' ')); else console.log.apply(console, arguments); }catch(e){} }

    if(!view || !view.Filters){
      log('[date-or] no view/Filters found - abort');
      return;
    }

    // find keys for DateC and DateD in view.Filters (key names contain those suffixes)
    function findFilterKey(suffix){
      return Object.keys(view.Filters || {}).find(k => k.indexOf(suffix) !== -1);
    }

    const keyC = findFilterKey('DateC');
    const keyD = findFilterKey('DateD');

    if(!keyC && !keyD){
      log('[date-or] neither DateC nor DateD filter found - nothing to do');
      return;
    }

    // parse filter value robustly (handles JSON array strings or comma separated)
    function parseFilterVal(raw){
      if(raw == null) return null;
      var s = String(raw).trim();
      if(!s) return null;
      // try JSON parse
      try {
        if(s[0] === '[') {
          var arr = JSON.parse(s);
          if(Array.isArray(arr)) return arr.map(function(x){ return (x||'').toString().trim().replace(/,$/,''); });
        }
      } catch(e){}
      // remove surrounding brackets/quotes and split by comma
      s = s.replace(/^\[|\]$/g,'').replace(/^["']|["']$/g,'').trim();
      var parts = s.split(',').map(function(x){ return (x||'').toString().trim().replace(/,$/,''); }).filter(function(x){ return x !== ''; });
      return parts.length ? parts : null;
    }

    function rangeFromParts(parts){
      if(!parts) return null;
      if(parts.length === 1) return { from: parts[0], to: parts[0] };
      return { from: parts[0], to: parts[1] || parts[0] };
    }

    const rawC = keyC ? view.Filters[keyC] : null;
    const rawD = keyD ? view.Filters[keyD] : null;
    const partsC = parseFilterVal(rawC);
    const partsD = parseFilterVal(rawD);
    const rangeC = rangeFromParts(partsC);
    const rangeD = rangeFromParts(partsD);

    if(!rangeC && !rangeD){
      log('[date-or] no usable ranges parsed for DateC/DateD');
      return;
    }

    // escape for SQL literal (basic)
    function escSql(s){ return (s||'').replace(/'/g,"''"); }

    // map filter key -> SQL column identifier
    // NOTE: default strategy: take key after "ViewFilters__" and use as column. Adjust if your DB mapping differs.
    function colFromFilterKey(k){
      if(!k) return null;
      var id = k.replace(/^ViewFilters__/, '');
      // sanitize brackets removal
      id = id.replace(/]/g,'').replace(/\[/g,'');
      // if your DB column names differ, replace here
      return id; // we will not wrap in [] here; put in finalWhere using proper quoting if needed
    }

    var colC = keyC ? colFromFilterKey(keyC) : null;
    var colD = keyD ? colFromFilterKey(keyD) : null;

    var sqlParts = [];
    if(rangeC && colC){
      sqlParts.push("(" + colC + " BETWEEN '" + escSql(rangeC.from) + "' AND '" + escSql(rangeC.to) + "')");
    }
    if(rangeD && colD){
      sqlParts.push("(" + colD + " BETWEEN '" + escSql(rangeD.from) + "' AND '" + escSql(rangeD.to) + "')");
    }

    if(sqlParts.length === 0){
      log('[date-or] no SQL parts built - abort');
      return;
    }

    var finalWhere = "(" + sqlParts.join(" OR ") + ")";
    log('[date-or] finalWhere ->', finalWhere);

    // Try common view properties that accept raw where fragment
    var candidates = ['CustomWhere','SqlWhere','Where','ExtraWhere','Query','CustomFilter','WhereClause'];
    var wrote = false;
    for(var i=0;i<candidates.length;i++){
      var p = candidates[i];
      try {
        if(typeof view[p] !== 'undefined'){
          view[p] = finalWhere;
          wrote = true;
          log('[date-or] wrote to view.'+p);
          break;
        }
      } catch(e){
        log('[date-or] write to view.'+p+' failed: '+ (e && e.message ? e.message : e));
      }
    }

    if(!wrote){
      // fallback: remove original filter keys (so they don't AND) and inject the raw where into a special Filters key (some Pleasanter builds use view.Filters for extra conditions)
      try {
        if(keyC) delete view.Filters[keyC];
        if(keyD) delete view.Filters[keyD];
        view.Filters['__OrDateRange__'] = finalWhere;
        wrote = true;
        log('[date-or] fallback: removed original keys and injected into view.Filters.__OrDateRange__');
      } catch(e){
        log('[date-or] fallback injection failed: '+(e && e.message? e.message : e));
      }
    }

    log('[date-or] done. view.Filters keys now: ' + Object.keys(view.Filters || {}).join(','));
  } catch(err){
    try{ if(context && context.Log) context.Log(err && err.stack ? err.stack : err); }catch(e){}
  }
})();