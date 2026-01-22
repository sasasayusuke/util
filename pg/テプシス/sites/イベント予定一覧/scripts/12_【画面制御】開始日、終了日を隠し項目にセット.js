(function ($) {
  'use strict';

  var $rows = $(startEnd_table_id +' tbody tr.grid-row');
  let $id = $('#Results_ResultId').text();
  if ($rows.length === 0) return;

  var minStartDate = null;
  var maxEndDate   = null;
  var minStartStr  = '';
  var maxEndStr    = '';

  $rows.each(function () {
    var $tds = $(this).find('td');

    var startText = $tds.eq(1).text().trim(); // 開始日
    var endText   = $tds.eq(2).text().trim(); // 終了日

    // 「2025/12/27 土」→「2025/12/27」
    var startDateStr = startText.split(' ')[0];
    var endDateStr   = endText.split(' ')[0];

    var startDate = new Date(startDateStr.replace(/\//g, '-'));
    var endDate   = new Date(endDateStr.replace(/\//g, '-'));

    // ★ 開始日：一番古いもの
    if (!minStartDate || startDate < minStartDate) {
      minStartDate = startDate;
      minStartStr  = startDateStr;
    }

    // ★ 終了日：一番新しいもの
    if (!maxEndDate || endDate > maxEndDate) {
      maxEndDate = endDate;
      maxEndStr  = endDateStr;
    }
  });

  if (!minStartDate || !maxEndDate) return;

  // 値セット（画面 & 保存値）
  $('#Results_DateA').val(minStartStr);
  $('#Results_DateB').val(maxEndStr);

  $p.set($('#Results_DateA'), minStartStr);
  $p.set($('#Results_DateB'), maxEndStr);

  // 画面離脱時に保存
  window.addEventListener('beforeunload', function () {
    $p.apiUpdate({
      id: $id,
      data: {
        DateHash: {
          DateA: minStartStr,
          DateB: maxEndStr
        }
      }
    });
  });

})(jQuery);
