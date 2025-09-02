{
    // 発注書出力
    const category = '見積・発注';
    const dialogId = "PurchaseOrderOutput";
    const dialogName = "発注書出力";
    const btnLabel = "出力";
    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'estimateNo', label: '見積番号', options: {
            width: 'normal',
            required:true,
            digitsNum:6,
            varidate:{type:'int',maxlength:6},
            get_lastTime_output:'発注書',
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号'},
            searchDialog:{id:'estimateNoSearch1',title:'見積番号検索',multiple:false}
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
        { type: 'text-set', id: 'AmountDisplayCategory', label: '金額表示', options: {
            valueFrom:"0",
            type:"number",
            width:'wide',
            values: [
                { value: '0', text: 'する', checked: true },
                { value: '1', text: 'しない' }
            ],
        }},
        { type: 'text-set', id: 'FormatCategory', label: '書式区分', options: {
            valueFrom:"0",
            type:"number",
            width:'wide',
            values:[
                { value: "0", text: "通常" },
                { value: "1", text: "集計ACSなし" },
            ],
        }},

        { type: 'range-text', id: 'Supplier', label: '仕入先', options: {
            width: 'wide',
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
        }},

        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
        { type: 'borderText', id: 'test2', label: '前回出力情報', options: {
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: btnLabel, options: {
            icon:'disk',
            onclick:`download_PurchaseOrder_report('${category}','${dialogId}','${dialogName}','${btnLabel}')`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}


async function download_PurchaseOrder_report(category,dialogId,dialogName,btnLabel){
    try{
        const estimateNo     = $('#PurchaseOrderOutput_estimateNo');                    //見積番号
        const estimateName   = $('#PurchaseOrderOutput_estimateName');                  //見積件名
        const moneyDisplay   = $('#PurchaseOrderOutput_AmountDisplayCategoryFrom');     //金額表示
        const formatCategory = $('#PurchaseOrderOutput_FormatCategoryFrom');            //書式区分
        const suplier_from   = $('#PurchaseOrderOutput_SupplierFrom');                  //仕入先From
        const suplier_to     = $('#PurchaseOrderOutput_SupplierTo')                     //仕入先To

        if(estimateNo.val() == ""){
            estimateNo.focus();
            alert('見積番号が未入力です。');
        }

        if(!await get_estimateDB_for_purchaseOrder(estimateNo.val())){
            estimateNo.val('').focus();
            alert('指定の見積番号は存在しません。');
            return;
        }

        if(!["0","1"].includes(moneyDisplay.val())){
            moneyDisplay.val('').focus();
            alert('金額表示区分を入力してください。');
            return;
        }

        if(!["0","1"].includes(formatCategory.val())){
            formatCategory.val('').focus();
            alert('書式区分を入力してください。');
            return;
        }

        const file_name = `発注書-${estimateName.val()}${new Date().toLocaleDateString('sv-SE').replaceAll('-','').slice(-6)}.xlsx`;

        // パラメータ作成
        const item_params = {
            "@i見積番号":estimateNo.val(),
            "@i開始CD":SpcToNull(suplier_from.val()),
            "@i終了CD":SpcToNull(suplier_to.val()),
            "@format_category":formatCategory.val(),
            "@money_display":moneyDisplay.val(),
            "@pc_name":$("#LoginId").val()
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
            await download_report(param,file_name);
        }catch(err){
            return;
        }

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error('発注書出力',err);
        return;
    }
}

async function get_estimateDB_for_purchaseOrder(estimateNo){
    try{
        const sql = `
            SELECT
                見積番号, 見積件名, 得意先名1, 得意先名2, 現場名, 納期S, 納期E,
                受注区分, 売上日付, 見積区分
            From
                TD見積
            WHERE
                見積番号 = '${estimateNo}'
        `;

        try{
            var res = await fetchSql(sql);
        }catch(err){
            return false;
        }
        res = res.results;

        if(res.length == 0){
            return false;
        }

        if(res.見積区分 == 0){
            alert('見積区分が仮見積です。');
        }
        return true;

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        return false;
    }



}