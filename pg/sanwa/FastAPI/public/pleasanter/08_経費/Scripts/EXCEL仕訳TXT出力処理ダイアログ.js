//EXCEL仕訳TXT出力処理
{
    let category = "経費";
    let dialogId = "ExcelJournalTXTOutputDialog";
    let dialogName = "EXCEL仕訳TXT出力処理";
    let btnLabel = "実行";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'radio', id: 'processCategory', label: '処理区分', options: {
            width:'wide',
            valueFrom:'0',
            values:[
                {value:0,text:'チェックのみ',checked:true},
                {value:1,text:'CSV出力'}
            ]
        }},
        { type: 'radio', id: 'listOutput', label: 'リスト出力', options: {
            width:'wide',
            values:[
                {value:0,text:'する',checked:true},
                {value:1,text:'しない'}
            ]
        }},
        { type: 'ImportFile', id: 'excelData', label: '【取込先】', options: {
            fileType:'.xls'
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '実行', options: {
            icon:'disk',
            onclick:`ExcelJournalTXTOutputDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function ExcelJournalTXTOutputDialog_report(dialogId, category, dialogName, btnLabel) {
    try {
        // バリデーション
        if (!validateDialog(dialogId)) {
            return;
        }

        console.log(`start : ${dialogName} report`);

        let file = $(`#excelData`)[0].files[0]; // ファイルを取得
        if (file == null) {
            alert('ファイルを指定してください');
            return;
        }

        // Base64化
        let base64excelData = await commonToBase64(file);

        let kubun = ""
        if ($(`#${dialogId}_processCategoryField input:checked`).val() == 0) {
            kubun = "チェックのみ";
        } else {
            kubun = "CSV出力";
        }

        let output = ""
        if ($(`#${dialogId}_listOutputField input:checked`).val() == 0) {
            output = "する";
        } else {
            output = "しない";
        }

        let item_params = {
            kubun: kubun,
            excelData: base64excelData,
            output: output,
            filename: $(`#excelData`)[0].files[0].name
        };

        const param = {
            category: category,
            title: dialogName,
            button: btnLabel,
            user: $p.userName(),
            opentime: dialog_openTime,
            params: item_params,
        };

        try{
            await download_report(param);
        }catch{
            return;
        }

        console.log(`end : ${dialogName} report`);
    } catch (err) {
        alert("予期せぬエラーが発生しました。");
        console.error(dialogName + "エラー", err);
    }
}