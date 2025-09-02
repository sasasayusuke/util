{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let category = "請求・売掛";
    let dialogId = "listOfAccountsReceivebleDialog";
    let dialogName = "売掛明細一覧表";
    let btnLabel = "出力";

    // 売掛明細一覧表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'salesDay', label: '売上日付', options: {
            width: 'wide',
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            required:true
        }},
        { type: 'range-text', id: 'custmerCD', label: '得意先CD', options: {
            width: 'wide',
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'custmerNoSearch',title:'得意先検索'}
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'output', label: btnLabel, options: {
            icon:'disk',
            onclick:`listOfAccountsReceivebleDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function listOfAccountsReceivebleDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 売掛明細一覧表');

        let item_params = {
            "@iS集計日付":SpcToNull($('#listOfAccountsReceivebleDialog_salesDayFrom').val().replaceAll('-','/')),
            "@iE集計日付":SpcToNull($('#listOfAccountsReceivebleDialog_salesDayTo').val().replaceAll('-','/')),
            "@iS得意先CD":SpcToNull($('#listOfAccountsReceivebleDialog_custmerCDFrom').val()),
            "@iE得意先CD":SpcToNull($('#listOfAccountsReceivebleDialog_custmerCDTo').val())
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "売掛明細一覧表" + SpcToNull($('#listOfAccountsReceivebleDialog_salesDayFrom').val().replaceAll('-','')).slice(2) + "-" + SpcToNull($('#listOfAccountsReceivebleDialog_salesDayTo').val().replaceAll('-','')).slice(2) + ".xlsx"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 売掛明細一覧表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}
