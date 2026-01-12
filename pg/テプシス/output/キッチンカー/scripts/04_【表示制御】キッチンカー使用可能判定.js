(function ($) {
  'use strict';

  /************************************************************************
   * キッチンカー状況テーブルの取得・判定・描画
   ************************************************************************/

  // キッチンカー状況データを保持
  window.kitchenCarStatusRecords = [];

  // キッチンカー出力テーブルの予約データを保持
  window.kitchenCarOutputRecords = [];

  // 更新モード初期表示時の既存キッチンカーIDs（チェック復元用）
  window.existingKitchenCarIds = [];

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

      // キッチンカー状況テーブルから取得（RESERVED_FROM/RESERVED_TO は不要）
      var records = await api.getRecords(KITCHEN_CAR_STATUS_SITE_ID, {
        columns: [TABLES.KITCHEN_CAR_STATUS.COLUMNS.NAME, TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_FROM, TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_TO, 'ResultId'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!records || records.length === 0) {
        window.force && console.warn('キッチンカー状況データが取得できませんでした');
        return;
      }

      window.force && console.log('キッチンカー状況データ:', records);

      // キッチンカー出力テーブル（253143）から全件取得
      var outputRecords = await api.getRecords(KITCHEN_CAR_OUTPUT_SITE_ID, {
        columns: [TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.KITCHEN_CAR_IDS, TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_FROM, TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO, TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.SHOP_NAME, TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.NOTE, 'ResultId'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      window.force && console.log('キッチンカー出力データ:', outputRecords);
      window.kitchenCarOutputRecords = outputRecords || [];

      // モード判定（2x2マトリクス）
      // ┌─────────────┬──────────────────┬──────────────────┐
      // │             │ URLパターン=NEW  │ URLパターン=Edit │
      // │             │ (店舗変更可)     │ (店舗変更可)     │
      // ├─────────────┼──────────────────┼──────────────────┤
      // │ レコードあり │ 更新モード       │ 更新モード       │
      // │ レコードなし │ 登録モード       │ 登録モード       │
      // └─────────────┴──────────────────┴──────────────────┘
      // - isUrlCreateMode: URLパターン（EVENT_ID取得方法を決定）
      // - isCreateMode: 実際のモード（API処理 create/update を決定）
      // - キッチンカーは「店舗名」でレコードを特定
      // - 店舗変更時は常にモード判定を行う
      var formShopName = $('#fn-formShop').val() || '';
      var existingRecord = (outputRecords || []).find(function (rec) {
        return rec[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.SHOP_NAME] === formShopName;
      });

      if (existingRecord) {
        // レコードあり → 更新モード
        window.isCreateMode = false;
        window.existingRecordId = existingRecord.ResultId;
        window.force && console.log('同店舗レコード発見、更新モードに切り替え:', existingRecord.ResultId);

        // タイトル・ボタンテキストを更新
        $('.sdt-kitchen-car-title__text').text('キッチンカー更新');
        $('#fn-submitButton .sdt-button__text').text('更新');

        // 削除ボタンを表示
        $('#fn-deleteButton').show();

        // 既存レコードの日付・その他詳細をフォームにセット
        var existingDateFrom = existingRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_FROM] || '';
        var existingDateTo = existingRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO] || '';
        var existingNote = existingRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.NOTE] || '';

        if (existingDateFrom) {
          $('#fn-formDateFrom').val(window.formatDateForInput(existingDateFrom));
        }
        if (existingDateTo) {
          $('#fn-formDateTo').val(window.formatDateForInput(existingDateTo));
        }
        $('#fn-formNote').val(existingNote);

        // 更新モード初期表示時（URLから直接開いた時）は既存のKITCHEN_CAR_IDSを保持
        if (!window.isUrlCreateMode) {
          try {
            window.existingKitchenCarIds = JSON.parse(existingRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.KITCHEN_CAR_IDS] || '[]');
          } catch (e) {
            window.existingKitchenCarIds = [];
          }
          window.force && console.log('既存キッチンカーIDs:', window.existingKitchenCarIds);
        }
      } else {
        // レコードなし → 登録モード
        window.isCreateMode = true;
        window.existingRecordId = null;
        window.force && console.log('同店舗レコードなし、登録モードに切り替え');

        // タイトル・ボタンテキストを登録に変更
        $('.sdt-kitchen-car-title__text').text('キッチンカー登録');
        $('#fn-submitButton .sdt-button__text').text('登録');

        // 削除ボタンを非表示
        $('#fn-deleteButton').hide();

        // 登録モード時はその他詳細をクリア
        $('#fn-formNote').val('');
      }

      // データを保持
      window.kitchenCarStatusRecords = records;

      // テーブルに描画
      renderKitchenCarTable(records);

    } catch (error) {
      window.force && console.error('キッチンカー状況データ取得エラー:', error);
    }
  }

  /**
   * isDateRangeOverlap
   * - 2つの期間が重複しているかを判定
   *
   * @param {Date} start1 - 期間1の開始日
   * @param {Date} end1 - 期間1の終了日
   * @param {Date} start2 - 期間2の開始日
   * @param {Date} end2 - 期間2の終了日
   * @returns {boolean} 重複していればtrue
   */
  function isDateRangeOverlap(start1, end1, start2, end2) {
    return start1 <= end2 && end1 >= start2;
  }

  /**
   * getKitchenCarStatus
   * - キッチンカーの使用状況を判定
   * - フォームの開催期間とKITCHEN_CAR_OUTPUTの期間が重複していれば「使用中」
   *
   * @param {Object} record - キッチンカー状況レコード
   * @returns {Object} { status: string, canSelect: boolean }
   */
  function getKitchenCarStatus(record) {
    var kitchenCarResultId = String(record.ResultId || '');

    // フォームから開催期間を取得
    var formDateFrom = $('#fn-formDateFrom').val();
    var formDateTo = $('#fn-formDateTo').val();

    // 使用不可期間チェック (UNAVAILABLE_FROM ~ UNAVAILABLE_TO)
    var unavailableStart = record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_FROM] ? new Date(record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_FROM]) : null;
    var unavailableEnd = record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_TO] ? new Date(record[TABLES.KITCHEN_CAR_STATUS.COLUMNS.UNAVAILABLE_TO]) : null;

    if (unavailableStart && unavailableEnd) {
      unavailableStart.setHours(0, 0, 0, 0);
      unavailableEnd.setHours(0, 0, 0, 0);

      // フォームの期間と使用不可期間が重複しているかチェック
      if (formDateFrom && formDateTo) {
        var formStart = new Date(formDateFrom);
        var formEnd = new Date(formDateTo);
        formStart.setHours(0, 0, 0, 0);
        formEnd.setHours(0, 0, 0, 0);

        if (isDateRangeOverlap(formStart, formEnd, unavailableStart, unavailableEnd)) {
          return { status: '使用不可期間', canSelect: false };
        }
      }
    }

    // KITCHEN_CAR_OUTPUT（253143）の予約データと期間重複チェック
    // 同店舗で使用中の場合はpreCheckedフラグを立てる
    var preChecked = false;
    var formShopName = $('#fn-formShop').val() || '';

    if (formDateFrom && formDateTo && window.kitchenCarOutputRecords.length > 0) {
      var formStart = new Date(formDateFrom);
      var formEnd = new Date(formDateTo);
      formStart.setHours(0, 0, 0, 0);
      formEnd.setHours(0, 0, 0, 0);

      // 更新モード時は自分自身のレコードを除外
      var currentRecordId = !window.isCreateMode ? String(window.existingRecordId || $p.id()) : '';

      for (var i = 0; i < window.kitchenCarOutputRecords.length; i++) {
        var outputRecord = window.kitchenCarOutputRecords[i];

        // 自分自身のレコードは除外
        if (currentRecordId && String(outputRecord.ResultId) === currentRecordId) {
          continue;
        }

        // KITCHEN_CAR_IDS（JSON配列）に該当キッチンカーが含まれているかチェック
        var kitchenCarIds = [];
        try {
          kitchenCarIds = JSON.parse(outputRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.KITCHEN_CAR_IDS] || '[]');
        } catch (e) {
          kitchenCarIds = [];
        }

        if (kitchenCarIds.indexOf(kitchenCarResultId) < 0 && kitchenCarIds.indexOf(Number(kitchenCarResultId)) < 0) {
          continue;
        }

        // 期間重複チェック
        var reservedFrom = outputRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_FROM] ? new Date(outputRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_FROM]) : null;
        var reservedTo = outputRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO] ? new Date(outputRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO]) : null;

        if (reservedFrom && reservedTo) {
          reservedFrom.setHours(0, 0, 0, 0);
          reservedTo.setHours(0, 0, 0, 0);

          if (isDateRangeOverlap(formStart, formEnd, reservedFrom, reservedTo)) {
            var shopName = outputRecord[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.SHOP_NAME] || '他店舗';

            // 同店舗の場合は使用可能＋チェック済みとする
            if (shopName === formShopName) {
              preChecked = true;
              continue;
            }

            return { status: shopName + 'で使用中', canSelect: false, preChecked: false };
          }
        }
      }
    }

    // 上記以外は使用可能
    return { status: '使用可能', canSelect: true, preChecked: preChecked };
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

    // キッチンカー名で昇順ソート
    var sortedRecords = records.slice().sort(function (a, b) {
      var nameA = a[TABLES.KITCHEN_CAR_STATUS.COLUMNS.NAME] || '';
      var nameB = b[TABLES.KITCHEN_CAR_STATUS.COLUMNS.NAME] || '';
      return nameA.localeCompare(nameB, 'ja');
    });

    // DocumentFragment を使ってパフォーマンス向上
    var fragment = document.createDocumentFragment();

    sortedRecords.forEach(function (record) {
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

        // チェック状態の設定
        var resultId = String(record.ResultId || '');
        if (!window.isUrlCreateMode && window.existingKitchenCarIds.length > 0) {
          // 更新モード初期表示時: 既存のKITCHEN_CAR_IDSに含まれていればチェック
          if (window.existingKitchenCarIds.indexOf(resultId) >= 0 || window.existingKitchenCarIds.indexOf(Number(resultId)) >= 0) {
            $checkbox.prop('checked', true);
          }
        } else if (statusInfo.preChecked && window.isUrlCreateMode) {
          // 登録モードで店舗切替時: 同店舗で既に使用中の場合はチェック
          $checkbox.prop('checked', true);
        }
      }

      fragment.appendChild($row.get(0));
    });

    $tbody.get(0).appendChild(fragment);

    // ボタン活性状態を更新
    if (typeof window.updateSubmitButtonState === 'function') {
      window.updateSubmitButtonState();
    }
  }

  /* ========================================
   * 初期化
   * ======================================== */

  // loadKitchenCarStatusをグローバルに公開（店舗変更時に呼び出し）
  window.loadKitchenCarStatus = loadKitchenCarStatus;

  document.addEventListener('DOMContentLoaded', function () {
    // 作成モード時はテーブルを非表示（店舗選択後に表示）
    // 更新モード時は既存データがあるので表示
    if (window.isCreateMode) {
      $('#sdt-kitchen-car-table').hide();
    }
  });

})(jQuery);
