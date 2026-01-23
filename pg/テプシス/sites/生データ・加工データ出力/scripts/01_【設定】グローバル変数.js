  window.force = true;

const ENDPOINT = "/api/extended/sql";  // 相対パスで環境依存なし
const API_VERSION = 1.1;


/**
 * 列名ごとの日付フォーマット定義
 */
const DATE_FORMAT_MAP = {
  // 01.イベント
  "イベント開始日": "YYYY-MM-DD",
  "イベント終了日": "YYYY-MM-DD",
  "作成日": "YYYY-MM-DD HH:mm:ss",
  "更新日": "YYYY-MM-DD HH:mm:ss",

  // 02.イベント店舗
  "イベント店舗開催日": "YYYY-MM-DD",
  "イベント店舗終了日": "YYYY-MM-DD",

  // 03.イベント勤務
  "イベント日": "YYYY-MM-DD",
  "勤務開始時間": "HH:mm:ss",
  "勤務終了時間": "HH:mm:ss",

  // 06.キッチンカー
  "使用不可期間（開始日）": "YYYY-MM-DD",
  "使用不可期間（終了日）": "YYYY-MM-DD",

  // 07.キッチンカー利用
  "開始日": "YYYY-MM-DD",
  "終了日": "YYYY-MM-DD",

  // 08.派遣者出力
  "派遣不可期間(開始日)": "YYYY-MM-DD",
  "派遣不可期間(終了日)": "YYYY-MM-DD",

  // 09.派遣者利用出力
  "派遣日": "YYYY-MM-DD",

  // 12.メディア露出の経済価値出力
  "放送日/発行日": "YYYY-MM-DD"

};
