//買掛金集計表
{
    const cls_dates = new clsDates('買掛月次更新日');
    new Promise(async (resolve,reject) =>{
        cls_dates.GetbyID().then(() => resolve()).catch(err => reject(err));
    })
    .then(e =>{
        let tmp = new Date(cls_dates.updateDate);
        tmp.setMonth(tmp.getMonth() + 1,1);
        tmp = tmp.toLocaleDateString('sv-SE').slice(0,7);
        var requestYM = tmp
        var requestD = '99'
        requestYM = requestYM.slice(0, 5) + ( '00' + (Number(requestYM.slice(5, 7))) ).slice( -2 )
        var setDate = getLastDateOfScope(requestYM,requestD,'');
        let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM')
        let category = "支払・買掛";
        let dialogId = "TabulationOfAccountsPayableDialog";
        let dialogName = "買掛金集計表";
        let btnLabel = "印刷";
        createAndAddDialog(dialogId, dialogName, [
            { type: 'datepicker', id: 'requestDate', label: '仕入年月', options: {
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
                disabled:true
            }},
            { type: 'range-text', id: 'Supplier', label: '仕入先CD', options: {
                width: 'wide',
                digitsNum:4,
                varidate:{type:'zeroPadding',maxlength:4},
                searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
            }},
            { type: 'datepicker', id: 'classificationDate', label: '仕訳日付', options: {
                width: 'normal',
                required:true,
                value:setDate
            }},
        ],
        [
            { type: 'button_inline', id: 'print', label: '印刷', options: {
                icon:'print',
                onclick:`TabulationOfAccountsPayableDialog_report('${dialogId}','${category}','${dialogName}','印刷', 'pdf');`
            }},
            { type: 'button_inline', id: 'output', label: '仕訳出力', options: {
                icon:'disk',
                onclick:`TabulationOfAccountsPayableDialog_report('${dialogId}','${category}','${dialogName}','仕訳出力', 'csv');`
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
async function TabulationOfAccountsPayableDialog_report(dialogId,category,dialogName,btnLabel,type){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 買掛金集計表');

        if (btnLabel == '印刷'){
            if (!confirm("指定された範囲で印刷します。")) {
                return;
            }
        } else {
            if (!confirm("指定された範囲で出力します。\r(締め処理も行います。)")) {
                return;
            }
        }

        let requestYMD = SpcToNull($('#TabulationOfAccountsPayableDialog_requestDate').val()) + '-01'

        let item_params = {
            "@iDATE":requestYMD.replaceAll('-','/'),
            "@i開始仕入先":SpcToNull($('#TabulationOfAccountsPayableDialog_SupplierFrom').val()),
            "@i終了仕入先":SpcToNull($('#TabulationOfAccountsPayableDialog_SupplierTo').val()),
            "@i締日":SpcToNull($('#TabulationOfAccountsPayableDialog_deadLine').val()),
            "@i仕訳日付":SpcToNull($('#TabulationOfAccountsPayableDialog_classificationDate').val().replaceAll('-','/'))
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "買掛金集計表_" + requestYMD.replaceAll('-','').slice(2, 8) + "." + type

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 買掛金集計表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}
