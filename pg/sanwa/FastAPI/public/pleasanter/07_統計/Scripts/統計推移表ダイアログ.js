{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM')
    let category = "統計";
    let dialogId = "StatisticalTrendsDialog";
    let dialogName = "統計推移表";
    let btnLabel = "出力";

    //統計推移表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'AggregateYear', label: '集計年', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            format:"month"
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '出力', options: {
            icon:'disk',
            onclick:`StatisticalTrends_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function StatisticalTrends_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 統計推移表');
        
        let AggregateYear_last_date = getLastDateOfScope(SpcToNull($('#StatisticalTrendsDialog_AggregateYearTo').val()),'99','');

        let item_params = {
            "@iS集計日付":SpcToNull($('#StatisticalTrendsDialog_AggregateYearFrom').val().replaceAll('-','/')) + '/01',
            "@iE集計日付":AggregateYear_last_date.replaceAll('-','/')
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "統計推移表" + SpcToNull($('#StatisticalTrendsDialog_AggregateYearFrom').val().replaceAll('-','')).slice(2,6) + "-" + SpcToNull($('#StatisticalTrendsDialog_AggregateYearTo').val().replaceAll('-','')).slice(2,6) + ".xlsx"

        let assept = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';

        try{
            await download_report(param,filename,assept);
        }
        catch(err){
            return;
        }

        console.log('end : 統計推移表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }
    
}