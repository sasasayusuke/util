$(function () {

  $(document).on(
    'click',
    '#Results_Source253630 tbody tr.grid-row',
    function (e) {

      if ($(e.target).closest('button, a').length) return;

      // ===== ① クリックした行の0列目（ID）を取得 =====
      const itemId = $(this).find('td').eq(0).text().trim();

      if (!itemId) {
        alert('レコードIDが取得できません');
        return;
      }

      // ===== ② 店舗名取得 =====
      const shopName = $(this).find('td').eq(1).text().trim();

      const data = getShopItemsByShopName(shopName);

      if (data.length === 0) {
        alert('この店舗にはチェックされた品目がありません');
        return;
      }

      // ===== ③ パラメータ生成 =====
      const param = encodeURIComponent(JSON.stringify(data));

      // ===== ④ URL生成（★要件どおり） =====
      const url =
        `/items/${itemId}`
        + `?back=1`
        + `&FromTabIndex=0`
        + `&SelectedShops=${param}`;

      location.href = url;
    }
  );

});

function getShopItemsByShopName(shopName) {

  const result = [];
  const $table  = $('#Results_Source253149Field');
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
