(function ($) {
  'use strict';

  /************************************************************************
   * キッチンカー登録処理
   *
   * 概要：
   * - ページの「キッチンカー登録」ボタンを押すと実行されます。
   * - 処理の流れ：
   *   1) フォームの入力値を読み取る（店舗、期間、選択したキッチンカー）
   *   2) バリデーションを実行
   *   3) ダミーデータをテーブルに描画（初期表示用）
   *   4) Pleasanterのフォーム要素に値をセット
   *   5) 保存ボタンをクリック
   *
   * 重要な設計方針：
   * - 関数はすべて内部に閉じています（グローバルに公開しません）
   * - HTMLのテンプレート行（tr.fn-tableColumn-loop）を使って一覧を描画します
   ************************************************************************/

  /* ----------------------------------------
   * 1) キッチンカーデータ（本来はAPI/マスタから取得）
   * ---------------------------------------- */
  var kitchenCarData = [
    { id: 'A', name: 'キッチンカーA', status: 'テストで使用中' },
    { id: 'B', name: 'キッチンカーB', status: '使用可能' },
    { id: 'D', name: 'キッチンカーD', status: '使用可能' }
  ];

  /* ----------------------------------------
   * 2) キッチンカー一覧をテーブルに描画
   * ---------------------------------------- */
  function renderKitchenCarTable(cars) {
    if (!Array.isArray(cars) || cars.length === 0) {
      window.force && console.error('renderKitchenCarTable: no data');
      return;
    }

    var $tbody = $('#kc-kitchen-car-table .kc-table__body');
    if ($tbody.length === 0) {
      $tbody = $('#kc-kitchen-car-table tbody');
      if ($tbody.length === 0) {
        window.force && console.error('renderKitchenCarTable: tbody not found');
        return;
      }
    }

    var $template = $tbody.children('tr.fn-tableColumn-loop').first();
    if ($template.length === 0) {
      window.force && console.error('renderKitchenCarTable: template row not found');
      return;
    }

    // テンプレートは非表示のまま保持
    $template.hide();

    // 既存の出力行（テンプレート以外）を削除
    $tbody.find('tr').not($template).remove();

    // DocumentFragment を使ってパフォーマンス向上
    var fragment = document.createDocumentFragment();

    cars.forEach(function (car) {
      var $row = $template.clone().removeClass('fn-tableColumn-loop').show();

      // data-target に基づいて各セルにデータを入れる
      $row.find('[data-target="fn-table-name"]').text(car.name || '');
      $row.find('[data-target="fn-table-status"]').text(car.status || '');

      // チェックボックスに値をセット
      var $checkbox = $row.find('[data-target="fn-table-checkbox"] .fn-pick');
      if ($checkbox.length) {
        $checkbox.val(car.id || '');
      }

      fragment.appendChild($row.get(0));
    });

    $tbody.get(0).appendChild(fragment);
  }

  /* ----------------------------------------
   * 3) フォームから値を集める
   * ---------------------------------------- */
  function collectFormValues() {
    return {
      shop: $('#fn-formShop').length ? String($('#fn-formShop').val() || '') : '',
      dateFrom: $('#fn-formDateFrom').length ? String($('#fn-formDateFrom').val() || '') : '',
      dateTo: $('#fn-formDateTo').length ? String($('#fn-formDateTo').val() || '') : '',
      note: $('#fn-formNote').length ? String($('#fn-formNote').val() || '') : '',
      picked: (function () {
        var arr = [];
        $('.fn-pick:checked').each(function () {
          arr.push($(this).val());
        });
        return arr;
      })()
    };
  }

  /* ----------------------------------------
   * 4) バリデーション
   * ---------------------------------------- */
  function validateForm(values) {
    if (!values.shop) {
      alert('店舗を選択してください');
      return false;
    }
    if (!values.dateFrom || !values.dateTo) {
      alert('開催期間を入力してください');
      return false;
    }
    if (values.picked.length === 0) {
      alert('キッチンカーを1つ以上選択してください');
      return false;
    }
    return true;
  }

  /* ----------------------------------------
   * 5) Pleasanterの入力欄に値をセット
   * ---------------------------------------- */
  function setPleasanterValue(selector, value) {
    var el = document.querySelector(selector);
    if (!el) return false;
    el.value = value;
    el.dispatchEvent(new Event('input', { bubbles: true }));
    el.dispatchEvent(new Event('change', { bubbles: true }));
    return true;
  }

  /* ----------------------------------------
   * 6) 登録ボタンクリック時の処理
   * ---------------------------------------- */
  function handleSubmitButtonClick(evt) {
    if (evt && typeof evt.preventDefault === 'function') evt.preventDefault();

    try {
      var values = collectFormValues();

      if (!validateForm(values)) {
        return;
      }

      // Pleasanter項目にマッピング（セレクタは環境に合わせて調整）
      setPleasanterValue('#ClassA', values.shop);
      setPleasanterValue('#DateA', values.dateFrom);
      setPleasanterValue('#DateB', values.dateTo);
      setPleasanterValue('#TextA', values.picked.join(','));
      setPleasanterValue('#Description', values.note);

      // 保存ボタンをクリック
      var saveBtn = document.querySelector('button[name="Save"], .command-save, #SaveCommand');
      if (!saveBtn) {
        alert('保存ボタンが見つかりません（セレクタ調整が必要）');
        return;
      }
      saveBtn.click();
    } catch (ex) {
      window.force && console.error('handleSubmitButtonClick: unexpected error', ex);
    }
  }

  /* ----------------------------------------
   * 7) キャンセルボタンクリック時の処理
   * ---------------------------------------- */
  function handleCancelButtonClick(evt) {
    if (evt && typeof evt.preventDefault === 'function') evt.preventDefault();
    history.back();
  }

  /* ----------------------------------------
   * 8) イベントバインド（読み込み時に一度だけ）
   * ---------------------------------------- */
  document.addEventListener('DOMContentLoaded', function () {
    try {
      // キッチンカー一覧を描画
      renderKitchenCarTable(kitchenCarData);

      // 登録ボタン
      var $submitBtn = $('#fn-submitButton');
      if ($submitBtn.length) {
        if ($submitBtn.attr('onclick')) {
          $submitBtn.removeAttr('onclick');
        }
        $submitBtn.off('click.kitchencar').on('click.kitchencar', handleSubmitButtonClick);
      }

      // キャンセルボタン
      var $cancelBtn = $('#fn-cancelButton');
      if ($cancelBtn.length) {
        if ($cancelBtn.attr('onclick')) {
          $cancelBtn.removeAttr('onclick');
        }
        $cancelBtn.off('click.kitchencar').on('click.kitchencar', handleCancelButtonClick);
      }
    } catch (e) {
      window.force && console.error('kitchencar: bind error', e);
    }
  });

})(jQuery);
