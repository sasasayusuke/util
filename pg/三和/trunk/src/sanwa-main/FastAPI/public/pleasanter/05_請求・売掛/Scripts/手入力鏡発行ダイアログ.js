{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')

    let category = "請求・売掛";
    let dialogId = "mirrorOutput";
    let dialogName = "手入力鏡発行";
    let btnLabel = "保存";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'datepicker', id: 'salesDay', label: '請求日付', options: {
            value: today,
            required:true,
            varidate:{type:'str'},
        }},
		{ type: 'text-set', id: 'customerCode', label: '得意先CD', options: {
            width: 'wide',
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'custmerNoSearch',title:'得意先検索'}
		}},
		{ type: 'text-set', id: 'deliveryCode', label: '納入先CD', options: {
			width: 'wide',
            varidate:{type:'zeroPadding',maxlength:4},
            searchDialog:{id:'recipientNoSearch',title:'納入先検索',}
		}},
        { type: 'text', id: 'estimate_no', label: '見積番号', options: {
			disabled:true,
            width: 'wide',
            searchDialog:{id:'estimateNoSearch5',title:'見積番号検索',multiple:true}
        }},
        { type: 'text', id: 'tm', label: '添付明細書数', options: {
            width: 'normal',
            unit:'枚'
        }},
        { type: 'text', id: 'location', label: '受渡地', options: {
            width: 'wide',
        }},
        { type: 'text', id: 'im', label: '印刷枚数', options: {
            value: 1,
            disabled:true,
            width: 'normal',
            unit:'枚'
        }},
		{ type: 'radio', id: 'blank_form', label: '印刷書式', options: {
			values: [
				{ value: '1', text: 'B5' },
				{ value: '2', text: 'A4', checked: true }
			],
			width: 'wide',
            disabled:true,
		}},
        // { type: 'textarea', id: 'remark', label: '摘要', options: {
        //     width: 'wide',
        // }},
        { type: 'text', id: 'remark1', label: '摘要1', options: {
            width: 'wide',
        }},
        { type: 'text', id: 'remark2', label: '摘要2', options: {
            width: 'wide',
        }},
        { type: 'text', id: 'remark3', label: '摘要3', options: {
            width: 'wide',
        }},
        { type: 'text', id: 'remark4', label: '摘要4', options: {
            width: 'wide',
        }},
        { type: 'text', id: 'remark5', label: '摘要5', options: {
            width: 'wide',
        }},
        { type: 'text', id: 'remark6', label: '摘要6', options: {
            width: 'wide',
        }},
        { type: 'text', id: 'money', label: '金額', options: {
            alignment:'end',
            width: 'normal',
        }},
        { type: 'text', id: 'tax', label: '消費税', options: {
            alignment:'end',
            width: 'normal',
        }},
        { type: 'text', id: 'sum', label: '合計', options: {
            alignment:'end',
            width: 'normal',
            disabled:true,
        }},
        { type: 'radio', id: 'textDisplayCategory', label: '振込手数料負担', options: {
            values: [
                { value: '1', text: '表示する', checked: true },
                { value: '2', text: '表示しない' }
            ],
            width: 'normal'
        }},
        { type: 'text', id: 'mirrorNo', label: '鏡番号', options: {
            hidden:true,
            lock:'手入力鏡',
        }},
    ],
    [
        { type: 'button_inline', id: 'select', label: '選択', options: {
            icon:'select',
            onclick:`openDialog_for_searchDialog('mirrorSearch','mirrorOutput_mirrorNo',1000,'手入力鏡選択');`
        }},
        { type: 'button_inline', id: 'delete', label: '削除', options: {
            icon:'cancel',
            hidden:true,
            onclick:`mirrorOutput_report('${dialogId}','${category}','${dialogName}','削除');`
        }},
        { type: 'button_inline', id: 'clear', label: '中止', options: {
            icon:'cancel',
            onclick:`mirrorOutput_cancel();`
        }},
        { type: 'button_inline', id: 'create', label: '保存', options: {
            icon:'disk',
            onclick:`mirrorOutput_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,550,'px',function(){
        $('#mirrorOutput_delete').css('display','none');
        $('#mirrorOutput').prev().children('.ui-dialog-title').text('手入力鏡発行');
        $('#mirrorOutput_customerCodeTo').attr('disabled',false);
        $('#mirrorOutput_deliveryCodeTo').attr('disabled',false);
    })})
    $('#mirrorOutput_remarkField').css('display','inline-table');
}

var current_mirror_no = 0

async function mirrorOutput_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }
        const result = $('#mirrorOutput').prev().children('.ui-dialog-title').text();
        var button_str = btnLabel
        if(result == '手入力鏡発行 -- 修正'){
            if(button_str == '保存'){
                button_str = '更新'
            }
        }
        console.log(button_str)

        if(button_str == '保存'){
            if (!confirm("保存します\rよろしいですか？")) {
                return;
            }
        }else if(button_str == '更新'){
            if (!confirm("保存します\rよろしいですか？")) {
                return;
            }
        }else if(button_str == '削除'){
            if (!confirm("削除します\rよろしいですか？")) {
                return;
            }
        }

        console.log(`start : ${dialogName}`);

        // 画面項目のデータ取得
        let elements = document.getElementsByName('mirrorOutput_textDisplayCategory');
        let len = elements.length;
        let checkValue = '';
        for (let i = 0; i < len; i++){
            if (elements.item(i).checked){
                checkValue = elements.item(i).value;
            }
        }

        var requestDate = new Date($(`#${dialogId}_salesDay`).val());

        const formatDate = (date) => {
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0から始まるので+1
            const day = String(date.getDate()).padStart(2, '0'); // 日を2桁に
            return `${year}/${month}/${day}`;
        };

        // バイト数チェック
        if(!byte_check(`受渡地`,$(`#${dialogId}_location`).val(),28)){
            $(`#${dialogId}_location`).focus();
            return;
        }

        for(i = 1; i <= 6;i++){
            if(!byte_check(`摘要${i}`,$(`#${dialogId}_remark${i}`).val(),60)){
                $(`#${dialogId}_remark${i}`).focus();
                return;
            }
        }

        let item_params = {
            "@鏡番号":SpcToNull($(`#${dialogId}_mirrorNo`).val()),
            "@請求日付":SpcToNull(formatDate(requestDate)),
            "@得意先CD":SpcToNull($(`#${dialogId}_customerCodeFrom`).val()),
            "@得意先名":SpcToNull($(`#${dialogId}_customerCodeTo`).val()),
            "@納入先CD":SpcToNull($(`#${dialogId}_deliveryCodeFrom`).val()),
            "@納入先名":SpcToNull($(`#${dialogId}_deliveryCodeTo`).val()),
            "@見積番号":SpcToNull($(`#${dialogId}_estimate_no`).val()),
            "@受渡地":SpcToNull($(`#${dialogId}_location`).val()),
            "@明細書数":SpcToNull($(`#${dialogId}_tm`).val()),
            "@金額":SpcToNull($(`#${dialogId}_money`).val()),
            "@消費税":SpcToNull($(`#${dialogId}_tax`).val()),
            "@摘要1":SpcToNull($(`#${dialogId}_remark1`).val()),
            "@摘要2":SpcToNull($(`#${dialogId}_remark2`).val()),
            "@摘要3":SpcToNull($(`#${dialogId}_remark3`).val()),
            "@摘要4":SpcToNull($(`#${dialogId}_remark4`).val()),
            "@摘要5":SpcToNull($(`#${dialogId}_remark5`).val()),
            "@摘要6":SpcToNull($(`#${dialogId}_remark6`).val()),
            "@振込負担テキスト表示flg":checkValue,
            "LoginId":$("#LoginId").val(),
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": button_str,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };
        let filename = "請求書_鏡.pdf"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        clear_diarog(false);

        console.log(`end : ${dialogName}`);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        clear_diarog(false);
        console.error(dialogName + "エラー" ,err);
    }

}

async function mirrorOutput_cancel(){
    clear_diarog(true)
}

function clear_diarog(msg_bool){
    if (msg_bool){
        if (!confirm("現在の変更内容を破棄します。\rよろしいですか？")) {
            return;
        }
    }
    const mirror_no = SpcToNull($('#mirrorOutput_mirrorNo').val(), 0);
    $('#mirrorOutput input').val('');
    $('#mirrorOutput_salesDay').val(new Date().toLocaleDateString('sv-SE'));
    $('#mirrorOutput_im').val('1');
    $('#mirrorOutput_delete').css('display','None');
    $('#mirrorOutput').prev().children('.ui-dialog-title').text('手入力鏡発行');
    var checkbox = document.getElementById('mirrorOutput_textDisplayCategory_1');
    checkbox.checked = true;
    // ロックの解除
    UnLockData("手入力鏡", mirror_no)
    current_mirror_no = 0
}

// 得意先CDのルックアップ
$(document).on('blur','#mirrorOutput_customerCodeFrom',async function(){
    let digitsNum = 4;
    if($(this).val().length != digitsNum){
        $('#mirrorOutput_customerCodeTo').val('')
        return;
    }
    const custmerCD = $('#mirrorOutput_customerCodeFrom').val();

    const cls_custmer = new ClsCustmer(custmerCD);
    const res = await cls_custmer.GetByData();

    if(!res){
        $('#mirrorOutput_customerCodeTo').val('')
        $('#mirrorOutput_customerCodeFrom').val('')
        alert('取得データが不正です');
        return;
    }
    // メモ　得意先CDの結果は、得意先CD横の入力欄に得意先名１、納入先CD横の入力欄に得意先名２をセットする仕様
    $('#mirrorOutput_customerCodeTo').val(cls_custmer["得意先名1"]);
    $('#mirrorOutput_deliveryCodeTo').val(cls_custmer["得意先名2"]);
})

// 納入先CD検索ダイアログを開く
$(document).on('click','#mirrorOutput_deliveryCodeField .ui-icon-search',async function(e){
    let custmerCD = $('#mirrorOutput_customerCodeFrom').val();
    let custmerName = $('#mirrorOutput_customerCodeTo').val();
    if(custmerName == ""){
        alert('得意先CDを入力してください。');
        $('#mirrorOutput_customerCodeFrom').focus();
        return;
    }
    openDialog_for_searchDialog('recipientNoSearch','mirrorOutput_deliveryCodeFrom','750px','納入先検索');
    $('#recipientNoSearch_custmerFrom').val(custmerCD);
    $('#recipientNoSearch_custmerTo').val(custmerName);
})

// 納入先CDのルックアップ
$(document).on('blur','#mirrorOutput_deliveryCodeFrom',async function(){
    let digitsNum = 4;
    if($(this).val().length != digitsNum){
        $('#mirrorOutput_deliveryCodeTo').val('')
        return;
    }
    const custmerCD = $('#mirrorOutput_customerCodeFrom').val();
    const recipientCD = $('#mirrorOutput_deliveryCodeFrom').val();

    const cls_recipient = new ClsRecipient(custmerCD,recipientCD);
    const res = await cls_recipient.GetByData();

    if(!res){
        $('#mirrorOutput_deliveryCodeTo').val('')
        $('#mirrorOutput_deliveryCodeFrom').val('')
        alert('取得データが不正です');
        return;
    }
    // メモ　得意先CDの結果は、得意先CD横の入力欄に得意先名１、納入先CD横の入力欄に得意先名２をセットする仕様
    $('#mirrorOutput_customerCodeTo').val(cls_recipient["納入先名1"]);
    $('#mirrorOutput_deliveryCodeTo').val(cls_recipient["納入先名2"]);
})

// 鏡番号ルックアップ
$(document).on('blur','#mirrorOutput_mirrorNo',async function(){
    try{
        const mirror_no = SpcToNull($('#mirrorOutput_mirrorNo').val(), 0);

        if(mirror_no == ""){
            return;
        }

        // ロックの解除
        UnLockData("手入力鏡", current_mirror_no)

        // データロック
        if (!(await LockData("手入力鏡", mirror_no))){
            $('#mirrorOutput_mirrorNo').val('');
            return;
        }

        current_mirror_no = mirror_no

        const query1 = `
            SELECT 鏡番号,請求日付,得意先CD,得意先名1,得意先名2,納入先CD,
                摘要1,摘要2,摘要3,摘要4,摘要5,摘要6,
                受渡地,明細書数,金額, 消費税
            FROM TD手入力鏡
            WHERE 鏡番号 = ${mirror_no}
        `;
        const query2 = `
            SELECT 鏡番号,枝番,見積番号
            FROM TD手入力鏡内訳
            WHERE 鏡番号 = ${mirror_no}
        `;
        try{
            var res = await fetchSql(query1);
            res = res.results;

            var res2 = await fetchSql(query2);
            res2 = res2.results;
        }catch(e){
            return;
        }

        if(res.length != 0){
            $('#mirrorOutput_salesDay').val(new Date(res[0].請求日付).toLocaleDateString('sv-SE'));
            $('#mirrorOutput_customerCodeFrom').val(res[0].得意先CD);
            $('#mirrorOutput_customerCodeTo').val(null_to_zero(res[0].得意先名1,""));
            $('#mirrorOutput_deliveryCodeFrom').val(res[0].納入先CD).trigger('input');
            $('#mirrorOutput_deliveryCodeTo').val(null_to_zero(res[0].得意先名2,""));
            $('#mirrorOutput_tm').val(res[0].明細書数);
            $('#mirrorOutput_location').val(res[0].受渡地);
            $('#mirrorOutput_money').val(res[0].金額).trigger('blur');
            $('#mirrorOutput_tax').val(res[0].消費税).trigger('blur');

            $('#mirrorOutput_remark1').val(res[0].摘要1);
            $('#mirrorOutput_remark2').val(res[0].摘要2);
            $('#mirrorOutput_remark3').val(res[0].摘要3);
            $('#mirrorOutput_remark4').val(res[0].摘要4);
            $('#mirrorOutput_remark5').val(res[0].摘要5);
            $('#mirrorOutput_remark6').val(res[0].摘要6);
        }

        if(res2.length != 0){
            $('#mirrorOutput_estimate_no').val(res2.map(e => e.見積番号).join());
        }

        // タイトル・削除ボタン
        $('#mirrorOutput').prev().children('.ui-dialog-title').text('手入力鏡発行 -- 修正');
        $('#mirrorOutput_delete').css('display','inline-block');

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
})

$(document).on('blur','#mirrorOutput_money',async function(){
    var amount = Number(SpcToNull($('#mirrorOutput_money').val().replace(',',''),0))
    let element1 = document.getElementById('mirrorOutput_tax');
    let element2 = document.getElementById('mirrorOutput_sum');

    if (amount == 0) {
        element1.value = 0;
        element2.value = 0;
    } else {
        let date = new Date();// 現在日時の取得
        let res;
        res = await GetTax(date);
        var tax = Number(res.results[0].税率)
        tax = Math.round(amount * tax * 0.01)
        var sum = amount + tax
        element1.value = tax.toLocaleString();
        element2.value = sum.toLocaleString();
    }
})

$(document).on('blur','#mirrorOutput_tax',async function(){
    var amount = Number(SpcToNull($('#mirrorOutput_money').val().replace(',',''),0))
    var tax = Number(SpcToNull($('#mirrorOutput_tax').val().replace(',',''),0))
    let element = document.getElementById('mirrorOutput_sum');
    var sum = amount + tax
    element.value = sum.toLocaleString();
})
