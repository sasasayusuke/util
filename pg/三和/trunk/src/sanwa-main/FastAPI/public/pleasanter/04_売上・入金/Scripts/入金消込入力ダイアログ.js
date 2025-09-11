{
    let today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    const category = "売上・入金";
    const dialogId = "ReceiptEntry";
    const dialogName = "入金消込入力";
    const btnLabel = "保存";

    createAndAddDialog(dialogId, dialogName, [

        { type: 'datepicker', id: 'PaymentDate', label: '入金日付', options: {
            newLine:true,
            width:'wide',
            value:today
		}},
		{ type: 'number', id: 'PaymentNo', label: '入金番号', options: {
            newLine:true,
            width:'wide',
            disabled:true,
            lock:'入金番号',
            varidate:{type:'int',maxlength:7},
		}},
		{ type: 'text-set', id: 'SupplierCD', label: '得意先コード', options: {
            newLine:false,
            width:'wide',
            digitsNum:4,
            lookupOrigin:{tableId:'TM得意先',keyColumnName:'得意先CD',forColumnName:"得意先名",specialTerms:"得意先名 = 得意先名1 + '　　　' + 得意先名2"},
            disabled:true,
		}},
		{ type: 'text', id: 'Sum', label: '合計金額', options: {
            alignment:'end',
            width:'wide',
            disabled:true
		}},
        { type: 'input-table', id: 'inputTable', label: '', options: {
            lineNumber:true,
            row:15,
            t_heads:[
                {label:'入金区分',ColumnName:'',type:'text-set',varidateFrom:{type:'int',maxLength:1},search:true,requiredFrom:true},
                {label:'入金種別',ColumnName:'',disabled:true},
                {label:'入金金額',ColumnName:'',alignment:'end',varidate:{type:'int',maxLength:15},required:true},
                {label:'手形期日/番号',ColumnName:'',type:'date',varidateFrom:{type:'str',maxLength:10},varidateTo:{type:'str',maxLength:10},disabled:true,requiredFrom:true},
                {label:'備考',ColumnName:'',varidate:{type:'str',maxLength:30}},
            ]
        }},
        { type: 'free', id: '', label: '', options: {
            str:`
                <div style="display: inline-block;width: 28%;vertical-align: top; padding-top:16px;">
                    <div style="width: 110px; font-weight: bold; text-align: right; font-size: small; margin-bottom: 10px; height:30px; margin-top: 5px;">請求情報</div>

                    <div id="ReceiptEntry_paymentPeriodField" class="field-wide both" style="">
                        <p class="field-label" style="width:80px;"><label for="ReceiptEntry_paymentPeriod" >請求期間</label></p>
                        <div class="field-control">
                            <div class="container-normal"  style="margin-left:80px;">
                                <div class="oneItem">
                                    <input id="ReceiptEntry_paymentPeriod" name="ReceiptEntry_paymentPeriod" class="control-textbox  " type="text" disabled="" style="">



                            </div>
                            </div>
                        </div>
                        <div class="error-message" id="ReceiptEntry_paymentPeriod-error"></div>
                    </div>

                    <div id="ReceiptEntry_leftOverField" class="field-wide both" style="">
                        <p class="field-label" style="width:80px;"><label for="ReceiptEntry_leftOver" >前残</label></p>
                        <div class="field-control">
                            <div class="container-normal"  style="margin-left:80px;">
                                <div class="oneItem">
                                    <input id="ReceiptEntry_leftOver" name="ReceiptEntry_leftOver" class="control-textbox input-Number " type="text" disabled="" style="text-align:end">



                            </div>
                            </div>
                        </div>
                        <div class="error-message" id="ReceiptEntry_leftOver-error"></div>
                    </div>

                    <div id="ReceiptEntry_stockingField" class="field-wide both" style="">
                        <p class="field-label"  style="width:80px;"><label for="ReceiptEntry_stocking">売上</label></p>
                        <div class="field-control">
                            <div class="container-normal"  style="margin-left:80px;">
                                <div class="oneItem">
                                    <input id="ReceiptEntry_stocking" name="ReceiptEntry_stocking" class="control-textbox input-Number " type="text" disabled="" style="text-align:end">



                            </div>
                            </div>
                        </div>
                        <div class="error-message" id="ReceiptEntry_stocking-error"></div>
                    </div>

                    <div id="ReceiptEntry_paymentField" class="field-wide both" style="">
                        <p class="field-label"  style="width:80px;"><label for="ReceiptEntry_payment">入金</label></p>
                        <div class="field-control">
                            <div class="container-normal"  style="margin-left:80px;">
                                <div class="oneItem">
                                    <input id="ReceiptEntry_payment" name="ReceiptEntry_payment" class="control-textbox input-Number " type="text" disabled="" style="text-align:end">



                            </div>
                            </div>
                        </div>
                        <div class="error-message" id="ReceiptEntry_payment-error"></div>
                    </div>

                    <div id="ReceiptEntry_remainingField" class="field-wide both" style="">
                        <p class="field-label" style="width:80px;"><label for="ReceiptEntry_remaining">当残</label></p>
                        <div class="field-control">
                            <div class="container-normal"  style="margin-left:80px;">
                                <div class="oneItem">
                                    <input id="ReceiptEntry_remaining" name="ReceiptEntry_remaining" class="control-textbox input-Number " type="text" disabled="" style="text-align:end">



                            </div>
                            </div>
                        </div>
                        <div class="error-message" id="ReceiptEntry_remaining-error"></div>
                    </div>

                    <div class="both" id="ReceiptEntry_buttons" style="margin-left:120px;">
                        <button id="receptEntry_now" style="margin-right:20px;">今回</button>
                        <button id="receptEntry_before">前情報</button>
                        <button id="receptEntry_after">次情報</button>
                    </div>
                </div>
            `
		}},
		{ type: 'text', id: 'estimateNo', label: '番号検索', options: {

		}},
        { type: 'button_inline', id: 'search', label: '検索', options: {
            icon:'search',
            onclick:`NoSearch_for_receptEntry()`
        }},
		{ type: 'search-table', id: 'searchTable', label: '', options: {
			multiple:true,
            individualSql:true,
            searchTableId:'TD見積',
            row:15,
            t_heads:[
                {label:'保存用',ColumnName:'',hidden:true},
                {label:'入金状況',ColumnName:'入金状況'},
                {label:'得意先CD',ColumnName:'得意先CD'},
                {label:'得意先名',ColumnName:'得意先名1'},
                {label:'請求日',ColumnName:'請求日付'},
				{label:'伝票番号',ColumnName:'済売上番号'},
				{label:'請求金額',ColumnName:'税込金額',alignment:'end'},
				{label:'入金済金額',ColumnName:'対象入金済金額',alignment:'end'},
				{label:'未入金額',ColumnName:'未入金額',alignment:'end'},
				{label:'消込額',ColumnName:''},
				{label:'対象入金済金額',ColumnName:'',hidden:true},
            ]
        }},
    ],
    [
        { type: 'button_inline', id: 'delete', label: '削除', options: {
            icon:'disk',
            onclick:`purge_for_receptEntry('${category}','${dialogId}','${dialogName}')`
        }},
        { type: 'button_inline', id: 'save', label: btnLabel, options: {
            icon:'disk',
            onclick:`saveClick_for_receptEntry('${category}','${dialogId}','${dialogName}','${btnLabel}')`
        }},
        { type: 'button_inline', id: 'stop', label: "中止", options: {
            icon:'disk',
            onclick:`receptEntry_initilize(true)`
        }},
    ]);
}

