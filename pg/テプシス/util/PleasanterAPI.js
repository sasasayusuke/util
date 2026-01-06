/* ========= ここだけ書き換えてください ========= */
const SITE_ID = 24;               //  SiteId
const BASE    = location.origin;  // サーバー直下に Pleasanter がある例
/* ============================================= */

(async () => {
  // ---- リクエスト用ペイロード ----
  const payload = {
    ApiVersion: 1.1,
    View      : {
      GridColumns: ["ResultId", "Title", "Body", "UpdatedTime"],
      SortHash   : { UpdatedTime: "DESC" },
    }
  };

  // ---- API 実行 ----
  const res  = await fetch(`${BASE}/api/items/${SITE_ID}/get`, {
    method : "POST",
    headers: { "Content-Type": "application/json" },
    body   : JSON.stringify(payload)
  });

  if (!res.ok) {
    console.error("HTTP Error:", res.status, res.statusText);
    return;
  }

  const data = await res.json();
  const pages = data.Response?.Data ?? [];

  // ---- 一覧をコンソールに表示 ----
  console.table(
    pages.map(p => ({
      id     : p.ResultId,
      title  : p.Title,
      updated: p.UpdatedTime
    }))
  );

  // ---- いちばん新しいページの本文を変数に保持 ----
  window.latestWiki = pages[0];          // 0 件なら undefined
  console.log("最新ページ HTML を window.latestWiki.Body に格納しました ↓");
  console.log(window.latestWiki?.Body);
})();
