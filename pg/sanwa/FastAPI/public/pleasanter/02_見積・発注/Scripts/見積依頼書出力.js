{
    // 見積依頼書出力
    let dialogId = "EstimateRequestOutput";
    let dialogName = "見積依頼書出力";
    let category = "見積・発注";
    let btnLabel = "出力";
    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'estimateNo', label: '見積番号', options: {
            width: 'normal',
            required:true,
            digitsNum:6,
            varidate:{type:'int',maxlength:6},
            lookupOrigin:{tableId:"TD見積",keyColumnName:'見積番号'},
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
        { type: 'datepicker', id: 'DesiredDeliveryDate', label: '希望納期', options: {
            width: 'normal',
            varidate:{type:'str'},
        }},
        { type: 'datepicker', id: 'ResponseDeadline', label: '回答期限', options: {
            required: true,
            width: 'normal',
            varidate:{type:'str'},
        }},

        { type: 'range-text', id: 'Supplier', label: '仕入先', options: {
            width: 'wide',
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            search:true,
            searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
        }},

        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '出力', options: {
            icon:'disk',
            onclick:`estimateRequestOutput_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}


/**
 * 見積依頼書出力処理
 * @param {string} dialogId ダイアログID
 * @param {string} category カテゴリー
 * @param {string} dialogName ダイアログ名
 * @param {string} btnLabel ボタンラベル
 */
async function estimateRequestOutput_report(dialogId, category, dialogName, btnLabel) {
    try {
        // バリデーション
        if (!validateDialog(dialogId)) {
            return;
        }

        console.log('start : 見積依頼書');

        let item_params = {
            "@i見積番号": SpcToNull($('#EstimateRequestOutput_estimateNo').val()),
            "希望納期": SpcToNull($('#EstimateRequestOutput_DesiredDeliveryDate').val()),
            "回答期限": SpcToNull($('#EstimateRequestOutput_ResponseDeadline').val()),
            "@i開始CD": SpcToNull($('#EstimateRequestOutput_SupplierFrom').val()),
            "@i終了CD": SpcToNull($('#EstimateRequestOutput_SupplierTo').val()),
        };


        // パラメータ確認用ログ
        console.log("送信するパラメータ:", item_params);

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };

        let dt = new Date().toLocaleDateString().replaceAll('/', '').slice(2);
        let filename = "見積依頼書" + "-" + SpcToNull($('#EstimateRequestOutput_estimateName').val()) + dt;
        
        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 見積依頼書');

    } catch (err) {
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー", err);
    }
}