{
	let dialogId = "ReceiptEntry2";
    let dialogName = "入金消込入力";
	// 入金消込入力
	createAndAddDialog(dialogId, dialogName, [
		{ type: 'text-set', id: 'processCategory', label: '処理区分', options: {
			required:true,
            valueFrom:'0',
            type:'number',
            varidate:{type:'int',maxLength:1},
			values: [
				{ value: '0', text: '新規',checked:true},
				{ value: '1', text: '修正・削除' }
			],
			width: 'wide',
		}},
		{ type: 'range-date', id: 'requestDate', label: '請求日付', options: {
			width: 'normal',
			required:true,
            varidate:{type:'str'},
            valueFrom:monthDateGein(1),
            valueTo:monthDateGein(99)
		}},
		{ type: 'number', id: 'requestPeriod', label: '入金番号', options: {
			width: 'normal',
            disabled:true,
            varidate:{type:'int',maxlength:7},
            searchDialog:{id:'paymentSearch',title:'入金検索',multiple:false},
            digitsNum:7,
		}},
		{ type: 'text', id: 'custmerCode', label: '得意先CD', options: {
			width: 'normal',
            disabled:true,
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            lookupOrigin:{tableId:'TM得意先',keyColumnName:'得意先CD'},
            searchDialog:{id:'custmerNoSearch',title:'得意先検索',multiple:false},
            lookupFor:[{columnName:'得意先CD',id:'requestPeriod'}]
		}},
		{ type: 'text', id: 'custmerName1', label: '', options: {
			width: 'wide',
			disabled:true,
            lookupFor:[{columnName:'得意先名1',id:'custmerCode'}]
		}},
		{ type: 'text', id: 'custmerName2', label: '', options: {
			width: 'wide',
			disabled:true,
            lookupFor:[{columnName:'得意先名2',id:'custmerCode'}]
		}},
	],
    [
        { type: 'button_inline', id: 'save', label: '実行', options: {
            icon:'disk',
            onclick:'DepositErasingInput()'
        }},
    ]);
	// ダイアログを開くボタンを追加
	commonModifyLink(dialogName, function() {openDialog(dialogId)})

}
// -------------------------------------------------------------------------------------------------------
// ↓ 画面系処理


// 手形番号で半角文字のみ許可する
$(document).on('input','.ReceiptEntry_inputTable_3',function(){
    if(now_IME)return;
    let element = $(this);
    element.val(element.val().replace(/[^!-~\s]/g, ""));
})

// 下の表の列番号
class columnsNo_for_receptEntry{
    constructor(){
        this.col_check             = 0;    //チェック
        this.col_saveCheck         = 1;    //保存用チェック
        this.col_paymentStatus     = 2;    //入金状況
        this.col_custmerCD         = 3;    //得意先CD
        this.col_custmerName       = 4;    //得意先名
        this.col_requestDate       = 5;    //請求日付
        this.col_estimateNo        = 6;    //伝票番号（見積番号）
        this.col_requestMoney      = 7;    //請求金額
        this.col_DepositedMoney    = 8;    //入金済金額
        this.col_unpaidMoney       = 9;    //未入金額
        this.col_kesikomi          = 10;   //消込額
        this.col_targetPayedMoney  = 11;   //対象入金済金額
    }
}

$(document).on('click','#ReceiptEntry tbody .ui-icon-search',function(){
    openDialog_for_searchDialog('depositCategorySearch',$(this).prev().attr('id'),'750px','入金区分検索');
})

