開発資料
プロジェクト全体

* 進捗管理
    * 各自対応しているタスク、依頼しているタスクの登録・管理を行う場所です
    * https://ssj-pleasanter-02.local.sdt-autolabo.com/items/2801416/kamban
* 確認事項
    * チーム内で確認事項・報告事項がある場合に使用します。朝会で確認いたします。
    * スラックでは過去ログが消えてしまうため基本確認事項はこのサイトからお願いいたします。
    * https://ssj-pleasanter-02.local.sdt-autolabo.com/items/2833371/index

開発環境

* 67環境
    * 各自アカウントを発行しているのでログインして作業をお願いいたします。
    * http://192.168.10.67/items/252590/index



開発ルール

* 画面内から起動用のトリガーを自分で追加する場合
    * スクリプトのトリガーについてプリザンターデフォルトで生成されているDOMを使う場合は特段指定はありません。
    * 自分でclass等を追加する場合は必ず接頭辞にfn-とつけてください。これはスクリプト処理のclassを明示する目的となります。
* スクリプト作成について
    * APIを使うときのサイトIDをスクリプト内で使用することは禁止となります。
        * サイトIDと記載したスクリプト内にてグローバル変数として定義しそれを使用してください。
        * お客様環境へアップロードする際、変更箇所をまとめるためです。
    * 作業するにあたり軽くコードレビューした際に統一したほうがいい箇所をまとめましたのでご対応お願いいたします。
        各自、スクリプトを作る際下記のスクリプトが作成されていなければ中身あるなしにかかわらず必ず追加するようにしてください
        ※必ずすべてにチェックを入れてください。また、各スクリプトの役割が全体的にわかりにくいです。最終的にサンプルなどは削除していただき【】で役割を定義してください。
        * 【設定】サイトID
        * 【設定】サイトID取得
        * 【設定】グローバル変数値


    * カテゴリは下記のような形で分かりやすくしてください。
        * 【設定】：CDN読み込みや変数定義や何か定義が必要な場合に指定して下さい。
        * 【画面制御】
        * 【表示制御】
        * 【共通処理】
        * 【View】：Vue.jsのビューティファイ使用しているものに指定してください。
            
* ログについて
    * console.logなどを使用してチェックなどを行う場合、そのままconsole.logを使うのではなく下記のコードをグローバル変数値にいれてください

// デバッグON/OFF（グローバル）window.force = true; // false にすると出ない


    * 使用する際は各コードに以下のような感じでログを記載して下さい。
        ※これはリリース時にも残します。

window.force && console.log('処理開始', { id: 123, name: 'テスト' });
window.force && console.warn('注意: 設定が未定義です');window.force && console.error('致命的エラー');

汎用関数

* ローディング表示
    * カウントが１以上の場合は読み込み中を出し、０の時は消す。
        ⇒こうすると、各処理で開始時にカウント＋１、完了時にカウント－1にするだけなので制御が楽です。
    * 実装手順

下記のコードをスクリプトを新規作成して入れる

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

* エラー表示
    * 使うことでポップアップでエラー表示を行う
    * 実装手順

下記のコードをスクリプトを新規作成して入れる

function genErrorNo() {
  return 'E-' + Date.now();
}

// 共通エラーアラート
function showCommonError(errNo) {
  const no = errNo || genErrorNo();
  alert(
    'エラーが発生しました。\n' +
    '操作をやり直しても解消しない場合は、\n' +
    'このエラー内容を管理者にお伝えください。\n\n' +
    no
  );
}
// 使い方：
// どこでもこれだけ
// showCommonError();

// 既にエラー番号がある場合
// showCommonError('E-API-00123');

