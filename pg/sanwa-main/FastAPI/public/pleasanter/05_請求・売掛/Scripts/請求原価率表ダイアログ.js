// 請求原価率表
{
    const category = "請求・売掛";
    const dialogId = "invoiceCost";
    const dialogName = "請求原価率表";
    const btnLabel = "出力";
    const today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD');

    createAndAddDialog(dialogId, dialogName,[
        { type: 'text', id: 'custmerNo', label: '得意先CD',forColumnName:'MH.得意先CD', options: {
            search:true,
            searchDialog:{id:'custmerNoSearch',title:'得意先検索'}
        }},
        { type: 'text', id: 'DeliveryDestinationNo', label: '納入先CD',forColumnName:'MH.納入先CD', options: {
            width: 'normal',
            searchDialog:{id:'recipientNoSearch',title:'納入先検索',multiple:false}
        }},
        { type: 'text', id: 'IndustryClassification', label: '業種区分',forColumnName:'MH.業種区分', options: {
        }},
        { type: 'text', id: 'propertyType', label: '物件種別',forColumnName:'MH.物件種別', options: {
        }},
        { type: 'text', id: 'Quotationsubject', label: '見積件名', forColumnName:'MH.見積件名',options: {
            width :'wide'
        }},
        { type: 'range-text', id: 'propertyNoRange', label: '物件番号範囲',forColumnName:'MH.物件番号', options: {
            width :'normal',
            search:true,
            searchDialog:{id:'propertyDataSearch',title:'物件検索',multiple:false}
        }},
        { type: 'range-date', id: 'billingDateRange', label: '請求日付範囲',forColumnName:'SU.請求日付', options: {
            required:true,
            varidate:{type:'str'},
            valueFrom:today,
            valueTo:today,
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            multiple:true,
            individualSql:true,
            searchTableId:'TD見積',
            t_heads:[
                {label:'見積番号',ColumnName:'見積番号'},
                {label:'見積日付',ColumnName:'見積日付'},
                {label:'種別',ColumnName:'物件種別',specialTerms:`CASE 物件種別 WHEN 0 THEN '新店' WHEN 1 THEN '改装' WHEN 2 THEN 'メンテ' WHEN 3 THEN '委託' END as 物件種別`},
                {label:'得意先',ColumnName:'得意先CD'},
                {label:'納入先',ColumnName:'納入先CD'},
                {label:'見積件名',ColumnName:'見積件名'},
                {label:'物件番号',ColumnName:'物件番号'},
                {label:'見積金額',ColumnName:'合計金額',specialTerms:'合計金額 + 出精値引 as 合計金額',alignment:'end'},
            ]
        }},

        
    ],
    [
        { type: 'button_inline', id: 'create', label: btnLabel, options: {
            icon:'disk',
            onclick:`invoiceCostDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,'750')})
}

/**
 *
 * @param {string} dialogId     ダイアログID
 * @param {string} category     カテゴリー
 * @param {string} dialogName   ダイアログ名
 * @param {string} btnLabel     ボタンラベル
 * @returns
 */
async function invoiceCostDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 請求原価率表');

        let items = Array.from($('#invoiceCost tbody tr input[type="checkbox"]:checked'));
        let estimateNos = items.map(e => $(e).parent().next().text()).join(',');

        let item_params = {
            "@is請求日付":SpcToNull($('#invoiceCost_billingDateRangeFrom').val().replaceAll('-','/')),
            "@ie請求日付":SpcToNull($('#invoiceCost_billingDateRangeTo').val().replaceAll('-','/')),
            "@i見積番号配列":estimateNos
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "請求原価率表作成" + SpcToNull($('#invoiceCost_billingDateRangeFrom').val().replaceAll('-','')).slice(2,8) + "-" + SpcToNull($('#invoiceCost_billingDateRangeTo').val().replaceAll('-','')).slice(2,8) + ".xlsx"

        let assept = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';

        try{
            await download_report(param,filename,assept);
        }
        catch(err){
            return;
        }

        console.log('end : 請求原価率表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }
}


// 納入先CD検索ダイアログを開く際に得意先情報を検索ダイアログに登録
$(document).on('click','#invoiceCost_DeliveryDestinationNoField .ui-icon-search',async function(e){
    let custmerCD = $('#invoiceCost_custmerNo').val();

    const cls_custmer = new ClsCustmer(custmerCD);
    const res = await cls_custmer.GetByData();

    if(!res){
        alert('得意先CDを入力して下さい');
        $('#invoiceCost_custmerNo').focus();
        return;
    }

    openDialog_for_searchDialog('recipientNoSearch','invoiceCost_DeliveryDestinationNo','750px','納入先検索');
    $('#recipientNoSearch_custmerFrom').val(cls_custmer["custmerCD"]);
    $('#recipientNoSearch_custmerTo').val(cls_custmer["得意先名1"]);
})


const invoiceCost_baseSQL = (e) =>{
    return `
        SELECT TOP 1000
            MH.見積番号,MH.見積日付,
            CASE MH.物件種別 WHEN 0 THEN '新店' WHEN 1 THEN '改装' WHEN 2 THEN 'メンテ' WHEN 3 THEN '委託' END as 物件種別,
            MH.得意先CD,MH.納入先CD,MH.見積件名,MH.物件番号,MH.合計金額 + MH.出精値引 as 合計金額
        FROM TD見積 as MH
        INNER JOIN TD売上請求H as SU
            ON MH.見積番号 = SU.見積番号
        ${e}
        ORDER BY MH.見積日付 desc, MH.見積番号 desc
    `;
}