// 入金区分ルックアップ機能
$(document).on('blur',`.ReceiptEntry_inputTable_0`,async function(){
    try{
        // IME入力中だったら終了
        if(now_IME)return;

        const clear_items = ()=> {
            $(this).val('');
            $(this).nextAll().eq(2).val('');
            $(this).parent().next().children('input').val('');
        }


        let tmp = $(this).val().replace(/[^０-９0-9]/g, '');

        if(tmp == ''){
            clear_items();
            return;
        }


        // 全角文字が入力された場合は半角に置換して使用する
        const paymentCategory = convertFullWidthToHalfWidth(tmp);
        $(this).val(paymentCategory);
        sql = `SELECT 入金区分名, CASE 入金種別 WHEN '1' THEN '入金' WHEN '2' THEN '手形入金' WHEN '3' THEN '調整・値引' END AS 入金種別 FROM TM入金区分 WHERE 0=0 AND 入金区分CD = ${paymentCategory}`;

        try{
            var res = await fetchSql(sql);
        }
        catch(err){
            clear_items();
            return;
        }
        res = res.results;

        if(res.length != 1){
            clear_items();
            return;
        }

        $(this).nextAll().eq(2).val(res[0].入金区分名);
        $(this).parent().next().children('input').val(res[0].入金種別);

        // 値を初期化する
        $(this).parent().next().next().children('input').val('');
        $(this).parent().nextAll().eq(2).children('input').val('');
        $(this).parent().nextAll().eq(3).children('input').val('');

        if(['2','3'].includes($(this).val())){
            $(this).parent().nextAll().eq(2).children('input').attr('disabled',false);
        }else{
            $(this).parent().nextAll().eq(2).children('input').attr('disabled',true).val('');

        }
    }
    catch(err){
        alert('[GEN-041] 予期せぬエラーが発生しました。');
        console.error('[GEN-041] 入金消込入力', err);
        clear_items();
        return;
    }
})

$(document).on('click',`#ReceiptEntry_inputTable_inputTable tbody tr .textClear`,async function(){
    $(this).prevAll().eq(1).val('').trigger('input');
})

// クリア関数
function receptEntry_initilize(flg = false){
    try{
        if(flg){
            if(!confirm('現在の編集内容を破棄します。\nよろしいですか？'))return;
        }
        if($('#ReceiptEntry').dialog('isOpen') == true){
            UnLockData('入金番号',$('#ReceiptEntry2_requestPeriod').val());
            $('#ReceiptEntry_close').click();
        }
        $('#ReceiptEntry2_processCategoryFrom').val('0');
        $('#ReceiptEntry2_requestPeriod').val('').attr('disabled',true);
        $('#ReceiptEntry2_custmerCode').val('').attr('disabled',true).trigger('blur');
        $('#ReceiptEntry2_requestDateFrom').val(monthDateGein(1));
        $('#ReceiptEntry2_requestDateTo').val(monthDateGein(99));
        gGetuDateFlg_for_receptEntry = false;
    }catch(e){
        alert('[GEN-042] 予期せぬエラーが発生しました。');
        console.error('[GEN-042]', e);
        return;
    }
}

// 入金金額が変更されたとき、合計金額を再計算する関数イベント登録
async function recalculate_for_receptEntry(){
    await sleep(0);
    let rowCount = $(`#ReceiptEntry_inputTable_inputTable tbody tr`).length;
    let sumMoney = Array.from({ length: rowCount }, (_, i) => i).reduce((i,j) => i += Number($(`#ReceiptEntry_inputTable_${j}_2 input`).val().replaceAll(',','')),0);
    $('#ReceiptEntry_Sum').val(sumMoney).trigger('blur');
}
(function (){
    let rowCount = $(`#ReceiptEntry_inputTable_inputTable tbody tr`).length;
    for(let i = 0; i < rowCount; i++){
        $(document).on('blur',`#ReceiptEntry_inputTable_${i}_2 input`,function(){
            recalculate_for_receptEntry();
        })
    }
}())

// 下テーブルの合計計算
async function sum_total_for_receptEntry(){
    try{
        await sleep(0);
        const columnsNo = new columnsNo_for_receptEntry();
        const rows = $('#ReceiptEntry_searchTable_readTable tbody td:first-child input[type="checkbox"]:checked').closest('tr');

        let requestMoney = 0;
        let DepositedMoney = 0;
        let unpaidMoney = 0;
        let kesikomi = 0;
        for(const row of rows){
            requestMoney    += Number(null_to_zero($(row).children().eq(columnsNo.col_requestMoney).text().replaceAll(',','')));
            DepositedMoney  += Number(null_to_zero($(row).children().eq(columnsNo.col_DepositedMoney).text().replaceAll(',','')));
            unpaidMoney     += Number(null_to_zero($(row).children().eq(columnsNo.col_unpaidMoney).text().replaceAll(',','')));
            kesikomi        += Number(null_to_zero($(row).children().eq(columnsNo.col_kesikomi).find('input').val().replaceAll(',','')));
        }

        $('#ReceiptEntry_sum_requestMoney').text(requestMoney.toLocaleString());
        $('#ReceiptEntry_sum_DepositedMoney').text(DepositedMoney.toLocaleString());
        $('#ReceiptEntry_sum_col_unpaidMoney').text(unpaidMoney.toLocaleString());
        $('#ReceiptEntry_sum_col_kesikomi').text(kesikomi.toLocaleString());

    }catch(e){
        alert('[GEN-043] 予期せぬエラーが発生しました。');
        console.error('[GEN-043]', e);
        return;
    }
}
$(document).on('blur','#ReceiptEntry_searchTable_readTable .input-Number',function(){
    sum_total_for_receptEntry();
})
$(document).on('change','#ReceiptEntry_searchTable_readTable .allCheck',function(){
    sum_total_for_receptEntry();
})

