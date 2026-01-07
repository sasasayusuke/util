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
        columns: [TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_NAME, TABLES.DISPATCH_OUTPUT.COLUMNS.DISPATCH_IDS, TABLES.DISPATCH_OUTPUT.COLUMNS.DATE, TABLES.DISPATCH_OUTPUT.COLUMNS.EVENT_ID],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!record || Object.keys(record).length === 0) {
        window.force && console.warn('既存データが取得できませんでした');
        return;
      }

      window.force && console.log('既存データ:', record);

      // 店舗セレクトボックスに値をセット
      if (record[TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_NAME]) {
        $('#fn-formShop').val(record[TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_NAME]);
      }

      // 日付をセット（派遣は単日）
      if (record[TABLES.DISPATCH_OUTPUT.COLUMNS.DATE]) {
        $('#fn-formDate').val(window.formatDateForInput(record[TABLES.DISPATCH_OUTPUT.COLUMNS.DATE]));
      }

      // 派遣者選択をセット（JSON配列形式のResultId）
      if (record[TABLES.DISPATCH_OUTPUT.COLUMNS.DISPATCH_IDS]) {
        var selectedDispatchIds = [];
        try {
          selectedDispatchIds = JSON.parse(record[TABLES.DISPATCH_OUTPUT.COLUMNS.DISPATCH_IDS]);
        } catch (e) {
          // JSON形式でない場合はカンマ区切りとして処理（後方互換）
          selectedDispatchIds = String(record[TABLES.DISPATCH_OUTPUT.COLUMNS.DISPATCH_IDS]).split(',').map(function (s) {
            return s.trim();
          });
        }

        // チェックボックスにチェックを入れる（valueはResultId）
        $('.fn-pick').each(function () {
          var $checkbox = $(this);
          var dispatchId = String($checkbox.val());
          if (selectedDispatchIds.indexOf(dispatchId) >= 0 && !$checkbox.prop('disabled')) {
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
    var date = $('#fn-formDate').val();
    var selectedDispatches = [];

    // 選択された派遣者を取得
    $('.fn-pick:checked:not(:disabled)').each(function () {
      selectedDispatches.push($(this).val());
    });

    var errors = [];

    if (!shop) {
      errors.push('店舗を選択してください');
    }
    if (!date) {
      errors.push('日付を入力してください');
    }
    if (selectedDispatches.length === 0) {
      errors.push('派遣者を1人以上選択してください');
    }

    if (errors.length > 0) {
      alert(errors.join('\n'));
      return null;
    }

    return {
      shop: shop,
      date: date,
      selectedDispatches: selectedDispatches
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

      // 派遣者ResultIdは複数選択をJSON配列形式で登録
      var dispatchIds = JSON.stringify(formData.selectedDispatches);

      var data = {};
      data[TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_NAME] = formData.shop;
      data[TABLES.DISPATCH_OUTPUT.COLUMNS.DISPATCH_IDS] = dispatchIds;
      data[TABLES.DISPATCH_OUTPUT.COLUMNS.DATE] = formData.date;

      window.force && console.log('モード:', window.isCreateMode ? '作成' : '更新');
      window.force && console.log('データ:', data);

      var result;
      if (window.isCreateMode) {
        // 作成モード時のみEVENT_ID（LinkId/イベントID）とSHOP_RESULT_ID（店舗ResultId）をセット
        var linkId = window.getUrlParam('LinkId');
        data[TABLES.DISPATCH_OUTPUT.COLUMNS.EVENT_ID] = linkId || '';

        var selectedShop = window.shopRecords.find(function (record) {
          return record[TABLES.PERIOD.COLUMNS.NAME] === formData.shop;
        });

        if (selectedShop && selectedShop.ResultId) {
          data[TABLES.DISPATCH_OUTPUT.COLUMNS.SHOP_RESULT_ID] = String(selectedShop.ResultId);
          window.force && console.log('店舗ResultId:', selectedShop.ResultId);
        } else {
          alert('店舗のResultIdが取得できません。');
          return;
        }

        result = await api.createRecord($p.siteId(), data);
        window.force && console.log('作成結果:', result);
        alert('派遣者を登録しました');
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
        alert('派遣者を更新しました');
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
   * - $p.id()で現在のレコードを削除
   */
  async function handleDelete() {
    var recordId = $p.id();
    if (!recordId) {
      alert('レコードIDが取得できません。');
      return;
    }

    if (!confirm('この派遣者を削除しますか？')) {
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });
      var result = await api.deleteRecord(recordId);
      window.force && console.log('削除結果:', result);
      alert('派遣者を削除しました');
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
    }

    // ボタンイベント設定（重複防止のため.off()で既存イベント解除）
    $('#fn-submitButton').off('click').on('click', handleSubmit);
    $('#fn-deleteButton').off('click').on('click', handleDelete);
    $('#fn-cancelButton').off('click').on('click', handleCancel);
  });

})(jQuery);
