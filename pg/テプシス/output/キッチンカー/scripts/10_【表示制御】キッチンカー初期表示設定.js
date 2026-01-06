(function ($) {
  'use strict';

  /************************************************************************
   * 画面読み込み時に一度だけ実行する初期表示処理
   *
   * 目的：
   *  - ヘッダー（#Header）の高さを取得して、その高さ分だけ
   *    #fn-kitchen-car の top をずらします（重なり防止）。
   *  - さらに #fn-kitchen-car の高さを CSS の calc() を使って
   *    calc(100% - {headerHeight}px) に設定します。
   *  - 店舗マスタ（SiteId: 253154）からデータを取得し、セレクトボックスに設定
   *
   * 注意：
   *  - この処理は「読み込み時に一度だけ実行」されます。
   *  - #fn-kitchen-car に top を効かせるには position が必要です。
   ************************************************************************/

  /* ========================================
   * 定数・変数
   * ======================================== */
  var SHOP_SITE_ID = 253154;
  var KITCHEN_CAR_STATUS_SITE_ID = 253125;
  window.force = true;

  // 店舗データを保持（セレクトボックス変更時に参照）
  var shopRecords = [];

  // キッチンカー状況データを保持
  var kitchenCarStatusRecords = [];
  /* ========================================
   * レイアウト関連
   * ======================================== */

  /**
   * getHeaderHeight
   * - #Header 要素の「見た目の高さ」をピクセル単位で返します。
   * - 要素が見つからなければ 0 を返します。
   *
   * @returns {number} headerHeightPx - 整数ピクセル
   */
  function getHeaderHeight() {
    var $header = $('#Header');

    if ($header.length === 0) {
      return 0;
    }

    var rect = $header[0].getBoundingClientRect();
    var h = Math.round(rect.height || 0);

    return (h > 0) ? h : 0;
  }

  /**
   * buildCalcHeightCss
   * - ヘッダーのピクセル値を受け取り、CSS の calc() 文字列を返します。
   *
   * @param {number} px - ヘッダー高さ（ピクセル）
   * @returns {string} CSS の値
   */
  function buildCalcHeightCss(px) {
    var n = (typeof px === 'number' && !isNaN(px)) ? px : 0;
    return 'calc(100% - ' + String(n) + 'px)';
  }

  /**
   * applyKitchenCarLayout
   * - 実際に DOM に対して style を当てる関数
   *
   * @param {number} headerHeightPx - ヘッダーの高さ（ピクセル）
   */
  function applyKitchenCarLayout(headerHeightPx) {
    var $target = $('#fn-kitchen-car');

    if ($target.length === 0) {
      return;
    }

    $target.css('top', String(headerHeightPx) + 'px');
    $target.css('height', buildCalcHeightCss(headerHeightPx));
  }

  /**
   * initKitchenCarLayout
   * - レイアウト初期化
   */
  function initKitchenCarLayout() {
    var headerH = getHeaderHeight();
    applyKitchenCarLayout(headerH);
  }

  /* ========================================
   * URLパラメータ取得
   * ======================================== */

  /**
   * getUrlParam
   * - URLから指定したパラメータの値を取得
   *
   * @param {string} name - パラメータ名
   * @returns {string|null} パラメータ値（存在しない場合はnull）
   */
  function getUrlParam(name) {
    var params = new URLSearchParams(window.location.search);
    return params.get(name);
  }

  /* ========================================
   * 店舗セレクトボックス初期化
   * ======================================== */

  /**
   * loadShopOptions
   * - 店舗マスタからデータを取得し、セレクトボックスに設定
   * - URLのLinkIdパラメータとClassBを突合してフィルタリング
   */
  async function loadShopOptions() {
    var $select = $('#fn-formShop');
    if ($select.length === 0) {
      return;
    }

    // 既存のオプションをクリア（placeholder以外）
    $select.find('option:not([disabled])').remove();

    // URLからLinkIdを取得
    var linkId = getUrlParam('LinkId');
    window.force && console.log('LinkId:', linkId);

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      var records = await api.getRecords(SHOP_SITE_ID, {
        columns: ['ClassA', 'ClassB', 'DateA', 'DateB'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!records || records.length === 0) {
        window.force && console.warn('店舗データが取得できませんでした');
        return;
      }

      // LinkIdとClassBを突合してフィルタリング
      var filteredRecords = linkId
        ? records.filter(function (record) {
            return String(record.ClassB) === String(linkId);
          })
        : records;

      window.force && console.log('フィルタ後の店舗データ:', filteredRecords);

      // 店舗データを保持
      shopRecords = filteredRecords;

      filteredRecords.forEach(function (record) {
        var name = record.ClassA || '';

        if (name) {
          var $option = $('<option></option>').val(name).text(name);
          $select.append($option);
        }
      });

      // セレクトボックス変更時のイベントハンドラ
      $select.off('change.shop').on('change.shop', handleShopChange);

    } catch (error) {
      window.force && console.error('店舗データ取得エラー:', error);
    }
  }

  /**
   * handleShopChange
   * - 店舗セレクトボックス変更時に開催期間を設定
   */
  function handleShopChange() {
    var selectedValue = $(this).val();
    if (!selectedValue) return;

    // 選択された店舗のレコードを検索
    var selectedRecord = shopRecords.find(function (record) {
      return record.ClassA === selectedValue;
    });

    if (!selectedRecord) {
      window.force && console.warn('選択された店舗のデータが見つかりません:', selectedValue);
      return;
    }

    window.force && console.log('選択された店舗データ:', selectedRecord);

    // DateA → 開催期間From、DateB → 開催期間To
    var dateFrom = selectedRecord.DateA || '';
    var dateTo = selectedRecord.DateB || '';

    // 日付形式を input[type="date"] 用に変換 (YYYY-MM-DD)
    $('#fn-formDateFrom').val(formatDateForInput(dateFrom));
    $('#fn-formDateTo').val(formatDateForInput(dateTo));
  }

  /**
   * formatDateForInput
   * - 日付文字列を input[type="date"] 用のフォーマット (YYYY-MM-DD) に変換
   *
   * @param {string} dateStr - 日付文字列
   * @returns {string} YYYY-MM-DD 形式の日付
   */
  function formatDateForInput(dateStr) {
    if (!dateStr) return '';

    // ISO形式 (2025-01-07T00:00:00) の場合
    if (dateStr.includes('T')) {
      return dateStr.split('T')[0];
    }

    // すでに YYYY-MM-DD 形式の場合
    if (/^\d{4}-\d{2}-\d{2}$/.test(dateStr)) {
      return dateStr;
    }

    // その他の形式の場合はDateオブジェクトで変換を試みる
    try {
      var d = new Date(dateStr);
      if (!isNaN(d.getTime())) {
        return d.toISOString().split('T')[0];
      }
    } catch (e) {
      window.force && console.warn('日付変換エラー:', dateStr, e);
    }

    return '';
  }

  /* ========================================
   * キッチンカー状況テーブル初期化
   * ======================================== */

  /**
   * loadKitchenCarStatus
   * - キッチンカー状況テーブルからデータを取得し、テーブルに表示
   */
  async function loadKitchenCarStatus() {
    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      var records = await api.getRecords(KITCHEN_CAR_STATUS_SITE_ID, {
        columns: ['ClassA', 'ClassB', 'DateA', 'DateB', 'DateC', 'DateD'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!records || records.length === 0) {
        window.force && console.warn('キッチンカー状況データが取得できませんでした');
        return;
      }

      window.force && console.log('キッチンカー状況データ:', records);

      // データを保持
      kitchenCarStatusRecords = records;

      // テーブルに描画
      renderKitchenCarTable(records);

    } catch (error) {
      window.force && console.error('キッチンカー状況データ取得エラー:', error);
    }
  }

  /**
   * getKitchenCarStatus
   * - キッチンカーの使用状況を判定
   *
   * @param {Object} record - キッチンカー状況レコード
   * @returns {Object} { status: string, canSelect: boolean }
   */
  function getKitchenCarStatus(record) {
    var today = new Date();
    today.setHours(0, 0, 0, 0);

    // 使用不可期間チェック (DateC ~ DateD)
    var unavailableStart = record.DateC ? new Date(record.DateC) : null;
    var unavailableEnd = record.DateD ? new Date(record.DateD) : null;

    if (unavailableStart && unavailableEnd) {
      unavailableStart.setHours(0, 0, 0, 0);
      unavailableEnd.setHours(0, 0, 0, 0);

      if (today >= unavailableStart && today <= unavailableEnd) {
        return { status: '使用不可期間', canSelect: false };
      }
    }

    // 直近の予約期間チェック (DateA ~ DateB)
    var reservedStart = record.DateA ? new Date(record.DateA) : null;
    var reservedEnd = record.DateB ? new Date(record.DateB) : null;

    if (reservedStart && reservedEnd) {
      reservedStart.setHours(0, 0, 0, 0);
      reservedEnd.setHours(0, 0, 0, 0);

      if (today >= reservedStart && today <= reservedEnd) {
        var eventName = record.ClassB || '他イベント';
        return { status: eventName + 'で使用中', canSelect: false };
      }
    }

    // 上記以外は使用可能
    return { status: '使用可能', canSelect: true };
  }

  /**
   * renderKitchenCarTable
   * - キッチンカー一覧をテーブルに描画
   *
   * @param {Array} records - キッチンカー状況レコード配列
   */
  function renderKitchenCarTable(records) {
    var $tbody = $('#kc-kitchen-car-table .kc-table__body');
    if ($tbody.length === 0) {
      $tbody = $('#kc-kitchen-car-table tbody');
      if ($tbody.length === 0) {
        window.force && console.error('renderKitchenCarTable: tbody not found');
        return;
      }
    }

    var $template = $tbody.children('tr.fn-tableColumn-loop').first();
    if ($template.length === 0) {
      window.force && console.error('renderKitchenCarTable: template row not found');
      return;
    }

    // テンプレートは非表示のまま保持
    $template.hide();

    // 既存の出力行（テンプレート以外）を削除
    $tbody.find('tr').not($template).remove();

    // DocumentFragment を使ってパフォーマンス向上
    var fragment = document.createDocumentFragment();

    records.forEach(function (record) {
      var $row = $template.clone().removeClass('fn-tableColumn-loop').show();

      // キッチンカー名
      var name = record.ClassA || '';
      $row.find('[data-target="fn-table-name"]').text(name);

      // 使用状況を判定
      var statusInfo = getKitchenCarStatus(record);
      $row.find('[data-target="fn-table-status"]').text(statusInfo.status);

      // チェックボックスの設定
      var $checkbox = $row.find('[data-target="fn-table-checkbox"] .fn-pick');
      if ($checkbox.length) {
        $checkbox.val(name);

        // 使用不可の場合はdisabled
        if (!statusInfo.canSelect) {
          $checkbox.prop('disabled', true);
          $row.addClass('kc-table__row--disabled');
        }
      }

      fragment.appendChild($row.get(0));
    });

    $tbody.get(0).appendChild(fragment);
  }

  /* ========================================
   * 初期化
   * ======================================== */

  document.addEventListener('DOMContentLoaded', function () {
    try {
      // レイアウト初期化
      initKitchenCarLayout();

      // 店舗セレクトボックス初期化
      loadShopOptions();

      // キッチンカー状況テーブル初期化
      loadKitchenCarStatus();

    } catch (e) {
      window.force && console.error('initKitchenCarLayout error', e);
    }
  });

})(jQuery);
