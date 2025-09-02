//按分経費集計表
{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM')

    const category = '経費';
    const dialogId = "ProportionalExpenseTallyDialog";
    const dialogName = "按分経費集計表";
    const btnLabel = "出力"

    createAndAddDialog(dialogId, dialogName, [
        { type: 'datepicker', id: 'aggregationYear', label: '集計年月', options: {
            required:true,
            varidate:{type:'str'},
            format:"month",
            value:today
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: btnLabel, options: {
            icon:'disk',
            onclick:`create_ProportionalExpenseTally_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}

async function create_ProportionalExpenseTally_report(dialogId,category,dialogName,btnLabel){

    // バリデーション
    if(!validateDialog(dialogId)){
        return;
    }

    const date = $('#ProportionalExpenseTallyDialog_aggregationYear').val();

    if(date == ""){
        alert('集計年月を入力してください');
        return;
    }

    if(!confirm('経費集計表をExcelBookファイルに出力します。')){
        return;
    }

    const fileName = 'チーム別按分経費集計表作成' + date.replace('-','').slice(-4) + ".xlsx";

    let getDate   = new Date($(`#ProportionalExpenseTallyDialog_aggregationYear`).val());

    const formatDate = (date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0から始まるので+1
        const day = String(date.getDate()).padStart(2, '0'); // 日を2桁に
        return `${year}年${month}月分`;
    };

    const item_params = {
        "@iS集計日付":getFirstDateOfScope(date,99,date),
        "@iE集計日付":getLastDateOfScope(date,99,date),
        "@集計年月":formatDate(getDate)
    }

    const param = {
        "category": category,
        "title": dialogName,
        "button": btnLabel,
        "user": $p.userName(),
        "opentime": dialog_openTime,
        "params":item_params
    };

    try{
        await download_report(param,fileName);
    }catch{
        return;
    }

}