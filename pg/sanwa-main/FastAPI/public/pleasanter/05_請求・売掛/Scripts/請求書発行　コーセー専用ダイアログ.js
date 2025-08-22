// 請求書発行　コーセー専用
{
    // ルックアップ用SQL
    const base_sql = `
        SELECT  UR.見積番号, MT.見積件名,得意先名 = MT.得意先名1 + MT.得意先名2 , MT.現場名,
        ISNULL(FORMAT(MT.納期S, 'yyyy/MM/dd'), '') + (CASE WHEN MT.納期E IS NULL THEN '' ELSE '~' END) + ISNULL(FORMAT(納期E, 'yyyy/MM/dd'), '') as 納期,
        TM.締日 ,MT.担当者CD
            ,USH.請求日付,USH.請求書発行日付
        FROM (SELECT    見積番号
                    From TD売上v2
                    GROUP BY 見積番号
            ) AS UR
            LEFT JOIN TD見積 AS MT
                ON UR.見積番号 = MT.見積番号
            LEFT JOIN TD売上請求H AS USH
                ON UR.見積番号 = USH.見積番号
            LEFT JOIN TM得意先 AS TM
                ON MT.得意先CD = TM.得意先CD
        WHERE UR.見積番号 =
    `

    const dialogId = "KOSEInvoicingDialog";
    const dialogName = "請求書発行　コーセー専用";
    const category = "請求・売掛";
    const btnLabel1 = "Excel出力";
    const btnLabel2 = "PDF出力";


    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'headerquotation', label: '見出し見積', options: {
            width: 'normal',
            required:true,
            digitsNum:6,
            lock:'見積番号',
            get_lastTime_output:'請求書',
            varidate:{type:'int',maxlength:6},
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号',base_sql:base_sql},
            searchDialog:{id:'estimateNoSearch5',title:'見積番号検索',}
        }},
        { type: 'text', id: 'estimateName', label: '見積件名', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'見積件名',id:'headerquotation'}]
        }},
        { type: 'text', id: 'recipientOfAnOrder', label: '受注先', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'得意先名',id:'headerquotation'}]
        }},
        { type: 'text', id: 'deliveryDay', label: '納期', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'納期',id:'headerquotation'}]
        }},
        { type: 'text', id: 'siteName', label: '現場名', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'現場名',id:'headerquotation'}]
        }},
        { type: 'text', id: 'itemizedQuotation', label: '明細見積', options: {
            width: 'normal',
            required:true,
            varidate:{type:'int',maxlength:6},
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号'},
            digitsNum:6,
            searchDialog:{id:'estimateNoSearch5',title:'見積番号検索',}
        }},
        { type: 'text', id: 'estimateName2', label: '見積件名', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'見積件名',id:'itemizedQuotation'}]
        }},
        { type: 'text', id: 'Recipient2', label: '受注先', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'得意先名',specialTerms:'得意先名 = 得意先名1 + 得意先名2',id:'itemizedQuotation'}]
        }},
        { type: 'text-set', id: 'displayManager', label: '表示担当', options: {
            width: 'wide',
            digitsNum:3,
            required:true,
            varidate:{type:'int',maxlength:3},
            searchDialog:{id:'personSearch',title:'担当者検索',multiple:false},
            lookupOrigin:{tableId:'TM担当者',keyColumnName:'担当者CD',forColumnName:'担当者名'},
            lookupFor:[{columnName:'担当者CD',id:'headerquotation'}]
        }},
        { type: 'datepicker', id: 'billingDate', label: '請求日付', options: {
            width: 'normal',
            required:true,
            varidate:{type:'str'},
            lookupFor:[{columnName:'請求日付',id:'headerquotation',func:(record)=>{
                let dt = record.請求日付;
                let deadline = record.締日;
                if(dt == "" || dt == null){
                    billed_flg_for_billing_KOSE = false;
                    H_SEIKYUDATE_for_billing_KOSE = new Date(getLastDateOfScope(new Date(),deadline));
                    return getLastDateOfScope(new Date(),deadline);
                }
                dt = new Date(dt);
                billed_flg_for_billing_KOSE = true;
                H_SEIKYUDATE_for_billing_KOSE = new Date(getLastDateOfScope(dt,deadline));
                return getLastDateOfScope(dt,deadline);
            }}]
        }},
        { type: 'datepicker', id: 'IssueDate', label: '発行日付', options: {
            width: 'normal',
            required:true,
            varidate:{type:'str'},
            lookupFor:[{columnName:'請求書発行日付',id:'headerquotation',func:(record)=>{
                let dt = record.請求書発行日付;
                let deadline = record.締日;
                if(dt == "" || dt == null){
                    return getLastDateOfScope(new Date(),deadline);
                }
                dt = new Date(dt);
                return getLastDateOfScope(dt,deadline);
            }}]
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
        { type: 'borderText', id: 'test2', label: '前回出力情報', options: {
        }},
    ],
    [
        { type: 'button_inline', id: 'output1', label: btnLabel1, options: {
            icon:'disk',
            onclick:`create_KOSEInvoicing_report('${category}','${dialogId}','${dialogName}','${btnLabel1}','xlsx')`
        }},
        { type: 'button_inline', id: 'output2', label: btnLabel2, options: {
            icon:'disk',
            onclick:`create_KOSEInvoicing_report('${category}','${dialogId}','${dialogName}','${btnLabel2}','pdf')`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}

// ----------------------------------------------------------------------
// ↓ ロック処理
$(document).on('focus','#KOSEInvoicingDialog_headerquotation',async function(){
    UnLockData('見積番号',$(this).val());
    $('#KOSEInvoicingDialog input').val('');
    $(this).closest('.dialog').find('.input-container span').text('');
})

$(document).on('blur','#KOSEInvoicingDialog_estimateName',async function(){

    const estimate_name = $('#KOSEInvoicingDialog_estimateName');
    if(estimate_name.val() == "")return;
    if(await LockData('見積番号',$('#KOSEInvoicingDialog_headerquotation').val()) == false){
        $('#KOSEInvoicingDialog input').val('');
        $('#KOSEInvoicingDialog_headerquotation').focus();
    }
})
// ↑ ロック処理
// ----------------------------------------------------------------------

// 請求日付のバリデーション
$(document).on('blur','#KOSEInvoicingDialog_billingDate',async function(){
    let dt = $(this).val();
    billing_date_varidate_for_billingKOSE();
})

let billed_flg_for_billing_KOSE = false;
let H_SEIKYUDATE_for_billing_KOSE = "";
let gGetuDate_for_billing_KOSE = "";
let flg_for_billing_KOSE = false;
async function billing_date_varidate_for_billingKOSE(){
    try{
        if(flg_for_billing_KOSE)return false;
        flg_for_billing_KOSE=true;

        let dt = new Date($('#KOSEInvoicingDialog_billingDate').val());
        let dt_element = $('#KOSEInvoicingDialog_billingDate');

        if(H_SEIKYUDATE_for_billing_KOSE == ""){
            flg_for_billing_KOSE = false;
            return false;
        }

        if(H_SEIKYUDATE_for_billing_KOSE.getTime() == dt.getTime() || $('#KOSEInvoicingDialog_headerquotation').val() == ""){
            flg_for_billing_KOSE = false;
            return true;
        }

        if(gGetuDate_for_billing_KOSE == ""){
            dates = await fetchSql(`SELECT * FROM TMDates WHERE DateID = '売掛月次更新日'`);
            if(dates.results.length != 0){
                gGetuDate_for_billing_KOSE = new Date(dates.results[0].更新日付);
            }
        }

        if(H_SEIKYUDATE_for_billing_KOSE.getTime() != dt.getTime()) {
            if(dt <= gGetuDate_for_billing_KOSE || H_SEIKYUDATE_for_billing_KOSE <= gGetuDate_for_billing_KOSE){
                alert('更新済みのため、修正できません。');
                dt_element.val(H_SEIKYUDATE_for_billing_KOSE.toLocaleDateString('sv-SE'));
                await sleep(0);
                flg_for_billing_KOSE = false;
                $(this).focus();
                return false;
            }
        }

        if(billed_flg_for_billing_KOSE){
            alert('請求日付を変更した場合、請求金額に影響を及ぼすことがあります。\n確認をお願いします。');
            await sleep(0);
        }
        H_SEIKYUDATE_for_billing_KOSE = dt;
        flg_for_billing_KOSE = false;
        return true;
    }
    catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error('請求書発行　コーセー専用',err);
        flg_for_billing_KOSE = false;
    }

}

async function create_KOSEInvoicing_report(category,dialogId,dialogName,btnLabel,type){
    try{
        if(!validateDialog(dialogId)){
            return;
        }

        const estimate_no1  = $('#KOSEInvoicingDialog_headerquotation');
        const estimate_name = $('#KOSEInvoicingDialog_estimateName');
        const estimate_no2  = $('#KOSEInvoicingDialog_itemizedQuotation');
        const person        = $('#KOSEInvoicingDialog_displayManagerFrom');
        const billing_date  = $('#KOSEInvoicingDialog_billingDate');
        const publish_date = $('#KOSEInvoicingDialog_IssueDate');



        let filename = `請求書-${estimate_no1.val()}${estimate_name.val()}${new Date().toLocaleDateString('sv-SE').replaceAll('-','').slice(-6)}.${type}`

        const item_params = {
            "@i見積番号":estimate_no1.val(),
            "@i見積明細番号":estimate_no2.val(),
            "@i表示担当":person.val(),
            "@i請求日付":billing_date.val(),
            "@i請求書発行日付":publish_date.val(),
            "@type":type,
            "@pc_name":$("#LoginId").val()
        }

        const param = {
            "category": category,
            "title": dialogName,
            "button": '出力',
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

    }catch(err){
        console.error(err);
        alert('予期せぬエラーが発生しました。');
    }
}