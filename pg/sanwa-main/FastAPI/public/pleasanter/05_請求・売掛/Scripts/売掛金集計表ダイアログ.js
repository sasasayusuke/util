

{
    new Promise(async () =>{
        const query = 'SELECT MAX(TM売掛金額.売掛日付) AS 売掛日付 FROM TM売掛金額';
        var res = await fetchSql(query);
        res = res.results;
        max_date = res[0].売掛日付
        let tmp = new Date(max_date);
        tmp.setMonth(tmp.getMonth() + 1,1);
        tmp = tmp.toLocaleDateString('sv-SE').slice(0,7);
        var requestYM = tmp
        var requestD = '99'
        requestYM = requestYM.slice(0, 5) + ( '00' + (Number(requestYM.slice(5, 7))) ).slice( -2 )
        var setDate = getLastDateOfScope(requestYM,requestD,'');
        let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM')
        let category = "請求・売掛";
        let dialogId = "AccountsReceivableListDialog";
        let dialogName = "売掛金集計表";
        let btnLabel = "印刷";
        // 売掛金集計表
        createAndAddDialog(dialogId, dialogName, [
            { type: 'datepicker', id: 'accountsreceivableDate', label: '売掛年月', options: {
                width: 'normal',
                format:"month",
                required:true,
                varidate:{type:'str'},
                value:tmp
            }},
            { type: 'number', id: 'deadLine', label: '締日', options: {
                width: 'normal',
                required:true,
                disabled:true,
                value:99
            }},
            { type: 'range-text', id: 'custmerCD', label: '得意先CD', options: {
                width: 'wide',
                varidate:{type:'zeroPadding',maxlength:4},
                searchDialog:{id:'custmerNoSearch',title:'得意先検索',}
            }},
            { type: 'datepicker', id: 'classificationDate', label: '仕訳日付', options: {
                width: 'normal',
                varidate:{type:'str'},
                value:setDate
            }},
        ],
        [
            { type: 'button_inline', id: 'create', label: '印刷', options: {
                icon:'print',
                onclick:`AccountsReceivableListDialog_report('${dialogId}','${category}','${dialogName}','印刷', 'pdf');`
            }},
            { type: 'button_inline', id: 'create', label: '仕訳出力', options: {
                icon:'disk',
                onclick:`AccountsReceivableListDialog_report('${dialogId}','${category}','${dialogName}','仕訳出力', 'csv');`
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
async function AccountsReceivableListDialog_report(dialogId,category,dialogName,btnLabel,type){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 売掛金集計表');

        if (btnLabel == '印刷'){
            if (!confirm("指定された範囲で印刷します。")) {
                return;
            }
        } else {
            if (!confirm("指定された範囲で出力します。\r(締め処理も行います。)")) {
                return;
            }
        }

        let requestYMD = SpcToNull($('#AccountsReceivableListDialog_accountsreceivableDate').val()) + '-01'

        let item_params = {
            "@iDATE":requestYMD.replaceAll('-','/'),
            "@i開始得意先":SpcToNull($('#AccountsReceivableListDialog_custmerCDFrom').val()),
            "@i終了得意先":SpcToNull($('#AccountsReceivableListDialog_custmerCDTo').val()),
            "@i締日":SpcToNull($('#AccountsReceivableListDialog_deadLine').val()),
            "@i仕訳日付":SpcToNull($('#AccountsReceivableListDialog_classificationDate').val().replaceAll('-','/'))
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        let filename = "売掛金集計表_" + requestYMD.replaceAll('-','').slice(2, 8) + "." + type

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 売掛金集計表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}

