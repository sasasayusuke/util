// 請求書出力
{
    // ルックアップ用SQL
    const base_sql = `
        SELECT TOP 1000 UR.見積番号, MT.見積件名,得意先名 = MT.得意先名1 + MT.得意先名2 , MT.現場名,
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
    `;
    const category = "請求・売掛";
    const dialogId = "billDialog";
    const dialogName = "請求書発行";
    const btn_label1 = "Excel出力";
    const btn_label2 = "PDF出力";
    createAndAddDialog(dialogId, dialogName,[
        { type: 'text', id: 'estimateNo', label: '見積番号', options: {
            width: 'normal',
            required:true,
            varidate:{type:'int',maxlength:6},
            digitsNum:6,
            lock:'見積番号',
            get_lastTime_output:'請求書',
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号',base_sql:base_sql},
            searchDialog:{id:'estimateNoSearch5',title:'見積番号検索',multiple:false}
        }},
        { type: 'text', id: 'estimateName', label: '見積件名', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'見積件名',id:'estimateNo'}]
        }},
        { type: 'text', id: 'recipientOfAnOrder', label: '受注先', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'得意先名',specialTerms:'得意先名 = 得意先名1 + 得意先名2',id:'estimateNo'}]
        }},
        { type: 'text', id: 'deliveryDay', label: '納期', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'納期',specialTerms:`ISNULL(FORMAT(納期S, 'yyyy/MM/dd'), '') + (CASE WHEN 納期E IS NULL THEN '' ELSE '~' END) + ISNULL(FORMAT(納期E, 'yyyy/MM/dd'), '') as 納期`,id:'estimateNo'}]
        }},
        { type: 'text', id: 'siteName', label: '現場名', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'現場名',id:'estimateNo'}]
        }},
        { type: 'text-set', id: 'displayManager', label: '表示担当', options: {
            width: 'wide',
            digitsNum:3,
            required:true,
            varidate:{type:'int',maxlength:3},
            search:true,
            searchDialog:{id:'personSearch',title:'担当者検索',multiple:false},
            lookupOrigin:{tableId:'TM担当者',keyColumnName:'担当者CD',forColumnName:'担当者名'},
            lookupFor:[{columnName:'担当者CD',id:'estimateNo'}]
        }},
        { type: 'datepicker', id: 'requestDate', label: '請求日付', options: {
            width: 'normal',
            required:true,
            varidate:{type:'str'},
            lookupFor:[{columnName:'請求日付',id:'estimateNo',func:(record)=>{
                let dt = record.請求日付;
                let deadline = record.締日;
                if(dt == "" || dt == null){
                    billed_flg_for_billing = false;
                    H_SEIKYUDATE_for_billing = new Date(getLastDateOfScope(new Date(),deadline));
                    return getLastDateOfScope(new Date(),deadline);
                }
                dt = new Date(dt);
                billed_flg_for_billing = true;
                H_SEIKYUDATE_for_billing = new Date(getLastDateOfScope(dt,deadline));
                return getLastDateOfScope(dt,deadline);
            }}]
        }},
        { type: 'datepicker', id: 'publishingDate', label: '発行日付', options: {
            width: 'normal',
            required:true,
            varidate:{type:'str'},
            lookupFor:[{columnName:'請求書発行日付',id:'estimateNo',func:(record)=>{
                let dt = record.請求書発行日付;
                let deadline = record.締日;
                if(dt == "" || dt == null){
                    return getLastDateOfScope(new Date(),deadline);
                }
                dt = new Date(dt);
                return getLastDateOfScope(dt,deadline);
            }}]
        }},
        { type: 'borderText', id: 'test2', label: '前回出力情報', options: {
        }},
    ],
    [
        { type: 'button_inline', id: 'delete', label: '削除', options: {
            icon:'disk',
            onclick:`delete_billDialog('${category}','${dialogId}','${dialogName}')`
        }},
        { type: 'button_inline', id: 'output1', label: btn_label1, options: {
            icon:'disk',
            onclick:`download_bill_report('${category}','${dialogId}','${dialogName}','${btn_label1}','xlsx')`
        }},
        { type: 'button_inline', id: 'output2', label: btn_label2, options: {
            icon:'disk',
            onclick:`download_bill_report('${category}','${dialogId}','${dialogName}','${btn_label2}','pdf')`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)});

}

// ----------------------------------------------------------------------
// ↓ ロック処理
$(document).on('focus','#billDialog_estimateNo',async function(){
    UnLockData('見積番号',$(this).val());
    $('#billDialog input').val('');
    $(this).closest('.dialog').find('.input-container span').text('');
})

