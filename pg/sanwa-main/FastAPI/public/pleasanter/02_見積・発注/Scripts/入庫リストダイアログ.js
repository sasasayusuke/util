{
    // 入庫リスト出力
    let category = "見積・発注";
    let dialogId = "storing_list_Output";
    let dialogName = "入庫リスト作成";
    let btnLabel = "出力";

    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    createAndAddDialog(dialogId, dialogName, [
        
        { type: 'datepicker', id: 'storing_date', label: '入庫日', options: {
            width: 'normal',
            required:true,
            value:today
        }},
        { type: 'range-text', id: 'custmer', label: '得意先CD', options: {
            width: 'wide',
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            search:true,
            searchDialog:{id:'custmerNoSearch',title:'得意先検索',multiple:false}
        }},
        { type: 'range-text', id: 'Supplier', label: '仕入先CD', options: {
            width: 'wide',
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            search:true,
            searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '出力', options: {
            icon:'disk',
            onclick:`DownloadStoringList('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}

async function DownloadStoringList(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        let storingDate = new Date($(`#${dialogId}_storing_date`).val());

        const formatDate = (date) => {
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0から始まるので+1
            const day = String(date.getDate()).padStart(2, '0'); // 日を2桁に
            return `${year}/${month}/${day}`;
        };

        let item_params = {
            "@i入庫日":SpcToNull(formatDate(storingDate)),
            "@iS得意先CD":SpcToNull($(`#${dialogId}_custmerFrom`).val()),
            "@iE得意先CD":SpcToNull($(`#${dialogId}_custmerTo`).val()),
            "@iS仕入先CD":SpcToNull($(`#${dialogId}_SupplierFrom`).val()),
            "@iE仕入先CD":SpcToNull($(`#${dialogId}_SupplierTo`).val()),
            "AccountUserName":$("#AccountUserName").text(),
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };

        let filename = "入庫リスト" + $(`#${dialogId}_storing_date`).val().replaceAll('-','').slice(2) + ".xlsx"

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