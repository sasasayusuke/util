// マミーマート請求明細表
{

    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let category = "請求・売掛";
    let dialogId = "MAMIMARTitemizedBillingListDialog";
    let dialogName = "マミーマート請求明細表";
    let btnLabel = "出力";
    createAndAddDialog(dialogId, dialogName,[
        { type: 'datepicker', id: 'requestDate', label: '請求日付', options: {
            width: 'normal',
            required:true,
            varidate:{type:'str'},
            format:"month",
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
        { type: 'text', id: 'orderPerson', label: '発注担当者', options: {
            width: 'wide',
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'create', label: btnLabel, options: {
            icon:'disk',
            onclick:`MAMIMARTitemizedBillingListDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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
async function MAMIMARTitemizedBillingListDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : マミーマート請求明細表');

        let item_params = {
            "@iS請求日付":SpcToNull($('#MAMIMARTitemizedBillingListDialog_requestPeriodFrom').val().replaceAll('-','/')),
            "@iE請求日付":SpcToNull($('#MAMIMARTitemizedBillingListDialog_requestPeriodTo').val().replaceAll('-','/')),
            "@i発注担当者名":SpcToNull($('#MAMIMARTitemizedBillingListDialog_orderPerson').val())
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "請求書_MM_" + SpcToNull($('#MAMIMARTitemizedBillingListDialog_requestPeriodTo').val().replaceAll('-','')).slice(2, 8) + ".xlsx"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : マミーマート請求明細表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}


$(document).on('blur','#MAMIMARTitemizedBillingListDialog_deadLine',function(){
    var requestYM = SpcToNull($('#MAMIMARTitemizedBillingListDialog_requestDate').val())
    var requestD = SpcToNull($('#MAMIMARTitemizedBillingListDialog_deadLine').val())

    let element1 = document.getElementById('MAMIMARTitemizedBillingListDialog_requestPeriodFrom');
    element1.value = getFirstDateOfScope(requestYM,requestD,'');
    let element2 = document.getElementById('MAMIMARTitemizedBillingListDialog_requestPeriodTo');
    element2.value = getLastDateOfScope(requestYM,requestD,'');
})

$(document).on('blur','#MAMIMARTitemizedBillingListDialog_requestDate',function(){
    var requestYM = SpcToNull($('#MAMIMARTitemizedBillingListDialog_requestDate').val())
    var requestD = SpcToNull($('#MAMIMARTitemizedBillingListDialog_deadLine').val())

    if(requestD != null){
        let element1 = document.getElementById('MAMIMARTitemizedBillingListDialog_requestPeriodFrom');
        element1.value = getFirstDateOfScope(requestYM,requestD,'');
        let element2 = document.getElementById('MAMIMARTitemizedBillingListDialog_requestPeriodTo');
        element2.value = getLastDateOfScope(requestYM,requestD,'');
    }
})
