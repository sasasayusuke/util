(function ($) {
  'use strict';

insertSummaryTable();

function insertSummaryTable() {
    let DataList = getStoreRecors();
    console.log("DataList");
    console.log(DataList);


  // 二重生成防止
  if ($('.summary-table-wrap').length > 0) {
    return;
  }

  const html = `
    <div class="Sdt-table-wrap">
      <table class="Sdt-summary-table">
        <tr class="Sdt-title-top">
          <th  colspan="3">全店舗‐実績</th>
        </tr>
        <tr class="Sdt-text-normal">
          <th>米</th>
          <th>桃</th>
          <th>牛肉</th>
        </tr>
        <tr class="Sdt-text-normal">
          <td>総取扱量 <span class="num rice-qty">${DataList.totalNumA}</span> kg</td>
          <td>総取扱量 <span class="num peach-qty">${DataList.totalNumC}</span> kg</td>
          <td>総取扱量 <span class="num beef-qty">${DataList.totalNumB}</span> kg</td>
        </tr>
        <tr class="Sdt-text-normal Sdt-sales-row">
          <td>総売上 <span class="num rice-sales">${DataList.totalNumD}</span> 円</td>
          <td>総売上 <span class="num peach-sales">${DataList.totalNumE}</span> 円</td>
          <td>総売上 <span class="num beef-sales">${DataList.totalNumF}</span> 円</td>
        </tr>
        <tr class="Sdt-text-normal">
          <th>水産品</th>
          <th>ホタテ</th>
          <th>その他禁輸対象品</th>
        </tr>
        <tr class="Sdt-text-normal">
          <td>総取扱量 <span class="num sea-qty">${DataList.totalNumG}</span> kg</td>
          <td>総取扱量 <span class="num scallops-qty">${DataList.totalNumH}</span> kg</td>
          <td>総取扱量 <span class="num embargo-qty">${DataList.totalNumI}</span> kg</td>
        </tr>
        <tr class="Sdt-text-normal Sdt-sales-row">
          <td>総売上 <span class="num sea-sales">${DataList.totalNumJ}</span> 円</td>
          <td>総売上 <span class="num scallops-sales">${DataList.totalNumK}</span> 円</td>
          <td>総売上 <span class="num embargo-sales">${DataList.totalNumL}</span> 円</td>
        </tr>
        <tr class="Sdt-text-normal">
          <th>その他</th>
          <th>メニュー/品物</th>
        </tr>
        <tr class="Sdt-text-normal">
          <td>総取扱量 <span class="num other-qty">${DataList.totalNumM}</span> kg</td>
          <td>食数/点数（合計） <span class="num menu-qty">${DataList.totalNumN}</span> 食/点</td>
        </tr>
        <tr class="Sdt-text-normal Sdt-sales-row">
          <td>総売上 <span class="num other-sales">${DataList.totalNumO}</span> 円</td>
          <td>売上（合計） <span class="num menu-sales">${DataList.totalNumP}</span> 円</td>
        </tr>
        <tr class="Sdt-text-normal">
          <th>合計</th>
        </tr>
        <tr class="Sdt-text-normal">
          <td>総取扱量 <span class="num all-qty">${DataList.totalNumQ}</span> kg</td>
        </tr>
        <tr class="Sdt-text-normal Sdt-sales-row">
          <td>総売上 <span class="num all-sales">${DataList.totalNumR}</span> 円</td>
        </tr>
      </table>
    </div>
  `;

  // ▼ ここがポイント
  $(`${achievement_table_id }Field`).after(html);
}

// =====================================
// レコード取得
// =====================================

function getStoreRecors() {
  let DataList = [];

    $(`${achievement_table_id}  tbody tr.grid-row`).each(function () {
        const $tds = $(this).find('td');
        DataList.push({

            NumA: $tds.eq(2).text().trim(), 
            NumD: $tds.eq(3).text().trim(), 
            NumC: $tds.eq(4).text().trim(), 
            NumE: $tds.eq(5).text().trim(), 
            NumB: $tds.eq(6).text().trim(), 
            NumF: $tds.eq(7).text().trim(), 
            NumG: $tds.eq(8).text().trim(), 
            NumJ: $tds.eq(9).text().trim(), 
            NumH: $tds.eq(10).text().trim(), 
            NumK: $tds.eq(11).text().trim(), 
            NumI: $tds.eq(12).text().trim(), 
            NumL: $tds.eq(13).text().trim(), 
            NumM: $tds.eq(14).text().trim(), 
            NumO: $tds.eq(15).text().trim(), 
            NumN: $tds.eq(16).text().trim(), 
            NumP: $tds.eq(17).text().trim(), 
            NumQ: $tds.eq(18).text().trim(), 
            NumR: $tds.eq(19).text().trim()
        });

    });

  DataList = calculateTotal(DataList);
  return DataList;
}

// =====================================
// 共通：数値文字列 → 数値変換
// 例："20kg" / "10円" → 20
// =====================================
function parseNumber(value) {
  if (!value) return 0;

  const num = parseFloat(
    value.replace(/[^0-9.-]/g, '')
  );

  return isNaN(num) ? 0 : num;
}

// =====================================
// 共通：NumA / NumD 合計計算
// =====================================
function calculateTotal(rowDataList) {
  let totalNumA = 0;
  let totalNumD = 0;
  let totalNumC = 0;
  let totalNumE = 0;
  let totalNumB = 0;
  let totalNumF = 0;
  let totalNumG = 0;
  let totalNumJ = 0;
  let totalNumH = 0;
  let totalNumK = 0;
  let totalNumI = 0;
  let totalNumL = 0;
  let totalNumM = 0;
  let totalNumO = 0;
  let totalNumN = 0;
  let totalNumP = 0;
  let totalNumQ = 0;
  let totalNumR = 0;



  rowDataList.forEach(function (row) {
    totalNumA += parseNumber(row.NumA);
    totalNumD += parseNumber(row.NumD);
    totalNumC += parseNumber(row.NumC);
    totalNumE += parseNumber(row.NumE);
    totalNumB += parseNumber(row.NumB);
    totalNumF += parseNumber(row.NumF);
    totalNumG += parseNumber(row.NumG);
    totalNumJ += parseNumber(row.NumJ);
    totalNumH += parseNumber(row.NumH);
    totalNumK += parseNumber(row.NumK);
    totalNumI += parseNumber(row.NumI);
    totalNumL += parseNumber(row.NumL);
    totalNumM += parseNumber(row.NumM);
    totalNumO += parseNumber(row.NumO);
    totalNumN += parseNumber(row.NumN);
    totalNumP += parseNumber(row.NumP);
    totalNumQ += parseNumber(row.NumQ);
    totalNumR += parseNumber(row.NumR);
  });

  return {
    totalNumA,
    totalNumD,
    totalNumA,
    totalNumD,
    totalNumC,
    totalNumE,
    totalNumB,
    totalNumF,
    totalNumG,
    totalNumJ,
    totalNumH,
    totalNumK,
    totalNumI,
    totalNumL,
    totalNumM,
    totalNumO,
    totalNumN,
    totalNumP,
    totalNumQ,
    totalNumR
  };
}

})(jQuery);