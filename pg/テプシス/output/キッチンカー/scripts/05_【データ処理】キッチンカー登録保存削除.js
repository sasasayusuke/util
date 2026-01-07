(function ($) {
  'use strict';

  /************************************************************************
   * 既存データ読み込み・バリデーション・登録/更新処理
   ************************************************************************/

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
        $('#fn-formDateFrom').val(window.formatDateForInput(record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_FROM]));
      }
      if (record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO]) {
        $('#fn-formDateTo').val(window.formatDateForInput(record[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.DATE_TO]));
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

      window.force && console.log('モード:', window.isCreateMode ? '作成' : '更新');
      window.force && console.log('データ:', data);

      var result;
      if (window.isCreateMode) {
        // 作成モード時のみEVENT_ID（LinkId/イベントID）とSHOP_RESULT_ID（店舗ResultId）をセット
        var linkId = window.getUrlParam('LinkId');
        data[TABLES.KITCHEN_CAR_OUTPUT.COLUMNS.EVENT_ID] = linkId || '';

        var selectedShop = window.shopRecords.find(function (record) {
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
        // 更新モード（$p.id() または window.existingRecordId を使用）
        var recordId = window.existingRecordId || $p.id();
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
      alert((window.isCreateMode ? '登録' : '更新') + 'に失敗しました: ' + (error.message || error));
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

  /**
   * handleDelete
   * - 削除ボタンクリック時の処理
   * - $p.id() または window.existingRecordId で現在のレコードを削除
   */
  async function handleDelete() {
    var recordId = window.existingRecordId || $p.id();
    if (!recordId) {
      alert('レコードIDが取得できません。');
      return;
    }

    if (!confirm('このキッチンカーを削除しますか？')) {
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });
      var result = await api.deleteRecord(recordId);
      window.force && console.log('削除結果:', result);
      alert('キッチンカーを削除しました');
      handleCancel();
    } catch (error) {
      window.force && console.error('削除エラー:', error);
      alert('削除に失敗しました: ' + (error.message || error));
    }
  }

  /* ========================================
   * 初期化
   * ======================================== */

  document.addEventListener('DOMContentLoaded', function () {
    // 更新モード時は既存データを読み込み（テーブル描画後に実行）
    if (!window.isCreateMode) {
      // テーブル描画完了を待ってから既存データをセット
      setTimeout(function () {
        loadExistingData();
      }, 500);
      // 更新モード時は削除ボタンを表示
      $('#fn-deleteButton').show();
      // 更新モード時は店舗を変更不可に
      $('#fn-formShop').prop('disabled', true);
    }

    // ボタンイベント設定（重複防止のため.off()で既存イベント解除）
    $('#fn-submitButton').off('click').on('click', handleSubmit);
    $('#fn-deleteButton').off('click').on('click', handleDelete);
    $('#fn-cancelButton').off('click').on('click', handleCancel);
  });

})(jQuery);
