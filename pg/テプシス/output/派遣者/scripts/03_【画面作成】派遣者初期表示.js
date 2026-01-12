(function ($) {
  'use strict';

  /************************************************************************
   * 画面読み込み時に一度だけ実行する初期表示処理
   *
   * 目的：
   *  - ヘッダー（#Header）の高さを取得して、その高さ分だけ
   *    #fn-dispatch の top をずらします（重なり防止）。
   *  - さらに #fn-dispatch の高さを CSS の calc() を使って
   *    calc(100% - {headerHeight}px) に設定します。
   *  - 店舗マスタ（SiteId: 253154）からデータを取得し、セレクトボックスに設定
   *
   * 注意：
   *  - この処理は「読み込み時に一度だけ実行」されます。
   *  - #fn-dispatch に top を効かせるには position が必要です。
   ************************************************************************/

  // 店舗データを保持（セレクトボックス変更時に参照）
  window.shopRecords = [];

  // 編集モード判定（初期化時に設定）
  window.isCreateMode = false;
  // URL由来のモードを保持（動的モード切り替えの判定用）
  window.isUrlCreateMode = false;

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
   * applyDispatchLayout
   * - 実際に DOM に対して style を当てる関数
   *
   * @param {number} headerHeightPx - ヘッダーの高さ（ピクセル）
   */
  function applyDispatchLayout(headerHeightPx) {
    var $target = $('#fn-dispatch');

    if ($target.length === 0) {
      return;
    }

    $target.css('top', String(headerHeightPx) + 'px');
    $target.css('height', buildCalcHeightCss(headerHeightPx));
  }

  /**
   * initDispatchLayout
   * - レイアウト初期化
   */
  function initDispatchLayout() {
    var headerH = getHeaderHeight();
    applyDispatchLayout(headerH);
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
  window.getUrlParam = function (name) {
    var params = new URLSearchParams(window.location.search);
    return params.get(name);
  };

  /* ========================================
   * 店舗セレクトボックス初期化
   * ======================================== */

  /**
   * loadShopOptions
   * - 店舗マスタからデータを取得し、セレクトボックスに設定
   * - 登録パターン: URLのLinkIdでフィルタリング
   * - 更新パターン: $p.id()で既存レコードを取得し、EVENT_IDでフィルタリング
   */
  async function loadShopOptions() {
    var $select = $('#fn-formShop');
    if ($select.length === 0) {
      return;
    }

    // 既存のオプションをクリア（placeholder以外）
    $select.find('option:not([disabled])').remove();

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      // フィルタリング用のイベントIDを取得
      var eventId = null;

      if (window.isUrlCreateMode) {
        // 登録パターン: URLからLinkIdを取得
        eventId = window.getUrlParam('LinkId');
        window.force && console.log('登録パターン - LinkId:', eventId);
      } else {
        // 更新パターン: $p.id()で既存レコードを取得し、EVENT_IDを取得
        var recordId = $p.id();
        if (recordId) {
          var existingRecord = await api.getRecord(recordId, {
            columns: [TABLES.DISPATCH_OUTPUT.COLUMNS.EVENT_ID, TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_NAME, TABLES.DISPATCH_OUTPUT.COLUMNS.DATE],
            setLabelText: false,
            setDisplayValue: 'Value',
          });

          if (existingRecord && existingRecord[TABLES.DISPATCH_OUTPUT.COLUMNS.EVENT_ID]) {
            eventId = existingRecord[TABLES.DISPATCH_OUTPUT.COLUMNS.EVENT_ID];
            window.existingEventId = eventId;
            window.force && console.log('更新パターン - EVENT_ID:', eventId);

            // 既存の店舗名と日付を保持（後でセット）
            window.existingShopName = existingRecord[TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_NAME] || '';
            window.existingDate = existingRecord[TABLES.DISPATCH_OUTPUT.COLUMNS.DATE] || '';
          }
        }
      }

      // イベント予定一覧（PERIOD_SITE_ID）を取得
      var records = await api.getRecords(PERIOD_SITE_ID, {
        columns: [TABLES.PERIOD.COLUMNS.NAME, TABLES.PERIOD.COLUMNS.EVENT_ID, TABLES.PERIOD.COLUMNS.START_DATE, TABLES.PERIOD.COLUMNS.END_DATE, 'ResultId'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!records || records.length === 0) {
        window.force && console.warn('店舗データが取得できませんでした');
        return;
      }

      // eventIdでフィルタリング
      var filteredRecords = eventId
        ? records.filter(function (record) {
            return String(record[TABLES.PERIOD.COLUMNS.EVENT_ID]) === String(eventId);
          })
        : records;

      window.force && console.log('フィルタ後の店舗データ:', filteredRecords);

      // 店舗データを保持
      window.shopRecords = filteredRecords;

      filteredRecords.forEach(function (record) {
        var name = record[TABLES.PERIOD.COLUMNS.NAME] || '';

        if (name) {
          var $option = $('<option></option>').val(name).text(name);
          $select.append($option);
        }
      });

      // セレクトボックス変更時のイベントハンドラ
      $select.off('change.shop').on('change.shop', handleShopChange);

      // 日付変更時のイベントハンドラ
      $('#fn-formDate').off('change.date').on('change.date', handleDateChange);

      // 更新パターン時は既存の店舗と日付をセット、テーブルを表示
      if (!window.isUrlCreateMode && window.existingShopName) {
        // 初期表示時のexistingRecordIdを$p.id()でセット（preChecked判定用）
        window.existingRecordId = $p.id();

        $select.val(window.existingShopName);

        // 日付もセット
        if (window.existingDate) {
          $('#fn-formDate').val(window.formatDateForInput(window.existingDate));
        }

        // 派遣テーブルを表示して描画
        $('#sdt-dispatch-table').show();
        if (typeof window.loadDispatchStatus === 'function') {
          window.loadDispatchStatus();
        }
      }

    } catch (error) {
      window.force && console.error('店舗データ取得エラー:', error);
    }
  }

  /**
   * handleShopChange
   * - 店舗セレクトボックス変更時に日付の範囲制限を設定（派遣は単日入力）
   */
  function handleShopChange() {
    var selectedValue = $(this).val();
    if (!selectedValue) {
      // 店舗未選択時はテーブルを非表示
      $('#sdt-dispatch-table').hide();
      return;
    }

    // 選択された店舗のレコードを検索
    var selectedRecord = window.shopRecords.find(function (record) {
      return record[TABLES.PERIOD.COLUMNS.NAME] === selectedValue;
    });

    if (!selectedRecord) {
      window.force && console.warn('選択された店舗のデータが見つかりません:', selectedValue);
      return;
    }

    window.force && console.log('選択された店舗データ:', selectedRecord);

    // 日付入力のmin/max制限を設定
    var dateFrom = selectedRecord[TABLES.PERIOD.COLUMNS.START_DATE] || '';
    var dateTo = selectedRecord[TABLES.PERIOD.COLUMNS.END_DATE] || '';

    var $dateInput = $('#fn-formDate');
    if ($dateInput.length > 0) {
      $dateInput.attr('min', window.formatDateForInput(dateFrom));
      $dateInput.attr('max', window.formatDateForInput(dateTo));
      // 初期値として開始日を設定
      $dateInput.val(window.formatDateForInput(dateFrom));
    }

    // 派遣テーブルを表示して再描画
    $('#sdt-dispatch-table').show();
    if (typeof window.loadDispatchStatus === 'function') {
      window.loadDispatchStatus();
    }
  }

  /**
   * handleDateChange
   * - 日付変更時に派遣テーブルを再描画
   */
  function handleDateChange() {
    var shopValue = $('#fn-formShop').val();
    var dateValue = $(this).val();

    if (!shopValue || !dateValue) {
      return;
    }

    // 派遣テーブルを再描画（モード切替判定も含む）
    if (typeof window.loadDispatchStatus === 'function') {
      window.loadDispatchStatus();
    }
  }

  /**
   * formatDateForInput
   * - 日付文字列を input[type="date"] 用のフォーマット (YYYY-MM-DD) に変換
   *
   * @param {string} dateStr - 日付文字列
   * @returns {string} YYYY-MM-DD 形式の日付
   */
  window.formatDateForInput = function (dateStr) {
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
  };

  /* ========================================
   * 初期化
   * ======================================== */

  document.addEventListener('DOMContentLoaded', function () {
    try {
      // 編集モード判定（$p.action() が 'NEW' なら作成モード、それ以外は更新モード）
      var action = $p.action();
      window.isCreateMode = String(action).toUpperCase() === 'NEW';
      // URL由来のモードを保持（動的モード切り替えの判定用）
      window.isUrlCreateMode = window.isCreateMode;
      window.force && console.log('$p.action():', action);
      window.force && console.log('isCreateMode:', window.isCreateMode);

      // レイアウト初期化
      initDispatchLayout();

      // モードに応じてタイトル・ボタンテキストを変更
      var modeText = window.isCreateMode ? '登録' : '更新';
      $('.sdt-dispatch-title__text').text('派遣者' + modeText);
      $('#fn-submitButton .sdt-button__text').text(modeText);

      // 店舗セレクトボックス初期化
      // 注: 更新パターン時も店舗は変更可能（EVENT_IDでフィルタリング済み）
      loadShopOptions();

    } catch (e) {
      window.force && console.error('initDispatchLayout error', e);
    }
  });

})(jQuery);
