// 請求書発行　屋外広告物専用
{
    // ルックアップ用sql
    const base_sql = `
        SELECT  UR.見積番号,
                MT.見積件名,得意先名 = MT.得意先名1 + MT.得意先名2, MT.現場名,
                ISNULL(FORMAT(MT.納期S, 'yyyy/MM/dd'), '') + (CASE WHEN MT.納期E IS NULL THEN '' ELSE '~' END) + ISNULL(FORMAT(MT.納期E, 'yyyy/MM/dd'), '') as 納期,
                TM.締日
                ,MT.担当者CD
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
    `;

    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD');
    const category = '請求・売掛';
    const dialogId = "invoiceOutdoorAdvertisingDialog";
    const dialogName = "請求書発行　屋外広告物専用";
    const btnLabel1 = 'PDF出力';
    const btnLabel2 = 'Excel出力';
    createAndAddDialog(dialogId, dialogName,[
        { type: 'text', id: 'estimateNum', label: '見積番号', options: {
            width: 'normal',
            required:true,
            get_lastTime_output:'請求書',
            digitsNum:6,
            varidate:{type:'int',maxlength:6},
            lock:'見積番号',
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号',base_sql:base_sql},
            searchDialog:{id:'estimateNoSearch5',title:'見積番号検索'}
        }},
        { type: 'text', id: 'estimateName', label: '見積件名', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'見積件名',id:'estimateNum'}]
        }},
        { type: 'text', id: 'recipientOfAnOrder', label: '受注先', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'得意先名',id:'estimateNum'}]
        }},
        { type: 'text', id: 'deliveryDay', label: '納期', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'納期',id:'estimateNum'}]
        }},
        { type: 'text', id: 'siteName', label: '現場名', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'現場名',id:'estimateNum'}]
        }},
        { type: 'text-set', id: 'displayManager', label: '表示担当', options: {
            width: 'wide',
            digitsNum:3,
            required:true,
            varidate:{type:'int',maxlength:3},
            searchDialog:{id:'personSearch',title:'担当者検索',multiple:false},
            lookupOrigin:{tableId:'TM担当者',keyColumnName:'担当者CD',forColumnName:'担当者名'},
            lookupFor:[{columnName:'担当者CD',id:'estimateNum'}]
        }},
        { type: 'datepicker', id: 'billingDate', label: '請求日付', options: {
            width: 'normal',
            required:true,
            varidate:{type:'str'},
            lookupFor:[{columnName:'請求日付',id:'estimateNum',func:(record)=>{
                let dt = record.請求日付;
                let deadline = record.締日;
                if(dt == "" || dt == null){
                    billed_flg_for_billing_outdoor = false;
                    H_SEIKYUDATE_for_billing_outdoor = new Date(getLastDateOfScope(new Date(),deadline));
                    return getLastDateOfScope(new Date(),deadline);
                }
                dt = new Date(dt);
                billed_flg_for_billing_outdoor = true;
                H_SEIKYUDATE_for_billing_outdoor = new Date(getLastDateOfScope(dt,deadline));
                return getLastDateOfScope(dt,deadline);
            }}]
        }},
        { type: 'datepicker', id: 'IssueDate', label: '発行日付', options: {
            width: 'normal',
            required:true,
            varidate:{type:'str'},
            lookupFor:[{columnName:'請求書発行日付',id:'estimateNum',base_sql:base_sql,func:(record)=>{
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
        { type: 'button_inline', id: 'create_pdf', label: btnLabel1, options: {
            icon:'disk',
            onclick:`download_invoiceOutdoorAdvertising_report('${category}','${dialogId}','${dialogName}','${btnLabel1}','pdf')`
        }},
        { type: 'button_inline', id: 'creatt_excel', label: btnLabel2, options: {
            icon:'disk',
            onclick:`download_invoiceOutdoorAdvertising_report('${category}','${dialogId}','${dialogName}','${btnLabel2}','xlsx')`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)});
}

// ----------------------------------------------------------------------
// ↓ ロック処理
$(document).on('focus','#invoiceOutdoorAdvertisingDialog_estimateNum',async function(){
    UnLockData('見積番号',$(this).val());
    $('#invoiceOutdoorAdvertisingDialog input').val('');
    $(this).closest('.dialog').find('.input-container span').text('');
})

$(document).on('blur','#invoiceOutdoorAdvertisingDialog_estimateName',async function(){
    const estimate_name = $('#invoiceOutdoorAdvertisingDialog_estimateName');
    if(estimate_name.val() == "")return;
    if(await LockData('見積番号',$('#invoiceOutdoorAdvertisingDialog_estimateNum').val()) == false){
        $('#invoiceOutdoorAdvertisingDialog input').val('');
        $('#invoiceOutdoorAdvertisingDialog_estimateNum').focus();
    }
})
// ↑ ロック処理
// ----------------------------------------------------------------------

// 請求日付のバリデーション
$(document).on('blur','#invoiceOutdoorAdvertisingDialog_billingDate',async function(){
    let dt = $(this).val();
    billing_date_varidate_for_billingOutdoor();
})

let billed_flg_for_billing_outdoor = false;
let H_SEIKYUDATE_for_billing_outdoor = "";
let gGetuDate_for_billing_outdoor = "";
let flg_for_billing_outdoor = false;
async function billing_date_varidate_for_billingOutdoor(){
    try{
        if(flg_for_billing_outdoor)return false;
        flg_for_billing_outdoor=true;

        let dt = new Date($('#invoiceOutdoorAdvertisingDialog_billingDate').val());
        let dt_element = $('#invoiceOutdoorAdvertisingDialog_billingDate');

        if(H_SEIKYUDATE_for_billing_outdoor == ""){
            flg_for_billing_outdoor = false;
            return false;
        }

        if(H_SEIKYUDATE_for_billing_outdoor.getTime() == dt.getTime() || $('#invoiceOutdoorAdvertisingDialog_estimateNum').val() == ""){
            flg_for_billing_outdoor = false;
            return true;
        }

        if(gGetuDate_for_billing_outdoor == ""){
            dates = await fetchSql(`SELECT * FROM TMDates WHERE DateID = '売掛月次更新日'`);
            if(dates.results.length != 0){
                gGetuDate_for_billing_outdoor = new Date(dates.results[0].更新日付);
            }
        }
        if(H_SEIKYUDATE_for_billing_outdoor.getTime() != dt.getTime()) {
            if(dt <= gGetuDate_for_billing_outdoor || H_SEIKYUDATE_for_billing_outdoor <= gGetuDate_for_billing_outdoor){
                alert('更新済みのため、修正できません。');
                dt_element.val(H_SEIKYUDATE_for_billing_outdoor.toLocaleDateString('sv-SE'));
                await sleep(0);
                flg_for_billing_outdoor = false;
                $(this).focus();
                return false;
            }
        }

        if(billed_flg_for_billing_outdoor){
            alert('請求日付を変更した場合、請求金額に影響を及ぼすことがあります。\n確認をお願いします。');
            await sleep(0);
        }
        H_SEIKYUDATE_for_billing_outdoor = dt;
        flg_for_billing_outdoor = false;
        return true;
    }
    catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error('請求書発行　屋外広告物専用',err);
        flg_for_billing_outdoor = false;
    }

}

async function download_invoiceOutdoorAdvertising_report(category,dialogId,dialogName,btnLabel,type){
    try{
        if(!validateDialog(dialogId)){
            return;
        }

        const estimateNo = $('#invoiceOutdoorAdvertisingDialog_estimateNum');
        const estimateName = $('#invoiceOutdoorAdvertisingDialog_estimateName');
        const person = $('#invoiceOutdoorAdvertisingDialog_displayManagerFrom');
        const billing_date = $('#invoiceOutdoorAdvertisingDialog_billingDate');
        const publish_date = $('#invoiceOutdoorAdvertisingDialog_IssueDate');

        // バリデーション
        if(estimateNo.val() == ""){
            alert('見積番号が未入力です。');
            return;
        }

        if(person.val() == ""){
            alert('担当者CDが未入力で未入力です。');
            return;
        }

        if(billing_date.val() == ""){
            alert('請求日付が未入力です。');
            return;
        }

        // if(!await billing_date_varidate_for_billingOutdoor()){
        //     return;
        // }

        if(publish_date.val() == ""){
            alert('請求発行日が未入力です。');
            return;
        }
 
        const fileName = `請求書-${estimateNo.val()}${estimateName.val()}${new Date().toLocaleDateString('sv-SE').replaceAll('-','').slice(-6)}.${type}`

        const item_params = {
            "@i見積番号":estimateNo.val(),
            "@i担当者CD":person.val(),
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
            await download_report(param,fileName);
        }catch{
            return;
        }

    }catch(err){
        console.error(err);
        alert('予期せぬエラーが発生しました。');
    }
}
