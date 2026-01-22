(function ($) {
  'use strict';

  // 初期表示
  $(function () {
    syncClassN();
  });

  // ラジオ変更
  $(document).on('change', 'input[name="Results_ClassI"]', function () {
    syncClassN();
  });

  // マルチセレクト変更
  $(document).on('change', 'input[name="multiselect_Results_ClassJ"]', function () {
    syncClassN();
  });

  // 店舗数変更
  $(document).on('input change', '#Results_ClassK', function () {
    syncClassN();
  });

  /**
   * 店舗管理方法に応じて Results_ClassN に値をセット
   */
  function syncClassN() {

    // ★ ラジオの表示テキストで判定
    var classIText = $('input[name="Results_ClassI"]:checked')
      .closest('label')
      .text()
      .trim();

    var value = '';

    if (classIText === '店舗ごと入力') {

      // ClassK を空にする
      $('#Results_ClassK').val('');
      $p.set($('#Results_ClassK'), '');

      // 選択されている店舗名（表示名）を取得
      var selectedTexts = $('input[name="multiselect_Results_ClassJ"]:checked')
        .map(function () {
          return $(this).closest('label').find('span').text().trim();
        })
        .get();

      value = selectedTexts.join('、');

    } else if (classIText === '店舗数') {

      // ClassJ を空にする
      $('#Results_ClassJ').val('');
      $p.set($('#Results_ClassJ'), '');

      // 店舗数 → ClassK
      value = $('#Results_ClassK').val() || '';
    }

    // ClassN に反映
    $('#Results_ClassN').val(value);
    $p.set($('#Results_ClassN'), value);
  }

})(jQuery);
