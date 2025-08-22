// 納品書発行
{
    let category = "売上・入金";
    let dialogId = "DeliverySlipOutput";
    let dialogName = "納品書発行";
    let btnLabel = "出力";
    createAndAddDialog(dialogId, dialogName, [
        { type: 'text', id: 'estimateNo', label: '見積番号', options: {
            width: 'normal',
            required:true,
            digitsNum:6,
            get_lastTime_output:'納品書',
            varidate:{type:'int',maxlength:6},
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号'},
            search:true,
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
        { type: 'radio', id: 'AmountDisplayCategory', label: '金額表示', options: {
            values: [
                { value: '1', text: 'する', checked: true },
                { value: '2', text: 'しない' }
            ],
            width: 'normal'
        }},
        // { type: 'buttonTextSet', id: 'outputDestination', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
        { type: 'borderText', id: 'lastOutputData', label: '前回出力情報', options: {
        }},
    ],
    [
        { type: 'button_inline', id: 'create', label: 'Excel出力', options: {
            icon:'disk',
            onclick:`DeliverySlipOutput_report('${dialogId}','${category}','${dialogName}','${btnLabel}', 'xlsx');`
        }},
        { type: 'button_inline', id: 'exceloutput', label: "PDF出力", options: {
            icon:'disk',
            onclick:`DeliverySlipOutput_report('${dialogId}','${category}','${dialogName}','${btnLabel}', 'pdf');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)});

    $(document).on('change','#DeliverySlipOutput_AmountDisplayCategoryField input',function(){
        const checked_element = $('DeliverySlipOutput_AmountDisplayCategoryField input:checked');
        const listName = checked_element.val() == 1 ? "納品書" : "物品受領書";
        const estimateNo = $('#DeliverySlipOutput_estimateNo');
        if(estimateNo.val() == "")return;
        get_history(listName,estimateNo.val(),dialogId)
    })
}

/**
 *
 * @param {string} dialogId     ダイアログID
 * @param {string} category     カテゴリー
 * @param {string} dialogName   ダイアログ名
 * @param {string} btnLabel     ボタンラベル
 * @param {string} type         帳票種類
 * @returns
 */
async function DeliverySlipOutput_report(dialogId,category,dialogName,btnLabel,type){

    // 金額表示チェック
    let elements = document.getElementsByName('DeliverySlipOutput_AmountDisplayCategory');
    let len = elements.length;
    let checkValue = '';
    for (let i = 0; i < len; i++){
        if (elements.item(i).checked){
            checkValue = elements.item(i).value;
        }
    }
    console.log('================')
    console.log(checkValue)

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 納品書');

        let item_params = {
            "@i見積番号":SpcToNull($('#DeliverySlipOutput_estimateNo').val()),
            "type": type,
            "LoginId":$("#LoginId").val(),
        };

        var today = new Date();
        var rep_name;
        
        if (checkValue == 1){
            var param = {
                "category": category,
                "title": dialogName,
                "button": btnLabel,
                "user": $p.userName(),
                "opentime": dialog_openTime,
                "params":item_params
            };
            rep_name = '納品書'
        } else {
            var param = {
                "category": category,
                "title": dialogName,
                "button": '物品出力',
                "user": $p.userName(),
                "opentime": dialog_openTime,
                "params":item_params
            };
            rep_name = '物品受領書'
        }

        const dt = today.toLocaleDateString('sv-SE').replaceAll('-','').substring(2);
        const filename = rep_name + $('#DeliverySlipOutput_estimateNo').val() + $('#DeliverySlipOutput_estimateName').val() + dt + "." + type;
        try{
            await download_report(param,filename);
        }
        catch{
            return;
        }
        

        console.log('end : 納品書');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}