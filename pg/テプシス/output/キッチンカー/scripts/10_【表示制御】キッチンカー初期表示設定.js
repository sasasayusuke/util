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
  var KITCHEN_CAR_OUTPUT_SITE_ID = 253143;
  var KITCHEN_CAR_STATUS_SITE_ID = 253125;
  var PERIOD_SITE_ID = 253154;
  window.force = true;

  // テーブル・カラム定義
  var TABLES = {
    // キッチンカーテーブル
    KITCHEN_CAR_OUTPUT: {
      SITE_ID: KITCHEN_CAR_OUTPUT_SITE_ID,
      COLUMNS: {
        KITCHEN_CAR_IDS: 'ClassA',   // キッチンカーResultId（JSON配列）
        EVENT_ID: 'ClassB',          // LinkId/イベントID（作成時のみ）
        SHOP_NAME: 'ClassC',         // 店舗名
        NOTE: 'ClassD',              // その他詳細
        SHOP_RESULT_ID: 'ClassY',    // 店舗ResultId（作成時のみ）
        DATE_FROM: 'DateA',          // 開催期間（開始日）
        DATE_TO: 'DateB'             // 開催期間（終了日）
      }
    },
    // キッチンカー状況テーブル
    KITCHEN_CAR_STATUS: {
      SITE_ID: KITCHEN_CAR_STATUS_SITE_ID,
      COLUMNS: {
        NAME: 'ClassA',              // キッチンカー名
        EVENT_NAME: 'ClassB',        // 使用先イベント名称
        RESERVED_FROM: 'DateA',      // 直近の予約期間（開始日）
        RESERVED_TO: 'DateB',        // 直近の予約期間（終了日）
        UNAVAILABLE_FROM: 'DateC',   // 使用不可期間（開始日）
        UNAVAILABLE_TO: 'DateD'      // 使用不可期間（終了日）
      }
    },
    // 開始～終了期間テーブル
    PERIOD: {
      SITE_ID: PERIOD_SITE_ID,
      COLUMNS: {
        NAME: 'ClassA',              // 店舗名
        EVENT_ID: 'ClassB',          // イベントID（LinkId）
        START_DATE: 'DateA',         // 開始日
        END_DATE: 'DateB'            // 終了日
      }
    }
  };

  // 店舗データを保持（セレクトボックス変更時に参照）
  var shopRecords = [];

  // キッチンカー状況データを保持
  var kitchenCarStatusRecords = [];

  // 編集モード判定（初期化時に設定）
  var isCreateMode = false;
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

      var records = await api.getRecords(PERIOD_SITE_ID, {
        columns: [TABLES.PERIOD.COLUMNS.NAME, TABLES.PERIOD.COLUMNS.EVENT_ID, TABLES.PERIOD.COLUMNS.START_DATE, TABLES.PERIOD.COLUMNS.END_DATE, 'ResultId'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!records || records.length === 0) {
        window.force && console.warn('店舗データが取得できませんでした');
        return;
      }

      // LinkIdとEVENT_IDを突合してフィルタリング
      var filteredRecords = linkId
        ? records.filter(function (record) {
            return String(record[TABLES.PERIOD.COLUMNS.EVENT_ID]) === String(linkId);
          })
        : records;

      window.force && console.log('フィルタ後の店舗データ:', filteredRecords);

      // 店舗データを保持
      shopRecords = filteredRecords;

      filteredRecords.forEach(function (record) {
        var name = record[TABLES.PERIOD.COLUMNS.NAME] || '';

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
      return record[TABLES.PERIOD.COLUMNS.NAME] === selectedValue;
    });

    if (!selectedRecord) {
      window.force && console.warn('選択された店舗のデータが見つかりません:', selectedValue);
      return;
    }

    window.force && console.log('選択された店舗データ:', selectedRecord);

    // START_DATE → 開始日、END_DATE → 終了日
    var dateFrom = selectedRecord[TABLES.PERIOD.COLUMNS.START_DATE] || '';
    var dateTo = selectedRecord[TABLES.PERIOD.COLUMNS.END_DATE] || '';

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
        columns: [TABLES.KITCHEN_CAR_STATUS.COLUMNS.NAME, TABLES.KITCHEN_CAR_STATUS.COLUMNS.EVENT_NAME, TABLES.KITCHEN_CAR_STATUS.COLUMNS.RESERVED_FROM, TABLES.KITCHEN_CAR_STATUS.COLUMNS.RESERVED_TO, TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_FROM, TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_TO, 'ResultId'],
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

    // 使用不可期間チェック (UNAVAILABLE_FROM ~ UNAVAILABLE_TO)
    var unavailableStart = record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_FROM] ? new Date(record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_FROM]) : null;
    var unavailableEnd = record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_TO] ? new Date(record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_TO]) : null;

    if (unavailableStart && unavailableEnd) {
      unavailableStart.setHours(0, 0, 0, 0);
      unavailableEnd.setHours(0, 0, 0, 0);

      if (today >= unavailableStart && today <= unavailableEnd) {
        return { status: '使用不可期間', canSelect: false };
      }
    }

    // 直近の予約期間チェック (RESERVED_FROM ~ RESERVED_TO)
    var reservedStart = record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.RESERVED_FROM] ? new Date(record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.RESERVED_FROM]) : null;
    var reservedEnd = record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.RESERVED_TO] ? new Date(record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.RESERVED_TO]) : null;

    if (reservedStart && reservedEnd) {
      reservedStart.setHours(0, 0, 0, 0);
      reservedEnd.setHours(0, 0, 0, 0);

      if (today >= reservedStart && today <= reservedEnd) {
        var eventName = record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.EVENT_NAME] || '他イベント';
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
      var name = record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.NAME] || '';
      $row.find('[data-target="fn-table-name"]').text(name);

      // 使用状況を判定
      var statusInfo = getKitchenCarStatus(record);
      $row.find('[data-target="fn-table-status"]').text(statusInfo.status);

      // チェックボックスの設定（valueはResultId、表示はキッチンカー名）
      var $checkbox = $row.find('[data-target="fn-table-checkbox"] .fn-pick');
      if ($checkbox.length) {
        // valueにはResultIdを設定（ClassAに登録する値）
        $checkbox.val(record.ResultId || '');

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
   * 既存データ読み込み（更新モード用）
   * ======================================== */

  /**
   * loadExistingData
   * - 更新モード時に既存レコードのデータを取得してフォームにセット
   */
  async function loadExistingData() {
    var recordId = $p.id();
    if (!recordId) {
      window.force && console.warn('レコードIDが取得できません');
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      var record = await api.getRecord(recordId, {
        columns: [TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.SHOP_NAME, TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.KITCHEN_CAR_IDS, TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_FROM, TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO, TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.NOTE, TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.EVENT_ID],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!record || Object.keys(record).length === 0) {
        window.force && console.warn('既存データが取得できませんでした');
        return;
      }

      window.force && console.log('既存データ:', record);

      // 店舗セレクトボックスに値をセット
      if (record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.SHOP_NAME]) {
        $('#fn-formShop').val(record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.SHOP_NAME]);
      }

      // 開催期間をセット
      if (record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_FROM]) {
        $('#fn-formDateFrom').val(formatDateForInput(record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_FROM]));
      }
      if (record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO]) {
        $('#fn-formDateTo').val(formatDateForInput(record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO]));
      }

      // その他詳細をセット
      if (record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.NOTE]) {
        $('#fn-formNote').val(record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.NOTE]);
      }

      // キッチンカー選択をセット（JSON配列形式のResultId）
      if (record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.KITCHEN_CAR_IDS]) {
        var selectedCarIds = [];
        try {
          selectedCarIds = JSON.parse(record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.KITCHEN_CAR_IDS]);
        } catch (e) {
          // JSON形式でない場合はカンマ区切りとして処理（後方互換）
          selectedCarIds = String(record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.KITCHEN_CAR_IDS]).split(',').map(function (s) {
            return s.trim();
          });
        }

        // チェックボックスにチェックを入れる（valueはResultId）
        $('.fn-pick').each(function () {
          var $checkbox = $(this);
          var carId = String($checkbox.val());
          if (selectedCarIds.indexOf(carId) >= 0 && !$checkbox.prop('disabled')) {
            $checkbox.prop('checked', true);
          }
        });
      }

    } catch (error) {
      window.force && console.error('既存データ取得エラー:', error);
    }
  }

  /* ========================================
   * 登録処理
   * ======================================== */

  /**
   * validateForm
   * - フォームのバリデーション
   *
   * @returns {Object|null} エラーがあればnull、なければフォームデータ
   */
  function validateForm() {
    var shop = $('#fn-formShop').val();
    var dateFrom = $('#fn-formDateFrom').val();
    var dateTo = $('#fn-formDateTo').val();
    var selectedCars = [];

    // 選択されたキッチンカーを取得
    $('.fn-pick:checked:not(:disabled)').each(function () {
      selectedCars.push($(this).val());
    });

    var errors = [];

    if (!shop) {
      errors.push('店舗を選択してください');
    }
    if (!dateFrom) {
      errors.push('開催期間（開始日）を入力してください');
    }
    if (!dateTo) {
      errors.push('開催期間（終了日）を入力してください');
    }
    if (dateFrom && dateTo && dateFrom > dateTo) {
      errors.push('開催期間の開始日は終了日より前の日付を指定してください');
    }
    if (selectedCars.length === 0) {
      errors.push('キッチンカーを1台以上選択してください');
    }

    if (errors.length > 0) {
      alert(errors.join('\n'));
      return null;
    }

    return {
      shop: shop,
      dateFrom: dateFrom,
      dateTo: dateTo,
      selectedCars: selectedCars,
      note: $('#fn-formNote').val() || ''
    };
  }

  /**
   * handleSubmit
   * - 登録/更新ボタンクリック時の処理
   * - isCreateMode: true → 作成、false → 更新（$p.idを使用）
   */
  async function handleSubmit() {
    var formData = validateForm();
    if (!formData) return;

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      // キッチンカーResultIdは複数選択をJSON配列形式で登録
      var kitchenCarIds = JSON.stringify(formData.selectedCars);

      var data = {};
      data[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.SHOP_NAME] = formData.shop;
      data[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.KITCHEN_CAR_IDS] = kitchenCarIds;
      data[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_FROM] = formData.dateFrom;
      data[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO] = formData.dateTo;
      data[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.NOTE] = formData.note;

      window.force && console.log('モード:', isCreateMode ? '作成' : '更新');
      window.force && console.log('データ:', data);

      var result;
      if (isCreateMode) {
        // 作成モード時のみEVENT_ID（LinkId/イベントID）とSHOP_RESULT_ID（店舗ResultId）をセット
        var linkId = getUrlParam('LinkId');
        data[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.EVENT_ID] = linkId || '';

        var selectedShop = shopRecords.find(function (record) {
          return record[TABLES.PERIOD.COLUMNS.NAME] === formData.shop;
        });

        if (selectedShop && selectedShop.ResultId) {
          data[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.SHOP_RESULT_ID] = String(selectedShop.ResultId);
          window.force && console.log('店舗ResultId:', selectedShop.ResultId);
        } else {
          alert('店舗のResultIdが取得できません。');
          return;
        }

        result = await api.createRecord($p.siteId(), data);
        window.force && console.log('作成結果:', result);
        alert('キッチンカーを登録しました');
      } else {
        // 更新モード（$p.id()を使用）
        var recordId = $p.id();
        if (!recordId) {
          alert('レコードIDが取得できません。');
          return;
        }
        window.force && console.log('recordId:', recordId);
        // updateRecordは (recordId, updateData) の2引数
        result = await api.updateRecord(recordId, data);
        window.force && console.log('更新結果:', result);
        alert('キッチンカーを更新しました');
      }

      // 処理後は画面を閉じる（または元の画面に戻る）
      handleCancel();

    } catch (error) {
      window.force && console.error('処理エラー:', error);
      alert((isCreateMode ? '登録' : '更新') + 'に失敗しました: ' + (error.message || error));
    }
  }

  /**
   * handleCancel
   * - キャンセルボタンクリック時の処理
   * - 前の画面に戻る
   */
  function handleCancel() {
    history.back();
  }

  /* ========================================
   * 初期化
   * ======================================== */

  document.addEventListener('DOMContentLoaded', function () {
    try {
      // 編集モード判定（$p.action() が 'NEW' なら作成モード、それ以外は更新モード）
      var action = $p.action();
      isCreateMode = String(action).toUpperCase() === 'NEW';
      window.force && console.log('$p.action():', action);
      window.force && console.log('isCreateMode:', isCreateMode);

      // レイアウト初期化
      initKitchenCarLayout();

      // モードに応じてタイトル・ボタンテキストを変更
      var modeText = isCreateMode ? 'キッチンカー登録' : 'キッチンカー更新';
      $('.kc-kitchen-car-title__text').text(modeText);
      $('#fn-submitButton .kc-button__text').text(modeText);

      // 店舗セレクトボックス初期化
      loadShopOptions();

      // キッチンカー状況テーブル初期化
      loadKitchenCarStatus();

      // 更新モード時は既存データを読み込み（テーブル描画後に実行）
      if (!isCreateMode) {
        // テーブル描画完了を待ってから既存データをセット
        setTimeout(function () {
          loadExistingData();
        }, 500);
      }

      // ボタンイベント設定（重複防止のため.off()で既存イベント解除）
      $('#fn-submitButton').off('click').on('click', handleSubmit);
      $('#fn-cancelButton').off('click').on('click', handleCancel);

    } catch (e) {
      window.force && console.error('initKitchenCarLayout error', e);
    }
  });

})(jQuery);
