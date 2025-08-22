

// 検収一覧表
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
        let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
        let category = "支払・買掛";
        let dialogId = "InspectionListDialog";
        let dialogName = "検収一覧表";
        let btnLabel = "印刷";

        createAndAddDialog(dialogId, dialogName, [
            { type: 'datepicker', id: 'purchaseDate', label: '仕入年月', options: {
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
            { type: 'range-text', id: 'supplier', label: '仕入先CD', options: {
                width: 'wide',
                search:true,
                digitsNum:4,
                varidate:{type:'zeroPadding',maxlength:4},
                searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
            }},
        ],
        [
            { type: 'button_inline', id: 'print', label: '印刷', options: {
                icon:'print',
                onclick:`InspectionListDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
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

async function InspectionListDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        var requestYM = SpcToNull($(`#${dialogId}_purchaseDate`).val());
        var requestD = SpcToNull($(`#${dialogId}_deadLine`).val());
        var requestDate

        if(requestD == '99') {
            requestDate = getLastDateOfScope(requestYM,requestD,'');
        } else {
            requestDate = requestYM + '-' +SpcToNull($(`#${dialogId}_deadLine`).val());
        }
        let item_params = {
            "@i支払日付":SpcToNull(formatDate(requestDate)),
            "@締日":SpcToNull(requestD),
            "@i開始仕入先":SpcToNull($(`#${dialogId}_supplierFrom`).val()),
            "@i終了仕入先":SpcToNull($(`#${dialogId}_supplierTo`).val()),
            "@type":"pdf",
        };
        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };
        let filename = "検収一覧表" + requestDate.replaceAll("-","").substring(2) + ".pdf"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log(`end : ${dialogName}`);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}