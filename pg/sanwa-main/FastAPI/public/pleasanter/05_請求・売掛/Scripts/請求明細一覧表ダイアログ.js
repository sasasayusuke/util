

// 請求明細一覧表
{

    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let category = "請求・売掛";
    let dialogId = "requestParticularsListDialog";
    let dialogName = "請求明細一覧表";
    let btnLabel = "出力";

    createAndAddDialog(dialogId, dialogName,[
        { type: 'range-date', id: 'requestDate', label: '請求日付', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
            valueFrom:today,
            valueTo:today
        }},
        { type: 'range-text', id: 'custmerCode', label: '得意先CD', options: {
            width: 'wide',
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'custmerNoSearch',title:'見積番号検索'}
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'create', label: btnLabel, options: {
            icon:'disk',
            onclick:`requestParticularsListDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)});

}

/**
 *
 * @param {string} dialogId     ダイアログID
 * @param {string} category     カテゴリー
 * @param {string} dialogName   ダイアログ名
 * @param {string} btnLabel     ボタンラベル
 * @returns
 */
async function requestParticularsListDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 請求明細一覧表');

        let item_params = {
            "@iS集計日付":SpcToNull($('#requestParticularsListDialog_requestDateFrom').val().replaceAll('-','/')),
            "@iE集計日付":SpcToNull($('#requestParticularsListDialog_requestDateTo').val().replaceAll('-','/')),
            "@iS得意先CD":SpcToNull($('#requestParticularsListDialog_custmerCodeFrom').val()),
            "@iE得意先CD":SpcToNull($('#requestParticularsListDialog_custmerCodeTo').val())
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "請求明細一覧表" + SpcToNull($('#requestParticularsListDialog_requestDateFrom').val().replaceAll('-','')).slice(2) + "-" + SpcToNull($('#requestParticularsListDialog_requestDateTo').val().replaceAll('-','')).slice(2) + ".xlsx"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 請求明細一覧表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}