// 入金区分の値によって項目の活性・非活性を決める
$(document).on('blur','#ReceiptEntry2_processCategoryFrom',function(){
    switch($(this).val()){
        case '0':
            $('#ReceiptEntry2_requestPeriod').prop('disabled',true);
            $('#ReceiptEntry2_custmerCode').prop('disabled',false);
            document.querySelector('#ReceiptEntry2_requestPeriodField.field-normal.both .ui-icon.ui-icon-close.textClear').style.display = 'none'; 
            document.querySelector('#ReceiptEntry2_requestPeriodField.field-normal.both .ui-icon.ui-icon-search.dialogSearch.varidate_int').style.display = 'none'; 
            document.querySelector('#ReceiptEntry2_custmerCodeField.field-normal.both .ui-icon.ui-icon-close.textClear').style.display = 'block'; 
            document.querySelector('#ReceiptEntry2_custmerCodeField.field-normal.both .ui-icon.ui-icon-search.dialogSearch.varidate_zeroPadding').style.display = 'block'; 

            break;
        case '1':
            $('#ReceiptEntry2_requestPeriod').prop('disabled',false);
            $('#ReceiptEntry2_custmerCode').prop('disabled',true);
            document.querySelector('#ReceiptEntry2_requestPeriodField.field-normal.both .ui-icon.ui-icon-close.textClear').style.display = 'block'; 
            document.querySelector('#ReceiptEntry2_requestPeriodField.field-normal.both .ui-icon.ui-icon-search.dialogSearch.varidate_int').style.display = 'block'; 
            document.querySelector('#ReceiptEntry2_custmerCodeField.field-normal.both .ui-icon.ui-icon-close.textClear').style.display = 'none'; 
            document.querySelector('#ReceiptEntry2_custmerCodeField.field-normal.both .ui-icon.ui-icon-search.dialogSearch.varidate_zeroPadding').style.display = 'none'; 


            break;
        case '':
            return;
        default:
            alert('0か1を入力してください');
            $(this).val('');
            $(this).focus();
    }
})

// 入金消込入力2の実行ボタンが押されたときの動作
async function DepositErasingInput(){
    try{

        const category = $('#ReceiptEntry2_processCategoryFrom').val();
        const start_date = $('#ReceiptEntry2_requestDateFrom').val();
        const end_date = $('#ReceiptEntry2_requestDateTo').val();
        const paymentNo = $('#ReceiptEntry2_requestPeriod').val();
        const custmerNo = $('#ReceiptEntry2_custmerCode').val();
        if(category == ''){
            alert('処理区分を入力してください')
            return;
        }
        if(start_date == ""){
            alert('開始日付が未入力です。');
            return;
        }
        if(end_date == ""){
            alert('終了日付が未入力です。');
            return;
        }
        if(category == '0' && custmerNo == ""){
            alert('得意先CDを入力してください。');
            return;
        }
        if(category == '1' && paymentNo == ""){
            alert('入金番号を入力してください。');
            return;
        }
        if(await download_for_ReceiptEntry() != 0){
            receptEntry_initilize();
            return;
        }
        openDialog("ReceiptEntry",'1200','px',function(){
            $('#ReceiptEntry_searchTable_readTable').prev().remove();
            $('#ReceiptEntry_PaymentDateField').parent().css('width','30%');
            $('#ReceiptEntry_PaymentNoField').parent().css('width','30%');
            $('#ReceiptEntry_PaymentDateField').css('width','100%');
            $('#ReceiptEntry_PaymentNoField').css('width','100%');
            $('#ReceiptEntry_SupplierCDField').css('width','60%');
            $('#ReceiptEntry_SumField').css('width','30%');
            $('.ReceiptEntry_inputTable_0').parent().css('width','120px');
            $('.ReceiptEntry_inputTable_3').parent().css('width','150px');
            $('.ReceiptEntry_inputTable_4').parent().css('width','170px');
            $('#ReceiptEntry_inputTable_inputTable').css({
                "width":'70%',
                "margin-top":'10px'
            });
            $('#ReceiptEntry_searchTable_readTable').css('margin-top','0');
            $('#ReceiptEntry_searchTable_readTable .h300').css({
                'min-height':'0',
                'height':'250px'
            });
            $('#ReceiptEntry_searchTable_readTable div:first-child').css('padding-bottom','0px');
            $('#ReceiptEntry_inputTable_inputTable div:first-child').css('padding','0');

            if(category == '0'){
                $('#ReceiptEntry').prev().children('.ui-dialog-title').text('入金消込入力 -- 登録');
            }else{
                $('#ReceiptEntry').prev().children('.ui-dialog-title').text('入金消込入力 -- 修正');
            }
        });
    }catch(e){
        console.error('[GEN-044]', e);
        alert('[GEN-044] 予期せぬエラーが発生しました。');
    }
}
let HAddDate_for_receptEntry;
let gGetuDateFlg_for_receptEntry = false;
async function download_for_ReceiptEntry(){
    const category = $('#ReceiptEntry2_processCategoryFrom').val();
    const paymentNo = $('#ReceiptEntry2_requestPeriod').val();
    const custmerNo = $('#ReceiptEntry2_custmerCode').val();
    const custmerName = $('#ReceiptEntry2_custmerName1').val();

    if(category == 0){
        // 新規
        // 得意先情報表示
        $('#ReceiptEntry_PaymentDate').val(new Date().toLocaleDateString('sv-SE'));
        $('#ReceiptEntry_SupplierCDFrom').val(custmerNo);
        $('#ReceiptEntry_SupplierCDTo').val(custmerName).trigger('input');
        $('#ReceiptEntry_PaymentNo').val('');
        HAddDate_for_receptEntry = new Date().toLocaleDateString('sv-SE');
    }
    else{
        // 修正・削除
        if(await download_NyukinD_for_receptEntry() == -2){
            return -1;
        }
    }
    switch(await Download_UriageD_for_receptEntry()){
        case -2:
            return -1;
    }
    return 0;
}

