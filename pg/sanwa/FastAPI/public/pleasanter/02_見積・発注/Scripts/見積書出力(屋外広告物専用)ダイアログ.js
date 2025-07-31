{
    // 見積書出力（屋外広告物専用）
    let dialogId = "EstimateOutputForOutdoorAdvertising";
    let dialogName = "見積書出力（屋外広告物専用）";
    let category = "見積・発注";
    let btnLabel = "出力";
    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'estimateNo', label: '見積番号', options: {
            width: 'normal',
            required:true,
            digitsNum:6,
            get_lastTime_output:'見積書',
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
        { type: 'text-set', id: 'displayManager', label: '表示担当', options: {
            width: 'wide',
            required:true,
            digitsNum:3,
            varidate:{type:'zeroPadding',maxlength:3},
            searchDialog:{id:'personSearch',title:'担当者検索',multiple:false},
            lookupOrigin:{tableId:'TM担当者',keyColumnName:'担当者CD',forColumnName:'担当者名'},
            lookupFor:[{columnName:'担当者CD',id:'estimateNo'}]
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
        { type: 'borderText', id: 'test2', label: '前回出力情報', options: {
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: 'Excel出力', options: {
            icon:'disk',
            onclick:`estimateOutputForOutdoorAdvertising_report('${dialogId}','${category}','${dialogName}','${btnLabel}','excel');`
        }},
        { type: 'button_inline', id: 'output', label: 'PDF出力', options: {
            icon:'disk',
            onclick:`estimateOutputForOutdoorAdvertising_report('${dialogId}','${category}','${dialogName}','${btnLabel}','pdf');`
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

async function estimateOutputForOutdoorAdvertising_report(dialogId, category, dialogName, btnLabel, format) {
    console.log("クリックcheck")
    try {
        // バリデーション
        if (!validateDialog(dialogId)) {
            return;
        }

        console.log('start : 見積書出力（屋外広告物専用）');

        const estimate_no = $('#EstimateOutputForOutdoorAdvertising_estimateNo');
        if(estimate_no.val() == ""){
            alert('見積番号が未入力です。');
            estimate_no.focus();
            return;
        }
        const cMitumoriH = new ClsMitumoriH(estimate_no.val());
        
        if(await cMitumoriH.IsUCategory('B') == true){
            alert('売価未確定項目があります。');
            estimate_no.val('').focus();
            return;
        }

        if(await cMitumoriH.GetbyID() == true){
            if(cMitumoriH.原価率 >= 100){
                alert('原価率が100％に達しています。');
            }
        }else{
            return;
        }

        let item_params = {
            "@i見積番号": SpcToNull($('#EstimateOutputForOutdoorAdvertising_estimateNo').val()),
            "書式区分": SpcToNull($('#EstimateOutputForOutdoorAdvertising_FormatCategoryFrom').val()),
            "表示担当": SpcToNull($('#EstimateOutputForOutdoorAdvertising_displayManagerFrom').val()),
            "ListName":"見積書", // 前回出力情報登録用
            "EstimateNo":$(`#${dialogId}_estimateNo`).val(), // 前回出力情報登録用
            "LoginId":$("#LoginId").val(), // 前回出力情報登録用
            "format":format, // excel・PDF出力判別用
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

        let dt = new Date().toLocaleDateString('sv-SE').replaceAll('-', '').substring(2);
        let filename = "見積書" + "-" + estimate_no.val() + SpcToNull($('#EstimateOutputForOutdoorAdvertising_estimateName').val()) + dt + (format == "excel" ? ".xlsx":".pdf");

        try{
            await download_report(param,filename);
        }catch(e){
            return;
        }

        console.log('end : 見積書出力（屋外広告物専用）');

    } catch (err) {
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー", err);
    }
}