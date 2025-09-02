{
    let dialogId = "EstimateOutputForKose";
    let dialogName = "見積書出力（コーセー専用）";
    let category = "見積・発注";
    let btnLabel = "出力";
    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'HeadEstimateNo', label: '見出し見積', options: {
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
            lookupFor:[{columnName:'見積件名',id:'HeadEstimateNo'}]
        }},
        { type: 'text', id: 'recipientOfAnOrder', label: '受注先', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'得意先名',specialTerms:'得意先名 = 得意先名1 + 得意先名2',id:'HeadEstimateNo'}]
        }},
        { type: 'text', id: 'deliveryDay', label: '納期', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'納期',specialTerms:`ISNULL(FORMAT(納期S, 'yyyy/MM/dd'), '') + (CASE WHEN 納期E IS NULL THEN '' ELSE '~' END) + ISNULL(FORMAT(納期E, 'yyyy/MM/dd'), '') as 納期`,id:'HeadEstimateNo'}]
        }},
        { type: 'text', id: 'siteName', label: '現場名', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'現場名',id:'HeadEstimateNo'}]
        }},
        { type: 'text', id: 'ListEstimateNo', label: '明細見積', options: {
            width: 'normal',
            marginTop: true,
            required:true,
            digitsNum:6,
            varidate:{type:'int',maxlength:6},
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号'},
            searchDialog:{id:'estimateNoSearch1',title:'見積番号検索',multiple:false}
        }},
        { type: 'text', id: 'estimateName2', label: '見積件名', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'見積件名',id:'ListEstimateNo'}]
        }},
        { type: 'text', id: 'recipientOfAnOrder2', label: '受注先', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'得意先名',specialTerms:'得意先名 = 得意先名1 + 得意先名2',id:'ListEstimateNo'}]
        }},
        { type: 'text-set', id: 'DisplayManager', label: '表示担当', options: {
            width:'wide',
            required:true,
            marginTop: true,
            digitsNum:3,
            varidate:{type:'zeroPadding',maxlength:3},
            searchDialog:{id:'personSearch',title:'担当者検索',multiple:false},
            lookupOrigin:{tableId:'TM担当者',keyColumnName:'担当者CD',forColumnName:'担当者名'},
            lookupFor:[{columnName:'担当者CD',id:'HeadEstimateNo'}]
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
            onclick:`estimateOutputForKose_report('${dialogId}','${category}','${dialogName}','${btnLabel}','excel');`
        }},
        { type: 'button_inline', id: 'output', label: 'PDF出力', options: {
            icon:'disk',
            onclick:`estimateOutputForKose_report('${dialogId}','${category}','${dialogName}','${btnLabel}','pdf');`
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

async function estimateOutputForKose_report(dialogId, category, dialogName, btnLabel, format) {
    console.log("クリックcheck")
    try {
        // 必須項目チェック
        if (!validateDialog(dialogId)) {
            return;
        }

        const head_estimateNo = $('#EstimateOutputForKose_HeadEstimateNo');
        const detail_estimateNo = $('#EstimateOutputForKose_ListEstimateNo');

        const cMitumoriH_head = new ClsMitumoriH(head_estimateNo.val());
        const cMitumoriH_detail = new ClsMitumoriH(detail_estimateNo.val());

        if(await cMitumoriH_head.IsUCategory('B')){
            alert('見出し見積に売価未確定項目があります。');
            head_estimateNo.val('').focus();
            return;
        }
        
        if(await cMitumoriH_head.GetbyID() == true && await cMitumoriH_detail.GetbyID() == true){
            if(cMitumoriH_head.原価率 >= 100){
                alert('見出し見積の原価率が100％に達しています。');
            }
            if(cMitumoriH_detail.原価率 >= 100){
                alert('明細見積の原価率が100％に達しています。');
            }
        }else{
            return;
        }

        console.log('start : 見積書出力（コーセー専用）');

        let item_params = {
            "@i見積番号": SpcToNull($('#EstimateOutputForKose_HeadEstimateNo').val()),
            "@i明細見積": SpcToNull($('#EstimateOutputForKose_ListEstimateNo').val()),
            "表示担当": SpcToNull($('#EstimateOutputForKose_DisplayManagerFrom').val()),
            "ListName":"見積書", // 前回出力情報登録用
            "EstimateNo":$(`#${dialogId}_HeadEstimateNo`).val(), // 前回出力情報登録用
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
        let filename = "見積書出力" + "-" + head_estimateNo.val() + SpcToNull($('#EstimateOutputForKose_estimateName').val()) + dt + (format == "excel" ? ".xlsx":".pdf");

        try{
            await download_report(param,filename);
        }catch(e){
            return;
        }

        console.log('end : 見積書出力（コーセー専用）');

    } catch (err) {
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー", err);
    }
}