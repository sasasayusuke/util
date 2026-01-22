(function(){
  const TARGET_TEXT = '予定使用日時';
  const REPLACEMENT = '希望予約期間';

  // 書き換え済み管理（同じ要素を何度も変更しないように）
  const processed = new WeakSet();

  function replaceLabelIfMatches(label){
    if(!label || processed.has(label)) return;
    // 厳密一致（余白除去）
    if(label.textContent && label.textContent.trim() === TARGET_TEXT){
      label.textContent = REPLACEMENT;
      processed.add(label);
    }
  }

  // 初回スキャン（DOMContentLoaded 後に実行）
  function initialScan(){
    // IDに `DateC_DateRangeField` を含むコンテナ内の label に限定して探す（効率良く狙える）
    const labels = document.querySelectorAll('[id*="DateC_DateRangeField"] label');
    labels.forEach(replaceLabelIfMatches);
  }

  if(document.readyState === 'loading'){
    document.addEventListener('DOMContentLoaded', initialScan);
  } else {
    initialScan();
  }

  // 動的に追加される場合に備えて MutationObserver で監視
  const observer = new MutationObserver((mutations) => {
    for(const m of mutations){
      for(const node of m.addedNodes){
        if(node.nodeType !== 1) continue; // Element でなければ無視
        // 追加されたノード自体がターゲットコンテナなら直接処理
        if(node.matches && node.matches('[id*="DateC_DateRangeField"]')){
          const label = node.querySelector('label');
          replaceLabelIfMatches(label);
          continue;
        }
        // 追加ノードの子孫にターゲットラベルがあれば処理
        const labels = node.querySelectorAll && node.querySelectorAll('[id*="DateC_DateRangeField"] label');
        if(labels && labels.length){
          labels.forEach(replaceLabelIfMatches);
        }
      }
    }
  });

  observer.observe(document.body, { childList: true, subtree: true });

  // 必要なら外部から observer を停止できるようにする（デバッグ用）
  // window.__replaceLabelObserver = observer;
})();