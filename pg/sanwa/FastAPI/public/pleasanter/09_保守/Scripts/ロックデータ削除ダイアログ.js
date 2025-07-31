{
    // 製品NO変更
	let category = "保守";
    let dialogId = "deleteLockDialog";
    let dialogName = "ロックデータ削除";

    createAndAddDialog(dialogId, dialogName, [
        {type:'text',id:'person',label:'担当者CD',forColumnName:'PCName',options:{
            value:$p.loginId(),
            hidden:true
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            multiple:true,
            sql:true,
            searchTableId:'AppLockData',
            t_heads:[
                {label:"データ名",ColumnName:'DataName'},
                {label:"番号",ColumnName:'Number'},
                {label:"ログインID",ColumnName:'PCName'},
                {label:"登録日時",ColumnName:'CreateDate',type:'time'},
            ]
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '削除', options: {
            icon:'disk',
			onclick:`delete_appLockData();`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,1000,'px',async function(){
        try{
            $('#deleteLockDialog_searchTable_readTable th:first-child').css('width','20px');
            $('#deleteLockDialog_searchTable_searchButton').css('display','none');
            $('#deleteLockDialog_searchTable_searchButton').click();

        }catch(e){
            alert('予期せぬエラーが発生しました。');
            console.error(e);
            return;
        }
    })})
}

async function delete_appLockData(){
    try{
        // チェックされている行を取得
        const checked_rows = $('#deleteLockDialog_searchTable_readTable tr:has(td:first-child input:checked)').toArray();
        if(checked_rows.length == 0){
            alert('選択してください。');
            return;
        }
        const numbers = checked_rows.map(e =>{
            const DataName  = $(e).children().eq(1).text();
            const Number    = $(e).children().eq(2).text();
            return {DataName:DataName,Number:Number};
        })

        const item_params = {
            numbers:numbers
        }

        const param = {
            "category": '保守',
            "title": "ロックデータ削除",
            "button": "削除",
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        try{
            await download_report(param);
        }catch{
            return;
        }
        $('#deleteLockDialog_searchTable_searchButton').click();

        alert('削除処理が終了しました。');

    }catch(e){
            alert('予期せぬエラーが発生しました。');
            console.error(e);
            return;
        }
}