async function download_NyukinD_for_receptEntry(){
    try{

        const paymentNo = $('#ReceiptEntry2_requestPeriod').val();

        const query = `select * from TD入金 where 入金番号 = '${paymentNo}'`;
        let res;
        try{
            res = await fetchSql(query);
            res = res.results;
        }catch{
            return -2;
        }

        if(res.length == 0){
            alert('指定の入金番号は存在しません。');
            return -2;
        }
        else{
            var count = res.length;
        }

        if(!(await LockData('入金番号',paymentNo))){
            return -2;
        }

        // 月次更新日チェック
        const cls_dates = new clsDates('売掛月次更新日');
        await cls_dates.GetbyID();
        gGetuDate = cls_dates.updateDate;
        if(new Date(res[0].入金日付) <= gGetuDate){
            alert('更新済みのため修正できません。');
            gGetuDateFlg_for_receptEntry = true;

            $('#ReceiptEntry input').attr('readonly',true);
            $('#ReceiptEntry_save').css('display','none');
        }
        const custmerNo = res[0].得意先CD
        $('#ReceiptEntry_SupplierCDFrom').val(custmerNo).trigger('input');
        $('#ReceiptEntry_PaymentNo').val(paymentNo);
        $('#ReceiptEntry_PaymentDate').val(new Date(res[0].入金日付).toLocaleDateString('sv-SE'));
        HAddDate_for_receptEntry = new Date(res[0].初期登録日).toLocaleDateString('sv-SE');
        // 明細セット
        for(i = 0;i < res.length;i++){
            // 入金区分CD
            $(`#ReceiptEntry_inputTable_${i}_0_From`).val(res[i].入金区分CD);
            // 入金区分名
            $(`#ReceiptEntry_inputTable_${i}_0_To`).val(res[i].入金区分名);
            // 入金種別
            $(`#ReceiptEntry_inputTable_${i}_1 input`).val(
                res[i].入金種別 == 1 ? "入金" : res[i].入金種別 == 2 ? "手形入金" : res[i].入金種別 == 3 ? "調整・値引":""
            );
            // 入金金額
            $(`#ReceiptEntry_inputTable_${i}_2 input`).val(null_to_zero(res[i].入金金額)).trigger('blur');
            // 手形期日
            $(`#ReceiptEntry_inputTable_${i}_3 input[type="date"]`).val(res[i].手形期日 == null ? '': new Date(res[i].手形期日).toLocaleDateString('sv-SE'));
            // 手形番号
            $(`#ReceiptEntry_inputTable_${i}_3 input[type="text"]`).val(null_to_zero(res[i].手形番号,""));

            // 備考
            $(`#ReceiptEntry_inputTable_${i}_4 input[type="text"]`).val(null_to_zero(res[i].摘要名,""));

            if([2,3].includes(res[i].入金区分CD)){
                $(`#ReceiptEntry_inputTable_${i}_3 input`).attr('disabled',false);
            }

        }
        recalculate_for_receptEntry();

        await GetFirstSeik_for_receptEntry(custmerNo);
        await GetNowseik_for_receptEntry();

        HlastDate_for_receptEntry = new Date(HnowDate_for_receptEntry);
        $('#receptEntry_after').attr('disabled',true);

        if(res[0].請求更新FLG == 1){
            alert('請求処理済みデータです。');
            $('#ReceiptEntry input').attr('readonly',true);
            $('#ReceiptEntry_save').css('display','none');
        }

        return 0;

    }
    catch(e){
        alert('[GEN-045] 予期せぬエラーが発生しました。');
        console.error('[GEN-045] download_NyukinD_for_receptEntry', e);
        return -2;
    }
}

function SetupSeik_for_receptEntry(results){
    try{
        result = results[0];
        let tmp = formatDate_for_japanese(result.請求開始日付) + " ～ " + formatDate_for_japanese(result.請求終了日付);
        $('#ReceiptEntry_paymentPeriod').val(tmp);
        $('#ReceiptEntry_leftOver').val(result.前月残高.toLocaleString());
        $('#ReceiptEntry_stocking').val(result.売上金額.toLocaleString());
        $('#ReceiptEntry_payment').val(result.入金金額.toLocaleString());
        $('#ReceiptEntry_remaining').val(result.当月残高.toLocaleString());
        HnowDate_for_receptEntry = new Date(result.請求日付);

        if(HnowDate_for_receptEntry.getTime() == HlastDate_for_receptEntry.getTime()){
            $('#receptEntry_after').attr('disabled',true);
        }else{
            $('#receptEntry_after').attr('disabled',false);
        }

        if(HnowDate_for_receptEntry.getTime() == HfirstDate_for_receptEntry.getTime()){
            $('#receptEntry_before').attr('disabled',true);
        }else{
            $('#receptEntry_before').attr('disabled',false);
        }

    }
    catch(e){
        alert('[GEN-046] 予期せぬエラーが発生しました。');
        console.error('[GEN-046]', e);
        return;
    }
}

