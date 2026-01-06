(function () {
  "use strict";

  if (!window.$p || typeof $p.apiGet !== "function" || !$p.events) return;

  let all = [];
  let getFlag = false;
  let clickBound = false;
  let storeAll = [];
  let storeFlag = false;
  let periodAll = [];
  let periodFlag = false;
  let dispatchAll = [];
  let dispatchReady = false;
  let noveltyAll = [];
  let noveltyReady = false;

  // ============================================================
  // HTML
  // ============================================================
  function fnPopupHtml(params) {
    const title = params?.title || "店舗ごと入力";
    const bodyHtml = params?.bodyHtml || "";

    return `
      <div class="sdt-popup-overlay" data-sdt-popup-overlay="1">
        <div class="sdt-popup" role="dialog" aria-modal="true">
          <div class="sdt-popup__header">
		  <div class="sdt-popup__title">店舗詳細</div>
            <button type="button" class="sdt-popup__close" data-sdt-popup-close="1" aria-label="閉じる">×</button>
          </div>
          <div class="sdt-popup__body">
            ${bodyHtml}
          </div>
        </div>
      </div>
    `;
  }

  // ============================================================
  // ポップアップ表示/閉じる
  // ============================================================
  let fnPopupRootEl = null;
  function fnPopupClose() {
    if (!fnPopupRootEl) return;
    fnPopupRootEl.remove();
    fnPopupRootEl = null;
    document.removeEventListener("keydown", fnPopupOnKeydown, true);
  }

  function fnPopupOnKeydown(e) {
    if (e.key === "Escape") fnPopupClose();
  }
  function fnPopupOpen(params) {
    fnPopupClose();
    const wrap = document.createElement("div");
    wrap.className = "sdt-popup-root";
    wrap.innerHTML = fnPopupHtml(params);
    wrap.addEventListener("click", function (e) {
      const overlay =
        e.target.closest && e.target.closest('[data-sdt-popup-overlay="1"]');
      if (overlay && e.target === overlay) fnPopupClose();
    });
    wrap.addEventListener("click", function (e) {
      const btn =
        e.target.closest && e.target.closest('[data-sdt-popup-close="1"]');
      if (btn) fnPopupClose();
    });
    document.body.appendChild(wrap);
    fnPopupRootEl = wrap;
  }

  // ============================================================
  // クリックされた要素から ResultId を拾う
  // ============================================================
  function fnPickId(target) {
    const tr = target.closest && target.closest("tr");
    if (!tr) return null;
    const cand = tr.getAttribute("data-id");
    return cand ? String(cand) : null;
  }

  // ============================================================
  // キャッシュ(all)から ResultId 一致でレコードを引く
  // ============================================================
  function fnFindByResultId(resultId) {
    if (!resultId) return null;
    const rid = String(resultId);
    return all.find((r) => String(r?.ResultId ?? "") === rid) || null;
  }

  // ============================================================
  // JSON文字列→配列
  // ============================================================
  function fnParseIdList(jsonStr) {
    if (!jsonStr) return [];
    try {
      const v = JSON.parse(jsonStr);
      return Array.isArray(v) ? v.map(String) : [];
    } catch (e) {
      console.log("ClassJのJSON.parseに失敗", { jsonStr, e });
      return [];
    }
  }

  // ============================================================
  // 店舗ID → 店舗名(ClassA)
  // ============================================================
  function fnFindStoreNameByResultId(resultId) {
    if (!resultId) return null;
    const rid = String(resultId);
    const rec = storeAll.find((s) => String(s?.ResultId ?? "") === rid) || null;
    return rec?.ClassHash?.ClassA || null;
  }

  // ============================================================
  // 文字列正規化
  // ============================================================
  function fnNormalizeName(name) {
    return String(name || "")
      .replace(/\r?\n/g, "")
      .replace(/\u3000/g, " ")
      .replace(/\s+/g, " ")
      .trim();
  }

  // ============================================================
  // ClassB が JSON文字列/空白混入でも一致させる
  // ============================================================
  function fnNormalizeIdValue(v) {
    const s = String(v ?? "").trim();
    if (!s) return "";
    if (s.startsWith("[")) {
      try {
        const arr = JSON.parse(s);
        if (Array.isArray(arr) && arr.length) return String(arr[0]).trim();
      } catch (e) {
        /* noop */
      }
    }
    return s;
  }

  function fnFindPeriodsByEventId(eventResultId) {
    const eid = String(eventResultId).trim();
    return periodAll.filter(
      (p) => fnNormalizeIdValue(p?.ClassHash?.ClassB) === eid
    );
  }

  // ============================================================
  // 店舗名をキーに、開始/終了を作る
  // ============================================================
  function fnBuildPeriodMap(periodRows) {
    const map = {}; // { 店舗名: { start, end } }
    periodRows.forEach((pe) => {
      const storeName = fnNormalizeName(pe?.ClassHash?.ClassA);
      if (!storeName) return;
      const start = pe?.DateHash?.DateA || "";
      const end = pe?.DateHash?.DateB || "";
      if (!map[storeName]) {
        map[storeName] = { start, end };
        return;
      }
      if (start && (!map[storeName].start || start < map[storeName].start))
        map[storeName].start = start;
      if (end && (!map[storeName].end || end > map[storeName].end))
        map[storeName].end = end;
    });
    return map;
  }

  function fnFormatDate(v) {
    const s = String(v || "");
    // 2025-12-24T00:00:00 → 2025-12-24
    if (s.includes("T")) return s.split("T")[0];
    return s;
  }

  // ============================================================
  // 遷移を強制停止
  // ============================================================
  function fnStop(e) {
    e.preventDefault();
    e.stopPropagation();
    if (typeof e.stopImmediatePropagation === "function")
      e.stopImmediatePropagation();
  }

  // ============================================================
  // apiGet を Promise 化
  // ============================================================
  function fnApiGetAsPromise(siteId) {
    return new Promise((resolve, reject) => {
      $p.apiGet({
        id: siteId,
        done: function (data) {
          resolve(data?.Response?.Data || []);
        },
        fail: function (err) {
          reject(err);
        },
      });
    });
  }

  // ============================================================
  // ============================================================
  // 派遣者：イベントIDで行を集める（ClassB=イベントResultId）
  // ============================================================
  // ============================================================
  function fnFindDispatchersByEventId(eventResultId) {
    const eid = String(eventResultId ?? "").trim();
    if (!eid) return [];
    return dispatchAll.filter(
      (d) => fnNormalizeIdValue(d?.ClassHash?.ClassB) === eid
    );
  }

  // ============================================================
  // ============================================================
  // 派遣Map作成（店舗名キー → true）
  // ============================================================
  // ============================================================
  // ・派遣者テーブルの ClassA を「店舗名」として扱う
  // ・比較ブレ防止のため、必ず fnNormalizeName で正規化してキーにする
  // ・同一店舗が複数あっても true のまま（存在判定だけ）
  // ============================================================
  function fnBuildDispatchMapByStoreName(dispatchRows) {
    const map = {}; // { 正規化店舗名: true }

    dispatchRows.forEach((d) => {
      const storeNameRaw = d?.ClassHash?.ClassA; // ★店舗名
      const storeKey = fnNormalizeName(storeNameRaw);
      if (!storeKey) return;
      map[storeKey] = true;
    });

    return map;
  }

  // ============================================================
  // ノベルティ：イベントIDで行を集める（ClassB=イベントResultId）
  // ============================================================
  function fnFindNoveltiesByEventId(eventResultId) {
    const eid = String(eventResultId ?? "").trim();
    if (!eid) return [];
    return noveltyAll.filter(
      (n) => fnNormalizeIdValue(n?.ClassHash?.ClassB) === eid
    );
  }

  // ============================================================
  // ノベルティMap作成（店舗名キー → true）
  // ============================================================
  function fnBuildNoveltyMapByStoreName(noveltyRows) {
    const map = {};
    noveltyRows.forEach((n) => {
      const storeNameRaw = fnNormalizeIdValue(n?.ClassHash?.ClassA);
      const storeKey = fnNormalizeName(storeNameRaw);
      if (!storeKey) return;
      map[storeKey] = true;
    });
    return map;
  }

  // ============================================================
  // ポップアップ内 店舗名リンク押下 → 詳細・編集画面へ遷移
  // ============================================================
  document.addEventListener(
    "click",
    function (e) {
      const link =
        e.target.closest && e.target.closest(".sdt-popup-event-link");
      if (!link) return;
      const eventResultId = link.getAttribute("data-event-resultid");
      if (!eventResultId) return;
      const url = `/items/${eventResultId}`;
      e.preventDefault();
      e.stopPropagation();
      window.location.href = url;
    },
    true
  );

  // ============================================================
  // レコード押下時：ClassHash.ClassI を見て、条件一致なら遷移停止
  // ============================================================
  function fnPush(e) {
    const grid = document.getElementById("Grid");
    const clickedId = fnPickId(e.target);
    const row = fnFindByResultId(clickedId);
    const classI = row?.ClassHash?.ClassI;

    if (!getFlag || !all.length) return;
    if (!grid || !grid.contains(e.target)) return;
    if (!clickedId) return;
    if (!row) return;
    if (classI !== "店舗ごと入力") return;

    fnStop(e);
    if (!storeFlag) {
      fnPopupOpen({
        title: "店舗ごと入力（ポップアップ）",
        bodyHtml: `<div class="sdt-popup__note">店舗情報を読込中です。もう一度クリックしてください。</div>`,
      });
      return;
    }
    if (!periodFlag) {
      fnPopupOpen({
        title: "店舗ごと入力（ポップアップ）",
        bodyHtml: `<div class="sdt-popup__note">期間情報を読込中です。もう一度クリックしてください。</div>`,
      });
      return;
    }
    if (!dispatchReady) {
      fnPopupOpen({
        title: "店舗ごと入力（ポップアップ）",
        bodyHtml: `<div class="sdt-popup__note">派遣者情報を読込中です。もう一度クリックしてください。</div>`,
      });
      return;
    }
    if (!noveltyReady) {
      fnPopupOpen({
        title: "店舗ごと入力（ポップアップ）",
        bodyHtml: `<div class="sdt-popup__note">ノベルティ情報を読込中です。もう一度クリックしてください。</div>`,
      });
      return;
    }

    const eventResultId = String(row?.ResultId ?? "");
    const storeIds = fnParseIdList(row?.ClassHash?.ClassJ);
    const periodRows = fnFindPeriodsByEventId(row?.ResultId);
    const periodMap = fnBuildPeriodMap(periodRows);
    const dispatchRows = fnFindDispatchersByEventId(row?.ResultId);
    const dispatchMap = fnBuildDispatchMapByStoreName(dispatchRows);
    const noveltyRows = fnFindNoveltiesByEventId(row?.ResultId);
    const noveltyMap = fnBuildNoveltyMapByStoreName(noveltyRows);
    const rowsHtml = storeIds
      .map((id) => {
        const storeName =
          fnFindStoreNameByResultId(id) || `店舗名が同一ではありません`;
        const key = fnNormalizeName(storeName);
        const period = periodMap[key] || {};
        const start = fnFormatDate(period.start || "");
        const end = fnFormatDate(period.end || "");
        const dispatchChecked = !!dispatchMap[key];
        const noveltyChecked = !!noveltyMap[key];

        return `
        <tr class="sdt-popup-table__tr" data-store-resultid="${id}">
          <td class="sdt-popup-table__td">
            <span
              class="sdt-popup-event-link"
              data-event-resultid="${eventResultId}"
            >${storeName}</span>
          </td>
          <td class="sdt-popup-table__td">${start}</td>
          <td class="sdt-popup-table__td">${end}</td>
          <td class="sdt-popup-table__td">
            <input class="sdt-popup-checkbox" type="checkbox" ${
              dispatchChecked ? "checked" : ""
            } disabled>
          </td>
          <td class="sdt-popup-table__td">
            <input class="sdt-popup-checkbox" type="checkbox" ${
              noveltyChecked ? "checked" : ""
            } disabled>
          </td>
        </tr>
      `;
      })
      .join("");

    const safeRowsHtml =
      rowsHtml ||
      `<tr><td class="sdt-popup-table__td" colspan="5">店舗が選択されていません</td></tr>`;

    fnPopupOpen({
      bodyHtml: `
        <table class="sdt-popup-table">
          <thead>
            <tr>
              <th class="sdt-popup-table__th">店舗名</th>
              <th class="sdt-popup-table__th">開始日</th>
              <th class="sdt-popup-table__th">終了日</th>
              <th class="sdt-popup-table__th">派遣要否</th>
              <th class="sdt-popup-table__th">ノベルティ配布</th>
            </tr>
          </thead>
          <tbody>
            ${safeRowsHtml}
          </tbody>
        </table>
      `,
    });
  }

  // ============================================================
  // 一覧ロード：全件取得
  // ============================================================
  $p.events.on_grid_load = function () {
    getFlag = false;
    all = [];
    storeFlag = false;
    storeAll = [];
    periodFlag = false;
    periodAll = [];
    dispatchReady = false;
    dispatchAll = [];
    noveltyReady = false;
    noveltyAll = [];

    Promise.all([
      fnApiGetAsPromise(event_Site_Id),
      fnApiGetAsPromise(store_Site_Id),
      fnApiGetAsPromise(period_Site_Id),
      fnApiGetAsPromise(dispatch_Site_Id),
      fnApiGetAsPromise(novelty_Site_Id),
    ])
      .then(([eventRows, storeRows, periodRows, dispatchRows, noveltyRows]) => {
        all = eventRows;
        storeAll = storeRows;
        periodAll = periodRows;
        dispatchAll = dispatchRows;
        noveltyAll = noveltyRows;

        getFlag = true;
        storeFlag = true;
        periodFlag = true;
        dispatchReady = true;
        noveltyReady = true;

        console.log("イベント予定一覧の全レコード", all);
        console.log("店舗の全レコード", storeAll);
        console.log("開始～終了全レコード", periodAll);
        console.log("派遣者の全レコード", dispatchAll);
        console.log("ノベルティの全レコード", noveltyAll);
      })
      .catch((err) => {
        getFlag = false;
        storeFlag = false;
        periodFlag = false;
        dispatchReady = false;
        noveltyReady = false;
        all = [];
        storeAll = [];
        periodAll = [];
        dispatchAll = [];
        noveltyAll = [];

        console.warn("一覧ロード取得失敗", err);
      });

    if (!clickBound) {
      clickBound = true;
      document.addEventListener("click", fnPush, true);
    }
  };
})();
