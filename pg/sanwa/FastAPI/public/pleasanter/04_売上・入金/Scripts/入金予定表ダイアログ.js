// 入金予定表作成
{
    let category = "売上・入金";
    let dialogId = "paymentScheduleListDialog";
    let dialogName = "入金予定表";
    let btnLabel = "印刷";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'scheduleDate', label: '予定日付', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
            valueFrom:monthDateGein(1),
            valueTo:monthDateGein(99)
        }},
        { type: 'range-text', id: 'custmerCode', label: '得意先CD', options: {
            width: 'wide',
            varidate:{type:'zeroPadding',maxlength:4},
            search:true,
            searchDialog:{id:'custmerNoSearch',title:'得意先検索'}
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'output', label: btnLabel, options: {
            icon:'disk',
            onclick:`paymentScheduleList_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function paymentScheduleList_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 入金予定表');

        let item_params = {
            "@iS入金予定日付":SpcToNull($('#paymentScheduleListDialog_scheduleDateFrom').val().replaceAll('-','/')),
            "@iE入金予定日付":SpcToNull($('#paymentScheduleListDialog_scheduleDateTo').val().replaceAll('-','/')),
            "@iS得意先CD":SpcToNull($('#paymentScheduleListDialog_custmerCodeFrom').val()),
            "@iE得意先CD":SpcToNull($('#paymentScheduleListDialog_custmerCodeTo').val())
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        const date_from = $('#paymentScheduleListDialog_scheduleDateFrom').val().replaceAll('-','').substring(2);
        const date_to = $('#paymentScheduleListDialog_scheduleDateTo').val().replaceAll('-','').substring(2);

        let filename = `入金予定表${date_from}-${date_to}.xlsx`;
        try{
            await download_report(param,filename);
        }catch{
            return;
        }
        

        console.log('end : 入金予定表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}