// 見積件名に値が入ったらロックをかける
$(document).on('blur','#billDialog_estimateName',async function(){
    const estimate_name = $('#billDialog_estimateName');
    if(estimate_name.val() == "")return;
    if(await LockData('見積番号',$('#billDialog_estimateNo').val()) == false){
        $('#billDialog input').val('');
        $('#billDialog_estimateNo').focus();
    }
})
// ↑ ロック処理
// ----------------------------------------------------------------------


$(document).on('blur','#billDialog_requestDate',function(e){
    e.stopPropagation();
    billing_date_varidate_for_billing();
})

let billed_flg_for_billing = false;
let H_SEIKYUDATE_for_billing = "";
let gGetuDate_for_billing = "";
let flg_for_billing = false;
async function billing_date_varidate_for_billing(){
    try{
        if(flg_for_billing)return false;
        flg_for_billing=true;

        let dt = new Date($('#billDialog_requestDate').val());
        let dt_element = $('#billDialog_requestDate');

        // if(H_SEIKYUDATE_for_billing == ""){
        //     flg_for_billing = false;
        //     return false;
        // }

        // if(dt_element.val() == ""){
        //     alert('請求日付が不正です。');
        //     flg_for_billing = false;
        //     return false;
        // }

        if($('#billDialog_estimateNo').val() == "" || H_SEIKYUDATE_for_billing.getTime() == dt.getTime()){
            flg_for_billing = false;
            return true;
        }

        if(gGetuDate_for_billing == ""){
            dates = await fetchSql(`SELECT * FROM TMDates WHERE DateID = '売掛月次更新日'`);
            if(dates.results.length != 0){
                gGetuDate_for_billing = new Date(dates.results[0].更新日付);
            }
        }

        if(H_SEIKYUDATE_for_billing.getTime() != dt.getTime()) {
            if(dt <= gGetuDate_for_billing || (H_SEIKYUDATE_for_billing != null && H_SEIKYUDATE_for_billing <= gGetuDate_for_billing)){
                alert('更新済みのため、修正できません。');
                dt_element.val(H_SEIKYUDATE_for_billing.toLocaleDateString('sv-SE'));
                await sleep(0);
                flg_for_billing = false;
                $(this).focus();
                return false;
            }
        }

        if(billed_flg_for_billing){
            alert('請求日付を変更した場合、請求金額に影響を及ぼすことがあります。\n確認をお願いします。');
            await sleep(3);
        }
        H_SEIKYUDATE_for_billing = dt;
        flg_for_billing = false;
        return true;
    }catch(err){

        alert('予期せぬエラーが発生しました。');
        console.error('請求書発行',err);
        flg_for_billing = false;
    }

}


// 請求書発行処理
async function download_bill_report(category,dialogId,dialogName,btnLabel,type){
    try{
        if(!validateDialog(dialogId)){
            return;
        }
    
        const estimate_no = $('#billDialog_estimateNo');
        const estimate_name = $('#billDialog_estimateName');
        const person      = $('#billDialog_displayManagerFrom');
        const billing_date  = $('#billDialog_requestDate');
        const publish_date = $('#billDialog_publishingDate');
    
        if(!await billing_date_varidate_for_billing()){
            return;
        }
    
        if(publish_date.val() == ""){
            alert('請求発行日が未入力です。');
            publish_date.focus();
            return;
        }
    
        const filename = `請求書-${estimate_no.val()}${estimate_name.val()}${new Date().toLocaleDateString('sv-SE').replaceAll('-','').slice(-6)}.${type}`;
    
        const item_params = {
            "@i見積番号":estimate_no.val(),
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
        }catch(e){
            return;
        }
    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error('請求書発行',err);
        flg_for_billing = false;
    }
    
}

async function delete_billDialog(category,dialogId,dialogName){
    try{
        const estimate_no = $('#billDialog_estimateNo').val();
        if(estimate_no == ""){
            alert('見積番号が未入力です。');
            return;
        }

        const item_params = {
            "@i見積番号":estimate_no,
        }
    
        const param = {
            "category": category,
            "title": dialogName,
            "button": '削除',
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };
    
        try{
            await download_report(param);
        }catch(e){
            return;
        }
        $('#billDialog_estimateNo').focus();
        alert('削除に成功しました。');
    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error('請求書発行',err);
        flg_for_billing = false;
    }
}