
  /* ========================================
   * 定数・変数
   * ======================================== */
  var KITCHEN_CAR_OUTPUT_SITE_ID = 253143;
  var KITCHEN_CAR_STATUS_SITE_ID = 253125;
  var PERIOD_SITE_ID = 253154;
  window.force = true;

  // テーブル・カラム定義
  var TABLES = {
    // キッチンカーテーブル
    KITCHEN_CAR_OUTPUT: {
      SITE_ID: KITCHEN_CAR_OUTPUT_SITE_ID,
      COLUMNS: {
        KITCHEN_CAR_IDS: 'ClassA',   // キッチンカーResultId（JSON配列）
        EVENT_ID: 'ClassB',          // LinkId/イベントID（作成時のみ）
        SHOP_NAME: 'ClassC',         // 店舗名
        NOTE: 'ClassD',              // その他詳細
        SHOP_RESULT_ID: 'ClassY',    // 店舗ResultId（作成時のみ）
        DATE_FROM: 'DateA',          // 開催期間（開始日）
        DATE_TO: 'DateB'             // 開催期間（終了日）
      }
    },
    // キッチンカー状況テーブル
    KITCHEN_CAR_STATUS: {
      SITE_ID: KITCHEN_CAR_STATUS_SITE_ID,
      COLUMNS: {
        NAME: 'ClassA',              // キッチンカー名
        EVENT_NAME: 'ClassB',        // 使用先イベント名称
        RESERVED_FROM: 'DateA',      // 直近の予約期間（開始日）
        RESERVED_TO: 'DateB',        // 直近の予約期間（終了日）
        UNAVAILABLE_FROM: 'DateC',   // 使用不可期間（開始日）
        UNAVAILABLE_TO: 'DateD'      // 使用不可期間（終了日）
      }
    },
    // 開始～終了期間テーブル
    PERIOD: {
      SITE_ID: PERIOD_SITE_ID,
      COLUMNS: {
        NAME: 'ClassA',              // 店舗名
        EVENT_ID: 'ClassB',          // イベントID（LinkId）
        START_DATE: 'DateA',         // 開始日
        END_DATE: 'DateB'            // 終了日
      }
    }
  };
