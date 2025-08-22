{
    let category = "見積・発注";
    let dialogId = "OrderOutput";
    let dialogName = "注文書出力";
    let btnLabel = "出力";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'estimateNo', label: '見積番号', options: {
            width: 'normal',
            required:true,
            digitsNum:6,
            get_lastTime_output:'注文書',
            varidate:{type:'int',maxlength:6},
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号'},
            searchDialog:{id:'estimateNoSearch2',title:'見積番号検索',multiple:false}
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
        { type: 'text-set', id: 'FormatCategory', label: '書式区分', options: {
            width: 'wide',
            valueFrom:'0',
            type:"number",
            required:true,
            values:[
                { "value": "0", "text": "通常" },
                { "value": "1", "text": "相見積" },
                { "value": "2", "text": "客先在庫" },
                { "value": "3", "text": "部材計" },
                { "value": "4", "text": "部材計・客在" }
            ]
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
        { type: 'borderText', id: 'test2', label: '前回出力情報', options: {
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: "Excel出力", options: {
            icon:'disk',
            onclick:`OrderOutput_report('${dialogId}','${category}','${dialogName}','${btnLabel}', 'excel');`
        }},
        { type: 'button_inline', id: 'exceloutput', label: "PDF出力", options: {
            icon:'disk',
            onclick:`OrderOutput_report('${dialogId}','${category}','${dialogName}','${btnLabel}', 'pdf');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})

}

/**
 *
 * @param {string} dialogId     ダイアログID
 * @param {string} category     カテゴリー
 * @param {string} dialogName   ダイアログ名
 * @param {string} btnLabel     ボタンラベル
 * @returns
 */
async function OrderOutput_report(dialogId,category,dialogName,btnLabel,type){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        let dt = new Date().toLocaleDateString('sv-SE').replaceAll('-','').substring(2);

        let item_params = {
            "@i見積番号":SpcToNull($(`#${dialogId}_estimateNo`).val()),
            "@i相見積区分":SpcToNull($(`#${dialogId}_FormatCategoryFrom`).val()),
            "type": type
        };


        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $("#LoginId").val(),
            "opentime": dialog_openTime,
            "params": item_params
        };
        
        let file_extension;
        if(type == "pdf"){
            file_extension = "pdf";
        } else {
            file_extension = "xlsx";
        }

        let filename = "注文書-" + $(`#${dialogId}_estimateNo`).val() + $(`#${dialogId}_estimateName`).val() + dt + "送信用." + file_extension

        try{
            await download_report(param,filename);
        }catch(e){
            return;
        }

        console.log(`end : ${dialogName}`);


    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}