window.force = true;

  // テーブル・カラム定義
  var TABLES = {
    // イベント当日テーブル
    EVENT_DAY: {
      SITE_ID: EVENT_DAY_SITE_ID,
      COLUMNS: {
        SHOP_NAME: 'ClassA',           // 店舗名
        STAFF: 'ClassB',               // 担当者（リンク項目）
        FREE_TEXT: 'DescriptionA',     // 自由欄記入
        WORK_DATE: 'DateC',            // 勤務日
        WORK_START: 'DateA',           // 勤務開始時間
        WORK_END: 'DateB',             // 勤務終了時間
        PERIOD_ID: 'ClassZ'            // 開始～終了期間（リンク項目）
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
    },
    // イベント予定一覧テーブル
    EVENT_LIST: {
      SITE_ID: EVENT_LIST_SITE_ID,
      COLUMNS: {
        CATEGORY: 'ClassA',            // カテゴリー
        EVENT_NAME: 'ClassB',          // イベント名・出店名
        ORGANIZER: 'ClassC',           // 開催者or依頼主
        AREA: 'ClassF',                // 開催エリア
        AREA_DETAIL: 'DescriptionB'    // 開催エリア詳細
      }
    },
    // 先方（要人）テーブル
    VIP: {
      SITE_ID: VIP_SITE_ID,
      COLUMNS: {
        SHOP_NAME: 'ClassA',           // 店舗名
        COMPANY: 'ClassB',             // 会社
        DEPT: 'ClassC',                // 部署
        POSITION: 'ClassD',            // 役職
        NAME: 'ClassE',                // 氏名
        EVENT_ID: 'ClassF',            // イベントID（リンク項目）
        EVENT_SHOP_ID: 'ClassY'        // イベント店舗ID（開始～終了期間のID）
      }
    }
  };
