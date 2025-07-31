{
    // 製品NO変更
	let category = "保守";
    let dialogId = "changeProductDialog";
    let dialogName = "製品No変更";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'text-set', id: 'formatCategory', label: '書式区分', options: {
            width: 'wide',
            required:true,
            varidate:{type:'int',maxLength:1},
            values:[
                {value:0,text:'変換'},
                {value:1,text:'統合'}
            ]
        }},
        { type: 'text', id: 'productNo', label: '製品No', options: {
            width: 'normal',
            required: true,
            varidate:{maxlength:7},
            searchDialog:{id:'productSearch1',title:'製品No検索'}
        }},
        { type: 'text', id: 'methodNo', label: '仕様No', options: {
            width: 'normal',
            varidate:{maxlength:7},
        }},
        { type: 'text', id: 'productName', label: '', options: {
            width: 'wide',
            disabled:true,
        }},
        { type: 'text', id: 'AfterChangeNo', label: '変更後製品No', options: {
            width: 'normal',
            required: true,
            varidate:{maxlength:7},
            searchDialog:{id:'productSearch1',title:'製品No検索'}
        }},
        { type: 'text', id: 'AfterMethodNo', label: '変更後仕様No', options: {
            width: 'normal',
            varidate:{maxlength:7},
        }},
        { type: 'text', id: 'AfterProductName', label: '', options: {
            width: 'wide',
            disabled:true,
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: '実行', options: {
            icon:'disk',
			onclick:`execute_part_number_change('${dialogId}','${category}','${dialogName}','実行');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}

$(document).on('blur','#changeProductDialog_formatCategoryFrom',function(){
    let txt;
    if($(this).val() == 1){
        $('#changeProductDialog_AfterChangeNoField > p > label').text('統合後製品No');
        $('#changeProductDialog_AfterMethodNoField > p > label').text('統合後仕様No');
        $('#changeProductDialog_AfterChangeNo').next().show();
        $('#changeProductDialog_AfterChangeNo').next().next().show();
    }
    else{
        $('#changeProductDialog_AfterChangeNoField > p > label').text('変更後製品No');
        $('#changeProductDialog_AfterMethodNoField > p > label').text('変更後仕様No');
        $('#changeProductDialog_AfterChangeNo').next().hide();
        $('#changeProductDialog_AfterChangeNo').next().next().hide();
    }

})

async function execute_part_number_change(dialogId, category, dialogName, btnLabel){

    console.log(`ボタン実行処理start : ${dialogName}`);

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        const before_productNo        = $('#changeProductDialog_productNo');
        const before_siyouNo          = $('#changeProductDialog_methodNo');
        const before_productName      = $('#changeProductDialog_productName');
        const after_productNo         = $('#changeProductDialog_AfterChangeNo');
        const after_siyouNo           = $('#changeProductDialog_AfterMethodNo');
        const after_productName       = $('#changeProductDialog_AfterProductName');
        const formatCategory          = $('#changeProductDialog_formatCategoryFrom');
        
        const after_data = new ClsProduct(after_productNo.val().trim(),after_siyouNo.val().trim());
        const after_res = await after_data.GetByData(true);

        // ボタンを無効化して連打を防ぐ
        const button = document.getElementById('changeProductDialog_output');
        button.disabled = true;

        // ロジックチェック
        if(before_productNo.val() == after_productNo.val() && before_siyouNo.val() == after_siyouNo.val()){
            alert('同一のコードには変更できません。');
            after_productNo.val('').focus();
            after_siyouNo.val('');
            after_productName.val('');
            return;
        }

        // if(formatCategory.val() == 0 && after_data.length > 0){
        if(formatCategory.val() == 0 && after_res){
            alert('登録済みのコードには登録できません。');
            after_productNo.val('').focus();
            after_siyouNo.val('');
            after_productName.val('');
            return;
        }

        // if(formatCategory.val() == 1 && after_data.length == 0){
        if(formatCategory.val() == 1 && !after_res){
            alert('未登録のコードには変更できません。');
            after_productNo.val('').focus();
            after_siyouNo.val('');
            after_productName.val('');
            return;
        }

        const data = new ClsProduct(before_productNo.val().trim(),before_siyouNo.val().trim());
        const res = await data.GetByData(true);
        if(!res){
            alert('指定の製品は存在しません。');
            before_productNo.val('').focus();
            before_siyouNo.val('');
            before_productName.val('');
            return;
        }

        // 確認メッセージ
        if (!confirm("実行しますか？")) {
            button.disabled = false;
            return;
        }

        // formデータ格納
        let item_params = {
            "書式区分":SpcToNull($('#changeProductDialog_formatCategoryFrom').val()),
            "変更前製品No":SpcToNull($('#changeProductDialog_productNo').val()),
            "変更前仕様No":SpcToNull($('#changeProductDialog_methodNo').val()),
            "変更後製品No":SpcToNull($('#changeProductDialog_AfterChangeNo').val()),
            "変更後仕様No":SpcToNull($('#changeProductDialog_AfterMethodNo').val()),
        };

        // 送信データの準備
        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(), // 必要ならログインユーザー情報を入れる
            "opentime": dialog_openTime, // ダイアログのオープン時間を渡す
            "params": item_params,
        };

        // Python (FastAPI) へリクエスト送信
        try{
            await download_report(param,"");
        }catch{
            return;
        }

        // ログ設定
        setLog(`変換: ${item_params["製品No"]} → ${item_params["変更後製品No"]}`);
        // 終了後メッセージ
        alert("変更しました。");
        // フォーカスを変更
        document.getElementById('changeProductDialog_productNo').focus();


        // 処理終了
        console.log(`ボタン実行処理end : ${dialogName}`);

    }catch(err){
        console.error(dialogName + "エラー" ,err);
    }finally {
        // 処理が終わった後にボタンを有効化
        const button = document.getElementById('changeProductDialog_output');
        button.disabled = false;
    }
}

// ログ出力関数
function setLog(message) {
    console.log("ログ: " + message);
}

// 変更前製品No(製品があるかチェックのみ)
$(document).on('blur','#changeProductDialog_productNo',async function(){
    const before_productNo        = $('#changeProductDialog_productNo');
    const before_siyouNo          = $('#changeProductDialog_methodNo');
    const before_productName      = $('#changeProductDialog_productName');

    if(before_productNo.val() == ""){
        return;
    }

    const cls_product = new ClsProduct(before_productNo.val().trim(),before_siyouNo.val().trim());
    const res = await cls_product.GetByData();

    if(!res){
        before_productNo.val('').focus();
        before_productName.val('');
        return;
    }

    if (!before_productNo.val())return;

    if(!/^[a-zA-Z0-9!@#\$%\^\&\*\(\)_\+\-=\[\]\{\};:'",<>\.\?\/\\|`~]+$/.test(before_productNo.val())){
        alert('変更前製品Noは英数字または記号のみ入力可能です。');
        before_productNo.val('').focus();
        before_siyouNo.val('');
        before_productName.val('');
        return;
    }
})

