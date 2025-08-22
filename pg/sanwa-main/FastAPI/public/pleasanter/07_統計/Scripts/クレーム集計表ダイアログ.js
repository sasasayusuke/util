//クレーム集計表
{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM')

    const category = '統計';
    const dialogId = "ClaimsAggregationDialog";
    const dialogName = "クレーム集計表";
    const btnLabel = '出力';


    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'aggregationYear', label: '集計年月', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
            format:"month",
            valueFrom:monthDateGein(1,'month'),
            valueTo:monthDateGein(99,'month')
        }},
        { type: 'range-text', id: 'clientCD', label: '顧客指定', options: {
            width: 'wide',
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'custmerNoSearch',title:'得意先検索',}
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: btnLabel, options: {
            icon:'disk',
            onclick:`ClaimsAggregation('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)});
}


async function ClaimsAggregation(dialogId,category,dialogName,btnLabel){
    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        let fromDt = new Date($('#ClaimsAggregationDialog_aggregationYearFrom').val());
        let toDt   = new Date($('#ClaimsAggregationDialog_aggregationYearTo').val());

        if(fromDt > toDt){
            alert('集計年が不正です。');
            $('#ClaimsAggregationDialog_aggregationYearFrom').focus();
            return;
        }

        // 範囲チェック（2年以内）
        if(DateDiff(fromDt,toDt) >= 24){
            alert('範囲が大きすぎます。（最大24ヶ月）');
        $('#ClaimsAggregationDialog_aggregationYearFrom').focus();
            return;
        }

        console.log('start : クレーム集計表');

        let item_params = {
            "@iS集計日付":getFirstDateOfScope(fromDt,99,fromDt),
            "@iE集計日付":getLastDateOfScope(toDt,99,toDt),
            "@iS得意先CD":SpcToNull($('#ClaimsAggregationDialog_clientCDFrom').val()),
            "@iE得意先CD":SpcToNull($('#ClaimsAggregationDialog_clientCDTo').val()),
        }

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = `クレーム集計表作成${fromDt.toLocaleString('sv-SE').slice(2,7).replace('-','')}-${toDt.toLocaleString('sv-SE').slice(2,7).replace('-','')}.xlsx`;

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : クレーム集計表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }
}