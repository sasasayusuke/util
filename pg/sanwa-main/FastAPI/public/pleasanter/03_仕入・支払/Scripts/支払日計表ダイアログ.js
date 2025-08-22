//支払日計表出力
{

    let category = "仕入・支払";
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let dialogId = "PaymentDailyTotalDialog";
    let dialogName = "支払日計表";
    let btnLabel = "印刷";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'SupplyDate', label: '支払日付', options: {
            width: 'wide',
            valueFrom:today,
            valueTo:today,
            varidate:{type:'str'},
            required:true,
        }},
        { type: 'range-text', id: 'Supplier', label: '仕入先CD', options: {
            width: 'wide',
            varidate:{type:'zeroPadding',maxlength:4},
            search:true,
            searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
        }},
    ],
    [
        { type: 'button_inline', id: 'print', label: '印刷', options: {
            icon:'print',
            onclick:`PaymentDailyTotalDialog_Report('${dialogId}','${category}','${dialogName}','印刷', 'pdf');`
        }},
        { type: 'button_inline', id: 'output', label: '仕訳出力', options: {
            icon:'disk',
            onclick:`PaymentDailyTotalDialog_Report('${dialogId}','${category}','${dialogName}','仕訳出力', 'csv');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}


async function PaymentDailyTotalDialog_Report(dialogId,category,dialogName,btnLabel,type){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        let item_params = {
            "@is支払日付":SpcToNull($(`#${dialogId}_SupplyDateFrom`).val().replaceAll('-','/')),
            "@ie支払日付":SpcToNull($(`#${dialogId}_SupplyDateTo`).val().replaceAll('-','/')),
            "@is仕入先CD":SpcToNull($(`#${dialogId}_SupplierFrom`).val()),
            "@ie仕入先CD":SpcToNull($(`#${dialogId}_SupplierTo`).val()),
            "@type":"pdf",
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            // "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };

        let filename = dialogName + $(`#${dialogId}_SupplyDateFrom`).val().replaceAll("-","").substring(2) + "-" + $(`#${dialogId}_SupplyDateTo`).val().replaceAll("-","").substring(2) + "." + type;
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
