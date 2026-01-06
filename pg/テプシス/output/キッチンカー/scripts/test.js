(() => {
  // 1) 既存UIを隠す（PleasanterのDOMに合わせてセレクタ調整）
  const hideTargets = [
    "#MainForm",        // 例
    ".main-form",       // 例
    ".content-body"     // 例
  ];
  hideTargets.forEach(sel => {
    const el = document.querySelector(sel);
    if (el) el.style.display = "none";
  });

  // 2) 新UIのルートを追加
  const root = document.createElement("div");
  root.id = "kitchen-car-app";
  root.style.padding = "16px";
  document.querySelector("#Contents")?.prepend(root); // 例: 挿入先
  if (!root.parentElement) document.body.prepend(root);

  // 3) UIを描画（超簡易。実際はあなたのレイアウトに合わせてHTML生成）
  root.innerHTML = `
    <style>
      .kc-card{ border:1px solid #ddd; border-radius:8px; padding:12px; background:#fff; }
      .kc-row{ display:flex; gap:12px; flex-wrap:wrap; margin-bottom:12px; }
      .kc-field{ display:flex; flex-direction:column; gap:6px; min-width:240px; }
      .kc-table{ width:100%; border-collapse:collapse; }
      .kc-table th,.kc-table td{ border-bottom:1px solid #eee; padding:10px; text-align:left; }
      .kc-table-wrap{ max-height:220px; overflow:auto; border:1px solid #eee; border-radius:8px; }
      .kc-actions{ display:flex; gap:12px; justify-content:center; margin-top:16px; }
      .btn{ padding:10px 16px; border-radius:8px; border:1px solid #ccc; cursor:pointer; background:#f8f8f8; }
      .btn.primary{ background:#2e7dff; color:#fff; border-color:#2e7dff; }
    </style>

    <div class="kc-card">
      <div class="kc-row">
        <div class="kc-field">
          <label>店舗 *</label>
          <select id="kc-shop">
            <option value="">選択してください</option>
            <option value="北海道">北海道</option>
          </select>
        </div>

        <div class="kc-field">
          <label>開催期間 *</label>
          <div style="display:flex; gap:8px; align-items:center;">
            <input id="kc-from" type="date" />
            <span>〜</span>
            <input id="kc-to" type="date" />
          </div>
        </div>
      </div>

      <div class="kc-table-wrap">
        <table class="kc-table">
          <thead>
            <tr>
              <th style="width:50%;">キッチンカー</th>
              <th style="width:35%;">使用状況</th>
              <th style="width:15%;">選択</th>
            </tr>
          </thead>
          <tbody id="kc-rows"></tbody>
        </table>
      </div>

      <div class="kc-row" style="margin-top:12px;">
        <div class="kc-field" style="flex:1; min-width:320px;">
          <label>その他詳細</label>
          <input id="kc-note" type="text" placeholder="" />
        </div>
      </div>

      <div class="kc-actions">
        <button class="btn primary" id="kc-submit">キッチンカー登録</button>
        <button class="btn" id="kc-cancel">キャンセル</button>
      </div>
    </div>
  `;

  // 4) 一覧データ（本来はAPI/マスタから取得）
  const cars = [
    { id: "A", name: "キッチンカーA", status: "テストで使用中" },
    { id: "B", name: "キッチンカーB", status: "使用可能" },
    { id: "D", name: "キッチンカーD", status: "使用可能" },
  ];

  const rows = document.querySelector("#kc-rows");
  rows.innerHTML = cars.map(c => `
    <tr>
      <td>${c.name}</td>
      <td>${c.status}</td>
      <td><input type="checkbox" class="kc-pick" value="${c.id}" /></td>
    </tr>
  `).join("");

  // 5) 既存Pleasanterの入力欄に値をセットして「保存」する（セレクタ要調整）
  function setPleasanterValue(selector, value) {
    const el = document.querySelector(selector);
    if (!el) return false;
    el.value = value;
    el.dispatchEvent(new Event("input", { bubbles: true }));
    el.dispatchEvent(new Event("change", { bubbles: true }));
    return true;
  }

  document.querySelector("#kc-submit").addEventListener("click", () => {
    const shop = document.querySelector("#kc-shop").value;
    const from = document.querySelector("#kc-from").value;
    const to   = document.querySelector("#kc-to").value;
    const note = document.querySelector("#kc-note").value;
    const picked = Array.from(document.querySelectorAll(".kc-pick:checked")).map(x => x.value);

    if (!shop || !from || !to) {
      alert("店舗と開催期間は必須です");
      return;
    }
    if (picked.length === 0) {
      alert("キッチンカーを1つ以上選択してください");
      return;
    }

    // ▼ここをあなたのPleasanter項目にマッピング（例）
    // 例: 店舗 = #ClassA, 期間From = #DateA, 期間To = #DateB, 選択車 = #TextA, 詳細 = #Description
    setPleasanterValue("#ClassA", shop);
    setPleasanterValue("#DateA", from);
    setPleasanterValue("#DateB", to);
    setPleasanterValue("#TextA", picked.join(","));
    setPleasanterValue("#Description", note);

    // 保存ボタンを押す（Pleasanterの保存ボタンのセレクタを合わせる）
    const saveBtn = document.querySelector('button[name="Save"], .command-save, #SaveCommand');
    if (!saveBtn) {
      alert("保存ボタンが見つかりません（セレクタ調整が必要）");
      return;
    }
    saveBtn.click();
  });

  document.querySelector("#kc-cancel").addEventListener("click", () => {
    history.back(); // もしくは Pleasanterのキャンセルボタンclick
  });
})();