// 変更前仕様No（ルックアップ）
$(document).on('blur','#changeProductDialog_methodNo',async function(){
    try{
        const before_productNo        = $('#changeProductDialog_productNo');
        const before_siyouNo          = $('#changeProductDialog_methodNo');
        const before_productName      = $('#changeProductDialog_productName');

        if(before_productNo.val() == ""){
            alert('製品Noを入力して下さい')
            before_productNo.focus();
            return;
        }

        if(!/^[a-zA-Z0-9ｦ-ﾟ!@#\$%\^\&\*\(\)_\+\-=\[\]\{\};:'",<>\.\?\/\\|`~]*$/.test(before_siyouNo.val())){
            alert('変更前仕様Noは英数字半角カナ記号のみ入力可能です。');
            before_siyouNo.val('').focus();
            before_productName.val('');
            return;
        }

        const cls_product = new ClsProduct(before_productNo.val(),before_siyouNo.val());
        const res = await cls_product.GetByData(true);

        if(!res){
            alert('指定の製品は存在しません。');
            before_productNo.val('').focus();
            before_siyouNo.val('');
            before_productName.val('');
            return;
        }

        if (!before_productNo.val())return;

        before_productName.val(cls_product["漢字名称"]);

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
    }
})

// 変更後製品No
$(document).on('blur','#changeProductDialog_AfterChangeNo',async function(){

    const after_productNo         = $('#changeProductDialog_AfterChangeNo');
    const after_siyouNo           = $('#changeProductDialog_AfterMethodNo');
    const after_productName       = $('#changeProductDialog_AfterProductName');
    const formatCategory          = $('#changeProductDialog_formatCategoryFrom');


    if (!after_productNo.val())return;

    if(!/^[a-zA-Z0-9!@#\$%\^\&\*\(\)_\+\-=\[\]\{\};:'",<>\.\?\/\\|`~]+$/.test(after_productNo.val())){
        alert('変更後製品Noは英数字または記号のみ入力可能です。');
        after_productNo.val('').focus();
        after_siyouNo.val('');
        after_productName.val('');
        return;
    }

    if (formatCategory.val() == 1){
        const cls_product = new ClsProduct(after_productNo.val(),after_siyouNo.val());
        const res = await cls_product.GetByData();
        if(!res){
            after_productNo.val('').focus();
            after_productName.val('');
            return;
        }
    }
})

// 変更後仕様No
$(document).on('blur','#changeProductDialog_AfterMethodNo',async function(){
    try{
        const formatCategory          = $('#changeProductDialog_formatCategoryFrom');
        const after_productNo         = $('#changeProductDialog_AfterChangeNo');
        const after_siyouNo           = $('#changeProductDialog_AfterMethodNo');
        const after_productName       = $('#changeProductDialog_AfterProductName');
        
        if(!/^[a-zA-Z0-9ｦ-ﾟ!@#\$%\^\&\*\(\)_\+\-=\[\]\{\};:'",<>\.\?\/\\|`~]*$/.test(after_siyouNo.val())){
            alert('変更後仕様Noは英数字半角カナ記号のみ入力可能です。');
            after_siyouNo.val('').focus();
            after_productName.val('');
            return;
        }

        if (!after_productNo.val())return;

        const cls_product = new ClsProduct(after_productNo.val(),after_siyouNo.val());
        const res = await cls_product.GetByData(true);

        if(formatCategory.val() == "0"){
            if(res){
                alert('登録済みのコードには変更できません。');
                $(this).val('').focus();
                after_productName.val('');
            }
            return;
        }else{
            if(!res){
                alert('未登録のコードには変更できません。');
                after_siyouNo.val('').focus();
                after_productName.val('');
                return;
            }
        }

        after_productName.val(cls_product["漢字名称"]);

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
    }
})


$(document).on('click','#changeProductDialog_productNoField .textClear',function(){
    $('#changeProductDialog_methodNo').val('');
    $('#changeProductDialog_productName').val('');
})
$(document).on('click','#changeProductDialog_AfterChangeNoField .textClear',function(){
    $('#changeProductDialog_AfterMethodNo').val('');
    $('#changeProductDialog_AfterProductName').val('');
})