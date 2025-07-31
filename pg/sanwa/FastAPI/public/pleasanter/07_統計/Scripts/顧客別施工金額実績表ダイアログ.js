{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM')

    let category = "統計";
    let dialogId = "ConstructionValueByClientDialog";
    let dialogName = "顧客別施工金額実績表";
    let btnLabel = "出力";

    //顧客別施工金額実績表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'aggregationYear', label: '集計年', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            format:"month"
        }},
        { type: 'range-text', id: 'clientCD', label: '顧客指定', options: {
            width: 'wide',
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'custmerNoSearch',title:'得意先検索',}
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '出力', options: {
            icon:'disk',
            onclick:`DownloadConstructionValueByClient('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}


async function DownloadConstructionValueByClient(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        // let storingDate = new Date($(`#${dialogId}_storing_date`).val());
        // let requestDateTo   = new Date($(`#${dialogId}_salesDateTo`).val());
        let fromDt = new Date($(`#${dialogId}_aggregationYearFrom`).val());
        let toDt   = new Date($(`#${dialogId}_aggregationYearTo`).val());

        // const formatDate = (date) => {
        //     const year = date.getFullYear();
        //     const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0から始まるので+1
        //     const day = String(date.getDate()).padStart(2, '0'); // 日を2桁に
        //     return `${year}/${month}/${day}`;
        // };

        let param_fromDt = getFirstDateOfScope(fromDt, 99, fromDt);
        let param_toDt = getLastDateOfScope(toDt, 99, fromDt);

        let item_params = {
            // "@i入庫日":SpcToNull(formatDate(storingDate)),
            "@iS集計日付":SpcToNull(param_fromDt.replaceAll('-','/')),
            "@iE集計日付":SpcToNull(param_toDt.replaceAll('-','/')),
            "@iS得意先CD":SpcToNull($(`#${dialogId}_clientCDFrom`).val()),
            "@iE得意先CD":SpcToNull($(`#${dialogId}_clientCDTo`).val()),
            // "AccountUserName":$("#AccountUserName").text(),
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };

        let filename = "顧客別施工金額実績表" + $(`#${dialogId}_aggregationYearFrom`).val().replaceAll("-","").slice(2) + "-" + $(`#${dialogId}_aggregationYearTo`).val().replaceAll("-","").slice(2) + ".xlsx"

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