
{

    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let category = "請求・売掛";
    let dialogId = "JOYTECrequestParticularsListDialog";
    let dialogName = "ジョイテック請求明細一覧表";
    let btnLabel = "出力";

    // ジョイテック請求明細一覧表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'requestDate', label: '請求日付', options: {
            width: 'wide',
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            required:true
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'create', label: btnLabel, options: {
            icon:'disk',
            onclick:`JOYTECrequestParticularsListDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function JOYTECrequestParticularsListDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : ジョイテック請求明細一覧表');

        let item_params = {
            "@iS集計日付":SpcToNull($('#JOYTECrequestParticularsListDialog_requestDateFrom').val().replaceAll('-','/')),
            "@iE集計日付":SpcToNull($('#JOYTECrequestParticularsListDialog_requestDateTo').val().replaceAll('-','/'))
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "B請求明細一覧表" + SpcToNull($('#JOYTECrequestParticularsListDialog_requestDateFrom').val().replaceAll('-','')).slice(2) + "-" + SpcToNull($('#JOYTECrequestParticularsListDialog_requestDateTo').val().replaceAll('-','')).slice(2) + ".xlsx"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : ジョイテック請求明細一覧表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}