(function ($) {
  'use strict';

  /************************************************************************
   * 派遣状況テーブルの取得・判定・描画
   ************************************************************************/

  // 派遣状況データを保持
  window.dispatchStatusRecords = [];

  /* ========================================
   * 派遣状況テーブル初期化
   * ======================================== */

  /**
   * loadDispatchStatus
   * - 派遣状況テーブルからデータを取得し、テーブルに表示
   */
  async function loadDispatchStatus() {
    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      var records = await api.getRecords(DISPATCH_STATUS_SITE_ID, {
        columns: [TABLES.DISPATCH_STATUS.COLUMNS.NAME, TABLES.DISPATCH_STATUS.COLUMNS.EVENT_NAME, TABLES.DISPATCH_STATUS.COLUMNS.RESERVED_DATE, TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_FROM, TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_TO, 'ResultId'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!records || records.length === 0) {
        window.force && console.warn('派遣状況データが取得できませんでした');
        return;
      }

      window.force && console.log('派遣状況データ:', records);

      // データを保持
      window.dispatchStatusRecords = records;

      // テーブルに描画
      renderDispatchTable(records);

    } catch (error) {
      window.force && console.error('派遣状況データ取得エラー:', error);
    }
  }

  /**
   * getDispatchStatus
   * - 派遣者の使用状況を判定
   *
   * @param {Object} record - 派遣状況レコード
   * @returns {Object} { status: string, canSelect: boolean }
   */
  function getDispatchStatus(record) {
    var today = new Date();
    today.setHours(0, 0, 0, 0);

    // 派遣不可期間チェック (UNAVAILABLE_FROM ~ UNAVAILABLE_TO)
    var unavailableStart = record[TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_FROM] ? new Date(record[TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_FROM]) : null;
    var unavailableEnd = record[TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_TO] ? new Date(record[TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_TO]) : null;

    if (unavailableStart && unavailableEnd) {
      unavailableStart.setHours(0, 0, 0, 0);
      unavailableEnd.setHours(0, 0, 0, 0);

      if (today >= unavailableStart && today <= unavailableEnd) {
        return { status: '派遣不可期間', canSelect: false };
      }
    }

    // 直近の派遣日チェック (RESERVED_DATE) - 派遣は単日
    var reservedDate = record[TABLES.DISPATCH_STATUS.COLUMNS.RESERVED_DATE] ? new Date(record[TABLES.DISPATCH_STATUS.COLUMNS.RESERVED_DATE]) : null;

    if (reservedDate) {
      reservedDate.setHours(0, 0, 0, 0);

      if (today.getTime() === reservedDate.getTime()) {
        var eventName = record[TABLES.DISPATCH_STATUS.COLUMNS.EVENT_NAME] || '他イベント';
        return { status: eventName + 'で派遣中', canSelect: false };
      }
    }

    // 上記以外は派遣可能
    return { status: '派遣可能', canSelect: true };
  }

  /**
   * renderDispatchTable
   * - 派遣者一覧をテーブルに描画
   *
   * @param {Array} records - 派遣状況レコード配列
   */
  function renderDispatchTable(records) {
    var $tbody = $('#sdt-dispatch-table .sdt-table__body');
    if ($tbody.length === 0) {
      $tbody = $('#sdt-dispatch-table tbody');
      if ($tbody.length === 0) {
        window.force && console.error('renderDispatchTable: tbody not found');
        return;
      }
    }

    var $template = $tbody.children('tr.fn-tableColumn-loop').first();
    if ($template.length === 0) {
      window.force && console.error('renderDispatchTable: template row not found');
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

      // 派遣者名
      var name = record[TABLES.DISPATCH_STATUS.COLUMNS.NAME] || '';
      $row.find('[data-target="fn-table-name"]').text(name);

      // 使用状況を判定
      var statusInfo = getDispatchStatus(record);
      $row.find('[data-target="fn-table-status"]').text(statusInfo.status);

      // チェックボックスの設定（valueはResultId、表示は派遣者名）
      var $checkbox = $row.find('[data-target="fn-table-checkbox"] .fn-pick');
      if ($checkbox.length) {
        // valueにはResultIdを設定（ClassCに登録する値）
        $checkbox.val(record.ResultId || '');

        // 派遣不可の場合はdisabled
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
    // 派遣状況テーブル初期化
    loadDispatchStatus();
  });

})(jQuery);
