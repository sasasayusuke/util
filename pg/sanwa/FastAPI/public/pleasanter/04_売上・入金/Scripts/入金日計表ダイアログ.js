// 入金日計表作成
{

    let category = "売上・入金";
    let dialogId = "dailyDepositDialog";
    let dialogName = "入金日計表";
    let btnLabel = "印刷";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'depositDate', label: '入金日付', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
        }},
        { type: 'range-text', id: 'custmerCode', label: '得意先CD', options: {
            width: 'wide',
            search:true,
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'custmerNoSearch',title:'得意先検索'}
        }},
    ],
    [
        { type: 'button_inline', id: 'print', label: '印刷', options: {
            icon:'print',
            onclick:`downloadDailyDeposit('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]
    );
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})

}


async function downloadDailyDeposit(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        let requestDateFrom = new Date($(`#${dialogId}_depositDateFrom`).val());
        let requestDateTo   = new Date($(`#${dialogId}_depositDateTo`).val());

        const formatDate = (date) => {
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0から始まるので+1
            const day = String(date.getDate()).padStart(2, '0'); // 日を2桁に
            return `${year}/${month}/${day}`;
        };

        // let param_fromDt = getFirstDateOfScope(fromDt, 99, fromDt);
        // let param_toDt = getLastDateOfScope(toDt, 99, fromDt);

        let item_params = {
            "@iDateFrom":SpcToNull(formatDate(requestDateFrom)),
            "@iDateTo":SpcToNull(formatDate(requestDateTo)),
            "@is得意先CD":SpcToNull($(`#${dialogId}_custmerCodeFrom`).val()),
            "@ie得意先CD":SpcToNull($(`#${dialogId}_custmerCodeTo`).val()),
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

        let filename = dialogName + $(`#${dialogId}_depositDateFrom`).val().replaceAll("-","").substring(2) + "-" + $(`#${dialogId}_depositDateTo`).val().replaceAll("-","").substring(2) + ".pdf"

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
