
{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    let category = "請求・売掛";
    let dialogId = "WELCIArequestParticularsListDialog";
    let dialogName = "ウエルシア請求明細";
    let btnLabel = "出力";


    // ウエルシア請求明細
    createAndAddDialog(dialogId, dialogName, [
        { type: 'range-date', id: 'requestDate', label: '請求日付', options: {
            width: 'wide',
            varidate:{type:'str'},
            valueFrom: today,
            valueTo: today,
            required:true
        }},
        { type: 'text-set', id: 'formatCategory', label: '書式区分', options: {
            width: 'wide',
            type:"number",
            required:true,
            values:[
                { value: '0', text: '通常' },
                { value: '1', text: '経理用' }
            ]
        }},
        // { type: 'buttonTextSet', id: 'test', label: '【出力先】', options: {
        //     btnLabel:'変更'
        // }},
    ],
    [
        { type: 'button_inline', id: 'create', label: btnLabel, options: {
            icon:'disk',
            onclick:`WELCIArequestParticularsListDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})

}

/**
 *
 * @param {string} dialogId     ダイアログID
 * @param {string} category     カテゴリー
 * @param {string} dialogName   ダイアログ名
 * @param {string} btnLabel     ボタンラベル
 * @returns
 */
async function WELCIArequestParticularsListDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : ウエルシア請求明細');

        let item_params = {
            "@iS集計日付":SpcToNull($('#WELCIArequestParticularsListDialog_requestDateFrom').val().replaceAll('-','/')),
            "@iE集計日付":SpcToNull($('#WELCIArequestParticularsListDialog_requestDateTo').val().replaceAll('-','/')),
            "@i書式区分":SpcToNull($('#WELCIArequestParticularsListDialog_formatCategoryFrom').val())
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };

        if (item_params['@i書式区分'] == 0)  {
            ProfileKey = "Wel請求明細"
        } else {
            ProfileKey = "Wel請求明細_経理用"
        }

        let filename = ProfileKey + SpcToNull($('#WELCIArequestParticularsListDialog_requestDateFrom').val().replaceAll('-','')).slice(2) + "-" + SpcToNull($('#WELCIArequestParticularsListDialog_requestDateTo').val().replaceAll('-','')).slice(2) + ".xlsx"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }
        

        console.log('end : ウエルシア請求明細');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}