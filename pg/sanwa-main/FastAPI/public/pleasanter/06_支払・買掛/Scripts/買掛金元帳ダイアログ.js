// 買掛金元帳
{
    const cls_dates = new clsDates('買掛月次更新日');
    new Promise(async (resolve,reject) =>{
        cls_dates.GetbyID().then(() => resolve()).catch(err => reject(err));
    })
    .then(e =>{
        let tmp = new Date(cls_dates.updateDate);
        tmp.setMonth(tmp.getMonth() + 1,1);
        tmp = tmp.toLocaleDateString('sv-SE').slice(0,7);
        let category = "支払・買掛";
        let dialogId = "accountsPayableLedgerDialog";
        let dialogName = "買掛金元帳";
        let btnLabel = "印刷";
        createAndAddDialog(dialogId, dialogName, [
            { type: 'datepicker', id: 'purchaseDate', label: '仕入日付', options: {
                width: 'normal',
                required:true,
                varidate:{type:'str'},
                format:"month",
                value:tmp
            }},
            { type: 'number', id: 'deadLine', label: '締日', options: {
                width: 'normal',
                required:true,
                value:99,
                varidate:{type:'int',maxlength:2},
            }},
            { type: 'range-text', id: 'supplier', label: '仕入先範囲', options: {
                width: 'wide',
                varidate:{type:'zeroPadding',maxlength:4},
                digitsNum:4,
                searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
            }},
            { type: 'text-set', id: 'issueClassification', label: '発行区分', options: {
                type:"number",
                width: 'wide',
                required:true,
                values:[
                    {value:'0',text:'発行する'},
                    {value:'1',text:'発行しない'},
                ],
                valueFrom:"0",
            }},
        ],
        [
            { type: 'button_inline', id: 'print', label: '印刷', options: {
                icon:'print',
                onclick:`accountsPayableLedgerDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
            }},
            { type: 'button_inline', id: 'check', label: '確認', options: {
                icon:'disk',
                onclick:"openDialog_for_searchDialog('kaikakeKakuninSearch','',1000,'請求確認画面');"
            }},
        ]
        );
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
async function accountsPayableLedgerDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 買掛金元帳');

        var requestYM = SpcToNull($('#accountsPayableLedgerDialog_purchaseDate').val())
        var requestD = SpcToNull($('#accountsPayableLedgerDialog_deadLine').val())
        let requestYMD = getLastDateOfScope(requestYM,requestD,'');
        let st_date = getFirstDateOfScope(requestYM,requestD,'');

        let item_params = {
            "@iSDate":st_date.replaceAll('-','/'),
            "@iEDate":requestYMD.replaceAll('-','/'),
            "@is仕入先CD":SpcToNull($('#accountsPayableLedgerDialog_supplierFrom').val()),
            "@ie仕入先CD":SpcToNull($('#accountsPayableLedgerDialog_supplierTo').val()),
            "@i締日":SpcToNull($('#accountsPayableLedgerDialog_deadLine').val()),
            "@iDATE":SpcToNull(requestYMD).replaceAll('-','/'),
            "@発行区分":$('#accountsPayableLedgerDialog_issueClassificationFrom').val()
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "買掛金元帳_" + requestYMD.replaceAll('-','').slice(2, 8) + ".pdf"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 買掛金元帳');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }
}