async function Download_UriageD_for_receptEntry(){
    try{

        var interval = setInterval(() =>{
            showLoading();
        },1000)
        const param = {
            "storedname": "usp_ND0600入金消込抽出",
            "params": {
                "@i得意先CD":$('#ReceiptEntry_SupplierCDFrom').val(),
                "@iDateFrom":$('#ReceiptEntry2_requestDateFrom').val(),
                "@iDateTo":$('#ReceiptEntry2_requestDateTo').val(),
                "@i入金番号":null_to_zero($('#ReceiptEntry_PaymentNo').val()),
            },
            "output_params":{
                "@RetST":'INT',
                "@RetMsg":'VARCHAR(255)'
            }
        }
        try{
            await sleep(0);
            var res = await fetchStored(param);
            var records = res.results[0]
        }
        catch(e){
            return -1;
        }
        if(records.length == 0){
            return -1;
        }
        let arr = ['入金状況', '得意先CD', '得意先名1', '請求日付', '見積番号', '税込金額','入金済金額', '未入金額', '対象入金済金額','対象入金済金額']

        let html = '';
        for(item of records){
            html += `<tr tabindex="0"><td class="searchTd bgc"><input type="checkbox" ${item.選択 == 0 ? '' : 'checked'}></td><td class="searchTd bgc"  style="display:none;"><input type="checkbox" ${item.選択 == 0 ? '' : 'checked'}></td>`;
            arr.forEach((e,i) => {
                if(i == 8){
                    if(item.選択 == 1){
                        html += `<td class="searchTd bgc" ><input type="text" class="input-Number" style='border: none; rgb(192,192,192) 1px; height:50%; width:100%; background-color:unset;text-align:end'value='${null_to_zero(item[e]).toLocaleString()}'></td>`;
                    }
                    else{
                        html += `<td class="searchTd bgc" ><input type="text" class="input-Number" style='border: none; rgb(192,192,192) 1px; height:50%; width:100%; background-color:unset;text-align:end'value='' readonly></td>`;
                    }

                }
                else if(i == 9){
                    html += `<td class="searchTd bgc" style='text-align:end; display:none;' >${null_to_zero(item[e]).toLocaleString()}</td>`;
                }
                else if(5 <= i){
                    html += `<td class="searchTd bgc" style='text-align:end'>${null_to_zero(item[e]).toLocaleString()}</td>`;
                }
                else if(i ==3){
                    html += `<td class="searchTd bgc" >${item[e].slice(0,10)}</td>`;
                }
                else{
                    html += `<td class="searchTd bgc" >${null_to_zero(item[e],"")}</td>`;
                }
            });
            html += "</tr>";
        }
        html += `
            <tr style="position: sticky;bottom: 0;z-index: 100;background-color: #f5f5f5;">
                <td colspan="2">合計</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td style="text-align:end;" id="ReceiptEntry_sum_requestMoney"></td>
                <td style="text-align:end;" id="ReceiptEntry_sum_DepositedMoney"></td>
                <td style="text-align:end;" id="ReceiptEntry_sum_col_unpaidMoney"></td>
                <td style="text-align:end;" id="ReceiptEntry_sum_col_kesikomi"></td>
            </tr>
        `;

        // 追加する
        $(`#ReceiptEntry_searchTable_readTable tbody tr`).remove();
        $(`#ReceiptEntry_searchTable_readTable tbody`).append(html);
        $(`#ReceiptEntry_searchTable_readTable tbody`).children().eq(0).focus();

        sum_total_for_receptEntry();
    }
    catch(e){
        alert('[GEN-047] 予期せぬエラーが発生しました。');
        console.error('[GEN-047]', e);
        return -1;
    }finally{
        clearInterval(interval);
        hideLoading();
    }
}

// 下の表で、チェックボックス変更時の処理
$(document).on('click','#ReceiptEntry_searchTable_readTable tr:has(".searchTd")',async function(){
    try{
        await sleep(0);

        const columnsNo = new columnsNo_for_receptEntry();

        const focus_element = $(':focus');
        if(focus_element.hasClass("input-Number") && focus_element.prop('tagName') == 'INPUT'){
            return;
        }

        // 選択状態を取得
        const item = $(this);
        const checkbox_attr = item.find('input:first').prop('checked');

        if(checkbox_attr){
            // ロック解除
            item.children().eq(columnsNo.col_kesikomi).children('input').attr('readonly',false);

            // 保存選択用を取得
            const for_save_checkbox_attr = item.children().eq(columnsNo.col_saveCheck).children('input').prop('checked');
            let bufkin = "";
            if(for_save_checkbox_attr){
                bufkin = item.children().eq(columnsNo.col_targetPayedMoney).text();
            }else{
                bufkin = item.children().eq(columnsNo.col_unpaidMoney).text();
            }
            item.children().eq(columnsNo.col_kesikomi).children('input').val(bufkin);
        }
        else{
            item.children().eq(columnsNo.col_kesikomi).children('input').val('').attr('readonly',true);
        }
        sum_total_for_receptEntry();

    }catch(e){
        alert('[GEN-048] 予期せぬエラーが発生しました。');
        console.error('[GEN-048]', e);
        return;
    }

})

// 全選択の時の消込額の処理
$(document).on('click','#ReceiptEntry_searchTable_readTable .allCheck',async function(){
    const check_flg = $(this).prop('checked');
    const rows = $('#ReceiptEntry_searchTable_readTable tbody tr');

    // falseの場合
    if(!check_flg){
        $(`#ReceiptEntry_searchTable_readTable tbody .input-Number`).val('').attr('readonly',true);
        return;
    }

    await sleep(0);
    const columnsNo = new columnsNo_for_receptEntry();

    for(row of rows){
        // ロック解除
        row = $(row);
        row.children().eq(columnsNo.col_kesikomi).children('input').attr('readonly',false);

        // 保存選択用を取得
        const for_save_checkbox_attr = row.children().eq(columnsNo.col_saveCheck).children('input').prop('checked');
        let bufkin = "";
        if(for_save_checkbox_attr){
            bufkin = row.children().eq(columnsNo.col_targetPayedMoney).text();
        }else{
            bufkin = row.children().eq(columnsNo.col_unpaidMoney).text();
        }
        row.children().eq(columnsNo.col_kesikomi).children('input').val(bufkin);
    }
})

// 番号検索
function NoSearch_for_receptEntry(){
    try{
        const estimate_no = $('#ReceiptEntry_estimateNo').val();
        const columnsNo = new columnsNo_for_receptEntry();

        const items = $(`#ReceiptEntry_searchTable_readTable tbody tr`).find(`td:nth-child(${columnsNo.col_estimateNo + 1})`);

        if(estimate_no == ""){
            alert('該当データは存在しません。');
            return;
        }
        if(items.length == 0){
            return;
        }

        const match_item = items.filter(function(index, element) {
            return $(element).text().trim() == estimate_no;
        });
        console.log(match_item);

        if(match_item.length == 0){
            alert('該当データは存在しません。');
            return;
        }
        const target_row = $(match_item[0]).parent();
        target_row[0].focus();
        console.log(document.activeElement);

    }catch(e){
        alert('[GEN-049] 予期せぬエラーが発生しました。');
        console.error('[GEN-049]', e);
        return;
    }
}

