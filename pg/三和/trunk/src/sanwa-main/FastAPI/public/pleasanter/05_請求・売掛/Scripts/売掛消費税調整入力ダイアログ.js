{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')

    let category = "請求・売掛"
    let dialogId = "ReceivableSalesTaxAdjustmentEntry";
    let dialogName = "売掛消費税調整入力";

    // 売掛金集計表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'adjustmentNo', label: '調整NO', options: {
            varidate:{type:'int',maxlength:15},
            disabled:true,
            lock:'消費税調整番号'
        }},
        { type: 'datepicker', id: 'adjustmentDate', label: '調整日付', options: {
            varidate:{type:'str'},
            required:true
        }},
        { type: 'text-set', id: 'custmerCD', label: '得意先CD',forColumnName:'MH.得意先CD', options: {
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            lookupOrigin:{tableId:'TM得意先',keyColumnName:"得意先CD",forColumnName:'得意先名1'},
            width:'wide',
            searchDialog:{id:'custmerNoSearch',title:'得意先番号検索'}
        }},
        { type: 'text', id: 'adjustmentMoney', label: '調整金額', options: {
            alignment:'end',
            varidate:{type:'int',maxlength:15},
        }},
        { type: 'range-date', id: 'selectionDate', label: '抽出日付', forColumnName:'USH.請求日付' ,options: {
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            t_heads:[
                {label:'見積NO',ColumnName:'見積番号',},
                {label:'請求日付',ColumnName:'請求日付',},
                {label:'見積件名',ColumnName:'見積件名'},
                {label:'請求金額',ColumnName:'合計金額',alignment:'end'},
                {label:'請求消費税',ColumnName:'請求消費税',alignment:'end'},
                {label:'売上金額',ColumnName:'売上金額',alignment:'end'},
                {label:'売上消費税',ColumnName:'売上消費税',alignment:'end'},
            ]
        }},
    ],
    [
        { type: 'button_inline', id: 'select', label: '選択', options: {
            icon:'select',
        }},
        { type: 'button_inline', id: 'delete', label: '削除', options: {
            icon:'cancel',
            hidden:true,
            onclick:`ReceivableSalesTaxAdjustmentEntry_deleteClick('${category}','${dialogId}','${dialogName}','削除')`
        }},
        { type: 'button_inline', id: 'clear', label: '中止', options: {
            icon:'cancel',
            onclick:`ReceivableSalesTaxAdjustmentEntry_cancelClick()`
        }},
        { type: 'button_inline', id: 'create', label: '保存', options: {
            icon:'disk',
            onclick:`ReceivableSalesTaxAdjustmentEntry_saveClick('${category}','${dialogId}','${dialogName}','保存')`
        }},
    ]);
    
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,1000,'px',function(){
        $('#ReceivableSalesTaxAdjustmentEntry_delete').css('display','none');
        $('#ReceivableSalesTaxAdjustmentEntry').prev().children('.ui-dialog-title').text('売掛消費税調整入力 -- 登録');
        document.getElementById("ReceivableSalesTaxAdjustmentEntry_searchTable_searchButton").innerHTML = `
            <span class="ui-button-icon ui-icon ui-icon-search search" style="position: static;"></span>
            <span class="ui-button-icon-space"></span>
            抽出
        `;
    })})


}

