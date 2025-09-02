//支払予定表出力
{

    let category = "仕入・支払";
    let dialogId = "PaymentScheduleDialog";
    let dialogName = "支払予定表";
    let btnLabel = "印刷";

    const firstDate = getFirstDateOfScope(new Date(),99);
    const lastDate = getLastDateOfScope(new Date(),99);

    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'SupplyDate', label: '支払日付', options: {
            width: 'wide',
            required:true,
            valueFrom:firstDate,
            valueTo:lastDate,
            varidate:{type:'str'},
        }},
        { type: 'range-text', id: 'Supplier', label: '仕入先CD', options: {
            width: 'wide',
            search:true,
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'output', label: btnLabel, options: {
            icon:'disk',
            onclick:`PaymentSchedule_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function PaymentSchedule_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 支払予定表');

        let item_params = {
            "@iS支払予定日付":SpcToNull($('#PaymentScheduleDialog_SupplyDateFrom').val().replaceAll('-','/')),
            "@iE支払予定日付":SpcToNull($('#PaymentScheduleDialog_SupplyDateTo').val().replaceAll('-','/')),
            "@iS仕入先CD":SpcToNull($('#PaymentScheduleDialog_SupplierFrom').val()),
            "@iE仕入先CD":SpcToNull($('#PaymentScheduleDialog_SupplierTo').val())
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };


        const date_from = $('#PaymentScheduleDialog_SupplyDateFrom').val().replaceAll('-','/').substring(2);
        const date_to = $('#PaymentScheduleDialog_SupplyDateTo').val().replaceAll('-','/').substring(2);
        let filename = `支払予定表${date_from}-${date_to}.xlsx`;

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 支払予定表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}