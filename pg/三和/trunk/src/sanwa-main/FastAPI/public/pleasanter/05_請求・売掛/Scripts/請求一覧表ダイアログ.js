
{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let category = "請求・売掛";
    let dialogId = "RequestListDialog";
    let dialogName = "請求一覧表";
    let btnLabel = "印刷";

    // 請求一覧表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'datepicker', id: 'requestDate', label: '請求年月', options: {
            width: 'normal',
            value: today,
            varidate:{type:'str'},
            required:true
        }},
        { type: 'range-text', id: 'custmerCode', label: '得意先CD', options: {
            width: 'wide',
            varidate:{type:'zeroPadding',maxlength:4},
            search:true,
            searchDialog:{id:'custmerNoSearch',title:'見積番号検索'},
        }},
    ],
    [
        { type: 'button_inline', id: 'print', label: '印刷', options: {
            icon:'print',
            onclick:`RequestListDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})

}


async function RequestListDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        let requestDate = new Date($(`#${dialogId}_requestDate`).val());

        let item_params = {
            "@i請求日付":SpcToNull(formatDate(requestDate)),
            "@i開始得意先":SpcToNull($(`#${dialogId}_custmerCodeFrom`).val()),
            "@i終了得意先":SpcToNull($(`#${dialogId}_custmerCodeTo`).val()),
            "@type":"pdf",
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };

        let filename = "請求一覧表" + $(`#${dialogId}_requestDate`).val().replaceAll("-","") + ".pdf"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log(`end : ${dialogName}`);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}