
{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let category = "請求・売掛";
    let dialogId = "WELCIAConstructionTallySheetDialog";
    let dialogName = "ウエルシア中日施工集計表";
    let btnLabel = "出力";

    // ウエルシア中日施工集計表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'billingDate', label: '請求日付', options: {
            width: 'wide',
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            required:true,
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'create', label: btnLabel, options: {
            icon:'disk',
            onclick:`WELCIAConstructionTallySheetDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function WELCIAConstructionTallySheetDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : ウエルシア中日施工集計表');

        let item_params = {
            "@iS請求日付":SpcToNull($('#WELCIAConstructionTallySheetDialog_billingDateFrom').val().replaceAll('-','/')),
            "@iE請求日付":SpcToNull($('#WELCIAConstructionTallySheetDialog_billingDateTo').val().replaceAll('-','/'))
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "Wel中日施工" + SpcToNull($('#WELCIAConstructionTallySheetDialog_billingDateFrom').val().replaceAll('-','')).slice(2,8) + "-" + SpcToNull($('#WELCIAConstructionTallySheetDialog_billingDateTo').val().replaceAll('-','')).slice(2,8) + ".xlsx"
        try{
            await download_report(param,filename);
        }catch{
            return;
        }
        

        console.log('end : ウエルシア中日施工集計表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}