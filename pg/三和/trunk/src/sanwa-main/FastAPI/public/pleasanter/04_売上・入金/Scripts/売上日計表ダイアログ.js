// 売上日計表作成
{

    let category = "売上・入金";
    let dialogId = "dailySalesLedgerDialog";
    let dialogName = "売上日計表";
    let btnLabel = "印刷";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'radio', id: 'selectDate', label: '日付選択', options: {
            values: [
                { value: '1', text: '売上日付',checked:true },
                { value: '2', text: '請求日付' }
            ],
            width: 'wide'
        }},
        { type: 'range-date', id: 'salesDate', label: '売上日付', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
        }},
        { type: 'range-text', id: 'custmerCode', label: '得意先CD', options: {
            width: 'wide',
            search:true,
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'custmerNoSearch',title:'得意先検索'}
        }},
    ],
    [
        { type: 'button_inline', id: 'print', label: '印刷', options: {
            icon:'print',
            onclick:`downloadDailySalesLedger('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})

}

$(document).on('change','#dailySalesLedgerDialog_selectDateField input',function(){

    if($(this).val() == 1){
        $('#dailySalesLedgerDialog_salesDateField .field-label label').text('売上日付');
    }else{
        $('#dailySalesLedgerDialog_salesDateField .field-label label').text('請求日付');
    }
})


async function downloadDailySalesLedger(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        let requestDateFrom = new Date($(`#${dialogId}_salesDateFrom`).val());
        let requestDateTo   = new Date($(`#${dialogId}_salesDateTo`).val());

        const formatDate = (date) => {
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0から始まるので+1
            const day = String(date.getDate()).padStart(2, '0'); // 日を2桁に
            return `${year}/${month}/${day}`;
        };

        let checkedValue = $(`input[name="${dialogId}_selectDate"]:checked`).val();

        let item_params = {
            "@iDateFrom":SpcToNull(formatDate(requestDateFrom)),
            "@iDateTo":SpcToNull(formatDate(requestDateTo)),
            "@is得意先CD":SpcToNull($(`#${dialogId}_custmerCodeFrom`).val()),
            "@ie得意先CD":SpcToNull($(`#${dialogId}_custmerCodeTo`).val()),
            "@条件":$(`input[name="${dialogId}_selectDate"]:checked`).val(),
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

        let filename = dialogName + $(`#${dialogId}_salesDateFrom`).val().replaceAll("-","").substring(2) + "-" + $(`#${dialogId}_salesDateTo`).val().replaceAll("-","").substring(2) + ".pdf"

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
