(function(){
  // sessionStorage のキー名
  const STORAGE_KEY = 'pleasanter.linkPayload';

  // ペイロードを生成する関数（必要ならこの関数をカスタマイズ）
  // targetBtn: DOM element（クリックされたボタン）
  // ev: MouseEvent
  function buildPayload(targetBtn, ev) {
    // ボタンの所属行
    const $row = $(targetBtn).closest('tr');

    // 例：ボタンの data-* 属性を渡す + タイムスタンプ + 任意パラメータ
    return {
      fromDataId: targetBtn.dataset.id || null,
      fromSiteId: targetBtn.dataset.fromSiteId || null,
      toSiteId: targetBtn.dataset.toSiteId || null,

      StoreManageMethodId: $p.getControl('ClassI').val(),
      StoreManageMethod: commonGetVal('ClassI'),
      PerStoreId: $p.getControl('ClassJ').val(),
      PerStoreName: commonGetVal('ClassJ'),
      NumberOfStoreId: $p.getControl('ClassK').val(),
      NumberOfStoreName: commonGetVal('ClassK'),
      EventDate: $p.getControl('DateC').val(),

      dateA: $row.find('td[data-name="ClassB~~2832879,DateA"]').text().trim(),
      dateB: $row.find('td[data-name="ClassB~~2832879,DateB"]').text().trim(),

      listSiteId: $p.siteId,

      customParam: window.myCustomParam || null,
      ts: Date.now()
    };
  }

  // 分類項目の値取得ヘルパー関数
  function commonGetVal(label, valueFlg = false) {
    let value = "";
    try {
      if ($p.getControl(label).prop("tagName") === "SELECT") {
        if ($p.getControl(label).attr("multiple")) {
          value = valueFlg ? $p.getControl(label).val() : $p.getControl(label).next().children().last().text();
        } else {
          value = valueFlg ? $p.getControl(label).children(':selected').val() : $p.getControl(label).children(':selected').text();
        }
      } else if ($p.getControl(label).prop("tagName") === "INPUT") {
        value = $p.getControl(label).val();
      } else if ($p.getControl($p.getColumnName(label)).prop("tagName") === "TEXTAREA") {
        value = $p.getControl(label).val();
      }
    } catch (e) {
      console.log(label, e);
      value = "";
    } finally {
      return value;
    }
  }

  // オーバーレイの作成
  function showOverlay() {
    if(document.getElementById('overlay-screen')) return;
    const overlay = document.createElement('div');
    overlay.id = 'overlay-screen';
    overlay.style.position = 'fixed';
    overlay.style.top = 0;
    overlay.style.left = 0;
    overlay.style.width = '100%';
    overlay.style.height = '100%';
    overlay.style.backgroundColor = '#fff';
    overlay.style.zIndex = 9999;
    document.body.appendChild(overlay);
  }

  // オーバーレイ解除（siteID 判定）
  function removeOverlay(payload) {
    const currentSiteId = $p.siteId;
    const targetSiteId = payload.toSiteId || payload.listSiteId;

    if(currentSiteId === targetSiteId || currentSiteId === LIST_SCREEN_SITE_ID){
      const overlayEl = document.getElementById('overlay-screen');
      if(overlayEl) overlayEl.remove();
    }
  }

  // 画面遷移の処理
  function processScreens(payload) {
    showOverlay();

    // まず個別イベント管理のテーブルをチェック
    $.get(urlA, function(dataA){
      const docA = $(dataA);
      const rowA = docA.find('td[data-name="ClassB~~2832879,DateA"], td[data-name="ClassB~~2832879,DateB"]');
      const matchA = rowA.filter(function(i, td){
        const val = $(td).text().trim();
        return val === payload.dateA || val === payload.dateB;
      });

      if(matchA.length === 0){
        console.log('条件不一致: 個別イベント管理');
        removeOverlay(payload);
        window.location.href = `/items/${LIST_SCREEN_SITE_ID}/index`;
        return;
      }

      // 条件一致 → 開始～終了期間画面へ
      $.get(urlB, function(dataB){
        const docB = $(dataB);
        const rowB = docB.find('td[data-name="ClassB~~2832879,DateA"], td[data-name="ClassB~~2832879,DateB"]');
        const matchB = rowB.filter(function(i, td){
          const val = $(td).text().trim();
          return val === payload.dateA || val === payload.dateB;
        });

        if(matchB.length > 0){
          // 条件一致 → レコードをクリックして最終画面へ
          const recordId = matchB.first().closest('tr.grid-row').data('id');
          $(function () {
            $('tr.grid-row[data-id=' + recordId + '] td').first().trigger('click');
          });
        } else {
          // 条件不一致 → 作成ボタンを押下
          $(docB).find('button[data-id="2842211"]').trigger('click');
        }

        removeOverlay(payload);
      });
    });
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

      // 画面遷移処理
      processScreens(payload);

      // 重要：ここで何も返さない → 以降に $p.new($(this)) の onclick ハンドラが実行される
    } catch (e) {
      // ログのみ。既存処理を阻害しない。
      console.error('pleasanter: link payload write error', e);
    }
  }, /* useCapture = */ true);

})();