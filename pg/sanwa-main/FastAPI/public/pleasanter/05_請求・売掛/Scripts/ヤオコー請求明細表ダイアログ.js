//ヤオコー請求明細表
{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let category = "請求・売掛";
    let dialogId = "YAOKOitemizedBillingListDialog";
    let dialogName = "ヤオコー請求明細表";
    let btnLabel = "出力";

    createAndAddDialog(dialogId, dialogName,[
        { type: 'datepicker', id: 'requestDate', label: '請求日付', options: {
            width: 'normal',
            required:true,
            varidate:{type:'str'},
            format:"month"
        }},
        { type: 'number', id: 'deadLine', label: '締日', options: {
            width: 'normal',
            required:true,
            varidate:{type:'int',maxlength:2},
        }},
        { type: 'range-date', id: 'requestPeriod', label: '請求期間', options: {
            width: 'wide',
            disabled:true,
        }},
        { type: 'text-set', id: 'supplySegment', label: 'サプライ区分', options: {
            width: 'wide',
            valueFrom:'1',
            type:"number",
            required:true,
            values:[
                {value:'1',text:'サプライ'},
                {value:'2',text:'システム'},
            ]
        }},
        { type: 'text-set', id: 'propertySegment', label: '物件区分', options: {
            width: 'wide',
            valueFrom:'1',
            type:"number",
            required:true,
            values:[
                {value:'1',text:'物件'},
                {value:'2',text:'メンテ・製品'},
                {value:'3',text:'担当者案件'},
            ]
        }},
        { type: 'datepicker', id: 'issueDate', label: '発行日付', options: {
            width: 'normal',
            varidate:{type:'str'},
            required:true,
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'create', label: btnLabel, options: {
            icon:'disk',
            onclick:`YAOKOitemizedBillingListDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)});
}

/**
 *
 * @param {string} dialogId     ダイアログID
 * @param {string} category     カテゴリー
 * @param {string} dialogName   ダイアログ名
 * @param {string} btnLabel     ボタンラベル
 * @returns
 */
async function YAOKOitemizedBillingListDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : ヤオコー請求明細表');

        let item_params = {
            "@iS請求日付":SpcToNull($('#YAOKOitemizedBillingListDialog_requestPeriodFrom').val().replaceAll('-','/')),
            "@iE請求日付":SpcToNull($('#YAOKOitemizedBillingListDialog_requestPeriodTo').val().replaceAll('-','/')),
            "@iYKサプライ区分":SpcToNull($('#YAOKOitemizedBillingListDialog_supplySegmentFrom').val()),
            "@iYK物件区分":SpcToNull($('#YAOKOitemizedBillingListDialog_propertySegmentFrom').val()),
            "@i発行日付":SpcToNull($('#YAOKOitemizedBillingListDialog_issueDate').val().replaceAll('-','/'))
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };

        let filename = "請求書_YK-" + SpcToNull($('#YAOKOitemizedBillingListDialog_supplySegmentFrom').val()) + "-" + SpcToNull($('#YAOKOitemizedBillingListDialog_propertySegmentFrom').val()) + "-" + SpcToNull($('#YAOKOitemizedBillingListDialog_requestPeriodTo').val().replaceAll('-','')).slice(2)+ ".xlsx"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : ヤオコー請求明細表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}

$(document).on('blur','#YAOKOitemizedBillingListDialog_deadLine',function(){
    let date = SpcToNull($('#YAOKOitemizedBillingListDialog_requestDate').val())
    let deadLine = SpcToNull($('#YAOKOitemizedBillingListDialog_deadLine').val())

    let fromDt = getFirstDateOfScope(date,deadLine,'');
    let toDt = getLastDateOfScope(date,deadLine,'');

    $('#YAOKOitemizedBillingListDialog_requestPeriodFrom').val(fromDt);
    $('#YAOKOitemizedBillingListDialog_requestPeriodTo').val(toDt);
})

