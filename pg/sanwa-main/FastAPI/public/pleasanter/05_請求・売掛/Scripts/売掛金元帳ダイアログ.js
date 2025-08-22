// 売掛金元帳
{
    const cls_dates = new clsDates('売掛月次更新日');
    new Promise((resolve,reject) =>{
        cls_dates.GetbyID().then(() => resolve()).catch(err => reject(err));
    })
    .then(e =>{
        let tmp = new Date(cls_dates.updateDate);
        tmp.setMonth(tmp.getMonth() + 1,1);
        tmp = tmp.toLocaleDateString('sv-SE').slice(0,7);
        let category = "請求・売掛";
        let dialogId = "accountsReceivableDialog";
        let dialogName = "売掛金元帳";
        let btnLabel = "印刷";
        createAndAddDialog(dialogId, dialogName, [
            { type: 'datepicker', id: 'requestDate', label: '請求日付', options: {
                width: 'normal',
                value: tmp,
                required:true,
                format:"month",
                varidate:{type:'str'},
            }},
            { type: 'number', id: 'deadLine', label: '締日', options: {
                width: 'normal',
                required:true,

            }},
            { type: 'range-date', id: 'period', label: '請求期間', options: {
                width: 'wide',
                disabled:true,
            }},
            { type: 'range-text', id: 'clientRange', label: '得意先範囲', options: {
                width: 'wide',
                search:true,
                varidate:{type:'zeroPadding',maxlength:4},
                searchDialog:{id:'custmerNoSearch',title:'得意先検索',},
                
            }},
            { type: 'text-set', id: 'issueCategory', label: '発行区分', options: {
                width: 'wide',
                valueFrom: '0',
                type:"number",
                values:[
                    { value: '0', text: '発行する' },
                    { value: '1', text: '発行しない' }
                ]
            }},
        ],
        [
            { type: 'button_inline', id: 'print', label: '印刷', options: {
                icon:'print',
                onclick:`accountsReceivableDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
            }},
            { type: 'button_inline', id: 'check', label: '確認', options: {
                icon:'print',
                onclick:`openDialog_for_searchDialog('urikakeKakuninSearch','',1000,'請求確認画面');`

            }},
        ]);
        // ダイアログを開くボタンを追加
        commonModifyLink(dialogName, function() {openDialog(dialogId)})

    })
    .catch(err =>{
        console.error(err);
    })
}


/**
 *
 * @param {string} dialogId     ダイアログID
 * @param {string} category     カテゴリー
 * @param {string} dialogName   ダイアログ名
 * @param {string} btnLabel     ボタンラベル
 * @returns
 */
async function accountsReceivableDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 売掛金元帳');

        var requestYM = SpcToNull($('#accountsReceivableDialog_requestDate').val())
        var requestD = SpcToNull($('#accountsReceivableDialog_deadLine').val())
        let requestYMD = getLastDateOfScope(requestYM,requestD,'');

        let item_params = {
            "@iS請求日付":SpcToNull($('#accountsReceivableDialog_periodFrom').val().replaceAll('-','/')),
            "@iE請求日付":SpcToNull($('#accountsReceivableDialog_periodTo').val().replaceAll('-','/')),
            "@is得意先CD":SpcToNull($('#accountsReceivableDialog_clientRangeFrom').val()),
            "@ie得意先CD":SpcToNull($('#accountsReceivableDialog_clientRangeTo').val()),
            "@i締日":SpcToNull($('#accountsReceivableDialog_deadLine').val()),
            "@iDATE":SpcToNull(requestYMD).replaceAll('-','/'),
            "@発行区分":$('#accountsReceivableDialog_issueCategoryFrom').val()
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "売掛金元帳_" + SpcToNull($('#accountsReceivableDialog_periodFrom').val().replaceAll('-','')).slice(2, 8) + ".pdf"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 売掛金元帳');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}


$(document).on('blur','#accountsReceivableDialog_deadLine',function(){
    var requestYM = SpcToNull($('#accountsReceivableDialog_requestDate').val())
    var requestD = SpcToNull($('#accountsReceivableDialog_deadLine').val())
    console.log(requestYM)
    console.log(requestD)

    let element1 = document.getElementById('accountsReceivableDialog_periodFrom');
    element1.value = getFirstDateOfScope(requestYM,requestD,'');
    let element2 = document.getElementById('accountsReceivableDialog_periodTo');
    element2.value = getLastDateOfScope(requestYM,requestD,'');
})
