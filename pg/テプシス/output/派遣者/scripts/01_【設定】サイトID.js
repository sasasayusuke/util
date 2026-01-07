
  /* ========================================
   * 定数・変数
   * ======================================== */
  var DISPATCH_OUTPUT_SITE_ID = 253132;      // 派遣者テーブル
  var DISPATCH_STATUS_SITE_ID = 253126;      // 派遣状況テーブル
  var PERIOD_SITE_ID = 253154;               // 店舗・期間マスタ
  window.force = true;

  // テーブル・カラム定義
  var TABLES = {
    // 派遣者テーブル（出力先）
    DISPATCH_OUTPUT: {
      SITE_ID: DISPATCH_OUTPUT_SITE_ID,
      COLUMNS: {
        SHOP_NAME: 'ClassA',           // 店舗名
        EVENT_ID: 'ClassB',            // イベント予定一覧（LinkId）
        DISPATCH_IDS: 'ClassC',        // 派遣者名（JSON配列）
        SHOP_RESULT_ID: 'ClassY',      // イベント店舗ID
        DATE: 'DateA'                  // 日付（単日）
      }
    },
    // 派遣状況テーブル（マスタ）
    DISPATCH_STATUS: {
      SITE_ID: DISPATCH_STATUS_SITE_ID,
      COLUMNS: {
        NAME: 'ClassA',                // 派遣名
        EVENT_NAME: 'ClassB',          // 派遣先イベント名称
        RESERVED_DATE: 'DateA',        // 直近の派遣日
        UNAVAILABLE_FROM: 'DateB',     // 派遣不可期間（開始日）
        UNAVAILABLE_TO: 'DateC'        // 派遣不可期間（終了日）
      }
    },
    // 開始～終了期間テーブル
    PERIOD: {
      SITE_ID: PERIOD_SITE_ID,
      COLUMNS: {
        NAME: 'ClassA',                // 店舗名
        EVENT_ID: 'ClassB',            // イベントID（LinkId）
        START_DATE: 'DateA',           // 開始日
        END_DATE: 'DateB'              // 終了日
      }
    }
  };
