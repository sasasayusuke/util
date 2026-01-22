(function ($) {
  'use strict';

  // -----------------------
  // オーケストレーター（エントリポイント）
  // -----------------------
  async function indexController() {
    try {
      // 1) ラジオの現在選択表示名を取得（値ではなく表示名で返す）
      var selectedDisplay = getSelectedClassI();

      // 2) 選択表示名に応じて表示/非表示を切り替え
      updateVisibilityBySelection(selectedDisplay);

      // 3) イベント（ラジオ change）の登録（冪等にする）
      bindUiEvents();

      return;
    } catch (err) {
      if (typeof console !== 'undefined' && console.error) console.error('indexController error:', err);
    }
  }

  // -----------------------
  // ヘルパー群
  // -----------------------

  // ラジオの現在選択表示名を取得する（.control-radio の checked を優先）
  // 取得順序: closest label -> label[for=] -> 隣接テキストノード -> data-display -> 値からのマッピング
  function getSelectedClassI() {
    var $checked = $('input.control-radio:checked');
    if ($checked.length) {
      // 1) label 要素を探す（<label><input>表示</label> の想定）
      var labelText = '';
      var $lbl = $checked.closest('label');
      if ($lbl.length) labelText = $lbl.text();

      // 2) label[for="id"] のパターン
      if (!labelText) {
        var id = $checked.attr('id');
        if (id) {
          var $for = $('label[for="' + id + '"]');
          if ($for.length) labelText = $for.text();
        }
      }

      // 3) input の隣接ノードに直接テキストがある場合
      if (!labelText) {
        var next = $checked[0].nextSibling;
        if (next) {
          // テキストノードや要素ノードの両方に対応
          labelText = (next.nodeType === 3) ? (next.nodeValue || '') : $(next).text();
        }
      }

      labelText = (labelText || '').trim();
      if (labelText) return labelText;

      // 4) data 属性で表示名が持たれているケース
      var dd = $checked.data('display');
      if (dd) return String(dd).trim();

      // 5) 最後のフォールバック: 値から既知の表示名へマッピング
      var val = String($checked.val());
      if (val === '1') return '店舗ごと入力';
      if (val === '2') return '店舗数';

      return '';
    }

    // フォールバック: 隠し input の値を見てマッピング（既存の仕組みを壊さない）
    var $hidden = $('#Results_ClassI');
    if ($hidden.length) {
      var hv = String($hidden.val());
      if (hv === '1') return '店舗ごと入力';
      if (hv === '2') return '店舗数';
      return '';
    }

    return '';
  }

  // 選択表示名（'店舗ごと入力' または '店舗数'）に基づいて表示/非表示を切り替える
  function updateVisibilityBySelection(selectedDisplay) {
    var $labelA = $('#Results_ClassJField').prev('div'); // 「店舗ごと入力」側ラベル
    var $groupA = $('#Results_ClassJField');

    var $labelB = $('#Results_ClassKField').prev('div'); // 「店舗数」側ラベル
    var $groupB1 = $('#Results_ClassKField');
    var $groupB2 = $('#Results_NumRField');

    // 表示名が "店舗ごと入力" のとき A を表示、B を非表示
    if (selectedDisplay === '店舗ごと入力') {
      $labelA.show();
      $groupA.show();

      $labelB.hide();
      $groupB1.hide();
      $groupB2.hide();
      return;
    }

    // 表示名が "店舗数" のとき B を表示、A を非表示
    if (selectedDisplay === '店舗数') {
      $labelA.hide();
      $groupA.hide();

      $labelB.show();
      $groupB1.show();
      $groupB2.show();
      return;
    }

    // 想定外の表示名: 両方非表示（安全策）
    $labelA.hide();
    $groupA.hide();
    $labelB.hide();
    $groupB1.hide();
    $groupB2.hide();
  }

  // ラジオ change イベントをバインド（何度呼ばれても重複登録しない）
  function bindUiEvents() {
    $('input.control-radio').off('.resultsClassI').on('change.resultsClassI', function () {
      try {
        indexController();
      } catch (e) {
        if (typeof console !== 'undefined' && console.error) console.error('radio change handler error:', e);
      }
    });
  }

  // -----------------------
  // 初期トリガー設定（DOM ready / $p.events.after_set）
  // -----------------------

  $(function () {
    try {
      indexController();
    } catch (e) {
      if (typeof console !== 'undefined' && console.error) console.error('DOMContentLoaded trigger error:', e);
    }
  });

  if (typeof $p !== 'undefined' && $p.events) {
    var originalAfterSet = $p.events.after_set;
    $p.events.after_set = function () {
      if (typeof originalAfterSet === 'function') {
        try { originalAfterSet.apply(this, arguments); } catch (e) { if (typeof console !== 'undefined' && console.error) console.error(e); }
      }
      try {
        indexController();
      } catch (e) {
        if (typeof console !== 'undefined' && console.error) console.error('$p.events.after_set trigger error:', e);
      }
    };
  }

})(jQuery);
