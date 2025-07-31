{
    // 部材リスト（一覧）出力
    let category = "見積・発注";
    let dialogId = "partsListDialog";
    let dialogName = "部材リスト（一覧）出力";
    let btnLabel = "出力";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'estimateNo', label: '見積番号', options: {
            width: 'normal',
            required:true,
            digitsNum:6,
            get_lastTime_output:'部材リスト',
            varidate:{type:'int',maxlength:6},
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
        // { type: 'text-set', id: 'Category', label: '集計区分', options: {
        //     width: 'normal',
        //     required:true,
        //     varidate:{type:'zeroPadding',maxlength:1},
        //     valueFrom:'0',
        //     type:"number",
        //     values:[
        //         { "value": "0", "text": "通常" },
        //         { "value": "1", "text": "集計" },
        //     ]
        // }},
        { type: 'borderText', id: 'test2', label: '前回出力情報', options: {
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '出力', options: {
            icon:'disk',
            onclick:`DownloadPartsList('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}


async function DownloadPartsList(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }
        
        console.log(`start : ${dialogName}`);

        let item_params = {
            "EstimateNo":$(`#${dialogId}_estimateNo`).val(),
            // "Category":$(`#${dialogId}_CategoryFrom`).val(),
            "LoginId":$("#LoginId").val(),
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };

        let dt = new Date().toLocaleDateString('sv-SE').replaceAll('-','').slice(2);
        let filename = "部材リスト" + "-" + SpcToNull($('#partsListDialog_estimateName').val()) + dt;

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log(`end : ${dialogName}`);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}