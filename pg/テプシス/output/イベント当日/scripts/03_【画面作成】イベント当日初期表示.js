(function ($) {
  'use strict';

  /************************************************************************
   * 画面読み込み時に一度だけ実行する初期表示処理
   *
   * 目的：
   *  - イベント情報を開始～終了期間とイベント予定一覧から取得
   *  - 担当者プルダウンを初期化
   *  - 先方テーブルと勤務登録テーブルを初期化
   ************************************************************************/

  // グローバル変数
  window.periodRecord = null;      // 開始～終了期間レコード
  window.eventRecord = null;       // イベント予定一覧レコード
  window.eventShopId = null;       // イベント店舗ID（開始～終了期間のID）
  window.eventId = null;           // イベントID
  window.siblingRecords = [];      // 兄弟レコード（同日付・同イベント店舗ID）

  /* ========================================
   * レイアウト関連
   * ======================================== */

  /**
   * getHeaderHeight
   * - #Header 要素の「見た目の高さ」をピクセル単位で返します。
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
   */
  function buildCalcHeightCss(px) {
    var n = (typeof px === 'number' && !isNaN(px)) ? px : 0;
    return 'calc(100% - ' + String(n) + 'px)';
  }

  /**
   * applyEventDayLayout
   * - 実際に DOM に対して style を当てる関数
   */
  function applyEventDayLayout(headerHeightPx) {
    var $target = $('#fn-event-day');
    if ($target.length === 0) {
      return;
    }
    $target.css('top', String(headerHeightPx) + 'px');
    $target.css('height', buildCalcHeightCss(headerHeightPx));
  }

  /**
   * initEventDayLayout
   * - レイアウト初期化
   */
  function initEventDayLayout() {
    var headerH = getHeaderHeight();
    applyEventDayLayout(headerH);
  }

  /* ========================================
   * URLパラメータ取得
   * ======================================== */

  /**
   * getUrlParam
   * - URLから指定したパラメータの値を取得
   */
  window.getUrlParam = function (name) {
    var params = new URLSearchParams(window.location.search);
    return params.get(name);
  };

  /* ========================================
   * イベント情報取得
   * ======================================== */

  /**
   * loadEventInfo
   * - 開始～終了期間（親）とイベント予定一覧（祖父）からイベント情報を取得
   * - URLのLinkIdまたは既存レコードのPERIOD_IDから開始～終了期間を取得
   * - さらに開始～終了期間のEVENT_IDからイベント予定一覧を取得
   * - PleasanterAPIのgetRecordメソッドを使用
   */
  async function loadEventInfo() {
    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      // イベント店舗ID（開始～終了期間のID）を取得
      var periodId = null;
      var isNewRecord = location.pathname.includes('/new');

      if (isNewRecord) {
        // 新規作成画面：URLのLinkIdを使用
        periodId = window.getUrlParam('LinkId');
        window.force && console.log('URLのLinkId:', periodId);
      } else {
        // 既存レコード編集画面：PERIOD_IDを取得
        var currentRecordId = $p.id();
        if (currentRecordId) {
          var currentRecord = await api.getRecord(currentRecordId, {
            columns: [TABLES.EVENT_DAY.COLUMNS.PERIOD_ID],
            setLabelText: false,
            setDisplayValue: 'Value',
          });

          if (currentRecord && currentRecord[TABLES.EVENT_DAY.COLUMNS.PERIOD_ID]) {
            periodId = currentRecord[TABLES.EVENT_DAY.COLUMNS.PERIOD_ID];
            window.force && console.log('既存レコードのPERIOD_ID:', periodId);
          }
        }
      }

      // LinkIdもPERIOD_IDも取得できない場合、画面上のPERIOD_IDから取得
      if (!periodId) {
        var classZValue = $p.getControl(TABLES.EVENT_DAY.COLUMNS.PERIOD_ID).val();
        if (classZValue) {
          periodId = classZValue;
          window.force && console.log('画面上のPERIOD_IDから取得:', periodId);
        }
      }

      if (!periodId) {
        window.force && console.warn('開始～終了期間のIDが取得できません');
        return;
      }

      window.eventShopId = periodId;

      // 開始～終了期間（親）レコードを取得
      var periodRecord = await api.getRecord(periodId, {
        columns: [
          TABLES.PERIOD.COLUMNS.NAME,
          TABLES.PERIOD.COLUMNS.EVENT_ID,
          TABLES.PERIOD.COLUMNS.START_DATE,
          TABLES.PERIOD.COLUMNS.END_DATE
        ],
        setLabelText: false,
        setDisplayValue: 'Value',
      });

      if (!periodRecord) {
        window.force && console.warn('開始～終了期間レコードが取得できません');
        return;
      }

      window.force && console.log('開始～終了期間レコード:', periodRecord);
      window.periodRecord = periodRecord;
      window.eventId = periodRecord[TABLES.PERIOD.COLUMNS.EVENT_ID];

      // イベント予定一覧（祖父）レコードを取得
      if (window.eventId) {
        var eventRecord = await api.getRecord(window.eventId, {
          columns: [
            TABLES.EVENT_LIST.COLUMNS.CATEGORY,
            TABLES.EVENT_LIST.COLUMNS.EVENT_NAME,
            TABLES.EVENT_LIST.COLUMNS.ORGANIZER,
            TABLES.EVENT_LIST.COLUMNS.AREA,
            TABLES.EVENT_LIST.COLUMNS.AREA_DETAIL,
            'ResultId'],
          setLabelText: false,
          setDisplayValue: 'Value',
        });

        if (eventRecord) {
          window.force && console.log('イベント予定一覧レコード:', eventRecord);
          window.eventRecord = eventRecord;

          // イベント情報を画面に表示
          displayEventInfo();

          // 勤務日カレンダーの入力制限を設定（開始日〜終了日のみ）
          setWorkDateRange();
        }
      }

      // 個別イベント管理リンクを設定
      var eventResultId = window.eventRecord && window.eventRecord.ResultId;
      if (eventResultId) {
        $('#fn-linkEventManage').attr('href', location.origin + '/items/' + eventResultId);
      }

      // 開催イベント一覧に戻るリンクを設定
      $('#fn-linkBackToList').attr('href', location.origin + '/items/' + EVENT_OPEN_LIST_SITE_ID + '/index');

      // 先方テーブル初期化（04で実装）
      if (typeof window.loadVipList === 'function') {
        window.loadVipList();
      }

      // 勤務登録テーブル初期化（05で実装）
      if (typeof window.loadWorkList === 'function') {
        window.loadWorkList();
      }

    } catch (error) {
      window.force && console.error('イベント情報取得エラー:', error);
    }
  }

  /**
   * displayEventInfo
   * - イベント情報を画面に表示
   */
  function displayEventInfo() {
    if (!window.eventRecord || !window.periodRecord) {
      return;
    }

    // カテゴリー
    var category = window.eventRecord[TABLES.EVENT_LIST.COLUMNS.CATEGORY] || '-';
    $('#fn-infoCategory').text(category);

    // イベント名称・出店先
    var eventName = window.eventRecord[TABLES.EVENT_LIST.COLUMNS.ORGANIZER] || '-';
    $('#fn-infoEventName').text(eventName);

    // 事業者名or店舗名
    var shopName = window.periodRecord[TABLES.PERIOD.COLUMNS.NAME] || '-';
    $('#fn-infoOrganizer').text(shopName);

    // 開催エリア
    var area = window.eventRecord[TABLES.EVENT_LIST.COLUMNS.AREA] || '-';
    $('#fn-infoArea').text(area);

    // 開催エリア詳細
    var areaDetail = window.eventRecord[TABLES.EVENT_LIST.COLUMNS.AREA_DETAIL] || '-';
    $('#fn-infoAreaDetail').text(areaDetail);

    // 開催期間
    var startDate = window.periodRecord[TABLES.PERIOD.COLUMNS.START_DATE] || '';
    var endDate = window.periodRecord[TABLES.PERIOD.COLUMNS.END_DATE] || '';
    var periodText = formatDate(startDate) + '〜' + formatDate(endDate);
    $('#fn-infoPeriod').text(periodText);
  }

  /**
   * setWorkDateRange
   * - 勤務日カレンダーの入力範囲を開始日〜終了日に制限
   */
  function setWorkDateRange() {
    if (!window.periodRecord) {
      return;
    }

    var startDate = window.formatDateForInput(window.periodRecord[TABLES.PERIOD.COLUMNS.START_DATE]);
    var endDate = window.formatDateForInput(window.periodRecord[TABLES.PERIOD.COLUMNS.END_DATE]);

    if (startDate) {
      $('#fn-workDate').attr('min', startDate);
      $('#fn-modalWorkDate').attr('min', startDate);
    }

    if (endDate) {
      $('#fn-workDate').attr('max', endDate);
      $('#fn-modalWorkDate').attr('max', endDate);
    }

    window.force && console.log('勤務日制限設定:', startDate, '〜', endDate);
  }

  /**
   * formatDate
   * - 日付をYYYY年MM月DD日形式に変換
   */
  function formatDate(dateStr) {
    if (!dateStr) return '';

    var date = new Date(dateStr);
    if (isNaN(date.getTime())) return dateStr;

    var year = date.getFullYear();
    var month = ('0' + (date.getMonth() + 1)).slice(-2);
    var day = ('0' + date.getDate()).slice(-2);

    return year + '年' + month + '月' + day + '日';
  }

  /**
   * formatDateForInput
   * - 日付をYYYY-MM-DD形式に変換
   */
  window.formatDateForInput = function (dateStr) {
    if (!dateStr) return '';

    if (dateStr.includes('T')) {
      return dateStr.split('T')[0];
    }

    if (/^\d{4}-\d{2}-\d{2}$/.test(dateStr)) {
      return dateStr;
    }

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

  /**
   * formatTime
   * - 時刻をHH:MM形式に変換
   */
  window.formatTime = function (dateTimeStr) {
    if (!dateTimeStr) return '';

    try {
      var date = new Date(dateTimeStr);
      if (isNaN(date.getTime())) return '';

      var hours = ('0' + date.getHours()).slice(-2);
      var minutes = ('0' + date.getMinutes()).slice(-2);

      return hours + ':' + minutes;
    } catch (e) {
      window.force && console.warn('時刻変換エラー:', dateTimeStr, e);
      return '';
    }
  };

  /* ========================================
   * 担当者プルダウン初期化
   * ======================================== */

  // ユーザーIDから名前へのマップ（グローバル）
  window.staffMap = {};

  /**
   * loadStaffOptions
   * - PleasanterのユーザーAPIからユーザー情報を取得し、プルダウンに設定
   * - ユーザーマップ（UserId→Name）をグローバルに保存
   */
  async function loadStaffOptions() {
    var $select = $('#fn-workStaff');
    if ($select.length === 0) {
      return;
    }

    try {
      var api = new PleasanterAPI(location.origin, { logging: window.force });

      // Pleasanterのユーザー一覧を取得（部署IDで絞り込み）
      var users = await api.getUsers({
        deptIds: [STAFF_DEPT_ID],
        ignoreErrors: false,
      });

      if (!users || users.length === 0) {
        window.force && console.warn('ユーザーデータが取得できませんでした');
        return;
      }

      window.force && console.log('ユーザーデータ:', users);

      users.forEach(function (user) {
        var userName = user.Name || '';
        var userId = user.UserId || '';

        if (userName && userId) {
          // マップに追加
          window.staffMap[String(userId)] = userName;

          var $option = $('<option></option>').val(userId).text(userName);
          $select.append($option);
        }
      });

      window.force && console.log('staffMap:', window.staffMap);

    } catch (error) {
      window.force && console.error('ユーザーデータ取得エラー:', error);
    }
  }

  /* ========================================
   * 折り畳み機能
   * ======================================== */

  /**
   * initToggle
   * - セクションの折り畳み機能を初期化
   */
  function initToggle() {
    $('#fn-toggleVip').on('click', function () {
      var $body = $('#fn-sectionVipBody');
      var $button = $(this);

      if ($body.is(':visible')) {
        $body.slideUp();
        $button.addClass('sdt-toggle-button--collapsed');
      } else {
        $body.slideDown();
        $button.removeClass('sdt-toggle-button--collapsed');
      }
    });
  }

  /* ========================================
   * 初期化
   * ======================================== */

  document.addEventListener('DOMContentLoaded', function () {
    try {
      // レイアウト初期化
      initEventDayLayout();

      // イベント情報取得
      loadEventInfo();

      // 担当者プルダウン初期化
      loadStaffOptions();

      // 折り畳み機能初期化
      initToggle();

    } catch (e) {
      window.force && console.error('initEventDay error', e);
    }
  });

})(jQuery);
