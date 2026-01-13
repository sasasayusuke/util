(function ($) {
  'use strict';

  /************************************************************************
   * 先方（要人）の登録・削除処理
   ************************************************************************/

  // 先方データを保持
  window.vipRecords = [];

  /* ========================================
   * 先方一覧取得
   * ======================================== */

  /**
   * loadVipList
   * - 先方テーブルからイベント店舗IDでフィルタリングしてデータを取得
   */
  async function loadVipList() {
    if (!window.eventShopId) {
      window.force && console.warn('イベント店舗IDが設定されていません');
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      // 先方テーブルからイベント店舗IDでフィルタリング
      var records = await api.getRecords(VIP_SITE_ID, {
        columns: [TABLES.VIP.COLUMNS.SHOP_NAME, TABLES.VIP.COLUMNS.COMPANY, TABLES.VIP.COLUMNS.DEPT, TABLES.VIP.COLUMNS.NAME, TABLES.VIP.COLUMNS.POSITION, TABLES.VIP.COLUMNS.EVENT_SHOP_ID, 'ResultId'],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!records || records.length === 0) {
        window.force && console.log('先方データなし');
        window.vipRecords = [];
        renderVipTable([]);
        return;
      }

      // イベント店舗ID（ClassY）でフィルタリング
      var filteredRecords = records.filter(function (record) {
        return String(record[TABLES.VIP.COLUMNS.EVENT_SHOP_ID]) === String(window.eventShopId);
      });

      window.force && console.log('先方データ:', filteredRecords);
      window.vipRecords = filteredRecords;

      // テーブルに描画
      renderVipTable(filteredRecords);

    } catch (error) {
      window.force && console.error('先方データ取得エラー:', error);
    }
  }

  /**
   * renderVipTable
   * - 先方一覧をテーブルに描画
   */
  function renderVipTable(records) {
    var $tbody = $('#fn-vipTableBody');
    if ($tbody.length === 0) {
      window.force && console.error('renderVipTable: tbody not found');
      return;
    }

    var $template = $tbody.find('tr.fn-vip-row-template').first();
    if ($template.length === 0) {
      window.force && console.error('renderVipTable: template row not found');
      return;
    }

    // テンプレートは非表示のまま保持
    $template.hide();

    // 既存の出力行（テンプレート以外）を削除
    $tbody.find('tr').not($template).remove();

    if (records.length === 0) {
      // データがない場合は「データなし」行を追加
      var $emptyRow = $('<tr class="sdt-table__row"><td class="sdt-table__cell" colspan="5" style="text-align:center;">先方データがありません</td></tr>');
      $tbody.append($emptyRow);
      return;
    }

    // DocumentFragment を使ってパフォーマンス向上
    var fragment = document.createDocumentFragment();

    records.forEach(function (record) {
      var $row = $template.clone().removeClass('fn-vip-row-template').show();

      // 会社名
      var company = record[TABLES.VIP.COLUMNS.COMPANY] || '';
      $row.find('[data-target="fn-vip-company"]').text(company);

      // 部署名
      var dept = record[TABLES.VIP.COLUMNS.DEPT] || '';
      $row.find('[data-target="fn-vip-dept"]').text(dept);

      // 氏名
      var name = record[TABLES.VIP.COLUMNS.NAME] || '';
      $row.find('[data-target="fn-vip-name"]').text(name);

      // 役職
      var position = record[TABLES.VIP.COLUMNS.POSITION] || '';
      $row.find('[data-target="fn-vip-position"]').text(position);

      // 削除ボタン
      var $deleteButton = $row.find('.fn-vip-delete');
      $deleteButton.attr('data-record-id', record.ResultId || '');
      $deleteButton.on('click', handleVipDelete);

      fragment.appendChild($row.get(0));
    });

    $tbody.get(0).appendChild(fragment);
  }

  /* ========================================
   * 先方登録処理
   * ======================================== */

  /**
   * handleVipRegister
   * - 先方登録ボタンクリック時の処理
   */
  async function handleVipRegister() {
    var company = $('#fn-vipCompany').val() || '';
    var dept = $('#fn-vipDept').val() || '';
    var name = $('#fn-vipName').val() || '';
    var position = $('#fn-vipPosition').val() || '';

    // バリデーション
    if (!company && !dept && !name && !position) {
      alert('いずれかの項目を入力してください');
      return;
    }

    if (!window.eventShopId) {
      alert('イベント店舗IDが取得できません');
      return;
    }

    if (!window.eventId) {
      alert('イベントIDが取得できません');
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      var data = {};
      data[TABLES.VIP.COLUMNS.SHOP_NAME] = window.periodRecord ? window.periodRecord[TABLES.PERIOD.COLUMNS.NAME] : '';
      data[TABLES.VIP.COLUMNS.EVENT_SHOP_ID] = String(window.eventShopId);
      data[TABLES.VIP.COLUMNS.COMPANY] = company;
      data[TABLES.VIP.COLUMNS.DEPT] = dept;
      data[TABLES.VIP.COLUMNS.NAME] = name;
      data[TABLES.VIP.COLUMNS.POSITION] = position;
      data[TABLES.VIP.COLUMNS.EVENT_ID] = String(window.eventId);

      window.force && console.log('先方登録データ:', data);

      var result = await api.createRecord(VIP_SITE_ID, data);
      window.force && console.log('先方登録結果:', result);

      alert('先方（要人）を登録しました');

      // フォームをクリア
      $('#fn-vipCompany').val('');
      $('#fn-vipDept').val('');
      $('#fn-vipName').val('');
      $('#fn-vipPosition').val('');

      // テーブル再描画
      await loadVipList();

    } catch (error) {
      window.force && console.error('先方登録エラー:', error);
      alert('登録に失敗しました: ' + (error.message || error));
    }
  }

  /* ========================================
   * 先方削除処理
   * ======================================== */

  /**
   * handleVipDelete
   * - 先方削除ボタンクリック時の処理
   */
  async function handleVipDelete() {
    var recordId = $(this).attr('data-record-id');

    if (!recordId) {
      alert('レコードIDが取得できません');
      return;
    }

    if (!confirm('この先方（要人）を削除しますか？')) {
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      var result = await api.deleteRecord(recordId);
      window.force && console.log('先方削除結果:', result);

      alert('先方（要人）を削除しました');

      // テーブル再描画
      await loadVipList();

    } catch (error) {
      window.force && console.error('先方削除エラー:', error);
      alert('削除に失敗しました: ' + (error.message || error));
    }
  }

  /* ========================================
   * 初期化
   * ======================================== */

  // loadVipListをグローバルに公開（初期表示で呼び出し）
  window.loadVipList = loadVipList;

  document.addEventListener('DOMContentLoaded', function () {
    // 登録ボタンイベント設定
    $('#fn-vipRegister').off('click').on('click', handleVipRegister);
  });

})(jQuery);
