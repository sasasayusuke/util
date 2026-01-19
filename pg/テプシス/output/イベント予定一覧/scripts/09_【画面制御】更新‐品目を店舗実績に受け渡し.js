$(function () {

  // ★ 先に mousedown で止める
  $(document).on(
    'mousedown',
    achievement_table_id + ' tbody tr.grid-row',
    function (e) {

      // ボタン・リンクは除外
      if ($(e.target).closest('button, a').length) return;

      const shopName = $(this).find('td').eq(1).text().trim();
      const data = getShopItemsByShopName(shopName);

      if (data.length === 0) {
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();

        alert(`${shopName}店の「品目」を登録してください`);
        return false;
      }
    }
  );

  // ===== 通常クリック時の処理（遷移させたい場合だけ）=====
  $(document).on(
    'click',
    achievement_table_id  + ' tbody tr.grid-row',
    function (e) {

      if ($(e.target).closest('button, a').length) return;

      const shopName = $(this).find('td').eq(1).text().trim();
      const data = getShopItemsByShopName(shopName);

      // data がある時だけ処理続行（＝遷移OK）
      if (data.length === 0) return;

      const itemId = $(this).find('td').eq(0).text().trim();
      const param = encodeURIComponent(JSON.stringify(data));

      location.href =
        `/items/${itemId}?back=1&FromTabIndex=0&SelectedShops=${param}`;
    }
  );

});


function getShopItemsByShopName(shopName) {

  const result = [];
  const $table  = $(items_table_id  + 'Field');
  const headers = $table.find('thead th');

  $table.find('tbody tr.grid-row').each(function () {

    const $row = $(this);
    const rowShopName = $row.find('td').eq(0).text().trim();

    if (rowShopName !== shopName) return;

    const rowData = {
      shop: rowShopName,
      items: []
    };

    $row.find('td').each(function (index) {

      if (index === 0) return;

      if ($(this).find('.ui-icon-circle-check').length > 0) {
        const itemName = headers.eq(index).text().trim();
        rowData.items.push(itemName);
      }

    });

    if (rowData.items.length > 0) {
      result.push(rowData);
    }

  });

  return result;
}
