//仕入日計表出力
{

    let category = "仕入・支払";
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let dialogId = "PurchaseDailyTotalDialog";
    let dialogName = "仕入日計表";
    let btnLabel = "印刷";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'radio', id: 'DateSelect', label: '日付選択', options: {
            values: [
                { value: '1', text: '仕入日付', checked: true },
                { value: '2', text: '支払日付' }
            ],
            width: 'wide'
        }},
        { type: 'range-date', id: 'SupplyDate', label: '仕入日付', options: {
            width: 'wide',
            valueFrom:today,
            valueTo:today,
            varidate:{type:'str'},
            required:true,
        }},
        { type: 'range-date', id: 'PaymentDate', label: '支払日付', options: {
            width: 'wide',
            valueFrom:today,
            valueTo:today,
            varidate:{type:'str'},
            required:true,
        }},
        { type: 'range-text', id: 'Supplier', label: '仕入先CD', options: {
            width: 'wide',
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            search:true,
            searchDialog:{id:'supplierSearch',title:'納入先CD検索',multiple:false,focusTarget:false}
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '印刷', options: {
            icon:'print',
            onclick:`PurchaseDailyTotalDialog_Report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})

    // 項目の表示変更
    const supplyDateElem = document.getElementById(`${dialogId}_SupplyDateField`);
    const paymentDateElem = document.getElementById(`${dialogId}_PaymentDateField`);
    paymentDateElem.style.display = 'none';
    document.querySelectorAll(`#${dialogId}_DateSelectField input`).forEach(input => {
        input.addEventListener('change', function(event) {
            if(event.target.value === '1') {
                supplyDateElem.style.display = 'block';
                paymentDateElem.style.display = 'none';
            } else {
                supplyDateElem.style.display = 'none';
                paymentDateElem.style.display = 'block';
            }
        });
    });

}

async function PurchaseDailyTotalDialog_Report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        let checkedValue = $(`input[name="${dialogId}_DateSelect"]:checked`).val();
        console.log(checkedValue)

        let item_params = {
            "@iDateFrom":SpcToNull($(`#${dialogId}_SupplyDateFrom`).val().replaceAll('-','/')),
            "@iDateTo":SpcToNull($(`#${dialogId}_SupplyDateTo`).val().replaceAll('-','/')),
            "@is仕入先CD":SpcToNull($(`#${dialogId}_SupplierFrom`).val()),
            "@ie仕入先CD":SpcToNull($(`#${dialogId}_SupplierTo`).val()),
            "@条件":checkedValue,
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

        let filename = dialogName + $(`#${dialogId}_SupplyDateFrom`).val().replaceAll("-","").substring(2) + "-" + $(`#${dialogId}_SupplyDateTo`).val().replaceAll("-","").substring(2) + ".pdf"
        try{
            await download_report(param,filename);
        }catch(e){
            return;
        }
        

        console.log(`end : ${dialogName}`);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}
