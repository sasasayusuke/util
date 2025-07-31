//支払データ取込処理
{
    let category = "仕入・支払";
    let dialogId = "PaymentImportDialog";
    let dialogName = "支払データ取込処理";
    let btnLabel = "実行";


    createAndAddDialog(dialogId, dialogName, [
        { type: 'radio', id: 'ProcessingCategory', label: '処理区分', options: {
            values: [
                { value: '1', text: 'チェックのみ' ,checked:true},
                { value: '2', text: '取込'}
            ],
            width: 'wide'
        }},
        { type: 'radio', id: 'ListFlag', label: 'リスト出力', options: {
            values: [
                { value: '1', text: 'する',checked:true },
                { value: '2', text: 'しない' }
            ],
            width: 'wide'
        }},
        { type: 'ImportFile', id: 'csvData', label: '【取込先】', options: {
            fileType:'text/csv'
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'output', label: btnLabel, options: {
            icon:'disk',
            onclick:`PaymentImportDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function PaymentImportDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        if ($(`#csvData`)[0].files[0] == null) {
            alert('ファイルを指定してください');
            return;
        }

        let result = $(`#csvData`)[0].files[0]
        let text = await result.text()
        let csvData = commonConvertCsvTo2D(text)

        if ($(`#${dialogId}_ProcessingCategoryField input:checked`).val() == 2)  {
            kubun = "取込"
        } else {
            kubun = "チェックのみ"
        }

        if ($(`#${dialogId}_ListFlagField input:checked`).val() == 1)  {
            output = "する"
        } else {
            output = "しない"
        }

        let item_params = {
            "kubun": kubun,
            "csvData":csvData,
            "output": output
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };
        try{
            await download_report(param);
        }catch{
            return;
        }
        

        console.log(`end : ${dialogName}`);

        if (kubun == "チェックのみ"){
            alert('チェックが完了しました。');
        }

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}

