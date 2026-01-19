// 1.グローバル変数値として下記を設定
window.loadingCount = window.loadingCount ?? 0;
window.loadingMsg   = window.loadingMsg   ?? '読み込み中...';

// 2．共通処理で読み込み中表示を作成する
// =======================================
// 読み込み表示
// - count が 1 以上なら表示、0 で非表示
// - incLoading(msg) / decLoading() を使う
// =======================================


// =======================================
// _ensureOL
// 説明：読み込みオーバーレイの DOM とスタイルを作成（既にあれば何もしない）
// =======================================
function _ensureOL(){
  if (document.getElementById('loading-ol')) return;
  // 最低限のアニメーション CSS を追加
  const s = document.createElement('style');
  s.textContent = '@keyframes spin{to{transform:rotate(360deg)}}';
  document.head.appendChild(s);

  // オーバーレイ本体
  const ol = document.createElement('div');
  ol.id = 'loading-ol';
  ol.style.cssText = 'position:fixed;inset:0;display:none;align-items:center;justify-content:center;background:rgba(0,0,0,.25);z-index:2147483647';

  // 中身（白ボックス + スピナー + メッセージ）
  ol.innerHTML = '<div style="background:#fff;padding:10px 14px;border-radius:6px;display:flex;gap:10px;align-items:center">'
               + '<span style="width:18px;height:18px;border:3px solid #ddd;border-top-color:#333;border-radius:50%;animation:spin 1s linear infinite"></span>'
               + `<span id="loading-ol-msg">${window.loadingMsg}</span></div>`;

  document.body.appendChild(ol);
}

// =======================================
// incLoading
// 説明：読み込みカウントをインクリメント。最初の1回目で表示。
// 引数：message（省略可） — 表示メッセージを一時的に変更
// =======================================
window.incLoading = function(msg){
  window.loadingCount = (window.loadingCount || 0) + 1;
  _ensureOL();
  if (msg) {
    const m = document.getElementById('loading-ol-msg');
    if (m) m.textContent = msg;
  }
  if (window.loadingCount === 1) {
    document.getElementById('loading-ol').style.display = 'flex';
  }
};

// =======================================
// decLoading
// 説明：読み込みカウントをデクリメント。0 になったら非表示。
// 注意：呼び出し回数と対応させる（負にはならない）
// =======================================
window.decLoading = function(){
  window.loadingCount = Math.max(0, (window.loadingCount || 0) - 1);
  if (window.loadingCount === 0) {
    const el = document.getElementById('loading-ol');
    if (el) el.style.display = 'none';
  }
};
// ■使い方
// incLoading();             // 表示（count = +1）
// incLoading('読込中...');  // メッセージ変更して表示

// decLoading();             // 非表示は count が 0 になった時