$(document).on('focus',`#ReceiptEntry2_processCategoryFrom`,function(){
    receptEntry_initilize();
})

// ↑ 画面系処理
// ----------------------------------------------------------------------------------------------------------------
// ↓ 請求情報取得系

$(document).on('input','#ReceiptEntry_SupplierCDTo',async function(){
    const custmerNo = $('#ReceiptEntry_SupplierCDFrom').val();
    await GetFirstSeik_for_receptEntry(custmerNo);
    await GetNowseik_for_receptEntry();

    HlastDate_for_receptEntry = new Date(HnowDate_for_receptEntry);
    $('#receptEntry_after').attr('disabled',true);
})

let HfirstDate_for_receptEntry = "";
let HlastDate_for_receptEntry = new Date();
let HnowDate_for_receptEntry = "";
async function GetFirstSeik_for_receptEntry(custmerCD){
    try{
        query = `
            SELECT 得意先CD, MIN(DISTINCT 請求日付) AS 請求日付
            FROM TM請求金額
            WHERE 得意先CD = '${custmerCD}'
            GROUP BY 得意先CD
        `;
        try{
            var res = await fetchSql(query);
            res = res.results;
        }catch(e){
            return;
        }
        if(res.length > 0){
            HfirstDate_for_receptEntry = new Date(res[0].請求日付);
        }
    }catch(e){
        alert('[GEN-050] 予期せぬエラーが発生しました。');
        console.error('[GEN-050]', e);
        return;
    }
}


async function GetNowseik_for_receptEntry(){
    try{
        const custmerNo = $('#ReceiptEntry_SupplierCDFrom').val();
        const query = `
            SELECT MOTO.得意先CD,MOTO.請求日付,請求開始日付,請求終了日付,前月残高,(入金金額+調整金額) AS 入金金額,
                (売上金額+返品金額+訂正金額+消費税額) AS 売上金額,当月残高
            FROM TM請求金額 AS MOTO
            Inner Join
                (SELECT 得意先CD, MAX(DISTINCT 請求日付) AS 請求日付
                    From TM請求金額
                    GROUP BY 得意先CD
                ) AS MAXDATE
                ON MOTO.得意先CD = MAXDATE.得意先CD
                AND MOTO.請求日付 = MAXDATE.請求日付
            WHERE MOTO.得意先CD = '${custmerNo}'
        `;
        try{
            var res = await fetchSql(query);
            res = res.results;
        }catch(e){
            return;
        }

        if(res.length == 0){
            $('#ReceiptEntry_paymentPeriod').val('');
            $('#ReceiptEntry_leftOver').val('');
            $('#ReceiptEntry_stocking').val('');
            $('#ReceiptEntry_payment').val('');
            $('#ReceiptEntry_remaining').val('');
            $('#ReceiptEntry_buttons button').attr('disabled',true);
        }
        else{
            SetupSeik_for_receptEntry(res);
        }
    }catch(e){
        console.error('[GEN-051]', e);
        alert('[GEN-051] 予期せぬエラーが発生しました。');
        return;
    }
}

// 今回ボタン
$(document).on('click','#receptEntry_now',function(){
    GetNowseik_for_receptEntry();
})

// 前情報
$(document).on('click','#receptEntry_before',async function(){
    try{

        const custmer_no = $('#ReceiptEntry_SupplierCDFrom').val();
        const query = `
            SELECT MOTO.得意先CD,MOTO.請求日付,請求開始日付,請求終了日付,前月残高,(入金金額+調整金額) AS 入金金額,
                (売上金額+返品金額+訂正金額+消費税額) AS 売上金額,当月残高
                FROM TM請求金額 AS MOTO
                WHERE 得意先CD= '${custmer_no}'
                AND 請求日付 = (SELECT MAX(請求日付) AS 請求日付
                            FROM TM請求金額
                            WHERE 請求日付 < '${HnowDate_for_receptEntry.toLocaleDateString('sv-SE')}'
                            AND 得意先CD = '${custmer_no}'
                            )
        `;
        try{
            var res = await fetchSql(query);
        }catch(e){
            return;
        }
        SetupSeik_for_receptEntry(res.results);


    }catch(e){
        alert('[GEN-052] 予期せぬエラーが発生しました。');
        console.error('[GEN-052]', e);
        return;
    }
})

// 次情報
$(document).on('click','#receptEntry_after',async function(){
    try{
        const custmer_no = $('#ReceiptEntry_SupplierCDFrom').val();
        const query = `
            SELECT MOTO.得意先CD,MOTO.請求日付,請求開始日付,請求終了日付,前月残高,(入金金額+調整金額) AS 入金金額, 
                (売上金額+返品金額+訂正金額+消費税額) AS 売上金額,当月残高 
                FROM TM請求金額 AS MOTO 
                WHERE 得意先CD= '${custmer_no}' 
                    AND 請求日付 = (SELECT MIN(請求日付) AS 請求日付 
                            FROM TM請求金額 
                            WHERE 請求日付 > '${HnowDate_for_receptEntry.toLocaleDateString('sv-SE')}' 
                                AND 得意先CD = '${custmer_no}'
                            )
        `;
        try{
            var res = await fetchSql(query);
        }catch(e){
            return;
        }
        SetupSeik_for_receptEntry(res.results);
    }catch(e){
        alert('[GEN-053] 予期せぬエラーが発生しました。');
        console.error('[GEN-053]', e);
        return;
    }
})

async function save_for_receptEntry(){
    try{

    }catch(e){
        alert('[GEN-054] 予期せぬエラーが発生しました。');
        console.error('[GEN-054]', e);
        return;
    }
}

