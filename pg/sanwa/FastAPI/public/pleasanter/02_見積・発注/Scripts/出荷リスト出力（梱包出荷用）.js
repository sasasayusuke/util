{
    let category = "見積・発注";
    let dialogId = "shippingList_packing";
    let dialogName = "出荷リスト出力（梱包出荷用）";
    let btnLabel = "出力";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'estimateNo', label: '見積番号', options: {
            width: 'normal',
            required:true,
            digitsNum:6,
            varidate:{type:'int',maxlength:6},
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号'},
            search:true,
            searchDialog:{id:'estimateNoSearch2',title:'見積番号検索',multiple:false}
        }},
        { type: 'text', id: 'estimateName', label: '見積件名', options: {
            width: 'wide',
            marginTop: true,
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
    ],
    [
        { type: 'button_inline', id: 'create', label: '出力', options: {
            icon:'disk',
            onclick:`shippingList_packing_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]
    );
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
async function shippingList_packing_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 入金予定表');

        let item_params = {
            "@i見積番号":SpcToNull($('#shippingList_packing_estimateNo').val()),
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let dt = new Date().toLocaleDateString('sv-SE').replaceAll('-','').substring(2);
        let filename = "出荷リスト" + "-" + SpcToNull($('#shippingList_packing_estimateName').val()) + dt;

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 出荷リスト出力（梱包出荷用）');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}

