(function ($) {
  'use strict';

  /************************************************************************
   * キッチンカー状況テーブルの取得・判定・描画
   ************************************************************************/

  // キッチンカー状況データを保持
  window.kitchenCarStatusRecords = [];

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
      window.kitchenCarStatusRecords = records;

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
    var $tbody = $('#sdt-kitchen-car-table .sdt-table__body');
    if ($tbody.length === 0) {
      $tbody = $('#sdt-kitchen-car-table tbody');
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
          $row.addClass('sdt-table__row--disabled');
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
    // キッチンカー状況テーブル初期化
    loadKitchenCarStatus();
  });

})(jQuery);
