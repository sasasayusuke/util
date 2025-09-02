// 未回収一覧表作成
{

    let category = "売上・入金";
    let dialogId = "uncollectedListDialog";
    let dialogName = "未回収一覧表";
    let btnLabel = "印刷";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'requestDate', label: '請求日付', options: {
            width: 'wide',
            required:true,
            varidate:{type:'str'},
            valueFrom:monthDateGein(1),
            valueTo:monthDateGein(99)
        }},
        { type: 'range-text', id: 'custmerCode', label: '得意先CD', options: {
            width: 'wide',
            search:true,
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'custmerNoSearch',title:'得意先検索'}
        }},
        { type: 'checkbox', id: 'moneyCheck', label: '全額入金含む', options: {
            width: 'normal',
        }},
    ],
    [
        { type: 'button_inline', id: 'print', label: '印刷', options: {
            icon:'print',
            onclick:`downloadUncollectedList('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]
    );
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
    
}


async function downloadUncollectedList(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

        let requestDateFrom = new Date($(`#${dialogId}_requestDateFrom`).val());
        let requestDateTo   = new Date($(`#${dialogId}_requestDateTo`).val());

        const formatDate = (date) => {
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0から始まるので+1
            const day = String(date.getDate()).padStart(2, '0'); // 日を2桁に
            return `${year}/${month}/${day}`;
        };

        let item_params = {
            "@iDateFrom":SpcToNull(formatDate(requestDateFrom)),
            "@iDateTo":SpcToNull(formatDate(requestDateTo)),
            "@is得意先CD":SpcToNull($(`#${dialogId}_custmerCodeFrom`).val()),
            "@ie得意先CD":SpcToNull($(`#${dialogId}_custmerCodeTo`).val()),
            "@i全額含む":$(`#${dialogId}_moneyCheck`).is(':checked') ? 1 : 0,
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

        let filename = dialogName + $(`#${dialogId}_requestDateFrom`).val().replaceAll("-","").substring(2) + "-" + $(`#${dialogId}_requestDateTo`).val().replaceAll("-","").substring(2) + ".pdf"

        try{
            await download_report(param,filename);
        }catch(e){
            return;
        }
        

        console.log(`end : ${dialogName}`);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}