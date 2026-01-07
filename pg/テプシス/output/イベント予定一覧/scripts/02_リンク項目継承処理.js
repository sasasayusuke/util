// 親画面に置くコード（1回だけ実行されるように）
(function(){
  // sessionStorage のキー名（必要に応じて変更）
  const STORAGE_KEY = 'pleasanter.linkPayload';

  // ペイロードを生成する関数（必要ならこの関数をカスタマイズ）
  // targetBtn: DOM element（クリックされたボタン）
  // ev: MouseEvent
  function buildPayload(targetBtn, ev) {
    // 例：ボタンの data-* 属性を渡す + タイムスタンプ + 任意パラメータ
    return {
      fromDataId: targetBtn.dataset.id || null,
      fromSiteId: targetBtn.dataset.fromSiteId || null,
      toSiteId: targetBtn.dataset.toSiteId || null,
      // ここに任意のパラメータを追加（例：現在ログインユーザーID等）
      StoreManageMethodId: $p.getControl('ClassI').val(),
      StoreManageMethod: commonGetVal('ClassI'),
      PerStoreId: $p.getControl('ClassJ').val(),
      PerStoreName: commonGetVal('ClassJ'),
      NumberOfStoreId: $p.getControl('ClassK').val(),
      NumberOfStoreName: commonGetVal('ClassK'),
      customParam: window.myCustomParam || null,
      ts: Date.now()
    };
  }


  //分類項目の値取得ヘルパー関数
  function commonGetVal(label, valueFlg = false) {
    let value = ""
    try {
        if ($p.getControl(label).prop("tagName") === "SELECT") {
            if ($p.getControl(label).attr("multiple")) {
                value = valueFlg ? $p.getControl(label).val() : $p.getControl(label).next().children().last().text()
            } else {
                value = valueFlg ? $p.getControl(label).children(':selected').val() : $p.getControl(label).children(':selected').text()
            }
        } else if ($p.getControl(label).prop("tagName") === "INPUT") {
            value = $p.getControl(label).val()
        } else if ($p.getControl($p.getColumnName(label)).prop("tagName") === "TEXTAREA") {
            value = $p.getControl(label).val()
        }
    } catch (e) {
        console.log(label)
        console.log(e)
        value = ""
    } finally {
        return value
    }
  }

  // キャプチャフェーズでクリックを捕まえる（既存 onclick より先に実行される）
  document.addEventListener('click', function(ev){
    try {
      const btn = ev.target.closest && ev.target.closest('button[data-id]');
      if (!btn) return; // 対象外のクリックは無視

      // 一定の条件のみ処理したい場合はここで判定（例：class 名や data-icon 等）
      // if (!btn.classList.contains('applied')) return;

      // ペイロード生成（アプリ側が独自に payload を構築できるようにフックも用意）
      let payload;
      if (typeof window.__pleasanterLinkPayloadProvider === 'function') {
        // もしホストページが独自の provider を定義していればそれを使う
        payload = window.__pleasanterLinkPayloadProvider(btn, ev);
      } else {
        payload = buildPayload(btn, ev);
      }

      // 書き込み（JSON 文字列化）
      sessionStorage.setItem(STORAGE_KEY, JSON.stringify(payload));

      // 重要：ここで何も返さない → 以降に $p.new($(this)) の onclick ハンドラが実行される
    } catch (e) {
      // ログのみ。既存処理を阻害しない。
      console.error('pleasanter: link payload write error', e);
    }
  }, /* useCapture = */ true);
})();