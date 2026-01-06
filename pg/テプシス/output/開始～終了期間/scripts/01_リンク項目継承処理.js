// 子画面に置くコード例
(function(){
  const STORAGE_KEY = 'pleasanter.linkPayload';

  function readAndConsumePayload() {
    try {
      const raw = sessionStorage.getItem(STORAGE_KEY);
      if (!raw) return null;
      const payload = JSON.parse(raw);
      // 必要なら消す（1回だけ使いたい場合）
      sessionStorage.removeItem(STORAGE_KEY);
      return payload;
    } catch (e) {
      console.error('pleasanter: failed to read payload', e);
      return null;
    }
  }

  // 通常のページロード時
  document.addEventListener('DOMContentLoaded', function(){
    const payload = readAndConsumePayload();
    if (payload) {
      console.log('received payload', payload);
      // ここで画面に反映したり API 呼び出ししたりする
      if(payload.StoreManageMethod === '店舗ごと入力') {
        console.log('店舗ごと入力');
        $p.set($p.getControl('ClassA'), payload.PerStoreName)
      } else if(payload.StoreManageMethod === '店舗数') {
        console.log('店舗数');
        $p.set($p.getControl('ClassA'), payload.NumberOfStoreName)
      }
    }
  });

  // Pleasanter がオンページ遷移のイベントを出すならフック（存在する場合）
  if (window.$p && $p.events && typeof $p.events.on_page_loaded === 'function') {
    $p.events.on_page_loaded(function(){
      const payload = readAndConsumePayload();
      if (payload) {
        console.log('received payload (on_page_loaded)', payload);
        // 追加処理
      }
    });
  }
})();
