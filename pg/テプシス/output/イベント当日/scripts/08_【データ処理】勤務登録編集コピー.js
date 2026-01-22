(function ($) {
  'use strict';

  /************************************************************************
   * 勤務開始終了の登録・編集・コピー処理
   *
   * 重要：
   * - 同じ日付・同じイベント店舗IDを持つレコードを「兄弟レコード」として扱う
   * - 兄弟レコードはどれに入っても統一の表示にする
   ************************************************************************/

  // 勤務登録データを保持
  window.workRecords = [];

  // 編集モード管理
  window.editingRecordId = null;

  /* ========================================
   * 勤務一覧取得
   * ======================================== */

  /**
   * loadWorkList
   * - イベント当日テーブルからイベント店舗IDでフィルタリングしてデータを取得
   * - 兄弟レコード（同日付・同イベント店舗ID）を統一表示
   */
  async function loadWorkList() {
    if (!window.eventShopId) {
      window.force && console.warn('イベント店舗IDが設定されていません');
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      // イベント当日テーブルから全件取得
      var records = await api.getRecords(EVENT_DAY_SITE_ID, {
        columns: [
          TABLES.EVENT_DAY.COLUMNS.SHOP_NAME,
          TABLES.EVENT_DAY.COLUMNS.STAFF,
          TABLES.EVENT_DAY.COLUMNS.FREE_TEXT,
          TABLES.EVENT_DAY.COLUMNS.WORK_DATE,
          TABLES.EVENT_DAY.COLUMNS.WORK_START,
          TABLES.EVENT_DAY.COLUMNS.WORK_END,
          TABLES.EVENT_DAY.COLUMNS.PERIOD_ID,
          'ResultId'
        ],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!records || records.length === 0) {
        window.force && console.log('勤務登録データなし');
        window.workRecords = [];
        window.siblingRecords = [];
        renderWorkTable([]);
        return;
      }

      // イベント店舗ID（PERIOD_ID）でフィルタリング
      var filteredRecords = records.filter(function (record) {
        return String(record[TABLES.EVENT_DAY.COLUMNS.PERIOD_ID]) === String(window.eventShopId);
      });

      window.force && console.log('勤務登録データ:', filteredRecords);
      window.workRecords = filteredRecords;
      window.siblingRecords = filteredRecords;

      // テーブルに描画
      renderWorkTable(filteredRecords);

    } catch (error) {
      window.force && console.error('勤務登録データ取得エラー:', error);
    }
  }

  /**
   * renderWorkTable
   * - 勤務一覧をテーブルに描画
   */
  function renderWorkTable(records) {
    var $tbody = $('#fn-workTableBody');
    if ($tbody.length === 0) {
      window.force && console.error('renderWorkTable: tbody not found');
      return;
    }

    var $template = $tbody.find('tr.fn-work-row-template').first();
    if ($template.length === 0) {
      window.force && console.error('renderWorkTable: template row not found');
      return;
    }

    // テンプレートは非表示のまま保持
    $template.hide();

    // 既存の出力行（テンプレート以外）を削除
    $tbody.find('tr').not($template).remove();

    if (records.length === 0) {
      // データがない場合は「データなし」行を追加
      var $emptyRow = $('<tr class="sdt-table__row"><td class="sdt-table__cell" colspan="6" style="text-align:center;">勤務登録データがありません</td></tr>');
      $tbody.append($emptyRow);
      return;
    }

    // 勤務日で昇順ソート
    var sortedRecords = records.slice().sort(function (a, b) {
      var dateA = new Date(a[TABLES.EVENT_DAY.COLUMNS.WORK_DATE] || 0);
      var dateB = new Date(b[TABLES.EVENT_DAY.COLUMNS.WORK_DATE] || 0);
      return dateA - dateB;
    });

    // DocumentFragment を使ってパフォーマンス向上
    var fragment = document.createDocumentFragment();

    sortedRecords.forEach(function (record) {
      var $row = $template.clone().removeClass('fn-work-row-template').show();

      // 勤務日
      var workDate = record[TABLES.EVENT_DAY.COLUMNS.WORK_DATE] || '';
      $row.find('[data-target="fn-work-date"]').text(formatWorkDate(workDate));

      // 勤務開始時刻
      var workStart = record[TABLES.EVENT_DAY.COLUMNS.WORK_START] || '';
      $row.find('[data-target="fn-work-start"]').text(window.formatTime(workStart));

      // 勤務終了時刻
      var workEnd = record[TABLES.EVENT_DAY.COLUMNS.WORK_END] || '';
      $row.find('[data-target="fn-work-end"]').text(window.formatTime(workEnd));

      // 担当者（UserIdから名前に変換）
      var staffId = record[TABLES.EVENT_DAY.COLUMNS.STAFF] || '';
      var staffName = (window.staffMap && window.staffMap[String(staffId)]) || staffId;
      $row.find('[data-target="fn-work-staff"]').text(staffName);

      // 自由記入欄
      var freeText = record[TABLES.EVENT_DAY.COLUMNS.FREE_TEXT] || '';
      $row.find('[data-target="fn-work-note"]').text(freeText);

      // 編集ボタン
      var $editButton = $row.find('.fn-work-edit');
      $editButton.attr('data-record-id', record.ResultId || '');
      $editButton.on('click', handleWorkEdit);

      // コピーボタン
      var $copyButton = $row.find('.fn-work-copy');
      $copyButton.attr('data-record-id', record.ResultId || '');
      $copyButton.on('click', handleWorkCopy);

      fragment.appendChild($row.get(0));
    });

    $tbody.get(0).appendChild(fragment);
  }

  /**
   * formatWorkDate
   * - 勤務日をYYYY年MM月DD日形式に変換
   */
  function formatWorkDate(dateStr) {
    if (!dateStr) return '';

    var date = new Date(dateStr);
    if (isNaN(date.getTime())) return dateStr;

    var year = date.getFullYear();
    var month = ('0' + (date.getMonth() + 1)).slice(-2);
    var day = ('0' + date.getDate()).slice(-2);

    return year + '年' + month + '月' + day + '日';
  }

  /* ========================================
   * 勤務登録処理
   * ======================================== */

  /**
   * handleWorkRegister
   * - 勤務登録ボタンクリック時の処理
   */
  async function handleWorkRegister() {
    var workDate = $('#fn-workDate').val() || '';
    var workStartTime = $('#fn-workStartTime').val() || '';
    var workEndTime = $('#fn-workEndTime').val() || '';
    var staffId = $('#fn-workStaff').val() || '';
    var freeText = $('#fn-workNote').val() || '';

    // バリデーション
    var errors = [];
    if (!workDate) {
      errors.push('勤務日を入力してください');
    }
    if (!workStartTime) {
      errors.push('勤務開始時刻を入力してください');
    }
    if (!workEndTime) {
      errors.push('勤務終了時刻を入力してください');
    }
    if (!staffId) {
      errors.push('担当者を選択してください');
    }
    if (freeText.length > 1000) {
      errors.push('自由記入欄は1000文字以内で入力してください（現在: ' + freeText.length + '文字）');
    }

    if (errors.length > 0) {
      alert(errors.join('\n'));
      return;
    }

    if (!window.eventShopId) {
      alert('イベント店舗IDが取得できません');
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      // 勤務開始時刻と勤務終了時刻をISO形式に変換
      var workStartDateTime = workDate + 'T' + workStartTime + ':00';
      var workEndDateTime = workDate + 'T' + workEndTime + ':00';
      var workDateDateTime = workDate + 'T00:00:00';

      var data = {};
      data[TABLES.EVENT_DAY.COLUMNS.SHOP_NAME] = window.periodRecord ? window.periodRecord[TABLES.PERIOD.COLUMNS.NAME] : '';
      data[TABLES.EVENT_DAY.COLUMNS.STAFF] = String(staffId);
      data[TABLES.EVENT_DAY.COLUMNS.FREE_TEXT] = freeText;
      data[TABLES.EVENT_DAY.COLUMNS.WORK_DATE] = workDateDateTime;
      data[TABLES.EVENT_DAY.COLUMNS.WORK_START] = workStartDateTime;
      data[TABLES.EVENT_DAY.COLUMNS.WORK_END] = workEndDateTime;
      data[TABLES.EVENT_DAY.COLUMNS.PERIOD_ID] = String(window.eventShopId);

      window.force && console.log('勤務登録データ:', data);

      var result;
      if (window.editingRecordId) {
        // 編集モード
        result = await api.updateRecord(window.editingRecordId, data);
        window.force && console.log('勤務更新結果:', result);
        alert('勤務情報を更新しました');
      } else {
        // 登録モード
        result = await api.createRecord(EVENT_DAY_SITE_ID, data);
        window.force && console.log('勤務登録結果:', result);
        alert('勤務情報を登録しました');
      }

      // フォームをクリア
      clearWorkForm();

      // テーブル再描画
      await loadWorkList();

    } catch (error) {
      window.force && console.error('勤務登録エラー:', error);
      alert('登録に失敗しました: ' + (error.message || error));
    }
  }

  /**
   * clearWorkForm
   * - 勤務登録フォームをクリア
   */
  function clearWorkForm() {
    $('#fn-workDate').val('');
    $('#fn-workStartTime').val('');
    $('#fn-workEndTime').val('');
    $('#fn-workStaff').val('');
    $('#fn-workNote').val('');
    window.editingRecordId = null;
    $('#fn-workRegister .sdt-button__text').text('登録');
  }

  /* ========================================
   * モーダル処理
   * ======================================== */

  // モーダルモード管理（'edit' or 'copy'）
  var modalMode = null;

  /**
   * openWorkModal
   * - モーダルを開く
   */
  function openWorkModal(mode, record) {
    modalMode = mode;

    // モーダル用担当者プルダウンを初期化
    initModalStaffSelect();

    if (mode === 'edit') {
      // 編集モード
      $('#fn-workModalTitle').text('勤務情報編集');
      $('#fn-modalWorkSubmit .sdt-button__text').text('更新');
      $('#fn-modalWorkDelete').show();
      window.editingRecordId = record.ResultId;

      // フォームにデータをセット
      var workDate = record[TABLES.EVENT_DAY.COLUMNS.WORK_DATE] || '';
      $('#fn-modalWorkDate').val(window.formatDateForInput(workDate));
    } else {
      // コピーモード
      $('#fn-workModalTitle').text('勤務情報コピー');
      $('#fn-modalWorkSubmit .sdt-button__text').text('登録');
      $('#fn-modalWorkDelete').hide();
      window.editingRecordId = null;

      // 勤務日は空白
      $('#fn-modalWorkDate').val('');
    }

    // 共通データセット
    var workStart = record[TABLES.EVENT_DAY.COLUMNS.WORK_START] || '';
    var workEnd = record[TABLES.EVENT_DAY.COLUMNS.WORK_END] || '';
    var staff = record[TABLES.EVENT_DAY.COLUMNS.STAFF] || '';
    var freeText = record[TABLES.EVENT_DAY.COLUMNS.FREE_TEXT] || '';

    $('#fn-modalWorkStartTime').val(window.formatTime(workStart));
    $('#fn-modalWorkEndTime').val(window.formatTime(workEnd));
    $('#fn-modalWorkStaff').val(staff);
    $('#fn-modalWorkNote').val(freeText);

    // モーダル表示
    $('#fn-workModal').show();
  }

  /**
   * closeWorkModal
   * - モーダルを閉じる
   */
  function closeWorkModal() {
    $('#fn-workModal').hide();
    modalMode = null;
    window.editingRecordId = null;
  }

  /**
   * initModalStaffSelect
   * - モーダル用担当者プルダウンを初期化
   */
  function initModalStaffSelect() {
    var $modalSelect = $('#fn-modalWorkStaff');
    var $originalSelect = $('#fn-workStaff');

    // 既存のオプションをクリア（最初のデフォルトオプション以外）
    $modalSelect.find('option:not(:first)').remove();

    // 元のプルダウンからオプションをコピー
    $originalSelect.find('option:not(:first)').each(function () {
      var $option = $(this).clone();
      $modalSelect.append($option);
    });
  }

  /**
   * handleWorkEdit
   * - 勤務編集ボタンクリック時の処理（モーダル表示）
   */
  function handleWorkEdit() {
    var recordId = $(this).attr('data-record-id');

    if (!recordId) {
      alert('レコードIDが取得できません');
      return;
    }

    var record = window.workRecords.find(function (r) {
      return String(r.ResultId) === String(recordId);
    });

    if (!record) {
      alert('レコードが見つかりません');
      return;
    }

    openWorkModal('edit', record);
  }

  /**
   * handleWorkCopy
   * - 勤務コピーボタンクリック時の処理（モーダル表示）
   */
  function handleWorkCopy() {
    var recordId = $(this).attr('data-record-id');

    if (!recordId) {
      alert('レコードIDが取得できません');
      return;
    }

    var record = window.workRecords.find(function (r) {
      return String(r.ResultId) === String(recordId);
    });

    if (!record) {
      alert('レコードが見つかりません');
      return;
    }

    openWorkModal('copy', record);
  }

  /**
   * handleModalSubmit
   * - モーダル内の更新/登録ボタンクリック時の処理
   */
  async function handleModalSubmit() {
    var workDate = $('#fn-modalWorkDate').val() || '';
    var workStartTime = $('#fn-modalWorkStartTime').val() || '';
    var workEndTime = $('#fn-modalWorkEndTime').val() || '';
    var staffId = $('#fn-modalWorkStaff').val() || '';
    var freeText = $('#fn-modalWorkNote').val() || '';

    // バリデーション
    var errors = [];
    if (!workDate) {
      errors.push('勤務日を入力してください');
    }
    if (!workStartTime) {
      errors.push('勤務開始時刻を入力してください');
    }
    if (!workEndTime) {
      errors.push('勤務終了時刻を入力してください');
    }
    if (!staffId) {
      errors.push('担当者を選択してください');
    }
    if (freeText.length > 1000) {
      errors.push('自由記入欄は1000文字以内で入力してください（現在: ' + freeText.length + '文字）');
    }

    if (errors.length > 0) {
      alert(errors.join('\n'));
      return;
    }

    if (!window.eventShopId) {
      alert('イベント店舗IDが取得できません');
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      var workStartDateTime = workDate + 'T' + workStartTime + ':00';
      var workEndDateTime = workDate + 'T' + workEndTime + ':00';
      var workDateDateTime = workDate + 'T00:00:00';

      var data = {};
      data[TABLES.EVENT_DAY.COLUMNS.SHOP_NAME] = window.periodRecord ? window.periodRecord[TABLES.PERIOD.COLUMNS.NAME] : '';
      data[TABLES.EVENT_DAY.COLUMNS.STAFF] = String(staffId);
      data[TABLES.EVENT_DAY.COLUMNS.FREE_TEXT] = freeText;
      data[TABLES.EVENT_DAY.COLUMNS.WORK_DATE] = workDateDateTime;
      data[TABLES.EVENT_DAY.COLUMNS.WORK_START] = workStartDateTime;
      data[TABLES.EVENT_DAY.COLUMNS.WORK_END] = workEndDateTime;
      data[TABLES.EVENT_DAY.COLUMNS.PERIOD_ID] = String(window.eventShopId);

      var result;
      if (modalMode === 'edit' && window.editingRecordId) {
        result = await api.updateRecord(window.editingRecordId, data);
        window.force && console.log('勤務更新結果:', result);
        alert('勤務情報を更新しました');
      } else {
        result = await api.createRecord(EVENT_DAY_SITE_ID, data);
        window.force && console.log('勤務登録結果:', result);
        alert('勤務情報を登録しました');
      }

      closeWorkModal();

      // テーブル再描画
      await loadWorkList();

    } catch (error) {
      window.force && console.error('勤務登録エラー:', error);
      alert('登録に失敗しました: ' + (error.message || error));
    }
  }

  /**
   * handleModalDelete
   * - モーダル内の削除ボタンクリック時の処理
   */
  async function handleModalDelete() {
    if (!window.editingRecordId) {
      alert('削除対象のレコードが選択されていません');
      return;
    }

    if (!confirm('この勤務情報を削除してもよろしいですか？')) {
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });
      await api.deleteRecord(window.editingRecordId);
      window.force && console.log('勤務削除完了:', window.editingRecordId);
      alert('勤務情報を削除しました');

      closeWorkModal();

      // テーブル再描画
      await loadWorkList();

    } catch (error) {
      window.force && console.error('勤務削除エラー:', error);
      alert('削除に失敗しました: ' + (error.message || error));
    }
  }

  /* ========================================
   * 初期化
   * ======================================== */

  // loadWorkListをグローバルに公開（初期表示で呼び出し）
  window.loadWorkList = loadWorkList;

  /* ========================================
   * 文字数カウント
   * ======================================== */
  var FREE_TEXT_MAX_LENGTH = 1000;

  /**
   * setupCharCounter
   * - textarea に文字数カウンターを設定
   */
  function setupCharCounter($textarea) {
    if ($textarea.length === 0) return;

    // カウンター要素を作成（初期は非表示）
    var $counter = $('<div class="fn-char-counter" style="text-align:right; font-size:12px; color:#E80000; margin-top:4px; display:none;"></div>');
    $textarea.after($counter);

    // 更新関数
    function updateCounter() {
      var length = $textarea.val().length;
      var remaining = FREE_TEXT_MAX_LENGTH - length;

      if (remaining < 0) {
        // オーバー時のみ表示
        $counter.show();
        $counter.text(length + ' / ' + FREE_TEXT_MAX_LENGTH + '文字（' + Math.abs(remaining) + '文字オーバー）');
      } else {
        // 範囲内は非表示
        $counter.hide();
      }
    }

    // イベント設定
    $textarea.on('input', updateCounter);

    // 初期表示
    updateCounter();
  }

  document.addEventListener('DOMContentLoaded', function () {
    // 登録ボタンイベント設定
    $('#fn-workRegister').off('click').on('click', handleWorkRegister);

    // モーダルイベント設定
    $('#fn-workModalClose').off('click').on('click', closeWorkModal);
    $('#fn-workModalOverlay').off('click').on('click', closeWorkModal);
    $('#fn-modalWorkSubmit').off('click').on('click', handleModalSubmit);
    $('#fn-modalWorkDelete').off('click').on('click', handleModalDelete);

    // 文字数カウンター設定
    setupCharCounter($('#fn-workNote'));
    setupCharCounter($('#fn-modalWorkNote'));
  });

})(jQuery);
