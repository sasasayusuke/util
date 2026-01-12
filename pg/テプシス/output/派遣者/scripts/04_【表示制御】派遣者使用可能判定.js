(function ($) {
  'use strict';

  /************************************************************************
   * 派遣状況テーブルの取得・判定・描画
   ************************************************************************/

  // 派遣状況データを保持
  window.dispatchStatusRecords = [];

  // 派遣出力テーブルの予約データを保持
  window.dispatchOutputRecords = [];

  // 更新モード初期表示時の既存派遣者IDs（チェック復元用）
  window.existingDispatchIds = [];

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

      // 派遣状況テーブルから取得（RESERVED_DATE は不要）
      var records = await api.getRecords(DISPATCH_STATUS_SITE_ID, {
        columns: [TABLES.DISPATCH_STATUS.COLUMNS.NAME, TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_FROM, TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_TO, 'ResultId'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!records || records.length === 0) {
        window.force && console.warn('派遣状況データが取得できませんでした');
        return;
      }

      window.force && console.log('派遣状況データ:', records);

      // 派遣出力テーブル（253132）から全件取得
      var outputRecords = await api.getRecords(DISPATCH_OUTPUT_SITE_ID, {
        columns: [TABLES.DISPATCH_OUTPUT.COLUMNS.DISPATCH_IDS, TABLES.DISPATCH_OUTPUT.COLUMNS.DATE, TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_NAME, 'ResultId'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      window.force && console.log('派遣出力データ:', outputRecords);
      window.dispatchOutputRecords = outputRecords || [];

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
      // - 派遣は「店舗+日付」の組み合わせでレコードを特定
      // - 店舗・日付変更時は常にモード判定を行う
      var formShopName = $('#fn-formShop').val() || '';
      var formDate = $('#fn-formDate').val() || '';
      var existingRecord = (outputRecords || []).find(function (rec) {
        // 店舗名と日付の両方が一致するレコードを検索
        var recShopName = rec[TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_NAME] || '';
        var recDate = rec[TABLES.DISPATCH_OUTPUT.COLUMNS.DATE] ? window.formatDateForInput(rec[TABLES.DISPATCH_OUTPUT.COLUMNS.DATE]) : '';
        return recShopName === formShopName && recDate === formDate;
      });

      if (existingRecord) {
        // レコードあり → 更新モード（動的更新 or 更新）
        window.isCreateMode = false;
        window.existingRecordId = existingRecord.ResultId;
        window.force && console.log('同店舗・同日レコード発見、更新モードに切り替え:', existingRecord.ResultId);

        // タイトル・ボタンテキストを更新
        $('.sdt-dispatch-title__text').text('派遣者更新');
        $('#fn-submitButton .sdt-button__text').text('更新');

        // 削除ボタンを表示
        $('#fn-deleteButton').show();

        // 更新モード初期表示時（URLから直接開いた時）は既存のDISPATCH_IDSを保持
        if (!window.isUrlCreateMode) {
          var dispatchIdsRaw = existingRecord[TABLES.DISPATCH_OUTPUT.COLUMNS.DISPATCH_IDS];
          if (Array.isArray(dispatchIdsRaw)) {
            window.existingDispatchIds = dispatchIdsRaw;
          } else if (typeof dispatchIdsRaw === 'string') {
            try {
              window.existingDispatchIds = JSON.parse(dispatchIdsRaw || '[]');
            } catch (e) {
              window.existingDispatchIds = [];
            }
          } else {
            window.existingDispatchIds = [];
          }
          window.force && console.log('既存派遣者IDs:', window.existingDispatchIds);
        }
      } else {
        // レコードなし → 登録モード（登録 or 動的登録）
        window.isCreateMode = true;
        window.existingRecordId = null;
        window.force && console.log('同店舗・同日レコードなし、登録モードに切り替え');

        // タイトル・ボタンテキストを登録に変更
        $('.sdt-dispatch-title__text').text('派遣者登録');
        $('#fn-submitButton .sdt-button__text').text('登録');

        // 削除ボタンを非表示
        $('#fn-deleteButton').hide();
      }

      // データを保持
      window.dispatchStatusRecords = records;

      // テーブルに描画
      renderDispatchTable(records);

    } catch (error) {
      window.force && console.error('派遣状況データ取得エラー:', error);
    }
  }

  /**
   * isDateMatch
   * - 2つの日付が一致しているかを判定（派遣は単日）
   *
   * @param {Date} date1 - 日付1
   * @param {Date} date2 - 日付2
   * @returns {boolean} 一致していればtrue
   */
  function isDateMatch(date1, date2) {
    return date1.getTime() === date2.getTime();
  }

  /**
   * isDateInRange
   * - 日付が期間内にあるかを判定
   *
   * @param {Date} date - 判定する日付
   * @param {Date} start - 期間開始日
   * @param {Date} end - 期間終了日
   * @returns {boolean} 期間内ならtrue
   */
  function isDateInRange(date, start, end) {
    return date >= start && date <= end;
  }

  /**
   * getDispatchStatus
   * - 派遣者の使用状況を判定
   * - フォームの日付とDISPATCH_OUTPUTの日付が一致していれば「派遣中」
   *
   * @param {Object} record - 派遣状況レコード
   * @returns {Object} { status: string, canSelect: boolean, preChecked: boolean }
   */
  function getDispatchStatus(record) {
    var dispatchResultId = String(record.ResultId || '');

    // フォームから日付を取得
    var formDate = $('#fn-formDate').val();

    // 派遣不可期間チェック (UNAVAILABLE_FROM ~ UNAVAILABLE_TO)
    var unavailableStart = record[TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_FROM] ? new Date(record[TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_FROM]) : null;
    var unavailableEnd = record[TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_TO] ? new Date(record[TABLES.DISPATCH_STATUS.COLUMNS.UNAVAILABLE_TO]) : null;

    if (unavailableStart && unavailableEnd) {
      unavailableStart.setHours(0, 0, 0, 0);
      unavailableEnd.setHours(0, 0, 0, 0);

      // フォームの日付が派遣不可期間内かチェック
      if (formDate) {
        var formDateObj = new Date(formDate);
        formDateObj.setHours(0, 0, 0, 0);

        if (isDateInRange(formDateObj, unavailableStart, unavailableEnd)) {
          return { status: '派遣不可期間', canSelect: false, preChecked: false };
        }
      }
    }

    // DISPATCH_OUTPUT（253132）の予約データと日付一致チェック
    // 同店舗で使用中の場合はpreCheckedフラグを立てる
    var preChecked = false;
    var formShopName = $('#fn-formShop').val() || '';

    if (formDate && window.dispatchOutputRecords.length > 0) {
      var formDateObj = new Date(formDate);
      formDateObj.setHours(0, 0, 0, 0);

      // 更新モード時は自分自身のレコードを識別するためのID
      var currentRecordId = !window.isCreateMode ? String(window.existingRecordId || $p.id()) : '';

      for (var i = 0; i < window.dispatchOutputRecords.length; i++) {
        var outputRecord = window.dispatchOutputRecords[i];

        // DISPATCH_IDS（JSON配列）に該当派遣者が含まれているかチェック
        var dispatchIdsRaw = outputRecord[TABLES.DISPATCH_OUTPUT.COLUMNS.DISPATCH_IDS];
        var dispatchIds = [];

        if (Array.isArray(dispatchIdsRaw)) {
          // すでに配列の場合
          dispatchIds = dispatchIdsRaw;
        } else if (typeof dispatchIdsRaw === 'string') {
          // 文字列の場合はJSONパースを試みる
          try {
            dispatchIds = JSON.parse(dispatchIdsRaw || '[]');
          } catch (e) {
            dispatchIds = [];
          }
        }

        // 配列でない場合はスキップ
        if (!Array.isArray(dispatchIds)) {
          continue;
        }

        if (dispatchIds.indexOf(dispatchResultId) < 0 && dispatchIds.indexOf(Number(dispatchResultId)) < 0) {
          continue;
        }

        // 日付一致チェック（派遣は単日）
        var reservedDate = outputRecord[TABLES.DISPATCH_OUTPUT.COLUMNS.DATE] ? new Date(outputRecord[TABLES.DISPATCH_OUTPUT.COLUMNS.DATE]) : null;

        if (reservedDate) {
          reservedDate.setHours(0, 0, 0, 0);

          if (isDateMatch(formDateObj, reservedDate)) {
            var shopName = outputRecord[TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_NAME] || '他店舗';

            // 自分自身のレコード、または同店舗・同日の場合は使用可能＋チェック済みとする
            if (currentRecordId && String(outputRecord.ResultId) === currentRecordId) {
              // 自分自身のレコード → preChecked
              preChecked = true;
              continue;
            }

            if (shopName === formShopName) {
              // 同店舗・同日の別レコード → preChecked
              preChecked = true;
              continue;
            }

            return { status: shopName + 'で選択中', canSelect: false, preChecked: false };
          }
        }
      }
    }

    // 上記以外は派遣可能
    return { status: '派遣可能', canSelect: true, preChecked: preChecked };
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

    // 派遣者名で昇順ソート
    var sortedRecords = records.slice().sort(function (a, b) {
      var nameA = a[TABLES.DISPATCH_STATUS.COLUMNS.NAME] || '';
      var nameB = b[TABLES.DISPATCH_STATUS.COLUMNS.NAME] || '';
      return nameA.localeCompare(nameB, 'ja');
    });

    // DocumentFragment を使ってパフォーマンス向上
    var fragment = document.createDocumentFragment();

    sortedRecords.forEach(function (record) {
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

        // チェック状態の設定
        var resultId = String(record.ResultId || '');
        if (!window.isUrlCreateMode && window.existingDispatchIds.length > 0) {
          // 更新モード初期表示時: 既存のDISPATCH_IDSに含まれていればチェック
          if (window.existingDispatchIds.indexOf(resultId) >= 0 || window.existingDispatchIds.indexOf(Number(resultId)) >= 0) {
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

  // loadDispatchStatusをグローバルに公開（店舗・日付変更時に呼び出し）
  window.loadDispatchStatus = loadDispatchStatus;

  document.addEventListener('DOMContentLoaded', function () {
    // 作成モード（URL=NEW）時はテーブルを非表示（店舗・日付選択後に表示）
    // 更新モード（URL=Edit）時は既存データがあるので表示
    if (window.isUrlCreateMode) {
      $('#sdt-dispatch-table').hide();
    }
  });

})(jQuery);
