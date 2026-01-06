// =====================================
// 開催エリア：福島県内 → 開催エリア詳細 必須制御
// =====================================

checkClassF();

function checkClassF() {

  // change は委譲
  $(document)
    .off('change', '#Results_ClassF')
    .on('change', '#Results_ClassF', toggleRequired);

  // DOM再描画監視（更新後対応）
  observeEditDom();

  // 初期
  toggleRequired();
}

function toggleRequired() {

  const $area = $('#Results_ClassF');
  const $desc = $('#Results_DescriptionB');
  const $label = $('label[for="Results_DescriptionB"]');

  // DOM未生成なら何もしない
  if ($area.length === 0 || $desc.length === 0) return;

  if ($area.val() === '2') {

    // 必須
    $desc
      .attr('data-validate-required', '1')
      .addClass('required');

    if ($label.find('.required-mark').length === 0) {
      $label.append('<span class="required-mark"> *</span>');
    }

  } else {

    $desc
      .removeAttr('data-validate-required')
      .removeClass('required');

    $label.find('.required-mark').remove();
  }
}

function observeEditDom() {

  const observer = new MutationObserver(function () {
    toggleRequired();
  });

  observer.observe(document.body, {
    childList: true,
    subtree: true
  });
}
