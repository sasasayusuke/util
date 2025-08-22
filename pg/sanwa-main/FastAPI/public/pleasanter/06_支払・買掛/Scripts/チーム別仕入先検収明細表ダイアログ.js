{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')

    let dialogId = "SupplierInspectionSchedulebyTeamDialog";
    let dialogName = "仕入先検収明細表";
    let category = "支払・買掛";
    let btnLabel = "出力";
    // 仕入先検収明細表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'purchaseDate', label: '仕入日付', options: {
            width: 'wide',
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            required:true,
        }},
        { type: 'range-text', id: 'supplier', label: '仕入先CD', options: {
            width: 'wide',
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
        }},
    ],
    [
        { type: 'button_inline', id: 'create', label: btnLabel, options: {
            icon:'disk',
            onclick:`SupplierInspectionSchedulebyTeamDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function SupplierInspectionSchedulebyTeamDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 仕入先検収明細表');

        let item_params = {
            "@iS集計日付":SpcToNull($('#SupplierInspectionSchedulebyTeamDialog_purchaseDateFrom').val().replaceAll('-','/')),
            "@iE集計日付":SpcToNull($('#SupplierInspectionSchedulebyTeamDialog_purchaseDateTo').val().replaceAll('-','/')),
            "@iS仕入先CD":SpcToNull($('#SupplierInspectionSchedulebyTeamDialog_supplierFrom').val()),
            "@iE仕入先CD":SpcToNull($('#SupplierInspectionSchedulebyTeamDialog_supplierTo').val())
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "仕入先検収明細表" + SpcToNull($('#SupplierInspectionSchedulebyTeamDialog_purchaseDateFrom').val().replaceAll('-','')).slice(2,8) + "-" + SpcToNull($('#SupplierInspectionSchedulebyTeamDialog_purchaseDateTo').val().replaceAll('-','')).slice(2,8) + ".xlsx"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 仕入先検収明細表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}