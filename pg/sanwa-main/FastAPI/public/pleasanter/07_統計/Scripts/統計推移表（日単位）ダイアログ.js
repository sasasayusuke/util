{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let category = "統計";
    let dialogId = "dailyStatisticalTrendsDialog";
    let dialogName = "統計推移表（日単位）";
    let btnLabel = "出力";

    //統計推移表（日単位）
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'AggregateDate', label: '集計日', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            required:true,
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '出力', options: {
            icon:'disk',
            onclick:`dailyStatisticalTrends_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function dailyStatisticalTrends_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 統計推移表（日単位）');

        let item_params = {
            "@iS集計日付":SpcToNull($('#dailyStatisticalTrendsDialog_AggregateDateFrom').val().replaceAll('-','/')),
            "@iE集計日付":SpcToNull($('#dailyStatisticalTrendsDialog_AggregateDateTo').val().replaceAll('-','/'))
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "統計推移表_日単位" + SpcToNull($('#dailyStatisticalTrendsDialog_AggregateDateFrom').val().replaceAll('-','')).slice(2,8) + "-" + SpcToNull($('#dailyStatisticalTrendsDialog_AggregateDateTo').val().replaceAll('-','')).slice(2,8) + ".xlsx"

        let assept = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';

        try{
            await download_report(param,filename,assept);
        }
        catch(err){
            return;
        }

        console.log('end : 統計推移表(日単位)');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }
    
}