(function ($) {
  'use strict';

  /************************************************************************
   * achievements — 最終版
   *
   * 概要（非エンジニア向けの簡単な説明）
   * - ページの「検索」ボタンを押すと、このスクリプトが実行されます。
   * - 処理の流れ：
   *   1) フォームの中の検索条件を読み取る
   *   2) 読み取った条件を画面にポップアップ（alert）で表示する（APIの代替）
   *   3) ダミーのデータを取得する（将来はここをAPIに置き換えます）
   *   4) 取得したデータをテーブルの「テンプレート行」をもとに一覧表示する
   *
   * 重要な設計方針：
   * - 関数はすべて内部に閉じています（グローバルに公開しません）
   * - ボタンはこのスクリプトが読み込まれたあと自動で監視（バインド）します
   * - HTMLのテンプレート行（tr.fn-tableColumn-loop）を残したまま複製して表示行を作ります
   ************************************************************************/

  /* ----------------------------------------
   * 1) フォームから値を集める（安全に）
   * ---------------------------------------- */
  function collectFormValues() {
    // 各フィールドを選んで値を取り出します。要素が無ければ空文字を返して安全にします。
    return {
      eventNo: $('#fn-formEventNo').length ? String($('#fn-formEventNo').val() || '') : '',
      category: $('#fn-formCategory').length ? String($('#fn-formCategory').val() || '') : '',
      eventName: $('#fn-formEventName').length ? String($('#fn-formEventName').val() || '') : '',
      storeName: $('#fn-StoreName').length ? String($('#fn-StoreName').val() || '') : '',
      dateHeld: $('#fn-formdateHeld').length ? String($('#fn-formdateHeld').val() || '') : '',
      eventStatus: $('#fn-EventStatus').length ? String($('#fn-EventStatus').val() || '') : '',
      group: $('#fn-formgroup').length ? String($('#fn-formgroup').val() || '') : '',
      item: $('#fn-formItem').length ? String($('#fn-formItem').val() || '') : ''
    };
  }

  /* ----------------------------------------
   * 2) 画面にアラート表示（わかりやすく）
   * ---------------------------------------- */
  function showFormValuesAlert(values) {
    // values が取れているか確認。無ければ簡単なメッセージ。
    if (!values || typeof values !== 'object') {
      alert('検索条件がありません');
      return;
    }

    // 表示ラベルを用意（英語のキーを日本語に変換）
    var labelMap = {
      eventNo: 'イベント番号',
      category: 'カテゴリ',
      eventName: 'イベント名称・出店先',
      storeName: '事業名・店舗名',
      dateHeld: '開催年月',
      eventStatus: 'イベント状況',
      group: '主管グループ',
      item: '取り扱い品目'
    };

    // 見やすい行リストを作る
    var lines = [];
    Object.keys(values).forEach(function (k) {
      var label = labelMap[k] || k;
      var val = values[k] || '(未指定)';
      lines.push(label + ': ' + val);
    });

    // アラートで表示（要求通り API の代替）
    alert(lines.join('\n'));
  }

  /* ----------------------------------------
   * 3) ダミーデータ取得（将来ここを API に差し替え）
   * ---------------------------------------- */
  function fetchAchievementsDummy(values) {
    // 非同期風に Deferred を返す（将来の $.ajax に置き換えやすい）
    var dfd = $.Deferred();

    var dummy = [
      {
        eventNo: 'A-001',
        category: 'セミナー',
        eventName: 'JS基礎セミナー',
        storeNo: 'S-101',
        storeName: '渋谷店',
        startDate: '2026/02/01',
        endDate: '2026/02/01',
        eventStatus: '完了',
        item: '家電',
        itemDetail: '冷蔵庫',
        totalSales: '¥120,000'
      },
      {
        eventNo: 'A-002',
        category: '展示',
        eventName: '春の展示会',
        storeNo: 'S-202',
        storeName: '大阪店',
        startDate: '2026/02/15',
        endDate: '2026/02/17',
        eventStatus: '開催中',
        item: '家具',
        itemDetail: 'ソファ',
        totalSales: '¥340,000'
      },
      {
        eventNo: 'A-003',
        category: 'ワークショップ',
        eventName: '手作り教室',
        storeNo: 'S-303',
        storeName: '名古屋店',
        startDate: '2026/03/05',
        endDate: '2026/03/05',
        eventStatus: '予定',
        item: 'クラフト',
        itemDetail: '材料セット',
        totalSales: '¥18,000'
      }
    ];

    // すぐ解決（非同期のふり）
    setTimeout(function () { dfd.resolve(dummy); }, 0);
    return dfd.promise();
  }

  /* ----------------------------------------
   * 4) 戻り値の正規化（多次元配列や単一オブジェクトへの対応）
   * ---------------------------------------- */
  function normalizeItems(data) {
    if (!data && data !== 0) return [];
    if (!Array.isArray(data) && typeof data === 'object') {
      return [data];
    }
    if (Array.isArray(data)) {
      var isMulti = data.length > 0 && Array.isArray(data[0]);
      if (isMulti) {
        var flat = [];
        data.forEach(function (el) {
          if (Array.isArray(el)) flat = flat.concat(el);
          else if (typeof el === 'object') flat.push(el);
        });
        return flat.filter(function (x) { return typeof x === 'object'; });
      } else {
        return data.filter(function (x) { return typeof x === 'object'; });
      }
    }
    return [];
  }

  /* ----------------------------------------
   * 5) テーブル「該当なし」行の作成
   * ---------------------------------------- */
  function renderNoResultsRow($tbody) {
    var $table = $tbody.closest('table');
    var colCount = $table.find('thead th').length || 1;
    var $tr = $('<tr>').addClass('achievements-no-data');
    var $td = $('<td>').attr('colspan', colCount).css({ 'text-align': 'center', padding: '12px' }).text('該当データがありません');
    $tr.append($td);
    $tbody.append($tr);
  }

  /* ----------------------------------------
   * 6) テーブルへの描画（テンプレート行を使って挿入）
   *    - ここで thead の display:none を解除する処理を追加
   * ---------------------------------------- */
  function renderAchievementsTable(rawData) {
    // 正規化
    var items = normalizeItems(rawData || []);

    // tbody を見つける（HTML 構造が異なる可能性を少し考慮）
    var $tbody = $('#sdt-achievements-table .sdt-table__body');
    if ($tbody.length === 0) {
      $tbody = $('#sdt-achievements-table tbody');
      if ($tbody.length === 0) {
        window.force && console.error('renderAchievementsTable: tbody not found');
        return;
      }
    }

    // テンプレート行を取得（最初の1行をテンプレートとする）
    var $template = $tbody.children('tr.fn-tableColumn-loop').first();
    if ($template.length === 0) {
      $tbody.empty();
      window.force && console.error('renderAchievementsTable: template row .fn-tableColumn-loop not found');
      return;
    }

    // テンプレートは見えないように隠しておく（テンプレートが画面に残らないように）
    $template.hide();

    // 既存の出力行（テンプレート以外）を削除
    $tbody.find('tr').not($template).remove();

    // データが空なら「該当なし」を表示して終わり（この時ヘッダは非表示のままにしておく仕様）
    if (!Array.isArray(items) || items.length === 0) {
      renderNoResultsRow($tbody);
      return;
    }

    // ここで「thead の display:none を解除」する
    (function unhideTableHeaderIfHidden() {
      var $table = $tbody.closest('table');
      var $thead = $table.find('thead.sdt-table__head');
      if ($thead.length) {
        // インライン style に display:none が設定されている場合のみ解除する
        var inlineDisplay = ($thead[0] && $thead[0].style) ? $thead[0].style.display : '';
        if (inlineDisplay === 'none') {
          // 空文字を入れることでインライン display を解除し、CSS 既定値に戻す
          $thead.css('display', '');
        }
      }
    })();

    // 追加用の DocumentFragment を作る（パフォーマンス向上）
    var fragment = document.createDocumentFragment();

    // data-target -> item プロパティ名のマップ
    var targetMap = {
      'fn-table-eventNo': 'eventNo',
      'fn-table-category': 'category',
      'fn-table-eventName': 'eventName',
      'fn-table-storeNo': 'storeNo',
      'fn-table-storeName': 'storeName',
      'fn-table-startDate': 'startDate',
      'fn-table-endDate': 'endDate',
      'fn-table-eventStatus': 'eventStatus',
      'fn-table-item': 'item',
      'fn-table-itemDetail': 'itemDetail',
      'fn-table-totalSales': 'totalSales'
    };

    // items を1つずつテンプレートから clone して埋める
    items.forEach(function (item) {
      // クローン（テンプレートクラスを外し表示可能にする）
      var $row = $template.clone().removeClass('fn-tableColumn-loop').show();

      // data-target に基づいて各セルにテキストを入れる
      $row.find('[data-target]').each(function () {
        var $cell = $(this);
        var key = $cell.attr('data-target') || '';
        var prop = targetMap[key] || null;
        var text = '';
        if (prop && Object.prototype.hasOwnProperty.call(item, prop)) {
          text = item[prop] === null || typeof item[prop] === 'undefined' ? '' : String(item[prop]);
        }
        // text() を使うことで HTML 注入を防ぎます（安全）
        $cell.text(text);
      });

      // 生の DOM 要素をフラグメントに追加
      fragment.appendChild($row.get(0));
    });

    // まとめて tbody に追加（テンプレートはそのまま残る）
    $tbody.get(0).appendChild(fragment);
  }

  /* ----------------------------------------
   * 7) オーケストレーション（内部で完結）
   *    - ボタンは DOMContentLoaded 時に自動で監視を登録します（外部化しません）
   * ---------------------------------------- */
  function handleSearchButtonClick(evt) {
    // フォーム内のボタンなので、既定の submit を防ぐ
    if (evt && typeof evt.preventDefault === 'function') evt.preventDefault();

    try {
      // 1) フォーム値を取得
      var values = collectFormValues();

      // 2) 取得値を alert で表示（要件）
      showFormValuesAlert(values);

      // 3) ダミー取得（将来はここで API を呼ぶ）
      fetchAchievementsDummy(values).done(function (resp) {
        try {
          // 4) 取得データをテーブルに描画
          renderAchievementsTable(resp);
          window.force && console.log('achievements: rendered items');
        } catch (e) {
          window.force && console.error('achievements: render error', e);
        }
      }).fail(function (err) {
        window.force && console.error('achievements: fetch failed', err);
      });
    } catch (ex) {
      window.force && console.error('achievements: unexpected error', ex);
    }
  }

  /* ----------------------------------------
   * 8) イベントバインド（読み込み時に一度だけ）
   *    - HTML 側に onclick が残っている場合、重複実行を避けるため onclick 属性を削除します
   * ---------------------------------------- */
  document.addEventListener('DOMContentLoaded', function () {
    try {
      // フォーム内の「検索」ボタン（type="button" のもの）をターゲットにする
      var $btn = $('#fn-achievements-form').find('button[type="button"]').first();

      if ($btn.length === 0) {
        // もし見つからなければ、フォーム内の sdt-button のうち type="submit" 以外を試す（互換）
        $btn = $('#fn-achievements-form').find('button').not('[type="reset"]').first();
      }

      if ($btn.length === 0) {
        window.force && console.error('achievements: search button not found for binding');
        return;
      }

      // もし HTML 側に onclick 属性があれば削除して重複実行を避ける
      if ($btn.attr('onclick')) {
        $btn.removeAttr('onclick');
      }

      // 既存の click ハンドラを外してからバインド（多重バインド防止）
      $btn.off('click.achievements').on('click.achievements', handleSearchButtonClick);

      // 必要なら開発時に一度自動で実行することも出来るが、
      // 今回はユーザーの操作（ボタンクリック）を待ちます。
    } catch (e) {
      window.force && console.error('achievements: bind error', e);
    }
  });


})(jQuery);
