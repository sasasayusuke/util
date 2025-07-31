{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM')

    let category = "統計";
    let dialogId = "TeamSpecificStackChecksDialog";
    let dialogName = "チーム別積上チェック表";
    let btnLabel = "出力";

    //チーム別積上チェック表
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'AggregateDate', label: '集計年', options: {
            width: 'wide',
            format:'month',
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            required:true,
        }},
        { type: 'radio', id: 'PropertyAggregation', label: '物件集計', options: {
            width: 'wide',
            required:true,
            values:[
                {value:'物件集計',text:'物件集計',checked:true},
                {value:'個別',text:'個別'},
            ]
        }},
        { type: 'text-set', id: 'IndustryClassification', label: '業種区分', options: {
            type:"number",
            width: 'wide',
            // required:true,
            values:[
                {value:'0',text:'什器'},
                {value:'1',text:'内装'},
            ]
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '出力', options: {
            icon:'disk',
            onclick:`TeamSpecificStackChecksDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function TeamSpecificStackChecksDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        if ($(`#${dialogId}_PropertyAggregationField input:checked`).val() == '物件集計')  {
            syukei = "物件集計"
        } else {
            syukei = "個別集計"
        }

        let fromDt = new Date($(`#${dialogId}_AggregateDateFrom`).val());
        let toDt   = new Date($(`#${dialogId}_AggregateDateTo`).val());


        if(fromDt > toDt){
            alert('集計年が不正です。');
            $(`#${dialogId}_AggregateDateFrom`).focus();
            return;
        }

        // 範囲チェック（2年以内）
        if(DateDiff(fromDt,toDt) >= 24){
            alert('範囲が大きすぎます。（最大24ヶ月）');
        $(`#${dialogId}_AggregateDateFrom`).focus();
            return;
        }

        let param_fromDt = getFirstDateOfScope(fromDt, 99, fromDt);
        let param_toDt = getLastDateOfScope(toDt, 99, fromDt);

        let item_params = {
            "@iS集計日付":SpcToNull(param_fromDt.replaceAll('-','/')),
            "@iE集計日付":SpcToNull(param_toDt.replaceAll('-','/')),
            "@i業種区分":SpcToNull($(`#${dialogId}_IndustryClassificationFrom`).val()),
            "@i物件集計":syukei
        };


        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };


        let filename = dialogName + "作成" + $(`#${dialogId}_AggregateDateFrom`).val().replaceAll("-","").slice(2) + "-" + $(`#${dialogId}_AggregateDateTo`).val().replaceAll("-","").slice(2) + ".xlsx"

        try{
            await download_report(param,filename);
        }catch(e){
            return
        }

        console.log(`end : ${dialogName}`);


    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}