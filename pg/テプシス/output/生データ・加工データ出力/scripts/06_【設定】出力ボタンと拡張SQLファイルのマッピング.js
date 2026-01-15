// パラメーター無しのエクスポート
const exportWithoutParameter =
{
  "fn-product-achivement-export": "getStorePerformance",        // 品目実績
  "fn-kitchencar-export": "getKitchenCar",                      // キッチンカー
  "fn-kitchencar-use-export": "getUseOfFoodTrucks",             // キッチンカー利用
  "fn-employee-export": "DispatchStatus",                       // 派遣者
  "fn-employee-use-export": "getForDispatchedWorkers",          // 派遣者利用
  "fn-client-export": "getImportantPerson",                     // 先方（要人）
  "fn-menu-export": "getMenuNumberOfSeparateMeals",             // メニュー別食数
  "fn-media-export": "getTheEconomicValueOfMediaExposure",      // メディア露出の経済価値
  "fn-eventphoto-export": "getStorePerformanceDetails",         // イベント写真（写真名のみ）
  "fn-center-export": "getRoomCenter",                          // 室・センター
  "fn-group-export": "getGroup",                                // グループ
  "fn-member-export": "getMemberVerificationRequired",          // メンバー
  "fn-store-export": "getShop",                                 // 店舗
  "fn-contractor-export": "getContractorInformation",           // 委託先情報(社名)
  "fn-tv-radio-export": "getEconomicValueTelevisionAndRadio",   // メディア露出の経済価値（テレビ・ラジオ）
  "fn-newspaper-export": "getEconomicValueNewspaper",           // メディア露出の経済価値（新聞）
  "fn-brand-export": "getVarietyBrand",                         // 品種（ブランド）
  "fn-supplier-export": "getSupplier",                          // 仕入先
  "fn-rank-export": "getRankAndGrade",                          // ランク
  "fn-peach-packaging-export": "getPackaging",                  // 包装
  "fn-beef-part-export": "getPart",                             // 部位
  "fn-scallops-type-export": "getKindsScallops"                 // 種類（ホタテ）
}                

// パラメーター有りのエクスポート
const exportWithParameter =
{
  "fn-event-export": "getEvent",                    // イベント
  "fn-event-store-export": "getEventShop",        // イベント店舗
  "fn-event-work-export": "getEventWork",          // イベント勤務
  "fn-event-weather-export": "getEventWeather",    // イベント天気
  "fn-processed-data-export": "getProcessingData"  // 加工データ
}          