// ↑ 請求情報取得系
// -------------------------------------------------------------------------------------------------------
// ↓ 登録処理

let tableData_for_receptEntry;
async function saveClick_for_receptEntry(category,dialogId,dialogName,btnLabel){
    try{
        if(await item_check_for_receptEntry()){
            return;
        }

        let checked_rows = $(`#ReceiptEntry_searchTable_readTable tbody tr`).has(`td:first-child input:checked`);
        checked_rows = checked_rows.toArray();
        // チェックされている件数が0ならメッセージを出して終了する
        if (checked_rows.length == 0){
            alert('選択されていません。');
            return;
        }

        if(!confirm('保存します。')){
            return;
        }
        
        const Cls_cols = new columnsNo_for_receptEntry();
        const checked_table_list = checked_rows.map(e => {
            return {
                estimateNo:$(e).children().eq(Cls_cols.col_estimateNo).text(),
                kesikomiMoney:Number($(e).children().eq(Cls_cols.col_kesikomi).children('input').val().replaceAll(',',''))

            }
        })

        const item_params = {
            HAddDate:HAddDate_for_receptEntry,
            processCategory:$('#ReceiptEntry2_processCategoryFrom').val(),
            paymentDate:$('#ReceiptEntry_PaymentDate').val(),
            paymentNo:$('#ReceiptEntry_PaymentNo').val(),
            custmerCD:$('#ReceiptEntry_SupplierCDFrom').val(),
            sumAll:Number(null_to_zero($('#ReceiptEntry_Sum').val().replaceAll(',',''),0)),
            input_tableData:tableData_for_receptEntry,
            check_tableData:checked_table_list
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
            await download_report(param);
        }catch{
            return;
        }
        
        receptEntry_initilize();
        
        alert('入金消込処理が終了しました。')

    }catch(e){
        alert('[GEN-055] 予期せぬエラーが発生しました。');
        console.error('[GEN-055]', e);
        return;
    }
}

async function item_check_for_receptEntry(){
    try{

        const paymentDate   = $('#ReceiptEntry_PaymentDate');
        const custmerCD     = $('#ReceiptEntry_SupplierCDFrom');

        let resFlg = true;

        if(paymentDate.val() == ""){
            alert('入金日付が未入力です。');
            paymentDate.focus();
            return resFlg;
        }

        // 月次更新日チェック
        const cls_dates = new clsDates('売掛月次更新日');
        await cls_dates.GetbyID();
        gGetuDate = cls_dates.updateDate;
        if(new Date(paymentDate.val()) <= gGetuDate){
            alert('更新済みの為、修正できません。');
            paymentDate.focus();
            return resFlg;
        }

        if(custmerCD.val() == ""){
            alert('得意先CDが未入力です。');
            custmerCD.focus();
            return resFlg;
        }

        // 入金区分チェック
        const rows_count = $('#ReceiptEntry_inputTable_inputTable tbody tr').length;
        let firstFlg = true;
        tableData_for_receptEntry = [];
        for(i = 0; i < rows_count; i++){
            const paymentCategory = $(`#ReceiptEntry_inputTable_${i}_0_From`);
            const paymentCategoryName = $(`#ReceiptEntry_inputTable_${i}_0_To`);
            const paymentType = $(`#ReceiptEntry_inputTable_${i}_1 input`);
            const paymentMoney    = null_to_zero($(`#ReceiptEntry_inputTable_${i}_2 input`),0);
            const tegataDate      = $(`#ReceiptEntry_inputTable_${i}_3 input[type="date"]`);
            const tegataNo        = $(`#ReceiptEntry_inputTable_${i}_3 input[type="text"]`);
            const remarks         = $(`#ReceiptEntry_inputTable_${i}_4 input`);

            if(firstFlg && paymentCategory.val() == ""){
                alert(`明細が未入力です。`);
                paymentCategory.focus();
                return resFlg;
            }
            else if(!firstFlg && paymentCategory.val() == ""){
                break;
            }
            if(paymentType.val() == "手形入金" && tegataDate.val() == ""){
                alert(`手形期日 (${i+1}行目)を入力して下さい。`);
                tegataDate.focus();
                return resFlg;
            }
            if(!byte_check(`備考${i+1}行目`,remarks.val(),30)){
                remarks.focus();
                return resFlg;
            }
            tableData_for_receptEntry.push({
                paymentCategory:paymentCategory.val(),
                paymentCategoryName:paymentCategoryName.val(),
                paymentType:paymentType.val(),
                paymentMoney:Number(SpcToNull(paymentMoney.val().replaceAll(',',''),0)),
                tegataDate:SpcToNull(tegataDate.val()),
                tegataNo:SpcToNull(tegataNo.val()),
                remarks:SpcToNull(remarks.val())
            });

            firstFlg = false;
        }

        return false;

    }catch(e){
        alert('[GEN-056] 予期せぬエラーが発生しました。');
        console.error('[GEN-056]', e);
        return;
    }
}

// ↑ 登録処理
// -------------------------------------------------------------------------------------------------------
// ↓ 削除処理
async function purge_for_receptEntry(category,dialogId,dialogName){
    try{

        if(gGetuDateFlg_for_receptEntry){
            alert('更新済みの為、修正できません。');
            return;
        }

        if(!confirm('削除します。\nよろしいですか？'))return;

        const paymentNo = $('#ReceiptEntry_PaymentNo').val();

        const item_params = {
            paymentNo:paymentNo
        }

        const param = {
            "category": category,
            "title": dialogName,
            "button": '削除',
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };
        try{
            await download_report(param);
        }catch(e){
            return;
        }
        alert('削除しました。');
        receptEntry_initilize();

    }catch(e){
        alert('[GEN-057] 予期せぬエラーが発生しました。');
        console.error('[GEN-057]', e);
        return;
    }
}
