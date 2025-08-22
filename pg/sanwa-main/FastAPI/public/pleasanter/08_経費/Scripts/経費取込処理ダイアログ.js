//経費取込処理
{
    let category = "経費";
    let dialogId = "expenseAccountingDialog";
    let dialogName = "経費取込処理";
    let btnLabel = "実行";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'radio', id: 'processCategory', label: '集計区分', options: {
            width:'wide',
            values:[
                {value:0,text:'チェックのみ',checked:true},
                {value:1,text:'取込'}
            ]
        }},
        { type: 'radio', id: 'listOutput', label: 'リスト出力', options: {
            width:'wide',
            values:[
                {value:0,text:'する',checked:true},
                {value:1,text:'しない'}
            ]
        }},

        {type:'range-date',id:"deleteDate",label:'削除日付',options:{
            width:"normal",
            varidate:{type:'str'},
        }},

        { type: 'ImportFile', id: 'csvData', label: '【取込先】', options: {
            fileType:'.txt'
        }},
    ],
    [
        { type: 'button_inline', id: 'execute_btn', label: '実行', options: {
            icon:'disk',
            onclick:`expenseAccountingDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
        { type: 'button_inline', id: 'delete_btn', label: '削除', options: {
            icon:'trash',
            onclick:`expenseAccountingDialog_delete('${dialogId}','${category}','${dialogName}','削除');`
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
async function expenseAccountingDialog_report(dialogId, category, dialogName, btnLabel) {
    try {
        // バリデーション
        if (!validateDialog(dialogId)) {
            return;
        }

        console.log(`start : ${dialogName} report`);

        if ($(`#csvData`)[0].files[0] == null) {
            alert('ファイルを指定してください');
            return;
        }

        let file = $(`#csvData`)[0].files[0]; // ファイルを取得
        let csvData;

        if (file) {
            const reader = new FileReader();

            // ファイル読み込み成功時の処理
            reader.onload = function () {
                const arrayBuffer = reader.result; // ArrayBufferを取得
                const decoder = new TextDecoder('shift_jis'); // Shift JISでデコード
                const text = decoder.decode(arrayBuffer); // デコードして文字列を取得
                csvData = commonConvertCsvTo2D(text); // csvData を設定
            };

            // ファイル読み込み失敗時の処理
            reader.onerror = function () {
                alert('ファイルの読み込みに失敗しました');
            };

            reader.readAsArrayBuffer(file); // ファイルをArrayBufferとして読み込む

            // 非同期処理の完了を待つ
            await new Promise((resolve) => (reader.onloadend = resolve));
        }

        if ($(`#${dialogId}_processCategoryField input:checked`).val() == 0) {
            kubun = "チェックのみ";
        } else {
            kubun = "取込";
        }

        if ($(`#${dialogId}_listOutputField input:checked`).val() == 0) {
            output = "する";
        } else {
            output = "しない";
        }

        let item_params = {
            kubun: kubun,
            csvData: csvData,
            output: output,
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

/**
 *
 * @param {string} dialogId     ダイアログID
 * @param {string} category     カテゴリー
 * @param {string} dialogName   ダイアログ名
 * @param {string} btnLabel     ボタンラベル
 * @returns
 */
async function expenseAccountingDialog_delete(dialogId, category, dialogName, btnLabel) {
    try {

        console.log(`start : ${dialogName} delete`);

        if(!$(`#${dialogId}_deleteDateFrom`).val()){
            alert('開始日付が不正です。');
            $(`#${dialogId}_deleteDateFrom`).focus();
            return;
        }

        if(!$(`#${dialogId}_deleteDateTo`).val()){
            alert('終了日付が不正です。');
            $(`#${dialogId}_deleteDateTo`).focus();
            return;
        }

        let fromDt = new Date($(`#${dialogId}_deleteDateFrom`).val());
        let toDt   = new Date($(`#${dialogId}_deleteDateTo`).val());

        if(fromDt > toDt){
            alert('日付範囲が不正です。');
            $(`#${dialogId}_deleteDateFrom`).focus();
            return;
        }


        let item_params = {
            "@is日付":SpcToNull($(`#${dialogId}_deleteDateFrom`).val().replaceAll('-','/')),
            "@ie日付":SpcToNull($(`#${dialogId}_deleteDateTo`).val().replaceAll('-','/'))
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

        console.log(`end : ${dialogName} delete`);
    } catch (err) {
        alert("予期せぬエラーが発生しました。");
        console.error(dialogName + "エラー", err);
    }
}