// 調整NOルックアップ
$(document).on('blur','#ReceivableSalesTaxAdjustmentEntry_adjustmentNo',async function(){
    try{
        const tyosei_no = $('#ReceivableSalesTaxAdjustmentEntry_adjustmentNo').val();

        if(tyosei_no == ""){
            return;
        }

        const query1 = `
            SELECT ND.消費税調整番号,ND.調整日付,ND.得意先CD,ND.調整金額,ND.初期登録日,ND.登録変更日,TM.略称 
            FROM TD消費税調整 AS ND
            INNER JOIN TM得意先 AS TM
            ON ND.得意先CD = TM.得意先CD
            WHERE 消費税調整番号 = ${tyosei_no}
        `;
        try{
            var res = await fetchSql(query1);
            res = res.results;

        }catch(e){
            return;
        }

        if(res.length != 0){
            // データロック
            var Lockres = await LockData("消費税調整番号", SpcToNull(tyosei_no, 0));
            if (!Lockres){
                $('#ReceivableSalesTaxAdjustmentEntry_adjustmentNo').val("");
                return;
            }
            $('#ReceivableSalesTaxAdjustmentEntry_adjustmentDate').val(new Date(res[0].調整日付).toLocaleDateString('sv-SE'));
            $('#ReceivableSalesTaxAdjustmentEntry_custmerCDFrom').val(res[0].得意先CD);
            $('#ReceivableSalesTaxAdjustmentEntry_custmerCDTo').val(res[0].略称);
            $('#ReceivableSalesTaxAdjustmentEntry_adjustmentMoney').val(res[0].調整金額);
        }

        // タイトル・削除ボタン
        $('#ReceivableSalesTaxAdjustmentEntry').prev().children('.ui-dialog-title').text('売掛消費税調整入力 -- 修正');

        // 月次更新日チェック
        const cls_dates = new clsDates('売掛月次更新日');
        await cls_dates.GetbyID();
        gGetuDate = cls_dates.updateDate;
        if(new Date(res[0].調整日付) <= gGetuDate){
            $('#ReceivableSalesTaxAdjustmentEntry_create').css('display','none');
            $('#ReceivableSalesTaxAdjustmentEntry input').attr('readonly',true);
            alert('更新済みの為、修正できません。');
        }else{
            $('#ReceivableSalesTaxAdjustmentEntry_delete').css('display','inline-block');
        }

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
    
})

// 登録処理
async function ReceivableSalesTaxAdjustmentEntry_saveClick(category,dialogId,dialogName,btnLabel){
    try{
        if(await ReceivableSalesTaxAdjustmentEntry_itemCheck()){
            return;
        }
        if(!confirm('保存します。')){
            return;
        }

        if($(`#ReceivableSalesTaxAdjustmentEntry_adjustmentNo`).val() == ""){
            // 新規
            modeF = 0
        }else{
            // 修正
            modeF = 1
        }

        const item_params = {
            modeF:modeF,
            adjustmentNo:$(`#ReceivableSalesTaxAdjustmentEntry_adjustmentNo`).val(),
            adjustmentDate:$(`#ReceivableSalesTaxAdjustmentEntry_adjustmentDate`).val().replaceAll('-','/'),
            customerCD:$(`#ReceivableSalesTaxAdjustmentEntry_custmerCDFrom`).val(),
            adjustmentMoney:Number(null_to_zero($(`#ReceivableSalesTaxAdjustmentEntry_adjustmentMoney`).val().replaceAll(',',''),0)),
        }

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        try{
            await download_report(param);
        }catch{
            return;
        }

        // 初期化
        ReceivableSalesTaxAdjustmentEntry_initilize()

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}

async function ReceivableSalesTaxAdjustmentEntry_itemCheck(){
    try{

        const adjustmentDate = $(`#ReceivableSalesTaxAdjustmentEntry_adjustmentDate`);
        const customerCD =  $(`#ReceivableSalesTaxAdjustmentEntry_custmerCDFrom`);

        let resFlg = true;

        if(!adjustmentDate.val()){
            alert('調整日付が未入力です。');
            adjustmentDate.focus();
            return resFlg;
        }

        const cls_dates2 = new clsDates('売掛月次更新日');
        await cls_dates2.GetbyID();
        gGetuDate = cls_dates2.updateDate;
        if(new Date(adjustmentDate.val()) <= gGetuDate){
            alert('更新済みの為、修正できません。');
            adjustmentDate.focus();
            return resFlg;
        }

        if(customerCD.val() == ""){
            alert('得意先CDが未入力です。');
            customerCD.focus();
            return resFlg;
        }

        return false;

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}

// 削除処理
async function ReceivableSalesTaxAdjustmentEntry_deleteClick(category,dialogId,dialogName,btnLabel){
    try{
        if(!confirm('削除します。')){
            return;
        }

        const item_params = {
            adjustmentNo:$(`#ReceivableSalesTaxAdjustmentEntry_adjustmentNo`).val()
        }

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        try{
            await download_report(param);
        }catch{
            return;
        }

        // 初期化
        ReceivableSalesTaxAdjustmentEntry_initilize()

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}

// 中止処理
async function ReceivableSalesTaxAdjustmentEntry_cancelClick(){
    try{
        if(!confirm('現在の編集内容を破棄します。')){
            return;
        }

        // 初期化
        ReceivableSalesTaxAdjustmentEntry_initilize()

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}

// クリア関数
async function ReceivableSalesTaxAdjustmentEntry_initilize(){
    // ロックの解除
    UnLockData("消費税調整番号", SpcToNull($(`#ReceivableSalesTaxAdjustmentEntry_adjustmentNo`).val(), 0))

    // 項目のクリア
    $(`#ReceivableSalesTaxAdjustmentEntry_adjustmentNo`).val('')
    $(`#ReceivableSalesTaxAdjustmentEntry_adjustmentDate`).val('')
    $(`#ReceivableSalesTaxAdjustmentEntry_custmerCDFrom`).val('')
    $(`#ReceivableSalesTaxAdjustmentEntry_custmerCDTo`).val('')
    $(`#ReceivableSalesTaxAdjustmentEntry_adjustmentMoney`).val('')
    $(`#ReceivableSalesTaxAdjustmentEntry_selectionDateFrom`).val('')
    $(`#ReceivableSalesTaxAdjustmentEntry_selectionDateTo`).val('')
    $('#ReceivableSalesTaxAdjustmentEntry input').attr('readonly',false);

    $('#ReceivableSalesTaxAdjustmentEntry_create').css('display','inline-block');
    $('#ReceivableSalesTaxAdjustmentEntry_delete').css('display','none');

    $('#ReceivableSalesTaxAdjustmentEntry').prev().children('.ui-dialog-title').text('売掛消費税調整入力 -- 登録');

    // 抽出テーブルのリセット
    $(`#ReceivableSalesTaxAdjustmentEntry_searchTable_readTable tbody`).html("")

    $(`#ReceivableSalesTaxAdjustmentEntry_adjustmentDate`).focus();
}

// $(document).on('blur',`#ReceivableSalesTaxAdjustmentEntry_adjustmentDate`,async function(){
//     // 月次更新日チェック
//     console.log($(':focus'))
//     if($('#ReceivableSalesTaxAdjustmentEntrySearchDialog').dialog('isOpen') == true || $(this).val() == ''){
//         return;
//     }
//     const cls_dates = new clsDates('売掛月次更新日');
//     await cls_dates.GetbyID();
//     gGetuDate = cls_dates.updateDate;
//     if(new Date($(`#ReceivableSalesTaxAdjustmentEntry_adjustmentDate`).val()) <= gGetuDate){
//         alert('更新済みの為、修正できません。');
//     }
// })

 const ReceivableSalesTaxAdjustmentEntry_baseSQL = e=>{ 
    return `
        SELECT TOP 1000 MH.得意先CD, MH.得意先名1,USH.請求日付,USH.見積番号,MH.見積件名,
        USH.合計金額, UH.売上金額, 
            USH.外税額 AS 請求消費税 , UH.外税額 AS 売上消費税
        FROM    TD売上請求H AS USH
            INNER JOIN
                    (SELECT UH.見積番号,
                            SUM(UH.税抜金額) AS 売上金額,
                            SUM(UH.外税額) AS 外税額
                        FROM    TD売上v2 AS UH
                        GROUP BY    UH.見積番号
                    ) AS UH
                    ON USH.見積番号 = UH.見積番号
            LEFT JOIN TD見積 AS MH
                ON USH.見積番号 = MH.見積番号
        ${e} AND NOT(USH.合計金額 = UH.売上金額 AND USH.外税額 = UH.外税額)
        ORDER BY    USH.請求日付,USH.見積番号`
    }

$(document).on('mousedown',`#ReceivableSalesTaxAdjustmentEntry_select`, function(){
    UnLockData("消費税調整番号", SpcToNull($(`#ReceivableSalesTaxAdjustmentEntry_adjustmentNo`).val(), 0))
    $(`#ReceivableSalesTaxAdjustmentEntry input`).val('')
    openDialog_for_searchDialog('ReceivableSalesTaxAdjustmentEntrySearch','ReceivableSalesTaxAdjustmentEntry_adjustmentNo',1000,'売掛消費税調整選択画面');
})