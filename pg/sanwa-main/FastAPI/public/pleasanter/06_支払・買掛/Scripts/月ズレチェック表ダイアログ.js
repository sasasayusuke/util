{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM')

    let category = "支払・買掛";
    let dialogId = "MonthlyDiscrepancyCheckDialog";
    let dialogName = "月ズレチェック表";
    let btnLabel = "出力";

    //月ズレチェック表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'aggregationYear', label: '集計年', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            format:"month"
        }},
        { type: 'range-text', id: 'clientCD', label: '得意先CD', options: {
            width: 'wide',
            varidate:{type:'zeroPadding',maxlength:4},
            digitsNum:4,
            searchDialog:{id:'custmerNoSearch',title:'得意先検索',multiple:false}
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '出力', options: {
            icon:'print',
            onclick:`DownloadMonthlyDiscrepancyCheck('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}

async function DownloadMonthlyDiscrepancyCheck(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        let fromDt = new Date($(`#${dialogId}_aggregationYearFrom`).val());
        let toDt   = new Date($(`#${dialogId}_aggregationYearTo`).val());

        if(fromDt > toDt){
            alert('集計年が不正です。');
            $(`#${dialogId}_aggregationYearFrom`).focus();
            return;
        }

        // 範囲チェック（2年以内）
        if(DateDiff(fromDt,toDt) >= 24){
            alert('範囲が大きすぎます。（最大24ヶ月）');
        $(`#${dialogId}_aggregationYearFrom`).focus();
            return;
        }
    
        let param_fromDt = getFirstDateOfScope(fromDt, 99, fromDt);
        let param_toDt = getLastDateOfScope(toDt, 99, fromDt);

        let item_params = {
            "@iS集計日付":SpcToNull(param_fromDt.replaceAll('-','/')),
            "@iE集計日付":SpcToNull(param_toDt.replaceAll('-','/')),
            "@iS得意先CD":SpcToNull($(`#${dialogId}_clientCDFrom`).val()),
            "@iE得意先CD":SpcToNull($(`#${dialogId}_clientCDTo`).val()),
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };

        let filename = "チーム別月ズレチェック表" + $(`#${dialogId}_aggregationYearFrom`).val().replaceAll("-","") + "-" + $(`#${dialogId}_aggregationYearTo`).val().replaceAll("-","") + ".xlsx"

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