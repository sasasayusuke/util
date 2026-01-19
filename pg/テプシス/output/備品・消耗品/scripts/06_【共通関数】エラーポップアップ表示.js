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

  // デバッグ用（任意）
  window.force && console.error('ERROR:', no);
}
// 使い方：
// どこでもこれだけ
// showCommonError();

// 既にエラー番号がある場合
// showCommonError('E-API-00123');
